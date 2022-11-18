using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts.DbMaps;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;
using SimpleAdmin.WebApi.Infrastructure.Constant;
using SimpleAdmin.WebApi.Infrastructure.Utils;
using SixLabors.ImageSharp;
using Yitter.IdGenerator;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.ISecurityApi" />
public class SecurityApi : ApiBase<ISecurityApi>, ISecurityApi, IScoped
{
    private const int CACHE_EXPIRES_CAPTCHA = 60;

    private const int    CACHE_EXPIRES_SMSCODE = 300;
    private const string CACHE_KEY_SMSCODE     = $"{nameof(SendSmsCode)}_{{0}}";

    private const int SEND_LIMIT_SMSCODE = 60;

    /// <inheritdoc />
    public SecurityApi(IDistributedCache cache, IOptions<SecretOptions> secretOptions, IFreeSql freeSql)
    {
        _cache         = cache;
        _freeSql       = freeSql;
        _secretOptions = secretOptions.Value;
    }


    private readonly IDistributedCache _cache;

    private readonly IFreeSql      _freeSql;
    private readonly SecretOptions _secretOptions;


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<GetCaptchaRsp> GetCaptchaImage()
    {
        var baseDir = $@"{AppContext.BaseDirectory}/.res/captcha";


        var captchaImage =
            await CaptchaImageHelper.CreateSawSliderImage($"{baseDir}/backgrounds",
                                                          $"{baseDir}/templates",
                                                          (1, 101),
                                                          (1, 7),
                                                          new Size(50, 50));


        var cacheKey = $"{nameof(GetCaptchaImage)}_{YitIdHelper.NextId()}";
        var captchaData = new GetCaptchaRsp {
            CacheKey        = cacheKey,
            BackgroundImage = captchaImage.backgroundImage,
            SliderImage     = captchaImage.sliderImage
        };


        // 将缺口坐标保存到缓存
        await _cache.SetStringAsync(cacheKey,
                                    captchaImage.offsetSaw.X.ToString(),
                                    new DistributedCacheEntryOptions {
                                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CACHE_EXPIRES_CAPTCHA)
                                    });

        return captchaData;
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task SendSmsCode([FromServices] ISmsSender smsSender, SendSmsCodeReq req)
    {
        if (!await VerifyCaptcha(req.VerifyCaptchaReq)) throw Oops.Oh(Enums.ErrorCodes.HumanVerification);

        //人机验证通过，删除人机验证缓存
        await _cache.RemoveAsync(req.VerifyCaptchaReq.CacheKey);


        //如果缓存（手机号做key）存在，且创建时间小于1分钟，不得再次发送
        var cacheKey    = string.Format(CACHE_KEY_SMSCODE, req.Mobile);
        var sentCodeStr = await _cache.GetStringAsync(cacheKey);
        if (sentCodeStr is not null) {
            var sentCode     = sentCodeStr.Object<SmsCodeInfo>();
            var timeInterval = (DateTime.Now - sentCode.CreateTime).TotalSeconds;
            if (timeInterval < SEND_LIMIT_SMSCODE)
                throw Oops.Oh(Enums.ErrorCodes.InvalidOperation,
                              string.Format(Strings.TEMP_TRYSEND_SECS_AFTER, SEND_LIMIT_SMSCODE - (int)timeInterval));
        }

        // 如果是创建用户，但手机号存在，不得发送。
        if (req.Type == Enums.SmsCodeTypes.CreateUser)
            if (await _freeSql.Select<TbSysUser>().AnyAsync(a => a.Mobile == req.Mobile))
                throw Oops.Oh(Enums.ErrorCodes.InvalidOperation, Strings.MSG_MOBILE_EXISTS);

        var smsCode = new SmsCodeInfo {
            Code = new[] {
                    0,
                    10000
                }.Rand()
                 .ToString()
                 .PadLeft(4, '0'),
            CreateTime = DateTime.Now,
            Mobile     = req.Mobile
        };
        // 调用短信接口发送验证码
        smsSender.SendCode(req.Mobile!.Value, smsCode.Code);
        // 写入缓存，用于校验

        await _cache.SetStringAsync(cacheKey,
                                    smsCode.Json(),
                                    new DistributedCacheEntryOptions {
                                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CACHE_EXPIRES_SMSCODE)
                                    });
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<bool> VerifyCaptcha(VerifyCaptchaReq req)
    {
        var point = await _cache.GetStringAsync(req.CacheKey);
        if (point is null) return false;


        bool ret;
        try {
            var aesKey = req.CacheKey.Aes(_secretOptions.SecretKeyA)[..32];
            ret = Math.Abs(point.Float() - req.VerifyData.AesDe(aesKey).Float()) < 5f;
        }
        catch {
            ret = false;
        }

        if (!ret) await _cache.RemoveAsync(req.CacheKey);
        return ret;
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<bool> VerifySmsCode(VerifySmsCodeReq req)
    {
        var cacheKey = string.Format(CACHE_KEY_SMSCODE, req.Mobile);
        var code     = await _cache.GetStringAsync(cacheKey);
        if (code is null) return false;
        var codeObj = code.Object<SmsCodeInfo>();
        var success = codeObj.Code == req.Code;
        await _cache.RemoveAsync(cacheKey);
        return success;
    }
}
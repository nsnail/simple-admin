using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NSExt.Extensions;
using SimpleAdmin.WebApi.DataContracts.Dto.Security;
using SimpleAdmin.WebApi.Infrastructure.Configuration.Options;
using SimpleAdmin.WebApi.Infrastructure.Constants;
using SimpleAdmin.WebApi.Infrastructure.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Yitter.IdGenerator;

namespace SimpleAdmin.WebApi.Api.Implements;

/// <inheritdoc cref="SimpleAdmin.WebApi.Api.ISecurityApi" />
public class SecurityApi : ApiBase<ISecurityApi>, ISecurityApi
{
    private const int SMS_CODE_SEND_LIMIT = 60;

    /// <inheritdoc />
    public SecurityApi(IDistributedCache cache, IOptions<SecretOptions> secretOptions)
    {
        _cache         = cache;
        _secretOptions = secretOptions.Value;
    }

    private readonly string _baseDir = $@"{AppContext.BaseDirectory}/.res/captcha";

    private readonly int[] _bgroundIndexScope = {
        1,
        101
    };

    private readonly IDistributedCache _cache;
    private readonly SecretOptions     _secretOptions;

    private readonly Size _sliderSize = new(50, 50);

    private readonly int[] _templateIndexScope = {
        1,

        7
    };

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<GetCaptchaRsp> GetCaptchaImage()
    {
        //底图
        using var backgroundImage =
            await Image.LoadAsync<Rgba32>($"{_baseDir}/backgrounds/{_bgroundIndexScope.Rand()}.jpg");
        //深色模板图
        var       templateIndex     = _templateIndexScope.Rand();
        using var darkTemplateImage = await Image.LoadAsync<Rgba32>($@"{_baseDir}/templates/{templateIndex}/dark.png");
        //透明模板图
        using var transparentTemplateImage =
            await Image.LoadAsync<Rgba32>($@"{_baseDir}/templates/{templateIndex}/transparent.png");


        //调整模板图大小
        darkTemplateImage.Mutate(x => { x.Resize(_sliderSize); });
        transparentTemplateImage.Mutate(x => { x.Resize(_sliderSize); });

        //新建拼图
        using var blockImage = new Image<Rgba32>(_sliderSize.Width, _sliderSize.Height);
        //新建滑块拼图
        using var sliderBlockImage = new Image<Rgba32>(_sliderSize.Width, backgroundImage.Height);

        //随机生成拼图坐标
        var blockPoint = GeneratePoint(backgroundImage.Width,
                                       backgroundImage.Height,
                                       _sliderSize.Width,
                                       _sliderSize.Height);

        //根据深色模板图计算轮廓形状
        var blockShape = CalcBlockShape(darkTemplateImage);

        //生成拼图
        blockImage.Mutate(x => {
                              x.Clip(blockShape,
                                     // ReSharper disable once AccessToDisposedClosure
                                     p => p.DrawImage(backgroundImage, new Point(-blockPoint.X, -blockPoint.Y), 1));
                          });

        //拼图叠加透明模板图层
        // ReSharper disable once AccessToDisposedClosure
        blockImage.Mutate(x => x.DrawImage(transparentTemplateImage, new Point(0, 0), 1));

        //生成滑块拼图
        // ReSharper disable once AccessToDisposedClosure
        sliderBlockImage.Mutate(x => x.DrawImage(blockImage, new Point(0, blockPoint.Y), 1));

        var opacity = (float)(new[] {
                                     70,
                                     100
                                 }.Rand() * 0.01);

        //底图叠加深色模板图
        // ReSharper disable once AccessToDisposedClosure
        backgroundImage.Mutate(x => x.DrawImage(darkTemplateImage, new Point(blockPoint.X, blockPoint.Y), opacity));
        //生成干扰图坐标
        var interferencePoint = GenerateInterferencePoint(backgroundImage.Width,
                                                          backgroundImage.Height,
                                                          _sliderSize.Width,
                                                          _sliderSize.Height,
                                                          blockPoint.X,
                                                          blockPoint.Y);
        //底图叠加深色干扰模板图
        // ReSharper disable once AccessToDisposedClosure
        backgroundImage.Mutate(x => x.DrawImage(darkTemplateImage,
                                                new Point(interferencePoint.X, interferencePoint.Y),
                                                opacity));


        var cacheKey = $"{nameof(GetCaptchaImage)}_{YitIdHelper.NextId()}";
        var captchaData = new GetCaptchaRsp {
            CacheKey        = cacheKey,
            BackgroundImage = backgroundImage.ToBase64String(PngFormat.Instance),
            SliderImage     = sliderBlockImage.ToBase64String(PngFormat.Instance)
        };


        // 将缺口坐标保存到缓存
        await _cache.SetStringAsync(cacheKey,
                                    blockPoint.X.ToString(),
                                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

        return captchaData;
    }

    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<SendSmsCodeRsp> SendSmsCode([FromServices] ISmsSender smsSender, SendSmsCodeReq req)
    {
        if (!await VerifyCaptcha(req.VerifyCaptchaReq)) throw Oops.Oh(Enums.ErrorCodes.人机验证);
        //人机验证通过，删除人机验证缓存
        await _cache.RemoveAsync(req.VerifyCaptchaReq.CacheKey);


        var cacheKey = $"{nameof(SendSmsCode)}_{req.Mobile}";

        //如果缓存（手机号做key）存在，且创建时间小于1分钟，不得再次发送
        var sentCodeStr = await _cache.GetStringAsync(cacheKey);
        if (sentCodeStr is not null) {
            var sentCode     = sentCodeStr.Object<SendSmsCodeRsp>();
            var timeInterval = (DateTime.Now - sentCode.CreateTime).TotalSeconds;
            if (timeInterval < SMS_CODE_SEND_LIMIT)
                throw Oops.Oh(Enums.ErrorCodes.无效操作, $"{SMS_CODE_SEND_LIMIT - timeInterval}秒 后方可再次发送");
        }

        var ret = new SendSmsCodeRsp {
            Code = new[] {
                    0,
                    10000
                }.Rand()
                 .ToString()
                 .PadLeft(4, '0'),
            CreateTime    = DateTime.Now,
            Mobile        = req.Mobile,
            OperationType = req.OperationType
        };
        // 调用短信接口发送数字码
        smsSender.SendCode(req.Mobile, ret.Code);
        // 写入缓存，用于校验
        await _cache.SetStringAsync(cacheKey, ret.Json());

        #if !DEBUG
            ret.Code = null;
        #endif
        return ret;
    }


    /// <inheritdoc />
    [AllowAnonymous]
    public async Task<bool> VerifyCaptcha(VerifyCaptchaReq req)
    {
        var point = await _cache.GetStringAsync(req.CacheKey);
        if (point is null) return false;


        var ret = false;
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


    private static ComplexPolygon CalcBlockShape(Image<Rgba32> templateDarkImage)
    {
        var temp     = 0;
        var pathList = new List<IPath>();
        templateDarkImage.ProcessPixelRows(accessor => {
                                               for (var y = 0; y < templateDarkImage.Height; y++) {
                                                   var rowSpan = accessor.GetRowSpan(y);
                                                   for (var x = 0; x < rowSpan.Length; x++) {
                                                       ref var pixel = ref rowSpan[x];
                                                       if (pixel.A != 0) {
                                                           temp = temp switch {
                                                                      0 => x,
                                                                      _ => temp
                                                                  };
                                                       }
                                                       else {
                                                           if (temp == 0) continue;
                                                           pathList.Add(new RectangularPolygon(temp, y, x - temp, 1));
                                                           temp = 0;
                                                       }
                                                   }
                                               }
                                           });

        return new ComplexPolygon(new PathCollection(pathList));
    }

    /// <summary>
    ///     随机生成干扰图坐标
    /// </summary>
    /// <param name="originalWidth"></param>
    /// <param name="originalHeight"></param>
    /// <param name="templateWidth"></param>
    /// <param name="templateHeight"></param>
    /// <param name="blockX"></param>
    /// <param name="blockY"></param>
    /// <returns></returns>
    private static Point GenerateInterferencePoint(int originalWidth,
                                                   int originalHeight,
                                                   int templateWidth,
                                                   int templateHeight,
                                                   int blockX,
                                                   int blockY)
    {
        var x =
            //在原扣图右边插入干扰图
            originalWidth - blockX - 5 > templateWidth * 2
                ? GetRandomInt(blockX + templateWidth + 5, originalWidth - templateWidth)
                :
                //在原扣图左边插入干扰图
                GetRandomInt(100, blockX - templateWidth - 5);

        var y =
            //在原扣图下边插入干扰图
            originalHeight - blockY - 5 > templateHeight * 2
                ? GetRandomInt(blockY + templateHeight + 5, originalHeight - templateHeight)
                :
                //在原扣图上边插入干扰图
                GetRandomInt(5, blockY - templateHeight - 5);

        return new Point(x, y);
    }

    /// <summary>
    ///     随机生成拼图坐标
    /// </summary>
    /// <param name="originalWidth"></param>
    /// <param name="originalHeight"></param>
    /// <param name="templateWidth"></param>
    /// <param name="templateHeight"></param>
    /// <returns></returns>
    private static Point GeneratePoint(int originalWidth, int originalHeight, int templateWidth, int templateHeight)
    {
        var widthDifference  = originalWidth  - templateWidth;
        var heightDifference = originalHeight - templateHeight;
        var x = widthDifference switch {
                    <= 0 => 5,
                    _ => new[] {
                        0,
                        originalWidth - templateWidth - 100
                    }.Rand() + 100
                };

        var y = heightDifference switch {
                    <= 0 => 5,
                    _ => new[] {
                        0,
                        originalHeight - templateHeight - 5
                    }.Rand() + 5
                };

        return new Point(x, y);
    }


    /// <summary>
    ///     随机范围内数字
    /// </summary>
    /// <param name="startNum"></param>
    /// <param name="endNum"></param>
    /// <returns></returns>
    private static int GetRandomInt(int startNum, int endNum)
    {
        return (endNum > startNum
                    ? new[] {
                        0,
                        endNum - startNum
                    }.Rand()
                    : 0) + startNum;
    }
}

public record SendSmsCodeRsp : SmsCodeInfo
{ }
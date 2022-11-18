<template>
	<common-page title="注册新账号">
		<el-steps :active="stepActive" simple finish-status="success">
		    <el-step title="基础信息" />
			<el-step title="验证手机"/>
		    <el-step title="完成注册" />
		</el-steps>
		<el-form v-if="stepActive==0" ref="stepForm_0" :model="form" :rules="rules" :label-width="120">
			<el-form-item label="登录账号" prop="userName">
				<el-input v-model="form.userName" maxlength="20" placeholder="请输入登录账号"></el-input>
				<div class="el-form-item-msg">登录账号将作为登录时的唯一凭证</div>
			</el-form-item>
			<el-form-item label="登录密码" prop="password">
				<el-input v-model="form.password" type="password" show-password placeholder="请输入登录密码"></el-input>
				<sc-password-strength v-model="form.password"></sc-password-strength>
				<div class="el-form-item-msg">请输入包含英文、数字的8位以上密码</div>
			</el-form-item>
			<el-form-item label="确认密码" prop="password2">
				<el-input v-model="form.password2" type="password" show-password placeholder="请再一次输入登录密码"></el-input>
			</el-form-item>
			<el-form-item label="" prop="agree">
				<el-checkbox v-model="form.agree" label="">已阅读并同意</el-checkbox><span class="link" @click="showAgree=true">《平台服务协议》</span>
			</el-form-item>
		</el-form>
		<el-form v-if="stepActive==1" ref="stepForm_1" :model="form" :rules="rules" :label-width="120">
			<el-form-item label="手机号" prop="verifySmsCodeReq.mobile">
				<el-input v-model="form.verifySmsCodeReq.mobile" clearable maxlength="11"
						  placeholder="请输入手机号码" prefix-icon="el-icon-iphone">
					<template #prepend>+86</template>
				</el-input>


			</el-form-item>
			<el-form-item label="验证码" prop="verifySmsCodeReq.code">
				<div class="yzm">
					<el-input v-model="form.verifySmsCodeReq.code" clearable maxlength="4"
							  placeholder="请输入4位短信验证码"
							  prefix-icon="el-icon-unlock"></el-input>
					<el-button :disabled="disabled" @click="getYzm">发送验证码短信
						<span v-if="disabled"> ({{ time }})</span></el-button>
				</div>
			</el-form-item>
		</el-form>
		<div v-if="stepActive==2">
			<el-result icon="success" title="注册成功" sub-title="可以使用登录账号以及手机号登录系统">
				<template #extra>
					<el-button type="primary" @click="goLogin">前去登录</el-button>
				</template>
			</el-result>
		</div>
		<el-form style="text-align: center;">
			<el-button v-if="stepActive>0 && stepActive<2" @click="pre">上一步</el-button>
			<el-button v-if="stepActive<1" type="primary" @click="next">下一步</el-button>
			<el-button v-if="stepActive==1" type="primary" @click="save">提交</el-button>
		</el-form>
		<el-dialog v-model="showAgree" title="平台服务协议" :width="800" destroy-on-close>
			平台服务协议
			<template #footer>
				<el-button @click="showAgree=false">取消</el-button>
				<el-button type="primary" @click="showAgree=false;form.agree=true;">我已阅读并同意</el-button>
			</template>
		</el-dialog>
		<Verify
			ref="verify"
			:captchaType="captchaType"
			:imgSize="{width:'400px',height:'200px'}"
			mode="pop"
			@success="captchaSuccess"
		></Verify>
	</common-page>
</template>

<script>
import scPasswordStrength from '@/components/scPasswordStrength';
import commonPage from './components/commonPage'
import Verify from './components/verifition/Verify'
import {aesEncrypt} from "@/views/login/components/verifition/utils/ase";

export default {
	components: {
		Verify,
		commonPage,
		scPasswordStrength
	},
	data() {
		return {
			captchaType: 'blockPuzzle',
			disabled: false,
			time: 0,
			stepActive: 0,
			showAgree: false,
			form: {
				userName: "",
				password: "",
				password2: "",
				agree: false,
				verifySmsCodeReq: {
					mobile: '',
					code: ''
				}
			},
			rules: {
				userName: [
					{
						required: true, message: this.$CONFIG.STRINGS.MSG_USERNAME_STRONG,
						pattern: this.$CONFIG.STRINGS.REGEX_USERNAME
					},
					{
						validator: async (rule, valueEquals, callback) => {
							const res = await this.$API.account.checkUserName.post({userName: valueEquals});
							if (res.code == 0 && res.data) {
								callback()
							} else
								callback(new Error('用户名已被使用'))
						}, trigger: 'blur'
					}
				],
				password: [
					{
						required: true,
						message: this.$CONFIG.STRINGS.MSG_PASSWORD_STRONG, pattern: this.$CONFIG.STRINGS.REGEX_PASSWORD
					}
				],
				password2: [
					{required: true, message: '请再次输入密码'},
					{
						validator: (rule, value, callback) => {
							if (value !== this.form.password) {
								callback(new Error('两次输入密码不一致'));
							} else {
								callback();
							}
						}
					}
				],
				agree: [
					{
						validator: (rule, value, callback) => {
							if (!value) {
								callback(new Error('请阅读并同意协议'));
							} else {
								callback();
							}
						}
					}
				],
				'verifySmsCodeReq.mobile': [
					{
						required: true,
						message: this.$CONFIG.STRINGS.MSG_MOBILE_USEFUL, pattern: this.$CONFIG.STRINGS.REGEX_MOBILE
					},
					{
						validator: async (rule, valueEquals, callback) => {
							const res = await this.$API.account.checkMobile.post({mobile: valueEquals});
							if (res.code == 0 && res.data) {
								callback()
							} else
								callback(new Error('手机号已被使用'))
						}, trigger: 'blur'
					}
				],
				'verifySmsCodeReq.code': [
					{
						required: true,
						message: this.$CONFIG.STRINGS.MSG_SMSCODE_NUMBER,
						pattern: this.$CONFIG.STRINGS.REGEX_SMSCODE,
						trigger: 'change'
					}
				],
			}
		}
	},
	mounted() {

	},
	methods: {
		async captchaSuccess(obj) {
			const validate = await this.$refs.stepForm_1.validateField("verifySmsCodeReq.mobile").catch(() => {
			});
			if (!validate) {
				return false
			}

			await this.$API.security.sendSmsCode.post({
				"mobile": this.form.verifySmsCodeReq.mobile,
				"type": this.$CONFIG.ENUMS.smsCodeTypes.createUser.value,
				"verifyCaptchaReq": obj
			})


			this.$message.success("已发送短信至手机号码")
			this.disabled = true
			this.time = 60
			const t = setInterval(() => {
				this.time -= 1
				if (this.time < 1) {
					clearInterval(t)
					this.disabled = false
					this.time = 0
				}
			}, 1000);
		},
		async getYzm() {
			const validate = await this.$refs.stepForm_1.validateField("verifySmsCodeReq.mobile").catch(() => {
			});
			if (!validate) {
				return false
			}
			this.$refs.verify.show();

		},
		pre() {
			this.stepActive -= 1
		},
		next() {
			const formName = `stepForm_${this.stepActive}`
			this.$refs[formName].validate((valid) => {
				if (valid) {
					this.stepActive += 1
				} else {
					return false
				}
			})
		},
		save() {
			const formName = `stepForm_${this.stepActive}`
			this.$refs[formName].validate(async (valid) => {
				if (valid) {
					const res = await
						this.$API.account.create.post(Object.assign({},this.form,{password:aesEncrypt(this.form.password),password2:null}));
					if (res.code == 0)
						this.stepActive += 1
				} else {
					return false
				}
			})
		},
		goLogin() {
			this.$router.push({
				path: '/login'
			})
		}
	}
}
</script>

<style scoped>


</style>

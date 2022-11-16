/**
 *  安全接口
 *  @module @/api/security
 */

import config from "@/config"
import http from "@/utils/request"

export default {

	/**
	 * 获取验证图片
	 */
	getCaptchaImage :{
		url: `${config.API_URL}/security/get-captcha-image`,
		name: `获取验证图片`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},


	/**
	 * 发送短信数字码
	 */
	sendSmsCode :{
		url: `${config.API_URL}/security/send-sms-code`,
		name: `发送短信数字码`,
		post:async function(data) {
			return await http.post(this.url,data)
		}
	},


	/**
	 * 完成图片验证
	 */
	verifyCaptcha :{
		url: `${config.API_URL}/security/verify-captcha`,
		name: `完成图片验证`,
		post:async function(data) {
			return await http.post(this.url,data)
		}
	},

}

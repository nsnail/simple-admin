/**
 *  帐号接口
 *  @module @/api/account
 */

import config from "@/config"
import http from "@/utils/request"

export default {

	/**
	 * 检查用户名可用性
	 */
	checkUserName :{
		url: `${config.API_URL}/account/check-user-name`,
		name: `检查用户名可用性`,
		post:async function(data) {
			return await http.post(this.url,data)
		}
	},


	/**
	 * 创建帐号
	 */
	create :{
		url: `${config.API_URL}/account/create`,
		name: `创建帐号`,
		post:async function(data) {
			return await http.post(this.url,data)
		}
	},

}

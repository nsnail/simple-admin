/**
 *  用户接口
 *  @module @/api/user
 */

import config from "@/config"
import http from "@/utils/request"

export default {

	/**
	 * 获取个人信息
	 */
	getProfile :{
		url: `${config.API_URL}/user/get-profile`,
		name: `获取个人信息`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},


	/**
	 * 分页获取用户列表
	 */
	queryUsers :{
		url: `${config.API_URL}/user/query-users`,
		name: `分页获取用户列表`,
		post:async function(data) {
			return await http.post(this.url,data)
		}
	},

}

/**
 *  工具接口
 *  @module @/api/tools
 */

import config from "@/config"
import http from "@/utils/request"

export default {

	/**
	 * 服务器时间
	 */
	getServerUtcTime :{
		url: `${config.API_URL}/tools/get-server-utc-time`,
		name: `服务器时间`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},


	/**
	 * 获取版本号
	 */
	getVersion :{
		url: `${config.API_URL}/tools/get-version`,
		name: `获取版本号`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},

}

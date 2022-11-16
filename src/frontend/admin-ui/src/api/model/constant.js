/**
 *  常量接口
 *  @module @/api/constant
 */

import config from "@/config"
import http from "@/utils/request"

export default {

	/**
	 * 获得枚举常量
	 */
	getEnums :{
		url: `${config.API_URL}/constant/get-enums`,
		name: `获得枚举常量`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},


	/**
	 * 获得字符串常量
	 */
	getStrings :{
		url: `${config.API_URL}/constant/get-strings`,
		name: `获得字符串常量`,
		get:async function(data) {
			return await http.get(this.url,data)
		}
	},

}

/**
 *  常量接口
 *  @module @/api/constant/constant
 */
import config from "@/config"
import http from "@/utils/request";


export default {
	getStrings: {
		url: `${config.API_URL}/constant/get-strings`,
		name: "获得常用消息",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
}

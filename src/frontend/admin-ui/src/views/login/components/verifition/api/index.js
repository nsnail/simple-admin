

import security from "@/api/model/security";

// 获取验证图片  以及token
export function reqGet(data) {
	return security.getCaptchaImage.get(data)
}

// 滑动或者点选验证
export function reqCheck(data) {
	return security.verifyCaptcha.post(data)
}

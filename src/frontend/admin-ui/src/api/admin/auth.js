/**
 *  认证授权服务
 *  @module @/api/admin/auth
 */
import appConfig from "@/config"
import request from '@/utils/request'

/**
 * 查询密钥
 */
export const getPasswordEncryptKey = (params, config = {}) => {
  return request.get(`${appConfig.API_URL}/get-password-encrypt-key`, { params: params, ...config })
}

/**
 * 查询用户信息
 */
export const getUserInfo = (params, config = {}) => {
  return request.get(`${appConfig.API_URL}/get-user-info`, { params: params, ...config })
}

/**
 * 登录
 */
export const login = (params, config = {}) => {
  return request.post(`${appConfig.API_URL}/login`, params, config)
}

/**
 * 刷新Token
以旧换新
 */
export const refresh = (params, config = {}) => {
  return request.get(`${appConfig.API_URL}/refresh`, { params: params, ...config })
}

/**
 * 获取验证数据
 */
export const getCaptcha = () => {
  return request.get(`${appConfig.API_URL}/security/get-captcha-image`)
}

/**
 * 检查验证数据
 */
export const verifyCaptcha = params => {
  return request.post(`${appConfig.API_URL}/security/verify-captcha`, params)
}

export default {
  getPasswordEncryptKey,
  getUserInfo,
  login,
  refresh,
  getCaptcha,
  verifyCaptcha
}

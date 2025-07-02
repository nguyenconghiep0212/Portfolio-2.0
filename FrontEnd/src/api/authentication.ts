import { http } from '@/utils/axios'

export const registerUser = (params: { username: string; password: string }) => {
  return http.post(
    { url: '/api/Auth/register', data: params },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

export const loginUser = (params: { username: string; password: string }) => {
  return http.post(
    { url: '/api/Auth/login', data: params },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

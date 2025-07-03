import { ACCESS_TOKEN_KEY, CURRENT_PROJECT, REFRESH_TOKEN_KEY } from '@/enums/authen'
import { jwtDecode } from 'jwt-decode'

const token = 'YOUR_JWT_TOKEN_HERE' // Replace with your JWT

export function getAccessToken() {
  return getLocalCache(ACCESS_TOKEN_KEY)
}

export function setAccessToken(token: string) {
  return setLocalCache(ACCESS_TOKEN_KEY, token)
}
export function clearAccessToken() {
  clearLocalCache(ACCESS_TOKEN_KEY)
}
export function getRefreshToken() {
  return getLocalCache(REFRESH_TOKEN_KEY)
}

export function setRefreshToken(token: string) {
  return setLocalCache(REFRESH_TOKEN_KEY, token)
}
export function clearRefreshToken() {
  clearLocalCache(REFRESH_TOKEN_KEY)
}
export function getCurrentProject() {
  return getLocalCache(CURRENT_PROJECT)
}

export function setCurrentProject(token: string = '') {
  return setLocalCache(CURRENT_PROJECT, token)
}

export function getUserData() {
  const token = getAccessToken()
  if (token)
    try {
      const decoded = jwtDecode(token)
      return decoded
    } catch (error) {
      console.error('Invalid token:', error)
      return {}
    }
  else return {}
}

function getLocalCache(key: string) {
  return localStorage.getItem(key)
}

function setLocalCache(key: string, value: string) {
  return localStorage.setItem(key, value)
}

function clearLocalCache(key: string) {
  localStorage.removeItem(key)
}

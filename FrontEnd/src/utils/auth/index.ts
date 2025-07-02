import { ACCESS_TOKEN_KEY, CURRENT_PROJECT, REFRESH_TOKEN_KEY } from '@/enums/authen'

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

function getLocalCache(key: string) {
  return localStorage.getItem(key)
}

function setLocalCache(key: string, value: string) {
  return localStorage.setItem(key, value)
}

function clearLocalCache(key: string) {
  localStorage.removeItem(key)
}

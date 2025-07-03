import { ref } from 'vue'
import { defineStore } from 'pinia'
import {
  setAccessToken as setToken,
  setRefreshToken as setRefresh,
  getUserData,
} from '@/utils/auth'

export const useAuthen = defineStore('authen', () => {
  const accessToken = ref('')
  const refreshToken = ref('')
  const user = ref()

  function setAccessToken(token: string) {
    accessToken.value = token
    setToken(token)
    user.value = getUserData().sub
  }
  function setRefreshToken(token: string) {
    refreshToken.value = token
    setRefresh(token)
  }
  return { user, setAccessToken, setRefreshToken }
})

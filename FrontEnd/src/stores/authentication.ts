import { ref } from 'vue'
import { defineStore } from 'pinia'
import { setAccessToken as setToken, setRefreshToken as setRefresh } from '@/utils/auth'

export const useAuthen = defineStore('authen', () => {
  const accessToken = ref('')
  const refreshToken = ref('')
  const user = ref({
    user: '',
    role: '',
  })

  function setAccessToken(token: string) {
    accessToken.value = token
    setToken(token)
  }
  function setRefreshToken(token: string) {
    refreshToken.value = token
    setRefresh(token)
  }
  return { user, setAccessToken, setRefreshToken }
})

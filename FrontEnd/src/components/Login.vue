<template>
  <div class="flex items-center justify-center h-full">
    <div class="w-96">
      <div class="px-4 py-8 bg-white shadow sm:rounded-lg sm:px-10">
        <form class="space-y-6" action="#" method="POST">
          <div>
            <div for="email" class="flex justify-between text-sm font-medium text-gray-700">
              <div>Email address</div>
              <v-tooltip :open-on-hover="false" open-on-click interactive>
                <template v-slot:activator="{ props }">
                  <div class="opacity-50">
                    <v-icon v-bind="props" icon="mdi-information-outline"></v-icon>
                  </div>
                </template>
                <div class="text-lg font-thin tracking-wider opacity-75">
                  <div>admin | admin</div>
                </div>
              </v-tooltip>
            </div>
            <div class="mt-1">
              <input
                id="email"
                name="email"
                type="email"
                v-model="loginInfo.username"
                autocomplete="email"
                required
                class="relative block w-full px-3 py-2 text-gray-900 placeholder-gray-500 border border-gray-300 rounded-md appearance-none focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                placeholder="Enter your email address"
              />
            </div>
          </div>

          <div>
            <div for="password" class="block text-sm font-medium text-gray-700">
              <div>Password</div>
            </div>
            <div class="mt-1">
              <input
                id="password"
                name="password"
                type="password"
                autocomplete="current-password"
                v-model="loginInfo.password"
                required
                class="relative block w-full px-3 py-2 text-gray-900 placeholder-gray-500 border border-gray-300 rounded-md appearance-none focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                placeholder="Enter your password"
              />
            </div>
          </div>

          <div class="flex justify-center">
            <v-btn variant="elevated" color="blue" :loading="isLoading" @click="Login">
              <span> Sign in </span>
            </v-btn>
          </div>
          <div class="flex flex-col items-center justify-center text-sm font-thin opacity-50">
            <div>Don't have an account ?</div>
            <div class="text-blue-400 underline cursor-pointer" @click="goToRegister">
              Create one now
            </div>
          </div>
        </form>
        <!-- <div class="mt-6">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-gray-300"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-2 text-gray-500 bg-gray-100"> Or continue with </span>
            </div>
          </div>

          <div class="grid grid-cols-3 gap-3 mt-6">
            <div>
              <a
                href="#"
                class="flex items-center justify-center w-full px-8 py-3 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md shadow-sm hover:bg-gray-50"
              >
                <img
                  class="w-5 h-5"
                  src="https://www.svgrepo.com/show/512120/facebook-176.svg"
                  alt=""
                />
              </a>
            </div>
            <div>
              <a
                href="#"
                class="flex items-center justify-center w-full px-8 py-3 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md shadow-sm hover:bg-gray-50"
              >
                <img
                  class="w-5 h-5"
                  src="https://www.svgrepo.com/show/513008/twitter-154.svg"
                  alt=""
                />
              </a>
            </div>
            <div>
              <a
                href="#"
                class="flex items-center justify-center w-full px-8 py-3 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md shadow-sm hover:bg-gray-50"
              >
                <img class="w-6 h-6" src="https://www.svgrepo.com/show/506498/google.svg" alt="" />
              </a>
            </div>
          </div>
        </div> -->
      </div>
    </div>
    <v-snackbar v-model="wrongCredential">
      <div class="flex justify-center text-red-400">username or password is incorrect</div>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { loginUser } from '@/api/authentication'
import { ref } from 'vue'
import { useAuthen } from '@/stores/authentication'
import { useRouter, useRoute } from 'vue-router'
import { setCurrentProject } from '@/utils/auth'

const router = useRouter()
const route = useRoute()
const useAuthenStore = useAuthen()
const isLoading = ref<boolean>(false)
const wrongCredential = ref<boolean>(false)
const loginInfo = ref<{ username: string; password: string }>({
  username: '',
  password: '',
})
function goToRegister() {
  router.push({
    name: 'register',
    query: { project: route.query.project },
  })
}
function Login() {
  isLoading.value = true
  setTimeout(async () => {
    const res = await loginUser(loginInfo.value)
    if (res) {
      isLoading.value = false
      useAuthenStore.setAccessToken(res.token)
      useAuthenStore.setRefreshToken(res.refreshToken)
      setCurrentProject(route.query.project?.toString())
      router.push({
        name: route.query.project?.toString(),
      })
    } else {
      wrongCredential.value = true
      isLoading.value = false
    }
  }, 0)
}
</script>

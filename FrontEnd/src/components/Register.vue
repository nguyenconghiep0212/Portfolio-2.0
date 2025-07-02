<template>
  <div class="flex items-center justify-center h-full">
    <div class="w-96">
      <div class="px-4 py-8 bg-white shadow sm:rounded-lg sm:px-10">
        <form class="space-y-6" action="#" method="POST">
          <div>
            <div for="email" class="flex justify-between text-sm font-medium text-gray-700">
              <div>Email address</div>
            </div>
            <div class="mt-1">
              <input
                id="email"
                name="email"
                type="email"
                v-model="registerInfo.username"
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
                v-model="registerInfo.password"
                required
                class="relative block w-full px-3 py-2 text-gray-900 placeholder-gray-500 border border-gray-300 rounded-md appearance-none focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                placeholder="Enter your password"
              />
            </div>
          </div>

          <div>
            <div for="password" class="block text-sm font-medium text-gray-700">
              <div>Re-enter password</div>
            </div>
            <div class="mt-1">
              <input
                id="password"
                name="password"
                type="password"
                autocomplete="current-password"
                v-model="registerInfo.repassword"
                required
                class="relative block w-full px-3 py-2 text-gray-900 placeholder-gray-500 border border-gray-300 rounded-md appearance-none focus:outline-none focus:ring-blue-500 focus:border-blue-500 focus:z-10 sm:text-sm"
                placeholder="Enter your password"
              />
            </div>
          </div>

          <div class="flex justify-center">
            <v-btn variant="elevated" color="blue" @click="Register">
              <span> Sign up </span>
            </v-btn>
          </div>
          <div class="flex flex-col items-center justify-center text-sm font-thin opacity-50">
            <div>Already have an account ?</div>
            <div class="text-blue-400 underline cursor-pointer" @click="goToLogin">Login</div>
          </div>
        </form>
      </div>
    </div>
    <v-snackbar v-model="wrongCredential">
      <div class="flex justify-center text-red-400">
        {{ textFail }}
      </div>
    </v-snackbar>
    <v-snackbar v-model="registerSuccess">
      <div class="flex flex-col items-center">
        <div class="text-green-400">
          Account registered successfully, redirect to login in {{ redirectCountdown }}
        </div>
        <div class="mt-1 text-blue-400 underline cursor-pointer" @click="goToLogin">
          Redirect to login now
        </div>
      </div>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { registerUser } from '@/api/authentication'
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()
const route = useRoute()
const redirectCountdown = ref<number>(5)
const registerSuccess = ref<boolean>(false)
const wrongCredential = ref<boolean>(false)
const isWrongPassword = ref<boolean>(false)
const textFail = ref<string>('')
const registerInfo = ref<{ username: string; password: string; repassword: string }>({
  username: '',
  password: '',
  repassword: '',
})
function goToLogin() {
  router.push({
    name: 'login',
    query: { project: route.query.project },
  })
}
async function Register() {
  if (!Validate()) {
    return
  }
  const params = {
    username: registerInfo.value.username,
    password: registerInfo.value.password,
  }
  const res = await registerUser(params)
  if (res) {
    registerSuccess.value = true
    Countdown()
  } else {
    wrongCredential.value = true
    textFail.value = 'username already exist'
  }
}

function Countdown() {
  const interval = setInterval(() => {
    redirectCountdown.value--
    if (redirectCountdown.value == 0) {
      clearInterval(interval)
      goToLogin()
    }
  }, 1000)
}

function Validate(): boolean {
  if (
    registerInfo.value.repassword === registerInfo.value.password &&
    registerInfo.value.username != null &&
    registerInfo.value.password != null
  ) {
    isWrongPassword.value = false
    return true
  } else {
    textFail.value = 're-enter password is not matched'
    wrongCredential.value = true
    isWrongPassword.value = true
    return false
  }
}
</script>

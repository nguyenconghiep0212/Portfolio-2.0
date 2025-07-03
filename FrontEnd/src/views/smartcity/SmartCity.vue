<template>
  <div>
    <!-- DISCLAIMER -->
    <div class="absolute z-10 w-full">
      <div class="flex justify-center mt-1 font-thin tracking-widest opacity-75">
        *this work does not represent any real smart city project, it is just a demo for experience
        showcase purposes*
      </div>
    </div>

    <v-layout>
      <!-- NAV -->
      <v-navigation-drawer v-model="drawer" :rail="rail" permanent @click="rail = false">
        <v-list>
          <v-list-item prepend-icon="mdi-city" title="Smart City">
            <template v-slot:append>
              <v-btn icon="mdi-chevron-left" variant="text" @click.stop="rail = !rail"></v-btn>
            </template>
          </v-list-item>
        </v-list>

        <v-divider :thickness="2" class="border-opacity-100"></v-divider>

        <v-list nav>
          <v-list-item
            v-for="(item, index) in navList"
            :key="index"
            :prepend-icon="item.icon"
            :value="item.value"
            @click="routerGoTo(item.value)"
          >
            <template v-slot:title>
              <div class="font-light tracking-wide">
                {{ item.title }}
              </div>
            </template>
          </v-list-item>
        </v-list>
      </v-navigation-drawer>

      <!-- CONTENT -->
      <v-main class="flex flex-col min-h-screen">
        <!-- HEAEDER -->
        <div class="w-full h-[72px] bg-white grid grid-cols-2">
          <div class="flex items-center"></div>
          <div class="flex items-center justify-end mr-8">
            <Bell class="mr-12" />
            <div class="pr-2">
              <div class="flex items-end tracking-wide">{{ useAuthenStore.user }}</div>
              <div
                class="flex justify-end text-sm text-blue-400 underline cursor-pointer"
                @click="logout()"
              >
                Logout
              </div>
            </div>
            <div>
              <v-avatar size="x-large">
                <v-img alt="John" src="https://cdn.vuetifyjs.com/images/john.jpg"></v-img>
              </v-avatar>
            </div>
          </div>
        </div>
        <v-divider :thickness="2" class="border-opacity-100"></v-divider>

        <!-- BODY -->
        <RouterView class="flex-1" />
      </v-main>
    </v-layout>
  </div>
</template>

<script setup lang="ts">
import { RouterView, useRouter } from 'vue-router'
import { ref } from 'vue'
import Bell from '@/components/SmartCity/Bell.vue'
import { clearAccessToken, clearRefreshToken } from '@/utils/auth'
import { useAuthen } from '@/stores/authentication'

const useAuthenStore = useAuthen()
const router = useRouter()
const drawer = ref(true)
const rail = ref(true)
const navList = [
  { icon: 'mdi-map', title: 'Map', value: 'map', onClick: routerGoTo },
  { icon: 'mdi-format-list-bulleted', title: 'List', value: 'list', onClick: routerGoTo },
  { icon: 'mdi-chart-arc', title: 'Chart', value: 'chart', onClick: routerGoTo },
]

function routerGoTo(des: string) {
  router.push({ name: des })
}

function logout() {
  clearAccessToken()
  clearRefreshToken()
  router.push({ name: 'home' })
}
</script>

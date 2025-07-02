<template>
  <div>
    <!-- HEADER -->
    <div class="flex">
      <div class="text-4xl font-light tracking-widest text-nowrap w-fit">My works</div>
      <div class="flex items-center w-full pl-4">
        <v-divider :thickness="5" class="border-opacity-100"></v-divider>
      </div>
    </div>

    <!-- CONTENT -->
    <div class="grid grid-cols-1 gap-4 mt-8 md:grid-cols-2 lg:grid-cols-3">
      <div v-for="(item, index) in workAll" :key="index">
        <div
          class="relative flex flex-col p-4 bg-white rounded-lg shadow-md cursor-pointer h-80"
          @click="routerGoTo(item)"
        >
          <div class="">
            <div class="opacity-50">{{ formatIndex(index + 1) }}</div>
            <div class="-mt-4 opacity-25">________________________</div>
            <div class="text-xl font-light">{{ item.name }}</div>
          </div>
          <div class="flex-1 mt-2 overflow-hidden border rounded-lg">
            <div
              class="w-full h-full transition-transform duration-300 ease-in-out hover:scale-110"
              :style="{
                backgroundImage: `url(${item.thumbnail})`,
                backgroundSize: 'cover',
                backgroundPosition: 'center',
              }"
            ></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import onthedesk from '@/assets/img/onthedesk.png'
import smartcity from '@/assets/img/smartcity.jpg'
import cctv from '@/assets/img/cctv.jpg'
import socialtracking from '@/assets/img/socialtracking.png'
import digitalfarm from '@/assets/img/digitalfarm.webp'
import crawler from '@/assets/img/crawler.jpg'

interface WorkItem {
  name: string
  routerName: string
  isExternal?: boolean
  externalLink?: string
  thumbnail?: string
}

const router = useRouter()
const workAll = computed(() => {
  return works.value.internal.concat(works.value.public)
})
const works = ref<{ internal: WorkItem[]; public: WorkItem[] }>({
  internal: [
    {
      name: 'Smart City',
      routerName: 'smartcity',
      isExternal: false,
      thumbnail: smartcity,
    },
    {
      name: 'CCTV',
      routerName: 'cctv',
      isExternal: false,
      thumbnail: cctv,
    },
    {
      name: 'Social Tracker',
      routerName: 'socialtracker',
      isExternal: false,
      thumbnail: socialtracking,
    },
    {
      name: 'Digital Farm',
      routerName: 'digitalfarm',
      isExternal: false,
      thumbnail: digitalfarm,
    },
    {
      name: 'Data Crawler',
      routerName: 'datacrawler',
      isExternal: false,
      thumbnail: crawler,
    },
  ],
  public: [
    {
      name: 'Onthedesk',
      routerName: '#',
      isExternal: true,
      externalLink: 'https://efolio.onthedesk.vn/',
      thumbnail: onthedesk,
    },
  ],
})

function formatIndex(index: number): string {
  return index < 10 ? `0${index}` : `${index}`
}

function routerGoTo(item: WorkItem) {
  if (item.isExternal && item.externalLink) {
    window.open(item.externalLink, '_blank')
    return
  } else {
    router.push({
      name: 'login',
      query: { project: item.routerName },
    })
    return
  }
}
</script>

<style scoped></style>

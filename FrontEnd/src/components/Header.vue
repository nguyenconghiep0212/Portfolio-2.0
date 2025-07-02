<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()

function routerMoveTo(des: string) {
  router.push({
    name: des,
  })
}

function jumpTo() {}
</script>

<template>
  <div class="sticky top-0 grid w-full grid-cols-2 px-4 py-6 bg-white">
    <div>
      <v-btn class="!text-xl" variant="plain" :ripple="false" @click="routerMoveTo('home')">
        Home
      </v-btn>
    </div>
    <div class="flex justify-end">
      <v-btn class="!text-xl" variant="plain" :ripple="false" @click="routerMoveTo('about')">
        About Me
      </v-btn>
      <v-btn class="!text-xl" variant="plain" :ripple="false" @click="jumpTo">Experiences</v-btn>
      <v-btn class="!text-xl" variant="plain" :ripple="false" @click="jumpTo">My Works</v-btn>
      <v-btn class="!text-xl" variant="plain" :ripple="false" @click="jumpTo">Contact</v-btn>
    </div>
  </div>
</template>

<style scoped></style>

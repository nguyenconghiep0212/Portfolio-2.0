<template>
  <div class="container flex flex-col p-4 bg-white rounded-lg shadow-md">
    <div>
      <img :src="imgSrc" alt="Quick Preview" class="w-64 h-48 bg-red-300" />
      <div class="mt-2 text-3xl">{{ title }}</div>
      <div class="mt-1 mb-6 text-lg font-light">
        {{ description }}
      </div>
    </div>
    <div class="mt-auto">
      <span class="text-lg font-light">{{ duration }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  imgSrc: string
  title: string
  description: string
  duration: string
}>()
</script>

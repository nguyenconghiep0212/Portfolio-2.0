<template>
  <div>
    <v-badge color="error" :content="notifications" class="cursor-pointer">
      <v-icon icon="mdi-bell-outline"></v-icon>
    </v-badge>
  </div>
</template>

<script lang="ts" setup>
import { useSmartCity } from '@/stores/smartcity'

const smartCityStore = useSmartCity()
const { notifications, getCityName } = smartCityStore

console.log(getCityName)
</script>

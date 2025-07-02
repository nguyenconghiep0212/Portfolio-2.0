<script setup lang="ts">
function moveTo() {}
</script>

<template>
  <div class="flex justify-end w-full py-6 pr-4 bg-white">
    <v-btn class="!text-xl" variant="plain" :ripple="false" @click="moveTo">About Me</v-btn>
    <v-btn class="!text-xl" variant="plain" :ripple="false" @click="moveTo">Experiences</v-btn>
    <v-btn class="!text-xl" variant="plain" :ripple="false" @click="moveTo">My Works</v-btn>
    <v-btn class="!text-xl" variant="plain" :ripple="false" @click="moveTo">Contact</v-btn>
  </div>
</template>

<style scoped></style>

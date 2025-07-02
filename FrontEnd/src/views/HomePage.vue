<script setup lang="ts">
import HeaderComponent from '../components/Header.vue'
import FooterComponent from '../components/Footer.vue'
import QuickPreview from '../components/HomePage/QuickPreview.vue'
import Experience from '../components/HomePage/Experience.vue'
import MyWork from '../components/HomePage/MyWork.vue'
import Hobbies from '../components/HomePage/Hobbies.vue'
import Technologies from '../components/HomePage/Technologies.vue'

</script>

<template>
  <div class="">
    <HeaderComponent class="z-10" />
    <div class="flex flex-col items-center pb-8 px-44">
      <QuickPreview />
      <div class="w-[60rem]">
        <Experience />
        <Technologies class="mt-24" />
        <MyWork class="mt-24" />
        <Hobbies class="mt-24" />
      </div>
    </div>
    <FooterComponent class="z-10" />
  </div>
</template>

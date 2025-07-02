<script setup lang="ts">
import Item from '@/components/HomePage/ExperienceItem.vue'
</script>

<template>
  <div>
    <!-- Header -->
    <div class="flex">
      <div class="text-4xl font-light tracking-widest">Experiences</div>
      <div class="flex items-center w-full pl-4">
        <v-divider :thickness="5" class="border-opacity-100"></v-divider>
      </div>
    </div>

    <!-- Content -->
    <div class="grid grid-cols-1 gap-12 mt-8 md:grid-cols-2 lg:grid-cols-3">
      <!-- FE -->
      <Item
        imgSrc="#"
        title="Front End Developer"
        description="Engage in innovating, designing and building a quality online experiences"
        duration="2+ years"
      />

      <!-- UNITY -->
      <Item
        imgSrc="#"
        title="Unity Developer"
        description="Develop 2D and 3D games, leveraging innovative design and programming skills to deliver engaging player experiences"
        duration="6 months"
      />

      <!-- EIF -->
      <Item
        imgSrc="#"
        title="EIF SM Assistant"
        description="Online system management for machines in factory, trouble shooting maintaining source code for machines driver"
        duration="400+ hours"
      />
    </div>
  </div>
</template>

<template>
  <div class="grid w-full grid-cols-5 h-[70vh]">
    <!-- INTRODUCTION -->
    <div class="col-span-3 flex justify-start content-center h-full !flex-wrap pl-32 space-y-4">
      <div class="text-5xl font-light">
        Hi, my name is
        <span class="font-semibold">Nguyen Cong Hiep</span>
      </div>
      <div class="text-3xl font-light leading-normal">
        I’m a
        <span class="px-2 py-1 font-semibold bg-red-300">Front-End Developer</span>
        , engage in innovating, designing and building a quality online experiences
      </div>
      <div class="transition duration-100 origin-left hover:scale-125">
        <span
          class="text-lg font-light tracking-wider underline cursor-pointer decoration-1 underline-offset-8"
        >
          Contact me
        </span>
      </div>
    </div>

    <!-- SELFIE -->
    <div class="flex items-center justify-start col-span-2 pl-32">
      <img src="#" alt="Quick Preview" class="object-cover w-96 h-[32rem] bg-slate-700" />
    </div>
  </div>
</template>

<script setup lang="ts"></script>

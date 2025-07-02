<script setup lang="ts">
import piniaLogo from '@/assets/img/pinia_logo.svg'

const language = [
  {
    name: 'HTML5',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg',
  },
  {
    name: 'CSS',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/css3/css3-original.svg',
  },
   {
    name: 'Tailwind',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/tailwindcss/tailwindcss-original.svg',
  },
  {
    name: 'JavaScript',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg',
  },
  {
    name: 'TypeScript',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/typescript/typescript-original.svg',
  },
  {
    name: 'Vue.js',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/vuejs/vuejs-original.svg',
  },
  {
    name: 'Pinia',
    img: piniaLogo,
  },
  {
    name: 'React',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/react/react-original.svg',
  },
  {
    name: 'C#',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg',
  },
  {
    name: '.NET',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg',
  },
]
const application = [
  {
    name: 'Visual Studio Code',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/vscode/vscode-original.svg',
  },
  {
    name: 'Visual Studio',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/visualstudio/visualstudio-plain.svg',
  },
  {
    name: 'Git',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/git/git-original.svg',
  },
  {
    name: 'SQL Server',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg',
  },
  {
    name: 'Postman',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postman/postman-original.svg',
  },
  {
    name: 'Figma',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/figma/figma-original.svg',
  },

  {
    name: 'Unity',
    img: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/unity/unity-original.svg',
  },
]
</script>

<template>
  <div>
    <!-- Header -->
    <div class="flex">
      <div class="text-4xl font-light tracking-widest">Technologies</div>
      <div class="flex items-center w-full pl-4">
        <v-divider :thickness="5" class="border-opacity-100"></v-divider>
      </div>
    </div>

    <!-- Content -->
    <div class="mt-8">
      <!-- LANGUAGE -->
      <div>
        <div class="text-2xl font-light tracking-wider">Language & Framework</div>
        <div class="grid grid-cols-5 gap-4 mt-2">
          <div v-for="(item, index) in language" :key="index" class="flex">
            <div class="flex items-center justify-center">
              <img :src="item.img" alt="Alt Icon" class="inline-block w-10 h-10 mr-2" />
            </div>
            <div class="flex items-center pl-2 text-lg opacity-75">
              {{ item.name }}
            </div>
          </div>
        </div>
      </div>

      <!-- APPLICATION -->
      <div class="mt-8">
        <div class="text-2xl font-light tracking-wider">Application</div>
        <div class="grid grid-cols-5 gap-4 mt-2">
          <div v-for="(item, index) in application" :key="index" class="flex">
            <div class="flex items-center justify-center">
              <img :src="item.img" alt="Alt Icon" class="inline-block w-10 h-10 mr-2" />
            </div>
            <div class="flex items-center pl-2 text-lg opacity-75">
              {{ item.name }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

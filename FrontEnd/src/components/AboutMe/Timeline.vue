<template>
  <div>
    <!-- HEADER -->
    <div class="flex">
      <div class="text-4xl font-light tracking-widest text-nowrap w-fit">Past employment</div>
      <div class="flex items-center w-full pl-4">
        <v-divider :thickness="5" class="border-opacity-100"></v-divider>
      </div>
    </div>

    <!-- CONTENT -->
    <div class="flex justify-start mt-8">
      <v-timeline align="center" side="end">
        <v-timeline-item
          v-for="(item, index) in timeline"
          :key="index"
          :dot-color="item.color"
          :icon="item.icon"
          fill-dot
        >
          <template v-slot:opposite>
            <div>
              {{ item.date }}
            </div>
          </template>
          <div v-for="(item2, index2) in item.jobs" :key="index2">
            <div>
              {{ item2.title }}
            </div>
            <div>
              {{ item2.company }}
            </div>
          </div>
        </v-timeline-item>
      </v-timeline>
    </div>
  </div>
</template>

<script setup lang="ts">
const timeline = [
  {
    date: '04/2025 - 06/2025',
    color: 'red',
    icon: 'mdi-robot-industrial',
    jobs: [
      {
        title: 'EIF SM Assistant',
        company: 'LG CNS',
      },
    ],
  },
  {
    date: '07/2024 - 04/2025',
    color: 'red',
    icon: 'mdi-gamepad-variant',
    jobs: [
      {
        title: 'Unity Developer',
        company: 'Freelance',
      },
    ],
  },
  {
    date: '05/2022 - 06/2024',
    color: 'red',
    icon: 'mdi-code-block-tags',
    jobs: [
      {
        title: 'Front End Developer',
        company: 'ICOMM Media & Tech., Jsc',
      },
      {
        title: 'Fullstack Developer',
        company: 'Onthedesk',
      },
    ],
  },
]
</script>

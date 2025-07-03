<template>
  <div>
    <el-popover placement="bottom-end" :width="300" trigger="click">
      <template #reference>
        <v-badge color="error" :content="scenarioIncident.length" class="cursor-pointer">
          <v-icon icon="mdi-bell-outline"></v-icon>
        </v-badge>
      </template>
      <div class="space-y-2 overflow-scroll bg-white max-h-96">
        <v-card
          v-for="(item, index) in scenarioIncident"
          :title="item.name"
          variant="elevated"
          :key="index"
          class="cursor-pointer"
        >
          <template v-slot:subtitle>
            <v-chip :color="getColor(item.severity)">{{ getText(item.severity) }}</v-chip>
          </template>
          <template v-slot:text>
            <div>
              {{ item.description }}
            </div>
          </template>
        </v-card>
      </div>
    </el-popover>
  </div>
</template>

<script lang="ts" setup>
import emitter from '@/emitter'
import { SMARTCITY_SCENARIO_SIGNALR } from '@/enums/emitterKey'
import { useSmartCity } from '@/stores/smartcity'
import type { Incident } from '@/types/smartcity'
import { ElNotification } from 'element-plus'
import { computed, h } from 'vue'

const smartCityStore = useSmartCity()
const scenarioIncident = computed(() => {
  return smartCityStore.notifications
})

emitter.on(SMARTCITY_SCENARIO_SIGNALR, (data: any) => {
  try {
    const temp = { ...data }
    if (typeof temp.latLng == 'string') temp.latLng = JSON.parse(temp.latLng)
    const newIncident: Incident = temp
    scenarioIncident.value.push(newIncident)
    ElNotification({
      title: temp.name,
      message: h('div', { class: 'flex flex-col' }, [
        h('div', {}, temp.description),
        h(
          'div',
          {
            class: 'px-2 mt-1 rounded-md w-fit text-white',
            style: `background-color:${getColor(temp.severity)}`,
          },
          getText(temp.severity),
        ),
      ]),
      duration: 5000,
    })
  } catch (e) {
    console.error(e)
  }
})
function getText(severity: number): string {
  switch (severity) {
    case 1:
      return 'Under control'
    case 2:
      return 'Need attention'
    case 3:
      return 'Dangerous'
    case 4:
      return 'Catastrophic'
    default:
      return 'Unknown'
  }
}
function getColor(severity: number): string {
  switch (severity) {
    case 1:
      return 'green'
    case 2:
      return 'brown'
    case 3:
      return 'orange'
    case 4:
      return 'red'
    default:
      return 'grey'
  }
}
</script>

<template>
  <div
    class="absolute flex flex-col items-end space-y-4 rounded-md top-1/4 right-8 w-fit"
    style="z-index: 1000"
  >
    <v-btn
      variant="elevated"
      :color="selectedLegend.includes(FILTER_POI.INCIDENT) ? 'blue' : 'white'"
      density="comfortable"
      icon="mdi-map-marker-alert"
      @click="selectLegend(FILTER_POI.INCIDENT)"
    >
    </v-btn>
    <v-btn
      variant="elevated"
      :color="selectedLegend.includes(FILTER_POI.FIRE_STATION) ? 'blue' : 'white'"
      density="comfortable"
      icon="mdi-fire-hydrant"
      @click="selectLegend(FILTER_POI.FIRE_STATION)"
    >
    </v-btn>
    <v-btn
      variant="elevated"
      :color="selectedLegend.includes(FILTER_POI.POLICE) ? 'blue' : 'white'"
      density="comfortable"
      icon="mdi-police-badge"
      @click="selectLegend(FILTER_POI.POLICE)"
    >
    </v-btn>
    <v-btn
      variant="elevated"
      :color="selectedLegend.includes(FILTER_POI.HOSPITAL) ? 'blue' : 'white'"
      density="comfortable"
      icon="mdi-hospital-building"
      @click="selectLegend(FILTER_POI.HOSPITAL)"
    >
    </v-btn>
  </div>
</template>

<script setup lang="ts">
import { useSmartCity } from '@/stores/smartcity'
import { computed } from 'vue'
import { FILTER_POI } from '@/enums/smartcity'
import emitter from '@/emitter'
import { SMARTCITY_LEGEND_FILTER } from '@/enums/emitterKey'

const useSmartCityStore = useSmartCity()
const selectedLegend = computed(() => useSmartCityStore.selectedLegend)

function selectLegend(value: string) {
  if (selectedLegend.value.includes(value)) {
    const index = selectedLegend.value.indexOf(value)
    selectedLegend.value.splice(index, 1)
  } else {
    selectedLegend.value.push(value)
  }
  useSmartCityStore.selectedLegend = selectedLegend.value
  emitter.emit(SMARTCITY_LEGEND_FILTER)
}
</script>

<style scoped></style>

<template>
  <div class="absolute bg-white bg-opacity-50 rounded-md right-8 bottom-8" style="z-index: 1000">
    <el-card v-if="useSmartCityStore.selectedCity" class="rounded-md">
      <div class="flex flex-col items-start tracking-wide w-52">
        <div class="text-2xl font-semibold">
          {{ useSmartCityStore.selectedCity?.name }}
        </div>
        <div class="flex flex-col items-start mt-2 text-lg leading-normal tracking-wide">
          <div class="flex items-center space-x-2">
            <v-icon class="!opacity-75" size="small" icon="mdi-human-male-female-child"></v-icon>
            <div>
              {{ useSmartCityStore.selectedCity?.population.toLocaleString() }}
            </div>
          </div>
          <div class="flex items-center space-x-2">
            <v-icon class="!opacity-75" size="small" icon="mdi-city"></v-icon>
            <div>
              {{ useSmartCityStore.selectedCity?.urban.toLocaleString() }}
            </div>
            <div class="font-light">
              {{
                getPercentage(
                  useSmartCityStore.selectedCity?.urban,
                  useSmartCityStore.selectedCity?.population,
                )
              }}
            </div>
          </div>
          <div class="flex items-center space-x-2">
            <v-icon class="!opacity-75" size="small" icon="mdi-home-silo"></v-icon>
            <div>
              {{ useSmartCityStore.selectedCity?.rural.toLocaleString() }}
            </div>
            <div class="font-light">
              {{
                getPercentage(
                  useSmartCityStore.selectedCity?.rural,
                  useSmartCityStore.selectedCity?.population,
                )
              }}
            </div>
          </div>
          <div class="flex items-center space-x-2">
            <v-icon class="!opacity-75" size="small" icon="mdi-vector-square-close"></v-icon>
            <div>
              {{ useSmartCityStore.selectedCity?.area.toLocaleString() }}
            </div>
            <div class="font-light">(km²)</div>
          </div>
          <div class="flex items-center space-x-2">
            <v-icon class="!opacity-75" size="small" icon="mdi-currency-usd"></v-icon>
            <div>
              {{ useSmartCityStore.selectedCity?.GRDP.toLocaleString() }}
            </div>
            <div class="font-light">billion</div>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script lang="ts" setup>
import { useSmartCity } from '@/stores/smartcity'

const useSmartCityStore = useSmartCity()

function getPercentage(first: number | undefined, second: number | undefined): string {
  let final: string = ''
  if (first && second) {
    final = ((first / second) * 100).toFixed(1).toString()
  }
  return `(${final}%)`
}
</script>

<style scoped>
::v-deep(.el-card__body) {
  padding: 4px 8px;
}
</style>

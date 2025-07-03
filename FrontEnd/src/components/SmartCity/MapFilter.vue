<template>
  <div class="absolute rounded-md top-4 right-8" style="z-index: 1000">
    <el-card class="rounded-md">
      <v-tabs v-model="useSmartCityStore.tab" bg-color="primary">
        <v-tab :value="FILTER_TAB.OVERVIEW">Overview</v-tab>
        <v-tab :value="FILTER_TAB.REGION">Region</v-tab>
      </v-tabs>
      <v-tabs-window v-model="useSmartCityStore.tab" class="px-2 pt-4 pb-2 w-96">
        <v-tabs-window-item value="overview">
          <div class="flex flex-col items-end">
            <div class="flex justify-center w-full">
              <el-radio-group v-model="useSmartCityStore.overview" size="large">
                <el-radio-button label="Population" value="population" />
                <el-radio-button label="Urban" value="urban" />
                <el-radio-button label="Rural" value="rural" />
                <el-radio-button label="GRDP" value="grdp" />
              </el-radio-group>
            </div>
            <v-btn class="mt-4" color="blue" flat variant="text" @click="RefreshOverview">
              <v-icon icon="mdi-refresh"></v-icon>
            </v-btn>
          </div>
        </v-tabs-window-item>
        <v-tabs-window-item value="region">
          <div>
            <el-select v-model="selectedCity" placeholder="Select city">
              <el-option
                v-for="item in cities"
                :key="item.code"
                :label="item.name"
                :value="item.code"
                :disabled="item.code != '27'"
              />
            </el-select>

            <div class="flex justify-end mt-2">
              <v-btn color="blue" flat @click="applyFilter">Apply</v-btn>
            </div>
          </div>
        </v-tabs-window-item>
      </v-tabs-window>
    </el-card>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, ref } from 'vue'
import { fetchCity } from '@/api/smartcity'
import { useSmartCity } from '@/stores/smartcity'
import emitter from '@/emitter'
import { SMARTCITY_OVERVIEW_REFRESH, SMARTCITY_REGION_FILTER } from '@/enums/emitterKey'
import { FILTER_TAB } from '@/enums/smartcity'

const useSmartCityStore = useSmartCity()
const selectedCity = ref<string | null>(null)
const cities = ref<{ code: string; name: string }[]>()

onMounted(() => {
  getCity()
})

async function getCity() {
  const params = {
    offset: 0,
    limit: 10,
  }
  const res = await fetchCity(params)
  if (res) {
    cities.value = res.data
  }
}
function RefreshOverview() {
  emitter.emit(SMARTCITY_OVERVIEW_REFRESH)
}

function applyFilter() {
  if (selectedCity.value) {
    const features = useSmartCityStore.original_geojson.features.filter(
      (e: any) => e.properties.gid == selectedCity.value,
    )
    useSmartCityStore.region_geojson.features = features
    emitter.emit(SMARTCITY_REGION_FILTER, selectedCity.value)
  }
}
</script>

<style scoped>
::v-deep(.el-card__body) {
  padding: 0;
}
</style>

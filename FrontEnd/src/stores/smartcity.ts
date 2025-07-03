import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { geojson_string } from './mock.ts'
import type { CityData, Incident, ListRequest, POI } from '@/types/smartcity.ts'
import { FILTER_TAB, FILTER_POI } from '@/enums/smartcity.ts'
import { fetchIncident, fetchPoi } from '@/api/smartcity.ts'
export const useSmartCity = defineStore('smartcity', () => {
  const notifications = ref<Incident[]>([])
  const original_geojson = JSON.parse(geojson_string)
  const region_geojson = ref(JSON.parse(geojson_string))
  const selectedCity = ref<CityData | null>()
  const overview = ref('population')
  const tab = ref<string>(FILTER_TAB.OVERVIEW)
  const selectedLegend = ref<string[]>([FILTER_POI.INCIDENT])
  const poiList = ref<POI[]>()
  const incidentList = ref<{ data: Incident[]; from: number; to: number; total: number }>()

  async function getPOI(params: ListRequest) {
    const res = await fetchPoi(params)
    if (res) {
      poiList.value = res.data
    }
  }
  async function getIncident(params: ListRequest) {
    const res = await fetchIncident(params)
    if (res) {
      incidentList.value = res
    }
  }
  return {
    notifications,
    original_geojson,
    region_geojson,
    overview,
    selectedLegend,
    tab,
    selectedCity,
    poiList,
    incidentList,
    getPOI,
    getIncident,
  }
})

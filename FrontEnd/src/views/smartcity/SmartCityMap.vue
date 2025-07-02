<template>
  <div class="relative h-full">
    <Filter></Filter>
    <Info></Info>
    <Legend v-show="useSmartCityStore.tab == FILTER_TAB.REGION"> </Legend>
    <Scenario></Scenario>
    <Map></Map>
  </div>
</template>

<script setup lang="ts">
import Map from '@/components/SmartCity/Map.vue'
import Filter from '@/components/SmartCity/MapFilter.vue'
import Info from '@/components/SmartCity/MapInfo.vue'
import Legend from '@/components/SmartCity/MapLegend.vue'
import Scenario from '@/components/SmartCity/TestSignalR.vue'
import { FILTER_TAB } from '@/enums/smartcity'
import { useSmartCity } from '@/stores/smartcity'

const useSmartCityStore = useSmartCity()
</script>

<template>
  <div id="map"></div>
</template>

<script setup lang="ts">
import leaflet from 'leaflet'
import { computed, onMounted, ref, watch } from 'vue'
import { useSmartCity } from '@/stores/smartcity'
import emitter from '@/emitter'
import {
  SMARTCITY_LEGEND_FILTER,
  SMARTCITY_OVERVIEW_REFRESH,
  SMARTCITY_REGION_FILTER,
  SMARTCITY_SCENARIO_SIGNALR,
} from '@/enums/emitterKey'
import { FILTER_POI, FILTER_TAB } from '@/enums/smartcity'

let map: leaflet.Map
const useSmartCityStore = useSmartCity()
let geojson: any

let scenarioIncidentLayer: any
let scenarioIncidentMarkers: any[] = []

let incidentLayer: any
let hospitalLayer: any
let policeLayer: any
let fireStationLayer: any

onMounted(() => {
  map = leaflet
    .map('map', { zoomControl: false })
    .setView([20.84339525070901, 106.67629146250825], 10)
  map.on('contextmenu', function (e: any) {
    console.log(e.latlng)
    leaflet.marker([e.latlng.lat, e.latlng.lng]).addTo(map)
  })

  initMap()

  emitter.on(SMARTCITY_OVERVIEW_REFRESH, () => {
    map.eachLayer(function (layer: any) {
      map.removeLayer(layer) // Remove existing layers
    })
    initMap()
  })
  emitter.on(SMARTCITY_REGION_FILTER, (value: any) => {
    map.eachLayer(function (layer: any) {
      map.removeLayer(layer) // Remove existing layers
    })
    leaflet
      .tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap',
      })
      .addTo(map)
    geojson = leaflet
      .geoJson(useSmartCityStore.region_geojson, {
        style: {
          fillColor: 'blue',
          weight: 2,
          opacity: 1,
          color: 'white',
          dashArray: '3',
          fillOpacity: 0.1,
        },
        onEachFeature: onEachFeature,
      })
      .addTo(map)
    getPOI()
    getIncident(value)
  })
  emitter.on(SMARTCITY_LEGEND_FILTER, () => {
    if (hospitalLayer) map.removeLayer(hospitalLayer)
    if (policeLayer) map.removeLayer(policeLayer)
    if (fireStationLayer) map.removeLayer(fireStationLayer)
    if (incidentLayer) map.removeLayer(incidentLayer)

    if (useSmartCityStore.selectedLegend.includes(FILTER_POI.HOSPITAL)) {
      map.addLayer(hospitalLayer)
    }
    if (useSmartCityStore.selectedLegend.includes(FILTER_POI.POLICE)) {
      map.addLayer(policeLayer)
    }
    if (useSmartCityStore.selectedLegend.includes(FILTER_POI.FIRE_STATION)) {
      map.addLayer(fireStationLayer)
    }
    if (useSmartCityStore.selectedLegend.includes(FILTER_POI.INCIDENT)) {
      map.addLayer(incidentLayer)
    }
  })
  emitter.on(SMARTCITY_SCENARIO_SIGNALR, (data: any) => {
    try {
      scenarioIncidentMarkers.push(
        leaflet
          .marker([JSON.parse(data.latLng)[0], JSON.parse(data.latLng)[1]])
          .bindPopup(data.name),
      )
      scenarioIncidentLayer = leaflet.layerGroup(scenarioIncidentMarkers)
      map.addLayer(scenarioIncidentLayer)
    } catch (e) {
      console.error(e)
    }
  })
})
const overview = computed(() => {
  return useSmartCityStore.overview
})

watch(overview, (oldValue, newValue) => {
  map.eachLayer(function (layer: any) {
    map.removeLayer(layer) // Remove existing layers
  })
  initMap()
})

function initMap() {
  leaflet
    .tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '© OpenStreetMap',
    })
    .addTo(map)

  geojson = leaflet
    .geoJson(useSmartCityStore.original_geojson, {
      style: style,
      onEachFeature: onEachFeature,
    })
    .addTo(map)
}

// #region | CUSTOM MARKERS
async function getPOI() {
  if (hospitalLayer) map.removeLayer(hospitalLayer)
  if (policeLayer) map.removeLayer(policeLayer)
  if (fireStationLayer) map.removeLayer(fireStationLayer)

  let hospitalMarkers: any[] = []
  let policeMarkers: any[] = []
  let fireStationMarkers: any[] = []
  const params = {
    offset: 0,
    limit: 100,
  }
  await useSmartCityStore.getPOI(params)
  useSmartCityStore.poiList?.forEach((element) => {
    element.latLng = JSON.parse(element.latLng)
    if (element.type == FILTER_POI.HOSPITAL)
      hospitalMarkers.push(
        leaflet.marker([element.latLng[0], element.latLng[1]]).bindPopup(element.name),
      )
    if (element.type == FILTER_POI.POLICE)
      policeMarkers.push(
        leaflet.marker([element.latLng[0], element.latLng[1]]).bindPopup(element.name),
      )
    if (element.type == FILTER_POI.FIRE_STATION)
      fireStationMarkers.push(
        leaflet.marker([element.latLng[0], element.latLng[1]]).bindPopup(element.name),
      )
  })
  hospitalLayer = leaflet.layerGroup(hospitalMarkers)
  policeLayer = leaflet.layerGroup(policeMarkers)
  fireStationLayer = leaflet.layerGroup(fireStationMarkers)

  if (useSmartCityStore.selectedLegend.includes(FILTER_POI.HOSPITAL)) {
    map.addLayer(hospitalLayer)
  }
  if (useSmartCityStore.selectedLegend.includes(FILTER_POI.POLICE)) {
    map.addLayer(policeLayer)
  }
  if (useSmartCityStore.selectedLegend.includes(FILTER_POI.FIRE_STATION)) {
    map.addLayer(fireStationLayer)
  }
}

async function getIncident(cityCode: string) {
  if (incidentLayer) map.removeLayer(incidentLayer)
  let incidentMarkers: any[] = []
  const params = {
    offset: 0,
    limit: 100,
    cityCode: [cityCode],
  }
  await useSmartCityStore.getIncident(params)
  useSmartCityStore.incidentList?.data.forEach((element: any) => {
    element.latLng = JSON.parse(element.latLng)
    incidentMarkers.push(
      leaflet.marker([element.latLng[0], element.latLng[1]]).bindPopup(element.name),
    )
  })
  incidentLayer = leaflet.layerGroup(incidentMarkers)
  if (useSmartCityStore.selectedLegend.includes(FILTER_POI.INCIDENT)) {
    map.addLayer(incidentLayer)
  }
}

// #endregion

// #region | MAP FEATURES
function onEachFeature(feature: any, layer: any) {
  layer.on({
    mouseover: highlightFeature,
    mouseout: resetHighlight,
    click: zoomToFeature,
  })
}
function zoomToFeature(e: any) {
  // map.fitBounds(e.target.getBounds())
}
function highlightFeature(e: any) {
  const layer = e.target

  layer.setStyle({
    weight: 2,
    color: '#666',
    dashArray: '',
    fillOpacity: useSmartCityStore.tab == FILTER_TAB.OVERVIEW ? 0.6 : 0.1,
  })

  layer.bringToFront()

  useSmartCityStore.selectedCity = {
    name: layer.feature.properties.ten_tinh,
    ...layer.feature.properties.data,
  }
}
function resetHighlight(e: any) {
  geojson.resetStyle(e.target)
}
function style(feature: any) {
  return {
    fillColor:
      overview.value == 'population'
        ? getColorPopulation(feature.properties.data.population)
        : overview.value == 'urban'
          ? getColorUrban(feature.properties.data)
          : overview.value == 'rural'
            ? getColorRural(feature.properties.data)
            : getColorMoney(feature.properties.data.GRDP),
    weight: 2,
    opacity: 1,
    color: 'white',
    dashArray: '3',
    fillOpacity: 0.6,
  }
}
function getColorPopulation(d: any) {
  return d > 10_000_000
    ? '#800026'
    : d > 7_500_000
      ? '#BD0026'
      : d > 5_000_000
        ? '#E31A1C'
        : d > 2_500_000
          ? '#FC4E2A'
          : d > 1_000_000
            ? '#FD8D3C'
            : d > 200_000
              ? '#FEB24C'
              : '#FFEDA0'
}
function getColorUrban(d: any) {
  const result = Math.ceil((d.urban / d.population) * 100)
  return result >= 100
    ? '#800026'
    : result > 75
      ? '#BD0026'
      : result > 50
        ? '#FC4E2A'
        : result > 25
          ? '#FD8D3C'
          : result > 0
            ? '#FEB24C'
            : '#FFEDA0'
}
function getColorRural(d: any) {
  const result = Math.ceil((d.rural / d.population) * 100)
  return result >= 100
    ? '#800026'
    : result > 75
      ? '#BD0026'
      : result > 50
        ? '#FC4E2A'
        : result > 25
          ? '#FD8D3C'
          : result > 0
            ? '#FEB24C'
            : '#FFEDA0'
}
function getColorMoney(d: any) {
  return d > 50
    ? '#1b4332'
    : d > 45
      ? '#588157'
      : d > 30
        ? '#2d6a4f'
        : d > 15
          ? '#52b788'
          : d > 5
            ? '#74c69d'
            : '#95d5b2'
}
// #endregion
</script>

<style scoped>
#map {
  height: 100%;
  width: 100%;
}
</style>

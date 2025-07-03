<template>
  <div>
    <v-tabs v-model="tab" bg-color="primary">
      <v-tab value="incident">Incident</v-tab>
      <v-tab value="city">City</v-tab>
    </v-tabs>

    <div v-if="tab == 'incident'" class="flex flex-col m-4 space-y-4">
      <div class="grid grid-cols-2 gap-4">
        <EChartComponent
          title="Incident status"
          subtitle="Status of incidents"
          :key="incidentSolveChart"
          :chartData="incidentSolveChart"
        />
        <EChartComponent
          title="Incident severity"
          subtitle="Severity of incidents"
          :key="incidentSeverityChart"
          :chartData="incidentSeverityChart"
        />
      </div>
      <EChartComponent
        title="Incident by city"
        subtitle="Incident by city"
        :key="incidentCityChart"
        :chartData="incidentCityChart"
      />
    </div>

    <div v-if="tab == 'city'" class="flex flex-col m-4 space-y-4">
      <EChartComponent
        title="Urbanize"
        subtitle="Percentage of urban and rural population"
        :key="citiesChart"
        :chartData="citiesChart"
      />
      <div class="grid grid-cols-2 gap-4">
        <EChartComponent
          title="Population"
          subtitle="Total population"
          :key="citiesPopulationChart"
          :chartData="citiesPopulationChart"
        />
        <EChartComponent
          title="GRDP"
          subtitle="Gross Regional Domestic Product"
          :key="citiesGRDPChart"
          :chartData="citiesGRDPChart"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, ref } from 'vue'
import EChartComponent from '../Echart.vue'
import {
  fetchIncidentTotalByCity,
  fetchIncidentTotalBySeverity,
  fetchIncidentTotalBySolved,
} from '@/api/smartcity'
import { useSmartCity } from '@/stores/smartcity'

const useSmartCityStore = useSmartCity()
const tab = ref<string>('')

const citiesChart = ref({})
const citiesPopulationChart = ref({})
const citiesGRDPChart = ref({})
const incidentSolveChart = ref({})
const incidentSeverityChart = ref({})
const incidentCityChart = ref({})

const incidentTotalBySolved = ref()
const incidentTotalBySeverity = ref()
const incidentTotalByCity = ref()
const cities = computed<any[]>(() => {
  return useSmartCityStore.original_geojson.features.map((e: any) => e.properties)
})

onMounted(() => {
  getData()
})

async function getData() {
  incidentTotalBySolved.value = await fetchIncidentTotalBySolved()
  incidentTotalBySeverity.value = await fetchIncidentTotalBySeverity()
  incidentTotalByCity.value = await fetchIncidentTotalByCity()

  mapCitiesUrbanize()
  mapCityPopulation()
  mapCityGRDP()

  mapIncidentSolve()
  mapIncidentSeverity()
  mapIncidentCity()
}

function mapCitiesUrbanize() {
  let rawData: any[] = []
  rawData[0] = cities.value.map((e: any) => e.data.rural)
  rawData[1] = cities.value.map((e: any) => e.data.urban)
  let totalData: any[] = cities.value.map((e: any) => e.data.population)
  citiesChart.value = {
    legend: {
      selectedMode: false,
    },
    grid: {
      left: 100,
      right: 100,
      top: 50,
      bottom: 50,
    },
    yAxis: {
      type: 'value',
    },
    xAxis: {
      type: 'category',
      data: cities.value.map((e: any) => e.ten_tinh),
    },
    series: ['Rural', 'Urban'].map((name, sid) => {
      return {
        name,
        type: 'bar',
        stack: 'total',
        barWidth: '60%',
        label: {
          show: true,
          formatter: (params: any) => Math.round(params.value * 1000) / 10 + '%',
        },
        data: rawData[sid].map((item: any, index: number) => item / totalData[index]),
      }
    }),
  }
}
function mapCityPopulation() {
  citiesPopulationChart.value = {
    xAxis: {
      type: 'category',
      data: cities.value.map((e: any) => e.ten_tinh),
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        label: {
          show: true,
          formatter: (params: any) => params.value.toLocaleString(),
        },
        data: cities.value.map((e: any) => e.data.population),
        type: 'bar',
      },
    ],
  }
}
function mapCityGRDP() {
  citiesGRDPChart.value = {
    tooltip: {
      trigger: 'item',
    },
    legend: {
      top: '5%',
      left: 'center',
    },
    series: [
      {
        name: 'GRDP',
        type: 'pie',
        radius: ['40%', '70%'],
        avoidLabelOverlap: false,
        label: {
          show: false,
          position: 'center',
        },
        emphasis: {
          label: {
            show: true,
            fontSize: 15,
            fontWeight: 'bold',
            formatter: (params: any) => params.value + `\n billion USD`,
          },
        },
        labelLine: {
          show: false,
        },
        data: cities.value.map((e) => {
          return {
            value: e.data.GRDP,
            name: e.ten_tinh,
          }
        }),
      },
    ],
  }
}
function mapIncidentSolve() {
  incidentSolveChart.value = {
    tooltip: {
      trigger: 'item',
    },
    legend: {
      orient: 'vertical',
      left: 'left',
    },
    series: [
      {
        name: 'Incidents',
        type: 'pie',
        radius: '50%',
        data: [
          {
            value: incidentTotalBySolved.value[0].total,
            name: 'Unsolved',
          },
          {
            value: incidentTotalBySolved.value[1].total,
            name: 'Solved',
          },
        ],

        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)',
          },
        },
      },
    ],
  }
}
function mapIncidentSeverity() {
  incidentSeverityChart.value = {
    tooltip: {
      trigger: 'item',
    },
    legend: {
      orient: 'vertical',
      left: 'left',
    },
    series: [
      {
        name: 'Incidents',
        type: 'pie',
        radius: '50%',
        data: incidentTotalBySeverity.value.map((e) => {
          return {
            value: e.total,
            name: getText(e.severity),
          }
        }),

        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)',
          },
        },
      },
    ],
  }
}
function mapIncidentCity() {
  incidentCityChart.value = {
    xAxis: {
      type: 'category',
      data: incidentTotalByCity.value.map((e: any) => e.cityName),
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        label: {
          show: true,
          formatter: (params: any) => params.value.toLocaleString(),
        },
        data: incidentTotalByCity.value.map((e: any) => e.total),
        type: 'bar',
      },
    ],
  }
}
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
</script>

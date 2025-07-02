<template>
  <div>
    <!-- FILTER -->
    <v-card class="!p-2 m-2 bg-white rounded-sm">
      <div class="grid grid-cols-3 gap-4">
        <!--  -->
        <v-text-field v-model="filter.name" density="compact" label="Incident name" clearable>
        </v-text-field>

        <!--  -->
        <v-select v-model="filter.cityCode" :items="locations" label="Location" multiple>
          <template v-slot:prepend-item>
            <v-list-item title="Select All" @click="toggle">
              <template v-slot:prepend>
                <v-checkbox-btn
                  :color="selectSomeLocation ? 'indigo-darken-4' : undefined"
                  :indeterminate="selectSomeLocation && !selectAllLocation"
                  :model-value="selectAllLocation"
                ></v-checkbox-btn>
              </template>
            </v-list-item>
            <v-divider class="mt-2"></v-divider>
          </template>
          <template v-slot:selection="{ item, index }">
            <span class="px-2 bg-white rounded-sm" v-if="index < 3">
              {{ item.title }}
            </span>
            <span v-if="index === 3" class="text-grey text-caption align-self-center">
              (+{{ filter.cityCode.length - 3 }} others)
            </span>
          </template>
        </v-select>

        <!--  -->
        <v-select v-model="filter.severity" :items="severity" label="Severity" multiple>
          <template v-slot:selection="{ item }">
            <span class="px-2 bg-white rounded-sm">
              {{ item.title }}
            </span>
          </template>
        </v-select>
      </div>

      <div class="flex justify-end mr-4">
        <v-btn variant="tonal" color="#4f8bcf" @click="applyFilter()"> Filter </v-btn>
      </div>
    </v-card>

    <!-- DATA -->
    <v-card class="m-2 mt-6" title="Incident List">
      <v-pagination :length="pagination.total" v-model="pagination.page"></v-pagination>
      <v-table fixed-header>
        <thead>
          <tr>
            <th class="text-left">Name</th>
            <th class="text-left">Description</th>
            <th class="text-left">Location</th>
            <th class="text-left">Time</th>
            <th class="text-left">Severity</th>
            <th class="text-left">Solved</th>
            <th class="text-left">Dispatcher</th>
            <th class="text-center">Action</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="item in incidents"
            :key="item.name"
            :class="`${item.solved ? 'bg-green-100' : 'bg-red-100'}`"
          >
            <td>{{ item.name }}</td>
            <td>{{ item.description }}</td>
            <td>{{ item.cityName }}</td>
            <td>{{ formatTime(item.time) }}</td>
            <td>
              <v-chip :color="getColor(item.severity)" class="!text-white">
                {{ getText(item.severity) }}
              </v-chip>
            </td>

            <td>
              <v-icon v-if="item.solved" color="green" icon="mdi-check"></v-icon>
              <v-icon v-else color="red" icon="mdi-close"></v-icon>
            </td>
            <td>{{ item.dispatcher_team_id ? item.dispatcher_team_id : 'N/A' }}</td>
            <td>
              <div class="flex justify-center space-x-2">
                <v-btn variant="tonal" class="!text-sm" color="blue" @click="() => {}">
                  Action
                </v-btn>
                <v-btn variant="tonal" class="!text-sm" color="red" @click="() => {}">
                  Remove
                </v-btn>
              </div>
            </td>
          </tr>
        </tbody>
      </v-table>
      <v-pagination
        :length="pagination.total"
        v-model="pagination.page"
        @click="changePage"
      ></v-pagination>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { fetchCity } from '@/api/smartcity'
import { useSmartCity } from '@/stores/smartcity'
import type { City, Incident } from '@/types/smartcity'
import { ref, computed, watch, onMounted } from 'vue'

const useSmartCityStore = useSmartCity()
const severity = [
  { title: 'Under control', value: 1 },
  { title: 'Need attention', value: 2 },
  { title: 'Dangerous', value: 3 },
  { title: 'Catastrophic', value: 4 },
  { title: 'Unknown', value: 5 },
]
const locations = ref<{ value: string; title: string }[]>([])
const filter = ref<{
  offset: number
  limit: number
  name: string
  cityCode: string[]
  severity: number[]
}>({
  offset: 0,
  limit: 10,
  name: '',
  cityCode: [],
  severity: [],
})
const pagination = ref<{ page: number; total: number }>({ page: 1, total: 0 })
const selectAllLocation = computed(() => {
  return filter.value.cityCode.length === locations.value.length
})
const selectSomeLocation = computed(() => {
  return filter.value.cityCode.length > 0
})
const incidents = ref<Incident[]>([])

onMounted(() => {
  getCity()
  getIncident()
})

watch(pagination.value, () => {
  changePage()
})

async function getIncident() {
  console.log(filter.value)
  await useSmartCityStore.getIncident(filter.value)
  if (useSmartCityStore.incidentList) {
    incidents.value = useSmartCityStore.incidentList.data
    pagination.value.total = Math.ceil(useSmartCityStore.incidentList.total / filter.value.limit)
  }
}
function changePage() {
  filter.value.offset = (pagination.value.page - 1) * filter.value.limit
  getIncident()
}
function formatTime(date: Date): string {
  return new Date(date).toLocaleString('en-US')
}
async function getCity() {
  const params = {
    offset: 0,
    limit: 10,
  }
  const res = await fetchCity(params)
  if (res) {
    res.data.forEach((e: City) => {
      locations.value.push({ value: e.code, title: e.name })
    })
  }
}
function applyFilter() {
  filter.value.offset = 0
  pagination.value.page = 1
  getIncident()
}
function toggle() {
  if (selectAllLocation.value) {
    filter.value.cityCode = []
  } else {
    filter.value.cityCode = locations.value.map((e) => e.value)
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

<style scoped>
::v-deep(.v-pagination__list) {
  justify-content: end;
}
</style>

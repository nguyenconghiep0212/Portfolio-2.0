<template>
  <div class="absolute flex flex-col space-y-2 top-4 left-4" style="z-index: 1000">
    <v-btn v-if="!isRunning" elevated color="red" @click="runScenario()"> Run Scenario </v-btn>
    <v-btn v-if="isRunning" elevated color="red" @click="cancelScenario()"> Cancel Scenario </v-btn>
  </div>
</template>

<script lang="ts" setup>
import { connection, StartIncidentRandomizer, CancelIncidentRandomizer } from '@/utils/signalR'
import { ref } from 'vue'

connection

const isRunning = ref<boolean>(false)

function runScenario() {
  if (connection) {
    isRunning.value = true
    StartIncidentRandomizer()
  }
}
function cancelScenario() {
  if (connection) {
    isRunning.value = false
    CancelIncidentRandomizer()
  }
}
</script>

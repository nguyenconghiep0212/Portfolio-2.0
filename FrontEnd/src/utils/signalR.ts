import * as signalR from '@microsoft/signalr'
import { useGlobSetting } from '@/hooks/setting'
import emitter from '@/emitter'
import { SMARTCITY_SCENARIO_SIGNALR } from '@/enums/emitterKey'

const globSetting = useGlobSetting()
export const connection = new signalR.HubConnectionBuilder()
  .withUrl(globSetting.apiUrl + '/IncidentHub')
  .build()

// Start the connection
connection
  .start()
  .then(() => {
    console.log('Connected to SignalR')
  })
  .catch((err) => console.error(err))

// Send message to the hub
export function StartIncidentRandomizer() {
  connection.invoke('SendRandomUnsolvedIncidentLoop').catch((err) => console.error(err))
}

export function CancelIncidentRandomizer() {
  connection.invoke('CancelRandomUnsolvedIncidentLoop').catch((err) => console.error(err))
}

// Receive messages from the hub
connection.on('incidentReceived', function (message) {
  emitter.emit(SMARTCITY_SCENARIO_SIGNALR, message)
})
connection.on('onStartScenario', function (message) {
  console.log(message)
})
connection.on('onCancelScenario', function (message) {
  console.log(message)
})

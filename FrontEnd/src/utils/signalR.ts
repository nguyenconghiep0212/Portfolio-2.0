import * as signalR from '@microsoft/signalr'
import { useGlobSetting } from '@/hooks/setting'

const globSetting = useGlobSetting()
export const connection = new signalR.HubConnectionBuilder()
  .withUrl(globSetting.apiUrl + '/IncidentHub')
  .build()

// Start the connection
connection
  .start()
  .then(() => {
    console.log('Connected to SignalR')
    SendMessage()
  })
  .catch((err) => console.error(err))

// Send message to the hub
export function SendMessage() {
  connection.invoke('SendMessage', 'data1', 'data2').catch((err) => console.error(err))
}

// Receive messages from the hub
connection.on('messageReceived', function (user, message) {
  const li = document.createElement('li')
  li.textContent = `${user}: ${message}`
  console.log(`signalr data: ${user} - ${message}`)
})

export interface Incident {
  id: number
  name: string
  cityName: string
  severity: number
  description?: string
  latlng: number[]
  time: Date
  solved: boolean
  dispatcher_team_id: string
}

export interface City {
  code: string
  name: string
}

export interface CityData {
  name: string
  population: number
  area: number
  urban: number
  rural: number
  GRDP: number
}

export interface ListRequest {
  offset: number
  limit: number
  date?: {
    from: Date
    to: Date
  }
}

export interface DispatcherTeam {
  id: string
  name: string
  members: Dispatcher[]
}

export interface Dispatcher {
  id: string
  name: string
  phone: string
  email: string
}

export interface POI {
  id: number
  name: string
  description: string
  latLng: string
  type: string
}

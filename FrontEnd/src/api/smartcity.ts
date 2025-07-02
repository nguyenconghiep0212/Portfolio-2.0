import { http } from '@/utils/axios'

export const fetchCity = (filter: { offset: Number; limit: Number }) => {
  return http.post(
    { url: '/api/City/list', data: filter },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

export const fetchPoi = (filter: { offset: Number; limit: Number }) => {
  return http.post(
    { url: '/api/Poi/List', data: filter },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

export const fetchIncident = (filter: { offset: Number; limit: Number }) => {
  return http.post(
    { url: '/api/Incident/List', data: filter },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

// #region || CHART
export const fetchIncidentTotalByCity = () => {
  return http.get(
    { url: '/api/Incident/TotalByCity' },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}
export const fetchIncidentTotalBySeverity = () => {
  return http.get(
    { url: '/api/Incident/TotalBySeverity' },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}
export const fetchIncidentTotalBySolved = () => {
  return http.get(
    { url: '/api/Incident/TotalBySolved' },
    { isTransformResponse: false, joinParamsToUrl: false },
  )
}

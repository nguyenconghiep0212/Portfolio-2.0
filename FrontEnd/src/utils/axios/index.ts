import { deepMerge, setObjToUrlParams } from '../index'
import { clone } from 'lodash-es'
import type { AxiosTransform, CreateAxiosOptions } from './axiosTransform'
import { VAxios } from './Axios'
import {
  ContentTypeEnum,
  RequestEnum,
  type ErrorMessageMode,
  type RequestOptions,
  type Result,
} from '@/types/axios.d'
import type { AxiosResponse } from 'axios'
import { ResultEnum } from '@/enums/httpEnum'
import { isString } from '../is'
import { formatRequestDate, joinTimestamp } from './helper'
import { getAccessToken } from '../auth'
import { useGlobSetting } from '@/hooks/setting'

const globSetting = useGlobSetting()
const apiMessage = {
  api: {
    operationFailed: 'Operation failed',
    errorTip: 'Error Tip',
    errorMessage: 'The operation failed, the system is abnormal!',
    timeoutMessage: 'Login timed out, please log in again!',
    apiTimeoutMessage: 'The interface request timed out, please refresh the page and try again!',
    apiRequestFailed: 'The interface request failed, please try again later!',
    networkException: 'network anomaly',
    networkExceptionMsg:
      'Please check if your network connection is normal! The network is abnormal',

    errMsg401: 'The user does not have permission (token, user name, password error)!',
    errMsg403: 'The user is authorized, but access is forbidden!',
    errMsg404: 'Network request error, the resource was not found!',
    errMsg405: 'Network request error, request method not allowed!',
    errMsg408: 'Network request timed out!',
    errMsg500: 'Server error, please contact the administrator!',
    errMsg501: 'The network is not implemented!',
    errMsg502: 'Network Error!',
    errMsg503: 'The service is unavailable, the server is temporarily overloaded or maintained!',
    errMsg504: 'Network timeout!',
    errMsg505: 'The http version does not support the request!',
  },
}
let isRefreshing = false
let failedQueue: any[] = []

const transform: AxiosTransform = {
  transformRequestHook: (res: AxiosResponse<Result>, options: RequestOptions) => {
    const { isTransformResponse, isReturnNativeResponse } = options
    if (isReturnNativeResponse) {
      return res
    }
    if (!isTransformResponse) {
      return res.data
    }

    const { data } = res
    if (!data) {
      throw new Error(apiMessage.api.apiRequestFailed)
    }
    const { statusCode, result, message } = data

    const hasSuccess = data && Reflect.has(data, 'statusCode') && statusCode === ResultEnum.SUCCESS
    if (hasSuccess) {
      return result
    }

    let timeoutMsg = ''
    switch (statusCode) {
      case ResultEnum.TIMEOUT:
        timeoutMsg = apiMessage.api.timeoutMessage

        break
      default:
        if (message) {
          timeoutMsg = message
        }
    }

    throw new Error(timeoutMsg || apiMessage.api.apiRequestFailed)
  },

  beforeRequestHook: (config, options) => {
    const { apiUrl, joinParamsToUrl, formatDate, joinTime = true } = options

    if (apiUrl && isString(apiUrl)) {
      config.url = `${apiUrl}${config.url}`
    }
    const params = config.params || {}
    const data = config.data || false
    formatDate && data && !isString(data) && formatRequestDate(data)
    if (config.method?.toUpperCase() === RequestEnum.GET) {
      if (!isString(params)) {
        config.params = Object.assign(params || {}, joinTimestamp(joinTime, false))
      } else {
        config.url = config.url + params + `${joinTimestamp(joinTime, true)}`
        config.params = undefined
      }
    } else {
      if (!isString(params)) {
        formatDate && formatRequestDate(params)
        if (Reflect.has(config, 'data') && config.data && Object.keys(config.data).length > 0) {
          config.data = data
          config.params = params
        } else {
          config.data = params
          config.params = undefined
        }
        if (joinParamsToUrl) {
          config.url = setObjToUrlParams(
            config.url as string,
            Object.assign({}, config.params, config.data),
          )
        }
      } else {
        config.url = config.url + params
        config.params = undefined
      }
    }
    return config
  },

  requestInterceptors: (config, options) => {
    const token = getAccessToken()
    if (token && (config as Recordable)?.requestOptions?.withToken !== false) {
      ;(config as Recordable).headers.Authorization = options.authenticationScheme
        ? `${options.authenticationScheme} ${token}`
        : token
    }
    return config
  },

  responseInterceptors: (res: AxiosResponse<any>) => {
    return res
  },

  responseInterceptorsCatch: (error: any) => {
    const { response, statusCode, message, config } = error || {}
    const errorMessageMode = config?.requestOptions?.errorMessageMode || 'none'
    const msg: string = response?.data?.error?.message ?? ''
    const err: string = error?.toString?.() ?? ''
    let errMessage = ''

    try {
      if (statusCode === 'ECONNABORTED' && message.indexOf('timeout') !== -1) {
        errMessage = apiMessage.api.apiTimeoutMessage
      }
      if (err?.includes('Network Error')) {
        errMessage = apiMessage.api.networkExceptionMsg
      }

      if (errMessage) {
        return Promise.reject(error)
      }
    } catch (error) {
      throw new Error(error as unknown as string)
    }

    checkStatus(error?.response?.status, msg, errorMessageMode)
    return Promise.reject(error)
  },
}

function checkStatus(
  status: number,
  msg: string,
  errorMessageMode: ErrorMessageMode = 'message',
): void {
  let errMessage = ''

  switch (status) {
    case 400:
      errMessage = `${msg}`
      break
    // 401: Not logged in
    // Jump to the login page if not logged in, and carry the path of the current page
    // Return to the current page after successful login. This step needs to be operated on the login page.
    case 401:
      errMessage = msg || apiMessage.api.errMsg401
      break
    case 403:
      errMessage = apiMessage.api.errMsg403
      break
    // 404请求不存在
    case 404:
      errMessage = apiMessage.api.errMsg404
      break
    case 405:
      errMessage = apiMessage.api.errMsg405
      break
    case 408:
      errMessage = apiMessage.api.errMsg408
      break
    case 500:
      errMessage = apiMessage.api.errMsg500
      break
    case 501:
      errMessage = apiMessage.api.errMsg501
      break
    case 502:
      errMessage = apiMessage.api.errMsg502
      break
    case 503:
      errMessage = apiMessage.api.errMsg503
      break
    case 504:
      errMessage = apiMessage.api.errMsg504
      break
    case 505:
      errMessage = apiMessage.api.errMsg505
      break
    default:
  }

  if (errMessage) {
    if (errorMessageMode === 'modal') {
      console.error(`${apiMessage.api.errorTip}')}: ${errMessage}`)
    } else if (errorMessageMode === 'message') {
      console.error(`${errMessage}`)
    }
  }
}


function createAxios(opt?: Partial<CreateAxiosOptions>) {
  return new VAxios(
    deepMerge(
      {
        // See https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#authentication_schemes
        // authentication schemes，e.g: Bearer
        authenticationScheme: 'Bearer',
        // authenticationScheme: '',
        timeout: 180 * 1000,
        // baseURL: globSetting.apiUrl,

        headers: {
          'Content-Type': ContentTypeEnum.JSON,
        },
        // headers: { 'Content-Type': ContentTypeEnum.FORM_URLENCODED },
        transform: clone(transform),
        requestOptions: {
          isReturnNativeResponse: false,
          isTransformResponse: true,
          joinParamsToUrl: false,
          formatDate: true,
          errorMessageMode: 'message',
          apiUrl: globSetting.apiUrl,
          joinTime: true,
          ignoreCancelToken: true,
          withToken: true,
        },
      },
      opt || {},
    ),
  )
}
export const http = createAxios()

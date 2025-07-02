import type { GlobConfig } from '@/types/config'

import { getAppEnvConfig } from '@/utils/env'
export const useGlobSetting = (): Readonly<GlobConfig> => {
  const { VITE_GLOB_API_URL, ACCESS_TOKEN, REFRESH_TOKEN } = getAppEnvConfig()

  // Take global configuration
  const glob: Readonly<GlobConfig> = {
    apiUrl: VITE_GLOB_API_URL,
    accessToken: ACCESS_TOKEN,
    refreshToken: REFRESH_TOKEN,
  }
  return glob as Readonly<GlobConfig>
}

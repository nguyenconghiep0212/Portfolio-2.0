import type { GlobEnvConfig } from '@/types/config'

export function getAppEnvConfig() {
  const ENV = import.meta.env as unknown as GlobEnvConfig

  const { VITE_GLOB_API_URL, ACCESS_TOKEN, REFRESH_TOKEN } = ENV
  return {
    VITE_GLOB_API_URL,
    ACCESS_TOKEN,
    REFRESH_TOKEN,
  }
}
export function getEnv(): string {
  return import.meta.env.MODE
}

const base = import.meta.env.VITE_STREAM_URL ?? 'http://localhost:8090'

export const usePeerStream = (mediaId) => `${base}/${mediaId}`

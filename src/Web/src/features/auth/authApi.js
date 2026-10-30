import { api } from '../../shared/api/client'

export const login = (body) => api('/auth/login', { method: 'POST', body })
export const register = (body) => api('/auth/register', { method: 'POST', body })

import { useNavigate } from 'react-router-dom'
import { login, register } from './authApi'

export function useAuth() {
  const nav = useNavigate()

  return {
    signIn: async (credentials) => {
      const { token } = await login(credentials)
      localStorage.setItem('token', token)
      nav('/browse')
    },
    signUp: async (data) => {
      await register(data)
      nav('/login')
    },
    signOut: () => {
      localStorage.removeItem('token')
      nav('/')
    }
  }
}

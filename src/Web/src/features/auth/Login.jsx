import { useState } from 'react'
import { Link } from 'react-router-dom'
import AnimatedContent from '../../shared/components/AnimatedContent'
import Navbar from '../../shared/components/Navbar'
import { useAuth } from './useAuth'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const { signIn } = useAuth()

  const submit = async (e) => {
    e.preventDefault()
    setError('')
    try {
      await signIn({ email, password })
    } catch {
      setError('Invalid email or password.')
    }
  }

  return (
    <main className="page auth">
      <Navbar />
      <AnimatedContent trigger="mount" distance={100} duration={0.8} ease="power3.out" stagger={0.08} className="card">
        <h2>Sign In</h2>
        <form onSubmit={submit}>
          <input type="email" placeholder="Email address" value={email} onChange={(e) => setEmail(e.target.value)} required />
          <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required />
          {error && <span className="error">{error}</span>}
          <button className="btn btn-lg" type="submit">Sign In</button>
        </form>
        <p className="muted">New to Stremix? <Link to="/signup">Sign up now</Link></p>
      </AnimatedContent>
    </main>
  )
}

import { useState } from 'react'
import { Link, useLocation } from 'react-router-dom'
import AnimatedContent from '../../shared/components/AnimatedContent'
import Navbar from '../../shared/components/Navbar'
import { useAuth } from './useAuth'

export default function Signup() {
  const { state } = useLocation()
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState(state?.email ?? '')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const { signUp } = useAuth()

  const submit = async (e) => {
    e.preventDefault()
    setError('')
    try {
      await signUp({ username, email, password })
    } catch {
      setError('Could not create account.')
    }
  }

  return (
    <main className="page auth">
      <Navbar>
        <Link to="/login" className="btn btn-sm">Sign In</Link>
      </Navbar>
      <AnimatedContent trigger="mount" distance={100} duration={0.8} ease="power3.out" stagger={0.08} className="card">
        <h2>Create Account</h2>
        <form onSubmit={submit}>
          <input type="text" placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} required />
          <input type="email" placeholder="Email address" value={email} onChange={(e) => setEmail(e.target.value)} required />
          <input type="password" placeholder="Password (min 8 characters)" value={password} onChange={(e) => setPassword(e.target.value)} minLength={8} required />
          {error && <span className="error">{error}</span>}
          <button className="btn btn-lg" type="submit">Sign Up</button>
        </form>
        <p className="muted">Already a member? <Link to="/login">Sign in</Link></p>
      </AnimatedContent>
    </main>
  )
}

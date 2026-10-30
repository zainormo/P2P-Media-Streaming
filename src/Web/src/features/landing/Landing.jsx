import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import AnimatedContent from '../../shared/components/AnimatedContent'
import Navbar from '../../shared/components/Navbar'

export default function Landing() {
  const [email, setEmail] = useState('')
  const nav = useNavigate()

  const submit = (e) => {
    e.preventDefault()
    nav('/signup', { state: { email } })
  }

  return (
    <main className="page hero">
      <Navbar>
        <Link to="/login" className="btn btn-sm">Sign In</Link>
      </Navbar>
      <AnimatedContent
        trigger="mount"
        distance={100}
        direction="vertical"
        duration={0.8}
        ease="power3.out"
        blur={0}
        stagger={0.08}
        threshold={0.1}
        className="hero-content"
      >
        <h1>Unlimited streams, shared by peers, and more</h1>
        <h3>Free to start. Cancel anytime.</h3>
        <p>Ready to watch? Enter your email to create or restart your membership.</p>
        <form className="cta" onSubmit={submit}>
          <input type="email" placeholder="Email address" value={email} onChange={(e) => setEmail(e.target.value)} required />
          <button className="btn btn-lg" type="submit">Get Started ›</button>
        </form>
      </AnimatedContent>
    </main>
  )
}

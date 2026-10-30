import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import AnimatedContent from '../../shared/components/AnimatedContent'
import Navbar from '../../shared/components/Navbar'
import { useAuth } from '../auth/useAuth'
import { getMedia } from './browseApi'

export default function Browse() {
  const [items, setItems] = useState([])
  const { signOut } = useAuth()

  useEffect(() => {
    getMedia().then(setItems).catch(signOut)
  }, [])

  return (
    <main className="page">
      <Navbar>
        <button className="btn btn-sm" onClick={signOut}>Sign Out</button>
      </Navbar>
      <AnimatedContent trigger="mount" distance={100} duration={0.8} ease="power3.out" className="grid">
        {items.map((m) => (
          <Link key={m.id} to={`/watch/${m.id}`} className="tile">
            <h3>{m.title}</h3>
            <p>{m.description}</p>
          </Link>
        ))}
      </AnimatedContent>
    </main>
  )
}

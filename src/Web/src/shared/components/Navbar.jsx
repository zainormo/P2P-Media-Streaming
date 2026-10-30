import { Link } from 'react-router-dom'

export default function Navbar({ children }) {
  return (
    <header className="bar">
      <Link to="/" className="logo">STREMIX</Link>
      {children}
    </header>
  )
}

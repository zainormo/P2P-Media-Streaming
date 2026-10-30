import { Link, useParams } from 'react-router-dom'
import Navbar from '../../shared/components/Navbar'
import { usePeerStream } from './usePeerStream'

export default function Player() {
  const { id } = useParams()
  const src = usePeerStream(id)

  return (
    <main className="page">
      <Navbar>
        <Link to="/browse" className="btn btn-sm">Back</Link>
      </Navbar>
      <video className="player" src={src} controls autoPlay />
    </main>
  )
}

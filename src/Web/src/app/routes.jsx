import Landing from '../features/landing/Landing'
import Login from '../features/auth/Login'
import Signup from '../features/auth/Signup'
import Browse from '../features/browse/Browse'
import Player from '../features/player/Player'
import ProtectedRoute from './ProtectedRoute'

export default [
  { path: '/', element: <Landing /> },
  { path: '/login', element: <Login /> },
  { path: '/signup', element: <Signup /> },
  { path: '/browse', element: <ProtectedRoute><Browse /></ProtectedRoute> },
  { path: '/watch/:id', element: <ProtectedRoute><Player /></ProtectedRoute> }
]

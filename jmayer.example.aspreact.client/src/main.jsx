import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { PrimeReactProvider } from 'primereact/api'
import App from './App.jsx'
import { ErrorProvider } from './components/errorDialog/ErrorProvider.jsx'
import './index.css'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <PrimeReactProvider>
        <ErrorProvider>
            <App />
        </ErrorProvider>
    </PrimeReactProvider>
  </StrictMode>,
)

import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { PrimeReactProvider } from 'primereact/api';
import { ErrorProvider } from './components/errorDialog/ErrorProvider.jsx';
import App from './App.jsx';
import './index.css';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <PrimeReactProvider>
        <ErrorProvider>
            <App />
        </ErrorProvider>
    </PrimeReactProvider>
  </StrictMode>,
)

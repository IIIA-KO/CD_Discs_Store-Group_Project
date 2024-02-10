import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import App from './App.jsx'
import { QueryClient, QueryClientProvider } from 'react-query';
import './index.css'

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')).render(

        <QueryClientProvider client={queryClient}>
    <BrowserRouter>
            <App />
    </BrowserRouter>
        </QueryClientProvider>

)

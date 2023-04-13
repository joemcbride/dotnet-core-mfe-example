import { createRoot } from 'react-dom/client'
import { ApolloProvider } from '@apollo/client'
import App from './App'
import client from './apolloClient'

if (window.initialData && window.initialData !== '@Model') {
  console.log('initial data', window.initialData)
  const decoded = JSON.parse(atob(window.initialData))
  console.log('decoded', decoded)
  localStorage.setItem('csrf-token', decoded.csrfToken)
}

const container = document.getElementById('app')
const root = createRoot(container)
root.render(
  <ApolloProvider client={client}>
    <App />
  </ApolloProvider>
)

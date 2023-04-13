const HttpsAgent = require('agentkeepalive').HttpsAgent
const { createProxyMiddleware } = require('http-proxy-middleware')

module.exports = function (app) {
  app.use(
    createProxyMiddleware('/api', {
      target: 'https://localhost:5001/',
      // ignore certificate errors
      secure: false,
      // change the Host header to the target domain name
      changeOrigin: true,
      // include the x-forwarded* headers
      xfwd: true,
      agent: new HttpsAgent({
        maxSockets: 100,
        keepAlive: true,
        maxFreeSockets: 10,
        keepAliveMsecs: 1000,
        timeout: 60000,
        freeSocketTimeout: 30000,
      }),
      onProxyRes: (proxyRes, req) => {
        const authKey = 'www-authenticate'
        proxyRes.headers[authKey] =
          proxyRes.headers[authKey] && proxyRes.headers[authKey].split(',')
      },
    })
  )
}

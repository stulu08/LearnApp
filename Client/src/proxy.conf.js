const { env } = require('process');


const backendUrl = process.env.BACKEND_URL || 'http://localhost:8080';

const PROXY_CONFIG = [
  {
    context: [
      "/api/**",
    ],
    target: backendUrl,
    secure: false
  }
]

module.exports = PROXY_CONFIG;

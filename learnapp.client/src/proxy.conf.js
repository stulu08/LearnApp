const { env } = require('process');

const target = 'http://localhost:5145'

const PROXY_CONFIG = [
  {
    context: [
      "/api/User/**",
      "/api/Lesson/**",
    ],
    target: target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;

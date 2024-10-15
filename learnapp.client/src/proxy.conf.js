const { env } = require('process');

const target = 'http://localhost:5145'

const PROXY_CONFIG = [
  {
    context: [
      "/user/get/*",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;


export default {
  mode: 'spa',
  /*
  ** Headers of the page
  */
  head: {
    title: process.env.npm_package_name || '',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: process.env.npm_package_description || '' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },
  /*
  ** Customize the progress-bar color
  */
  loading: { color: '#fff' },
  /*
  ** Global CSS
  */
  css: [
    'element-ui/lib/theme-chalk/index.css'
  ],
  /*
  ** Plugins to load before mounting the App
  */
  plugins: [
    '@/plugins/axios',
    '@/plugins/element-ui'
  ],
  /*
  ** Nuxt.js modules
  */
  modules: [
    '@nuxtjs/axios',
    '@nuxtjs/auth'
  ],
  axios: {
    proxy: true, // Can be also an object with default options
    prefix: '/api/',
    credentials: true
  },
  proxy: {
    '/api/': { 
      target: 'https://localhost:44379/', 
      "secure": false, //不检验证书
      pathRewrite: {'^/api/': '/', changeOrigin: true} 
    }
  },
  auth: {
    // Options
    redirect: {
      login: '/login',
      logout: '/login',
      callback: '/login',
      home: '/'
    },
    strategies: {
      local: {
        endpoints: {
          login: { url: 'https://localhost:44379/api/auth/Login', method: 'post' },
          user: { url: 'https://localhost:44379/api/auth/User', method: 'get', propertyName: 'user' },
          reset: { url: 'https://localhost:44379/api/auth/Refresh', method: 'post' },
          logout: { url: 'https://localhost:44379/api/auth/Logout', method: 'post' }
        }
      }
    }
  },
  router: {
    middleware: ['auth']
  },
  /*
  ** Build configuration
  */
  build: {
    transpile: [/^element-ui/],
    /*
    ** You can extend webpack config here
    */
    extend(config, ctx) {
    }
  }
}

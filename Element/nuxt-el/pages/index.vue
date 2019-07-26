<template>
  <div class="container">
    <div>
      <logo />
      <h1 class="title">
        nuxt-el
      </h1>
      <h2 class="subtitle">
        My fantastic Nuxt.js project
      </h2>
      <div class="links">
        <a
          href="https://nuxtjs.org/"
          target="_blank"
          class="button--green"
        >
          Documentation
        </a>
        <a
          href="https://github.com/nuxt/nuxt.js"
          target="_blank"
          class="button--grey"
        >
          GitHub
        </a>
      </div>
      <div>
          User: {{ $auth.hasScope('user') }}
          Test: {{ $auth.hasScope('test') }}
          Admin: {{ $auth.hasScope('admin') }}
          <hr>
          <el-button @click="$auth.fetchUser()">Fetch User</el-button>
          <el-button @click="$auth.logout()">Logout</el-button>
          <el-button ref="testGet" @click="testGet()">测试</el-button>
          <el-button @click="refreshToken()">刷新Token</el-button>
      </div>
    </div>
  </div>
</template>

<script>
import Logo from '~/components/Logo.vue'

let page = {
  components: {
    Logo
  },
  methods: {
    testGet(){
      $nuxt.$axios.get("/api/values").then((res) => {
        console.log(res.data);
        console.log($nuxt.$auth.getToken('local'))
      })
      //setTimeout(() => {
      //  page.methods.testGet()
      //}, 2000);
    },
    refreshToken(){
      
      
      $nuxt.$axios.post("/api/auth/refresh", {}).then((res) => {
        let token = this.$auth.strategies["local"].options.tokenType + ' ' + res.data.token
        this.$auth.setToken("local", token)
        this.$auth.mounted()
        //this.$auth.setUserToken(res.data.token)
        //this.$auth.syncRefreshToken("local")
        console.log($nuxt.$auth.getToken('local'))
      })
      
    }
  }
}
export default page
</script>

<style>
.container {
  margin: 0 auto;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}

.title {
  font-family: 'Quicksand', 'Source Sans Pro', -apple-system, BlinkMacSystemFont,
    'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
  display: block;
  font-weight: 300;
  font-size: 100px;
  color: #35495e;
  letter-spacing: 1px;
}

.subtitle {
  font-weight: 300;
  font-size: 42px;
  color: #526488;
  word-spacing: 5px;
  padding-bottom: 15px;
}

.links {
  padding-top: 15px;
}
</style>

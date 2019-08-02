<template>
  <div id="error">
    {{msg}}
  </div>
</template>

<script>
export default {
  data () {
    return {
      msg: '错误的请求！'
    }
  },
  //middleware: ['auth'],
  options: {
    auth: false
  }
}
</script>

<style scoped>
#error {
  font-size: 32px;
  text-align: center;
  padding-top: 15%;
}
</style>

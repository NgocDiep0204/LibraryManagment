<template>
  <div id="app">
      <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App',
  components: {
    
  },
}
</script>


<style>
    #app {
  margin: 0;
  padding: 0;
  width: 100vw; /* Chiều rộng viewport */
}

.header {
  width: 100%; /* Chiếm 100% chiều rộng của phần tử cha */
  background-color: #2c3e50; /* Màu nền cho header */
  color: white; /* Màu chữ */
  text-align: center;
}
</style>
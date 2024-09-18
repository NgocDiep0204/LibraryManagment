<template>
 <div class="w-full h-screen flex">
    <Sidebar v-if="user" :dataOpenSideBar="openSideBar"/>
    <div class="flex flex-col w-full h-full">
        <HeaderTop :dataOpenSideBar="openSideBar" :clickHambuger="toggleSidebar" />
        <div class="flex-grow flex-1 overflow-auto">
            <router-view  />
        </div>
    </div>
</div>

</template>
HeaderTop
<script>
import Sidebar from './../../components/sidebar.vue'
import HeaderTop from './../../components/header.vue'
import {  mapState, mapActions  } from 'vuex/dist/vuex.cjs.js'
export default {
   
    data() {
        return {
            openSideBar: true,
        };
    },
    components: {
        Sidebar,
        HeaderTop,
       
    },
   
    computed: {
        ...mapState(['user'])
    },
    
    methods: {
        toggleSidebar() {
            this.openSideBar = !this.openSideBar;
        },
       
    },
    

}
</script>

<style>

</style>
<template>
<div class="w-full bg-gray-100">
    <div class="flex justify-between items-center h-[50px] ">
        <div v-if="user" class="p-4 hover:bg-gray-50"  @click="clickHambuger">
            <i class="pi pi-bars" ></i>
        </div>
        <div>
            <div class="text-xl font-bold text-center flex items-center justify-center text-[#002D74] p-2" v-if="!user">
                Management System
            </div>
        </div>
        <!-- <div class="py-2">
            <InputText class="h-[40px] bg-clip-border" type="text" v-model="value" aria-placeholder="seacrch"/>
           
        </div> -->
        <div class="bg-gray-100 rounded-lg">
            <router-link class="p-9 m-4 text-[#002D74] text-[22px] font-bold" :to="{name: 'login'}" v-if="!user">
                Login
            </router-link>
        </div>
        <div class="flex space-x-3 items-center justify-center px-3" v-if="user">
            <div class="text-md">Admin</div>
            <Avatar icon="pi pi-user" shape="circle" class="p-2 m-4" @click="toggle"/>
            <Menu id="overlay_menu" ref="menu" :model="items" :popup="true" />
        </div>
    </div>
</div>
</template>

<script>
import Avatar from 'primevue/avatar';
import InputText from 'primevue/inputtext';
import { data } from 'autoprefixer';
import { mapState } from 'vuex/dist/vuex.cjs.js'
export default {
    props: {
        dataOpenSideBar: Boolean,
        clickHambuger: Function,    
    },
    computed: {
        ...mapState(['user'])
    },
    mounted(){
        this.$store.dispatch('fetchUserInfo');
    },
    data() {
        return {
            value: null,
            items: [
                {
                    label: 'Profile',
                    icon: 'pi pi-user',
                    command: () => {
                        this.$router.push('/profile');
                    }
                },
                {
                    label: 'Logout',
                    icon: 'pi pi-sign-out',
                    command: () => {
                        localStorage.removeItem('token');
                        this.$store.dispatch('logout'); 
                        this.$router.push('/login');
                    }
                }
            ]
        };
    },
    methods: {
        
        toggle(event) {
           this.$refs.menu.toggle(event);
        },
       
    },
}
</script>

<style>

</style>
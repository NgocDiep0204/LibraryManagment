<template>
    <div :user="user">
        this is home
        {{ user }}
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex/dist/vuex.cjs.js';
export default {
    data() {
        return {
          //  user: null,
        };
    },
    computed:{
       ...mapState(['user'])
    },
    mounted(){
        this.fetchUserInfo();
        console.log("Token:", this.user);
    },
    methods:{
        ...mapActions(['fetchUserInfo']),
        // async fetchUserInfo() {
        //     const response = await axiosClient.get('Authenlication/GetUserProfile');
        //     user = response.data;
        // },
     
  }
};
</script>
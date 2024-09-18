<template>
    <section class="bg-gray-300 min-h-screen flex items-center justify-center">
     <!-- Login contaoner -->
      <div class="bg-gray-100 flex rounded-2xl shadow-lg max-w-3xl p-5">
          <!-- Form -->
          <div class="sm:w-1/2 px-16">
             <h2 class="font-bold text-2xl">Login</h2>
             <p class="text-sm mt-4">If you already a member, eassily login</p>
             <form  @submit.prevent="handleSubmit" action="" class="flex flex-col gap-4">
                <div class="relative">
                    <input v-model="email" class="p-2 mt-8 rounded-xl border" type="text" name="email" placeholder="Enter your email">
                    <input v-model="password" class="p-2 mt-8 rounded-xl border" type="password" name="password" placeholder="Enter your password">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="gray" class="bi bi-eye absolute top-1/2 right-3 -translate-y-1/2" viewBox="0 0 16 16">
                        <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8zM1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z" />
                        <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5zM4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0z" />
                    </svg>
                </div>    
                <button class="bg-[#002D74] rounded-xl text-white py-2 hover:scale-105 duration-300 w-full">Login</button>
               
            </form>
           
            <div class="mt-5 text-xs border-b border-[#002D74] py-4 text-[#002D74]">
                <a href="#">Forgot your password?</a>
            </div>
          </div >
          <!-- Image -->
          <div class="w-1/2 p-5 items-center pt-28">
              <img class="sm:block hidden rounded-xl" :src=imgUrl alt="library">
          </div>
      </div>
    </section>
  </template>
  
  <script>
  import libraryImage from '../assets/book.png';
  import axiosClient from '../axiosClient';
  export default {
      data(){
          return{
                email: '',
                password: '',
                imgUrl: libraryImage
          }
      },

      methods: {
        async handleSubmit(){
            if(!this.email || !this.password){
                alert('Please enter email and password');
                return;
            }
            try {
                const response = await axiosClient.post(`Authenlication/Login`, {
                    email: this.email,
                    password: this.password
                });
                if (response.data.token) {
                    const userRole = response.data.userRoles.$values[0]
                    ;
                    console.log("User Role : " , userRole);
               if (userRole === 'Admin') {
                    localStorage.setItem('token', response.data.token);
                    console.log("Token : " , userRole);
                    this.$router.push('/home');
               } else {
                   alert('You do not have the necessary permissions to log in.');
               }
                } else {
                    alert('Login failed. Please check your email and password.');
                }
            } catch (error) {
            if (error.response) {
                console.error('Login error:', error.response.data);
                if (error.response.status === 401) {
                    alert('Incorrect email or password. Please try again.');
                } else {
                    alert('An error occurred during login. Please try again.');
                }
            } else if (error.request) {
                console.error('Login error: No response from server', error.request);
            } else {
                console.error('Login error:', error.message);
            }
        }
    },
  },
}
</script>
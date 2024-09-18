<template>
    <div class="max-w-7xl h-auto mx-auto bg-gray-200 mt-2 shadow-lg">
         <div class=" flex justify-between">
             <div class=" ml-1 flex justify-between w-full">
                 <div class="page-title">
                     <h4 class="ml-2 mb-1 text-[15px] md:text-[20px] font-bold">Product List</h4>
                     <h6 class="ml-2 p-0 text-xs md:text-[13px]">Manage your products</h6>
                 </div>
                 <div class="flex space-x-3 items-center justify-end px-3 w-full">
                     <div class="text-[15px] md:text-[17px]  text-gray-300 bg-gray-800 p-1 rounded-lg">
                         <router-link :to="{name:'addbook'}" class="hover:text-red-400">
                             <span class="pi pi-plus text-[15px] md:text-[17px]"></span>
                             <span class=""> Add new book</span>
                         </router-link>
                     </div>
                 </div>
             </div>
         </div>
 
         <div class="flex items-center mb-4">
             <div class="relative w-full max-w-md mx-auto">
                 <input v-model="keyword" @input="searchstudentsbyname" type="text" class="w-full px-10 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500" placeholder="Search...">
                 <div class="absolute inset-y-0 left-0 flex items-center pl-3">
                     <svg class="w-5 h-5 text-gray-500" fill="currentColor" viewBox="0 0 20 20">
                         <path fill-rule="evenodd" d="M12.9 14.32a8 8 0 111.42-1.42l5.34 5.33a1 1 0 01-1.42 1.42l-5.33-5.33zM10 16a6 6 0 100-12 6 6 0 000 12z" clip-rule="evenodd"/>
                     </svg>
                 </div>
             </div>
         </div>
         <div class="overflow-x-auto px-1 sm:px-3 text-[12px] sm:text-[17px] w-full">
         <table class="min-w-full bg-white rounded-lg sm:pl-[10px] md:pl-[40px] lg:pl-[60px]">
           <thead>
             <tr>
               <th class="text-left px-4 py-2 border-b border-gray-200">
                 No
               </th>
               <th class="text-left px-4 py-2 border-b border-gray-200">
                 Name
               </th>
               <th class="text-left px-4 py-2 border-b border-gray-200">
                 PhoneNumber
               </th>
               <th class="text-left px-4 py-2 border-b border-gray-200">
                 Addresss
               </th>
               <th class="text-left px-4 py-2 border-b border-gray-200">
                 Actions
               </th>
             </tr>
           </thead>
           <tbody v-if="students && students.length > 0">
             <tr v-for="(student, index) in students" :key="index">
                 <td class="px-4 py-2 border-b border-gray-200 text-left">
                   {{ index + 1 }}
                 </td>
                 <td class="px-4 py-2 border-b border-gray-200 text-left">
                   {{ student.name }}
                 </td>
                 <td class="px-4 py-2 border-b border-gray-200 text-left">
                   {{ student.phoneNumber }}
                 </td>
                 <td class="px-4 py-2 border-b border-gray-200 text-left">
                   {{ student.address }}
                 </td>
                 <!-- <td class="px-4 py-2 border-b border-gray-200 text-left">
                 <span class="pi pi-file-edit hover:text-red-400" @click="editBook(book.bookId)"></span>
                 </td> -->
               </tr>
             </tbody>
           </table>
           <div class="pl-[500px]">
             <pagination :total-pages="totalPages" :current-page="currentPage" @page-changed="changePageNumber"></pagination>
           </div>
         </div>
       </div>
 
 </template> 

<script>
import axiosClient from '../axiosClient';
import pagination from '../components/pagination.vue';
export default {
    components: {
        pagination
    },
    data() {
        return {
            students: [],
            keyword: '',
            currentPage: 1,
            totalPages: 0,
        }
    },
    mounted() {
        this.fetchStudents();
    },
    methods: {
        async fetchStudents() {
            try {
                const response = await axiosClient.get(`Students/GetAllStudents/${this.currentPage}`);
                this.students = response.data.pagedStudents.$values;
                this.totalPages = response.data.totalPages;
               //console.log( response.data.pagedStudents);
            } catch (error) {
                console.log(error);
            }
        },
        async searchstudentsbyname() {
            if(this.keyword === ''){
            await this.fetchStudents();
            return;
        }else{
            const response = await axiosClient.get(`Students/GetStudentByName/${this.keyword}`, {
                params: {
                pageNumber: this.currentPage
                }
            });
            this.students = response.data.pagedStudents.$values;
            this.totalPages = response.data.totalPages;
            
        }
    },
       
    changePageNumber(page) {
        this.currentPage = page;
        this.fetchData();
    }
}
  
}
</script>

<style>

</style>
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
                <input v-model="keyword" @input="searchbookbyname" type="text" class="w-full px-10 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-orange-500" placeholder="Search...">
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
                Publisher
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Authors
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Category
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Quantities
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Position
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Language
              </th>
              <th class="text-left px-4 py-2 border-b border-gray-200">
                Actions
              </th>
            </tr>
          </thead>
          <tbody v-if="data && data.length > 0">
            <tr v-for="(book, index) in data" :key="index">
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ index + 1 }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.name }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.publicatorIdNavigation.name }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.authorIdNavigation.name }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.categoryIdNavigation.name }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.quantity }}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.positionIdNavigation.compartment + ' - ' + book.positionIdNavigation.shelf + ' - ' + book.positionIdNavigation.area}}
                </td>
                <td class="px-4 py-2 border-b border-gray-200 text-left">
                  {{ book.language }}
                </td>

                <td class="px-4 py-2 border-b border-gray-200 text-left">
                <span class="pi pi-file-edit hover:text-red-400" @click="editBook(book.bookId)"></span>
                </td>
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
  props: {
     changePage: Function,
    },
  components: {
    pagination,
  },
  data() {
    return {
      data: [],
      keyword: '',
      currentPage: 1,
      totalPages: 0,
    };
  },
  methods: {
    async fetchData() {
      try {
        const response = await axiosClient.get(`Books/GetAll/${this.currentPage}`);
        if (response.status === 200) {
          this.data = response.data.pagedBooks.$values; 
          this.totalPages = response.data.totalPages; 
          console.log("data", this.data)
        }
      } catch (error) {
        console.error('There was an error fetching the data:', error);
      }
    },
    editBook(bookId){
      this.$router.push({ name: 'EditBook', params: { id: bookId } });
    },
    addbook(){
      this.$router.push({ name: 'addbook' });
    },
    async searchbookbyname(){
      if(this.keyword === ''){
       await this.fetchData();
        return;
      }else{
      const response = await axiosClient.get(`Books/GetByName/${this.keyword}`, {
        params: {
          pageNumber: this.currentPage
        }
      });
     this.data = response.data.pagedBooks.$values;
     this.totalPages = response.data.totalPages;
      }
    },
    changePageNumber(page) {
      this.currentPage = page;
      this.fetchData();
    },
  },
  async mounted() {
   await this.fetchData();
  },
};
</script>
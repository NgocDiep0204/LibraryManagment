<template>
    <div class="mx-auto bg-gray-200 mt-2  shadow-lg h-auto">
        <div class=" flex justify-between">
            <div class=" ml-1 flex justify-between w-full">
                <div class="page-title">
                    <h4 class="ml-2 mb-1 text-[15px] md:text-[20px] font-bold">Edit book</h4>
                    <h6 class="ml-2 p-0 text-xs md:text-[13px]">Manage your products</h6>
                </div>
            </div>
        </div>
        <div class="bg-gray-50 rounded-lg max-w-xl mx-auto mt-7 items-center p-[30px]" v-if="book"> 
        
                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Name:</span> 
                    <input type="text" v-model="book.name" class="border border-gray-300 rounded-md p-1 ml-9" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                    <span class="ml-5">Publisher:</span> 
                    <select class="border border-gray-300 rounded-md p-[8px] px-10  ml-8 " @change="onPublisherSelected($event.target.value)" v-if="publishers && publishers.length > 0">
                      <option class="pl-0" :value="null" disabled selected hidden>{{ book.publicatorIdNavigation.name }}</option>
                      <option  v-for="publisher in publishers" :key="publisher.publicatorId" :value="publisher.publicatorId">{{publisher.name}}</option>
                    </select>
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                    <span class="ml-5">Author:</span> 
                    <select class="border border-gray-300 rounded-md p-[8px] px-10  ml-8 "  @change="onAuthorSelected($event.target.value)" v-if="authors && authors.length > 0">     
                      <option class="pl-0" :value="null" disabled selected hidden>{{ book.authorIdNavigation.name }}</option>
                      <option  v-for="author in authors" :key="author.authorId" :value="author.authorId">{{author.name}}</option>
                    </select>
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                    <span class="ml-5">Category:</span> 
                    <select class="border border-gray-300 rounded-md p-[8px] px-10  ml-8 "  @change="onCategorySelected($event.target.value)" v-if="categories && categories.length > 0">     
                      <option class="pl-0" :value="null" disabled selected hidden>{{ book.categoryIdNavigation.name }}</option>
                      <option  v-for="category in categories" :key="category.categoryId" :value="category.categoryId">{{category.name}}</option>
                    </select>
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Quantities:</span> 
                    <input type="text" v-model="book.quantity" class="border border-gray-300 rounded-md p-1  ml-4" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                    <span class="ml-5">Postion:</span> 
                    <select  @change="onPositionSelected($event.target.value)" class="border border-gray-300 rounded-md p-[8px] px-10 ml-8" v-if="positions && positions.length > 0">  
                      <option class="pl-0" :value="null" disabled selected hidden>{{ book.positionIdNavigation.compartment + "-"+ book.positionIdNavigation.shelf + "-" + book.positionIdNavigation.area  }}</option>
                      <option  v-for="positions in positions" :key="positions.postionId" :value="positions.Id">{{positions.compartment + "-" + positions.shelf + "-" + positions.area}}</option>
                      <option>hjk</option>
                    </select>
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Language:</span> 
                    <input type="text" v-model="book.language" class="border border-gray-300 rounded-md p-1 ml-4" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Image:</span> 
                    <input type="text" v-model="book.image" class="border border-gray-300 rounded-md p-1 ml-11" />
                </div>       
            <div>
                <button @click="saveBook"  class="border-solid rounded-lg bg-gray-500 p-1 justify-center items-center m-4">
                    Save
                </button>
            </div>
    </div>
        
   
    </div>
    

       
    
</template>


<script>
import axiosClient from '../../axiosClient';
import { mapState, mapActions } from 'vuex';
export default {
  data() {
    return {
      book: null,
    };
  },
  computed: {
    ...mapState([ 'authors', 'categories', 'positions', 'publishers']),
  },
  async mounted() {
    await this.fetchAuthor();
     await  this.fetchCategory();
     await  this.fetchPosition();
     await  this.fetchPublisher();
   
  },
  created() {
    this.fetchBook(this.$route.params.id);
  },
  methods: {
    ...mapActions([ 'fetchAuthor', 'fetchCategory', 'fetchPosition', 'fetchPublisher']),
    
    async fetchBook(bookId) {
      try {
        const response = await axiosClient.get(`Books/GetById/${bookId}`);
        if (response.status !== 200) {
          throw new Error('Failed to fetch book data');
        }
       if (response.data && response.data.$values && response.data.$values.length > 0) {
          this.book = response.data.$values[0];
        } else {
          throw new Error('Incomplete data received');
        }
        console.log('Book:', this.book.name);

      } catch (error) {
        console.error('Error fetching book data:', error);
      }
    },
    
  async saveBook() {
      try {
        const response = await axiosClient.put(`Books/Update/${this.book.bookId}`, this.book);
        if (response.status === 200) {
          this.$router.push({ name: 'books' });
        } else {
          throw new Error('Failed to update book');
        }
      } catch (error) {
        console.error('Error updating book:', error);
        // Handle error scenario
      }
    },

      onAuthorSelected(authorId) {
            this.book.authorId = authorId;
        },

        onCategorySelected(categoryId) {
            this.book.categoryId = categoryId;
        },
        onPositionSelected(positionId) {
            this.book.positionId = positionId;
        },
        onPublisherSelected(publisherId) {
            this.book.publicatorId = publisherId;
        },
  }
};

</script>
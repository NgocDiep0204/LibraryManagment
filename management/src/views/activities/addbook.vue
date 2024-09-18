<template>
    <div class="mx-auto bg-gray-200 mt-2  shadow-lg h-auto">
        <div class=" flex justify-between">
            <div class=" ml-1 flex justify-between w-full">
                <div class="page-title">
                    <h4 class="ml-2 mb-1 text-[15px] md:text-[20px] font-bold">Add book</h4>
                    <h6 class="ml-2 p-0 text-xs md:text-[13px]">Manage your products</h6>
                </div>
            </div>
        </div>

        <div class="bg-gray-50 rounded-lg max-w-xl mx-auto mt-7 items-center p-[30px]" v-if="book">

            <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                <span class="ml-5">Id:</span> 
                <input type="text" v-model="book.bookId" class="border border-gray-300 rounded-md p-1 ml-9" />
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                <span class="ml-5">Name:</span> 
                <input type="text" v-model="book.name" class="border border-gray-300 rounded-md p-1 ml-9" />
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                <span class="ml-5">Publisher:</span> 
                <select class="border border-gray-300 rounded-md p-[8px] px-10 ml-8" @change="onPublisherSelected($event.target.value)" v-if="publishers && publishers.length > 0">
                    <option class="text-gray-300" :value="null" disabled selected hidden>Enter publisher</option>
                    <option v-for="publisher in publishers" :key="publisher.publicatorId" :value="publisher.publicatorId">{{publisher.name}}</option>
                </select>
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                <span class="ml-5">Author:</span> 
                <select class="border border-gray-300 rounded-md p-[8px] px-10 ml-8" @change="onAuthorSelected($event.target.value)" v-if="authors && authors.length > 0">
                    <option class="pl-0" :value="null" disabled selected hidden></option>
                    <option v-for="author in authors" :key="author.authorId" :value="author.authorId"  @change="onAuthorSelected(value)">{{ author.name }}</option>
                </select>
            </div>  

            <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                <span class="ml-5">Category:</span> 
                <select class="border border-gray-300 rounded-md p-[8px] px-10 ml-8" @change="onCategorySelected($event.target.value)" v-if="categories && categories.length > 0">
                    <option class="pl-0" :value="null" disabled selected hidden></option>
                    <option v-for="category in categories" :key="category.categoryId" :value="category.categoryId">{{category.name}}</option>
                </select>
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 p-[15px]">
                <span class="ml-5">Quantities:</span> 
                <input type="text" v-model="book.quantity" class="border border-gray-300 rounded-md p-1 ml-4" />
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 text-left p-[15px]">
                <span class="ml-5">Position:</span> 
                <select @change="onPositionSelected($event.target.value)" class="border border-gray-300 rounded-md p-[8px] px-10 ml-8" v-if="positions && positions.length > 0">
                    <option class="pl-0" :value="null" disabled selected hidden></option>
                    <option v-for="position in positions" :key="position.positionId" :value="position.positionId">{{position.compartment + "-" + position.shelf + "-" + position.area}}</option>
                </select>
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 p-[15px]">
                <span class="ml-5">Language:</span> 
                <input type="text" v-model="book.language" class="border border-gray-300 rounded-md p-1 ml-4" />
            </div>

            <div class="flex flex-wrap justify-center items-center gap-2 p-[15px]">
                <span class="ml-5">Image:</span> 
                <input type="text" v-model="book.image" class="border border-gray-300 rounded-md p-1 ml-11" />
            </div>       
            <div>
                <button @click="saveBook" class="border-solid rounded-lg bg-gray-500 p-1 justify-center items-center m-4">
                    Save
                </button>
            </div>
        </div>
    </div>
</template>

<script>

import { mapState, mapActions } from 'vuex';
import axiosClient from '../../axiosClient';

export default {
    data() {
        return {
            book: {
                bookId: null,
                name: '',
                publicatorId: null,
                authorId: null,
                categoryId: null,
                quantity: 0,
                positionId: null,
                image: '',
                language: '',
            },
        };
    },
    computed: {
    ...mapState([ 'authors', 'categories', 'positions', 'publishers']),
    },
    
    async mounted() {
     await this.$store.dispatch('fetchAuthor');
     await this.$store.dispatch('fetchCategory');
     await this.$store.dispatch('fetchPosition');
     await this.$store.dispatch('fetchPublisher');
    },

    methods: {
    async saveBook() {
    try {
        const response = await axiosClient.post(`Books/Create`, this.book);
        console.log('Created new book:', response.data);
        // Kiểm tra mã trạng thái của response
        if (response.status === 200) {
            console.log('Book added successfully:', this.book);
            // Chuyển hướng về danh sách sách sau khi thêm mới thành công
            this.$router.push({ name: 'books' });
        } else {
            throw new Error('Failed to add book');
        }
    } catch (error) {
        console.error('Error adding book:', error);
        // Xử lý các trường hợp lỗi, ví dụ hiển thị thông báo lỗi cho người dùng
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
       
    },
};
</script>

<template>
    <div class="mx-auto bg-gray-200 mt-2 shadow-lg h-auto">
        <div class="flex justify-between">
            <div class="ml-1 flex justify-between w-full">
                <div class="page-title">
                    <h4 class="ml-2 mb-1 text-[15px] md:text-[20px] font-bold">Add Rental</h4>
                    <h6 class="ml-2 p-0 text-xs md:text-[13px]">Manage your products</h6>
                </div>
            </div>
        </div>

        <div class="bg-gray-50 rounded-lg max-w-xl mx-auto mt-7 items-center p-[30px]">
            <div class="grid grid-cols-2 gap-4 items-center">
                <div class="flex flex-wrap justify-center items-center gap-2">
                    <input type="text" @input="getId" placeholder="Enter id" v-model="rent.rentId" class="border border-gray-300 rounded-md p-1 ml-9" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 ">
                   
                    <input type="text" placeholder="Enter phone" v-model="phone" @input="fetchStudentByPhone" class="border border-gray-300 rounded-md p-1 ml-9" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2 " v-if="student" >
                    <input type="text" v-model="student.name" class="border border-gray-300 rounded-md p-0 ml-2 text-gray-200" readonly />
                   
                </div>
             

                <div class="flex flex-wrap justify-center items-center gap-2">
                    <input type="text" v-model="rent.returnDate" class="border border-gray-300 rounded-md p-1 ml-9  text-gray-400" readonly />
                </div>
            </div>

            <div class="pt-5">
                
                <h5 class="ml-2 mb-1 text-[13px] md:text-[17px] font-bold">Add Rental Details</h5> 
                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Mã sách:</span> 
                    <input type="text" v-model="rentialdetail.bookId" class="border border-gray-300 rounded-md p-1 ml-9" />
                </div>

                <div class="flex flex-wrap justify-center items-center gap-2  p-[15px]">
                    <span class="ml-5">Số lượng:</span> 
                    <input type="text" v-model="rentialdetail.quantity" class="border border-gray-300 rounded-md p-1 ml-9" />
                </div>

                
            </div>

            <div>
                <button @click="handleClick" class="border-solid rounded-lg bg-gray-500 p-1 justify-center items-center m-4">
                    Save
                </button>

                <button @click="reset" class="border-solid rounded-lg bg-gray-500 p-1 justify-center items-center m-4">
                    Reset 
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import { generate } from '@vue/compiler-core';
import axiosClient from '../../axiosClient';
import moment from 'moment';
import { mapState, mapActions, mapGetters } from 'vuex';
export default {
    data() {
        return {
            rentsaved: false,
            isCheck: false,
            phone: '',
            student: null,
            rent: {
                rentId: null,
                studentId: null,
                userId:"",
                rentDate: this.getCurrentDate(),   
                returnDate: this.getReturnDate(),  
                status: 0,
            },
            rentialdetail: {
                rentId: '',
                bookId: '',
                quantity: 1,
            },        
        };
    },
    watch: {
    'rent.rentId': function(newRentId, oldRentId) {
        this.rentialdetail.rentId = newRentId;
        this.rent.rentId = newRentId;
        },
    },
    async mounted() {
        await this.$store.dispatch('fetchRential');
        await this.generateId();    
    },
    computed: {
        ...mapState(['rentials']),
        ...mapActions(['fetchRential']),  
    },
    methods: {
        async fetchStudentByPhone() {
            if (this.phone.trim() !== '') {              
                const response = await axiosClient.get(`Students/GetStudentByPhone/${this.phone}`);     
                if (response.data && response.data.$values && response.data.$values.length > 0) {
                    this.student = response.data.$values[0];
                    this.rent.studentId = this.student.studentId
                    console.log( this.rent.studentId )
                }
            } else {
                this.rent.studentId = '';
            }
        },
        async saveRential() {
    for (let i = 0; i < this.rentials.length; i++) {
        if (this.rentials[i].rentId === this.rent.rentId) {
            this.isCheck = true;
            return false; // Trả về false nếu rentId đã tồn tại
        }
    }
    try {
        const response = await axiosClient.post('Rents/CreateRent', this.rent);
        this.rentials.push(response.data);
        return true; // Trả về true nếu lưu thành công
    } catch (error) {
        console.error(error);
        return false; // Trả về false nếu lưu không thành công
    }
},
        async saveRentialDetail() {
            try{
            const response =  axiosClient.post(`RentDetails/CreateRentDetail`, this.rentialdetail);
            if(response.data) {
                //alert('Create new rent detail successfully');
            }
           
            }catch(response) {
                console.error('Failed to create new rent detail:', response);
            }
        },
        async handleClick() {
           
             if (!this.rent.rentId || !this.rentialdetail.bookId || !this.rentialdetail.quantity || !this.phone) {
                 alert('Please fill in all required fields');
              return;
             }
          
            if (this.isCheck === false) {
              
             
                 this.rentsaved = await this.saveRential();
             
                 if(this.rentsaved){
                    await this.saveRentialDetail();
                    alert(' saved')
                }      
            } else {
       const save =  await this.saveRentialDetail();
         if(save) {
            alert(' saved')
         }

    }
        },
        

      

        async reset() {
        this.rent = {
            rentId: null,
            studentId: null,
            userId:"",
            rentDate: this.getCurrentDate(),
            returnDate: this.getReturnDate(),
            status: 0,
        };    
        this.rentialdetail = {
            rentId: null,
            bookId: null,
            quantity: null,
        };
        this.phone = '';
        this.isCheck = false;
        await this.generateId();
        },
        getCurrentDate() {
            return moment().format('YYYY-MM-DD');
        },
        getReturnDate() {
            return moment().add(14, 'days').format('YYYY-MM-DD');
        },
        async generateId() {
            const number = this.rentials.length + 1;
            this.rent.rentId = "HD" + number ;
        },
    }
}
</script>
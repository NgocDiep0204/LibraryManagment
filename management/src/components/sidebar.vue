<template>
  <div class="h-full bg-gray-800 hidden md:block" id="side-bar" :class="dataOpenSideBar ? 'side-bar-visible' : 'side-bar-close' ">
    <div class="bg-gray-700 h-[50px]">
      <div class="text-x font-bold text-center flex items-center justify-center text-white p-2  hover:text-red-400" v-show="dataOpenSideBar">
       Management System
      </div>
    </div>
    <div class="py-3 rounded-sm cursor-pointer text-gray-300 hover:text-white ">
      <router-link :to="{ name: 'home' }" class="px-2 flex space-x-2  hover:text-red-400">
        <span class="pi pi-briefcase m-3 pl-0" v-tooltip.right="'Home'"></span>
        <span v-show="dataOpenSideBar" class="m-3 pl-2">Home</span>
      </router-link>
      <router-link :to="{ name: 'books' }" class="px-2 flex space-x-2  hover:text-red-400">
        <span class="pi pi-file m-3 pl-0" v-tooltip.right="'Books'"></span>
        <span v-show="dataOpenSideBar" class="m-3 pl-2">Books</span>
      </router-link>
      <router-link :to="{ name: 'student' }" class="px-2 flex space-x-2  hover:text-red-400">
        <span class="pi pi-user m-3 pl-0" v-tooltip.right="'Students'"></span>
        <span v-show="dataOpenSideBar" class="m-3 pl-2">Students</span>
      </router-link>
      <router-link :to="{ name: 'rentalinvoice' }" class="px-2 flex space-x-2  hover:text-red-400">
        <span class="pi pi-money-bill m-3 pl-0" v-tooltip.right="'Rental invoicea'"></span>
        <span v-show="dataOpenSideBar" class="m-3 pl-2">Rental invoice</span>
      </router-link>
      <div class="dropdown " @click="toggleMenu">
        <span class="pi pi-database m-3 pl-2" v-tooltip.right="'Report'"></span>
        <span v-show="dataOpenSideBar" class="m-3 pl-2  hover:text-red-400">Report</span>
        <div v-if="isOpen" class="dropdown-content grid grid-rows-2 pl-8 m-1">
          <a @click="navigateTo('/chart')" class=" hover:text-red-400">
            <span  class="pi pi-chart-line" v-tooltip.right="'Chart'"></span >
              <span v-show="dataOpenSideBar" class="m-3 pl-2 ">Chart</span>
          </a>
          <a @click="navigateTo('/topcustomers')" class=" hover:text-red-400"> 
            <span class="pi pi-users" v-tooltip.right="'Top customers'"></span>
            <span v-show="dataOpenSideBar"  class="m-3 pl-2">Top customers</span>
          </a>
          <a @click="navigateTo('/topbooks')" class=" hover:text-red-400">
            <span class="pi pi-book" v-tooltip.right="'Top books'"></span>
            <span v-show="dataOpenSideBar"  class="m-3 pl-2">Top books</span>
          </a>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
export default {
  props: {
    //user: Object,
    dataOpenSideBar: Boolean,
  },
  data() {
    return {
      isOpen: false,
    };
  },
  methods: {
    toggleMenu() {
      this.isOpen = !this.isOpen;
    },
    navigateTo(route) {
      this.$router.push(route);
      this.isOpen = false;
    },
  },
};
</script>

<style>
.p-tooltip-text {
  font-size: 10px !important;
}

#side-bar {
  overflow: hidden;
  transition: width 300ms;
}

.side-bar-visible {
  width: 350px !important;
}

.side-bar-close {
  width: 80px !important;
}
</style>

<template>
   <div>
        <h2></h2> <strong>Biểu đồ biểu thị số lượng hóa đơn và số sách đc mượn qua các tháng trong năm</strong>

   </div>
    <div>
     <canvas id="myChart" width="300" height="500"></canvas>
    </div>
  </template>
  
  <script>
  import Chart from 'chart.js/auto';
  import { mapState, mapActions } from 'vuex';
  export default{
    data(){
      return{
        dataArray: [],
      }
    },
    computed: {
      ...mapState(['countbookbymonth', 'countrentsbymonth']),
      ...mapActions(['fetchCountRentsByMonth', 'fetchCountBookByMonth']),
    },

    async mounted(){
        await this.$store.dispatch('fetchCountRentsByMonth');
        await this.$store.dispatch('fetchCountBookByMonth');
        //console.log(this.countrentsbymonth);
        //console.log(this.countbookbymonth);
       const ctx = document.getElementById('myChart').getContext('2d');
       const rentCounts = [];
       const bookCounts = [];
        for(let i = 0; i < 12; i++){
          rentCounts.push(this.countrentsbymonth[i].rentCount);
          bookCounts.push(this.countbookbymonth[i].bookCount);
        }

        const rentColors = 'rgba(255, 99, 132, 0.2)'; 
        const bookColors = 'rgba(54, 162, 235, 0.2)';
    
        const myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                datasets: [{
                    label: '# of Rentials',
                    data: rentCounts,
                    backgroundColor:Array(12).fill(rentColors),
                    borderWidth: 1
                },
                {
                    label: '# of Books',
                    data: bookCounts,
                    backgroundColor: Array(12).fill(bookColors),
                    borderWidth: 1
            
                }
                    
            ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,

                scales: {
                    y: {
                        beginAtZero: true,
                    }
                }
            }
        });
        myChart;
    },
  }
  </script>
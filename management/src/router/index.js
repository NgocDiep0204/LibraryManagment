import { createRouter, createWebHistory } from 'vue-router'
import books from '../views/books.vue';
import home from '../views/home.vue';
import profile from '../views/profile.vue';
import master from '../views/layouts/master.vue';
import rentalinvoice from '../views/rentalinvoice.vue';
import rentalinvoicedetail from '../views/rentalinvoicedetail.vue';
import chart from '../views/chart.vue';
import student from '../views/student.vue';
import topcustomers from '../views/topcustomers.vue';
import topbooks from '../views/topbooks.vue';
import editbook from '../views/activities/editbook.vue';
import addbook from '../views/activities/addbook.vue';
import addrentialinvoice from '../views/activities/addrentialinvoice.vue';
import login from '../views/login.vue';

const isAuthenticated = false; // Biến này sẽ được thay đổi khi người dùng đăng nhập

const routes = [
   
    {
        path: '/',
        name: 'master',
        component: master,
        children: [
            {
                path: 'login',
                name: 'login',
                component: login
            },
            {
                path: 'home',
                name: 'home',
                component: home
            },
            {
                path: 'books',
                name: 'books',
                component: books
            },
            {
                path: 'profile',
                name: 'profile',
                component: profile
            },
            {
                path: 'rentalinvoice',
                name: 'rentalinvoice',
                component: rentalinvoice
            },
            {
                path: 'rentalinvoicedetail/:id',
                name: 'rentalinvoicedetail',
                component: rentalinvoicedetail
            },
            {
                path: 'chart',
                name: 'chart',
                component: chart
            },
            {
                path: 'student',
                name: 'student',
                component: student
            },
            {
                path: 'topcustomers',
                name: 'topcustomers',
                component: topcustomers
            },
            {
                path: 'topbooks',
                name: 'topbooks',
                component: topbooks
            },
            {
                path: 'edit-book/:id',
                name: 'editbook',
                component: editbook
            },
            {
                path: 'add-book',
                name: 'addbook',
                component: addbook
            },
            {
                path: 'add-rential-invoice',
                name: 'addrentialinvoice',
                component: addrentialinvoice
            }
        ]
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('token');
    if (!token && to.path !== '/login') {
      next('/login');
    } else if (token && to.path === '/login') {
      next('/');
    } else {
      next();
    }
  });

export default router;

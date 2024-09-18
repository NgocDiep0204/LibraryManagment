import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import router from './router'
import store from './store'
import primeVue from 'primevue/config'

import 'primevue/resources/themes/tailwind-light/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'

import Avatar from 'primevue/avatar'
import InputText from 'primevue/inputtext'
import Menu from 'primevue/menu'
import Tooltip from 'primevue/tooltip'

const app = createApp(App)
app.component('InputText',InputText)
app.component('Avatar',Avatar)
app.component('Menu',Menu)

app.directive('tooltip', Tooltip)

app.use(router)
app.use(store)
app.use(primeVue)
app.mount('#app')


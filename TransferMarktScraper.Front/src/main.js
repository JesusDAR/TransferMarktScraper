import { createApp } from 'vue'
import App from './App.vue'
import { Quasar } from 'quasar'
import quasarUserOptions from './quasar-user-options'
import router from './router'
import store from './store'

const app = createApp(App)
app.use(store).use(router).use(Quasar, quasarUserOptions)
app.mount('#app')

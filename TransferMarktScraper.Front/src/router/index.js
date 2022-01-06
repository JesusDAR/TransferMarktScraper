import { createRouter, createWebHistory } from 'vue-router'
import Layout from '@/views/Layout.vue'
import Teams from '@/views/Teams.vue'
import SearchPlayer from '@/views/SearchPlayer.vue'
import Scraper from '@/views/Scraper.vue'
import Browser from '@/views/Browser.vue'
const routes = [
  {
    path: '/',
    component: Layout,
    title:  'Layout',
    children: [
      {
        path: '/teams',
        component: Teams,
        title: 'Teams'
      },
      {
        path: '/search-player',
        component: SearchPlayer,
        title: 'SearchPlayer'
      },            
      {
        path: '/scraper',
        component: Scraper,
        title: 'Scraper'
      },
      {
        path: '/browser',
        component: Browser,
        title: 'Browser'
      }  
    ]
  },
  // {
  //   path: '/scraper',
  //   component: Scraper,
  // }  
  // {
  //   path: '/',
  //   title: 'Login Page',  
  //   component: () => import ('../components/Layout.vue'),
  //   children: [
  //     { path: '/scraper', component: () => import('../components/Scraper.vue') },
  //     { path: '/browser', component: () => import('../components/Browser.vue') },
  //   ]
  // },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

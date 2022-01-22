import { createRouter, createWebHistory } from 'vue-router'
import Layout from '@/views/Layout.vue'
import Teams from '@/views/Teams.vue'
import SearchPlayer from '@/views/SearchPlayer.vue'
import Scraper from '@/views/Scraper.vue'
const routes = [
  {
    path: '/',
    component: Layout,
    title:  'Layout',
    children: [
      {
        path: 'teams',
        name: 'teams',
        component: Teams,
        title: 'Teams',
        children: [
          {
            path: 'scraper',
            component: Scraper,
            title: 'Scraper'
          },
        ]
      },
      {
        path: 'search-player',
        component: SearchPlayer,
        title: 'SearchPlayer'
      }
    ]
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

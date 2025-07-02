import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomePage.vue'
import SmartCity from '../views/smartcity/SmartCity.vue'
import SmartCityMap from '../views/smartcity/SmartCityMap.vue'
import { getAccessToken } from '@/utils/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      meta: {
        title: 'Home',
      },
      component: HomeView,
    },
    {
      path: '/about-me',
      name: 'about',

      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutMe.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/Login.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/Register.vue'),
    },
    {
      path: '/smart-city',
      name: 'smartcity',
      redirect: { name: 'map' },
      meta: {
        requiresAuth: true,
      },
      children: [
        {
          path: 'map',
          name: 'map',
          component: SmartCityMap,
        },
        {
          path: 'list',
          name: 'list',
          component: () => import('../views/smartcity/SmartCityList.vue'),
        },
        {
          path: 'chart',
          name: 'chart',
          component: () => import('../views/smartcity/SmartCityChart.vue'),
        },
      ],
      component: SmartCity,
    },
  ],
})

// Global before guard
// router.beforeEach((to, from, next) => {
//   const isLoggedIn = !!getAccessToken()
//   console.log(to.meta.requiresAuth && !isLoggedIn)

//   if (to.meta.requiresAuth && !isLoggedIn) {
//     next({ name: 'login', query: { redirect: to.fullPath } })
//   } else {
//     next()
//   }
// })

export default router

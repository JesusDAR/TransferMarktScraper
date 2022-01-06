import { createStore } from 'vuex'

export default createStore({
  state: {
    drawer: false,
    info: {}
  },
  mutations: {
    setDrawer(state, drawer){
      state.drawer = drawer
    },
  },
  actions: {
    scrape(context){
      
    }
  },
  modules: {
  }
})

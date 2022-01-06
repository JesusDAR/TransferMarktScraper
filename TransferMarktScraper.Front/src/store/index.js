import { createStore } from 'vuex'

export default createStore({
  state: {
    drawer: false
  },
  mutations: {
    setDrawer(state, drawer){
      state.drawer = drawer
    },
  },
  actions: {
  },
  modules: {
  }
})

import { createStore } from 'vuex'
import axios from 'axios'

export default createStore({
  state : {
    url : "https://localhost:44379/api",
    successCode : 0,
    errorCode : 1,

    successOutput : '',
    errorOutput : '',

    teams : [],
  },
  mutations : {
    setSuccessOutput(state, successOutput){
      state.successOutput = successOutput
    }, 
    setErrorOutput(state, errorOutput){
      state.errorOutput = errorOutput
    },    
  },
  actions : {
    async scrapeTeams(context){
      return axios.post(this.state.url + "/teams/scrape", {}).then((response) => {
        console.log(response)
        let successes = response.data.results.filter( r => r.code === this.state.successCode ).map( r => r.message ).join('\r\n')   
        console.log(successes)     
        let errors = response.data.results.filter( r => r.code === this.state.errorCode ).map( r => r.message ).join('\r\n')
        console.log(errors) 
        context.commit('setSuccessOutput', this.state.successOutput.concat(successes))
        context.commit('setErrorOutput', this.state.errorOutput.concat(errors))
      }).catch( (error) => {
        console.log(error)
      })
      // console.log(response)
    }
  },
  modules: {
  }
})

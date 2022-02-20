import { createStore } from 'vuex'
import axios from 'axios'

export default createStore({
  state : {
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
    setTeams(state, teams){
      state.teams = teams
    },         
  },
  actions : {
    async scrapeTeams(context){
      return axios.get(process.env.VUE_APP_URL + "/teams/scrape").then((response) => {
        let successes = response.data.results.filter( r => r.code === this.state.successCode ).map( r => r.message ).join('\r\n')    
        let errors = response.data.results.filter( r => r.code === this.state.errorCode ).map( r => r.message ).join('\r\n')
        context.commit('setSuccessOutput', this.state.successOutput.concat(successes))
        context.commit('setErrorOutput', this.state.errorOutput.concat(errors))
      }).catch( (error) => {
        console.log(error)
      })
    },

    async scrapePlayers(context, id){
      return axios.get(process.env.VUE_APP_URL + "/players/scrape/teams/" + id ).then((response) => {
        let successes = response.data.results.filter( r => r.code === this.state.successCode ).map( r => r.message ).join('\r\n')   
        let errors = response.data.results.filter( r => r.code === this.state.errorCode ).map( r => r.message ).join('\r\n')
        context.commit('setSuccessOutput', this.state.successOutput.concat(successes))
        context.commit('setErrorOutput', this.state.errorOutput.concat(errors))
      }).catch( (error) => {
        console.log(error)
      })
    },

    async getTeams(context){
      return axios.get(process.env.VUE_APP_URL + "/teams").then((response) => {
        console.log(response)
        let teams = response.data
        context.commit('setTeams', teams)
      }).catch( (error) => {
        console.log(error)
      })
    }    
  },
  modules: {
  }
})

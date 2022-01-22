<template>
  <Container>
    <div class="row">
      <div class="col-6 q-pa-md">
        <q-input
          v-model="$store.state.successOutput"
          label="Successes"
          filled
          readonly
          rows="30"
          type="textarea"
        />
      </div>
      <div class="col-6 q-pa-md">
        <q-input
          v-model="$store.state.errorOutput"
          label="Errors"
          filled
          readonly
          rows="30"
          type="textarea"
        />
      </div>
    </div>
    <div class="row justify-center">
      <q-btn color="primary" label="Start Scraping" @click="scrape"/>
    </div>
  </Container>
</template>

<script>
import Container from '@/components/Container.vue'
import Team from '@/views/Team.vue'

export default {
  name : 'Scraper',
  components : {
    Container
  },
  data() {
  },
  methods : {
    async scrape() {
      await this.$store.dispatch('scrapeTeams')
      await this.$store.dispatch('getTeams')

      console.log(this.$router.getRoutes())
      // let route = { name : 'nombre' , path : 'nombre', component : Team}
      // let route = { name : this.$store.state.teams[0].name , path : this.$store.state.teams[0].tfmData.name, component : Team}
      // this.$router.addRoute(route)
      // this.$router.addRoute('teams' , route)
      // console.log(this.$store.state.teams)
      // let routes = []
      this.$store.state.teams.forEach(async (team) => {
        let route = { name : team.name, path : team.tfmData.name, component : Team}
        this.$router.addRoute('teams' , route)

        await this.$store.dispatch('scrapePlayers', team.id)

      });
      console.log(this.$router.getRoutes())
      // let teamRoute = this.$router.getRoutes().find( r => r.name === 'Teams')
      // console.log(teamRoute.children)
      // teamRoute.children = teamRoute.children.concat(routes)
      // console.log(teamRoute.children)
      // this.$router.addRoute(teamRoute)
      // console.log(this.$router.getRoutes())
    }
  }
}
</script>

<style>
  .q-textarea .q-field__native {
    resize : none;
  }

</style>

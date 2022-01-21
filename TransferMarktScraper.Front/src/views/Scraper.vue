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

export default {
  name: 'Scraper',
  components: {
    Container
  },
  data() {
    return {
      text: ""
    }
  },
  methods: {
    async scrape() {
      await this.$store.dispatch('scrapeTeams')
    }
  }
}
</script>

<style>
  .q-textarea .q-field__native {
    resize: none;
  }

</style>

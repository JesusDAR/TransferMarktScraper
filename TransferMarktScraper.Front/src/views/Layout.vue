<template>
    <q-layout view="hHh LpR fFf">
        <Header></Header>
        <Footer></Footer>
        <router-view />
    </q-layout>
</template>

<script>

import Footer from '@/components/Footer.vue'
import Header from '@/components/Header.vue'

export default {
    name : 'Layout',
    components : {
        Footer,
        Header
    },
    data () {
    }
}
</script>

<style scoped>
</style>
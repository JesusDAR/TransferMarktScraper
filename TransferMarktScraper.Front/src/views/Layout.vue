<template>
    <q-layout view="hHh LpR fFf">
        <Header></Header>
        <Footer></Footer>
        <router-view />
        <!-- <Drawer></Drawer> -->

        <!-- <q-page-container>
            <q-page class="row no-wrap">
                <div class="col">
                    <div class="full-height">
                        <q-scroll-area class="col q-pr-sm full-height" visible>
                            <router-view />
                        </q-scroll-area>
                    </div>
                </div>
            </q-page>
        </q-page-container> -->
    </q-layout>
</template>

<script>

import Footer from '@/components/Footer.vue'
import Header from '@/components/Header.vue'
import Drawer from '@/components/Drawer.vue'

export default {
    name: 'Layout',
    components: {
        Footer,
        Header,
        Drawer
    },
    data () {
        return {
            // drawer: false,
            // menuItemSelected: null
        }
    }
}
</script>

<style scoped>

</style>
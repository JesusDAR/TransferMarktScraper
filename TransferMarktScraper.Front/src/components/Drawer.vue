<template>
    <!-- <q-drawer v-model="$store.state.drawer" show-if-above side="left" elevated :width="220" class="bg-grey-3"> -->
    <q-drawer v-model="getDrawer" show-if-above side="left" elevated :width="220" class="bg-grey-3">
        <q-list>
            <q-item clickable :active="false" to="/scraper" exact v-ripple>
                <q-item-section avatar>
                    <q-icon name="download" />
                </q-item-section>
                <q-item-section>
                    Scrape Teams
                </q-item-section>
            </q-item>
            <q-separator/>
            <!-- <q-item clickable :active="false" to="/browser" exact v-ripple>
                <q-item-section avatar>
                    <q-icon name="search" />
                </q-item-section>
                <q-item-section>
                    Browse Data
                </q-item-section>
            </q-item>                
            <q-separator/> -->
        </q-list>
      </q-drawer>  
</template>

<script>
export default {
    name: 'Drawer',
    methods: {
        getDrawer() {
            console.log($store.state.drawer)
            return $store.state.drawer
        }
    }
}
</script>

<style>

</style>
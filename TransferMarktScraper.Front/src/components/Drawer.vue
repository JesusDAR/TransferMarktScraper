<template>
    <q-drawer show-if-above side="left" elevated :width="220" class="bg-grey-3">
        <q-list>
            <q-item clickable :active="false" to="/teams/scraper" exact v-ripple>
                <q-item-section avatar>
                    <q-icon name="download" />
                </q-item-section>
                <q-item-section>
                    Scrape Teams
                </q-item-section>
            </q-item>
            <q-separator/>
            <!-- <q-item clickable :active="false" to="/teams/athletic-bilbao" exact v-ripple>
                <q-item-section avatar>
                    <q-icon name="download" />
                </q-item-section>
                <q-item-section>
                    Scrape Teams
                </q-item-section>
            </q-item>
            <q-separator/> -->
            <template v-for="(team, index) in $store.state.teams">
                <q-item clickable :active="false" :to=" '/teams/' + team.tfmData.name " exact v-ripple>
                    <q-item-section avatar>
                        <q-icon :name="'img:' + team.image" />
                    </q-item-section>
                    <q-item-section>
                        {{ team.name }}
                    </q-item-section>
                </q-item>
                <q-separator/>
            </template>

        </q-list>
      </q-drawer>  
</template>

<script>
export default {
    name: 'Drawer',
}
</script>

<style>

</style>
<template>
    <q-header elevated>
        
        <q-toolbar class="bg-green-9">
            <q-toolbar-title class="text-center">
                <q-icon name="sports_soccer" />
                TransferMarktScraper
            </q-toolbar-title>
        </q-toolbar>

        <q-tabs align="left" class="bg-green-9">
            <q-route-tab to="/teams" label="Teams" />
            <q-route-tab to="/search-player" label="Search Player" />
        </q-tabs>

    </q-header>    
</template>

<script>
export default {
    name: 'Header',
    data() {
        return {
        }

    }
}
</script>

<style>

</style>
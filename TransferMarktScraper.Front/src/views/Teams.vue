<template>
    <Drawer/>
    <router-view />
</template>

<script>
import Drawer from '@/components/Drawer.vue'
export default {
    name: 'Teams',
    components: { 
        Drawer 
    },
}
</script>

<style>

</style>
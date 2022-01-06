<template>
    <q-page-container>
        <q-page padding class="row no-wrap">
            <div class="col">
                <q-scroll-area class="col q-pr-sm full-height" visible>
                    <slot></slot>
                </q-scroll-area>
            </div>
        </q-page>
    </q-page-container>
</template>

<script>
export default {
    name: 'Container'
}
</script>

<style>

</style>
<template>
  <v-sheet elevation="0" color="rgba(255, 255, 255, 0.6)" tile class="pa-2">
        <v-text-field 
            v-model="searchField" 
            solo 
            :outlined="outlined" 
            dense 
            flat 
            placeholder="Search" 
            hide-details
        >
            <template slot="append">
                <v-btn icon color="blue" ripple @click="searchField=''">
                    <v-icon>{{searchField ? "mdi-close" : "mdi-magnify"}}</v-icon>
                </v-btn>
                <v-btn icon disabled>|</v-btn>
                <v-btn icon color="blue" ripple>
                    <v-icon>mdi-tune</v-icon>
                </v-btn>
            </template>
        </v-text-field>
    </v-sheet>
</template>

<script>
export default {
    data(){
        return {
            searchField: ''
        }
    },
    props: ['outlined'],
    watch: {
        searchField: function(val){
            this.$emit('search', val)
        }
    }
}
</script>

<style>

</style>
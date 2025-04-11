<template>
    <v-container fluid>
        <v-row>
            <v-col cols="12" xs="12" sm="12" md="12" class="px-4">
                <router-view></router-view>
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
    export default {
        data() {
            return {
                WorkspaceID: 0,
            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
        },
    }
</script>
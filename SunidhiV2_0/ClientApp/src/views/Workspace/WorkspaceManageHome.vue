<template>
    <v-container fluid style="padding-left:5vw; padding-right:5vw">
        <v-row>
            <v-col cols="12" xs="12" sm="12" md="12">
                <router-view></router-view>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    import { dbImworks } from '../../ImworksDb'

    export default {
        data: function () {
            return {

            };
        },
        async mounted() {
            await dbImworks.User.where("IsLoggedIn").equals(1).modify({ WorkspaceID: 0 });
            //var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();

        },
        methods: {

        }
    };
</script>
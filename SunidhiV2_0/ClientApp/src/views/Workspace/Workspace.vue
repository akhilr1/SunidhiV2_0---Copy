<template>
    <v-container fluid style="padding-left:5vw; padding-right:5vw">
        <v-row>
            <v-col cols="12" md="12" sm="12" xs="12" class="px-4 mx-3">
                <v-btn height="39" rounded link :to="{ name: 'workspace'}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyWorkspace' || $route.query.Shared == 0}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-key-outline</v-icon>
                    </v-avatar>My Workspace
                </v-btn>
                <v-btn height="39" rounded link :to="{ name: 'SharedWorkspace' }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'SharedWorkspace'  || $route.query.Shared == 1}"
                       class="mb-1 mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-key-network-outline</v-icon>
                    </v-avatar>Shared Workspace

                </v-btn>
                <v-btn height="39" rounded link :to="{ name: 'ArchivedWorkspace' }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'ArchivedWorkspace' }"
                       class="mb-1 mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-folder-remove-outline</v-icon>
                    </v-avatar>Archived Workspace
                </v-btn>
            </v-col>

            <v-col cols="12" xs="12" sm="12" md="12">
                <router-view></router-view>
            </v-col>

            <!--Data table-->
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
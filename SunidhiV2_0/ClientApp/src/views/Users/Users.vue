<template>
    <v-container fluid>
        <v-row>
            <v-col cols="12" md="12" sm="12" xs="12" class="px-4" v-show="$route.name == 'userlist'
                                                                        || $route.name == 'usersgroup'
                                                                        || $route.name == 'usersroles'">
                <v-btn id="AutoTest_U172"  height="39" rounded link :to="{ name: 'userlist' ,query: { WsID: WorkspaceID ,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'userindividual' || $route.name == 'users'}"
                       class="mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button"
                        >
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-account-outline</v-icon>
                    </v-avatar>Users
                </v-btn>

                <v-btn id="AutoTest_U173"  height="39" rounded link :to="{ name: 'usersgroup' ,query: { WsID: WorkspaceID ,Access:RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'usergroupindividual' }"
                       class="mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button"
                        >
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-account-group-outline</v-icon>
                    </v-avatar>User Groups

                </v-btn>
                <v-btn id="AutoTest_U174"  height="39" rounded link :to="{ name: 'usersroles' ,query: { WsID: WorkspaceID ,Access:RWAccess} }"
                       v-bind:class="{ 'active-primary-button': $route.name == 'userroleindividual' }"
                       class="mr-4 elevation-0 normal-outlined-button"
                       exact-active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-account-tie-outline</v-icon>
                    </v-avatar>User Roles
                </v-btn>
            </v-col>

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
          UserListViewPanel: true,
          UserGroupViewPanel: false,
          UserRolesViewPanel: false,
         WorkspaceID: 0,        
         RWAccess: 0,
          };
      },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;         

        },
  };
</script>
<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12" md="12" sm="12" xs="12" class="px-4" v-show="$route.name == 'SearchVenture'
                                                                    || $route.name == 'MyVenture'
                                                                    || $route.name == 'MyFinance'">
        <v-btn height="39" rounded link :to="{ name: 'SearchVenture' ,query: { WsID: WorkspaceID , Access: RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyprojectsDraft'}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-search-outline</v-icon>
                    </v-avatar>Search Venture
                </v-btn>
      
      <v-btn height="39" rounded link :to="{ name: 'MyVenture' ,query: { WsID: WorkspaceID , Access: RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyVenture'}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-file-document-box-plus-outline</v-icon>
                    </v-avatar>My Venture
                </v-btn>
      <v-btn height="39" rounded link :to="{ name: 'MyFinance' ,query: { WsID: WorkspaceID , Access: RWAccess}}"
                       v-bind:class="{ 'active-primary-button': $route.name == 'MyFinance'}"
                       class="mr-4 mb-1 elevation-0 normal-outlined-button"
                       active-class="active-primary-button">
                    <v-avatar size="25" class="mr-2">
                        <v-icon size="15">mdi-account-cash-outline</v-icon>
                    </v-avatar>My Finance
                </v-btn>
      </v-col>

      <v-col cols="12" xs="12" sm="12" md="12" class="px-4">
        <router-view></router-view>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
  export default{
  data(){
  return{
      WorkspaceID: 0,     
      RWAccess: 0,
  }
  },
  mounted: async function () {
      this.WorkspaceID = this.$route.query.WsID;
      this.RWAccess = this.$route.query.Access;
     

  },
  }
</script>
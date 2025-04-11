<template>
  <v-container fluid>
      <v-row>
          <!--<v-col cols="12" md="12" sm="12" xs="12" class="px-4">
              <h3 class="text-left page-head">
                  <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                      <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                  </v-avatar>
                  <router-link :to="{ name: 'SearchVenture', query: {WsID: WorkspaceID} }" class="page-head text-link" v-if="VentureType == 'Search Venture'">
                      {{VentureType}}
                  </router-link>
                  <router-link :to="{ name: 'MyVenture', query: {WsID: WorkspaceID} }" class="page-head text-link" v-if="VentureType == 'MyVenture'">
                      MyVenture
                  </router-link>
                  <v-icon size="15">mdi-chevron-right</v-icon> {{ projectInfo.ProjectName }}
              </h3>
          </v-col>-->
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
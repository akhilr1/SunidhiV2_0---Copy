<template>
  <v-container fluid class="elevation-2 mx-2 px-0 pb-0">
    <v-row class="px-3">
      <v-col>
        <h1 class="page-head-dark font-weight-medium">
          <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
            <v-icon size="15" color="primary">mdi-view-dashboard</v-icon>
          </v-avatar>
          Reports
        </h1>
      </v-col>
    </v-row>
    <v-row no-gutters>
      <v-col cols="12">
        <v-row no-gutters align="center" justify="right" class="px-2" v-if="!GuestMode">
          <v-slide-group>
            <v-slide-item v-slot="{  toggle }">
              <v-btn
                tile=""
                link=""
                :to="{
                  name: 'MapViewReport',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                v-bind:class="{
                  'active-white-btn': $route.name == 'MapViewReport',
                }"
                class="mb-1 elevation-0"
                active-class="active-white-btn"
                @click="toggle"
              >
                Map View
              </v-btn>
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
              <!--<v-btn
                tile=""
                link=""
                :to="{
                  name: 'ProjectReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                v-bind:class="{
                  'active-white-btn': $route.name == 'ProjectReports',
                }"
                class="mb-1 elevation-0"
                active-class="active-white-btn"
                @click="toggle"
              >
                Project
              </v-btn>-->
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
              <!--<v-btn
                tile=""
                link=""
                :to="{
                  name: 'InputReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                v-bind:class="{
                  'active-white-btn': $route.name == 'InputReports',
                }"
                class="mb-1 elevation-0"
                active-class="active-white-btn"
                @click="toggle"
              >
                Inputs
              </v-btn>-->
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
              <v-btn
                tile=""
                link=""
                :to="{
                  name: 'SurveryReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                v-bind:class="{
                  'active-white-btn': $route.name == 'SurveryReports',
                }"
                class="mb-1 elevation-0"
                active-class="active-white-btn"
                @click="toggle"
              >
                Survey
              </v-btn>
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
                <v-btn tile=""
                       link=""
                       :to="{
                  name: 'OutcomesReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                       v-bind:class="{
                  'active-white-btn': $route.name == 'OutcomesReports' || $route.name == 'OutcomesTable',
                }"
                       class="mb-1 elevation-0"
                       active-class="active-white-btn"
                       @click="toggle">
                    Indicators
                </v-btn>
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
                <v-btn tile=""
                       link=""
                       :to="{
                  name: 'OutputReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                       v-bind:class="{
                  'active-white-btn': $route.name == 'OutputReports' || $route.name == 'OutputTable',
                }"
                       class="mb-1 elevation-0"
                       active-class="active-white-btn"
                       @click="toggle">
                    Activities & Inputs
                </v-btn>
            </v-slide-item>
            <v-slide-item v-slot="{  toggle }">
              <!--<v-btn
                tile=""
                link=""
                :to="{
                  name: 'BeneficiariesReports',
                  query: { WsID: WorkspaceID, Access: RWAccess },
                }"
                v-bind:class="{
                  'active-white-btn': $route.name == 'BeneficiariesReports',
                }"
                class="mb-1 elevation-0"
                active-class="active-white-btn"
                @click="toggle"
              >
                Beneficiaries
              </v-btn>-->
            </v-slide-item>
          </v-slide-group>
        </v-row>
        <v-row no-gutters align="center" justify="right" class="px-2" v-if="GuestMode && $route.name == 'MapViewReport'">
          <v-slide-group>
            <v-slide-item>
              <v-btn
                :class="{'active-white-btn': $route.name == 'MapViewReport'}"
                class="elevation-0"
                active-class="active-white-btn"
                x-large
              >
                Map View
              </v-btn>
            </v-slide-item>
          </v-slide-group>
        </v-row>
      </v-col>
      <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
        <v-card class="elevation-0">
          <router-view></router-view>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
export default {
  data() {
    return {
      RWAccess: 0,
      WorkspaceID: 0,
    };
  },
  computed: {
    GuestMode() {
      return this.$store.getters["MapReport/GuestMode"];
    },
  },
  mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
  },
};
</script>
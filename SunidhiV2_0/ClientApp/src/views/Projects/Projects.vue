<template>
    <v-container fluid>
        <v-row no-gutters>
            <v-col>
                <v-card class="pa-4">
                    <v-row>
                        <v-col>
                            <h3 class="text-left page-head">
                                <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                    <v-icon size="15" color="primary">mdi-hexagon-multiple-outline</v-icon>
                                </v-avatar>
                                <router-link :to="{ name: 'DeployedInitiatives', query: {WsID: WorkspaceID,Access:RWAccess} }" class="page-head text-link">Deployed Initiative </router-link>
                                <v-icon size="15">mdi-chevron-right</v-icon>
                                {{ initiativeInfo.InitiativeName }}
                            </h3>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="12" sm="12" xs="12" class="px-4 mx-3" align="center">
                            <v-btn name="btnProjectDraft" tile="" link="" :to="{ name: 'DraftProjects', query: { WsID: WorkspaceID,Access:RWAccess,InitID:InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'DraftProjects' || $route.name == 'Projects' }"
                                   class="mb-1 elevation-0"
                                   exact-active-class="active-white-btn">
                                Draft
                            </v-btn>
                            <v-btn name="btnProjectDeployed" tile="" link="" :to="{ name: 'DeployedProjects', query: { WsID: WorkspaceID,Access:RWAccess,InitID:InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'DeployedProjects' }"
                                   class="mb-1 elevation-0"
                                   exact-active-class="active-white-btn">
                                Deployed
                            </v-btn>
                            <v-btn name="btnProjectCompleted" tile="" link="" :to="{ name: 'CompletedProjects', query: { WsID: WorkspaceID,Access:RWAccess,InitID:InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'CompletedProjects' || $route.name == 'Projects' }"
                                   class="mb-1 elevation-0"
                                   exact-active-class="active-white-btn">
                                Completed
                            </v-btn>
                            <v-btn name="btnProjectArchived" tile="" link="" :to="{ name: 'ArchivedProjects', query: { WsID: WorkspaceID,Access:RWAccess,InitID:InitiativeId} }"
                                   v-bind:class="{ 'active-white-btn': $route.name == 'ArchivedProjects' }"
                                   class="mb-1 elevation-0"
                                   exact-active-class="active-white-btn">
                                Archived
                            </v-btn>
                        </v-col>
                        <v-col cols="12" xs="12" sm="12" md="12">
                            <router-view></router-view>
                        </v-col>
                    </v-row>
                </v-card>
            </v-col>
        </v-row>
        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>
    </v-container>
</template>

<script>
    var objInitiative;
    async function importscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
        ]);
    };
    export default {
        data() {
            return {
                RWAccess: 0,
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                InitiativeId: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: "",
                    Description: "",
                    Code: "",
                    ThemeID: 0,
                    WorkspaceID: 0,
                    IsActive: false,
                    IsArchived: false,
                    IsDeleted: false,
                    CreatedBy: 0,
                },
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;   
            this.InitiativeId = this.$route.query.InitID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            await importscript();
            await this.fnGetInitiativeDetailsByID();
        },
        methods: {
            async fnGetInitiativeDetailsByID() {
                var data = new FormData();
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objInitiative.GetInitiativeDetailsByID(data);
                if (result.data) {
                    this.initiativeInfo = result.data;
                } else {
                    this.Msg = ("Failed to get details of Initiative " + this.initiativeInfo.InitiativeId);
                    this.snackbarMsg = true;
                }
            },
        }
    };
</script>
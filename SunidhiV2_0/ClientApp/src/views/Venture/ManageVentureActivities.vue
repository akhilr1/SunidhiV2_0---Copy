<template>
    <v-container fluid>
        <v-row>
            <v-col md="auto">
                <h5 class="page-head">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                    </v-avatar>
                    Activities
                </h5>
            </v-col>
            <v-spacer></v-spacer>
            <v-col md="3">
                <!--Search-->
                <v-text-field label="Search" class="mr-2 float-right" v-model="SearchActivity"
                              dense="" prepend-inner-icon="mdi-magnify"
                              outlined="" hide-details="" style="min-width:400px"></v-text-field>
            </v-col>
        </v-row>
        <!--Tree table section-->
        <v-row v-if="ActivityList.length >0">
            <v-col cols="12" class="px-0">
                <tree-table class="table tree-table"
                            :columns="ActivityListHeader"
                            :table-data="filterActivityList">
                    <!-- Header Row -->
                    <template #headerTemplate="headerProps">
                        <div v-bind="headerProps">
                            <div class="row header" style="background-color:#F5F5F5">
                                <div class="indentation" :style="{ width:65 + 'px' }">

                                </div>

                                <div class="cell" v-for="column in headerProps.columns" :key="column.id">
                                    {{ column.label }}
                                    <div style="display:inline-block; cursor:pointer">
                                        <div id="AutoTest_V9" style="height:10px" class="" @click="sortTable({key: column.id, asc: true})">
                                            <v-icon size="10">mdi-arrow-up</v-icon>
                                        </div>
                                        <div id="AutoTest_V10" class="" style="height:10px" @click="sortTable({key: column.id, asc: false})">
                                            <v-icon size="10">mdi-arrow-down</v-icon>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </template>

                    <!-- Rows with sub-activities -->
                    <template #nodeTemplate="nodeProps">
                        <div v-bind="nodeProps">
                            <div class="row">
                                <div v-if="nodeProps.depth > 0">
                                    <div class="indentation" :style="{ width:(step*nodeProps.depth) + 'px' }">

                                    </div>
                                </div>
                                <div id="AutoTest_V11" @click="nodeProps.onToggle">
                                    <v-btn id="AutoTest_V12" v-if="nodeProps.rowData.children.length>0" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen,fnGetGrandChildrenActivity(nodeProps.rowData)">
                                        <div v-if="!nodeProps.rowData.isOpen">
                                            <v-icon>mdi-chevron-right</v-icon>
                                        </div>
                                        <div v-if="nodeProps.rowData.isOpen">
                                            <v-icon>mdi-chevron-down</v-icon>
                                        </div>

                                    </v-btn>
                                </div>

                                <div v-if="nodeProps.rowData.children.length == 0">
                                    <v-btn id="AutoTest_V13" text="" @click="nodeProps.rowData.isOpen=!nodeProps.rowData.isOpen">
                                        <div v-if="!nodeProps.rowData.isOpen" hidden>
                                            <v-icon>mdi-chevron-right</v-icon>
                                        </div>
                                        <div v-if="nodeProps.rowData.isOpen" hidden>
                                            <v-icon>mdi-chevron-down</v-icon>
                                        </div>

                                    </v-btn>
                                </div>
                                <div class="cell">
                                    <router-link class="text--primary d-flex" :to="{name:'ManageVentureActivityIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: nodeProps.rowData.ActivityId }}">
                                        <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                        </v-avatar>
                                      <span> {{ nodeProps.rowData.ActivityName }}</span>
                                    </router-link>
                                </div>

                                <div class="cell">
                                    {{ formatDate(nodeProps.rowData.StartDate) }}
                                </div>

                                <div class="cell">
                                    {{ formatDate(nodeProps.rowData.EndDate) }}
                                </div>

                                <div class="cell">
                                    {{ formatDate(nodeProps.rowData.UpdatedDate) }}
                                </div>

                                <div class="cell">
                                    {{ nodeProps.rowData.WeightagePercentage }}
                                </div>

                                <div class="cell">
                                    <v-progress-circular :rotate="360"
                                                         :size="40"
                                                         :width="5"
                                                         :value="nodeProps.rowData.progress"
                                                         color="secondary">
                                        {{ nodeProps.rowData.progress }}
                                    </v-progress-circular>
                                    {{ nodeProps.rowData.progress }}%
                                </div>

                                <!-- Users-->
                                <div class="cell">
                                    <v-avatar v-for="usr in nodeProps.rowData.ActivityUsers.slice(0, 1)" :key="usr.ID" :color="`${getcolor(usr.Name)}`" size="30" class="team-avatar">
                                        <v-img v-bind:src="userimage + usr.ProfileImage" alt="User DP" height="30" width="30" class="ma-4" v-if="usr.ProfileImage.length >0"></v-img>
                                        <span class="white--text" v-else>{{usr.Name == null ? "" : usr.Name.substring(0, 1).toUpperCase()}}</span>
                                    </v-avatar>
                                </div>
                            </div>
                        </div>
                    </template>

                    <!-- Rows with no sub-activities -->
                    <template #leafTemplate="leafProps">
                        <div v-bind="leafProps">
                            <div class="row">
                                <!--<div class="cell" style="width:50px">
                                <v-checkbox v-model="leafProps.rowData.checkbox" @change="fnAddRemove(leafProps.rowData)" color="#52B962"></v-checkbox>
                            </div>-->
                                <div v-if="leafProps.depth > 0">
                                    <div class="indentation" :style="{ width:(step*leafProps.depth)  + 'px' }">

                                    </div>
                                </div>
                                <div class="cell">
                                    <router-link class="text--primary d-flex" :to="{name:'ManageVentureActivityIndividual',query: { WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId, ActID: leafProps.rowData.ActivityId }}">
                                        <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                            <v-icon size="15" color="primary">mdi-chart-areaspline</v-icon>
                                        </v-avatar>
                                       <span>{{ leafProps.rowData.ActivityName }}</span>
                                     </router-link>
                                </div>

                                <div class="cell">
                                    {{ formatDate(leafProps.rowData.StartDate) }}
                                </div>

                                <div class="cell">
                                    {{ formatDate(leafProps.rowData.EndDate) }}
                                </div>

                                <div class="cell">
                                    {{ formatDate(leafProps.rowData.UpdatedDate) }}
                                </div>

                                <div class="cell">
                                    {{ leafProps.rowData.WeightagePercentage }}
                                </div>

                                <div class="cell">
                                    <v-progress-circular :rotate="360"
                                                         :size="40"
                                                         :width="5"
                                                         :value="leafProps.rowData.progress"
                                                         color="secondary">
                                        {{ leafProps.rowData.progress }}
                                    </v-progress-circular>
                                    {{ leafProps.rowData.progress }}%
                                </div>

                                <!-- Users-->
                                <div class="cell">
                                    <v-avatar v-for="usr in leafProps.rowData.ActivityUsers" :key="usr.ID" color="#0CBA2F" size="25" class="mr-1">
                                        <span>{{usr.Name.charAt(0)}}</span>
                                    </v-avatar>
                                </div>
                            </div>
                        </div>
                    </template>
                </tree-table>
            </v-col>
        </v-row>
        <!--if no data-->
        <v-row v-show="ActivityList.length==0" >
            <!--If there is no Activities-->
            <v-col class="text-center">
                <p class="mt-2 pt-2">
                    <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="100" width="100" />
                </p>

                <p class="heading-3-dark font-weight-medium pt-0">No activities available</p>
            </v-col>
            <!--______________________________________-->
        </v-row>
        <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </v-container>
</template>
<script>
    import TreeTable from 'vue-tree-table-component'
    import SuccessButton from '@/components/success-button.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import objUtils from '../../utils.js'

    var objManageProjectActivity;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };

    export default {
        name: 'DefaultTableTree',
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                //General parameters
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                },
                projectInfo: {
                    ProjectId: 0,
                },
                //Listing parameters
                ActivityList: [],
                filterActivityList: [],
                selectedActivityList: [],
                SearchActivity :"",
                ActivityListHeader: [
                    { label: 'Name', id: 'ActivityName', props: 'ActivityName' },
                    { label: 'Start Date', id: 'StartDate', props: 'StartDate' },
                    { label: 'End Date', id: 'EndDate', props: 'EndDate' },
                    { label: 'Last updated', id: 'UpdatedDate', props: 'UpdatedDate' },
                    { label: 'Weightage', id: 'WeightagePercentage', props: 'WeightagePercentage' },
                    { label: 'Progress', id: 'progress', props: 'progress' },
                    { label: 'Users', id: 'ActivityUsers', props: 'ActivityUsers' }
                ],
                step:50,
                tableData: [
                    {
                        'ActivityName': 'Act1',
                        'StartDate': '01/02/2020',
                        'EndDate': '30/12/2020',
                        'UpdatedDate': '5/06/2020',
                        'WeightagePercentage': 0,
                        'progress': 0,
                        'ActivityUsers': 'Kishor',
                    },
                    
                ],
                ParentActivityOfSubAct: "",
                userimage: '',
            }
        },

        components: {
            'Success-Button': SuccessButton,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            TreeTable,
        },


        mounted: async function () {
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            await importscript();
            await this.fnGetProjectDetailsByID();
            await this.fnGetProjectActivityList();
        },

        methods: {

            /// <summary>
            ///  getcolor :- Function used to get color of users
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            /// formatDate :- Function used to format date
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDate(date) {
                return objUtils.common.formatDate(date);
            },

            /// <summary>
            /// fnGetProjectActivityList :- Function to get Project Activity List
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectActivityList() {
                var result = await objManageProjectActivity.GetProjectActivityList(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterActivityList = result.data;
                    this.ActivityList = result.data;
                    this.ParentActivityList = this.ActivityList.filter(a => a.ParentActivityId == 0);
                } else {
                    this.Msg = ("Failed to get activity list of Project " + this.projectInfo.ProjectName);
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            /// fnGetGrandChildrenActivity :- Function to get Sub Activities of an Activity
            /// </summary>
            /// <param name="actitem"></param>
            /// <returns> </returns>
            async fnGetGrandChildrenActivity(actitem) {

                if (actitem.isOpen == false) return;
                var child = actitem.children;
                for (var i = 0; i < child.length; i++) {
                    var result = await objManageProjectActivity.GetSubActivityListByID(child[i].ActivityId);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        child[i].children = result.data;
                    } else {
                        this.Msg = ("Failed to get sub-activity list of Activity " + oneact.ActivityName);
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId,InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProjectActivity.GetProjectDetailsByID(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                } else {
                    this.Msg = ("Failed to get details of Project " + this.projectInfo.ProjectId);
                    this.errorSnackbarMsg = true;
                }
            },
        },

        watch: {
                /// <summary>
                ///  SearchActivity :- Function used to Filter out the Activity List based on Search.
                /// </summary>
                /// <param name=""></param>
                /// <returns> </returns>
                SearchActivity: function () {
                    this.filterActivityList = this.ActivityList;
                    if (this.filterActivityList.length > 0 && this.SearchActivity != "")
                        this.filterActivityList = this.filterActivityList.filter(x => x.ActivityName.includes(this.SearchActivity));
            },
        }
    }
</script>

<style>
    .row {
        padding-left: 0px;
        border: 0px none !important;
        margin-right: -12px !important;
        margin-left: -12px !important;
    }
</style>

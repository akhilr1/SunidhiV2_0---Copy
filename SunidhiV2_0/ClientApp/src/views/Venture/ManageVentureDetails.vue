<template>
    <div>
        <v-col cols="12" md="12" sm="12" xs="12" class="pt-0">
            <v-row :justify="justifyCenter" >
                <v-col md="10" class="v-scrolling-div" style="height:65vh;">
                    <!-- View Project Details-->
                    <v-row v-if="editProjectEnableSave == true" :justify="justifyCenter">
                        <v-col md="7" class="pb-0">
                            <!-- Project Name -->
                            <v-row >
                                <v-col>
                                    <div class="pb-0 text-limit">
                                      <h4 class="font-weight-regular heading-3-dark">Project Name</h4>
                                      <h4 class="font-weight-regular heading-3">{{ projectInfo.ProjectName }}</h4>
                                    </div>
                                </v-col>
                                <v-col v-if="VentureType=='MyVenture'">
                                    <div class="pb-0 text-limit">
                                        <h4 class="font-weight-regular heading-3-dark">Progress</h4>
                                        <v-progress-linear dark rounded
                                                           background-color="#52B962"
                                                           color="#0A8C45"
                                                           height="20"
                                                           :value="FundUtilized">{{FundUtilized}}%</v-progress-linear>
                                        
                                    </div>
                                </v-col>
                            </v-row>
                                <!-- Project Description -->
                                <div class="pb-0">
                                    <h4 class="font-weight-regular heading-3-dark">Project Description</h4>
                                    
                                    <h4 class="font-weight-regular heading-3">
                                        {{ projectInfo.Description }}
                                    </h4>
                                </div>
                                
                                <!-- Initiative Name-->
                                <div class="pb-0 text-limit">
                                    <h4 class="font-weight-regular heading-3-dark">Initiative</h4> 
                                    <h4 class="font-weight-regular heading-3">{{ initiativeInfo.InitiativeName }}</h4>
                                </div>
                                <!-- Project Code & Budget -->
                                <v-row >
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">Code</h4>
                                            <h4 class="font-weight-regular heading-3">{{ projectInfo.Code }}</h4>
                                        </div>
                                    </v-col>
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">Budget</h4>
                                            <h4 class="font-weight-regular heading-3">{{ projectInfo.Budget }}</h4>
                                        </div>
                                    </v-col>
                                </v-row>
                                <v-row >
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">Start Date</h4> 
                                            <h4 class="font-weight-regular heading-3">{{formatDate(this.projectInfo.StartDate.substr(0, 10)) }}</h4>
                                        </div>
                                    </v-col>
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">End Date</h4> 
                                            <h4 class="font-weight-regular heading-3-dark">{{ formatDate(this.projectInfo.EndDate.substr(0, 10)) }}</h4>
                                        </div>
                                    </v-col>
                                </v-row>
                                <v-row >
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">Country</h4>
                                            <h4 class="font-weight-regular heading-3-dark">{{ GetCountryName(projectInfo.CountryID) }}</h4>
                                        </div>
                                    </v-col>
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">State</h4> 
                                            <h4 class="font-weight-regular heading-3-dark">{{ GetStateName(projectInfo.StateID) }}</h4>
                                        </div>
                                    </v-col>
                                </v-row>
                                <v-row >
                                    <v-col>
                                        <div class="pb-0 text-limit">
                                            <h4 class="font-weight-regular heading-3-dark">District</h4>  
                                            <h4 class="font-weight-regular heading-3">{{ GetDistrictName(projectInfo.DistrictID) }}</h4>
                                        </div>
                                    </v-col>
                                </v-row>
                        </v-col>
                    </v-row>
                    <!-- Tag Section modified-->
                    <v-row :justify="justifyCenter">
                        <v-col md="7" class="pb-0 pt-0">
                            <v-row class="mb-1">
                                <v-col class="text-left d-inline-flex" md="12">
                                    <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                    <div v-if="!allTags">
                                   
                                        <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                                        <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                                        <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
                                    </div>
                                </v-col>
                            </v-row>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
        </v-col>
    </div>
</template>
<script>
    import AddTagButton from '@/components/add-tag-button.vue'
    import SaveButtonSuccessSmall from '@/components/save-button-success-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import objUtils from '../../utils.js'

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    export default {
        data() {
            return {
                justifyCenter: 'center',
                //step 2 showing the project details
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                    InitiativeName: ""
                },
                editProjectEnableSave: true,
                Progress:"",
                projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    StartDate: "",
                    EndDate: "",
                    CountryID: 0,
                    StateID: 0,
                    DistrictID: 0,
                    ProjectTags: "",
                    ProjectTagsList: [],
                },

                CountryList: [],
                StatesList: [],
                DistrictsList: [],
                filterStatesList: [],
                filterDistrictsList: [],
                editProjectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    ProjectTagsList: [],
                },
                editCountry: {
                    CountryID: 0,
                    CountryName: ""
                },
                editState: {
                    StateID: 0,
                    StateName: ""
                },
                editDistrict: {
                    DistrictID: 0,
                    DistrictName: ""
                },
                //Description: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Description:"Description",
                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",

                isNewTag: false,
                allTags: false,
                newTagID: 0,
                TagIDList: "",

                TagList: [],
                initTagItems: [],
                selectedColor: false,
                VentureType: "",
                FundUtilized:0,

                // ToDate
                FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                ToDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                // Altered Tag Menu
                AllTagListSteps: 0,
                NewTagListSteps: 0,
                TagHeaderName: "Add Tag",
                NewTagMenu: false,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                TagMenu: false,
                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                color: "",

                UpdatingTagItem: "",
                SelectedTag: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                attr: "",
                selected: ""
            };
        },
        components: {
            'Add-Tag-Button': AddTagButton,
            'Save-Button-Success-Small': SaveButtonSuccessSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
        },
        computed: {

            editStartDate() {
                return this.formatDate(this.FrmDate)
            },
            editEndDate() {
                return this.formatDate(this.ToDate)
            },
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID; 
            this.VentureType = this.$route.query.VentureType;
            this.FundUtilized = this.$route.query.FundUtilized;
            await importscript();
            await this.fnGetCountryStatesDistricts();
            await this.fnGetInitiativeDetailsByID();
            await this.fnGetProjectDetailsByID();
        },

        methods: {

            /// <summary>
            /// formatDate :- Function used to formatting a date
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDate(date) {
                return objUtils.common.formatDate(date);
            },

            /// <summary>
            /// fnGetCountryStatesDistricts :- Function used to fetch Countries,States and Districts
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetCountryStatesDistricts() {
                var result = await objManageProject.GetCountryStatesDistricts();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.CountryList = result.data.countries;
                    this.StatesList = result.data.states;
                    this.DistrictsList = result.data.districts;
                    this.filterStatesList = result.data.states;
                    this.filterDistrictsList = result.data.districts;
                } else {
                    this.Msg = "Failed to get details of States and Districts";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            /// GetCountryName :- Function used to get Country Name based on CountryID
            /// </summary>
            /// <param name="countryid"></param>
            /// <returns>Country Name </returns>
            GetCountryName(countryid) {
                var temp = this.CountryList.filter(x => x.CountryID == countryid);
                if (temp.length > 0) {
                    return temp[0].CountryName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            /// GetStateName :- Function used to get State Name based on StateID
            /// </summary>
            /// <param name="stateid"></param>
            /// <returns>State Name </returns>
            GetStateName(stateid) {
                var temp = this.StatesList.filter(x => x.StateID == stateid);
                if (temp.length > 0) {
                    return temp[0].StateName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            /// GetDistrictName :- Function used to get Distric Name based on DistricID
            /// </summary>
            /// <param name="stateid"></param>
            /// <returns>Distric Name </returns>
            GetDistrictName(districtid) {
                var temp = this.DistrictsList.filter(x => x.DistrictID == districtid);
                if (temp.length > 0) {
                    return temp[0].DistrictName;
                } else {
                    return "Not Defined";
                }
            },

            /// <summary>
            /// fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId and InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProject.GetProjectDetailsByID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    if (this.projectInfo.ProjectTags != null) {
                        this.SelectedTag = JSON.parse(this.projectInfo.ProjectTags);
                    }
                } else {

                    this.Msg = "Failed to get details of Project " + this.projectInfo.ProjectId;
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            /// fnGetInitiativeDetailsByID :- Function used to get Initiative details based on ProjectId and InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetInitiativeDetailsByID() {
                var data = new FormData();
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetInitiativeDetailsByID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.initiativeInfo = result.data;
                } else {
                    this.Msg = "Failed to get details of Initiative ", this.initiativeInfo.InitiativeId;
                    this.errorSnackbarMsg = true;
                }
            },
        },
    };
</script>

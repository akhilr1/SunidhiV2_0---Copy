<template>
    <v-card flat>
        <v-card-title style="height:73px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-alert-box-outline</v-icon>
                </v-avatar>
             <!--if path is ManageProjectRiskActivityLog-->
                <router-link v-show="$route.name=='ManageProjectRiskActivityLog'" :to="{ name: 'ManageProjectRisks', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Risks</router-link>
              <!--_________________________________-->
              <!--if path is MyprojectsDraftIndividualRiskActivityLog-->
              <router-link v-show="$route.name=='MyprojectsDraftIndividualRiskActivityLog'" :to="{ name: 'MyprojectsDraftIndividualRisks', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Risks</router-link>
              <!--_________________________________-->
              <!--if path is MyprojectsDeployedIndividualRiskActivityLog-->
              <router-link v-show="$route.name=='MyprojectsDeployedIndividualRiskActivityLog'" :to="{ name: 'MyprojectsDeployedIndividualRisks', query: {WsID: WorkspaceID, Access:RWAccess,ProjID:ProjectId,InitID:InitiativeId}}" class="page-head text-link">Risks</router-link>
              <!--_________________________________-->
               > {{RiskInfo.RiskName}}
            </h5>
        </v-card-title>
        <!-- if No Data-->
        <!--<v-card-text>
        <v-row>
            <v-col class="text-center">
                <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                <h4 class="title text-center" color="#585858">No risks available</h4>
                <Success-Button class="mt-4" style="width:200px" title="Create Risks" @click.native="CreateRisksDialog=true"></Success-Button>
            </v-col>
        </v-row>
    </v-card-text>-->
        <v-card-text>
            <v-col cols="12" md="12" sm="12" xs="12">
                <v-tabs centered v-model="RiskActivityTab" class="pt-0 elevation-1" color="tab-active-line">
                    <v-tab href='#tab-1'>Details</v-tab>
                    <v-tab href='#tab-2'>Activity Log</v-tab>

                    <!--Details-->
                    <v-tab-item value="tab-1">
                        <v-row :justify="justifyCenter" class="mt-4">
                            <!--- View Risk Details  -->
                            <v-col md="4" class="v-scrolling-div" style="height:40vh" v-if="editRiskDetails == false">
                              <v-row>
                                <v-col md="12">
                                  <!--Edit button-->
                                  <Secondary-Button id="AutoTest_IP618" title="Edit Details" class="float-right" @click.native="fnEditRiskClick(),editRiskDetails = true" v-bind:disabled ="!screenAccess"></Secondary-Button>
                                </v-col>
                              </v-row>
                                <v-row :justify="justifyCenter">
                                    <v-col md="12" class="pb-0">
                                        <!-- Project Name -->
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Risk Name
                                      </h4>
                                      <h4 class="font-weight-regular heading-3">{{RiskInfo.RiskName}}</h4>
                                        <!-- Project Description -->
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Risk Description
                                       </h4>
                                      <h4 class="font-weight-regular heading-3">
                                        {{RiskInfo.Description}}
                                      </h4>
                                        <v-row no-gutters>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Start Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{formatDate(this.RiskInfo.StartDate.substr(0, 10)) }}</h4>
                                            </v-col>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                End Date
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{ formatDate(this.RiskInfo.EndDate.substr(0, 10)) }}</h4>
                                            </v-col>
                                        </v-row>
                                      <h4 class="font-weight-regular heading-3-dark mt-4">
                                        Risk Periodicity
                                      </h4>
                                      <h4 class="font-weight-regular heading-3">{{RiskInfo.RiskPeriodicityName}}</h4>

                                        <!-- Project Code & Budget -->
                                        <v-row no-gutters>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Risk Type
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{RiskInfo.RiskType}}</h4>
                                            </v-col>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Risk Rating
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{RiskInfo.RiskRating}}</h4>
                                            </v-col>
                                        </v-row>

                                        <v-row no-gutters>
                                            <v-col md="6">
                                              <h4 class="font-weight-regular heading-3-dark mt-4">
                                                Workflow
                                              </h4>
                                              <h4 class="font-weight-regular heading-3">{{GetWFName(RiskInfo.WorkflowID)}}</h4>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-col>

                            <!-- Edit Ledger Book Details-->
                            <v-col md="4" class="v-scrolling-div" style="height:40vh" v-if="editRiskDetails == true">
                                <v-text-field outlined="" dense="" label="Risk Name" v-model="EditRisk.RiskName" :rules="[rules.required]">
                                </v-text-field>
                                <v-textarea outlined="" dense="" name="" label="Risk Description" value="" v-model="EditRisk.Description" :rules="[rules.required]"></v-textarea>
                                <!-- Date Between-->
                                <v-row>
                                    <!-- From Date -->
                                    <v-col>
                                        <v-menu v-model="FrmDateMenu"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y
                                                max-width="290px"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="FrmDate"
                                                              label="Start Date"
                                                              persistent-hint outlined hide-details
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="startDate" color="primary" no-title @input="FrmDateMenu = false,IsValidDate()" format="DD-MM-YYYY" 
                                                           dense :min="ProjectStartDate" :max ="ProjectEndDate"></v-date-picker>
                                        </v-menu>
                                    </v-col>
                                    <!-- To Date -->
                                    <v-col>
                                        <v-menu ref="ToDateMenu"
                                                v-model="ToDateMenu"
                                                :close-on-content-click="false"
                                                transition="scale-transition"
                                                offset-y
                                                max-width="290px"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="ToDate"
                                                              label="End Date"
                                                              persistent-hint outlined hide-details
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="endDate" color="primary" no-title @input="ToDateMenu = false" dense
                                                           :min="startDate" :max ="ProjectEndDate"></v-date-picker>
                                        </v-menu>
                                    </v-col>
                                </v-row>

                                <v-select hide-details dense="" :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="EditRisk.RiskPeriodicity" :rules="[rules.required]">
                                </v-select>
                                <!-- Once -->
                                <div v-show="EditRisk.RiskPeriodicity === 2">
                                    <v-menu v-model="periodicityDateMenu"
                                            :close-on-content-click="false"
                                            transition="scale-transition"
                                            offset-y
                                            max-width="290px"
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field v-model="FormattedDate"
                                                          label="Choose a Date"
                                                          persistent-hint outlined
                                                          v-on="on" class="mt-4" hide-details></v-text-field>
                                        </template>
                                        <v-date-picker v-model="periodicityDate" color="primary" no-title @input="periodicityDateMenu = false" dense
                                                        :min="startDate" :max ="endDate"></v-date-picker>
                                    </v-menu>
                                </div>
                                <!-- Weekly-->
                                <div v-show="EditRisk.RiskPeriodicity === 4" class="mt-4">
                                    <v-select dense="" :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="ReportingWeek" hide-details>
                                    </v-select>
                                </div>
                                <!-- Monthly-->
                                <div v-show="EditRisk.RiskPeriodicity === 5" class="mt-4">
                                    <v-select dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="EditRisk.ReportingMonthDay" hide-details>
                                    </v-select>
                                </div>
                                <!-- Quaterly , Halyearly and Yearly-->
                                <div class="d-inline-flex mt-4" v-if="EditRisk.RiskPeriodicity == 6|| EditRisk.RiskPeriodicity == 7 ||  EditRisk.RiskPeriodicity == 8">
                                    <v-select hide-details class="mr-1 mb-4" dense="" :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="ReportingMonth">
                                    </v-select>
                                    <v-select class="mb-4" hide-details dense="" :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDateDay">
                                    </v-select>
                                </div>

                                <v-select class="mt-4" dense="" label="Risk Types" :items="SelectRiskTypes" outlined="" v-model="EditRisk.RiskType" :rules="[rules.required]">
                                </v-select>

                                <v-select dense="" :items="SelectRiskRating" label="Risk Rating" outlined="" v-model="EditRisk.RiskRating" :rules="[rules.required]">
                                </v-select>

                                <v-select dense="" label="Workflow" :items="workflows" :item-text="i => i.WfName" :item-value="i => i.WorkflowID" outlined="" v-model="editWorkFlow.WorkflowID" >
                                </v-select>

                            </v-col>
                        </v-row>
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="4">
                                <span class="small-text mr-1">
                                    <v-icon size="15">mdi-account</v-icon> Users <span class="mandatory-star" v-if="RiskInfo.UserList.length==0">*</span>
                                </span>
                                <v-tooltip left>
                                    <template v-slot:activator="{ on, attrs }">
                                        <div v-on="on">
                                            <v-avatar size="25" class="team-avatar" v-for="(item,key) in RiskInfo.UserList.slice(0,3)" v-bind:key="item.ID" :color="`${getcolor(item.Name)}`">
                                               <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                               <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                            </v-avatar>                                   
                                            <v-avatar v-if="RiskInfo.UserList.length>=4" size="25" class="team-count">
                                                +{{RiskInfo.UserList.length-3}}
                                            </v-avatar>
                                        </div>
                                    </template>
                                    <span v-if="RiskInfo.UserList.length >= 1">{{RiskInfo.UserList[0].Name}}</span>
                                    <span v-if="RiskInfo.UserList.length >= 2">, {{RiskInfo.UserList[1].Name}}</span>
                                </v-tooltip>
                                <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                    <template v-slot:activator="{ on }">
                                        <v-btn id="AutoTest_IP619" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                               @click="" v-bind:disabled="!screenAccess">
                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                        </v-btn>
                                    </template>
                                    <!--Assign Account User pop up-->
                                    <v-card>
                                        <v-card-title class="heading-3">
                                            Add People to Notify
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_IP620" text height="25" min-width="25" max-width="25" color="#707070" @click="AssignAccountUserPopUp=false">
                                                <v-icon size="20">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-divider></v-divider>
                                        <v-card-text style="height:150px" class="v-scrolling-div">
                                            <v-data-table :headers="lstuserTableheaders"
                                                          :items="lstUsers"
                                                          show-select
                                                          item-key="ID"
                                                          sort-by="Name"
                                                          v-model="RiskInfo.UserList">

                                                <template v-slot:item.Name="{item}">
                                                    <v-list class="py-0 table-v-list">
                                                        <v-list-item class="my-0 px-0">
                                                            <v-list-item-icon>
                                                                <v-avatar size="27" light :color="`${getcolor(item.Name)}`">
                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                    <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                </v-avatar>
                                                            </v-list-item-icon>
                                                            <v-list-item-content>
                                                                {{ item.Name }}
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list>
                                                </template>

                                            </v-data-table>
                                        </v-card-text>
                                        <v-divider></v-divider>
                                        <v-card-actions>
                                            <v-row no-gutters="">
                                                <v-col md="12" class="py-0" align="center">
                                                    <Success-Button id="AutoTest_IP621" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignAccountUserPopUp=false"></Success-Button>
                                                </v-col>
                                            </v-row>
                                        </v-card-actions>
                                    </v-card>
                                    <!--_______________________________________-->
                                </v-menu>
                            </v-col>
                        </v-row>
                        <v-row :justify="justifyCenter">
                            <v-col md="6" class="text-center" v-if="editRiskDetails == true">
                              <Success-Button id="AutoTest_IP622" class="btn-122x36 mr-4" title="Save" v-bind:disabled="fnCheckMandatory()" @click.native="fnUpdateRisk(),editRiskDetails=false"></Success-Button>
                              <Default-Button-Outlined id="AutoTest_IP623"  class="btn-122x36"  title="Cancel" @click.native="editRiskDetails=false"></Default-Button-Outlined>
                            </v-col>
                        </v-row>

                    </v-tab-item>
                    <!-- Activity Log-->
                    <v-tab-item value="tab-2">
                        <v-row :justify="justifyCenter" class="mt-1">
                            <v-col md="6" class="py-0">
                                <v-card flat  class="py-0">
                                    <v-card-text style="height:300px" class="v-scrolling-div">
                                        <v-list  dense class="py-0">
                                            <template v-for="(item, index) in lstActivityLog">
                                                <v-list-item class="py-0">
                                                    <v-list-item-icon>
                                                        <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                        </v-avatar>
                                                    </v-list-item-icon>
                                                    <v-list-item-content>
                                                        <v-list-item-title class="py-0 pb-0">
                                                            <v-row class="d-lg-inline-flex py-0 pb-0">
                                                                <v-col class="px-0 pl-3 py-0">
                                                                    <h4 class="font-weight-large   py-0 pb-0 px-0">{{item.Name}}</h4>
                                                                </v-col>
                                                                <v-col class="ml-3 px-0 py-0">
                                                                    <h4 class="font-weight-regular  py-0 pb-0">{{item.Message}}</h4>
                                                                </v-col>
                                                            </v-row>
                                                        </v-list-item-title>
                                                        <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                            <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                        </v-list-item-subtitle>
                                                    </v-list-item-content>
                                                </v-list-item>
                                            </template>
                                        </v-list>
                                    </v-card-text>
                                </v-card>
                            </v-col>
                        </v-row>


                    </v-tab-item>
                </v-tabs>

            </v-col>
        </v-card-text>

        <v-snackbar v-model="RiskSaveSuccessSnackbar" top="" color="primary">
            Risk details updated successfully
        </v-snackbar>

        <v-snackbar v-model="snackbarMsg" top color="primary">
            {{Msg}}
        </v-snackbar>

    </v-card>
</template>
<script>
    import AddButtonFab from '@/components/add-button-fab.vue'
    import SuccessButtonMd from '@/components/success-button-md.vue'
    import PrimaryButtonLight from '@/components/primary-button-light.vue'
    import OutlinedButtonDark from '@/components/outlined-button-dark.vue'
    import SuccessButton from '@/components/success-button.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import BulkActionButtonOutlined from '@/components/bulk-action-button-outlined.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import PreviousButtonOutlined from '@/components/previous-button-outlined.vue'
    import NextButtonPrimaryLight from '@/components/next-button-blue.vue'
    import PrimaryButtonOutlined from '@/components/primary-button-outlined.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    
    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
     var objManageProjectActivity;
    async function importobjManageProjectscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };
    export default {

        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                WorkspaceID: 0,
                ProjectId: 0,
                InitiativeId:0,
                riskId: 0,
                Type:'',
                justifyCenter: 'center',
                RiskActivityTab :"",
                editRiskDetails : false,
                SelectRiskTypes: ['Financial', 'Political', 'Personal'],
                SelectRiskRating: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                workflows: [],
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                AssignAccountUserPopUp: false,
                //From Date
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate 
                endDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,
                
                EditRisk: {
                    riskId:0,
                    RiskName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    RiskPeriodicity: "",
                    RiskPeriodicityName: "",
                    RiskType: "",
                    RiskRating: "",
                    Workflow: "",
                    UserList: [],
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                    periodicityDate: new Date().toISOString().substr(0, 10),
                },
                editWorkFlow: {
                    WorkflowID: 0,
                    WfName: ""
                },
                RiskInfo: {
                    riskId: 0,
                    RiskName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    RiskPeriodicity: "",
                    RiskPeriodicityName: "",
                    RiskType: "",
                    RiskRating: "",
                    WorkflowID:0,
                    Workflow: "",
                    UserList: [],
                    ReportingMonthDay: 0,
                    ReportingMonth: 0,
                    ReportingMonthDateDay: 0,
                    ReportingWeek: 0,
                    periodicityDate: new Date().toISOString().substr(0, 10),
                },
                selectedteamUsers: [],
                closeonClick: false,
                closeOnContentClick: false,
                lstuserTableheaders: [{ text: "Name", align: "left", value: "Name" }, { text: "User Role", value: "UserRole" },],
                lstUsers: [],
                lstActivityLog: [],
                lstPeriodicity: [],
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                //ReportingFrequency: 0,
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,
                rules: {
                    required: value => !!value || 'Required.',
                },
                periodicityDateMenu: false,
                RiskSaveSuccessSnackbar: false,
                snackbarMsg: false,
                Msg: "",
                ProjectStartDate: "",
                ProjectEndDate: "",
                periodicityDate: "",
                RiskDetailsTableItems: [],
                userimage: "",
            }
        },

        components: {
            'Add-Button-Fab': AddButtonFab,
            'Success-Button-Md': SuccessButtonMd,
            'Primary-Button-Light': PrimaryButtonLight,
            'Outlined-Button-Dark': OutlinedButtonDark,
            'Primary-Button-Outlined': PrimaryButtonOutlined,
            'Primary-Button': PrimaryButton,
            'Bulk-Action-Button-Outlined': BulkActionButtonOutlined,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Previous-Button-Outlined': PreviousButtonOutlined,
            'Next-button-primary-light': NextButtonPrimaryLight,
            'Success-Button-Md': SuccessButtonMd,
            'Success-Button': SuccessButton,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,

        },
        computed: {
            FrmDate() {
                return this.formatDate(this.startDate)
            },
            ToDate() {
                return this.formatDate(this.endDate)
            },
            FormattedDate() {
                return this.formatDate(this.periodicityDate)
            },
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.userimage = window.SERVER_URL + "/Upload";
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.ProjectId = this.$route.query.ProjID;
            this.Type = this.$route.query.Type;
            this.riskId = this.$route.query.riskId;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await importobjManageProjectscript();
            await this.ChangeTab();
            await this.fnGetGroupDetailsByprjID();
            await this.fnGetTeamUsersList();
            await this.fnGetRiskByriskId();
            await this.fnGetWorkflows();
            await this.fnGetActivityLog();
            await this.GetReportingFrequencyList();
            await this.GetProjectDate();
           
        },
        methods: {

            getcolor(name) {
                let colors = [
                    "#1abc9c",
                    "#2ecc71",
                    "#3498db",
                    "#9b59b6",
                    "#34495e",
                    "#16a085",
                    "#27ae60",
                    "#2980b9",
                    "#8e44ad",
                    "#2c3e50",
                    "#f1c40f",
                    "#e67e22",
                    "#e74c3c",
                    "#95a5a6",
                    "#f39c12",
                    "#d35400",
                    "#c0392b",
                    "#bdc3c7",
                    "#7f8c8d",
                ];

                let sum = 0;
                for (let j = 0; j < name.length; j++) {
                    sum += name.charCodeAt(j);
                }
                let ascii = sum;
                let colortest = ascii % 19; // ths is the make sure that the colors are based on sum
                return colors[colortest];
            },

            formatDateAndTime(date) {
                if (!date) return null;
                const [day, month, year] = date.substr(0, 10).split("-");
                const [hh, mm, ss] = date.substr(11, 19).split(":");
                if (month === "01") {
                    if (hh == 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JAN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JAN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "02") {
                    if (hh == 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"FEB"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"FEB"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "03") {
                    if (hh == 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "04") {
                    if (hh == 12) {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"APR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"APR"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "05") {
                    if (hh == 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"MAY"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"MAY"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "06") {
                    if (hh == 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUN"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "07") {
                    if (hh == 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"JUL"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"JUL"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "08") {
                    if (hh == 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"AUG"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"AUG"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "09") {
                    if (hh == 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"SEP"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"SEP"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "10") {
                    if (hh == 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"OCT"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"OCT"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "11") {
                    if (hh == 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"NOV"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"NOV"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
                if (month === "12") {
                    if (hh == 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} PM`;
                    } else if (hh > 12) {
                        return `${"DEC"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                    } else {
                        return `${"DEC"} ${day}, ${year} at ${hh}:${mm} AM`;
                    }
                }
            },

            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }

                if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.snackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                return true;

            },
            IsValidPeriodicityDate() {
                if (this.startDate != null) {
                    if (Date.parse(this.periodicityDate) < Date.parse(this.startDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.periodicityDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }

                if (this.endDate != null) {
                    if (Date.parse(this.periodicityDate) > Date.parse(this.endDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.periodicityDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.periodicityDate = "";
                        return;
                    }
                }
                return true;

            },
            async GetProjectDate() {

                var result = await objManageProject.GetProjectDate(this.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {

                    this.ProjectStartDate = result.PrjStartDate;
                    this.ProjectEndDate = result.PrjEndDate;
                }
            },
            fnEditRiskClick() {
                this.editRiskDetails = true;
                this.EditRisk.riskId = this.RiskInfo.riskId;
                this.EditRisk.RiskName = this.RiskInfo.RiskName;
                this.EditRisk.Description = this.RiskInfo.Description;
                this.EditRisk.UserList = this.RiskInfo.UserList;
                this.EditRisk.RiskPeriodicity = this.RiskInfo.RiskPeriodicity;
                this.EditRisk.ReportingMonthDay = this.RiskInfo.ReportingMonthDay;
                this.EditRisk.ReportingMonth = this.RiskInfo.ReportingMonth;
                this.EditRisk.ReportingMonthDateDay = this.RiskInfo.ReportingMonthDateDay;
                this.EditRisk.ReportingWeek = this.RiskInfo.ReportingWeek;
                this.periodicityDate = this.formatDate(this.RiskInfo.periodicityDate);
                this.EditRisk.RiskType = this.RiskInfo.RiskType;
                this.EditRisk.RiskRating = this.RiskInfo.RiskRating;
                this.startDate = this.RiskInfo.StartDate.substr(0, 10);
                this.endDate = this.RiskInfo.EndDate.substr(0, 10);
                this.editWorkFlow.WorkflowID = this.RiskInfo.WorkflowID;
            },
            async fnGetRiskByprjID() {
               
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetRiskByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.RiskDetailsTableItems = result.data;

                    }
            },
            async fnUpdateRisk() {
                /*Chk Duplicate Name*/
                await this.fnGetRiskByprjID();
               
                for (var i = 0; i < this.RiskDetailsTableItems.length; i++) {
                    if (this.RiskDetailsTableItems[i].riskId != this.EditRisk.riskId) {
                        if (this.RiskDetailsTableItems[i].RiskName == this.EditRisk.RiskName) {
                            this.Msg = "Duplicate Entry - Same Risk Name existis..";
                            this.snackbarMsg = true;
                            this.EditRisk.RiskName= "";
                            return;
                        }
                    }

                }
                if (!this.IsValidDate()) {
                    this.Msg = " Invalid Date ";
                    this.snackbarMsg = true;
                    return;
                }
                if (this.EditRisk.RiskPeriodicity == 2) {
                    if (!this.IsValidPeriodicityDate()) {
                        this.Msg = " Invalid PeriodicityDate ";
                        this.periodicityDate = "";
                        this.snackbarMsg = true;
                        return;
                    }
                }
                var data = new FormData();
                data.append("riskId", this.EditRisk.riskId);
                data.append("RiskName", this.EditRisk.RiskName);
                data.append("Description", this.EditRisk.Description);
                data.append("RiskPeriodicity", this.EditRisk.RiskPeriodicity);
                data.append("RiskType", this.EditRisk.RiskType);
                data.append("RiskRating", this.EditRisk.RiskRating);
                data.append("Startdate", this.startDate);
                data.append("Enddate", this.endDate);
                data.append("WorkflowID", this.editWorkFlow.WorkflowID);
                data.append("ProjectId", this.ProjectId);
                data.append("Users", JSON.stringify(this.RiskInfo.UserList));
                data.append("ReportingMonth", this.EditRisk.ReportingMonth);
                data.append("ReportingMonthDay", this.EditRisk.ReportingMonthDay);
                data.append("ReportingWeek", this.EditRisk.ReportingWeek);
                data.append("ReportingOnceDate", this.formatDate(this.periodicityDate));
                data.append("ReportingMonthDateDay", this.EditRisk.ReportingMonthDateDay);
                var result = await objManageProject.UpdateRisk(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else
                    if (result.status == "success") {
                        this.CreateRisksDialog = false;
                        this.RiskSaveSuccessSnackbar = true;
                        this.fnGetRiskByriskId();
                    }
                    else {

                        if (result.status == "rptfrequencyfailed") {
                            this.Msg= "Reporting Frequency Failed";
                        } else {
                            this.Msg = "Failed";
                        }
                        this.snackbarMsg = true;
                    }
            },
            GetWFName(workflowid) {
                var temp = this.workflows.filter(x => x.WorkflowID == workflowid);
                if (temp.length > 0) {
                    return temp[0].WfName;
                } else {
                    return "Not Defined";
                }
            },
           async ChangeTab(index) {
               var index;
               if (this.Type == 'Edit') {
                   index = 1;
               } else if (this.Type == 'Log'){
                   index = 2;
               }
                if (index == 1) {
                    this.RiskActivityTab = 'tab-1';
                }
                if (index == 2) {
                    this.RiskActivityTab = 'tab-2';
                }

            },
            async fnGetRiskByriskId() {
                var data = new FormData();
                data.append("riskId", this.riskId);
                var result = await objManageProject.GetRiskByriskId(data);
               
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        
                        this.RiskInfo = result.data;
                        this.RiskInfo.UserList = JSON.parse(this.RiskInfo.Users);
                        this.RiskInfo.RiskPeriodicity = result.data.RiskPeriodicity;
                        this.RiskInfo.RiskPeriodicityName = result.data.ReportingFrequencyName;
                        this.RiskInfo.ReportingMonth = result.data.ReportingMonth;
                        this.RiskInfo.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.RiskInfo.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.RiskInfo.ReportingWeek = result.data.ReportingWeek;

                        if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                          //  this.RiskInfo.FormattedDate = result.data.ReportingOnceDate;
                            this.RiskInfo.periodicityDate = result.data.ReportingOnceDate;
                        }
                    }
            },
            async fnGetWorkflows() {
            
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProject.GetWorkflows(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.workflows = result.data.workflows;

                }
            },
            async fnGetTeamUsersList() {
               
                var data = new FormData();
                data.append("GroupId", this.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectActivity.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.lstUsers  = result.data;
                }
            },
            async fnGetGroupDetailsByprjID() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageProject.GetGroupDetailsByprjID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.data) {
                        this.GroupId = result.data.GroupId;
                    }
            },
            async fnGetActivityLog() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                data.append("riskId", this.riskId);
                var result = await objManageProject.GetActivityLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstActivityLog = result.data;
                    }
                }
            },
            async GetReportingFrequencyList() {
                var result = await objManageProject.GetReportingFrequencyList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {

                    if (result.data.RptFrequencyList.length > 0) {
                        this.lstPeriodicity = result.data.RptFrequencyList;
                    }
                    if (result.data.WeekList.length > 0) {
                        this.lstWeek = result.data.WeekList;
                    }
                    if (result.data.MonthList.length > 0) {
                        this.lstMonth = result.data.MonthList;
                    }
                    if (result.data.MonthDate.length > 0) {
                        this.lstMonthDate = result.data.MonthDate;
                    }
                }
            },
            fnCheckMandatory() {
               
                if (this.EditRisk.RiskName == "" ||
                    this.EditRisk.Description == "" ||
                    this.EditRisk.RiskType == "" ||
                    this.EditRisk.RiskRating == "" ||                   
                    this.EditRisk.RiskPeriodicity == 0 ||
                    (this.EditRisk.RiskPeriodicity == 2  && (this.periodicityDate == "" || this.periodicityDate == null)) ||
                    (this.EditRisk.startDate == "" || this.EditRisk.startDate == null) ||
                    (this.EditRisk.endDate == "" || this.EditRisk.endDate == null )||                  
                    this.RiskInfo.UserList== 0)
                    return false;
                else return true;
            },
            
        }
    }
</script>
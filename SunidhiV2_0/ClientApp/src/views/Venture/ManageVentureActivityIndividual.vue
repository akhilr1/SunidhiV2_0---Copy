<template>
    <div>
        <v-card elevation="0">
            <v-card-title style="height:60px">
                <h5 class="page-head">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                    </v-avatar>
                    <router-link :to="{ name: 'ManageVentureActivities', query: {WsID: WorkspaceID, Access:RWAccess, InitID: initiativeInfo.InitiativeId, ProjID: projectInfo.ProjectId} }" class="page-head text-link">Activity</router-link> > {{ activityInfo.ActivityName }}
                </h5>
            </v-card-title>

            <v-card-text>
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-tabs centered v-model="ActivityTab" class="pt-0" color="tab-active-line">
                        <v-tab href='#tab-1'>Details</v-tab>
                        <v-tab id="AutoTest_V14" href='#tab-2' @click="fnGetActivityLogList()">Activity Log</v-tab>

                        <!--Details-->
                        <v-tab-item value="tab-1">
                            <v-row :justify="justifyCenter">
                                <v-col md="6" class="v-scrolling-div mt-0" style="height:55vh">
                                    <!--- View Activity Details  -->
                                    <v-row :justify="justifyCenter" v-if="editActivityDetails == false">
                                        <v-col md="7">
                                            <v-row :justify="justifyCenter">
                                                <v-col md="12" class="pb-0">
                                                    <h3 class="mb-3">
                                                        Basic Details
                                                    </h3>
                                                    <!-- Activity  Name -->
                                                    <div class="page-sub-head pb-0 text-limit">
                                                        Activity Name
                                                    </div>
                                                    <P class="heading-1">{{ activityInfo.ActivityName}}</P>

                                                    <!-- Project Description -->
                                                    <div class="page-sub-head pt-0 pb-0 text-limit">
                                                        Activity Description
                                                    </div>
                                                    <p class="text-left heading-1" style="max-height:120px;overflow-y:auto">
                                                        {{ activityInfo.Description}}
                                                    </p>

                                                    <v-row>
                                                        <v-col md="6">
                                                            <div class="page-sub-head pb-1 text-limit">
                                                                Start Date
                                                            </div>
                                                            <P class="heading-1">{{ formatDate(activityInfo.StartDate) }}</P>
                                                        </v-col>
                                                        <v-col md="6">
                                                            <div class="page-sub-head pb-1 text-limit">
                                                                End Date
                                                            </div>
                                                            <P class="heading-1">{{ formatDate(activityInfo.EndDate) }}</P>
                                                        </v-col>
                                                    </v-row>

                                                    <div class="page-sub-head pt-0 pb-0 text-limit">
                                                        Weigthage
                                                    </div>
                                                    <P class="heading-1">{{ activityInfo.WeightagePercentage }}%</P>

                                                    <!-- Activity Type-->
                                                    <div class="page-sub-head pb-0 text-limit">
                                                        Activity Type
                                                    </div>
                                                    <P class="heading-1">{{ activityInfo.ActivityType }}</P>

                                                    <h3 class="mb-3">Payment</h3>

                                                    <div class="page-sub-head pb-0 d-inline-flex">
                                                        <p class="page-sub-head pb-0 text-limit">Payment Linked</p>
                                                        <P class="heading-1"> : {{ editActivity.IsPaymentLinked}}</P>
                                                    </div>

                                                    <!-- Payment Budget -->
                                                    <div class="page-sub-head pb-0 text-limit" v-if="activityInfo.IsPaymentLinked == true">
                                                        Budget
                                                        <P class="heading-1 ">{{ activityInfo.PaymentLinkedBudget }}</P>
                                                    </div>

                                                    <!-- Expense Tag-->
                                                    <v-row v-if="activityInfo.IsPaymentLinked==true" class="mb-0">
                                                        <v-col class="text-left d-inline-flex" md="12">
                                                            <p class="page-sub-head pb-0 text-limit">
                                                                <v-icon size="20">mdi-tag-multiple</v-icon> Expense Tags
                                                            </p>
                                                            <div>
                                                                <v-chip id="AutoTest_V15" v-for="(expenseTag, key) in editActivity.ExpenseTags" :key="expenseTag.TagID" class="tags mr-2" small label close
                                                                        @click:close="$delete(activityInfo.ExpenseTags, key)">
                                                                    {{expenseTag.TagName}}
                                                                </v-chip>
                                                            </div>
                                                        </v-col>
                                                    </v-row>
                                                    <!-- expense tag Notify User-->
                                                    <v-row v-if="activityInfo.IsPaymentLinked == true" class="mt-0 mb-0">
                                                        <v-col class="text-left d-inline-flex" md="12">
                                                            <p class="page-sub-head pb-0 text-limit mr-2">
                                                                <v-icon size="20">mdi-account</v-icon> Notify Users
                                                            </p>
                                                            <v-avatar size="30" class="team-avatar" v-if="editActivity.ExpenseTagsNotifyUsers && editActivity.ExpenseTagsNotifyUsers.length == 0">
                                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />
                                                            </v-avatar>
                                                            <v-avatar size="30" class="team-avatar" color="red" v-if="editActivity.ExpenseTagsNotifyUsers && editActivity.ExpenseTagsNotifyUsers.length >= 1">
                                                                <span>{{editActivity.ExpenseTagsNotifyUsers[0].Name.charAt(0)}}</span>
                                                            </v-avatar>
                                                            <v-avatar size="30" class="team-avatar" color="blue" v-if="editActivity.ExpenseTagsNotifyUsers && editActivity.ExpenseTagsNotifyUsers.length >= 2">
                                                                <span>{{editActivity.ExpenseTagsNotifyUsers[1].Name.charAt(0)}}</span>
                                                            </v-avatar>
                                                            <v-avatar size="30" class="team-avatar" color="green" v-if="editActivity.ExpenseTagsNotifyUsers && editActivity.ExpenseTagsNotifyUsers.length >= 3">
                                                                +{{ editActivity.ExpenseTagsNotifyUsers.length-2 }}
                                                            </v-avatar>
                                                        </v-col>
                                                    </v-row>
                                                    <!-- Tags-->
                                                    <v-row class="mb-1">
                                                        <v-col class="text-left d-inline-flex" md="12">
                                                            <p class="page-sub-head pb-0 text-limit mr-2">
                                                                <v-icon size="20">mdi-tag-multiple</v-icon> Tags
                                                            </p>
                                                            <div v-if="!allTags && editActivityDetails==false">
                                                                <v-chip v-if="activityInfo.ActivityTags.length>0" :color="activityInfo.ActivityTags[0].TagColor" label small class="tags mr-1">{{activityInfo.ActivityTags[0].TagName}}</v-chip>
                                                                <v-chip v-if="activityInfo.ActivityTags.length>1" :color="activityInfo.ActivityTags[1].TagColor" label small class="tags mr-1">{{activityInfo.ActivityTags[1].TagName}}</v-chip>
                                                                <v-chip v-if="activityInfo.ActivityTags.length>2" label small class="tags tag-count mr-1">+{{activityInfo.ActivityTags.length-2}}</v-chip>
                                                            </div>
                                                            <v-btn id="AutoTest_V16" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                            </v-btn>
                                                            <v-btn id="AutoTest_V17" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                    </v-row>

                                                    <v-row no-gutter v-if="allTags" class="mt-0">
                                                        <v-col v-if="editActivityDetails==false && activityInfo.ActivityTags && activityInfo.ActivityTags.length > 0" md="12" class="chip-container mb-0 pt-0">
                                                            <v-chip-group v-for="(TagItem, key) in activityInfo.ActivityTags" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                                                <v-chip small :color="TagItem.TagColor" class="mb-0">
                                                                    {{TagItem.TagName}}
                                                                </v-chip>
                                                            </v-chip-group>
                                                        </v-col>
                                                    </v-row>
                                                    <div class="page-sub-head pb-1 text-limit">
                                                        Assignment
                                                    </div>
                                                    <P class="heading-1">These fields has been transferred to the leaf level activity Sub Activity 1.1.1.1</P>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-tab-item>

                        <!-- Activity Log-->
                        <v-tab-item value="tab-2">
                            <v-row :justify="justifyCenter" class="mt-1">
                                <v-col md="6">
                                    <v-card elevation="0">
                                        <v-card-text style="height:300px" class="v-scrolling-div">
                                            <v-list three-line dense="">
                                                <template v-for="(item, index) in ActivityLogList">
                                                    <v-list-item>
                                                        <v-list-item-icon>
                                                            <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                                                <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            <v-list-item-title class="pb-0 pt-0">
                                                                <v-col md="12" class="d-lg-inline-flex page-sub-head pb-0">
                                                                    <v-col md="6">
                                                                        {{item.Name}}
                                                                    </v-col>
                                                                    <v-col md="6" class="text-right">
                                                                        {{item.CreatedDate}}
                                                                    </v-col>
                                                                </v-col>
                                                            </v-list-item-title>
                                                            <v-list-item-subtitle class="page-sub-head ml-3">
                                                                <v-col>
                                                                    {{item.Message}}
                                                                </v-col>
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
        </v-card>
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
    </div>
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
        data() {
            return {
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
                    ProjectName: "",
                    GroupId: 0,
                },
                activityInfo: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },
                editActivity: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },

                ExpenseTagStepper: 1,
                ExpenseTagPopUp: false,
                NewExpenseTagName: "",
                SearchExpenseTag: "",
                EditExpenseTagItem: { TagID: 0, TagName: "" },
                ProjectExpenseTagList: [],
                filterProjectExpenseTagList: [],
                ProjectUserListTableHeaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                ProjectUserList: [],
                selectedNotifyUsers: [],
                ExpenseTagsNotifyUsersPopUp: false,

                //Activity Log tab parameters
                ActivityLogList: [],

                justifyCenter: 'center',
                ActivityTab: "tab-1",
                editActivityDetails: false,
                PaymentLinked: false,
                // Tags ------------
                NewTagMenu: false,
                SearchTagDetails: "",
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                NewTagName: "",
                color: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],
                ],
                allTags: false,
                EditTagName: "",

                // Expense Tag
                ExpenseTagPopUp: false,
                offset: false,
                closeonClick: false,
                closeOnContentClick: false,

                FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,

                // ToDate
                ToDate: new Date().toISOString().substr(0, 10),
                ToDateMenu: false,

                // Notify User
                NotificationSelectPopUp: false,
               
                RWAccess: 0,
                userimage: "",
            }
        },
        mounted: async function () {
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            this.projectInfo.ProjectId = this.$route.query.ProjID;
            this.activityInfo.ActivityId = this.$route.query.ActID;
            await importscript();
            await this.fnGetProjectDetailsByID();
            await this.fnGetActivityDetailsByID();
            await this.fnGetUsersListByGroupId();
            await this.fnGetExpenseTagList();
            await this.fnGetAllProjectTags();
        },
        methods: {

            /// <summary>
            /// getcolor :- Function used to get color of users
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
            /// fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId,InitiativeId
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

            /// <summary>
            /// fnGetActivityDetailsByID :- Function used to fetch Activity details based on ActivityId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetActivityDetailsByID() {
                var result = await objManageProjectActivity.GetActivityDetailsByID(this.activityInfo.ActivityId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.activityInfo = result.data;
                } else {
                    this.Msg = ("Failed to get activity details.");
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            /// fnGetUsersListByGroupId :- Function used to fetch UsersList  based on GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectActivity.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectUserList = result.data;
                    this.filterProjectUserList = result.data;
                }
            },

            /// <summary>
            /// fnGetExpenseTagList :- Function used to fetch Expense TagList based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetExpenseTagList() {
                var result = await objManageProjectActivity.GetAllExpenseTags(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg = ("Failed to get all expense tag list.");
                    this.errorSnackbarMsg = true;
                }
            },
            
            /// <summary>
            /// fnAddExpenseTagToList :- Function used to Add Expense Tags
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnAddExpenseTagToList(item) {
                if (this.editActivity.ExpenseTags.length == 0)
                    this.editActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                else {
                    var temp = this.editActivity.ExpenseTags.filter(x => x.TagID == item.TagID);
                    if (temp.length == 0) {
                        this.editActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                    }
                }
            },

            /// <summary>
            /// fnInitializeEditExpenseTagItem :- Function used to Initialize Edit Expense TagItem
            /// </summary>
            /// <param name="exptagitem"></param>
            /// <returns> </returns>
            fnInitializeEditExpenseTagItem(exptagitem = null) {
                if (exptagitem == null) {
                    this.EditExpenseTagItem.TagID = 0;
                    this.EditExpenseTagItem.TagName = "";
                } else {
                    this.EditExpenseTagItem.TagID = exptagitem.TagID;
                    this.EditExpenseTagItem.TagName = exptagitem.TagName;
                }
            },
            
           
            /// <summary>
            /// fnGetActivityLogList :- Function used to fetch Activty Log List based on ActivityId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetActivityLogList() {
                var result = await objManageProjectActivity.GetActivityLogList(this.projectInfo.ProjectId, this.activityInfo.ActivityId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ActivityLogList = result.data;
                } else {
                    this.Msg = ("Failed to get activity details.");
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            /// fnInitializeSteper :- Function used to Initialize Steper
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },


            /// <summary>
            /// fnGetAllProjectTags :- Function used to fetch all Project Tags based on InitiativeId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllProjectTags() {
                var result = await objManageProjectActivity.GetAllProjectTags(this.initiativeInfo.InitiativeId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = ("Failed to get tag list.");
                    this.errorSnackbarMsg = true;
                }
            },
            
            
            
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
        },
    }
</script>
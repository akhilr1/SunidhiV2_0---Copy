<template>
    <v-card>
        <v-container fluid class="px-0">
            <v-row class="px-4">
                <!-- Header text-->
                <v-col>
                    <h3 class="text-left page-head">
                        <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                            <v-icon size="15" color="primary">mdi-folder-remove-outline</v-icon>
                        </v-avatar>
                        Archived Initiative
                    </h3>
                </v-col>
                <v-col md="auto">
                    <v-row>
                        <v-spacer></v-spacer>

                        <!--Bulk Action -->
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4">
                                    Bulk Actions
                                    <v-icon>
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item id="AutoTest_I1" @click.stop="fnBulkClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-plus-circle-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Unarchive
                                    </v-list-item-content>
                                </v-list-item>
                                <v-list-item id="AutoTest_I2" @click.stop="fnBulkRemoveClick()">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>
                            <v-dialog v-model="ArchiveBulkDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Unarchive Initiative
                                        <v-spacer></v-spacer>
                                        <v-btn text id="AutoTest_I3" @click="ArchiveBulkDailog=false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text class="pt-0">
                                        <v-container fluid class="pt-4">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-0">
                                                        <v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <h4 class="text-center heading-3">Are you sure you want to unarchive this initiative ?</h4>
                                                            </v-col>
                                                            <v-col>
                                                                <h4 class="text-center heading-3">
                                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                    {{NotifyMessage}}
                                                                </h4>
                                                            </v-col>

                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_I16" color="secondary elevation-0" class="mr-2" @click="UnarchiveInitiative(1),ArchiveBulkDailog=false">Unarchive</v-btn>
                                                                <v-btn id="AutoTest_I4" outlined color="grey" @click="ArchiveBulkDailog=false">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                            <v-dialog v-model="RemoveBulkInitiativeDailog" width="800">
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Initiative
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I5" text @click="RemoveBulkInitiativeDailog=false">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text class="pt-0">
                                        <v-container fluid class="pt-4">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-0">
                                                        <v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <h4 class="text-center heading-3">Are you sure want to remove this initiative ?</h4>
                                                            </v-col>
                                                            <div>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <v-list class="base-border-radius warning-div pa-0">
                                                                        <v-list-item>
                                                                            <v-list-item-icon class="mt-2">
                                                                                <v-avatar size="27" color="#F2C64D" class="elevation-0">
                                                                                    <v-icon size="20" color="#fff">mdi-exclamation</v-icon>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content class="pb-0">
                                                                                <p class="pa-0">
                                                                                    <span class="name">
                                                                                        <v-avatar size="20" color="#fff" class="elevation-0">
                                                                                            <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                                        </v-avatar>

                                                                                    </span>
                                                                                    <span class="message">Selected initiatives have {{0}} projects in it and by removing thisinitiative all the active projects and tasks will become inactive.</span>
                                                                                </p>
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12">
                                                                    <!--User List with checkbox add here-->
                                                                    <v-data-table :headers="InitiativeAssignedprojectTableHeader"
                                                                                  :items="InitiativeAssignedprojectTableItems"
                                                                                  hide-default-footer
                                                                                  item-key="Name"
                                                                                  fixed-header
                                                                                  height="25vh"
                                                                                  class="elevation-0">


                                                                    </v-data-table>
                                                                </v-col>

                                                            </div>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <v-btn id="AutoTest_I6" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(1),RemoveBulkInitiativeDailog=false">Remove</v-btn>
                                                                <v-btn id="AutoTest_I7" outlined color="grey" @click="RemoveBulkInitiativeDailog=false">Cancel</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>

                        </v-menu>
                        <!--Search-->
                        <v-text-field label="Search" class="mr-4 table-search"
                                      dense prepend-inner-icon="mdi-magnify" v-model="searchInitiative"
                                      outlined hide-details></v-text-field>
                    </v-row>
                </v-col>
            </v-row>
            <!--Main Body-->

            <v-row v-if="InitiativeTableItems.length>0">
                <v-col cols="12" md="12" sm="12" xs="12">
                    <v-data-table :headers="InitiativeTableheaders"
                                  :items="InitiativeTableItems"
                                  v-model="selectedInitiativeID"
                                  show-select=""
                                  item-key="InitiativeId"
                                  :search="searchInitiative"
                                  class="elevation-0">
                        <!-- Draft Initiatives Actions-->
                        <template v-slot:item.Actions="{item}">
                            <v-menu close-on-content-click offset-y left transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">

                                    <v-list-item id="AutoTest_I8" @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),ArchiveDailog=true,InitiativeName =item.InitiativeName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Unarchive
                                        </v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_I9" @click.stop="GetInitiative(item.InitiativeId,item.InitiativeName),RemoveInitiativeDailog=true,InitiativeName =item.InitiativeName">
                                        <v-list-item-icon>
                                            <v-icon>mdi-trash-can-outline</v-icon>
                                        </v-list-item-icon>
                                        <v-list-item-content>
                                            Remove
                                        </v-list-item-content>
                                    </v-list-item>
                                </v-list>
                                <v-dialog v-model="ArchiveDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Unarchive Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I10" text="" @click="ArchiveDailog=false,InitiativeName =null">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>

                                        </v-card-title>
                                        <v-card-text class="pt-0">
                                            <v-container fluid class="pt-4">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-container fluid class="pt-0">
                                                            <v-row>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h4 class="text-center heading-3">Are you sure you want to unarchive this initiative ?</h4>
                                                                </v-col>
                                                                <v-col>
                                                                    <h4 class="text-center heading-3">
                                                                        <v-icon class="tmr-2" color="#707070" size="25">mdi-account-group</v-icon>
                                                                        {{InitiativeName}}
                                                                    </h4>
                                                                </v-col>

                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I11" color="secondary elevation-0" class="mr-2" @click="UnarchiveInitiative(2),ArchiveDailog=false">Unarchive</v-btn>
                                                                    <v-btn id="AutoTest_I12" outlined color="grey" @click="ArchiveDailog=false,InitiativeName =null">Cancel</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>
                                <v-dialog v-model="RemoveInitiativeDailog" width="800">
                                    <v-card>
                                        <v-card-title primary-title class="page-head py-4">
                                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                            </v-avatar>
                                            Remove Initiative
                                            <v-spacer></v-spacer>
                                            <v-btn id="AutoTest_I13" text="" @click="RemoveInitiativeDailog=false,InitiativeName =null">
                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                            </v-btn>
                                        </v-card-title>
                                        <v-card-text class="pt-0">
                                            <v-container fluid class="pt-4">
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-container fluid class="pt-0">
                                                            <v-row>
                                                                <v-col cols="12" md="12" class="pt-0">
                                                                    <h4 class="text-center heading-3">Are you sure want to remove this initiative ?</h4>
                                                                </v-col>
                                                                <div>
                                                                    <v-col cols="12" md="12" class="pt-0">
                                                                        <v-list class="base-border-radius warning-div pa-0">
                                                                            <v-list-item>
                                                                                <v-list-item-icon class="mt-2">
                                                                                    <v-avatar size="27" color="#F2C64D" class="elevation-0">
                                                                                        <v-icon size="20" color="#fff">mdi-exclamation</v-icon>
                                                                                    </v-avatar>
                                                                                </v-list-item-icon>
                                                                                <v-list-item-content class="pb-0">
                                                                                    <p class="pa-0">
                                                                                        <span class="name">
                                                                                            <v-avatar size="20" color="#fff" class="elevation-0">
                                                                                                <v-icon size="12" color="grey">mdi-account-tie-outline</v-icon>
                                                                                            </v-avatar>
                                                                                            {{InitiativeName}}
                                                                                        </span>
                                                                                        <span class="message"> have {{0}} projects in it and by removing thisinitiative all the active projects and tasks will become inactive.</span>
                                                                                    </p>
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                        </v-list>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12">
                                                                        <!--User List with checkbox add here-->
                                                                        <v-data-table :headers="InitiativeAssignedprojectTableHeader"
                                                                                      :items="InitiativeAssignedprojectTableItems"
                                                                                      hide-default-footer
                                                                                      item-key="Name"
                                                                                      fixed-header
                                                                                      height="25vh"
                                                                                      class="elevation-0">


                                                                        </v-data-table>
                                                                    </v-col>

                                                                </div>
                                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                    <v-btn id="AutoTest_I14" color="secondary elevation-0" class="mr-2" @click="DeleteInitiative(2),RemoveInitiativeDailog=false">Remove</v-btn>
                                                                    <v-btn id="AutoTest_I15" outlined color="grey" @click="RemoveInitiativeDailog=false,InitiativeName =null">Cancel</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                            </v-container>
                                        </v-card-text>
                                    </v-card>
                                </v-dialog>
                            </v-menu>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
            <!-- else part if No initiative available-->
            <v-row v-else>
                <v-col cols="12" sm="12" md="12">
                    <v-row :justify="justifyCenter">
                        <v-col md="6" class="text-center">
                            <v-avatar size="80" tile class="elevation-1 base-border-radius mr-2">
                                <v-img src="../../images/ic_no_workspace.svg" height="80" width="80" class="ma-4"></v-img>
                            </v-avatar>
                            <p class="heading-3 pt-3">No initiatives available</p>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>
<script type="text/javascript">

 var objInitiative;
async function importscript() {
    return Promise.all([
        import("../../BL/Initiative.js").then(mod => {
            objInitiative = new mod.Initiative();
        })
    ]);
}


    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                snackbarMsg: false,
                Msg: "",
                justifyCenter: "center",
                WorkspaceID: 0,
                InitiativeDialog :false,
                NewInitiativeSteps: 0,

                tagColor: "",
                tagName: "",
                vmTag: "",
                tagID: "",
                SortMenu: false,

                isNewTag: false,

                newTagID: 0,
                TagIDList: "",

                TagList: [],
                initTagItems: [],
                selectedColor: false,

                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                type: 'hex',
                hex: '#FF00FF',
                filterMenu: false,
                TagMenu: false,

                InitiativeTableheaders: [
                    { text: "Name", align: "left", value: "InitiativeName" },
                    { text: "Code", value: "Code" },
                    { text: "Projects", value: "0" },
                    { text: "Beneficiaries", value: "0", },
                    { text: "Actions", value: "Actions", sortable: false }

                ],
                InitiativeTableItems: [],
                selectedInitiativeID:[],
                searchInitiative: "",
                selectedInitiativeName:"",
                RemoveInitiativeDailog: false,
                RemoveBulkInitiativeDailog:false,
                ArchiveDailog: false,
                ArchiveBulkDailog: false,

                InitiativeAssignedprojectTableHeader: [
                    { text: 'ID', value: 'ID', align: ' d-none' },
                    { text: 'Projects', align: 'left', value: 'Name' },
                    { text: 'Tasks', value: 'Tasks' },

                ],

                InitiativeAssignedprojectTableItems: [],
                InitiativeID: "",
                InitiativeName: "",
                NotifyMessage: "",

                data: vm => ({
                    startDate: new Date().toLocaleDateString().substr(0, 10),
                    endDate: new Date().toLocaleDateString().substr(0, 10),
                    //startDate: new Date().toISOString().substr(0, 10),
                    dateFormatted: vm.formatDate(new Date().toLocaleDateString().substr(0, 10)),
                    menuStartDate: false,
                    menuEndDate: false,

                }),
                // initiatives add user sub steps
                InitiativeAddUserSteps: 0,
                InviteOrSelectStep: 2,
                InitiativeAddUserGroupSteps: 0,
                InitiativeProjectTemplateSteps: 0,
                Theme: [
                    { Name: "Basic Template" },
                    { Name: "Water Conservation" },
                    { Name: "Rural Department" },
                    { Name: "Health" },
                    { Name: "Electricity" },
                    { Name: "Education&Livlihood" },
                    { Name: "Agriculture&Livestock" },
                    { Name: "Nutrition" },
                    { Name: "Sport Training & Promotion" },
                    { Name: "Women and child sector" },
                ],

                async InviteOrSelectUser(type) {

                    if (type == "Invite") {
                        this.InviteOrSelectStep = 2;
                    }
                    if (type == "Select") {
                        this.InviteOrSelectStep = 4;
                    }
                    if (type == "Import") {

                        this.InviteOrSelectStep = 5;
                    }
                },
                async NextStep() {
                    this.InitiativeAddUserSteps = this.InviteOrSelectStep
                },
            };
        },
        watch: {
            startDate(val) {
                this.dateFormatted = this.formatDate(this.startDate)
            },
            endDate(val) {
                this.dateFormatted = this.formatDate(this.endDate)
            },
            selectedInitiativeID: function () {
                this.NotifyMessage = "";
                if (this.selectedInitiativeID.length > 2) {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName + "," + this.selectedInitiativeID[1].InitiativeName + " and " + (this.selectedInitiativeID.length - 2) + " more ";
                } else if (this.selectedInitiativeID.length == 2) {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName + "," + this.selectedInitiativeID[1].InitiativeName;
                } else {
                    this.NotifyMessage = this.selectedInitiativeID[0].InitiativeName;
                }
            }
        },

        computed: {
            color: {
                get() {
                    return this[this.type]
                },
                set(v) {
                    this[this.type] = v
                },
            },

        },
        mounted: async function () {
          
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await this.fnGetInitiativeList();

        },

    methods: {

        fnBulkClick() {
            if (this.selectedInitiativeID.length > 0) {
                this.ArchiveBulkDailog = true;
            } else {
                this.Msg = ("Please select the initiatives");
                this.snackbarMsg = true;
            }
        },
        fnBulkRemoveClick() {
            if (this.selectedInitiativeID.length > 0) {
                this.RemoveBulkInitiativeDailog = true;
            } else {
                this.Msg = ("Please select the initiatives");
                this.snackbarMsg = true;
            }
        },
        async fnGetInitiativeList() {
           
            var data = new FormData();
            data.append("stage", 2);
            data.append("WorkspaceId", this.WorkspaceID);
            var result = await objInitiative.GetInitiativeDetails(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
                if (result.status == "success" && result.data) {
                this.InitiativeTableItems = result.data;
            }
        },
        async DeleteInitiative(Type) {
           
            var data = new FormData();
            if (Type == 1) {
                data.append("InitiativeId", JSON.stringify(this.selectedInitiativeID));
                data.append("Type", 1);
            }
            else {
                data.append("InitiativeId", JSON.stringify(this.InitiativeID));
                data.append("Type", 2);
            }

            var result = await objInitiative.DeleteInitiative(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
            if (result.status = "success") {
                this.Msg =("Successfully deleted");
                this.snackbarMsg = true;
            } else {

                this.Msg =("Failed to delete");
                this.snackbarMsg = true;
            }
            this.fnGetInitiativeList();
            this.InitiativeID = 0;
            this.RemoveInitiativeDailog = false;

        },
         //Type 1 is bulk Unarchive and Type 2 single Unarchive
        //InitiativeStatus 0=draft,1=deploy ,2=archive
        async UnarchiveInitiative(Type) {
            
            var data = new FormData();
            if (Type == 1) {
                data.append("InitiativeId", JSON.stringify(this.selectedInitiativeID));
                data.append("Type", 1);
                data.append("InitiativeStatus", 1)
            }
            else {
                data.append("InitiativeId", JSON.stringify(this.InitiativeID));
                data.append("Type", 2);
                data.append("InitiativeStatus", 1)
            }

            var result = await objInitiative.ChangeInitiativeStatus(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
            if (result.status = "success") {
                this.Msg = ("Successfully Unarchive");
                this.snackbarMsg = true;
            } else {
                this.Msg = ("Failed to Unarchive");
                this.snackbarMsg = true;
            }
            this.fnGetInitiativeList();
            this.InitiativeID = 0;
            this.ArchiveDailog = false;
            this.ArchiveBulkDailog = false;
        },
        async GetInitiative(InitiativeID, InitiativeName) {
            this.InitiativeID = InitiativeID;
            this.InitiativeName = InitiativeName;
        },
},


};
</script>
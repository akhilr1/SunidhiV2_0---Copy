<template>
    <v-card elevation="0">
        <v-card-title style="height:60px">
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                </v-avatar>
                Outcomes
            </h5>
            <v-spacer></v-spacer>
            <v-text-field label="Search"
                          class="mr-4 table-search"
                          v-model="SearchOutcomeDetails"
                          dense
                          prepend-inner-icon="mdi-magnify"
                          outlined
                          hide-details></v-text-field>
        </v-card-title>
        <v-row v-if="FilterOutcomeDetailsTableItems.length > 0">
            <!--Outcome Table-->
            <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
                <v-data-table :headers="OutcomeDetailsTableheaders"
                              :items="FilterOutcomeDetailsTableItems"
                              :search="SearchOutcomeDetails"
                              item-key="pk_OutcomeID"
                              v-model="selectedOutcomes"
                              fixed-header
                              height="60vh"
                              class="elevation-0">
                    <!-- DateTime-->
                    <template v-slot:item.StartDate="{ item }">
                        <span>
                            {{new Date(item.StartDate).toLocaleDateString("en-GB")}}
                        </span>
                    </template>
                    <template v-slot:item.EndDate="{ item }">
                        <span>
                            {{new Date(item.EndDate).toLocaleDateString("en-GB")}}
                        </span>
                    </template>

                    <!-- Users-->
                    <template v-slot:item.lstUserList="{ item }">
                        <div v-if="item.lstUserList.length > 0">
                            <v-tooltip left>
                                <template v-slot:activator="{ on }">
                                    <div v-on="on" class="d-inline-flex">
                                        <v-avatar :color="`${getcolor(item.lstUserList[0].Name)}`"
                                                  size="25"
                                                  class="mr-1"
                                                  v-if="item.lstUserList.length >= 1 && item.lstUserList[0].Name.length > 0">
                                            <img v-bind:src="userimage + item.lstUserList[0].ProfileImage" alt="User DP" v-if="item.lstUserList[0].ProfileImage.length >0" />
                                            <span class="white--text" v-else>{{item.lstUserList[0].Name == null ? "" : item.lstUserList[0].Name.substring(0, 1).toUpperCase()}}</span>
                                            <!--<span class="text--white">
                                                {{item.lstUserList[0].Name.charAt(0)}}
                                            </span>-->
                                        </v-avatar>
                                        <v-avatar size="25"
                                                  class="mr-1"
                                                  color="grey"
                                                  v-if="item.lstUserList.length > 1">
                                            <span class="text--white">
                                                +{{ item.lstUserList.length - 1 }}
                                            </span>
                                        </v-avatar>
                                    </div>
                                </template>
                                <span v-if="item.lstUserList.length>= 1 &&item.lstUserList[0].Name.length > 0">
                                    {{ item.lstUserList[0].Name }}
                                </span>
                                <span v-if="item.lstUserList.length >= 2">,{{ item.lstUserList[1].Name }}</span>
                            </v-tooltip>
                        </div>
                    </template>

                </v-data-table>
            </v-col>
        </v-row>

        <!-- if No Data-->
        <v-row v-else>
            <v-col cols="12" sm="12" md="12">
                <v-row :justify="justifyCenter">
                    <v-col md="12" class="text-center">
                        <p class="mt-12 pt-12">
                            <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                        </p>
                        <p class="heading-3-dark font-weight-medium pt-6">No outcomes data available</p>

                    </v-col>
                </v-row>
            </v-col>
        </v-row>
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

    var objManageProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objManageProject = new mod.ManageProject();
            })
        ]);
    };
    var objInitiative;
    async function importInitscript() {
        return Promise.all([
            import("../../BL/Initiative.js").then(mod => {
                objInitiative = new mod.Initiative();
            })
        ]);
    };
    var objManageOutcomes;
    async function importOutcomesscript() {
        return Promise.all([
            import("../../BL/Outcomes.js").then((mod) => {
                objManageOutcomes = new mod.Outcomes();
            }),
        ]);
    }

    export default {

        data() {
            return {
                justifyCenter: 'center',
                WorkspaceID: 0,
                ProjectId: 0,
                InitiativeId: 0,
                OutcomeDetailsTableheaders: [
                    { text: "ID", align: " d-none", value: "pk_OutcomeID" },
                    { text: "Name", value: "OutcomeName" },
                    { text: "Start date", value: "StartDate" },
                    { text: "End Date", value: "EndDate" },
                    { text: "Last Updated Date", value: "UpdatedDate" },
                    { text: "Work Flow", value: "WorkflowName" },
                    { text: "Survey", value: "SurveyName" },
                    { text: "Users", value: "lstUserList" },
                ],
                FilterOutcomeDetailsTableItems: [],
                SearchOutcomeDetails: "",
                RWAccess: 0,
                selectedOutcomes: [],
                PreLoader: false,
                userimage: '',
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
            this.userimage = window.SERVER_URL + "/Upload";
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.ProjectId = this.$route.query.ProjID;
            this.InitiativeId = this.$route.query.InitID;
            await importscript();
            await importInitscript();
            await importOutcomesscript();
            await this.FetchAllOutcomeList();
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
                if (name != null) {
                    for (let j = 0; j < name.length; j++) {
                        sum += name.charCodeAt(j);
                    }
                }
                let ascii = sum;
                let colortest = ascii % 19; // ths is the make sure that the colors are based on sum
                return colors[colortest];
            },
            formatDate(date) {
                if (!date) return null
                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            async FetchAllOutcomeList() {
                var data = new FormData();
                data.append("ProjectId", this.ProjectId);
                var result = await objManageOutcomes.FetchAllOutcomeList(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.OutcomeDetailsTableItems = result.data;
                    this.FilterOutcomeDetailsTableItems = result.data;
                }
            },
        },


    }
</script>
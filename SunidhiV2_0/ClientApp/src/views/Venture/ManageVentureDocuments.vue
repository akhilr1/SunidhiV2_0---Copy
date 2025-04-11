<template>
    <v-card class="elevation-0">
        <v-container fluid>
            <v-row>
                <v-col cols="12" xs="12" sm="12" md="12" class="mb-0">
                    <v-row no-gutters>
                        <h3 class="text-left page-head ml-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-file-outline</v-icon>
                            </v-avatar>
                            Documents
                        </h3>
                        <v-spacer></v-spacer>
                        <!--Search-->
                        <v-select style="max-width:180px;" class="mr-4" hide-selected="" hide-details dense
                                  :items="DocFilterList"
                                  v-model="DocFilterItem"
                                  label="Document Type" @change="GetFilteredDocList()"
                                  outlined>
                        </v-select>
                        <div >
                            <v-text-field id="AutoTest_V18" label="Search" class="mr-2 table-search" v-model="searchDoc" @click=""
                                          dense prepend-inner-icon="mdi-magnify"
                                          outlined hide-details></v-text-field>
                        </div>
                    </v-row>
                </v-col>
            </v-row>
          <!-- if No Data-->
          <v-row v-if="docList.length == 0" justify="center">
              <v-col class="text-center">
                  <p class="mt-2 pt-2">
                      <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="100" width="100" />
                  </p>
                  <p class="heading-4-dark font-weight-medium pt-0">No documents available</p>
              </v-col>
          </v-row>
                        <v-row no-gutters v-else>
                            <v-col cols="12" md="12" sm="12" xs="12">
                                <v-data-table :headers="docListTableheaders"
                                              :items="docList"
                                              item-key="DocumentID"
                                              sort-by="DocumentName"
                                              v-model="vmDocList"
                                              class="elevation-0"
                                              :search="searchDoc">
                                    <template v-slot:item.DocumentName="{item}">
                                        <v-list class="py-0 table-v-list">
                                            <v-list-item class="my-0 px-0">
                                                <v-list-item-icon>
                                                    <v-avatar size="30" color="#fff" class="elevation-1">
                                                        <v-icon size="20" color="primary">mdi-book</v-icon>
                                                    </v-avatar>
                                                </v-list-item-icon>
                                                <v-list-item-content>
                                                    {{item.DocumentName}}
                                                </v-list-item-content>
                                            </v-list-item>
                                        </v-list>
                                    </template>

                                    <template v-slot:item.UploadedDate="{ item }">
                                        <span>
                                            {{formatDate(new Date(item.UploadedDate).toISOString().substr(0, 10))+' ' + item.UploadedDate.substr(11, 5) }}
                                        </span>
                                    </template>
                                    <!--user list actions-->
                                    <template v-slot:item.Actions="{item}">
                                        <v-btn id="AutoTest_V19" min-width="22" height="29" color="grey" outlined dark class="elevation-0 mr-2 px-0" @click="DownloadDocument(item.DocumentName,item.UniqueDocumentName)" v-bind:disabled="!screenAccess">
                                            <v-icon>mdi-download-outline</v-icon>
                                        </v-btn>
                                    </template>
                                </v-data-table>
                            </v-col>
                        </v-row>
        </v-container>
    </v-card>
</template>
<script type="text/javascript">

    import SaveButtonSuccessSmall from '@/components/save-button-success-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'

import { Date } from 'core-js'

    var objProject;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProject.js").then(mod => {
                objProject = new mod.ManageProject();
            })
        ]);
    };
    export default {
        data() {
            return {
                screenAccess: 0,
                justifyCenter: 'center',
                docListTableheaders: [
                    { text: "Name", align: "left", value: "DocumentName" },
                    { text: "Size", value: "DocumentSize" },
                    { text: "Uploaded date", value: "UploadedDate" },
                    { text: "Uploaded By", value: "UploadedUser" },
                    { text: "Linked To", value: "LinkedTo" },
                    { text: "Actions", value: "Actions" },
                ],
                docList: [],
                allDocList:[],
                searchDoc: "",
                vmDocList:[],

                DeleteDocumentDialog: false,
                RenameDocumentDialog: false,
                filterMenu: false,
                chkProject : false,
                chkActivities : false,
                chkKPI: false,
                chkRisks: false,
                chkPayment : false,

                fileInput: null,
                docID:0,
                docUName: "",
                docName: "",
                docUrl: "",
                ProjectID: 0,
                vmDocName :"",
                isBulk: false,
                removeList: [],
                filterChipList: [],
                ProjectName: "",
                Msg: "",
                DocFilterList :['All Document','Project','Activities','KPI', 'Risks'],
                DocFilterItem:"All Document"

            };
        },
        components: {
            'Save-Button-Success-Small': SaveButtonSuccessSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
        },
        mounted: async function () {

            this.ProjectID = this.$route.query.ProjID;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            await importscript();
            await this.GetAllDocumentList();
            await this.GetProjectName();
        },
        methods: {
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            async GetAllDocumentList() {

                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                var result = await objProject.GetAllDocumentList(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.allDocList = result.data;
                        this.docList = result.data;
                }
            },
            async GetProjectName() {

                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                var result = await objProject.GetProjectName(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.ProjectName = result.data;
                }
            },
            async DownloadDocument(docName, docUName) {

                var downloadFileName = docName;
                var fileUrl = window.SERVER_URL + "/Upload/Documents/" + docUName;

                fetch(fileUrl).then(function (t) {
                    return t.blob().then((b) => {
                        var a = document.createElement("a");
                        a.href = URL.createObjectURL(b);
                        a.setAttribute("download", downloadFileName);
                        a.click();
                    });
                });
            },  
            GetFilteredDocList() {

                var docListTemp = [];
                this.docList = [];
                this.filterChipList = [];
                this.filterMenu = false;

                if (this.DocFilterItem == 'All Document') {
                    this.filterChipList = [];
                    this.docList = this.allDocList;
                }
                if (this.DocFilterItem == 'Activities') {
                    this.filterChipList.push({ ChipName: "Activities" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Activity');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.DocFilterItem == 'Project' ) {

                    this.filterChipList.push({ ChipName: "Projects" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Project');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.DocFilterItem == 'KPI') {

                    this.filterChipList.push({ ChipName: "KPI" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'KPI');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.DocFilterItem == 'Risks') {

                    this.filterChipList.push({ ChipName: "Risks" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Risks');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }

            },
        }
    };
</script>
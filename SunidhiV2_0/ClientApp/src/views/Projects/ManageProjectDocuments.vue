<template>
    <v-card class="elevation-0">
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid class="pt-0">
            <v-row>
                <v-col cols="12" xs="12" sm="12" md="12" class="mb-0">
                    <v-row no-gutters>
                        <h3 class="text-left page-head ml-2 pt-2">
                            <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-file</v-icon>
                            </v-avatar>
                            Documents
                        </h3>
                        <v-spacer></v-spacer>
                        <!--<v-btn height="32" v-on="on" width="160" color="success-btn" dark class="elevation-0 mr-2" @click="DeleteDocumentDialog = true">Upload Document</v-btn>-->
                        <v-col class="text-right">
                            <!--<input type="file" accept="image/*" ref="capture1" class="fileInput hide" id="capture1" />
                        <v-btn height="32" width="160" color="success-btn" dark class="elevation-0 mr-2" @click="$refs.capture1.click()">Upload Document</v-btn>-->
                            <v-file-input prepend-icon="" label="Upload File" outlined dense style="display:none"
                                          id="file" v-model="fileInput" accept="*" @change="onFileUpload"></v-file-input>
                            <v-btn id="AutoTest_IP411" height="39" color="success-btn" dark class="elevation-0 mr-4" @click.native="openFileDialog" v-bind:disabled ="!screenAccess">Upload Document</v-btn>

                        </v-col>
                        <!--Bulk Action -->
                        <v-menu offset-y transition="scroll-y-transition" v-bind:disabled ="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn id="AutoTest_IP412" height="39" v-on="on" color="grey" outlined="" class="elevation-0 mr-4" @click="isBulk =true">
                                    Bulk Actions
                                    <v-icon>
                                        mdi-chevron-down
                                    </v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                <v-list-item id="AutoTest_IP413" @click.stop="DeleteDocumentDialog = true"
                                             v-bind:disabled="isBulk && vmDocList.length ==0">
                                    <v-list-item-icon>
                                        <v-icon>mdi-trash-can-outline</v-icon>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        Remove
                                    </v-list-item-content>
                                </v-list-item>
                            </v-list>

                        </v-menu>
                        <!--Search-->

                        <v-text-field id="AutoTest_IP414" hide-details label="Search" class="mr-4 table-search" v-model="searchDoc" @click="isBulk=false"
                                      dense prepend-inner-icon="mdi-magnify"
                                      outlined></v-text-field>
                        <!--Filter button-->
                        <v-menu :close-on-content-click="false" offset-y transition="scroll-y-transition" v-model="filterMenu">
                            <template v-slot:activator="{ on }">
                                <v-btn id="AutoTest_IP415" v-on="on" height="39" outlined dense class="outlined-btn-dark mr-4" @click="isBulk=false">
                                    <v-icon>mdi-filter-outline</v-icon>
                                    Filters
                                </v-btn>
                            </template>
                            <v-list color="white" light="" width="auto" max-width="250" min-width="50" class="action-button-list pa-2">
                                <p class="title">Filter by :</p>
                                <v-checkbox color="success-check-box" input-value="true" v-model="chkProject" label="Projects"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" v-model="chkActivities" label="Activities"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" v-model="chkKPI" label="KPI"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" v-model="chkRisks" label="Risks"></v-checkbox>
                                <v-checkbox color="success-check-box" input-value="true" v-model="chkPayment" label="Payment Transaction"></v-checkbox>
                                <v-btn id="AutoTest_IP416" height="32" block color="success-btn" dark class="elevation-0 mr-2" @click="GetFilteredDocList()">
                                    Done
                                </v-btn>
                            </v-list>
                        </v-menu>
                    </v-row>
                </v-col>
            </v-row>
            <!-- Filter chips-->
            <v-row no-gutters>
                <v-col cols="12" xs="12" sm="12" md="12">
                    <div class="chip-container mb-0" v-if="filterChipList.length >0">
                        <v-chip-group multiple column active-class="primary--text">
                            <v-chip id="AutoTest_IP417" v-for="(item,idx) in filterChipList" :key="idx" close class="ma-2" @click:close="removeFilterChip(item)" small>
                                <v-icon left color="#707070" size="17">mdi-file</v-icon>
                                {{ item.ChipName }}
                            </v-chip>
                        </v-chip-group>
                    </div>
                </v-col>
            </v-row>
            <v-row no-gutters>
                <v-col cols="12" xs="12" sm="12" md="12">
                    <v-chip label outlined dark color="red"
                            v-if="(isBulk && vmDocList.length ==0)">
                        * Please select documents from the list
                    </v-chip>
                </v-col>
            </v-row>
            <v-row v-if="docList.length > 0">
                <v-col cols="12" xs="12" sm="12" md="12" class="mb-0">
                    <v-row no-gutters>
                        <v-col cols="12" md="12" sm="12" xs="12">
                            <v-data-table :headers="docListTableheaders"
                                          :items="docList"
                                          show-select
                                          item-key="DocumentID"
                                          sort-by="DocumentName"
                                          v-model="vmDocList"
                                          :search="searchDoc"
                                          @item-selected="isBulk=false">
                                <template v-slot:item.DocumentName="{ item }">
                                    <div class="pa-0 text-limit" style="width:180px">
                                        {{item.DocumentName }}
                                    </div>
                                </template>
                                <template v-slot:item.DocumentSize="{ item }">
                                    <div class="pa-0 text-limit" style="width:100px">
                                        {{item.DocumentSize }}
                                    </div>
                                </template>
                                <template v-slot:item.UploadedUser="{ item }">
                                    <!--<div class="pa-0 text-limit" style="width:100px">
                                        {{item.UploadedUser }}
                                    </div>-->
                                    <v-list class="py-0 table-v-list">
                                        <v-list-item class="my-0 px-0">
                                            <div class="pa-0 text-limit" style="width:200px">
                                                <v-avatar size="30" light :color="`${getcolor(item.UploadedUser)}`">
                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                    <span class="white--text" v-else>  {{item.UploadedUser == null ? "" :item.UploadedUser.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>
                                                {{item.UploadedUser}}
                                            </div>
                                        </v-list-item>
                                    </v-list>
                                </template>
                                <template v-slot:item.LinkedTo="{ item }">
                                    <div class="pa-0 text-limit" style="width:100px">
                                        {{item.LinkedTo }}
                                    </div>
                                </template>
                                <template v-slot:item.UploadedDate="{ item }">
                                    <div class="pa-0 text-limit" style="width:135px">
                                        {{formatDate(new Date(item.UploadedDate).toISOString().substr(0, 10))+' ' + item.UploadedDate.substr(11, 5) }}
                                    </div>
                                </template>
                                <!--user list actions-->
                                <template v-slot:item.Actions="{item}">
                                    <v-menu close-on-content-click transition="scroll-y-transition" left offset-y v-bind:disabled="!screenAccess">
                                        <template v-slot:activator="{ on }">
                                            <v-btn id="AutoTest_IP418" min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0" @click="isBulk=false">
                                                <v-icon>mdi-dots-vertical</v-icon>
                                            </v-btn>
                                        </template>
                                        <v-list color="white" light width="auto" class="action-button-list">

                                            <!-- Rename Doument name-->
                                            <v-list-item id="AutoTest_IP419" link @click.stop="RenameDocumentDialog = true,docID=item.DocumentID,docName= item.DocumentName">
                                                <v-list-item-icon>
                                                    <v-icon>mdi-file-edit-outline</v-icon>
                                                </v-list-item-icon>
                                                <v-list-item-content>Rename</v-list-item-content>
                                            </v-list-item>

                                            <!-- Remove-->
                                            <v-list-item id="AutoTest_IP420" link @click.stop="DeleteDocumentDialog = true,isBulk =false,docID=item.DocumentID,docUName=item.UniqueDocumentName,docName= item.DocumentName ">
                                                <v-list-item-icon>
                                                    <v-icon>mdi-trash-can-outline</v-icon>
                                                </v-list-item-icon>
                                                <v-list-item-content>Remove</v-list-item-content>
                                            </v-list-item>
                                            <!-- Download Document-->
                                            <v-list-item id="AutoTest_IP421" link @click.stop="">
                                                <v-list-item-icon>
                                                    <v-icon>mdi-file-download-outline</v-icon>
                                                </v-list-item-icon>
                                                <v-list-item-content id="AutoTest_IP422" @click="DownloadDocument(item.DocumentName,item.UniqueDocumentName)">Download</v-list-item-content>
                                            </v-list-item>

                                        </v-list>
                                        <!-- Rename Documnet-->
                                        <v-dialog v-model="RenameDocumentDialog" width="800" persistent>
                                            <v-card>
                                                <v-card-title primary-title class="page-head py-4">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                        <v-icon size="20" color="primary">mdi-pencil</v-icon>
                                                    </v-avatar>
                                                    Rename
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP423" text @click="RenameDocumentDialog = false,vmDocName ='',docID=0,docName=''">
                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-card-text class="pt-0">
                                                    <v-container fluid class="pt-0">
                                                        <v-row no-gutters>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <v-container fluid class="pt-0">
                                                                    <v-row>
                                                                        <v-col cols="12" md="12" class="pt-0">
                                                                            <h4 class="text-center heading-3">Please enter a new name for the document.</h4>
                                                                        </v-col>
                                                                        <v-row class="heading-3" :justify="justifyCenter">
                                                                            <v-col md="6">
                                                                                <v-text-field outlined dense label="Document" type="Text" v-model="vmDocName">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                            <Secondary-Button id="AutoTest_IP424" class="btn-122x36 mr-2" title="Rename" @click.native="RenameDocument(docID,docName,vmDocName)"></Secondary-Button>
                                                                            <Default-Button-Outlined id="AutoTest_IP425" class="btn-122x36" title="Cancel" @click.native="RenameDocumentDialog = false,docID=0,docName='',vmDocName=''"></Default-Button-Outlined>
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
                    <v-row>
                        <v-col>
                            <!-- Remove Document-->
                            <v-dialog v-model="DeleteDocumentDialog" width="800" persistent>
                                <v-card>
                                    <v-card-title primary-title class="page-head py-4">
                                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                        </v-avatar>
                                        Remove Document
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP426" text @click="DeleteDocumentDialog = false,docID =0,docName='',removeList=''">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-card-text class="pt-0">
                                        <v-container fluid class="pt-0">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid class="pt-0">
                                                        <v-row>
                                                            <v-col cols="12" md="12" class="pt-0">
                                                                <h4 class="text-center heading-3">Are you sure want to remove this Document ?</h4>
                                                            </v-col>
                                                            <v-row class="heading-3" :justify="justifyCenter" v-if="!isBulk">
                                                                <h3 class="text-left page-head">
                                                                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                                                        <v-icon size="15" color="primary">mdi-file</v-icon>
                                                                    </v-avatar>
                                                                    {{docName}}
                                                                </h3>
                                                            </v-row>
                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                                <Secondary-Button id="AutoTest_IP427" class="btn-122x36 mr-2" title="Remove" @click.native="DeleteDocument(isBulk)"></Secondary-Button>
                                                                <Default-Button-Outlined id="AutoTest_IP428" class="btn-122x36" title="Cancel" @click.native="DeleteDocumentDialog = false,docID=0,docName=''"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-card-text>
                                </v-card>
                            </v-dialog>
                        </v-col>
                    </v-row>

                </v-col>
            </v-row>
            <!--if no data-->
            <v-row v-else :justify="justifyCenter">
                <v-col md="12" class="text-center">
                    <p class="mt-12 pt-12">
                        <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                    </p>

                    <p class="heading-3-dark font-weight-medium pt-6">No documents available</p>
                </v-col>
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
    </v-card>
</template>
<script type="text/javascript">      
    import SaveButtonSuccessSmall from '@/components/save-button-success-small.vue'
    import CancelButtonOutlinedSm from '@/components/cancel-button-outlined-sm.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
  
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
                PreLoader: false,
                docListTableheaders: [                
                    { text: "Name", align: "left", value: "DocumentName"},
                    { text: "Size", value: "DocumentSize" },
                    { text: "Uploaded date", value: "UploadedDate"},
                    { text: "Uploaded By", value: "UploadedUser" },
                    { text: "Linked To", value: "LinkedTo" },
                    { text: "Actions", align: "center", value: "Actions", width: "100px" },
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
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                userimage: "",
            };
        },
        components: {           
            'Save-Button-Success-Small': SaveButtonSuccessSmall,
            'Cancel-Button-Outlined-Sm': CancelButtonOutlinedSm,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
        },
        mounted: async function () {

            this.ProjectID = this.$route.query.ProjID;             
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.userimage = window.SERVER_URL + "/Upload";
            await importscript();   
            this.PreLoader = true;
            await this.GetAllDocumentList();
            await this.GetProjectName();
            this.PreLoader = false;
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
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
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

            //============= Filter Document ==========================
            GetFilteredDocList() {
               
                var docListTemp = [];
                this.docList = [];
                this.filterChipList = [];              
                this.filterMenu = false;

                if (!this.chkRisks && !this.chkProject && !this.chkKPI && !this.chkActivities && !this.chkPayment) {
                    this.filterChipList = [];
                    this.docList = this.allDocList;
                }
                if (this.chkActivities) {
                    this.filterChipList.push({ ChipName: "Activities" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Activity');
                    for (var i = 0; i < this.docListTemp.length; i++)                        
                        this.docList.push(this.docListTemp[i])
                }
                if (this.chkProject) {

                    this.filterChipList.push({ ChipName: "Projects" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Project');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.chkKPI) {

                    this.filterChipList.push({ ChipName: "KPI" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'KPI');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.chkRisks) {

                    this.filterChipList.push({ ChipName: "Risks" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Risks');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.chkPayment) {

                    this.filterChipList.push({ ChipName: "Payment Transaction" });
                    this.docListTemp = this.allDocList.filter(p => p.UploadedFrom == 'Payment Transaction');
                    for (var i = 0; i < this.docListTemp.length; i++)
                        this.docList.push(this.docListTemp[i])
                }
                if (this.chkRisks && this.chkProject && this.chkKPI && this.chkActivities && this.chkPayment) 
                    this.docList = this.allDocList;                            
            },
            removeFilterChip(chipitem) {               
                this.filterChipList = this.filterChipList.filter(x => x.ChipName != chipitem.ChipName);

                if (chipitem.ChipName == "Projects")
                    this.chkProject = false;
                else if (chipitem.ChipName == "Risks")
                    this.chkRisks = false;
                else if (chipitem.ChipName == "KPI")
                    this.chkKPI = false;
                else if (chipitem.ChipName == "Activities")
                    this.chkActivities = false;
                else if (chipitem.ChipName == "Payment Transaction")
                    this.chkPayment = false;
                this.GetFilteredDocList();
            },
            //============= Upload Document ==========================
            openFileDialog() {
                this.isBulk = false;
                document.getElementById('file').click();
            },
            formatFileSize(bytes, decimalPoint) {
             
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },
            async onFileUpload() {  
                                
                const formData = new FormData()
                formData.append('myFile', this.fileInput);
                formData.append('DocumentOriginalName', this.fileInput.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', this.formatFileSize(this.fileInput.size, 0));
                formData.append('DocumentType', this.fileInput.type);
                formData.append('DocumentTypeIcon', this.fileInput.name.split('.').pop());
                if (this.fileInput.lastModifiedDate !=null) {
                    var lstModDate = new Date(this.fileInput.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.fileInput.lastModifiedDate).toISOString().substr(11, 5);
                    formData.append('DocumentLastModifiedDate', lstModDate);

                }
                formData.append('ProjectId', this.ProjectID);
                formData.append('UploadedFrom', "Project");               
                formData.append('LinkedTo', this.ProjectName);
                formData.append('LinkedToID', this.ProjectID);

                var result = await objProject.UploadDocument(formData);
               
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "Document uploaded successfully...";
                        this.GetAllDocumentList();
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in uploading document...";
                        this.errorSnackbarMsg = true;
                    }
                        
                    
                }
               
            },      
            //==========================================================
            async RenameDocument(docID,docName,newDocName) {   
                var data = new FormData();
                data.append("docID", docID);
                data.append("oldFileName", docName);
                data.append("newFileName", newDocName.split('.'));
                var result = await objProject.RenameDocument(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = "Document renamed successfully...";
                        this.GetAllDocumentList();
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in renaming the document...";
                        this.errorSnackbarMsg = true;
                    }
                        
                }
                this.RenameDocumentDialog = false;
            },
            async DeleteDocument(isBulk) {             
                if (!isBulk)
                    this.removeList.push({ DocumentID: this.docID, UniqueDocumentName: this.docUName });
                else
                    for (var i = 0; i < this.vmDocList.length; i++) 
                        this.removeList.push(this.vmDocList[i]); 
                
                var data = new FormData();               
                data.append("DocList", JSON.stringify(this.removeList));
                var result = await objProject.RemoveDocument(data);
               
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = "Document removed ...";
                        this.GetAllDocumentList();
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in removing the document...";
                        this.errorSnackbarMsg = true;
                    }
                        
                }
                this.DeleteDocumentDialog = false;
                this.removeList = [];
               

            },
            async DownloadDocument(docName,docUName) { 
               
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
        }
    };
</script>
<style scoped>
    .text-limit {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

        .text-limit:hover {
            white-space: normal;
            overflow: visible;
        }
</style>
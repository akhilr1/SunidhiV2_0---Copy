<template>
    <div class="gridMainContainer" ref="gridMain">
        <div class="spinnerWrapper" v-if="isLoading">
            <v-progress-circular
                class="gridSpinner"
                :size="70"
                color="primary2"
                indeterminate
            ></v-progress-circular>
        </div>
        <!-- Survey Grid Controls -->
        <v-card-title class="actionsContainer" v-if="responseMode">
            Responses
            <v-spacer/>
            <!-- <v-btn icon v-show="isLoading" :loading="isLoading"><v-icon>mdi-refresh</v-icon></v-btn> -->
            <v-text-field
                v-model="quickFilterBox"
                append-icon="mdi-magnify"
                label="Search"
                :hide-details="true"
                outlined
                dense
                clearable
                class="mx-2"
                style="max-width: 300px"
            ></v-text-field>
            <v-btn
                depressed
                dense
                @click="clearAllFilters"
                v-if="showClearFilter"
                class="d-flex justify-space-between align-center"
            >
                Clear Filters
                <v-icon>mdi-close</v-icon>
            </v-btn>
            <v-btn v-show="undoRedo" icon @click="undo()" :disabled="!undoBtn"><v-icon>mdi-undo</v-icon></v-btn>
            <v-btn v-show="undoRedo" icon @click="redo()" :disabled="!redoBtn"><v-icon>mdi-redo</v-icon></v-btn>
            <v-btn
                v-if="disableEdit"
                class="mx-1"
                text
                dense
                @click="editSurvey('init')"
            >
                Edit 
                <v-icon class="ml-2">mdi-pencil-outline</v-icon>
            </v-btn>
            <v-btn
                v-if="!disableEdit"
                class="mx-1"
                text
                dense
                @click="editSurvey('cancel')"
            >
                Cancel 
                <v-icon class="ml-2">mdi-close</v-icon>
            </v-btn>
            <v-btn
                v-if="!disableEdit"
                class="mx-1"
                color="primary2"
                dark
                dense
                @click="editSurvey('save')"
            >
                Save
                <v-icon class="ml-2">mdi-content-save</v-icon>
            </v-btn>
            <v-btn 
                class="mx-1" 
                text 
                dense 
                color="red" 
                @click="deleteRowController"
            >
                Delete 
                <v-icon class="ml-2">mdi-delete-outline</v-icon>
            </v-btn>
            <v-btn
            class="mx-1"
            depressed
            dense
            color="primary2"
            dark
            @click="exportController"
            ><v-icon class="mr-2">mdi-export-variant</v-icon>Export</v-btn
            >
        </v-card-title>
        <!-- MyTasks Controls -->
        <div class="actionsContainer" v-if="false">
            <!-- Approve / Reject Button for All surveys Section Only-->
            <!-- <v-checkbox v-if="!disableEdit" v-model="beneficiaryEdit" label="Enable Editing Beneficiaries"
                :hide-details="true" class="my-auto ml-2"
                @click="beneficiaryEdit && (benefEditConfirmDlg = true)"></v-checkbox> -->
            <v-spacer />
            <!-- <v-text-field v-model="quickFilterBox" append-icon="mdi-magnify" label="Search" :hide-details="true" solo
                dense clearable class="mx-2" style="max-width: 300px;" flat>
            </v-text-field> -->
            <!-- <v-btn depressed dense @click="clearAllFilters" v-if="showClearFilter"
                class="d-flex justify-space-between align-center">Clear Filters<v-icon>mdi-close</v-icon>
            </v-btn> -->

            <!-- Edit Survey Button for Sections -->
            <!-- <v-btn v-show="undoRedo" icon @click="undo()" :disabled="!undoBtn"><v-icon>mdi-undo</v-icon></v-btn>
            <v-btn v-show="undoRedo" icon @click="redo()" :disabled="!redoBtn"><v-icon>mdi-redo</v-icon></v-btn> -->
            <!-- Edit Init -->
            <!-- <v-btn color="primary2" class="mr-2 _btn_rounded" @click="editSurvey('init')" v-if="disableEdit" dark text
                depressed>
                Edit
            </v-btn> -->
            <!-- Edit Save / Discard -->
            <!-- <v-btn color="primary" class="mr-2" @click="editSurvey('cancel')" v-if="!disableEdit" text>
                Cancel
            </v-btn>
            <v-btn color="primary2" class="mr-2 _btn_rounded" @click="editSurvey('save')" v-if="!disableEdit" dark
                depressed>
                Save Changes
            </v-btn>
            <v-btn class="mx-1" depressed dense color="primary2" dark @click="exportController"><v-icon
                    class="mr-2">mdi-export-variant</v-icon>Export</v-btn> -->
        </div>
        <!-- excel Editor section -->
        <div class="editorContainer" ref="editorMain">
            <ag-grid-vue 
                :style="{width: width, height: responseMode ? height : 'auto'}" 
                :class="agTheme" 
                rowSelection="multiple"
                :columnDefs="surveyColDefs" 
                :rowData="surveyResponses" 
                :suppressRowClickSelection="true"
                :overlayLoadingTemplate="loadingOverlay" 
                :undoRedoCellEditing="true" 
                :undoRedoCellEditingLimit="50" 
                :enableCellChangeFlash="true" 
                :animateRows="true"
                :pagination="paginationToggle"
                :paginationPageSize="20"
                :suppressPaginationPanel="true"
                :domLayout="domLayout"
                @gridReady="onGridReady" 
                @firstDataRendered="onFirstDataRendered" 
                @selection-changed="onSelectionChanged" 
                @cellValueChanged="onCellValueChanged"
                @filterChanged="onFilterChanged"
                @cellClicked="cellClickWatchDog"    
            />
        </div>
        <!-- custom pagination controls -->
        <div class="editorFooter" v-if="responseMode">
            <div class="contextContainer v-scrolling-div">
                <!-- tabActions -->
                <v-btn v-for="sheet in sheetsList" color="#EBF5FF" depressed class="mr-2 _btn_rounded"
                    @click="switchDataSets(sheet.id)">{{ sheet.name }}<v-icon>{{ sheet.id === currentSheetID ? "mdi-chevron-up" : "" }}</v-icon></v-btn>
            </div>

            <div class="paginationControl" v-if="gridApi">
                <div class="d-inline-flex align-center"><p class="mr-2 mb-0 pageNoLabel">Page No: </p><div class="page">{{ gridApi.paginationGetCurrentPage() + 1 }}</div></div>
                <div v-if="gridApi.paginationGetTotalPages() > 1" class="d-inline-flex align-center mx-1"><p class="mr-2 mb-0 pageNoLabel">of</p><div class="page">{{ gridApi.paginationGetTotalPages() }}</div></div>
                <v-btn :disabled="gridApi.paginationGetCurrentPage() === 0" :ripple="false" depressed color="primary2" icon @click="gridApi.paginationGoToPreviousPage()"><v-icon>mdi-chevron-left</v-icon></v-btn>
                <v-btn :disabled="gridApi.paginationGetTotalPages() === gridApi.paginationGetCurrentPage() + 1  " :ripple="false" depressed color="primary2" icon @click="gridApi.paginationGoToNextPage()"><v-icon>mdi-chevron-right</v-icon></v-btn>
            </div>
        </div>
        <!-- cpc END -->
        <!-- Beneficiar Edit Confirmation Dialog -->
        <v-dialog v-model="benefEditConfirmDlg" max-width="500">
          <v-card>
            <v-card-title>Edit Beneficiary Details ?</v-card-title>
            <v-card-text>
              <p>Editing Beneficiary Details will affect all the Surveys linked to the selected Beneficiary</p>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="primary2" text @click="benefEditConfirmDlg = false, beneficiaryEdit = false"> Cancel </v-btn>
              <v-btn
                color="primary2"
                class="_btn_rounded"
                dark
                depressed
                @click="benefEditConfirmDlg = false, gridApi.refreshCells({
                    force: true,
                    suppressFlash: true
                })"
              >
                Confirm
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <!-- Dialog END -->
        
        <!-- snackbar -->
        <v-snackbar top v-model="snackbar.value" :color="snackbar.color">
          {{ snackbar.message }}
        </v-snackbar>
    </div>
</template>

<script>
import { AgGridVue } from 'ag-grid-vue';
import { dbImworks } from '@/ImworksDb';
import TaskBulkActions from '@/BL/TaskBulkActions';
import { createNamespacedHelpers } from 'vuex';
const { mapGetters: gGetters, mapActions: gActions } = createNamespacedHelpers('GridContext');
export default {
    components: {
        AgGridVue
    },
    props: {
        rawData: {
            type: Object,
        },
        contextData: {
            type: Object,
        },
        responseMode: {
            type: Boolean,
        },
        gridTheme: {
            type: String,
            default: 'alpine'
        },
        height: {
            type: String,
            default: '350px'
        },
        width: {
            type: String,
            default: '100%'
        }
    },
    data() {
        return {
            TaskActions: new TaskBulkActions(),
            disableEdit: true,
            // beneficiaryEdit: false,
            benefEditConfirmDlg: false,
            showClearFilter: false,
            undoRedo: false, 
            undoBtn: false,
            redoBtn: false,
            currentUser: null,
            loadingOverlay: '',
            sheet: {},
            sheetsList: [],
            currentSheetID: '',
            gridApi: null,
            gridColumnApi: null,
            isLoading: false,
            isBeneficiary: {
                linked: false,
                TypeID: 0,
            },
            surveyQuestionType: null,
            sectionLogicMap: null,
            surveyResponsesMap: null,
            questionMap: new Map(),
            BenefRegMap: new Map(),
            BenefQAMap: new Map(),
            BenefDetails: [],
            surveyColDefs: [],
            surveyResponses: [],
            SyncDBID: 0,
            updationData: [],
            selectedRows: [],
            showClearFilter: false,
            fileUrl: window.SERVER_URL + "/Upload/SureyQuestionnaire",
            BeneffileUrl: window.SERVER_URL + "/Upload/BeneficiaryRegistration",
            ProjectId: this.$route.query.ProjectId,
            SurveyId: this.$route.query.SurveyID,
            responseExtras: null,
            snackbar: {
                message: "",
                value: false,
                color: "primary2",
            },
            domLayout: "normal"
        }
    },
    computed: {
        ...gGetters([
            'logicList',
            'currentLogic',
            'filterBox',
            'editToggle',
            "beneficiaryEdit",
            "globalUpdationData"
        ]),
        agTheme() {
            return `ag-theme-${this.gridTheme}`;
        },
        quickFilterBox: {
            get() {
                if (this.gridApi) {
                    return this.gridApi.quickFilterText;
                } else {
                    return "";
                }
            },
            set(value) {
                if (this.gridApi) {
                    this.gridApi.setQuickFilter(value);
                }
                return;
            },
        },
        _beneficiaryEdit: {
            get() {
                return this.beneficiaryEdit
            },
            set(val) {
                this.setBeneficiaryEditToggle(val)
            }
        },
        paginationToggle() {
            return this.responseMode
        }
    },
    async created() {
        this.currentUser = (await dbImworks.User.where({ IsLoggedIn: 1 }).toArray())[0];
        this.loadingOverlay = `<span class="ag-overlay-loading-center">Please wait while survey answers are loaded</span>`; 
    },
    async mounted() {
        // this.loadResponses(this.rawData)
        if(!this.SurveyId) {
            // Match Query from Survey Responses Page
            this.SurveyId = this.$route.query.SID
        }
    },
    methods: {
        ...gActions([
            "updateLogicList",
            "updateUpdationData",
            "updateMaxScrollWidth",
            "updateGrids"
        ]),
        async onGridReady(params) {
            this.gridApi = params.api;
            this.gridColumnApi = params.columnApi;
            console.log('Grid Loaded', this.responseMode)
            // this.updateGrids({grid: this.gridApi, uid: })
            if(this.responseMode) {
                console.log('Survey Responses Mode')
                const result = await this.getAllSurveyResponses()
                await this.loadResponses(params, result)
                return
            } else {
                console.log('Task Responses Mode')
                this.gridApi.setDomLayout("autoHeight")
                this.SyncDBID = this.rawData.lstDetails.MyTaskSummary[0].SyncTaskID
                this.updateGrids({grid: this.gridApi, uid: this.SyncDBID})
                await this.loadResponses(params, this.rawData)
                this.gridApi.setQuickFilter(this.filterBox)
                return
            }

        },
        onFirstDataRendered(params) {
            this.$emit('gridLoaded', this.$refs.editorMain.offsetHeight)
            this.updateMaxScrollWidth(this.gridColumnApi.getAllColumns().reduce((a,b) => a + b.actualWidth, 0));
        },
        onSelectionChanged(params) {
            this.selectedRows = params.api.getSelectedRows();
        },
        onCellValueChanged(params) {
            let newValue = params.newValue;
            const undoSize = params.api.getCurrentUndoSize();
            undoSize > 0 ? (this.undoBtn = true) : (this.undoBtn = false);
            const redoSize = params.api.getCurrentRedoSize();
            redoSize > 0 ? (this.redoBtn = true) : (this.redoBtn = false);
            let QuestionID = params.colDef.meta.question.QuestionID;
            let isBeneficiaryAnswer = params.colDef.meta.isBeneficiaryQuestion
            let pID = params.data[QuestionID].ID;
            let payload = {
                ID: pID,
                QuestionID,
                Answer: newValue,
            }
            if (isBeneficiaryAnswer) payload['isBeneficiary'] = true
            if(this.responseMode) {
                let index = this.updationData.findIndex((x) => x.ID === pID);
                if (index >= 0) {
                    this.updationData[index].Answer = newValue;
                } else {
                    this.updationData.push(payload);
                }
            } else {
                this.updateUpdationData(payload);
            }
        },
        async loadResponses(params, data) {
            if(params.type !== 'gridReady') return;
            if(this.isLoading) return
            this.isLoading = true
            // this.gridApi.showLoadingOverlay();
            try {
                if(!data) throw new Error("No Row Found")
                const { IndResponses: responseStack, lstDetails } = data
                const { MyTaskResponseDetails: ResponseDetails, MyTaskSummary } = lstDetails
                const {
                    lstMyTaskBeneficiaryTypeQuestion: BenefTypeQuestions,
                    lstTbMyTaskProjectBeneficiaryType: ProjBenefType,
                    lstTbMyTaskBeneficiaryRegistrationDetails: BenefRegistrationDetails,
                    lstMyTaskSurveyQuestion: SurveyQstnList,
                    lstMyTaskSurveySectionLogic: SurveySectionLogic,
                    lstMyTaskQuestionType: SurveyQstnType,
                    lstMyTaskProjectSurveyQSTOption: SurveyQSTOpt,
                    lstMyTaskBeneficiarySurveyCascadingQuestionOptions: SurveyCQOptions,
                    lstMyTaskBeneficiarySurveyQSTOption: BenSurveyQSTOptions,
                    lstMyTaskProjectBenSurveyQSTCondition: SurveyQSTConditions,
                } = this.responseExtras || this.contextData
            
                if( (!ResponseDetails || !ResponseDetails.length) || 
                    (!responseStack || !responseStack.length) ||
                    (!MyTaskSummary || !MyTaskSummary.length)) {
                    throw new Error("Something went wrong")
                } else if(MyTaskSummary[0].BeneficiaryTypeID) {
                    this.isBeneficiary = {
                        linked: true,
                        TypeID: MyTaskSummary[0].BeneficiaryTypeID
                    }
                }

                if(this.responseMode) {
                    this.surveyColDefs.push({
                            headerName: "",
                            field: "check",
                            width: 50,
                            checkboxSelection: true,
                            headerCheckboxSelection: true,
                            meta: {
                                isDefaultQuestion: true
                            }
                        },
                    )
                }
                // Map the Objects for Faster Access
                this.surveyQuestionType = new Map(SurveyQstnType.map(x => [x.ID, x]))
                this.sectionLogicMap = new Map(SurveySectionLogic.map(x => [x.SectionID, x]))
                this.surveyResponsesMap = new Map(responseStack.map(x => [x.ID, x]))
                this.questionMapper(SurveyQstnList)
                
                if(this.isBeneficiary.linked) {

                    if(BenefRegistrationDetails && BenefRegistrationDetails.length) {
                        this.BenefDetails = BenefRegistrationDetails.filter(x => x.BeneficiaryTypeID === this.isBeneficiary.TypeID)
                    }

                    BenefRegistrationDetails.forEach((brd) => {
                        if(!this.BenefRegMap.has(brd.BeneficiaryID)) {
                            this.BenefRegMap.set(brd.BeneficiaryID, [])
                        }

                        this.BenefRegMap.get(brd.BeneficiaryID).push(brd)
                    })
                    this.getBenefQstnAns(this.isBeneficiary.TypeID, BenefTypeQuestions)
                        .forEach((x) => {
                            if (!this.BenefQAMap.has(x.BeneficiaryID)) {
                                this.BenefQAMap.set(x.BeneficiaryID, [])
                            }
                            this.BenefQAMap.get(x.BeneficiaryID).push(x)
                        })

                }
                ResponseDetails.forEach((entry) => {
                    let responseObj = {}
                    if(entry.length) {
                        let isColExists = this.surveyColDefs.some(x => x.headerName.includes("Submitted Date") || x.headerName.includes("Submitted By"))
                        if(!isColExists) {
                            this.surveyColDefs.push({
                                headerName: "Submitted Date",
                                valueGetter: (params) => {
                                    return new Date(params.data["SyncDetails"]?.SubmittedDateTime).toLocaleString("en-IN")
                                },
                                editable: false,
                                sortable: false,
                                meta: {
                                    isDefaultQuestion: true,
                                },
                                cellClass: () => {
                                    if(!this.disableEdit) {
                                        return 'disableCell'
                                    } else {
                                        return ''
                                    }
                                }
                            })
                            this.surveyColDefs.push({
                                headerName: "Submitted By",
                                valueGetter: (params) => {
                                    return params.data["SyncDetails"]?.SubmittedBy
                                },
                                editable: false,
                                sortable: false,
                                meta: {
                                    isDefaultQuestion: true,
                                },
                                cellClass: () => {
                                    if(!this.disableEdit) {
                                        return 'disableCell'
                                    } else {
                                        return ''
                                    }
                                }
                            })
                        }
                        this.addBeneficiaryColsAns(entry, responseObj, this.surveyColDefs)
                        entry.forEach((response, rIndex) => {
                            let tempQstnID = response.QuestionId
                            const matchingResponse = this.surveyResponsesMap.get(response.ID)
                            let currentQuestion = this.questionMap.get(tempQstnID)
                            let _response = {
                                ...response,
                                OrderBy: rIndex,
                                isBeneficiaryQuestion: false,
                                QuestionDetails: currentQuestion
                            }

                            if(matchingResponse) {
                                _response = {
                                    ...matchingResponse,
                                    ..._response
                                }
                            }

                            responseObj[tempQstnID] = _response
                            let isColExists = this.surveyColDefs.some(x => {
                                return !x.meta.isBeneficiaryQuestion &&
                                !x.meta.isDefaultQuestion &&
                                x.headerName === currentQuestion.QuestionName &&
                                x.colId.includes(tempQstnID)
                            })
                            const isRepeat = this.sectionLogicMap.get(currentQuestion.SectionID)?.LogicType.includes('Repeat')
                            if(!isColExists && !isRepeat) {
                                this.surveyColDefs.push({
                                    colId: `Survey_${tempQstnID}`,
                                    headerName: currentQuestion.QuestionName,
                                    valueGetter: (params) => {
                                        return params.data[tempQstnID]?.Answer
                                    },
                                    valueSetter: (params) => {
                                        params.data[tempQstnID].Answer = params.newValue
                                        return true
                                    },
                                    editable: (params) => {
                                        if (params.data[tempQstnID]?.Answer) return !this.disableEdit;
                                        else return false;
                                    },
                                    cellClass: (params) => {
                                        let answer = params.data[tempQstnID]?.Answer
                                        const arr = [this.isHyperLink(answer)]
                                        if (!this.disableEdit) {
                                            let editable = params.column.isCellEditable(params.node)
                                            arr.push(this.isEditableCell(editable))
                                        }
                                        return arr
                                    },
                                    meta: {
                                        isBeneficiaryQuestion: false,
                                        isDefaultQuestion: false,
                                        isSurveyQuestion: true,
                                        isConditionQuestion: false,
                                        question: currentQuestion
                                    },
                                    flex: 0,
                                    sortable: true,
                                    resizable: true,
                                    filter: true,
                                })
                            }
                        })
                        responseObj["SyncDetails"] = MyTaskSummary.find(x => x.SyncTaskID === entry[0].SyncTaskID)
                        if (Object.keys(responseObj).length) {
                            this.surveyResponses.push(responseObj)
                        }
                    }
                })

                this.autoSizeAll()
                this.sheetsList.push({
                    name: "Survey Responses",
                    id: this.SyncDBID,
                    isLogicSection: false,
                    responses: this.surveyResponses,
                    colDefs: this.surveyColDefs,
                    isSurveyResponses: true,
                })
                this.currentSheetID = this.SyncDBID
                this.loadLogicResponses(SurveySectionLogic, responseStack, MyTaskSummary, SurveyQSTOpt)
            } catch (error) {
                console.error(error)
                this.snackbar = {
                    value: true,
                    message: "Error Fetching Data",
                    color: "error",
                }
            } finally {
                this.isLoading = false
                // this.gridApi.hideOverlay();
                if (!this.gridApi.getDisplayedRowCount()) {
                    this.gridApi.showNoRowsOverlay();
                }
            }
        },
        loadLogicResponses(SurveySectionLogic, responseStack, MyTaskSummary, SurveyQSTOpt) {
            if (!SurveySectionLogic.length || !responseStack.length) return
            for (let i = 0; i < SurveySectionLogic.length; i++) {
                const section = SurveySectionLogic[i]
                const sectionLogic = section.LogicType.split(' ')[0]
                if (sectionLogic.toLowerCase().includes('skip')) {
                    continue;
                }
                const { SectionID: rptSectionID } = section
                const currentQuestion = this.questionMap.get(section.QuestionID)
                const { QuestionName } = currentQuestion
                let logicSheetName = QuestionName
                const QuestionOpts = SurveyQSTOpt.filter(x => x.QuestionID === section.QuestionID)
                if (QuestionOpts && QuestionOpts.length > 1) {
                    logicSheetName = `${QuestionName} - ${section.Answer}`
                }
                const logicResponses = responseStack.filter(response => response.SectionID === rptSectionID);
                let logicalColDefs = []
                if (this.responseMode) {
                    logicalColDefs.push({
                        headerName: "",
                        field: "check",
                        width: 50,
                        checkboxSelection: true,
                        headerCheckboxSelection: true,
                        meta: {
                            isDefaultQuestion: true
                        }
                    })
                }
                const logicResponsesBySID = new Map()
                logicResponses.forEach(response => {
                    const id = `${response.SyncTaskID}${response.ResponseID}`
                    if (!logicResponsesBySID.has(id)) {
                        logicResponsesBySID.set(id, [])
                    }

                    logicResponsesBySID.get(id).push(response)
                })

                const logicResponsesArray = []
                const duplicateColResponses = []
                Array.from(logicResponsesBySID.entries()).forEach((res) => {
                    let responseObj = {}
                    if (this.isBeneficiary.linked) {
                        let isColExists = logicalColDefs.some(x => x.headerName.includes("Submitted Date") || x.headerName.includes("Submitted By"))
                        if (!isColExists) {
                            logicalColDefs.push({
                                // colId: `Submitted_${}`,
                                headerName: "Submitted Date",
                                valueGetter: (params) => {
                                    return new Date(params.data["SyncDetails"]?.SubmittedDateTime).toLocaleDateString("en-IN")
                                },
                                editable: false,
                                sortable: true,
                                filter: true,
                                meta: {
                                    isDefaultQuestion: true,
                                },
                                cellClass: (params) => {
                                    if (!this.disableEdit) {
                                        return 'disableCell'
                                    } else {
                                        return ''
                                    }
                                }
                            })
                            logicalColDefs.push({
                                // colId: `Submitted_${}`,
                                headerName: "Submitted By",
                                valueGetter: (params) => {
                                    return params.data["SyncDetails"]?.SubmittedBy
                                },
                                editable: false,
                                sortable: true,
                                filter: true,
                                meta: {
                                    isDefaultQuestion: true,
                                },
                                cellClass: (params) => {
                                    if (!this.disableEdit) {
                                        return 'disableCell'
                                    } else {
                                        return ''
                                    }
                                }
                            })
                        }
                    }
                    let BeneficiaryID = res[1][0].ResponseID
                    this.addBeneficiaryColsAns(res[1], responseObj, logicalColDefs)
                    res[1].forEach((response) => {
                        let tempQstnID = response.QuestionID
                        const currentQuestion = this.questionMap.get(tempQstnID)
                        let isExists = Object.keys(responseObj).find((x) => x.toString().includes(response.QuestionID))
                        if (isExists) {
                            duplicateColResponses.push(response)
                        } else {
                            responseObj[tempQstnID] = response
                        }
                        let isColExists = logicalColDefs.some(x => x.headerName === currentQuestion.QuestionName)
                        if (!isColExists) {
                            logicalColDefs.push({
                                headerName: currentQuestion.QuestionName,
                                valueGetter: (params) => {
                                    return params.data[tempQstnID]?.Answer
                                },
                                valueSetter: (params) => {
                                    params.data[tempQstnID].Answer = params.newValue
                                },
                                meta: {
                                    QuestionId: tempQstnID,
                                    question: currentQuestion
                                },
                                editable: () => {
                                    return !this.disableEdit
                                },
                                cellClass: (params) => {
                                    let answer = params.data[tempQstnID]?.Answer
                                    const arr = [this.isHyperLink(answer)]
                                    if (!this.disableEdit) {
                                        let editable = params.column.isCellEditable(params.node)
                                        arr.push(this.isEditableCell(editable))
                                    }
                                    return arr
                                },
                                sortable: true,
                                resizable: true,
                                filter: true,
                            })
                        }
                    })
                    if (Object.keys(responseObj).length) {
                        responseObj["SyncDetails"] = MyTaskSummary.find(x => x.SyncTaskID === res[1][0].SyncTaskID)
                        logicResponsesArray.push(responseObj)
                    }
                })
                if(duplicateColResponses.length) {
                    logicResponsesArray.push(...this.processDuplicateRows(duplicateColResponses, MyTaskSummary))
                }
                this.sheetsList.push({
                    name: `${sectionLogic} - ${logicSheetName}`,
                    id: section.SectionID,
                    isLogicSection: true,
                    responses: logicResponsesArray,
                    colDefs: logicalColDefs
                })
                if(!this.logicList.includes(`${sectionLogic} - ${logicSheetName}`)) {
                    this.updateLogicList(`${sectionLogic} - ${logicSheetName}`);
                }
            }
        },
        processDuplicateRows(duplicates, MyTaskSummary) {
            if (!duplicates.length) return
            duplicates.sort((a, b) => a.ID - b.ID);
            const duplicateMap = new Map()
            let pos = 0
            const questionIds = duplicates.map((x) => x.QuestionID);
            for (let i = 0; i < duplicates.length; i++) {
                const el = duplicates[i];
                if (!duplicateMap.has(pos)) {
                    duplicateMap.set(pos, {})
                }
                const ob = duplicateMap.get(pos)
                if (Object.keys(ob).length && el.QuestionID === questionIds[0]) {
                    pos++;
                    duplicateMap.set(pos, {})
                }

                duplicateMap.get(pos)[el.QuestionID] = el
            }

            const duplicateRows = [...duplicateMap.values()]
            duplicateRows.forEach((row) => {
                let SyncTaskID = Object.values(row)[0].SyncTaskID
                row["SyncDetails"] = MyTaskSummary.find((x) => x.SyncTaskID === SyncTaskID)
                if (this.isBeneficiary.linked) {
                    let BeneficiaryID = Object.values(row)[0].ResponseID
                    let BeneficiaryQA = this.BenefQAMap.get(BeneficiaryID)
                    if (BeneficiaryQA && BeneficiaryQA.length) {
                        BeneficiaryQA.forEach((question) => {
                            row[question.QuestionID] = {
                                ...question,
                                ID: this.BenefRegMap.get(question.BeneficiaryId)?.find(x => x.QuestionID === question.QuestionID)?.BeneficiaryRegID,
                                isBeneficiaryQuestion: true,
                            }
                        })
                    }
                }
            })

            return duplicateRows
        },
        questionMapper(questionList) {
            this.questionMap = new Map()
            questionList.forEach(question => {
                if(!this.questionMap.has(question.QuestionID)) {
                    const QuestionType = this.surveyQuestionType.get(question.QuestionTypeID);
                    const Section = this.sectionLogicMap.get(question.SectionLogicID); 
                    this.questionMap.set(question.QuestionID, {
                        ...question,
                        Section,
                        QuestionType
                    })
                }
            })
        },
        getBenefQstnAns(TypeID, BenefTypeQuestions) {
            const BenefDetailsAns = []

            const currentBenefQstns = BenefTypeQuestions.filter(x => {
                return x.BeneficiaryTypeID === TypeID
            })
            for (let [key, value] of this.BenefDetails.entries()) {
                let lstDetails = this.BenefRegMap.get(value.BeneficiaryID)
                if (lstDetails) {
                    for (let k of lstDetails) {
                        let q = currentBenefQstns.find((x) => x.QuestionID === k.QuestionID);
                        let ob = {
                            QuestionName: "",
                            Answer: "",
                            BeneficiaryID: 0,
                            QuestionID: k.QuestionID,
                        };
                        if (q && q.QuestionName) {
                            ob.QuestionName = q ? q.QuestionName : "";
                            ob.Answer = k.Answer;
                            ob.BeneficiaryID = k.BeneficiaryID;
                            BenefDetailsAns.push(ob);
                        }
                    }
                }
            }
            return BenefDetailsAns;
        },
        addBeneficiaryColsAns(entry, responseObj, colDefs) {
            let { BeneficiaryId } = entry[0]
            if(!BeneficiaryId) {
               BeneficiaryId = entry[0].ResponseID 
            }
            const BeneficiaryQA = this.BenefQAMap.get(BeneficiaryId)
            if(BeneficiaryQA) {
                BeneficiaryQA.forEach((question) => {
                    responseObj[question.QuestionID] = {
                        ...question,
                        ID: this.BenefRegMap.get(question.BeneficiaryID)?.find(x => x.QuestionID == question.QuestionID)?.BeneficiaryRegID,
                        isBeneficiaryQuestion: true,
                    }
                    let isExists = colDefs.some(x => x.meta.isBeneficiaryQuestion && x.headerName.includes(question.QuestionName))
                    if(!isExists) {
                        colDefs.push({
                            colId: `Benef_${question.QuestionName}`,
                            headerName: question.QuestionName,
                            valueGetter: (params) => {
                                return params.data[question.QuestionID]?.Answer
                            },
                            valueSetter: (params) => {
                                params.data[question.QuestionID].Answer = params.newValue
                                return true
                            },
                            meta: {
                                question,
                                isBeneficiaryQuestion: true
                            },
                            cellClass: (params) => {
                                let answer = params.data[question.QuestionID]?.Answer
                                const arr = [this.isHyperLink(answer)]
                                if (!this.disableEdit) {
                                    let editable = params.column.isCellEditable(params.node)
                                    arr.push(this.isEditableCell(editable))
                                }
                                return arr
                            },
                            editable: () => {
                                return !this.disableEdit && this.beneficiaryEdit
                            },
                            sortable: true,
                            resizable: true,
                            filter: true,
                        })
                    }
                })
            }
            return {responseObj, colDefs}
        },
        isHyperLink(answer) {
            if (!answer) return '';
            let coordinateRegex = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/g
            let allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar']
            if (answer && allowedExtensions.includes(answer.split('.').pop())) {
                return 'hyperLink'
            } else if (answer && coordinateRegex.test(answer)) {
                return 'hyperLink'
            }
            else return ''
        },
        isEditableCell(editable) {
            if (editable) {
                return 'enableCell'
            } else {
                return 'disableCell'
            }
        },
        autoSizeAll() {
            const allColumnIds = [];
            this.gridColumnApi.getColumns().forEach((column) => {
                allColumnIds.push(column.getId());
            });
            this.gridColumnApi.autoSizeColumns(allColumnIds, false);
        },
        switchDataSets(id) {
            if(!this.disableEdit) {
                this.snackbar = {
                    value: true,
                    message: "Please confirm the changes",
                    color: "warning",
                };
                return
            }
            const currentSheet = this.sheetsList.find(x => x.id === id)
            if(!currentSheet) {
                this.snackbar = {
                    value: true,
                    message: "Data for sheet not found",
                    color: "error",
                };
                return
            } else {
                this.gridApi.setColumnDefs(currentSheet.colDefs)
                this.gridApi.setRowData(currentSheet.responses)
                this.currentSheetID = id
                if(currentSheet.isSurveyResponses) {
                    this.gridColumnApi.applyColumnState({
                        defaultState: {
                            sort: null
                        }
                    })
                } else {
                    this.$nextTick(() => {
                        this.gridColumnApi.applyColumnState({
                            state: [{
                                colId: 'Benef_Name',
                                sort: 'asc'
                            }]
                        })
                    })
                }

            }
            this.autoSizeAll()
            this.$emit('gridLoaded', this.$refs.editorMain.offsetHeight)
        },
        exportController() {
            if (!this.disableEdit) {
                this.snackbar = {
                    value: true,
                    message: "Cant export while editing, Please save the changes",
                    color: 'error'
                }
                return
            }
            this.isLoading = true
            const sheets = this.sheetsList.filter(x => x.ID !== 0 && x.responses)
            const workbook = XLSX.utils.book_new();
            let responsesID = 0
            let count = 1
            sheets.forEach((x) => {
                let name = x.name
                if (x.name.length > 30) {
                    name = `(${count++}) ${x.name.split(' - ')[0]}`
                }
                const worksheet = XLSX.utils.json_to_sheet([]);
                if (x.isSurveyResponses) responsesID = x.id
                this.switchDataSets(x.id)
                let csv = this.gridApi.getDataAsCsv({
                    suppressQuotes: true,
                    columnSeparator: `~`,
                })
                let data = csv.trim().split("\n").map(row => row.split("~"))
                XLSX.utils.sheet_add_aoa(worksheet, data);
                XLSX.utils.book_append_sheet(workbook, worksheet, name);
            })
            XLSX.writeFile(workbook, `${this.$route.params.SurveyName}.xlsx`);
            this.isLoading = false;
            this.switchDataSets(responsesID)
        },
        globalExportWorker() {
            if (!this.disableEdit) {
                this.snackbar = {
                    value: true,
                    message: "Cant export while editing, Please save the changes",
                    color: 'error'
                }
                return
            }
            this.isLoading = true
            const sheets = this.sheetsList.filter(x => x.ID !== 0 && x.responses)
            const workbook = XLSX.utils.book_new();
            let responsesID = 0
            let count = 1
            sheets.forEach((x) => {
                let name = x.name
                if (x.name.length > 30) {
                    name = `(${count++}) ${x.name.split(' - ')[0]}`
                }
                const worksheet = XLSX.utils.json_to_sheet([]);
                if (x.isSurveyResponses) responsesID = x.id
                this.switchDataSets(x.id)
                let csv = this.gridApi.getDataAsCsv({
                    suppressQuotes: true,
                    columnSeparator: `~`,
                })
                let data = csv.trim().split("\n").map(row => row.split("~"))
                XLSX.utils.sheet_add_aoa(worksheet, data);
                XLSX.utils.book_append_sheet(workbook, worksheet, name);
            })
            this.isLoading = false;
            this.switchDataSets(responsesID)
            return workbook;
        },
        editSurvey(mode) {
            switch (mode) {
                case "init":
                    this.$nextTick(() => {
                        this.gridApi.refreshCells({
                            force: true,
                            suppressFlash: true
                        });
                    })
                    this.disableEdit = false;
                    this.undoRedo = true;
                    break;
                case "save":
                    this.editResponsesBulk();
                    break;
                case "cancel":
                    this.undoBtn = false;
                    this.redoBtn = false;
                    this.undoRedo = false;
                    this.beneficiaryEdit = false;
                    this.gridApi.stopEditing();
                    this.disableEdit = true;
                    const len = this.gridApi.getCurrentUndoSize();
                    for (let i = len; i >= 0; i--) {
                        this.undo();
                    }
                    this.$nextTick(() => {
                        this.gridApi.refreshCells({
                            force: true,
                            suppressFlash: true
                        });
                    })
                    break;
                default:
                    break;
            }
        },
        undo() {
            this.gridApi.undoCellEditing();
            this.updationData.pop();
        },
        redo() {
            this.gridApi.redoCellEditing();
        },
        cellClickWatchDog(params) {
            if (params.value) {
                let isHyperLink = params.event.target.classList.contains("hyperLink");
                if (isHyperLink) {
                    let coordinateRegex = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/g
                    if (coordinateRegex.test(params.value)) {
                        window.open(
                            "https://www.google.com.sa/maps/search/" + params.value + "?hl=en"
                        );
                    }
                    else if (params.colDef.meta.isBeneficiaryQuestion) {
                        window.open(`${this.BeneffileUrl}/${this.ProjectId}/${params.value}`);
                    } else {
                        window.open(`${this.fileUrl}/${this.ProjectId}/${params.value}`);
                    }
                }
            }
        },
        async editResponsesBulk() {
            this.gridApi.stopEditing();
            if (this.isLoading) return;
            setTimeout(async () => {
                if (this.updationData?.length) {
                    this.isLoading = true
                    await this.TaskActions
                        .updateBulkData(this.updationData)
                        .then(() => {
                            this.disableEdit = true;
                            this.gridApi.redrawRows();
                            this.snackbar = {
                                value: true,
                                message: "Data Saved",
                                color: "success",
                            };
                            this.sheetsList.forEach((sheet) => {
                                if ("responses" in sheet) {
                                    this.updationData.forEach(({ Answer, ID, QuestionID }) => {
                                        sheet.responses.forEach((response, rIndex) => {
                                            if (response[QuestionID]?.ID === ID) {
                                                response[QuestionID].Answer = Answer
                                            }
                                        })
                                    })
                                }
                            })
                        })
                        .catch((e) => {
                            console.error(e);
                            this.snackbar = {
                                value: true,
                                message: "An Error Occured",
                                color: "error",
                            };
                        })
                        .finally(() => {
                            this.undoBtn = false;
                            this.redoBtn = false;
                            this.undoRedo = false;
                            this.gridApi.refreshClientSideRowModel("map");
                            this.isLoading = false;
                            this.updationData = []
                            this.beneficiaryEdit = false
                        });
                } else {
                    this.snackbar = {
                        value: true,
                        message: "No Data to Save",
                        color: "error",
                    };
                }
            }, 1000);
        },
        clearAllFilters() {
            this.gridApi.setFilterModel(null);
        },
        onFilterChanged(params) {
            this.$emit('gridChanged', this.$refs.editorMain.offsetHeight)
            // console.log()
            let activeFilters = this.gridApi.getFilterModel();
            this.showClearFilter = typeof activeFilters === "object" && Object.keys(activeFilters).length;
        },
        async deleteRowController() {
            if (this.isLoading) return;
            if (this.selectedRows.length) {
                try {
                    this.isLoading = true
                    let syncTaskIDs = [];
                    let BID = 0;
                    this.selectedRows.forEach((row) => {
                        if (this.isBeneficiary.linked) {
                            BID = [...new Set(Object.values(row).map(x => x.BeneficiaryID || x.ResponseID))]
                            if (BID.length) {
                                BID = BID[0]
                            } else {
                                BID = 0
                            }
                        }
                        syncTaskIDs.push({
                            SyncTaskID: row.SyncDetails.SyncTaskID,
                            ResponseID: BID
                        })
                    })

                    if (syncTaskIDs.length) {
                        const result = await this.TaskActions.deleteSurveyResponses(syncTaskIDs);

                        if (result) {
                            this.gridApi.applyTransaction({ remove: this.gridApi.getSelectedRows() });
                            this.sheetsList.forEach((sheet) => {
                                const { responses } = sheet
                                responses.forEach((response, rIndex) => {
                                    if (syncTaskIDs.includes(response.SyncDetails.SyncTaskID)) {
                                        responses.splice(rIndex, 1)
                                    }
                                })
                            })
                            this.snackbar = {
                                value: true,
                                message: "Response deleted successfully",
                                color: "success",
                            }
                        } else {
                            throw new Error("Error updating Data");
                        }
                    }
                } catch (error) {
                    console.error(error);
                    this.snackbar = {
                        value: true,
                        message: error.message,
                        color: "error",
                    }
                } finally {
                    this.isLoading = false
                }
            } else {
                this.snackbar = {
                    message: "No row selected",
                    color: "warning",
                    value: true
                }
            }
        },
        async getAllSurveyResponses() {
            try {
                this.isLoading = true
                const result = await this.TaskActions.getSurveyResponsesByID({
                    SurveyID: this.SurveyId
                })
                const { IndResponses: responseStack, lstDetails } = result
                if(!responseStack.length) {
                    throw new Error("No Responses Found")
                }
                const { MyTaskResponseDetails: ResponseDetails, MyTaskSummary } = lstDetails
                const IndResponses = responseStack.reduce((acc, curr) => {
                    acc.push(...curr.QuestionResponses)
                    return acc
                }, [])
                
                const _ResponseDetails = []
                ResponseDetails.forEach((response) => {
                    const uniqueBIDs = [...new Set(response.SurveyQuestions.map(x => x.BeneficiaryId))];
                    if(uniqueBIDs.length > 1) {
                        uniqueBIDs.forEach((BID) => {
                            _ResponseDetails.push({
                                SurveyQuestions: response.SurveyQuestions.filter(x => x.BeneficiaryId === BID),
                                SyncDetails: response.SyncTaskID
                            })
                        })
                    } else {
                        _ResponseDetails.push({
                            SurveyQuestions: response.SurveyQuestions,
                            SyncDetails: response.SyncTaskID
                        })
                    }
                })
                
                this.responseExtras = lstDetails
                const response = {
                    IndResponses,
                    lstDetails: {
                        MyTaskResponseDetails: _ResponseDetails.map(x => x.SurveyQuestions),
                        MyTaskSummary
                    }
                }
                console.log(response)
                return response
            } catch (error) {
                console.error(error)
                this.snackbar = {
                    value: true,
                    message: error.message,
                    color: "error",
                }
            } finally {
                this.isLoading = false
            }
        }
    },
    watch: {
        _beneficiaryEdit(val) {
            this.gridApi.refreshCells({
                force: true,
                suppressFlash: true,
            })
        },
        currentLogic(val) {
            if(val) {
                const sheet = this.sheetsList.find(x => x.name === val)
                this.switchDataSets(sheet.id)
            }
        },
        filterBox(val) {
            this.gridApi.setQuickFilter(val)
        },
        editToggle(val){
            this.disableEdit = val;
            this.$nextTick(() => {
                this.gridApi.refreshCells({
                    force: true,
                    suppressFlash: true
                });
            })
        }
    },
    destroyed() {
        // remove from gridMap if destroyed
        // this.updateGrids({uid: this.SyncDBID}) 
    },
}
</script>

<style lang="scss" scoped>
@import "~ag-grid-community/styles/ag-grid.css";
@import "~ag-grid-community/styles/ag-theme-alpine.css";
@import "~ag-grid-community/styles/ag-theme-balham.css";
@import "~ag-grid-community/styles/ag-theme-material.css";

.actionsContainer {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  background-color: white !important;
  margin: 0 -1rem;
  padding: 0.25rem 4px;
}
.responsesContainer,
.editorContainer {
  background-color: white;
  margin: 0 -1rem;
  padding: 1rem 3px;
  padding-top: 0;
}

$editorWidth: width('.editorContainer');

.editorFooter{

    display: flex;
    flex-wrap: nowrap;
    flex-direction: row;
    justify-content: space-between;
    align-items: flex-start;
    .contextContainer {
      display: flex;
      justify-content: flex-start;
      align-items: center;
      background-color: white;
      margin: 0 -1rem;
      padding: 0.75rem 1rem;
      width: $editorWidth;
      overflow-x: scroll;
      width: 80%;
    }

    .paginationControl{
        width: 20%;
        display: flex;
        flex-direction: row;
        justify-content: flex-end;
        align-items: center;
        .pageNoLabel{
            color: #18426B;
            font-family: 'Poppins';
            font-style: normal;
            font-weight: 400;
            font-size: 12px;
            line-height: 16px;
        }
        
        .page{
            font-family: 'Poppins';
            font-style: normal;
            font-weight: 400;
            font-size: 12px;
            line-height: 16px;
            display: grid;
            place-content: center;
            padding: 8px;
            background: #EBF5FF;
            border: 1px solid #EBF5FF;
            border-radius: 8px;
            width: 36px;
            height: 36px;
        }
    }
}
._btn_rounded {
  border-radius: 8px;
}

.gridMainContainer {
    position: relative;

    .spinnerWrapper {
        position: absolute;
        top: 0px;
        bottom: 0px;
        left: -20px;
        right: -20px;
        width: inherit;
        height: 100%;
        z-index: 999;
        display: grid;
        place-items: center;
        background: rgba(133, 133, 133, 0.1)
    }
}


.ag-theme-alpine {
  --ag-borders: none;
  --ag-header-background-color: #ebecee;
  --ag-checkbox-border-radius: 5px;
  --ag-checkbox-checked-color: rgba(57, 156, 255, 1);
  --ag-checkbox-checked-border-color: rgba(255, 255, 255, 1);

    .ag-cell {
        border-right: 0.25px solid lightgrey !important;
    }
    
}
.ag-center-cols-clipper{
    min-height: 10px !important;
}

</style>
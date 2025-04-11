<template>
  <v-container fluid class="mx-3" ref="mainContainer0">
    <pre-loader v-show="isLoading" />
    <v-card class="cardWrapper" max-width="98vw">
      <v-card-title class="d-flex flex-nowrap subtitle-2">
        <v-btn icon color="primary2" @click="$router.go(-1)"
          ><v-icon>mdi-keyboard-backspace</v-icon></v-btn
        >
        <span>My Task</span>
        <v-icon color="rgba(122, 189, 255, 1)">mdi-chevron-right</v-icon>
        {{ $route.params.SurveyName }}
      </v-card-title>
      <v-card-text class="cardBody">
        <v-tabs v-model="mainTab" dense>
          <v-tab href="#dataSummary">Data Summary</v-tab>
          <v-tab href="#activityTab">Activity</v-tab>
        </v-tabs>

        <v-tabs-items v-model="mainTab">
          <v-tab-item value="dataSummary">
            <v-expand-transition hide-on-leave="true">
              <div
                class="summaryContainer"
                v-if="selectedResponses && selectedResponses.length"
              >
                <div class="cell">
                  <div class="pill" />
                  <div class="content d-flex flex-column">
                    <p class="_title mb-0">{{ headerValues.submissionsCount }}</p>
                    <p class="_subtitle mb-0">Total submissions</p>
                  </div>
                </div>
                <div class="cell">
                  <div class="pill" />
                  <div class="content d-flex flex-column">
                    <p class="_title mb-0">{{ headerValues.beneficiariesCount }}</p>
                    <p class="_subtitle mb-0">No. of Beneficiaries</p>
                  </div>
                </div>
                <div class="cell">
                  <div class="pill" />
                  <div class="content d-flex flex-column">
                    <p class="_title mb-0">{{ headerValues.questionsCount }}</p>
                    <p class="_subtitle mb-0">No. of questions/ submission</p>
                  </div>
                </div>
                <div class="cell">
                  <div class="pill" />
                  <div class="content d-flex flex-column">
                    <p class="_title mb-0">{{ headerValues.completionRatio }}%</p>
                    <p class="_subtitle mb-0">Complete ratio</p>
                  </div>
                </div>
              </div>
            </v-expand-transition>

            <div class="actionsContainer">
              <v-checkbox v-if="!editToggle" v-model="_beneficiaryEdit" label="Enable Editing Beneficiaries" :hide-details="true" class="my-auto ml-2"></v-checkbox>
              <v-spacer/>
              <!-- <v-btn icon :loading="isLoading" @click="redraw"><v-icon>mdi-refresh</v-icon></v-btn> -->
              <v-text-field v-model="quickFilterBox" append-icon="mdi-magnify" label="Search" :hide-details="true" solo dense clearable class="mx-2" style="max-width: 300px;" flat></v-text-field>
              <template v-if="false">
                <v-btn v-show="undoRedo" icon @click="undo()" :disabled="!undoBtn"><v-icon>mdi-undo</v-icon></v-btn>
                <v-btn v-show="undoRedo" icon @click="redo()" :disabled="!redoBtn"><v-icon>mdi-redo</v-icon></v-btn>
              </template>
              <template v-if="!editToggle">
                <v-btn color="primary" class="mr-2"  text @click="gridsAction('cancel')">Cancel</v-btn>
                <v-btn color="primary2" class="mr-2 _btn_rounded"  dark depressed @click="gridsAction('save')">Save Changes</v-btn>
              </template>
              <template v-else>
                <v-btn color="primary2" class="mr-2 _btn_rounded" dark text depressed @click="gridsAction('edit')">Edit</v-btn>
                <v-btn class="mx-1" depressed dense color="primary2" dark @click="globalExportController"><v-icon class="mr-2">mdi-export-variant</v-icon>Export</v-btn>
                <v-btn
                  color="primary2"
                  text
                  class="mr-2"
                  @click="(gridApi.stopEditing()), approveRejectTaskBulk('Reject')"
                  depressed
                  :disabled="!selectedResponses || !selectedResponses.length"
                >
                  Reject
                </v-btn>
                <v-btn
                  color="primary2"
                  class="mr-2 _btn_rounded"
                  @click="(gridApi.stopEditing()), approveRejectTaskBulk('Approve')"
                  depressed
                  dark
                  :disabled="!selectedResponses || !selectedResponses.length"
                >
                  Approve
                </v-btn>
              </template>
            </div>

            <!-- excel Editor section -->
            <div class="editorContainer">
              <ag-grid-vue
                style="width: 100%; height: auto"
                class="ag-theme-alpine"
                rowSelection="multiple"
                :columnDefs="defaultCols"
                :rowData="responseList"
                :getRowId="getRowId"
                :suppressRowClickSelection="true"
                :overlayLoadingTemplate="loadingOverlay"
                :pagination="paginationToggle"
                :paginationPageSize="10"
                :undoRedoCellEditing="true"
                :undoRedoCellEditingLimit="50"
                :enableCellChangeFlash="true"
                :animateRows="true"
                :suppressPaginationPanel="true"
                :getRowHeight="getRowHeight"
                :isFullWidthRow="isFullWidthRow"
                :fullWidthCellRenderer="FullWidthRowRenderer"
                :fullWidthCellRendererParams="fullWidthCellRendererParams"
                domLayout="autoHeight"
                @gridReady="onGridReady"
                @selection-changed="onSelectionChanged"
                @firstDataRendered="onFirstDataRendered"
                @filterChanged="onFilterChanged"
              >
              </ag-grid-vue>
              <div id="CrossGridScroller" @scroll="crossGridScroller">
                <!-- Cross Grid ScrollBar -->
                <div id="fakeDiv" :style="{ width: (maxWidth + 200) + 'px' }" ref="crossGridScrollBar"></div>
              </div>
            </div>
          </v-tab-item>
          <v-tab-item value="activityTab">
            <bulk-task-activity :syncTaskIDList="syncTaskIDList" class="activityContainer" />
          </v-tab-item>
        </v-tabs-items>
      </v-card-text>
      <v-card-actions v-if="gridApi && (mainTab === 'dataSummary')" class="editorFooter">
        <v-spacer />
        <div class="contextContainer v-scrolling-div">
            <!-- tabActions -->
            <v-btn v-for="sheet in logicList" color="#EBF5FF" depressed class="mr-2 _btn_rounded" @click="switchSheetGlobal(sheet)">{{ sheet }}<v-icon>{{ sheet === currentLogic ? "mdi-chevron-up" : "" }}</v-icon></v-btn>
        </div>

        <div class="paginationControl" v-if="gridApi">
            <div class="d-inline-flex align-center"><p class="mr-2 mb-0 pageNoLabel">Page No: </p><div class="page">{{ gridApi.paginationGetCurrentPage() + 1 }}</div></div>
            <div v-if="gridApi.paginationGetTotalPages() > 1" class="d-inline-flex align-center mx-1"><p class="mr-2 mb-0 pageNoLabel">of</p><div class="page">{{ gridApi.paginationGetTotalPages() }}</div></div>
            <v-btn :disabled="gridApi.paginationGetCurrentPage() === 0" :ripple="false" depressed color="primary2" icon @click="gridApi.paginationGoToPreviousPage(), pageSwitchHelper($event)"><v-icon>mdi-chevron-left</v-icon></v-btn>
            <v-btn :disabled="gridApi.paginationGetTotalPages() === gridApi.paginationGetCurrentPage() + 1" :ripple="false" depressed color="primary2" icon @click="gridApi.paginationGoToNextPage($event), pageSwitchHelper($event)"><v-icon>mdi-chevron-right</v-icon></v-btn>
        </div>
      </v-card-actions>
    </v-card>


    <!-- snackbar -->
    <v-snackbar top v-model="snackbar.value" :color="snackbar.color">
      {{ snackbar.message }}
    </v-snackbar>
    
    <!-- Beneficiar Edit Confirmation Dialog -->
      <v-dialog max-width="500">
        <v-card>
          <v-card-title>Edit Beneficiary Details ?</v-card-title>
          <v-card-text>
            <p>Editing Beneficiary Details will affect all the Surveys linked to the selected Beneficiary</p>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary2" text> Cancel </v-btn>
            <v-btn
              color="primary2"
              class="_btn_rounded"
              dark
              depressed
            >
              Confirm
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    <!-- Dialog END -->

    <!-- Approve / Reject Dialog -->
    <v-dialog v-model="surveyAuthDlg" max-width="600">
      <v-card>
        <v-card-title class="headline"> {{ surveyAuthType }} Selected </v-card-title>
        <v-card-text>
          <p>
            Are you sure you want to bulk {{ surveyAuthType }} all these survey at once?
          </p>
          <br />
          <v-textarea
            v-model="authMessage"
            outlined
            hide-details="auto"
            placeholder="Add a comment if required"
          >
          </v-textarea>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary2" text @click="surveyAuthDlg = false"> Cancel </v-btn>
          <v-btn
            color="primary2"
            class="_btn_rounded"
            dark
            depressed
            @click="approveRejectTaskBulk(surveyAuthType)"
          >
            {{ surveyAuthType }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <!-- Dialog END -->
  </v-container>
</template>

<script>
import { AgGridVue } from "ag-grid-vue";
import { dbImworks } from "../../ImworksDb";
import TaskBulkActions from "@/BL/TaskBulkActions.js";
import BulkTaskActivity from "./BulkTaskActivity.vue";
import { v4 as uuidv4 } from "uuid";
import PreLoader from "@/components/pre-loader.vue";
import Dexie from "dexie";
import syncData from "../../ImworksMobApp";
import ResponsesGridView from "./MyTaskComponents/ResponsesGridView.vue";
import FullWidthRowRenderer from './MyTaskComponents/FullWidthRowRenderer.vue'
import { createNamespacedHelpers } from "vuex";
const { mapGetters: gGetters, mapActions: gActions, mapMutations: gMutations } = createNamespacedHelpers("GridContext");
export default {
  name: "BulkTaskMan",
  components: {
    PreLoader,
    AgGridVue,
    BulkTaskActivity,
    ResponsesGridView,
    FullWidthRowRenderer
  },
  props: {
    TaskCardData: {
      type: Object,
      required: true,
    },
  },
  async created() {
    this.currentUser = (await dbImworks.User.where({ IsLoggedIn: 1 }).toArray())[0];
    this.loadingOverlay = `<span class="ag-overlay-loading-center">Please wait while survey answers are loaded</span>`;
    this.getRowHeight = (params) => {
      return 400;
    }
    await this.getPendingSurveys();
    this.FullWidthRowRenderer = `FullWidthRowRenderer`;
    this.isFullWidthRow = (params) => {
      return true;
    }
  },
  async mounted() {
    if (!this.$route.query.SurveyID || !this.TaskCardData) this.$router.go(-1);
    this.syncTaskIDList = this.TaskCardData.TaskLog.map(x => {
      return x.SyncDBID
    })
  },
  data() {
    return {
      isLoading: false,
      bulkTaskController: new TaskBulkActions(),
      mainTab: null,
      defaultCols: [
        {
          headerName: "",
          field: "check",
          width: 50,
          checkboxSelection: true,
          headerCheckboxSelection: true,
        },
        {
          headerName: "Submitted By",
          field: "TaskdataAddedBy",
          flex: 1,
          sortable: true,
          filter: true,
        },
        {
          headerName: "Survey Submissions",
          field: "WorkflowStage",
          sortable: true,
          filter: true,
        },
        {
          headerName: "Submission Date",
          valueGetter: (params) =>
            new Date(params.data.TaskSubmittedDate).toLocaleDateString(),
          sortable: true,
          filter: true,
        },
        {
          headerName: "Status",
          field: "ActionRequired",
          sortable: true,
          filter: true,
        },
        {
          headerName: "Comment",
          valueGetter: (params) => {
            return params.data["TaskComment"];
          },
          valueSetter: (params) => {
            params.data["TaskComment"] = params.newValue;
            return true;
          },
          width: 300,
          sortable: true,
          filter: true,
          editable: (params) => {
            if (params.node.selected) {
              return true;
            }
          },
          cellClass: (params) => {
            if (params.node.selected) {
              return "comment";
            }
          },
        },
      ],
      allResponses: [],
      gridApi: null,
      gridColumnApi: null,
      surveyAuthDlg: false,
      surveyAuthType: "Approve",
      authMessage: "",
      selectedResponses: [],
      allAnswers: [],
      responseCols: [],
      loadingOverlay: null,
      currentUser: [],
      snackbar: {
        message: "",
        value: false,
        color: "primary2",
      },
      gridOverlay: null,
      showClearFilter: false,
      syncTaskIDList: [],
      fileUrl: window.SERVER_URL + "/Upload/SureyQuestionnaire",
      BeneffileUrl: window.SERVER_URL + "/Upload/BeneficiaryRegistration",
      ProjectId: this.$route.query.ProjectId,
      SurveyID: this.TaskCardData.TypeID,
      mappedResponses: new Map(),
      responseExtras: {},
      getRowHeight: null,
      isFullWidthRow: null,
      FullWidthRowRenderer: null,
      fullWidthCellRendererParams: null,
      getRowId: null,
      paginationToggle: true,
    };
  },
  computed: {
    ...gGetters([
      "logicList",
      "currentLogic",
      "filterBox",
      "editToggle",
      "beneficiaryEdit",
      "globalUpdationData",
      "maxWidth",
      "gridMap",
    ]),
    headerValues() {
      let retOb = {
        submissionsCount: 0,
        beneficiariesCount: 0,
        questionsCount: 0,
        completionRatio: 0,
      };
          
      let CompletionPerc = 0;
      for (let i = 0; i < this.selectedResponses.length; i++) {
              
        let Beneficiaries = [];
        retOb.submissionsCount += 1;
        let curr = this.TaskCardData.TaskLog.find(
          (x) => x.SyncDBID === this.selectedResponses[i].SyncDBID
        );
        if (this.isBeneficiary.linked) {
          if (!Beneficiaries.includes(curr.BeneficiaryTypeID)) {
            Beneficiaries.push(curr.BeneficiaryTypeID);
            retOb.beneficiariesCount += curr.BeneficiaryCount;
          }
        }
        if (curr.AnswerCount >= 0) {
          retOb.questionsCount += curr.AnswerCount;
        }
        if (curr.QuestionCount > 0 && curr.AnswerCount > 0) {
          CompletionPerc += (curr.AnswerCount / curr.QuestionCount) * 100;
          retOb.completionRatio = Math.round(CompletionPerc / this.selectedResponses.length);
        }
        else {
          retOb.completionRatio = 0;
        }
      }
      return retOb;
    },
    responseList: {
      get() {
        return this.allResponses
          .filter((x) => x.TypeID === parseInt(this.$route.query.SurveyID))
          .map((y) => {
            return {
              ...y,
              TaskAddedDate: new Date(y.TaskAddedDate).toLocaleDateString(),
            };
          });
      },
    },
    isBeneficiary: {
      get() {
        let data = {
          linked: false,
          BID: 0,
        };
        let BenefTypeID;
        if (this.TaskCardData && this.TaskCardData.BeneficiaryTypeID) {
          BenefTypeID = this.TaskCardData.BeneficiaryTypeID;
        }
        if (BenefTypeID && BenefTypeID > 0) {
          data.linked = true;
          data.BID = BenefTypeID;
        }

        return data;
      },
    },
    quickFilterBox: {
      get() {
        return this.filterBox;
      },
      set(value) {
        this.updateFilterBox(value);
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
  },
  methods: {
    ...gActions([
      "updateCurrentLogic",
      "updateFilterBox",
      "updateUpdationData",
      "saveGridData",
      "clearAllGrids"
    ]),
    ...gMutations([
      "resetLogicList",
      "setEditToggle",
      "setBeneficiaryEditToggle",
      "setMappedResponses",
      "setResponseContext"
    ]),
    onFirstDataRendered(params) {
      this.$nextTick(() => {
        this.gridApi.redrawRows();
      })
      return;
    },
    onGridReady(params) {
      this.gridApi = params.api;
      this.gridColumnApi = params.columnApi;
      this.getRowId = (params) => params.data.SyncDBID
      console.log("l;f");
    },
    autoSizeAll() {
      const allColumnIds = [];
      this.gridColumnApi.getColumns().forEach((column) => {
        allColumnIds.push(column.getId());
      });
      this.gridColumnApi.autoSizeColumns(allColumnIds, false);
    },
    onSelectionChanged(event) {
      this.selectedResponses = event.api.getSelectedRows();
    },
    async getPendingSurveys() {
      try {
        this.isLoading = true
        const mappedResponses = new Map();
        this.setResponseContext({})
        let User = (await dbImworks.User.toArray())[0];
        let { UserId, WorkspaceID, UserName } = User;
        /* 

          ### can use SyncServerID later

          // let lstFetched = await dbImworks.TaskSettings.where("UserId")
          //   .equals(parseInt(UserId))
          //   .toArray();
          // let SyncServerID = lstFetched[lstFetched.length - 1].ServerPKID; 
        
        */
        const p1 = this.bulkTaskController.getSurveyResponsesByID({
          SurveyID: this.SurveyID
        })
        const p2 = this.bulkTaskController.getPendingTasks({
          UserID: UserId,
          TypeID: parseInt(this.$route.query.SurveyID),
          WorkspaceID,
        });
        const [result, pendingTasks] = await Promise.all([p1, p2]);
        this.allResponses = pendingTasks
        // for(let i = 0; i < 5000; i++) {
        //   this.allResponses.push(...pendingTasks)
        // }

        // we make a map out of surveyResponses to be accessible by SyncTaskID
        const { IndResponses, lstDetails } = result
        this.setResponseContext(lstDetails)
        const responseStack = IndResponses.reduce((acc, curr) => {
          acc.push(...curr.QuestionResponses)
          return acc
        }, [])
        responseStack.forEach((res) => {
          if(!mappedResponses.has(res.SyncTaskID)){
            mappedResponses.set(res.SyncTaskID, {
              IndResponses: [],
              lstDetails: {
                MyTaskSummary: [],
                MyTaskResponseDetails: [],
              },
            })
          }

          mappedResponses.get(res.SyncTaskID).IndResponses.push(res)
        })

        lstDetails.MyTaskSummary.forEach((summary) => {
          if(mappedResponses.has(summary.SyncTaskID)){
            mappedResponses.get(summary.SyncTaskID).lstDetails.MyTaskSummary.push(summary)            
          }
        })
        lstDetails.MyTaskResponseDetails.forEach((response) => {
          if(mappedResponses.has(response.SyncTaskID)){
            const uniqueBIDs = [...new Set(response.SurveyQuestions.map(x => x.BeneficiaryId))];
            if(uniqueBIDs.length > 1) {
              uniqueBIDs.forEach((ubid) => {
                mappedResponses.get(response.SyncTaskID).lstDetails.MyTaskResponseDetails.push(response.SurveyQuestions.filter(x => x.BeneficiaryId === ubid))
              })
            }
            else {
              mappedResponses.get(response.SyncTaskID).lstDetails.MyTaskResponseDetails.push(response.SurveyQuestions)
            }
          }
        })

        this.setMappedResponses(mappedResponses)
      } catch (error) {
        console.error(error)
      } finally {
        this.isLoading = false
      }
    },
    async approveRejectTaskBulk(mode) {
      if(this.isLoading) return
      this.isLoading = true;
      const taskSyncData = [];
      const tbMyTaskData = [];
      const today = new Date().toLocaleString("en-US");
      let User = (await dbImworks.User.toArray())[0];
      let { UserName } = User
      // selected Task = each node in grid
      try {
        for (let i = 0; i < this.selectedResponses.length; i++) {
          let current = this.selectedResponses[i];
          const taskResponse = [
            {
              SyncDBID: current.SyncDBID,
              Type: current.Type,
              TypeID: current.TypeID,
              TypeIDColumn: "",
              Status: "",
              WorkflowID: current.WorkflowID,
              TotalWFLevel: current.TotalWFLevel,
              WFLevel: current.WFLevel,
              WFLevelStatus: "",
              TaskUpdateComment: current.TaskComment ?? "",
              AttachedDocumentName: "",
              SurveyAnswers: "",
              ResponseSyncID: current.ResponseSyncID,
              DataCollectedBy: current.DataCollectedBy,
              TaskDate: current.TaskDate,
              TaskSubmittedDate: current.TaskSubmittedDate,
              WorkspaceID: current.WorkspaceID,
            },
          ];
          switch(mode) {
            case 'Approve':
              taskResponse[0].Status = "DataApproved"
              taskResponse[0].WFLevelStatus = "Completed"
              break;
            case 'Reject':
              taskResponse[0].Status = "Rejected"
              taskResponse[0].WFLevelStatus = "Rejected"
              break;
          }
          let TaskStatus = current.TotalWFLevel === current.WFLevel ? "Completed" : "DataApproved";
          
          taskSyncData.push({
            Type: current.Type,
            Command: "TaskResponse",
            TypeID: current.TypeID,
            TypeIDColumn: "",
            Status: TaskStatus,
            WorkflowID: current.WorkflowID,
            TotalWFLevel: current.TotalWFLevel,
            WFLevel: current.WFLevel,
            ClientData: JSON.stringify(taskResponse),
            Periodicity: this.TaskCardData.Periodicity,
            lastSyncDBID: current.SyncDBID,
            IsBeneficiaryLinked: true, // Default Behaviour
            TaskDate: current.TaskDate,
            TaskSubmittedDate: current.TaskSubmittedDate,
            ClientGUID: uuidv4(),
          });
          let AssignedTo = "";
          let NextLevelAssignedUsersIds = "";
          if (current.WFLevel >= current.TotalWFLevel) {
            AssignedTo = null;
            NextLevelAssignedUsersIds = 0;
          } else {
            if(mode === 'Approve') {
              AssignedTo = current.NextLevelAssignedUser;
              NextLevelAssignedUsersIds = current.NextLevelAssignedUsersIds;
            } else if(mode === 'Reject') {
              AssignedTo = current.TaskdataAddedBy;
              NextLevelAssignedUsersIds = current.DataCollectedBy;
            }
          }
          let myTaskDataItem = {
            ClientSyncTaskID: "",
            Type: current.Type,
            TypeID: current.TypeID,
            WFLevel: current.WFLevel,
            WFLevelStatus: "Completed",
            TaskUpdateComment: '',
            AttachedDocumentName: '',
            SyncDBID: current.SyncDBID,
            ProjectName: this.TaskCardData.ProjectName,
            TaskName: '',
            Description: '',
            CreatedDate: today,
            TaskDate: current.TaskDate,
            TaskSubmittedDate: today,
            WorkspaceID: current.WorkspaceID,
            TaskSubmittedBy: UserName,
            ToDelete: 1,
            NextAssignToUserId: NextLevelAssignedUsersIds,
            NextLevelAssignedUser: AssignedTo,
            selectedTaskpkId: 0,
            Command: 'TaskResponse'
          }
          switch (mode) {
            case 'Approve':
              myTaskDataItem.WFLevelStatus = "Completed"
              break;
          
            case 'Reject':
              myTaskDataItem.WFLevelStatus = "Rejected"
              break;
            }
            tbMyTaskData.push(myTaskDataItem)
        }
        const payload = {
          UserID: this.currentUser.UserId,
          TaskSyncData: taskSyncData,
          TaskBulkData: tbMyTaskData,
          isBulk: this.selectedResponses.length > 1 ? true : false
        }
        let result = await this.bulkTaskController.authorizeTasks(payload)
        if(result.status === 'success') {
          await dbImworks.TbMyTask.bulkPut(tbMyTaskData)
            .then((lastkey)=> {
              console.log('updated to TbMyTask with', lastkey)
            })
            .catch(Dexie.BulkError, (e)=>{
              throw new Error(e)
            })
          console.log('syncFromFlag', result)
          await this.GetMyTaskFromServer();
          switch (mode) {
            case 'Approve':
              this.snackbar = {
                value: true,
                message: "Task Approved Successfully",
                color: "success",
              }
              break;
            case 'Reject':
              this.snackbar = {
                value: false,
                message: "Task Rejected Successfully",
                color: "error",
              }
              break;
          }
        } else {
          throw new Error('An Error Occured')
        }
      } catch (error) {
        this.snackbar = {
          value: true,
          message: "An Error Occured",
          color: "error",
        };
        console.error(error);
      } finally {
        this.surveyAuthDlg = false;
        this.surveyAuthType = "";
        this.authMessage = ""
        this.allResponses = await this.getPendingSurveys();
        this.selectedResponses = [];
        this.isLoading = false;
      }
    },
    onCellValueChanged(params) {
      // if(!newValue)
      if(!this.responseEditMode) {
        // handle approve/reject related changes here
        return 
      }
      let newValue = params.newValue;
      const undoSize = params.api.getCurrentUndoSize();
      undoSize > 0 ? (this.undoBtn = true) : (this.undoBtn = false);
      const redoSize = params.api.getCurrentRedoSize();
      redoSize > 0 ? (this.redoBtn = true) : (this.redoBtn = false);
      let QuestionID = params.colDef.meta.question.QuestionID;
      let isBeneficiaryAnswer = params.colDef.meta.isBeneficiaryQuestion
      let pID = params.data[QuestionID].ID;
      let index = this.updationData.findIndex((x) => x.ID === pID);
      if (index >= 0) {
        this.updationData[index].Answer = newValue;
      } else {
        let payload = {
          ID: pID,
          QuestionID,
          Answer: newValue,
        }
        if(isBeneficiaryAnswer) payload['isBeneficiary'] = true
        this.updationData.push(payload);
      }
    },
    clearAllFilters(){
      this.gridApi.setFilterModel(null);
    },
    onFilterChanged(params) {
      let activeFilters = this.gridApi.getFilterModel();
      this.showClearFilter = typeof activeFilters === "object" && Object.keys(activeFilters).length;
    },
    async GetMyTaskFromServer() {
      var obj = syncData;
      await obj.syncData.SyncFromServer();
    },
    redraw(){
      this.isLoading = true
      this.gridApi.redrawRows();
      this.isLoading = false
    },
    async syncResponseData() {
      // When updating Beneficiary Data only unbind the map and resync data from server
      this.setMappedResponses(new Map())
      this.setResponseContext({})
      await this.getPendingSurveys()
    },
    switchSheetGlobal(sheet) {
      // this.$bus.$emit('switchSheet', sheet)
      const renderedInstances = this.gridApi.getCellRendererInstances()
      for (let index = 0; index < renderedInstances.length; index++) {
        // console.log(renderedInstances[index].children)
        const element = renderedInstances[index].$children[2];
        const { sheetsList } = element
        const _sheet = sheetsList.find((x) => {
          return x.name === sheet
        })
        element.switchDataSets(_sheet.id)
      }
      this.updateCurrentLogic(sheet)
    },
    async gridsAction(mode) {
      switch(mode) {
        case "edit": 
          this.setEditToggle(false)
          break;
        case "cancel":
          this.setEditToggle(true)
          this._beneficiaryEdit = false;
          this.updateUpdationData();
          this.redraw();
          break;
        case "save":
          // run save action globally
          this.isLoading = true;
          // check if the current global updation data has benficiary changes
          const isBen = this.globalUpdationData.some(x => x.isBeneficiary)
          await this.saveGridData()
          if(isBen) {
            await this.syncResponseData();
          }
          this.setEditToggle(true)
          this._beneficiaryEdit = false
          this.updateUpdationData()
          this.isLoading = false;
          break;
      }
    },
    pageSwitchHelper(e) {
      this.switchSheetGlobal("Survey Responses")
      window.scroll(0,0);
    },
    crossGridScroller(e) {
      const curr = e.target.scrollLeft;
      const els = document.getElementsByClassName("ag-center-cols-viewport")  
      for(let i = 1; i < els.length; i++) {
        els[i].scroll({
          left: curr,
          behavior: "instant"
        })
      }
    },
    globalExportController() {
      const workbook = XLSX.utils.book_new();
      const books = []
      const gridInstances = this.gridApi.getCellRendererInstances()
      gridInstances.forEach((parent) => {
        const grid = parent.$children[2]
        books.push(grid.globalExportWorker())
      })
      books.forEach((book) => {
        book.SheetNames.forEach((sheetName) => {
          const sheetData = book.Sheets[sheetName];
          const existingsheet = workbook.Sheets[sheetName];
          if (existingsheet) {
            const mergedSheet = XLSX.utils.sheet_add_json(existingsheet, XLSX.utils.sheet_to_json(sheetData), {
              origin: -1
            })
            workbook.Sheets[sheetName] = mergedSheet
          } else {
            XLSX.utils.book_append_sheet(workbook, sheetData, sheetName);
          }
        })
      })
      XLSX.writeFile(workbook, `${this.$route.params.SurveyName}.xlsx`);
    },
  },
  beforeRouteEnter(to, from, next) {
    if ([to.params.TaskCardData, to.query.SurveyID, to.query.ProjectId].some(x => !x)) {
      next(-1);
    } else {
      next()
    }
  },
  beforeRouteLeave (to, from, next) {
    this.resetLogicList();
    next();
  },
  destroyed() {
    this.clearAllGrids();
  },
};
</script>

<style lang="scss" scoped>
@import "~ag-grid-community/styles/ag-grid.css";
@import "~ag-grid-community/styles/ag-theme-alpine.css";
@import "~ag-grid-community/styles/ag-theme-balham.css";
.cardWrapper {
  background-color: #f5f6f8;
  overflow: hidden;

  .cardBody {
    padding-bottom: 0;
  }

  .summaryContainer {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    padding: 1.5rem 0.5rem;
    padding-right: 1rem;
    margin: 1rem 0;
    border-radius: 8px;
    background-color: #fff;
    gap: 10%;

    .cell {
      display: flex;
      .pill {
        width: 4px;
        height: 24px;
        border-radius: 80px;
        background-color: rgba(57, 156, 255, 1);
        margin: auto 1rem;
      }

      .content {
        ._title {
          color: #18426b;
          font-family: "Poppins";
          font-style: normal;
          font-weight: 500;
          font-size: 1.5rem;
          line-height: 32px;
        }

        ._subtitle {
          font-family: "Poppins";
          font-style: normal;
          font-weight: 500;
          font-size: 1rem;
          line-height: 1.5rem;
        }
      }
    }
  }
}

.actionsContainer {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  background-color: white;
  margin: 0 -1rem;
  padding: 0.25rem 4px;
}

// .activityContainer {
//   // margin: 0 -1rem;
// }

.editorFooter{

    display: flex;
    flex-wrap: nowrap;
    flex-direction: row;
    justify-content: space-between;
    align-items: flex-start;
    background-color: #fff;

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

.contextContainer {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  background-color: white;
  margin: 0 -1rem;
  padding: 0.75rem 1rem;
  width: 97vw;
  overflow-x: scroll
}

.responsesContainer,
.editorContainer {
  background-color: white;
  margin: 0 -1rem;
  padding: 1rem 3px;
  padding-top: 0;

  #CrossGridScroller{
    width: 100%;
    height: 50px;
    color: red;
    overflow-x: scroll;
  }

}

._btn_rounded {
  border-radius: 8px;
}

.ag-theme-alpine {
  // .ag-root-wrapper {
  //   border: none;

  //   .ag-header {
  //     border-bottom: none;
  //   }
  // }
  --ag-borders: none;
  --ag-header-background-color: #ebecee;
  --ag-checkbox-border-radius: 5px;
  --ag-checkbox-checked-color: rgba(57, 156, 255, 1);
  --ag-checkbox-checked-border-color: rgba(255, 255, 255, 1);

  .ag-cell {
    border-right: 0.25px solid lightgrey !important;
  }
}

/* Deep Selectors Begin */
::v-deep .v-tabs-bar {
  background-color: transparent !important;
}
::v-deep .v-tabs-items {
  background-color: transparent !important;
}
</style>

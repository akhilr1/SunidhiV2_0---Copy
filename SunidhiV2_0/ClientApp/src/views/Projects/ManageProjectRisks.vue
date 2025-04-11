<template>
  <v-card class="elevation-0">
    <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
    <v-card-title style="height: 73px"
                  v-show="RiskDetailsTableItems.length > 0">
        <v-row>
            <h5 class="page-head">
                <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-alert-box-outline</v-icon>
                </v-avatar>
                Risks
            </h5>
            <v-spacer></v-spacer>
            <!--<Success-Button class="mr-4" title="Create Risks" @click.native="CreateRisksDialog=true"></Success-Button>-->
            <v-menu offset-y
                    transition="scroll-y-transition"
                    v-bind:disabled="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="39"
                           v-on="on"
                           color="success-btn"
                           dark
                           class="elevation-0 mr-4">
                        Create Risks
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white"
                        light=""
                        width="auto"
                        max-width="250"
                        class="action-button-list">
                    <v-list-item id="AutoTest_IP624"
                                 @click.stop="
              (CreateRisksDialog = true),
                (AddEditActivityDialogHeader = 'Create Risk'),
                (AddEditActivityHeader1 =
                  'Please enter the basic details to create a new risk.')
            ">
                        <v-list-item-icon>
                            <v-icon>mdi-plus-circle-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content> Create Risk </v-list-item-content>
                    </v-list-item>
                    <v-list-item id="AutoTest_IP625"
                                 @click.stop="ImportRiskDialogBox = true">
                        <v-list-item-icon>
                            <v-icon>mdi-file-account-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content> Import Risk </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>
            <!--Bulk Action -->
            <v-menu :close-on-click="!closeOnClick"
                    offset-y
                    transition="scroll-y-transition"
                    v-bind:disabled="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="39"
                           v-on="on"
                           color="grey"
                           outlined
                           class="elevation-0 mr-4">
                        Bulk Actions
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white"
                        light=""
                        width="auto"
                        max-width="250"
                        class="action-button-list">
                    <v-list-item id="AutoTest_IP626" @click.stop="fnBulkRemoveClick()">
                        <v-list-item-icon>
                            <v-icon>mdi-trash-can-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content> Remove </v-list-item-content>
                    </v-list-item>
                </v-list>
                <!-- Remove Risk-->
                <v-dialog v-model="RemoveBulkRiskDialog" width="800">
                    <v-card>
                        <v-card-title primary-title class="page-head py-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                            </v-avatar>
                            Remove Risk
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_IP627"
                                   text=""
                                   @click="(RemoveBulkRiskDialog = false), (selectedRisk = [])">
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
                                                    <h4 class="text-center heading-3">
                                                        Are you sure you want to remove this risks ?
                                                    </h4>
                                                </v-col>

                                                <v-col cols="12"
                                                       xs="12"
                                                       sm="12"
                                                       md="12"
                                                       class="text-right py-0">
                                                    <v-btn id="AutoTest_IP628"
                                                           color="secondary elevation-0"
                                                           class="mr-2"
                                                           @click="
                              fnDeleteRisk(1), (RemoveBulkRiskDialog = false)
                            ">Remove</v-btn>
                                                    <v-btn id="AutoTest_IP629"
                                                           outlined
                                                           color="grey"
                                                           @click="
                              (RemoveBulkRiskDialog = false),
                                (selectedRisk = [])
                            ">Cancel</v-btn>
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
            <v-text-field label="Search"
                          class="mr-4 table-search"
                          v-model="SearchRiskDetails"
                          dense
                          prepend-inner-icon="mdi-magnify"
                          outlined
                          hide-details></v-text-field>
            <!--Filter-->
            <v-menu :close-on-content-click="false"
                    offset-y
                    transition="scroll-y-transition">
                <template v-slot:activator="{ on }">
                    <v-btn id=""
                           v-on="on"
                           height="39"
                           outlined
                           dense
                           class="outlined-btn-dark mr-2"
                           @click="filterContent = !filterContent">
                        <v-icon>mdi-filter-outline</v-icon>
                        Filters
                    </v-btn>
                </template>
            </v-menu>
        </v-row>
    </v-card-title>

    <v-row no-gutters v-if="filterContent">
      <h4 class="mx-4 my-2 page-head">Filters</h4>
      <!--Date range-->
      <v-menu
        :close-on-content-click="false"
        offset-y
        transition="scroll-y-transition"
        v-bind:disabled="!screenAccess"
        v-model="dateRangeMenu"
      >
        <template v-slot:activator="{ on }">
            <v-btn height="39"
                   v-on="on"
                   color="grey"
                   outlined=""
                   class="elevation-0 mx-1">
                {{dateRangeHeaderName}}
                <v-icon class="ml-10"> mdi-chevron-down </v-icon>
            </v-btn>
        </template>
        <v-list color="white" width="auto" class="px-2">
          <v-row justify="center" no-gutters>
            <v-col cols="4" class="px-2">
              <h3 style="text-align: center">Start Date</h3>
              <v-date-picker color="primary" v-model="FiterStartDate" no-title @input="IsValidDate()"
                             class="elevation-0"></v-date-picker>
            </v-col>
            <v-col cols="4" class="px-2">
              <h3 style="text-align: center">End Date</h3>
              <v-date-picker color="primary" v-model="FiterEndDate" no-title :min="FiterStartDate" @input="IsValidDate()"
                             class="elevation-0"></v-date-picker>
            </v-col>
            <v-col cols="4">
              <v-list class="px-2 mx-4">
                <v-header style="font-size: large" class="pl-4"
                  >Predefined Periods:</v-header
                >
                <v-list-item-group v-model="selectedItem" color="primary">
                  <v-list-item v-for="(item, i) in items" :key="i">
                    <v-list-item-content>
                      <v-list-item-title v-text="item.text"></v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                </v-list-item-group>
              </v-list>
            </v-col>
          </v-row>
          <v-divider></v-divider>
          <v-row justify="center" class="px-2">
            <v-col cols="2">
              <v-btn
                height="32"
                block=""
                class="elevation-0"
                @click="dateRangeMenu = false"
              >
                cancel
              </v-btn>
            </v-col>
            <v-spacer></v-spacer>
            <v-col cols="2">
              <v-btn
                height="32"
                block=""
                color="primary"
                class="elevation-0"
                @click="fnClickFilterDone(),dateRangeMenu = false"
              >
                Done
              </v-btn>
            </v-col>
          </v-row>
        </v-list>
      </v-menu>

      <!--Workflow-->
      <v-menu
        :close-on-content-click="false"
        offset-y
        transition="scroll-y-transition"
        v-bind:disabled="!screenAccess"
        v-model="workflowMenu"
      >
        <template v-slot:activator="{ on }">
          <v-btn
            height="39"
            v-on="on"
            color="grey"
            outlined=""
            class="elevation-0 mx-1"
          >
            Workflow
            <v-icon class="ml-12"> mdi-chevron-down </v-icon>
          </v-btn>
        </template>
        <v-list
          color="white"
          light=""
          width="auto"
          max-width="250"
          min-width="50"
          class="action-button-list pa-2"
        >
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="Evaluation Approval"
          ></v-checkbox>
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="Data Collection Approval"
          ></v-checkbox>
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="Financial Approval"
          ></v-checkbox>

          <div class="d-inline-flex">
            <v-btn
              id=""
              height="32"
              width="32"
              block
              color="#BEBEBE"
              dark
              class="elevation-0 mr-1"
              @click="workflowMenu = false"
            >
              Cancel
            </v-btn>
            <v-btn
              id=""
              height="32"
              width="32"
              block
              color="primary"
              class="elevation-0"
              @click="workflowMenu = false"
            >
              Done
            </v-btn>
          </div>
        </v-list>
      </v-menu>

      <!--Assigned User-->
      <v-menu
        :close-on-content-click="false"
        offset-y
        transition="scroll-y-transition"
        v-bind:disabled="!screenAccess"
        v-model="assignedUserMenu"
      >
        <template v-slot:activator="{ on }">
          <v-btn
            height="39"
            v-on="on"
            color="grey"
            outlined=""
            class="elevation-0 mx-1"
          >
            Assigned User
            <v-icon class="ml-12"> mdi-chevron-down </v-icon>
          </v-btn>
        </template>
        <v-list
          color="white"
          light=""
          width="auto"
          max-width="250"
          min-width="50"
          class="action-button-list pa-2"
        >
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="koushik"
          ></v-checkbox>
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="subhash"
          ></v-checkbox>
          <v-checkbox
            color="success-check-box"
            input-value="true"
            label="kishore"
          ></v-checkbox>

          <div class="d-inline-flex">
            <v-btn
              id=""
              height="32"
              width="32"
              block
              color="#BEBEBE"
              dark
              class="elevation-0 mr-1"
              @click="assignedUserMenu = false"
            >
              Cancel
            </v-btn>
            <v-btn
              id=""
              height="32"
              width="32"
              block
              color="primary"
              class="elevation-0"
              @click="assignedUserMenu = false"
            >
              Done
            </v-btn>
          </div>
        </v-list>
      </v-menu>
    </v-row>

    <v-row v-if="filterChipsList.length>0" class="pa-0">
            <v-col cols="12" xs="12" sm="12" md="12" class="pa-0">
                <div class="chip-container ma-0 ml-2">
                    <v-chip-group multiple column active-class="primary--text">
                        <v-chip id="" close v-for="(item,key) in filterChipsList" v-bind:key="item.ChipID" class="mx-2" @click:close="fnChipClose(item)" memdium label>
                            {{ item.ChipName }}
                        </v-chip>
                    </v-chip-group>
                </div>
            </v-col>
    </v-row>

    <v-row v-if="FilterRiskDetailsTableItems.length > 0">
      <!--Risk Table-->
      <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
        <v-data-table
          :headers="RiskDetailsTableheaders"
          :items="FilterRiskDetailsTableItems"
          :search="SearchRiskDetails"
          show-select
          item-key="riskId"
          v-model="selectedRisk"
          class="elevation-0"
        >
         <template v-slot:item.StartDate="{ item }">
              <div class="pa-0 text-limit" style="width:90px">
                  {{new Date(item.StartDate).toLocaleDateString("en-GB")}}
              </div>
          </template>
          <template v-slot:item.EndDate="{ item }">
              <div class="pa-0 text-limit" style="width:90px">
                  {{new Date(item.EndDate).toLocaleDateString("en-GB")}}
              </div>
          </template>
            <template v-slot:item.LastUpdatedDate="{ item }">
                <div class="pa-0 text-limit" style="width:135px">
                    {{item.LastUpdatedDate == '' || item.LastUpdatedDate == undefined ? 'N/A':item.LastUpdatedDate}}
                </div>
          </template>
          <!-- Risk Name-->
          <template v-slot:item.RiskName="{ item }">
            <v-list class="py-0 table-v-list">
              <!--if path is ManageProjectRisks-->
              <v-list-item
                v-if="$route.name == 'ManageProjectRisks'"
                class="my-0 px-0"
                link
                :to="{
                  name: 'ManageProjectRiskActivityLog',
                  query: {
                    WsID: WorkspaceID,
                    Access: RWAccess,
                    riskId: item.riskId,
                    ProjID: ProjectId,
                    InitID: InitiativeId,
                    Type: 'Edit',
                  },
                }"
              >
                  <div class="pa-0 text-limit">
                      <v-avatar size="25" color="#fff" class="elevation-1">
                          <v-icon size="15" color="primary">mdi-book</v-icon>
                      </v-avatar>
                      
                          {{ item.RiskName }}
                  </div>
              </v-list-item>
              <!--______________________________-->
              <!--if path is MyprojectsDraftIndividualRisks-->
              <v-list-item
                v-if="$route.name == 'MyprojectsDraftIndividualRisks'"
                class="my-0 px-0"
                link=""
                :to="{
                  name: 'MyprojectsDraftIndividualRiskActivityLog',
                  query: {
                    WsID: WorkspaceID,
                    Access: RWAccess,
                    riskId: item.riskId,
                    ProjID: ProjectId,
                    InitID: InitiativeId,
                    Type: 'Edit',
                  },
                }"
              >
                  <div class="pa-0 text-limit" >
                      <v-avatar size="25" color="#fff" class="elevation-1">
                          <v-icon size="15" color="primary">mdi-book</v-icon>
                      </v-avatar>
                      {{ item.RiskName }}
                  </div>
              </v-list-item>
              <!--______________________________-->
              <!--if path is MyprojectsDeployedIndividualRisks-->
              <v-list-item
                v-if="$route.name == 'MyprojectsDeployedIndividualRisks'"
                class="my-0 px-0"
                link=""
                :to="{
                  name: 'MyprojectsDeployedIndividualRiskActivityLog',
                  query: {
                    WsID: WorkspaceID,
                    Access: RWAccess,
                    riskId: item.riskId,
                    ProjID: ProjectId,
                    InitID: InitiativeId,
                    Type: 'Edit',
                  },
                }"
              >
                  <div class="pa-0 text-limit" >
                      <v-avatar size="25" color="#fff" class="elevation-1">
                          <v-icon size="15" color="primary">mdi-book</v-icon>
                      </v-avatar>
                      {{ item.RiskName }}
                  </div>
              </v-list-item>
              <!--______________________________-->
            </v-list>
          </template>
              <!--WorkFlow-->
          <template v-slot:item.WorkflowName="{ item }">
              <div class="pa-0 text-limit" style="width:100px">
                  {{item.WorkflowName == '' || item.WorkflowName == undefined ? 'N/A':item.WorkflowName}}
              </div>
          </template>
            <!--RiskType-->
          <template v-slot:item.RiskType="{ item }">
              <div class="pa-0 text-limit" style="width:90px">
                  {{item.RiskType == '' || item.RiskType == undefined ? 'N/A':item.RiskType}}
              </div>
          </template>
          <!-- Risk Rate-->
          <template v-slot:item.RiskRating="{ item }">
            <div v-if="item.RiskRating <= 3"  class="pa-0 text-limit" style="width:100px">
              <v-avatar color="#E0F4E8" size="20" class="mr-1">
                <v-icon size="10" color="#47BB76"
                  >mdi-arrow-bottom-right</v-icon
                >
              </v-avatar>
              Low Risk
            </div>
            <div
              class="pa-0 text-limit" style="width:100px"
              v-if="item.RiskRating > 3 && item.RiskRating <= 6"
            >
              <v-avatar color="#FEEEC2" size="20" class="mr-1">
                <v-icon size="10" color="##FCC735">mdi-arrow-right</v-icon>
              </v-avatar>
              Medium Risk
            </div>
            <div  class="pa-0 text-limit" style="width:100px" v-if="item.RiskRating > 6">
              <v-avatar color="#FDE4E4" size="20" class="mr-1">
                <v-icon size="10" color="#F24646">mdi-arrow-top-right</v-icon>
              </v-avatar>
              High Risk
            </div>
          </template>
          <!-- Users-->
          <template v-slot:item.UserList="{ item }">
            <div v-if="item.UserList.length > 0" style="width:60px">
              <v-tooltip left>
                <template v-slot:activator="{ on }">
                  <div v-on="on" class="d-inline-flex">
                    <v-avatar
                      :color="`${getcolor(item.UserList[0].Name)}`"
                      size="30"
                      class="team-avatar"
                      v-if="
                        item.UserList.length >= 1 &&
                        item.UserList[0].Name.length > 0
                      "
                    >
                      <v-img
                        v-bind:src="userimage + item.UserList[0].ProfileImage"
                        alt="User DP" height="30" width="30" class="ma-4"
                        v-if="item.UserList[0].ProfileImage.length > 0"
                      ></v-img>
                      <span class="white--text" v-else>{{
                        item.UserList[0].Name == null
                          ? ""
                          : item.UserList[0].Name.substring(0, 1).toUpperCase()
                      }}</span>
                      <!--<span class="text--white">{{item.UserList[0].Name.charAt(0)}}</span>-->
                    </v-avatar>
                    <!--<v-avatar :color="`${getcolor(item.UserList[1].Name)}`" size="25" class="mr-1" v-if="item.UserList.length >=2">
                                                    <span class="text--white">{{item.UserList[1].Name.charAt(0)}}</span>
                                                </v-avatar>-->
                  <v-avatar size="30"
                            class="team-count"
                            v-if="item.UserList.length > 1">
                      +{{ item.UserList.length - 1 }}
                  </v-avatar>
                  </div>
                </template>
                <span
                  v-if="
                    item.UserList.length >= 1 &&
                    item.UserList[0].Name.length > 0
                  "
                  >{{ item.UserList[0].Name }}</span
                >
                <span v-if="item.UserList.length >= 2"
                  >,{{ item.UserList[1].Name }}</span
                >
              </v-tooltip>
            </div>
          </template>
          <!-- Row Actions-->
          <template v-slot:item.Actions="{ item }">
            <v-menu
              close-on-content-click=""
              transition="scroll-y-transition"
              left=""
              offset-y=""
              v-bind:disabled="!screenAccess"
            >
              <template v-slot:activator="{ on }">
                <v-btn
                  min-width="27"
                  height="29"
                  v-on="on"
                  color="grey"
                  outlined=""
                  dark=""
                  class="elevation-0 mr-2 px-0"
                >
                  <v-icon>mdi-dots-vertical</v-icon>
                </v-btn>
              </template>
              <v-list
                color="white"
                light
                width="auto"
                class="action-button-list"
              >
                <!-- Edit-->
                <!--if path is ManageProjectRisks-->
                <v-list-item
                  v-if="$route.name == 'ManageProjectRisks'"
                  link
                  :to="{
                    name: 'ManageProjectRiskActivityLog',
                    query: {
                      WsID: WorkspaceID,
                      Access: RWAccess,
                      riskId: item.riskId,
                      ProjID: ProjectId,
                      InitID: InitiativeId,
                      Type: 'Edit',
                    },
                  }"
                >
                  <v-list-item-icon
                    ><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon
                  >
                  <v-list-item-content>Edit</v-list-item-content>
                </v-list-item>
                <!--______________-->
                <!--if path is MyprojectsDraftIndividualRisks-->
                <!--<v-list-item v-if="$route.name=='MyprojectsDraftIndividualRisks'" link="" :to="{name:'MyprojectsDraftIndividualRiskActivityLog',query: { WsID: WorkspaceID, Access:RWAccess,riskId:item.riskId,ProjID:ProjectId,InitID:InitiativeId, Type :'Edit'}}">
        <v-list-item-icon>
            <v-icon>mdi-pencil-outline</v-icon>
        </v-list-item-icon>
        <v-list-item-content>Edit</v-list-item-content>
    </v-list-item>-->

                <v-list-item
                  id="AutoRisk_IP211"
                  link=""
                  @click.stop="
                    (CreateRisksDialog = true),
                      (AddEditActivityDialogHeader = 'Edit Risk'),
                      fnGetRiskByriskIdEdit(item.riskId),
                      (AddEditActivityHeader1 =
                        'Please enter the basic details of the risk.')
                  "
                >
                  <v-list-item-icon>
                    <v-icon>mdi-pencil-outline</v-icon>
                  </v-list-item-icon>
                  <v-list-item-content>Edit</v-list-item-content>
                </v-list-item>

                <!--______________-->
                <!-- Assign Workflow-->
                <v-list-item
                  id="AutoTest_IP630"
                  link
                  @click.stop="
                    (riskId = item.riskId),
                      fnGetWorkflowsListByProjectID(item),
                      (SelectWorkflowDialog = true)
                  "
                >
                  <v-list-item-icon
                    ><v-icon
                      >mdi-timeline-text-outline</v-icon
                    ></v-list-item-icon
                  >
                  <v-list-item-content>Assign Workflow</v-list-item-content>
                </v-list-item>
                <!-- Assign User-->
                <v-list-item
                  id="AutoTest_IP631"
                  link
                  @click.stop="
                    (AssignUserDialog = true),
                      (riskId = item.riskId),
                      fnGetRiskByriskId((riskId = item.riskId))
                  "
                >
                  <v-list-item-icon
                    ><v-icon>mdi-account-outline</v-icon></v-list-item-icon
                  >
                  <v-list-item-content>Assign User</v-list-item-content>
                </v-list-item>
                <!-- View Activity Log-->
                <!--if path is ManageProjectRisks-->
                <v-list-item
                  v-if="$route.name == 'ManageProjectRisks'"
                  link
                  :to="{
                    name: 'ManageProjectRiskActivityLog',
                    query: {
                      WsID: WorkspaceID,
                      Access: RWAccess,
                      riskId: item.riskId,
                      ProjID: ProjectId,
                      InitID: InitiativeId,
                      Type: 'Log',
                    },
                  }"
                >
                  <v-list-item-icon
                    ><v-icon>mdi-eye-outline</v-icon></v-list-item-icon
                  >
                  <v-list-item-content>View Activity Log</v-list-item-content>
                </v-list-item>
                <!--______________________-->
                <!--if path is MyprojectsDraftIndividualRisks-->
                <v-list-item
                  v-if="$route.name == 'MyprojectsDraftIndividualRisks'"
                  link=""
                  :to="{
                    name: 'MyprojectsDraftIndividualRiskActivityLog',
                    query: {
                      WsID: WorkspaceID,
                      Access: RWAccess,
                      riskId: item.riskId,
                      ProjID: ProjectId,
                      InitID: InitiativeId,
                      Type: 'Log',
                    },
                  }"
                >
                  <v-list-item-icon>
                    <v-icon>mdi-eye-outline</v-icon>
                  </v-list-item-icon>
                  <v-list-item-content>View Activity Log</v-list-item-content>
                </v-list-item>
                <!--______________________-->
                <!--if path is MyprojectsDeployedIndividualRisks-->
                <v-list-item
                  v-if="$route.name == 'MyprojectsDeployedIndividualRisks'"
                  link=""
                  :to="{
                    name: 'MyprojectsDeployedIndividualRiskActivityLog',
                    query: {
                      WsID: WorkspaceID,
                      Access: RWAccess,
                      riskId: item.riskId,
                      ProjID: ProjectId,
                      InitID: InitiativeId,
                      Type: 'Log',
                    },
                  }"
                >
                  <v-list-item-icon>
                    <v-icon>mdi-eye-outline</v-icon>
                  </v-list-item-icon>
                  <v-list-item-content>View Activity Log</v-list-item-content>
                </v-list-item>
                <!--______________________-->
                <!-- Remove-->
                <v-list-item
                  id="AutoTest_IP632"
                  link
                  @click.stop="
                    (RemoveRiskDialog = true),
                      (riskId = item.riskId),
                      (riskName = item.RiskName)
                  "
                >
                  <v-list-item-icon
                    ><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon
                  >
                  <v-list-item-content>Remove</v-list-item-content>
                </v-list-item>
              </v-list>
            </v-menu>
          </template>
        </v-data-table>
      </v-col>
    </v-row>

    <!-- if No Data-->
    <v-row v-else>
        <v-col class="text-center">
            <p class="mt-12 pt-12">
                <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
            </p>
            <p class="heading-4-dark font-weight-medium pt-0">No risks available</p>
            <!--<Success-Button class="mt-4" style="width:200px" title="Create Risks" @click.native="CreateRisksDialog=true"></Success-Button>-->
            <v-menu offset-y
                    transition="scroll-y-transition"
                    v-bind:disabled="!screenAccess">
                <template v-slot:activator="{ on }">
                    <!--<Bulk-Action-Button-Outlined v-on="on" ></Bulk-Action-Button-Outlined>-->
                    <v-btn height="38"
                           v-on="on"
                           color="success-btn"
                           dark
                           class="elevation-0">
                        Create Risks
                        <v-icon>mdi-chevron-down</v-icon>
                    </v-btn>
                </template>
                <v-list color="white"
                        light=""
                        width="auto"
                        max-width="250"
                        class="action-button-list">
                    <v-list-item id="AutoTest_IP633"
                                 @click.stop="(CreateRisksDialog = true),(AddEditActivityDialogHeader = 'Create Risk')">
                        <v-list-item-icon>
                            <v-icon>mdi-plus-circle</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content> Create Risk </v-list-item-content>
                    </v-list-item>
                    <v-list-item id="AutoTest_IP634"
                                 @click.stop="ImportRiskDialogBox = true">
                        <v-list-item-icon>
                            <v-icon>mdi-file-account-outline</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content> Import Risk </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-menu>
        </v-col>
    </v-row>

    <!--Add Risk Dialog-->
    <v-dialog v-model="CreateRisksDialog" width="800" persistent>
      <v-card>
        <v-card-title primary-title="" class="page-head pb-0">
          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
            <v-icon size="20" color="primary">mdi-alert-box-outline</v-icon>
          </v-avatar>
          {{ AddEditActivityDialogHeader }}
          <v-spacer></v-spacer>
          <v-btn id="AutoTest_IP635" text="" @click="CloseRiskDialog()">
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-form ref="form">
          <v-card-text>
            <h4 class="text-center heading-3 mt-4">
              {{ AddEditActivityHeader1 }}
            </h4>
            <v-row justify="center">
              <v-col md="6" class="v-scrolling-div mt-4" style="height: 50vh">
                <v-text-field
                  outlined
                  dense
                  v-model="RiskName"
                  :rules="[rules.required]"
                  label="Risk Name"
                >
                  <template v-slot:append>
                    <span v-if="RiskName.length == 0" class="error--text"
                      >*</span
                    >
                  </template>
                </v-text-field>
                <v-textarea
                  outlined
                  dense
                  name
                  value=""
                  hide-details
                  v-model="Description"
                  :rules="[rules.required]"
                  label="Risk Description"
                >
                  <template v-slot:append>
                    <span v-if="Description.length == 0" class="error--text"
                      >*</span
                    >
                  </template>
                </v-textarea>
                <!-- Date Between-->
                <v-row>
                  <!-- From Date -->
                  <v-col>
                    <v-menu
                      v-model="FrmDateMenu"
                      :close-on-content-click="false"
                      transition="scale-transition"
                      offset-y
                      max-width="290px"
                      min-width="290px"
                    >
                      <template v-slot:activator="{ on }">
                        <v-text-field
                          v-model="FrmDate"
                          label="Start Date"
                          dense
                          persistent-hint
                          outlined
                          hide-details
                          v-on="on"
                        >
                          <template
                            v-if="startDate == '' || startDate == null"
                            v-slot:append=""
                          >
                            <span class="mandatory-star">*</span>
                          </template>
                        </v-text-field>
                      </template>
                      <v-date-picker
                        v-model="startDate"
                        color="primary"
                        no-title
                        @input="(FrmDateMenu = false), IsValidDate()"
                        format="DD-MM-YYYY"
                        dense
                        :min="ProjectStartDate"
                        :max="ProjectEndDate"
                      ></v-date-picker>
                    </v-menu>
                  </v-col>
                  <!-- To Date -->
                  <v-col>
                    <v-menu
                      ref="ToDateMenu"
                      v-model="ToDateMenu"
                      :close-on-content-click="false"
                      transition="scale-transition"
                      offset-y
                      max-width="290px"
                      min-width="290px"
                    >
                      <template v-slot:activator="{ on }">
                        <v-text-field
                          v-model="ToDate"
                          label="End Date"
                          dense
                          persistent-hint
                          outlined
                          hide-details
                          v-on="on"
                        >
                          <template
                            v-if="endDate == '' || endDate == null"
                            v-slot:append=""
                          >
                            <span class="mandatory-star">*</span>
                          </template>
                        </v-text-field>
                      </template>
                      <v-date-picker
                        v-model="endDate"
                        color="primary"
                        no-title
                        @input="(ToDateMenu = false), IsValidDate()"
                        dense
                        :min="startDate"
                        :max="ProjectEndDate"
                      ></v-date-picker>
                    </v-menu>
                  </v-col>
                </v-row>
                <v-select
                  hide-details
                  dense=""
                  :items="lstPeriodicity"
                  :item-text="(i) => i.Value"
                  :item-value="(i) => i.Key"
                  outlined
                  v-model="ReportingFrequency"
                  :rules="[rules.required]"
                  label="Select Reporting Frequency"
                >
                  <template
                    v-if="
                      ReportingFrequency == '' || ReportingFrequency == null
                    "
                    v-slot:append=""
                  >
                    <span class="mandatory-star">*</span>
                  </template>
                </v-select>
                <!-- Once -->
                <div v-show="ReportingFrequency === 2">
                  <v-menu
                    v-model="periodicityDateMenu"
                    :close-on-content-click="false"
                    transition="scale-transition"
                    offset-y
                    max-width="290px"
                    min-width="290px"
                  >
                    <template v-slot:activator="{ on }">
                      <v-text-field
                        v-model="FormattedDate"
                        label="Choose a Date"
                        dense
                        persistent-hint
                        outlined
                        v-on="on"
                        class="mt-4"
                        hide-details
                      ></v-text-field>
                    </template>
                    <v-date-picker
                      v-model="periodicityDate"
                      color="primary"
                      no-title
                      @input="
                        (periodicityDateMenu = false),
                          IsValidDate(),
                          IsValidPeriodicityDate()
                      "
                      dense
                      :min="startDate"
                      :max="endDate"
                      >></v-date-picker
                    >
                  </v-menu>
                </div>
                <!-- Weekly-->
                <div v-show="ReportingFrequency === 4" class="mt-4">
                  <v-select
                    dense=""
                    :items="lstWeek"
                    :item-text="(i) => i.Value"
                    :item-value="(i) => i.Key"
                    label="Choose a day"
                    outlined=""
                    v-model="ReportingWeek"
                    hide-details
                  >
                  </v-select>
                </div>
                <!-- Monthly-->
                <div v-show="ReportingFrequency === 5" class="mt-4">
                  <v-select
                    dense=""
                    :items="lstMonthDate"
                    :item-text="(i) => i.Value"
                    :item-value="(i) => i.Key"
                    label="Choose a Date"
                    outlined=""
                    v-model="ReportingMonthDay"
                    hide-details
                  >
                  </v-select>
                </div>
                <!-- Quaterly , Halyearly and Yearly-->
                <div
                  class="d-inline-flex mt-4"
                  v-if="
                    ReportingFrequency == 6 ||
                    ReportingFrequency == 7 ||
                    ReportingFrequency == 8
                  "
                >
                  <v-select
                    hide-details
                    class="mr-1 mb-4"
                    dense=""
                    :items="lstMonth"
                    :item-text="(i) => i.Value"
                    :item-value="(i) => i.Key"
                    label="Choose a Month"
                    outlined=""
                    v-model="ReportingMonth"
                  >
                  </v-select>
                  <v-select
                    class="mb-4"
                    hide-details
                    dense=""
                    :items="lstMonthDate"
                    :item-text="(i) => i.Value"
                    :item-value="(i) => i.Key"
                    label="Choose a Date"
                    outlined=""
                    v-model="ReportingMonthDateDay"
                  >
                  </v-select>
                </div>
                <v-select
                  class="mt-4"
                  dense=""
                  :items="SelectRiskTypes"
                  outlined=""
                  v-model="RiskType"
                  :rules="[rules.required]"
                  label="Select Risk Type"
                >
                  <template
                    v-if="RiskType == '' || RiskType == null"
                    v-slot:append=""
                  >
                    <span class="mandatory-star">*</span>
                  </template>
                </v-select>
                <v-select
                  dense=""
                  :items="SelectRiskRating"
                  outlined
                  v-model="RiskRating"
                  :rules="[rules.required]"
                  label="Select Risk Rating"
                >
                  <template
                    v-if="RiskRating == '' || RiskRating == null"
                    v-slot:append=""
                  >
                    <span class="mandatory-star">*</span>
                  </template>
                </v-select>
                <v-select
                  dense=""
                  :items="workflows"
                  :item-text="(i) => i.WfName"
                  :item-value="(i) => i.WorkflowID"
                  label="Select Workflow"
                  outlined=""
                  v-model="Workflow"
                >
                </v-select>
                <span class="small-text mr-1">
                  <v-icon size="15">mdi-account</v-icon> Users
                  <span class="mandatory-star">*</span>
                </span>
                <v-avatar
                  size="25"
                  class="team-avatar"
                  v-for="(item, key) in selectedteamUsers.slice(0, 3)"
                  v-bind:key="item.ID"
                  :color="`${getcolor(item.Name)}`"
                >
                  <img
                    v-bind:src="userimage + item.ProfileImage"
                    alt="User DP"
                    v-if="item.ProfileImage.length > 0"
                  />
                  <span class="white--text" v-else>{{
                    item.Name == null
                      ? ""
                      : item.Name.substring(0, 1).toUpperCase()
                  }}</span>
                </v-avatar>
                <v-avatar
                  v-if="selectedteamUsers.length >= 4"
                  size="25"
                  class="team-count"
                >
                  +{{ selectedteamUsers.length - 3 }}
                </v-avatar>
                <v-menu
                  style="height: 500px"
                  absolute=""
                  v-model="AssignAccountUserPopUp"
                  top=""
                  :offset-y="offset"
                  :close-on-click="closeonClick"
                  :close-on-content-click="closeOnContentClick"
                >
                  <template v-slot:activator="{ on }">
                    <v-btn
                      id="AutoTest_IP636"
                      v-on="on"
                      max-height="24"
                      max-width="24"
                      min-width="24"
                      min-height="24"
                      depressed=""
                      fab=""
                      light=""
                      color="#E5F2FF"
                      @click=""
                    >
                      <v-icon size="15" light="" color="#707070"
                        >mdi-plus</v-icon
                      >
                    </v-btn>
                  </template>
                  <!--Assign Account User pop up-->
                  <v-card>
                    <!--<v-card-title class="heading-3">
                      Add People to Notify
                      <v-spacer></v-spacer>
                      <v-btn
                        id="AutoTest_IP637"
                        text=""
                        height="25"
                        min-width="25"
                        max-width="25"
                        color="#707070"
                        @click="AssignAccountUserPopUp = false"
                      >
                        <v-icon size="20">mdi-close</v-icon>
                      </v-btn>
                    </v-card-title>
                    <v-divider></v-divider>-->
                    <v-card-text style="height: 300px">
                      <v-col cols="12" md="12" sm="12" xs="12" class="pa-0 ma-0">
                      <v-data-table
                        :headers="lstuserTableheaders"
                        :items="lstUsers"
                        show-select
                        fixed-header
                        height="32vh"
                        item-key="ID"
                        sort-by="Name"
                        v-model="selectedteamUsers"
                      >
                        <template v-slot:item.Name="{ item }">
                          <v-list class="py-0 table-v-list">
                            <v-list-item class="my-0 px-0">
                              <v-list-item-icon>
                                <v-avatar
                                  size="27"
                                  light
                                  :color="`${getcolor(item.Name)}`"
                                >
                                  <img
                                    v-bind:src="userimage + item.ProfileImage"
                                    alt="User DP"
                                    v-if="item.ProfileImage.length > 0"
                                  />
                                  <span class="white--text" v-else>
                                    {{
                                      item.Name == null
                                        ? ""
                                        : item.Name.substring(
                                            0,
                                            1
                                          ).toUpperCase()
                                    }}</span
                                  >
                                </v-avatar>
                              </v-list-item-icon>
                              <v-list-item-content>
                                {{ item.Name }}
                              </v-list-item-content>
                            </v-list-item>
                          </v-list>
                        </template>
                      </v-data-table>
                      </v-col>
                    </v-card-text>
                    <v-divider></v-divider>
                    <v-card-actions>
                      <v-row no-gutters="">
                        <v-col md="12" class="" align="right">
                          <Success-Button
                            id="AutoTest_IP638"
                            style="width: 130px"
                            class="btn-122x36"
                            title="Save"
                            @click.native="AssignAccountUserPopUp = false"
                          ></Success-Button>
                          <Default-Button-Outlined id="AutoTest_RI01" class="btn-122x36" title="Cancel" @click.native="AssignAccountUserPopUp = false"></Default-Button-Outlined>
                        </v-col>
                      </v-row>
                    </v-card-actions>
                  </v-card>
                  <!--_______________________________________-->
                </v-menu>
              </v-col>
            </v-row>
          </v-card-text>
        </v-form>
        <v-card-actions class="pb-4">
          <v-col>
            <Default-Button-Outlined
              id="AutoTest_IP639"
              class="btn-122x36 float-right"
              title="Cancel"
              @click.native="CloseRiskDialog()"
            ></Default-Button-Outlined>
            <Secondary-Button
              id="AutoTest_IP640"
              v-bind:disabled="fnCheckMandatory()"
              class="btn-122x36 mr-2 float-right"
              title="Confirm"
              @click.native="fnCreateRisk()"
            ></Secondary-Button>
          </v-col>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
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

    <!-- Assign Workflow-->
    <!-- Select Work flow-->
    <!-- Assign Workflow-->
    <!-- Select Work flow-->
    <v-dialog v-model="SelectWorkflowDialog" width="1100" persistent>
      <v-card>
        <v-card-title primary-title class="page-head pb-0">
          <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
            <img style="width: 70%" :src="Workflow_Tab_icon" /> </v-avatar
          >Add Workflow
          <v-spacer></v-spacer>
          <v-btn
            id="AutoTest_IP641"
            text
            @click="
              (SelectWorkflowDialog = false),
                (checkboxAddWorkflow = []),
                (selectedAddWorkflow = []),
                (SelectWorkflowDlgItems = []),
                (confirmError = false)
            "
          >
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-card-text class="py-0">
          <v-container fluid class="pt-0">
            <v-row no-gutters>
              <v-col cols="12" md="12" class="pt-0">
                <v-row no-gutters>
                  <v-col
                    v-if="
                      SelectWorkflowDlgItems.length > 0 && confirmError == false
                    "
                    cols="12"
                    md="12"
                    class="pt-0 mb-2"
                  >
                    <h4 class="text-center heading-3">
                      Please select workflow from below
                    </h4>
                  </v-col>
                  <v-col
                    v-if="
                      SelectWorkflowDlgItems.length > 0 && confirmError == true
                    "
                    cols="12"
                    md="12"
                    class="pt-0 mb-2"
                  >
                    <h4 class="text-center heading-3 red--text">
                      Please select workflow from below *
                    </h4>
                  </v-col>
                </v-row>
                <v-stepper v-model="SelectWorkflowDlgSteps" class="elevation-0">
                  <v-stepper-items>
                    <!-- Select Workflow step-->
                    <v-stepper-content step="1" class="pa-0">
                      <!-- if workflow available -->
                      <v-row
                        no-gutters
                        v-if="SelectWorkflowDlgItems.length > 0"
                      >
                        <!-- Work flow group chip-->
                        <v-col md="12" class="py-0">
                          <div class="chip-container mb-2 ml-5">
                            <v-chip-group
                              multiple
                              column
                              active-class="primary--text"
                            >
                              <v-chip
                                id="AutoTest_IP642"
                                v-for="(chipitem, key) in selectedAddWorkflow"
                                :key="chipitem.WFitem.WorkflowID"
                                @click:close="fnRemove(chipitem)"
                                close
                                class="ma-2"
                                small
                              >
                                <v-icon left size="15" color="#707070"
                                  >mdi-folder-key-network-outline</v-icon
                                >
                                {{ chipitem.WFitem.WfName }}
                              </v-chip>
                            </v-chip-group>
                          </div>
                        </v-col>

                        <!-- Work flow details card-->
                        <v-row
                          class="mx-4 v-scrolling-div"
                          style="height: 45vh"
                        >
                          <v-col
                            xs="4"
                            sm="6"
                            md="4"
                            v-for="(
                              addWorkflowItem, index
                            ) in SelectWorkflowDlgItems"
                            :key="addWorkflowItem.WorkflowID"
                          >
                            <v-card class="workspace-card" flat>
                              <v-row>
                                <v-col
                                  xs="6"
                                  sm="6"
                                  md="10"
                                  class="d-inline-flex pt-0"
                                  :ripple="false"
                                >
                                  <div>
                                    <v-avatar
                                      size="60"
                                      tile
                                      class="elevation-1 base-border-radius"
                                    >
                                      <v-img
                                        :src="Workflow_Icon"
                                        height="40"
                                        width="40"
                                        class="ma-4"
                                      ></v-img>
                                    </v-avatar>
                                  </div>
                                  <div class="title pt-4 px-2 text-limit">
                                    {{ addWorkflowItem.WfName }}
                                  </div>
                                </v-col>
                                <v-col
                                  xs="2"
                                  sm="2"
                                  md="2"
                                  class="d-inline-flex pt-0 text-right"
                                >
                                  <!-- <v-checkbox
                                    color="success-check-box"
                                    v-model="checkboxAddWorkflow[index]"
                                    @change="fnCheck(addWorkflowItem, index)"
                                    label=""
                                  ></v-checkbox> -->
                                  <input
                                    type="radio"
                                    class="mt-3"
                                    v-if="selectedAddWorkflow.length > 0"
                                    style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                    name="workflow"
                                    :checked="
                                      selectedAddWorkflow[0].checkboxindex ==
                                      index
                                        ? true
                                        : false
                                    "
                                    @click="
                                      (checkboxAddWorkflow[index] = true),
                                        fnCheck(addWorkflowItem, index)
                                    "
                                  />
                                  <input
                                    type="radio"
                                    class="mt-3"
                                    style="
                                      border: 0px;
                                      width: 100%;
                                      height: 1.4em;
                                    "
                                    name="workflow"
                                    v-else
                                    @click="
                                      (checkboxAddWorkflow[index] = true),
                                        fnCheck(addWorkflowItem, index)
                                    "
                                  />
                                </v-col>
                              </v-row>
                              <!--Description-->
                              <!--Description-->
                              <v-row>
                                <v-col
                                  sm="12"
                                  style="display: flex; flex-direction: row"
                                  class="py-0"
                                  :ripple="false"
                                >
                                  <span class="heading-5 black--text mt-2">
                                    <img :src="Description_icon" />
                                  </span>
                                  <div class="pt-2 pl-1">
                                    <span class="heading-5 black--text"
                                      >Description</span
                                    >
                                  </div>
                                </v-col>
                                <v-col sm="12" class="pt-0 pl-0">
                                  <p style="height: 24px" class="ml-3">
                                    {{
                                      addWorkflowItem.WfDescription.length >= 60
                                        ? addWorkflowItem.WfDescription.substring(
                                            0,
                                            60
                                          ) + "..."
                                        : addWorkflowItem.WfDescription
                                    }}
                                  </p>
                                </v-col>
                              </v-row>
                              <!-- Levels-->
                              <v-row>
                                <v-col class="py-0">
                                  <span class="heading-5 black--text ml-1">
                                    <img
                                      :src="Levels_icon"
                                      style="width: 3.5%"
                                    />
                                    Levels : {{ addWorkflowItem.WfNoOfLevels }}
                                  </span>
                                </v-col>
                              </v-row>
                              <!-- buttons-->
                              <v-row>
                                <v-col>
                                  <v-btn
                                    id="AutoTest_IP643"
                                    color="success-btn elevation-0 enter-btn"
                                    dark
                                    @click="showDetail(addWorkflowItem, index)"
                                  >
                                    Show Details
                                  </v-btn>
                                </v-col>
                              </v-row>
                            </v-card>
                          </v-col>
                        </v-row>
                        <!--Buttons-->
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right my-4 py4"
                        >
                          <Secondary-Button
                            id="AutoTest_IP644"
                            v-if="selectedAddWorkflow.length > 0"
                            class="btn-122x36 mr-2"
                            title="Confirm"
                            @click.native="fnAddSelectWorkflow()"
                          ></Secondary-Button>
                          <Secondary-Button
                            id="AutoTest_IP644"
                            v-else
                            class="btn-122x36 mr-2"
                            title="Confirm"
                            @click.native="confirmError = true"
                          ></Secondary-Button>
                          <Default-Button-Outlined
                            id="AutoTest_IP645"
                            class="btn-122x36"
                            title="Cancel"
                            @click.native="
                              (SelectWorkflowDialog = false),
                                (checkboxAddWorkflow = []),
                                (selectedAddWorkflow = []),
                                (SelectWorkflowDlgItems = []),
                                (confirmError = false)
                            "
                          ></Default-Button-Outlined>
                        </v-col>
                      </v-row>
                      <!-- if no workflow available -->
                      <v-row v-else no-gutter>
                        <v-col cols="12" sm="12" md="12">
                          <v-row :justify="justifyCenter">
                            <v-col md="6" class="text-center">
                              <v-list
                                class="base-border-radius warning-div pa-0"
                              >
                                <v-list-item>
                                  <v-list-item-icon class="mt-2">
                                    <v-avatar
                                      size="27"
                                      color="red"
                                      class="elevation-0"
                                    >
                                      <v-icon size="20" color="white"
                                        >mdi-exclamation</v-icon
                                      >
                                    </v-avatar>
                                  </v-list-item-icon>
                                  <v-list-item-content class="pb-0">
                                    <p class="pa-0">
                                      <span class="message">
                                        Sorry could not find any workflow.
                                      </span>
                                    </p>
                                  </v-list-item-content>
                                </v-list-item>
                              </v-list>
                            </v-col>
                          </v-row>
                        </v-col>
                      </v-row>
                    </v-stepper-content>

                    <!-- Show Details of Workflow-->
                    <v-stepper-content step="2" class="pa-0">
                      <v-container fluid class="pt-0">
                        <!-- Work flow group chip-->
                        <v-row>
                          <v-col sm="1">
                            <v-avatar
                              size="55"
                              tile
                              class="elevation-1 base-border-radius"
                            >
                              <v-img
                                :src="Workflow_Icon"
                                height="38"
                                width="38"
                                class="ma-4"
                              ></v-img>
                            </v-avatar>
                          </v-col>
                          <v-col sm="2" class="pt-8">
                            <span style="font-weight: bold; font-size: 18px">{{
                              ViewWorkflowItem.WfName
                            }}</span>
                          </v-col>
                          <v-col
                            style="text-align: end"
                            sm="9"
                            class="pt-6"
                            v-if="
                              selectedAddWorkflow.length > 0 &&
                              selectedAddWorkflow[0].checkboxindex ==
                                radioSelecIndex
                            "
                          >
                            <v-btn
                              elevation="1"
                              class="success-btn white--text"
                              @click="fnRemove(selectedAddWorkflow[0])"
                              >Deselect Workflow</v-btn
                            >
                          </v-col>
                          <v-col
                            style="text-align: end"
                            sm="9"
                            class="pt-6"
                            v-else
                          >
                            <v-btn
                              elevation="1"
                              class="success-btn white--text"
                              @click="
                                (checkboxAddWorkflow[radioSelecIndex] = true),
                                  fnCheck(ViewWorkflowItem, radioSelecIndex)
                              "
                              >Select Workflow</v-btn
                            >
                          </v-col>
                        </v-row>
                        <v-row
                          class="v-scrolling-div mx-4"
                          style="height: 45vh"
                          justify="center"
                        >
                          <v-col
                            md="8"
                            v-for="i in ViewWorkflowItem.WfNoOfLevels"
                            :key="i"
                          >
                            <v-card class="workspace-card pa-0" flat>
                              <v-row>
                                <v-col class="pt-0">
                                  <v-row class="pb-3">
                                    <v-col cols="12" md="12" class="py-0 pr-0">
                                      <v-row>
                                        <v-col
                                          cols="12"
                                          sm="12"
                                          class="pt-2 pb-2 pl-5"
                                        >
                                          Level {{ i }}
                                        </v-col>
                                      </v-row>
                                      <v-col cols="12" sm="12" class="pa-0">
                                        <v-divider></v-divider>
                                      </v-col>
                                      <div class="text-center">
                                        <v-progress-circular
                                          :rotate="360"
                                          :size="85"
                                          class="mt-3"
                                          :width="15"
                                          :value="
                                            (100 /
                                              ViewWorkflowItem.WfNoOfLevels) *
                                            i
                                          "
                                          color="#1EC2ED"
                                        >
                                          <span
                                            class="subtitle-1 text--primary"
                                          >
                                            {{ i }}
                                          </span>
                                        </v-progress-circular>
                                      </div>
                                    </v-col>
                                  </v-row>
                                </v-col>
                                <v-divider vertical></v-divider>
                                <v-col cols="6" md="6" class="text-left">
                                  <!---   if there is no data-->
                                  <v-row
                                    v-if="
                                      ViewWorkflowItem.WfLevelDetails.length ==
                                        0 ||
                                      ViewWorkflowItem.WfLevelDetails[i - 1]
                                        .usersList.length == 0
                                    "
                                    no-gutters
                                  >
                                    <v-col
                                      cols="12"
                                      md="12"
                                      class="pt-2 mb-0 text-center"
                                    >
                                      <p class="heading-1">No Users Added</p>
                                    </v-col>
                                  </v-row>
                                  <!-- if User Assigned-->
                                  <v-row v-else no-gutters>
                                    <v-col
                                      v-if="
                                        ViewWorkflowItem.WfLevelDetails[i - 1]
                                          .usersList.length > 0
                                      "
                                      cols="12"
                                      md="12"
                                    >
                                      <v-col class="text-center mb-0 pt-1">
                                        <v-avatar
                                          size="25"
                                          class="team-avatar"
                                          v-for="(
                                            item, key
                                          ) in ViewWorkflowItem.WfLevelDetails[
                                            i - 1
                                          ].usersList.slice(0, 3)"
                                          v-bind:key="item.ID"
                                          :color="`${getcolor(item.Name)}`"
                                        >
                                          <img
                                            v-bind:src="
                                              userimage + item.ProfileImage
                                            "
                                            alt="User DP"
                                            v-if="item.ProfileImage.length > 0"
                                          />
                                          <span class="white--text" v-else>{{
                                            item.Name == null
                                              ? ""
                                              : item.Name.substring(
                                                  0,
                                                  1
                                                ).toUpperCase()
                                          }}</span>
                                        </v-avatar>
                                        <v-avatar
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length >= 4
                                          "
                                          size="25"
                                          class="team-count"
                                        >
                                          +{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length - 3
                                          }}
                                        </v-avatar>
                                      </v-col>
                                      <v-col class="text-center mb-0 pt-0">
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length == 1
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }}
                                        </p>
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length == 2
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }},{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[1].Name
                                          }}
                                        </p>
                                        <p
                                          v-if="
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length > 2
                                          "
                                        >
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[0].Name
                                          }},{{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList[1].Name
                                          }}
                                          +
                                          {{
                                            ViewWorkflowItem.WfLevelDetails[
                                              i - 1
                                            ].usersList.length
                                          }}
                                        </p>
                                      </v-col>
                                    </v-col>
                                  </v-row>
                                </v-col>
                              </v-row>
                            </v-card>
                          </v-col>
                        </v-row>
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-right mt-4 py-4"
                        >
                          <Secondary-Button
                            id="AutoTest_IP648"
                            class="btn-122x36 mr-2"
                            v-if="selectedAddWorkflow.length > 0"
                            title="Confirm"
                            @click.native="fnAddSelectWorkflow()"
                          ></Secondary-Button>
                          <Secondary-Button
                            id="AutoTest_IP648"
                            class="btn-122x36 mr-2"
                            v-else
                            title="Confirm"
                            @click.native="confirmError = true"
                          ></Secondary-Button>

                          <Default-Button-Outlined
                            id="AutoTest_IP649"
                            class="btn-122x36"
                            title="Previous"
                            @click.native="SelectWorkflowDlgSteps = 1"
                          ></Default-Button-Outlined>
                        </v-col>
                      </v-container>
                    </v-stepper-content>
                  </v-stepper-items>
                </v-stepper>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-divider></v-divider>
      </v-card>
    </v-dialog>
    

    <!-- Assign Users-->
    <!-- Select User -->
    <v-dialog v-model="AssignUserDialog" width="800">
      <v-card>
        <v-card-title primary-title class="page-head pb-0">
          <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
            <v-icon size="20" color="primary"
              >mdi-account-outline</v-icon
            > </v-avatar
          >Add Users
          <v-spacer></v-spacer>
          <v-btn id="AutoTest_IP650" text @click="AssignUserDialog = false">
            <v-icon color="#707070" size="15">mdi-close</v-icon>
          </v-btn>
        </v-card-title>
        <v-card-text>
          <v-row no-gutters>
            <v-col cols="12" md="12" class="pt-0">
              <v-container fluid class="pt-2 px-1">
                <v-row no-gutters>
                  <v-col cols="12" md="12" class="text-right pt-5">
                    <v-text-field
                      outlined
                      dense
                      label="Search"
                      type="Text"
                      prepend-inner-icon="mdi-magnify"
                      v-model="searchUser"
                    >
                    </v-text-field>
                  </v-col>

                  <div
                    class="chip-container mb-2"
                    v-if="selectedteamUsers.length > 0"
                  >
                    <v-chip-group multiple column active-class="primary--text">
                      <v-chip
                        id="AutoTest_IP651"
                        close
                        v-for="item in selectedteamUsers"
                        :key="item.ID"
                        v-model="chipUsrItem"
                        @click:close="RemoveUser(item)"
                        class="ma-2"
                        small
                      >
                        <v-avatar
                          size="12"
                          left
                          :color="`${getcolor(item.Name)}`"
                        >
                          <img
                            v-bind:src="userimage + item.ProfileImage"
                            alt="User DP"
                            v-if="item.ProfileImage.length > 0"
                          />
                          <span class="white--text" v-else>{{
                            item.Name == null
                              ? ""
                              : item.Name.substring(0, 1).toUpperCase()
                          }}</span>
                        </v-avatar>
                        {{ item.Name }}
                      </v-chip>
                    </v-chip-group>
                  </div>
                  <!--v-chip for Assign roles ends here -->
                  <v-col cols="12" xs="12" sm="12" md="12">
                    <!--User List with checkbox add here-->
                    <v-data-table
                      :headers="lstuserTableheaders"
                      :items="lstUsers"
                      show-select
                      item-key="ID"
                      sort-by="Name"
                      fixed-header
                      height="25vh"
                      :search="searchUser"
                      v-model="selectedteamUsers"
                      class="elevation-0"
                    >
                      <template v-slot:item.Name="{ item }">
                        <v-list class="py-0 table-v-list">
                          <v-list-item class="my-0 px-0">
                            <v-list-item-icon>
                              <v-avatar
                                size="27"
                                light
                                :color="`${getcolor(item.Name)}`"
                              >
                                <img
                                  v-bind:src="userimage + item.ProfileImage"
                                  alt="User DP"
                                  v-if="item.ProfileImage.length > 0"
                                />
                                <span class="white--text" v-else>
                                  {{
                                    item.Name == null
                                      ? ""
                                      : item.Name.substring(0, 1).toUpperCase()
                                  }}</span
                                >
                              </v-avatar>
                            </v-list-item-icon>
                            <v-list-item-content>
                              {{ item.Name }}
                            </v-list-item-content>
                          </v-list-item>
                        </v-list>
                      </template>
                    </v-data-table>
                  </v-col>
                  <v-col
                    cols="12"
                    xs="12"
                    sm="12"
                    md="12"
                    class="text-left"
                    v-if="selectedteamUsers.length > 0"
                  >
                    <v-row>
                      <v-col cols="12" md="12" class="text-right">
                        <v-checkbox
                          color="success-check-box"
                          input-value="true"
                          label="Notify user"
                          class="float-right"
                        ></v-checkbox>
                      </v-col>
                    </v-row>
                  </v-col>
                  <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                    <Secondary-Button
                      id="AutoTest_IP652"
                      v-bind:disabled="selectedteamUsers.length == 0"
                      class="btn-122x36 mr-2"
                      title="Save"
                      @click.native="
                        fnAssignUserstoRisk(),
                          (AssignUserDialog = false)
                      "
                    ></Secondary-Button>
                    <Default-Button-Outlined
                      id="AutoTest_IP653"
                      class="btn-122x36"
                      title="Cancel"
                      @click.native="AssignUserDialog = false"
                    ></Default-Button-Outlined>
                  </v-col>
                </v-row>
              </v-container>
            </v-col>
          </v-row>
        </v-card-text>

        <v-divider></v-divider>
      </v-card>
    </v-dialog>
    

    <!-- Remove Risk-->
    <v-dialog v-model="RemoveRiskDialog" width="800">
      <v-card>
        <v-card-title primary-title class="page-head py-4">
          <v-avatar color="white" size="25" class="elevation-1 mr-2">
            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
          </v-avatar>
          Remove Risk
          <v-spacer></v-spacer>
          <v-btn
            id="AutoTest_IP654"
            text=""
            @click="(RemoveRiskDialog = false), (selectedRisk = [])"
          >
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
                      <h4 class="text-center heading-3">
                        Are you sure you want to remove this risk ?
                      </h4>
                    </v-col>
                    <v-col cols="12" md="12" class="pt-0">
                      <v-row justify="center">
                        <h5 class="page-head mt-4">
                          <v-avatar
                            color="white"
                            size="25"
                            class="elevation-1 mr-2 mb-1"
                          >
                            <v-icon size="15" color="primary"
                              >mdi-cash-multiple</v-icon
                            >
                          </v-avatar>
                          {{ riskName }}
                        </h5>
                      </v-row>
                    </v-col>
                    <v-col
                      cols="12"
                      xs="12"
                      sm="12"
                      md="12"
                      class="text-right py-0"
                    >
                      <Secondary-Button
                        id="AutoTest_IP655"
                        class="btn-122x36 mr-2"
                        title="Remove"
                        @click.native="fnDeleteRisk(2)"
                      ></Secondary-Button>
                      <Default-Button-Outlined
                        id="AutoTest_IP656"
                        class="btn-122x36"
                        title="Cancel"
                        @click.native="
                          (RemoveRiskDialog = false), (selectedRisk = [])
                        "
                      ></Default-Button-Outlined>
                    </v-col>
                  </v-row>
                </v-container>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
      </v-card>
    </v-dialog>
    

    <!-- Import User -->
    <v-dialog v-model="ImportRiskDialogBox" width="800">
        <v-card class="elevation-0 dialogHeight" style="overflow-y:hidden;overflow-x:hidden;">
            <v-card-title primary-title="" class="page-head pa-4 pb-0">
                <v-avatar color="white" size="25" class="elevation-1">
                    <v-icon size="20" color="primary">mdi-file-account-outline</v-icon>
                </v-avatar>Import Risk
                <v-spacer></v-spacer>
                <v-btn id="AutoTest_IP657"
                       text=""
                       @click="ImportRiskDialogBox = false">
                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                </v-btn>
            </v-card-title>
            <v-divider class="mt-3"></v-divider>
            <v-card-text class="pa-3">
                <v-container fluid="" class="pa-0">
                    <v-row class="pa-0">
                        <v-col cols="12" md="12" class="py-0">
                            <v-stepper v-model="ImportRiskSteps" class="elevation-0 py-0">
                                <v-stepper-items>
                                    <v-stepper-content step="1" class="px-3 py-0">
                                        <v-container fluid class="pa-0">
                                            <v-row no-gutters="" class="uploadBoxHeight pa-0 pt-3">
                                                <v-col cols="12" md="12" class="pa-0">
                                                    <h4 class="text-center heading-3">
                                                        Step1 : Upload your CSV file
                                                    </h4>
                                                </v-col>
                                                <v-col cols="12" md="12" class="text-center">
                                                    <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
                                                        <v-card-text class="py-0">
                                                            <v-row no-gutters>
                                                                <v-col>
                                                                    <v-avatar size="100" tile>
                                                                        <img src="../../images/Import-icon.svg"
                                                                             alt="Import" />
                                                                    </v-avatar>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row no-gutters>
                                                                <v-col>
                                                                    <p class="text-medium">
                                                                        Your CSV file must map all our mandatory
                                                                        fields<br />
                                                                        and your files cannot exceed more than
                                                                        1000 rows
                                                                    </p>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row>
                                                                <v-col align-center justify-center>
                                                                    <v-btn id="AutoTest_IP658"
                                                                           color="grey"
                                                                           outlined=""
                                                                           dark=""
                                                                           class="elevation-0"
                                                                           height="40"
                                                                           @click.stop="downloadSampleFile()">
                                                                        <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                    </v-btn>
                                                                </v-col>
                                                                <v-col align-center justify-center>
                                                                    <v-file-input prepend-icon=""
                                                                                  label="Upload File"
                                                                                  outlined
                                                                                  dense
                                                                                  id="file"
                                                                                  v-model="fileInput"
                                                                                  prepend-inner-icon="mdi-file-upload-outline"
                                                                                  accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                  @change="handleFileUpload">
                                                                    </v-file-input>
                                                                </v-col>
                                                            </v-row>
                                                            <v-row v-if="csvfile.name">
                                                                <v-col align-center="" justify-center="">
                                                                    <v-chip id="AutoTest_IP659"
                                                                            close
                                                                            label
                                                                            color="white"
                                                                            @click:close="
                                        (csvfile.name = null),
                                          (csvfile.size = 0),
                                          (csvfile.namedisplay = null)
                                      ">
                                                                        <v-icon size="30"
                                                                                color="success"
                                                                                height="100">mdi-file-delimited-outline</v-icon>
                                                                        {{ fileInput.name }}
                                                                    </v-chip>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-text>
                                                    </v-card>
                                                </v-col>
                                            </v-row>
                                            <v-row class="my-0 py-0">
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                                    <Secondary-Button id="AutoTest_IP660"
                                                                      v-bind:disabled="fileInput == null"
                                                                      class="btn-122x36 mr-2"
                                                                      title="Confirm"
                                                                      @click.native="ImportRisk()"></Secondary-Button>
                                                    <Default-Button-Outlined id="AutoTest_IP661"
                                                                             class="btn-122x36 float-right"
                                                                             title="Cancel"
                                                                             @click.native="ImportRiskDialogBox = false"></Default-Button-Outlined>
                                                </v-col>
                                            </v-row>
                                        </v-container>
                                    </v-stepper-content>
                                </v-stepper-items>
                            </v-stepper>
                        </v-col>
                    </v-row>
                </v-container>
            </v-card-text>
        </v-card>
    </v-dialog>
    
  </v-card>
</template>
<script>
import AddButtonFab from "@/components/add-button-fab.vue";
import SuccessButtonMd from "@/components/success-button-md.vue";
import PrimaryButtonLight from "@/components/primary-button-light.vue";
import OutlinedButtonDark from "@/components/outlined-button-dark.vue";
import SuccessButton from "@/components/success-button.vue";
import PrimaryButton from "@/components/primary-button-normal.vue";
import BulkActionButtonOutlined from "@/components/bulk-action-button-outlined.vue";
import CancelButtonOutlinedSm from "@/components/cancel-button-outlined-sm.vue";
import PreviousButtonOutlined from "@/components/previous-button-outlined.vue";
import NextButtonPrimaryLight from "@/components/next-button-blue.vue";
import PrimaryButtonOutlined from "@/components/primary-button-outlined.vue";
import SecondaryButton from "@/components/secondary-button.vue";
import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
import PreLoader from "@/components/pre-loader.vue";

var objManageProject;
async function importscript() {
  return Promise.all([
    import("../../BL/ManageProject.js").then((mod) => {
      objManageProject = new mod.ManageProject();
    }),
  ]);
}
var objManageProjectActivity;
async function importscriptActivity() {
  return Promise.all([
    import("../../BL/ManageProjectActivity.js").then((mod) => {
      objManageProjectActivity = new mod.ManageProjectActivity();
    }),
  ]);
}
export default {
  data() {
    return {
      screenAccess: 0,
      RWAccess: 0,
      justifyCenter: "center",
      WorkspaceID: 0,
      ProjectId: 0,
      PreLoader: false,
      // Edit Variables //
      AddEditActivityDialogHeader: "",
      AddEditActivityHeader1: "",
      RiskDetailsTableheaders: [
        { text: "ID", align: " d-none", value: "riskId" },
          { text: "Name", align: "left", value: "RiskName", width: "130px" },
        { text: "Start date", value: "StartDate" },
        { text: "End Date", value: "EndDate" },
        { text: "Last Updated Date", value: "LastUpdatedDate" },
        //{ text: "Last Updated Value", value: "LastUpdatedValue" },
        { text: "Risk Type", value: "RiskType" },
        { text: "Risk Rate", value: "RiskRating" },
        { text: "Work Flow", value: "WorkflowName" },
        { text: "Users", value: "UserList" },
          { text: "Action", align: "center", value: "Actions", width: "100px" },
      ],
        RiskDetailsTableItems: [],
        FilterRiskDetailsTableItems: [],
        selectedItem: "",
        items: [
            { text: "This month" },
            { text: "This quarter" },
            { text: "This Year" },
        ],
        filterContent: false,
        dateRangeHeaderName: 'Date Range',
        dateRangeMenu: false,
        workflowMenu: false,
        assignedUserMenu: false,
        filterChipsList: [],
      CreateRisksDialog: false,
      RemoveRiskDialog: false,
      RemoveBulkRiskDialog: false,
      SearchRiskDetails: "",
      EditRiskDialog: false,
      AssignUserDialog: false,
      SelectRiskTypes: ["Financial", "Political", "Personal"],
      SelectRiskRating: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"],
      //SelectSampleWorkFlow :['Sample Workflow 1', 'Sample Workflow 2', 'Sample Workflow 3'],
      AssignAccountUserPopUp: false,
      offset: true,
      closeonClick: false,
      closeOnContentClick: false,
      SelectWorkflowDialog: false,
      SelectWorkflowDlgSteps: 0,
      searchUser: "",
      chipUsrItem: "",
      

      //From Date
      //startDate: new Date().toISOString().substr(0, 10),
      startDate: "",
      FrmDateMenu: false,

      // ToDate
      //endDate: new Date().toISOString().substr(0, 10),
      endDate: "",
      ToDateMenu: false,
      periodicityDate: new Date().toISOString().substr(0, 10),
      workflows: [],
      RiskName: "",
      Description: "",
      RiskType: "",
      RiskRating: "",
      RiskPeriodicity: "",
      Workflow: "",
      UserList: [],
      selectedRisk: [],
      riskId: "",
      riskName: "",
      GroupId: 0,
      selectedteamUsers: [],
      lstuserTableheaders: [
        { text: "ID", align: " d-none", value: "ID" },
        { text: "Name", align: "left", value: "Name" },
        { text: "User Role", value: "UserRole" },
      ],
      lstUsers: [],
      //Select workflow starts here
      SelectWorkflowDialog: false,
      SelectWorkflowDlgSteps: 0,
      SelectWorkflowDlgItems: [],
      checkboxAddWorkflow: [],
      selectedAddWorkflow: [],
      //step 2 of Select Workflow
      ViewWorkflowItem: { WfLevelDetails: [] },
      //Select workflow ends here

      //Import Risk
      ImportRiskDialogBox: false,
      ImportRiskSteps: 0,
      fileInput: null,
      csvfile: {
        name: "",
        size: 0,
        namedisplay: "",
      },
      csvfiledata: [],
      data: [
        [
          "Risk Name Here",
          "Financial/Political/Personal",
          "Risk description here",
          "1/2/3/4 etc",
          "Never/Once/Daily/Weekly etc",
          "Workflow Name here",
          "Start Date (dd/mm/yyyy) here",
          "End Date (dd/mm/yyyy) here",
        ],
      ],
      lstPeriodicity: [],
      lstMonthDate: [],
      lstMonth: [],
      lstWeek: [],
      ReportingFrequency: 0,
      ReportingMonthDay: 0,
      ReportingMonth: 0,
      ReportingMonthDateDay: 0,
      ReportingWeek: 0,
      confirmError: false,
      Description_icon: require("@/assets/ProjectWorkflow/Description_icon.svg"),
      Levels_icon: require("@/assets/ProjectWorkflow/Levels_icon.svg"),
      Workflow_Icon: require("@/assets/ProjectWorkflow/Workflow_Icon.svg"),
      Workflow_Tab_icon: require("@/assets/ProjectWorkflow/Workflow_Tab_icon.svg"),
      radioSelecIndex: "",
      rules: {
        required: (value) => !!value || "Required.",
      },
      periodicityDateMenu: false,
      Msg: "",
      successSnackbarMsg: false,
      errorSnackbarMsg: false,
      infoSnackbarMsg: false,
      ProjectStartDate: "",
      ProjectEndDate: "",
      closeOnClick: false,
        userimage: "",

        //Filtration
        FiterStartDate: "",
        FiterEndDate: "",
    };
  },

  components: {
    "Add-Button-Fab": AddButtonFab,
    "Success-Button-Md": SuccessButtonMd,
    "Primary-Button-Light": PrimaryButtonLight,
    "Outlined-Button-Dark": OutlinedButtonDark,
    "Primary-Button-Outlined": PrimaryButtonOutlined,
    "Primary-Button": PrimaryButton,
    "Bulk-Action-Button-Outlined": BulkActionButtonOutlined,
    "Cancel-Button-Outlined-Sm": CancelButtonOutlinedSm,
    "Previous-Button-Outlined": PreviousButtonOutlined,
    "Next-button-primary-light": NextButtonPrimaryLight,
    "Success-Button-Md": SuccessButtonMd,
    "Success-Button": SuccessButton,
    "Secondary-Button": SecondaryButton,
    "Default-Button-Outlined": DefaultButtonOutlined,
    "Pre-Loader": PreLoader,
  },

  computed: {
    FrmDate() {
      return this.formatDate(this.startDate);
    },
    ToDate() {
      return this.formatDate(this.endDate);
    },
    FormattedDate() {
      return this.formatDate(this.periodicityDate);
    },
  },
  mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
    this.userimage = window.SERVER_URL + "/Upload";
    this.screenAccess = this.$route.query.Access == 3 ? true : false;
    this.ProjectId = this.$route.query.ProjID;
    this.InitiativeId = this.$route.query.InitID;
    await importscript();
    await importscriptActivity();
    this.PreLoader = true;
    await this.fnGetWorkflows();
    await this.fnGetRiskByprjID();
    await this.fnGetGroupDetailsByprjID();
    await this.fnGetTeamUsersList();
    await this.GetReportingFrequencyList();
    await this.GetProjectDate();
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

    formatDate(date) {
      if (!date) return null;

      const [year, month, day] = date.split("-");
      return `${day}-${month}-${year}`;
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
    IsValidDate() {
      if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
        this.Msg = "Invalid Date : End date should be greater than Start Date";
          this.infoSnackbarMsg = true;
        this.endDate = "";
        return;
      }

      if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
        this.Msg =
          "Invalid Date : Start date should not be less than Project Start Date - " +
          this.formatDate(this.ProjectStartDate.substr(0, 10));
          this.infoSnackbarMsg = true;
        this.startDate = "";
        return;
      }
      if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
        this.Msg =
          "Invalid Date : Start date should not be greater than Project End Date - " +
          this.formatDate(this.ProjectEndDate.substr(0, 10));
          this.infoSnackbarMsg = true;
        this.startDate = "";
        return;
      }

      if (
        Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))
      ) {
        this.Msg =
          "Invalid Date : End date should not be greater than Project End Date - " +
          this.formatDate(this.ProjectEndDate.substr(0, 10));
          this.infoSnackbarMsg = true;
        this.endDate = "";
        return;
      }
      if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
        this.Msg =
          "Invalid Date : End date should be greater than Project Start Date (" +
          this.formatDate(this.ProjectStartDate.substr(0, 10)) +
          ")";
          this.infoSnackbarMsg = true;
        this.endDate = "";
        return;
      }
      if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
        this.Msg = "Invalid Date : Start Date and End date should not be same";
          this.infoSnackbarMsg = true;
        this.endDate = "";
        return;
      }
      return true;
    },
    IsValidPeriodicityDate() {
      if (this.startDate != null) {
        if (Date.parse(this.periodicityDate) < Date.parse(this.startDate)) {
          this.Msg =
            "Invalid Date : Report Frequency Date should be greater than Start Date";
            this.infoSnackbarMsg = true;
          this.periodicityDate = "";
          return;
        }
      } else {
        if (
          Date.parse(this.periodicityDate) < Date.parse(this.ProjectStartDate)
        ) {
          this.Msg =
            "Invalid Date : Report Frequency Date should be greater than Project Start Date - " +
            this.formatDate(this.ProjectStartDate.substr(0, 10));
            this.infoSnackbarMsg = true;
          this.periodicityDate = "";
          return;
        }
      }

      if (this.endDate != null) {
        if (Date.parse(this.periodicityDate) > Date.parse(this.endDate)) {
          this.Msg =
            "Invalid Date : Report Frequency Date should be less than End Date";
            this.infoSnackbarMsg= true;
          this.periodicityDate = "";
          return;
        }
      } else {
        if (
          Date.parse(this.periodicityDate) >
          Date.parse(this.ProjectEndDate.substr(0, 10))
        ) {
          this.Msg =
            "Invalid Date : Report Frequency Date should be less than Project End Date- " +
            this.formatDate(this.ProjectEndDate.substr(0, 10));
            this.infoSnackbarMsg = true;
          this.periodicityDate = "";
          return;
        }
      }
      return true;
    },
    resetValidation() {
      this.$refs.form.resetValidation();
    },
    async fnCreateRisk() {
      /*Chk Duplicate Name*/

      this.PreLoader = true;
      for (var i = 0; i < this.RiskDetailsTableItems.length; i++) {
        if (this.RiskDetailsTableItems[i].riskId != this.riskId) {
          if (this.RiskDetailsTableItems[i].RiskName == this.RiskName) {
            this.Msg = "Duplicate Entry - Same Risk Name existis..";
              this.infoSnackbarMsg = true;
            this.RiskName = "";
            return;
          }
        }
      }
      if (!this.IsValidDate()) {
        this.Msg = " Invalid Date ";
          this.infoSnackbarMsg = true;
        return;
      }
      if (this.ReportingFrequency == 2) {
        if (!this.IsValidPeriodicityDate()) {
          this.Msg = " Invalid PeriodicityDate ";
          this.periodicityDate = "";
            this.infoSnackbarMsg = true;
          return;
        }
      }
      var data = new FormData();
      data.append("riskId", this.riskId);
      data.append("RiskName", this.RiskName);
      data.append("Description", this.Description);
      data.append("RiskPeriodicity", this.ReportingFrequency);
      data.append("RiskType", this.RiskType);
      data.append("RiskRating", this.RiskRating);
      data.append("Startdate", this.startDate);
      data.append("Enddate", this.endDate);
      data.append("WorkflowID", this.Workflow);
      data.append("ProjectId", this.ProjectId);
      data.append("Users", JSON.stringify(this.selectedteamUsers));
      data.append("ReportingMonth", this.ReportingMonth);
      data.append("ReportingMonthDay", this.ReportingMonthDay);
      data.append("ReportingWeek", this.ReportingWeek);
      data.append("ReportingOnceDate", this.FormattedDate);
      data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);

      if (this.riskId != 0) {
        // Update //
        var result = await objManageProject.UpdateRisk(data);
      } else {
        // Create //
        var result = await objManageProject.CreateRisk(data);
      }

      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.status == "success") {
          this.Msg = "Risk details saved successfully";
          (this.CreateRisksDialog = false), (this.successSnackbarMsg = true);
        this.fnGetRiskByprjID();
        this.selectedteamUsers = [];
        this.CloseRiskDialog();
      } else {
        if (result.status == "rptfrequencyfailed") {
          this.Msg = "Reporting Frequency Failed";
        } else {
          this.Msg = "Failed";
        }
          this.errorSnackbarMsg = true;
      }
      this.PreLoader = false;
    },
    async fnAssignUserstoRisk() {
      var data = new FormData();
      data.append("riskId", this.riskId);
      data.append("ProjectId", this.ProjectId);
      data.append("Users", JSON.stringify(this.selectedteamUsers));

      var result = await objManageProject.AssignUserstoRisk(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.status == "success") {
          this.Msg = "User(s) successfully assigned to risk";
          this.successSnackbarMsg = true;
        this.fnGetRiskByprjID();
        this.selectedteamUsers = [];
      } else {
        this.Msg = "Failed";
          this.errorSnackbarMsg = true;
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
    async fnGetRiskByprjID() {
      var data = new FormData();
      data.append("ProjectId", this.ProjectId);
      var result = await objManageProject.GetRiskByprjID(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
          this.RiskDetailsTableItems = result.data;
          this.FilterRiskDetailsTableItems = result.data;
      }
    },

      fnClickFilterDone() {
          this.filterChipsList = [];

          this.FilterRiskDetailsTableItems = this.RiskDetailsTableItems;

          //date range filter
          if (this.FiterStartDate != '' && this.FiterEndDate != '') {

              this.filterChipsList.push({ ChipID: 777, ChipName: this.filterdateformat(this.FiterStartDate) + "-" + this.filterdateformat(this.FiterEndDate) });
              this.dateRangeHeaderName = this.filterdateformat(this.FiterStartDate) + "-" + this.filterdateformat(this.FiterEndDate);
              this.FilterRiskDetailsTableItems = this.FilterRiskDetailsTableItems.filter(x => x.StartDate.substr(0, 10) >= this.FiterStartDate && x.EndDate.substr(0, 10) <= this.FiterEndDate);
          } else {
              this.dateRangeHeaderName = "Date Range";
          }
      },

      fnChipClose(chipitem) {
          this.filterChipsList = this.filterChipsList.filter(x => x.ChipID != chipitem.ChipID);
          if (chipitem.ChipName == this.filterdateformat(this.FiterStartDate) + "-" + this.filterdateformat(this.FiterEndDate)) {
              this.FiterStartDate = "";
              this.FiterEndDate = "";
              this.dateRangeHeaderName = "Date Range";
          }
          this.fnClickFilterDone();
      },

      filterdateformat(date) {
          if (!date) return null;
          const [year, month, day] = date.substr(0, 10).split("-");
          if (month === "01") {
              return `${day} ${"Jan"}`;
          }
          if (month === "02") {
              return `${day} ${"Feb"}`;
          }
          if (month === "03") {
              return `${day} ${"Mar"}`;
          }
          if (month === "04") {
              return `${day} ${"Apr"}`;
          }
          if (month === "05") {
              return `${day} ${"May"}`;
          }
          if (month === "06") {
              return `${day} ${"Jun"}`;
          }
          if (month === "07") {
              return `${day} ${"Jul"}`;
          }
          if (month === "08") {
              return `${day} ${"Aug"}`;
          }
          if (month === "09") {
              return `${day} ${"Sep"}`;
          }
          if (month === "10") {
              return `${day} ${"Oct"}`;
          }
          if (month === "11") {
              return `${day} ${"Nov"}`;
          }
          if (month === "12") {
              return `${day} ${"Dec"}`;
          }
      },

    fnBulkRemoveClick() {
      if (this.selectedRisk.length > 0) {
        this.RemoveBulkRiskDialog = true;
        closeOnClick = true;
      } else {
        this.Msg = "Please select the risk";
          this.infoSnackbarMsg = true;
      }
    },
    showDetail(data, index) {
      this.SelectWorkflowDlgSteps = 2;
      this.ViewWorkflowItem = data;
      this.radioSelecIndex = index;
    },
    async fnDeleteRisk(Type) {
      var data = new FormData();
      if (Type == 1) {
        data.append("riskId", JSON.stringify(this.selectedRisk));
        data.append("Type", 1);
      } else {
        data.append("riskId", JSON.stringify(this.riskId));
        data.append("Type", 2);
      }
      var result = await objManageProject.DeleteRisk(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else {
        if ((result.status = "Success")) {
            this.RemoveRiskDialog = false;
            this.Msg = "Risk Successfully removed";
            this.successSnackbarMsg = true;
        } else {
          this.ErrorPaymentTermSuccessSnackbar = true;
        }
      }
      this.selectedRisk = [];
      this.fnGetRiskByprjID();
      this.riskId = 0;
    },
    async fnGetGroupDetailsByprjID() {
      var data = new FormData();
      data.append("ProjectId", this.ProjectId);
      var result = await objManageProject.GetGroupDetailsByprjID(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.GroupId = result.data.GroupId;
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
        this.lstUsers = result.data;
      }
    },
    async fnGetRiskByriskId(riskId) {
      this.selectedUsers = [];
      var data = new FormData();
      data.append("riskId", riskId);
      var result = await objManageProject.GetRiskByriskId(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.selectedteamUsers = JSON.parse(result.data.Users);
      }
    },
    fnCheckMandatory() {
      if (
        this.RiskName == "" ||
        this.Description == "" ||
        this.RiskType == "" ||
        this.RiskRating == "" ||
        this.ReportingFrequency == 0 ||
        (this.ReportingFrequency == 2 &&
          (this.periodicityDate == "" || this.periodicityDate == null)) ||
        this.startDate == "" ||
        this.endDate == "" ||
        this.selectedteamUsers == 0
      )
        return true;
      else return false;
    },
    CloseRiskDialog() {
      this.RiskName = "";
      this.Description = "";
      this.RiskType = "";
      this.RiskRating = "";
      this.Workflow = "";
      this.ReportingFrequency = 0;
      this.startDate = null;
      //new Date().toISOString().substr(0, 10);
      this.endDate = null;
      //new Date().toISOString().substr(0, 10);
      this.CreateRisksDialog = false;
      this.selectedteamUsers = [];
      this.resetValidation();
    },
    async GetReportingFrequencyList() {
      var result = await objManageProject.GetReportingFrequencyList();
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
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
    //*** Select Workflow functions starts here ***//

    async fnGetWorkflowsListByProjectID(item) {
      var data = new FormData();
      data.append("ProjectId", this.ProjectId);
      data.append("WorkspaceId", this.WorkspaceID);
      var result = await objManageProject.GetWorkflowsListByProjectID(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.SelectWorkflowDlgItems = result.data;
        this.filterWorkflowsList = result.data;
        var ID;
        var temp = "";

        for (ID = 0; ID <= this.SelectWorkflowDlgItems.length - 1; ID++) {
          if (this.SelectWorkflowDlgItems[ID].WorkflowID == item.WorkflowID) {
            temp = this.SelectWorkflowDlgItems.filter(
              (x) => x.WorkflowID == item.WorkflowID
            );
          }
          if (temp.length > 0) {
            this.selectedAddWorkflow.push({
              checkboxindex: ID,
              WFitem: temp[0],
            });
            this.checkboxAddWorkflow[ID] = true;
          }
          temp = [];
        }
      }
    },
    fnCheck(item, index) {
      var i;
      var preval = this.checkboxAddWorkflow[index];
      this.selectedAddWorkflow = [];
      for (i = 0; i <= this.SelectWorkflowDlgItems.length - 1; i++) {
        this.checkboxAddWorkflow[i] = false;
      }
      if (preval) {
        this.checkboxAddWorkflow[index] = true;
        this.selectedAddWorkflow.push({ checkboxindex: index, WFitem: item });
        this.confirmError = false;
      } else {
        if (this.selectedAddWorkflow.length > 0) {
          this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
            (x) => x.WFitem.WorkflowID != item.WorkflowID
          );
        }
      }
    },
    fnRemove(item) {
      this.checkboxAddWorkflow[item.checkboxindex] = false;
      this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
        (x) => x.WFitem.WorkflowID != item.WFitem.WorkflowID
      );
    },
    RemoveUser(item) {
      this.selectedteamUsers.splice(this.selectedteamUsers.indexOf(item), 1);
      this.selectedteamUsers = [...this.selectedteamUsers];
    },
    async fnAddSelectWorkflow() {
      var data = new FormData();
      data.append("WorkFlowID", this.selectedAddWorkflow[0].WFitem.WorkflowID);
      data.append("ProjectId", this.ProjectId);
      data.append("RiskId", this.riskId);

      var result = await objManageProject.UpdateWorkflow(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.status == "success") {
        this.Msg = "Workflow(s) successfully assigned to risk";
        this.successSnackbarMsg = true;
        this.SelectWorkflowDialog = false;
        this.SelectWorkflowDlgSteps = 1;
        this.SelectWorkflowDlgItems = [];
        this.checkboxAddWorkflow = [];
        this.selectedAddWorkflow = [];
        this.confirmError = false;
        this.fnGetRiskByprjID();
        //this.fnGetWorkflowsListByProjectID();
      }
    },
    //*** Select Workflow functions ends here ***//
    // workflow filter
    async fnFilterWorkflowBasedOnUser() {
      var ID = 0;
      var userId = 0;
      var WorkFlowID = 0;
      var WorkflowLevel = 0;
      var UserListID = 0;
      var find = false;
      var RemovingWorflow = [];
      for (
        WorkFlowID = 0;
        WorkFlowID <= this.SelectWorkflowDlgItems.length - 1;
        WorkFlowID++
      ) {
        for (
          WorkflowLevel = 0;
          WorkflowLevel <=
          this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails.length - 1;
          WorkflowLevel++
        ) {
          // level
          for (
            UserListID = 0;
            UserListID <=
            this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[
              WorkflowLevel
            ].usersList.length -
              1;
            UserListID++
          ) {
            // user
            ID = this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[
              WorkflowLevel
            ].usersList[UserListID].ID;
            for (
              userId = 0;
              userId <= this.WorkflowAssignUsersTableItems.length - 1;
              userId++
            ) {
              if (this.WorkflowAssignUsersTableItems[userId].ID != ID) {
                find = false;
              } else {
                find = true;
                break;
              }
            }
            if (find == false) {
              RemovingWorflow.push({
                WFID: this.SelectWorkflowDlgItems[WorkFlowID].WorkflowID,
              });
            }
          }
        }
      }
      if (RemovingWorflow.length > 0) {
        var WFID = 0;
        for (WFID = 0; WFID <= RemovingWorflow.length - 1; WFID++) {
          this.SelectWorkflowDlgItems = this.SelectWorkflowDlgItems.filter(
            (x) => x.WorkflowID != RemovingWorflow[WFID].WFID
          );
        }
      }
    },

    async ImportRisk() {
      var data = new FormData();

      data.append("ImportRiskList", JSON.stringify(this.csvfiledata));
      data.append("ProjectId", this.ProjectId);

      this.ImportInviteUserSteps = 3;

      var result = await objManageProject.ImportRisk(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else {
        if (result.status == "success") {
            this.fnGetRiskByprjID();
            this.Msg = "Risk Successfully imported";
            this.successSnackbarMsg = true;
          this.ImportRiskDialogBox = false;
        } else {
          this.Msg = "Wrong file / data ";
            this.errorSnackbarMsg = true;
        }
        this.csvfile.name = "";
        this.csvfile.size = "";
        this.csvfile.namedisplay = "";
        this.fileInput = [];
      }
    },

    handleFileUpload() {
      this.csvfile.name = this.fileInput.name;
      this.csvfile.size = this.fileInput.size;
      this.csvfile.namedisplay =
        this.csvfile.name +
        "\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0\xa0" +
        this.formatFileSize(this.csvfile.size, 0);

      this.fnCSVLoadParse();
    },
    formatFileSize(bytes, decimalPoint) {
      if (bytes == 0) return "0 Bytes";
      var k = 1000,
        dm = decimalPoint || 2,
        sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"],
        i = Math.floor(Math.log(bytes) / Math.log(k));
      return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + " " + sizes[i];
    },
    fnCSVLoadParse() {
      if (this.csvfile.name) {
        var vm = this;
        if (window.FileReader) {
          var reader = new FileReader();
          reader.readAsText(this.fileInput);
          // Handle errors load
          reader.onload = function (event) {
            var csv = event.target.result;
            vm.csvfiledata = vm.csvJSON(csv);
          };
          reader.onerror = function (evt) {
            if (evt.target.error.name == "NotReadableError") {
              this.Msg = "Cannot read file !";
                this.infoSnackbarMsg = true;
            }
          };
        } else {
          this.Msg = "FileReader is not supported in this browser.";
            this.infoSnackbarMsg = true;
        }
      }
    },
    csvJSON(csv) {
      var vm = this;

      vm.roleErrorCount = 0; //reset before loading the file datas
      var lines = csv.split(/\r\n|\n/);

      var result = [];

      var headers = lines[0].split(",");
      var reqHeading1 =
        "RiskName,RiskType,Description,RiskRating,ReportingFrequencyName,WorkflowName,StartDate,EndDate\n";
      var headers1 = reqHeading1.split(/\r\n|\n/)[0].split(",");

      if (headers1.length != headers.length) {
        this.Msg =
          "Wrong file selected ...columns are not matching .. Please check sample file format";
          this.infoSnackbarMsg = true;
        this.fileInput = [];
        this.csvfile.name = null;
        this.csvfile.size = 0;
        this.csvfile.namedisplay = null;
        return;
      }

      //console.log("headers = ", headers[0], headers[1]);
      for (var i = 1; i < lines.length; i++) {
        var obj = {};
        var errEmail = false;
        var currentline = lines[i].split(",");
        if (currentline != "") {
          for (var j = 0; j < headers.length; j++) {
            //if (!this.reg.test(currentline[0])) {
            //    errEmail = true;
            //    break;
            //}

            obj[headers[j]] = currentline[j];
          }
          result.push(obj);
        }
      }
      return result;
    },
    downloadSampleFile() {
      var csv =
        "RiskName,RiskType,Description,RiskRating,ReportingFrequencyName,WorkflowName,StartDate,EndDate\n";
      this.data.forEach(function (row) {
        csv += row.join(",");
        csv += "\n";
      });

      // console.log(Math.round(Math.random() * 1000));
      var hiddenElement = document.createElement("a");
      hiddenElement.href = "data:text/csv;charset=utf-8," + encodeURI(csv);
      hiddenElement.target = "_blank";
      hiddenElement.download =
        "sample_project_Risk_" + Math.round(Math.random() * 1000) + ".csv";
      hiddenElement.click();
    },

    async fnGetRiskByriskIdEdit(riskId) {
      var data = new FormData();
      data.append("riskId", riskId);
      var result = await objManageProject.GetRiskByriskId(data);

      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.riskId = result.data.riskId;
        this.RiskName = result.data.RiskName;
        this.Description = result.data.Description;
        this.startDate = result.data.StartDate.substr(0, 10);
        this.endDate = result.data.EndDate.substr(0, 10);
        this.ReportingFrequency = result.data.RiskPeriodicity;
        this.RiskType = result.data.RiskType;
        this.RiskRating = result.data.RiskRating.toString();
        this.Workflow = result.data.WorkflowID;
        this.selectedteamUsers = JSON.parse(result.data.Users);
      }
    },
  },
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
    .dialogHeight {
        height: 70vh;
    }

    .uploadBoxHeight {
        height: 50vh;
    }

    .uploadScrollHeight {
        height: 43vh;
    }

    @media screen and (max-width: 750px) {

        .dialogHeight {
            height: 85vh;
        }


        .uploadBoxHeight {
            height: 65vh;
        }

        .uploadScrollHeight {
            height: 59vh;
        }
    }

</style>
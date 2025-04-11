<template>
  <v-card class="elevation-0 " height="640">
    <v-container fluid>
      <v-stepper hide v-model="ProjectWorkflowSteps" class="elevation-0">
        <v-stepper-items>
          <!-- Workflows -->
          <v-stepper-content step="1" class="pa-0">
            <v-card-title style="margin-right:75px" class="pt-1 pb-4">
              <h3 class="text-left page-head">
                <v-avatar
                    color="white"
                    size="20"
                    class="elevation-1 mr-2 mb-1"
                  >
                    <img :src="Workflow_Tab_icon" style="width: 70%" />
                  </v-avatar>
                Payment Worklfow
              </h3>
              <v-spacer></v-spacer>
              <v-btn
                id="AutoTest_F226"
                height="39"
                width="160"
                color="success-btn"
                dark=""
                class="elevation-0 mr-4"
                @click="CreateWorkFlowDlg = true"
                v-bind:disabled="!screenAccess"
              >
                Add Workflow
              </v-btn>
              <!--Bulk Action -->
              <v-menu
                v-bind:disabled="checkboxWorkflow.length == 0"
                offset-y=""
                transition="scroll-y-transition"
                v-model="BulkActionMenu"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    height="39"
                    v-on="on"
                    color="grey"
                    outlined=""
                    class="elevation-0 mr-4"
                  >
                    Bulk Actions
                    <v-icon>mdi-chevron-down</v-icon>
                  </v-btn>
                </template>
                <v-list
                  color="white"
                  light=""
                  width="auto"
                  max-width="250"
                  class="action-button-list"
                >
                  <v-list-item
                    id="AutoTest_F227"
                    link=""
                    @click.stop="fnRemoveWorkflows()"
                  >
                    <v-list-item-icon>
                      <v-icon>mdi-trash-can-outline</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>Remove</v-list-item-content>
                  </v-list-item>
                </v-list>
              </v-menu>
              <!--Search-->
              <v-text-field
                label="Search"
                v-model="searchWorkflow"
                class="mr-4 table-search"
                dense
                prepend-inner-icon="mdi-magnify"
                outlined
                hide-details
              ></v-text-field>
              <!-- Sorting Menu-->
              <v-menu
                offset-y=""
                transition="scroll-y-transition"
                v-model="workflowSortMenu"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    min-width="40"
                    height="39"
                    v-on="on"
                    color="grey"
                    outlined=""
                    dark=""
                    class="elevation-0 mr-0 px-0"
                  >
                    <v-icon>mdi-tune-vertical</v-icon>
                  </v-btn>
                </template>
                <v-list
                  color="white"
                  light=""
                  width="auto"
                  max-width="250"
                  class="action-button-list"
                >
                  <v-list-item>
                    <v-list-item-content> Sort by : </v-list-item-content>
                  </v-list-item>
                  <v-list-item
                    id="AutoTest_F228"
                    @click.stop="fnWorkflowSort('Name-Ascending')"
                  >
                    <v-list-item-content> Name-Ascending </v-list-item-content>
                  </v-list-item>
                  <v-list-item
                    id="AutoTest_F229"
                    @click.stop="fnWorkflowSort('Name-Descending')"
                  >
                    <v-list-item-content> Name-Descending </v-list-item-content>
                  </v-list-item>
                  <v-list-item
                    id="AutoTest_F230"
                    @click.stop="fnWorkflowSort('Recently Added')"
                  >
                    <v-list-item-content> Recently Added </v-list-item-content>
                  </v-list-item>
                </v-list>
              </v-menu>
            </v-card-title>
            <!-- Workflow Card Section-->
            <!-- cards with workflows -->
             <v-row v-if="workflowsList.length > 0" class="pr-3">
              <v-col
                class="v-scrolling-div pt-0"
                style="height: 75vh"
                cols="12"
                xs="12"
                sm="12"
                md="12"
              >
                <v-row>
                  <v-col
                    class="pt-0"
                    xs="6"
                    sm="6"
                    md="3"
                    v-for="(WFitem, index) in filterWorkflowsList"
                    :key="WFitem.WorkflowID"
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
                          <div class="pt-6 px-2 text-limit">
                            <span style="font-size: 15px">
                              {{ WFitem.WfName }}</span
                            >
                          </div>
                        </v-col>
                        <v-col
                          xs="2"
                          sm="2"
                          md="2"
                          class="d-inline-flex pt-0 text-right"
                        >
                          <v-checkbox
                            color="success-check-box"
                            input-value="true"
                            v-model="checkboxWorkflow[index]"
                            label=""
                          ></v-checkbox>
                        </v-col>
                      </v-row>
                      <!--Description-->
                      <v-row>
                        <v-col class="py-0" :ripple="false">
                          <span class="heading-5 black--text">
                            <img :src="Description_icon" />
                            Description
                          </span>
                          <p style="height: 45px">
                            {{
                              WFitem.WfDescription.length >= 60
                                ? WFitem.WfDescription.substring(0, 60) + "..."
                                : WFitem.WfDescription
                            }}
                          </p>
                        </v-col>
                      </v-row>
                      <!-- Levels-->
                      <v-row>
                        <v-col class="py-0">
                          <span class="heading-5 black--text">
                            <img :src="Levels_icon" style="width: 3.5%" />
                            Levels : {{ WFitem.WfNoOfLevels }}
                          </span>
                        </v-col>
                      </v-row>
                      <!--Buttons-->
                      <v-row>
                        <v-col sm="7">
                          <v-btn
                            id="AutoTest_IP863"
                            color="success-btn elevation-0 enter-btn"
                            block
                            dark
                            @click="
                              fnSetWorkflowShowDetailsItem(WFitem),
                                (EditWorkflow.WorkflowID =
                                  WorkflowShowDetailsItem.WorkflowID),
                                (EditWorkflow.WfName =
                                  WorkflowShowDetailsItem.WfName),
                                (EditWorkflow.WfDescription =
                                  WorkflowShowDetailsItem.WfDescription),
                                (EditWorkflow.WfNoOfLevels =
                                  WorkflowShowDetailsItem.WfNoOfLevels),
                                (EditWorkflow.WfLevelDetails =
                                  WorkflowShowDetailsItem.WfLevelDetails),
                                (ProjectWorkflowSteps = 2),
                                (ActivityTab = 'tab-2'),
                                fnCheckEditWorkflowSaveButton()
                            "
                          >
                            View Workflow
                          </v-btn>
                        </v-col>
                        <v-col sm="5">
                          <v-btn
                            id="AutoTest_IP864"
                            color="normal elevation-0"
                            class="manage-btn"
                            block
                            @click="
                              fnSetWorkflowShowDetailsItem(WFitem),
                                (ProjectWorkflowSteps = 2),
                                (EditWorkflow.WorkflowID =
                                  WorkflowShowDetailsItem.WorkflowID),
                                (EditWorkflow.WfName =
                                  WorkflowShowDetailsItem.WfName),
                                (EditWorkflow.WfDescription =
                                  WorkflowShowDetailsItem.WfDescription),
                                (EditWorkflow.WfNoOfLevels =
                                  WorkflowShowDetailsItem.WfNoOfLevels),
                                (EditWorkflow.WfLevelDetails =
                                  WorkflowShowDetailsItem.WfLevelDetails),
                                (ProjectWorkflowSteps = 2),
                                (ActivityTab = 'tab-3')
                            "
                            v-bind:disabled="!screenAccess"
                          >
                            Manage
                          </v-btn>
                        </v-col>
                      </v-row>
                    </v-card>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
            <v-card-text v-if="workflowsList.length == 0">
              <v-col cols="12" sm="12" md="12">
                <v-row :justify="justifyCenter">
                  <v-col md="6" class="text-center">
                    <p class="mt-2 pt-2">
                      <img
                        src="..\..\..\src\assets\EmptyPlaceholders\project.svg"
                        height="80"
                        width="80"
                      />
                    </p>
                    <p class="heading-4-dark font-weight-medium pt-0">
                      No workflows available
                    </p>
                    <!--<v-btn id="AutoTest_F233" height="32" color="success-btn" dark class="elevation-0 mr-2" @click="CreateWorkFlowDlg = true" v-bind:disabled="!screenAccess">
                                            Add Workflow
                                        </v-btn>-->
                  </v-col>
                </v-row>
              </v-col>
            </v-card-text>
          </v-stepper-content>

          <!-- Workflow Show Details -->
                <!-- Workflow Show Details -->
          <v-stepper-content step="2" class="pa-0">
            <!-- Workflow Header -->
            <v-row>
              <v-col>
                 <h3 class="text-left page-head">
                  <v-avatar
                    color="white"
                    size="20"
                    class="elevation-1 mr-2 mb-1"
                  >
                    <img :src="Workflow_Tab_icon" style="width: 70%" />
                  </v-avatar>
                 <span @click="
                      fnResetWorkflowShowDetailsItem(),
                        (ProjectWorkflowSteps = 1)
                    "> Workflow</span> > {{EditWorkflow.WfName}}
                </h3>
              </v-col>
            </v-row>
            <v-row no-gutters :justify="justifyCenter">
              <v-col xs="6" sm="6" md="6">
                <v-card flat>
                  <v-tabs
                    centered
                    v-model="ActivityTab"
                    color="tab-active-line"
                  >
                    <v-tab href="#tab-3"> Details </v-tab>
                    <v-tab
                      href="#tab-2"
                      @click="
                        (EditWorkflow.WorkflowID =
                          WorkflowShowDetailsItem.WorkflowID),
                          (EditWorkflow.WfName =
                            WorkflowShowDetailsItem.WfName),
                          (EditWorkflow.WfDescription =
                            WorkflowShowDetailsItem.WfDescription),
                          (EditWorkflow.WfNoOfLevels =
                            WorkflowShowDetailsItem.WfNoOfLevels),
                          (EditWorkflow.WfLevelDetails =
                            WorkflowShowDetailsItem.WfLevelDetails),
                          fnCheckEditWorkflowSaveButton()
                      "
                      >Levels</v-tab
                    >

                    <v-tab-item value="tab-3" >
                      <template v-if="EditWorkflowDetailsEdit == false">
                        <v-row class="mt-8">
                          <v-col style="text-align: end" cols="12" sm="12">
                            <v-btn
                              color="#439bde"
                              style="color: white"
                              @click="
                                (EditWorkflow.WorkflowID =
                                  WorkflowShowDetailsItem.WorkflowID),
                                  (EditWorkflow.WfName =
                                    WorkflowShowDetailsItem.WfName),
                                  (EditWorkflow.WfDescription =
                                    WorkflowShowDetailsItem.WfDescription),
                                  (EditWorkflowDetailsEdit = true)
                              "
                              v-bind:disabled="!screenAccess"
                              >Edit Details</v-btn
                            >
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col class="py-0">
                            <h3 class="font-weight-medium">
                              Workflow Name
                              <!-- <v-btn
                              id="AutoTest_IP868"
                              text
                              max-width="13"
                              min-width="13"
                              height="13"
                              @click="
                                (EditWorkflow.WorkflowID =
                                  WorkflowShowDetailsItem.WorkflowID),
                                  (EditWorkflow.WfName =
                                    WorkflowShowDetailsItem.WfName),
                                  (EditWorkflow.WfDescription =
                                    WorkflowShowDetailsItem.WfDescription),
                                  (ProjectWorkflowSteps = 3)
                              "
                              v-bind:disabled="!screenAccess"
                            >
                              <v-icon size="13" color="#3374B9"
                                >mdi-pencil-outline</v-icon
                              >
                            </v-btn> -->
                            </h3>

                            <p class="text--secondary">
                              {{ WorkflowShowDetailsItem.WfName }}
                            </p>
                          </v-col>
                        </v-row>
                        <!--Description-->
                        <v-row>
                          <v-col class="py-0">
                            <h3 class="font-weight-medium">
                              Description
                              <!-- <v-btn
                              id="AutoTest_IP869"
                              text
                              max-width="13"
                              min-width="13"
                              height="13"
                              @click="
                                (EditWorkflow.WorkflowID =
                                  WorkflowShowDetailsItem.WorkflowID),
                                  (EditWorkflow.WfName =
                                    WorkflowShowDetailsItem.WfName),
                                  (EditWorkflow.WfDescription =
                                    WorkflowShowDetailsItem.WfDescription),
                                  (ProjectWorkflowSteps = 3)
                              "
                              v-bind:disabled="!screenAccess"
                            >
                              <v-icon size="13" color="#3374B9"
                                >mdi-pencil-outline</v-icon
                              >
                            </v-btn> -->
                            </h3>

                            <p
                              class="text--secondary v-scrolling-div"
                              style="max-height: 100px"
                            >
                              {{ WorkflowShowDetailsItem.WfDescription }}
                            </p>
                          </v-col>
                        </v-row>
                        <!-- Levels-->
                        <v-row>
                          <v-col class="py-0">
                            <h3 class="font-weight-medium text--secondary">
                              <v-icon size="12">mdi-stairs</v-icon>
                              Levels :
                              {{ WorkflowShowDetailsItem.WfNoOfLevels }}
                              <v-btn
                                id="AutoTest_IP870"
                                text
                                max-width="13"
                                min-width="13"
                                height="13"
                                @click="
                                  (EditWorkflow.WorkflowID =
                                    WorkflowShowDetailsItem.WorkflowID),
                                    (EditWorkflow.WfName =
                                      WorkflowShowDetailsItem.WfName),
                                    (EditWorkflow.WfDescription =
                                      WorkflowShowDetailsItem.WfDescription),
                                    (EditWorkflow.WfNoOfLevels =
                                      WorkflowShowDetailsItem.WfNoOfLevels),
                                    (EditWorkflow.WfLevelDetails =
                                      WorkflowShowDetailsItem.WfLevelDetails),
                                    fnCheckEditWorkflowSaveButton(),
                                    (ActivityTab = 'tab-2')
                                "
                                v-bind:disabled="!screenAccess"
                              >
                                <v-icon size="13" color="#3374B9"
                                  >mdi-pencil-outline</v-icon
                                >
                              </v-btn>
                            </h3>
                          </v-col>
                        </v-row>
                      </template>
                      <template v-else>
                        <v-form
                          ref="entryForm"
                          @submit.prevent="editSubmitHandler"
                        >
                          <v-row>
                            <v-col
                              cols="12"
                              xs="12"
                              sm="12"
                              md="12"
                              style="text-align: end"
                              class="py-0 mb-5 mt-10"
                            >
                              <v-btn
                                class="btn-122x36 mr-2 mt-1"
                                id="AutoTest_IP871"
                                color="success-btn"
                                depressed
                                dark
                                title="Save"
                                @click.native="editSubmitHandler()"
                                v-bind:disabled="!screenAccess"
                                >Save</v-btn
                              >
                              <Default-Button-Outlined
                                id="AutoTest_IP874"
                                class="btn-122x36 mt-1"
                                title="Cancel"
                                @click.native="
                                  (EditWorkflow.WorkflowID = 0),
                                    (EditWorkflow.WfName = ''),
                                    (EditWorkflow.WfDescription = ''),
                                    (ProjectWorkflowSteps = 2),
                                    (editRules = []),
                                    (EditWorkflowDetailsEdit = false)
                                "
                              ></Default-Button-Outlined>
                            </v-col>
                          </v-row>
                          <v-row>
                            <!-- Edit Workflow details-->
                            <v-row>
                              <v-col
                                cols="12"
                                xs="12"
                                sm="12"
                                md="12"
                                class="pt-2"
                              >
                                <v-row :justify="justifyCenter">
                                  <v-col md="12">
                                    <v-text-field
                                      outlined
                                      dense
                                      label="Workflow Name"
                                      type="text"
                                      :rules="editRules.name"
                                      v-model="EditWorkflow.WfName"
                                    >
                                    </v-text-field>
                                    <v-textarea
                                      rows="4"
                                      outlined
                                      dense
                                      label="Description"
                                      type="text"
                                      :rules="editRules.desc"
                                      v-model="EditWorkflow.WfDescription"
                                    >
                                    </v-textarea>
                                  </v-col>
                                </v-row>
                              </v-col>
                            </v-row>
                          </v-row>
                        </v-form>
                      </template>
                    </v-tab-item>
                    <v-tab-item value="tab-2">
                      <v-row v-if="editLevel == true">
                        <v-col
                          cols="12"
                          xs="6"
                          sm="6"
                          md="12"
                          class="text-right mr-4 pr-4 pt-10"
                        >
                          <span
                            style="font-size: 13px; color: red"
                            class="mr-4"
                            v-if="EditWorkflowNoOfLevelsError == true"
                            >Minimum 1 level is required for a workflow</span
                          >
                          <v-btn
                            id="AutoTest_IP876"
                            color="success-btn elevation-0 enter-btn"
                            dark
                            class="btn-122x36 mr-2"
                            @click="fnAddLevelEditWorkflow()"
                            v-bind:disabled="!screenAccess"
                          >
                            Add Level
                          </v-btn>
                          <v-btn
                            id="AutoTest_IP876"
                            color="success-btn elevation-0 enter-btn"
                            dark
                            class="btn-122x36 mr-2"
                            @click.native="fncheckAssignIsEditLevelNotEmpty(),
                                (ProjectWorkflowSteps = 2)
                            "
                          >
                            Save
                          </v-btn>
                          <Default-Button-Outlined
                            id="AutoTest_IP881"
                            class="btn-122x36"
                            title="Cancel"
                            @click.native="levePrevData(), (editLevel = false)"
                          ></Default-Button-Outlined>
                        </v-col>

                        <!-- Workflow Level Edit card -->
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-center pt-1"
                        >
                          <v-card class="elevation-0">
                            <v-card-text class="py-0 pl-0">
                              <v-row :justify="justifyCenter">
                                <v-col cols="12" md="12" class="pt-0">
                                  <v-container fluid class="pt-0 pl-0">
                                    <v-row>
                                      <v-col
                                        cols="12"
                                        md="12"
                                        class="v-scrolling-div"
                                        style="height: 50vh"
                                      >
                                        <v-col
                                          md="12"
                                          v-for="i in EditWorkflow.WfNoOfLevels"
                                          :key="i"
                                        >
                                          <v-card outlined flat>
                                            <v-row>
                                              <v-row :justify="justifyCenter">
                                                <v-col
                                                  cols="12"
                                                  md="12"
                                                  class="pt-0"
                                                >
                                                  <v-row>
                                                    <v-col
                                                      cols="7"
                                                      sm="7"
                                                      v-if="
                                                        editLevelNameButton ==
                                                          false ||
                                                        editLevelNameIndex != i
                                                      "
                                                      style="text-align: start"
                                                      class="pb-1 pt-2 pl-9"
                                                    >
                                                      {{
                                                        EditWfNoOfLevelName[
                                                          i - 1
                                                        ]
                                                      }}
                                                    </v-col>
                                                    <v-col
                                                      cols="7"
                                                      style="text-align: center"
                                                      sm="7"
                                                      v-if="
                                                        editLevelNameButton ==
                                                          true &&
                                                        editLevelNameIndex == i
                                                      "
                                                      class="pb-1 pt-2 pl-8"
                                                    >
                                                      <input
                                                        class="edit-level-text-field"
                                                        maxlength="20"
                                                        v-model="newLevelName"
                                                        placeholder="Edit level Name"
                                                      />
                                                    </v-col>
                                                    <v-col
                                                      cols="5"
                                                      style="text-align: end"
                                                      sm="5"
                                                      class="pb-1 pr-5 pt-1"
                                                    >
                                                      <v-btn
                                                        elevation="0"
                                                        style="
                                                          border-radius: 3px;
                                                        "
                                                        color="#EEEEEE"
                                                        icon
                                                        v-if="
                                                          editLevelNameButton ==
                                                            false ||
                                                          editLevelNameIndex !=
                                                            i
                                                        "
                                                        height="30"
                                                        @click="
                                                          editManageLevelNameHandler(
                                                            i
                                                          )
                                                        "
                                                      >
                                                        <v-icon
                                                          style="
                                                            font-size: 20px;
                                                          "
                                                          color="black"
                                                          >mdi-pencil-outline</v-icon
                                                        >
                                                      </v-btn>
                                                      <v-btn
                                                        elevation="0"
                                                        style="
                                                          border-radius: 3px;
                                                        "
                                                        color="#EEEEEE"
                                                        v-if="
                                                          editLevelNameButton ==
                                                            true &&
                                                          editLevelNameIndex ==
                                                            i
                                                        "
                                                        icon
                                                        height="30"
                                                        @click="
                                                          editLevelManageSaveNameHandler(
                                                            i - 1
                                                          )
                                                        "
                                                      >
                                                        <v-icon
                                                          style="
                                                            font-size: 20px;
                                                          "
                                                          color="black"
                                                          >mdi-content-save-outline</v-icon
                                                        >
                                                      </v-btn>
                                                    </v-col>
                                                  </v-row>
                                                  <v-col
                                                    cols="12"
                                                    sm="12"
                                                    class="pa-0 pl-3"
                                                  >
                                                    <v-divider></v-divider>
                                                  </v-col>
                                                  <v-row>
                                                    <v-col
                                                      sm="12"
                                                      style="text-align: center"
                                                    >
                                                      <v-progress-circular
                                                        :rotate="360"
                                                        :size="75"
                                                        :width="10"
                                                        :value="
                                                          (100 /
                                                            EditWorkflow.WfNoOfLevels) *
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
                                                    </v-col>
                                                  </v-row>
                                                </v-col>
                                              </v-row>
                                              <v-divider vertical></v-divider>
                                              <v-col
                                                cols="6"
                                                md="6"
                                                class="text-left pl-0"
                                              >
                                                <v-row no-gutters>
                                                  <v-col
                                                    cols="12"
                                                    md="12"
                                                    class="text-center"
                                                  >
                                                    <h4 class="heading-4">
                                                      <v-icon
                                                        size="16"
                                                        class="mr-2"
                                                        >mdi-account</v-icon
                                                      >Users
                                                      <span class="float-right">
                                                        <v-btn
                                                          id="AutoTest_IP877"
                                                          text
                                                          small
                                                          max-height="20"
                                                          max-width="20"
                                                          min-width="20"
                                                          @click="
                                                            EditWorkflow.WfNoOfLevels >
                                                            1
                                                              ? fnDeleteLevelEditWorkflow(
                                                                  i
                                                                )
                                                              : fnDeleteLevelEditWorkflowError()
                                                          "
                                                          v-bind:disabled="
                                                            !screenAccess
                                                          "
                                                        >
                                                          <v-icon
                                                            color="#707070"
                                                            size="15"
                                                            >mdi-close</v-icon
                                                          >
                                                        </v-btn>
                                                      </span>
                                                    </h4>
                                                    <v-divider
                                                      class="mt-1"
                                                    ></v-divider>
                                                  </v-col>
                                                </v-row>
                                                <!---   if there is no data-->
                                                <v-row
                                                  v-if="
                                                    EditWorkflow.WfLevelDetails
                                                      .length == 0 ||
                                                    EditWorkflow.WfLevelDetails[
                                                      i - 1
                                                    ].usersList.length == 0
                                                  "
                                                  no-gutters
                                                >
                                                  <v-col
                                                    cols="12"
                                                    md="12"
                                                    class="pt-2 mb-0 text-center"
                                                  >
                                                   <span v-if="EditAssignError == false">
                                                      <p class="heading-1" >
                                                      No Users Added <span style="color:red;">*</span>
                                                                                                        </p>
                                                   </span>
                                                   <span v-else>
                                                      <p class="heading-1" style="color:red;" >
                                                      No Users Added <span style="color:red;">*</span>
                                                                                                        </p>
                                                   </span>
                                                  </v-col>
                                                </v-row>
                                                <!-- if User Assigned-->
                                                <v-row
                                                  v-if="
                                                    EditWorkflow.WfLevelDetails
                                                      .length > 0 &&
                                                    EditWorkflow.WfLevelDetails[
                                                      i - 1
                                                    ].usersList.length > 0
                                                  "
                                                  no-gutters
                                                >
                                                  <v-col cols="12" md="12">
                                                    <v-col
                                                      class="text-center mb-0 pt-1"
                                                    >
                                                      <!--<v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length >= 1
                                              "
                                              size="30"
                                              class="team-avatar"
                                            >
                                              <img
                                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                class="team-image"
                                                alt="Team name"
                                              />
                                            </v-avatar>
                                            <v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length >= 2
                                              "
                                              size="30"
                                              class="team-avatar"
                                            >
                                              <img
                                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                class="team-image"
                                                alt="Team name"
                                              />
                                            </v-avatar>
                                            <v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length > 2
                                              "
                                              size="30"
                                              class="team-count"
                                              color="red"
                                            >
                                              +{{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length
                                              }}
                                            </v-avatar>-->
                                                      <v-avatar
                                                        size="25"
                                                        class="team-avatar"
                                                        v-for="(
                                                          item, key
                                                        ) in EditWorkflow.WfLevelDetails[
                                                          i - 1
                                                        ].usersList.slice(0, 3)"
                                                        v-bind:key="item.ID"
                                                        :color="`${getcolor(
                                                          item.Name
                                                        )}`"
                                                      >
                                                        <img
                                                          v-bind:src="
                                                            userimage +
                                                            item.ProfileImage
                                                          "
                                                          alt="User DP"
                                                          v-if="
                                                            item.ProfileImage
                                                              .length > 0
                                                          "
                                                        />
                                                        <span
                                                          class="white--text"
                                                          v-else
                                                          >{{
                                                            item.Name == null
                                                              ? ""
                                                              : item.Name.substring(
                                                                  0,
                                                                  1
                                                                ).toUpperCase()
                                                          }}</span
                                                        >
                                                      </v-avatar>
                                                      <v-avatar
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length >=
                                                          4
                                                        "
                                                        size="25"
                                                        class="team-count"
                                                      >
                                                        +{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length - 3
                                                        }}
                                                      </v-avatar>
                                                    </v-col>
                                                    <v-col
                                                      class="text-center mb-0 pt-0"
                                                    >
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length ==
                                                          1
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }}
                                                      </p>
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length ==
                                                          2
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }},{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[1].Name
                                                        }}
                                                      </p>
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length > 2
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }},{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[1].Name
                                                        }}
                                                        +
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length
                                                        }}
                                                      </p>
                                                    </v-col>
                                                  </v-col>
                                                </v-row>
                                                <!-- Assign Or Reassign Button-->
                                                <v-row no-gutters>
                                                  <v-col
                                                    v-if="
                                                      EditWorkflow
                                                        .WfLevelDetails.length >
                                                        0 &&
                                                      EditWorkflow
                                                        .WfLevelDetails[i - 1]
                                                        .usersList.length == 0
                                                    "
                                                    cols="12"
                                                    md="12"
                                                    class="text-center pt-0 mb-0"
                                                  >
                                                    <v-btn
                                                      id="AutoTest_IP878"
                                                      color="success-btn elevation-0 enter-btn"
                                                      dark
                                                      @click="
                                                        fnEditWorkflowAssignClick(
                                                          i
                                                        )
                                                      "
                                                      v-bind:disabled="
                                                        !screenAccess
                                                      "
                                                    >
                                                      Assign
                                                    </v-btn>
                                                  </v-col>
                                                  <v-col
                                                    v-else
                                                    cols="12"
                                                    md="12"
                                                    class="text-center pt-0 mb-0"
                                                  >
                                                    <v-btn
                                                      id="AutoTest_IP879"
                                                      color="success-btn elevation-0 enter-btn"
                                                      dark
                                                      @click="
                                                        fnEditWorkflowAssignClick(
                                                          i
                                                        )
                                                      "
                                                      v-bind:disabled="
                                                        !screenAccess
                                                      "
                                                    >
                                                      Re-Assign
                                                    </v-btn>
                                                  </v-col>
                                                </v-row>
                                              </v-col>
                                            </v-row>
                                          </v-card>
                                        </v-col>
                                      </v-col>
                                    </v-row>
                                  </v-container>
                                </v-col>
                              </v-row>
                            </v-card-text>
                          </v-card>
                        </v-col>
                      </v-row>

                      <!-- Edit Workflow Assign User Dailog Box-->
                      <v-dialog v-model="EditWorkflowAssignDialog" width="900">
                        <v-col cols="12" md="12" class="pt-0 mb-0">
                          <v-card>
                            <v-card-title
                              primary-title
                              class="page-head pb-0 text-center"
                            >
                              Assign Users
                              <v-spacer></v-spacer>
                              <v-btn
                                id="AutoTest_IP882"
                                text
                                @click="EditWorkflowAssignDialog = false, EditWorkflowAssignLevelClicked = 0, editWorkflowAssignUsersSelected = []"
                                v-bind:disabled="!screenAccess"
                              >
                                <v-icon color="#707070" size="15"
                                  >mdi-close</v-icon
                                >
                              </v-btn>
                            </v-card-title>
                            <v-row>
                              <!-- Users-->
                              <v-col
                                cols="12"
                                md="12"
                                sm="12"
                                xs="12"
                                class="px-4"
                              >
                                <v-data-table
                                  :headers="WorkflowAssignUsersTableHeaders"
                                  :items="WorkflowAssignUsersTableItems"
                                  item-key="ID"
                                  sort-by="Name"
                                  show-select
                                  class="elevation-1"
                                  fixed-header
                                  height="40vh"
                                  v-model="editWorkflowAssignUsersSelected"
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
                                              v-bind:src="
                                                userimage + item.ProfileImage
                                              "
                                              alt="User DP"
                                              v-if="
                                                item.ProfileImage.length > 0
                                              "
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
                            </v-row>
                            <v-row>
                              <v-col
                                cols="12"
                                xs="12"
                                sm="12"
                                md="12"
                                class="text-center py-4 mb-4"
                              >
                                <Secondary-Button
                                  id="AutoTest_IP883"
                                  class="btn-122x36 mr-2"
                                  title="Save"
                                  @click.native="fnEditWorkflowAssignSave()"
                                  v-bind:disabled="
                                    editWorkflowAssignUsersSelected.length == 0
                                  "
                                ></Secondary-Button>
                                <Default-Button-Outlined
                                  id="AutoTest_IP884"
                                  class="btn-122x36"
                                  title="Cancel"
                                  @click.native="EditWorkflowAssignDialog = false, EditWorkflowAssignLevelClicked = 0, editWorkflowAssignUsersSelected = []"
                                ></Default-Button-Outlined>
                              </v-col>
                            </v-row>
                          </v-card>
                        </v-col>
                      </v-dialog>
                      <v-row v-if="editLevel == false">
                        <v-col
                          cols="12"
                          xs="6"
                          sm="6"
                          md="12"
                          class="text-right mr-4 pr-4 pt-10"
                        >
                          <v-btn
                            id="AutoTest_IP876"
                            dark
                            color="#439bde"
                            style="color: white"
                            @click="editLevel = true"
                          >
                            Edit Level
                          </v-btn>
                        </v-col>

                        <!-- Workflow Level Edit card -->
                        <v-col
                          cols="12"
                          xs="12"
                          sm="12"
                          md="12"
                          class="text-center pt-1 pl-0"
                        >
                          <v-card class="elevation-0">
                            <v-card-text class="py-0">
                              <v-row :justify="justifyCenter">
                                <v-col cols="12" md="12" class="pt-0 pl-0">
                                  <v-container fluid class="pt-0">
                                    <v-row>
                                      <v-col
                                        cols="12"
                                        md="12"
                                        class="v-scrolling-div"
                                        style="height: 50vh"
                                      >
                                        <v-col
                                          md="12"
                                          v-for="i in EditWorkflow.WfNoOfLevels"
                                          :key="i"
                                        >
                                          <v-card outlined flat>
                                            <v-row>
                                              <v-row :justify="justifyCenter">
                                                <v-col
                                                  class="pt-0"
                                                  cols="12"
                                                  md="12"
                                                >
                                                  <v-row>
                                                    <!-- input level name -->
                                                    <v-col
                                                      cols="12"
                                                      sm="12"
                                                      style="text-align: start"
                                                      class="pb-2 pt-2 pl-9"
                                                    >
                                                      <span>{{
                                                        EditWfNoOfLevelName[
                                                          i - 1
                                                        ]
                                                      }}</span>
                                                    </v-col>
                                                  </v-row>
                                                  <v-col
                                                    cols="12"
                                                    sm="12"
                                                    class="pa-0 pl-3"
                                                  >
                                                    <v-divider></v-divider>
                                                  </v-col>
                                                  <v-row>
                                                    <v-col
                                                      sm="12"
                                                      style="text-align: center"
                                                    >
                                                      <v-progress-circular
                                                        :rotate="360"
                                                        :size="75"
                                                        :width="10"
                                                        :value="
                                                          (100 /
                                                            EditWorkflow.WfNoOfLevels) *
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
                                                    </v-col>
                                                  </v-row>
                                                </v-col>
                                              </v-row>
                                              <v-divider vertical></v-divider>
                                              <v-col
                                                cols="6"
                                                md="6"
                                                class="text-left pl-0"
                                              >
                                                <v-row no-gutters>
                                                  <v-col
                                                    cols="12"
                                                    md="12"
                                                    class="text-center"
                                                  >
                                                    <h4 class="heading-4">
                                                      <v-icon
                                                        size="16"
                                                        class="mr-2"
                                                        >mdi-account</v-icon
                                                      >Users
                                                    </h4>
                                                    <v-divider
                                                      class="mt-1"
                                                    ></v-divider>
                                                  </v-col>
                                                </v-row>
                                                <!---   if there is no data-->
                                                <v-row
                                                  v-if="
                                                    EditWorkflow.WfLevelDetails
                                                      .length == 0 ||
                                                    EditWorkflow.WfLevelDetails[
                                                      i - 1
                                                    ].usersList.length == 0
                                                  "
                                                  no-gutters
                                                >
                                                  <v-col
                                                    cols="12"
                                                    md="12"
                                                    class="pt-2 mb-0 text-center"
                                                  >
                                                    <p class="heading-1">
                                                      No Users Added
                                                    </p>
                                                  </v-col>
                                                </v-row>
                                                <!-- if User Assigned-->
                                                <v-row
                                                  v-if="
                                                    EditWorkflow.WfLevelDetails
                                                      .length > 0 &&
                                                    EditWorkflow.WfLevelDetails[
                                                      i - 1
                                                    ].usersList.length > 0
                                                  "
                                                  no-gutters
                                                >
                                                  <v-col cols="12" md="12">
                                                    <v-col
                                                      class="text-center mb-0 pt-1"
                                                    >
                                                      <!--<v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length >= 1
                                              "
                                              size="30"
                                              class="team-avatar"
                                            >
                                              <img
                                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                class="team-image"
                                                alt="Team name"
                                              />
                                            </v-avatar>
                                            <v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length >= 2
                                              "
                                              size="30"
                                              class="team-avatar"
                                            >
                                              <img
                                                src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                class="team-image"
                                                alt="Team name"
                                              />
                                            </v-avatar>
                                            <v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length > 2
                                              "
                                              size="30"
                                              class="team-count"
                                              color="red"
                                            >
                                              +{{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length
                                              }}
                                            </v-avatar>-->
                                                      <v-avatar
                                                        size="25"
                                                        class="team-avatar"
                                                        v-for="(
                                                          item, key
                                                        ) in EditWorkflow.WfLevelDetails[
                                                          i - 1
                                                        ].usersList.slice(0, 3)"
                                                        v-bind:key="item.ID"
                                                        :color="`${getcolor(
                                                          item.Name
                                                        )}`"
                                                      >
                                                        <img
                                                          v-bind:src="
                                                            userimage +
                                                            item.ProfileImage
                                                          "
                                                          alt="User DP"
                                                          v-if="
                                                            item.ProfileImage
                                                              .length > 0
                                                          "
                                                        />
                                                        <span
                                                          class="white--text"
                                                          v-else
                                                          >{{
                                                            item.Name == null
                                                              ? ""
                                                              : item.Name.substring(
                                                                  0,
                                                                  1
                                                                ).toUpperCase()
                                                          }}</span
                                                        >
                                                      </v-avatar>
                                                      <v-avatar
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length >=
                                                          4
                                                        "
                                                        size="25"
                                                        class="team-count"
                                                      >
                                                        +{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length - 3
                                                        }}
                                                      </v-avatar>
                                                    </v-col>
                                                    <v-col
                                                      class="text-center mb-0 pt-0"
                                                    >
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length ==
                                                          1
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }}
                                                      </p>
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length ==
                                                          2
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }},{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[1].Name
                                                        }}
                                                      </p>
                                                      <p
                                                        v-if="
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length > 2
                                                        "
                                                      >
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[0].Name
                                                        }},{{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList[1].Name
                                                        }}
                                                        +
                                                        {{
                                                          EditWorkflow
                                                            .WfLevelDetails[
                                                            i - 1
                                                          ].usersList.length
                                                        }}
                                                      </p>
                                                    </v-col>
                                                  </v-col>
                                                </v-row>
                                                <!-- Assign Or Reassign Button-->
                                                <v-row no-gutters>
                                                  <v-col
                                                    v-if="
                                                      EditWorkflow
                                                        .WfLevelDetails.length >
                                                        0 &&
                                                      EditWorkflow
                                                        .WfLevelDetails[i - 1]
                                                        .usersList.length == 0
                                                    "
                                                    cols="12"
                                                    md="12"
                                                    class="text-center pt-0 mb-0"
                                                  >
                                                    <v-btn
                                                      id="AutoTest_IP878"
                                                      color="success-btn elevation-0 enter-btn"
                                                      dark
                                                      @click="
                                                        fnEditWorkflowAssignClick(
                                                          i
                                                        )
                                                      "
                                                      v-bind:disabled="
                                                        !screenAccess
                                                      "
                                                    >
                                                      Assign
                                                    </v-btn>
                                                  </v-col>
                                                </v-row>
                                              </v-col>
                                            </v-row>
                                          </v-card>
                                        </v-col>
                                      </v-col>
                                    </v-row>
                                  </v-container>
                                </v-col>
                              </v-row>
                            </v-card-text>
                          </v-card>
                        </v-col>
                      </v-row>
                    </v-tab-item>
                  </v-tabs>
                  <!--<v-row>
                                <v-col>
                                    <v-btn id="AutoTest_IP871" color="success-btn" depressed dark @click="">View Workflow</v-btn>
                                </v-col>
                            </v-row>-->
                </v-card>
              </v-col>
            </v-row>
          </v-stepper-content>

          <!-- Workflow Edit Details -->
          <v-stepper-content step="3" class="pa-0">
            <!-- Workflow Header -->
            <v-row>
              <v-col cols="12" md="12" class="pt-1 mb-2">
                <h4 class="text-left page-head">
                  <v-avatar color="white" size="20" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary"
                      >mdi-timeline-text-outline</v-icon
                    >
                  </v-avatar>
                  <span
                    id="AutoTest_F239"
                    style="cursor: pointer"
                    @click="
                      fnResetWorkflowShowDetailsItem(),
                        (ProjectWorkflowSteps = 1)
                    "
                    >Workflow</span
                  >
                  > Workflow Details > {{ EditWorkflow.WfName }}
                </h4>
              </v-col>
              <!-- Edit Workflow details-->
              <v-row>
                <v-col
                  cols="12"
                  xs="12"
                  sm="12"
                  md="12"
                  class="text-center pt-1"
                >
                  <v-avatar
                    size="60"
                    tile
                    class="elevation-1 base-border-radius"
                  >
                    <v-img
                      src="../../images/WorkSpace-dummy-icon.png"
                      height="60"
                      width="60"
                      class="ma-4"
                    ></v-img>
                  </v-avatar>
                </v-col>
                <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                  <v-row :justify="justifyCenter">
                    <v-col md="3">
                      <v-text-field
                        outlined
                        dense
                        label="Workflow Name"
                        type="text"
                        v-model="EditWorkflow.WfName"
                      >
                      </v-text-field>
                      <v-textarea
                        rows="4"
                        outlined
                        dense
                        label="Description"
                        type="text"
                        v-model="EditWorkflow.WfDescription"
                        hide-details
                      >
                      </v-textarea>
                    </v-col>
                  </v-row>
                </v-col>
                <v-col
                  cols="12"
                  xs="12"
                  sm="12"
                  md="12"
                  class="text-center py-0 mb-0"
                >
                  <Secondary-Button
                    id="AutoTest_F240"
                    class="btn-122x36 mr-2"
                    title="Save"
                    @click.native="
                      fnEditWorkflowDetailsSave(), (ProjectWorkflowSteps = 2)
                    "
                    v-bind:disabled="!screenAccess"
                  ></Secondary-Button>
                  <Default-Button-Outlined
                    id="AutoTest_F241"
                    class="btn-122x36"
                    title="Cancel"
                    @click.native="
                      (EditWorkflow.WorkflowID = 0),
                        (EditWorkflow.WfName = ''),
                        (EditWorkflow.WfDescription = ''),
                        (ProjectWorkflowSteps = 2)
                    "
                  ></Default-Button-Outlined>
                </v-col>
              </v-row>
            </v-row>
          </v-stepper-content>

          <!-- Workflow Edit Levels -->
          <v-stepper-content step="4" class="pa-0">
            <!-- Workflow Header -->
            <v-row>
              <v-col cols="12" md="8" class="pt-1 mb-2">
                <h4 class="text-left page-head">
                  <v-avatar color="white" size="20" class="elevation-1 mr-2">
                    <v-icon size="15" color="primary"
                      >mdi-timeline-text-outline</v-icon
                    >
                  </v-avatar>
                  <span
                    id="AutoTest_F242"
                    style="cursor: pointer"
                    @click="
                      fnResetWorkflowShowDetailsItem(),
                        (ProjectWorkflowSteps = 1)
                    "
                    >Workflow</span
                  >
                  > Workflow Details > {{ EditWorkflow.WfName }}
                </h4>
              </v-col>
              <v-spacer></v-spacer>
              <v-col cols="6" xs="6" sm="6" md="9" class="text-right mr-4 pr-4">
                <v-btn
                  id="AutoTest_F243"
                  color="success-btn elevation-0 enter-btn"
                  dark
                  @click="fnAddLevelEditWorkflow()"
                  v-bind:disabled="!screenAccess"
                >
                  Add Level
                </v-btn>
              </v-col>

              <!-- Workflow Level Edit card -->
              <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                <v-card class="elevation-0">
                  <v-card-text class="py-0">
                    <v-row :justify="justifyCenter">
                      <v-col cols="6" md="6" class="pt-0">
                        <v-container fluid class="pt-0">
                          <v-row>
                            <v-col
                              cols="12"
                              md="12"
                              class="v-scrolling-div"
                              style="height: 50vh"
                            >
                              <v-col
                                md="12"
                                v-for="i in EditWorkflow.WfNoOfLevels"
                                :key="i"
                              >
                                <v-card outlined flat>
                                  <v-row>
                                    <v-row :justify="justifyCenter">
                                      <v-col
                                        cols="6"
                                        md="6"
                                        class="text-center py-8"
                                      >
                                        <v-progress-circular
                                          :rotate="360"
                                          :size="100"
                                          :width="10"
                                          :value="
                                            (100 / EditWorkflow.WfNoOfLevels) *
                                            i
                                          "
                                          color="#1EC2ED"
                                        >
                                          <span
                                            class="subtitle-1 text--primary"
                                          >
                                            {{ i }}<br />
                                            <span
                                              class="subtitle-2 text--disabled"
                                              >Level</span
                                            >
                                          </span>
                                        </v-progress-circular>
                                      </v-col>
                                    </v-row>
                                    <v-divider vertical></v-divider>
                                    <v-col
                                      cols="6"
                                      md="6"
                                      class="text-left pl-0"
                                    >
                                      <v-row no-gutters>
                                        <v-col
                                          cols="12"
                                          md="12"
                                          class="text-center"
                                        >
                                          <h4 class="heading-4">
                                            <v-icon size="16" class="mr-2"
                                              >mdi-account</v-icon
                                            >Users
                                            <span class="float-right">
                                              <v-btn
                                                id="AutoTest_F244"
                                                text
                                                small
                                                max-height="20"
                                                max-width="20"
                                                min-width="20"
                                                @click="
                                                  fnDeleteLevelEditWorkflow(i)
                                                "
                                                v-bind:disabled="!screenAccess"
                                              >
                                                <v-icon
                                                  color="#707070"
                                                  size="15"
                                                  >mdi-close</v-icon
                                                >
                                              </v-btn>
                                            </span>
                                          </h4>
                                          <v-divider class="mt-1"></v-divider>
                                        </v-col>
                                      </v-row>
                                      <!---   if there is no data-->
                                      <v-row
                                        v-if="
                                          EditWorkflow.WfLevelDetails.length ==
                                            0 ||
                                          EditWorkflow.WfLevelDetails[i - 1]
                                            .usersList.length == 0
                                        "
                                        no-gutters
                                      >
                                        <v-col
                                          cols="12"
                                          md="12"
                                          class="pt-2 mb-0 text-center"
                                        >
                                          <p class="heading-1">
                                            No Users Added
                                          </p>
                                        </v-col>
                                      </v-row>
                                      <!-- if User Assigned-->
                                      <v-row
                                        v-if="
                                          EditWorkflow.WfLevelDetails.length >
                                            0 &&
                                          EditWorkflow.WfLevelDetails[i - 1]
                                            .usersList.length > 0
                                        "
                                        no-gutters
                                      >
                                        <v-col cols="12" md="12">
                                          <v-col class="text-center mb-0 pt-1">
                                            <!--<v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>=1" size="30" class="team-avatar">
                                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                        </v-avatar>
                                                                                        <v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>=2" size="30" class="team-avatar">
                                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                        </v-avatar>
                                                                                        <v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>2" size="30" class="team-count" color="red">
                                                                                            <span class="text--white">+{{ EditWorkflow.WfLevelDetails[i-1].usersList.length }}</span>
                                                                                        </v-avatar>-->
                                            <v-avatar
                                              size="25"
                                              class="team-avatar"
                                              v-for="(
                                                item, key
                                              ) in EditWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.slice(0, 2)"
                                              v-bind:key="item.ID"
                                              :color="`${getcolor(item.Name)}`"
                                            >
                                              <img
                                                v-bind:src="
                                                  userimage + item.ProfileImage
                                                "
                                                alt="User DP"
                                                v-if="
                                                  item.ProfileImage.length > 0
                                                "
                                              />
                                              <span
                                                class="white--text"
                                                v-else
                                                >{{
                                                  item.Name == null
                                                    ? ""
                                                    : item.Name.substring(
                                                        0,
                                                        1
                                                      ).toUpperCase()
                                                }}</span
                                              >
                                            </v-avatar>
                                            <v-avatar
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length >= 4
                                              "
                                              size="25"
                                              class="team-count"
                                            >
                                              +{{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length - 3
                                              }}
                                            </v-avatar>
                                          </v-col>
                                          <v-col class="text-center mb-0 pt-0">
                                            <p
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length == 1
                                              "
                                            >
                                              {{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList[0].Name
                                              }}
                                            </p>
                                            <p
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length == 2
                                              "
                                            >
                                              {{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList[0].Name
                                              }},{{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList[1].Name
                                              }}
                                            </p>
                                            <p
                                              v-if="
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length > 2
                                              "
                                            >
                                              {{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList[0].Name
                                              }},{{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList[1].Name
                                              }}
                                              +
                                              {{
                                                EditWorkflow.WfLevelDetails[
                                                  i - 1
                                                ].usersList.length
                                              }}
                                            </p>
                                          </v-col>
                                        </v-col>
                                      </v-row>
                                      <!-- Assign Or Reassign Button-->
                                      <v-row no-gutters>
                                        <v-col
                                          v-if="
                                            EditWorkflow.WfLevelDetails.length >
                                              0 &&
                                            EditWorkflow.WfLevelDetails[i - 1]
                                              .usersList.length == 0
                                          "
                                          cols="12"
                                          md="12"
                                          class="text-center pt-0 mb-0"
                                        >
                                          <v-btn
                                            id="AutoTest_F245"
                                            color="success-btn elevation-0 enter-btn"
                                            dark
                                            @click="
                                              fnEditWorkflowAssignClick(i)
                                            "
                                            v-bind:disabled="!screenAccess"
                                          >
                                            Assign
                                          </v-btn>
                                        </v-col>
                                        <v-col
                                          v-else
                                          cols="12"
                                          md="12"
                                          class="text-center pt-0 mb-0"
                                        >
                                          <v-btn
                                            id="AutoTest_F246"
                                            color="success-btn elevation-0 enter-btn"
                                            dark
                                            @click="
                                              fnEditWorkflowAssignClick(i)
                                            "
                                            v-bind:disabled="!screenAccess"
                                          >
                                            Re-Assign
                                          </v-btn>
                                        </v-col>
                                      </v-row>
                                    </v-col>
                                  </v-row>
                                </v-card>
                              </v-col>
                            </v-col>
                          </v-row>
                          <v-col
                            cols="12"
                            xs="12"
                            sm="12"
                            md="12"
                            class="text-center py-4 mt-4 mb-0"
                          >
                            <Secondary-Button
                              id="AutoTest_F247"
                              class="btn-122x36 mr-4"
                              title="Save"
                              @click.native="
                                fnEditWorkflowLevelSave(),
                                  (ProjectWorkflowSteps = 2)
                              "
                              v-bind:disabled="
                                EditWorkflowSaveButton || !screenAccess
                              "
                            ></Secondary-Button>
                            <Default-Button-Outlined
                              id="AutoTest_F248"
                              class="btn-122x36"
                              title="Cancel"
                              @click.native="
                                (EditWorkflow.WorkflowID = 0),
                                  (EditWorkflow.WfName = ''),
                                  (EditWorkflow.WfDescription = ''),
                                  (EditWorkflow.WfNoOfLevels = 0),
                                  (EditWorkflow.WfLevelDetails = []),
                                  (ProjectWorkflowSteps = 2)
                              "
                            ></Default-Button-Outlined>
                          </v-col>
                        </v-container>
                      </v-col>
                    </v-row>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>

            <!-- Edit Workflow Assign User Dailog Box-->
            <v-dialog v-model="EditWorkflowAssignDialog" width="900">
              <v-col cols="12" md="12" class="pt-0 mb-0">
                <v-card>
                  <v-card-title
                    primary-title
                    class="page-head pb-0 text-center"
                  >
                    Assign Users
                    <v-spacer></v-spacer>
                    <v-btn
                      id="AutoTest_F249"
                      text
                      @click="EditWorkflowAssignDialog = false, EditWorkflowAssignLevelClicked = 0, editWorkflowAssignUsersSelected = []"
                    >
                      <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                  </v-card-title>
                  <v-row>
                    <!-- Users-->
                    <v-col cols="12" md="12" sm="12" xs="12" class="px-4">
                      <v-data-table
                        :headers="WorkflowAssignUsersTableHeaders"
                        :items="WorkflowAssignUsersTableItems"
                        item-key="ID"
                        sort-by="Name"
                        show-select
                        class="elevation-1"
                        fixed-header
                        height="40vh"
                        v-model="editWorkflowAssignUsersSelected"
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
                  </v-row>
                  <v-row>
                    <v-col
                      cols="12"
                      xs="12"
                      sm="12"
                      md="12"
                      class="text-center py-4 mb-4"
                    >
                      <Secondary-Button
                        id="AutoTest_F250"
                        class="btn-122x36 mr-2"
                        title="Save"
                        @click.native="fnEditWorkflowAssignSave()"
                        v-bind:disabled="
                          editWorkflowAssignUsersSelected.length == 0
                        "
                      ></Secondary-Button>
                      <Default-Button-Outlined
                        id="AutoTest_F251"
                        class="btn-122x36"
                        title="Cancel"
                        @click.native="EditWorkflowAssignDialog = false, EditWorkflowAssignLevelClicked = 0, editWorkflowAssignUsersSelected = []"
                      ></Default-Button-Outlined>
                    </v-col>
                  </v-row>
                </v-card>
              </v-col>
            </v-dialog>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>

      <!-- Create Workflow Dailog Box-->
      <v-dialog v-model="CreateWorkFlowDlg" width="850">
        <v-card style="height: 600px">
          <v-card-title primary-title class="page-head pb-0">
            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
              <img :src="Workflow_Tab_icon" style="width: 70%" /> </v-avatar
            >Add Workflows
            <v-spacer></v-spacer>
            <v-btn
              id="AutoTest_IP885"
              text
              @click="resetData(), (CreateWorkFlowDlg = false)"
            >
              <v-icon color="#707070" size="15">mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text class="py-0">
            <v-container fluid class="pt-0">
              <v-row no-gutters>
                <v-col cols="12" md="12" class="pt-0">
                  <v-stepper
                    v-model="CreateWorkflowSteps"
                    class="elevation-0"
                    style="height: 535px"
                  >
                    <v-stepper-items>
                      <!-- Step 1 Create Work Flow -->
                      <v-stepper-content step="1" class="pa-0">
                        <v-form ref="entryForm" @submit.prevent="submitHandler">
                          <v-col cols="12" md="12" class="pt-8">
                            <h4 class="text-center heading-3">
                              Step 1 : Please create a project creation approval
                              workflow.
                            </h4>
                          </v-col>
                          <v-col
                            cols="12"
                            xs="12"
                            sm="12"
                            md="12"
                            class="text-center pt-3"
                          >
                            <v-avatar
                              size="60"
                              tile
                              class="elevation-1 base-border-radius"
                            >
                              <v-img
                                :src="Workflow_Icon"
                                height="42"
                                width="42"
                                class="ma-4"
                              ></v-img>
                            </v-avatar>
                          </v-col>
                          <v-col cols="12" xs="12" sm="12" md="12" class="pt-4">
                            <v-row
                              :justify="justifyCenter"
                              style="height: 250px"
                            >
                              <v-col md="6">
                                <v-text-field
                                  outlined
                                  dense
                                  label="Workflow Name"
                                  type="text"
                                  v-model="CreateWorkflow.WfName"
                                  :rules="rules.name"
                                  required
                                >
                                  <template v-slot:append>
                                    <span
                                      v-if="CreateWorkflow.WfName.length == 0"
                                      class="error--text"
                                      >*</span
                                    >
                                  </template>
                                </v-text-field>
                                <v-textarea
                                  rows="3"
                                  outlined
                                  dense
                                  label="Description"
                                  type="text"
                                  class="pt-2"
                                  v-model="CreateWorkflow.WfDescription"
                                  :rules="rules.desc"
                                  required
                                >
                                  <template v-slot:append>
                                    <span
                                      v-if="
                                        CreateWorkflow.WfDescription.length == 0
                                      "
                                      class="error--text"
                                      >*</span
                                    >
                                  </template>
                                </v-textarea>
                                <!-- <v-select
                                  dense=""
                                  outlined=""
                                  :items="maxCreateWorkflowLevel"
                                  label="Number of Levels"
                                  solo
                                  flat
                                  return-object
                                  v-model="CreateWorkflow.WfNoOfLevels"
                                  :rules="rules.select2"
                                  required
                                >
                                <template v-slot:append>
                                            <span v-if="CreateWorkflow.WfNoOfLevels.length==0">*</span>
                                        </template>
                                </v-select> -->
                                <v-text-field
                                  outlined
                                  dense
                                  class="mt-1"
                                  label="Number of Levels"
                                  v-model="CreateWorkflow.WfNoOfLevels"
                                  :rules="[
                                    rulesLevel.required,
                                    rulesLevel.min,
                                    rulesLevel.max,
                                  ]"
                                  required
                                  type="number"
                                  min="1"
                                >
                                  <template v-slot:append>
                                    <span
                                      v-if="CreateWorkflow.WfNoOfLevels == 0"
                                      class="error--text"
                                      >*</span
                                    >
                                  </template>
                                </v-text-field>
                              </v-col>
                            </v-row>
                          </v-col>
                          <v-col
                            cols="12"
                            xs="12"
                            sm="12"
                            md="12"
                            class="text-right py-0 mb-3 pt-10"
                          >
                            <v-btn
                              id="AutoTest_IP886"
                              elevation="1"
                              class="secondary btn-122x36 mr-2"
                              type="submit"
                              color="#b3d4fc"
                              height="37"
                              depressed
                              dark
                              @click="submitHandler()"
                            >
                              Next
                            </v-btn>
                            <Default-Button-Outlined
                              id="AutoTest_IP887"
                              class="btn-122x36"
                              title="Cancel"
                              @click.native="
                                resetData(), (CreateWorkFlowDlg = false)
                              "
                            ></Default-Button-Outlined>
                          </v-col>
                        </v-form>
                      </v-stepper-content>

                      <!-- Step 2 - Assgin Users to Work flow-->
                      <v-stepper-content step="2" class="pa-0">
                        <v-container fluid class="pt-0">
                          <v-col cols="12" md="12" class="pt-3">
                            <h4 class="text-center heading-3">
                              Step 2 : Please assign users to different levels
                            </h4>
                          </v-col>
                          <v-spacer></v-spacer>
                          <v-row>
                            <v-col sm="6" cols="12" class="pt-7 pl-5">
                              <span
                                v-if="CreateWorkflowNoOfLevelsError == true"
                                class="error-level"
                              >
                                Minimum 1 level is required for a workflow</span
                              >
                            </v-col>
                            <v-col cols="12" sm="6" class="text-right pt-5">
                              <v-btn
                                id="AutoTest_IP888"
                                color="success-btn elevation-0 enter-btn"
                                dark
                                @click="fnAddLevelCreateWorkflow()"
                              >
                                Add Level
                              </v-btn>
                            </v-col>
                          </v-row>
                          <v-row class="v-scrolling-div" style="height: 45vh">
                            <v-col
                              md="12"
                              v-for="i in CreateWorkflow.WfLevelDetails.length"
                              :key="i"
                            >
                              <v-card outlined flat>
                                <v-row>
                                  <v-row no-gutters :justify="justifyCenter">
                                    <v-col cols="12" md="12">
                                      <v-row>
                                        <v-col
                                          cols="7"
                                          sm="7"
                                          v-if="
                                            editLevelNameButton == false ||
                                            editLevelNameIndex != i
                                          "
                                          class="pb-1 pt-2 pl-10"
                                        >
                                          {{ WfNoOfLevelName[i - 1] }}
                                        </v-col>
                                        <v-col
                                          cols="7"
                                          style="text-align: center"
                                          sm="7"
                                          v-if="
                                            editLevelNameButton == true &&
                                            editLevelNameIndex == i
                                          "
                                          class="pb-1 pt-2 pl-7"
                                        >
                                          <input
                                            class="edit-level-text-field"
                                            maxlength="20"
                                            v-model="newLevelName"
                                            placeholder="Edit level Name"
                                          />
                                        </v-col>
                                        <v-col
                                          cols="5"
                                          style="text-align: end"
                                          sm="5"
                                          class="pb-1 pr-5 pt-1"
                                        >
                                          <v-btn
                                            elevation="0"
                                            style="border-radius: 3px"
                                            color="#EEEEEE"
                                            icon
                                            v-if="
                                              editLevelNameButton == false ||
                                              editLevelNameIndex != i
                                            "
                                            height="30"
                                            @click="
                                              editManageLevelNameHandler(i)
                                            "
                                          >
                                            <v-icon
                                              style="font-size: 20px"
                                              color="black"
                                              >mdi-pencil-outline</v-icon
                                            >
                                          </v-btn>
                                          <v-btn
                                            elevation="0"
                                            style="border-radius: 3px"
                                            color="#EEEEEE"
                                            v-if="
                                              editLevelNameButton == true &&
                                              editLevelNameIndex == i
                                            "
                                            icon
                                            height="30"
                                            @click="
                                              editLevelSaveNameHandler(i - 1)
                                            "
                                          >
                                            <v-icon
                                              style="font-size: 20px"
                                              color="black"
                                              >mdi-content-save-outline</v-icon
                                            >
                                          </v-btn>
                                        </v-col>
                                      </v-row>
                                      <v-col
                                        cols="12"
                                        sm="12"
                                        class="pa-0 pl-3"
                                      >
                                        <v-divider></v-divider>
                                      </v-col>
                                      <v-row>
                                        <v-col
                                          sm="12"
                                          style="text-align: center"
                                        >
                                          <v-progress-circular
                                            :rotate="360"
                                            :size="75"
                                            :width="10"
                                            :value="
                                              (100 /
                                                CreateWorkflow.WfNoOfLevels) *
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
                                        </v-col>
                                      </v-row>
                                    </v-col>
                                  </v-row>
                                  <v-divider vertical></v-divider>
                                  <v-col cols="6" md="6" class="text-left pl-0">
                                    <v-row no-gutters>
                                      <v-col
                                        cols="12"
                                        md="12"
                                        class="text-center"
                                      >
                                        <h4 class="heading-4">
                                          <v-icon size="16" class="mr-2"
                                            >mdi-account</v-icon
                                          >Users
                                          <span class="float-right">
                                            <v-btn
                                              id="AutoTest_IP889"
                                              text
                                              small
                                              max-height="20"
                                              max-width="20"
                                              min-width="20"
                                              @click="
                                                CreateWorkflow.WfNoOfLevels == 1
                                                  ? fnDeleteLevelCreateWorkflowError(
                                                      i
                                                    )
                                                  : fnDeleteLevelCreateWorkflow(
                                                      i
                                                    )
                                              "
                                            >
                                              <v-icon color="#707070" size="15"
                                                >mdi-close</v-icon
                                              >
                                            </v-btn>
                                          </span>
                                        </h4>
                                        <v-divider class="mt-1"></v-divider>
                                      </v-col>
                                    </v-row>
                                    <!---   if there is no data-->
                                    <v-row
                                      v-if="
                                        CreateWorkflow.WfLevelDetails.length ==
                                          0 ||
                                        CreateWorkflow.WfLevelDetails[i - 1]
                                          .usersList.length == 0
                                      "
                                      no-gutters
                                    >
                                      <v-col
                                        cols="12"
                                        md="12"
                                        class="pt-4 mb-0 text-center"
                                      >
                                        <p
                                          v-if="
                                            assignUserEmptyIndex.includes(i) ===
                                            true
                                          "
                                          class="heading-1"
                                          style="color: red"
                                        >
                                          No Users Added *
                                        </p>
                                        <p v-else class="heading-1">
                                          No Users Added
                                          <span style="color: red">* </span>
                                        </p>
                                      </v-col>
                                    </v-row>
                                    <v-row v-else no-gutters>
                                      <!-- Users-->
                                      <v-col
                                        v-if="
                                          CreateWorkflow.WfLevelDetails.length >
                                            0 &&
                                          CreateWorkflow.WfLevelDetails[i - 1]
                                            .usersList.length > 0
                                        "
                                        cols="12"
                                        md="12"
                                      >
                                        <v-col class="text-center mb-0 pt-1">
                                          <!--<v-avatar v-if="
                                                        CreateWorkflow.WfLevelDetails[
                                                        i - 1
                                                        ].usersList.length>
                                                  = 1
                                                  "
                                                  size="30"
                                                  class="team-avatar"
                                                  >
                                                  <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                       class="team-image"
                                                       alt="Team name" />
                                              </v-avatar>
                                              <v-avatar v-if="
                                                        CreateWorkflow.WfLevelDetails[
                                                        i - 1
                                                        ].usersList.length>
                                                  = 2
                                                  "
                                                  size="30"
                                                  class="team-avatar"
                                                  >
                                                  <img src="https://cdn.vuetifyjs.com/images/john.jpg"
                                                       class="team-image"
                                                       alt="Team name" />
                                              </v-avatar>
                                              <v-avatar v-if="
                                                        CreateWorkflow.WfLevelDetails[
                                                        i - 1
                                                        ].usersList.length>
                                                  2
                                                  "
                                                  size="30"
                                                  class="team-count"
                                                  color="red"
                                                  >
                                                  +{{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length
                                                  }}
                                              </v-avatar>-->
                                          <v-avatar
                                            size="25"
                                            class="team-avatar"
                                            v-for="(
                                              item, key
                                            ) in CreateWorkflow.WfLevelDetails[
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
                                              v-if="
                                                item.ProfileImage.length > 0
                                              "
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
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length >= 4
                                            "
                                            size="25"
                                            class="team-count"
                                          >
                                            +{{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length - 3
                                            }}
                                          </v-avatar>
                                        </v-col>
                                        <v-col class="text-center mb-0 pt-0">
                                          <p
                                            v-if="
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length == 1
                                            "
                                          >
                                            {{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList[0].Name
                                            }}
                                          </p>
                                          <p
                                            v-if="
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length == 2
                                            "
                                          >
                                            {{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList[0].Name
                                            }},{{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList[1].Name
                                            }}
                                          </p>
                                          <p
                                            v-if="
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length > 2
                                            "
                                          >
                                            {{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList[0].Name
                                            }},{{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList[1].Name
                                            }}
                                            +
                                            {{
                                              CreateWorkflow.WfLevelDetails[
                                                i - 1
                                              ].usersList.length
                                            }}
                                          </p>
                                        </v-col>
                                      </v-col>
                                    </v-row>
                                    <!-- Assign and ReAssign Button-->
                                    <v-row no-gutters>
                                      <v-col
                                        v-if="
                                          CreateWorkflow.WfLevelDetails
                                            .length == 0 ||
                                          CreateWorkflow.WfLevelDetails[i - 1]
                                            .usersList.length == 0
                                        "
                                        cols="12"
                                        md="12"
                                        class="text-center pt-0 mb-0"
                                      >
                                        <v-btn
                                          id="AutoTest_IP890"
                                          color="success-btn elevation-0 enter-btn"
                                          dark
                                          @click="
                                            fnCreateWorkflowAssignClick(i)
                                          "
                                        >
                                          Assign
                                        </v-btn>
                                      </v-col>
                                      <v-col
                                        v-else
                                        cols="12"
                                        md="12"
                                        class="text-center pt-0 mb-0"
                                      >
                                        <v-btn
                                          id="AutoTest_IP891"
                                          color="success-btn elevation-0 enter-btn"
                                          dark
                                          @click="
                                            fnCreateWorkflowAssignClick(i)
                                          "
                                        >
                                          Re-Assign
                                        </v-btn>
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
                            class="text-right py-4 mb-0"
                          >
                            <Secondary-Button
                              id="AutoTest_IP892"
                              class="btn-122x36 mr-2"
                              title="Save"
                              @click.native="fncheckAssignIsNotEmpty()"
                            ></Secondary-Button>
                            <!-- <v-btn
                              id="AutoTest_IP886"
                              elevation="1"
                              class="secondary btn-122x36 mr-2"
                              type="submit"
                              color="#b3d4fc"
                              height="37"
                              depressed
                              dark
                              @click="submitHandler()"
                            >
                              Save
                            </v-btn> -->
                            <Default-Button-Outlined
                              id="AutoTest_IP893"
                              class="btn-122x36"
                              title="Cancel"
                              @click.native="
                                fnCreateWorkflowCloseORCancel(),
                                  resetData(),
                                  (CreateWorkFlowDlg = false)
                              "
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
      <!-- Create Workflow Assing Users to Workflow-->
      <v-dialog v-model="CreateWorkflowAssignDialog" width="900">
        <v-col cols="12" md="12" class="pt-0 mb-0">
          <v-card>
            <v-card-title primary-title class="page-head pb-0 text-center">
              Assign Users
              <v-spacer></v-spacer>
              <v-btn
                id="AutoTest_IP894"
                text
                @click="CreateWorkflowAssignDialog = false, CreateWorkflowAssignLevelClicked = 0, createWorkflowAssignUsersSelected = []"
                v-bind:disabled="!screenAccess"
              >
                <v-icon color="#707070" size="15">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-row>
              <!-- Users-->
              <v-col
                cols="12"
                xs="12"
                sm="12"
                md="12"
                class="text-center py-0 mb-0"
              >
                <v-data-table
                  :headers="WorkflowAssignUsersTableHeaders"
                  :items="WorkflowAssignUsersTableItems"
                  item-key="ID"
                  sort-by="Name"
                  show-select
                  height="30vh"
                  fixed-header
                  class="elevation-0"
                  v-model="createWorkflowAssignUsersSelected"
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
            </v-row>
            <v-row>
              <v-col
                cols="12"
                xs="12"
                sm="12"
                md="12"
                class="text-center py-4 mb-4"
              >
                <Secondary-Button
                  id="AutoTest_IP895"
                  class="btn-122x36 mr-2"
                  title="Save"
                  @click.native="fnCreateWorkflowAssignSave()"
                  v-bind:disabled="
                    createWorkflowAssignUsersSelected.length == 0
                  "
                ></Secondary-Button>
                <Default-Button-Outlined
                  id="AutoTest_IP896"
                  class="btn-122x36"
                  title="Cancel"
                  @click.native="CreateWorkflowAssignDialog = false, CreateWorkflowAssignLevelClicked = 0, createWorkflowAssignUsersSelected = []"
                ></Default-Button-Outlined>
              </v-col>
            </v-row>
          </v-card>
        </v-col>
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
    </v-container>
  </v-card>
</template>
<script>
import AddTagButton from "@/components/add-tag-button.vue";
import SaveButtonSuccessSmall from "@/components/save-button-success-small.vue";
import CancelButtonOutlinedSm from "@/components/cancel-button-outlined-sm.vue";
import NextButtonPrimaryLight from "@/components/next-button-blue.vue";
import PreviousButtonOutlined from "@/components/previous-button-outlined.vue";
import ViewWorkflowButton from "@/components/view-workflow-button.vue";
import SecondaryButton from "@/components/secondary-button.vue";
import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
import objUtils from '../../utils.js';

var objManageProject;
async function importscript() {
  return Promise.all([
    import("../../BL/ManageProject.js").then((mod) => {
      objManageProject = new mod.ManageProject();
    }),
  ]);
}
var objFinance;
async function importFinancescript() {
  return Promise.all([
    import("../../BL/Finance.js").then((mod) => {
      objFinance = new mod.Finance();
    }),
  ]);
}

export default {
  data() {
    return {
      screenAccess: 0,
      RWAccess: 0,
      successSnackbarMsg: false,
      errorSnackbarMsg: false,
      infoSnackbarMsg: false,
      Msg: "",
      justifyCenter: "center",
      WorkspaceID: 0,
      initiativeInfo: {
        InitiativeId: 0,
        InitiativeName: "",
      },
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
        GroupId: 0,
      },

      ProjectWorkflowSteps: 0,
      //Project Workflow tab
      workflowsList: [],
      filterWorkflowsList: [],
      searchWorkflow: "",
      workflowSortMenu: false,
      checkboxWorkflow: [],
      BulkActionMenu: false,
      //Edit workflow starts here
      WorkflowShowDetailsItem: {
        WorkflowID: 0,
        WfName: "",
        WfDescription: "",
        WfNoOfLevels: 0,
        WfLevelDetails: [],
      },
      EditWorkflow: {
        WorkflowID: 0,
        WfName: "",
        WfDescription: "",
        WfNoOfLevels: 0,
        WfLevelDetails: [],
      },
      EditWorkflowSaveButton: true,
      EditWorkflowAssignDialog: false,
      editWorkflowAssignUsersSelected: [],
      EditWorkflowAssignLevelClicked: 0,
      //Edit workflow ends here
      //*** these two headers is common for both Create and Edit workflow Assign ***//
      WorkflowAssignUsersTableHeaders: [
        { text: "Name", align: "left", value: "Name" },
        { text: "User Role", value: "UserRole" },
      ],
      WorkflowAssignUsersTableItems: [],
      //Create workflow starts here
      CreateWorkFlowDlg: false,
      CreateWorkflowSteps: 0,
      maxCreateWorkflowLevel: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      CreateWorkflow: {
        WorkflowID: 0,
        WfName: "",
        WfDescription: "",
        WfNoOfLevels: "",
        WfLevelDetails: [],
      },
      CreateWorkflowSaveButton: true,
      CreateWorkflowAssignDialog: false,
      createWorkflowAssignUsersSelected: [],
      CreateWorkflowAssignLevelClicked: 0,
      //Create workflow ends here
      
      //step 2 of Select Workflow
      ViewWorkflowItem: { WfLevelDetails: [] },
      //Select workflow ends here

      
      userimage: "",
      EditAssignError: false,
      prevNoofLevel: "",
      EditWorkflowNoOfLevelsError: false,
      EditWorkflowDetailsEdit: false,
      editLevel: false,
      editRules: [],
      CreateWorkflowNoOfLevelsError: false,
      ActivityTab: "tab-1",
      editLevelNameIndex: "",
      assignUserEmpty: false,
      assignUserEmptyIndex: [],
      newLevelName: "",
      editLevelNameButton: false,
      rules: [],
      rulesLevel: {
        required: (value) => !!value || "Level Number is Required.",
        min: (v) => v >= 1 || `The Min Level Number is 1`,
        max: (v) => v <= 999 || `The Max Level Number is 999`,
      },
      Description_icon: require("@/assets/ProjectWorkflow/Description_icon.svg"),
      Levels_icon: require("@/assets/ProjectWorkflow/Levels_icon.svg"),
      Workflow_Icon: require("@/assets/ProjectWorkflow/Workflow_Icon.svg"),
      Workflow_Tab_icon: require("@/assets/ProjectWorkflow/Workflow_Tab_icon.svg"),
      EditWfNoOfLevelName: ["Level 1", "Level 2"],
      WfNoOfLevelName: [],
    };
  },
  mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
    this.userimage = window.SERVER_URL + "/Upload";
    this.screenAccess = this.$route.query.Access == 3 ? true : false;
    this.initiativeInfo.InitiativeId = this.$route.query.InitID;
    this.projectInfo.ProjectId = this.$route.query.ProjID;
    await importscript();
    await importFinancescript();
    await this.fnGetProjectDetailsByID();
    await this.fnGetUsersListByGroupId();
    await this.GetWorkflowsListToactivity();
  },
   methods: {
      /// <summary>
      ///  fncheckAssignIsEditLevelNotEmpty :- Function used to check assign user box is not empty while editing a workflow
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      fncheckAssignIsEditLevelNotEmpty() {
      this.EditAssignError = false;
      for (var i = 0; i < this.EditWorkflow.WfNoOfLevels; i++) {
        if (this.EditWorkflow.WfLevelDetails[i].usersList == undefined || this.EditWorkflow.WfLevelDetails[i].usersList.length == 0) {
          this.assignUserEmptyIndex.push(i + 1);
          this.EditAssignError = true;
        } else {
          if (this.assignUserEmptyIndex.includes(i + 1)) {
            this.assignUserEmptyIndex = this.assignUserEmptyIndex.filter((v) => v != i + 1);
          }
        }
      }
      if (this.EditAssignError == false) {
        this.fnEditWorkflowLevelSave()
      }
    },

    /// <summary>
    ///  editManageLevelNameHandler :- Function used to handle level name in manage edit section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    editManageLevelNameHandler(i) {
      this.newLevelName = this.EditWfNoOfLevelName[i - 1];
      this.editLevelNameIndex = i;
      this.editLevelNameButton = true;
    },

    /// <summary>
    ///  editLevelSaveNameHandler :- Function used to save new level name after edit in create workflow step 2 section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    editLevelSaveNameHandler(i) {
      if (this.newLevelName != "") {
        this.WfNoOfLevelName[i] = this.newLevelName;
      }
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    /// <summary>
    ///  editLevelManageSaveNameHandler :- Function used to save new level name after edit in manage edit level section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    editLevelManageSaveNameHandler(i) {
      if (this.newLevelName != "") {
        this.EditWfNoOfLevelName[i] = this.newLevelName;
      }
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    /// <summary>
    ///  resetData :- Function used to clear data when creating a workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    resetData() {
      this.CreateWorkflowNoOfLevelsError = false;
      this.rules = [];
      this.newLevelName = "";
      this.editLevelNameIndex = "";
      this.editLevelNameButton = false;
      this.CreateWorkflow.WfName = "";
      this.CreateWorkflow.WfDescription = "";
      this.WfNoOfLevelName = [];
      this.rulesLevel = {};
      this.CreateWorkflow.WfNoOfLevels = "";
      this.CreateWorkflowSteps = 1;
      this.editLevelNameIndex = "";
      this.assignUserEmpty = false;
      this.assignUserEmptyIndex = [];
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    /// <summary>
    ///  submitHandler :- Function used to validate the required fields in create workflow dialog and move to the next step
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    submitHandler() {
      this.rules = {
        name: [(v) => !!v || "Name is required"],
        desc: [(v) => !!v || "Description is required"],
        select2: [(v) => (!!v && v != 0) || "Level is required"],
      };
      this.rulesLevel = {
        required: (value) => !!value || "Level Number is Required.",
        min: (v) => v >= 1 || `The Min Level Number is 1`,
        max: (v) => v <= 999 || `The Max Level Number is 999`,
      };
      let self = this;
      setTimeout(() => {
        if (self.$refs.entryForm.validate()) {
          this.CreateWorkflow.WfLevelDetails = [];
          this.fnCreateWorkflowLevelInitialize();
        }
      });
    },

    /// <summary>
    ///  fncheckAssignIsNotEmpty :- Function used to check assign user box is not empty while creating a workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fncheckAssignIsNotEmpty() {
      for (var i = 0; i < this.CreateWorkflow.WfNoOfLevels; i++) {
        if (this.CreateWorkflow.WfLevelDetails[i].usersList.length == 0) {
          this.assignUserEmptyIndex.push(i + 1);
          this.assignUserEmpty = true;
        } else {
          if (this.assignUserEmptyIndex.includes(i + 1)) {
            this.assignUserEmptyIndex = this.assignUserEmptyIndex.filter((v) => v != i + 1);
          }
        }
      }
      if (this.assignUserEmpty == false) {
        this.CreateWorkflowNoOfLevelsError = false;
        this.fnCreateWorkflowSave();
      } else {
        this.assignUserEmpty = false;
      }
    },

    /// <summary>
    ///  getcolor :- Function used to get color of users
    /// </summary>
    /// <param name="name"></param>
    /// <returns> </returns>
    getcolor(name) {
      return objUtils.common.getcolor(name);
    },

    /// <summary>
    ///  fnSetWorkflowShowDetailsItem :- Function used to show workflow details
    /// </summary>
    /// <param name="item"></param>
    /// <returns> </returns>
    fnSetWorkflowShowDetailsItem(item) {
      this.WorkflowShowDetailsItem.WorkflowID = item.WorkflowID;
      this.WorkflowShowDetailsItem.WfName = item.WfName;
      this.WorkflowShowDetailsItem.WfDescription = item.WfDescription;
      this.WorkflowShowDetailsItem.WfNoOfLevels = item.WfNoOfLevels;
      this.WorkflowShowDetailsItem.WfLevelDetails = item.WfLevelDetails;
      this.prevNoofLevel = item.WfNoOfLevels
      this.ActivityTab = "tab-3";
    },

    /// <summary>
    ///  fnResetWorkflowShowDetailsItem :- Function used to reset workflow details
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnResetWorkflowShowDetailsItem() {
      this.WorkflowShowDetailsItem.WorkflowID = 0;
      this.WorkflowShowDetailsItem.WfName = "";
      this.WorkflowShowDetailsItem.WfDescription = "";
      this.WorkflowShowDetailsItem.WfNoOfLevels = 0;
      this.WorkflowShowDetailsItem.WfLevelDetails = [];
    },

    /// <summary>
    ///  fnGetProjectDetailsByID :- Function used to fetch project details based on ProjectId
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
        this.projectInfo.ProjectTagsList = JSON.parse(this.projectInfo.ProjectTags);
      } else {
          this.Msg = "Failed";
          this.errorSnackbarMsg = true;
      }
    },

    /// <summary>
    ///  fnGetUsersListByGroupId :- Function used to fetch user details based on GroupId,WorkspaceID
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnGetUsersListByGroupId() {
      var data = new FormData();
      data.append("GroupId", this.projectInfo.GroupId);
      data.append("WorkspaceID", this.WorkspaceID);
      var result = await objManageProject.GetUsersListByGroupId(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.WorkflowAssignUsersTableItems = result.data;
      }
    },

    /// <summary>
    /// GetWorkflowsListToactivity :- Function to fetch Workflows List to Activity
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async GetWorkflowsListToactivity() {
      var data = new FormData();
      data.append("ProjectId", this.projectInfo.ProjectId);
      data.append("WorkspaceId", this.WorkspaceID);
      var result = await objFinance.GetWorkflowsListToactivity(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.workflowsList = result.data;
        this.filterWorkflowsList = result.data;
      }
    },

    /// <summary>
    ///  fnWorkflowSort :- Function used to sort workflow based on type (Name-Ascending, Name-Descending, Recently Added)
    /// </summary>
    /// <param name="type"></param>
    /// <returns> </returns>
    fnWorkflowSort(type) {
      this.workflowSortMenu = false;
      switch (type) {
        case "Name-Ascending":
          this.filterWorkflowsList.sort((a, b) => a.WfName > b.WfName ? 1 : -1);
          break;
        case "Name-Descending":
          this.filterWorkflowsList.sort((a, b) => a.WfName < b.WfName ? 1 : -1);
          break;
        case "Recently Added":
          this.filterWorkflowsList.sort((a, b) => a.WorkflowID < b.WorkflowID ? 1 : -1);
          break;
        default:
        // code block
      }
    },
    //*** Edit Workflow functions starts here ***//
    /// <summary>
    ///  levePrevData :- Function used to get the previous level data of workflow when click on cancel buttion in edit mode
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    levePrevData() {
      this.EditWorkflow.WfLevelDetails = this.EditWorkflow.WfLevelDetails.filter((val, i) => i < this.prevNoofLevel);
      this.EditWorkflow.WfNoOfLevels = this.prevNoofLevel;
      this.editLevel = false;
      this.EditWorkflowNoOfLevelsError = false;
    },

    /// <summary>
    ///  editSubmitHandler :- Function used to save workflow details after edit
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    editSubmitHandler() {
      this.editRules = {
        name: [(v) => !!v || "Name is required"],
        desc: [(v) => !!v || "Description is required"],
      };
      let self = this;
      setTimeout(() => {
        if (self.$refs.entryForm.validate()) {
          this.editRules = [];
          this.fnEditWorkflowDetailsSave();
        }
      });
    },

    /// <summary>
    ///  fnAddLevelEditWorkflow :- Function used to add a level when editing a workflow level
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnAddLevelEditWorkflow() {
      this.EditWorkflow.WfLevelDetails.push({
        WfLevel: this.EditWorkflow.WfNoOfLevels + 1,
        usersList: [],
      });
      this.EditWorkflow.WfNoOfLevels += 1;
      this.EditWorkflowSaveButton = true;
    },

    /// <summary>
    ///  fnDeleteLevelEditWorkflow :- Function used to delete a level of workflow in edit section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    fnDeleteLevelEditWorkflow(i) {
      this.EditWorkflow.WfNoOfLevels -= 1;
      this.EditWorkflow.WfLevelDetails = this.EditWorkflow.WfLevelDetails.filter((x) => x.WfLevel != i);
      if (this.EditWorkflow.WfNoOfLevels == 0) {
        this.EditWorkflow.WfLevelDetails = [];
        this.EditWorkflowSaveButton = true;
      } else {
        this.EditWorkflow.WfLevelDetails.forEach((u) => {
          if (u.WfLevel > i) u.WfLevel -= 1;
        });
      }
    },

    /// <summary>
    ///  fnCheckEditWorkflowSaveButton :- Function used to check save button in edit workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCheckEditWorkflowSaveButton() {
      if (this.EditWorkflow.WfNoOfLevels == 0) {
        this.EditWorkflowSaveButton = true;
        return;
      } else {
        for (var index = 0; index < this.EditWorkflow.WfNoOfLevels; index++) {
          var temp = this.EditWorkflow.WfLevelDetails.filter((x) => x.WfLevel == index + 1);
          if (temp.length > 0 && temp[0].usersList.length == 0) {
            this.EditWorkflowSaveButton = true;
            return;
          }
        }
        this.EditWorkflowSaveButton = false;
      }
    },

    /// <summary>
    ///  fnEditWorkflowAssignClick :- Function used to assign/re-assign user to the workflow
    /// </summary>
    /// <param name="index"></param>
    /// <returns> </returns>
    fnEditWorkflowAssignClick(index) {
      this.EditWorkflowAssignDialog = true;
      this.EditWorkflowAssignLevelClicked = index;
      var temp = this.EditWorkflow.WfLevelDetails.filter((x) => x.WfLevel == index);
      if (temp.length > 0) {
        this.editWorkflowAssignUsersSelected = temp[0].usersList;
      }
    },

    /// <summary>
    ///  fnEditWorkflowAssignSave :- Function used to save users while assign/re-assign 
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnEditWorkflowAssignSave() {
      var temp = this.EditWorkflow.WfLevelDetails.filter((x) => x.WfLevel == this.EditWorkflowAssignLevelClicked);
      if (temp.length > 0) {
        //this is for re-assign
        temp[0].usersList = this.editWorkflowAssignUsersSelected;
      } else {
        //this is for assign
        this.EditWorkflow.WfLevelDetails.push({
          WfLevel: this.EditWorkflowAssignLevelClicked,
          usersList: this.editWorkflowAssignUsersSelected,
        });
      }
      this.fnCheckEditWorkflowSaveButton();
      this.EditWorkflowAssignDialog = false;
      this.EditWorkflowAssignLevelClicked = 0;
      this.editWorkflowAssignUsersSelected = [];
    },

    /// <summary>
    ///  fnEditWorkflowDetailsSave :- Function used to update project workflow details
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnEditWorkflowDetailsSave() {
      var data = new FormData();
      data.append("WorkflowID", this.EditWorkflow.WorkflowID);
      data.append("WfName", this.EditWorkflow.WfName);
      data.append("WfDescription", this.EditWorkflow.WfDescription);
      var result = await objFinance.UpdateFinanceWorkflowDetails(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully updated the workflow";
        this.successSnackbarMsg = true;
        this.EditWorkflowDetailsEdit = false;
        this.WorkflowShowDetailsItem.WfName = this.EditWorkflow.WfName;
        this.WorkflowShowDetailsItem.WfDescription = this.EditWorkflow.WfDescription;
        this.EditWorkflow.WorkflowID = 0;
        this.EditWorkflow.WfName = "";
        this.EditWorkflow.WfDescription = "";
        this.GetWorkflowsListToactivity();
      }
    },

    /// <summary>
    ///  fnEditWorkflowLevelSave :- Function used to update project workflow level details
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnEditWorkflowLevelSave() {
      var data = new FormData();
      data.append("WorkflowID", this.EditWorkflow.WorkflowID);
      data.append("WfName", this.EditWorkflow.WfName);
      data.append("WfDescription", this.EditWorkflow.WfDescription);
      data.append("WfNoOfLevels", this.EditWorkflow.WfNoOfLevels);
      data.append("WfLevelDetails", JSON.stringify(this.EditWorkflow.WfLevelDetails));
      var result = await objFinance.UpdateFinanceWorkflowLevelDetails(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully updated the workflow";
        this.successSnackbarMsg = true;
        this.WorkflowShowDetailsItem.WfLevelDetails = this.EditWorkflow.WfLevelDetails;
        this.WorkflowShowDetailsItem.WfNoOfLevels = this.EditWorkflow.WfNoOfLevels;
        this.GetWorkflowsListToactivity();
        this.editLevel = false;
        this.EditWorkflowNoOfLevelsError = false;
        this.prevNoofLevel =  this.EditWorkflow.WfNoOfLevels;
      }
    },
    //*** Edit Workflow functions ends here ***//

    //*** Create Workflow functions starts here ***//
    /// <summary>
    ///  fnCreateWorkflowLevelInitialize :- Function used to reset create workflow level
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowLevelInitialize() {
      for (var index = 0; index < this.CreateWorkflow.WfNoOfLevels; index++) {
        this.WfNoOfLevelName.push("Level " + (index + 1));
        this.CreateWorkflow.WfLevelDetails.push({
          WfLevel: index + 1,
          usersList: [],
        });
      }
      this.CreateWorkflowSteps = 2;
    },

    /// <summary>
    ///  fnAddLevelCreateWorkflow :- Function used to add a level in create workflow section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnAddLevelCreateWorkflow() {
      this.CreateWorkflow.WfLevelDetails.push({
        WfLevel: parseInt(this.CreateWorkflow.WfNoOfLevels) + 1,
        usersList: [],
      });
      this.CreateWorkflow.WfNoOfLevels =
        parseInt(this.CreateWorkflow.WfNoOfLevels) + 1;
      this.CreateWorkflowSaveButton = true;
    },

    /// <summary>
    ///  fnDeleteLevelCreateWorkflow :- Function used to delete a level of workflow in create workflow section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    fnDeleteLevelCreateWorkflow(i) {
      this.CreateWorkflow.WfNoOfLevels -= 1;
      this.CreateWorkflow.WfLevelDetails = this.CreateWorkflow.WfLevelDetails.filter((x) => x.WfLevel != i);
      if (this.CreateWorkflow.WfNoOfLevels == 0) {
        this.CreateWorkflow.WfLevelDetails = [];
        this.CreateWorkflowSaveButton = true;
      } else {
        this.CreateWorkflow.WfLevelDetails.forEach((u) => {
          if (u.WfLevel > i) u.WfLevel -= 1;
        });
      }
    },

    /// <summary>
    ///  fnCreateWorkflowCloseORCancel :- Function used to close create workflow dialog
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowCloseORCancel() {
      this.CreateWorkFlowDlg = false;
      this.newLevelName = "";
      this.editLevelNameIndex = "";
      this.editLevelNameButton = false;
      this.CreateWorkflowSteps = 1;
      this.CreateWorkflowSaveButton = true;
      this.CreateWorkflow.WfName = "";
      this.CreateWorkflow.WfDescription = "";
      this.CreateWorkflow.WfNoOfLevels = "";
      this.CreateWorkflow.WfLevelDetails = [];
      this.rulesLevel = {};
      this.WfNoOfLevelName = [];
      this.editLevelNameIndex = "";
      this.assignUserEmpty = false;
      this.assignUserEmptyIndex = [];
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    /// <summary>
    ///  fnCreateWorkflowAssignClick :- Function used to open assign user dialog when click on assign/re-assign button
    /// </summary>
    /// <param name="index"></param>
    /// <returns> </returns>
    fnCreateWorkflowAssignClick(index) {
      this.CreateWorkflowAssignDialog = true;
      this.CreateWorkflowAssignLevelClicked = index;
      var temp = this.CreateWorkflow.WfLevelDetails.filter((x) => x.WfLevel == index);
      if (temp.length > 0) {
        this.createWorkflowAssignUsersSelected = temp[0].usersList;
      }
     },

    /// <summary>
    ///  fnCheckCreateWorkflowSaveButton :- Function used to check save button
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCheckCreateWorkflowSaveButton() {
      var savebtn = true;
      for (var index = 0; index < this.CreateWorkflow.WfNoOfLevels && savebtn; index++) {
        if (this.CreateWorkflow.WfLevelDetails[index].usersList.length == 0) {
          savebtn = false;
        }
      }
      if (savebtn) {
        this.CreateWorkflowSaveButton = false;
      }
    },

    /// <summary>
    ///  fnCreateWorkflowAssignSave :- Function used to assign/re-assign users to workflow in create workflow section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowAssignSave() {
      var levelitem = this.CreateWorkflow.WfLevelDetails.filter((x) => x.WfLevel == this.CreateWorkflowAssignLevelClicked);
      if (levelitem.length > 0) {
        //this is for re-assign
        levelitem[0].usersList = this.createWorkflowAssignUsersSelected;
      } else {
        //this is for assign
        this.CreateWorkflow.WfLevelDetails.push({
          WfLevel: this.CreateWorkflowAssignLevelClicked,
          usersList: this.createWorkflowAssignUsersSelected,
        });
      }
      //check whether all levels assigned or not, if assigned Save button enable otherwise no
      this.fnCheckCreateWorkflowSaveButton();
      this.CreateWorkflowAssignDialog = false;
      this.CreateWorkflowAssignLevelClicked = 0;
      this.createWorkflowAssignUsersSelected = [];
    },

    /// <summary>
    ///  fnCreateWorkflowSave :- Function used to add project workFlow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnCreateWorkflowSave() {
      var data = new FormData();
      data.append("WorkspaceID", this.WorkspaceID);
      data.append("ProjectId", this.projectInfo.ProjectId);
      data.append("WfName", this.CreateWorkflow.WfName);
      data.append("WfDescription", this.CreateWorkflow.WfDescription);
      data.append("WfNoOfLevels", this.CreateWorkflow.WfNoOfLevels);
      data.append("WfLevelDetails", JSON.stringify(this.CreateWorkflow.WfLevelDetails));

      var result = await objFinance.AddFinanceWorkflow(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully added the workflow";
        this.successSnackbarMsg = true;
        this.CreateWorkflow.WorkflowID = 0;
        this.CreateWorkflow.WfName = "";
        this.CreateWorkflow.WfDescription = "";
        this.CreateWorkflow.WfNoOfLevels = "";
        this.CreateWorkflow.WfLevelDetails = [];
        this.CreateWorkFlowDlg = false;
        this.CreateWorkflowSteps = 1;
        this.CreateWorkflowSaveButton = true;
        this.resetData();
        this.GetWorkflowsListToactivity();
      } else {
        this.Msg = "Failed";
        this.errorSnackbarMsg = true;
      }
    },

    /// <summary>
    ///  fnRemoveWorkflows :- Function used to open the remove dialog box, print a message if workflow is not selected
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnRemoveWorkflows() {
      this.BulkActionMenu = false;
      this.WorkflowBulkRemoveDialog = false;
      if (this.checkboxWorkflow.length == 0) {
        this.Msg = "Please select the workflows you want to remove";
        this.infoSnackbarMsg = true;
        return;
      }

      var data = new FormData();
      var wfidlist = [];
      for (var i = 0; i < this.filterWorkflowsList.length; i++) {
        if (this.checkboxWorkflow[i]) {
          wfidlist.push(this.filterWorkflowsList[i].WorkflowID);
        }
      }
      var wfids = wfidlist.toString();
      data.append("WorkflowIdList", wfids);

      var result = await objFinance.RemoveFinanceWorkflows(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.status == "success") {
        this.Msg = "Successfully removed";
        this.successSnackbarMsg = true;
        this.checkboxWorkflow = [];
        this.GetWorkflowsListToactivity();
      }
    },
    //*** Create Workflow functions ends here ***//
  },
  watch: {
    /// <summary>
    ///  searchWorkflow :- Function used to search workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    searchWorkflow: function () {
      this.checkboxWorkflow = [];
      if (this.searchWorkflow == "") {
         this.filterWorkflowsList = this.workflowsList;
      }
      else {
         this.filterWorkflowsList = this.workflowsList.filter((x) => x.WfName.includes(this.searchWorkflow));
      }
    },
  },
  components: {
    "Add-Tag-Button": AddTagButton,
    "Save-Button-Success-Small": SaveButtonSuccessSmall,
    "Cancel-Button-Outlined-Sm": CancelButtonOutlinedSm,
    "Next-button-primary-light": NextButtonPrimaryLight,
    "Previous-Button-Outlined": PreviousButtonOutlined,
    "View-Workflow-Button": ViewWorkflowButton,
    "Secondary-Button": SecondaryButton,
    "Default-Button-Outlined": DefaultButtonOutlined,
  },
};
</script>
<style scoped>
.edit-level-text-field {
  border-bottom-style: solid;
  border-bottom-width: 1px;
  padding-left: 5px !important;
}
.edit-level-text-field:focus {
  outline: none !important;
}
.error-level {
  color: red;
  margin-top: 30px;
}
.level-empty {
  color: black;
}
</style>
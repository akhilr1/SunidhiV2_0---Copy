<template>
  <v-card class="elevation-0">
    <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
    <v-container fluid class="pt-0">
      <v-stepper hide v-model="ProjectWorkflowSteps" class="elevation-0">
        <v-stepper-items>
          <!-- Workflows -->
          <v-stepper-content step="1" class="pa-0">
            <v-row>
              <v-col md="auto">
                <h3 class="text-left page-head">
                  <v-avatar
                    color="white"
                    size="20"
                    class="elevation-1 mr-2 mb-1"
                  >
                    <img :src="Workflow_Tab_icon" style="width: 70%" />
                  </v-avatar>
                  Workflow
                </h3>
              </v-col>
              <v-spacer></v-spacer>
              <!-- Top Buttons -->
              <v-col
                class="pt-4 d-flex flex-row"
                v-if="workflowsList.length > 0"
              >
                <!--Create Workflow Button-->
                <v-menu :offset-y="true" v-bind:disabled="!screenAccess">
                  <template v-slot:activator="{ on }">
                    <v-btn
                      height="39"
                      v-on="on"
                      width="160"
                      color="success-btn"
                      dark=""
                      class="elevation-0 mr-2"
                    >
                      Add Workflow
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
                    <!-- Create Workflow-->
                    <v-list-item
                      id="AutoTest_IP857"
                      link=""
                      @click.stop="fnCheckProjectTeam()"
                    >
                      <v-list-item-content>Create Workflow</v-list-item-content>
                    </v-list-item>
                    <!-- Select Workflow-->
                    <!-- <v-list-item
                      id="AutoTest_IP858"
                      link=""
                      @click.stop="SelectWorkflowDialog = true"
                    >
                      <v-list-item-content>Select Workflow</v-list-item-content>
                    </v-list-item> -->
                  </v-list>
                </v-menu>

                <!--Bulk Action -->
                <v-menu v-bind:disabled="
                    checkboxWorkflow.length == 0 || !screenAccess
                  "
                        offset-y=""
                        transition="scroll-y-transition"
                        v-model="BulkActionMenu">
                    <template v-slot:activator="{ on }">
                        <v-btn height="39"
                               v-on="on"
                               color="grey"
                               outlined=""
                               class="elevation-0 mr-2">
                            Bulk Actions
                            <v-icon> mdi-chevron-down </v-icon>
                        </v-btn>
                    </template>
                    <v-list color="white"
                            light=""
                            width="auto"
                            max-width="250"
                            class="action-button-list">
                        <v-list-item id="AutoTest_IP859"
                                     link=""
                                     @click.stop="fnRemoveWorkflows()">
                            <v-list-item-icon>
                                <v-icon>mdi-trash-can-outline</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content>Remove</v-list-item-content>
                        </v-list-item>
                    </v-list>
                    <v-dialog v-model="RemoveBulkWorkflowDailog" width="800">
                        <v-card>
                            <v-card-title primary-title class="page-head py-4">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                    <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                </v-avatar>
                                Remove Workflow
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_MP114" text="" @click="RemoveBulkWorkflowDailog=false">
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
                                                        <h4 class="text-center heading-3">Are you sure you want to remove this Workflows ?</h4>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                        <Secondary-Button id="AutoTest_MP115" class="btn-122x36 mr-2" title="Remove" @click.native="DeleteWorkflow()"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_MP116" class="btn-122x36" title="Cancel" @click.native="CancelWorkflow()"></Default-Button-Outlined>
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
                <v-text-field
                  label="Search"
                  v-model="searchWorkflow"
                  class="mr-2"
                  dense
                  prepend-inner-icon="mdi-magnify"
                  outlined
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
                      height="38"
                      v-on="on"
                      color="grey"
                      outlined=""
                      dark=""
                      class="elevation-0 mr-2 px-0"
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
                      id="AutoTest_IP860"
                      @click.stop="fnWorkflowSort('Name-Ascending')"
                    >
                      <v-list-item-content>
                        Name-Ascending
                      </v-list-item-content>
                    </v-list-item>
                    <v-list-item
                      id="AutoTest_IP861"
                      @click.stop="fnWorkflowSort('Name-Descending')"
                    >
                      <v-list-item-content>
                        Name-Descending
                      </v-list-item-content>
                    </v-list-item>
                    <v-list-item
                      id="AutoTest_IP862"
                      @click.stop="fnWorkflowSort('Recently Added')"
                    >
                      <v-list-item-content>
                        Recently Added
                      </v-list-item-content>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </v-col>
            </v-row>

            <!-- Workflow Card Section-->
            <!-- cards with workflows -->
            <v-row v-if="workflowsList.length > 0" class="pr-3">
              <v-col
                class="v-scrolling-div pt-0"
                style="height: 70vh"
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
                                (ProjectWorkflowSteps = 2)
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
            <!-- cards no workflows -->
            <v-row v-else>
              <v-col cols="12" sm="12" md="12">
                <v-row :justify="justifyCenter">
                    <v-col md="6" class="text-center">
                        <p class="mt-12 pt-12">
                            <img src="..\..\..\src\assets\EmptyPlaceholders\project.svg" height="150" width="150" />
                        </p>

                        <p class="heading-3-dark font-weight-medium pt-6">No workflows available</p>
                        <!-- Create Or Select Workflow Button -->
                        <v-menu offset-y
                                transition="scroll-y-transition"
                                close-on-content-click
                                internal-activator
                                v-bind:disabled="!screenAccess">
                            <template v-slot:activator="{ on }">
                                <v-btn height="32"
                                       v-on="on"
                                       color="success-btn"
                                       dark
                                       class="elevation-0 mr-2">
                                    Add Workflow
                                    <v-icon>mdi-chevron-down</v-icon>
                                </v-btn>
                            </template>
                            <v-list color="white"
                                    light
                                    width="auto"
                                    max-width="250"
                                    class="action-button-list">
                                <v-list-item id="AutoTest_IP865"
                                             link
                                             @click.stop="fnCheckProjectTeam()">
                                    <v-list-item-content>Create Workflow</v-list-item-content>
                                </v-list-item>
                                <!-- <v-list-item
              id="AutoTest_IP866"
              link
              @click.stop="SelectWorkflowDialog = true"
            >
              <v-list-item-content
                >Select Workflow</v-list-item-content
              >
            </v-list-item> -->
                            </v-list>
                        </v-menu>
                    </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-stepper-content>

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

                    <v-tab-item value="tab-3">
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
                      <v-dialog v-model="EditWorkflowAssignDialog" width="900" :retain-focus="false">
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
                                @click="fnEditWorkflowAssignClose()"
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
                                  @click.native="fnEditWorkflowAssignClose()"
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
                    id="AutoTest_IP872"
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
                    id="AutoTest_IP873"
                    class="btn-122x36 mr-2"
                    title="Save"
                    @click.native="fnEditWorkflowDetailsSave()"
                    v-bind:disabled="!screenAccess"
                  ></Secondary-Button>
                  <Default-Button-Outlined
                    id="AutoTest_IP874"
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
                    id="AutoTest_IP875"
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
                  id="AutoTest_IP876"
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
                                                id="AutoTest_IP877"
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
                                            id="AutoTest_IP878"
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
                                            id="AutoTest_IP879"
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
                              id="AutoTest_IP880"
                              class="btn-122x36 mr-2"
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
                              id="AutoTest_IP881"
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
            <v-dialog v-model="EditWorkflowAssignDialog" width="900" :retain-focus="false">
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
                      @click="fnEditWorkflowAssignClose()"
                      v-bind:disabled="!screenAccess"
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
                        @click.native="fnEditWorkflowAssignClose()"
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
                              v-for="i in CreateWorkflow.WfNoOfLevels"
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
                                          class="pb-1 pt-2 pl-9"
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
                              @click.native="fnCreateWorkflowCloseORCancel()"
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
                @click="fnCreateWorkflowAssignClose()"
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
                  @click.native="fnCreateWorkflowAssignClose()"
                ></Default-Button-Outlined>
              </v-col>
            </v-row>
          </v-card>
        </v-col>
      </v-dialog>

      <!-- Select Work flow-->
      <v-dialog v-model="SelectWorkflowDialog" width="800" persistent>
        <v-card>
          <v-card-title primary-title class="page-head pb-0">
            <v-avatar color="white" size="25" class="elevation-1 mr-2">
              <v-icon size="20" color="primary"
                >mdi-hexagon-multiple-outline</v-icon
              > </v-avatar
            >Add Workflow
            <v-spacer></v-spacer>
            <v-btn
              id="AutoTest_IP897"
              text
              @click="
                (SelectWorkflowDialog = false),
                  (checkboxAddWorkflow = []),
                  (selectedAddWorkflow = [])
              "
            >
              <v-icon color="#707070" size="15">mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text class="py-0">
            <v-container fluid class="pt-0">
              <v-row no-gutters>
                <v-col cols="12" md="12" class="pt-0">
                  <v-stepper
                    v-model="SelectWorkflowDlgSteps"
                    class="elevation-0"
                  >
                    <v-stepper-items>
                      <!-- Select Workflow step-->
                      <v-stepper-content step="1" class="pa-0">
                        <v-row no-gutters>
                          <v-col cols="12" md="8" class="pt-1">
                            <h3 class="text-left page-head">
                              <v-avatar
                                color="white"
                                size="20"
                                class="elevation-1 mr-2 mb-1"
                              >
                                <v-icon size="15" color="primary"
                                  >mdi-folder-key-network-outline</v-icon
                                >
                              </v-avatar>
                              WorkFlow
                            </h3>
                          </v-col>
                          <v-col
                            v-if="SelectWorkflowDlgItems.length > 0"
                            cols="12"
                            md="12"
                            class="pt-0 mb-2"
                          >
                            <h4 class="text-center heading-3">
                              Please select workflow from below
                            </h4>
                          </v-col>
                        </v-row>
                        <!-- if workflow available -->
                        <v-row
                          no-gutters
                          v-if="SelectWorkflowDlgItems.length > 0"
                        >
                          <!-- Work flow group chip-->
                          <v-col md="12" class="py-0">
                            <div class="chip-container mb-2">
                              <v-chip-group
                                multiple
                                column
                                active-class="primary--text"
                              >
                                <v-chip
                                  id="AutoTest_IP898"
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
                          <v-row style="overflow-y: scroll; height: 45vh">
                            <v-col
                              xs="6"
                              sm="6"
                              md="6"
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
                                          src="../../images/WorkSpace-dummy-icon.png"
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
                                    <v-checkbox
                                      v-model="checkboxAddWorkflow[index]"
                                      @change="fnCheck(addWorkflowItem, index)"
                                      label=""
                                    ></v-checkbox>
                                  </v-col>
                                </v-row>
                                <!--Description-->
                                <!--Description-->
                                <v-row>
                                  <v-col class="py-0" :ripple="false">
                                    <span class="title small-text">
                                      <v-icon size="12"
                                        >mdi-folder-key-network-outline</v-icon
                                      >
                                      Description
                                    </span>
                                    <p
                                      style="overflow-y: auto; height: 100px"
                                      class="ml-3"
                                    >
                                      {{ addWorkflowItem.WfDescription }}
                                    </p>
                                  </v-col>
                                </v-row>
                                <!-- Levels-->
                                <v-row>
                                  <v-col class="py-0">
                                    <span class="title small-text">
                                      <v-icon size="12">mdi-stairs</v-icon>
                                      Level : {{ addWorkflowItem.WfNoOfLevels }}
                                    </span>
                                  </v-col>
                                </v-row>
                                <!-- buttons-->
                                <v-row :justify="justifyCenter">
                                  <v-col md="6">
                                    <v-btn
                                      id="AutoTest_IP899"
                                      color="success-btn elevation-0 enter-btn"
                                      dark
                                      @click="
                                        (SelectWorkflowDlgSteps = 2),
                                          (ViewWorkflowItem = addWorkflowItem)
                                      "
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
                            class="text-right py-0 mb-0"
                          >
                            <v-btn
                              id="AutoTest_IP900"
                              v-bind:disabled="selectedAddWorkflow.length == 0"
                              color="secondary"
                              class="mr-2 elevation-0"
                              @click="fnAddSelectWorkflow()"
                              >Confirm</v-btn
                            >
                            <v-btn
                              id="AutoTest_IP901"
                              outlined
                              color="grey"
                              @click="
                                (SelectWorkflowDialog = false),
                                  (checkboxAddWorkflow = []),
                                  (selectedAddWorkflow = [])
                              "
                              >Cancel</v-btn
                            >
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
                                          Please create a new workflow.
                                        </span>
                                      </p>
                                    </v-list-item-content>
                                  </v-list-item>
                                </v-list>
                              </v-col>
                            </v-row>
                          </v-col>
                          <v-col cols="12" sm="12" md="12">
                            <v-row :justify="justifyCenter">
                              <v-col md="6" class="text-center">
                                <v-avatar
                                  size="60"
                                  tile
                                  class="elevation-1 base-border-radius mr-2"
                                >
                                  <v-img
                                    src="../../images/ic_no_workspace.svg"
                                    height="60"
                                    width="60"
                                    class="ma-4"
                                  ></v-img>
                                </v-avatar>
                                <p class="heading-3 pt-3">
                                  No workflow available
                                </p>
                              </v-col>
                            </v-row>
                          </v-col>
                          <v-col cols="12" xs="12" sm="12" md="12">
                            <v-row :justify="justifyCenter">
                              <v-col md="6" class="text-center">
                                <v-btn
                                  id="AutoTest_IP902"
                                  @click="
                                    (SelectWorkflowDialog = false),
                                      (CreateWorkFlowDlg = true)
                                  "
                                  height="32"
                                  color="success-btn"
                                  dark
                                  class="elevation-0 mr-2"
                                >
                                  Create Workflow
                                </v-btn>
                              </v-col>
                            </v-row>
                          </v-col>
                        </v-row>
                      </v-stepper-content>

                      <!-- Show Details of Workflow-->
                      <v-stepper-content step="2" class="pa-0">
                        <v-container fluid class="pt-0">
                          <v-col>
                            <h3 class="text-left page-head">
                              <v-avatar
                                color="white"
                                size="20"
                                class="elevation-1 mr-2 mb-1"
                              >
                                <v-icon size="15" color="primary"
                                  >mdi-folder-key-network-outline</v-icon
                                > </v-avatar
                              ><span
                                id="AutoTest_IP903"
                                style="cursor: pointer"
                                @click="SelectWorkflowDlgSteps = 1"
                                >Workflow</span
                              >
                              <v-icon size="20">mdi-chevron-right</v-icon>
                              <span>Workflow Details</span>
                              <v-icon size="20">mdi-chevron-right</v-icon
                              >{{ ViewWorkflowItem.WfName }}
                            </h3>
                          </v-col>
                          <!-- Work flow group chip-->
                          <v-col md="12" class="py-0">
                            <div class="chip-container mb-2">
                              <v-chip-group
                                multiple
                                column
                                active-class="primary--text"
                              >
                                <v-chip
                                  id="AutoTest_IP904"
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
                          <v-row
                            style="
                              overflow-y: scroll;
                              overflow-x: hidden;
                              height: 45vh;
                            "
                            justify="center"
                          >
                            <v-col
                              md="8"
                              v-for="i in ViewWorkflowItem.WfNoOfLevels"
                              :key="i"
                            >
                              <v-card class="workspace-card" flat>
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
                                          (100 /
                                            ViewWorkflowItem.WfNoOfLevels) *
                                          i
                                        "
                                        color="#1EC2ED"
                                      >
                                        <span class="subtitle-1 text--primary">
                                          {{ i }}<br />
                                          <span
                                            class="subtitle-2 text--disabled"
                                            >Level</span
                                          >
                                        </span>
                                      </v-progress-circular>
                                    </v-col>
                                  </v-row>
                                  <v-divider vertical class="mt-3"></v-divider>
                                  <v-col cols="6" md="6" class="text-left">
                                    <!---   if there is no data-->
                                    <v-row
                                      v-if="
                                        ViewWorkflowItem.WfLevelDetails
                                          .length == 0 ||
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
                                          <!--<v-avatar
                                            v-if="
                                              ViewWorkflowItem.WfLevelDetails[
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
                                              ViewWorkflowItem.WfLevelDetails[
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
                                              ViewWorkflowItem.WfLevelDetails[
                                                i - 1
                                              ].usersList.length > 2
                                            "
                                            size="30"
                                            class="team-count"
                                            color="red"
                                          >
                                            +{{
                                              ViewWorkflowItem.WfLevelDetails[
                                                i - 1
                                              ].usersList.length
                                            }}
                                          </v-avatar>-->
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
                            class="text-right py-0 mb-0"
                          >
                            <v-btn
                              id="AutoTest_IP905"
                              color="secondary"
                              class="mr-2 elevation-0"
                              @click="fnAddSelectWorkflow()"
                              >Confirm</v-btn
                            >
                            <v-btn
                              id="AutoTest_IP906"
                              outlined
                              color="grey"
                              @click="SelectWorkflowDlgSteps = 1"
                              >Previous</v-btn
                            >
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
import PreLoader from "@/components/pre-loader.vue";
import objUtils from '../../utils.js';

var objManageProject;
async function importscript() {
  return Promise.all([
    import("../../BL/ManageProject.js").then((mod) => {
      objManageProject = new mod.ManageProject();
    }),
  ]);
}
export default {
  data() {
    return {
      EditAssignError : false,
      prevNoofLevel:"",
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
      RemoveBulkWorkflowDailog : false,
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
      screenAccess: 0,
      RWAccess: 0,
      successSnackbarMsg: false,
      errorSnackbarMsg: false,
      infoSnackbarMsg: false,
      Msg: "",
      justifyCenter: "center",
      WorkspaceID: 0,
      PreLoader: false,
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
      prevEditWorkflow: {
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
      EditWfNoOfLevelName: ["Level 1", "Level 2"],
      WfNoOfLevelName: [],
      CreateWorkflowSaveButton: true,
      CreateWorkflowAssignDialog: false,
      createWorkflowAssignUsersSelected: [],
      CreateWorkflowAssignLevelClicked: 0,
      //Create workflow ends here
      //Select workflow starts here
      SelectWorkflowDialog: false,
      SelectWorkflowDlgSteps: 0,
      SelectWorkflowDlgItems: [],
      checkboxAddWorkflow: [],
      selectedAddWorkflow: [],
      //step 2 of Select Workflow
      ViewWorkflowItem: { WfLevelDetails: [] },
      //Select workflow ends here
      userimage: "",
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
    //not sure whether this is needed await this.fnGetInitiativeDetailsByID();
    this.PreLoader = true;
    await this.fnGetProjectDetailsByID();
    await this.fnGetUsersListByGroupId();
    await this.fnGetWorkflowsListByProjectID();
    await this.fnGetWorkflowsListByIntitiativeID();
    this.PreLoader = false;
  },
  methods: {
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
      this.ActivityTab = "tab-3";
      this.EditWorkflow.WfName = item.WfName;
      this.WorkflowShowDetailsItem.WorkflowID = item.WorkflowID;
      this.WorkflowShowDetailsItem.WfName = item.WfName;
      this.WorkflowShowDetailsItem.WfDescription = item.WfDescription;
      this.WorkflowShowDetailsItem.WfNoOfLevels = item.WfNoOfLevels;
      this.WorkflowShowDetailsItem.WfLevelDetails = item.WfLevelDetails;
      this.prevEditWorkflow.WfName = item.WfName;
      this.prevEditWorkflow.WorkflowID = item.WorkflowID;
      this.prevEditWorkflow.WfName = item.WfName;
      this.prevEditWorkflow.WfDescription = item.WfDescription;
      this.prevEditWorkflow.WfNoOfLevels = item.WfNoOfLevels;
      this.prevEditWorkflow.WfLevelDetails = item.WfLevelDetails;
      this.prevNoofLevel = item.WfNoOfLevels;
      if (item.WfLevelDetails.length > 0) { 
          this.EditWfNoOfLevelName = [];
            for (var i = 0; i < item.WfLevelDetails.length; i++) { 
                this.EditWfNoOfLevelName.push(item.WfLevelDetails[i].workflowlevelname);
            }
        }
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
        this.projectInfo.ProjectTagsList = JSON.parse(
          this.projectInfo.ProjectTags
        );
      } else {
        this.Msg =
          ("Failed to get details of Project ", this.projectInfo.InitiativeId);
          this.errorSnackbarMsg= true;
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
        //alert("WorkflowAssignUsersTableItems=", this.WorkflowAssignUsersTableItems.length);
      }
    },

    /// <summary>
    ///  fnCheckProjectTeam :- Function used to check if any team users are exist under the project to create a workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCheckProjectTeam() {
      if (this.projectInfo.GroupId && this.WorkflowAssignUsersTableItems.length > 0)
        this.CreateWorkFlowDlg = true;
      else {
        this.Msg = "No users found under this project. Please add the team users for this project to continue creating a workflow. ";
          this.infoSnackbarMsg = true;
      }
    },

    /// <summary>
    ///  fnGetWorkflowsListByProjectID :- Function used to fetch project workflow details based on ProjectId,WorkspaceID
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnGetWorkflowsListByProjectID() {
      var data = new FormData();
      data.append("ProjectId", this.projectInfo.ProjectId);
      data.append("WorkspaceId", this.WorkspaceID);
      var result = await objManageProject.GetWorkflowsListByProjectID(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.workflowsList = result.data;
        this.filterWorkflowsList = result.data;
        //alert("worflow list=" + this.filterWorkflowsList.length);
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
          this.filterWorkflowsList.sort((a, b) =>
            a.WfName > b.WfName ? 1 : -1
          );
          break;
        case "Name-Descending":
          this.filterWorkflowsList.sort((a, b) =>
            a.WfName < b.WfName ? 1 : -1
          );
          break;
        case "Recently Added":
          this.filterWorkflowsList.sort((a, b) =>
            a.WorkflowID < b.WorkflowID ? 1 : -1
          );
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
      this.EditWorkflow.WfLevelDetails = this.EditWorkflow.WfLevelDetails.filter((val,i)=>i< this.prevNoofLevel)
      this.EditWorkflow.WfNoOfLevels =this.prevNoofLevel
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
    ///  fnDeleteLevelEditWorkflowError :- Function used to print a warning message when deleting a level in edit section
    ///  Minimum 1 level is required for a workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnDeleteLevelEditWorkflowError() {
      this.EditWorkflowNoOfLevelsError = true;
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
    ///  fnEditWorkflowAssignClose :- Function used to close assign user dialog
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnEditWorkflowAssignClose() {
      this.EditWorkflowAssignDialog = false;
      this.EditWorkflowAssignLevelClicked = 0;
      this.editWorkflowAssignUsersSelected = [];
    },

    /// <summary>
    ///  fnEditWorkflowAssignSave :- Function used to save users while assign/re-assign 
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnEditWorkflowAssignSave() {
      var temp = this.EditWorkflow.WfLevelDetails.filter(
        (x) => x.WfLevel == this.EditWorkflowAssignLevelClicked
      );
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
      var result = await objManageProject.UpdateProjectWorkflowDetails(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully updated the workflow details";
        this.successSnackbarMsg = true;
        this.WorkflowShowDetailsItem.WfName = this.EditWorkflow.WfName;
        this.WorkflowShowDetailsItem.WfDescription = this.EditWorkflow.WfDescription;
        this.EditWorkflow.WorkflowID = 0;
        this.EditWorkflow.WfName = "";
        this.EditWorkflow.WfDescription = "";
        this.fnGetWorkflowsListByProjectID();
      }
      this.EditWorkflowDetailsEdit = false;
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
      data.append("WfLevelDetails",JSON.stringify(this.EditWorkflow.WfLevelDetails));
      var result = await objManageProject.UpdateProjectWorkflowLevelDetails(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully updated the workflow details";
        this.successSnackbarMsg = true;
        this.WorkflowShowDetailsItem.WfLevelDetails = this.EditWorkflow.WfLevelDetails;
        this.WorkflowShowDetailsItem.WfNoOfLevels = this.EditWorkflow.WfNoOfLevels;
        this.editLevel = false;
        this.fnGetWorkflowsListByProjectID();
        this.EditWorkflowNoOfLevelsError = false;
        this.prevNoofLevel =  this.EditWorkflow.WfNoOfLevels;
      }
    },
    //*** Edit Workflow functions ends here ***//

    //*** Create Workflow functions starts here ***//

    /// <summary>
    ///  editManageLevelNameHandler :- Function used to handle level name in manage edit section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    editManageLevelNameHandler(i) {
      this.newLevelName = this.EditWfNoOfLevelName[i - 1];
      this.editLevelNameIndex = i;
      this.editLevelNameButton = true;
    },

    editLevelSaveNameHandler(i) {
      if (this.newLevelName != "") {
        this.WfNoOfLevelName[i] = this.newLevelName;
        this.CreateWorkflow.WfLevelDetails[i].workflowlevelname = this.newLevelName;
      }
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    editLevelManageSaveNameHandler(i) {
      if (this.newLevelName != "") {
        this.EditWfNoOfLevelName[i] = this.newLevelName;
        this.EditWorkflow.WfLevelDetails[i].workflowlevelname = this.newLevelName;
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
      //this.rulesLevel = {};
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
          this.CreateWorkflowSteps = 2;
          this.CreateWorkflow.WfLevelDetails = [];
          this.fnCreateWorkflowLevelInitialize();
        }
      });
    },

    /// <summary>
    ///  fnCreateWorkflowLevelInitialize :- Function used to reset create workflow level
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowLevelInitialize() {
      this.CreateWorkflow.WfNoOfLevels = parseInt(
        this.CreateWorkflow.WfNoOfLevels
      );
      for (var i = 1; i <= this.CreateWorkflow.WfNoOfLevels; i++) {
        this.WfNoOfLevelName.push("Level " + i);
      }
      for (var index = 0; index < this.CreateWorkflow.WfNoOfLevels; index++) {
        this.CreateWorkflow.WfLevelDetails.push({
          WfLevel: index + 1,
          usersList: [],
        });
      }
    },

    /// <summary>
    ///  fnAddLevelCreateWorkflow :- Function used to add a level in create workflow section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnAddLevelCreateWorkflow() {
      var nextLevel = this.CreateWorkflow.WfNoOfLevels + 1;
      this.WfNoOfLevelName[this.CreateWorkflow.WfNoOfLevels] =
        "Level " + nextLevel;
      this.CreateWorkflow.WfLevelDetails.push({
        WfLevel: this.CreateWorkflow.WfNoOfLevels + 1,
        usersList: [],
      });
      this.CreateWorkflow.WfNoOfLevels += 1;
      this.CreateWorkflowSaveButton = true;
    },

    /// <summary>
    ///  fnDeleteLevelCreateWorkflow :- Function used to delete a level of workflow in create workflow section
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    fnDeleteLevelCreateWorkflow(i) {
      this.CreateWorkflow.WfNoOfLevels -= 1;
      this.CreateWorkflow.WfLevelDetails = this.CreateWorkflow.WfLevelDetails.filter(
        (x) => x.WfLevel != i
      );
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
    ///  fnDeleteLevelCreateWorkflowError :- Function used to print a warning message when deleting a level of workflow in create workflow section
    ///  Minimum 1 level is required for a workflow
    /// </summary>
    /// <param name="i"></param>
    /// <returns> </returns>
    fnDeleteLevelCreateWorkflowError(i) {
      this.CreateWorkflowNoOfLevelsError = true;
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
      //this.rulesLevel = {};
      this.WfNoOfLevelName = [];
      this.editLevelNameIndex = "";
      this.assignUserEmpty = false;
      this.assignUserEmptyIndex = [];
      this.newLevelName = "";
      this.editLevelNameButton = false;
    },

    /// <summary>
    ///  fnCreateWorkflowAssignClose :- Function used to close assign users dialog in create workflow section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowAssignClose() {
      this.CreateWorkflowAssignDialog = false;
      this.CreateWorkflowAssignLevelClicked = 0;
      this.createWorkflowAssignUsersSelected = [];
    },

    /// <summary>
    ///  fnCreateWorkflowAssignClick :- Function used to open assign user dialog when click on assign/re-assign button
    /// </summary>
    /// <param name="index"></param>
    /// <returns> </returns>
    fnCreateWorkflowAssignClick(index) {
      this.CreateWorkflowAssignDialog = true;
      this.CreateWorkflowAssignLevelClicked = index;
      var temp = this.CreateWorkflow.WfLevelDetails.filter(
        (x) => x.WfLevel == index
      );
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
      if (savebtn) this.CreateWorkflowSaveButton = false;
    },

    /// <summary>
    ///  fnCreateWorkflowAssignSave :- Function used to assign/re-assign users to workflow in create workflow section
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fnCreateWorkflowAssignSave() {
      var levelitem = this.CreateWorkflow.WfLevelDetails.filter(
        (x) => x.WfLevel == this.CreateWorkflowAssignLevelClicked
      );
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
    ///  fncheckAssignIsEditLevelNotEmpty :- Function used to check assign user box is not empty while editing a workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    fncheckAssignIsEditLevelNotEmpty() {
      this.EditAssignError = false;
      for (var i = 0; i < this.EditWorkflow.WfNoOfLevels; i++) {
        if (this.EditWorkflow.WfLevelDetails[i].usersList.length == 0) {
          this.assignUserEmptyIndex.push(i + 1);
          this.EditAssignError = true;
        } else {
          if (this.assignUserEmptyIndex.includes(i + 1)) {
            this.assignUserEmptyIndex = this.assignUserEmptyIndex.filter(
              (v) => v != i + 1
            );
          }
        }
      }
      if (this.EditAssignError == false) {
        this.fnEditWorkflowLevelSave()
      }
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
            this.assignUserEmptyIndex = this.assignUserEmptyIndex.filter(
              (v) => v != i + 1
            );
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
      data.append(
        "WfLevelDetails",
        JSON.stringify(this.CreateWorkflow.WfLevelDetails)
      );
      var result = await objManageProject.AddProjectWorkflow(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if ((result.status = "success")) {
        this.Msg = "Successfully added the workflow";
        this.successSnackbarMsg = true;
        this.CreateWorkflow.WorkflowID = 0;
        this.CreateWorkflow.WfName = "";
        this.CreateWorkflow.WfDescription = "";
        this.CreateWorkflow.WfNoOfLevels = "";
        this.WfNoOfLevelName = [];
        this.CreateWorkflow.WfLevelDetails = [];
        this.CreateWorkFlowDlg = false;
        this.CreateWorkflowSteps = 1;
        this.CreateWorkflowSaveButton = true;
        this.fnGetWorkflowsListByProjectID();
        //this.rulesLevel = {};
        this.CreateWorkflow.WfNoOfLevels = "";
      } else {
        this.Msg = "Failed to add the workflow ";
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
          this.RemoveBulkWorkflowDailog = false;
          if (this.checkboxWorkflow.length == 0) {
              this.Msg = "Please select the workflows you want to remove";
              this.infoSnackbarMsg = true;
              return;
          }
          else {
              this.RemoveBulkWorkflowDailog = true;
          }
          
    },

    /// <summary>
    ///  DeleteWorkflow :- Function used to delete project workflow & project workflow level
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async DeleteWorkflow() {
          
            var data = new FormData();
            var wfidlist = [];
              for (var i = 0; i < this.filterWorkflowsList.length; i++) {
                if (this.checkboxWorkflow[i]) {
                  wfidlist.push(this.filterWorkflowsList[i].WorkflowID);
                }
              }
             var wfids = wfidlist.toString();
            data.append("WorkflowIdList", wfids);
            var result = await objManageProject.RemoveProjectWorkflows(data);
          if (result.status == "Unhandled Exception") {
              this.$router.push({ name: "error" });
          } else if (result.status == "success") {
             // this.Msg = "Successfully removed" + wfids;
              this.Msg = "Successfully removed";
              this.successSnackbarMsg = true;
              this.checkboxWorkflow = [];
              this.fnGetWorkflowsListByProjectID();
              this.RemoveBulkWorkflowDailog = false;
          }
          else if (result.status == "workflowexists") {
              this.Msg = "Workflow already assigned, cannot be remove";
              this.errorSnackbarMsg = true;
              this.checkboxWorkflow = [];
              this.fnGetWorkflowsListByProjectID();
          }

    },

    /// <summary>
    ///  CancelWorkflow :- Function used to cancel the process of bulk removal of project workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async CancelWorkflow() {          
          this.checkboxWorkflow = [];
          this.fnGetWorkflowsListByProjectID();
          this.RemoveBulkWorkflowDailog = false;
    },

    //*** Create Workflow functions ends here ***//

    //*** Select Workflow functions starts here ***//
    /// <summary>
    ///  fnGetWorkflowsListByIntitiativeID :- Function used to fetch initiative workflow details based on InitiaveId,WorkspaceID
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnGetWorkflowsListByIntitiativeID() {
      var data = new FormData();
      data.append("InitiativeId", this.initiativeInfo.InitiativeId);
      data.append("WorkspaceId", this.WorkspaceID);
      var result = await objManageProject.GetWorkflowsListByIntitiativeID(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.data) {
        this.SelectWorkflowDlgItems = result.data;
        //alert("worflow list=" + this.SelectWorkflowDlgItems.length);
        this.fnFilterWorkflowBasedOnUser();
      }
    },

    /// <summary>
    ///  fnCheck :- Function used to check selected workflow
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <returns> </returns>
    fnCheck(item, index) {
      if (this.checkboxAddWorkflow[index]) {
        this.selectedAddWorkflow.push({ checkboxindex: index, WFitem: item });
      } else {
        if (this.selectedAddWorkflow.length > 0) {
          this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
            (x) => x.WFitem.WorkflowID != item.WorkflowID
          );
        }
      }
    },

    /// <summary>
    ///  fnRemove :- Function used to remove workflow
    /// </summary>
    /// <param name="item"></param>
    /// <returns> </returns>
    fnRemove(item) {
      this.checkboxAddWorkflow[item.checkboxindex] = false;
      this.selectedAddWorkflow = this.selectedAddWorkflow.filter(
        (x) => x.WFitem.WorkflowID != item.WFitem.WorkflowID
      );
    },

    /// <summary>
    ///  fnAddSelectWorkflow :- Function used to add selected project workFlow & add project workFlow level 
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnAddSelectWorkflow() {
      if (this.selectedAddWorkflow.length == 0) {
        this.Msg("Please select the workflow you want to add");
        this.infoSnackbarMsg = true;
        return;
      }

      var workflowlist = [];
      for (var i = 0; i < this.selectedAddWorkflow.length; i++) {
        workflowlist.push(this.selectedAddWorkflow[i].WFitem);
      }
      var data = new FormData();
      data.append("WorkflowList", JSON.stringify(workflowlist));
      data.append("ProjectId", this.projectInfo.ProjectId);
      data.append("WorkspaceID", this.WorkspaceID);
      var result = await objManageProject.AddSelectWorkflow(data);
      if (result.status == "Unhandled Exception") {
        this.$router.push({ name: "error" });
      } else if (result.status == "success") {
        this.Msg = "Successfully added the workflows you slected";
        this.successSnackbarMsg = true;
        this.SelectWorkflowDialog = false;
        this.SelectWorkflowDlgSteps = 1;
        this.SelectWorkflowDlgItems = [];
        this.checkboxAddWorkflow = [];
        this.selectedAddWorkflow = [];
        this.fnGetWorkflowsListByProjectID();
      }
    },
    //*** Select Workflow functions ends here ***//
    // workflow filter
    /// <summary>
    ///  fnFilterWorkflowBasedOnUser :- Function used to filter workflow based on user
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    async fnFilterWorkflowBasedOnUser() {
      var ID = 0;
      var userId = 0;
      var WorkFlowID = 0;
      var WorkflowLevel = 0;
      var UserListID = 0;
      var find = false;
      var RemovingWorflow = [];
      for (WorkFlowID = 0; WorkFlowID <= this.SelectWorkflowDlgItems.length - 1; WorkFlowID++) {
        for (WorkflowLevel = 0; WorkflowLevel <= this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails.length - 1; WorkflowLevel++) {
          // level
          for (UserListID = 0; UserListID <= this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[WorkflowLevel].usersList.length - 1; UserListID++) {
            // user
            ID = this.SelectWorkflowDlgItems[WorkFlowID].WfLevelDetails[WorkflowLevel].usersList[UserListID].ID;
            for (userId = 0; userId <= this.WorkflowAssignUsersTableItems.length - 1; userId++) {
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
  },

  watch: {
    /// <summary>
    ///  searchWorkflow :- Function used to search workflow
    /// </summary>
    /// <param name=""></param>
    /// <returns> </returns>
    searchWorkflow: function () {
      this.checkboxWorkflow = [];
      if (this.searchWorkflow == "")
        this.filterWorkflowsList = this.workflowsList;
      else
        this.filterWorkflowsList = this.workflowsList.filter((x) =>
          x.WfName.includes(this.searchWorkflow)
        );
    },
    "CreateWorkflow.WfName"(val) {
      this.rules = [];
    },
    "CreateWorkflow.WfDescription"(val) {
      this.rules = [];
    },
    "CreateWorkflow.WfNoOfLevels"(val) {
      this.rules = [];
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
    "Pre-Loader": PreLoader,
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
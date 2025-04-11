<template>
    <v-card class="elevation-0">
        <v-container fluid class="pt-0">
            <v-stepper hide v-model="ProjectWorkflowSteps" class="elevation-0">
                <v-stepper-items>
                    <!-- Workflows -->
                    <v-stepper-content step="1" class="pa-0">
                        <v-row>
                            <v-col>
                                <h3 class="text-left page-head">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> Workflow
                                </h3>
                            </v-col>
                        </v-row>
                        <v-col v-if="workflowsList.length>0" cols="12" xs="12" sm="12" md="12" class="pt-0">
                            <v-row class="text-right">
                                <v-spacer></v-spacer>
                                <!--Create Workflow Button-->
                                <v-menu :offset-y=true>
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" width="160" color="success-btn" dark class="elevation-0 mr-2">
                                            Create Workflow
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I194" link @click.stop="CreateWorkFlowDlg = true">
                                            <v-list-item-icon>
                                                <v-icon>mdi-plus-circle-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Create Workflow</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Bulk Action -->
                                <v-menu v-bind:disabled="checkboxWorkflow.length==0" offset-y transition="scroll-y-transition" v-model="BulkActionMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn height="32" v-on="on" color="grey" outlined="" class="elevation-0 mr-2">
                                            Bulk Actions
                                            <v-icon>
                                                mdi-chevron-down
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item id="AutoTest_I195" link @click.stop="WorkflowBulkRemoveDialog=true">
                                            <v-list-item-icon>
                                                <v-icon>mdi-trash-can-outline</v-icon>
                                            </v-list-item-icon>
                                            <v-list-item-content>Remove</v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>

                                <!--Search-->
                                <div class="small-text-field">
                                    <v-text-field label="Search" v-model="searchWorkflow" class="mr-2"
                                                  dense prepend-inner-icon="mdi-magnify"
                                                  outlined></v-text-field>
                                </div>

                                <!-- Sorting Menu-->
                                <v-menu offset-y transition="scroll-y-transition" v-model="workflowSortMenu">
                                    <template v-slot:activator="{ on }">
                                        <v-btn min-width="40" height="32" v-on="on" color="grey" outlined="" dark="" class="elevation-0 mr-2 px-0">
                                            <v-icon>mdi-tune-vertical</v-icon>
                                        </v-btn>
                                    </template>
                                    <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                                        <v-list-item>
                                            <v-list-item-content>
                                                Sort by :
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I196" @click.stop="fnWorkflowSort('Name-Ascending')">
                                            <v-list-item-content>
                                                Name-Ascending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I197" @click.stop="fnWorkflowSort('Name-Descending')">
                                            <v-list-item-content>
                                                Name-Descending
                                            </v-list-item-content>
                                        </v-list-item>
                                        <v-list-item id="AutoTest_I198" @click.stop="fnWorkflowSort('Recently Added')">
                                            <v-list-item-content>
                                                Recently Added
                                            </v-list-item-content>
                                        </v-list-item>
                                    </v-list>
                                </v-menu>
                            </v-row>
                        </v-col>

                        <!-- Workflow Card Section-->
                        <!-- cards with workflows -->
                        <v-row v-if="workflowsList.length>0">
                            <v-col cols="12" xs="12" sm="12" md="12">
                                <v-row style="overflow-y:scroll;height:600px;overflow-x:hidden">
                                    <v-col xs="6" sm="6" md="3" v-for="(WFitem,index) in filterWorkflowsList" :key="WFitem.WorkflowID">
                                        <v-card class="workspace-card" flat>
                                            <v-row>
                                                <v-col xs="6" sm="6" md="10" class="d-inline-flex pt-0" :ripple="false">
                                                    <div>
                                                        <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                            <v-img src="../../images/WorkSpace-dummy-icon.png" height="40" width="40" class="ma-4"></v-img>
                                                        </v-avatar>
                                                    </div>
                                                    <div class="title pt-4 px-2 text-limit">
                                                        {{ WFitem.WfName }}
                                                    </div>
                                                </v-col>
                                                <v-col xs="2" sm="2" md="2" class="d-inline-flex pt-0 text-right">
                                                    <v-checkbox input-value="true" v-model="checkboxWorkflow[index]" label=""></v-checkbox>
                                                </v-col>
                                            </v-row>
                                            <!--Description-->
                                            <v-row>
                                                <v-col class="py-0" :ripple="false">
                                                    <span class="title small-text">
                                                        <v-icon size="12">mdi-folder-key-network-outline</v-icon>
                                                        Description
                                                    </span>
                                                    <p style="overflow-y :auto; height:100px" class="ml-3">{{ WFitem.WfDescription }}</p>
                                                </v-col>
                                            </v-row>
                                            <!-- Levels-->
                                            <v-row>
                                                <v-col class="py-0">
                                                    <span class="title small-text">
                                                        <v-icon size="12">mdi-tag</v-icon>
                                                        Levels : {{ WFitem.WfNoOfLevels }}
                                                    </span>
                                                </v-col>
                                            </v-row>
                                            <!--Buttons-->
                                            <v-row>
                                                <v-col>
                                                    <v-btn id="AutoTest_I199" color="success-btn elevation-0 enter-btn" block dark @click="fnSetWorkflowShowDetailsItem(WFitem),
                                                                                                              EditWorkflow.WorkflowID=WorkflowShowDetailsItem.WorkflowID,
                                                                                                              EditWorkflow.WfName=WorkflowShowDetailsItem.WfName,
                                                                                                              EditWorkflow.WfDescription=WorkflowShowDetailsItem.WfDescription,
                                                                                                              EditWorkflow.WfNoOfLevels=WorkflowShowDetailsItem.WfNoOfLevels,
                                                                                                              EditWorkflow.WfLevelDetails=WorkflowShowDetailsItem.WfLevelDetails,
                                                                                                              ProjectWorkflowSteps=4,
                                                                                                              fnCheckEditWorkflowSaveButton()">
                                                        View Workflow
                                                    </v-btn>
                                                </v-col>
                                                <v-col>
                                                    <v-btn id="AutoTest_I200" color="normal elevation-0" class="manage-btn" block @click="fnSetWorkflowShowDetailsItem(WFitem),ProjectWorkflowSteps=2">
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
                                        <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                            <v-img src="../../images/ic_no_workspace.svg" height="60" width="60"></v-img>
                                        </v-avatar>
                                        <p class="pt-1">No Workflows available</p>
                                        <v-btn id="AutoTest_I201" height="32" width="160" color="success-btn" dark class="elevation-0 mr-2"
                                               @click="CreateWorkFlowDlg = true">
                                            Create Workflow
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>

                        <!--Workflow Bulk Remove Dialog-->
                        <v-dialog v-model="WorkflowBulkRemoveDialog" width="800">
                            <v-card>
                                <v-card-title primary-title class="page-head">
                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                                    </v-avatar>
                                    Delete Themes from Initiative
                                    <v-spacer></v-spacer>
                                    <v-btn id="AutoTest_I202" text="" @click="WorkflowBulkRemoveDialog=false">
                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                    </v-btn>
                                </v-card-title>
                                <v-card-text class="pt-0">
                                    <v-container fluid="" class="pt-0">
                                        <v-row>
                                            <v-col cols="12" md="12" class="pt-0">
                                                <h4 class="text-center heading-3">
                                                    Are you sure you want to remove themes from this initiative?
                                                </h4>
                                            </v-col>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                                <v-btn id="AutoTest_I203" color="secondary" class="mr-2 elevation-0" @click="fnRemoveWorkflows()">Delete</v-btn>
                                                <v-btn id="AutoTest_I204" outlined color="grey" @click="WorkflowBulkRemoveDialog=false">Cancel</v-btn>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                            </v-card>
                        </v-dialog>
                    </v-stepper-content>

                    <!-- Workflow Show Details -->
                    <v-stepper-content step="2" class="pa-0">
                        <!-- Workflow Header -->
                        <v-row>
                            <v-col>
                                <h3 class="text-left page-head">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> <span id="AutoTest_I205" style="cursor:pointer" @click="fnResetWorkflowShowDetailsItem(),ProjectWorkflowSteps=1">Workflow</span>
                                    > Workflow Details
                                </h3>
                            </v-col>
                        </v-row>
                        <v-row no-gutters :justify="justifyCenter">
                            <v-col xs="6" sm="6" md="4">
                                <v-card class="workspace-card elevation-0" flat>
                                    <v-row>
                                        <v-col class="py-0">
                                            <span class="title small-text">
                                                Workflow Name
                                            </span>
                                            <v-btn id="AutoTest_I206" text max-width="13" min-width="13" height="13" @click="EditWorkflow.WorkflowID=WorkflowShowDetailsItem.WorkflowID,
                                                                                                          EditWorkflow.WfName=WorkflowShowDetailsItem.WfName,
                                                                                                          EditWorkflow.WfDescription=WorkflowShowDetailsItem.WfDescription,
                                                                                                          ProjectWorkflowSteps=3">
                                                <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                            </v-btn>
                                            <p>{{ WorkflowShowDetailsItem.WfName }}</p>
                                        </v-col>
                                    </v-row>
                                    <!--Description-->
                                    <v-row>
                                        <v-col class="py-0">
                                            <span class="title small-text">
                                                Description
                                            </span>
                                            <v-btn id="AutoTest_I207" text max-width="13" min-width="13" height="13" @click="EditWorkflow.WorkflowID=WorkflowShowDetailsItem.WorkflowID,
                                                                                                          EditWorkflow.WfName=WorkflowShowDetailsItem.WfName,
                                                                                                          EditWorkflow.WfDescription=WorkflowShowDetailsItem.WfDescription,
                                                                                                          ProjectWorkflowSteps=3">
                                                <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                            </v-btn>
                                            <p style="overflow-y :auto; height:100px">{{ WorkflowShowDetailsItem.WfDescription }}</p>
                                        </v-col>
                                    </v-row>
                                    <!-- Levels-->
                                    <v-row>
                                        <v-col class="py-0">
                                            <span class="title small-text">
                                                <v-icon size="12">mdi-tag</v-icon>
                                                Levels : {{ WorkflowShowDetailsItem.WfNoOfLevels }}
                                            </span>
                                            <v-btn id="AutoTest_I208" text max-width="13" min-width="13" height="13" @click="EditWorkflow.WorkflowID=WorkflowShowDetailsItem.WorkflowID,
                                                                                                          EditWorkflow.WfName=WorkflowShowDetailsItem.WfName,
                                                                                                          EditWorkflow.WfDescription=WorkflowShowDetailsItem.WfDescription,
                                                                                                          EditWorkflow.WfNoOfLevels=WorkflowShowDetailsItem.WfNoOfLevels,
                                                                                                          EditWorkflow.WfLevelDetails=WorkflowShowDetailsItem.WfLevelDetails,
                                                                                                          fnCheckEditWorkflowSaveButton(),
                                                                                                          ProjectWorkflowSteps=4">
                                                <v-icon size="13" color="#3374B9">mdi-pencil-outline</v-icon>
                                            </v-btn>
                                        </v-col>
                                    </v-row>
                                </v-card>
                            </v-col>
                        </v-row>
                    </v-stepper-content>

                    <!-- Workflow Edit Details -->
                    <v-stepper-content step="3" class="pa-0">
                        <!-- Workflow Header -->
                        <v-row>
                            <v-col cols="12" md="8" class="pt-1 mb-2">
                                <h4 class="text-left heading-1">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> <span id="AutoTest_I209" style="cursor:pointer" @click="fnResetWorkflowShowDetailsItem(),ProjectWorkflowSteps=1">Workflow</span>
                                    > Workflow Details > {{ EditWorkflow.WfName }}
                                </h4>
                            </v-col>
                            <!-- Edit Workflow details-->
                            <v-row>
                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                                    <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="60" width="60" class="ma-4"></v-img>
                                    </v-avatar>
                                </v-col>
                                <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                    <v-row :justify="justifyCenter">
                                        <v-col md="3">
                                            <v-text-field outlined dense label="Workflow Name" type="text" v-model="EditWorkflow.WfName">
                                            </v-text-field>
                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="EditWorkflow.WfDescription">
                                            </v-textarea>
                                        </v-col>
                                    </v-row>
                                </v-col>
                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center py-0 mb-0">
                                    <v-btn id="AutoTest_I210" color="secondary" class="mr-2" @click="fnEditWorkflowDetailsSave(),ProjectWorkflowSteps=2">Save</v-btn>
                                    <v-btn id="AutoTest_I211" outlined color="grey" @click="EditWorkflow.WorkflowID=0,EditWorkflow.WfName='',
                                                                         EditWorkflow.WfDescription='',ProjectWorkflowSteps=2">Cancel</v-btn>
                                </v-col>
                            </v-row>
                        </v-row>
                    </v-stepper-content>

                    <!-- Workflow Edit Levels -->
                    <v-stepper-content step="4" class="pa-0">
                        <!-- Workflow Header -->
                        <v-row>
                            <v-col cols="12" md="8" class="pt-1 mb-2">
                                <h4 class="text-left heading-1">
                                    <v-avatar color="white" size="20" class="elevation-1 mr-2">
                                        <v-icon size="15" color="primary">mdi-folder-key-network-outline</v-icon>
                                    </v-avatar> <span id="AutoTest_I212" style="cursor:pointer" @click="fnResetWorkflowShowDetailsItem(),ProjectWorkflowSteps=1">Workflow</span>
                                    > Workflow Details > {{ EditWorkflow.WfName }}
                                </h4>
                            </v-col>
                            <v-spacer></v-spacer>
                            <v-col cols="12" xs="12" sm="12" md="6" class="text-right">
                                <v-btn id="AutoTest_I213" color="success-btn elevation-0 enter-btn" dark
                                       @click="fnAddLevelEditWorkflow()">
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
                                                    <v-row style="overflow-y :scroll;overflow-x :hidden ;height:500px">
                                                        <v-col cols="12" md="12">
                                                            <v-col md="12" v-for="i in EditWorkflow.WfNoOfLevels" :key="i">
                                                                <v-card class="workspace-card" flat>
                                                                    <v-row>
                                                                        <v-row :justify="justifyCenter">
                                                                            <v-col cols="6" md="6" class="text-center py-8">
                                                                                <v-progress-circular :rotate="360"
                                                                                                     :size="100"
                                                                                                     :width="15"
                                                                                                     :value="(100/EditWorkflow.WfNoOfLevels)*i"
                                                                                                     color="teal">
                                                                                    {{i}}
                                                                                </v-progress-circular>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-divider vertical class="mt-3"></v-divider>
                                                                        <v-col cols="6" md="6" class="text-left">
                                                                            <v-row no-gutters>
                                                                                <v-col cols="12" md="12" class="heading-1 pt-0 text-center">
                                                                                    <v-icon size="16" class="mr-2">mdi-account</v-icon>Users
                                                                                    <v-btn id="AutoTest_I214" text @click="fnDeleteLevelEditWorkflow(i)">
                                                                                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                                    </v-btn>
                                                                                    <v-divider class="mt-1"></v-divider>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <!---   if there is no data-->
                                                                            <v-row v-if="EditWorkflow.WfLevelDetails.length==0 ||
                                                                                         EditWorkflow.WfLevelDetails[i-1].usersList.length==0" no-gutters>
                                                                                <v-col cols="12" md="12" class="pt-2 mb-0 text-center">
                                                                                    <p class="heading-1"> No Users Added</p>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <!-- if User Assigned-->
                                                                            <v-row v-if="EditWorkflow.WfLevelDetails.length>0 && EditWorkflow.WfLevelDetails[i-1].usersList.length>0" no-gutters>
                                                                                <v-col cols="12" md="12">
                                                                                    <v-col class="text-center mb-0 pt-1">
                                                                                        <v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>=1" size="30" class="team-avatar">
                                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                        </v-avatar>
                                                                                        <v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>=2" size="30" class="team-avatar">
                                                                                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                        </v-avatar>
                                                                                        <v-avatar v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>2" size="30" class="team-count" color="red">
                                                                                            +{{ EditWorkflow.WfLevelDetails[i-1].usersList.length }}
                                                                                        </v-avatar>
                                                                                    </v-col>
                                                                                    <v-col class="text-center mb-0 pt-0">
                                                                                        <p v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length==1">{{ EditWorkflow.WfLevelDetails[i-1].usersList[0].Name }}</p>
                                                                                        <p v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length==2">{{ EditWorkflow.WfLevelDetails[i-1].usersList[0].Name }},{{ EditWorkflow.WfLevelDetails[i-1].usersList[1].Name }}</p>
                                                                                        <p v-if="EditWorkflow.WfLevelDetails[i-1].usersList.length>2">{{ EditWorkflow.WfLevelDetails[i-1].usersList[0].Name }},{{ EditWorkflow.WfLevelDetails[i-1].usersList[1].Name }} + {{ EditWorkflow.WfLevelDetails[i-1].usersList.length }}</p>
                                                                                    </v-col>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <!-- Assign Or Reassign Button-->
                                                                            <v-row no-gutters>
                                                                                <v-col v-if="EditWorkflow.WfLevelDetails.length>0 && EditWorkflow.WfLevelDetails[i-1].usersList.length==0" cols="12" md="12" class="text-center pt-0 mb-0">
                                                                                    <v-btn id="AutoTest_I215" color="success-btn elevation-0 enter-btn" dark @click="fnEditWorkflowAssignClick(i)">
                                                                                        Assign
                                                                                    </v-btn>
                                                                                </v-col>
                                                                                <v-col v-else cols="12" md="12" class="text-center pt-0 mb-0">
                                                                                    <v-btn id="AutoTest_I216" color="success-btn elevation-0 enter-btn" dark @click="fnEditWorkflowAssignClick(i)">
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
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                        <v-btn id="AutoTest_I217" color="secondary" class="mr-2 elevation-0" v-bind:disabled="EditWorkflowSaveButton"
                                                               @click="fnEditWorkflowLevelSave(),ProjectWorkflowSteps=2">Save</v-btn>
                                                        <v-btn id="AutoTest_I218" outlined color="grey" @click="EditWorkflow.WorkflowID=0,
                                                                                             EditWorkflow.WfName='',
                                                                                             EditWorkflow.WfDescription='',
                                                                                             EditWorkflow.WfNoOfLevels=0,
                                                                                             EditWorkflow.WfLevelDetails=[],
                                                                                             ProjectWorkflowSteps=2">Cancel</v-btn>
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
                                    <v-card-title primary-title class="page-head pb-0 text-center">
                                        Assign Users
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_I219" text @click="fnEditWorkflowAssignClose()">
                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-row>
                                        <!-- Users-->
                                        <v-col cols="6" md="6" sm="6" xs="6" class="px-4">
                                            <v-data-table :headers="WorkflowAssignUsersTableHeaders"
                                                          :items="WorkflowAssignUsersTableItems"
                                                          item-key="ID"
                                                          sort-by="Name"
                                                          show-select
                                                          class="elevation-1"
                                                          v-model="editWorkflowAssignUsersSelected">
                                            </v-data-table>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center py-0 mb-0">
                                            <v-btn id="AutoTest_I220" color="secondary" class="mr-2 elevation-0" v-bind:disabled="editWorkflowAssignUsersSelected.length==0" @click="fnEditWorkflowAssignSave()">Save</v-btn>
                                            <v-btn id="AutoTest_I221" outlined color="grey" @click="fnEditWorkflowAssignClose()">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-card>
                            </v-col>
                        </v-dialog>
                    </v-stepper-content>
                </v-stepper-items>
            </v-stepper>

            <!-- Create Workflow Dailog Box-->
            <v-dialog v-model="CreateWorkFlowDlg" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-folder-key-network-outline</v-icon>
                        </v-avatar>Add Workflow
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_I222" text @click="fnCreateWorkflowCloseORCancel()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="py-0">
                        <v-container fluid class="pt-0">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-stepper v-model="CreateWorkflowSteps" class="elevation-0">
                                        <v-stepper-items>
                                            <!-- Step 1 Create Work Flow -->
                                            <v-stepper-content step="1" class="pa-0">
                                                <v-col cols="12" md="12" class="pt-3">
                                                    <h4 class="text-center heading-3">Step 1 : Please create a project creation approval workflow.</h4>
                                                </v-col>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center pt-1">
                                                    <v-avatar size="60" tile class="elevation-1 base-border-radius">
                                                        <v-img src="../../images/WorkSpace-dummy-icon.png" height="60" width="60" class="ma-4"></v-img>
                                                    </v-avatar>
                                                </v-col>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="pt-2">
                                                    <v-row :justify="justifyCenter">
                                                        <v-col md="6">
                                                            <v-text-field outlined dense label="Workflow Name" type="text" v-model="CreateWorkflow.WfName">
                                                            </v-text-field>
                                                            <v-textarea rows="3" outlined dense label="Description" type="text" v-model="CreateWorkflow.WfDescription">
                                                            </v-textarea>
                                                            <v-select dense="" outlined="" hide-details :items="maxCreateWorkflowLevel"
                                                                      label="Number of Levels" solo flat return-object v-model="CreateWorkflow.WfNoOfLevels">
                                                            </v-select>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                                <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                    <v-btn id="AutoTest_I223" color="secondary" v-bind:disabled="CreateWorkflow.WfName.length==0 || CreateWorkflow.WfNoOfLevels==0" class="mr-2 elevation-0" @click="fnCreateWorkflowLevelInitialize(),CreateWorkflowSteps=2">Next</v-btn>
                                                    <v-btn id="AutoTest_I224" outlined color="grey" @click="fnCreateWorkflowCloseORCancel()">Cancel</v-btn>
                                                </v-col>
                                            </v-stepper-content>

                                            <!-- Step 2 - Assgin Users to Work flow-->
                                            <v-stepper-content step="2" class="pa-0">
                                                <v-container fluid class="pt-0">
                                                    <v-col cols="12" md="12" class="pt-3">
                                                        <h4 class="text-center heading-3">Step 2 : Please assign users to different levels</h4>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-5">
                                                        <v-btn id="AutoTest_I225" color="success-btn elevation-0 enter-btn" dark
                                                               @click="fnAddLevelCreateWorkflow()">
                                                            Add Level
                                                        </v-btn>
                                                    </v-col>
                                                    <v-row style="overflow-y :scroll;overflow-x :hidden ;height:400px">
                                                        <v-col md="12" v-for="i in CreateWorkflow.WfNoOfLevels" :key="i">
                                                            <v-card class="workspace-card" flat>
                                                                <v-row>
                                                                    <v-row no-gutters :justify="justifyCenter">
                                                                        <v-col cols="6" md="6" class="text-center ml-5 py-8">
                                                                            <v-progress-circular :rotate="360"
                                                                                                 :size="100"
                                                                                                 :width="15"
                                                                                                 :value="(100/CreateWorkflow.WfNoOfLevels)*i"
                                                                                                 color="teal">
                                                                                {{i}}
                                                                            </v-progress-circular>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-divider vertical class="mt-1"></v-divider>
                                                                    <v-col cols="6" md="6" class="text-left">
                                                                        <v-row no-gutters>
                                                                            <v-col cols="12" md="12" class="heading-1 pt-0 text-center">
                                                                                <v-icon size="16" class="mr-2">mdi-account</v-icon>User
                                                                                <v-btn id="AutoTest_I226" text @click="fnDeleteLevelCreateWorkflow(i)">
                                                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                                </v-btn>
                                                                                <v-divider class="mt-1"></v-divider>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <!---   if there is no data-->
                                                                        <v-row v-if="CreateWorkflow.WfLevelDetails.length==0||
                                                                                     CreateWorkflow.WfLevelDetails[i-1].usersList.length==0" no-gutters>
                                                                            <v-col cols="12" md="12" class="pt-1 mb-0 text-center">
                                                                                <p class="heading-1"> No Users Added</p>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row v-else no-gutters>
                                                                            <!-- Users-->
                                                                            <v-col v-if="CreateWorkflow.WfLevelDetails.length>0&&CreateWorkflow.WfLevelDetails[i-1].usersList.length>0" cols="12" md="12">
                                                                                <v-col class="text-center mb-0 pt-1">
                                                                                    <v-avatar v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length>=1" size="30" class="team-avatar">
                                                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                    </v-avatar>
                                                                                    <v-avatar v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length>=2" size="30" class="team-avatar">
                                                                                        <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Team name" />
                                                                                    </v-avatar>
                                                                                    <v-avatar v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length>2" size="30" class="team-count" color="red">
                                                                                        +{{ CreateWorkflow.WfLevelDetails[i-1].usersList.length }}
                                                                                    </v-avatar>
                                                                                </v-col>
                                                                                <v-col class="text-center mb-0 pt-0">
                                                                                    <p v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length==1">{{ CreateWorkflow.WfLevelDetails[i-1].usersList[0].Name }}</p>
                                                                                    <p v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length==2">{{ CreateWorkflow.WfLevelDetails[i-1].usersList[0].Name }},{{ CreateWorkflow.WfLevelDetails[i-1].usersList[1].Name }}</p>
                                                                                    <p v-if="CreateWorkflow.WfLevelDetails[i-1].usersList.length>2">{{ CreateWorkflow.WfLevelDetails[i-1].usersList[0].Name }},{{ CreateWorkflow.WfLevelDetails[i-1].usersList[1].Name }} + {{ CreateWorkflow.WfLevelDetails[i-1].usersList.length }}</p>
                                                                                </v-col>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <!-- Assign and ReAssign Button-->
                                                                        <v-row no-gutters>
                                                                            <v-col v-if="CreateWorkflow.WfLevelDetails.length==0||
                                                                                         CreateWorkflow.WfLevelDetails[i-1].usersList.length==0 " cols="12" md="12" class="text-center pt-0 mb-0">
                                                                                <v-btn id="AutoTest_I227" color="success-btn elevation-0 enter-btn" dark @click="fnCreateWorkflowAssignClick(i)">
                                                                                    Assign
                                                                                </v-btn>
                                                                            </v-col>
                                                                            <v-col v-else cols="12" md="12" class="text-center pt-0 mb-0">
                                                                                <v-btn id="AutoTest_I228" color="success-btn elevation-0 enter-btn" dark @click="fnCreateWorkflowAssignClick(i)">
                                                                                    Re-Assign
                                                                                </v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                        </v-col>
                                                    </v-row>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0 mb-0">
                                                        <v-btn id="AutoTest_I229" color="secondary" class="mr-2 elevation-0" @click="fnCreateWorkflowSave()" v-bind:disabled="CreateWorkflowSaveButton">Save</v-btn>
                                                        <v-btn id="AutoTest_I230" outlined color="grey" @click="fnCreateWorkflowCloseORCancel()">Cancel</v-btn>
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

            <!-- Create Workflow Assing User to Workflow-->
            <v-dialog v-model="CreateWorkflowAssignDialog" width="900">
                <v-col cols="12" md="12" class="pt-0 mb-0">
                    <v-card>
                        <v-card-title primary-title class="page-head pb-0 text-center">
                            Assign Users
                            <v-spacer></v-spacer>
                            <v-btn id="AutoTest_I231" text @click="fnCreateWorkflowAssignClose()">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-card-title>
                        <v-row>
                            <!-- Users-->
                            <v-col cols="6" md="6" sm="6" xs="6" class="px-4">
                                <v-data-table :headers="WorkflowAssignUsersTableHeaders"
                                              :items="WorkflowAssignUsersTableItems"
                                              item-key="ID"
                                              sort-by="Name"
                                              show-select
                                              class="elevation-1"
                                              v-model="createWorkflowAssignUsersSelected">
                                </v-data-table>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center py-0 mb-0">
                                <v-btn id="AutoTest_I232" color="secondary" class="mr-2 elevation-0" v-bind:disabled="createWorkflowAssignUsersSelected.length==0" @click="fnCreateWorkflowAssignSave()">Save</v-btn>
                                <v-btn id="AutoTest_I233" outlined color="grey" @click="fnCreateWorkflowAssignClose()">Cancel</v-btn>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-col>
            </v-dialog>
            <v-snackbar v-model="snackbarMsg" top color="primary">
                {{Msg}}
            </v-snackbar>
        </v-container>
    </v-card>
</template>

<script>
    var objInitiativeWorkflow;
    async function importscript() {
        return Promise.all([
            import("../../BL/InitiativeWorkflow.js").then(mod => {
                objInitiativeWorkflow = new mod.InitiativeWorkflow();
            })
        ]);
    };
    export default {
        data() {
            return {
                snackbarMsg: false,
                Msg: "",
                WorkspaceID: 0,
                InitiativeId: 0,
                justifyCenter: 'center',
                ProjectWorkflowSteps: 1,
                //Project Workflow tab
                workflowsList: [],
                filterWorkflowsList: [],
                searchWorkflow: "",
                workflowSortMenu: false,
                checkboxWorkflow: [],
                BulkActionMenu: false,
                WorkflowBulkRemoveDialog: false,
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
                    { text: "User Role", value: "userRole" },
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
                    WfNoOfLevels: 0,
                    WfLevelDetails: [],
                },
                CreateWorkflowSaveButton: true,
                CreateWorkflowAssignDialog: false,
                createWorkflowAssignUsersSelected: [],
                CreateWorkflowAssignLevelClicked: 0,
                //Create workflow ends here
            };
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.InitiativeId = this.$route.query.InitiativeID;
            await importscript();
            await this.fnGetUsersListByIntitiativeID();
            await this.fnGetWorkflowsListByInitiativeID();
        },
        methods: {
            fnSetWorkflowShowDetailsItem(item) {
                this.WorkflowShowDetailsItem.WorkflowID = item.WorkflowID;
                this.WorkflowShowDetailsItem.WfName = item.WfName;
                this.WorkflowShowDetailsItem.WfDescription = item.WfDescription;
                this.WorkflowShowDetailsItem.WfNoOfLevels = item.WfNoOfLevels;
                this.WorkflowShowDetailsItem.WfLevelDetails = item.WfLevelDetails;
            },
            fnResetWorkflowShowDetailsItem() {
                this.WorkflowShowDetailsItem.WorkflowID = 0;
                this.WorkflowShowDetailsItem.WfName = ""
                this.WorkflowShowDetailsItem.WfDescription = "";
                this.WorkflowShowDetailsItem.WfNoOfLevels = 0;
                this.WorkflowShowDetailsItem.WfLevelDetails = [];
            },
            async fnGetUsersListByIntitiativeID() {
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiativeWorkflow.GetUsersListByIntitiativeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.WorkflowAssignUsersTableItems = result.data;
                }
            },
            //Project Workflow tab functions starts here
            async fnGetWorkflowsListByInitiativeID() {
                var data = new FormData();
                data.append("InitiativeId", this.InitiativeId);
                data.append("WorkspaceId", this.WorkspaceID);
                var result = await objInitiativeWorkflow.GetWorkflowsListByIntitiativeID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.workflowsList = result.data;
                    this.filterWorkflowsList = result.data;
                    //alert("worflow list=" + this.filterWorkflowsList.length);
                }
            },
            fnWorkflowSort(type) {
                this.workflowSortMenu = false;
                switch (type) {
                    case "Name-Ascending":
                        this.filterWorkflowsList.sort((a, b) => (a.WfName > b.WfName) ? 1 : -1);
                        break;
                    case "Name-Descending":
                        this.filterWorkflowsList.sort((a, b) => (a.WfName < b.WfName) ? 1 : -1);
                        break;
                    case "Recently Added":
                        this.filterWorkflowsList.sort((a, b) => (a.WorkflowID < b.WorkflowID) ? 1 : -1);
                        break;
                    default:
                    // code block
                }
            },
            //*** Edit Workflow functions starts here ***//
            fnAddLevelEditWorkflow() {
                this.EditWorkflow.WfLevelDetails.push({ WfLevel: this.EditWorkflow.WfNoOfLevels + 1, usersList: [] });
                this.EditWorkflow.WfNoOfLevels += 1;
                this.EditWorkflowSaveButton = true;
            },
            fnDeleteLevelEditWorkflow(i) {
                this.EditWorkflow.WfNoOfLevels -= 1;
                this.EditWorkflow.WfLevelDetails = this.EditWorkflow.WfLevelDetails.filter(x => x.WfLevel != i);
                if (this.EditWorkflow.WfNoOfLevels == 0) {
                    this.EditWorkflow.WfLevelDetails = [];
                    this.EditWorkflowSaveButton = true;
                } else {
                    this.EditWorkflow.WfLevelDetails.forEach(u => {
                        if (u.WfLevel > i)
                            u.WfLevel -= 1;
                    });
                }
            },
            fnCheckEditWorkflowSaveButton() {
                if (this.EditWorkflow.WfNoOfLevels == 0) {
                    this.EditWorkflowSaveButton = true;
                    return;
                } else {
                    for (var index = 0; index < this.EditWorkflow.WfNoOfLevels; index++) {
                        var temp = this.EditWorkflow.WfLevelDetails.filter(x => x.WfLevel == index + 1);
                        if (temp.length > 0 && temp[0].usersList.length == 0) {
                            this.EditWorkflowSaveButton = true;
                            return;
                        }
                    }
                    this.EditWorkflowSaveButton = false;
                }
            },
            fnEditWorkflowAssignClick(index) {
                this.EditWorkflowAssignDialog = true;
                this.EditWorkflowAssignLevelClicked = index;
                var temp = this.EditWorkflow.WfLevelDetails.filter(x => x.WfLevel == index);
                if (temp.length > 0) {
                    this.editWorkflowAssignUsersSelected = temp[0].usersList;
                }
            },
            fnEditWorkflowAssignClose() {
                this.EditWorkflowAssignDialog = false;
                this.EditWorkflowAssignLevelClicked = 0;
                this.editWorkflowAssignUsersSelected = [];
            },
            fnEditWorkflowAssignSave() {
                var temp = this.EditWorkflow.WfLevelDetails.filter(x => x.WfLevel == this.EditWorkflowAssignLevelClicked);
                if (temp.length > 0) {//this is for re-assign
                    temp[0].usersList = this.editWorkflowAssignUsersSelected;
                } else {//this is for assign
                    this.EditWorkflow.WfLevelDetails.push({ WfLevel: this.EditWorkflowAssignLevelClicked, usersList: this.editWorkflowAssignUsersSelected });
                }
                this.fnCheckEditWorkflowSaveButton();
                this.EditWorkflowAssignDialog = false;
                this.EditWorkflowAssignLevelClicked = 0;
                this.editWorkflowAssignUsersSelected = [];
            },
            async fnEditWorkflowDetailsSave() {
                var data = new FormData();
                data.append("WorkflowID", this.EditWorkflow.WorkflowID);
                data.append("WfName", this.EditWorkflow.WfName);
                data.append("WfDescription", this.EditWorkflow.WfDescription);
                var result = await objInitiativeWorkflow.UpdateWorkflowDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully updated the workflow details");
                    this.snackbarMsg = true;
                    this.WorkflowShowDetailsItem.WfName = this.EditWorkflow.WfName;
                    this.WorkflowShowDetailsItem.WfDescription = this.EditWorkflow.WfDescription;
                    this.EditWorkflow.WorkflowID = 0;
                    this.EditWorkflow.WfName = "";
                    this.EditWorkflow.WfDescription = "";
                    this.fnGetWorkflowsListByInitiativeID();
                }
            },
            async fnEditWorkflowLevelSave() {
                var data = new FormData();
                data.append("WorkflowID", this.EditWorkflow.WorkflowID);
                data.append("WfName", this.EditWorkflow.WfName);
                data.append("WfDescription", this.EditWorkflow.WfDescription);
                data.append("WfNoOfLevels", this.EditWorkflow.WfNoOfLevels);
                data.append("WfLevelDetails", JSON.stringify(this.EditWorkflow.WfLevelDetails));
                var result = await objInitiativeWorkflow.UpdateWorkflowLevelDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully updated the workflow details");
                    this.snackbarMsg = true;
                    this.WorkflowShowDetailsItem.WfLevelDetails = this.EditWorkflow.WfLevelDetails;
                    this.WorkflowShowDetailsItem.WfNoOfLevels = this.EditWorkflow.WfNoOfLevels;
                    this.EditWorkflow.WorkflowID = 0;
                    this.EditWorkflow.WfName = "";
                    this.EditWorkflow.WfDescription = "";
                    this.EditWorkflow.WfLevelDetails = [];
                    this.fnGetWorkflowsListByInitiativeID();
                }
            },
            //*** Edit Workflow functions ends here ***//

            //*** Create Workflow functions starts here ***//
            fnCreateWorkflowLevelInitialize() {
                for (var index = 0; index < this.CreateWorkflow.WfNoOfLevels; index++) {
                    this.CreateWorkflow.WfLevelDetails.push({ WfLevel: index + 1, usersList: [] });
                }
            },
            fnAddLevelCreateWorkflow() {
                this.CreateWorkflow.WfLevelDetails.push({ WfLevel: this.CreateWorkflow.WfNoOfLevels + 1, usersList: [] });
                this.CreateWorkflow.WfNoOfLevels += 1;
                this.CreateWorkflowSaveButton = true;
            },
            fnDeleteLevelCreateWorkflow(i) {
                this.CreateWorkflow.WfNoOfLevels -= 1;
                this.CreateWorkflow.WfLevelDetails = this.CreateWorkflow.WfLevelDetails.filter(x => x.WfLevel != i);
                if (this.CreateWorkflow.WfNoOfLevels == 0) {
                    this.CreateWorkflow.WfLevelDetails = [];
                    this.CreateWorkflowSaveButton = true;
                } else {
                    this.CreateWorkflow.WfLevelDetails.forEach(u => {
                        if (u.WfLevel > i)
                            u.WfLevel -= 1;
                    });
                }
            },
            fnCreateWorkflowCloseORCancel() {
                this.CreateWorkFlowDlg = false;
                this.CreateWorkflowSteps = 1;
                this.CreateWorkflowSaveButton = true;
                this.CreateWorkflow.WfName = '';
                this.CreateWorkflow.WfDescription = '';
                this.CreateWorkflow.WfNoOfLevels = 0;
                this.CreateWorkflow.WfLevelDetails = [];
            },
            fnCreateWorkflowAssignClose() {
                this.CreateWorkflowAssignDialog = false;
                this.CreateWorkflowAssignLevelClicked = 0;
                this.createWorkflowAssignUsersSelected = [];
            },
            fnCreateWorkflowAssignClick(index) {
                this.CreateWorkflowAssignDialog = true;
                this.CreateWorkflowAssignLevelClicked = index;
                var temp = this.CreateWorkflow.WfLevelDetails.filter(x => x.WfLevel == index);
                if (temp.length > 0) {
                    this.createWorkflowAssignUsersSelected = temp[0].usersList;
                }
            },
            fnCheckCreateWorkflowSaveButton() {
                var savebtn = true;
                for (var index = 0; index < this.CreateWorkflow.WfNoOfLevels && savebtn; index++) {
                    if (this.CreateWorkflow.WfLevelDetails[index].usersList.length == 0) {
                        savebtn = false;
                    }
                }
                if (savebtn) this.CreateWorkflowSaveButton = false;
            },
            fnCreateWorkflowAssignSave() {
                var levelitem = this.CreateWorkflow.WfLevelDetails.filter(x => x.WfLevel == this.CreateWorkflowAssignLevelClicked);
                if (levelitem.length > 0) {//this is for re-assign
                    levelitem[0].usersList = this.createWorkflowAssignUsersSelected;
                } else {//this is for assign
                    this.CreateWorkflow.WfLevelDetails.push({ WfLevel: this.CreateWorkflowAssignLevelClicked, usersList: this.createWorkflowAssignUsersSelected });
                }
                //check whether all levels assigned or not, if assigned Save button enable otherwise no
                this.fnCheckCreateWorkflowSaveButton();
                this.CreateWorkflowAssignDialog = false;
                this.CreateWorkflowAssignLevelClicked = 0;
                this.createWorkflowAssignUsersSelected = [];
            },
            async fnCreateWorkflowSave() {
                var data = new FormData();
                data.append("WorkspaceID", this.WorkspaceID);
                data.append("InitiativeId", this.InitiativeId);
                data.append("WfName", this.CreateWorkflow.WfName);
                data.append("WfDescription", this.CreateWorkflow.WfDescription);
                data.append("WfNoOfLevels", this.CreateWorkflow.WfNoOfLevels);
                data.append("WfLevelDetails", JSON.stringify(this.CreateWorkflow.WfLevelDetails));

                var result = await objInitiativeWorkflow.AddWorkflow(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {
                    this.Msg = ("Successfully added the workflow");
                    this.snackbarMsg = true;
                    this.CreateWorkflow.WorkflowID = 0;
                    this.CreateWorkflow.WfName = "";
                    this.CreateWorkflow.WfDescription = "";
                    this.CreateWorkflow.WfNoOfLevels = 0;
                    this.CreateWorkflow.WfLevelDetails = [];
                    this.CreateWorkFlowDlg = false;
                    this.CreateWorkflowSteps = 1;
                    this.CreateWorkflowSaveButton = true;
                    this.fnGetWorkflowsListByInitiativeID();
                } else {
                    this.Msg = ("Failed to add the workflow ");
                    this.snackbarMsg = true;
                }
            },
            async fnRemoveWorkflows() {
                this.BulkActionMenu = false;
                this.WorkflowBulkRemoveDialog = false;
                if (this.checkboxWorkflow.length == 0) {
                    this.Msg = ("Please select the workflows you want to remove");
                    this.snackbarMsg = true;
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

                var result = await objInitiativeWorkflow.RemoveWorkflows(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                    this.Msg = ("Successfully removed" + wfids);
                    this.snackbarMsg = true;
                    this.checkboxWorkflow = [];
                    this.fnGetWorkflowsListByInitiativeID();
                }
            },
            //*** Create Workflow functions ends here ***//
            //Project Workflow tab functions ends here
        },
        watch: {
            searchWorkflow: function () {
                this.checkboxWorkflow = [];
                if (this.searchWorkflow == "") this.filterWorkflowsList = this.workflowsList;
                else this.filterWorkflowsList = this.workflowsList.filter(x => x.WfName.includes(this.searchWorkflow));
            },
        }
    };
</script>
<template>
    <v-container fluid :class="[isMobile && MySurveyTaskSteps == 2 ? 'pa-0 ma-0' : '']">
        <Pre-Loader v-if="PreLoader"></Pre-Loader>

        <v-row justify="center" no-gutters>
            <v-col cols="12" class="ma-0 pa-0">
                <v-stepper v-model="MySurveyTaskSteps" class="elevation-0 pa-0 ma-0">
                    <v-stepper-items>
                        <v-stepper-content step="1" class="pa-0 ma-0">
                            <v-expansion-panels :value="closeaccordion" flat>

                                <v-expansion-panel class="expansion_card"
                                    :class="[isMobile ? 'mobile_expansion_card' : 'desktop_expansion_card']"
                                    v-for=" (item, index) in customSearch" :key="item.TypeID"
                                    @click.stop="isMobile ? mobileDialog(item, index) : null">

                                    <v-expansion-panel-header hide-actions class="mb-4 elevation-4 expansion-header">
                                        <div style="padding:8px 8px;width:98%">

                                            <v-list-item class="list-item-container">
                                                <v-list-item-content class="list-item-content">
                                                    <v-list-item-subtitle class="my-1 heading-3 date-subtext">
                                                        {{ FormatDate(item.CreatedDate) }}</v-list-item-subtitle>
                                                    <v-list-item-title
                                                        class="text-left heading-3-dark taskname-heading text-truncate my-1">
                                                        {{ item.TaskName }}
                                                    </v-list-item-title>
                                                </v-list-item-content>
                                            </v-list-item>
                                            <v-row no-gutters
                                                :class="['align-center', isMobile ? 'pull-right' : ['pull-left', 'full-size']]">
                                                <v-col class="col-auto notif_chip_group">
                                                    <v-chip class="bell_chip" dark left>
                                                        <v-avatar left style="background-color:white">
                                                            <v-hover v-slot="{ hover }">
                                                                <span v-if="hover">{{ item.Notifications }}</span>
                                                                <v-icon v-else color="#3D7EFE" size="15">mdi-bell</v-icon>
                                                            </v-hover>
                                                        </v-avatar>
                                                        {{ item.NotificationType }}
                                                    </v-chip>
                                                </v-col>
                                                <v-col class="d-flex flex-grow-1 align-center col-auto task-details-row">
                                                    <v-spacer></v-spacer>
                                                    <v-col>
                                                        <div class="pa-0 text-limit" style="width:130px">
                                                            <v-avatar size="25">
                                                                <v-img :src="BenIcon" alt="beneficiary_icon"
                                                                    size="15"></v-img>
                                                            </v-avatar>
                                                            {{ item.ProjectName }}
                                                        </div>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                    <v-col cols="auto">
                                                        <v-avatar size="25">
                                                            <v-img :src="calIcon" alt="calender_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <span class="mx-4 date_created">{{ FormatDate(item.CreatedDate)
                                                        }}</span>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                    <v-col>
                                                        <div class="pa-0 text-limit" style="width:130px">
                                                            <v-avatar size="25">
                                                                <v-img :src="RecIcon" alt="reccurring_icon"
                                                                    size="15"></v-img>
                                                            </v-avatar>
                                                            {{ item.Periodicity }}
                                                        </div>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                    <v-col>
                                                        <div class="pa-0 text-limit" style="width:130px">
                                                            <v-avatar size="25">
                                                                <v-img :src="BenIcon" alt="beneficiary_icon"
                                                                    size="15"></v-img>
                                                            </v-avatar>
                                                            {{ item.ProjectName }}
                                                        </div>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                    <v-col>
                                                        <div class="pa-0 text-limit" style="width:130px">
                                                            <v-avatar size="25">
                                                                <v-img :src="BenIcon" alt="beneficiary_icon"
                                                                    size="15"></v-img>
                                                            </v-avatar>
                                                            {{ item.BeneficiaryType == null ? "N/A" : item.BeneficiaryType
                                                            }}
                                                        </div>
                                                    </v-col>
                                                </v-col>
                                            </v-row>
                                        </div>
                                        <v-spacer></v-spacer>
                                        <div v-if="!isMobile" class="grow text-center mr-4"
                                            style="background-color:lightblue;height:115px;">
                                            <p class="hide px-2 text--white">{{ item.TaskType }}</p>
                                        </div>
                                    </v-expansion-panel-header>


                                    <v-expansion-panel-content class="pa-0 ma-0 myTaskExpansionPanelContent"
                                        :class="[isMobile ? 'd-none' : '']">
                                        <div class="timeline_container bulkImportContainer" v-if="item.isApprover">
                                            <div class="timeline_body">
                                                <div class="task_head">
                                                </div> 
                                                <div class="pa-2">
                                                    <v-btn solo depressed link :to="{ name: 'MyTasksManage', query: { SurveyID: item.TypeID, ProjectId:  ProjectId }, params: { TaskCardData: item, clientGuid: clientguid, SurveyName: item.TaskName} }">Bulk Function</v-btn>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="timeline_container">
                                            <div class="timeline_body">
                                                <div class="task_head">
                                                    <div class="d-md-flex pa-2">
                                                        <div class="name">Sent by</div>
                                                        <div class="name">Assigned to</div>
                                                        <div class="date">Sent on</div>
                                                        <div class="status">Status</div>
                                                        <div class="action_required">Action Required</div>
                                                        <div class="workflow_stage">Workflow Stage</div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="timeline_container" v-if="item.Periodicity != 'Anytime'">
                                            <div class="timeline_body" :style="fnBindStyle(Qitem, 'font')"
                                                v-for="(Qitem, ind) in item.TaskLog" :key="Qitem.TaskIndex">
                                                <!--New Layout testing-->
                                                <div class="task_row">
                                                    <span :class="fnBindStyle(Qitem, 'dot')"></span>
                                                    <div class="d-flex pl-2 pt-2">
                                                        <!--Sent By-->
                                                        <div class="name text-limit">
                                                            {{ Qitem.Name == "" ? "N/A" : Qitem.Name }}
                                                        </div>
                                                        <!--Assigned To-->
                                                        <div class="name text-limit">
                                                            {{ Qitem.AssignedTo == "" ? "N/A" : Qitem.AssignedTo }}

                                                        </div>
                                                        <!--Sent On-->
                                                        <div class="date">{{ FormatDateWithTime(Qitem.CreatedDate) }}</div>
                                                        <!--Status-->
                                                        <div class="status">
                                                            <v-chip class="text-center" :class="{
                                                                'task-overdue-chip': Qitem.CurrentOrOverdue == 'Overdue' || Qitem.CurrentOrOverdue == 'Pending',
                                                                'task-current-chip': Qitem.CurrentOrOverdue == 'Current',
                                                                'task-completed-chip': Qitem.CurrentOrOverdue == 'Review',
                                                                'task-saved-chip': Qitem.CurrentOrOverdue == 'Saved',
                                                                'task-sync-chip': Qitem.CurrentOrOverdue == 'Syncing'
                                                            }" small label text-color="white">
                                                                {{ Qitem.CurrentOrOverdue }}
                                                            </v-chip>
                                                        </div>
                                                        <!--Action Required-->
                                                        <div class="action_required text-limit">{{ Qitem.ActionRequired }}
                                                        </div>
                                                        <!--Workflow stage-->
                                                        <div class="workflow_stage text-limit">{{ Qitem.WorkflowStage }}
                                                        </div>

                                                        <!--<div class="task_type text-limit">{{Qitem.Message}}</div>
                                                    <div class="comment text-limit">{{Qitem.Comment}}</div>-->
                                                        <div class="log_chip" style="background:white;margin-top:-1px;">
                                                            <div class="d-flex d-inline">
                                                                <v-btn class="EditActButton" icon
                                                                    @click="GetSurveyTaskDetails(Qitem, 0, index)">
                                                                    <v-avatar size="25">
                                                                        <v-img :src="EditIcon" alt="Edit_icon"
                                                                            size="15"></v-img>
                                                                    </v-avatar>
                                                                </v-btn>
                                                                <v-btn class="ActivityActButton" icon
                                                                    @click.stop="GetMyTaskActivityLog(Qitem.SyncDBID), SurveyNewActivityDialog = true">
                                                                    <v-avatar size="25">
                                                                        <v-img :src="ActIcon" alt="Activit_icon"
                                                                            size="15"></v-img>
                                                                    </v-avatar>
                                                                </v-btn>
                                                                <div class="text--disabled pt-1 px-2 PlaceHolderText">Hover
                                                                    to Take Action</div>
                                                                <div class="EditIconHover pt-1 px-2"
                                                                    v-if="fnBindStyle(Qitem, 'font') === 'font-weight:400'">
                                                                    View <span class="pl-7"> >> </span></div>
                                                                <div class="EditIconHover pt-1 px-2" v-else>Update <span
                                                                        class="pl-7"> >> </span></div>
                                                                <div class="AcitivityIconHover pt-1 px-2">Activity Log <span
                                                                        class="pl-7"> >> </span></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--Anytime periodicity layout-->
                                        <div class="timeline_container" v-if="item.Periodicity == 'Anytime'">
                                            <div class="timeline_body" v-if="item.IsDataCollector == true">
                                                <div class="task_col" v-if="item.btnAnytimeSubVisible == true">
                                                    <div>
                                                        <v-btn class="mx-0" depressed :disabled="item.btnDisable"
                                                            @click="fnAddAnyTimeNewSubmission(item, index)">
                                                            <v-icon>mdi-plus-circle-outline</v-icon>
                                                            Add New Submission
                                                        </v-btn>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Any time Add new Submission UI-->
                                            <v-slide-x-transition group>
                                                <div class="timeline_body" :style="fnBindStyle(Aitem, 'font')"
                                                    v-for="(Aitem, ind) in lstAnyTimeNewSubmission" :key="ind"
                                                    v-if="Aitem.TaskData.TaskType == item.TaskType && Aitem.TaskData.TypeID == item.TypeID">
                                                    <!--New Layout testing-->
                                                    <div class="task_row">
                                                        <span :class="fnBindStyle(Aitem, 'dot')"></span>
                                                        <div class="d-md-flex pl-2 pt-2">
                                                            <!--Sent By-->
                                                            <div class="name text-limit">
                                                                {{ Aitem.TaskData.Name == "" ? "N/A" : Aitem.TaskData.Name
                                                                }}
                                                            </div>
                                                            <!--Assigned To-->
                                                            <div class="name text-limit">
                                                                {{ Aitem.TaskData.AssignedTo == "" ? "N/A" :
                                                                    Aitem.TaskData.AssignedTo }}
                                                            </div>
                                                            <!--Sent On-->
                                                            <div class="date">
                                                                {{ FormatDateWithTime(Aitem.TaskData.CreatedDate) }}</div>
                                                            <!--Status-->
                                                            <div class="status">
                                                                <v-chip class="text-center" :class="{
                                                                    'task-overdue-chip': Aitem.TaskData.CurrentOrOverdue == 'Overdue' || Aitem.TaskData.CurrentOrOverdue == 'Pending',
                                                                    'task-current-chip': Aitem.TaskData.CurrentOrOverdue == 'Current',
                                                                    'task-completed-chip': Aitem.TaskData.CurrentOrOverdue == 'Review',
                                                                    'task-saved-chip': Aitem.TaskData.CurrentOrOverdue == 'Saved',
                                                                    'task-sync-chip': Aitem.TaskData.CurrentOrOverdue == 'Syncing'
                                                                }" small label text-color="white">
                                                                    {{ Aitem.TaskData.CurrentOrOverdue }}
                                                                </v-chip>
                                                            </div>
                                                            <!--Action Required-->
                                                            <div class="action_required text-limit">
                                                                {{ Aitem.TaskData.ActionRequired }}</div>
                                                            <!--Workflow stage-->
                                                            <div class="workflow_stage text-limit">
                                                                {{ Aitem.TaskData.WorkflowStage }}</div>
                                                            <!--<div class="task_type text-limit">{{Aitem.TaskData.Message}}</div>
                                                        <div class="comment text-limit">{{Aitem.TaskData.Comment}}</div>-->
                                                            <div class="log_chip"
                                                                style="background:white;margin-top: -1px;">
                                                                <div class="d-flex d-inline">
                                                                    <v-btn class="EditActButton" icon
                                                                        @click="GetSurveyTaskDetails(Aitem.TaskData, Aitem.pkId, index)">
                                                                        <v-avatar size="25">
                                                                            <v-img :src="EditIcon" alt="Edit_icon"
                                                                                size="15"></v-img>
                                                                        </v-avatar>
                                                                    </v-btn>
                                                                    <v-btn class="ActivityActButton" icon
                                                                        @click.stop="GetMyTaskActivityLog(Aitem.SyncDBID), SurveyNewActivityDialog = true">
                                                                        <v-avatar size="25">
                                                                            <v-img :src="ActIcon" alt="Activit_icon"
                                                                                size="15"></v-img>
                                                                        </v-avatar>
                                                                    </v-btn>
                                                                    <div class="text--disabled pt-1 px-2 PlaceHolderText">
                                                                        Hover to Take Action</div>
                                                                    <div class="EditIconHover pt-1 px-2"
                                                                        v-if="fnBindStyle(Aitem, 'font') === 'font-weight:400'">
                                                                        View <span class="pl-7"> >> </span></div>
                                                                    <div class="EditIconHover pt-1 px-2" v-else>Update <span
                                                                            class="pl-7"> >> </span></div>
                                                                    <div class="AcitivityIconHover pt-1 px-2">Activity Log
                                                                        <span class="pl-7"> >> </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </v-slide-x-transition>
                                            <v-slide-x-transition group>
                                                <div class="timeline_body" :style="fnBindStyle(Qitem, 'font')"
                                                    v-for="(Qitem, ind) in item.TaskLog" :key="ind">
                                                    <!--New Layout testing-->
                                                    <div class="task_row">
                                                        <span :class="fnBindStyle(Qitem, 'dot')"></span>
                                                        <div class="d-md-flex pl-2 pt-2">
                                                            <!--Sent By-->
                                                            <div class="name text-limit">
                                                                {{ Qitem.Name == "" ? "N/A" : Qitem.Name }}
                                                            </div>
                                                            <!--Assigned To-->
                                                            <div class="name text-limit">
                                                                {{ Qitem.AssignedTo == "" ? "N/A" : Qitem.AssignedTo }}
                                                            </div>
                                                            <!--Sent On-->
                                                            <div class="date">{{ FormatDateWithTime(Qitem.CreatedDate) }}
                                                            </div>
                                                            <!--Status-->
                                                            <div class="status">
                                                                <v-chip class="text-center" :class="{
                                                                    'task-overdue-chip': Qitem.CurrentOrOverdue == 'Overdue' || Qitem.CurrentOrOverdue == 'Pending',
                                                                    'task-current-chip': Qitem.CurrentOrOverdue == 'Current',
                                                                    'task-completed-chip': Qitem.CurrentOrOverdue == 'Review',
                                                                    'task-saved-chip': Qitem.CurrentOrOverdue == 'Saved',
                                                                    'task-sync-chip': Qitem.CurrentOrOverdue == 'Syncing'
                                                                }" small label text-color="white">
                                                                    {{ Qitem.CurrentOrOverdue }}
                                                                </v-chip>
                                                            </div>
                                                            <!--Action Required-->
                                                            <div class="action_required text-limit">{{ Qitem.ActionRequired
                                                            }}
                                                            </div>
                                                            <!--Workflow stage-->
                                                            <div class="workflow_stage text-limit">{{ Qitem.WorkflowStage }}
                                                            </div>
                                                            <!--<div class="task_type text-limit">{{Qitem.Message}}</div>
                                                        <div class="comment text-limit">{{Qitem.Comment}}</div>-->
                                                            <div class="log_chip" style="background:white;margin-top:-1px">
                                                                <div class="d-flex d-inline">
                                                                    <v-btn class="EditActButton" icon
                                                                        @click="GetSurveyTaskDetails(Qitem, 0, index)">
                                                                        <v-avatar size="25">
                                                                            <v-img :src="EditIcon" alt="Edit_icon"
                                                                                size="15"></v-img>
                                                                        </v-avatar>
                                                                    </v-btn>
                                                                    <v-btn class="ActivityActButton" icon
                                                                        @click.stop="GetMyTaskActivityLog(Qitem.SyncDBID), SurveyNewActivityDialog = true">
                                                                        <v-avatar size="25">
                                                                            <v-img :src="ActIcon" alt="Activit_icon"
                                                                                size="15"></v-img>
                                                                        </v-avatar>
                                                                    </v-btn>
                                                                    <div class="text--disabled pt-1 px-2 PlaceHolderText">
                                                                        Hover to Take Action</div>
                                                                    <div class="EditIconHover pt-1 px-2"
                                                                        v-if="fnBindStyle(Qitem, 'font') === 'font-weight:400'">
                                                                        View <span class="pl-7"> >> </span></div>
                                                                    <div class="EditIconHover pt-1 px-2" v-else>Update <span
                                                                            class="pl-7"> >> </span></div>
                                                                    <div class="AcitivityIconHover pt-1 px-2">Activity Log
                                                                        <span class="pl-7"> >> </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </v-slide-x-transition>
                                        </div>
                                        <div>
                                            <v-btn
                                                dense
                                                text
                                                @click="fnLoadNewTasks(item, index, false)"
                                                v-if="item.TaskLog.length > 19 && item.TaskLog.length <= item.TaskCount"
                                                :loading="newTaskLoader"
                                                :disabled="newTaskLoader"
                                                :ripple="false"
                                            >
                                                {{ item.loadMore ? 'Load More' : 'End of List' }}
                                            </v-btn>
                                        </div>
                                    </v-expansion-panel-content>

                                </v-expansion-panel>
                            </v-expansion-panels>
                        </v-stepper-content>
                        <v-stepper-content step="2" class="pa-0 ma-0">
                            <v-card style="background-color: #F5F6F8">
                                <v-card-title style="background-color:white">
                                    <v-icon color="#707070" size="20"
                                        @click="closeMobileCard(), closeaccordion = false">mdi-keyboard-backspace</v-icon>
                                    <v-spacer></v-spacer>
                                    <v-icon color="#707070" size="20"
                                        @click="closeMobileCard(), closeaccordion = false">mdi-close</v-icon><!--SurveyActivityLog(mobileTaskItem)-->
                                </v-card-title>
                                <v-divider></v-divider>
                                <v-card-title>
                                    <div class="heading-4-dark text-limit">{{ mobileTaskItem.TaskName }}</div>
                                </v-card-title>

                                <v-card class="elevation-2 my-2"
                                    style="border-bottom-left-radius: 16px;border-bottom-right-radius:16px">
                                    <v-row justify="center" no-gutters>
                                        <v-col cols="12">
                                            <v-slide-group multiple class="ma-2">
                                                <v-slide-item class="ma-2">
                                                    <div>
                                                        <v-avatar size="25">
                                                            <v-img :src="BenIcon" alt="project_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <v-tooltip top>
                                                            <template v-slot:activator="{ on, attrs }">
                                                                <span v-bind="attrs" v-on="on" class="mx-4">{{
                                                                    mobileTaskItem.ProjectName }}</span>
                                                            </template>
                                                            <span class="mx-4">Project Name</span>
                                                        </v-tooltip>
                                                    </div>
                                                </v-slide-item>
                                                <v-slide-item class="ma-2">
                                                    <div>
                                                        <v-avatar size="25">
                                                            <v-img :src="calIcon" alt="calender_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <v-tooltip top>
                                                            <template v-slot:activator="{ on, attrs }">
                                                                <span v-bind="attrs" v-on="on" class="mx-4">{{
                                                                    FormatDate(mobileTaskItem.CreatedDate) }}</span>
                                                            </template>
                                                            <span class="mx-4">Assigned Date</span>
                                                        </v-tooltip>
                                                    </div>
                                                </v-slide-item>
                                                <v-slide-item class="ma-2">
                                                    <div>
                                                        <v-avatar size="25">
                                                            <v-img :src="RecIcon" alt="reccurring_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <v-tooltip top>
                                                            <template v-slot:activator="{ on, attrs }">
                                                                <span v-bind="attrs" v-on="on" class="mx-4">{{
                                                                    mobileTaskItem.Periodicity }}</span>
                                                            </template>
                                                            <span class="mx-4">Reccuring</span>
                                                        </v-tooltip>
                                                    </div>
                                                </v-slide-item>
                                                <v-slide-item class="ma-2">
                                                    <div>
                                                        <v-avatar size="25">
                                                            <v-img :src="BenIcon" alt="beneficiary_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <v-tooltip top>
                                                            <template v-slot:activator="{ on, attrs }">
                                                                <span v-bind="attrs" v-on="on" class="mx-4">{{
                                                                    mobileTaskItem.BeneficiaryType == null ?
                                                                    "N/A" : mobileTaskItem.BeneficiaryType.length > 3 ?
                                                                        mobileTaskItem.BeneficiaryType.substring(0, 3) + "..." :
                                                                        mobileTaskItem.BeneficiaryType }}</span>
                                                            </template>
                                                            <span>{{ mobileTaskItem.BeneficiaryType == null ? "N/A" :
                                                                mobileTaskItem.BeneficiaryType }}</span>
                                                        </v-tooltip>
                                                    </div>
                                                </v-slide-item>
                                                <v-slide-item class="ma-2">
                                                    <div>
                                                        <v-avatar size="25">
                                                            <v-img :src="BenIcon" alt="project_icon" size="15"></v-img>
                                                        </v-avatar>
                                                        <v-tooltip top>
                                                            <template v-slot:activator="{ on, attrs }">
                                                                <span v-bind="attrs" v-on="on" class="mx-4">{{
                                                                    mobileTaskItem.ProjectName }}</span>
                                                            </template>
                                                            <span class="mx-4">Assigned By</span>
                                                        </v-tooltip>
                                                    </div>
                                                </v-slide-item>
                                            </v-slide-group>
                                        </v-col>
                                    </v-row>
                                </v-card>
                                <v-row no-gutters justify="center">
                                    <!--max-height="68vh" class=" v-scrolling-div"-->
                                    <v-col cols="12" justify="center">
                                        <v-expansion-panels class="mobile_expansion_panels" :value="closeaccordion" flat>
                                            <!--style="box-shadow: none; height: 68vh;"-->
                                            <span class="mobile_timeline"></span>
                                            <v-expansion-panel class="expansion_card mob_expansion_card"
                                                v-for="(Qitem, ind) in mobileTaskItem.TaskLog" :key="Qitem.TaskIndex"
                                                v-if="mobileTaskItem.Periodicity != 'Anytime'"
                                                style="width: 100%; background-color: #F5F6F8">
                                                <v-expansion-panel-header class="expansion-header" style="z-index:2;">
                                                    <v-row no-gutters justify="center" :style="fnBindStyle(Qitem, 'font')">
                                                        <v-col cols="12" class="d-inline-flex ma-2">
                                                            <span class="ma-2">{{ Qitem.Month }}</span>
                                                            <span class="ma-2">
                                                                <v-chip class="text-center" :class="{
                                                                    'task-overdue-chip': Qitem.CurrentOrOverdue == 'Overdue' || Qitem.CurrentOrOverdue == 'Pending',
                                                                    'task-current-chip': Qitem.CurrentOrOverdue == 'Current',
                                                                    'task-completed-chip': Qitem.CurrentOrOverdue == 'Review',
                                                                    'task-saved-chip': Qitem.CurrentOrOverdue == 'Saved',
                                                                    'task-sync-chip': Qitem.CurrentOrOverdue == 'Syncing'
                                                                }" small label text-color="white">
                                                                    {{ Qitem.CurrentOrOverdue }}
                                                                </v-chip>
                                                            </span>
                                                            <div class="task_type ma-2">{{ Qitem.ActionRequired }}</div>
                                                        </v-col>
                                                    </v-row>

                                                </v-expansion-panel-header>
                                                <v-expansion-panel-content
                                                    class="myTaskExpansionPanelContent expansion-content"
                                                    style="z-index:1;">
                                                    <span :class="fnBindStyle(Qitem, 'dot')"></span>
                                                    <div class="ml-8 mt-2">
                                                        <span>{{ Qitem.Name == "" ? "N/A" : Qitem.Name }}</span>
                                                        <span class="mx-1">{{ Qitem.Message }}</span>
                                                        <div class="d-flex float-right">
                                                            <v-btn @click="GetSurveyTaskDetails(Qitem, 0, mobIndex)"
                                                                v-if="fnBindStyle(Qitem, 'font') === 'font-weight:400'"
                                                                color="primary">View</v-btn>
                                                            <v-btn @click="GetSurveyTaskDetails(Qitem, 0, mobIndex)" v-else
                                                                color="primary">Update</v-btn>
                                                            <v-btn
                                                                @click.stop="GetMyTaskActivityLog(Qitem.SyncDBID), SurveyNewActivityDialog = true"
                                                                color="success" class="ml-2">Activity</v-btn>
                                                        </div>
                                                    </div>
                                                </v-expansion-panel-content>
                                            </v-expansion-panel>
                                            <!--mobile Anytime periodicity layout-->
                                            <div v-if="mobileTaskItem.Periodicity == 'Anytime'" style="width:100%"
                                                class="pa-0 ma-0">
                                                <div v-if="mobileTaskItem.IsDataCollector == true" class="pa-0 ma-0">
                                                    <v-btn class="ml-4" v-if="mobileTaskItem.btnAnytimeSubVisible == true"
                                                        depressed :disabled="mobileTaskItem.btnDisable"
                                                        @click="fnAddAnyTimeNewSubmission(mobileTaskItem, mobIndex)">
                                                        <v-icon>mdi-plus-circle-outline</v-icon>
                                                        Add New Submission
                                                    </v-btn>
                                                </div>

                                                <!--mobile  Any time Add new Submission UI-->
                                                <v-slide-x-transition group class="ma-0 pa-0">
                                                    <v-expansion-panel class="expansion_card mob_expansion_card"
                                                        style="background-color: #F5F6F8"
                                                        v-for="(Aitem, ind) in lstAnyTimeNewSubmission" :key="ind"
                                                        v-if="Aitem.TaskData.TaskType == mobileTaskItem.TaskType && Aitem.TaskData.TypeID == mobileTaskItem.TypeID">
                                                        <v-expansion-panel-header class="expansion-header"
                                                            style="z-index:2;">
                                                            <v-row no-gutters justify="center"
                                                                :style="fnBindStyle(Aitem, 'font')">
                                                                <v-col cols="12" class="d-inline-flex ma-2">
                                                                    <span class="ma-2">{{ Aitem.TaskData.Month }}</span>
                                                                    <span class="ma-2">
                                                                        <v-chip class="text-center" :class="{
                                                                            'task-overdue-chip': Aitem.TaskData.CurrentOrOverdue == 'Overdue' || Aitem.TaskData.CurrentOrOverdue == 'Pending',
                                                                            'task-current-chip': Aitem.TaskData.CurrentOrOverdue == 'Current',
                                                                            'task-completed-chip': Aitem.TaskData.CurrentOrOverdue == 'Review',
                                                                            'task-saved-chip': Aitem.TaskData.CurrentOrOverdue == 'Saved',
                                                                            'task-sync-chip': Aitem.TaskData.CurrentOrOverdue == 'Syncing'
                                                                        }" small label text-color="white">
                                                                            {{ Aitem.TaskData.CurrentOrOverdue }}
                                                                        </v-chip>
                                                                    </span>
                                                                    <div class="task_type ma-2">
                                                                        {{ Aitem.TaskData.ActionRequired }}</div>
                                                                </v-col>
                                                            </v-row>
                                                        </v-expansion-panel-header>
                                                        <v-expansion-panel-content
                                                            class="myTaskExpansionPanelContent  expansion-content"
                                                            style="z-index:1">
                                                            <span :class="fnBindStyle(Aitem, 'dot')"></span>
                                                            <div class="ml-8 mt-2">
                                                                <span>{{ Aitem.TaskData.Name == "" ? "N/A" :
                                                                    Aitem.TaskData.Name }}</span>
                                                                <span class="mx-1">{{ Aitem.TaskData.Message }}</span>
                                                                <div class="d-flex float-right">
                                                                    <v-btn
                                                                        @click="GetSurveyTaskDetails(Aitem.TaskData, Aitem.pkId, mobIndex)"
                                                                        v-if="fnBindStyle(Aitem, 'font') === 'font-weight:400'"
                                                                        color="primary">View</v-btn>
                                                                    <v-btn
                                                                        @click="GetSurveyTaskDetails(Aitem.TaskData, Aitem.pkId, mobIndex)"
                                                                        v-else color="primary">Update</v-btn>
                                                                    <v-btn
                                                                        @click.stop="GetMyTaskActivityLog(Aitem.SyncDBID), SurveyNewActivityDialog = true"
                                                                        color="success" class="ml-2">Activity</v-btn>
                                                                </div>
                                                            </div>
                                                        </v-expansion-panel-content>
                                                    </v-expansion-panel>
                                                </v-slide-x-transition>
                                                <v-slide-x-transition group class="ma-0 pa-0">
                                                    <v-expansion-panel class="expansion_card mob_expansion_card"
                                                        style="background-color: #F5F6F8"
                                                        v-for="(Qitem, ind) in mobileTaskItem.TaskLog"
                                                        :key="Qitem.TaskIndex">
                                                        <v-expansion-panel-header class="expansion-header"
                                                            style="z-index:2;">
                                                            <v-row no-gutters justify="center"
                                                                :style="fnBindStyle(Qitem, 'font')">
                                                                <v-col cols="12" class="d-inline-flex ma-2">
                                                                    <span class="ma-2">{{ Qitem.Month }}</span>
                                                                    <span class="ma-2">
                                                                        <v-chip class="text-center" :class="{
                                                                            'task-overdue-chip': Qitem.CurrentOrOverdue == 'Overdue' || Qitem.CurrentOrOverdue == 'Pending',
                                                                            'task-current-chip': Qitem.CurrentOrOverdue == 'Current',
                                                                            'task-completed-chip': Qitem.CurrentOrOverdue == 'Review',
                                                                            'task-saved-chip': Qitem.CurrentOrOverdue == 'Saved',
                                                                            'task-sync-chip': Qitem.CurrentOrOverdue == 'Syncing'
                                                                        }" small label text-color="white">
                                                                            {{ Qitem.CurrentOrOverdue }}
                                                                        </v-chip>
                                                                    </span>
                                                                    <div class="task_type ma-2">{{ Qitem.ActionRequired }}
                                                                    </div>
                                                                </v-col>
                                                            </v-row>
                                                        </v-expansion-panel-header>
                                                        <v-expansion-panel-content
                                                            class="myTaskExpansionPanelContent expansion-content"
                                                            style="z-index:1">
                                                            <span :class="fnBindStyle(Qitem, 'dot')"></span>
                                                            <div class="ml-8 mt-2">
                                                                <span>{{ Qitem.Name == "" ? "N/A" : Qitem.Name }}</span>
                                                                <span class="mx-1">{{ Qitem.Message }}</span>
                                                                <div class="d-flex float-right">
                                                                    <v-btn @click="GetSurveyTaskDetails(Qitem, 0, mobIndex)"
                                                                        v-if="fnBindStyle(Qitem, 'font') === 'font-weight:400'"
                                                                        color="primary">View</v-btn>
                                                                    <v-btn @click="GetSurveyTaskDetails(Qitem, 0, mobIndex)"
                                                                        v-else color="primary">Update</v-btn>
                                                                    <v-btn
                                                                        @click.stop="GetMyTaskActivityLog(Qitem.SyncDBID), SurveyNewActivityDialog = true"
                                                                        color="success" class="ml-2">Activity</v-btn>
                                                                </div>
                                                            </div>
                                                        </v-expansion-panel-content>
                                                    </v-expansion-panel>
                                                </v-slide-x-transition>
                                            </div>
                                            <div>
                                                <v-btn
                                                    dense
                                                    text
                                                    @click="fnLoadNewTasks(mobileTaskItem, 0, true)"
                                                    v-if="mobileTaskItem.TaskLog && mobileTaskItem.TaskLog.length > 2"
                                                    :loading="newTaskLoader"
                                                    :disabled="newTaskLoader || !mobileTaskItem.loadMore"
                                                >
                                                    {{ mobileTaskItem.loadMore ? 'Load More' : 'End of List' }}
                                                </v-btn>
                                            </div>
                                        </v-expansion-panels>
                                    </v-col>
                                </v-row>
                            </v-card>
                        </v-stepper-content>

                    </v-stepper-items>
                </v-stepper>

            </v-col>
        </v-row>

        <!--New Activity log-->
        <v-dialog v-model="SurveyNewActivityDialog" width="800" hide-overlay persistent
            :fullscreen="$vuetify.breakpoint.smAndDown">
            <v-card class="myTasksActivityDialogCardHeight">
                <v-card-title>
                    Activity
                    <v-spacer></v-spacer>
                    <v-btn text @click.stop="SurveyNewActivityDialog = false">
                        <v-icon color="#707070" size="20">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <!--<v-divider></v-divider>-->
                <v-card-text class="px-0 py-3 ma-0 v-scrolling-div myTasksActivityDialogCardHeight"
                    style="background:#F5F6F8;">
                    <v-card elevation="2" class="ma-3" v-for="(item, index) in lstLogFrame" :key="index">
                        <v-card-title class="ma-0 pa-0">
                        </v-card-title>
                        <v-card-text>
                            <v-row class="pa-2">
                                <v-col class="pa-0 pt-2 ma-0 d-flex d-inline" style="color:#374151">
                                    <h5 class="subtitle-2">
                                        <v-avatar size="25">
                                            <v-img :src="LebelsIcon" alt="lebels_icon" size="15"></v-img>
                                        </v-avatar>
                                        {{ item.WorkflowLevelName }}
                                    </h5>
                                    <v-spacer></v-spacer>
                                    <small class="text-right" style="color:#374151">{{
                                        formatDateAndTime(item.SubmissionDate) }}</small>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col class="pa-2 ma-0">
                                    <div v-if="item.WorkflowStageStatus == 'Approved'"
                                        class="heading-3-dark font-weight-bolder" style="color:#8EDC92">
                                        <v-avatar size="25" style="background:#8EDC92">
                                            <v-img :src="ApprovedIcon" class="pa-1" alt="action_icon"></v-img>
                                        </v-avatar>
                                        {{ item.WorkflowStageStatus }}
                                    </div>
                                    <div v-else-if="item.WorkflowStageStatus == 'Rejected'" class="heading-3-dark"
                                        style="color:#ED9A98">
                                        <v-avatar size="25" style="background:#ED9A98">
                                            <v-img :src="RejectedIcon" class="pa-1" alt="action_icon"></v-img>
                                        </v-avatar>
                                        {{ item.WorkflowStageStatus }}
                                    </div>
                                    <div v-else-if="item.WorkflowStageStatus == 'Submitted'" class="heading-3-dark"
                                        style="color:#7395DA">
                                        <v-avatar size="25" style="background:#7395DA">
                                            <v-img :src="SubmittedIcon" alt="action_icon"></v-img>
                                        </v-avatar>
                                        {{ item.WorkflowStageStatus }}
                                    </div>
                                    <div v-else-if="item.WorkflowStageStatus == 'Assigned'" class="heading-3-dark"
                                        style="color:#7395DA">
                                        <v-avatar size="25" style="background:#7395DA">
                                            <v-img :src="ApprovedIcon" class="pa-1" alt="action_icon"></v-img>
                                        </v-avatar>
                                        {{ item.WorkflowStageStatus }}
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col class="pa-2 ma-0">
                                    <div v-if="item.DefaultText.length > 0">
                                        <small color="#707070">{{ item.DefaultText }}</small>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col class="pa-0 px-2 ma-0 heading-4-dark">
                                    <v-avatar size="25" :color="`${getcolor(item.SubmittedBy)}`">
                                        <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP"
                                            v-if="item.ProfileImage.length > 0" />
                                        <span class="white--text" v-else>
                                            {{ item.SubmittedBy == null ? "N/A" : item.SubmittedBy.substring(0,
                                                1).toUpperCase() }}
                                        </span>
                                    </v-avatar>
                                    {{ item.SubmittedBy }}
                                </v-col>
                            </v-row>
                        </v-card-text>
                    </v-card>
                </v-card-text>
            </v-card>
        </v-dialog>

        <!--Survey Benificiary Linked dialog-->
        <template>
            <v-dialog v-model="SurveyBenLinkDlg" persistent :fullscreen="$vuetify.breakpoint.smAndDown">
                <v-card class="myTasksDialogCardHeight">
                    <v-card-title>
                        <v-btn v-show="BenefLinkedSurveySteps > 2" text small @click="fnBackspace()">
                            <v-icon color="#707070" size="20">mdi-keyboard-backspace</v-icon>
                        </v-btn>
                        <span class="limited-width-CardTitle text-truncate page-head hidden-sm-and-down">{{
                            SelectedSurveyName }}</span>
                        <span class="limited-width-CardTitle text-truncate page-head hidden-md-and-up"
                            v-if="BenefLinkedSurveySteps != 4">{{ SelectedSurveyName }}</span>
                        <v-spacer></v-spacer>
                        <v-btn text v-if="BenefLinkedSurveySteps == 4" @click.stop="fnResetQuestionsAns()"
                            class="hidden-md-and-up">
                            <v-icon color="black" size="30">mdi-autorenew</v-icon>
                        </v-btn>
                        <v-btn text v-if="BenefLinkedSurveySteps == 4" @click.stop="GetBenefSurveyQstnsForImport()"
                            class="hidden-md-and-up">
                            <v-icon color="black" size="30">mdi-download-outline</v-icon>
                        </v-btn>
                        <v-btn text @click.stop="fnClearAnyTimeList(), fnClearData()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text class="px-0 py-0 ma-0 hidden-sm-and-down">
                        <v-list dense class="py-0 ma-0" style="background: rgba(61, 126, 254, 0.1);">
                            <template>
                                <v-list-item class="py-0">
                                    <v-list-item-icon>
                                        <v-avatar size="40" :color="`${getcolor(DataCollectorInfo.DatacollectorName)}`">
                                            <!--<img v-bind:src="userimage + DataCollectorInfo.ProfileImage" alt="User DP" v-if="DataCollectorInfo.ProfileImage.length >0" />-->
                                            <span class="white--text">{{ DataCollectorInfo.DatacollectorName == null ? "N/A"
                                                : DataCollectorInfo.DatacollectorName.substring(0, 1).toUpperCase()
                                            }}</span>
                                        </v-avatar>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        <v-list-item-title class="py-0 pb-0">
                                            <v-row class="d-lg-inline-flex py-0 pb-0">
                                                <v-col class="px-0 pl-3 py-0">
                                                    <h4 class="font-weight-large   py-0 pb-0 px-0">
                                                        {{ DataCollectorInfo.DatacollectorName }}</h4>
                                                </v-col>
                                                <v-col class="ml-3 px-0 py-0">
                                                    <h4 class="font-weight-regular  py-0 pb-0">
                                                        {{ DataCollectorInfo.AssignedTime }}</h4>
                                                </v-col>
                                            </v-row>
                                        </v-list-item-title>
                                        <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                            <h4 class="font-weight-regular py-0 pb-0 px-0">Survey Data has been assigned.
                                            </h4>
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-spacer></v-spacer>
                                    <v-btn text @click.stop="">
                                        <v-icon color="primary" size="30">mdi-chevron-up</v-icon>
                                    </v-btn>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-card-text>
                    <v-divider class="hidden-sm-and-down"></v-divider>

                    <v-row no-gutters justify="center" style="background-color: #F5F6F8;">
                        <v-col cols="12" md="12" sm="12" xs="12">
                            <v-stepper v-model="BenefLinkedSurveySteps" class="elevation-0">
                                <v-stepper-items>
                                    <!-- Select Create or Existing -->
                                    <v-stepper-content step="1" class="pa-0">
                                        <v-row class="justify-center" style="background-color: #F5F6F8;">
                                            <v-col md="6" sm="10" xs="10" class="v-scrolling-div" style="height:74vh;">
                                                <v-col md="12" sm="12" xs="12" class="text-center">
                                                    <h3>Please Choose How You Like To Proceed</h3>
                                                </v-col>
                                                <v-col md="12" sm="12" xs="12">
                                                    <v-hover v-slot="{ hover }">
                                                        <v-card
                                                            @click="SearchExistingBeneficiary = true, AddNewBenificiary = false, fnSearchBeneficiaries()"
                                                            v-model="SearchExistingBeneficiary" :elevation="hover ? 12 : 2">
                                                            <v-card-title>
                                                                <v-avatar tile size="70" color="rgba(61, 126, 254, 0.1)">
                                                                    <v-icon size="40" color="primary">mdi-magnify</v-icon>
                                                                </v-avatar>
                                                                <v-spacer></v-spacer>
                                                                <h5>Search Existing Benificiary</h5>
                                                            </v-card-title>
                                                        </v-card>
                                                    </v-hover>
                                                </v-col>
                                                <v-col md="12" sm="12" xs="12">
                                                    <v-hover v-slot="{ hover }">
                                                        <v-card
                                                            @click="AddNewBenificiary = true, SearchExistingBeneficiary = false, GetBenefRegistrationQstns()"
                                                            v-model="AddNewBenificiary" :elevation="hover ? 12 : 2">
                                                            <v-card-title>
                                                                <v-avatar tile size="70" color="rgba(61, 126, 254, 0.1)"
                                                                    style="box-shadow:5px">
                                                                    <v-icon size="40"
                                                                        color="primary">mdi-account-plus-outline</v-icon>
                                                                </v-avatar>
                                                                <v-spacer></v-spacer>
                                                                <h5>Add New Benificiary</h5>
                                                            </v-card-title>
                                                        </v-card>
                                                    </v-hover>
                                                </v-col>
                                                <v-col md="12" sm="12" xs="12" v-if="SaveAndContinue">
                                                    <v-hover v-slot="{ hover }">
                                                        <v-card @click="fnGetDataFromSavedList('Saved')"
                                                            :elevation="hover ? 12 : 2">
                                                            <v-card-title>
                                                                <v-avatar tile size="70" color="rgba(61, 126, 254, 0.1)"
                                                                    style="box-shadow:5px">
                                                                    <v-icon size="40"
                                                                        color="primary">mdi-account-plus-outline</v-icon>
                                                                </v-avatar>
                                                                <v-spacer></v-spacer>
                                                                <h5>Continue Where You Left</h5>
                                                            </v-card-title>
                                                        </v-card>
                                                    </v-hover>
                                                </v-col>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- Select from Exisiting beneficiaries-->
                                    <v-stepper-content step="2" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col cols="12" class="v-scrolling-div pa-0 ma-0 myTasksInnerCardHeight">
                                                <v-col cols="12" class="d-flex flex-wrap pa-0 ma-2">
                                                    <v-col md="5" xs="6" class="pa-0 ma-0">
                                                        <input label="Search Beneficiaries"
                                                            v-model.lazy="vmSearchBeneficiary" placeholder="Search"
                                                            class="custom-text-field elevation-0"
                                                            append-icon="mdi-magnify" />
                                                    </v-col>
                                                    <!--<v-col md="2" xs="2" align="center" class="pa-0 ma-0">
                                                    <v-btn height="39" dense="" style="background-color:white" class="" @click="">
                                                        <v-icon>mdi-tune-vertical</v-icon>
                                                        <span>Filter</span>
                                                    </v-btn>
                                                </v-col>-->
                                                    <v-col md="3" xs="2" class="pa-0 mx-2">
                                                        <v-btn height="39" dense="" style="background-color:white;" class=""
                                                            @click="GetBenefRegistrationQstns()">
                                                            <v-icon>mdi-plus</v-icon>
                                                            <span>Add</span>
                                                        </v-btn>
                                                    </v-col>
                                                    <v-col md="3" xs="2" class="pa-0 ma-0">
                                                        <v-card align="center" style="background-color:white;"
                                                            class="hidden-sm-and-down">
                                                            <div class="pa-2">{{ lstSelectedBeneficiary.length }}
                                                                beneficiaries selected</div>
                                                        </v-card>
                                                        <v-btn height="39" dense="" tile style="background-color:white;"
                                                            class="hidden-md-and-up" @click="">
                                                            <v-icon>mdi-account-multiple-outline</v-icon>
                                                            <span>{{ lstSelectedBeneficiary.length }}</span>
                                                        </v-btn>
                                                    </v-col>
                                                </v-col>
                                                <v-col cols="12" class="mx-2 pa-0">
                                                    <v-data-table :headers="BeneficiaryTableheaders"
                                                        v-model="lstSelectedBeneficiary" :items="lstBeneficiary"
                                                        item-key="BeneficiaryID" sort-by="BeneficiaryName"
                                                        :search="vmSearchBeneficiary" show-select fixed-header height="45vh"
                                                        class="elevation-0">
                                                    </v-data-table>
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white;"
                                                class="ma-0 pa-0">
                                                <Default-Button-Outlined class="btn-122x36 ma-2" title="Cancel"
                                                    @click.native="fnClearAnyTimeList(), fnClearData()"></Default-Button-Outlined>
                                                <Secondary-Button class="btn-122x36 mr-2" title="Proceed"
                                                    @click.native="GetBeneficiayBasedSectionQuestions()"
                                                    v-bind:disabled="lstSelectedBeneficiary.length == 0"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- How to add new beneficiary-->
                                    <v-stepper-content step="3" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col cols="12">
                                                <v-col cols="12" class="v-scrolling-div px-4 myTasksInnerCardHeight">
                                                    <v-tabs v-model="BenefImportTab" id="scrollToTop">
                                                        <v-tab href='#tab-3'>Upload</v-tab>
                                                        <v-tab href='#tab-4'>Manual</v-tab>
                                                        <!--Upload-->
                                                        <v-tab-item value="tab-3" style="background-color: #F5F6F8;">
                                                            <v-row class="d-flex justify-center">
                                                                <v-col cols="12" md="12" class="mt-6" align="center">

                                                                    <!-- Drag and Drop Section-->
                                                                    <div class="text-center box_outline" @dragover.prevent
                                                                        @drop.prevent
                                                                        v-if="lstImportBeneficiary.length == 0">
                                                                        <div @drop="dragFile">
                                                                            <div><v-icon color="#535353"
                                                                                    size="70">mdi-file</v-icon></div>
                                                                            <div v-if="File.length == 0" @click="">
                                                                                <a href="#">Drag & Drop The Filled Out
                                                                                    Form</a><br />
                                                                                <a href="#">Upload Only Xls File (Not Above
                                                                                    15mb Of Size)</a>
                                                                            </div>
                                                                            <div v-if="File.length" class="mt-4">
                                                                                <div v-for="file in File" :key="file">
                                                                                    <v-chip close="" label="" color="white">
                                                                                        <v-icon size="20" color="success"
                                                                                            height="100">mdi-file-delimited-outline</v-icon>
                                                                                        {{ file.name }}
                                                                                    </v-chip>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <!-- Show vue-excel-editor -->
                                                                    <div no-gutters class="uploadBoxHeight pa-0"
                                                                        v-if="lstImportBeneficiary.length > 0">
                                                                        <v-col cols="12" xs="12" sm="12" md="12"
                                                                            class="text-center pa-0">
                                                                            <h4 class="heading-3">We are running a format
                                                                                check this will take a moment</h4>
                                                                        </v-col>

                                                                        <v-col cols="12" xs="12" sm="12" md="12"
                                                                            class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                            <vue-excel-editor no-paging
                                                                                :resetOnClick="resetOnClick"
                                                                                disable-panel-setting disable-panel-filter
                                                                                v-model="lstImportBeneficiary">
                                                                                <div
                                                                                    v-for="Hearder in lstImportBeneficiaryHearder">
                                                                                    <vue-excel-column :field="Hearder.text"
                                                                                        :type="Hearder.type"
                                                                                        :mandatory="Hearder.mandatory"
                                                                                        :options="Hearder.options"
                                                                                        :validate="onBeforeMandatoryChange"
                                                                                        width="80px" />
                                                                                </div>
                                                                            </vue-excel-editor>
                                                                        </v-col>
                                                                    </div>
                                                                    <div class="mt-4" align="center"
                                                                        v-if="lstImportBeneficiary.length == 0">
                                                                        <!--<input ref="uploader"
                                                                    class="d-none"
                                                                    type="file"
                                                                    accept=""
                                                                    @change="onFileChanged">-->
                                                                        <Secondary-Button class="btn-122x36 mr-2"
                                                                            title="Download"
                                                                            @click.native="DownloadBenefRegSampleFile()"></Secondary-Button>
                                                                    </div>
                                                                </v-col>
                                                            </v-row>
                                                        </v-tab-item>
                                                        <!--Manual-->
                                                        <v-tab-item value="tab-4" style="background-color: #F5F6F8;"
                                                            class="v-scrolling-div" height="30vh">
                                                            <v-row>
                                                                <v-col md="8" class="ma-0">
                                                                    <div v-for="(item, index) in lstBenefRegQstns">
                                                                        <label v-bind:id="item.QuestionID.toString()"
                                                                            class="text-left heading-3 mb-2">{{
                                                                                item.QuestionName }}</label>
                                                                        <sub v-if="item.IsMandatoryResponse == true"
                                                                            style="color:red;font-size:large"> *</sub>
                                                                        <!-- Question Hint-->
                                                                        <sub style="color:blue;font-size:smaller"
                                                                            v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{
                                                                                item.QuestionHint }}</sub>
                                                                        <!-- error text-->
                                                                        <sub style="color:red;font-size:smaller"
                                                                            v-if="item.IsValidationError == true">{{
                                                                                item.ValidationErrorTest }}</sub>

                                                                        <div
                                                                            v-if="item.QuestionType == 'Single Line Text' && item.IsValidationQuestion == 1">
                                                                            <input type="text" class="custom-text-field"
                                                                                solo :value="item.Answer"
                                                                                @change="fnBindValue($event, index)" />
                                                                        </div>
                                                                        <div
                                                                            v-if="item.QuestionType == 'Single Line Text' && item.IsValidationQuestion == 0">
                                                                            <input type="text" class="custom-text-field"
                                                                                v-if="item.QuestionType == 'Single Line Text'"
                                                                                full-width outlined dense
                                                                                label="Type Your answer"
                                                                                :value="item.Answer"
                                                                                @change="fnBindValue($event, index)">

                                                                        </div>
                                                                        <!--Multiline Answer-->
                                                                        <textarea class="custom-text-field" full-width
                                                                            outlined dense
                                                                            v-if="item.QuestionType === 'Multi Line Text'"
                                                                            label="Type your answer" :value="item.Answer"
                                                                            @change="fnBindValue($event, index)"></textarea>
                                                                        <!--Numeric-->
                                                                        <div v-if="item.QuestionType === 'Numeric'"
                                                                            class="mt-0">
                                                                            <v-text-field type="number" full-width outlined
                                                                                dense solo label="Type your answer"
                                                                                :value="item.Answer"
                                                                                onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                :readonly="item.IsFormulaQuestion == true"
                                                                                :min="item.minVal" :max="item.maxVal"
                                                                                @change="onFocusCheckQstnValidation(item), fnBindValue($event, index)"
                                                                                :rules="item.qRules">
                                                                            </v-text-field>
                                                                        </div>
                                                                        <!--Single Choice -->
                                                                        <div v-if="item.QuestionType === 'Single Choice'"
                                                                            class="mt-0">

                                                                            <v-radio-group class="my-0 py-0"
                                                                                v-model="item.Answer">
                                                                                <v-radio color="success" hide-details
                                                                                    v-for="(optItem, ind) in item.BTQstnAnsOptionList"
                                                                                    :key="ind" :label="optItem.OptionValue"
                                                                                    :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>

                                                                        </div>
                                                                        <!--Multiple Choice -->
                                                                        <div v-if="item.QuestionType === 'Multi-Choice'"
                                                                            class="mt-0 mb-6">
                                                                            <div class="my-0 py-0"
                                                                                v-if="item.BTQstnAnsOptionList.length > 0">
                                                                                <v-checkbox color="success" hide-details
                                                                                    input-value="true"
                                                                                    :label="optItem.OptionValue"
                                                                                    v-for="(optItem, ind) in item.BTQstnAnsOptionList"
                                                                                    :key="ind"
                                                                                    v-model="optItem.IsOptSelected"
                                                                                    @change="GetMultiChoice_BT(optItem, optItem.IsOptSelected, optItem.OptionValue)"></v-checkbox>

                                                                            </div>
                                                                        </div>
                                                                        <!--Dropdown -->
                                                                        <div v-if="item.QuestionType === 'Dropdown'"
                                                                            class="mt-0 mb-6">
                                                                            <div v-if="item.BTQstnAnsOptionList.length > 0">
                                                                                <v-select dense
                                                                                    :items="item.BTQstnAnsOptionList"
                                                                                    :menu-props="{ maxHeight: '300' }"
                                                                                    filled item-text="OptionValue"
                                                                                    label="Select" outlined hide-details
                                                                                    v-model="item.Answer">
                                                                                </v-select>
                                                                            </div>
                                                                        </div>
                                                                        <!--Cascading Dropdown-->
                                                                        <div v-if="item.QuestionType === 'Cascading Dropdown'"
                                                                            class="mt-0 py-0 my-0">
                                                                            <v-row class="my-0 py-0">
                                                                                <v-col class="my-0 py-0">
                                                                                    <v-select dense
                                                                                        :menu-props="{ maxHeight: '300' }"
                                                                                        filled v-model="optItem[ind]"
                                                                                        :label="` ${optItem.OptionValue}`"
                                                                                        outlined=""
                                                                                        v-for="(optItem, ind) in item.CascadingQuestionOptions"
                                                                                        :key="ind"
                                                                                        :items="optItem.CascadingList"
                                                                                        item-text="Name" item-value="ID"
                                                                                        @change="FilterCascadingItems(item, optItem, optItem[ind], true, 1, -1)">
                                                                                    </v-select>


                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                        <!--Date Picker-->
                                                                        <div class="mb-6"
                                                                            v-if="item.QuestionType === 'Date Picker'">
                                                                            <v-menu v-model="item.mBTDate"
                                                                                :close-on-content-click="false"
                                                                                :nudge-right="40"
                                                                                transition="scale-transition" offset-y
                                                                                min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field v-model="item.Answer"
                                                                                        prepend-inner-icon="mdi-calendar"
                                                                                        outlined readonly
                                                                                        @click:prepend-inner="item.mBTDate = true"
                                                                                        dense v-on="on"
                                                                                        hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model="item.Answer"
                                                                                    no-title @input="item.mBTDate = false"
                                                                                    format="DD-MM-YYYY"></v-date-picker>
                                                                            </v-menu>
                                                                        </div>
                                                                        <!--GPS Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="item.QuestionType === 'GPS Capture'"
                                                                            v-model="item.Answer">
                                                                            <span class="d-none">{{
                                                                                GetGPSCoordinates(item, true) }}</span>
                                                                            <label class="text-left heading-1 mt-3">{{
                                                                                item.Answer }}</label>
                                                                            <GmapMap :center="Coordinates" :zoom="13"
                                                                                style="width: 100%; height: 300px"
                                                                                @click.native="GetGPSCoordinates(item, true)">
                                                                                <GmapMarker :position="Coordinates"
                                                                                    :clickable="true" :draggable="true"
                                                                                    @dragend="updateCoordinates($event.latLng, item)" />
                                                                            </GmapMap>
                                                                        </div>
                                                                        <!--Photo Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="item.QuestionType === 'Photo Capture'">

                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="audUpload[index]"
                                                                                @change="item.IsUpload = audUpload[index]">
                                                                                <v-radio color="success"
                                                                                    label="Photo Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Photo Upload" hide-details>
                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="item.IsUpload === 0">
                                                                                <v-row>
                                                                                    <v-col md="12" class="my-0 py-0">

                                                                                        <v-btn
                                                                                            @click="startCamera = true, camOn = true, onStart()"
                                                                                            color="primary" depressed
                                                                                            class="mr-4">Start</v-btn>
                                                                                        <v-btn class="mr-4"
                                                                                            color="secondary" depressed
                                                                                            @click="onCapture(item, item.QuestionID, true, 1), startCamera = false, camOn = false"
                                                                                            v-bind:disabled="!camOn">Capture</v-btn>
                                                                                        <v-btn
                                                                                            @click="startCamera = false, camOn = false, onStop()"
                                                                                            v-bind:disabled="!camOn"
                                                                                            depressed>Stop</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row>
                                                                                    <v-col md="8" v-if="startCamera">
                                                                                        <vue-web-cam ref="webcam"
                                                                                            :device-id="deviceId"
                                                                                            width="100%" height="200px"
                                                                                            @started="onStarted"
                                                                                            @stopped="onStopped"
                                                                                            @error="onError"
                                                                                            @cameras="onCameras1"
                                                                                            @camera-change="onCameraChange1" />
                                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,true,0)">Capture</v-btn>-->

                                                                                    </v-col>
                                                                                    <v-col md="4" v-if="img1 != null">
                                                                                        <figure class="figure">
                                                                                            <img :src="img1"
                                                                                                class="center-block my-2"
                                                                                                max-width="100%"
                                                                                                height="200"
                                                                                                aspect-ratio="1.7" />
                                                                                        </figure>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>

                                                                            <div v-show="item.IsUpload === 1">
                                                                                <v-row>
                                                                                    <v-col md="4">

                                                                                        <v-file-input label="Upload Image"
                                                                                            outlined dense accept="image/*"
                                                                                            v-model="item.BTAnsBlob"
                                                                                            @change="BTAnsBlobQID = item.QuestionID, SaveFile(item, 'Photo', item.BTAnsBlob, true, true, 0)"></v-file-input>
                                                                                    </v-col>
                                                                                    <v-col md="4"
                                                                                        v-if="benefUploadedImg != null">
                                                                                        <v-img :src=benefUploadedImg
                                                                                            width="250" height="180"
                                                                                            aspect-ratio="1.7"
                                                                                            class="center-block my-2"></v-img>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>

                                                                        </div>
                                                                        <!--Audio Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="item.QuestionType === 'Audio Capture'">
                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="audUpload[index]"
                                                                                @change="item.IsUpload = audUpload[index]">
                                                                                <v-radio color="success"
                                                                                    label="Audio Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Audio Upload" hide-details>

                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="item.IsUpload === 0">
                                                                                <audio-recorder upload-url="YOUR_API_URL"
                                                                                    id="recAudiobenef" :play="playSound"
                                                                                    :attempts="1" :time="1"
                                                                                    :headers="headers"
                                                                                    :before-recording="callback"
                                                                                    :pause-recording="callback"
                                                                                    :after-recording="SaveAudioBT"
                                                                                    :before-upload="callback"
                                                                                    :successful-upload="callback"
                                                                                    :failed-upload="callback"
                                                                                    v-model="item.Answer"
                                                                                    @click.native="BTAnsBlobQID = item.QuestionID, FindRecordedFileItem(item)" />

                                                                            </div>
                                                                            <div v-show="item.IsUpload === 1">

                                                                                <v-file-input label="Upload Audio" outlined
                                                                                    dense accept="audio/*"
                                                                                    v-model="item.BTAnsBlob"
                                                                                    @change="BTAnsBlobQID = item.QuestionID, SaveFile(item, 'Audio', item.BTAnsBlob, true, true, 0)"></v-file-input>

                                                                            </div>
                                                                        </div>

                                                                        <!--Signature Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="item.QuestionType === 'Signature Capture'"
                                                                            v-model="item.Answer">
                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="audUpload[index]"
                                                                                @change="item.IsUpload = audUpload[index]">
                                                                                <v-radio color="success"
                                                                                    label="Signature Capture"
                                                                                    hide-details></v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Signature Upload"
                                                                                    hide-details></v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="item.IsUpload === 0">
                                                                                <v-card outlined>
                                                                                    <VueSignaturePad
                                                                                        :options="{ onBegin: () => { $refs.signaturePad1[0].resizeCanvas() } }"
                                                                                        width="100%" height="300px"
                                                                                        ref="signaturePad1" />
                                                                                </v-card>
                                                                                <div>
                                                                                    <v-btn color="secondary" depressed
                                                                                        class="mt-2 mr-2"
                                                                                        @click="saveSign(item.QuestionID, true, 0)">Save</v-btn>
                                                                                    <v-btn default depressed
                                                                                        class="mt-2 mr-2"
                                                                                        @click="clearSign(true, item.QuestionID)">Clear</v-btn>
                                                                                </div>
                                                                            </div>
                                                                            <div v-show="item.IsUpload === 1">
                                                                                <v-file-input label="Upload Signature"
                                                                                    outlined dense accept="image/*"
                                                                                    v-model="item.BTAnsBlob"
                                                                                    @change="BTAnsBlobQID = item.QuestionID, SaveFile(item, 'Signature', item.BTAnsBlob, true, true, 0)"></v-file-input>
                                                                            </div>
                                                                            <v-card
                                                                                v-if="benefUploadedSign != null && benefUploadedSign != ''">
                                                                                <v-img :src=benefUploadedSign
                                                                                    max-width="50%" height="150"
                                                                                    class="my-4" aspect-ratio="1.7"></v-img>
                                                                            </v-card>
                                                                        </div>
                                                                        <!--Note-->
                                                                        <div v-if="item.QuestionType === 'Note'">
                                                                            <!-- <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea> -->
                                                                            <!--<textarea class="custom-text-field" outlined dense hide-details v-model="item.Answer"></textarea>-->
                                                                        </div>
                                                                        <!--File Upload-->
                                                                        <div v-if="item.QuestionType === 'File Upload'">

                                                                            <v-file-input label="Upload File" outlined dense
                                                                                accept="*" v-model="item.BTAnsBlob"
                                                                                @change="BTAnsBlobQID = item.QuestionID, SaveFile('File', item.BTAnsBlob, true, true, 0)"></v-file-input>
                                                                            <label class="text-left heading-1 mt-3">{{
                                                                                item.Answer }}</label>
                                                                            <div v-if="benefUploadedFile != null">
                                                                                <v-img :src=benefUploadedFile width="50"
                                                                                    height="80" aspect-ratio="1.7"
                                                                                    class="center-block my-2"></v-img>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </v-col>
                                                            </v-row>
                                                        </v-tab-item>
                                                    </v-tabs>
                                                </v-col>
                                                <v-col cols="12" align="right" style="background-color:white" class="py-2">
                                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Cancel"
                                                        @click.native="fnCancelBenefRegistration(BenefImportTab)"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Update"
                                                        @click.native="RegisterNewBenficiary(BenefImportTab)"></Secondary-Button>
                                                </v-col>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- Survey Question Update Manually -->
                                    <v-stepper-content step="4" class="pa-0 ma-0">
                                        <v-row style="background-color: #F5F6F8;" no-gutters>
                                            <v-col cols="12" class="pa-0 ma-0">
                                                <v-col cols="12"
                                                    class="pa-0 ma-0 d-md-inline-flex d-sm-block hidden-sm-and-down">
                                                    <!-- Beneficiary Stepper-->
                                                    <v-col cols="6" md="6" sm="12" xs="12"
                                                        class="d-inline-flex pa-0 ma-md-2">
                                                        <div class="text-center ml-4"
                                                            style="background-color:white;padding:7px 8px; width:auto"
                                                            v-for="(benefItem, Index) in vmSelectedBeneficiaries"
                                                            :key="Index" v-if="BenefStepper == Index + 1">
                                                            <div class="text-center text-limit">
                                                                {{ benefItem.BeneficiaryName }}</div>
                                                        </div>
                                                        <div class="text-center mx-1"
                                                            style="background-color:white;padding:7px 8px;">
                                                            {{ BenefStepper }}/{{ vmSelectedBeneficiaries.length }}
                                                        </div>
                                                        <v-btn style="border-radius:0px" class="elevation-0"
                                                            @click="previousBeneficiary(), scrollToNavbarTop()"><v-icon
                                                                color="primary">mdi-chevron-left</v-icon></v-btn>
                                                        <v-btn style="border-radius:0px" class="elevation-0"
                                                            @click="nextBeneficiary(), scrollToNavbarTop()"><v-icon
                                                                color="primary">mdi-chevron-right</v-icon></v-btn>
                                                    </v-col>
                                                    <v-spacer class="hidden-sm-and-down"></v-spacer>
                                                    <!-- Import-->
                                                    <v-col cols="3" md="3" sm="12" xs="12"
                                                        class="d-inline-flex pa-0 ma-md-2" align="right">
                                                        <v-btn elevation="1" tile style="border-radius:3px;" color="white"
                                                            @click="fnResetQuestionsAns()"
                                                            class="hidden-sm-and-down">Reset</v-btn>
                                                        <v-btn elevation="1" tile style="border-radius:3px;" color="white"
                                                            class="mx-1 hidden-sm-and-down"
                                                            @click.native="GetBenefSurveyQstnsForImport()"><v-icon
                                                                left>mdi-download</v-icon>Import</v-btn>
                                                    </v-col>
                                                </v-col>
                                                <!--<v-col cols="12" class="pa-0 ma-0">-->
                                                <!-- Progresss-->
                                                <!--<v-col md="8" class="mx-md-4">
                                                <h4 class="text-left heading-3-dark">{{CompletionPerc}}% To Complete Survey</h4><br />
                                                <v-progress-linear v-model="CompletionPerc" :buffer-value="bufferValue" color="primary">
                                                </v-progress-linear>
                                            </v-col>-->
                                                <!--</v-col>-->
                                                <v-card class="elevation-2 ma-2 hidden-md-and-up"
                                                    style="border-radius: 16px; background-color: #D4EBFF">
                                                    <v-card-title>
                                                        <v-icon size="24"
                                                            color="black">mdi-account-multiple-outline</v-icon>
                                                        <span style="padding:7px 8px; width:auto"
                                                            v-for="(benefItem, Index) in vmSelectedBeneficiaries"
                                                            :key="Index" v-if="BenefStepper == Index + 1">
                                                            <span class="text-center text-limit">{{
                                                                benefItem.BeneficiaryName }}</span>
                                                        </span>
                                                        <v-spacer></v-spacer>
                                                        <span class="text-center mx-1" style="padding:7px 8px;">
                                                            0{{ BenefStepper }}|0{{ vmSelectedBeneficiaries.length }}
                                                        </span>
                                                        <v-icon
                                                            @click="previousBeneficiary(), scrollToNavbarTop()">mdi-chevron-left</v-icon></v-btn>
                                                        <v-icon
                                                            @click="nextBeneficiary(), scrollToNavbarTop()">mdi-chevron-right</v-icon></v-btn>
                                                    </v-card-title>
                                                </v-card>
                                                <v-col cols="12"
                                                    class="v-scrolling-div pa-0 ma-0 myTasksQuestionsFormHeight">
                                                    <!-- Survey Sections and Questions-->
                                                    <v-col cols="12" class="pa-0 ma-0">
                                                        <v-col md="8" class="mx-md-4">
                                                            <div id="scrollToTop"></div>
                                                            <div class="pa-0 ma-0"
                                                                v-for="(SecItem, Sindex) in lstSectionsQstns"
                                                                v-if="SecItem.IslogicSection == 0">

                                                                <div class="ma-0 pa-0">
                                                                    <v-avatar size="20" class="mb-1 ma-0 pa-0"
                                                                        style="background-color:#3374B9;color:white">{{
                                                                            Sindex
                                                                            + 1 }}</v-avatar>
                                                                    <span class="mx-1 page-head-dark"
                                                                        style="font-weight:bold">{{ SecItem.SectionName
                                                                        }}</span>
                                                                </div>
                                                                <div class="ma-0 pa-0">
                                                                    <span class="mx-1">{{ SecItem.SectionDescription
                                                                    }}</span>
                                                                </div>
                                                                <br />
                                                                <div v-for="(item, QIndex) in SecItem.Questions">

                                                                    <label class="text-left subtitle-2 mb-2"><b>{{
                                                                        item.QuestionName }}</b></label>

                                                                    <sub v-if="item.IsMandatory == true"
                                                                        style="color:red;font-size:large"> *</sub>

                                                                    <sub style="color:blue;font-size:smaller"
                                                                        v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{
                                                                            item.QuestionHint }}</sub>

                                                                    <sub style="color:red;font-size:smaller"
                                                                        v-if="(item.IsMandatory == true && item.Answer === '') || item.IsValidationError">{{
                                                                            item.ValidationErrorTest }}</sub>


                                                                    <!-- Single Line Text with validation and Condition -->
                                                                    <div v-if="item.QuestionType == 'Single Line Text'">
                                                                        <input type="text"
                                                                            v-if="item.QuestionType == 'Single Line Text'"
                                                                            solo class="custom-text-field elevation-0"
                                                                            label="Type your answer"
                                                                            v-model.lazy="item.Answer"
                                                                            @focus="CalculateText(item, item.QuestionID, BenefStepper)" />
                                                                    </div>

                                                                    <!--Multiline Answer-->
                                                                    <textarea type="text"
                                                                        class="custom-text-field elevation-0"
                                                                        v-if="item.QuestionType === 'Multi Line Text'" solo
                                                                        label="Type your answer"
                                                                        v-model.lazy="item.Answer" />

                                                                    <!--Numeric-->

                                                                    <div v-if="item.QuestionType === 'Numeric'"
                                                                        class="mt-0">

                                                                        <v-text-field full-width outlined dense
                                                                            type="number"
                                                                            v-if="item.QuestionType === 'Numeric'" solo
                                                                            label="Type your answer"
                                                                            v-model.lazy="item.Answer"
                                                                            onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                            :value="item.DefaultValue" :min="item.minVal"
                                                                            :max="item.maxVal"
                                                                            :readonly="item.IsFormulaQuestion == true"
                                                                            @change="onFocusCheckQstnValidation(item), AddSectionLogic(item, item.QuestionID, BenefStepper)"
                                                                            @focus="onFocusCheckQstnValidation(item), Calculate(item, item.QuestionID, BenefStepper)"
                                                                            :rules="item.qRules">
                                                                        </v-text-field>

                                                                        <sub class="mr-2 ml-2"
                                                                            style="color:green;font-size:large">{{ item.UOM
                                                                            }}</sub>

                                                                    </div>


                                                                    <!--Single Choice Answer-->
                                                                    <div v-if="item.QuestionType === 'Single Choice'"
                                                                        class="mt-0">
                                                                        <v-radio-group class="my-0 py-0"
                                                                            v-model.lazy="item.Answer"
                                                                            @change="AddSectionLogic(item, item.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(item, item.Answer.toUpperCase())">
                                                                            <v-radio color="success" hide-details
                                                                                v-for="(optItem, ind) in item.QuestionOptions"
                                                                                :key="ind" :label="optItem.OptionValue"
                                                                                :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!--Multiple Choice -->
                                                                    <div v-if="item.QuestionType === 'Multi-Choice'"
                                                                        class="mt-0 mb-6">

                                                                        <div class="my-0 py-0"
                                                                            v-if="item.QuestionOptions.length > 0">

                                                                            <v-checkbox color="success" hide-details
                                                                                input-value="true"
                                                                                :label="optItem.OptionValue"
                                                                                v-for="(optItem, ind) in item.QuestionOptions"
                                                                                :key="ind" v-model="optItem.IsOptSelected"
                                                                                @change="GetMultiChoice(optItem, optItem.IsOptSelected, optItem.OptionValue, item.QuestionID, BenefStepper), AddSectionLogicForMultiChoice(optItem, item.QuestionID, BenefStepper, item.QuestionOptions), ShowOtherOption(item, optItem.OptionValue.toUpperCase(), optItem.IsOptSelected)"></v-checkbox>
                                                                        </div>
                                                                    </div>

                                                                    <!--Dropdown -->
                                                                    <div v-if="item.QuestionType === 'Dropdown'"
                                                                        class="mt-0 mb-6">
                                                                        <div v-if="item.QuestionOptions.length > 0">
                                                                            <v-select dense :items="item.QuestionOptions"
                                                                                :menu-props="{ maxHeight: '300' }" filled
                                                                                item-text="OptionValue" label="Select"
                                                                                outlined hide-details
                                                                                v-model.lazy="item.Answer"
                                                                                @change="AddSectionLogic(item, item.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(item, item.Answer.toUpperCase())">
                                                                            </v-select>
                                                                        </div>
                                                                    </div>

                                                                    <!--Date Picker-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'Date Picker'">
                                                                        <v-menu v-model="item.DateMenu"
                                                                            :close-on-content-click="false"
                                                                            :nudge-right="40" transition="scale-transition"
                                                                            offset-y min-width="290px">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-text-field v-model.lazy="item.Answer"
                                                                                    prepend-inner-icon="mdi-calendar"
                                                                                    readonly
                                                                                    @click:prepend-inner="item.DateMenu = true"
                                                                                    v-on="on" hide-details></v-text-field>
                                                                            </template>
                                                                            <v-date-picker v-model.lazy="item.Answer"
                                                                                no-title @input="item.DateMenu = false"
                                                                                format="DD-MM-YYYY"
                                                                                @change=""></v-date-picker>
                                                                        </v-menu>
                                                                    </div>

                                                                    <!--Cascading Dropdown-->
                                                                    <div v-if="item.QuestionType === 'Cascading Dropdown'"
                                                                        class="mt-0">
                                                                        <v-row class="my-0 py-0">
                                                                            <v-col class="my-0 py-0">
                                                                                <v-select class="my-0 py-0" dense
                                                                                    :menu-props="{ maxHeight: '300' }"
                                                                                    filled v-model="optItem[ind]"
                                                                                    :label="` ${optItem.OptionValue}`"
                                                                                    outlined=""
                                                                                    v-for="(optItem, ind) in item.CascadingQuestionOptions"
                                                                                    :key="ind"
                                                                                    :items="optItem.CascadingList"
                                                                                    item-text="Name" item-value="ID"
                                                                                    @change="FilterCascadingItems(item, optItem, optItem[ind], false, BenefStepper, -1)">
                                                                                </v-select>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!--GPS Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'GPS Capture'"
                                                                        v-model.lazy="item.Answer">
                                                                        <span class="d-none">{{
                                                                            GetGPSCoordinates(item, false) }}</span>
                                                                        <label class="text-left heading-1 mt-3">{{
                                                                            item.Answer }}</label>
                                                                        <GmapMap :center="Coordinates" :zoom="13"
                                                                            style="width: 100%; height: 200px"
                                                                            @click.native="GetGPSCoordinates(item, false)">
                                                                            <GmapMarker :position="Coordinates"
                                                                                :clickable="true" :draggable="true"
                                                                                @dragend="updateCoordinates($event.latLng, item)" />
                                                                        </GmapMap>
                                                                    </div>

                                                                    <!--File Upload-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'File Upload'">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <v-file-input outlined dense accept="*"
                                                                                    v-model="item.AnsBlob"
                                                                                    @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'File', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                            </v-col>
                                                                            <v-col md="4" v-if="item.uploadedFile != null">
                                                                                <label class="text-left heading-1 mt-3">{{
                                                                                    item.Answer }}</label>
                                                                                <v-img :src=item.uploadedFile width="250"
                                                                                    height="180" aspect-ratio="1.7"
                                                                                    class="center-block my-2"></v-img>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!-- Likert Scale-->
                                                                    <div v-if="item.QuestionType === 'Likert Scale'"
                                                                        class="mt-0 my-0 py-0">
                                                                        <v-radio-group class="my-0 py-0"
                                                                            v-model.lazy="item.Answer">
                                                                            <v-radio color="success" class="my-0 py-0"
                                                                                hide-details
                                                                                v-for="(optItem, ind) in item.QuestionOptions"
                                                                                :key="ind" :label="optItem.OptionValue"
                                                                                :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!-- Photo Caputure-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'Photo Capture'">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="audUpload[QIndex]"
                                                                            @change="item.IsUpload = audUpload[QIndex]">
                                                                            <v-radio color="success" label="Photo Capture"
                                                                                hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Photo Upload"
                                                                                hide-details>
                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <!-- Photo Capture-->
                                                                        <div v-show="item.IsUpload === 0">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-btn
                                                                                        @click="item.startCamera = true, item.camOn = true, onStart()"
                                                                                        color="primary" depressed
                                                                                        class="mr-4">Start</v-btn>
                                                                                    <v-btn class="mr-4" color="secondary"
                                                                                        depressed
                                                                                        @click="onCapture(item, item.QuestionID, false, BenefStepper), item.startCamera = false, item.camOn = false"
                                                                                        v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                                    <v-btn
                                                                                        @click="item.startCamera = false, item.camOn = false, onStop()"
                                                                                        v-bind:disabled="!item.camOn"
                                                                                        depressed class="mr-4">Stop</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col md="8" v-if="item.startCamera">
                                                                                    <vue-web-cam ref="webcam"
                                                                                        :device-id="deviceId" width="100%"
                                                                                        height="200px" @started="onStarted"
                                                                                        @stopped="onStopped"
                                                                                        @error="onError"
                                                                                        @cameras="onCameras"
                                                                                        @camera-change="onCameraChange" />
                                                                                    <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                </v-col>

                                                                                <v-col md="4"
                                                                                    v-if="item.img != null && item.img != ''">
                                                                                    <figure class="figure">
                                                                                        <img :src="item.img"
                                                                                            class="center-block my-2"
                                                                                            max-width="100%" height="200"
                                                                                            aspect-ratio="1.7" />
                                                                                    </figure>
                                                                                </v-col>

                                                                            </v-row>
                                                                        </div>
                                                                        <!-- Photo Upload-->
                                                                        <div v-show="item.IsUpload === 1">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-file-input label="Upload Image"
                                                                                        outlined dense accept="image/*"
                                                                                        v-model="item.AnsBlob"
                                                                                        @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Photo', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4"
                                                                                    v-if="item.uploadedImg != null">
                                                                                    <v-img :src=item.uploadedImg width="250"
                                                                                        height="180" aspect-ratio="1.7"
                                                                                        class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                    </div>

                                                                    <!--Signature Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'Signature Capture'"
                                                                        v-model="item.Answer">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="audUpload[QIndex]"
                                                                            @change="item.IsUpload = audUpload[QIndex]">
                                                                            <v-radio color="success"
                                                                                label="Signature Capture"
                                                                                hide-details></v-radio>
                                                                            <v-radio color="success"
                                                                                label="Signature Upload"
                                                                                hide-details></v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload === 0">
                                                                            <v-card outlined>
                                                                                <VueSignaturePad
                                                                                    :options="{ onBegin: () => { $refs.signaturePad[0].resizeCanvas() } }"
                                                                                    width="100%" height="300px"
                                                                                    ref="signaturePad" />
                                                                            </v-card>
                                                                            <div>
                                                                                <v-btn color="secondary" depressed
                                                                                    class="mt-2 mr-2"
                                                                                    :disabled=item.signaturebutton
                                                                                    @click="saveSign(item.QuestionID, false, BenefStepper, item), item.signaturebutton = true">Save</v-btn>
                                                                                <v-btn default depressed class="mt-2 mr-2"
                                                                                    @click="clearSign(item, false, item.QuestionID), item.signaturebutton = false">Clear</v-btn>
                                                                            </div>
                                                                        </div>
                                                                        <div v-show="item.IsUpload === 1">

                                                                            <v-file-input label="Upload Signature" outlined
                                                                                dense accept="image/*"
                                                                                v-model="item.AnsBlob"
                                                                                @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Signature', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                        </div>
                                                                        <v-card
                                                                            v-if="item.uploadedSign != null && item.uploadedSign != ''">
                                                                            <v-img :src=item.uploadedSign max-width="50%"
                                                                                height="150" class="my-4"
                                                                                aspect-ratio="1.7"></v-img>
                                                                            <!--max-width="100%" height="200" aspect-ratio="1.7"-->
                                                                            <!--<v-img :src=uploadedSign width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>-->
                                                                        </v-card>
                                                                    </div>

                                                                    <!--Audio Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="item.QuestionType === 'Audio Capture'">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="audUpload[QIndex]"
                                                                            @change="item.IsUpload = audUpload[QIndex]">
                                                                            <v-radio color="success" label="Audio Capture"
                                                                                hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Audio Upload"
                                                                                hide-details>

                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload === 0">
                                                                            <audio-recorder upload-url="YOUR_API_URL"
                                                                                id="recAudioOne" :play="playSound"
                                                                                :attempts="1" :time="1" :headers="headers"
                                                                                :before-recording="callback"
                                                                                :pause-recording="callback"
                                                                                :after-recording="SaveAudio"
                                                                                :before-upload="callback"
                                                                                :successful-upload="callback"
                                                                                :failed-upload="callback"
                                                                                v-model="item.Answer"
                                                                                @click.native="AnsBlobQID = item.QuestionID, FindRecordedFileItem(item)" />

                                                                        </div>
                                                                        <div v-show="item.IsUpload === 1">
                                                                            <v-file-input label="Upload Audio" outlined
                                                                                dense accept="audio/*"
                                                                                v-model="item.AnsBlob"
                                                                                @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Audio', item.AnsBlob, false, true, BenefStepper)"></v-file-input>

                                                                        </div>
                                                                    </div>
                                                                    <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                    <div v-if="item.isOtherOptionEnabled == true">
                                                                        <input type="text" solo
                                                                            class="custom-text-field elevation-0"
                                                                            label="Type your answer"
                                                                            v-model.lazy="item.OtherOptionAnswer" />
                                                                    </div>
                                                                    <!-- For Showing repeat or skip section Question Details-->
                                                                    <div v-if="RepeatOrSkipSection.length > 0 && Ritem.ParentQuestionID == item.QuestionID && BenefStepper == Ritem.benefIndex"
                                                                        v-for="(Ritem, RIndex) in RepeatOrSkipSection"
                                                                        :key="RIndex">

                                                                        <label class="text-left heading-3 mb-2"><b>{{
                                                                            Ritem.QuestionName }}</b></label>

                                                                        <sub v-if="Ritem.IsMandatory == true"
                                                                            style="color:red;font-size:large"> *</sub>

                                                                        <sub style="color:blue;font-size:smaller"
                                                                            v-if="Ritem.QuestionHint != null && Ritem.QuestionHint != 'null' && Ritem.QuestionHint.length > 0">{{
                                                                                Ritem.QuestionHint }}</sub>

                                                                        <sub style="color:red;font-size:smaller"
                                                                            v-if="(Ritem.IsMandatory == true && Ritem.Answer === '') || Ritem.IsValidationError">{{
                                                                                Ritem.ValidationErrorTest }}</sub>

                                                                        <!-- Single Line Text with validation and Condition -->
                                                                        <div
                                                                            v-if="Ritem.QuestionType == 'Single Line Text'">
                                                                            <input type="text"
                                                                                v-if="Ritem.QuestionType == 'Single Line Text'"
                                                                                solo class="custom-text-field elevation-0"
                                                                                label="Type your answer"
                                                                                v-model.lazy="Ritem.Answer"
                                                                                @focus="CalculateText(Ritem, Ritem.QuestionID, BenefStepper)" />
                                                                        </div>

                                                                        <!--Multiline Answer-->
                                                                        <input type="text"
                                                                            class="custom-text-field elevation-0"
                                                                            v-if="Ritem.QuestionType === 'Multi Line Text'"
                                                                            solo label="Type your answer"
                                                                            v-model.lazy="Ritem.Answer" />

                                                                        <!--Numeric-->

                                                                        <div v-if="Ritem.QuestionType === 'Numeric'"
                                                                            class="mt-0">

                                                                            <input type="number"
                                                                                v-if="Ritem.QuestionType === 'Numeric'" solo
                                                                                class="custom-text-field elevation-0"
                                                                                label="Type your answer"
                                                                                v-model.lazy="Ritem.Answer"
                                                                                :readonly="Ritem.IsFormulaQuestion == true"
                                                                                @focus="Calculate(Ritem, Ritem.QuestionID, BenefStepper)" />

                                                                            <sub class="mr-2 ml-2"
                                                                                style="color:green;font-size:large">{{
                                                                                    Ritem.UOM }}</sub>

                                                                        </div>


                                                                        <!--Single Choice Answer-->
                                                                        <div v-if="Ritem.QuestionType === 'Single Choice'"
                                                                            class="mt-0">
                                                                            <v-radio-group class="my-0 py-0"
                                                                                v-model.lazy="Ritem.Answer"
                                                                                @change="AddSectionLogic(Ritem, Ritem.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(Ritem, Ritem.Answer.toUpperCase())">
                                                                                <v-radio color="success" hide-details
                                                                                    v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                    :key="ind" :label="optItem.OptionValue"
                                                                                    :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!--Multiple Choice -->
                                                                        <div v-if="Ritem.QuestionType === 'Multi-Choice'"
                                                                            class="mt-0 mb-6">

                                                                            <div class="my-0 py-0"
                                                                                v-if="Ritem.QuestionOptions.length > 0">

                                                                                <v-checkbox color="success" hide-details
                                                                                    input-value="true"
                                                                                    :label="optItem.OptionValue"
                                                                                    v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                    :key="ind"
                                                                                    v-model="optItem.IsOptSelected"
                                                                                    @change="GetMultiChoiceRepeat(optItem, optItem.IsOptSelected, optItem.OptionValue, Ritem.RQstInd, Ritem.QuestionID, BenefStepper), ShowOtherOption(Ritem, optItem.OptionValue.toUpperCase(), optItem.IsOptSelected)"></v-checkbox>
                                                                            </div>
                                                                        </div>

                                                                        <!--Dropdown -->
                                                                        <div v-if="Ritem.QuestionType === 'Dropdown'"
                                                                            class="mt-0 mb-6">
                                                                            <div v-if="Ritem.QuestionOptions.length > 0">
                                                                                <v-select dense
                                                                                    :items="Ritem.QuestionOptions"
                                                                                    :menu-props="{ maxHeight: '300' }"
                                                                                    filled item-text="OptionValue"
                                                                                    label="Select" outlined hide-details
                                                                                    v-model.lazy="Ritem.Answer"
                                                                                    @change="AddSectionLogic(Ritem, Ritem.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(Ritem, Ritem.Answer.toUpperCase())">
                                                                                </v-select>
                                                                            </div>
                                                                        </div>

                                                                        <!--Date Picker-->
                                                                        <div class="mb-6"
                                                                            v-if="Ritem.QuestionType === 'Date Picker'">
                                                                            <v-menu v-model="Ritem.DateMenu"
                                                                                :close-on-content-click="false"
                                                                                :nudge-right="40"
                                                                                transition="scale-transition" offset-y
                                                                                min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field
                                                                                        v-model.lazy="Ritem.Answer"
                                                                                        prepend-inner-icon="mdi-calendar"
                                                                                        readonly
                                                                                        @click:prepend-inner="Ritem.DateMenu = true"
                                                                                        v-on="on"
                                                                                        hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model.lazy="Ritem.Answer"
                                                                                    no-title @input="Ritem.DateMenu = false"
                                                                                    format="DD-MM-YYYY"
                                                                                    @change=""></v-date-picker>
                                                                            </v-menu>
                                                                        </div>

                                                                        <!--Cascading Dropdown-->
                                                                        <div v-if="Ritem.QuestionType === 'Cascading Dropdown'"
                                                                            class="mt-0">
                                                                            <v-row class="my-0 py-0">
                                                                                <v-col class="my-0 py-0">
                                                                                    <v-select class="my-0 py-0" dense
                                                                                        :menu-props="{ maxHeight: '300' }"
                                                                                        filled v-model="optItem[ind]"
                                                                                        :label="` ${optItem.OptionValue}`"
                                                                                        outlined=""
                                                                                        v-for="(optItem, ind) in Ritem.CascadingQuestionOptions"
                                                                                        :key="ind"
                                                                                        :items="optItem.CascadingList"
                                                                                        item-text="Name" item-value="ID"
                                                                                        @change="FilterCascadingItems(Ritem, optItem, optItem[ind], false, BenefStepper, -1)">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!--GPS Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="Ritem.QuestionType === 'GPS Capture'"
                                                                            v-model.lazy="Ritem.Answer">

                                                                            <label class="text-left heading-1 mt-3">{{
                                                                                Ritem.Answer }}</label>
                                                                            <GmapMap :center="Coordinates" :zoom="13"
                                                                                style="width: 100%; height: 200px"
                                                                                @click.native="">
                                                                                <GmapMarker :position="Coordinates"
                                                                                    :clickable="true" :draggable="true"
                                                                                    @dragend="updateCoordinates($event.latLng, Ritem)" />
                                                                            </GmapMap>
                                                                        </div>

                                                                        <!--File Upload-->
                                                                        <div class="d-inline-flex pa-0 ma-0"
                                                                            style="width:100%"
                                                                            v-if="Ritem.QuestionType === 'File Upload'">

                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <!--<v-btn left elevation="1" tile style="border-radius:3px;" color="white" class="mx-1">UpLoad</v-btn>-->
                                                                                    <v-file-input truncate-length="15"
                                                                                        accept="*"
                                                                                        v-model.lazy="Ritem.AnsBlob"
                                                                                        @change="SaveFileRepeat(Ritem, 'File', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4"
                                                                                    v-if="Ritem.uploadedFile != null">
                                                                                    <v-img :src=Ritem.uploadedFile
                                                                                        width="250" height="180"
                                                                                        aspect-ratio="1.7"
                                                                                        class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!-- Likert Scale-->
                                                                        <div v-if="Ritem.QuestionType === 'Likert Scale'"
                                                                            class="mt-0 my-0 py-0">
                                                                            <v-radio-group class="my-0 py-0"
                                                                                v-model.lazy="Ritem.Answer">
                                                                                <v-radio color="success" class="my-0 py-0"
                                                                                    hide-details
                                                                                    v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                    :key="ind" :label="optItem.OptionValue"
                                                                                    :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!-- Photo Caputure-->
                                                                        <div class="mb-6"
                                                                            v-if="Ritem.QuestionType === 'Photo Capture'">
                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="Ritem.IsUpload" @change="">
                                                                                <v-radio color="success"
                                                                                    label="Photo Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Photo Upload" hide-details>
                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <!-- Photo Capture-->
                                                                            <div v-show="Ritem.IsUpload === 0">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-btn
                                                                                            @click="Ritem.startCamera = true, Ritem.camOn = true, onStart()"
                                                                                            color="primary" depressed
                                                                                            class="mr-4">Start</v-btn>
                                                                                        <v-btn class="mr-4"
                                                                                            color="secondary" depressed
                                                                                            @click="onCapture(Ritem, Ritem.QuestionID, false, 1), Ritem.startCamera = false, Ritem.camOn = false"
                                                                                            v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                                        <v-btn
                                                                                            @click="Ritem.startCamera = false, Ritem.camOn = false, onStop()"
                                                                                            v-bind:disabled="!item.camOn"
                                                                                            depressed
                                                                                            class="mr-4">Stop</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row>
                                                                                    <v-col md="8" v-if="Ritem.startCamera">
                                                                                        <vue-web-cam ref="webcam"
                                                                                            :device-id="deviceId"
                                                                                            width="100%" height="200px"
                                                                                            @started="onStarted"
                                                                                            @stopped="onStopped"
                                                                                            @error="onError"
                                                                                            @cameras="onCameras"
                                                                                            @camera-change="onCameraChange" />
                                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                    </v-col>

                                                                                    <v-col md="4"
                                                                                        v-if="Ritem.img != null && Ritem.img != ''">
                                                                                        <figure class="figure">
                                                                                            <img :src="Ritem.img"
                                                                                                class="center-block my-2"
                                                                                                max-width="100%"
                                                                                                height="200"
                                                                                                aspect-ratio="1.7" />
                                                                                        </figure>
                                                                                    </v-col>

                                                                                </v-row>
                                                                            </div>
                                                                            <!-- Photo Upload-->
                                                                            <div v-show="Ritem.IsUpload === 1">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-file-input label="Upload Image"
                                                                                            outlined dense accept="image/*"
                                                                                            v-model="Ritem.AnsBlob"
                                                                                            @change="SaveFileRepeat(Ritem, 'Photo', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>
                                                                                    </v-col>
                                                                                    <v-col md="4"
                                                                                        v-if="Ritem.uploadedImg != null">
                                                                                        <v-img :src=Ritem.uploadedImg
                                                                                            width="250" height="180"
                                                                                            aspect-ratio="1.7"
                                                                                            class="center-block my-2"></v-img>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                        </div>

                                                                        <!--Signature Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="Ritem.QuestionType === 'Signature Capture'"
                                                                            v-model="Ritem.Answer">
                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="Ritem.IsUpload" @change="">
                                                                                <v-radio color="success"
                                                                                    label="Signature Capture"
                                                                                    hide-details></v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Signature Upload"
                                                                                    hide-details></v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload === 0">
                                                                                <v-card outlined>
                                                                                    <VueSignaturePad width="100%"
                                                                                        height="300px" ref="signaturePad" />
                                                                                </v-card>
                                                                                <div>
                                                                                    <v-btn color="secondary" depressed
                                                                                        class="mt-2 mr-2"
                                                                                        :disabled=Ritem.signaturebutton
                                                                                        @click="SaveSignatureRepeatOrSkip(Ritem, false, BenefStepper, Ritem.RQstInd), Ritem.signaturebutton = true">Save</v-btn>
                                                                                    <v-btn default depressed
                                                                                        class="mt-2 mr-2"
                                                                                        @click="clearSign(Ritem, false, Ritem.QuestionID), Ritem.signaturebutton = false">Clear</v-btn>
                                                                                </div>
                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload === 1">
                                                                                <v-file-input label="Upload Signature"
                                                                                    outlined dense accept="image/*"
                                                                                    v-model="Ritem.AnsBlob"
                                                                                    @change="SaveFileRepeat(Ritem, 'Signature', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>
                                                                            </div>
                                                                            <v-card
                                                                                v-if="Ritem.uploadedSign != null && Ritem.uploadedSign != ''">
                                                                                <v-img :src=Ritem.uploadedSign
                                                                                    max-width="50%" height="150"
                                                                                    class="my-4" aspect-ratio="1.7"></v-img>
                                                                            </v-card>
                                                                        </div>

                                                                        <!--Audio Capture-->
                                                                        <div class="mb-6"
                                                                            v-if="Ritem.QuestionType === 'Audio Capture'">
                                                                            <v-radio-group class="my-0 py-0" row
                                                                                v-model="Ritem.IsUpload" @change="">
                                                                                <v-radio color="success"
                                                                                    label="Audio Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success"
                                                                                    label="Audio Upload" hide-details>

                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload === 0">
                                                                                <audio-recorder upload-url="YOUR_API_URL"
                                                                                    id="recAudioOne" :play="playSound"
                                                                                    :attempts="1" :time="1"
                                                                                    :headers="headers"
                                                                                    :before-recording="callback"
                                                                                    :pause-recording="callback"
                                                                                    :after-recording="SaveAudio"
                                                                                    :before-upload="callback"
                                                                                    :successful-upload="callback"
                                                                                    :failed-upload="callback"
                                                                                    v-model="Ritem.Answer"
                                                                                    @click.native="AnsBlobQID = Ritem.QuestionID, FindRecordedFileItemSkipRepeat(Ritem)" />

                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload === 1">
                                                                                <v-file-input label="Upload Audio" outlined
                                                                                    dense accept="audio/*"
                                                                                    v-model="Ritem.AnsBlob"
                                                                                    @change="AnsBlobQID = Ritem.QuestionID, SaveFileRepeat(Ritem, 'Audio', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>

                                                                            </div>
                                                                        </div>
                                                                        <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                        <div v-if="Ritem.isOtherOptionEnabled == true">
                                                                            <input type="text" solo
                                                                                class="custom-text-field elevation-0"
                                                                                label="Type your answer"
                                                                                v-model.lazy="Ritem.OtherOptionAnswer" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </v-col>
                                                    </v-col>
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white;">
                                                <Default-Button-Outlined class="btn-122x36 ma-2" title="Save & Exit"
                                                    @click.native="fnSaveAndExit()"></Default-Button-Outlined>
                                                <Secondary-Button class="btn-122x36 mr-2" title="Next"
                                                    @click.native="fnProccedToUpdate(), scrollToNavbarTop()"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- Survey Question Update by Import-->
                                    <v-stepper-content step="7" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col cols="12">
                                                <v-col cols="12" class="v-scrolling-div px-4 myTasksInnerCardHeight">
                                                    <v-row class="d-flex justify-center">
                                                        <v-col cols="12" md="12" class="mt-6" align="center">
                                                            <!-- Drag and Drop Section-->
                                                            <div class="text-center box_outline" @dragover.prevent
                                                                @drop.prevent v-if="lstImportSurvey.length == 0"
                                                                @click="handleFileImport">
                                                                <div @drop="dragFile_Survey">
                                                                    <div><v-icon color="#535353" size="70">mdi-file</v-icon>
                                                                    </div>
                                                                    <div v-if="SurveyFile.length == 0">
                                                                        <a href="#">Drag & Drop The Filled Out
                                                                            Form</a><br />
                                                                        <a href="#">Upload Only Xls File (Not Above 15mb Of
                                                                            Size)</a>
                                                                    </div>
                                                                    <div v-if="SurveyFile.length" class="mt-4">
                                                                        <div v-for="(file, ind) in SurveyFile" :key="ind">
                                                                            <v-chip close="" label="" color="white">
                                                                                <v-icon size="20" color="success"
                                                                                    height="100">mdi-file-delimited-outline</v-icon>
                                                                                {{ file.name }}
                                                                            </v-chip>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- Show vue-excel-editor -->
                                                            <div no-gutters class="uploadBoxHeight pa-0"
                                                                v-if="lstImportSurvey.length > 0">
                                                                <v-col cols="12" xs="12" sm="12" md="12"
                                                                    class="text-center pa-0">
                                                                    <h4 class="heading-3">We are running a format check,
                                                                        Please verify the data</h4>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12"
                                                                    class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                    <form ref="form">
                                                                        <vue-excel-editor ref="grid" no-paging
                                                                            disable-panel-setting disable-panel-filter
                                                                            v-model="lstImportSurvey">
                                                                            <div
                                                                                v-for="Hearder in lstImportSurveyQstnHearderN">
                                                                                <vue-excel-column :field="Hearder.text"
                                                                                    :type="Hearder.type"
                                                                                    :mandatory="Hearder.mandatory"
                                                                                    :options="Hearder.options"
                                                                                    :validate="onBeforeNameChange"
                                                                                    width="80px" />
                                                                            </div>
                                                                        </vue-excel-editor>
                                                                    </form>
                                                                </v-col>
                                                            </div>
                                                            <div class="mt-4" align="center"
                                                                v-if="lstImportSurvey.length == 0">
                                                                <input ref="uploader" class="d-none" type="file" accept=""
                                                                    @change="onFileChanged">
                                                                <Secondary-Button class="btn-122x36 mr-2" title="Download"
                                                                    @click.native="DownloadSampleBeneficiaryLinkedSurveyFile()"></Secondary-Button>
                                                            </div>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                                <v-col cols="12" align="right" style="background-color:white" class="py-2"
                                                    v-if="lstImportSurvey.length > 0">
                                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Cancel"
                                                        @click.native="CancelImport()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2"
                                                        v-bind:disabled="IsvalidexcelData == false" title="Next"
                                                        @click.native="ImportSurvey()"></Secondary-Button>
                                                </v-col>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!--Final steps-->
                                    <v-stepper-content step="5" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col md="12">
                                                <v-col cols="12" class="v-scrolling-div myTasksInnerCardHeight">
                                                    <div id="scrollToTop">
                                                        <v-avatar size="20"
                                                            style="background-color:#3374B9;color:white">1</v-avatar>
                                                        <span class="subtitle-2 mx-1">Submission Summary</span>
                                                    </div>
                                                    <br />
                                                    <v-card class="hidden-sm-and-down">
                                                        <v-row no-gutters class="text-center">
                                                            <v-col cols="12"
                                                                class="d-flex flex-wrap justify-space-around pa-2">
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ lstSelectedBeneficiary.length }}</h3>
                                                                    <p>No. of Beneficiaries </p>
                                                                </div>
                                                                <div>
                                                                    <p style="font-size:50px"
                                                                        class="font-weight-light grey--text">|</p>
                                                                </div>
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ QstnsPerBeneficiary }}</h3>
                                                                    <p>No.of Questions/Submission</p>
                                                                </div>
                                                                <div>
                                                                    <p style="font-size:50px"
                                                                        class="font-weight-light grey--text">|</p>
                                                                </div>
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ CompletionPerc }}%</h3>
                                                                    <p>Completion Ratio</p>
                                                                </div>
                                                                <!--<div>
                                                                <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                                                            </div>
                                                            <div>
                                                                <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                                                                <p>Data collected / Beneficiary</p>
                                                            </div>-->
                                                            </v-col>
                                                        </v-row>
                                                    </v-card>
                                                    <v-card class="hidden-md-and-up">
                                                        <v-row no-gutters>
                                                            <v-col cols="12">
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">No. of
                                                                        Beneficiaries </v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            lstSelectedBeneficiary.length }}</v-col>
                                                                </v-row>
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">No. of
                                                                        Questions/Submission </v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            QstnsPerBeneficiary }}</v-col>
                                                                </v-row>
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">Completion
                                                                        Ratio</v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            CompletionPerc }}%</v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card>
                                                    <br />
                                                    <div>
                                                        <v-avatar size="20"
                                                            style="background-color:#3374B9;color:white">2</v-avatar>
                                                        <span class="subtitle-2 mx-1">Add Comments</span>
                                                    </div>
                                                    <br />
                                                    <!--In Case You Have Any Comments Please Specify Them Below.-->
                                                    <v-textarea rows="8" color="black" dense
                                                        label="Summarize Your Comments On The Survey With Few Lines." solo
                                                        class="elevation-0" type="text" v-model.lazy="vmUpdateComment">
                                                    </v-textarea>
                                                    <!--<v-row>
                                                    <v-col md="12">
                                                        <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" prepend-icon="mdi-paperclip" outlined="" dense="" v-model="UploadDocument"
                                                                        id="file" accept="*"></v-file-input>
                                                    </v-col>
                                                </v-row>-->
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white">
                                                <Secondary-Button class="btn-122x36 ma-2" title="Update"
                                                    @click.native="UpdateTask(), scrollToNavbarTop()"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!--if Already updated-->
                                    <v-stepper-content step="6" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col md="12">
                                                <v-col cols="12" class="myTasksInnerCardHeight">
                                                    <v-tabs>
                                                        <v-tab href='#tab-1'>Submission Summary</v-tab>
                                                        <v-tab href='#tab-2'>Activity</v-tab>
                                                        <!--Sybmission Summary-->
                                                        <v-tab-item value="tab-1" class=" mt-4"
                                                            style="background-color: #F5F6F8;">
                                                            <h4 class="text-center heading-3-dark">{{ CustomMessage }}</h4>
                                                            <br />
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Submission Summary</span>
                                                            </div>
                                                            <br />
                                                            <v-card class="hidden-sm-and-down">
                                                                <v-row no-gutters class="text-center">
                                                                    <v-col cols="12"
                                                                        class="d-flex flex-wrap justify-space-around pa-2">
                                                                        <div class="text-limit">
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ this.NoOfSelectedBen }}</h3>
                                                                            <p>No.of Beneficiaries </p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div class="text-limit">
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ QstnsPerBeneficiary }}</h3>
                                                                            <p>No.of Questions/Submission</p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div class="text-limit">
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ CompletionPerc }}%</h3>
                                                                            <p>Completion Ratio</p>
                                                                        </div>
                                                                        <!--<div>
                                                                        <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                                                                    </div>
                                                                    <div class="text-limit">
                                                                        <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                                                                        <p>Data collected / Beneficiary</p>
                                                                    </div>-->
                                                                    </v-col>

                                                                </v-row>
                                                            </v-card>
                                                            <v-card class="hidden-md-and-up">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12">
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Beneficiaries </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    this.NoOfSelectedBen }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Questions/Submission </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    QstnsPerBeneficiary }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">Completion
                                                                                Ratio</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    CompletionPerc }}%</v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <!--<br />
                                                        <div>
                                                            <v-avatar size="20" style="background-color:#3374B9;color:white">2</v-avatar>
                                                            <span class="subtitle-2 mx-1">Add Comments</span>
                                                        </div>
                                                        <br />
                                                        <v-textarea rows="8" dense label="Summarize Your Comments On The Survey With Few Lines." solo class="elevation-0" type="text">
                                                        </v-textarea>-->


                                                        </v-tab-item>
                                                        <!--Activity Log-->
                                                        <v-tab-item value="tab-2" class="mt-md-4 mt-1"
                                                            style="background-color: #F5F6F8;">
                                                            <br class="hidden-sm-and-down" />
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Activity</span>
                                                            </div>
                                                            <br class="hidden-sm-and-down" />
                                                            <v-card class="v-scrolling-div ActivityLogCardHeight">
                                                                <!--<v-list dense class="py-0 ma-0" two-line>
                                                            <template v-for="(item, index) in IndividualActivityLog">
                                                                <v-list-item>
                                                                    <v-list-item-icon>
                                                                        <v-avatar size="40" color="pink">-->
                                                                <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                <!--<span class="white--text">K</span>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>

                                                                        <v-list-item-content>
                                                                            <v-list-item-title class="text-limit">
                                                                                {{item.Name}} had {{item.Message}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-title class="text-limit" v-if="item.TaskUpdateComment.length>0">
                                                                                Comments : {{item.TaskUpdateComment}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-subtitle>
                                                                                <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                            </v-list-item-subtitle>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </template>
                                                            </v-list>-->
                                                                <!--new Activity layout for approver-->
                                                                <v-card elevation="2" class="ma-3"
                                                                    v-for="(item, index) in lstLogFrame">
                                                                    <v-card-title class="ma-0 pa-0">
                                                                    </v-card-title>
                                                                    <v-card-text>
                                                                        <v-row class="pa-2">
                                                                            <v-col class="pa-0 pt-2 ma-0 d-flex d-inline"
                                                                                style="color:#374151">
                                                                                <h5 class="subtitle-2">
                                                                                    <v-avatar size="25">
                                                                                        <v-img :src="LebelsIcon"
                                                                                            alt="lebels_icon"
                                                                                            size="15"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowLevelName }}
                                                                                </h5>
                                                                                <v-spacer></v-spacer>
                                                                                <small class="text-right"
                                                                                    style="color:#374151">{{
                                                                                        formatDateAndTime(item.SubmissionDate)
                                                                                    }}</small>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.WorkflowStageStatus == 'Approved'"
                                                                                    class="heading-3-dark font-weight-bolder"
                                                                                    style="color:#8EDC92">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#8EDC92">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Rejected'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#ED9A98">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#ED9A98">
                                                                                        <v-img :src="RejectedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Submitted'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="SubmittedIcon"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Assigned'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.DefaultText.length > 0">
                                                                                    <small color="#707070">{{
                                                                                        item.DefaultText }}</small>
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-0 px-2 ma-0 heading-4-dark">
                                                                                <v-avatar size="25"
                                                                                    :color="`${getcolor(item.SubmittedBy)}`">
                                                                                    <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                                    <img v-bind:src="item.ProfileImage"
                                                                                        alt="User DP"
                                                                                        v-if="item.ProfileImage.length > 0" />
                                                                                    <span class="white--text" v-else>{{
                                                                                        item.SubmittedBy == null ?
                                                                                        "N/A" :
                                                                                        item.SubmittedBy.substring(0,
                                                                                            1).toUpperCase() }}</span>
                                                                                </v-avatar>
                                                                                {{ item.SubmittedBy }}
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                </v-card>
                                                            </v-card>
                                                        </v-tab-item>
                                                    </v-tabs>
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white;z-index:3">
                                                <Secondary-Button class="btn-122x36 ma-2" title="Download"
                                                    @click.native="MyTasksDownloadSummary('beneficiarylinked', SurveyInnerTaskIndividualSyncDBID)"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                </v-stepper-items>
                            </v-stepper>
                        </v-col>
                    </v-row>
                </v-card>
            </v-dialog>
        </template>
        <!--Survey Non - Benificiary Linked dialog-->
        <template>
            <v-dialog v-model="NonBenLinkSurveyDlg" persistent :fullscreen="$vuetify.breakpoint.smAndDown">
                <v-card class="myTasksDialogCardHeight">
                    <v-card-title>
                        <v-btn v-show="NonBenefLinkedSurveySteps > 1" text small @click="fnBackspace()">
                            <v-icon color="#707070" size="20">mdi-keyboard-backspace</v-icon>
                        </v-btn>
                        <span class="limited-width-CardTitle text-truncate page-head hidden-sm-and-down">{{
                            SelectedSurveyName }}</span>
                        <span class="limited-width-CardTitle text-truncate page-head hidden-md-and-up"
                            v-if="NonBenefLinkedSurveySteps != 1">{{ SelectedSurveyName }}</span>
                        <v-spacer></v-spacer>
                        <v-btn text v-if="NonBenefLinkedSurveySteps == 1" @click.stop="fnResetQuestionsAns()"
                            class="hidden-md-and-up">
                            <v-icon color="black" size="30">mdi-autorenew</v-icon>
                        </v-btn>
                        <v-btn text v-if="NonBenefLinkedSurveySteps == 1" @click.stop="GetBenefSurveyQstnsForImport()"
                            class="hidden-md-and-up">
                            <v-icon color="black" size="30">mdi-download-outline</v-icon>
                        </v-btn>
                        <v-btn text @click.stop="fnClearAnyTimeList(), fnClearData()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text class="px-0 py-0 ma-0 hidden-sm-and-down">
                        <v-list dense class="py-0 ma-0" style="background: rgba(61, 126, 254, 0.1);">
                            <template>
                                <v-list-item class="py-0">
                                    <v-list-item-icon>
                                        <v-avatar size="40" :color="`${getcolor(DataCollectorInfo.DatacollectorName)}`">
                                            <!--<img v-bind:src="userimage + DataCollectorInfo.ProfileImage" alt="User DP" v-if="DataCollectorInfo.ProfileImage.length >0" />-->
                                            <span class="white--text">{{ DataCollectorInfo.DatacollectorName == null ? "N/A"
                                                : DataCollectorInfo.DatacollectorName.substring(0, 1).toUpperCase()
                                            }}</span>
                                        </v-avatar>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        <v-list-item-title class="py-0 pb-0">
                                            <v-row class="d-lg-inline-flex py-0 pb-0">
                                                <v-col class="px-0 pl-3 py-0">
                                                    <h4 class="font-weight-large   py-0 pb-0 px-0">
                                                        {{ DataCollectorInfo.DatacollectorName }}</h4>
                                                </v-col>
                                                <v-col class="ml-3 px-0 py-0">
                                                    <h4 class="font-weight-regular  py-0 pb-0">
                                                        {{ DataCollectorInfo.AssignedTime }}</h4>
                                                </v-col>
                                            </v-row>
                                        </v-list-item-title>
                                        <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                            <h4 class="font-weight-regular py-0 pb-0 px-0">Survey Data has been assigned.
                                            </h4>
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-spacer></v-spacer>
                                    <v-btn text @click.stop="">
                                        <v-icon color="primary" size="30">mdi-chevron-up</v-icon>
                                    </v-btn>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-card-text>
                    <v-divider class="hidden-sm-and-down"></v-divider>
                    <v-row no-gutters justify="center" style="background-color: #F5F6F8;">
                        <v-col cols="12" md="12" sm="12" xs="12">
                            <v-stepper v-model="NonBenefLinkedSurveySteps" class="elevation-0">
                                <v-stepper-items>
                                    <!-- Survey Question Update Manually -->
                                    <v-stepper-content step="1" class="pa-0 ma-0">
                                        <v-row style="background-color: #F5F6F8;" no-gutters>
                                            <v-col cols="12" class="pa-0 ma-0">
                                                <v-col cols="12" class="pa-0 ma-0 hidden-sm-and-down">
                                                    <!-- Import & Reset Buttons-->
                                                    <v-col cols="3" align="right" class="d-inline-flex pa-0 my-2 mx-md-4">
                                                        <v-btn elevation="1" tile style="border-radius:3px;" color="white"
                                                            @click="fnResetQuestionsAns()">Reset</v-btn>
                                                        <v-btn elevation="1" tile style="border-radius:3px;" color="white"
                                                            class="mx-1"
                                                            @click.native="GetBenefSurveyQstnsForImport()"><v-icon
                                                                left>mdi-download</v-icon>Import</v-btn>
                                                    </v-col>
                                                </v-col>
                                                <!--<v-col cols="12" class="pa-0 ma-0">-->
                                                <!-- Progresss-->
                                                <!--<v-col md="8" xs="10" class="mx-md-4">fnAddAnyTimeNewSubmission
                                                <h4 class="text-left heading-3-dark">{{CompletionPerc}}% To Complete Survey</h4><br />
                                                <v-progress-linear v-model="CompletionPerc" :buffer-value="bufferValue" color="primary">
                                                </v-progress-linear>
                                            </v-col>-->
                                                <!--</v-col>-->
                                                <v-col cols="12"
                                                    class="v-scrolling-div pa-0 ma-0 myTasksNonBenQuestionsFormHeight">
                                                    <!-- Survey Sections and Questions-->
                                                    <v-col md="8" xs="10" class="mx-md-4">
                                                        <div id="scrollToTop"></div>
                                                        <div class="pa-0 ma-0" v-for="(SecItem, Sindex) in lstSectionsQstns"
                                                            v-if="SecItem.IslogicSection == 0">
                                                            <div class="ma-0 pa-0">
                                                                <v-avatar size="20" class="mb-1 ma-0 pa-0"
                                                                    style="background-color:#3374B9;color:white">{{ Sindex +
                                                                        1 }}</v-avatar>
                                                                <span class="mx-1 page-head-dark"
                                                                    style="font-weight:bold">{{ SecItem.SectionName
                                                                    }}</span>
                                                            </div>
                                                            <div class="ma-0 pa-0">
                                                                <span class="mx-1">{{ SecItem.SectionDescription }}</span>
                                                            </div>
                                                            <br />
                                                            <div v-for="(item, QIndex) in SecItem.Questions">

                                                                <label class="text-left subtitle-2  mb-2"><b>{{
                                                                    item.QuestionName }}</b></label>

                                                                <sub v-if="item.IsMandatory == true"
                                                                    style="color:red;font-size:large"> *</sub>

                                                                <sub style="color:blue;font-size:smaller"
                                                                    v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{
                                                                        item.QuestionHint }}</sub>

                                                                <sub style="color:red;font-size:smaller"
                                                                    v-if="(item.IsMandatory == true && item.Answer === '') || item.IsValidationError">{{
                                                                        item.ValidationErrorTest }}</sub>

                                                                <!-- Single Line Text with validation and Condition -->
                                                                <div v-if="item.QuestionType == 'Single Line Text'">
                                                                    <input type="text"
                                                                        v-if="item.QuestionType == 'Single Line Text'" solo
                                                                        class="custom-text-field elevation-0"
                                                                        label="Type your answer" v-model.lazy="item.Answer"
                                                                        @focus="CalculateText(item, item.QuestionID, BenefStepper)" />
                                                                </div>

                                                                <!--Multiline Answer-->
                                                                <textarea type="text" class="custom-text-field elevation-0"
                                                                    v-if="item.QuestionType === 'Multi Line Text'" solo
                                                                    label="Type your answer" v-model.lazy="item.Answer" />

                                                                <!--Numeric-->

                                                                <div v-if="item.QuestionType === 'Numeric'" class="mt-0">

                                                                    <v-text-field type="number"
                                                                        v-if="item.QuestionType === 'Numeric'" full-width
                                                                        outlined dense solo label="Type your answer"
                                                                        v-model.lazy="item.Answer"
                                                                        onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                        :value="item.DefaultValue" :min="item.minVal"
                                                                        :max="item.maxVal"
                                                                        :readonly="item.IsFormulaQuestion == true"
                                                                        @change="onFocusCheckQstnValidation(item), AddSectionLogic(item, item.QuestionID, BenefStepper)"
                                                                        @focus="onFocusCheckQstnValidation(item), Calculate(item, item.QuestionID, BenefStepper)"
                                                                        :rules="item.qRules">
                                                                    </v-text-field>

                                                                    <sub class="mr-2 ml-2"
                                                                        style="color:green;font-size:large">{{ item.UOM
                                                                        }}</sub>

                                                                </div>


                                                                <!--Single Choice Answer-->
                                                                <div v-if="item.QuestionType === 'Single Choice'"
                                                                    class="mt-0">
                                                                    <v-radio-group class="my-0 py-0"
                                                                        v-model.lazy="item.Answer"
                                                                        @change="AddSectionLogic(item, item.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(item, item.Answer.toUpperCase())">
                                                                        <v-radio color="success" hide-details
                                                                            v-for="(optItem, ind) in item.QuestionOptions"
                                                                            :key="ind" :label="optItem.OptionValue"
                                                                            :value="optItem.OptionValue"></v-radio>
                                                                    </v-radio-group>
                                                                </div>

                                                                <!--Multiple Choice -->
                                                                <div v-if="item.QuestionType === 'Multi-Choice'"
                                                                    class="mt-0 mb-6">

                                                                    <div class="my-0 py-0"
                                                                        v-if="item.QuestionOptions.length > 0">

                                                                        <v-checkbox color="success" hide-details
                                                                            input-value="true" :label="optItem.OptionValue"
                                                                            v-for="(optItem, ind) in item.QuestionOptions"
                                                                            :key="ind" v-model="optItem.IsOptSelected"
                                                                            @change="GetMultiChoice(optItem, optItem.IsOptSelected, optItem.OptionValue, item.QuestionID, BenefStepper), AddSectionLogicForMultiChoice(optItem, item.QuestionID, BenefStepper, item.QuestionOptions), ShowOtherOption(item, optItem.OptionValue.toUpperCase(), optItem.IsOptSelected)"></v-checkbox>
                                                                    </div>
                                                                </div>

                                                                <!--Dropdown -->
                                                                <div v-if="item.QuestionType === 'Dropdown'"
                                                                    class="mt-0 mb-6">
                                                                    <div v-if="item.QuestionOptions.length > 0">
                                                                        <v-select dense :items="item.QuestionOptions"
                                                                            :menu-props="{ maxHeight: '300' }" filled
                                                                            item-text="OptionValue" label="Select" outlined
                                                                            hide-details v-model.lazy="item.Answer"
                                                                            @change="AddSectionLogic(item, item.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(item, item.Answer.toUpperCase())">
                                                                        </v-select>
                                                                    </div>
                                                                </div>

                                                                <!--Date Picker-->
                                                                <div class="mb-6"
                                                                    v-if="item.QuestionType === 'Date Picker'">
                                                                    <v-menu v-model="item.DateMenu1"
                                                                        :close-on-content-click="false" :nudge-right="40"
                                                                        transition="scale-transition" offset-y
                                                                        min-width="290px">
                                                                        <template v-slot:activator="{ on }">
                                                                            <v-text-field v-model.lazy="item.Answer"
                                                                                prepend-inner-icon="mdi-calendar" readonly
                                                                                @click:prepend-inner="item.DateMenu1 = true"
                                                                                v-on="on" hide-details></v-text-field>
                                                                        </template>
                                                                        <v-date-picker v-model.lazy="item.Answer" no-title
                                                                            @input="item.DateMenu1 = false"
                                                                            format="DD-MM-YYYY" @change=""></v-date-picker>
                                                                    </v-menu>
                                                                </div>

                                                                <!--Cascading Dropdown-->
                                                                <div v-if="item.QuestionType === 'Cascading Dropdown'"
                                                                    class="mt-0">
                                                                    <v-row class="my-0 py-0">
                                                                        <v-col class="my-0 py-0">
                                                                            <v-select class="my-0 py-0" dense
                                                                                :menu-props="{ maxHeight: '300' }" filled
                                                                                v-model="optItem[ind]"
                                                                                :label="` ${optItem.OptionValue}`"
                                                                                outlined=""
                                                                                v-for="(optItem, ind) in item.CascadingQuestionOptions"
                                                                                :key="ind" :items="optItem.CascadingList"
                                                                                item-text="Name" item-value="ID"
                                                                                @change="FilterCascadingItems(item, optItem, optItem[ind], false, BenefStepper, -1)">
                                                                            </v-select>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>

                                                                <!--GPS Capture-->
                                                                <div class="mb-6" v-if="item.QuestionType === 'GPS Capture'"
                                                                    v-model.lazy="item.Answer">
                                                                    <span class="d-none">{{ GetGPSCoordinates(item, false)
                                                                    }}</span>
                                                                    <label class="text-left heading-1 mt-3">{{ item.Answer
                                                                    }}</label>
                                                                    <GmapMap :center="Coordinates" :zoom="13"
                                                                        style="width: 100%; height: 200px"
                                                                        @click.native="GetGPSCoordinates(item, false)">
                                                                        <GmapMarker :position="Coordinates"
                                                                            :clickable="true" :draggable="true"
                                                                            @dragend="updateCoordinates($event.latLng, item)" />
                                                                    </GmapMap>
                                                                </div>

                                                                <!--File Upload-->
                                                                <div class="mb-6"
                                                                    v-if="item.QuestionType === 'File Upload'">
                                                                    <v-row>
                                                                        <v-col md="12">
                                                                            <v-file-input outlined dense accept="*"
                                                                                v-model="item.AnsBlob"
                                                                                @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'File', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                        </v-col>
                                                                        <v-col md="4" v-if="item.uploadedFile != null">
                                                                            <label class="text-left heading-1 mt-3">{{
                                                                                item.Answer }}</label>
                                                                            <v-img :src=item.uploadedFile width="250"
                                                                                height="180" aspect-ratio="1.7"
                                                                                class="center-block my-2"></v-img>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>

                                                                <!-- Likert Scale-->
                                                                <div v-if="item.QuestionType === 'Likert Scale'"
                                                                    class="mt-0 my-0 py-0">
                                                                    <v-radio-group class="my-0 py-0"
                                                                        v-model.lazy="item.Answer">
                                                                        <v-radio color="success" class="my-0 py-0"
                                                                            hide-details
                                                                            v-for="(optItem, ind) in item.QuestionOptions"
                                                                            :key="ind" :label="optItem.OptionValue"
                                                                            :value="optItem.OptionValue"></v-radio>
                                                                    </v-radio-group>
                                                                </div>

                                                                <!-- Photo Caputure-->
                                                                <div class="mb-6"
                                                                    v-if="item.QuestionType === 'Photo Capture'">
                                                                    <v-radio-group class="my-0 py-0" row
                                                                        v-model="audUpload[QIndex]"
                                                                        @change="item.IsUpload = audUpload[QIndex]">
                                                                        <v-radio color="success" label="Photo Capture"
                                                                            hide-details>
                                                                        </v-radio>
                                                                        <v-radio color="success" label="Photo Upload"
                                                                            hide-details>
                                                                        </v-radio>
                                                                    </v-radio-group>
                                                                    <!-- Photo Capture-->
                                                                    <div v-show="item.IsUpload === 0">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <v-btn
                                                                                    @click="item.startCamera = true, item.camOn = true, onStart()"
                                                                                    color="primary" depressed
                                                                                    class="mr-4">Start</v-btn>
                                                                                <v-btn class="mr-4" color="secondary"
                                                                                    depressed
                                                                                    @click="onCapture(item, item.QuestionID, false, 1), item.startCamera = false, item.camOn = false"
                                                                                    v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                                <v-btn
                                                                                    @click="item.startCamera = false, item.camOn = false, onStop()"
                                                                                    v-bind:disabled="!item.camOn" depressed
                                                                                    class="mr-4">Stop</v-btn>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col md="8" v-if="item.startCamera">
                                                                                <vue-web-cam ref="webcam"
                                                                                    :device-id="deviceId" width="100%"
                                                                                    height="200px" @started="onStarted"
                                                                                    @stopped="onStopped" @error="onError"
                                                                                    @cameras="onCameras"
                                                                                    @camera-change="onCameraChange" />
                                                                                <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                            </v-col>

                                                                            <v-col md="4"
                                                                                v-if="item.img != null && item.img != ''">
                                                                                <figure class="figure">
                                                                                    <img :src="item.img"
                                                                                        class="center-block my-2"
                                                                                        max-width="100%" height="200"
                                                                                        aspect-ratio="1.7" />
                                                                                </figure>
                                                                            </v-col>

                                                                        </v-row>
                                                                    </div>
                                                                    <!-- Photo Upload-->
                                                                    <div v-show="item.IsUpload === 1">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <v-file-input label="Upload Image" outlined
                                                                                    dense accept="image/*"
                                                                                    v-model="item.AnsBlob"
                                                                                    @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Photo', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                            </v-col>
                                                                            <v-col md="4" v-if="item.uploadedImg != null">
                                                                                <v-img :src=item.uploadedImg width="250"
                                                                                    height="180" aspect-ratio="1.7"
                                                                                    class="center-block my-2"></v-img>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>
                                                                </div>

                                                                <!--Signature Capture-->
                                                                <div class="mb-6"
                                                                    v-if="item.QuestionType === 'Signature Capture'"
                                                                    v-model="item.Answer">
                                                                    <v-radio-group class="my-0 py-0" row
                                                                        v-model="audUpload[QIndex]"
                                                                        @change="item.IsUpload = audUpload[QIndex]">
                                                                        <v-radio color="success" label="Signature Capture"
                                                                            hide-details></v-radio>
                                                                        <v-radio color="success" label="Signature Upload"
                                                                            hide-details></v-radio>
                                                                    </v-radio-group>
                                                                    <div v-show="item.IsUpload === 0">
                                                                        <v-card outlined>
                                                                            <VueSignaturePad
                                                                                :options="{ onBegin: () => { $refs.signaturePad[0].resizeCanvas() } }"
                                                                                width="100%" height="300px"
                                                                                ref="signaturePad" />
                                                                        </v-card>
                                                                        <div>
                                                                            <v-btn color="secondary" depressed
                                                                                class="mt-2 mr-2"
                                                                                :disabled=item.signaturebutton
                                                                                @click="saveSign(item.QuestionID, false, BenefStepper, item), item.signaturebutton = true">Save</v-btn>
                                                                            <v-btn default depressed class="mt-2 mr-2"
                                                                                @click="clearSign(item, false, item.QuestionID), item.signaturebutton = false">Clear</v-btn>
                                                                        </div>
                                                                    </div>
                                                                    <div v-show="item.IsUpload === 1">
                                                                        <v-file-input label="Upload Signature" outlined
                                                                            dense accept="image/*" v-model="item.AnsBlob"
                                                                            @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Signature', item.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                    </div>
                                                                    <v-card
                                                                        v-if="item.uploadedSign != null && item.uploadedSign != ''">
                                                                        <v-img :src=item.uploadedSign max-width="50%"
                                                                            height="150" class="my-4"
                                                                            aspect-ratio="1.7"></v-img>
                                                                    </v-card>
                                                                </div>

                                                                <!--Audio Capture-->
                                                                <div class="mb-6"
                                                                    v-if="item.QuestionType === 'Audio Capture'">
                                                                    <v-radio-group class="my-0 py-0" row
                                                                        v-model="audUpload[QIndex]"
                                                                        @change="item.IsUpload = audUpload[QIndex]">
                                                                        <v-radio color="success" label="Audio Capture"
                                                                            hide-details>
                                                                        </v-radio>
                                                                        <v-radio color="success" label="Audio Upload"
                                                                            hide-details>

                                                                        </v-radio>
                                                                    </v-radio-group>
                                                                    <div v-show="item.IsUpload === 0">
                                                                        <audio-recorder upload-url="YOUR_API_URL"
                                                                            id="recAudioOne" :play="playSound" :attempts="1"
                                                                            :time="1" :headers="headers"
                                                                            :before-recording="callback"
                                                                            :pause-recording="callback"
                                                                            :after-recording="SaveAudio"
                                                                            :before-upload="callback"
                                                                            :successful-upload="callback"
                                                                            :failed-upload="callback" v-model="item.Answer"
                                                                            @click.native="AnsBlobQID = item.QuestionID, FindRecordedFileItem(item)" />

                                                                    </div>
                                                                    <div v-show="item.IsUpload === 1">
                                                                        <v-file-input label="Upload Audio" outlined dense
                                                                            accept="audio/*" v-model="item.AnsBlob"
                                                                            @change="AnsBlobQID = item.QuestionID, SaveFile(item, 'Audio', item.AnsBlob, false, true, BenefStepper)"></v-file-input>

                                                                    </div>
                                                                </div>
                                                                <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                <div v-if="item.isOtherOptionEnabled == true">
                                                                    <input type="text" solo
                                                                        class="custom-text-field elevation-0"
                                                                        label="Type your answer"
                                                                        v-model.lazy="item.OtherOptionAnswer" />
                                                                </div>

                                                                <!-- For Showing repeat or skip section Question Details-->
                                                                <div v-if="RepeatOrSkipSection.length > 0 && Ritem.ParentQuestionID == item.QuestionID && BenefStepper == Ritem.benefIndex"
                                                                    v-for="(Ritem, RIndex) in RepeatOrSkipSection"
                                                                    :key="RIndex">

                                                                    <label class="text-left heading-3 mb-2"><b>{{
                                                                        Ritem.QuestionName }}</b></label>

                                                                    <sub v-if="Ritem.IsMandatory == true"
                                                                        style="color:red;font-size:large"> *</sub>

                                                                    <sub style="color:blue;font-size:smaller"
                                                                        v-if="Ritem.QuestionHint != null && Ritem.QuestionHint != 'null' && Ritem.QuestionHint.length > 0">{{
                                                                            Ritem.QuestionHint }}</sub>

                                                                    <sub style="color:red;font-size:smaller"
                                                                        v-if="(Ritem.IsMandatory == true && Ritem.Answer === '') || Ritem.IsValidationError">{{
                                                                            Ritem.ValidationErrorTest }}</sub>

                                                                    <!-- Single Line Text with validation and Condition -->
                                                                    <div v-if="Ritem.QuestionType == 'Single Line Text'">
                                                                        <input type="text"
                                                                            v-if="Ritem.QuestionType == 'Single Line Text'"
                                                                            solo class="custom-text-field elevation-0"
                                                                            label="Type your answer"
                                                                            v-model.lazy="Ritem.Answer"
                                                                            @focus="CalculateText(Ritem, Ritem.QuestionID, BenefStepper)" />
                                                                    </div>

                                                                    <!--Multiline Answer-->
                                                                    <input type="text" class="custom-text-field elevation-0"
                                                                        v-if="Ritem.QuestionType === 'Multi Line Text'" solo
                                                                        label="Type your answer"
                                                                        v-model.lazy="Ritem.Answer" />

                                                                    <!--Numeric-->

                                                                    <div v-if="Ritem.QuestionType === 'Numeric'"
                                                                        class="mt-0">

                                                                        <input type="number"
                                                                            v-if="Ritem.QuestionType === 'Numeric'" solo
                                                                            class="custom-text-field elevation-0"
                                                                            label="Type your answer"
                                                                            v-model.lazy="Ritem.Answer"
                                                                            :readonly="Ritem.IsFormulaQuestion == true"
                                                                            @focus="Calculate(Ritem, Ritem.QuestionID, BenefStepper)" />

                                                                        <sub class="mr-2 ml-2"
                                                                            style="color:green;font-size:large">{{ Ritem.UOM
                                                                            }}</sub>

                                                                    </div>


                                                                    <!--Single Choice Answer-->
                                                                    <div v-if="Ritem.QuestionType === 'Single Choice'"
                                                                        class="mt-0">
                                                                        <v-radio-group class="my-0 py-0"
                                                                            v-model.lazy="Ritem.Answer"
                                                                            @change="AddSectionLogic(Ritem, Ritem.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(Ritem, Ritem.Answer.toUpperCase())">
                                                                            <v-radio color="success" hide-details
                                                                                v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                :key="ind" :label="optItem.OptionValue"
                                                                                :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!--Multiple Choice -->
                                                                    <div v-if="Ritem.QuestionType === 'Multi-Choice'"
                                                                        class="mt-0 mb-6">

                                                                        <div class="my-0 py-0"
                                                                            v-if="Ritem.QuestionOptions.length > 0">

                                                                            <v-checkbox color="success" hide-details
                                                                                input-value="true"
                                                                                :label="optItem.OptionValue"
                                                                                v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                :key="ind" v-model="optItem.IsOptSelected"
                                                                                @change="GetMultiChoiceRepeat(optItem, optItem.IsOptSelected, optItem.OptionValue, Ritem.RQstInd, Ritem.QuestionID, BenefStepper), ShowOtherOption(Ritem, optItem.OptionValue.toUpperCase(), optItem.IsOptSelected)"></v-checkbox>
                                                                        </div>
                                                                    </div>

                                                                    <!--Dropdown -->
                                                                    <div v-if="Ritem.QuestionType === 'Dropdown'"
                                                                        class="mt-0 mb-6">
                                                                        <div v-if="Ritem.QuestionOptions.length > 0">
                                                                            <v-select dense :items="Ritem.QuestionOptions"
                                                                                :menu-props="{ maxHeight: '300' }" filled
                                                                                item-text="OptionValue" label="Select"
                                                                                outlined hide-details
                                                                                v-model.lazy="Ritem.Answer"
                                                                                @change="AddSectionLogic(Ritem, Ritem.QuestionID, BenefStepper), ShowOtherOptionSingleChoice(Ritem, Ritem.Answer.toUpperCase())">
                                                                            </v-select>
                                                                        </div>
                                                                    </div>

                                                                    <!--Date Picker-->
                                                                    <div class="mb-6"
                                                                        v-if="Ritem.QuestionType === 'Date Picker'">
                                                                        <v-menu v-model="Ritem.DateMenu"
                                                                            :close-on-content-click="false"
                                                                            :nudge-right="40" transition="scale-transition"
                                                                            offset-y min-width="290px">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-text-field v-model.lazy="Ritem.Answer"
                                                                                    prepend-inner-icon="mdi-calendar"
                                                                                    readonly
                                                                                    @click:prepend-inner="Ritem.DateMenu = true"
                                                                                    v-on="on" hide-details></v-text-field>
                                                                            </template>
                                                                            <v-date-picker v-model.lazy="Ritem.Answer"
                                                                                no-title @input="Ritem.DateMenu = false"
                                                                                format="DD-MM-YYYY"
                                                                                @change=""></v-date-picker>
                                                                        </v-menu>
                                                                    </div>

                                                                    <!--Cascading Dropdown-->
                                                                    <div v-if="Ritem.QuestionType === 'Cascading Dropdown'"
                                                                        class="mt-0">
                                                                        <v-row class="my-0 py-0">
                                                                            <v-col class="my-0 py-0">
                                                                                <v-select class="my-0 py-0" dense
                                                                                    :menu-props="{ maxHeight: '300' }"
                                                                                    filled v-model="optItem[ind]"
                                                                                    :label="` ${optItem.OptionValue}`"
                                                                                    outlined=""
                                                                                    v-for="(optItem, ind) in Ritem.CascadingQuestionOptions"
                                                                                    :key="ind"
                                                                                    :items="optItem.CascadingList"
                                                                                    item-text="Name" item-value="ID"
                                                                                    @change="FilterCascadingItems(Ritem, optItem, optItem[ind], false, BenefStepper, -1)">
                                                                                </v-select>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!--GPS Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="Ritem.QuestionType === 'GPS Capture'"
                                                                        v-model.lazy="Ritem.Answer">
                                                                        <label class="text-left heading-1 mt-3">{{
                                                                            Ritem.Answer }}</label>
                                                                        <GmapMap :center="Coordinates" :zoom="13"
                                                                            style="width: 100%; height: 200px"
                                                                            @click.native="">
                                                                            <GmapMarker :position="Coordinates"
                                                                                :clickable="true" :draggable="true"
                                                                                @dragend="" />
                                                                        </GmapMap>
                                                                    </div>

                                                                    <!--File Upload-->
                                                                    <div class="d-inline-flex pa-0 ma-0" style="width:100%"
                                                                        v-if="Ritem.QuestionType === 'File Upload'">

                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <!--<v-btn left elevation="1" tile style="border-radius:3px;" color="white" class="mx-1">UpLoad</v-btn>-->
                                                                                <v-file-input truncate-length="15"
                                                                                    accept="*" v-model.lazy="Ritem.AnsBlob"
                                                                                    @change="SaveFileRepeat(Ritem, 'File', Ritem.AnsBlob, false, true, BenefStepper)"></v-file-input>
                                                                            </v-col>

                                                                            <v-col md="4" v-if="Ritem.uploadedFile != null">
                                                                                <v-img :src=Ritem.uploadedFile width="250"
                                                                                    height="180" aspect-ratio="1.7"
                                                                                    class="center-block my-2"></v-img>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!-- Likert Scale-->
                                                                    <div v-if="Ritem.QuestionType === 'Likert Scale'"
                                                                        class="mt-0 my-0 py-0">
                                                                        <v-radio-group class="my-0 py-0"
                                                                            v-model.lazy="Ritem.Answer">
                                                                            <v-radio color="success" class="my-0 py-0"
                                                                                hide-details
                                                                                v-for="(optItem, ind) in Ritem.QuestionOptions"
                                                                                :key="ind" :label="optItem.OptionValue"
                                                                                :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!-- Photo Caputure Or Upload-->
                                                                    <div class="mb-6"
                                                                        v-if="Ritem.QuestionType === 'Photo Capture'">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="Ritem.IsUpload" @change="">
                                                                            <v-radio color="success" label="Photo Capture"
                                                                                hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Photo Upload"
                                                                                hide-details>
                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <!-- Photo Capture-->
                                                                        <div v-show="Ritem.IsUpload === 0">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-btn
                                                                                        @click="Ritem.startCamera = true, Ritem.camOn = true, onStart()"
                                                                                        color="primary" depressed
                                                                                        class="mr-4">Start</v-btn>
                                                                                    <v-btn class="mr-4" color="secondary"
                                                                                        depressed
                                                                                        @click="onCapture(Ritem, Ritem.QuestionID, false, 1), Ritem.startCamera = false, Ritem.camOn = false"
                                                                                        v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                                    <v-btn
                                                                                        @click="Ritem.startCamera = false, Ritem.camOn = false, onStop()"
                                                                                        v-bind:disabled="!item.camOn"
                                                                                        depressed class="mr-4">Stop</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col md="8" v-if="Ritem.startCamera">
                                                                                    <vue-web-cam ref="webcam"
                                                                                        :device-id="deviceId" width="100%"
                                                                                        height="200px" @started="onStarted"
                                                                                        @stopped="onStopped"
                                                                                        @error="onError"
                                                                                        @cameras="onCameras"
                                                                                        @camera-change="onCameraChange" />
                                                                                    <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                </v-col>

                                                                                <v-col md="4"
                                                                                    v-if="Ritem.img != null && Ritem.img != ''">
                                                                                    <figure class="figure">
                                                                                        <img :src="Ritem.img"
                                                                                            class="center-block my-2"
                                                                                            max-width="100%" height="200"
                                                                                            aspect-ratio="1.7" />
                                                                                    </figure>
                                                                                </v-col>

                                                                            </v-row>
                                                                        </div>
                                                                        <!-- Photo Upload-->
                                                                        <div v-show="Ritem.IsUpload === 1">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-file-input label="Upload Image"
                                                                                        outlined dense accept="image/*"
                                                                                        v-model="Ritem.AnsBlob"
                                                                                        @change="SaveFileRepeat(Ritem, 'Photo', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4"
                                                                                    v-if="Ritem.uploadedImg != null">
                                                                                    <v-img :src=Ritem.uploadedImg
                                                                                        width="250" height="180"
                                                                                        aspect-ratio="1.7"
                                                                                        class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                    </div>

                                                                    <!--Signature Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="Ritem.QuestionType === 'Signature Capture'"
                                                                        v-model="Ritem.Answer">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="Ritem.IsUpload" @change="">
                                                                            <v-radio color="success"
                                                                                label="Signature Capture"
                                                                                hide-details></v-radio>
                                                                            <v-radio color="success"
                                                                                label="Signature Upload"
                                                                                hide-details></v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="Ritem.IsUpload === 0">
                                                                            <v-card outlined>
                                                                                <VueSignaturePad
                                                                                    :options="{ onBegin: () => { $refs.signaturePad[0].resizeCanvas() } }"
                                                                                    width="100%" height="300px"
                                                                                    ref="signaturePad" />
                                                                            </v-card>
                                                                            <div>
                                                                                <v-btn color="secondary" depressed
                                                                                    class="mt-2 mr-2"
                                                                                    :disabled=Ritem.signaturebutton
                                                                                    @click="SaveSignatureRepeatOrSkip(Ritem, false, BenefStepper, Ritem.RQstInd), Ritem.signaturebutton = true">Save</v-btn>
                                                                                <v-btn default depressed class="mt-2 mr-2"
                                                                                    @click="clearSign(Ritem, false, Ritem.QuestionID), Ritem.signaturebutton = false">Clear</v-btn>
                                                                            </div>
                                                                        </div>
                                                                        <div v-show="Ritem.IsUpload === 1">
                                                                            <v-file-input label="Upload Signature" outlined
                                                                                dense accept="image/*"
                                                                                v-model="Ritem.AnsBlob"
                                                                                @change="SaveFileRepeat(Ritem, 'Signature', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>
                                                                        </div>
                                                                        <v-card
                                                                            v-if="Ritem.uploadedSign != null && Ritem.uploadedSign != ''">
                                                                            <v-img :src=Ritem.uploadedSign max-width="50%"
                                                                                height="150" class="my-4"
                                                                                aspect-ratio="1.7"></v-img>
                                                                        </v-card>
                                                                    </div>

                                                                    <!--Audio Capture-->
                                                                    <div class="mb-6"
                                                                        v-if="Ritem.QuestionType === 'Audio Capture'">
                                                                        <v-radio-group class="my-0 py-0" row
                                                                            v-model="Ritem.IsUpload" @change="">
                                                                            <v-radio color="success" label="Audio Capture"
                                                                                hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Audio Upload"
                                                                                hide-details>

                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="Ritem.IsUpload === 0">
                                                                            <audio-recorder upload-url="YOUR_API_URL"
                                                                                id="recAudioOne" :play="playSound"
                                                                                :attempts="1" :time="1" :headers="headers"
                                                                                :before-recording="callback"
                                                                                :pause-recording="callback"
                                                                                :after-recording="SaveAudio"
                                                                                :before-upload="callback"
                                                                                :successful-upload="callback"
                                                                                :failed-upload="callback"
                                                                                v-model="Ritem.Answer"
                                                                                @click.native="AnsBlobQID = Ritem.QuestionID, FindRecordedFileItemSkipRepeat(Ritem)" />

                                                                        </div>
                                                                        <div v-show="Ritem.IsUpload === 1">
                                                                            <v-file-input label="Upload Audio" outlined
                                                                                dense accept="audio/*"
                                                                                v-model="Ritem.AnsBlob"
                                                                                @change="AnsBlobQID = Ritem.QuestionID, SaveFileRepeat(Ritem, 'Audio', Ritem.AnsBlob, false, true, BenefStepper, Ritem.RQstInd)"></v-file-input>

                                                                        </div>
                                                                    </div>
                                                                    <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                    <div v-if="Ritem.isOtherOptionEnabled == true">
                                                                        <input type="text" solo
                                                                            class="custom-text-field elevation-0"
                                                                            label="Type your answer"
                                                                            v-model.lazy="Ritem.OtherOptionAnswer" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </v-col>

                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white;">
                                                <Default-Button-Outlined class="btn-122x36 ma-2" title="Save & Exit"
                                                    @click.native="fnSaveAndExit()"></Default-Button-Outlined>
                                                <Secondary-Button class="btn-122x36 mr-2" title="Next"
                                                    @click.native="fnNonBenefProceedToUpdate(), scrollToNavbarTop()"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!-- Survey Question Update by Import-->
                                    <v-stepper-content step="2" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8; height: 100%">
                                            <v-col cols="12">
                                                <v-col cols="12" class="v-scrolling-div px-4 myTasksInnerCardHeight">
                                                    <v-row class="d-flex justify-center">
                                                        <v-col md="12" class="mt-6" align="center">
                                                            <!-- Drag and Drop Section-->
                                                            <div class="text-center box_outline" @dragover.prevent
                                                                @drop.prevent v-if="lstImportSurvey.length == 0"
                                                                @click="handleFileImport">
                                                                <div @drop="dragFile_Survey">
                                                                    <div><v-icon color="#535353" size="70">mdi-file</v-icon>
                                                                    </div>
                                                                    <div v-if="SurveyFile.length == 0">
                                                                        <a>Drag & Drop The Filled Out Form</a><br />
                                                                        <a>Upload Only Xls File (Not Above 15mb Of Size)</a>
                                                                    </div>
                                                                    <div v-if="SurveyFile.length" class="mt-4">
                                                                        <div v-for="(file, ind) in SurveyFile" :key="ind">
                                                                            <v-chip close="" label="" color="white">
                                                                                <v-icon size="20" color="success"
                                                                                    height="100">mdi-file-delimited-outline</v-icon>
                                                                                {{ file.name }}
                                                                            </v-chip>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- Show vue-excel-editor -->
                                                            <div no-gutters class="uploadBoxHeight pa-0"
                                                                v-if="lstImportSurvey.length > 0">
                                                                <v-col cols="12" xs="12" sm="12" md="12"
                                                                    class="text-center pa-0">
                                                                    <h4 class="heading-3">We are running a format check,
                                                                        Please verify the data</h4>
                                                                </v-col>
                                                                <v-col cols="12" xs="12" sm="12" md="12"
                                                                    class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                    <form ref="form">
                                                                        <vue-excel-editor ref="grid" no-paging
                                                                            disable-panel-setting disable-panel-filter
                                                                            v-model="lstImportSurvey">
                                                                            <div
                                                                                v-for="Hearder in lstImportSurveyQstnHearderN">
                                                                                <vue-excel-column :field="Hearder.text"
                                                                                    :type="Hearder.type"
                                                                                    :mandatory="Hearder.mandatory"
                                                                                    :options="Hearder.options"
                                                                                    :validate="onBeforeNameChange"
                                                                                    width="80px" />
                                                                            </div>
                                                                        </vue-excel-editor>
                                                                    </form>
                                                                </v-col>
                                                            </div>
                                                            <div class="mt-4" align="center"
                                                                v-if="lstImportSurvey.length == 0">
                                                                <input ref="uploader" class="d-none" type="file" accept=""
                                                                    @change="onFileChanged">
                                                                <Secondary-Button class="btn-122x36 mr-2" title="Download"
                                                                    @click.native="DownloadSampleNonBeneficiaryLinkedSurveyFile()"></Secondary-Button>
                                                            </div>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                                <v-col cols="12" align="right" style="background-color:white" class="py-2"
                                                    v-if="lstImportSurvey.length > 0">
                                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Cancel"
                                                        @click.native="CancelImport()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2"
                                                        v-bind:disabled="IsvalidexcelData == false" title="Next"
                                                        @click.native="ImportSurvey()"></Secondary-Button>
                                                </v-col>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!--Final steps-->
                                    <v-stepper-content step="3" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col md="12">
                                                <v-col cols="12" class="v-scrolling-div myTasksInnerCardHeight">
                                                    <div id="scrollToTop">
                                                        <v-avatar size="20"
                                                            style="background-color:#3374B9;color:white">1</v-avatar>
                                                        <span class="subtitle-2 mx-1">Submission Summary</span>
                                                    </div>
                                                    <br />
                                                    <v-card class="hidden-sm-and-down">
                                                        <v-row no-gutters class="text-center">
                                                            <v-col cols="12"
                                                                class="d-flex flex-wrap justify-space-around pa-2">
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ Responses }}</h3>
                                                                    <p>No.of Responses </p>
                                                                </div>
                                                                <div>
                                                                    <p style="font-size:50px"
                                                                        class="font-weight-light grey--text">|</p>
                                                                </div>
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ TotalQuestions }}</h3>
                                                                    <p>No.of Questions/Submission</p>
                                                                </div>
                                                                <div>
                                                                    <p style="font-size:50px"
                                                                        class="font-weight-light grey--text">|</p>
                                                                </div>
                                                                <div>
                                                                    <h3 class="blue--text" style="font-size:30px">
                                                                        {{ CompletionPerc }}%</h3>
                                                                    <p>Completion Ratio</p>
                                                                </div>
                                                                <!--<div>
                                                                <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                                                            </div>
                                                            <div>
                                                                <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                                                                <p>Data collected / Beneficiary</p>
                                                            </div>-->
                                                            </v-col>
                                                        </v-row>
                                                    </v-card>
                                                    <v-card class="hidden-md-and-up">
                                                        <v-row no-gutters>
                                                            <v-col cols="12">
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">No.of
                                                                        Responses</v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            Responses }}</v-col>
                                                                </v-row>
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">No. of
                                                                        Questions/Submission </v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            TotalQuestions }}</v-col>
                                                                </v-row>
                                                                <v-row class="pa-4 ma-0" no-gutters>
                                                                    <v-col cols="10" class="page heading-3-dark">Completion
                                                                        Ratio</v-col>
                                                                    <v-spacer></v-spacer>
                                                                    <v-col cols="2"
                                                                        class="blue--text text-right float-right heading-3-dark">{{
                                                                            CompletionPerc }}%</v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card>
                                                    <br />
                                                    <div>
                                                        <v-avatar size="20"
                                                            style="background-color:#3374B9;color:white">2</v-avatar>
                                                        <span class="subtitle-2 mx-1">Add Comments</span>
                                                    </div>
                                                    <br />
                                                    <v-textarea rows="8" color="black" dense
                                                        label="Summarize Your Comments On The Survey With Few Lines." solo
                                                        class="elevation-0" type="text" v-model.lazy="vmUpdateComment">
                                                    </v-textarea>
                                                    <!--<v-row>
                                                    <v-col md="12">
                                                        <v-file-input class="vFileInput" small-chips truncate-length="10" label="Upload Attachment" prepend-icon="mdi-paperclip" outlined="" dense="" v-model="UploadDocument"
                                                                        id="file" accept="*"></v-file-input>
                                                    </v-col>
                                                </v-row>-->
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white">
                                                <Secondary-Button class="btn-122x36 ma-2" title="Update"
                                                    @click.native="UpdateTask(), scrollToNavbarTop()"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                    <!--if Already updated-->
                                    <v-stepper-content step="4" class="pa-0">
                                        <v-row no-gutters style="background-color: #F5F6F8;">
                                            <v-col md="12">
                                                <v-col cols="12" class="myTasksInnerCardHeight">
                                                    <v-tabs>
                                                        <v-tab href="#tab-1">Submission Summary</v-tab>
                                                        <v-tab href="#tab-2">Activity</v-tab>
                                                        <!--Submission Summary-->
                                                        <v-tab-item value="tab-1" class="mt-4"
                                                            style="background-color: #F5F6F8;">
                                                            <h4 class="text-center heading-3-dark">{{ CustomMessage }}</h4>
                                                            <br />
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Submission Summary</span>
                                                            </div>
                                                            <br />
                                                            <v-card class="hidden-sm-and-down">
                                                                <v-row no-gutters class="text-center">
                                                                    <v-col cols="12"
                                                                        class="d-flex flex-wrap justify-space-around pa-2">
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ Responses }}</h3>
                                                                            <p>No.of Responses </p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ TotalQuestions }}</h3>
                                                                            <p>No.of Questions/Submission</p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ CompletionPerc }}%</h3>
                                                                            <p>Completion Ratio</p>
                                                                        </div>
                                                                        <!--<div>
                                                                        <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                                                                    </div>
                                                                    <div>
                                                                        <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                                                                        <p>Data collected / Beneficiary</p>
                                                                    </div>-->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <v-card class="hidden-md-and-up">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12">
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">No.of
                                                                                Responses</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    Responses }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Questions/Submission </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    TotalQuestions }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">Completion
                                                                                Ratio</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    CompletionPerc }}%</v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <!--<br />
                                                        <div>
                                                            <v-avatar size="20" style="background-color:#3374B9;color:white">2</v-avatar>
                                                            <span class="subtitle-2 mx-1">Add Comments</span>
                                                        </div>
                                                        <br />
                                                        <v-textarea rows="8" color="black" dense label="Summarize Your Comments On The Survey With Few Lines." solo class="elevation-0" type="text">
                                                        </v-textarea>-->
                                                        </v-tab-item>
                                                        <!--Activity Log-->
                                                        <v-tab-item value="tab-2" class="mt-md-4 mt-1"
                                                            style="background-color: #F5F6F8;">
                                                            <br class="hidden-sm-and-down" />
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Activity</span>
                                                            </div>
                                                            <br class="hidden-sm-and-down" />
                                                            <v-card class="v-scrolling-div ActivityLogCardHeight">
                                                                <!--<v-list dense class="py-0 ma-0" two-line>
                                                            <template v-for="(item, index) in IndividualActivityLog">
                                                                <v-list-item>
                                                                    <v-list-item-icon>
                                                                        <v-avatar size="40" color="pink">-->
                                                                <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                <!--<span class="white--text">K</span>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>

                                                                        <v-list-item-content>
                                                                            <v-list-item-title class="text-limit">
                                                                                {{item.Name}} had {{item.Message}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-title class="text-limit" v-if="item.TaskUpdateComment.length>0">
                                                                                Comments : {{item.TaskUpdateComment}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-subtitle>
                                                                                <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                            </v-list-item-subtitle>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </template>
                                                            </v-list>-->
                                                                <!--new Activity layout for approver-->
                                                                <v-card elevation="2" class="ma-3"
                                                                    v-for="(item, index) in lstLogFrame">
                                                                    <v-card-title class="ma-0 pa-0">
                                                                    </v-card-title>
                                                                    <v-card-text>
                                                                        <v-row class="pa-2">
                                                                            <v-col class="pa-0 pt-2 ma-0 d-flex d-inline"
                                                                                style="color:#374151">
                                                                                <h5 class="subtitle-2">
                                                                                    <v-avatar size="25">
                                                                                        <v-img :src="LebelsIcon"
                                                                                            alt="lebels_icon"
                                                                                            size="15"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowLevelName }}
                                                                                </h5>
                                                                                <v-spacer></v-spacer>
                                                                                <small class="text-right"
                                                                                    style="color:#374151">{{
                                                                                        formatDateAndTime(item.SubmissionDate)
                                                                                    }}</small>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.WorkflowStageStatus == 'Approved'"
                                                                                    class="heading-3-dark font-weight-bolder"
                                                                                    style="color:#8EDC92">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#8EDC92">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Rejected'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#ED9A98">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#ED9A98">
                                                                                        <v-img :src="RejectedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Submitted'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="SubmittedIcon"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Assigned'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.DefaultText.length > 0">
                                                                                    <small color="#707070">{{
                                                                                        item.DefaultText }}</small>
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-0 px-2 ma-0 heading-4-dark">
                                                                                <v-avatar size="25"
                                                                                    :color="`${getcolor(item.SubmittedBy)}`">
                                                                                    <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                                    <img v-bind:src="item.ProfileImage"
                                                                                        alt="User DP"
                                                                                        v-if="item.ProfileImage.length > 0" />
                                                                                    <span class="white--text" v-else>{{
                                                                                        item.SubmittedBy == null ?
                                                                                        "N/A" :
                                                                                        item.SubmittedBy.substring(0,
                                                                                            1).toUpperCase() }}</span>
                                                                                </v-avatar>
                                                                                {{ item.SubmittedBy }}
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                </v-card>
                                                            </v-card>
                                                        </v-tab-item>
                                                    </v-tabs>
                                                </v-col>
                                            </v-col>
                                            <v-col cols="12" align="right" style="background-color:white;z-index:3">
                                                <Secondary-Button class="btn-122x36 ma-2" title="Download"
                                                    @click.native="MyTasksDownloadSummary('nonbeneficiarylinked', SurveyInnerTaskIndividualSyncDBID)"></Secondary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-stepper-content>
                                </v-stepper-items>
                            </v-stepper>
                        </v-col>
                    </v-row>
                </v-card>
            </v-dialog>
        </template>
        <!--Survey Benef-linked Approver Dialog-->
        <template>
            <v-dialog v-model="surveyApproveBenfLinkedDialog" persistent :fullscreen="$vuetify.breakpoint.smAndDown">
                <v-card class="myTasksDialogCardHeight">
                    <v-card-title>
                        <v-btn text small @click="surveyApproveBenfLinkedSteps = 1"
                            v-if="surveyApproveBenfLinkedSteps == 2">
                            <v-icon color="#707070" size="20">mdi-keyboard-backspace</v-icon>
                        </v-btn>
                        <span class="limited-width-CardTitle text-truncate page-head">{{ SelectedSurveyName }}</span>
                        <v-spacer></v-spacer>
                        <v-btn text @click.stop="fnClearData()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text class="px-0 py-0 ma-0 hidden-sm-and-down">
                        <v-list dense class="py-0 ma-0" style="background: rgba(61, 126, 254, 0.1);">
                            <template>
                                <v-list-item class="py-0">
                                    <v-list-item-icon>
                                        <v-avatar size="40" :color="`${getcolor(DataCollectorInfo.DatacollectorName)}`">
                                            <!--<img v-bind:src="userimage + DataCollectorInfo.ProfileImage" alt="User DP" v-if="DataCollectorInfo.ProfileImage.length >0" />-->
                                            <span class="white--text">{{ DataCollectorInfo.DatacollectorName == null ? "N/A"
                                                : DataCollectorInfo.DatacollectorName.substring(0, 1).toUpperCase()
                                            }}</span>
                                        </v-avatar>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        <v-list-item-title class="py-0 pb-0">
                                            <v-row class="d-lg-inline-flex py-0 pb-0">
                                                <v-col class="px-0 pl-3 py-0">
                                                    <h4 class="font-weight-large   py-0 pb-0 px-0">
                                                        {{ DataCollectorInfo.DatacollectorName }}</h4>
                                                </v-col>
                                                <v-col class="ml-3 px-0 py-0">
                                                    <h4 class="font-weight-regular  py-0 pb-0">
                                                        {{ DataCollectorInfo.AssignedTime }}</h4>
                                                </v-col>
                                            </v-row>
                                        </v-list-item-title>
                                        <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                            <h4 class="font-weight-regular py-0 pb-0 px-0">Survey Data has been Updated.
                                            </h4>
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-spacer></v-spacer>
                                    <v-btn text @click.stop="">
                                        <v-icon color="primary" size="30">mdi-chevron-up</v-icon>
                                    </v-btn>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-card-text>
                    <v-divider class="hidden-sm-and-down"></v-divider>

                    <v-row no-gutters style="background-color: #F5F6F8;">
                        <v-col cols="12" md="12" sm="12" xs="12">

                            <v-stepper v-model="surveyApproveBenfLinkedSteps" class="elevation-0"
                                style="background-color: #F5F6F8;">
                                <v-stepper-items>
                                    <!--Data Summary & Activity Log-->
                                    <v-stepper-content step="1" class="pa-0 ma-0">
                                        <v-col cols="12">
                                            <v-tabs>
                                                <v-tab href='#tab-1'>Data Summary</v-tab>
                                                <v-tab href='#tab-2'>Activity</v-tab>
                                                <!--Data Collected-->
                                                <v-tab-item value="tab-1"
                                                    class="mt-4 v-scrolling-div myTasksApprovarInnerCardHeight">
                                                    <v-row no-gutters style="background-color: #F5F6F8;">
                                                        <v-col md="12">
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Data Summary</span>
                                                            </div>
                                                            <br />
                                                            <v-card class="hidden-sm-and-down">
                                                                <v-row no-gutters class="text-center">
                                                                    <v-col cols="12"
                                                                        class="d-flex flex-wrap justify-space-around pa-2">
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ this.NoOfSelectedBen }}</h3>
                                                                            <p>No.of Beneficiaries </p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ QstnsPerBeneficiary }}</h3>
                                                                            <p>No.of Questions/Submission</p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ CompletionPerc }}%</h3>
                                                                            <p>Completion Ratio</p>
                                                                        </div>
                                                                        <!--<div>
                    <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                </div>
                <div>
                    <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                    <p>Data collected / Beneficiary</p>
                </div>-->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <v-card class="hidden-md-and-up">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12">
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Beneficiaries </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    this.NoOfSelectedBen }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Questions/Submission </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    QstnsPerBeneficiary }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">Completion
                                                                                Ratio</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    CompletionPerc }}%</v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <br />
                                                            <div>
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">2</v-avatar>
                                                                <span class="subtitle-2 mx-1">Excel Table Data</span>
                                                                <Secondary-Button class="btn-122x36 mr-2 float-right"
                                                                    title="Download"
                                                                    @click.native="MyTasksDownloadSummary('beneficiarylinked', SurveyInnerTaskIndividualSyncDBID)"></Secondary-Button>
                                                            </div>

                                                            <br />
                                                            <!--<v-data-table :headers="ApprovalTableHeader"
                                                                    :items="ApprovalData"
                                                                    item-key="BeneficiaryID"
                                                                    sort-by="BeneficiaryName"
                                                                    fixed-header height="45vh"
                                                                    class="elevation-0">


                                                            </v-data-table>-->

                                                            <vue-excel-editor id="exceleditor" :resetOnClick="resetOnClick"
                                                                no-paging disable-panel-setting disable-panel-filter
                                                                v-model="ApprovalData" @cell-click="fnDownloadDoc"
                                                                @setting="checkFiles" ref="ApproverExcelTable">
                                                                <div v-for="(Hearder, ind) in ApprovalTableHeader"
                                                                    :key="ind">
                                                                    <vue-excel-column :field="Hearder.text"
                                                                        :type="Hearder.type" :mandatory="Hearder.mandatory"
                                                                        :readonly="true" width="80px" />
                                                                </div>
                                                            </vue-excel-editor>

                                                        </v-col>

                                                    </v-row>
                                                </v-tab-item>
                                                <!--Activity Log-->
                                                <v-tab-item value="tab-2"
                                                    class="mt-md-4 mt-1 myTasksApprovarInnerCardHeight"
                                                    style="background-color: #F5F6F8;">
                                                    <!--<br class="hidden-sm-and-down" />-->
                                                    <div id="scrollToTop" class="hidden-sm-and-down">
                                                        <v-avatar size="20"
                                                            style="background-color:#3374B9;color:white">1</v-avatar>
                                                        <span class="subtitle-2 mx-1">Activity</span>
                                                    </div>
                                                    <br class="hidden-sm-and-down" />
                                                    <v-card class="v-scrolling-div ActivityLogCardHeight">
                                                        <!--<v-list dense class="py-0 ma-0" two-line>
                                                    <template v-for="(item, index) in IndividualActivityLog">
                                                        <v-list-item>
                                                            <v-list-item-icon>
                                                                <v-avatar size="40" color="pink">-->
                                                        <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                        <!--<span class="white--text">K</span>
                                                                    </v-avatar>
                                                                </v-list-item-icon>

                                                                <v-list-item-content>
                                                                    <v-list-item-title class="text-limit">
                                                                        {{item.Name}} had {{item.Message}}
                                                                    </v-list-item-title>
                                                                    <v-list-item-title class="text-limit" v-if="item.TaskUpdateComment.length>0">
                                                                        Comments : {{item.TaskUpdateComment}}
                                                                    </v-list-item-title>
                                                                    <v-list-item-subtitle>
                                                                        <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                    </v-list-item-subtitle>
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                        </template>
                                                    </v-list>-->
                                                        <!--new Activity layout for approver-->
                                                        <v-card elevation="2" class="ma-3"
                                                            v-for="(item, index) in lstLogFrame">
                                                            <v-card-title class="ma-0 pa-0">
                                                            </v-card-title>
                                                            <v-card-text>
                                                                <v-row class="pa-2">
                                                                    <v-col class="pa-0 pt-2 ma-0 d-flex d-inline"
                                                                        style="color:#374151">
                                                                        <h5 class="subtitle-2">
                                                                            <v-avatar size="25">
                                                                                <v-img :src="LebelsIcon" alt="lebels_icon"
                                                                                    size="15"></v-img>
                                                                            </v-avatar>
                                                                            {{ item.WorkflowLevelName }}
                                                                        </h5>
                                                                        <v-spacer></v-spacer>
                                                                        <small class="text-right" style="color:#374151">{{
                                                                            formatDateAndTime(item.SubmissionDate)
                                                                        }}</small>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col class="pa-2 ma-0">
                                                                        <div v-if="item.WorkflowStageStatus == 'Approved'"
                                                                            class="heading-3-dark font-weight-bolder"
                                                                            style="color:#8EDC92">
                                                                            <v-avatar size="25" style="background:#8EDC92">
                                                                                <v-img :src="ApprovedIcon" class="pa-1"
                                                                                    alt="action_icon"></v-img>
                                                                            </v-avatar>
                                                                            {{ item.WorkflowStageStatus }}
                                                                        </div>
                                                                        <div v-else-if="item.WorkflowStageStatus == 'Rejected'"
                                                                            class="heading-3-dark" style="color:#ED9A98">
                                                                            <v-avatar size="25" style="background:#ED9A98">
                                                                                <v-img :src="RejectedIcon" class="pa-1"
                                                                                    alt="action_icon"></v-img>
                                                                            </v-avatar>
                                                                            {{ item.WorkflowStageStatus }}
                                                                        </div>
                                                                        <div v-else-if="item.WorkflowStageStatus == 'Submitted'"
                                                                            class="heading-3-dark" style="color:#7395DA">
                                                                            <v-avatar size="25" style="background:#7395DA">
                                                                                <v-img :src="SubmittedIcon"
                                                                                    alt="action_icon"></v-img>
                                                                            </v-avatar>
                                                                            {{ item.WorkflowStageStatus }}
                                                                        </div>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col class="pa-2 ma-0">
                                                                        <div v-if="item.DefaultText.length > 0">
                                                                            <small color="#707070">{{ item.DefaultText
                                                                            }}</small>
                                                                        </div>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col class="pa-0 px-2 ma-0 heading-4-dark">
                                                                        <v-avatar size="25"
                                                                            :color="`${getcolor(item.SubmittedBy)}`">
                                                                            <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                            <img v-bind:src="item.ProfileImage"
                                                                                alt="User DP"
                                                                                v-if="item.ProfileImage.length > 0" />
                                                                            <span class="white--text" v-else>{{
                                                                                item.SubmittedBy == null ? "N/A" :
                                                                                item.SubmittedBy.substring(0,
                                                                                    1).toUpperCase() }}</span>
                                                                        </v-avatar>
                                                                        {{ item.SubmittedBy }}
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-card>
                                                </v-tab-item>
                                            </v-tabs>
                                        </v-col>
                                        <v-col cols="12" align="right" class="d-flex" style="background-color:white">
                                            <v-select width="100px" label="Please select" :items="ApprovalStatus" dense
                                                outlined v-model="SelectedAppStatus" class="mr-2"></v-select>


                                            <Secondary-Button class="btn-122x36 mr-2" title="Next"
                                                @click.native="surveyApproveBenfLinkedSteps = 2, scrollToNavbarTop()"
                                                v-bind:disabled="SelectedAppStatus.length == 0"></Secondary-Button>
                                        </v-col>
                                    </v-stepper-content>
                                    <v-stepper-content step="2" class="pa-0 ma-0">
                                        <!--Comments-->
                                        <v-col md="12" style="background-color: #F5F6F8;"
                                            class="v-scrolling-div myTasksInnerCardHeight">
                                            <br />
                                            <div id="scrollToTop">
                                                <v-avatar size="20"
                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                <span class="subtitle-2 mx-1">Add Comments</span>
                                            </div>
                                            <br />
                                            <v-textarea rows="8" color="black" dense
                                                label="Summarize Your Comments On The Survey With Few Lines." solo
                                                class="elevation-0" type="text" v-model.lazy="TaskApproveDescription">
                                            </v-textarea>
                                        </v-col>
                                        <v-col cols="12" align="right" style="background-color:white">
                                            <Default-Button-Outlined class="btn-122x36 mr-2" title="Previous"
                                                @click.native="surveyApproveBenfLinkedSteps = 1"></Default-Button-Outlined>
                                            <Secondary-Button class="btn-122x36 mr-2" title="Submit"
                                                @click.native="ApproveOrReject(), scrollToNavbarTop()"
                                                v-bind:disabled="SelectedAppStatus.length == 0"></Secondary-Button>
                                        </v-col>
                                    </v-stepper-content>
                                </v-stepper-items>
                            </v-stepper>
                        </v-col>
                    </v-row>
                </v-card>
            </v-dialog>
        </template>
        <!--Non - Beneficiary Survey Approver Dialog-->
        <template>
            <v-dialog v-model="surveyApproveNonBenfLinkedDialog" persistent :fullscreen="$vuetify.breakpoint.smAndDown">
                <v-card class="myTasksDialogCardHeight">
                    <!--class="mytask-right-fixed-panel"-->
                    <v-card-title>
                        <v-btn text small @click="ApproveNonBenefLinkedSurveySteps = 1"
                            v-if="ApproveNonBenefLinkedSurveySteps == 2">
                            <v-icon color="#707070" size="20">mdi-keyboard-backspace</v-icon>
                        </v-btn>
                        <span class="limited-width-CardTitle text-truncate page-head">{{ SelectedSurveyName }}</span>
                        <v-spacer></v-spacer>
                        <v-btn text @click.stop="fnClearData()">
                            <v-icon color="#707070" size="20">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text class="px-0 py-0 ma-0 hidden-sm-and-down">
                        <v-list dense class="py-0 ma-0" style="background: rgba(61, 126, 254, 0.1);">
                            <template>
                                <v-list-item class="py-0">
                                    <v-list-item-icon>
                                        <v-avatar size="40" :color="`${getcolor(DataCollectorInfo.DatacollectorName)}`">
                                            <!--<img v-bind:src="userimage + DataCollectorInfo.ProfileImage" alt="User DP" v-if="DataCollectorInfo.ProfileImage.length >0" />-->
                                            <span class="white--text">{{ DataCollectorInfo.DatacollectorName == null ? "N/A"
                                                : DataCollectorInfo.DatacollectorName.substring(0, 1).toUpperCase()
                                            }}</span>
                                        </v-avatar>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        <v-list-item-title class="py-0 pb-0">
                                            <v-row class="d-lg-inline-flex py-0 pb-0">
                                                <v-col class="px-0 pl-3 py-0">
                                                    <h4 class="font-weight-large   py-0 pb-0 px-0">
                                                        {{ DataCollectorInfo.DatacollectorName }}</h4>
                                                </v-col>
                                                <v-col class="ml-3 px-0 py-0">
                                                    <h4 class="font-weight-regular  py-0 pb-0">
                                                        {{ DataCollectorInfo.AssignedTime }}</h4>
                                                </v-col>
                                            </v-row>
                                        </v-list-item-title>
                                        <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                            <h4 class="font-weight-regular py-0 pb-0 px-0">Survey Data has been Updated.
                                            </h4>
                                        </v-list-item-subtitle>
                                    </v-list-item-content>
                                    <v-spacer></v-spacer>
                                    <v-btn text @click.stop="">
                                        <v-icon color="primary" size="30">mdi-chevron-up</v-icon>
                                    </v-btn>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-card-text>
                    <v-divider class="hidden-sm-and-down"></v-divider>
                    <v-row no-gutters style="background-color: #F5F6F8;">
                        <v-col cols="12" md="12" sm="12" xs="12">
                            <v-stepper v-model="ApproveNonBenefLinkedSurveySteps" class="elevation-0"
                                style="background-color: #F5F6F8;">
                                <v-stepper-items>
                                    <!--Data Summary & Activity Log-->
                                    <v-stepper-content step="1" class="pa-0 ma-0">
                                        <v-col cols="12">
                                            <v-tabs class="pa-0 ma-0">
                                                <v-tab href='#tab-1'>Data Summary</v-tab>
                                                <v-tab href='#tab-2'>Activity</v-tab>
                                                <!--Data Collected-->
                                                <v-tab-item value="tab-1"
                                                    class="mt-4 v-scrolling-div myTasksApprovarInnerCardHeight">
                                                    <v-row no-gutters style="background-color: #F5F6F8;">
                                                        <v-col md="12">
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Submission Summary</span>
                                                            </div>
                                                            <br />
                                                            <v-card class="hidden-sm-and-down">
                                                                <v-row no-gutters class="text-center">
                                                                    <v-col cols="12"
                                                                        class="d-flex flex-wrap justify-space-around pa-2">
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ Responses }}</h3>
                                                                            <p>No.of Responses </p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ TotalQuestions }}</h3>
                                                                            <p>No.of Questions/Submission</p>
                                                                        </div>
                                                                        <div>
                                                                            <p style="font-size:50px"
                                                                                class="font-weight-light grey--text">|</p>
                                                                        </div>
                                                                        <div>
                                                                            <h3 class="blue--text" style="font-size:30px">
                                                                                {{ CompletionPerc }}%</h3>
                                                                            <p>Completion Ratio</p>
                                                                        </div>
                                                                        <!--<div>
                                                                        <p style="font-size:50px" class="font-weight-light grey--text">|</p>
                                                                    </div>
                                                                    <div>
                                                                        <h3 class="blue--text" style="font-size:30px">{{CompletionPerc}}%</h3>
                                                                        <p>Data collected / Beneficiary</p>
                                                                    </div>-->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>
                                                            <v-card class="hidden-md-and-up">
                                                                <v-row no-gutters>
                                                                    <v-col cols="12">
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">No.of
                                                                                Responses</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    Responses }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10" class="page heading-3-dark">No.
                                                                                of Questions/Submission </v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    TotalQuestions }}</v-col>
                                                                        </v-row>
                                                                        <v-row class="pa-4 ma-0" no-gutters>
                                                                            <v-col cols="10"
                                                                                class="page heading-3-dark">Completion
                                                                                Ratio</v-col>
                                                                            <v-spacer></v-spacer>
                                                                            <v-col cols="2"
                                                                                class="blue--text text-right float-right heading-3-dark">{{
                                                                                    CompletionPerc }}%</v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card>

                                                            <br />
                                                            <div>
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">2</v-avatar>
                                                                <span class="subtitle-2 mx-1">Excel Table Data</span>
                                                                <Secondary-Button class="btn-122x36 mr-2 float-right"
                                                                    title="Download"
                                                                    @click.native="MyTasksDownloadSummary('nonbeneficiarylinked', SurveyInnerTaskIndividualSyncDBID)"></Secondary-Button>
                                                            </div>
                                                            <br />
                                                            <!--<v-data-table :headers="NonBenfApprovalTableHeader"
                                                                        :items="NonBenfApprovalData"
                                                                        fixed-header height="45vh"
                                                                        class="elevation-0">
                                                        </v-data-table>-->
                                                            <vue-excel-editor id="exceldataNonBenf"
                                                                :resetOnClick="resetOnClick" no-paging disable-panel-setting
                                                                disable-panel-filter v-model="NonBenfApprovalData"
                                                                @cell-click="fnDownloadDoc" @setting="checkFiles"
                                                                ref="ApproverExcelTable">
                                                                <div v-for="(Hearder, ind) in NonBenfApprovalTableHeader"
                                                                    :key="ind">
                                                                    <vue-excel-column :field="Hearder.text"
                                                                        :type="Hearder.type" :mandatory="Hearder.mandatory"
                                                                        :readonly="true" width="80px" />
                                                                </div>
                                                            </vue-excel-editor>

                                                        </v-col>

                                                    </v-row>
                                                </v-tab-item>
                                                <!--Activity-->
                                                <v-tab-item value="tab-2"
                                                    class="mt-md-4 mt-1 myTasksApprovarInnerCardHeight">
                                                    <v-row no-gutters style="background-color: #F5F6F8">
                                                        <v-col md="12">
                                                            <!--<br class="hidden-sm-and-down" />-->
                                                            <div id="scrollToTop" class="hidden-sm-and-down">
                                                                <v-avatar size="20"
                                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                                <span class="subtitle-2 mx-1">Activity</span>
                                                            </div>
                                                            <br class="hidden-sm-and-down" />
                                                            <v-card class="v-scrolling-div ActivityLogCardHeight">
                                                                <!--<v-list dense class="py-0 ma-0" two-line>
                                                            <template v-for="(item, index) in IndividualActivityLog">
                                                                <v-list-item>
                                                                    <v-list-item-icon>
                                                                        <v-avatar size="40" color="pink">-->
                                                                <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                <!--<span class="white--text">K</span>
                                                                            </v-avatar>
                                                                        </v-list-item-icon>

                                                                        <v-list-item-content>
                                                                            <v-list-item-title class="text-limit">
                                                                                {{item.Name}} had {{item.Message}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-title class="text-limit" v-if="item.TaskUpdateComment.length>0">
                                                                                Comments : {{item.TaskUpdateComment}}
                                                                            </v-list-item-title>
                                                                            <v-list-item-subtitle>
                                                                                <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                            </v-list-item-subtitle>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </template>
                                                            </v-list>-->
                                                                <!--new Activity layout for approver-->
                                                                <v-card elevation="2" class="ma-3"
                                                                    v-for="(item, index) in lstLogFrame">
                                                                    <v-card-title class="ma-0 pa-0">
                                                                    </v-card-title>
                                                                    <v-card-text>
                                                                        <v-row class="pa-2">
                                                                            <v-col class="pa-0 pt-2 ma-0 d-flex d-inline"
                                                                                style="color:#374151">
                                                                                <h5 class="subtitle-2">
                                                                                    <v-avatar size="25">
                                                                                        <v-img :src="LebelsIcon"
                                                                                            alt="lebels_icon"
                                                                                            size="15"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowLevelName }}
                                                                                </h5>
                                                                                <v-spacer></v-spacer>
                                                                                <small class="text-right"
                                                                                    style="color:#374151">{{
                                                                                        formatDateAndTime(item.SubmissionDate)
                                                                                    }}</small>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.WorkflowStageStatus == 'Approved'"
                                                                                    class="heading-3-dark font-weight-bolder"
                                                                                    style="color:#8EDC92">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#8EDC92">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Rejected'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#ED9A98">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#ED9A98">
                                                                                        <v-img :src="RejectedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Submitted'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="SubmittedIcon"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                                <div v-else-if="item.WorkflowStageStatus == 'Assigned'"
                                                                                    class="heading-3-dark"
                                                                                    style="color:#7395DA">
                                                                                    <v-avatar size="25"
                                                                                        style="background:#7395DA">
                                                                                        <v-img :src="ApprovedIcon"
                                                                                            class="pa-1"
                                                                                            alt="action_icon"></v-img>
                                                                                    </v-avatar>
                                                                                    {{ item.WorkflowStageStatus }}
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-2 ma-0">
                                                                                <div v-if="item.DefaultText.length > 0">
                                                                                    <small color="#707070">{{
                                                                                        item.DefaultText }}</small>
                                                                                </div>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col class="pa-0 px-2 ma-0 heading-4-dark">
                                                                                <v-avatar size="25"
                                                                                    :color="`${getcolor(item.SubmittedBy)}`">
                                                                                    <!--<img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />-->
                                                                                    <img v-bind:src="item.ProfileImage"
                                                                                        alt="User DP"
                                                                                        v-if="item.ProfileImage.length > 0" />
                                                                                    <span class="white--text" v-else>{{
                                                                                        item.SubmittedBy == null ?
                                                                                        "N/A" :
                                                                                        item.SubmittedBy.substring(0,
                                                                                            1).toUpperCase() }}</span>
                                                                                </v-avatar>
                                                                                {{ item.SubmittedBy }}
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card-text>
                                                                </v-card>
                                                            </v-card>
                                                        </v-col>
                                                    </v-row>
                                                </v-tab-item>

                                            </v-tabs>
                                        </v-col>
                                        <v-col cols="12" align="right" class="d-flex" style="background-color:white">
                                            <v-select width="100px" label="Please select" :items="ApprovalStatus" dense
                                                outlined v-model="SelectedAppStatus" class="mr-2"></v-select>


                                            <Secondary-Button class="btn-122x36 mr-2" title="Next"
                                                @click.native="ApproveNonBenefLinkedSurveySteps = 2, scrollToNavbarTop()"
                                                v-bind:disabled="SelectedAppStatus.length == 0"></Secondary-Button>
                                        </v-col>
                                    </v-stepper-content>
                                    <v-stepper-content step="2" class="pa-0 ma-0">
                                        <!--Comments-->
                                        <v-col md="12" style="background-color: #F5F6F8;"
                                            class="v-scrolling-div myTasksInnerCardHeight">
                                            <br />
                                            <div id="scrollToTop">
                                                <v-avatar size="20"
                                                    style="background-color:#3374B9;color:white">1</v-avatar>
                                                <span class="subtitle-2 mx-1">Add Comments</span>
                                            </div>
                                            <br />
                                            <v-textarea rows="8" color="black" dense
                                                label="Summarize Your Comments On The Survey With Few Lines." solo
                                                class="elevation-0" type="text" v-model.lazy="TaskApproveDescription">
                                            </v-textarea>
                                        </v-col>
                                        <v-col cols="12" align="right" style="background-color:white">
                                            <Default-Button-Outlined class="btn-122x36 mr-2" title="Previous"
                                                @click.native="ApproveNonBenefLinkedSurveySteps = 1"></Default-Button-Outlined>
                                            <Secondary-Button class="btn-122x36 mr-2" title="Submit"
                                                @click.native="ApproveOrReject(), scrollToNavbarTop()"
                                                v-bind:disabled="SelectedAppStatus.length == 0"></Secondary-Button>
                                        </v-col>
                                    </v-stepper-content>
                                </v-stepper-items>
                            </v-stepper>


                        </v-col>
                    </v-row>
                </v-card>
            </v-dialog>
        </template>


        <v-dialog v-model="dlgValidationMessage" width="700" persistent>
            <template v-slot:activator="{ on, attrs }">
                <v-btn dark icon v-bind="attrs" v-on="on">
                    <v-icon>mdi-dots-vertical</v-icon>
                </v-btn>
            </template>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-information</v-icon>
                    </v-avatar>
                    Information
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_U166" text="" @click="dlgValidationMessage = false">
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
                                            <h4 class="text-center heading-3">There are some mandatory questions that are
                                                not answered</h4>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <Secondary-Button id="AutoTest_U167" class="btn-122x36 mr-2" title="OK"
                                                @click.native="dlgValidationMessage = false"></Secondary-Button>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>

        <v-snackbar v-model="SuccessSnackbar" top color="success" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="success" class="pa-0 ma-0">Data collected Successfully</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="InfoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{ this.SnackbarMsg }}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </v-container>
</template>

<script>
window.addEventListener("beforeunload", (event) => {
    if (window.navigator.onLine === false) {
        // Cancel the event as stated by the standard.
        event.preventDefault();
        // Chrome requires returnValue to be set.
        event.returnValue = "";
    }
});

import { mapState } from 'vuex'
import SecondaryButton from '@/components/secondary-button.vue'
import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
import { dbImworks } from '../../ImworksDb'
import objUtils from '../../utils.js';
import { Object, Math } from 'core-js';
import { WebCam } from "vue-web-cam";
import { v4 as uuidv4 } from 'uuid';
import PreLoader from "@/components/pre-loader.vue";
import syncData from '../../ImworksMobApp.js'
var tjsummary;
async function importscript() {
    return Promise.all([
        import("../../BL/ProjectBeneficiary.js").then(mod => {
            tjsummary = new mod.ProjectBeneficiary();
        })
    ]);
}
var objMyTask;
async function importscriptMyTask() {
    return Promise.all([
        import("../../BL/MyTask.js").then(mod => {
            objMyTask = new mod.MyTask();
        })
    ]);
}
export default {
    data() {

        return {
            dlgValidationMessage: false,
            isSelecting: false,
            userimage: '',
            SurveyNewActivityDialog: false,
            events: [],
            nonce: 0,
            cardImage: require("@/images/Fillers.svg"),
            calIcon: require("@/assets/Survey/Property 1=Calender.svg"),
            RecIcon: require("@/assets/Survey/Property 1=Recurring.svg"),
            BenIcon: require("@/assets/Survey/Property 1=User.svg"),
            ActIcon: require("@/assets/Survey/Property 1=Activity.svg"),
            ApprovedIcon: require("@/assets/Survey/Property 1=Approve.svg"),
            RejectedIcon: require("@/assets/Survey/Property 1=Rejected.svg"),
            SubmittedIcon: require("@/assets/Survey/Property 1=Submitted.svg"),
            LebelsIcon: require("@/assets/Survey/Property 1=Lebels.svg"),
            EditIcon: require("@/assets/Survey/Property 1=Edit.svg"),
            WorkspaceID: 0,
            File: [], // Include excel file when we do drag and drop for beneficiary
            SurveyFile: [], //Include excel file when we do drag and drop for Survey
            csvFileName: "",
            hover: true,
            TaskCardData: [],
            TaskStatus: 'Current',
            TaskType: "To DataCollect",
            TaskComment: "Survey Data Requested",
            IsBeneficiaryLinkedSurvey: false,
            SelectedSurveyName: '',
            SelectedTaskType: '',
            SelectedTaskTypeID: 0,
            DataCollectorInfo: [{ DatacollectorName: "", AssignedTime: "" }], //Thur, 8:30 AM
            IsTaskUpdated: false,
            lstSelectedTaskData: [],
            vmSearchBeneficiary: "",
            BeneficiariesWithName: [],
            BeneficiarySectionQstnList: [],
            lstSections: [],
            lstSectionsQstns: [],
            audUpload: [],
            //GPS Capture
            Coordinates: {
                lat: 0,
                lng: 0,
            },
            defaultCoordinates: {
                lat: 0,
                lng: 0,
            },
            savedAudio: "",
            headers: {
                'X-Custom-Header': 'some data'
            },
            blobFileInput: null,
            SuccessSnackbar: false,
            SnackbarMsg: '',
            SelectedTaskIndex: 0,
            SelectedTaskDate: "",
            QstnsPerBeneficiary: 0,
            CompletionPerc: 0,
            TotalQuestions: 0,
            Responses: 0,
            vmUpdateComment: '',
            NoOfSelectedBen: 0,
            UploadDocument: null,
            cascadingAnsArray: [],
            lstCountry: [],
            lstStates: [],
            TotalQstnsPerBeneficiary: 0,
            lstDistrict: [],
            lstSubDistrict: [],
            AnsBlob: [],
            deviceId: null,
            devices: [],
            multiChoiceAns: [],
            RepeatOrSkipSection: [],
            lstBenefRegQstns: [],
            newBenefID: 0,
            multiChoiceAnsBT: [],   // for beneficiary Registration
            cascadingAnsBT: [],     // for beneficiary Registration
            lstNewBeneficiary: [],  // Shows list of Newly Registered Beneficiary
            UnSavedNewBenfList: [], // Data getting directly from Dexie table
            SelectedBenefTypeID: 0,
            filterNewBenefList: [],// Data after filtered using beneficiary TypeID
            SavedTaskDetails: [],
            SaveAndContinue: false,
            SuccessSnackbar1: false,
            InfoSnackbarMsg: false,
            DataSavedSnackBar: false,
            lstImportBeneficiaryHearder: [],
            lstImportBeneficiary: [], // Shows imported Beneficiary List from excel
            BenefImportTab: 'tab-4',
            lstImportSurveyQstnHearder: [],
            lstImportSurvey: [],
            lstImportSurveyQstnHearderN: [],
            IsImportSurvey: false,  // variable for checking Survey updation is throug Import or not
            lstBenefSurveyQstnAnswers: [],

            ApprovalTableHeader: [],
            ApprovalData: [],
            TaskApproveAttachment: null,
            TaskApproveDescription: "",
            ApproveDocName: "",

            ApprovalStatus: ['Approve', 'Reject'],
            SelectedAppStatus: "",

            TaskCollect: true,
            listSurveySpecificTask: [],
            //survey dialog beneficiary linked
            SurveyBenLinkDlg: false,
            SearchExistingBeneficiary: false,
            AddNewBenificiary: false,
            BenefLinkedSurveySteps: 1,
            surveyApproveBenfLinkedSteps: 1,
            vmSelectedBeneficiaries: [],
            BenefStepper: 1,
            //survey dialog Non - Beneficiary linked
            NonBenLinkSurveyDlg: false,
            NonBenefLinkedSurveySteps: 1,
            ApproveNonBenefLinkedSurveySteps: 1,
            //beneficiary data table
            lstSelectedBeneficiary: [],
            BeneficiaryTableheaders: [
                { text: "Name", align: "left", value: "BeneficiaryName" },
                { text: "Created Date", value: "CreatedDate" }
            ],
            lstBeneficiary: [],
            progress_value: 39,
            bufferValue: 100,

            //survey Approver
            surveyApproveBenfLinkedDialog: false,
            surveyApproveNonBenfLinkedDialog: false,
            SurveyApproveSteps: 1,
            SurveyApproveNonBenSteps: 1,
            lstTaskList: [],
            lstNewBeneficiary: [],
            BTAnsBlob: [],
            //AuditLog: [],
            lstLogFrame: [],
            // lstfilterlist: [],
            IndividualActivityLog: [],
            PreLoader: false,
            resetOnClick: 0,
            IsvalidexcelData: true,
            MultiChoiceAnsForRepeat: [],
            delList: [],
            lstAnyTimeNewSubmission: [],

            AnyTimeSubmissionId: 0,
            AnyTimeSubmissionDateTime: '',
            //mobile
            openMobileDialog: false,
            mobileTaskItem: '',
            mobIndex: '',

            lstmonth: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"],
            // Beneficiary Camera
            startCamera: false,
            camOn: false,
            img1: null,
            benefUploadedImg: "",
            benefUploadedFile: "",
            benefUploadedSign: "",
            delListBT: [],
            // Approver Non Beneficiary Linked
            NonBenfApprovalTableHeader: [],
            NonBenfApprovalData: [],
            CustomMessage: "This Task is sent for Approval",
            fileUrl: "",
            BeneffileUrl: "",
            LoggedUserID: 0,
            //IsDataCollector: true,
            ParentItemIndex: -1,
            SurveyInnerTaskIndividualSyncDBID: '',
            AnytimeLog: [],

            phoneNoRules: [
                v => !v || /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/.test(v) || 'Please enter valid Phone No'
            ],
            aadharnumberNoRules: [
                v => !v || /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/.test(v) || 'Please enter valid Aadhaar number'
            ],
            minVal: 0,
            maxVal: 0,
            qRules: [],
            txtLength: 10000,
            QstType: '',
            beneficiaryStepperList: [],
            SelectedTaskStatus: '',
            selectedTaskpkId: 0,
            NextLevelAssignedUser: "",
            SelectedTaskAssignedTo: "",
            closeaccordion: null,
            clientguid: "",
            MySurveyTaskSteps: 1,
            newTaskLoader: false
        }
    },

    components: {
        'Secondary-Button': SecondaryButton,
        'Default-Button-Outlined': DefaultButtonOutlined,
        'vue-web-cam': WebCam,
        "Pre-Loader": PreLoader,
    },
    props: [
        "searchtask",
        "benFilterList",
        "reccuringFilterList",
        "projectFilterList",
        "sortType",
    ],
    mounted: async function () {
        var userId = window.LoggedUserID;
        if (userId == undefined || userId == 0 || userId == "") {
            var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
            if (data != null && data.length > 0) {
                window.LoggedUserID = data[0].UserId;
            }
        }
        var myTaskPageCount = localStorage.getItem("myTaskPageCount");

        if (myTaskPageCount == undefined || myTaskPageCount == null) {
            myTaskPageCount = 0;
        }

        await importscript();
        await importscriptMyTask();
        this.userimage = window.SERVER_URL + "/Upload";
        this.WorkspaceID = this.$route.query.WsID;

        if (myTaskPageCount > 0) {
            await this.fnGetLoggedUserDetails();
            this.PreLoader = true;
            try {
                await this.GetTaskData().then(() => this.PreLoader = false);
            } catch (err) {
                console.error("Mounted- GetTaskData" + err);
            }
            await this.UpdateAnyTimeDataStatus();
        }
        var self = this;
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(position => {
                self.Coordinates.lat = position.coords.latitude;
                self.Coordinates.lng = position.coords.longitude;
                self.defaultCoordinates.lat = position.coords.latitude;
                self.defaultCoordinates.lng = position.coords.longitude;
            });
        }
        this.passFilterItems();

        myTaskPageCount++;
        if (myTaskPageCount == 1) {
            localStorage.setItem("myTaskPageCount", 2);
            this.reloadPage();
        }
    },
    computed: {
        ...mapState('SyncBinders', [
            'syncFromFlag',
            'syncToFlag'
        ]),
        device: function () {
            return this.devices.find(n => n.deviceId === this.deviceId);
        },
        //dummy function that shows the latest at the top
        timeline() {
            return this.events.slice().reverse();
        },
        isMobile() {
            if (this.$vuetify.breakpoint.mdAndUp) {
                this.MySurveyTaskSteps = 1;
                this.$emit('checkMyStepper', this.MySurveyTaskSteps);
            }
            return this.$vuetify.breakpoint.smAndDown
        },
        sortedTaskCardData() {
            switch (this.sortType) {
                case "Name - Ascending":
                    return this.TaskCardData.sort((a, b) => (a.TaskName.toLowerCase() > b.TaskName.toLowerCase()) ? 1 : -1);

                    break;
                case "Name - Descending":
                    return this.TaskCardData.sort((a, b) => (a.TaskName.toLowerCase() < b.TaskName.toLowerCase()) ? 1 : -1);
                    break;
                case "Recently Added":
                    return this.TaskCardData.sort((a, b) => (a.CreatedDate < b.CreatedDate) ? 1 : -1);
                    break;
                default:
                    for (let i of this.TaskCardData) {
                        i.TaskLog.sort((a, b) => {
                            return new Date(b.CreatedDate) - new Date(a.CreatedDate);
                        })
                    }
                    return this.TaskCardData
            }
        },
        // Search or Filter
        customSearch() {
            let TaskCardDataDisplay = [];



            if ((this.benFilterList.length > 0) || (this.reccuringFilterList.length > 0) || (this.projectFilterList.length > 0)) {

                if (
                    this.benFilterList.length > 0 &&
                    this.reccuringFilterList.length > 0
                ) {
                    TaskCardDataDisplay = this.sortedTaskCardData.filter((task) => {
                        return (
                            this.benFilterList.indexOf(task.BeneficiaryType) > -1 &&
                            this.reccuringFilterList.indexOf(task.Periodicity) > -1
                        );
                    });
                } else if (this.benFilterList.length > 0) {
                    TaskCardDataDisplay = this.sortedTaskCardData.filter((task) => {
                        return this.benFilterList.indexOf(task.BeneficiaryType) > -1;
                    });
                } else if (this.reccuringFilterList.length > 0) {
                    TaskCardDataDisplay = this.sortedTaskCardData.filter((task) => {
                        return this.reccuringFilterList.indexOf(task.Periodicity) > -1;
                    });
                }
                else {
                    TaskCardDataDisplay = this.sortedTaskCardData
                }
                if (this.projectFilterList.length > 0) {
                    return TaskCardDataDisplay.filter((task) => {
                        return this.projectFilterList.indexOf(task.ProjectName) > -1;
                    });
                }

                else {
                    return TaskCardDataDisplay

                }
            } else {
                return this.sortedTaskCardData.filter((task) => {
                    return task.TaskName.toLowerCase().includes(
                        this.searchtask.toLowerCase()
                    );
                });
            }
        },
        ProjectId() {
            return this.TaskCardData[0]?.TaskLog[0]?.ProjectId ?? 0
        }
    },
    watch: {
        //Web Cam Functions
        camera: function (id) {
            this.deviceId = id;
        },
        devices: function () {
            // Once we have a list select the first one
            const [first, ...tail] = this.devices;
            if (first) {
                this.camera = first.deviceId;
                this.deviceId = first.deviceId;
            }
        },
        // searchtask: async function () {
        //     await this.GetTaskData();
        // },
        syncFromFlag: async function () {
            await this.GetTaskData()
                .then(() => {
                    if (this.isMobile) {
                        let data = this.TaskCardData.filter((x) => {
                            if (x.TypeID == this.mobileTaskItem.TypeID)
                                return x.TaskLog
                        })
                        this.$set(this.mobileTaskItem, 'TaskLog', data[0].TaskLog)
                    }
                })
        },
        syncToFlag: async function () {

            if (this.isMobile) {
                if (this.lstAnyTimeNewSubmission) {
                    this.lstAnyTimeNewSubmission.forEach((x) => {
                        if (x.TaskData.CurrentOrOverdue == "Syncing")
                            x.TaskData.CurrentOrOverdue = "Synced"
                    })
                }
                this.mobileTaskItem.TaskLog.forEach((x) => {
                    if (x.CurrentOrOverdue == "Syncing") {
                        x.CurrentOrOverdue = "Synced"
                    }
                })
            } else {
                await this.GetTaskData()
            }
        },
    },
    created() {
        //GPS Capture
        this.$getLocation({})
            .then(Coordinates => {
                this.Coordinates = Coordinates;
                this.defaultCoordinates = Coordinates;
            });
    },
    methods: {
        reloadPage() {
            window.location.reload();
        },
        checkFiles() {
            this.$nextTick(() => {
                let regex = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/g
                let allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar'];
                let c = this.$refs.ApproverExcelTable.systable.getElementsByTagName('td')
                let arrCols = Array.from(c)
                arrCols.forEach(x => {
                    let e = x.innerText
                    e && allowedExtensions.includes(e.split('.').pop()) && x.classList.add('hyperLink')
                    if (e && regex.test(e)) x.classList.add('hyperLink')
                })
            })
            return true
        },
        // filter dropdown data for landing page
        passFilterItems() {
            console.log(this.TaskCardData)
            let benTypes1 = [...new Set(this.TaskCardData.map(item => item.BeneficiaryType))];
            let periodTypes = [...new Set(this.TaskCardData.map(item => item.Periodicity))];
            let projects = [...new Set(this.TaskCardData.map(item => item.ProjectName))];
            let benTypes = benTypes1.filter(n => n)
            console.log(benTypes);
            console.log(periodTypes);
            console.log(projects);



            this.$emit("getFilters", { benTypes: benTypes, periodTypes: periodTypes, projects: projects })

        },
        //survey data import
        handleFileImport() {
            this.isSelecting = true;

            // After obtaining the focus when closing the FilePicker, return the button state to normal
            window.addEventListener('focus', () => {
                this.isSelecting = false
            }, { once: true });

            // Trigger click on the FileInput
            this.$refs.uploader.click();
        },
        async onFileChanged(e) {
            this.SurveyFile = e.target.files;
            await this.OnSurveyFileUpload();

            // Do whatever you need with the file, liek reading it with FileReader
        },
        //method that scrolls to top
        async scrollToNavbarTop() {
            // this.$refs.scrollingQst.scrollTop = 0
            var ele = document.getElementsByClassName('v-scrolling-div')
            for (let el of ele) {
                if (el.scrollTop > 0) {
                    el.scrollTop = 0
                }
            }
            $(document).scrollTop(0, 0);
            //document.getElementById("scrollToTop").scrollTop(0, 0);
            document.getElementById("scrollToTop").scrollIntoView();
        },

        /// <summary>
        ///  getcolor :- Function used to get color of users
        /// </summary>
        /// <param name="name"></param>
        /// <returns> </returns>
        getcolor(name) {

            var DisplayName = '';
            if (typeof name != 'undefined') {
                DisplayName = name;
            }
            return objUtils.common.getcolor(DisplayName);
        },
        /// <summary>
        ///  formatDateAndTime :- Function used to formatting date and time
        /// </summary>
        /// <param name="date"></param>
        /// <returns> </returns>
        formatDateAndTime(date) {
            return objUtils.common.formatDateAndTime(date);
        },
        FormatDate(date) {
            return objUtils.common.formatDate(date);

        },

        //mobile
        mobileDialog(item, index) {
            //this.openMobileDialog = true;
            this.mobileTaskItem = item;
            this.mobIndex = index;
            this.closeaccordion = true;
            this.MySurveyTaskSteps = 2;
            this.$emit('checkMyStepper', this.MySurveyTaskSteps);

        },

        closeMobileCard() {
            this.mobIndex = '';
            this.mobileTaskItem = '';
            this.MySurveyTaskSteps = 1;
            this.$emit('checkMyStepper', this.MySurveyTaskSteps);
        },
        //dummy method that adds a new task   only for reference
        addAnyTimeSubmission() {
            const time = (new Date()).toTimeString()
            this.events.push({
                id: this.nonce++,
                time: time.replace(/:\d{2}\sGMT-\d{4}\s\((.*)\)/, (match, contents, offset) => {
                    return ` ${contents.split(' ').map(v => v.charAt(0)).join('')}`
                }),
            })

        },
        /// <summary>
        ///  GetTaskData :- Function used to get task data from index table
        /// </summary>
        /// <param name></param>
        /// <returns> </returns>
        async GetTaskData() {
            console.time('started')
            this.TaskCardData = [];
            this.lstTaskList = [];
            this.TaskSubmittedList = [];
            this.lstTaskList = await dbImworks.MyTaskList.toArray();
            this.TaskSubmittedList = await dbImworks.TbMyTask.toArray();
            this.SavedTaskDetails = await dbImworks.SavedTaskDB.toArray();
            this.TaskSyncDb = await dbImworks.TaskSyncDB.toArray();
            //this.AuditLog = await dbImworks.TbTaskAuditLog.toArray();
            //this.lstfilterlist = await dbImworks.TbTaskAuditLog.toArray();
            this.lstAnyTimeNewSubmission = await dbImworks.TbAnyTimeData.toArray();
            var TypeID = 0;
            var index = 0;
            this.lstTaskList.sort(function (a, b) {
                return parseInt(b.TypeID) - parseInt(a.TypeID);
            })
            for (var i = 0; i <= this.lstTaskList.length - 1; i++) {
                var objSurvey = JSON.parse(this.lstTaskList[i].TaskData);
                //objSurvey.pkId = this.lstTaskList[i].pkId;
                if (objSurvey != null) {

                    if (this.lstTaskList[i].Type == 'Survey' && (TypeID != this.lstTaskList[i].TypeID || TypeID == 0)) {
                        this.TaskCardData.push({
                            ColIndex: this.TaskCardData.length,
                            ProjectName: objSurvey.ProjectName,
                            Command: this.lstTaskList[i].Command,
                            TaskName: objSurvey.SurveyName,
                            TaskType: "Survey",
                            TypeID: this.lstTaskList[i].TypeID,
                            CreatedDate: objSurvey.CreatedDate,
                            Periodicity: objSurvey.Periodicity,
                            BeneficiaryType: objSurvey.BeneficiaryType,
                            BeneficiaryTypeID: objSurvey.BeneficiaryTypeID,
                            TaskTypeColor: "rgba(33,150,243,.2)",
                            Notifications: "2",
                            NotificationType: "Survey assigned",
                            btnDisable: false,
                            btnAnytimeSubVisible: false,
                            IsDataCollector: true,
                            IsApprover: false,
                            TaskLog: [],
                            PageNumber: 1,
                            loadMore: true,
                            TaskCount: this.lstTaskList[i].TotalCount,
                        })
                    }
                    index = this.TaskCardData.length - 1;
                    var dtMyTask = [];
                    if (objSurvey.Periodicity == "Anytime" && this.lstTaskList[i].Command == "TaskResponse") {
                        dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.selectedTaskpkId == this.lstTaskList[i].pkId);
                        // Delete if exists from Anytime table using date
                        for (var aCnt = 0; aCnt <= this.lstAnyTimeNewSubmission.length - 1; aCnt++) {

                            var data = this.lstAnyTimeNewSubmission[aCnt].TaskData;

                            var SubmittedDate = objUtils.common.formatDateWithTimeAndSec(this.lstTaskList[i].TaskAddedDate);
                            var CreateDate = objUtils.common.formatDateWithTimeAndSec(data.CreatedDate);
                            if (SubmittedDate == CreateDate) {

                                await dbImworks.TbAnyTimeData.where("pkId").equals(parseInt(this.lstAnyTimeNewSubmission[aCnt].pkId)).delete();
                            }
                        }
                        this.lstAnyTimeNewSubmission = [];
                        this.lstAnyTimeNewSubmission = await dbImworks.TbAnyTimeData.toArray();
                    }
                    else if (objSurvey.Periodicity == "Anytime" && this.lstTaskList[i].Command == "ADD") {
                        dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.SyncDBID == this.lstTaskList[i].SyncDBID);
                    } else {
                        dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.TaskDate == this.lstTaskList[i].TaskDate);
                    }
                    if (objSurvey.EndDate != null) {
                        if (objSurvey.EndDate.substr(0, 10) >= new Date().toISOString().substr(0, 10)) {
                            if (objSurvey.Periodicity == "Anytime" && this.lstTaskList[i].Command == "ADD") {

                                var existsanytime = this.lstTaskList.filter(x => x.TypeID == this.lstTaskList[i].TypeID && x.Command == "TaskResponse");
                                var existsanytime1 = dtMyTask.filter(x => x.TypeID == this.lstTaskList[i].TypeID && x.Command == "TaskResponse");
                                if (existsanytime.length > 0) {
                                    this.TaskCardData[index].btnAnytimeSubVisible = true;
                                }
                                if (existsanytime1.length > 0) {
                                    this.TaskCardData[index].btnAnytimeSubVisible = true;
                                }

                            }
                            if (objSurvey.Periodicity == "Anytime") {
                                var existsanytime = this.lstTaskList.filter(x => x.TypeID == this.lstTaskList[i].TypeID && x.Command == "TaskResponse");
                                if (existsanytime.length > 0) {
                                    this.TaskCardData[index].btnAnytimeSubVisible = true;
                                }
                            }
                        }
                    }

                    let taskLog = await this.processTaskLog(this.lstTaskList[i], index)
                    taskLog && this.TaskCardData[index].TaskLog.push(taskLog)

                }
                TypeID = this.lstTaskList[i].TypeID;
            }
            console.timeEnd('started')
        },
        // async fnSortTaskDataByDate(){
        // 	for(var sCnt = 0; sCnt <=this.TaskCardData.length-1;sCnt++){
        //           this.TaskCardData[sCnt].TaskLog.sort(function (a, b) {
        //                         return new Date(b.CreatedDate) - new Date(a.CreatedDate);
        //                     });
        // 	}
        // },
        /// <summary>
        ///  GetSurveyTaskDetails :- Function used to get task data details and open corresponding beneficiary,non beneficiary,approver or the summary page
        /// </summary>
        /// <param name></param>
        /// <returns> </returns>
        async GetSurveyTaskDetails(TaskDataitem, pkId, pIndex) {

            if (typeof TaskDataitem != "undefined") {
                this.clientguid = uuidv4();
                this.PreLoader = true;
                this.TaskCardData[pIndex].btnDisable = true;
                this.ParentItemIndex = pIndex;
                this.AnyTimeSubmissionId = pkId;
                if (pkId > 0) {
                    if (this.lstAnyTimeNewSubmission.length > 0) {
                        this.AnyTimeSubmissionDateTime = this.lstAnyTimeNewSubmission.filter(x => x.pkId == pkId)[0].TaskData.CreatedDate;
                        //CreatedDate = "12/28/2021, 8:10:48 PM"
                    }
                }
                this.selectedTaskpkId = TaskDataitem.myTaskListpkId;
                this.IsBeneficiaryLinkedSurvey = false;
                if (TaskDataitem.BeneficiaryTypeID > 0) {
                    this.IsBeneficiaryLinkedSurvey = true;
                }
                this.beneficiaryStepperList.push(this.BenefStepper);
                TaskDataitem.TaskData = await this.GetTaskDataList(TaskDataitem.TypeID, TaskDataitem.SyncDBID, TaskDataitem.TaskData, TaskDataitem.DataCollectionSyncDBID, TaskDataitem.BeneficiaryTypeID);
                this.SelectedTaskType = TaskDataitem.TaskType;
                this.SelectedTaskTypeID = TaskDataitem.TypeID;
                this.SelectedSurveyName = TaskDataitem.TaskName;
                this.DataCollectorInfo.DatacollectorName = TaskDataitem.Name;
                this.DataCollectorInfo.AssignedTime = this.FormatDateWithTime(TaskDataitem.CreatedDate);
                this.lstSelectedTaskData = TaskDataitem;
                this.SelectedTaskDate = TaskDataitem.TaskDate;
                this.SelectedTaskIndex = TaskDataitem.TaskIndex;
                this.SelectedBenefTypeID = TaskDataitem.BeneficiaryTypeID;
                this.SelectedTaskStatus = TaskDataitem.CurrentOrOverdue;
                this.SelectedTaskAssignedTo = TaskDataitem.AssignedToIds;
                if (pkId == 0) {
                    this.AnyTimeSubmissionDateTime = TaskDataitem.CreatedDate;
                }

                this.UnSavedNewBenfList = await dbImworks.TbNewBeneficiary.toArray();

                this.filterNewBenefList = this.UnSavedNewBenfList.filter(x => x.BeneficiaryTypeID == this.SelectedBenefTypeID && x.WorkspaceID == this.WorkspaceID);
                this.fileUrl = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + TaskDataitem.ProjectId.toString() + "/";
                this.BeneffileUrl = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + TaskDataitem.ProjectId.toString() + "/";
                // this id we will pass as an argument in MyTasks download summary function
                this.SurveyInnerTaskIndividualSyncDBID = this.lstSelectedTaskData.SyncDBID;
                var lstAssignedToUser = ""; //this.SelectedTaskAssignedTo.split(',').map(element => {return Number(element);});
                if (this.SelectedTaskAssignedTo != null && this.SelectedTaskAssignedTo != 0) {

                    lstAssignedToUser = this.SelectedTaskAssignedTo.split(',').map(element => { return Number(element); });
                }
                // Goto Approval
                if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Review") && (lstAssignedToUser.includes(this.LoggedUserID)) && this.IsBeneficiaryLinkedSurvey) {
                    // Goto Approval Page
                    this.surveyApproveBenfLinkedDialog = true;
                    this.surveyApproveBenfLinkedSteps = 1;
                    await this.GetMyTaskActivityLog(TaskDataitem.SyncDBID);
                    this.BenefLinkedSurveySteps = 6;
                    await this.fnCalcProgressApprover();
                    //await this.fnGetApproverQstnAnswers();
                    await this.fnGetApproverQstnAnswers_Modified();
                    this.PreLoader = false;
                }
                if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Review") && (lstAssignedToUser.includes(this.LoggedUserID)) && !this.IsBeneficiaryLinkedSurvey) {
                    // Goto Approval Page
                    this.surveyApproveNonBenfLinkedDialog = true;
                    this.ApproveNonBenefLinkedSurveySteps = 1;
                    await this.fnCalcProgressApprover();
                    await this.fnGetNonBenfApproverQstnAnswers();
                    this.PreLoader = false;
                }
                // Already Data collected beneficiary linked
                if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Review" || TaskDataitem.CurrentOrOverdue == "Syncing" || TaskDataitem.CurrentOrOverdue == "Completed") && (!lstAssignedToUser.includes(this.LoggedUserID)) && this.IsBeneficiaryLinkedSurvey) {
                    // Goto progress showing page
                    this.SurveyBenLinkDlg = true;
                    await this.GetMyTaskActivityLog(TaskDataitem.SyncDBID);
                    this.BenefLinkedSurveySteps = 6;
                    await this.fnCalcProgressApprover();
                    this.PreLoader = false;
                }
                // Already Data collected non - beneficiary linked
                if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Review" || TaskDataitem.CurrentOrOverdue == "Syncing" || TaskDataitem.CurrentOrOverdue == "Completed") && (!lstAssignedToUser.includes(this.LoggedUserID)) && !this.IsBeneficiaryLinkedSurvey) {
                    // Goto progress showing page
                    this.NonBenLinkSurveyDlg = true;
                    this.NonBenefLinkedSurveySteps = 4;
                    await this.fnCalcProgressApprover();
                    this.PreLoader = false;
                }
                // Beneficiary Linked
                if ((TaskDataitem.Status == "Approved" || TaskDataitem.Status == "Rejected" || TaskDataitem.Status == "To Data Collect") && (TaskDataitem.CurrentOrOverdue == "Review" || TaskDataitem.CurrentOrOverdue == "Syncing" || TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Rejected") && this.IsBeneficiaryLinkedSurvey && (!lstAssignedToUser.includes(this.LoggedUserID))) {
                    // Goto progress showing page
                    this.SurveyBenLinkDlg = true;
                    await this.GetMyTaskActivityLog(TaskDataitem.SyncDBID);
                    this.BenefLinkedSurveySteps = 6;
                    await this.fnCalcProgressApprover();
                    if (TaskDataitem.Status == "Approved") {
                        this.CustomMessage = "Task Data Approved"
                    }
                    else if (TaskDataitem.Status == "Rejected") {
                        this.CustomMessage = "Task Data Rejeted"
                    }
                    this.PreLoader = false;
                }
                // Non Beneficiary Linked
                if ((TaskDataitem.Status == "Approved" || TaskDataitem.Status == "Rejected" || TaskDataitem.Status == "To Data Collect") && (TaskDataitem.CurrentOrOverdue == "Review" || TaskDataitem.CurrentOrOverdue == "Syncing" || TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Rejected") && !this.IsBeneficiaryLinkedSurvey && (!lstAssignedToUser.includes(this.LoggedUserID))) {
                    // Goto progress showing page
                    this.NonBenLinkSurveyDlg = true;
                    this.NonBenefLinkedSurveySteps = 4
                    if (TaskDataitem.Status == "Approved") {
                        this.CustomMessage = "Task Data Approved"
                    }
                    else if (TaskDataitem.Status == "Rejected") {
                        this.CustomMessage = "Task Data Rejeted"
                    }
                    await this.fnCalcProgressApprover();
                    this.PreLoader = false;
                }
                //Goto Data Collection Page
                if (TaskDataitem.Status == "To Data Collect" && (TaskDataitem.CurrentOrOverdue == "Overdue" || TaskDataitem.CurrentOrOverdue == "Current" || TaskDataitem.CurrentOrOverdue == "Rejected") && this.IsBeneficiaryLinkedSurvey && lstAssignedToUser.includes(this.LoggedUserID)) {

                    //this.BenefLinkedSurveySteps = 1;
                    this.SurveyBenLinkDlg = true;
                    //-- Goto Search beneficiary page ---
                    if (TaskDataitem.CurrentOrOverdue == "Rejected") {
                        await this.GetBeneficiary();
                        await this.fnGetDataFromSavedList("Reject");
                    }
                    else {
                        await this.fnSearchBeneficiaries();
                    }

                    this.PreLoader = false;
                }
                if (TaskDataitem.Status == "To Data Collect" && (TaskDataitem.CurrentOrOverdue == "Overdue" || TaskDataitem.CurrentOrOverdue == "Current" || TaskDataitem.CurrentOrOverdue == "Rejected") && !this.IsBeneficiaryLinkedSurvey && lstAssignedToUser.includes(this.LoggedUserID)) {
                    this.NonBenefLinkedSurveySteps = 1;
                    this.NonBenLinkSurveyDlg = true;
                    await this.GetNonBeneficiayBasedSectionQuestions();
                    if (TaskDataitem.CurrentOrOverdue == "Rejected") {
                        await this.fnGetDataFromSavedList("Reject");
                    }
                    this.PreLoader = false;
                }
                // Beneficiary linked Save and Exit
                if (TaskDataitem.Status == "To Data Collect" && TaskDataitem.CurrentOrOverdue == "Saved" && this.IsBeneficiaryLinkedSurvey) {

                    //this.BenefLinkedSurveySteps = 1;
                    //this.SaveAndContinue = true;
                    //this.SearchExistingBeneficiary = false;
                    this.SurveyBenLinkDlg = true;
                    await this.GetBeneficiary();
                    await this.fnGetDataFromSavedList("Saved");
                    this.PreLoader = false;
                }
                // Non Beneficiary linked Save and Exit
                if (TaskDataitem.Status == "To Data Collect" && TaskDataitem.CurrentOrOverdue == "Saved" && !this.IsBeneficiaryLinkedSurvey) {
                    this.NonBenefLinkedSurveySteps = 1;
                    //this.SaveAndContinue = true;
                    this.NonBenLinkSurveyDlg = true;
                    await this.fnGetDataFromSavedList("Saved");
                    this.PreLoader = false;
                }
                this.PreLoader = false;
                //var SurveyActivityList = await dbImworks.TbTaskAuditLog.toArray();
                //this.IndividualActivityLog = SurveyActivityList.filter(x => x.TypeID == TaskDataitem.TypeID && x.Type == "Survey");
                //this.IndividualActivityLog.sort((a, b) => b.pkLogID - a.pkLogID);
            }

        },

        async GetBeneficiayBasedSectionQuestions() {
            //this.PreLoader = true;
            this.Coordinates = {
                lat: this.defaultCoordinates.lat,
                lng: this.defaultCoordinates.lng,
            };
            await this.GetSelectedBeneficiaries();
            await this.FillSurveyQuestionaries();
            this.BenefLinkedSurveySteps = 4;
            if (this.lstSelectedTaskData.CurrentOrOverdue == "Saved" || this.lstSelectedTaskData.CurrentOrOverdue == "Rejected") {
                await this.fnFetchQstnAnswersSaveAndExit(this.lstSelectedTaskData.CurrentOrOverdue);
            }
            //Enable skip logic and repeat logic, if they have default answer
            await this.fnTriggerSkipOrRepeatLogic();
            await this.scrollToTop(this.lstSectionsQstns);
            this.scrollToNavbarTop();
            //this.PreLoader = false;

        },

        async fnTriggerSkipOrRepeatLogic() {
            if (this.lstSectionsQstns.length > 0) {
                for (var secCnt = 0; secCnt <= this.lstSectionsQstns.length - 1; secCnt++) {
                    var SectionItem = this.lstSectionsQstns[secCnt];
                    var lstDefaultAnsQuestions = SectionItem.Questions.filter(x => x.Answer != "");
                    if (lstDefaultAnsQuestions.length > 0) {
                        // Checking for condition Questions
                        var lstConditionQuestion = lstDefaultAnsQuestions.filter(x => x.IsConditionQuestion == 1);

                        if (lstConditionQuestion.length > 0) {
                            for (var Cnt = 0; Cnt <= lstConditionQuestion.length - 1; Cnt++) {
                                var IsExistInSkipLogic = [];
                                if (this.IsBeneficiaryLinkedSurvey == true) {
                                    IsExistInSkipLogic = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID == lstConditionQuestion[Cnt].QuestionID && x.benefIndex == this.BenefStepper);
                                }
                                else { IsExistInSkipLogic = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID == lstConditionQuestion[Cnt].QuestionID); }
                                if (IsExistInSkipLogic.length == 0) {
                                    if (lstConditionQuestion[Cnt].QuestionType == 'Multi-Choice') {
                                        var vaDefaultAnswer = lstConditionQuestion[Cnt].DefaultValue
                                        for (var OptCnt = 0; OptCnt <= lstConditionQuestion[Cnt].QuestionOptions.length - 1; OptCnt++) {
                                            if (lstConditionQuestion[Cnt].QuestionOptions[OptCnt].OptionValue == vaDefaultAnswer) {
                                                lstConditionQuestion[Cnt].QuestionOptions[OptCnt].IsOptSelected = true;
                                                this.multiChoiceAns.push({ OptionValue: lstConditionQuestion[Cnt].QuestionOptions[OptCnt].OptionValue, QuestionID: lstConditionQuestion[Cnt].QuestionID, benefIndex: this.BenefStepper });
                                            }
                                        }
                                        await this.AddSectionLogicForMultiChoice(null, lstConditionQuestion[Cnt].QuestionID, this.BenefStepper, lstConditionQuestion[Cnt].QuestionOptions);
                                        //  this.AddSectionLogic(lstConditionQuestion[Cnt].QuestionID, this.BenefStepper)
                                    }
                                    else {
                                        this.AddSectionLogic(lstConditionQuestion[Cnt], lstConditionQuestion[Cnt].QuestionID, this.BenefStepper);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            await this.fnDisplayRepeatOrSkipUploadQstn();
        },

        async fnDisplayRepeatOrSkipUploadQstn() {
            var tempRepeatOrSkip = this.RepeatOrSkipSection.filter(x => x.benefIndex == this.BenefStepper);
            for (var rCnt = 0; rCnt <= tempRepeatOrSkip.length - 1; rCnt++) {
                var RepeatItem = tempRepeatOrSkip[rCnt];
                if (RepeatItem.QuestionType == 'Photo Capture' || RepeatItem.QuestionType == 'Signature Capture' || RepeatItem.QuestionType == 'File Upload' || RepeatItem.QuestionType == 'Audio Capture') {
                    var Answer = RepeatItem.Answer;
                    var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();
                    var blbFile = "";
                    if (typeof ansBlobData[0] != "undefined") {
                        blbFile = ansBlobData[0].AttachedDocument;
                    }
                    if (RepeatItem.QuestionType == "Photo Capture" && blbFile != "") {

                        RepeatItem.IsUpload = 1;
                        this.audUpload[Cnt] = 1;
                        if (ansBlobData[0].IsUploadedFile) {
                            const fr = new FileReader()
                            fr.readAsDataURL(blbFile)
                            await fr.addEventListener('loadend', () => {
                                //currSecQstns[i].uploadedSign = fr.result
                                this.RepeatOrSkipSection.filter(x => x.QuestionID == RepeatItem.QuestionID && x.benefIndex == this.BenefStepper)[0].uploadedImg = fr.result

                            });
                        }
                        else {
                            const fr = new FileReader()
                            fr.readAsDataURL(blbFile)
                            await fr.addEventListener('loadend', () => {
                                this.RepeatOrSkipSection.filter(x => x.QuestionID == RepeatItem.QuestionID && x.benefIndex == this.BenefStepper)[0].img = fr.result
                            });
                        }
                    }
                    if (RepeatItem.QuestionType == "File Upload" && blbFile != "") {
                        const fr = new FileReader()
                        fr.readAsDataURL(blbFile)
                        fr.addEventListener('load', () => {
                            //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt - 1].uploadedFile = fr.result;
                            //this.RepeatOrSkipSection.filter(x => x.QuestionID == RepeatItem.QuestionID && x.benefIndex == this.BenefStepper)[0].uploadedFile = fr.result
                            //this.RepeatOrSkipSection[rCnt - 1].uploadedFile = fr.result;
                            tempRepeatOrSkip[rCnt - 1].uploadedFile = fr.result;
                            //if (this.RepeatOrSkipSection[rCnt - 1].benefIndex == this.BenefStepper) {
                            //    this.RepeatOrSkipSection[rCnt - 1].uploadedFile = fr.result;
                            //}
                        });
                    }
                }
            }
        },
        async FillSurveyQuestionaries() {

            this.BeneficiarySectionQstnList = [];
            this.lstSectionsQstns = [];
            if (this.vmSelectedBeneficiaries.length > 0) {

                for (var BenCnt = 0; BenCnt <= this.vmSelectedBeneficiaries.length - 1; BenCnt++) {
                    var BenDetails = this.vmSelectedBeneficiaries[BenCnt];
                    var object = new Object();
                    object.SurveyId = this.SelectedTaskTypeID;
                    object.BeneficiaryID = BenDetails.BeneficiaryID;
                    object.BeneficiaryName = BenDetails.BeneficiaryName;
                    object.BenefIndex = BenCnt + 1;
                    object.Sections = await this.GetSectionsForEachBeneficiary(object.BeneficiaryID, object.BenefIndex);

                    this.BeneficiarySectionQstnList.push(object);
                }
            }

            this.lstSectionsQstns = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;

        },

        async GetSectionsForEachBeneficiary(BeneficiaryID, BenefIndex) {

            this.lstSections = this.lstSelectedTaskData.TaskData.SectionList;
            var SectionList = [];
            for (var SectionCount = 0; SectionCount <= this.lstSections.length - 1; SectionCount++) {

                this.QstnsPerBeneficiary = this.lstSelectedTaskData.TaskData.SurveyQstnList.length;
                var SurveySectionQuestions = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => (x.SectionID == this.lstSections[SectionCount].SectionID));
                if (SurveySectionQuestions.length > 0) {

                    var object = new Object();
                    object.BeneficiaryID = BeneficiaryID;
                    object.BenefIndex = BenefIndex;
                    object.SurveyId = this.lstSections[SectionCount].SurveyID;
                    object.SectionID = this.lstSections[SectionCount].SectionID;
                    object.SectionName = this.lstSections[SectionCount].SectionName;
                    object.SectionDescription = this.lstSections[SectionCount].SectionDescription;
                    object.SectionOrder = this.lstSections[SectionCount].SectionOrder;
                    object.IslogicSection = this.lstSections[SectionCount].IslogicSection;
                    object.Questions = await this.GetSectionQuestions(object.SectionID, BeneficiaryID, BenefIndex);

                    SectionList.push(object);
                }
            }
            return SectionList;
        },

        async GetSectionQuestions(SectionID, BeneficiaryId, BenefIndex) {

            if (this.lstSelectedTaskData.TaskData.SurveyQstnList.length > 0) {

                var SectionQuestionList = [];
                var currSecQstns = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => (x.SectionID == SectionID));
                var lstCascadingItems = this.lstSelectedTaskData.TaskData.CascadingList;
                for (var i = 0; i <= currSecQstns.length - 1; i++) {

                    var object = new Object();
                    object.BeneficiaryID = BeneficiaryId;
                    object.BenefIndex = BenefIndex;
                    object.ResponseID = BeneficiaryId
                    object.SurveyID = this.SelectedTaskTypeID;
                    object.SectionID = currSecQstns[i].SectionID;
                    object.SectionName = currSecQstns[i].SectionName;
                    object.SectionDescription = currSecQstns[i].SectionDescription;
                    object.SectionOrder = currSecQstns[i].SectionOrder;
                    object.QuestionOrder = currSecQstns[i].QuestionOrder;
                    object.QuestionTypeID = currSecQstns[i].QuestionTypeID;
                    object.QuestionID = currSecQstns[i].QuestionID;
                    object.QuestionName = currSecQstns[i].QuestionName;
                    object.SectionLogic = currSecQstns[i].SectionLogic;
                    object.QuestionType = currSecQstns[i].QuestionType;
                    object.IsFormulaQuestion = currSecQstns[i].IsFormulaQuestion;
                    object.IsConditionQuestion = currSecQstns[i].IsConditionQuestion;
                    object.QuestionCondition = currSecQstns[i].QuestionCondition;
                    object.QuestionOptions = currSecQstns[i].QuestionOptions;
                    object.QuestionHint = currSecQstns[i].QuestionHint;
                    object.IsValidationQuestion = currSecQstns[i].IsValidationQuestion;
                    object.RegEx = currSecQstns[i].RegEx;
                    object.Answer = currSecQstns[i].Answer;
                    object.FieldVar = currSecQstns[i].FieldVar;
                    object.IsUnique = currSecQstns[i].IsUnique;
                    object.IsUpload = 0;//currSecQstns[i].IsUpload;
                    object.ErrorText = currSecQstns[i].ErrorText;
                    object.DefaultValue = currSecQstns[i].DefaultValue;
                    object.IsMandatory = currSecQstns[i].IsMandatory;
                    object.IsOtherOptionSelected = currSecQstns[i].IsOtherOptionSelected;
                    object.isOtherOptionEnabled = false;
                    object.OtherOptionAnswer = "";
                    object.IsSearchable = currSecQstns[i].IsSearchable;
                    object.QuestionCategory = currSecQstns[i].QuestionCategory;
                    object.UOM = currSecQstns[i].UOM;
                    object.FormulaOperation = currSecQstns[i].FormulaOperation;
                    object.FormulaExpression = currSecQstns[i].FormulaExpression;
                    object.FormulaFieldVariables = currSecQstns[i].FormulaFieldVariables;
                    object.CascadingQuestionOptions = currSecQstns[i].CascadingQuestionOptions;
                    object.IsValidationError = false;
                    object.ValidationErrorTest = "";
                    object.DateMenu = false;
                    object.DateMenu1 = false;
                    object.qRules = [];
                    object.uploadedSign = "";
                    object.img = '';
                    object.uploadedImg = '';
                    object.startCamera = false;
                    object.camOn = false;
                    object.signaturebutton = false;
                    object.uploadedFile = '';
                    object.textLength = 9999999
                    object.AnsBlob = [];

                    if (object.QuestionType == "Cascading Dropdown") {

                        if (lstCascadingItems.length > 0) {

                            // for (var QuestionCount = 0; QuestionCount <= object.CascadingQuestionOptions.length - 1; QuestionCount++)
                            // {
                            //   var QItem = vaCascadingQuestion[QuestionCount];
                            if (object.CascadingQuestionOptions != null) {
                                for (var nCnt = 0; nCnt < object.CascadingQuestionOptions.length; nCnt++) {
                                    if (object.CascadingQuestionOptions[nCnt].OptionValue == "Country") {
                                        this.lstCountry = lstCascadingItems[0].Country.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                        object.CascadingQuestionOptions[nCnt].CascadingList = this.lstCountry;
                                    }
                                    else if (object.CascadingQuestionOptions[nCnt].OptionValue == "States") {
                                        this.lstStates = lstCascadingItems[0].States.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                        object.CascadingQuestionOptions[nCnt].CascadingList = this.lstStates;
                                    }
                                    else if (object.CascadingQuestionOptions[nCnt].OptionValue == "District") {
                                        this.lstDistrict = lstCascadingItems[0].District.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                        object.CascadingQuestionOptions[nCnt].CascadingList = this.lstDistrict;
                                    }
                                    else if (object.CascadingQuestionOptions[nCnt].OptionValue == "SubDistrict") {
                                        this.lstSubDistrict = lstCascadingItems[0].SubDistrict.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                        object.CascadingQuestionOptions[nCnt].CascadingList = this.lstSubDistrict;

                                    }

                                }
                            }
                            // }
                        }

                    }
                    SectionQuestionList.push(object);
                    this.audUpload[i] = 0;
                }
                SectionQuestionList.sort(function (a, b) {
                    return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder);
                });
                return SectionQuestionList;
            }

        },

        // to Change Status  of Any time Submitted Data
        async UpdateAnyTimeDataStatus() {
            try {
                if (this.lstAnyTimeNewSubmission.length > 0) {
                    var TaskSyncDb = await dbImworks.TaskSyncDB.toArray();
                    for (var i = 0; i <= this.lstAnyTimeNewSubmission.length - 1; i++) {
                        var data = this.lstAnyTimeNewSubmission[i].TaskData;
                        var dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == data.TypeID && x.Type == 'Survey' && x.TaskSubmittedDate == data.CreatedDate && x.WFLevelStatus != 'Saved');
                        if (dtMyTask.length > 0) {
                            var IsSyncing = TaskSyncDb.filter(x => x.TypeID == data.TypeID && x.Type == 'Survey' && x.TaskSubmittedDate == data.CreatedDate);
                            if (IsSyncing.length > 0) { data.CurrentOrOverdue = "Syncing"; }
                            else { data.CurrentOrOverdue = "Review" }
                        }
                    }
                }
            } catch (err) {
                console.log("UpdateAnyTimeDataStatus" + err);
            }
        },
        async fnGetLoggedUserDetails() {
            try {
                var userId = window.LoggedUserID;
                if (userId == undefined || userId == 0 || userId == "") {
                    var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
                    if (data != null && data.length > 0) {
                        userId = data[0].UserId;
                    }
                }

                var loggeduserdata = await dbImworks.TbLoggedUserDetails.where("ID").equals(window.LoggedUserID).toArray();
                if (loggeduserdata != null && loggeduserdata.length > 0) {
                    this.LoggedUserID = loggeduserdata[0].ID;
                }
            } catch (err) {
                console.log("fnGetLoggedUserDetails" + err)
            }
        },
        // Get Non Beneficiary Linked Survey Question details
        async GetNonBeneficiayBasedSectionQuestions() {
            this.PreLoader = true;
            this.lstSectionsQstns = await this.GetSectionsForEachBeneficiary(0, 0);
            // await this.fnTriggerSkipOrRepeatLogic();
            this.PreLoader = false;

        },

        //=============== Survey Import ====================== //

        async fnResetQuestionsAns() {

            this.BenefLinkedSurveySteps = 2;
        },

        async GetBenefSurveyQstnsForImport() {

            var lstSurveyQstns = [];
            lstSurveyQstns = this.lstSelectedTaskData.TaskData.SurveyQstnList != null ? this.lstSelectedTaskData.TaskData.SurveyQstnList : [];

            if (lstSurveyQstns.length == 0) {
                this.SnackbarMsg = "Survey Sections and Questionnaire are not available...";
                this.InfoSnackbarMsg = true;
            }
            else {
                this.lstImportSurveyQstnHearder = [];
                for (var i = 0; i < lstSurveyQstns.length; i++) {

                    if (lstSurveyQstns[i].QuestionType == "Date Picker") {
                        this.lstImportSurveyQstnHearder.push({
                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            type: "date",
                            //mandatory:this.lstSurveyQstns[i].IsMandatory,
                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,
                        });
                    }
                    else if (lstSurveyQstns[i].QuestionType == "Numeric") {
                        this.lstImportSurveyQstnHearder.push({
                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            type: "number",
                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,
                        });
                    }
                    else if (lstSurveyQstns[i].QuestionType == 'Single Choice' || lstSurveyQstns[i].QuestionType == 'Multi-Choice' || lstSurveyQstns[i].QuestionType == 'Dropdown' || lstSurveyQstns[i].QuestionType == 'Likert Scale') {
                        var optionlist = [];
                        for (var j = 0; j < lstSurveyQstns[i].QuestionOptions.length; j++) {
                            optionlist.push(lstSurveyQstns[i].QuestionOptions[j].OptionValue)
                        }
                        this.lstImportSurveyQstnHearder.push({
                            text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                            type: "select",
                            mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                            options: optionlist,
                            QuestionCondition: lstSurveyQstns[i].QuestionCondition,

                        });
                    }
                    else {
                        if (lstSurveyQstns[i].QuestionType != 'Note') {
                            this.lstImportSurveyQstnHearder.push({
                                text: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                value: lstSurveyQstns[i].QuestionName.concat(' ', lstSurveyQstns[i].QuestionHint),
                                type: "string",
                                mandatory: lstSurveyQstns[i].IsMandatory == 1 ? 'Yes' : 'No',
                                QuestionCondition: lstSurveyQstns[i].QuestionCondition,

                            });

                        }
                    }
                }
            }
            this.BenefLinkedSurveySteps = 7;
            this.NonBenefLinkedSurveySteps = 2;
        },

        //************Benificiary************//
        async fnSearchBeneficiaries() {

            await this.GetBeneficiary();
            this.BenefLinkedSurveySteps = 2;
            //if (this.SearchExistingBeneficiary) {

            //    await this.GetBeneficiary();
            //    this.BenefLinkedSurveySteps = 2;
            //} else {
            //    // Add New Beneficiary Step
            //    this.BenefLinkedSurveySteps = 3;
            //}
        },


        // Calculate Task percentage for Approver side
        async fnCalcProgressApprover() {
            var NoOfBenfList = [];
            var AnswerCount = [];
            var totalQuestionCount = 0;
            if (this.IsBeneficiaryLinkedSurvey == true) {
                if (this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList == null || this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList == "undefined") {

                    var TaskSubmittedList = await dbImworks.TbMyTask.toArray();
                    var dtMyTask = TaskSubmittedList.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.SyncDBID == this.lstSelectedTaskData.SyncDBID);
                    if (dtMyTask.length > 0 && typeof dtMyTask[0].SurveyAnswers != "undefined") {
                        var BeneficiarySubmittedList = JSON.parse(dtMyTask[0].BeneficiarySubmittedList);
                        var SurveyAnswerList = JSON.parse(dtMyTask[0].SurveyAnswers);
                        NoOfBenfList = SurveyAnswerList.map(function (item) {
                            return item.ResponseID;
                        });
                        AnswerCount = SurveyAnswerList.filter(x => x.Answer != "");
                        totalQuestionCount = SurveyAnswerList.length;
                    }
                    else {
                        AnswerCount = this.lstSelectedTaskData.AnswerCount;
                        totalQuestionCount = (this.lstSelectedTaskData.QuestionCount * this.lstSelectedTaskData.BeneficiaryCount);

                    }
                }
                else {
                    NoOfBenfList = this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList.map(function (item) {
                        return item.BeneficiaryID;
                    });

                    AnswerCount = this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList.filter(x => x.Answer != "");
                    var NoteQuestions = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.QuestionType == "Note")

                    let SubmittedQstnList = [];
                    SubmittedQstnList = this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList.filter(el => {
                        return !NoteQuestions.find(element => {
                            return element.QuestionID === el.QuestionID;
                        });
                    });

                    totalQuestionCount = SubmittedQstnList.length;//this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList.length;
                }
                this.NoOfSelectedBen = "";
                this.NoOfSelectedBen = this.lstSelectedTaskData.BeneficiaryCount;
                if (this.NoOfSelectedBen == 0) {
                    // Now we will use set to remove duplicate value from array
                    var setOfValue = new Set(NoOfBenfList);
                    // Now we will convert set to array using spread operator
                    var NoOfSelectedBen = [...setOfValue];
                    this.NoOfSelectedBen = NoOfSelectedBen.length;
                }
                this.QstnsPerBeneficiary = totalQuestionCount;
                AnswerCount = AnswerCount.length > 0 ? AnswerCount.length : AnswerCount;
                if (AnswerCount > 0 && totalQuestionCount > 0) {
                    this.CompletionPerc = Math.round((AnswerCount / totalQuestionCount) * 100);
                } else {
                    this.CompletionPerc = 0;
                }
            }
            else {
                this.lstSectionsQstns = [];
                this.lstSectionsQstns = await this.GetSectionsForEachBeneficiary(0, 0);
                var AnsweredsQstnCnt = 0;
                var SurveySectionQuestions = [];
                var questionPerSub = 0;
                var ResponseCount = 1;
                var TaskSubmittedList = await dbImworks.TbMyTask.toArray();
                // while syncing
                if (TaskSubmittedList.length > 0) {

                    var dtMyTask = [];
                    dtMyTask = TaskSubmittedList.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.SyncDBID == this.lstSelectedTaskData.SyncDBID);

                    if (dtMyTask.length > 0 && typeof dtMyTask[0].SurveyAnswers != "undefined") {
                        var SurveyAnswers = JSON.parse(dtMyTask[0].SurveyAnswers);

                        AnsweredsQstnCnt = SurveyAnswers.filter(x => x.Answer != "").length;
                        totalQuestionCount = dtMyTask[0].QuestionCount;
                        questionPerSub = totalQuestionCount;
                    }
                    else {
                        //var ResponseCnt = this.lstSelectedTaskData.TaskData.SurveyResponse;
                        var ResponseCnt = this.lstSelectedTaskData.ResponseCount;
                        var QuestionCnt = this.lstSelectedTaskData.TaskData.SurveyQstnList;
                        if (ResponseCnt.length > QuestionCnt.length) // case of import
                        {
                            AnsweredsQstnCnt += parseInt(this.lstSelectedTaskData.TaskData.SurveyResponse.filter(x => x.Answer != "").length);
                            totalQuestionCount = this.lstSelectedTaskData.TaskData.SurveyResponse.length;
                            questionPerSub = QuestionCnt.length;
                            if (questionPerSub > 0 && totalQuestionCount > 0) {
                                ResponseCount = parseInt(totalQuestionCount / questionPerSub)
                            }
                        }
                        else {
                            AnsweredsQstnCnt += parseInt(this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.Answer1 != "").length);
                            totalQuestionCount = this.lstSelectedTaskData.TaskData.SurveyResponse.length;
                            questionPerSub = totalQuestionCount;
                            ResponseCount = 1;
                        }
                    }
                }
                else {  // after syncing
                    AnsweredsQstnCnt = this.lstSelectedTaskData.AnswerCount;
                    ResponseCount = this.lstSelectedTaskData.ResponseCount;
                    questionPerSub = this.lstSelectedTaskData.QuestionCount;
                    if (ResponseCount > 1) // case of import
                    {
                        totalQuestionCount = Math.round(questionPerSub * ResponseCount);
                    }
                    else {
                        totalQuestionCount = questionPerSub;
                    }

                }
                this.TotalQuestions = questionPerSub;//this.lstSelectedTaskData.TaskData.SurveyResponse.length;
                this.Responses = ResponseCount;
                if (totalQuestionCount > 0 && AnsweredsQstnCnt > 0) {
                    this.CompletionPerc = Math.round((AnsweredsQstnCnt / totalQuestionCount) * 100);
                }
                else {
                    this.CompletionPerc = 0;
                }

            }
        },
        async fnCalculateProgress() {

            this.CompletionPerc = 0;
            this.TotalQstnsPerBeneficiary = 0;
            this.QstnsPerBeneficiary = 0;
            if (this.IsImportSurvey == false) {

                if (this.IsBeneficiaryLinkedSurvey == true) {
                    var SelectedBenefCount = this.vmSelectedBeneficiaries.length;
                    var TotalQuestionsWithoutLogic = 0;
                    var TotalQuestionsWithLogic = 0;
                    var AnsweredsQstnCnt = 0;
                    var isMultichoiceAnswered = false;
                    var isCaseCadingAnswered = false;
                    for (var bCnt = 0; bCnt <= this.BeneficiarySectionQstnList.length - 1; bCnt++) {
                        var Sections = this.BeneficiarySectionQstnList[bCnt].Sections;
                        for (var i = 0; i <= Sections.length - 1; i++) {
                            if (Sections[i].IslogicSection === 0) {
                                TotalQuestionsWithoutLogic += parseInt(Sections[i].Questions.filter(x => x.QuestionType != 'Note').length)
                                //AnsweredsQstnCnt += parseInt(Sections[i].Questions.filter(x => x.Answer != "").length);
                                for (var j = 0; j < Sections[i].Questions.length; j++) {

                                    if (Sections[i].Questions[j].QuestionType == 'Cascading Dropdown') {

                                        if (Sections[i].Questions[j].CascadingQuestionOptions.length > 0) {

                                            var CascadingOption = Sections[i].Questions[j].CascadingQuestionOptions
                                            for (var k = 0; k <= CascadingOption.length - 1; k++) {
                                                if (Sections[i].Questions[j].CascadingQuestionOptions[k][0] !== null) {
                                                    isCaseCadingAnswered = true;
                                                    break;
                                                } else {
                                                    continue;
                                                }
                                            }
                                        }
                                        if (isCaseCadingAnswered) {
                                            AnsweredsQstnCnt++
                                            isCaseCadingAnswered = false;
                                        } else {
                                            continue;
                                        }
                                    }
                                    else if (Sections[i].Questions[j].QuestionType == 'Multi-Choice') {
                                        for (var k = 0; k <= Sections[i].Questions[j].QuestionOptions.length - 1; k++) {
                                            if (Sections[i].Questions[j].QuestionOptions[k].IsOptSelected == true) {
                                                isMultichoiceAnswered = true;
                                                break;
                                            } else {
                                                continue;
                                            }
                                        }
                                        if (isMultichoiceAnswered) {
                                            AnsweredsQstnCnt++
                                            isMultichoiceAnswered = false;
                                        } else {
                                            continue;
                                        }
                                    } else {
                                        if (Sections[i].Questions[j].Answer != "" && Sections[i].Questions[j].QuestionType != "Note") {
                                            AnsweredsQstnCnt++
                                        } else {
                                            continue;
                                        }
                                    }
                                }
                            } else {
                                continue;
                            }
                        }
                    }
                    var TotalNotNasRepeatOrSkipSection = this.RepeatOrSkipSection.filter(x => x.Answer === null || x.Answer === "").length
                    TotalQuestionsWithLogic += Math.abs(this.RepeatOrSkipSection.length - TotalNotNasRepeatOrSkipSection);
                    this.TotalQstnsPerBeneficiary = TotalQuestionsWithoutLogic + (this.RepeatOrSkipSection.filter(x => x.Answer !== null).length)
                    this.CompletionPerc = Math.round(((AnsweredsQstnCnt + TotalQuestionsWithLogic) / this.TotalQstnsPerBeneficiary) * 100);
                    this.QstnsPerBeneficiary = this.TotalQstnsPerBeneficiary;
                }
                else {

                    var NonBenefTotalQuestionsWithoutLogic = 0;
                    var NonBenefTotalQuestionsWithLogic = 0;
                    var NonBenefAnsweredsQstnCnt = 0;
                    var isMultichoiceAnswered = false;
                    var isCaseCadingAnswered = false;
                    for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                        if (this.lstSectionsQstns[i].IslogicSection === 0) {
                            NonBenefTotalQuestionsWithoutLogic += parseInt(this.lstSectionsQstns[i].Questions.filter(x => x.QuestionType != 'Note').length)
                            //NonBenefAnsweredsQstnCnt += parseInt(this.lstSectionsQstns[i].Questions.filter(x => x.Answer != "").length);
                            for (var j = 0; j < this.lstSectionsQstns[i].Questions.length; j++) {
                                if (this.lstSectionsQstns[i].Questions[j].QuestionType == 'Cascading Dropdown') {

                                    if (this.lstSectionsQstns[i].Questions[j].CascadingQuestionOptions.length > 0) {

                                        var CascadingOption = this.lstSectionsQstns[i].Questions[j].CascadingQuestionOptions
                                        for (var k = 0; k <= CascadingOption.length - 1; k++) {
                                            if (this.lstSectionsQstns[i].Questions[j].CascadingQuestionOptions[k][0] != null) {
                                                isCaseCadingAnswered = true;
                                                break;
                                            } else {
                                                continue;
                                            }

                                        }
                                    }
                                    if (isCaseCadingAnswered) {
                                        NonBenefAnsweredsQstnCnt++
                                        isCaseCadingAnswered = false;
                                    } else {
                                        continue;
                                    }
                                }
                                else if (this.lstSectionsQstns[i].Questions[j].QuestionType == 'Multi-Choice') {
                                    for (var k = 0; k <= this.lstSectionsQstns[i].Questions[j].QuestionOptions.length - 1; k++) {
                                        if (this.lstSectionsQstns[i].Questions[j].QuestionOptions[k].IsOptSelected == true) {
                                            isMultichoiceAnswered = true;
                                            break;
                                        } else {
                                            continue;
                                        }
                                    }
                                    if (isMultichoiceAnswered) {
                                        NonBenefAnsweredsQstnCnt++
                                        isMultichoiceAnswered = false;
                                    } else {
                                        continue;
                                    }
                                } else {
                                    if (this.lstSectionsQstns[i].Questions[j].Answer != "" && this.lstSectionsQstns[i].Questions[j].QuestionType != "Note") {
                                        NonBenefAnsweredsQstnCnt++
                                    } else {
                                        continue;
                                    }
                                }
                            }
                        } else {
                            continue;
                        }
                    }

                    var TotalNotNasRepeatOrSkipSection = this.RepeatOrSkipSection.filter(x => x.Answer === null || x.Answer === "").length
                    NonBenefTotalQuestionsWithLogic += Math.abs(this.RepeatOrSkipSection.length - TotalNotNasRepeatOrSkipSection);
                    this.NonBenefTotalQuestion = NonBenefTotalQuestionsWithoutLogic + (this.RepeatOrSkipSection.filter(x => x.Answer !== null).length)
                    this.NonBenefCompletionPerc = Math.round(((NonBenefAnsweredsQstnCnt + NonBenefTotalQuestionsWithLogic) / (this.NonBenefTotalQuestion)) * 100);
                    this.CompletionPerc = this.NonBenefCompletionPerc;
                    this.TotalQuestions = this.NonBenefTotalQuestion;
                    this.Responses = 1;

                }
            }
            else {
                NonBenefAnsweredsQstnCnt = this.lstBenefSurveyQstnAnswers.filter(x => x.Answer != "").length;
                this.NonBenefTotalQuestion = this.lstBenefSurveyQstnAnswers.length;
                this.QstnsPerBeneficiary = this.lstBenefSurveyQstnAnswers.length;
                this.NonBenefCompletionPerc = Math.round(((NonBenefAnsweredsQstnCnt) / (this.NonBenefTotalQuestion)) * 100);
                this.CompletionPerc = this.NonBenefCompletionPerc;
                //this.TotalQuestions = this.NonBenefTotalQuestion;
                var questionCount = 0;
                for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                    questionCount += this.lstSectionsQstns[i].Questions.filter(x => x.QuestionType != 'Note').length;
                }
                this.TotalQuestions = questionCount;
                this.Responses = Math.round(this.NonBenefTotalQuestion / questionCount);

            }
        },
        async GetBeneficiary() {
            // Get Beneficiary list based on Type and Type ID
            this.lstSelectedBeneficiary = [];
            if (this.lstBeneficiary.length == 0) {
                this.BeneficiariesWithName = this.lstSelectedTaskData.TaskData.BeneficiaryList;
                var BeneficiaryQuestions = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns
                BeneficiaryQuestions.sort(function (a, b) {
                    return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder);
                })
                if (this.lstSelectedTaskData.TaskData.BeneficiaryDetails == null) {
                    this.lstSelectedTaskData.TaskData.BeneficiaryDetails = [];
                }

                this.BeneficiaryTableheaders = [];
                this.BeneficiaryTableheaders.push({ text: 'CreatedDate', value: 'CreatedDate' });
                for (var i = 0; i <= BeneficiaryQuestions.length - 1; i++) {

                    if (BeneficiaryQuestions[i].QuestionType != 'Note') {
                        this.BeneficiaryTableheaders.push({ text: BeneficiaryQuestions[i].QuestionName, value: BeneficiaryQuestions[i].QuestionName });
                    }
                }

                var obj = new Object();
                var lstBen = [];
                //var lBeneficiaryList = this.lstSelectedTaskData.BeneficiaryDetails;
                // Filling Beneficiaries
                var lstBeneficiryID = this.lstSelectedTaskData.TaskData.BeneficiaryDetails.map(function (item) {
                    return item.BeneficiaryID;
                });

                // Now we will use set to remove duplicate value from array
                var setOfValue = new Set(lstBeneficiryID);
                // Now we will convert set to array using spread operator
                var uniqueValues = [...setOfValue];

                for (var header = 0; header <= uniqueValues.length - 1; header++) {
                    obj = new Object();
                    var temp = this.lstSelectedTaskData.TaskData.BeneficiaryDetails.filter(x => x.BeneficiaryID == uniqueValues[header]);
                    for (var row = 0; row <= temp.length - 1; row++) {
                        obj[temp[row].QuestionName] = temp[row].Answer;
                        obj['CreatedDate'] = objUtils.common.formatDate(temp[row].CreatedDate);
                    }
                    obj["BeneficiaryID"] = uniqueValues[header];
                    obj["TokenValue"] = temp[0].TokenValue;
                    lstBen.push(obj);
                }

                this.lstBeneficiary = lstBen;

            }

            if (this.filterNewBenefList.length > 0) {
                for (var BenfCnt = 0; BenfCnt <= this.filterNewBenefList.length - 1; BenfCnt++) {
                    var tempdata = this.lstBeneficiary.filter(x => x.BeneficiaryID == this.filterNewBenefList[BenfCnt].BeneficiaryID || x.TokenValue == this.filterNewBenefList[BenfCnt].TokenValue)

                    if (tempdata.length == 0) {


                        var BenfDetails = JSON.parse(this.filterNewBenefList[BenfCnt].BeneficiaryQstnAnswers);
                        var obj = new Object();
                        obj = new Object();

                        for (var Cnt = 0; Cnt <= BeneficiaryQuestions.length - 1; Cnt++) {
                            var temp = BenfDetails.filter(x => x.QuestionID == BenfDetails[Cnt].QuestionID);
                            if (temp.length > 0) {
                                obj[BeneficiaryQuestions[Cnt].QuestionName] = temp[0].Answer;
                                obj["CreatedDate"] = objUtils.common.formatNewDate(temp[0].CreatedDate);
                            }
                        }
                        obj["BeneficiaryID"] = this.filterNewBenefList[BenfCnt].BeneficiaryID;
                        obj["TokenValue"] = this.filterNewBenefList[BenfCnt].TokenValue;

                        //lstBen.push(obj);
                        // Adding new beneficiary to existing beneficiary list
                        this.lstBeneficiary.unshift(obj);
                    }
                }
            }

            //var lstUnSavedBeneficiary = this.UnSavedNewBenfList.filter(x => x.BeneficiaryTypeID == '' && SurveyID)

            //this.BenefLinkedSurveySteps = 2;
        },

        async nextBeneficiary() {
            if (this.BenefStepper < this.vmSelectedBeneficiaries.length) {

                if (await this.fnCheckMandatory()) {
                    this.beneficiaryStepperList.push(this.BenefStepper);
                    await this.fnCalculateProgress();
                    await this.fnStoreCurrentBenefQstnAns();
                    this.BenefStepper++;
                    await this.GetCurrentBenefSectionsQstns();
                    await this.fnPreSavedBenefQstnAns();
                    await this.fnTriggerSkipOrRepeatLogic();

                }
            }
            this.scrollToNavbarTop();
        },

        async scrollToTop(item) {
            if (item.length > 0) {
                for (var sCnt = 0; sCnt <= item.length - 1; sCnt++) {
                    var Questions = item[sCnt].Questions;
                    for (var i = 0; i < Questions.length; i++) {
                        if (Questions[i].ValidationErrorTest == "Required Field") {
                            document.getElementById(Questions[i].QuestionID).scrollIntoView()
                            break;
                        } else {
                            document.getElementById("scrollToTop").scrollIntoView()
                        }
                    }

                }
            }
        },

        // To Store Current Beneficiary Data before moving to Next beneficiary
        async fnStoreCurrentBenefQstnAns() {
            // To Store Question Answer to Before moving to Next beneficiary
            var Ans = "";
            var blobFile = "";
            var Sections = [];
            var BenefID = 0;
            if (this.IsBeneficiaryLinkedSurvey == true) {
                Sections = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections.filter(y => y.IslogicSection == 0);
                BenefID = this.vmSelectedBeneficiaries[this.BenefStepper - 1].BeneficiaryID;
            } else {
                Sections = this.lstSectionsQstns.filter(x => x.IslogicSection == 0);
            }
            for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                var lstQuestion = Sections[SecCnt].Questions;
                for (var QstCnt = 0; QstCnt <= lstQuestion.length - 1; QstCnt++) {

                    if (lstQuestion[QstCnt].QuestionType == 'Cascading Dropdown') {

                        if (this.cascadingAnsArray.length > 0) {
                            Ans = "";
                            var cascadingAns = this.cascadingAnsArray.filter(x => (x.QID == lstQuestion[QstCnt].QuestionID) && (x.IsBenefQstn == false) && (x.benefIndx == this.BenefStepper));

                            for (var j = 0; j < cascadingAns.length; j++) {
                                Ans = (Ans != '') ? Ans + ',' + cascadingAns[j].Ans : cascadingAns[j].Ans;
                            }

                            lstQuestion[QstCnt].Answer = Ans;
                        }
                    }
                    if (lstQuestion[QstCnt].QuestionType == 'Multi-Choice') {

                        Ans = "";
                        var ChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == lstQuestion[QstCnt].QuestionID && x.benefIndex == this.BenefStepper);
                        for (var mCnt = 0; mCnt < ChoiceAns.length; mCnt++) {

                            if (lstQuestion[QstCnt].IsOtherOptionSelected == true && ChoiceAns[mCnt].OptionValue.toUpperCase() == "OTHERS") {
                                Ans = (Ans != '') ? Ans + ',' + lstQuestion[QstCnt].OtherOptionAnswer : lstQuestion[QstCnt].OtherOptionAnswer;
                            }
                            else {
                                Ans = (Ans != '') ? Ans + ',' + ChoiceAns[mCnt].OptionValue : ChoiceAns[mCnt].OptionValue;
                            }
                        }
                        lstQuestion[QstCnt].Answer = Ans;
                    }
                    if (lstQuestion[QstCnt].QuestionType == 'Single Choice' || lstQuestion[QstCnt].QuestionType == 'Dropdown') {
                        if (lstQuestion[QstCnt].isOtherOptionEnabled == true) {
                            Ans = lstQuestion[QstCnt].OtherOptionAnswer;
                            lstQuestion[QstCnt].Answer = Ans;
                        }

                    }
                    if ((lstQuestion[QstCnt].QuestionType == "Audio Capture") || (lstQuestion[QstCnt].QuestionType == "Signature Capture")
                        || (lstQuestion[QstCnt].QuestionType == "Photo Capture") || (lstQuestion[QstCnt].QuestionType == "File Upload")) {
                        if (this.AnsBlob.length > 0) {
                            blobFile = this.AnsBlob.filter(x => (x.QuestionID == lstQuestion[QstCnt].QuestionID) && (x.benefIndex == this.BenefStepper))[0];
                            if (typeof blobFile != "undefined") {
                                await this.FileUpload(blobFile.blobData, lstQuestion[QstCnt].Answer, false, blobFile.IsUploadedFile, Sections[SecCnt].BeneficiaryID, lstQuestion[QstCnt].QuestionID);
                            }
                        }
                    }

                }

            }

            //Store Repeat Or Skip Section Questions Answers
            // Repeat Section Questions and Answers

            var RepeatSection = this.RepeatOrSkipSection.filter(x => x.benefIndex == this.BenefStepper)

            for (var Rcount = 0; Rcount < RepeatSection.length; Rcount++) {
                Ans = "";
                if (RepeatSection[Rcount].QuestionID > 0) {

                    if (RepeatSection[Rcount].QuestionType == 'Multi-Choice') {
                        var ChoiceAns = this.MultiChoiceAnsForRepeat.filter(x => x.QuestionID == RepeatSection[Rcount].QuestionID && x.RQstInd == RepeatSection[Rcount].RQstInd);
                        for (var j = 0; j < ChoiceAns.length; j++) {



                            if (RepeatSection[Rcount].IsOtherOptionSelected == true && ChoiceAns[j].OptionValue.toUpperCase() == "OTHERS") {
                                Ans = (Ans != '') ? Ans + ',' + RepeatSection[Rcount].OtherOptionAnswer : RepeatSection[Rcount].OtherOptionAnswer;
                            }
                            else {
                                Ans = (Ans != '') ? Ans + ',' + ChoiceAns[j].OptionValue : ChoiceAns[j].OptionValue;
                            }


                        }
                        RepeatSection[Rcount].Answer = Ans;
                    }
                    else if (RepeatSection[Rcount].QuestionType == 'Single Choice' || RepeatSection[Rcount].QuestionType == 'Dropdown') {
                        if (RepeatSection[Rcount].isOtherOptionEnabled == true) {
                            Ans = RepeatSection[Rcount].OtherOptionAnswer;
                            RepeatSection[Rcount].Answer = Ans;
                        }

                    }
                    else if (RepeatSection[Rcount].QuestionType == "Cascading Dropdown") {
                        this.cascadingAns = this.cascadingAnsArray.filter(x => (x.QID == RepeatSection[Rcount].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == this.BenefStepper));

                        for (var j = 0; j < this.cascadingAns.length; j++) {
                            Ans = (Ans != '') ? Ans + ',' + this.cascadingAns[j].Ans : this.cascadingAns[j].Ans;
                        }
                        RepeatSection[Rcount].Answer = Ans;
                    }

                    else if ((RepeatSection[Rcount].QuestionType == "Audio Capture") || (RepeatSection[Rcount].QuestionType == "Signature Capture")
                        || (RepeatSection[Rcount].QuestionType == "Photo Capture") || (RepeatSection[Rcount].QuestionType == "File Upload")) {
                        if (this.AnsBlob.length > 0) {
                            blobFile = this.AnsBlob.filter(x => (x.QuestionID == RepeatSection[Rcount].QuestionID) && (x.benefIndex == this.BenefStepper))[0];
                            if (typeof blobFile != "undefined") {
                                this.FileUpload(blobFile.blobData, blobFile.blobFileName, false, blobFile.IsUploadedFile, this.BenefStepper, RepeatSection[Rcount].QuestionID);
                            }
                        }
                        Ans = this.RepeatOrSkipSection[Rcount].Answer;

                    }
                    else {
                        Ans = RepeatSection[Rcount].Answer;
                    }

                    // RepeatSection[Rcount].Answer = Ans;
                }
            }
        },
        async fnPreSavedNonBenefQstnAns() {
            var Sections = this.lstSectionsQstns.filter(x => x.IslogicSection == 0);
            for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                var lstQuestion = Sections[SecCnt].Questions;
                for (var QstCnt = 0; QstCnt <= lstQuestion.length - 1; QstCnt++) {
                    if (lstQuestion[QstCnt].QuestionType == 'Single Choice' || lstQuestion[QstCnt].QuestionType == 'Dropdown') {
                        if (lstQuestion[QstCnt].isOtherOptionEnabled == true) {
                            lstQuestion[QstCnt].Answer = "Others";
                        }
                    }
                }
            }
        },
        async fnPreSavedBenefQstnAns() {
            var AnsOptions = "";
            var QuestionID = 0;

            var Sections = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections

            for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                var lstQuestion = Sections[SecCnt].Questions;
                for (var QstCnt = 0; QstCnt <= lstQuestion.length - 1; QstCnt++) {
                    if (lstQuestion[QstCnt].QuestionType == 'Cascading Dropdown') {

                        AnsOptions = lstQuestion[QstCnt].Answer.split(',');
                        if (lstQuestion[QstCnt].Answer.length > 0) {
                            for (var j = 0; j < AnsOptions.length; j++) {
                                var pID = 0;
                                if (lstQuestion[QstCnt].CascadingQuestionOptions[j].CascadingList != null)
                                    pID = lstQuestion[QstCnt].CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                lstQuestion[QstCnt].CascadingQuestionOptions[j][j] = pID;
                                this.FilterCascadingItems(lstQuestion[QstCnt], lstQuestion[QstCnt].CascadingQuestionOptions[j], pID, false, this.BenefStepper, -1);
                            }
                        }

                    }

                    if (lstQuestion[QstCnt].QuestionType == 'Multi-Choice') {
                        var AnsOptions = lstQuestion[QstCnt].Answer.split(',');
                        // this.multiChoiceAns = AnsOptions;
                        if (lstQuestion[QstCnt].Answer.length > 0) {
                            for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                if (lstQuestion[QstCnt].QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt]).length > 0) {
                                    lstQuestion[QstCnt].QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                }
                                else {
                                    if (lstQuestion[QstCnt].IsOtherOptionSelected == true && lstQuestion[QstCnt].isOtherOptionEnabled == true) {
                                        lstQuestion[QstCnt].QuestionOptions.filter(x => x.OptionValue == "Others")[0].IsOptSelected = true;
                                        await this.ShowOtherOption(lstQuestion[QstCnt], "OTHERS", true)
                                    }
                                }

                            }
                        }
                        //lstQuestion[QstCnt].Answer = "";
                    }
                    if (lstQuestion[QstCnt].QuestionType == 'Single Choice' || lstQuestion[QstCnt].QuestionType == 'Dropdown') {
                        if (lstQuestion[QstCnt].isOtherOptionEnabled == true) {
                            lstQuestion[QstCnt].Answer = "Others";

                        }
                    }

                    if ((lstQuestion[QstCnt].QuestionType == "Photo Capture") || (lstQuestion[QstCnt].QuestionType == "Signature Capture") ||
                        (lstQuestion[QstCnt].QuestionType == "File Upload") || (lstQuestion[QstCnt].QuestionType == "Audio Capture")) {


                        var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(lstQuestion[QstCnt].Answer).toArray();

                        var blbFile = "";
                        var ImageSrc = "";
                        if (typeof ansBlobData[0] != "undefined") {
                            blbFile = ansBlobData[0].AttachedDocument;
                        }
                        if (blbFile != "") {
                            ImageSrc = URL.createObjectURL(blbFile);
                        }
                        // To Read Photo
                        if (lstQuestion[QstCnt].QuestionType == "Photo Capture" && blbFile != "") {

                            //var UploadIndex = benefSurveyQstn.filter(x => x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                            lstQuestion[QstCnt].IsUpload = 1;
                            this.audUpload[QstCnt] = 1;
                            this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecCnt].Questions.filter(y => y.QuestionID == QuestionID)[0].uploadedFile = ImageSrc;
                            this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecCnt].Questions.filter(y => y.QuestionID == QuestionID)[0].img = ImageSrc;

                        }
                        if (lstQuestion[QstCnt].QuestionType == "File Upload" && blbFile != "") {

                            if (typeof ansBlobData[0].QuestionID != 'undefined') {
                                QuestionID = ansBlobData[0].QuestionID;


                                //this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecCnt].Questions.filter(y => y.QuestionID == QuestionID)[0].uploadedFile = ImageSrc;
                                this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecCnt].Questions[QstCnt].uploadedFile = ImageSrc;


                            }
                        }
                    }
                    //lstQuestion[QstCnt].Answer = Answer;
                }

            }
            // Repeat Section Questions and Answers
            var RepeatSection = this.RepeatOrSkipSection.filter(x => x.benefIndex == this.BenefStepper)
            var Ans = "";
            for (var Rcount = 0; Rcount < RepeatSection.length; Rcount++) {
                Ans = "";
                if (RepeatSection[Rcount].QuestionID > 0) {

                    if (RepeatSection[Rcount].QuestionType == 'Multi-Choice') {
                        var AnsOptions = RepeatSection[Rcount].Answer.split(',');
                        // this.multiChoiceAns = AnsOptions;
                        if (RepeatSection[Rcount].Answer.length > 0) {
                            for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                if (RepeatSection[Rcount].QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt]).length > 0) {
                                    RepeatSection[Rcount].QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                }
                                else {
                                    if (RepeatSection[Rcount].IsOtherOptionSelected == true && RepeatSection[Rcount].isOtherOptionEnabled == true) {
                                        RepeatSection[Rcount].QuestionOptions.filter(x => x.OptionValue == "Others")[0].IsOptSelected = true;
                                        await this.ShowOtherOption(RepeatSection[Rcount], "OTHERS", true)
                                    }
                                }

                            }
                        }
                    }
                    else if (RepeatSection[Rcount].QuestionType == 'Single Choice' || RepeatSection[Rcount].QuestionType == 'Dropdown') {
                        if (RepeatSection[Rcount].isOtherOptionEnabled == true) {
                            RepeatSection[Rcount].Answer = "Others";

                        }

                    }

                    // RepeatSection[Rcount].Answer = Ans;
                }
            }


        },

        async previousBeneficiary() {
            if (this.BenefStepper > 1) {

                if (await this.fnCheckMandatory()) {
                    await this.fnCalculateProgress()
                    await this.fnStoreCurrentBenefQstnAns();

                    this.BenefStepper--;
                    await this.GetCurrentBenefSectionsQstns();

                    await this.fnPreSavedBenefQstnAns();


                }
            }
            this.scrollToNavbarTop();

        },

        async GetCurrentBenefSectionsQstns() {

            this.lstSectionsQstns = [];
            this.lstSectionsQstns = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;

            await this.scrollToTop(this.lstSectionsQstns);
            // Clearing Cascading Questions and Multi-Choice Questions\
            for (var SecCnt = 0; SecCnt <= this.lstSectionsQstns.length - 1; SecCnt++) {

                var SectionQuestions = this.lstSectionsQstns[SecCnt].Questions;

                for (var j = 0; j < SectionQuestions.length; j++) {

                    if (SectionQuestions[j].QuestionType == 'Cascading Dropdown') {

                        if (SectionQuestions[j].CascadingQuestionOptions.length > 0) {

                            var CascadingOption = SectionQuestions[j].CascadingQuestionOptions
                            for (var k = 0; k < CascadingOption.length; k++) {
                                SectionQuestions[j].CascadingQuestionOptions[k][0] = null;
                                SectionQuestions[j].CascadingQuestionOptions[k][k] = null;
                            }
                        }
                    }
                    if (SectionQuestions[j].QuestionType == "Multi-Choice") {

                        //  this.multiChoiceAns = [];
                        for (var Cnt = 0; Cnt < SectionQuestions[j].QuestionOptions.length; Cnt++) {
                            SectionQuestions[j].QuestionOptions[Cnt].IsOptSelected = false;
                        }

                    }
                    if ((SectionQuestions[j].QuestionType == "Photo Capture") || (SectionQuestions[j].QuestionType == "Signature Capture")
                        || (SectionQuestions[j].QuestionType == "File Upload")) {

                        if (SectionQuestions[j].QuestionType == "Photo Capture") {

                            SectionQuestions[j].img = null;
                            SectionQuestions[j].uploadedImg = null;
                            SectionQuestions[j].startCamera = false;
                            this.camOn = false;
                            this.devices = [];
                            this.deviceId = null;
                            this.camera = null;
                            this.audUpload[j] = 0;
                        }
                        if (SectionQuestions[j].QuestionType == "Signature Capture") {
                            SectionQuestions[j].uploadedSign = null;
                            await this.clearSign(SectionQuestions[j], false, SectionQuestions[j].QuestionID);
                        }
                        if (SectionQuestions[j].QuestionType == "File Upload") {
                            SectionQuestions[j].uploadedFile = null;
                        }
                        SectionQuestions[j].AnsBlob = [];
                    }
                    if (SectionQuestions[j].QuestionType == "Audio Capture") {
                        var audioCatpure = document.getElementById("recAudioOne");

                        if (audioCatpure != null) {
                            audioCatpure.__vue__._data.recordList = [];
                            audioCatpure.__vue__._data.recorder.records = [];
                        }
                        this.savedAudio = "";
                    }


                }
            }


        },


        async fnGetApproverQstnAnswers_Modified() {
            if (this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList != null) {

                this.ApprovalTableHeader = [];
                this.ApprovalData = [];

                var benefQuestionAndAns = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns;

                var lstBenefSubmittedList = this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList;

                var lstQuestions = this.lstSelectedTaskData.TaskData.SurveyQstnList;

                // Creating Approval Table Headers
                var QItem = [];
                var BenefID = 0;
                var vaLogicType = "";
                var BeneficiaryId = 0;
                var repeatSectionIDList = "";
                var prevAnswer = [];
                var preAnswer1 = "";
                var tempQuestionHeaders = [];
                //// ---- In the case of Repeat section Question Count for each beneficiary may varies;
                ////----- So here we need to pick one beneficiayID which have maximum Question Count -- below code is todo that;
                // take a random beneficiary ID, currently we are taking from 0th index;
                var benefIDrdm = lstBenefSubmittedList[0].BeneficiaryID

                // this filter will gives BeneficiaryID and its Count
                var result = lstBenefSubmittedList.reduce((acc, o) => (acc[o.BeneficiaryID] = (acc[o.BeneficiaryID] || 0) + 1, acc), {})

                var maxCount = result[benefIDrdm];
                BeneficiaryId = benefIDrdm;
                for (var res in result) {
                    if (result[res] > maxCount) {
                        BeneficiaryId = res; // Beneficiary ID which have maximum questions
                    }
                }
                // -- Finding BenenficiryID with Max Question Count finish here -------------------

                this.ApprovalTableHeader.push({
                    text: "SubmittedDate",
                    value: "SubmittedDate",
                    type: "string",
                    mandatory: 'No',
                    //options:optionlist,
                    QuestionCondition: [],
                    QuestionID: 0,
                });

                // Adding Beneficiary Questions
                for (var bCnt = 0; bCnt <= benefQuestionAndAns.length - 1; bCnt++) {
                    var BenefQName = benefQuestionAndAns[bCnt].QuestionName;// +"(B)"
                    this.ApprovalTableHeader.push({
                        text: BenefQName,
                        value: BenefQName,
                        type: "string",
                        mandatory: 'No',
                        //options:optionlist,
                        QuestionCondition: [],
                        QuestionID: benefQuestionAndAns[bCnt].QuestionID,
                    });
                }
                for (var Cnt = 0; Cnt <= lstBenefSubmittedList.length - 1; Cnt++) {
                    if ((BeneficiaryId == lstBenefSubmittedList[Cnt].BeneficiaryID)) {

                        QItem = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.QuestionID == lstBenefSubmittedList[Cnt].QuestionID);

                        if (QItem[0].QuestionType != "Note") // avoiding Note Question Type
                        {
                            // to order Questions Based on Repeat and Skip Logic
                            if (QItem[0].IsConditionQuestion == 1) {
                                var sectionLogic = QItem[0].SectionLogic.filter(x => x.QuestionID == QItem[0].QuestionID && x.Answer == QItem[0].Answer1)[0];
                                if (typeof sectionLogic == "undefined") {
                                    sectionLogic = QItem[0].SectionLogic.filter(x => x.QuestionID == QItem[0].QuestionID)[0];
                                }
                                if (sectionLogic.LogicType == "Repeat Logic") {
                                    var exist = this.ApprovalTableHeader.filter(x => x.text.includes(QItem[0].QuestionName));
                                    if (exist.length > 0) {
                                        var len = exist.length;
                                        var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                                        this.ApprovalTableHeader.push({
                                            text: QName, value: QName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });
                                    }
                                    else {
                                        this.ApprovalTableHeader.push({
                                            text: QItem[0].QuestionName, value: QItem[0].QuestionName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });
                                    }

                                    repeatSectionIDList += sectionLogic.SkipSectionID + ",";
                                    var RepeatQuestions = lstQuestions.filter(x => x.SectionID == sectionLogic.SkipSectionID);
                                    if (RepeatQuestions.length > 0) {
                                        var RepeatCount = lstBenefSubmittedList[Cnt].Answer1;
                                        for (var rCnt = 0; rCnt <= RepeatCount - 1; rCnt++) {

                                            for (var kp = 0; kp <= RepeatQuestions.length - 1; kp++) {
                                                var exist = this.ApprovalTableHeader.filter(x => x.text.includes(RepeatQuestions[kp].QuestionName));
                                                if (exist.length > 0) {
                                                    var len = exist.length;
                                                    var QName = RepeatQuestions[kp].QuestionName + "(R" + len.toString() + ")";
                                                    this.ApprovalTableHeader.push({
                                                        text: QName, value: QName, type: "string", mandatory: 'No', QuestionCondition: [],
                                                        QuestionID: RepeatQuestions[kp].QuestionID,
                                                    });
                                                }
                                                else {

                                                    this.ApprovalTableHeader.push({
                                                        text: RepeatQuestions[kp].QuestionName, value: RepeatQuestions[kp].QuestionName, type: "string", mandatory: 'No', QuestionCondition: [],
                                                        QuestionID: RepeatQuestions[kp].QuestionID,
                                                    });
                                                }
                                            }

                                        }
                                    }
                                }
                                else // skip logic
                                {

                                    var exist = this.ApprovalTableHeader.filter(x => x.text.includes(QItem[0].QuestionName));
                                    if (exist.length > 0) {
                                        var len = exist.length;
                                        var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                                        this.ApprovalTableHeader.push({
                                            text: QName, value: QName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });
                                    }
                                    else {
                                        this.ApprovalTableHeader.push({
                                            text: QItem[0].QuestionName, value: QItem[0].QuestionName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });
                                    }

                                    // Get section and Question which comes under a skip logic
                                    var skipSectionLogic = QItem[0].SectionLogic.filter(x => x.QuestionID == QItem[0].QuestionID);
                                    var count = 0;
                                    var conditionvalue = '==';
                                    for (res in result) // loop of beneficiary (difference Question Names in each beneficiary)
                                    {
                                        count++;
                                        for (var n = 0; n <= skipSectionLogic.length - 1; n++) {
                                            var skipsecAnswer = skipSectionLogic[n].Answer;
                                            var qAnswer = lstBenefSubmittedList.filter(x => x.BeneficiaryID == res && x.QuestionID == QItem[0].QuestionID)[0].Answer;

                                            // to prevent duplicate header in case of skip logic , Question having different logic in different beneficiary.
                                            preAnswer1 = qAnswer;
                                            var skipSectionID = 0;
                                            if (count > 1) {
                                                if (prevAnswer.filter(x => x.text.includes(qAnswer)).length > 0) {
                                                    qAnswer = "";
                                                }
                                            }
                                            if (isNaN(qAnswer)) // is a string
                                            {
                                                qAnswer = "'" + qAnswer + "'";
                                                skipsecAnswer = "'" + skipSectionLogic[n].Answer + "'";
                                            }
                                            else {
                                                qAnswer = qAnswer;
                                            }
                                            if (skipSectionLogic[n].ConditionValue == '=') {
                                                conditionvalue = '==';
                                            }
                                            else {
                                                conditionvalue = skipSectionLogic[n].ConditionValue
                                            }
                                            //var skipSectionIDs = skipSectionLogic[n].filter(x=> x.QuestionID == QItem[0].QuestionID && x.Answer == QItem[0].Answer1)
                                            if (qAnswer != "") {
                                                if (eval(qAnswer + conditionvalue + skipsecAnswer))
                                                //if(skipSectionLogic[n].Answer == qAnswer   )
                                                {
                                                    skipSectionID = skipSectionLogic[n].SkipSectionID;
                                                }
                                            }
                                            repeatSectionIDList += skipSectionLogic[n].SkipSectionID + ","
                                            var skipSectionsQuestion = lstQuestions.filter(x => x.SectionID == skipSectionID)
                                            if (skipSectionsQuestion.length > 0) {
                                                for (var sk = 0; sk <= skipSectionsQuestion.length - 1; sk++) {
                                                    var exist = this.ApprovalTableHeader.filter(x => x.text.includes(skipSectionsQuestion[sk].QuestionName));
                                                    if (exist.length > 0) {
                                                        var len = exist.length;
                                                        var QName = skipSectionsQuestion[sk].QuestionName + "(R" + len.toString() + ")";
                                                        this.ApprovalTableHeader.push({
                                                            text: QName, value: QName, type: "string", mandatory: 'No', QuestionCondition: [],
                                                            QuestionID: skipSectionsQuestion[sk].QuestionID,
                                                        });
                                                        tempQuestionHeaders.push({ benefID: res, QuestionID: skipSectionsQuestion[sk].QuestionID, QuestionName: QName, OriginalQuestionName: skipSectionsQuestion[sk].QuestionName });
                                                    }
                                                    else {
                                                        this.ApprovalTableHeader.push({
                                                            text: skipSectionsQuestion[sk].QuestionName, value: skipSectionsQuestion[sk].QuestionName, type: "string", mandatory: 'No', QuestionCondition: [],
                                                            QuestionID: skipSectionsQuestion[sk].QuestionID,
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                        // pushing logic answer to an array
                                        // to prevent duplicate header in case of skip logic , Question having same logic in different beneficiary.
                                        prevAnswer.push({ text: preAnswer1 })
                                    }
                                }
                            }
                            else {
                                if (repeatSectionIDList.includes(QItem[0].SectionID) == false) {
                                    var exist = this.ApprovalTableHeader.filter(x => x.text.includes(QItem[0].QuestionName));
                                    if (exist.length > 0) {
                                        var len = exist.length;
                                        var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                                        this.ApprovalTableHeader.push({
                                            text: QName, value: QName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });
                                    }
                                    else {
                                        this.ApprovalTableHeader.push({
                                            text: QItem[0].QuestionName, value: QItem[0].QuestionName, type: "string", mandatory: 'No', QuestionCondition: [],
                                            QuestionID: QItem[0].QuestionID,
                                        });

                                    }

                                }
                            }
                        }

                    }
                }


                // Creating Row for table
                var lstBeneficiryID = lstBenefSubmittedList.map(function (item) {
                    return item.BeneficiaryID;
                });
                // Now we will use set to remove duplicate value from array
                var setOfValue = new Set(lstBeneficiryID);
                // Now we will convert set to array using spread operator
                var NoOfSelectedBen = [...setOfValue];

                var tempData = [];
                var obj = new Object();
                for (var row = 0; row <= NoOfSelectedBen.length - 1; row++) {
                    tempData = [];
                    obj = new Object();
                    var BenfData = lstBenefSubmittedList.filter(x => x.BeneficiaryID == NoOfSelectedBen[row])
                    var benefDetails = this.lstSelectedTaskData.TaskData.BeneficiaryDetails.filter(x => x.BeneficiaryID == NoOfSelectedBen[row])
                    for (var bCnt = 0; bCnt <= benefDetails.length - 1; bCnt++) {
                        var BenefQName = benefDetails[bCnt].QuestionName; //+"(B)";
                        var BenefQID = benefDetails[bCnt].QuestionID;
                        obj[BenefQName] = benefDetails[bCnt].Answer;
                        obj[BenefQID] = benefDetails[bCnt].QuestionID;
                        tempData.push({ text: BenefQName, value: BenefQName, benefID: 0 });
                    }
                    obj['SubmittedDate'] = this.FormatDateWithTime(BenfData[0].CreatedDate);
                    tempData.push({ text: 'SubmittedDate', value: BenfData[0].CreatedDate, benefID: 0 });
                    for (var row1 = 0; row1 <= BenfData.length - 1; row1++) {
                        QItem = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.QuestionID == BenfData[row1].QuestionID);
                        if (QItem.length > 1) {
                            var temp = BenfData.filter(x => x.QuestionID == QItem[0].QuestionID)
                            for (var mnCn = 0; mnCn <= temp.length - 1; mnCn++) {

                                if (mnCn > 0) {
                                    var exist = tempData.filter(x => x.text.includes(QItem[0].QuestionName)); //tempData
                                    if (exist.length > 0) {
                                        var len = exist.length;
                                        var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                                        var QID = QItem[0].QuestionID;
                                        obj[QName] = temp[mnCn].Answer;
                                        obj[QID] = temp[mnCn].QuestionID;
                                        tempData.push({ text: QName, value: QName, benefID: BenfData[row1].BeneficiaryID });
                                    }
                                } else {
                                    obj[QItem[0].QuestionName] = temp[mnCn].Answer;
                                    obj[QItem[0].QuestionID] = temp[mnCn].QuestionID;
                                    tempData.push({ text: QItem[0].QuestionName, value: QItem[0].QuestionName, benefID: BenfData[row1].BeneficiaryID });
                                }
                            }

                        } else {
                            //Check Question Name alreadyexist
                            var isExist = tempData.filter(x => x.text.includes(QItem[0].QuestionName)); //&& x.benefID == BenfData[row1].BeneficiaryID
                            if (isExist.length > 0) {
                                var len = isExist.length;
                                var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                                var QID = QItem[0].QuestionID;
                                obj[QName] = BenfData[row1].Answer;
                                obj[QID] = BenfData[row1].QuestionID;
                                tempData.push({ text: QName, value: QName, benefID: BenfData[row1].BeneficiaryID });
                            }
                            else {
                                var QName = QItem[0].QuestionName
                                var tempQName = tempQuestionHeaders.filter(x => x.benefID == BenfData[row1].BeneficiaryID && x.OriginalQuestionName == QItem[0].QuestionName)
                                if (tempQName.length > 0) {
                                    QName = tempQName[0].QuestionName;
                                }
                                obj[QName] = BenfData[row1].Answer;
                                obj[QItem[0].QuestionID] = BenfData[row1].QuestionID;
                                tempData.push({ text: QItem[0].QuestionName, value: QItem[0].QuestionName, benefID: BenfData[row1].BeneficiaryID });
                            }
                        }
                    }
                    this.ApprovalData.push(obj);

                }

            }

        },

        // To Get Collected Questions and Answers for Approval in Table format for Non Beneficiary Linked
        async fnGetNonBenfApproverQstnAnswers() {

            this.NonBenfApprovalTableHeader = [];
            this.NonBenfApprovalData = [];
            // Creating Approval Table Headers
            var QItem = [];
            var vaLogicType = "";
            var lstQuestions = this.lstSelectedTaskData.TaskData.SurveyQstnList;
            var lstRespose = this.lstSelectedTaskData.TaskData.SurveyResponse;
            var IsConditionQuestions = lstQuestions.filter(x => x.IsConditionQuestion == 1);
            var repeatSectionIDList = "";
            lstQuestions = lstQuestions.filter(el => {
                return lstRespose.find(element => {
                    return element.QuestionID === el.QuestionID;
                });
            });

            for (var Cnt = 0; Cnt <= lstQuestions.length - 1; Cnt++) {
                if (lstQuestions[Cnt].QuestionType != "Note") {
                    if (lstQuestions[Cnt].IsConditionQuestion == 1) {
                        var sectionLogic = lstQuestions[Cnt].SectionLogic.filter(x => x.QuestionID == lstQuestions[Cnt].QuestionID)[0];
                        if (sectionLogic.LogicType == "Repeat Logic") {
                            var exist = this.NonBenfApprovalTableHeader.filter(x => x.text.includes(lstQuestions[Cnt].QuestionName));
                            if (exist.length > 0) {
                                var len = exist.length;
                                var QName = lstQuestions[Cnt].QuestionName + "(R" + len.toString() + ")";
                                this.NonBenfApprovalTableHeader.push({ text: QName, value: QName });
                            }
                            else {
                                this.NonBenfApprovalTableHeader.push({ text: lstQuestions[Cnt].QuestionName, value: lstQuestions[Cnt].QuestionName });
                            }
                            repeatSectionIDList += sectionLogic.SkipSectionID + ","
                            var RepeatQuestions = lstQuestions.filter(x => x.SectionID == sectionLogic.SkipSectionID)
                            if (RepeatQuestions.length > 0) {
                                var RepeatCount = lstQuestions[Cnt].Answer1;
                                for (var rCnt = 0; rCnt <= RepeatCount - 1; rCnt++) {

                                    for (var kp = 0; kp <= RepeatQuestions.length - 1; kp++) {

                                        var exist = this.NonBenfApprovalTableHeader.filter(x => x.text.includes(RepeatQuestions[kp].QuestionName));

                                        if (exist.length > 0) {
                                            var len = exist.length;
                                            var QName = RepeatQuestions[kp].QuestionName + "(R" + len.toString() + ")";
                                            this.NonBenfApprovalTableHeader.push({ text: QName, value: QName });
                                        }
                                        else {
                                            this.NonBenfApprovalTableHeader.push({ text: RepeatQuestions[kp].QuestionName, value: RepeatQuestions[kp].QuestionName });
                                        }
                                    }

                                }
                            }
                        }
                        else // skip logic
                        {
                            var exist = this.NonBenfApprovalTableHeader.filter(x => x.text.includes(lstQuestions[Cnt].QuestionName));
                            if (exist.length > 0) {
                                var len = exist.length;
                                var QName = lstQuestions[Cnt].QuestionName + "(R" + len.toString() + ")";
                                this.NonBenfApprovalTableHeader.push({ text: QName, value: QName });
                            }
                            else {
                                this.NonBenfApprovalTableHeader.push({ text: lstQuestions[Cnt].QuestionName, value: lstQuestions[Cnt].QuestionName });
                            }
                            // Get section and Question which comes under a skip logic

                            repeatSectionIDList += sectionLogic.SkipSectionID + ","
                            var skipSectionsQuestion = lstQuestions.filter(x => x.SectionID == sectionLogic.SkipSectionID)
                            if (skipSectionsQuestion.length > 0) {
                                for (var sk = 0; sk <= skipSectionsQuestion.length - 1; sk++) {
                                    var exist = this.NonBenfApprovalTableHeader.filter(x => x.text.includes(skipSectionsQuestion[sk].QuestionName));
                                    if (exist.length > 0) {
                                        var len = exist.length;
                                        var QName = skipSectionsQuestion[sk].QuestionName + "(R" + len.toString() + ")";
                                        this.NonBenfApprovalTableHeader.push({ text: QName, value: QName });
                                    }
                                    else {
                                        this.NonBenfApprovalTableHeader.push({ text: skipSectionsQuestion[sk].QuestionName, value: skipSectionsQuestion[sk].QuestionName });
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (repeatSectionIDList.includes(lstQuestions[Cnt].SectionID) == false) {
                            // to do
                            var exist = this.NonBenfApprovalTableHeader.filter(x => x.text.includes(lstQuestions[Cnt].QuestionName));

                            if (exist.length > 0) {
                                var len = exist.length;
                                var QName = lstQuestions[Cnt].QuestionName + "(R" + len.toString() + ")";
                                this.NonBenfApprovalTableHeader.push({ text: QName, value: QName });
                            }
                            else {
                                this.NonBenfApprovalTableHeader.push({ text: lstQuestions[Cnt].QuestionName, value: lstQuestions[Cnt].QuestionName });
                            }
                        }


                    }
                }
            }
            var ReponseCount = this.lstSelectedTaskData.ResponseCount;
            // Creating Row for table

            var tempData = [];
            var obj = new Object();
            //obj['CreatedDate'] = lstQuestions[0].CreatedDate;
            if (ReponseCount > 1) // checking if multiple response are coming or not
            {
                // Creating Row for table
                var lstQuestionID = lstRespose.filter(x => x.QuestionID == lstQuestions[0].QuestionID)


                // Now we will use set to remove duplicate value from array
                var setOfValue = lstQuestionID;//new Set(lstQuestionID);
                // Now we will convert set to array using spread operator
                var ResponseCount = [...setOfValue];

                for (var resCnt = 0; resCnt <= ResponseCount.length - 1; resCnt++) {

                    var tempData = [];
                    var obj = new Object();
                    for (var cQID = 0; cQID <= lstQuestions.length - 1; cQID++) {

                        var AnswerList = lstRespose.filter(x => x.QuestionID == lstQuestions[cQID].QuestionID);
                        var Ans = "";
                        if (AnswerList.length > 0) {
                            Ans = AnswerList[resCnt] != null && typeof AnswerList[resCnt] != "undefined" ? AnswerList[resCnt].Answer : ""
                        }
                        var isExist = tempData.filter(x => x.text.includes(lstQuestions[cQID].QuestionName));
                        if (isExist.length > 0) {
                            var len = isExist.length;
                            var QName = lstQuestions[cQID].QuestionName + "(R" + len.toString() + ")";
                            obj[QName] = Ans;
                            tempData.push({ text: QName, value: QName });
                        }
                        else {
                            obj[lstQuestions[cQID].QuestionName] = Ans;
                            tempData.push({ text: lstQuestions[cQID].QuestionName, value: lstQuestions[cQID].QuestionName });

                        }

                    }
                    this.NonBenfApprovalData.push(obj);
                }
            }
            else {
                for (var row1 = 0; row1 <= lstRespose.length - 1; row1++) {
                    var QItem = lstQuestions.filter(x => x.QuestionID == lstRespose[row1].QuestionID);
                    var exist = tempData.filter(x => x.text.includes(QItem[0].QuestionName));
                    if (exist.length > 0) {
                        var len = exist.length;
                        var QName = QItem[0].QuestionName + "(R" + len.toString() + ")";
                        obj[QName] = lstRespose[row1].Answer;
                        tempData.push({ text: QName, value: QName });
                    }
                    else {
                        obj[QItem[0].QuestionName] = lstRespose[row1] != null && typeof lstRespose[row1] != undefined ? lstRespose[row1].Answer : ""
                        tempData.push({ text: QItem[0].QuestionName, value: QItem[0].QuestionName });
                    }

                }
                this.NonBenfApprovalData.push(obj);

            }

        },



        async fnClearAnyTimeList() {

            if (this.AnyTimeSubmissionId > 0) {
                const index = this.lstAnyTimeNewSubmission.findIndex((element, idx) => {
                    if (element.pkId === this.AnyTimeSubmissionId && (element.TaskData.CurrentOrOverdue === "Overdue" || element.TaskData.CurrentOrOverdue === "Current")) {
                        return true
                    }
                });
                if (index >= 0) {
                    this.lstAnyTimeNewSubmission.splice(index, 1);
                }
                await dbImworks.TbAnyTimeData.where("pkId").equals(parseInt(this.AnyTimeSubmissionId)).delete();
            }

        },

        // Method used to clear all data
        async fnClearData() {
            this.IsBeneficiaryLinkedSurvey = false;
            this.SelectedSurveyName = "";
            this.SelectedTaskType = "";
            this.SelectedTaskTypeID = 0;
            this.DataCollectorInfo = [{ DatacollectorName: "", AssignedTime: "" }]; //Thur, 8:30 AM
            this.IsTaskUpdated = false;
            this.lstSelectedTaskData = [];
            this.vmSearchBeneficiary = "";
            this.SurveyBenLinkDlg = false;
            this.NonBenLinkSurveyDlg = false;
            this.SnackbarMsg = "";
            this.BenefLinkedSurveySteps = 2;
            this.BeneficiariesWithName = [];
            this.BeneficiarySectionQstnList = [];
            this.lstSections = [];
            this.lstSectionsQstns = [];
            this.BenefStepper = 1;
            this.SelectedTaskDate = "";
            this.vmUpdateComment = "";
            this.cascadingAns = [];
            this.multiChoiceAns = [];
            this.multiChoiceAnsBT = [];
            this.MultiChoiceAnsForRepeat = [];
            this.cascadingAnsBT = [];
            this.lstBenefRegQstns = [];
            this.newBenefID = 0;
            this.lstNewBeneficiary = [];
            this.vmSelectedBeneficiaries = [];
            this.lstSelectedBeneficiary = [];
            this.UnSavedNewBenfList = [];
            this.lstBeneficiary = [];
            this.UploadDocument = null;
            this.lstImportBeneficiary = [];
            this.lstImportBeneficiaryHearder = [];
            this.File = [];
            this.BenefImportTab = 'tab-4';
            this.lstImportSurveyQstnHearder = [];
            this.lstBenefSurveyQstnAnswers = [];
            this.lstImportSurveyQstnHearderN = [];
            this.SurveyFile = [];
            this.IsImportSurvey = false;
            this.SaveAndContinue = false;
            this.CompletionPerc = 0;
            this.surveyApproveBenfLinkedDialog = false;
            this.surveyApproveNonBenfLinkedDialog = false;
            this.AnyTimeSubmissionId = 0;
            this.startCamera = false;
            this.camOn = false;
            this.img1 = null;
            this.benefUploadedImg = "";
            this.benefUploadedFile = "";
            this.benefUploadedSign = "";
            this.delListBT = [];
            this.BTAnsBlobQID = 0;
            this.CustomMessage = "This Task is sent for Approval";
            this.RepeatOrSkipSection = [];
            this.NonBenfApprovalData = [];
            this.NonBenfApprovalTableHeader = [];
            this.ApprovalTableHeader = [];
            this.ApprovalData = [];
            this.SelectedAppStatus = "";
            this.clientguid = "";

            this.TaskCardData[this.ParentItemIndex].btnDisable = false;
            this.ParentItemIndex = -1;

            var BenefExceldata = document.getElementById("exceleditor");
            if (BenefExceldata != null) {
                BenefExceldata.__vue__._data.fields = [];
                // BenefExceldata.__vue__._data.fields =[];
            }
            var NonBenfexcelData = document.getElementById("exceldataNonBenf");
            if (NonBenfexcelData != null) {
                NonBenfexcelData.__vue__._data.fields = [];
                //NonBenfexcelData.__vue__._data.fields = [];
            }
            //this.lstAnyTimeNewSubmission = [];
            //this.IsDataCollector = true;
            this.surveyApproveBenfLinkedSteps = 1;
            this.TaskApproveDescription = "";
            this.beneficiaryStepperList = [];
            this.Coordinates = {
                lat: this.defaultCoordinates.lat,
                lng: this.defaultCoordinates.lng,
            };
            this.BTAnsBlob = [];
            this.AnsBlob = [];
        },

        async GetSelectedBeneficiaries() {
            // Push to vmSelectedBeneficiaries
            this.vmSelectedBeneficiaries = [];
            this.lstNewBeneficiary = [];
            var BeneficiaryId = 0;
            for (var k = 0; k <= this.lstSelectedBeneficiary.length - 1; k++) {
                BeneficiaryId--;
                if (this.lstSelectedBeneficiary[k].BeneficiaryID > 0) {
                    var temp = this.BeneficiariesWithName.filter(x => x.BeneficiaryID == this.lstSelectedBeneficiary[k].BeneficiaryID);
                    if (temp.length > 0) {
                        this.vmSelectedBeneficiaries.push(temp[0])
                    }
                }
                else {
                    // if (this.SearchExistingBeneficiary == true) { // Search from existing beneficiary
                    this.UnSavedNewBenfList = await dbImworks.TbNewBeneficiary.toArray();
                    this.filterNewBenefList = this.UnSavedNewBenfList.filter(x => x.BeneficiaryID == this.lstSelectedBeneficiary[k].BeneficiaryID && x.WorkspaceID == this.WorkspaceID);

                    for (var Count = 0; Count <= this.filterNewBenefList.length - 1; Count++) {
                        //var temp = this.lstNewBeneficiary.filter(x => x.BeneficiaryID == this.filterNewBenefList[Count].BeneficiaryID)

                        var obj = new Object();
                        obj.BeneficiaryID = BeneficiaryId;
                        obj.BeneficiaryName = this.filterNewBenefList[Count].BeneficiaryName;
                        obj.BeneficiaryTypeID = this.filterNewBenefList[Count].BeneficiaryTypeID;
                        obj.CreatedDate = this.filterNewBenefList[Count].CreatedDate;
                        obj.TokenValue = this.filterNewBenefList[Count].TokenValue;
                        obj.BeneficiaryQstnAnswers = this.filterNewBenefList[Count].BeneficiaryQstnAnswers;
                        this.vmSelectedBeneficiaries.push(obj);
                        this.lstNewBeneficiary.push(obj);
                        //if (temp.length == 0) {

                        //    this.lstNewBeneficiary.push(obj);
                        //}

                    }

                    //  }
                }
            }
        },

        //************** Audio and Photo Capture Methods
        async GetMultiChoiceRepeat(optItem, optSelected, optVal, RQstInd, QuestionID, benefIndex) {
            if (optSelected) {
                this.MultiChoiceAnsForRepeat.push({ OptionValue: optVal, QuestionID: QuestionID, benefIndex: benefIndex, RQstInd: RQstInd });
            }
            else {
                const index = this.MultiChoiceAnsForRepeat.findIndex((element, index) => {
                    if (element.QuestionID === optItem.QuestionID && element.OptionValue == optVal && element.benefIndex == benefIndex) {
                        return true
                    }
                });
                this.MultiChoiceAnsForRepeat.splice(index, 1);
            }
        },
        callback(data) {
            console.log(data)
        },
        async playSound() {

            if (this.savedAudio) {
                var audio = new Audio(this.savedAudio);
                audio.play();
            }
        },
        async SaveAudio(data) {

            this.blobFileInput = data.blob;
            //this.SaveFile("Audio", this.blobFileInput, false, false, this.BenefStepper);
        },

        async FindRecordedFileItem(item) {
            if (this.blobFileInput !== null) {
                this.SaveFile(item, "Audio", this.blobFileInput, false, false, this.BenefStepper);
            }
        },
        async FindRecordedFileItemSkipRepeat(Ritem) {
            if (this.blobFileInput !== null) {
                this.SaveFileRepeat(Ritem, "Audio", this.blobFileInput, false, false, this.BenefStepper, Ritem.RQstInd);

            }
        },
        onStop() {
            this.$refs.webcam.stop();
        },
        onStart() {

            this.$refs.webcam[0].start();
        },
        onStartRepeat() {
            // this.$refs.webcam[0].start();
            if (typeof this.$refs.webcam != "undefined" && this.$refs.webcam.length > 0) {
                this.$refs.webcam[0].start();
            }
        },
        onStarted(stream) {
            console.log("On Started Event", stream);
        },
        onStopped(stream) {
            console.log("On Stopped Event", stream);
        },
        onError(error) {
            console.log("On Error Event", error);
        },
        onCameras(cameras) {
            this.devices = cameras;
            console.log("On Cameras Event", cameras);
        },

        onCameraChange(deviceId) {
            this.deviceId = deviceId;
            this.camera = deviceId;
            console.log("On Camera Change Event", deviceId);
        },
        // Camera functions for beneficiary
        onCameras1(cameras) {
            this.devices = cameras;
            console.log("On Cameras Event", cameras);
        },
        onCameraChange1(deviceId) {
            this.deviceId = deviceId;
            this.camera = deviceId;
            console.log("On Camera Change Event", deviceId);
        },

        // Function Trigger when we click on Capture button on WebCam
        // Function used take picture using webcam
        async onCapture(item, QID, IsBenefQstn, benefIndx) {
            var data = [];
            var img = "";
            if (IsBenefQstn) {
                this.BTAnsBlobQID = QID;
                this.img1 = this.$refs.webcam[0].capture();
                data = this.img1.split(',')[1];
            }
            else {
                this.AnsBlobQID = QID;
                item.img = this.$refs.webcam[0].capture();
                img = this.$refs.webcam[0].capture();
                data = img.split(',')[1];
                this.$refs.webcam[0].stop();

            }
            let blobFile = await this.b64toBlob(data, 'image/jpg"');
            this.SaveFile(item, "Photo", blobFile, IsBenefQstn, false, benefIndx);

        },


        async SaveFile(Qitem, fileType, blbFile, IsBenefQstn, IsUploadedFile, benefIndx) {

            const validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
            var fname = "";
            var ansType = "";
            var timestamp = new Date().valueOf();
            var prevData = "";

            switch (fileType) {
                case "Audio":
                    fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
                    ansType = "Audio Capture";
                    break;

                case "Signature":
                    fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Signature Capture";
                    break;
                case "Photo":
                    fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Photo Capture";
                    break;

                case "File":
                    fname = "File_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "File Upload";
                    break;
            }
            // In the case of Beneficairy Registrations
            if (IsBenefQstn) {

                prevData = this.BTAnsBlob.filter(x => x.QuestionID == this.BTAnsBlobQID);
                if (prevData.length > 0) {
                    // this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(this.BTAnsBlobQID), 1);
                    this.BTAnsBlob.splice(this.BTAnsBlob.indexOf(prevData), 1);
                    this.BTAnsBlob = [...this.BTAnsBlob];
                }


                if (fileType == "Audio")
                    this.savedAudioBenef = window.URL.createObjectURL(blbFile);
                else {
                    if (fileType == "Signature") {
                        if (!validImageTypes.includes(blbFile['type']))
                            this.benefUploadedSign = null;
                        else {
                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {

                                this.benefUploadedSign = fr.result

                            });
                        }

                    }
                    if (fileType == "File") {
                        if (!validImageTypes.includes(blbFile['type']))
                            this.benefUploadedFile = null;
                        else {
                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {
                                this.benefUploadedFile = fr.result

                            });
                        }

                    }
                    if (fileType == "Photo") {
                        if (!validImageTypes.includes(blbFile['type']))
                            this.benefUploadedImg = null;
                        else {
                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {
                                this.benefUploadedImg = fr.result

                            });
                        }

                    }

                }
                this.BTAnsBlob.push({ QuestionID: this.BTAnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, IsUploadedFile: IsUploadedFile });
                this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer = fname;



            }
            else {
                prevData = this.AnsBlob.filter(x => (x.QuestionID == Qitem.QuestionID) && (x.benefIndex == benefIndx));
                if (prevData.length > 0) {
                    this.AnsBlob.splice(this.AnsBlob.indexOf(prevData), 1);
                    this.AnsBlob = [...this.AnsBlob];
                }

                if (fileType == "Audio") {
                    this.savedAudio = window.URL.createObjectURL(blbFile);

                }

                else {
                    if (fileType == "Signature") {


                        if (!validImageTypes.includes(blbFile['type']))
                            //this.uploadedSign = null;
                            Qitem.uploadedSign = null;
                        else {

                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {
                                // this.uploadedSign = fr.result
                                Qitem.uploadedSign = fr.result;
                            });

                        }

                    }
                    if (fileType == "File") {

                        if (!validImageTypes.includes(blbFile['type']))
                            Qitem.uploadedFile = null;
                        //this.uploadedFile = null;
                        else {
                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {
                                //this.uploadedFile = fr.result
                                Qitem.uploadedFile = fr.result;
                            });
                        }

                    }
                    if (fileType == "Photo") {
                        if (!validImageTypes.includes(blbFile['type']))
                            //  this.uploadedImg = null;
                            Qitem.uploadedImg = null;
                        else {
                            const fr = new FileReader();
                            fr.readAsDataURL(blbFile)
                            fr.addEventListener('load', () => {
                                // this.uploadedImg = fr.result
                                Qitem.uploadedImg = fr.result;
                            });
                        }

                    }

                }

                const index = this.AnsBlob.findIndex((element, index) => {
                    if (element.benefIndex === benefIndx && element.QuestionID == Qitem.QuestionID) {
                        return true
                    }
                });
                if (index > -1) {
                    this.AnsBlob.splice(index, 1);
                }

                this.AnsBlob.push({ QuestionID: Qitem.QuestionID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, IsUploadedFile: IsUploadedFile, benefIndex: benefIndx, RQstInd: -1 });
                Qitem.Answer = fname;
            }
        },

        SaveFileRepeat(Qitem, fileType, blbFile, IsBenefQstn, IsUploadedFile, benefIndx, RQstInd) {

            var benefID = 0;
            var pQID = Qitem.QuestionID;
            if (this.IsBeneficiaryLinkedSurvey == true) {
                benefID = this.vmSelectedBeneficiaries[benefIndx - 1].BeneficiaryID;
            }

            const validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
            var fname = "";
            var ansType = "";
            var timestamp = new Date().valueOf();
            var prevData = "";

            switch (fileType) {
                case "Audio":
                    fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
                    ansType = "Audio Capture";
                    break;

                case "Signature":
                    fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Signature Capture";
                    break;
                case "Photo":
                    fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Photo Capture";
                    break;

                case "File":
                    fname = "File_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "File Upload";
                    break;
            }

            prevData = this.AnsBlob.filter(x => (x.QuestionID == pQID) && (x.benefIndex == this.BenefStepper) && (x.RQstInd == RQstInd));
            if (prevData.length > 0) {
                this.AnsBlob.splice(this.AnsBlob.indexOf(prevData), 1);
                this.AnsBlob = [...this.AnsBlob];
            }

            if (fileType == "Audio") {
                this.savedAudio = window.URL.createObjectURL(blbFile);

            }

            else {
                if (fileType == "Signature") {
                    if (!validImageTypes.includes(blbFile['type']))

                        Qitem.uploadedSign = null
                    else {
                        const fr = new FileReader();
                        fr.readAsDataURL(blbFile)
                        fr.addEventListener('load', () => {

                            Qitem.uploadedSign = fr.result

                        });
                    }

                }
                if (fileType == "File") {
                    if (!validImageTypes.includes(blbFile['type']))
                        //this.uploadedFile = null;
                        Qitem.uploadedFile = null;
                    else {
                        const fr = new FileReader();
                        fr.readAsDataURL(blbFile)
                        fr.addEventListener('load', () => {
                            // this.uploadedFile = fr.result

                            Qitem.uploadedFile = fr.result;

                        });
                    }

                }
                if (fileType == "Photo") {

                    if (!validImageTypes.includes(blbFile['type']))
                        //this.uploadedImg = null;
                        Qitem.uploadedImg = null;
                    else {
                        const fr = new FileReader();
                        fr.readAsDataURL(blbFile)
                        fr.addEventListener('load', () => {
                            //this.uploadedImg = fr.result
                            Qitem.uploadedImg = fr.result

                        });
                    }

                }

            }
            this.AnsBlob.push({ QuestionID: pQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, IsUploadedFile: IsUploadedFile, benefIndex: this.BenefStepper, RQstInd: RQstInd });
            Qitem.Answer = fname;
        },

        async saveSign(QID, IsBenefQstn, benefIndx, QItem) {

            var prevAns = "";
            var signPad = "";
            if (IsBenefQstn) {

                this.BTAnsBlobQID = QID;
                prevAns = this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer;
                if (prevAns != null)
                    this.delListBT.push({ QuestionID: QID, Answer: prevAns });

                signPad = this.$refs.signaturePad1[0].signaturePad;
            }
            else {

                this.AnsBlobQID = QID;
                prevAns = QItem.Answer;
                if (prevAns != null && prevAns != "")
                    this.delList.push({ QuestionID: QID, Answer: prevAns });

                //Filter the Question Based on Signature Question Type
                var SignatureQstnList = this.lstSectionsQstns.filter(x => x.SectionID == QItem.SectionID)[0].Questions.filter(x => x.QuestionType == "Signature Capture");
                // Getting the index of the signature question
                const Qindex = SignatureQstnList.findIndex((element, index) => {
                    if (element.QuestionID === this.AnsBlobQID) {
                        return true
                    }
                })
                signPad = this.$refs.signaturePad[Qindex].signaturePad;
                // signPad = this.$refs.QItem.signaturePad[0].signaturePad;
            }


            if (signPad.isEmpty()) {
                this.Msg = "Please provide a signature first.";
                this.infoSnackbarMsg = true;
                return;
            }
            else {

                var dataURL = signPad.toDataURL("image/png");
                var data = dataURL.split(',')[1];
                let blobFile = await this.b64toBlob(data, 'image/png');
                this.SaveFile(QItem, "Signature", blobFile, IsBenefQstn, false, benefIndx);
            }
        },

        async SaveSignatureRepeatOrSkip(QItem, IsBenefQstn, benefIndx, RQstInd) {
            var prevAns = "";
            var signPad = "";
            if (IsBenefQstn) {

                this.BTAnsBlobQID = QItem.QuestionID;
                prevAns = this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID)[0].Answer;
                if (prevAns != null)
                    this.delListBT.push({ QuestionID: QItem.QuestionID, Answer: prevAns });

                signPad = this.$refs.signaturePad1[0].signaturePad;
            }
            else {

                this.AnsBlobQID = QItem.QuestionID;
                prevAns = QItem.Answer;
                if (prevAns != null && prevAns != "")
                    this.delList.push({ QuestionID: QItem.QuestionID, Answer: prevAns });

                //Filter the Question Based on Signature Question Type
                var SignatureQstnList = this.lstSectionsQstns.filter(x => x.SectionID == QItem.SectionID)[0].Questions.filter(x => x.QuestionType == "Signature Capture");
                // Getting the index of the signature question
                const Qindex = SignatureQstnList.findIndex((element, index) => {
                    if (element.QuestionID === this.AnsBlobQID) {
                        return true
                    }
                })
                signPad = this.$refs.signaturePad[Qindex].signaturePad;
                // signPad = this.$refs.QItem.signaturePad[0].signaturePad;
            }


            if (signPad.isEmpty()) {
                this.Msg = "Please provide a signature first.";
                this.infoSnackbarMsg = true;
                return;
            }
            else {

                var dataURL = signPad.toDataURL("image/png");
                var data = dataURL.split(',')[1];
                let blobFile = await this.b64toBlob(data, 'image/png');
                this.SaveFileRepeat(QItem, "Signature", blobFile, IsBenefQstn, false, benefIndx, RQstInd);
            }
        },

        async clearSign(Qitem, IsBenefQstn, QID) {
            if (IsBenefQstn) {
                this.$refs.signaturePad1[0].signaturePad.clear();
                this.$refs.signaturePad1[0].signatureData = [];
            }

            else {
                //Filter the Question Based on Signature Question Type
                var SignatureQstnList = this.lstSectionsQstns.filter(x => x.SectionID == Qitem.SectionID)[0].Questions.filter(x => x.QuestionType == "Signature Capture");
                // Getting the index of the signature question
                const Qindex = SignatureQstnList.findIndex((element, index) => {
                    if (element.QuestionID === QID) {
                        return true
                    }
                })
                Qitem.uploadedSign = "";
                this.$refs.signaturePad[Qindex].signaturePad.clear();
                this.$refs.signaturePad[Qindex].signatureData = [];
            }
        },

        async b64toBlob(b64Data, contentType) {
            contentType = contentType || '';
            var sliceSize = 512;
            var byteCharacters = atob(b64Data);
            var byteArrays = [];

            for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                var slice = byteCharacters.slice(offset, offset + sliceSize);

                var byteNumbers = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                var byteArray = new Uint8Array(byteNumbers);

                byteArrays.push(byteArray);
            }

            var blob = new Blob(byteArrays, { type: contentType });
            return blob;
        },


        async FileUpload(pblobFileInput, FileName, IsBenefQstn, isUploadedFile, BeneficiaryID, QuestionID) {
            if (pblobFileInput != null) {
                if (IsBenefQstn) {
                    await dbImworks.TbTaskBenefRegDocument.put({

                        SyncTaskID: this.lstSelectedTaskData.SyncDBID, TaskDate: this.lstSelectedTaskData.TaskDate, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0,
                        AttachedDocument: pblobFileInput, FileName: FileName, DocumentTypeIcon: FileName.split('.').pop(),
                        ProjectID: this.lstSelectedTaskData.ProjectId, DocLinkedTo: this.lstSelectedTaskData.TaskName, IsUploadedFile: isUploadedFile, QuestionID: QuestionID, clientguid: this.clientguid
                    });
                    this.blobFileInputBT = null;
                }
                else {
                    await dbImworks.TbTaskResponseDocument.put({

                        SyncTaskID: this.lstSelectedTaskData.SyncDBID, TaskDate: this.lstSelectedTaskData.TaskDate, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0,
                        AttachedDocument: pblobFileInput, FileName: FileName, DocumentTypeIcon: FileName.split('.').pop(),
                        ProjectID: this.lstSelectedTaskData.ProjectId, DocLinkedTo: this.lstSelectedTaskData.TaskName, IsUploadedFile: isUploadedFile,
                        BeneficiaryID: BeneficiaryID, QuestionID: QuestionID, clientguid: this.clientguid
                    });
                    this.blobFileInput = null;
                }

            }
        },

        //************** Audio and Photo Capture Methods ends here


        async fnCheckMandatory() {

            var ValidationReturnValue = false;
            var IsValid = true;
            var SectionQstnDetails = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
            if (SectionQstnDetails.length > 0) {
                for (var SecCnt = 0; SecCnt <= SectionQstnDetails.length - 1; SecCnt++) {
                    if (SectionQstnDetails[SecCnt].IslogicSection == 0) { // if Its not a Logic Section

                        ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(SectionQstnDetails[SecCnt].Questions, this.BenefStepper, false)
                        if (ValidationReturnValue == true) {
                            IsValid = false;
                        }
                    }
                    else if (SectionQstnDetails[SecCnt].IslogicSection == 1) {
                        // Logic Section Validations
                        ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(this.RepeatOrSkipSection, this.BenefStepper, true)
                        if (ValidationReturnValue == true) {
                            IsValid = false;
                        }
                    }
                }
            }

            if (IsValid == false) {
                this.dlgValidationMessage = true;
                return false;
            }
            return true;
        },

        async fnCheckMandatoryNonBenefLinked() {

            var ValidationReturnValue = false;
            var IsValid = true;
            var SectionQstnDetails = this.lstSectionsQstns;
            if (SectionQstnDetails.length > 0) {
                for (var SecCnt = 0; SecCnt <= SectionQstnDetails.length - 1; SecCnt++) {

                    if (SectionQstnDetails[SecCnt].IslogicSection == 0) { // if Its not a Logic Section

                        ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(SectionQstnDetails[SecCnt].Questions, this.BenefStepper, false)
                        if (ValidationReturnValue == true) {
                            IsValid = false;
                        }
                    }
                    else if (SectionQstnDetails[SecCnt].IslogicSection == 1) {
                        // Logic Section Validations
                        ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(this.RepeatOrSkipSection, this.BenefStepper, true)
                        if (ValidationReturnValue == true) {
                            IsValid = false;
                        }
                    }
                }
            }

            if (IsValid == false) {
                this.dlgValidationMessage = true;
                return false;
            }
            return true;
        },

        async fnSaveAndExit() {
            await this.fnStoreCurrentBenefQstnAns();


            var today = new Date().toLocaleString("en-US");

            // To Get Survey Answers
            var lstAnswers = [];
            var Questions = [];


            // Delete Saved Data if Exists
            if (this.lstSelectedTaskData.Periodicity != "Anytime") {
                var pkId = [];
                pkId = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();
                if (pkId.length > 0) {
                    await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }
                pkId = [];
                pkId = await dbImworks.TbMyTask.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();
                if (pkId.length > 0) {
                    await dbImworks.TbMyTask.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }
            }
            else if (this.lstSelectedTaskData.Periodicity == "Anytime") {

                var pkId = [];
                pkId = await dbImworks.SavedTaskDB.where({ AnytimeSubmissionID: this.AnyTimeSubmissionId }).toArray();
                if (pkId.length > 0) {
                    await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }
                pkId = [];
                pkId = await dbImworks.TbMyTask.where({ AnytimeSubmissionID: this.AnyTimeSubmissionId }).toArray();
                if (pkId.length > 0) {
                    await dbImworks.TbMyTask.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }
            }

            if (this.IsBeneficiaryLinkedSurvey == true) {
                for (var BenCnt = 0; BenCnt <= this.BeneficiarySectionQstnList.length - 1; BenCnt++) {

                    var Sections = this.BeneficiarySectionQstnList[BenCnt].Sections;
                    for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                        Questions = [];
                        if (Sections[SecCnt].IslogicSection == 1) {

                            Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.benefIndex == Sections[SecCnt].BenefIndex && x.QuestionID > 0)

                        }
                        else {

                            Questions = Sections[SecCnt].Questions;
                        }
                        for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) {
                            var Obj = new Object();
                            Obj.ID = 0
                            Obj.SurveyID = Questions[QCnt].SurveyID;
                            Obj.SectionID = Questions[QCnt].SectionID;
                            Obj.ResponseID = Questions[QCnt].BeneficiaryID;
                            Obj.SyncTaskID = 0;
                            Obj.QuestionID = Questions[QCnt].QuestionID;
                            Obj.QuestionName = Questions[QCnt].QuestionName;
                            Obj.QuestionType = Questions[QCnt].QuestionType;
                            Obj.Answer = Questions[QCnt].Answer;
                            //Obj.CreatedDate = Questions[QCnt].CreatedDate;
                            //Obj.UpdatedDate = Questions[QCnt].UpdatedDate;
                            lstAnswers.push(Obj);
                        }
                    }
                }
            }
            else {

                var Sections = this.lstSectionsQstns;
                for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                    Questions = [];
                    if (Sections[SecCnt].IslogicSection == 1) {
                        Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.QuestionID > 0)
                    }
                    else {
                        Questions = Sections[SecCnt].Questions;
                    }
                    for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) {
                        var Obj = new Object();
                        Obj.ID = 0
                        Obj.SurveyID = Questions[QCnt].SurveyID;
                        Obj.SectionID = Questions[QCnt].SectionID;
                        Obj.ResponseID = Questions[QCnt].BeneficiaryID;
                        Obj.SyncTaskID = 0;
                        Obj.QuestionID = Questions[QCnt].QuestionID;
                        Obj.QuestionName = Questions[QCnt].QuestionName;
                        Obj.QuestionType = Questions[QCnt].QuestionType;
                        Obj.Answer = Questions[QCnt].Answer;
                        //Obj.CreatedDate = Questions[QCnt].CreatedDate;
                        //Obj.UpdatedDate = Questions[QCnt].UpdatedDate;
                        lstAnswers.push(Obj);
                    }
                }
            }

            var SurveyAnswers = JSON.stringify(lstAnswers);

            await dbImworks.SavedTaskDB.put({
                SyncDBID: this.lstSelectedTaskData.SyncDBID, Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn, WorkflowID: this.lstSelectedTaskData.WorkflowID,
                TotalWFLevel: this.lstSelectedTaskData.TotalWFLevel, WFLevel: 0, Status: 'Partially DataCollected', SurveyQstns: JSON.stringify(this.lstSelectedTaskData.TaskData.SurveyQstnList),
                SurveyAnswers: SurveyAnswers, ResponseSyncID: this.lstSelectedTaskData.ResponseSyncID, DataCollectedBy: this.lstSelectedTaskData.DataCollectedBy, NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                SelectedBeneficiaries: JSON.stringify(this.vmSelectedBeneficiaries), TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, RepeatOrSkipSection: JSON.stringify(this.RepeatOrSkipSection), CascadingAnsList: JSON.stringify(this.cascadingAnsArray), MultiChoiceAnsList: JSON.stringify(this.multiChoiceAns), MultiChoiceAnsListRepeat: JSON.stringify(this.MultiChoiceAnsForRepeat), AnytimeSubmissionID: this.AnyTimeSubmissionId,
                lstSelectedBeneficiary: JSON.stringify(this.lstSelectedBeneficiary)
            });


            var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();


            var lastClientSyncID = await dbImworks.SavedTaskDB.orderBy('pkId').last();
            await dbImworks.TbMyTask.put({
                ClientSyncTaskID: lastClientSyncID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0, WFLevelStatus: "Saved",
                TaskUpdateComment: this.vmUpdateComment, AttachedDocumentName: '', SyncDBID: this.lstSelectedTaskData.SyncDBID,
                ProjectName: '', TaskName: '', Description: '',
                CreatedDate: today, TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, Periodicity: '', TaskSubmittedBy: data[0].UserName, BeneficiarySubmittedList: JSON.stringify(this.vmSelectedBeneficiaries), AnytimeSubmissionID: this.AnyTimeSubmissionId, ToDelete: 1
            });


            if (this.AnyTimeSubmissionId == 0) {
                // Updating Current Tasks Status
                var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog.filter(y => y.TaskIndex == this.SelectedTaskIndex)[0]

                SelectedTaskDetails.CurrentOrOverdue = "Saved"
                SelectedTaskDetails.CreatedDate = today;
                SelectedTaskDetails.TaskStatus = "To Data Collect";
                //SelectedTaskDetails.Message = "Survey Data Partialy updated";
                SelectedTaskDetails.Comment = this.vmUpdateComment;
                SelectedTaskDetails.Name = data[0].UserName;

                var selectedCardData = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0];
                selectedCardData.btnAnytimeSubVisible = true;

            }
            else if (this.AnyTimeSubmissionId > 0) {
                // Update Status to any time dexie table
                var AnyTimeData = this.lstAnyTimeNewSubmission.filter(x => x.pkId == this.AnyTimeSubmissionId)[0];
                AnyTimeData.TaskData.CurrentOrOverdue = "Saved";
                AnyTimeData.TaskData.CreatedDate = today;
                AnyTimeData.TaskData.Status = "To Data Collect";
                //AnyTimeData.TaskData.Message = "Survey Data Partialy updated";
                AnyTimeData.TaskData.Comment = this.vmUpdateComment;
                AnyTimeData.TaskData.Name = data[0].UserName;

                var pkId = [];
                pkId = await dbImworks.TbAnyTimeData.where({ pkId: parseInt(this.AnyTimeSubmissionId) }).toArray();
                if (pkId.length > 0) {
                    await dbImworks.TbAnyTimeData.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }

                //var data = this.TaskCardData.filter(x => x.TaskType == this.SelectedTaskType && x.TypeID == this.SelectedTaskTypeID)[0].TaskLog;
                await dbImworks.TbAnyTimeData.put({
                    TaskData: AnyTimeData.TaskData
                });

                var lastAnytimeID = await dbImworks.TbAnyTimeData.orderBy('pkId').last();
                AnyTimeData.pkId = lastAnytimeID.pkId;
            }

            //this.SelectedTaskIndex;
            //this.SurveyBenLinkDlg = false;
            await this.fnClearData();
            this.SnackbarMsg = "Data saved successfully"
            this.InfoSnackbarMsg = true;
        },
        async fnProccedToUpdate() {

            if (this.SelectedTaskStatus != "Rejected" && this.SelectedTaskStatus != "Saved") {
                if ((this.beneficiaryStepperList.length < this.vmSelectedBeneficiaries.length) && this.IsBeneficiaryLinkedSurvey == true) {
                    this.SnackbarMsg = "Please fill all beneficiary details";
                    this.InfoSnackbarMsg = true;
                    return false;
                }
            }
            if (await this.fnCheckMandatory()) {


                this.IsImportSurvey = false;
                await this.fnStoreCurrentBenefQstnAns();
                await this.fnCalculateProgress();
                this.BenefLinkedSurveySteps = 5;
                //this.NonBenefLinkedSurveySteps = 3;
            }

        },

        async fnNonBenefProceedToUpdate() {
            if (await this.fnCheckMandatoryNonBenefLinked()) {
                await this.fnStoreCurrentBenefQstnAns();
                await this.fnCalculateProgress();
                this.NonBenefLinkedSurveySteps = 3;
            }
        },



        // Check mandatory and Validation, Condition before save for Survey Questions
        async fnCheckMandatoryQuestionValidationAndCondition(item, BenefIndex, IsLogicSection) {

            // Checking for mandatory questions
            var lstMandatoryQuestion = item.filter(x => x.IsMandatory == 1);
            //  Checking for Validation Question Min and Max
            var lstValidationQuestion = item.filter(x => x.IsValidationQuestion == 1);
            var lstRegEx = item.filter(x => (x.RegEx != null && x.RegEx.length > 0));

            var IsValid = false;
            if (lstMandatoryQuestion.length > 0) {
                // Selecting from Mandatory Questions without answer
                var IsQAnswers = lstMandatoryQuestion.filter(x => x.Answer == "");
                var QstnWithAnswer = lstMandatoryQuestion.filter(x => x.Answer != "");

                QstnWithAnswer.forEach(function (part, index) {
                    QstnWithAnswer[index].ValidationErrorTest = "";
                    QstnWithAnswer[index].IsValidationError = false;
                });

                //validation for gps question and Answers
                var gpsQuestions = lstMandatoryQuestion.filter(x => x.QuestionTypeID == 9)
                for (var n = 0; n <= gpsQuestions.length - 1; n++) {
                    if (gpsQuestions[n].QuestionType == "GPS Capture") {
                        var gpsObj = gpsQuestions[n].Answer.split(',');
                        if (gpsObj[0] == 0 || gpsObj[1] == 0) {
                            item.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].ValidationErrorTest = "Required Field";
                            item.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].IsValidationError = true;
                            IsValid = true;
                        }
                        else {
                            item.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].ValidationErrorTest = "";
                            item.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].IsValidationError = false;
                        }
                    }
                }
                if (IsQAnswers.length > 0) {
                    for (var i = 0; i <= IsQAnswers.length - 1; i++) {
                        if (IsQAnswers[i].QuestionType != "Cascading Dropdown" && IsQAnswers[i].QuestionType != "Photo Capture" && IsQAnswers[i].QuestionType != "Multi-Choice" && IsQAnswers[i].QuestionType != "GPS Capture") {
                            item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                            item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                            IsValid = true;
                        }
                        else if (IsQAnswers[i].QuestionType == "Cascading Dropdown") {

                            var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false) && (x.benefIndx == BenefIndex));
                            if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)) {
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                            }
                        }
                        else if (IsQAnswers[i].QuestionType == "Multi-Choice") {

                            var SelectedmultiChoiceAns = "";
                            if (IsLogicSection == true) {
                                SelectedmultiChoiceAns = this.MultiChoiceAnsForRepeat.filter(x => x.QuestionID == IsQAnswers[i].QuestionID && x.benefIndex == this.BenefStepper)
                            }
                            else {
                                SelectedmultiChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID && x.benefIndex == this.BenefStepper)
                            }

                            if (typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0) {
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                            }
                        }
                        else if (IsQAnswers[i].QuestionType == "Photo Capture") {

                            if (typeof IsQAnswers[i].AnsBlob != "undefined") {
                                if (IsQAnswers[i].AnsBlob.name == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                            else {
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                item.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                        }
                        else {
                            this.lstSectionQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                            this.lstSectionQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                        }
                    }
                    //
                }
            }

            // Checking Question Validation and Conditions

            if (lstValidationQuestion.length > 0) {

                var IsQAnswers = lstValidationQuestion.filter(x => x.Answer != "");
                var valConditions
                var NewErrorMsg = "";
                var OldErrorMsg = "";
                var today = new Date().toLocaleDateString('en-GB');
                for (var intValQstCnt = 0; intValQstCnt <= IsQAnswers.length - 1; intValQstCnt++) {
                    valConditions = IsQAnswers[intValQstCnt].QuestionCondition;
                    for (var i = 0; i < valConditions.length; i++) {
                        if (valConditions[i].ValidationCondition == 'Min & Max Range') {

                            if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue))) {

                                NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;

                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;

                            } else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }
                        if (valConditions[i].ValidationCondition == "Should be less than Current date") {

                            if (IsQAnswers[intValQstCnt].Answer > this.FormatDateTodayDate(today)) {
                                NewErrorMsg = "Should be less than Current date";
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }
                        if (valConditions[i].ValidationCondition == "Should be greater than Current date") {

                            if (IsQAnswers[intValQstCnt].Answer < this.FormatDateTodayDate(today)) {
                                NewErrorMsg = "Should be greater than Current date";
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }
                        // Checking Phone number validation
                        if (valConditions[i].ValidationCondition == "Phone Number") {

                            var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                            if (!IsQAnswers[intValQstCnt].Answer.match(phoneNum)) {

                                NewErrorMsg = "Please enter a valid Phone Number";
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }
                        // Checking Phone number validation
                        if (valConditions[i].ValidationCondition == "Email") {

                            var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                            if (!IsQAnswers[intValQstCnt].Answer.match(email)) {
                                NewErrorMsg = "Please enter a valid Email";
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }

                        // Checking aadhar number validation
                        if (valConditions[i].ValidationCondition == "Aadhaar Number") {
                            var aadharno = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                            if (IsQAnswers[intValQstCnt].Answer != null && IsQAnswers[intValQstCnt].Answer != undefined && !IsQAnswers[intValQstCnt].Answer.match(aadharno)) {
                                NewErrorMsg = "Please enter a valid Aadhaar Number";
                                OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                if (OldErrorMsg == "") {
                                    item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                } else { item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                this.qRules = "";
                                //this.chkQstnValid = true;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.chkQstnValid = false;
                                item.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                            }
                        }

                    }
                }
            }

            if (lstRegEx != null && lstRegEx != undefined && lstRegEx.length > 0) {

                var IsQAnswers1 = lstRegEx.filter(x => x.Answer != "");

                for (var intValQstCnt1 = 0; intValQstCnt1 <= IsQAnswers1.length - 1; intValQstCnt1++) {
                    var valRegEx = new RegExp(IsQAnswers1[intValQstCnt1].RegEx);
                    // console.log(valRegEx);
                    if (IsQAnswers1[intValQstCnt1].Answer != null && IsQAnswers1[intValQstCnt1].Answer != undefined && !IsQAnswers1[intValQstCnt1].Answer.match(valRegEx)) {

                        var NewErrorMsg = "Value does not match regular expression";
                        var OldErrorMsg = item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ErrorText;
                        item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].qRules = "";
                        if (OldErrorMsg == "") {
                            item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                        } else { item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ValidationErrorTest = OldErrorMsg }

                        item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].IsValidationError = true;
                        IsValid = true;
                    } else {
                        this.chkQstnValid = false;
                        item.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].IsValidationError = false
                    }
                }
            }

            return IsValid;
        },

        //// Check mandatory and Validation, Condition before save for Beneficiary Registration
        async fnCheckBeneficiaryMandatoryAndValidation() {


            var IsValid = false;
            if (this.lstBenefRegQstns.length > 0) {
                // Checking for mandatory questions
                var lstMandatoryQuestion = this.lstBenefRegQstns.filter(x => x.IsMandatoryResponse == 1);
                //  Checking for Validation Question Min and Max
                var lstValidationQuestion = this.lstBenefRegQstns.filter(x => x.IsValidationQuestion == 1 && (x.BTQstnValidationList != null));

                var lstRegEx = this.lstBenefRegQstns.filter(x => (x.RegexExpression != null && x.RegexExpression.length > 0));
                // console.log(this.lstBenefRegQstns);
                if (lstMandatoryQuestion.length > 0) {
                    // Selecting from Mandatory Questions without answer
                    var IsQAnswers = lstMandatoryQuestion.filter(x => x.Answer == "");
                    var QstnWithAnswer = lstMandatoryQuestion.filter(x => x.Answer != "");

                    QstnWithAnswer.forEach(function (part, index) {
                        QstnWithAnswer[index].ValidationErrorTest = "";
                        QstnWithAnswer[index].IsValidationError = false;
                    });

                    //validation for gps question and Answers
                    var gpsQuestions = lstMandatoryQuestion.filter(x => x.QuestionTypeID == 9)
                    for (var n = 0; n <= gpsQuestions.length - 1; n++) {
                        if (gpsQuestions[n].QuestionType == "GPS Capture") {
                            var gpsObj = gpsQuestions[n].Answer.split(',');
                            if (gpsObj[0] == 0 || gpsObj[1] == 0) {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].ValidationErrorTest = "Required Field";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].ValidationErrorTest = "";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == gpsQuestions[n].QuestionID)[0].IsValidationError = false;
                            }
                        }
                    }


                    for (var i = 0; i <= IsQAnswers.length - 1; i++) {
                        if (IsQAnswers[i].QuestionType != "Cascading Dropdown" && IsQAnswers[i].QuestionType != "Photo Capture" && IsQAnswers[i].QuestionType != "Multi-Choice") {
                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                            IsValid = true;
                        }
                        else if (IsQAnswers[i].QuestionType == "Cascading Dropdown") {
                            var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == true));
                            if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)) {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                            }
                        }
                        else if (IsQAnswers[i].QuestionType == "Multi-Choice") {

                            var SelectedmultiChoiceAns = this.multiChoiceAnsBT.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)
                            if (typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0) {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                            }
                        }
                        else if (IsQAnswers[i].QuestionType == "Photo Capture") {

                            if (typeof IsQAnswers[i].AnsBlob != "undefined") {
                                if (IsQAnswers[i].AnsBlob.name == "") {
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                            else {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                        }
                        else {
                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                        }
                    }
                }
                // Validations Checking
                if (lstValidationQuestion.length > 0) {

                    lstValidationQuestion.forEach(function (part, index) {
                        lstValidationQuestion[index].ValidationErrorTest = "";
                        lstValidationQuestion[index].IsValidationError = false;
                    });
                    var IsQAnswers = lstValidationQuestion.filter(x => x.Answer != "" && x.Answer != null);
                    var valConditions
                    var NewErrorMsg = "";
                    var OldErrorMsg = "";
                    var today = new Date().toLocaleDateString('en-GB');
                    for (var intValQstCnt = 0; intValQstCnt <= IsQAnswers.length - 1; intValQstCnt++) {
                        valConditions = IsQAnswers[intValQstCnt].BTQstnValidationList;
                        for (var i = 0; i < valConditions.length; i++) {
                            if (valConditions[i].ValidationCondition == 'Min & Max Range') // if Starts here
                            {
                                if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue))) {
                                    NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }


                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;

                                }
                                else {
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }// if ends here
                            if (valConditions[i].ValidationCondition == "Should be less than Current date") {
                                if (IsQAnswers[intValQstCnt].Answer > this.FormatDateTodayDate(today)) {
                                    NewErrorMsg = "Should be less than Current date";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    this.qRules = "";
                                    //this.chkQstnValid = true;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }// if ends here
                            if (valConditions[i].ValidationCondition == "Should be greater than Current date") {
                                if (IsQAnswers[intValQstCnt].Answer < this.FormatDateTodayDate(today)) {
                                    NewErrorMsg = "Should be greater than Current date";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            } // if ends here
                            // Checking Phone number validation
                            if (valConditions[i].ValidationCondition == "Phone Number") {
                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                if (!IsQAnswers[intValQstCnt].Answer.match(phoneNum)) {

                                    NewErrorMsg = "Please enter a valid Phone Number";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }
                            // Checking email  validation
                            if (valConditions[i].ValidationCondition == "Email") {

                                var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                if (!IsQAnswers[intValQstCnt].Answer.match(email)) {
                                    NewErrorMsg = "Please enter a valid Email";
                                    //OldErrorMsg = this.lstBTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }

                            // Checking Aadhaar number validation
                            if (valConditions[i].ValidationCondition == "Aadhaar Number") {
                                var aadhar = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/
                                if (IsQAnswers[intValQstCnt].Answer != null && IsQAnswers[intValQstCnt].Answer != undefined && !IsQAnswers[intValQstCnt].Answer.match(aadhar)) {

                                    NewErrorMsg = "Please enter a valid Addhar Number";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                    if (OldErrorMsg == "") {
                                        this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }

                        }
                    }
                }

                // console.log(lstRegEx.length);
                // for REGULAR EXPRESSION
                if (lstRegEx != null && lstRegEx != undefined && lstRegEx.length > 0) {
                    var IsQAnswers1 = lstRegEx.filter(x => x.Answer != "");
                    //console.log(IsQAnswers1.length);
                    for (var intValQstCnt1 = 0; intValQstCnt1 <= IsQAnswers1.length - 1; intValQstCnt1++) {
                        var valRegEx = new RegExp(IsQAnswers1[intValQstCnt1].RegexExpression);
                        //console.log(valRegEx);

                        if (IsQAnswers1[intValQstCnt1].Answer != null && IsQAnswers1[intValQstCnt1].Answer != undefined && !IsQAnswers1[intValQstCnt1].Answer.match(valRegEx)) {

                            var NewErrorMsg = "Value does not match regular expression";
                            var OldErrorMsg = this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ValidationErrorTest;

                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].qRules = "";
                            if (OldErrorMsg == "") {
                                this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                            } else { this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].ValidationErrorTest = OldErrorMsg }

                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].IsValidationError = true;
                            IsValid = true;
                        } else {
                            this.lstBenefRegQstns.filter(x => x.QuestionID == IsQAnswers1[intValQstCnt1].QuestionID)[0].IsValidationError = false
                        }
                    }
                }
            }

            return IsValid;
        },

        // Function Used to Filter Cascading dropdown based on parent cascading dropdown
        async FilterCascadingItems(QItem, OptItem, parentID, IsBenefQstn, benefIndx, RepeatSectionIndexID) {

            //RepeatSectionIndexID ---> shows the index of a question inside a repeat section (variable array name --> RepeatSection)
            // if it is not in repeat section RepeatSectionIndexID =-1, if yes RepeatSectionIndexID >=0
            var sublist = [];

            var selectedAnsOption = parentID > 0 ? OptItem.CascadingList.filter(x => x.ID == parentID)[0].Name : "";
            var nextHierarchyOrder = OptItem.HierarchyOrder + 1;

            var ifExists = this.cascadingAnsArray.filter(x => (x.QID == QItem.QuestionID) && (x.IsBenefQstn == IsBenefQstn)
                && (x.HierarchyOrder == OptItem.HierarchyOrder) && (x.benefIndx == benefIndx) && (x.RepeatSectionIndex == RepeatSectionIndexID));

            if (ifExists.length > 0) {
                var index = 0;
                for (var i = 0; i <= this.cascadingAnsArray.length - 1; i++) {
                    if (this.cascadingAnsArray[i].QID == QItem.QuestionID && this.cascadingAnsArray[i].IsBenefQstn == IsBenefQstn && this.cascadingAnsArray[i].HierarchyOrder == OptItem.HierarchyOrder && this.cascadingAnsArray[i].benefIndx == benefIndx && (this.cascadingAnsArray[i].RepeatSectionIndex == RepeatSectionIndexID)) {
                        index = i;
                        break;
                    }
                }
                this.cascadingAnsArray.splice(this.cascadingAnsArray.indexOf(this.cascadingAnsArray[index]), 1);
                this.cascadingAnsArray = [...this.cascadingAnsArray];
            }
            this.cascadingAnsArray.push({ QID: QItem.QuestionID, IsBenefQstn: IsBenefQstn, HierarchyOrder: OptItem.HierarchyOrder, Ans: selectedAnsOption, benefIndx: benefIndx, RepeatSectionIndex: RepeatSectionIndexID });


            // var len = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.lstSectionQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length;
            var len;
            if (RepeatSectionIndexID == -1) // if the cascading question is not comes under a repeat section , then the value of RepeatSectionIndexID == -1
            {
                //IsBenefQstn == true shows cascading list from Beneficiary Registration
                //  if(IsBenefQstn == true)
                // {
                //  len = this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length
                // }
                // else
                //{
                len = QItem.CascadingQuestionOptions[QItem.CascadingQuestionOptions.length - 1].HierarchyOrder;
                //}
            }
            else {
                // Fetching the count/ length of the cascading question option using RepeatSectionIndexID
                //len = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.RepeatSection.filter(x => x.QuestionID == QItem.QuestionID && x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.length;
            }
            if (len >= nextHierarchyOrder) {
                var nxtList = [];
                if (RepeatSectionIndexID == -1) // if the cascading question is not comes under a repeat section , then the value of RepeatSectionIndexID == -1
                {

                    if (IsBenefQstn == true) {
                        nxtList = this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                    }
                    else {
                        nxtList = QItem.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                    }
                }
                else {
                    //----- In the case of repeat section ---------------
                    //nxtList = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder) : this.RepeatSection.filter(x => x.QuestionID == QItem.QuestionID && x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
                }

                if (nxtList != null && nxtList.length != 0) {
                    if (nxtList[0].OptionValue == "States") {
                        sublist = this.lstStates.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                    }
                    else if (nxtList[0].OptionValue == "District") {
                        sublist = this.lstDistrict.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                    }
                    else if (nxtList[0].OptionValue == "SubDistrict") {
                        sublist = this.lstSubDistrict.filter(x => x.ParentID == parentID).sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                    }
                    if (IsBenefQstn == true) {
                        this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.
                            filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                    }
                    else {
                        QItem.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                    }

                }

            }

        },
        // To Get Multi-choice Question's Selected Answers for beneficiary
        async GetMultiChoice_BT(optItem, optSelected, optVal) {

            if (optSelected) {
                this.multiChoiceAnsBT.push(optVal);
            } else {
                this.multiChoiceAnsBT.splice(this.multiChoiceAnsBT.indexOf(optItem), 1);
            }
        },
        async SaveAudioBT(data) {

            this.blobFileInputBT = data.blob;
            var item = this.lstBenefRegQstns.filter(x => x.QuestionID == this.BTAnsBlobQID) //BTAnsBlobQID
            this.SaveFile(item, "Audio", this.blobFileInputBT, true, false, 0);
        },
        // To Get Multi-choice Question Selected Answers
        async GetMultiChoice(optItem, optSelected, optVal, QuestionID, benefIndex) {
            if (optSelected) {
                this.multiChoiceAns.push({ OptionValue: optVal, QuestionID: QuestionID, benefIndex: benefIndex });
            }
            else {
                const index = this.multiChoiceAns.findIndex((element, index) => {
                    if (element.QuestionID === optItem.QuestionID && element.OptionValue == optVal && element.benefIndex == benefIndex) { return true }
                });
                this.multiChoiceAns.splice(index, 1);
            }
        },

        // Function used to Check Section Logic (Repeat And Skip) and Logic Sections and Questions
        async AddSectionLogic(item, QID, benefIndex) {

            var IsLogicCondition = false;
            var QInput = "";
            var SecID;
            var SectionIndex = -1;
            if (typeof item != 'undefined') {
                QInput = item.Answer;
                SecID = item.SectionID;
            }

            if (typeof QInput != 'undefined' && QInput != null) {
                if (item.IsConditionQuestion) {
                    var SectionLogic = item.SectionLogic.filter(x => x.QuestionID == item.QuestionID);

                    if (SectionLogic != null) {
                        if (SectionLogic[0].LogicType == "Skip Logic") {
                            var IsANDoRLogic = SectionLogic.filter(x => x.LogicOperator == "AND" || x.LogicOperator == "OR");
                            if (IsANDoRLogic.length == 0) {
                                SectionLogic = SectionLogic.filter(x => x.QuestionID == QID);
                            }

                            SectionLogic.sort(function (a, b) {
                                return a.SkipSectionID - b.SkipSectionID;
                            }
                            );
                            // New Skip Logic Code
                            var IsLogicSatisfied = false;
                            var SkipSectionID = 0;
                            for (var LogicCnt = 0; LogicCnt <= SectionLogic.length - 1; LogicCnt++) {
                                SkipSectionID = SectionLogic[LogicCnt].SkipSectionID;
                                if (SectionLogic[LogicCnt].ConditionValue == "=") {
                                    SectionLogic[LogicCnt].ConditionValue = "==";
                                }
                                // Get the Input Given by the user
                                //var InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.QuestionID == QID));
                                var InputAnswer = item.Answer;//this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0].Answer;
                                //if (typeof this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0] != 'undefined') {
                                //    InputAnswer = this.lstSectionQstns.filter(x => x.QuestionID == SectionLogic[LogicCnt].QuestionID)[0].Answer;
                                //}
                                //if (InputAnswer == "" && typeof this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0] != 'undefined') {
                                //    InputAnswer = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndex) && (x.QuestionID == SectionLogic[LogicCnt].QuestionID))[0].Answer;
                                //}
                                var InputStringAnswer = SectionLogic[LogicCnt].Answer;
                                // checking the first condition
                                if (LogicCnt == 0) {
                                    if (InputAnswer != "") {
                                        if (isNaN(InputAnswer)) // is a string
                                        {
                                            InputAnswer = "'" + InputAnswer + "'";
                                            InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                        }
                                        else {
                                            InputAnswer = InputAnswer;
                                        }
                                        if (eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer)) {
                                            IsLogicSatisfied = true;
                                            SectionIndex = LogicCnt;
                                        }
                                        else {
                                            IsLogicSatisfied = false;
                                            SectionIndex = -1;
                                        }
                                    }
                                    else {
                                        IsLogicSatisfied = false;
                                        SectionIndex = -1;
                                    }
                                }
                                else {
                                    var LogicOperator = SectionLogic[LogicCnt].LogicOperator; // the variable shows AND Or OR (&& , ||)
                                    if (LogicOperator == "AND") {
                                        if (IsLogicSatisfied == true) {
                                            if (InputAnswer != "") {
                                                if (isNaN(InputAnswer)) // is a string
                                                {
                                                    InputAnswer = "'" + InputAnswer + "'";
                                                    InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                                }
                                                else {
                                                    InputAnswer = InputAnswer;
                                                }
                                                if (eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer)) {
                                                    IsLogicSatisfied = true;
                                                    SectionIndex = LogicCnt;
                                                }
                                                else {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = -1;
                                                }
                                            }
                                            else {
                                                IsLogicSatisfied = false;
                                                SectionIndex = -1;
                                            }
                                        }
                                    }
                                    if (LogicOperator == "OR") {
                                        if (IsLogicSatisfied == false) {
                                            if (InputAnswer != "") {
                                                if (isNaN(InputAnswer)) // is a string
                                                {
                                                    InputAnswer = "'" + InputAnswer + "'";
                                                    InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                                }
                                                else {
                                                    InputAnswer = InputAnswer;
                                                }
                                                if (eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer)) {
                                                    IsLogicSatisfied = true;
                                                    SectionIndex = LogicCnt;
                                                }
                                                else {
                                                    IsLogicSatisfied = false;
                                                    SectionIndex = -1;
                                                }
                                            }
                                            else {
                                                IsLogicSatisfied = false;
                                                SectionIndex = -1;
                                            }
                                        }
                                    }
                                    // if there no logic operator
                                    if (LogicOperator == "" && LogicCnt > 0 && IsLogicSatisfied == false) {
                                        // if(IsLogicSatisfied == false)
                                        // {
                                        if (InputAnswer != "") {
                                            if (isNaN(InputAnswer)) // is a string
                                            {
                                                InputAnswer = "'" + InputAnswer + "'";
                                                InputStringAnswer = "'" + SectionLogic[LogicCnt].Answer + "'";
                                            }
                                            else {
                                                InputAnswer = InputAnswer;
                                            }
                                            if (eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer)) {
                                                IsLogicSatisfied = true;
                                                SectionIndex = LogicCnt;
                                            }
                                            else {
                                                IsLogicSatisfied = false;
                                                SectionIndex = -1;
                                            }
                                        }
                                        else {
                                            IsLogicSatisfied = false;
                                            SectionIndex = -1;
                                        }
                                        //}
                                    }
                                }
                                if (LogicCnt <= SectionLogic.length - 1) {
                                    var Count = LogicCnt;
                                    var AddLogic = false;
                                    if (Count == SectionLogic.length - 1) {
                                        AddLogic = true;
                                        Count = -1;
                                    }
                                    if (SkipSectionID != SectionLogic[Count + 1].SkipSectionID) {
                                        AddLogic = true;
                                    }

                                    if (AddLogic == true) {
                                        var QuestionID = QID;
                                        // Print Skip Section  id Starts
                                        if (IsLogicSatisfied == true) {

                                            if (SectionLogic[SectionLogic.length - 1].LogicOperator == "AND") // if Last logic condition is 'AND' then, Print the skip logic section under the last condition question
                                            {
                                                QuestionID = SectionLogic[SectionLogic.length - 1].QuestionID;
                                            }
                                            var SkipSection = "";
                                            if (SectionIndex >= 0) {
                                                SkipSection = SectionLogic[SectionIndex].SkipSectionID;
                                            }
                                            else {
                                                SkipSection = SectionLogic[0].SkipSectionID;
                                            }

                                            var IsExistSkipLogic = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID === QuestionID && x.benefIndex == benefIndex);
                                            for (var i = 0; i <= IsExistSkipLogic.length - 1; i++) {
                                                const index = this.RepeatOrSkipSection.findIndex((element, index) => {
                                                    if (element.ParentQuestionID === IsExistSkipLogic[i].ParentQuestionID && element.benefIndex == benefIndex) {
                                                        return true
                                                    }
                                                });
                                                this.RepeatOrSkipSection.splice(index, 1);
                                            }
                                            this.fnGetRepeatOrSkipSectionBasedOnQuestion(benefIndex, 1, QuestionID, SkipSection, 0);
                                        }
                                        else {
                                            //  this.RepeatSection =[];
                                            var IsExistSkipLogic = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID === QuestionID && x.benefIndex == benefIndex && x.SectionID == SkipSectionID);
                                            for (var i = 0; i <= IsExistSkipLogic.length - 1; i++) {
                                                const index = this.RepeatOrSkipSection.findIndex((element, index) => {
                                                    if (element.ParentQuestionID === IsExistSkipLogic[i].ParentQuestionID && element.benefIndex == benefIndex && element.SectionID == SkipSectionID) {
                                                        return true
                                                    }
                                                });
                                                this.RepeatOrSkipSection.splice(index, 1);
                                            }
                                            var SkipSection = SectionLogic[0].SkipSectionID;
                                            this.fnGetRepeatOrSkipSectionBasedOnQuestion(benefIndex, 0, QID, SkipSectionID, 0);
                                        }
                                        // if ends
                                    }
                                }
                                // For lop end
                            }

                        }
                        else {
                            if (SectionLogic[0].LogicType == "Repeat Logic") {
                                if (SectionLogic.length > 0) {
                                    for (var nCnt = 0; nCnt <= SectionLogic.length - 1; nCnt++) {
                                        var RepeatSectionID = SectionLogic[nCnt].SkipSectionID;
                                        this.fnGetRepeatOrSkipSectionBasedOnQuestion(benefIndex, parseInt(QInput), QID, RepeatSectionID, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },

        //Function used to check section logic for Multi-Choice Questions
        async AddSectionLogicForMultiChoice(item, QuestionID, benefIndex, QuestionOptions) {
            for (var SecCnt = 0; SecCnt <= this.lstSectionsQstns.length - 1; SecCnt++) {
                var Questions = this.lstSectionsQstns[SecCnt].Questions;
                if (typeof Questions.filter(x => x.QuestionID == QuestionID)[0] != 'undefined') {

                    if (Questions.filter(x => x.QuestionID == QuestionID)[0].IsConditionQuestion == 1) {

                        var SectionLogic = Questions.filter(x => x.QuestionID == QuestionID)[0].SectionLogic
                        SectionLogic = SectionLogic.filter(x => x.QuestionID == QuestionID);
                        if (SectionLogic.length > 0) {
                            var SelectedOption = QuestionOptions.filter(x => x.IsOptSelected == true);
                            var IsLogicExists = SectionLogic.filter(function (o1) {
                                return SelectedOption.some(function (o2) {
                                    return o1.Answer === o2.OptionValue; // return the ones with equal id
                                });
                            });
                            //var IsLogicExists = SectionLogic.filter(x=> x.Answer == item.OptionValue)
                            if (IsLogicExists.length > 0) {
                                this.lstSectionsQstns[SecCnt].Questions.filter(x => x.QuestionID == QuestionID)[0].Answer = IsLogicExists[0].Answer;
                                var item1 = this.lstSectionsQstns[SecCnt].Questions.filter(x => x.QuestionID == QuestionID)[0]
                                var IsExistInSkipLogic = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID == QuestionID && x.benefIndex == benefIndex);
                                if (IsExistInSkipLogic.length == 0) {
                                    await this.AddSectionLogic(item1, QuestionID, benefIndex);
                                }
                                this.lstSectionsQstns[SecCnt].Questions.filter(x => x.QuestionID == QuestionID)[0].Answer = "";
                            }
                            else {
                                this.lstSectionsQstns[SecCnt].Questions.filter(x => x.QuestionID == QuestionID)[0].Answer = ""
                                var item1 = this.lstSectionsQstns[SecCnt].Questions.filter(x => x.QuestionID == QuestionID)[0];
                                await this.AddSectionLogic(item1, QuestionID, benefIndex);
                            }
                        }
                    }

                    // }

                }
            }
        },
        //===========Question Calculation ====================//
        async Calculate(item, QID, benefIndx) {
            var resultVal = 0;
            var fieldVar = '';
            var lstfieldVar = [];
            var formulaExp = "";
            if (typeof item != 'undefined') {
                if (item.IsFormulaQuestion) {
                    var FormulaQuestion = item;

                    formulaExp = FormulaQuestion.FormulaExpression;

                    if (FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate' || FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Subtraction' || FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Average' || FormulaQuestion.FormulaOperation == 'RepeatVal_Aggregate_Multiplication') {
                        this.RepeatVal_Aggregate(item, QID, benefIndx, FormulaQuestion.FormulaOperation);
                        return false;
                    }
                    if (FormulaQuestion.FormulaFieldVariables != "") {
                        var arr = FormulaQuestion.FormulaFieldVariables.split(",");
                        if (arr.length == 1) {
                            //if (this.lstBenefSurveyQstnAnswers.length > 0) {
                            //    if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[0])).length > 0)
                            //        lstfieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[0]));
                            //}
                            for (var i = 0; i < lstfieldVar.length; i++)
                                fieldVar = fieldVar == "" ? lstfieldVar[i].Answer : fieldVar + "," + lstfieldVar[i].Answer;
                            fieldVar = "'" + fieldVar + "'";
                            formulaExp = formulaExp.replace(arr[0], fieldVar);
                            fieldVar = '';

                        }
                        else {
                            for (var i = 0; i < arr.length; i++) {
                                //if (this.lstBenefSurveyQstnAnswers.length > 0) {
                                //   if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i])).length > 0)
                                //        fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer;
                                // }

                                if (fieldVar == null || fieldVar == '') {
                                    var SectionList = [];
                                    if (this.SurveyBenLinkDlg == true) {
                                        SectionList = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                                    }
                                    else {
                                        SectionList = this.lstSectionsQstns;
                                    }

                                    for (var Cnt = 0; Cnt <= SectionList.length - 1; Cnt++) {
                                        var Questions = SectionList[Cnt].Questions;

                                        if (typeof Questions.filter(x => x.FieldVar == arr[i])[0] != "undefined") {
                                            fieldVar = Questions.filter(x => x.FieldVar == arr[i])[0].Answer;
                                        }



                                        //if (typeof item.filter(x => x.FieldVar == arr[i])[0] != "undefined") {
                                        //    fieldVar = item.filter(x => x.FieldVar == arr[i])[0].Answer;
                                        //}
                                        if (typeof this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[i])[0] != "undefined") {
                                            fieldVar = this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[i])[0].Answer;
                                        }
                                        if (fieldVar == null || fieldVar == '') {
                                            //fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer
                                            //if (this.lstBenefSurveyQstnAnswers.length > 0) {
                                            //    if (this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i])).length > 0)
                                            //        fieldVar = this.lstBenefSurveyQstnAnswers.filter(x => (x.BeneficiaryInd == benefIndx) && (x.FieldVar == arr[i]))[0].Answer;
                                            //}
                                        }
                                    }
                                }

                                var FieldVarAns = fieldVar == "" ? 0 : fieldVar; //fieldVar ?"":0

                                switch (FieldVarAns) {
                                    case "Low":
                                        FieldVarAns = 1;
                                        break
                                    case "Medium":
                                        FieldVarAns = 3;
                                        break;
                                    case "High":
                                        FieldVarAns = 5;
                                        break;
                                    case "L":
                                        FieldVarAns = 1;
                                        break;
                                    case "M":
                                        FieldVarAns = 3;
                                        break;
                                    case "H":
                                        FieldVarAns = 5;
                                        break;
                                    case "N/A":
                                        FieldVarAns = 0;
                                        break;
                                }

                                formulaExp = formulaExp.replace(arr[i], FieldVarAns);
                                fieldVar = '';
                            }
                        }

                        var strFn = eval(formulaExp);
                        resultVal = eval(strFn);
                        item.Answer = resultVal;
                    }
                }
            }
        },

        // Calculation function for repeat sections
        async RepeatVal_Aggregate(item, QID, benefIndex, FormulaOperation) {
            var formulaExp = "";
            var resultVal = 0;

            if (this.RepeatOrSkipSection.length > 0) {

                if (item.IsFormulaQuestion) {
                    var FormulaQuestion = item
                    formulaExp = FormulaQuestion.FormulaExpression;
                    if (FormulaQuestion.FormulaFieldVariables != "") {
                        var arr = FormulaQuestion.FormulaFieldVariables.split(",");
                        for (var CondQst = 0; CondQst <= arr.length - 1; CondQst++) {
                            if (typeof this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[CondQst])[0] != "undefined" || typeof this.lstSectionsQstns.filter(x => x.FieldVar == arr[CondQst])[0] != "undefined") {
                                var RepeatSection = this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[CondQst] && x.benefIndex == benefIndex);
                                if (RepeatSection.length == 0)//this.lstSectionQstns
                                {
                                    RepeatSection = this.lstSectionsQstns.filter(x => x.FieldVar == arr[CondQst]);
                                }
                                if (RepeatSection.length > 0) {
                                    if (FormulaOperation == "RepeatVal_Aggregate" || FormulaOperation == "RepeatVal_Aggregate_Average") // Calculating Repeat Section Sum / Total
                                    {
                                        for (var i = 0; i <= RepeatSection.length - 1; i++) {
                                            var FieldVarAns = RepeatSection[i].Answer == "" ? 0 : RepeatSection[i].Answer; //fieldVar ?"":0
                                            resultVal = resultVal + Number(FieldVarAns);
                                        }
                                    }
                                    if (FormulaOperation == "RepeatVal_Aggregate_Subtraction")// Calculating Repeat Section Substraction
                                    {
                                        for (var i = 0; i <= RepeatSection.length - 1; i++) {
                                            if (CondQst == 0) {
                                                resultVal = RepeatSection[i].Answer == "" ? 0 : Number(RepeatSection[i].Answer)
                                            }
                                            else {
                                                var FieldVarAns = RepeatSection[i].Answer == "" ? 0 : RepeatSection[i].Answer; //fieldVar ?"":0
                                                resultVal = resultVal - Number(FieldVarAns);
                                            }

                                        }
                                    }
                                    if (FormulaOperation == "RepeatVal_Aggregate_Multiplication")// Calculating Repeat Section Substraction
                                    {
                                        for (var i = 0; i <= RepeatSection.length - 1; i++) {
                                            if (CondQst == 0 && i == 0) {
                                                resultVal = RepeatSection[i].Answer == "" ? 0 : Number(RepeatSection[i].Answer)
                                            }
                                            else {
                                                var FieldVarAns = RepeatSection[i].Answer == "" ? 0 : RepeatSection[i].Answer; //fieldVar ?"":0
                                                resultVal = resultVal * Number(FieldVarAns);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (FormulaOperation == "RepeatVal_Aggregate_Average") {
                    resultVal = resultVal / RepeatSection.length
                }
                item.Answer = resultVal;
            }
        },

        // Get Repeat or Skip logic details
        async fnGetRepeatOrSkipSectionBasedOnQuestion(benefIndex, RepeatCount, QuestionID, RepeatSectionID, ParentQID) {

            //this.lstSectionQstns = [];
            //this.RepeatSection =[];
            // checking the same section question already exist or not, if exist remove
            //for (var i = 0; i <= this.RepeatSection.length-1; i++) {
            //if(benefIndex == 0)

            for (var i = this.RepeatOrSkipSection.length - 1; i >= 0; i--) {
                var obj = this.RepeatOrSkipSection[i];
                if (obj.SectionID == RepeatSectionID && obj.benefIndex == benefIndex) {
                    this.RepeatOrSkipSection.splice(i, 1);
                }
            }

            var HirachyParentQID = ParentQID;
            if (HirachyParentQID == 0) {
                HirachyParentQID = QuestionID;
            }
            var currSecQstns = [];
            if (this.SurveyBenLinkDlg == true) {
                currSecQstns = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == benefIndex)[0].Sections.filter(x => (x.SectionID == RepeatSectionID));
            }
            else {
                currSecQstns = this.lstSectionsQstns.filter(x => x.SectionID == RepeatSectionID);
            }
            //var SectionDetails = this.SectionDetails.filter(x => x.SectionID == RepeatSectionID);
            var cnt = 0;


            var BeneficiaryID = 0;
            if (this.IsBeneficiaryLinkedSurvey) {
                BeneficiaryID = currSecQstns[0].BeneficiaryID;
            }

            currSecQstns.sort(function (a, b) {
                return parseInt(a.QuestionOrder) - parseInt(b.QuestionOrder)
            });
            if (RepeatCount > 0) {
                cnt = 0;
                for (var j = 1; j <= RepeatCount; j++) {

                    // this push is to print the section name like (section 2:1 , section 2:2)
                    this.RepeatOrSkipSection.push({
                        RQstInd: cnt, SurveyID: this.SurveyID,
                        SectionID: currSecQstns[0].SectionID, SectionName: currSecQstns[0].SectionName, SectionDescription: currSecQstns[0].SectionDescription, SectionOrder: currSecQstns[0].SectionOrder,
                        QuestionOrder: 1, QuestionTypeID: 0, QuestionID: 0, QuestionName: currSecQstns[0].SectionName + " : " + j, SectionLogic: [], QuestionType: "",
                        IsFormulaQuestion: 0, IsConditionQuestion: 0, QuestionCondition: null, QuestionOptions: null, QuestionHint: "", IsValidationQuestion: 0,
                        RegEx: "", Answer: null, FieldVar: "", IsUnique: 0, IsUpload: 0, ErrorText: "", DefaultValue: "", IsMandatory: 0, IsSearchable: 0, QuestionCategory: 1,
                        FormulaOperation: "", FormulaExpression: "", FormulaFieldVariables: null, CascadingQuestionOptions: null, UOM: "", IsValidationError: false, ValidationErrorTest: "", DateMenu: false, ParentQuestionID: HirachyParentQID, IndexPos: i + 1, startCamera: false, camOn: false, img: '', uploadedImg: '', uploadedFile: '', uploadedSign: '', benefIndex: benefIndex, BeneficiaryID: BeneficiaryID, isOtherOptionEnabled: false,
                        OtherOptionAnswer: "", IsOtherOptionSelected: currSecQstns[0].IsOtherOptionSelected
                    });
                    cnt++;
                    for (var i = 0; i < currSecQstns.length; i++) {
                        var lstQuestions = currSecQstns[i].Questions;
                        for (var qCnt = 0; qCnt <= lstQuestions.length - 1; qCnt++) {

                            if (lstQuestions[qCnt].QuestionType == "Multi-Choice") {
                                var RepeatOptionNew = [];
                                for (var RCnt = 0; RCnt <= lstQuestions[qCnt].QuestionOptions.length - 1; RCnt++) {
                                    var ObjOptions = new Object();
                                    ObjOptions.OptionID = lstQuestions[qCnt].QuestionOptions[RCnt].OptionID;
                                    ObjOptions.OptionValue = lstQuestions[qCnt].QuestionOptions[RCnt].OptionValue;
                                    ObjOptions.QuestionID = lstQuestions[qCnt].QuestionOptions[RCnt].QuestionID;
                                    ObjOptions.RQstInd = cnt;
                                    ObjOptions.IsOptSelected = lstQuestions[qCnt].QuestionOptions[RCnt].IsOptSelected;
                                    RepeatOptionNew.push(ObjOptions);
                                }
                                lstQuestions[qCnt].QuestionOptions = RepeatOptionNew;
                            }
                            if (lstQuestions[qCnt].QuestionType == "Cascading Dropdown") {
                                var RepeatOptionCascading = [];
                                for (var RCnt = 0; RCnt <= lstQuestions[qCnt].CascadingQuestionOptions.length - 1; RCnt++) {

                                    RepeatOptionCascading = [];
                                    for (var CasCnt = 0; CasCnt <= lstQuestions[qCnt].CascadingQuestionOptions[RCnt].CascadingList.length - 1; CasCnt++) {
                                        var CasCadLis = lstQuestions[qCnt].CascadingQuestionOptions[RCnt].CascadingList[CasCnt]
                                        var ObjOptions = new Object();
                                        ObjOptions.ID = CasCadLis.ID;
                                        ObjOptions.Name = CasCadLis.Name;
                                        ObjOptions.ParentID = CasCadLis.ParentID;
                                        ObjOptions.RQstInd = cnt;
                                        ObjOptions.OptionID = 0;
                                        RepeatOptionCascading.push(ObjOptions);
                                    }
                                    lstQuestions[qCnt].CascadingQuestionOptions[RCnt].CascadingList = RepeatOptionCascading;
                                }
                            }

                            // this push is to print the section questions
                            this.RepeatOrSkipSection.push({
                                RQstInd: cnt, SurveyID: lstQuestions[qCnt].SurveyID,
                                SectionID: lstQuestions[qCnt].SectionID, SectionName: lstQuestions[qCnt].SectionName, SectionDescription: lstQuestions[qCnt].SectionDescription, SectionOrder: lstQuestions[qCnt].SectionOrder,
                                QuestionOrder: lstQuestions[qCnt].QuestionOrder, QuestionTypeID: lstQuestions[qCnt].QuestionTypeID, QuestionID: lstQuestions[qCnt].QuestionID,
                                QuestionName: lstQuestions[qCnt].QuestionName, SectionLogic: lstQuestions[qCnt].SectionLogic, QuestionType: lstQuestions[qCnt].QuestionType,
                                IsFormulaQuestion: lstQuestions[qCnt].IsFormulaQuestion, IsConditionQuestion: lstQuestions[qCnt].IsConditionQuestion, QuestionCondition: lstQuestions[qCnt].QuestionCondition,
                                QuestionOptions: lstQuestions[qCnt].QuestionOptions, QuestionHint: lstQuestions[qCnt].QuestionHint, IsValidationQuestion: lstQuestions[qCnt].IsValidationQuestion,
                                RegEx: lstQuestions[qCnt].RegEx, Answer: lstQuestions[qCnt].Answer, FieldVar: lstQuestions[qCnt].FieldVar, IsUnique: lstQuestions[qCnt].IsUnique, IsUpload: lstQuestions[qCnt].IsUpload,
                                ErrorText: lstQuestions[qCnt].ErrorText, DefaultValue: lstQuestions[qCnt].DefaultValue, IsMandatory: lstQuestions[qCnt].IsMandatory, IsSearchable: lstQuestions[qCnt].IsSearchable,
                                QuestionCategory: lstQuestions[qCnt].QuestionCategory, FormulaOperation: lstQuestions[qCnt].FormulaOperation, FormulaExpression: lstQuestions[qCnt].FormulaExpression, UOM: lstQuestions[qCnt].UOM,
                                FormulaFieldVariables: lstQuestions[qCnt].FormulaFieldVariables, CascadingQuestionOptions: lstQuestions[qCnt].CascadingQuestionOptions, IsValidationError: false, ValidationErrorTest: "", DateMenu: false, ParentQuestionID: HirachyParentQID, IndexPos: i + 1, startCamera: false, camOn: false, img: '', uploadedImg: '', uploadedFile: '', uploadedSign: '', benefIndex: benefIndex, BeneficiaryID: BeneficiaryID,
                                isOtherOptionEnabled: false, OtherOptionAnswer: "", IsOtherOptionSelected: lstQuestions[qCnt].IsOtherOptionSelected
                            });

                        }



                        cnt++;
                    }

                }
            }
            //var SectionCurrentIndex = this.lstSections.findIndex(x => (x.SectionID == this.vmSection.SectionID));
            //this.BenefQstnStepper[benefIndx] = SectionCurrentIndex + 1;
        },

        //------ ======= Beneficiary Registrations ====================------ //
        // Function Used to Fetch Beneficiary Registration Questions details
        async GetBenefRegistrationQstns() {

            if (this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns == null) {
                this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns = [];
            }
            // To Fetch all Existing Beneficiaries
            await this.GetBeneficiary();

            this.lstBenefRegQstns = [];

            for (var i = 0; i < this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns.length; i++) {

                var obj = new Object();
                obj.Answer = '',
                    obj.BeneficiaryTypeID = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].BeneficiaryTypeID,
                    obj.BTQstnAnsOptionList = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].BTQstnAnsOptionList,
                    obj.CascadingQuestionOptions = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].CascadingQuestionOptions,
                    obj.IsMandatoryResponse = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].IsMandatoryResponse,
                    obj.IsSearchable = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].IsSearchable,
                    obj.IsUnique = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].IsUnique,
                    obj.IsUpload = 0,
                    obj.QuestionHint = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].QuestionHint,
                    obj.QuestionID = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].QuestionID,
                    obj.QuestionName = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].QuestionName,
                    obj.QuestionType = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].QuestionType,
                    obj.QuestionTypeID = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].QuestionTypeID, obj.mBTDate = false,
                    obj.ValidationErrorTest = "",
                    obj.IsValidationError = false,
                    obj.IsValidationQuestion = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].IsValidationQuestion,
                    obj.RegexExpression = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].RegexExpression,
                    obj.BTQstnValidationList = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns[i].BTQstnValidationList,
                    obj.qType = "", obj.QstType = "", obj.textLength = 10, obj.minVal = 0, obj.maxVal = 9999999999, obj.qRules = [],


                    this.lstBenefRegQstns.push(obj);

                this.audUpload[i] = 0;
                var lstCascadingItems = this.lstSelectedTaskData.TaskData.CascadingList != null ? this.lstSelectedTaskData.TaskData.CascadingList : [];
                if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown") {
                    if (this.lstBenefRegQstns[i] != null) {
                        for (var Cnt1 = 0; Cnt1 < this.lstBenefRegQstns[i].CascadingQuestionOptions.length; Cnt1++) {
                            if (this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].OptionValue == "Country") {
                                this.lstCountry = lstCascadingItems[0].Country.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].CascadingList = this.lstCountry;
                            }
                            else if (this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].OptionValue == "States") {
                                this.lstStates = lstCascadingItems[0].States.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].CascadingList = this.lstStates;
                            }
                            else if (this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].OptionValue == "District") {
                                this.lstDistrict = lstCascadingItems[0].District.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].CascadingList = this.lstDistrict;
                            }
                            else if (this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].OptionValue == "SubDistrict") {
                                this.lstSubDistrict = lstCascadingItems[0].SubDistrict.sort(function (a, b) { var x = a.Name < b.Name ? -1 : 1; return x; });
                                this.lstBenefRegQstns[i].CascadingQuestionOptions[Cnt1].CascadingList = this.lstSubDistrict;
                            }
                        }
                    }
                }

            }
            // Fetching excel file headers from Beneficiary
            await this.fnGetBeneficiaryExcelHeaderValues();
            this.BenefImportTab = 'tab-4';
            this.BenefLinkedSurveySteps = 3;
            this.scrollToNavbarTop();
            //await this.scrollToTop(this.lstBenefRegQstns);
        },

        async fnCancelBenefRegistration(type) {

            if (type == "tab-3") // Cancel Import Registration
            {

                this.lstImportBeneficiary = [];
                //this.lstImportBeneficiaryHearder = [];
                this.lstNewBeneficiary = [];
                this.vmSelectedBeneficiaries = [];
                await dbImworks.TbNewBeneficiary.clear();

                for (var i = 0; i <= this.lstSelectedBeneficiary.length - 1; i++) {

                    const index = this.lstBeneficiary.findIndex((element, index) => {
                        if ((element.BeneficiaryID === item.BeneficiaryID) && (element.TokenValue == item.TokenValue)) {
                            return true
                        }
                    });
                    this.lstBeneficiary.splice(index, 1);
                }
                this.lstSelectedBeneficiary = [];
                this.File = [];
                this.BenefLinkedSurveySteps = 2;
            }
            else {

                this.lstBenefRegQstns = [];
                this.BenefLinkedSurveySteps = 2;
            }
            this.scrollToNavbarTop();
        },
        async RegisterNewBenficiary(type) {

            if (type == "tab-4") // Register through manual
            {

                await this.RegisterNewBenficiaryManual()

            }
            else {

                await this.RegisterNewBeneficiaryByImport();
            }
            this.scrollToNavbarTop();


        },
        // Beneficiary Manual Registration
        async RegisterNewBenficiaryManual() {

            if (await this.fnCheckBeneficiaryMandatoryAndValidation() == false) {


                if (this.lstBenefRegQstns[0].Answer == "") {

                    this.SnackbarMsg = "Please enter Beneficiary Name";
                    this.InfoSnackbarMsg = true;
                    return false;


                }
                this.newBenefID--;
                var lstBenefRegQstnAnswers = [];
                var Ans = "";
                var blobFile = [];
                var tempData = await dbImworks.TbNewBeneficiary.toArray();
                var BenefIndexID = -(tempData.length + 1);
                for (var i = 0; i < this.lstBenefRegQstns.length; i++) {
                    Ans = "";
                    if (this.lstBenefRegQstns[i].QuestionType == 'Multi-Choice') {

                        for (var j = 0; j < this.multiChoiceAnsBT.length; j++) {
                            Ans = (Ans != '') ? Ans + ',' + this.multiChoiceAnsBT[j] : this.multiChoiceAnsBT[j];
                        }
                    }
                    else if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown") {

                        var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == this.lstBenefRegQstns[i].QuestionID) && (x.IsBenefQstn == true));
                        for (var j = 0; j < CascadingAns.length; j++) {
                            Ans = (Ans != '') ? Ans + ',' + CascadingAns[j].Ans : CascadingAns[j].Ans;
                        }
                    }
                    else if ((this.lstBenefRegQstns[i].QuestionType == "Audio Capture") || (this.lstBenefRegQstns[i].QuestionType == "Signature Capture")
                        || (this.lstBenefRegQstns[i].QuestionType == "Photo Capture") || (this.lstBenefRegQstns[i].QuestionType == "File Upload")) {
                        if (this.BTAnsBlob.length != 0) {

                            var bFile = this.BTAnsBlob.filter(x => x.QuestionID == this.lstBenefRegQstns[i].QuestionID)
                            if (bFile.length > 0) {
                                blobFile = this.BTAnsBlob.filter(x => x.QuestionID == this.lstBenefRegQstns[i].QuestionID)[0].blobData;
                                if (typeof blobFile != "undefined") {
                                    await this.FileUpload(blobFile, this.lstBenefRegQstns[i].Answer, true, bFile[0].IsUploadedFile, BenefIndexID, this.lstBenefRegQstns[i].QuestionID);
                                }
                            }

                        }

                        Ans = this.lstBenefRegQstns[i].Answer;
                    }
                    else if (this.lstBenefRegQstns[i].QuestionType == "GPS Capture" && (this.lstBenefRegQstns[i].Answer == undefined || this.lstBenefRegQstns[i].Answer == '')) {
                        var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                        Ans = gpsCoordinates;
                    }
                    else {
                        Ans = this.lstBenefRegQstns[i].Answer;
                    }

                    lstBenefRegQstnAnswers.push({ QuestionID: this.lstBenefRegQstns[i].QuestionID, AnswerTypeID: this.lstBenefRegQstns[i].QuestionTypeID, Answer: Ans, CreatedDate: new Date() });
                }


                // var tempData = await dbImworks.TbNewBeneficiary.toArray();
                //var BenefIndexID = -(tempData.length + 1);

                var now = new Date();
                var TokenValue = "";
                TokenValue = "Beneficiary_" + now.getFullYear().toString() + now.getMonth().toString() + now.getDate().toString() + now.getHours().toString() + now.getMinutes().toString() + now.getSeconds().toString() + now.getMilliseconds().toString();

                this.lstNewBeneficiary.push({
                    BeneficiaryID: BenefIndexID,
                    BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer,
                    BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                    CreatedDate: new Date(),
                    TokenValue: TokenValue
                });


                this.vmSelectedBeneficiaries.push({
                    BeneficiaryID: BenefIndexID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                    CreatedDate: new Date(), TokenValue: TokenValue
                });


                // Insert into New Dexie table, to get the newly registered beneficiary. when we are trying to update another task;
                await dbImworks.TbNewBeneficiary.put({
                    BeneficiaryID: BenefIndexID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                    CreatedDate: new Date(), TokenValue: TokenValue, BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers), WorkspaceID: this.WorkspaceID
                });

                for (var i = 0; i < this.lstBenefRegQstns.length; i++) {
                    this.lstBenefRegQstns[i].Answer = "";
                    if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown") {
                        for (var j = 0; j < this.lstBenefRegQstns[i].CascadingQuestionOptions.length; j++) {
                            this.lstBenefRegQstns[i].CascadingQuestionOptions[j][0] = null;
                            this.lstBenefRegQstns[i].CascadingQuestionOptions[j][j] = null;
                        }
                    }
                    if (this.lstBenefRegQstns[i].QuestionType == "Photo Capture" || this.lstBenefRegQstns[i].QuestionType == "Signature Capture" ||
                        this.lstBenefRegQstns[i].QuestionType == "File Upload") {

                        this.lstBenefRegQstns[i].img = null;
                        this.benefUploadedImg = null;
                        this.lstBenefRegQstns[i].startCamera = false;
                        this.lstBenefRegQstns[i].camOn = false;
                        this.devices = [];
                        this.deviceId = null;
                        this.camera = null;
                        this.img1 = null;
                    }
                }

                // Next Page
                var obj = new Object();
                var lstBen = [];
                obj = new Object();
                for (var Cnt = 0; Cnt <= this.lstBenefRegQstns.length - 1; Cnt++) {
                    var temp = lstBenefRegQstnAnswers.filter(x => x.QuestionID == this.lstBenefRegQstns[Cnt].QuestionID);
                    if (temp.length > 0) {
                        obj[this.lstBenefRegQstns[Cnt].QuestionName] = temp[0].Answer;
                        obj["CreatedDate"] = objUtils.common.formatNewDate(temp[0].CreatedDate);
                    }
                }
                obj["BeneficiaryID"] = BenefIndexID;
                obj["TokenValue"] = TokenValue;

                lstBen.push(obj);
                // Adding new beneficiary to existing beneficiary list
                this.lstBeneficiary.unshift(obj);
                this.lstSelectedBeneficiary = [];
                this.lstSelectedBeneficiary = lstBen;
                this.BenefLinkedSurveySteps = 2;
                this.BTAnsBlob = [];
                this.benefUploadedSign = "";

            }
            else {
                this.dlgValidationMessage = true;
            }
        },

        // Beneficiary Import Registration
        async RegisterNewBeneficiaryByImport() {

            this.PreLoader = true;
            if (await this.searchInvalidInputArray() == true) {


                this.lstSelectedBeneficiary = [];
                for (var i = 0; i < this.lstImportBeneficiary.length; i++) {
                    var lstBenefRegQstnAnswers = [];

                    for (var j = 0; j < this.lstImportBeneficiaryHearder.length; j++) {
                        lstBenefRegQstnAnswers.push({
                            QuestionID: this.lstBenefRegQstns[j].QuestionID, AnswerTypeID: this.lstBenefRegQstns[j].QuestionTypeID,
                            Answer: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[j].value], CreatedDate: new Date()
                        });
                    }

                    var now = new Date();
                    var TokenValue = "";
                    TokenValue = "Beneficiary_" + now.getFullYear().toString() + now.getMonth().toString() + now.getDate().toString() + now.getHours().toString() + now.getMinutes().toString() + now.getSeconds().toString() + now.getMilliseconds().toString();

                    var tempData = await dbImworks.TbNewBeneficiary.toArray();
                    var BenefIndexID = -(tempData.length + 1);

                    //this.lstNewBeneficiary.push({
                    //    BeneficiaryID: BenefIndexID,
                    //    BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID, BeneficiaryName: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[0].value],
                    //    BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                    //    CreatedDate: new Date(), TokenValue: TokenValue
                    //});

                    this.vmSelectedBeneficiaries.push({
                        BeneficiaryID: BenefIndexID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                        CreatedDate: new Date(), TokenValue: TokenValue
                    });

                    // Insert into New Dexie table, to get the newly registered beneficiary. when we are trying to update another task;
                    await dbImworks.TbNewBeneficiary.put({
                        BeneficiaryID: BenefIndexID, BeneficiaryName: lstBenefRegQstnAnswers[0].Answer, BeneficiaryTypeID: this.lstBenefRegQstns[0].BeneficiaryTypeID,
                        CreatedDate: new Date(), TokenValue: TokenValue, BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers), WorkspaceID: this.WorkspaceID
                    });


                    //-----------------------
                    var obj = new Object();
                    var lstBen = [];
                    obj = new Object();
                    for (var Cnt = 0; Cnt <= this.lstBenefRegQstns.length - 1; Cnt++) {
                        var temp = lstBenefRegQstnAnswers.filter(x => x.QuestionID == this.lstBenefRegQstns[Cnt].QuestionID);
                        if (temp.length > 0) {
                            obj[this.lstBenefRegQstns[Cnt].QuestionName] = temp[0].Answer;
                            obj["CreatedDate"] = objUtils.common.formatNewDate(temp[0].CreatedDate);
                        }
                    }
                    obj["BeneficiaryID"] = BenefIndexID;
                    obj["TokenValue"] = TokenValue;

                    lstBen.push(obj);
                    // Adding new beneficiary to existing beneficiary list
                    this.lstBeneficiary.unshift(obj);

                    this.lstSelectedBeneficiary.push(lstBen[0]);


                }

                this.File = [];
                this.lstImportBeneficiary = [];
                for (var i = 0; i < this.lstBenefRegQstns.length; i++)
                    this.lstBenefRegQstns[i].Answer = "";
                this.BenefLinkedSurveySteps = 2;
                this.PreLoader = false;
            }
            else {
                this.PreLoader = false;
                this.SnackbarMsg = "Please resolve all Invalid Input(s).";
                this.InfoSnackbarMsg = true;
            }
        },

        //Mandatory feild or Invalid input in Excel
        async searchInvalidInputArray() {

            var newList = this.lstImportBeneficiary.filter(element => {
                for (var property in element) {
                    if (element.hasOwnProperty(property)) {
                        if (element[property] == "Invalid Input") {
                            return true;
                        }
                    }
                }

            });
            if (newList.length > 0) {
                return false;
            }
            else {
                return true;
            }
        },
        async fnBindValue(e, index) {
            if (this.lstBenefRegQstns[index].QuestionType == "Numeric") {
                this.lstBenefRegQstns[index].Answer = e;
            }
            else {

                this.lstBenefRegQstns[index].Answer = e.target.value;
            }
        },

        // to Get Beneficiary Question as Excel Header
        async fnGetBeneficiaryExcelHeaderValues() {


            var colValue = "";
            this.lstImportBeneficiaryHearder = [];
            this.lstImportBeneficiary = [];
            this.File = [];
            for (var i = 0; i < this.lstBenefRegQstns.length; i++) {

                colValue = "Answer" + (i + 1).toString();
                if (this.lstBenefRegQstns[i].QuestionType == "Date Picker") {
                    this.lstImportBeneficiaryHearder.push({
                        text: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        value: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        type: "date",
                        //mandatory:this.lstBenefRegQstns[i].IsMandatoryResponse,
                        mandatory: this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                        QuestionCondition: this.lstBenefRegQstns[i].BTQstnValidationList,
                    });
                }
                else if (this.lstBenefRegQstns[i].QuestionType == "Numeric") {
                    this.lstImportBeneficiaryHearder.push({
                        text: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        value: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        type: "number",
                        mandatory: this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                        QuestionCondition: this.lstBenefRegQstns[i].BTQstnValidationList,
                    });
                }
                else if (this.lstBenefRegQstns[i].QuestionType == "Dropdown" || this.lstBenefRegQstns[i].QuestionType == "Multi-Choice" || this.lstBenefRegQstns[i].QuestionType == "Single Choice" || this.lstBenefRegQstns[i].QuestionType == "Likert Scale") {
                    var optionlist = [];
                    for (var j = 0; j < this.lstBenefRegQstns[i].BTQstnAnsOptionList.length; j++) {
                        optionlist.push(this.lstBenefRegQstns[i].BTQstnAnsOptionList[j].OptionValue)
                    }
                    this.lstImportBeneficiaryHearder.push({
                        text: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        value: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        type: "select",
                        mandatory: this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                        QuestionCondition: this.lstBenefRegQstns[i].BTQstnValidationList,
                        options: optionlist,
                    });
                }
                else {
                    this.lstImportBeneficiaryHearder.push({
                        text: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        value: this.lstBenefRegQstns[i].QuestionName.concat(' ', this.lstBenefRegQstns[i].QuestionHint),
                        type: "string",
                        mandatory: this.lstBenefRegQstns[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                        QuestionCondition: this.lstBenefRegQstns[i].BTQstnValidationList,
                    });
                }
            }
        },

        async GetGPSCoordinates(item, IsBenefQstn) {
            var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
            if (IsBenefQstn) {
                this.lstBenefRegQstns.filter(x => x.QuestionID == item.QuestionID)[0].Answer = gpsCoordinates;
            }
            else {
                item.Answer = gpsCoordinates;
            }

        },
        async updateCoordinates(evnt, Qitem) {

            this.Coordinates = {
                lat: evnt.lat(),
                lng: evnt.lng(),
            };
            Qitem.Answer = this.Coordinates.lat + "," + this.Coordinates.lng;


        },

        // Function Triggers when drag and drop happens for Survey Import
        async dragFile_Survey(e) {

            this.SurveyFile = e.dataTransfer.files;
            await this.OnSurveyFileUpload()
        },

        // Survey Import File Upload
        async OnSurveyFileUpload() {

            if (this.SurveyFile.length > 0) {
                var vm = this
                if (window.FileReader) {
                    var reader = new FileReader();
                    reader.readAsText(this.SurveyFile[0]);
                    // Handle errors load
                    reader.onload = function (event) {
                        var csv = event.target.result;
                        vm.ReadSurveyCsvFile(csv)
                    };
                    reader.onerror = function (evt) {
                        if (evt.target.error.name == "NotReadableError") {
                            this.SnackbarMsg = "Cannot read file !";
                            this.InfoSnackbarMsg = true;
                        }

                    };
                }
                else {
                    this.Msg = 'FileReader is not supported in this browser.';
                    this.InfoSnackbarMsg = true;
                }
            }
            else {
                this.lstImportSurvey = [];
            }
        },

        // Function to Read data from excel sheet for survey upload
        async ReadSurveyCsvFile(csv) {

            var lines = csv.split(/\r\n|\n/);
            //var headers = lines[0].split(",");
            var headers = objUtils.common.MergeCommas(lines[0]);
            var flag = true;
            var today = new Date().toLocaleString();
            //this.resetVueExcelEditor();
            this.lstImportSurveyQstnHearderN = [];
            //var optionlist=[];
            //for (var j = 0; j < this.vmSelectedBeneficiaries.length; j++) {
            //optionlist.push(this.vmSelectedBeneficiaries[j].BeneficiaryName)
            //}
            if (this.vmSelectedBeneficiaries.length > 0) {
                this.lstImportSurveyQstnHearderN.push({
                    text: "Beneficiary",
                    value: "Beneficiary",
                    type: "string",
                    mandatory: 'Yes',
                    //options:optionlist,
                    QuestionCondition: [],
                });
            }
            for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++)
                this.lstImportSurveyQstnHearderN.push(this.lstImportSurveyQstnHearder[i]);

            if (this.lstImportSurveyQstnHearderN.length == headers.length) {
                for (var c = 0; c < this.lstImportSurveyQstnHearderN.length; c++) {
                    if (this.lstImportSurveyQstnHearderN[c].text.trim != headers[c].trim) {
                        flag = false;
                        break;
                    }
                }
                if (!flag) {
                    this.MSg = ("Survey Questionnaire is not matching for the selected file...  ");
                    this.infoSnackbarMsg = true;
                    this.surveyfileInput = null;
                    this.csvSurveyFileName = "";
                }
                else {

                    for (var i = 1; i < lines.length; i++) {
                        var obj = {};
                        //var currentline = lines[i].split(",");
                        var currentline;
                        currentline = objUtils.common.MergeCommas(lines[i]);

                        if (currentline != "") {
                            for (var j = 0; j < headers.length; j++) {
                                // obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                var dataType = this.lstImportSurveyQstnHearderN[j].type
                                var QuestionCondition = [];
                                QuestionCondition = this.lstImportSurveyQstnHearderN[j].QuestionCondition != null ? this.lstImportSurveyQstnHearderN[j].QuestionCondition : [];

                                if (dataType == "number") {
                                    if (isNaN(currentline[j]) || (currentline[j] == "" && this.lstImportSurveyQstnHearderN[j].mandatory == 'Yes')) {

                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                    }

                                    // // validating Question Condition
                                    this.IsvalidexcelData = true;
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "") {
                                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                if (!currentline[j].match(phoneNum)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Min & Max Range" && currentline[j] != "") {
                                                if (!(parseInt(currentline[j]) > parseInt(QuestionCondition[Count].MinValue) && parseInt(currentline[j]) < parseInt(QuestionCondition[Count].MaxValue))) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }

                                            if (QuestionCondition[Count].ValidationCondition == "Aadhaar Number" && currentline[j] != "") {
                                                var Aadhaarnnm = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                                                if (currentline[j] != null && currentline[j] != undefined && !currentline[j].match(Aadhaarnnm)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (dataType == "date") {
                                    var dateField = currentline[j];
                                    var timestamp = function (dateField) {
                                        if (Object.prototype.toString.call(dateField) === "[object Date]")
                                            return true;
                                        return false;
                                    };

                                    if (timestamp == false || (currentline[j] == "" && this.lstImportSurveyQstnHearderN[j].mandatory == 'Yes')) {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                    }
                                    // validating Question Condition
                                    this.IsvalidexcelData = true;
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            // Date Validation
                                            if (QuestionCondition[Count].ValidationCondition == "Should be greater than Current date" && currentline[j] != "") {
                                                if (objUtils.common.formatExcelDate(dateField) < objUtils.common.formatExcelDate(today)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "") {
                                                if (objUtils.common.formatExcelDate(dateField) > objUtils.common.formatExcelDate(today)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (dataType == 'select') {

                                    if ((currentline[j] != "" && this.lstImportSurveyQstnHearderN[j].mandatory == 'Yes' && !this.lstImportSurveyQstnHearderN[j].options.includes(currentline[j]))) {

                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                    }
                                    else if ((currentline[j] != "" && this.lstImportSurveyQstnHearderN[j].mandatory != 'Yes' && !this.lstImportSurveyQstnHearderN[j].options.includes(currentline[j]))) {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                    }
                                }
                                else {
                                    if (currentline[j] == "" && this.lstImportSurveyQstnHearderN[j].mandatory == 'Yes') {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportSurveyQstnHearderN[j].value] = currentline[j];
                                    }
                                    // validating Question Condition
                                    this.IsvalidexcelData = true;
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "") {
                                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                if (!currentline[j].match(phoneNum)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Email" && currentline[j] != "") {
                                                var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                                if (!currentline[j].match(email)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }

                                            if (QuestionCondition[Count].ValidationCondition == "Aadhaar Number" && currentline[j] != "") {
                                                var AadhaarNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                                                if (currentline[j] != null && currentline[j] != undefined && !currentline[j].match(AadhaarNum)) {
                                                    obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            this.lstImportSurvey.push(obj);

                        }

                    }
                }
            }
            else {
                this.Msg = ("Survey Questionnaire format is not matching for the selected file...  ");
                this.infoSnackbarMsg = true;
                this.surveyfileInput = null;
                this.csvSurveyFileName = "";
            }
        },

        // Cancel Survey Import
        async CancelImport() {

            this.SurveyFile = [];
            this.lstImportSurvey = [];
            this.resetOnClick = 0;
            this.lstImportSurveyQstnHearderN = [];

        },
        // Function Triggers when drag and drop happens for Beneficiary Import
        async dragFile(e) {

            this.File = e.dataTransfer.files;
            await this.OnRegFileUpload()
        },
        // Register Beneficiary File upload
        async OnRegFileUpload() {

            //this.csvFileName = this.File[0].name;
            var vm = this
            if (window.FileReader) {
                var reader = new FileReader();
                reader.readAsText(this.File[0]);
                // Handle errors load
                reader.onload = function (event) {
                    var csv = event.target.result;
                    vm.ReadCsvFile(csv)
                };
                reader.onerror = function (evt) {
                    if (evt.target.error.name == "NotReadableError") {
                        this.SnackbarMsg = "Cannot read file !";
                        this.InfoSnackbarMsg = true;
                    }

                };
            }
            else {
                this.SnackbarMsg = 'FileReader is not supported in this browser.';
                this.InfoSnackbarMsg = true;
            }

        },

        // Function Used to Read Data from the excel
        async ReadCsvFile(csv) {

            //this.lstImportBeneficiary =[];
            var flag = true;
            var today = new Date().toLocaleString();
            var lines = csv.split(/\r\n|\n/);

            //var headers = lines[0].split(",");
            var headers = objUtils.common.MergeCommas(lines[0]);
            if (this.lstImportBeneficiaryHearder.length == headers.length) {
                for (var c = 0; c < this.lstImportBeneficiaryHearder.length; c++) {
                    if (this.lstImportBeneficiaryHearder[c].text != headers[c]) {
                        flag = false;
                        break;
                    }
                }
                if (!flag) {
                    this.SnackbarMsg = ("Beneficiary Registration Questionnaire is not matching for the selected file...  ");
                    this.InfoSnackbarMsg = true;
                    //this.regfileInput = null;
                    //this.csvFileName = "";
                }
                else {
                    for (var i = 1; i < lines.length; i++) {
                        var obj = {};
                        //var currentline = lines[i].split(",");
                        var currentline;
                        currentline = objUtils.common.MergeCommas(lines[i]);

                        if (currentline != "") {
                            //for (var j = 0; j < headers.length; j++)
                            //obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                            for (var j = 0; j < headers.length; j++) {
                                var dataType = this.lstImportBeneficiaryHearder[j].type
                                var QuestionCondition = [];
                                QuestionCondition = this.lstImportBeneficiaryHearder[j].QuestionCondition != null ? this.lstImportBeneficiaryHearder[j].QuestionCondition : [];
                                if (dataType == "number") {
                                    if (isNaN(currentline[j]) || (currentline[j] == "" && this.lstImportBeneficiaryHearder[j].mandatory == 'Yes')) {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                    }
                                    // validating Question Condition
                                    this.IsvalidexcelData = true;
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "") {
                                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                if (!currentline[j].match(phoneNum)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Min & Max Range" && currentline[j] != "") {
                                                if (!(parseInt(currentline[j]) > parseInt(QuestionCondition[Count].MinValue) && parseInt(currentline[j]) < parseInt(QuestionCondition[Count].MaxValue))) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }

                                            if (QuestionCondition[Count].ValidationCondition == "Aadhaar Number" && currentline[j] != "") {
                                                var addharNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                                                if (currentline[j] != null && currentline[j] != undefined && !currentline[j].match(addharNum)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                        }
                                    }// if ends here

                                }
                                else if (dataType == "date") {
                                    var dateField = currentline[j];
                                    var timestamp = function (dateField) {
                                        if (Object.prototype.toString.call(dateField) === "[object Date]")
                                            return true;
                                        return false;
                                    };

                                    if (timestamp == false || (currentline[j] == "" && this.lstImportBeneficiaryHearder[j].mandatory == 'Yes')) {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                    }
                                    // validating Question Condition
                                    this.IsvalidexcelData = true;
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            // Date Validation
                                            if (QuestionCondition[Count].ValidationCondition == "Should be greater than Current date" && currentline[j] != "") {
                                                if (objUtils.common.formatExcelDate(dateField) < objUtils.common.formatExcelDate(today)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "") {
                                                if (objUtils.common.formatExcelDate(dateField) > objUtils.common.formatExcelDate(today)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                        }
                                    } // if ends here

                                }
                                else if (this.lstBenefRegQstns[j].QuestionType == "Dropdown" || this.lstBenefRegQstns[j].QuestionType == "Multi-Choice" || this.lstBenefRegQstns[j].QuestionType == "Single Choice" || this.lstBenefRegQstns[j].QuestionType == "Likert Scale") {

                                    if ((currentline[j] != "" && this.lstImportBeneficiaryHearder[j].mandatory == 'Yes') || (!this.lstImportBeneficiaryHearder[j].options.includes(currentline[j]) && currentline[j] != "")) {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                    }
                                }
                                else {
                                    if (currentline[j] == "" && this.lstImportBeneficiaryHearder[j].mandatory == 'Yes') {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                                    }
                                    else {
                                        obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                                    }
                                    this.IsvalidexcelData = true;
                                    // Condition Starts here
                                    if (QuestionCondition.length > 0) {
                                        for (var Count = 0; Count <= QuestionCondition.length - 1; Count++) {
                                            if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "") {
                                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                                if (!currentline[j].match(phoneNum)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                            if (QuestionCondition[Count].ValidationCondition == "Email" && currentline[j] != "") {
                                                var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                                if (!currentline[j].match(email)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }

                                            if (QuestionCondition[Count].ValidationCondition == "Aadhaar Number" && currentline[j] != "") {
                                                var aahdarNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                                                if (currentline[j] != null && currentline[j] != undefined && !currentline[j].match(aahdarNum)) {
                                                    obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                    this.IsvalidexcelData = false;
                                                }
                                            }
                                        }
                                    }
                                    // ends here

                                }

                            }
                            this.lstImportBeneficiary.push(obj);

                        }
                    }
                }
            }
            else {
                this.SnackbarMsg = ("Beneficiary Type format is not matching for the selected file...  ");
                this.InfoSnackbarMsg = true;
                //this.regfileInput = null;
                //this.csvFileName = "";
            }
        },

        // Function Used to download Sample xls file for Beneficiary Import Registrations
        async DownloadBenefRegSampleFile() {

            var ColumnHeader = "";
            var ColumnData1 = [];
            var ColumnData = [];
            var strResposnse = "Registration Response here"
            for (var i = 0; i < this.lstImportBeneficiaryHearder.length; i++) {

                if (this.lstImportBeneficiaryHearder[i].text.includes(',')) {
                    ColumnHeader += ColumnHeader == "" ? "\"" + this.lstImportBeneficiaryHearder[i].text + "\"" : "," + "\"" + this.lstImportBeneficiaryHearder[i].text + "\"";
                }
                else {
                    ColumnHeader += ColumnHeader == "" ? this.lstImportBeneficiaryHearder[i].text : "," + this.lstImportBeneficiaryHearder[i].text;
                }


                if (this.lstImportBeneficiaryHearder[i].type == "date") {
                    ColumnData1.push(strResposnse + " *Please enter date in mm-dd-yyyy format.");
                }
                else {
                    ColumnData1.push(strResposnse);
                }

                //ColumnData1.push(this.lstImportBeneficiaryHearder[i].text + " value");
            }

            ColumnHeader = ColumnHeader + "\n";
            ColumnData.push(ColumnData1);


            ColumnData.forEach(function (row) {
                ColumnHeader += row.join(',');
                ColumnHeader += "\n";
            });

            var hiddenElement = document.createElement('a');
            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
            hiddenElement.target = '_blank';
            hiddenElement.download = 'sample_beneficiary_register_' + Math.round(Math.random() * 1000) + '.csv';
            hiddenElement.click();
        },

        //======= Beneficiary Regsitration ends here ==========//

        // Funtion used to get Questions and Answers from Saved list
        async fnGetDataFromSavedList(type) {
            var lstSavedData = [];
            var SelectedTaskDtls = [];
            var Questions = [];
            var SavedList = [];

            if (type == "Reject") {
                SavedList = this.lstSelectedTaskData.TaskData;
                //this.SavedTaskDetails = JSON.stringify(TaskData);
                SelectedTaskDtls = this.lstSelectedTaskData
                var Answerlist = [];
                //IsBeneficiaryLinkedSurvey
                if (this.IsBeneficiaryLinkedSurvey == true) {
                    Answerlist = this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList;

                    //var selectBeneficiary = this.lstBeneficiary.filter(x=> x.BeneficiaryID == )
                    let selectBeneficiary = this.lstBeneficiary.filter(o1 => this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList.some(o2 => o1.BeneficiaryID === o2.BeneficiaryID));

                    var vmSelectedBeneficiary = this.lstSelectedTaskData.TaskData.BeneficiaryList.filter(x => selectBeneficiary.some(y => x.BeneficiaryID === y.BeneficiaryID));
                    this.lstSelectedBeneficiary = selectBeneficiary;
                    this.vmSelectedBeneficiaries = vmSelectedBeneficiary;
                }
                else {

                    Answerlist = this.lstSelectedTaskData.TaskData.SurveyQstnList;
                    this.lstSelectedBeneficiary = [];
                    this.vmSelectedBeneficiaries = [];
                }

                this.cascadingAns = (this.lstSelectedTaskData.TaskData.CascadingList);
                this.multiChoiceAns = [];//(this.lstSelectedTaskData.TaskData.MultiChoiceAnsList);

            }
            else if (type == "Saved") {

                var lstSavedData = await dbImworks.SavedTaskDB.toArray();
                this.SavedTaskDetails = lstSavedData.filter(x => x.WorkspaceID == this.WorkspaceID);
                var SelectedTaskDtls = this.lstSelectedTaskData
                if (this.SavedTaskDetails.length > 0) {
                    var Questions = [];
                    var SavedList = []; //this.SavedTaskDetails.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.TaskDate == this.SelectedTaskDate);

                    if (this.lstSelectedTaskData.Periodicity != "Anytime") {
                        SavedList = this.SavedTaskDetails.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.TaskDate == this.SelectedTaskDate);
                    } else if (this.lstSelectedTaskData.Periodicity == "Anytime") {

                        SavedList = this.SavedTaskDetails.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.TaskSubmittedDate == this.AnyTimeSubmissionDateTime);
                    }
                    var Answerlist = JSON.parse(SavedList[0].SurveyAnswers);
                    this.lstSelectedBeneficiary = JSON.parse(SavedList[0].lstSelectedBeneficiary);
                    this.vmSelectedBeneficiaries = JSON.parse(SavedList[0].SelectedBeneficiaries);
                    this.lstNewBeneficiary = JSON.parse(SavedList[0].NewBeneficiaries);
                    this.cascadingAns = JSON.parse(SavedList[0].CascadingAnsList);
                    this.multiChoiceAns = JSON.parse(SavedList[0].MultiChoiceAnsList);
                    this.MultiChoiceAnsForRepeat = JSON.parse(SavedList[0].MultiChoiceAnsListRepeat);

                }

            }
            // Beneficiary Loop
            if (this.IsBeneficiaryLinkedSurvey == true) {

                await this.FillSurveyQuestionaries();
                for (var BenCnt = 0; BenCnt <= this.BeneficiarySectionQstnList.length - 1; BenCnt++) {
                    var Sections = this.BeneficiarySectionQstnList[BenCnt].Sections;
                    // Sections Loop
                    for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {

                        // Questions loop
                        for (var QCnt = 0; QCnt <= Sections[SecCnt].Questions.length - 1; QCnt++) {

                            var Questionsdlts = Sections[SecCnt].Questions[QCnt];
                            var Answer = "";
                            if (type == "Saved") {

                                if (typeof Answerlist.filter(x => x.ResponseID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                                    Answer = Answerlist.filter(x => x.ResponseID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                                }
                            } else {

                                if (typeof Answerlist.filter(x => x.BeneficiaryID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                                    Answer = Answerlist.filter(x => x.BeneficiaryID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                                }
                            }
                            if (Questionsdlts.QuestionType == 'Cascading Dropdown') {

                                // Clear data before assigning
                                if (Questionsdlts.CascadingQuestionOptions.length > 0) {

                                    var CascadingOption = Questionsdlts.CascadingQuestionOptions
                                    for (var k = 0; k < CascadingOption.length; k++) {
                                        Questionsdlts.CascadingQuestionOptions[k][0] = null;
                                        Questionsdlts.CascadingQuestionOptions[k][k] = null;
                                    }
                                }


                                if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {
                                    AnsOptions = Answer.split(',');
                                    for (var j = 0; j < AnsOptions.length; j++) {
                                        var pID = 0;
                                        if (Questionsdlts.CascadingQuestionOptions[j].CascadingList != null)
                                            pID = Questionsdlts.CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                        Questionsdlts.CascadingQuestionOptions[j][j] = pID;
                                        this.FilterCascadingItems(Questionsdlts, Questionsdlts.CascadingQuestionOptions[j], pID, false, this.BeneficiarySectionQstnList[BenCnt].BenefIndex, -1);
                                    }
                                    this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                                }

                            }
                            else if (Questionsdlts.QuestionType == 'Multi-Choice') {

                                if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {
                                    var AnsOptions = Answer.split(',');
                                    // Initialize IsOptSelected = false;
                                    for (var mCnt2 = 0; mCnt2 < Questionsdlts.QuestionOptions.length; mCnt2++) {
                                        Questionsdlts.QuestionOptions[mCnt2].IsOptSelected = false;
                                    }
                                    // this.multiChoiceAns = AnsOptions;
                                    for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                        var temp = Questionsdlts.QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt]);
                                        if (temp.length > 0) {
                                            Questionsdlts.QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                        }
                                    }
                                    if (type == "Reject") {

                                        for (var mCnt1 = 0; mCnt1 < Questionsdlts.QuestionOptions.length; mCnt1++) {
                                            if (Questionsdlts.QuestionOptions[mCnt1].IsOptSelected == true) {
                                                this.multiChoiceAns.push({ OptionValue: Questionsdlts.QuestionOptions[mCnt1].OptionValue, QuestionID: Questionsdlts.QuestionID, benefIndex: BenCnt + 1 });
                                            }
                                        }
                                    }
                                    this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                                }
                            }

                            else if ((Questionsdlts.QuestionType == "Photo Capture") || (Questionsdlts.QuestionType == "Signature Capture") ||
                                (Questionsdlts.QuestionType == "File Upload") || (Questionsdlts.QuestionType == "Audio Capture")) {
                                if (type == "Saved") {
                                    if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {

                                        var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();
                                        var blbFile = "";
                                        var ImageSrc = "";
                                        if (typeof ansBlobData[0] != "undefined") {
                                            blbFile = ansBlobData[0].AttachedDocument;
                                        }

                                        if (blbFile != "") {
                                            ImageSrc = URL.createObjectURL(blbFile);
                                            this.AnsBlob.push({ QuestionID: Questionsdlts.QuestionID, AnswerType: Questionsdlts.QuestionType, blobData: blbFile, blobFileName: ansBlobData[0].FileName, IsUploadedFile: ansBlobData[0].IsUploadedFile, benefIndex: BenCnt + 1, RQstInd: -1 });
                                        }

                                        // To Read Photo
                                        if (Questionsdlts.QuestionType == "Photo Capture" && blbFile != "") {

                                            //var UploadIndex = benefSurveyQstn.filter(x => x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                            Questionsdlts.IsUpload = 1;
                                            this.audUpload[QCnt] = 1;
                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedImg = ImageSrc;

                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].img = ImageSrc;

                                        }
                                        if (Questionsdlts.QuestionType == "File Upload" && blbFile != "") {
                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedFile = ImageSrc;
                                        }
                                        else if (Questionsdlts.QuestionType == "Signature Capture") {
                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedSign = ImageSrc;
                                        }
                                        this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                                        //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].AnsBlob = ansBlobData[0].FileName;
                                    }
                                }
                                else if (type == "Reject") {
                                    if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {
                                        var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();
                                        var blbFile = "";
                                        if (typeof ansBlobData[0] != "undefined") {
                                            blbFile = ansBlobData[0].AttachedDocument;
                                        }
                                        if (Questionsdlts.QuestionType == "Photo Capture") {
                                            Questionsdlts.IsUpload = 1;
                                            this.audUpload[QCnt] = 1;

                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedImg = this.fileUrl + Answer;
                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].img = this.fileUrl + Answer;
                                        }
                                        else if (Questionsdlts.QuestionType == "File Upload") {

                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedFile = this.fileUrl + Answer;
                                        }
                                        else if (Questionsdlts.QuestionType == "Signature Capture") {
                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].uploadedSign = this.fileUrl + Answer;
                                        }

                                        this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                                    }
                                }
                            } else if (Questionsdlts.QuestionType == "Date Picker") {
                                var Ans = Answer.replace(/\b(\d\/)/g, '0$1');
                                var regex_date = /^\d{4}\-\d{1,2}\-\d{1,2}$/;

                                if (!regex_date.test(Ans)) {
                                    this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = objUtils.common.formatDateTodayDate(Ans);
                                }
                                else { this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Ans; }

                            }
                            else {

                                this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                            }

                        }
                    }
                }
                this.lstSectionsQstns = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                this.BenefLinkedSurveySteps = 4;
            }
            else {
                this.lstSectionsQstns = [];
                this.lstSectionsQstns = await this.GetSectionsForEachBeneficiary(0, 0);
                var Sections = this.lstSectionsQstns;
                for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {

                    // Questions loop
                    for (var QCnt = 0; QCnt <= Sections[SecCnt].Questions.length - 1; QCnt++) {

                        var Questionsdlts = Sections[SecCnt].Questions[QCnt];
                        var Answer = "";
                        if (typeof Answerlist.filter(x => x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                            Answer = Answerlist.filter(x => x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                            if (Answer == "") {
                                Answer = Answerlist.filter(x => x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer1;
                            }
                        }

                        if (Questionsdlts.QuestionType == 'Cascading Dropdown') {

                            if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {
                                AnsOptions = Answer.split(',');
                                for (var j = 0; j < AnsOptions.length; j++) {
                                    var pID = 0;
                                    if (Questionsdlts.CascadingQuestionOptions[j].CascadingList != null)
                                        pID = Questionsdlts.CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                    Questionsdlts.CascadingQuestionOptions[j][j] = pID;
                                    this.FilterCascadingItems(Questionsdlts, Questionsdlts.CascadingQuestionOptions[j], pID, false, this.BeneficiarySectionQstnList[BenCnt].BenefIndex, -1);
                                }
                            }
                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }
                        else if (Questionsdlts.QuestionType == 'Multi-Choice') {

                            if (typeof Answer != 'undefied' && Answer != null && Answer.length > 0) {
                                var AnsOptions = Answer.split(',');
                                // this.multiChoiceAns = AnsOptions;
                                for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                    Questionsdlts.QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                }
                            }

                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }

                        else if ((Questionsdlts.QuestionType == "Photo Capture") || (Questionsdlts.QuestionType == "Signature Capture") ||
                            (Questionsdlts.QuestionType == "File Upload") || (Questionsdlts.QuestionType == "Audio Capture")) {

                            if (type == "Saved") {


                                var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();
                                var blbFile = "";
                                var ImageSrc = "";
                                if (typeof ansBlobData[0] != "undefined") {
                                    blbFile = ansBlobData[0].AttachedDocument;
                                }
                                if (blbFile != "") {
                                    ImageSrc = URL.createObjectURL(blbFile);
                                    this.AnsBlob.push({ QuestionID: Questionsdlts.QuestionID, AnswerType: Questionsdlts.QuestionType, blobData: blbFile, blobFileName: ansBlobData[0].FileName, IsUploadedFile: ansBlobData[0].IsUploadedFile, benefIndex: this.BenefStepper, RQstInd: -1 });
                                }

                                // To Read Photo
                                if (Questionsdlts.QuestionType == "Photo Capture" && blbFile != "") {
                                    Questionsdlts.IsUpload = 1;
                                    this.audUpload[QCnt] = 1;
                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedImg = ImageSrc;
                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].img = ImageSrc;

                                }
                                if (Questionsdlts.QuestionType == "File Upload" && blbFile != "") {

                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedFile = ImageSrc;
                                }
                                else if (Questionsdlts.QuestionType == "Signature Capture") {


                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedSign = ImageSrc;
                                }

                                this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                                //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].AnsBlob = ansBlobData[0].FileName;
                            }
                            else if (type == "Reject") {

                                if (Questionsdlts.QuestionType == "Photo Capture") {
                                    Questionsdlts.IsUpload = 1;
                                    this.audUpload[QCnt] = 1;

                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedImg = this.fileUrl + Answer;
                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].img = this.fileUrl + Answer;
                                }
                                else if (Questionsdlts.QuestionType == "File Upload") {

                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedFile = this.fileUrl + Answer;
                                }
                                else if (Questionsdlts.QuestionType == "Signature Capture") {
                                    this.lstSectionsQstns[SecCnt].Questions[QCnt].uploadedSign = this.fileUrl + Answer;
                                }
                                this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                            }

                        } else if (Questionsdlts.QuestionType == "Date Picker") {

                            var Ans = Answer.replace(/\b(\d\/)/g, '0$1');
                            var regex_date = /^\d{4}\-\d{1,2}\-\d{1,2}$/;

                            if (!regex_date.test(Ans)) {
                                this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = objUtils.common.formatDateTodayDate(Ans);
                            }
                            else { this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Ans; }

                        }
                        else {
                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }

                    }
                }
            }
            await this.fnTriggerSkipOrRepeatLogic();
        },

        //This function is used to perform, Fetching Questions and Answers from saved list.
        //If  Saved list Contains 2 beneficiary, we can add or remove beneficiary from the list, In this case we need to update the saved list , with new or existing beneficiary , on such case we are using this function to update the sections and Questions for each beneficiary.

        async fnFetchQstnAnswersSaveAndExit(type) {
            var lstSavedData = [];
            var SelectedTaskDtls = [];
            var Questions = [];
            var SavedList = [];
            if (type == "Saved") {
                lstSavedData = await dbImworks.SavedTaskDB.toArray();
                this.SavedTaskDetails = lstSavedData.filter(x => x.WorkspaceID == this.WorkspaceID);
                SelectedTaskDtls = this.lstSelectedTaskData
                if (this.SavedTaskDetails.length > 0) {

                    if (this.lstSelectedTaskData.Periodicity != "Anytime") {
                        SavedList = this.SavedTaskDetails.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.TaskDate == this.SelectedTaskDate);
                    } else if (this.lstSelectedTaskData.Periodicity == "Anytime") {
                        SavedList = this.SavedTaskDetails.filter(x => x.TypeID == this.SelectedTaskTypeID && x.Type == 'Survey' && x.TaskSubmittedDate == this.AnyTimeSubmissionDateTime);
                    }
                    var Answerlist = JSON.parse(SavedList[0].SurveyAnswers);
                    //this.lstSelectedBeneficiary = JSON.parse(SavedList[0].lstSelectedBeneficiary);
                    //this.vmSelectedBeneficiaries = JSON.parse(SavedList[0].SelectedBeneficiaries);
                    this.cascadingAns = JSON.parse(SavedList[0].CascadingAnsList);
                    this.multiChoiceAns = JSON.parse(SavedList[0].MultiChoiceAnsList);
                }
            }
            else { // in the case of reject
                SavedList = this.lstSelectedTaskData.TaskData;
                //this.SavedTaskDetails = JSON.stringify(TaskData);
                SelectedTaskDtls = this.lstSelectedTaskData
                var Answerlist = (this.lstSelectedTaskData.TaskData.BeneficiarySubmittedList);

                this.cascadingAns = (this.lstSelectedTaskData.TaskData.CascadingAnsList);
                this.multiChoiceAns = [];


            }
            // Beneficiary Loop
            if (this.IsBeneficiaryLinkedSurvey == true) {
                for (var BenCnt = 0; BenCnt <= this.BeneficiarySectionQstnList.length - 1; BenCnt++) {
                    var Sections = this.BeneficiarySectionQstnList[BenCnt].Sections;
                    // Sections Loop
                    for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {

                        // Questions loop
                        for (var QCnt = 0; QCnt <= Sections[SecCnt].Questions.length - 1; QCnt++) {

                            var Questionsdlts = Sections[SecCnt].Questions[QCnt];
                            var Answer = "";
                            if (type == "Saved") {
                                if (typeof Answerlist.filter(x => x.ResponseID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                                    Answer = Answerlist.filter(x => x.ResponseID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                                }
                            }
                            else {
                                if (typeof Answerlist.filter(x => x.BeneficiaryID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                                    Answer = Answerlist.filter(x => x.BeneficiaryID == Sections[SecCnt].Questions[QCnt].BeneficiaryID && x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                                }
                            }
                            if (Questionsdlts.QuestionType == 'Cascading Dropdown') {

                                // Clear data before assign
                                if (Questionsdlts.CascadingQuestionOptions.length > 0) {

                                    var CascadingOption = Questionsdlts.CascadingQuestionOptions
                                    for (var k = 0; k < CascadingOption.length; k++) {
                                        Questionsdlts.CascadingQuestionOptions[k][0] = null;
                                        Questionsdlts.CascadingQuestionOptions[k][k] = null;
                                    }
                                }
                                AnsOptions = Answer.split(',');
                                if (Answer.length > 0) {
                                    for (var j = 0; j < AnsOptions.length; j++) {
                                        var pID = 0;
                                        if (Questionsdlts.CascadingQuestionOptions[j].CascadingList != null)
                                            pID = Questionsdlts.CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                        Questionsdlts.CascadingQuestionOptions[j][j] = pID;
                                        this.FilterCascadingItems(Questionsdlts, Questionsdlts.CascadingQuestionOptions[j], pID, false, this.BeneficiarySectionQstnList[BenCnt].BenefIndex, -1);
                                    }
                                }
                                this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                            }
                            else if (Questionsdlts.QuestionType == 'Multi-Choice') {
                                //for (var mCnt2 = 0; mCnt2 < Questionsdlts.QuestionOptions.length; mCnt2++) {
                                //    Questionsdlts.QuestionOptions[mCnt2].IsOptSelected = false;
                                //}
                                var AnsOptions = Answer.split(',');
                                if (Answer.length > 0) {
                                    // this.multiChoiceAns = AnsOptions;
                                    for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                        Questionsdlts.QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                    }
                                    if (type == "Rejected") {

                                        for (var mCnt1 = 0; mCnt1 < Questionsdlts.QuestionOptions.length; mCnt1++) {
                                            if (Questionsdlts.QuestionOptions[mCnt1].IsOptSelected == true) {
                                                this.multiChoiceAns.push({ OptionValue: Questionsdlts.QuestionOptions[mCnt1].OptionValue, QuestionID: Questionsdlts.QuestionID, benefIndex: BenCnt + 1 });
                                            }
                                        }
                                    }
                                }

                                this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                            }

                            else if ((Questionsdlts.QuestionType == "Photo Capture") || (Questionsdlts.QuestionType == "Signature Capture") ||
                                (Questionsdlts.QuestionType == "File Upload") || (Questionsdlts.QuestionType == "Audio Capture")) {

                                var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();

                                var blbFile = "";
                                if (typeof ansBlobData[0] != "undefined") {
                                    blbFile = ansBlobData[0].AttachedDocument;
                                }
                                // To Read Photo
                                if (Questionsdlts.QuestionType == "Photo Capture" && blbFile != "") {

                                    //var UploadIndex = benefSurveyQstn.filter(x => x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                    Questionsdlts.IsUpload = 1;
                                    this.audUpload[QCnt] = 1;
                                    if (ansBlobData[0].IsUploadedFile) {
                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        // fr.addEventListener('load', () => {
                                        //     this.uploadedImg = fr.result

                                        // });
                                        await fr.addEventListener('loadend', () => {
                                            //currSecQstns[i].uploadedSign = fr.result

                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt - 1].uploadedImg = fr.result;
                                        });
                                    }
                                    else {
                                        const fr = new FileReader()
                                        fr.readAsDataURL(blbFile)
                                        // fr.addEventListener('load', () => {
                                        //     this.img = fr.result
                                        // });
                                        await fr.addEventListener('loadend', () => {
                                            //currSecQstns[i].uploadedSign = fr.result

                                            this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt - 1].img = fr.result;
                                        });
                                    }
                                }
                                if (Questionsdlts.QuestionType == "File Upload" && blbFile != "") {

                                    const fr = new FileReader()
                                    fr.readAsDataURL(blbFile)
                                    fr.addEventListener('loadend', () => {

                                        this.BeneficiarySectionQstnList[BenCnt - 1].Sections[SecCnt].Questions[QCnt + 1].uploadedFile = fr.result;

                                    });
                                }
                                this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                                //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].AnsBlob = ansBlobData[0].FileName;
                            }
                            else {
                                this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].Answer = Answer;
                            }

                        }
                    }
                }
                this.lstSectionsQstns = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                // this.BenefLinkedSurveySteps = 4;
            }
            else {
                this.lstSectionsQstns = [];
                this.lstSectionsQstns = await this.GetSectionsForEachBeneficiary(0, 0);
                var Sections = this.lstSectionsQstns;
                for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                    // Questions loop
                    for (var QCnt = 0; QCnt <= Sections[SecCnt].Questions.length - 1; QCnt++) {

                        var Questionsdlts = Sections[SecCnt].Questions[QCnt];
                        var Answer = "";
                        if (typeof Answerlist.filter(x => x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0] != "undefined") {

                            Answer = Answerlist.filter(x => x.QuestionID == Sections[SecCnt].Questions[QCnt].QuestionID)[0].Answer;
                        }

                        if (Questionsdlts.QuestionType == 'Cascading Dropdown') {

                            AnsOptions = Answer.split(',');
                            if (Answer.length > 0) {
                                for (var j = 0; j < AnsOptions.length; j++) {
                                    var pID = 0;
                                    if (Questionsdlts.CascadingQuestionOptions[j].CascadingList != null)
                                        pID = Questionsdlts.CascadingQuestionOptions[j].CascadingList.filter(x => x.Name == AnsOptions[j])[0].ID;
                                    Questionsdlts.CascadingQuestionOptions[j][j] = pID;
                                    this.FilterCascadingItems(Questionsdlts, Questionsdlts.CascadingQuestionOptions[j], pID, false, this.BeneficiarySectionQstnList[BenCnt].BenefIndex, -1);
                                }
                            }
                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }
                        else if (Questionsdlts.QuestionType == 'Multi-Choice') {
                            var AnsOptions = Answer.split(',');
                            if (Answer.length > 0) {
                                // this.multiChoiceAns = AnsOptions;
                                for (var mCnt = 0; mCnt < AnsOptions.length; mCnt++) {
                                    Questionsdlts.QuestionOptions.filter(x => x.OptionValue == AnsOptions[mCnt])[0].IsOptSelected = true;
                                }
                            }

                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }
                        else if ((Questionsdlts.QuestionType == "Photo Capture") || (Questionsdlts.QuestionType == "Signature Capture") ||
                            (Questionsdlts.QuestionType == "File Upload") || (Questionsdlts.QuestionType == "Audio Capture")) {

                            var ansBlobData = await dbImworks.TbTaskResponseDocument.where("FileName").equals(Answer).toArray();

                            var blbFile = "";
                            if (typeof ansBlobData[0] != "undefined") {
                                blbFile = ansBlobData[0].AttachedDocument;
                            }
                            // To Read Photo
                            if (Questionsdlts.QuestionType == "Photo Capture" && blbFile != "") {

                                //var UploadIndex = benefSurveyQstn.filter(x => x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                Questionsdlts.IsUpload = 1;
                                this.audUpload[QCnt] = 1;
                                if (ansBlobData[0].IsUploadedFile) {
                                    const fr = new FileReader()
                                    fr.readAsDataURL(blbFile)
                                    // fr.addEventListener('load', () => {
                                    //     this.uploadedImg = fr.result

                                    // });
                                    await fr.addEventListener('loadend', () => {
                                        //currSecQstns[i].uploadedSign = fr.result
                                        //this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                                        this.lstSectionsQstns[SecCnt].Questions[QCnt - 1].uploadedImg = fr.result;
                                    });
                                }
                                else {
                                    const fr = new FileReader()
                                    fr.readAsDataURL(blbFile)
                                    // fr.addEventListener('load', () => {
                                    //     this.img = fr.result
                                    // });
                                    await fr.addEventListener('loadend', () => {
                                        //currSecQstns[i].uploadedSign = fr.result

                                        this.lstSectionsQstns[SecCnt][SecCnt].Questions[QCnt - 1].img = fr.result;
                                    });
                                }
                            }
                            if (Questionsdlts.QuestionType == "File Upload" && blbFile != "") {

                                const fr = new FileReader()
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('loadend', () => {

                                    this.lstSectionsQstns[SecCnt].Questions[QCnt + 1].uploadedFile = fr.result;

                                });
                            }
                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                            //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt].AnsBlob = ansBlobData[0].FileName;
                        }
                        else {
                            this.lstSectionsQstns[SecCnt].Questions[QCnt].Answer = Answer;
                        }

                    }
                }
            }
        },

        async UpdateTask() {
            var lstTaskResponse = [];
            var today = new Date().toLocaleString("en-US");
            // To Get Survey Answers
            var lstAnswers = [];
            var Questions = [];
            var SurveyAnswers = "";
            var BenfID = 0; // in case of beneficiary ID is Less than zero
            if (this.IsImportSurvey == false) {

                // Manual Survey Update
                if (this.IsBeneficiaryLinkedSurvey == true) {
                    for (var BenCnt = 0; BenCnt <= this.BeneficiarySectionQstnList.length - 1; BenCnt++) {

                        if (this.BeneficiarySectionQstnList[BenCnt].BeneficiaryID < 0) {
                            BenfID--;
                        }
                        var Sections = this.BeneficiarySectionQstnList[BenCnt].Sections;
                        for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                            Questions = [];
                            if (Sections[SecCnt].IslogicSection == 1) {

                                Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.benefIndex == Sections[SecCnt].BenefIndex && x.QuestionID > 0)

                            }
                            else {

                                Questions = Sections[SecCnt].Questions;
                            }
                            for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) {
                                var Obj = new Object();
                                Obj.ID = 0
                                Obj.SurveyID = Questions[QCnt].SurveyID;
                                Obj.SectionID = Questions[QCnt].SectionID;
                                Obj.ResponseID = Questions[QCnt].BeneficiaryID < 0 ? BenfID : Questions[QCnt].BeneficiaryID;
                                Obj.SyncTaskID = 0;
                                Obj.QuestionID = Questions[QCnt].QuestionID;
                                Obj.QuestionName = Questions[QCnt].QuestionName;
                                Obj.QuestionType = Questions[QCnt].QuestionType;
                                Obj.Answer = Questions[QCnt].Answer;
                                if (Questions[QCnt].QuestionTypeID == 9 && (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer == '' || Questions[QCnt].Answer == '0,0')) {
                                    var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                                    if (this.Coordinates.lat == 0 && this.Coordinates.lng == 0) {
                                        gpsCoordinates = this.defaultCoordinates.lat.toString() + "," + this.defaultCoordinates.lng.toString();
                                    }
                                    Obj.Answer = gpsCoordinates;
                                }
                                //Obj.CreatedDate = Questions[QCnt].CreatedDate;
                                //Obj.UpdatedDate = Questions[QCnt].UpdatedDate;
                                Obj.BeneficiaryInd = BenCnt + 1;
                                lstAnswers.push(Obj);
                            }
                        }
                    }
                } else {

                    var Sections = this.lstSectionsQstns;
                    for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) {
                        Questions = [];
                        if (Sections[SecCnt].IslogicSection == 1) {

                            Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.QuestionID > 0)

                        }
                        else {

                            Questions = Sections[SecCnt].Questions;
                        }
                        for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) {
                            var Obj = new Object();
                            Obj.ID = 0
                            Obj.SurveyID = Questions[QCnt].SurveyID;
                            Obj.SectionID = Questions[QCnt].SectionID;
                            Obj.ResponseID = Questions[QCnt].BeneficiaryID < 0 ? BenfID : Questions[QCnt].BeneficiaryID;
                            Obj.SyncTaskID = 0;
                            Obj.QuestionID = Questions[QCnt].QuestionID;
                            Obj.QuestionName = Questions[QCnt].QuestionName;
                            Obj.QuestionType = Questions[QCnt].QuestionType;
                            Obj.Answer = Questions[QCnt].Answer;
                            if (Questions[QCnt].QuestionTypeID == 9 && (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer == '')) {
                                var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                                Obj.Answer = gpsCoordinates;
                            }
                            //Obj.CreatedDate = Questions[QCnt].CreatedDate;
                            //Obj.UpdatedDate = Questions[QCnt].UpdatedDate;
                            Obj.BeneficiaryInd = 1;
                            lstAnswers.push(Obj);
                        }
                    }
                }
                SurveyAnswers = JSON.stringify(lstAnswers);
            }
            else {
                // Import Survey Update
                SurveyAnswers = JSON.stringify(this.lstBenefSurveyQstnAnswers);
            }
            if (typeof this.lstSelectedTaskData != "undefined" && this.lstSelectedTaskData != null) {

                lstTaskResponse.push({
                    SyncDBID: this.lstSelectedTaskData.SyncDBID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn, Status: 'DataCollected',
                    WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: this.lstSelectedTaskData.TotalWFLevel, WFLevel: 0, WFLevelStatus: "Completed", TaskUpdateComment: this.vmUpdateComment,
                    AttachedDocumentName: "", SurveyAnswers: SurveyAnswers, ResponseSyncID: this.lstSelectedTaskData.ResponseSyncID, DataCollectedBy: this.lstSelectedTaskData.DataCollectedBy,
                    NewBeneficiaries: JSON.stringify(this.lstNewBeneficiary),
                    TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.lstSelectedTaskData.WorkspaceID
                });

                //var clientguid = uuidv4(); // generate 36 character client guid

                await dbImworks.TaskSyncDB.put({
                    Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn,
                    Status: 'DataCollected', WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: this.lstSelectedTaskData.WorkflowID, WFLevel: 0, ClientData: JSON.stringify(lstTaskResponse),
                    lastSyncDBID: this.lstSelectedTaskData.SyncDBID, IsBeneficiaryLinked: this.IsBeneficiaryLinkedSurvey, TaskDate: this.SelectedTaskDate, Periodicity: this.lstSelectedTaskData.Periodicity, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, TaskSubmittedDate: today, ClientGuid: this.clientguid

                });

                var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();

                // Getting Total Question Count
                var QuestionCount = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.IsConditionQuestion != 1).length;
                if (this.RepeatOrSkipSection.length > 0) {
                    QuestionCount += this.RepeatOrSkipSection.filter(x => x.QuestionID > 0).length
                }

                // Inserting into TbMyTask
                var lastClientSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                await dbImworks.TbMyTask.put({
                    ClientSyncTaskID: lastClientSyncID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0, WFLevelStatus: "Completed",
                    TaskUpdateComment: this.vmUpdateComment, AttachedDocumentName: '', SyncDBID: this.lstSelectedTaskData.SyncDBID,
                    ProjectName: '', TaskName: '', Description: '',
                    CreatedDate: today, TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, Periodicity: '', TaskSubmittedBy: data[0].UserName, BeneficiarySubmittedList: JSON.stringify(this.vmSelectedBeneficiaries), SurveyAnswers: SurveyAnswers, AnytimeSubmissionID: this.AnyTimeSubmissionId, ToDelete: 1, QuestionCount: QuestionCount,
                    NextAssignToUserId: this.lstSelectedTaskData.NextLevelAssignedUsersIds,
                    NextLevelAssignedUser: this.lstSelectedTaskData.NextLevelAssignedUser != null ? this.lstSelectedTaskData.NextLevelAssignedUser : "NA",
                    WorkflowStage: this.lstSelectedTaskData.WorkflowID == 0 ? "NA" : "Level " + (this.lstSelectedTaskData.WFLevel + 1),
                    //WorkflowStage :"Level " + (this.lstSelectedTaskData.WFLevel + 1),
                    //ActionRequired : "To Review"
                    ActionRequired: this.lstSelectedTaskData.WorkflowID == 0 ? "Completed" : "To Review",
                    selectedTaskpkId: this.selectedTaskpkId, Command: 'TaskResponse'
                });

            }

            this.SnackbarMsg = "Data Collection Completed ...";
            if (this.AnyTimeSubmissionId == 0) {
                // Updating Current Tasks Status
                if (this.TaskCardData.length > 0) {
                    var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog
                    if (typeof SelectedTaskDetails != "undefined") {
                        var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog.filter(y => y.TaskIndex == this.SelectedTaskIndex)[0]
                        SelectedTaskDetails.CurrentOrOverdue = "Syncing"
                        SelectedTaskDetails.CreatedDate = today;
                        SelectedTaskDetails.Status = "For Approval";
                        //SelectedTaskDetails.Message = "Survey Data Updated";
                        SelectedTaskDetails.Comment = this.vmUpdateComment;
                        SelectedTaskDetails.Name = data[0].UserName;
                        SelectedTaskDetails.WorkflowStage = "Level " + (SelectedTaskDetails.WFLevel + 1);
                        SelectedTaskDetails.ActionRequired = "To Review";
                        SelectedTaskDetails.AssignedTo = SelectedTaskDetails.NextLevelAssignedUser;
                        SelectedTaskDetails.AssignedToIds = SelectedTaskDetails.NextLevelAssignedUsersIds;
                        var SelectedAnytime = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0]
                        SelectedAnytime.btnAnytimeSubVisible = true;
                        if (SelectedTaskDetails.WorkflowID == 0) {
                            SelectedTaskDetails.WorkflowStage = "NA";
                            SelectedTaskDetails.ActionRequired = "Completed";
                            SelectedTaskDetails.AssignedTo = "NA";
                        }
                    }
                }

            }
            else if (this.AnyTimeSubmissionId > 0) {
                // Update Status to any time dexie table
                if (this.lstAnyTimeNewSubmission.length > 0) {
                    var AnyTimeData = this.lstAnyTimeNewSubmission.filter(x => x.pkId == this.AnyTimeSubmissionId)[0];
                    if (typeof AnyTimeData != "undefined") {
                        AnyTimeData.TaskData.CurrentOrOverdue = "Syncing";
                        AnyTimeData.TaskData.CreatedDate = today;
                        AnyTimeData.TaskData.Status = "For Approval";
                        // AnyTimeData.TaskData.Message = "Survey Data Updated";
                        AnyTimeData.TaskData.Comment = this.vmUpdateComment;
                        AnyTimeData.TaskData.Name = data[0].UserName;
                        AnyTimeData.TaskData.WorkflowStage = "Level " + (AnyTimeData.TaskData.WFLevel + 1);
                        AnyTimeData.TaskData.ActionRequired = "To Review";
                        AnyTimeData.TaskData.AssignedTo = AnyTimeData.TaskData.NextLevelAssignedUser;
                        if (AnyTimeData.TaskData.WorkflowID == 0) {
                            AnyTimeData.TaskData.WorkflowStage = "NA";
                            AnyTimeData.TaskData.ActionRequired = "Completed";
                            AnyTimeData.TaskData.AssignedTo = "NA";
                        }

                        var pkId = [];
                        pkId = await dbImworks.TbAnyTimeData.where({ pkId: parseInt(this.AnyTimeSubmissionId) }).toArray();

                        if (pkId.length > 0) {
                            await dbImworks.TbAnyTimeData.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                        }

                        //var data = this.TaskCardData.filter(x => x.TaskType == this.SelectedTaskType && x.TypeID == this.SelectedTaskTypeID)[0].TaskLog;
                        await dbImworks.TbAnyTimeData.put({
                            TaskData: AnyTimeData.TaskData
                        });
                    }
                }
            }


            // Deleting the Task fro SavedDb if Exists
            // Delete Saved Data if Exists

            var pkId = [];
            pkId = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();

            if (pkId.length > 0) {
                await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
            }
            if (this.selectedTaskpkId > 0) {
                await dbImworks.MyTaskList.where("pkId").equals(parseInt(this.selectedTaskpkId)).modify({ ToDelete: 2 });
            }

            //this.SelectedTaskIndex;
            this.SurveyBenLinkDlg = false;
            this.SuccessSnackbar = true;
            await this.fnClearData();

        },


        async ApproveOrReject() {

            if (this.SelectedAppStatus.length == 0) {

                this.SnackbarMsg = "Please select Approve / Reject";
                this.InfoSnackbarMsg = true;
                return false;
            }
            if (this.SelectedAppStatus == 'Approve') {

                await this.ApproveTask();
            }
            if (this.SelectedAppStatus == 'Reject') {

                await this.RejectTask();
            }
            this.SelectedAppStatus = "";
        },

        // For Approval Task

        async ApproveTask() {

            var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
            var lstTaskResponse = [];
            if (this.TaskApproveAttachment) {
                this.ApproveDocName = this.TaskApproveAttachment.name;
            }
            var today = new Date().toLocaleString("en-US");

            var totWF = this.lstSelectedTaskData.TotalWFLevel;

            var lastSyncDBID = this.lstSelectedTaskData.SyncDBID;

            var SurveyAnswers = "";//JSON.stringify(this.SurveySectionList);
            lstTaskResponse.push({
                SyncDBID: lastSyncDBID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn, Status: 'DataApproved',
                WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: totWF, WFLevel: this.lstSelectedTaskData.WFLevel, WFLevelStatus: "Completed", TaskUpdateComment: this.TaskApproveDescription,
                AttachedDocumentName: this.ApproveDocName, SurveyAnswers: SurveyAnswers, ResponseSyncID: this.lstSelectedTaskData.ResponseSyncID, DataCollectedBy: this.lstSelectedTaskData.DataCollectedBy,
                TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.WorkspaceID

            });

            var TaskStatus = "DataApproved";
            if (totWF == this.lstSelectedTaskData.WFLevel) {
                TaskStatus = "Completed"
            }

            // var clientguid = uuidv4(); // generate 36 character client guid

            await dbImworks.TaskSyncDB.put({
                Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn,
                Status: TaskStatus, WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: totWF, WFLevel: this.lstSelectedTaskData.WFLevel, ClientData: JSON.stringify(lstTaskResponse),
                Periodicity: this.lstSelectedTaskData.Periodicity, lastSyncDBID: lastSyncDBID, IsBeneficiaryLinked: true, TaskDate: this.SelectedTaskDate, WorkspaceID: this.WorkspaceID, TaskSubmittedDate: today, ClientGuid: this.clientguid
            });
            var AssignedTo = "";
            var NextLevelAssignedUsersIds = "";
            if ((this.lstSelectedTaskData.WFLevel >= this.lstSelectedTaskData.TotalWFLevel) && (this.lstSelectedTaskData.AssignedTo == this.lstSelectedTaskData.NextLevelAssignedUser)) {

                AssignedTo = null;
                NextLevelAssignedUsersIds = 0;
            }
            else {

                AssignedTo = this.lstSelectedTaskData.NextLevelAssignedUser;
                NextLevelAssignedUsersIds = this.lstSelectedTaskData.NextLevelAssignedUsersIds;
            }
            var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
            await dbImworks.TbMyTask.put({
                ClientSyncTaskID: lastSyncID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: this.lstSelectedTaskData.WFLevel, WFLevelStatus: "Completed",
                TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.lstSelectedTaskData.ProjectName,
                TaskName: this.lstSelectedTaskData.ActivityName, Description: this.lstSelectedTaskData.Description, CreatedDate: today,
                TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.WorkspaceID, TaskSubmittedBy: data[0].UserName, ToDelete: 1,
                NextAssignToUserId: NextLevelAssignedUsersIds,
                NextLevelAssignedUser: AssignedTo, selectedTaskpkId: this.selectedTaskpkId, Command: 'TaskResponse'
            });

            if (this.TaskApproveAttachment != null) {
                await dbImworks.TbTaskDocument.put({

                    SyncTaskID: lastSyncDBID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: this.lstSelectedTaskData.WFLevel, AttachedDocument: this.TaskApproveAttachment,
                    AttachedDocumentName: this.TaskApproveAttachment.name, DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                    DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0), DocumentType: this.TaskApproveAttachment.type, DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                    DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                    ProjectID: this.lstSelectedTaskData.ProjectID, ProjectName: this.lstSelectedTaskData.ProjectName, DocLinkedTo: this.lstSelectedTaskData.ActivityName, TaskDate: this.SelectedTaskDate
                });
            }

            // Updating Current Tasks Status
            var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog.filter(y => y.TaskIndex == this.SelectedTaskIndex)[0]
            SelectedTaskDetails.CurrentOrOverdue = "Syncing"
            SelectedTaskDetails.CreatedDate = today;
            SelectedTaskDetails.Status = "Approved";
            //SelectedTaskDetails.Message = "Survey Data Approved";
            SelectedTaskDetails.Comment = this.TaskApproveDescription;
            SelectedTaskDetails.Name = data[0].UserName;
            if ((SelectedTaskDetails.WFLevel >= SelectedTaskDetails.TotalWFLevel) && (SelectedTaskDetails.AssignedTo == SelectedTaskDetails.NextLevelAssignedUser)) {
                SelectedTaskDetails.WorkflowStage = "NA";
                SelectedTaskDetails.ActionRequired = "Completed";
                SelectedTaskDetails.AssignedTo = "NA";
                SelectedTaskDetails.NextLevelAssignedUser = null;
                SelectedTaskDetails.AssignedToIds = 0;
            }
            else {
                SelectedTaskDetails.WorkflowStage = "Level " + (SelectedTaskDetails.WFLevel + 1);
                SelectedTaskDetails.ActionRequired = "To Review";
                SelectedTaskDetails.AssignedTo = SelectedTaskDetails.NextLevelAssignedUser;
                SelectedTaskDetails.AssignedToIds = SelectedTaskDetails.NextLevelAssignedUsersIds;
            }



            // Deleting the Task fro SavedDb if Exists
            // Delete Saved Data if Exists

            var pkId = [];
            pkId = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();

            if (pkId.length > 0) {
                await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
            }

            await this.fnClearData();
            this.SnackbarMsg = "Data Approved Successfully ...";
            //this.SelectedTaskIndex;

            this.InfoSnackbarMsg = true;


        },

        // Reject Task
        async RejectTask() {

            var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
            var lstTaskResponse = [];
            if (this.TaskApproveAttachment) {
                this.ApproveDocName = this.TaskApproveAttachment.name;
            }

            var today = new Date().toLocaleString("en-US");
            var totWF = this.lstSelectedTaskData.TotalWFLevel;

            var lastSyncDBID = this.lstSelectedTaskData.SyncDBID;

            var SurveyAnswers = "";//JSON.stringify(this.SurveySectionList);


            lstTaskResponse.push({
                SyncDBID: lastSyncDBID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn, Status: 'Rejected',
                WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: totWF, WFLevel: this.lstSelectedTaskData.WFLevel, WFLevelStatus: "Rejected", TaskUpdateComment: this.TaskApproveDescription,
                AttachedDocumentName: this.ApproveDocName, SurveyAnswers: SurveyAnswers, ResponseSyncID: this.lstSelectedTaskData.ResponseSyncID, DataCollectedBy: this.lstSelectedTaskData.DataCollectedBy,
                TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.WorkspaceID

            });

            // var clientguid = uuidv4(); // generate 36 character client guid

            await dbImworks.TaskSyncDB.put({
                Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn,
                Status: 'Rejected', WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: totWF, WFLevel: this.lstSelectedTaskData.WFLevel, ClientData: JSON.stringify(lstTaskResponse),
                Periodicity: this.lstSelectedTaskData.Periodicity, lastSyncDBID: lastSyncDBID, IsBeneficiaryLinked: true, TaskDate: this.SelectedTaskDate, WorkspaceID: this.WorkspaceID, TaskSubmittedDate: today, ClientGuid: this.clientguid
            });

            var lastSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
            await dbImworks.TbMyTask.put({
                ClientSyncTaskID: lastSyncID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: this.lstSelectedTaskData.WFLevel, WFLevelStatus: "Rejected",
                TaskUpdateComment: this.TaskApproveDescription, AttachedDocumentName: this.ApproveDocName, SyncDBID: lastSyncDBID, ProjectName: this.lstSelectedTaskData.ProjectName,
                TaskName: this.lstSelectedTaskData.ActivityName, Description: this.lstSelectedTaskData.Description, CreatedDate: today,
                TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.WorkspaceID, TaskSubmittedBy: data[0].UserName, ToDelete: 1, currentWFLevel: this.lstSelectedTaskData.WFLevel, NextAssignToUserId: this.lstSelectedTaskData.DataCollectorIds,
                NextLevelAssignedUser: this.lstSelectedTaskData.DataCollectorName, selectedTaskpkId: this.selectedTaskpkId, Command: 'TaskResponse'
            });

            if (this.TaskApproveAttachment != null) {
                await dbImworks.TbTaskDocument.put({

                    SyncTaskID: lastSyncDBID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: this.lstSelectedTaskData.WFLevel, AttachedDocument: this.TaskApproveAttachment,
                    AttachedDocumentName: this.TaskApproveAttachment.name, DocumentOriginalName: this.TaskApproveAttachment.name.split('.').slice(0, -1).join('.'),
                    DocumentSize: this.formatFileSize(this.TaskApproveAttachment.size, 0), DocumentType: this.TaskApproveAttachment.type, DocumentTypeIcon: this.TaskApproveAttachment.name.split('.').pop(),
                    DocumentLastModifiedDate: this.TaskApproveAttachment.lastModifiedDate != null ? new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(this.TaskApproveAttachment.lastModifiedDate).toISOString().substr(11, 5) : null,
                    ProjectID: this.lstSelectedTaskData.ProjectID, ProjectName: this.lstSelectedTaskData.ProjectName, DocLinkedTo: this.lstSelectedTaskData.ActivityName, TaskDate: this.SelectedTaskDate
                });
            }

            // Updating Current Tasks Status
            var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog.filter(y => y.TaskIndex == this.SelectedTaskIndex)[0]

            SelectedTaskDetails.CurrentOrOverdue = "Syncing"
            SelectedTaskDetails.CreatedDate = today;
            SelectedTaskDetails.Status = "Rejected";
            // SelectedTaskDetails.Message = "Survey Data Rejected";
            SelectedTaskDetails.Comment = this.TaskApproveDescription;
            SelectedTaskDetails.Name = data[0].UserName;
            SelectedTaskDetails.ActionRequired = "To Resubmit";
            SelectedTaskDetails.WorkflowStage = "Survey Submission";
            SelectedTaskDetails.AssignedTo = SelectedTaskDetails.DataCollectorName;
            SelectedTaskDetails.AssignedToIds = SelectedTaskDetails.DataCollectorIds;

            // Deleting the Task fro SavedDb if Exists
            // Delete Saved Data if Exists

            var pkId = [];
            pkId = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();

            if (pkId.length > 0) {
                await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
            }

            await this.fnClearData();
            this.SnackbarMsg = "Data Rejected Successfully ...";
            //this.SelectedTaskIndex;

            this.InfoSnackbarMsg = true;


        },

        async GetSurveyTaskApproveDetails() {
            if (this.showTask == false) {
                this.surveyApproveBenfLinkedDialog = true;
            }
        },

        // to Download Beneficiary Linked Survey Sample excel file
        async DownloadSampleBeneficiaryLinkedSurveyFile() {

            //objUtils.common.MergeCommas(lines[0]);
            var ColumnHeader = "";
            var ColumnData1 = [];
            var ColumnData = [];
            var StrResponse = "Survey Response here";
            for (var j = 0; j < this.vmSelectedBeneficiaries.length; j++) {
                ColumnHeader = "Beneficiary";
                if (this.vmSelectedBeneficiaries[j].BeneficiaryName.includes(',')) {
                    ColumnData1.push("\"" + this.vmSelectedBeneficiaries[j].BeneficiaryName + "\"");
                }
                else {
                    ColumnData1.push(this.vmSelectedBeneficiaries[j].BeneficiaryName);
                }
                //var string = objUtils.common.MergeCommas(this.lstImportSurveyQstnHearder.toString());
                for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++) {

                    if (this.lstImportSurveyQstnHearder[i].text.includes(',')) {
                        ColumnHeader += ColumnHeader == "" ? "\"" + this.lstImportSurveyQstnHearder[i].text + "\"" : "," + "\"" + this.lstImportSurveyQstnHearder[i].text + "\"";
                    }
                    else {
                        ColumnHeader += ColumnHeader == "" ? this.lstImportSurveyQstnHearder[i].text : "," + this.lstImportSurveyQstnHearder[i].text;
                    }

                    if (this.lstImportSurveyQstnHearder[i].type == "date") {
                        ColumnData1.push(StrResponse + " *Please enter date in mm-dd-yyyy format.");
                    } else {
                        ColumnData1.push("Survey Response here");
                    }


                }
                // var string = objUtils.common.MergeCommas(ColumnHeader);
                ColumnHeader = ColumnHeader + "\n";
                ColumnData.push(ColumnData1);
                ColumnData1 = [];

                ColumnData.forEach(function (row) {
                    ColumnHeader += row.join(',');
                    ColumnHeader += "\n";
                });
            }
            // ColumnHeader['Beneficiary'].hidden = true;

            var hiddenElement = document.createElement('a');
            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
            hiddenElement.target = '_blank';
            hiddenElement.download = 'sample_surveyQuestionnaire_' + Math.round(Math.random() * 1000) + '.csv';
            hiddenElement.click();
        },

        // Import Survey
        async ImportSurvey() {
            var benefID = 0;
            var benefName = "";
            var NewBenefID = 0;
            this.IsImportSurvey = true;
            if (await this.ValidateImportSurveyData() == true) {
                for (var i = 0; i < this.lstImportSurvey.length; i++) {
                    for (var j = 0; j < this.lstImportSurveyQstnHearder.length; j++) {
                        if (this.IsBeneficiaryLinkedSurvey == true) // Beneficiaries linked
                        {
                            benefName = this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[0].value];
                            if (typeof this.vmSelectedBeneficiaries.filter(x => x.BeneficiaryName == benefName)[0] != "undefined") {
                                benefID = this.vmSelectedBeneficiaries.filter(x => x.BeneficiaryName == benefName)[0].BeneficiaryID;
                                if (benefID < 0 && j == 0) {
                                    NewBenefID--;
                                }
                            }
                            else {
                                //var length = this.lstNewBeneficiary.length;
                                //benefID = -(length + 1)
                                benefID = NewBenefID--;

                            }

                            if (benefID <= 0) // -1 means new beneficiaries
                            {
                                var Temp = this.lstNewBeneficiary.filter(x => x.BeneficiaryID == benefID)
                                benefID = NewBenefID;
                                if (Temp.length == 0) {
                                    var now = new Date();
                                    var TokenValue = "";
                                    TokenValue = "Beneficiary_" + now.getFullYear().toString() + now.getMonth().toString() + now.getDate().toString() + now.getHours().toString() + now.getMinutes().toString() + now.getSeconds().toString() + now.getMilliseconds().toString();

                                    var lstBenefRegQstnAnswers = [];
                                    this.lstNewBeneficiary.push({
                                        BeneficiaryID: benefID,
                                        BeneficiaryTypeID: this.SelectedBenefTypeID, BeneficiaryName: benefName,
                                        BeneficiaryQstnAnswers: JSON.stringify(lstBenefRegQstnAnswers),
                                        CreatedDate: new Date(),
                                        TokenValue: TokenValue
                                    });
                                }

                            }
                            this.lstBenefSurveyQstnAnswers.push({
                                BeneficiaryInd: i + 1, ResponseID: benefID, SurveyID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].SurveyID,
                                SectionID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].SectionID, QuestionID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].QuestionID,
                                Answer: this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[j + 1].value],
                                FieldVar: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].FieldVar, RQstInd: j
                            });
                        }
                        else // non -benficiary linked
                        {
                            this.lstBenefSurveyQstnAnswers.push({
                                BeneficiaryInd: i + 1, ResponseID: benefID, SurveyID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].SurveyID,
                                SectionID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].SectionID, QuestionID: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].QuestionID, Answer: this.lstImportSurvey[i][this.lstImportSurveyQstnHearderN[j].value], FieldVar: this.lstSelectedTaskData.TaskData.SurveyQstnList[j].FieldVar, RQstInd: j
                            });
                        }
                    }
                }
                await this.fnCalculateProgress();
                if (this.SurveyBenLinkDlg == true) {

                    this.BenefLinkedSurveySteps = 5;
                }
                else {
                    this.NonBenefLinkedSurveySteps = 3;
                }
                this.SurveyFile = [];
                this.lstImportSurvey = [];

            }
            else {
                this.SnackbarMsg = "Please resolve all Invalid Input(s).";
                this.InfoSnackbarMsg = true;
            }
        },
        // Survey Excel Validations
        onBeforeNameChange(...args) {
            var today = new Date().toLocaleString();
            this.IsvalidexcelData = true;
            var list = this.lstImportSurvey;
            var header = this.lstImportSurveyQstnHearderN;
            var IsMadatoryHearder = header.filter(x => x.mandatory == 'Yes')

            for (var i = 0; i <= IsMadatoryHearder.length - 1; i++) {
                for (var j = 0; j <= list.length - 1; j++) {
                    var Ans = list[j][IsMadatoryHearder[i].value];
                    if (Ans == "") {
                        this.IsvalidexcelData = false;
                    }
                }
            }
            var argument = args;
            if (argument[3].mandatory == 'Yes') {
                if (argument[0] == "") {
                    this.IsvalidexcelData = false;
                    argument[0] = "Invalid Input";
                    return 'Required field'

                }
            }
            var tempData = header.filter(x => x.QuestionCondition != null && x.QuestionCondition.length > 0)
            if (tempData.length > 0) {
                for (var hCnt = 0; hCnt <= tempData.length - 1; hCnt++) {
                    if (tempData[hCnt].QuestionCondition != null) {
                        for (var lCnt = 0; lCnt <= list.length - 1; lCnt++) {
                            var Answer = list[lCnt][tempData[hCnt].value];
                            var phoneNoValidation = tempData[hCnt].QuestionCondition;
                            for (var j = 0; j < phoneNoValidation.length; j++) {
                                if (phoneNoValidation[j].ValidationCondition == "Phone Number" && Answer != "") {
                                    var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                    if (!Answer.match(phoneNum)) {
                                        this.IsvalidexcelData = false;
                                    }
                                }
                                if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0] != "" && Answer != "") {
                                    var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                    if (!Answer.match(email)) {
                                        this.IsvalidexcelData = false;

                                    }
                                }
                                if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && Answer != "") {
                                    if (objUtils.common.formatExcelDate(Answer) < objUtils.common.formatExcelDate(today)) {
                                        this.IsvalidexcelData = false;

                                    }
                                }
                                if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && Answer != "") {
                                    if (objUtils.common.formatExcelDate(Answer) > objUtils.common.formatExcelDate(today)) {
                                        this.IsvalidexcelData = false;

                                    }
                                }
                                if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && Answer != "") {
                                    if (!(parseInt(Answer) > parseInt(phoneNoValidation[j].MinValue) && parseInt(Answer) < parseInt(phoneNoValidation[j].MaxValue))) {
                                        this.IsvalidexcelData = false;
                                    }
                                }
                                if (phoneNoValidation[j].ValidationCondition == "Aadhaar Number" && Answer != "") {
                                    var aadharNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                                    if (Answer != null && Answer != undefined && !Answer.match(aadharNum)) {
                                        this.IsvalidexcelData = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var tempIndDataData = this.lstImportSurveyQstnHearderN.filter(x => x.value == argument[3].name)
            if (tempIndDataData.length > 0) {
                if (tempIndDataData[0].QuestionCondition != null) {
                    var phoneNoValidation = tempIndDataData[0].QuestionCondition;
                    for (var j = 0; j < phoneNoValidation.length; j++) {
                        if (phoneNoValidation[j].ValidationCondition == "Phone Number" && argument[0] != "") {
                            var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                            if (!argument[0].match(phoneNum)) {

                                return 'Enter a valid Phone Number'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0] != "") {
                            var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                            if (!argument[0].match(email)) {

                                return 'Enter a valid Email'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && argument[0] != "") {
                            if (objUtils.common.formatExcelDate(argument[0]) < objUtils.common.formatExcelDate(today)) {

                                return 'Should be greater than Current date'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && argument[0] != "") {
                            if (objUtils.common.formatExcelDate(argument[0]) > objUtils.common.formatExcelDate(today)) {

                                return 'Should be less than Current date'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && argument[0] != "") {
                            if (!(parseInt(argument[0]) > parseInt(phoneNoValidation[j].MinValue) && parseInt(argument[0]) < parseInt(phoneNoValidation[j].MaxValue))) {

                                return "Value should be between " + phoneNoValidation[j].MinValue + " and " + phoneNoValidation[j].MaxValue;

                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Aadhaar Number" && argument[0] != "") {
                            var aadharNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;
                            if (argument[0] != null && argument[0] != undefined && !argument[0].match(aadharNum)) {
                                return 'Enter a valid Aadhaar Number'
                            }
                        }
                    }
                }

            }
            return ''
        },

        // Beneficiry excel data validations
        onBeforeMandatoryChange(...args) {
            this.IsvalidexcelData = true;
            var today = new Date().toLocaleString();
            var list = this.lstImportBeneficiary;
            var header = this.lstImportBeneficiaryHearder;
            var IsMadatoryHearder = header.filter(x => x.mandatory == 'Yes')

            for (var i = 0; i < IsMadatoryHearder.length - 1; i++) {
                for (var j = 0; j <= list.length - 1; j++) {
                    var Ans = list[j][IsMadatoryHearder[i].value];
                    if (Ans == "") {
                        this.IsvalidexcelData = false;
                    }
                }
            }
            var argument = args;
            if (argument[3].mandatory == 'Yes') {
                if (argument[0] == "") {
                    this.IsvalidexcelData = false;
                    return 'Required field'

                }
            }
            // for checking Question validations
            var tempData = this.lstImportBeneficiaryHearder.filter(x => x.value == argument[3].name)
            if (tempData.length > 0) {
                if (tempData[0].QuestionCondition != null) {
                    var phoneNoValidation = tempData[0].QuestionCondition;
                    for (var j = 0; j < phoneNoValidation.length; j++) {
                        if (phoneNoValidation[j].ValidationCondition == "Phone Number" && argument[0] != "") {
                            var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                            if (!argument[0].match(phoneNum)) {
                                this.IsvalidexcelData = false;
                                return 'Enter a valid Phone Number'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0] != "") {
                            var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                            if (!argument[0].match(email)) {
                                this.IsvalidexcelData = false;
                                return 'Enter a valid Email'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && argument[0] != "") {
                            if (objUtils.common.formatExcelDate(argument[0]) < objUtils.common.formatExcelDate(today)) {
                                this.IsvalidexcelData = false;
                                return 'Should be greater than Current date'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && argument[0] != "") {
                            if (objUtils.common.formatExcelDate(argument[0]) > objUtils.common.formatExcelDate(today)) {
                                this.IsvalidexcelData = false;
                                return 'Should be less than Current date'
                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && argument[0] != "") {
                            if (!(parseInt(argument[0]) > parseInt(phoneNoValidation[j].MinValue) && parseInt(argument[0]) < parseInt(phoneNoValidation[j].MaxValue))) {
                                this.IsvalidexcelData = false;
                                return "Value should be between " + phoneNoValidation[j].MinValue + " and " + phoneNoValidation[j].MaxValue;

                            }
                        }
                        if (phoneNoValidation[j].ValidationCondition == "Aadhaar Number" && argument[0] != "") {
                            var aadharNum = /^[2-9]{1}[0-9]{3}[0-9]{4}[0-9]{4}$/;;
                            if (argument[0] != null && argument[0] != undefined && !argument[0].match(aadharNum)) {
                                this.IsvalidexcelData = false;
                                return 'Enter a valid Aadhar Number'
                            }
                        }
                    }
                }
            }
        },

        // Function Used to check for invalid input in Import Survey
        async ValidateImportSurveyData() {
            var newList = this.lstImportSurvey.filter(element => {
                for (var property in element) {
                    if (element.hasOwnProperty(property)) {
                        if (element[property] == "Invalid Input") {
                            return true;
                        }
                    }
                }

            });
            if (newList.length > 0) {
                return false;
            }
            else {
                return true;
            }
        },
        async fnAddAnyTimeNewSubmission(item, index) {

            if (typeof item != "undefined") {

                //var data = this.TaskCardData.filter(x => x.TaskType == item.TaskType && x.TypeID == item.TypeID && x.Command == "ADD")[0].TaskLog;
                var AnyTimeTempData = await dbImworks.TbAnyTimeTempData.toArray()
                var data = AnyTimeTempData.filter(x => x.Type == item.TaskType && x.TypeID == item.TypeID)[0].TaskData.filter(y => y.CurrentOrOverdue != "Review" && y.Status == "To Data Collect");
                if (data.length > 0) {
                    var today = new Date().toLocaleString("en-US");
                    data[0].CreatedDate = today;
                    await dbImworks.TbAnyTimeData.put({
                        TaskData: data[0]
                    });
                    var lastClientSyncID = await dbImworks.TbAnyTimeData.orderBy('pkId').last();
                    var pkId = lastClientSyncID.pkId;
                    this.lstAnyTimeNewSubmission.push({ pkId: pkId, TaskData: data[0] });

                    this.lstAnyTimeNewSubmission.sort(function (a, b) {
                        return parseInt(b.pkId) - parseInt(a.pkId);
                    });
                    //Open Data collection panel
                    await this.GetSurveyTaskDetails(data[0], pkId, index);
                }

            }
        },

        // Non Beneficiary Linked Survey Sample excel file
        async DownloadSampleNonBeneficiaryLinkedSurveyFile() {
            var ColumnHeader = "";
            var ColumnData1 = [];
            var ColumnData = [];
            var StrResponse = "Survey Response here";
            for (var i = 0; i < this.lstImportSurveyQstnHearder.length; i++) {

                if (this.lstImportSurveyQstnHearder[i].text.includes(',')) {
                    ColumnHeader += ColumnHeader == "" ? "\"" + this.lstImportSurveyQstnHearder[i].text + "\"" : "," + "\"" + this.lstImportSurveyQstnHearder[i].text + "\"";
                }
                else {
                    ColumnHeader += ColumnHeader == "" ? this.lstImportSurveyQstnHearder[i].text : "," + this.lstImportSurveyQstnHearder[i].text;
                }
                //ColumnData1.push("Survey Response here");
                if (this.lstImportSurveyQstnHearder[i].type == "date") {
                    ColumnData1.push(StrResponse + " *Please enter date in mm-dd-yyyy format.");
                } else {
                    ColumnData1.push("Survey Response here");
                }

            }

            ColumnHeader = ColumnHeader + "\n";
            ColumnData.push(ColumnData1);
            ColumnData1 = [];

            ColumnData.forEach(function (row) {
                ColumnHeader += row.join(',');
                ColumnHeader += "\n";
            });

            // ColumnHeader['Beneficiary'].hidden = true;
            var hiddenElement = document.createElement('a');
            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(ColumnHeader);
            hiddenElement.target = '_blank';
            hiddenElement.download = 'sample_surveyNonBeneficiaryQuestionnaire_' + Math.round(Math.random() * 1000) + '.csv';
            hiddenElement.click();
        },

        async fnBackspace() {
            //beneficiary linked survey
            if (this.BenefLinkedSurveySteps == 2) {
                //this.BenefLinkedSurveySteps = 1;
            }
            else if (this.BenefLinkedSurveySteps == 4 || this.BenefLinkedSurveySteps == 7) {
                this.BenefLinkedSurveySteps = 2;
            } else if (this.BenefLinkedSurveySteps == 5) {
                this.BenefLinkedSurveySteps = 4;
            } else if (this.BenefLinkedSurveySteps == 3) {
                this.BenefLinkedSurveySteps = 2;
                this.scrollToNavbarTop();
            }
            //non_beneficiary linked survey
            if (this.NonBenefLinkedSurveySteps == 1) {
                //do nothing
            }
            else if (this.NonBenefLinkedSurveySteps == 2 || this.NonBenefLinkedSurveySteps == 3) {
                this.NonBenefLinkedSurveySteps = 1;
                await this.fnPreSavedNonBenefQstnAns();
                // Clear Multi Choice Answer when previous button click // the answer will update when we click Next button
                await this.fnClearMultiChoiceAns_When_Prev_Button_Click();
            }
            this.scrollToNavbarTop();

        },

        async fnClearMultiChoiceAns_When_Prev_Button_Click() {
            if (this.lstSectionsQstns.length > 0) {
                for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                    var MultiChoiceAns = "";
                    if (this.lstSectionsQstns[i].IslogicSection == 0) { // if Its not a Logic Section
                        MultiChoiceAns = this.lstSectionsQstns[i].Questions.filter(x => x.QuestionType == "Multi-Choice");
                    }
                    else if (this.lstSectionsQstns[i].IslogicSection == 1) {
                        MultiChoiceAns = this.RepeatOrSkipSection.filter(x => x.QuestionType == "Multi-Choice");
                    }

                    for (var j = 0; j <= MultiChoiceAns.length - 1; j++) {
                        MultiChoiceAns[j].Answer = "";
                    }
                }
            }
        },
        async onFocusCheckQstnValidation(QItem) {
            var valConditions = QItem.QuestionCondition;
            this.qType = "";
            this.QstType = "";
            this.txtLength = 0;
            this.minVal = 0;
            this.maxVal = 9999999999;
            this.qRules = [];
            if (valConditions != null) {
                this.IsQstnValidation = true;
                // ValidationCondition
                for (var i = 0; i < valConditions.length; i++) {
                    if (valConditions[i].ValidationCondition == 'Phone Number') {
                        QItem.QstType = "PhoneNumber";
                        QItem.txtLength = 10;
                        QItem.minVal = 0;
                        QItem.maxVal = 9999999999;
                        QItem.qRules = this.phoneNoRules;
                        // QItem.ErrorText ="";
                        QItem.IsValidationError = false;
                    }

                    if (valConditions[i].ValidationCondition == 'Aadhaar Number') {
                        QItem.QstType = "AadhaarNumber";
                        QItem.txtLength = 12;
                        QItem.minVal = 0;
                        QItem.maxVal = 9999999999;
                        QItem.qRules = this.aadharnumberNoRules;
                        // QItem.ErrorText ="";
                        QItem.IsValidationError = false;
                    }
                }
            }
        },
        fnDownloadDoc(Pos) {
            //var tCol = this.ApprovalTableHeader;
            //var tRow = this.ApprovalData;
            var IsSurveyQuestion = true;
            if (this.IsBeneficiaryLinkedSurvey == true) {
                var SelectedCol = this.ApprovalTableHeader[Pos.colPos];
                // check the question Name is in SurveyQuestion List
                var QItem = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.QuestionID == SelectedCol.QuestionID);

                // if not found in Survey Question list try to find in Beneficiary List
                if (QItem.length == 0) {
                    var BenefItem = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns.filter(x => x.QuestionID == SelectedCol.QuestionID);
                    QItem = BenefItem;
                    IsSurveyQuestion = false;
                }

                if (QItem.length > 0) {
                    if (QItem[0].QuestionType == "File Upload" || QItem[0].QuestionType == "Signature Capture" || QItem[0].QuestionType == "Photo Capture" || QItem[0].QuestionType == "Audio Capture") {

                        var Ans = this.ApprovalData[Pos.rowPos][SelectedCol.text]
                        if (IsSurveyQuestion == true) {
                            this.GotoNewTab_Survey(Ans);
                        }
                        else {
                            this.GotoNewTab_Beneficiary(Ans);
                        }

                    }
                    else if (QItem[0].QuestionType == "GPS Capture") {
                        var Ans = this.ApprovalData[Pos.rowPos][SelectedCol.text]
                        var url = "https://www.google.com.sa/maps/search/" + Ans + "?hl=en";

                        window.open(url, '_blank');

                    }
                }

            }
            else {
                var SelectedCol = this.NonBenfApprovalTableHeader[Pos.colPos]
                var QItem = this.lstSelectedTaskData.TaskData.SurveyQstnList.filter(x => x.QuestionName == SelectedCol.text);
                if (QItem.length > 0) {
                    if (QItem[0].QuestionType == "File Upload" || QItem[0].QuestionType == "Signature Capture" || QItem[0].QuestionType == "Photo Capture" || QItem[0].QuestionType == "Audio Capture") {

                        var Ans = this.NonBenfApprovalData[Pos.rowPos][SelectedCol.text]
                        this.GotoNewTab_Survey(Ans);
                    }
                    else if (QItem[0].QuestionType == "GPS Capture") {
                        var Ans = this.NonBenfApprovalData[Pos.rowPos][SelectedCol.text]
                        var url = "https://www.google.com.sa/maps/search/" + Ans + "?hl=en";

                        window.open(url, '_blank');

                    }
                }
            }
        },
        GotoNewTab_Survey(docName) {
            var docUrl = this.fileUrl + docName;
            //let route = this.$router.resolve({ path: docUrl });
            window.open(docUrl);
        },

        GotoNewTab_Beneficiary(docName) {
            var docUrl = this.BeneffileUrl + docName;
            //let route = this.$router.resolve({ path: docUrl });
            window.open(docUrl);
        },
        async GetMyTaskActivityLog(syncTaskID) {
            this.PreLoader = true;
            var data = new FormData();
            data.append("syncTaskID", syncTaskID);
            var result = await objMyTask.GetMyTaskActivityLog(data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else
                if (result.data) {
                    this.lstLogFrame = result.data;
                    this.PreLoader = false;
                }
        },
        updateSurveyApproveTask() {
            this.surveyApproveBenfLinkedDialog = false;

        },
        FormatDateInWeekName(Date) {
            return objUtils.common.formatDateInWeekName(Date)
        },
        FormatDateWithTime(Date) {
            return objUtils.common.formatDateWithTime(Date)
        },
        FormatDateTodayDate(Date) {
            return objUtils.common.formatDateTodayDate(Date)
        },
        async GetTaskDataList(surveyId, synctaskId, TaskData, responsesyncID, beneficiaryTypeId) {
            var IsCascadingInclude = false;
            var objectSurvey = new Object();
            if (TaskData != null && TaskData != undefined) {
                objectSurvey.BeneficiaryType = TaskData.BeneficiaryType;
                objectSurvey.BeneficiaryTypeID = TaskData.BeneficiaryTypeID;
                objectSurvey.CreatedDate = TaskData.CreatedDate;
                objectSurvey.Description = TaskData.Description;
                objectSurvey.EndDate = TaskData.EndDate;
                objectSurvey.Periodicity = TaskData.Periodicity;
                objectSurvey.ProjectId = TaskData.ProjectId;
                objectSurvey.ProjectName = TaskData.ProjectName;
                objectSurvey.StartDate = TaskData.StartDate;
                objectSurvey.SurveyID = TaskData.SurveyID;
                objectSurvey.SurveyName = TaskData.SurveyName;
                objectSurvey.VarResponseSynID = responsesyncID;
            }
            var surveySectionList = await dbImworks.MyTaskSurveySection.where({ SurveyID: surveyId }).toArray();
            var surveySectionLogicList = await dbImworks.MyTaskSurveySectionLogic.where({ SurveyID: surveyId }).toArray();
            var lstTaskMasterList = await dbImworks.MyTaskMasterList.toArray();
            var lstBeneficiaryType = await dbImworks.TbMyTaskProjectBeneficiaryType.where({ SurveyID: surveyId }).toArray();
            var lstMyTaskProjectSurveyResponses = await dbImworks.MyTaskProjectSurveyResponses.where({ SurveyID: surveyId, SyncTaskID: responsesyncID }).toArray();

            if (beneficiaryTypeId == null) {
                beneficiaryTypeId = undefined;
            }

            var arrSurveySectionLogic = new Array();
            objectSurvey.SectionList = new Array();

            var lstBeneficiaryTypeQuestion = [];

            var lstBeneficiaryRegistrationDetails = [];
            var lstProjectBeneficiary = [];

            if (beneficiaryTypeId == null || beneficiaryTypeId == undefined) {
                lstProjectBeneficiary = null;
                lstBeneficiaryTypeQuestion = null;
                lstBeneficiaryRegistrationDetails = null;
            } else {
                lstBeneficiaryTypeQuestion = await dbImworks.MyTaskBeneficiaryTypeQuestion.where({ BeneficiaryTypeID: beneficiaryTypeId }).toArray();
                lstBeneficiaryRegistrationDetails = await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.where({ BeneficiaryTypeID: beneficiaryTypeId }).toArray();
                lstProjectBeneficiary = await dbImworks.TbMyTaskProjectBeneficiary.where({ BeneficiaryTypeID: beneficiaryTypeId }).toArray();
            }

            objectSurvey.BeneficiaryList = new Array();

            objectSurvey.BeneficiaryDetails = new Array();
            objectSurvey.BeneficiarySubmittedList = new Array();
            if (lstProjectBeneficiary != null && lstProjectBeneficiary.length > 0) {
                objectSurvey.BeneficiaryList = lstProjectBeneficiary;
                var lstQuestion = await dbImworks.MyTaskBeneficiaryTypeQuestion.where({ BeneficiaryTypeID: beneficiaryTypeId }).toArray();

                for (var a = 0; a < lstProjectBeneficiary.length; a++) {
                    var lstdetails = lstBeneficiaryRegistrationDetails.filter(x => x.BeneficiaryID == lstProjectBeneficiary[a].BeneficiaryID);
                    if (lstdetails != null && lstdetails.length > 0) {
                        for (var b = 0; b < lstdetails.length; b++) {
                            var qst = lstQuestion.filter(x => x.QuestionID == lstdetails[b].QuestionID);
                            var objectreg = new Object();
                            objectreg.Answer = lstdetails[b].Answer;
                            objectreg['CreatedDate'] = lstProjectBeneficiary[a].CreatedDate;
                            objectreg.TokenValue = lstProjectBeneficiary[a].TokenValue;
                            if (qst != null && qst.length > 0) {
                                objectreg.QuestionName = qst[0].QuestionName;
                            } else {
                                objectreg.QuestionName = "";
                            }

                            objectreg.BeneficiaryID = lstdetails[b].BeneficiaryID;
                            objectSurvey.BeneficiaryDetails.push(objectreg);
                            objectSurvey.BeneficiaryDetails.sort(function (a, b) {
                                return new Date(b.CreatedDate) - new Date(a.CreatedDate);
                            });
                        }
                    }
                    var lstSurveyResponse = lstMyTaskProjectSurveyResponses.filter(x => x.ResponseID == lstProjectBeneficiary[a].BeneficiaryID);
                    if (lstSurveyResponse != null && lstSurveyResponse.length > 0) {
                        for (var l = 0; l < lstSurveyResponse.length; l++) {
                            var objectres = new Object();
                            objectres.Answer = lstSurveyResponse[l].Answer;
                            objectres.Answer1 = lstSurveyResponse[l].Answer;
                            objectres.SurveyID = lstSurveyResponse[l].SurveyID;
                            objectres.SectionID = lstSurveyResponse[l].SectionID;
                            objectres.QuestionID = lstSurveyResponse[l].QuestionID;
                            objectres.CreatedDate = lstProjectBeneficiary[a].CreatedDate;
                            objectres.BeneficiaryID = lstProjectBeneficiary[a].BeneficiaryID;
                            objectres.BeneficiaryName = lstProjectBeneficiary[a].BeneficiaryName;
                            objectres.BeneficiaryTypeID = lstProjectBeneficiary[a].BeneficiaryTypeID;
                            objectSurvey.BeneficiarySubmittedList.push(objectres);
                        }
                    }
                }
            } else {
                objectSurvey.BeneficiaryList = null;
                objectSurvey.BeneficiaryDetails = null;
                objectSurvey.BeneficiarySubmittedList = null;
            }

            if (objectSurvey.BeneficiarySubmittedList != null && objectSurvey.BeneficiarySubmittedList.length == 0) {
                objectSurvey.BeneficiarySubmittedList = null;
            }

            if (objectSurvey.BeneficiaryDetails != null && objectSurvey.BeneficiaryDetails.length == 0) {
                objectSurvey.BeneficiaryDetails = null;
            }


            objectSurvey.SurveyQstnList = new Array();

            var lstQuestionType = await dbImworks.MyTaskQuestionType.toArray();

            var lstSurveySectionLogic = await dbImworks.MyTaskSurveySectionLogic.where({ SurveyID: surveyId }).toArray();
            var lstCondition = await dbImworks.TbMyTaskProjectLogicConditions.toArray();

            if (lstSurveySectionLogic != null && lstSurveySectionLogic.length > 0) {
                for (var i = 0; i < lstSurveySectionLogic.length; i++) {

                    var object = new Object();
                    object.Answer = lstSurveySectionLogic[i].Answer;
                    object.LogicType = lstSurveySectionLogic[i].LogicType;
                    object.QuestionID = lstSurveySectionLogic[i].QuestionID;
                    object.ConditionID = lstSurveySectionLogic[i].ConditionID;
                    object.SkipSectionID = lstSurveySectionLogic[i].SectionID;

                    switch (lstSurveySectionLogic[i].LogicOperator) {
                        case 0:
                            object.LogicOperator = "AND";
                            break;
                        case 1:
                            object.LogicOperator = "OR";
                            break;
                        default:
                            object.LogicOperator = "";
                    }

                    if (lstSurveySectionLogic[i].ConditionID > 0) {
                        var itm = lstCondition.filter(x => x.ID == lstSurveySectionLogic[i].ConditionID);
                        if (itm != null && itm.length > 0) {
                            object.ConditionValue = itm[0].ConditionValue;
                        } else {
                            object.ConditionValue = "";
                        }
                    } else {
                        object.ConditionValue = lstSurveySectionLogic[i].ConditionValue;
                    }

                    arrSurveySectionLogic.push(object);
                }
            } else {
                arrSurveySectionLogic = null;
            }

            if (surveySectionList != null && surveySectionList.length > 0) {
                for (var i = 0; i < surveySectionList.length; i++) {
                    var objectSection = new Object();
                    objectSection.SurveyID = surveySectionList[i].SurveyID;
                    objectSection.SectionID = surveySectionList[i].SectionID;
                    objectSection.SectionName = surveySectionList[i].SectionName;
                    objectSection.SectionDescription = surveySectionList[i].SectionDescription;
                    objectSection.SectionOrder = surveySectionList[i].SectionOrder;

                    if (surveySectionLogicList != null && surveySectionLogicList.length > 0) {
                        //var item = surveySectionLogicList.filter(word => word.SectionID = object.SectionID);
                        var item = surveySectionLogicList.filter(word => word.SectionID == objectSection.SectionID)

                        if (item != null && item.length > 0) {
                            objectSection.IslogicSection = 1;
                        } else {
                            objectSection.IslogicSection = 0;
                        }
                    } else {
                        objectSection.IslogicSection = 0;
                    }

                    objectSurvey.SectionList.push(objectSection);

                    var lstTaskSurveyQuestion = await dbImworks.MyTaskSurveyQuestion.where({ SurveyID: surveyId, SectionID: objectSection.SectionID }).toArray();

                    if (lstTaskSurveyQuestion != null && lstTaskSurveyQuestion.length > 0) {
                        for (var j = 0; j < lstTaskSurveyQuestion.length; j++) {
                            var objectQuestion = new Object();
                            var question = lstQuestionType.filter(x => x.ID == lstTaskSurveyQuestion[j].QuestionTypeID);
                            var lstQuestionOptions = await dbImworks.MyTaskProjectSurveyQSTOption.where({ QuestionID: lstTaskSurveyQuestion[j].QuestionID }).toArray();
                            var lstQuestionCondition = await dbImworks.MyTaskProjectSurveyQSTCondition.where({ QuestionID: lstTaskSurveyQuestion[j].QuestionID }).toArray();

                            var lstSurveyResponse = null;
                            if (responsesyncID > 0) {
                                lstSurveyResponse = lstMyTaskProjectSurveyResponses.filter(x => x.SyncTaskID == responsesyncID && x.SurveyID == surveyId && x.QuestionID == lstTaskSurveyQuestion[j].QuestionID);
                            }
                            objectQuestion.UOM = lstTaskSurveyQuestion[j].UOM;
                            objectQuestion.RegEx = lstTaskSurveyQuestion[j].RegEx;

                            if (lstSurveyResponse != null && lstSurveyResponse.length > 0) {
                                objectQuestion.Answer = (lstTaskSurveyQuestion[j].DefaultValue == null || lstTaskSurveyQuestion[j].DefaultValue == 'null') ? lstSurveyResponse[0].Answer : lstTaskSurveyQuestion[j].DefaultValue;
                                objectQuestion.Answer1 = lstSurveyResponse[0].Answer;
                            } else {
                                objectQuestion.Answer = (lstTaskSurveyQuestion[j].DefaultValue == null || lstTaskSurveyQuestion[j].DefaultValue == 'null') ? "" : lstTaskSurveyQuestion[j].DefaultValue;
                                objectQuestion.Answer1 = "";
                            }

                            objectQuestion.FieldVar = lstTaskSurveyQuestion[j].FieldVar;
                            objectQuestion.IsUnique = (lstTaskSurveyQuestion[j].IsUnique == true ? 1 : 0);
                            objectQuestion.IsUpload = 0;
                            objectQuestion.SurveyID = lstTaskSurveyQuestion[j].SurveyID;
                            objectQuestion.ErrorText = lstTaskSurveyQuestion[j].ErrorText;
                            objectQuestion.SectionID = lstTaskSurveyQuestion[j].SectionID;
                            objectQuestion.QuestionID = lstTaskSurveyQuestion[j].QuestionID;

                            if (responsesyncID != null && responsesyncID != undefined && responsesyncID > 0) {
                                objectQuestion.SyncTaskID = responsesyncID;
                            } else {
                                objectQuestion.SyncTaskID = null;
                            }
                            objectQuestion.IsMandatory = (lstTaskSurveyQuestion[j].IsMandatory == true ? 1 : 0);
                            objectQuestion.SectionName = objectSection.SectionName;
                            objectQuestion.SectionDescription = objectSection.SectionDescription;
                            objectQuestion.DefaultValue = lstTaskSurveyQuestion[j].DefaultValue;
                            objectQuestion.IsSearchable = (lstTaskSurveyQuestion[j].IsSearchable == true ? 1 : 0);
                            objectQuestion.QuestionHint = lstTaskSurveyQuestion[j].QuestionHint;
                            objectQuestion.QuestionName = lstTaskSurveyQuestion[j].QuestionName;
                            objectQuestion.QuestionType = (question != null && question != undefined) ? question[0].Type : "";

                            objectQuestion.SectionLogic = new Array();

                            if (arrSurveySectionLogic != null && arrSurveySectionLogic.length > 0) {
                                objectQuestion.SectionLogic = arrSurveySectionLogic;
                            } else {
                                objectQuestion.SectionLogic = null;
                            }

                            objectQuestion.SectionOrder = objectSection.SectionOrder;
                            objectQuestion.QuestionOrder = lstTaskSurveyQuestion[j].QuestionOrder;
                            objectQuestion.QuestionTypeID = lstTaskSurveyQuestion[j].QuestionTypeID;
                            objectQuestion.IsRepeatSection = (lstTaskSurveyQuestion[j].IsRepeatSection == true ? true : false);
                            //if(lstQuestionOptions != null && lstQuestionOptions.length > 0) {  // sorting in alphabeticl ordering
                            //                           lstQuestionOptions.sort((a, b) => a.OptionValue.normalize().localeCompare(b.OptionValue.normalize()));
                            //                       }
                            objectQuestion.QuestionOptions = lstQuestionOptions;
                            objectQuestion.FormulaOperation = lstTaskSurveyQuestion[j].FormulaOperation;
                            objectQuestion.QuestionCategory = (question != null && question != undefined) ? question[0].Category : 0;
                            objectQuestion.FormulaExpression = lstTaskSurveyQuestion[j].FormulaExpression;
                            objectQuestion.IsFormulaQuestion = (lstTaskSurveyQuestion[j].IsFormulaQuestion == true ? 1 : 0);
                            objectQuestion.IsConditionQuestion = (lstTaskSurveyQuestion[j].IsConditionQuestion == true ? 1 : 0);
                            objectQuestion.IsValidationQuestion = (lstTaskSurveyQuestion[j].IsValidationQuestion == true ? 1 : 0);
                            objectQuestion.FormulaFieldVariables = lstTaskSurveyQuestion[j].FormulaFieldVariables; //IsOtherOptionSelected
                            objectQuestion.IsOtherOptionSelected = (lstTaskSurveyQuestion[j].IsOtherOptionSelected == true ? 1 : 0);
                            objectQuestion.QuestionCondition = new Array();
                            if (lstQuestionCondition != null && lstQuestionCondition.length > 0) {
                                for (var k = 0; k < lstQuestionCondition.length; k++) {
                                    var casObject = new Object();
                                    casObject.MaxValue = lstQuestionCondition[k].MaxValue;
                                    casObject.MinValue = lstQuestionCondition[k].MinValue;
                                    casObject.QuestionID = lstQuestionCondition[k].QuestionID;
                                    casObject.LogicOperator = lstQuestionCondition[k].LogicOperator;
                                    casObject.ConditionValue = lstQuestionCondition[k].ConditionValue;
                                    casObject.ValidationCondition = lstQuestionCondition[k].ValidationCondition;
                                    objectQuestion.QuestionCondition.push(casObject);
                                }
                            } else {
                                objectQuestion.QuestionCondition = null;
                            }

                            if (lstTaskSurveyQuestion[j].QuestionTypeID == 8) {
                                IsCascadingInclude = true;

                                objectQuestion.CascadingQuestionOptions = new Array();
                                if (lstQuestionOptions != null && lstQuestionOptions.length > 0) {
                                    for (var k = 0; k < lstQuestionOptions.length; k++) {
                                        var lstitem = await dbImworks.MyTaskSurveyCascadingQuestionOptions.where({ OptionValue: lstQuestionOptions[k].OptionValue }).toArray();

                                        if (lstitem != null && lstitem.length > 0) {
                                            var casObject = new Object();
                                            casObject.OptionID = lstQuestionOptions[k].OptionID;
                                            casObject.QuestionID = lstQuestionOptions[k].QuestionID;
                                            casObject.OptionValue = lstQuestionOptions[k].OptionValue;
                                            casObject.OptionGroupID = lstitem[0].OptionGroupID;
                                            casObject.HierarchyOrder = lstitem[0].HierarchyOrder;
                                            casObject.CascadingItems = "";
                                            objectQuestion.CascadingQuestionOptions.push(casObject);
                                        }
                                    }
                                } else {
                                    objectQuestion.CascadingQuestionOptions = null;
                                }
                            } else {
                                objectQuestion.CascadingQuestionOptions = null;
                            }

                            objectSurvey.SurveyQstnList.push(objectQuestion);
                        }
                    }
                }
            }

            objectSurvey.SurveyResponse = new Array();
            if (lstMyTaskProjectSurveyResponses != null && lstMyTaskProjectSurveyResponses.length > 0) {
                var lstMyTaskProjectSurveyResponses1 = lstMyTaskProjectSurveyResponses.filter(x => x.ResponseID == 0);

                for (var z = 0; z < lstMyTaskProjectSurveyResponses1.length; z++) {
                    var casObject = new Object();
                    casObject.Answer = lstMyTaskProjectSurveyResponses1[z].Answer;
                    casObject.Answer1 = lstMyTaskProjectSurveyResponses1[z].Answer;
                    casObject.SurveyID = lstMyTaskProjectSurveyResponses1[z].SurveyID;
                    casObject.SectionID = lstMyTaskProjectSurveyResponses1[z].SectionID;
                    casObject.QuestionID = lstMyTaskProjectSurveyResponses1[z].QuestionID;
                    objectSurvey.SurveyResponse.push(casObject);
                }
            } else {
                objectSurvey.SurveyResponse = null;
            }

            if (objectSurvey.SurveyResponse != null && objectSurvey.SurveyResponse.length == 0) {
                objectSurvey.SurveyResponse = null;
            }

            objectSurvey.CascadingList = new Array();

            var lstBeneficiaryQuestionOption = [];

            if (beneficiaryTypeId == null || beneficiaryTypeId == undefined) {
                lstBeneficiaryQuestionOption = null;
            } else {
                lstBeneficiaryQuestionOption = await dbImworks.MyTaskBeneficiaryTypeQuestionOption.where({ BeneficiaryTypeID: beneficiaryTypeId }).toArray();
            }
            objectSurvey.BeneficiaryRegistrationQstns = new Array();

            if (lstBeneficiaryTypeQuestion != null && lstBeneficiaryTypeQuestion.length > 0) {
                var lstBenAnswerType = await dbImworks.TbMyTaskBeneficiaryAnswerType.toArray();

                for (var t = 0; t < lstBeneficiaryTypeQuestion.length; t++) {
                    var lstOstOption = lstBeneficiaryQuestionOption.filter(x => x.QuestionID == lstBeneficiaryTypeQuestion[t].QuestionID);
                    var lstBenQuestionCondition = await dbImworks.MyTaskProjectBenSurveyQSTCondition.where({ QuestionID: lstBeneficiaryTypeQuestion[t].QuestionID }).toArray();
                    var qsttypefilter = lstBenAnswerType.filter(x => x.AnswerTypeID == lstBeneficiaryTypeQuestion[t].AnswerTypeID);

                    var objectBenQst = new Object();
                    objectBenQst.BeneficiaryTypeID = lstBeneficiaryTypeQuestion[t].BeneficiaryTypeID;
                    objectBenQst.QuestionID = lstBeneficiaryTypeQuestion[t].QuestionID;
                    objectBenQst.QuestionName = lstBeneficiaryTypeQuestion[t].QuestionName;
                    objectBenQst.QuestionTypeID = lstBeneficiaryTypeQuestion[t].AnswerTypeID;
                    objectBenQst.QuestionOrder = lstBeneficiaryTypeQuestion[t].QuestionOrder;

                    if (qsttypefilter != null && qsttypefilter.length > 0) {
                        objectBenQst.QuestionType = qsttypefilter[0].AnswerType;
                    } else {
                        objectBenQst.QuestionType = "";
                    }
                    objectBenQst.IsUnique = (lstBeneficiaryTypeQuestion[t].IsUnique == true ? 1 : 0);
                    objectBenQst.IsSearchable = (lstBeneficiaryTypeQuestion[t].IsSearchable == true ? 1 : 0);
                    objectBenQst.IsMandatoryResponse = (lstBeneficiaryTypeQuestion[t].IsMandatoryResponse == true ? 1 : 0);
                    objectBenQst.QuestionHint = lstBeneficiaryTypeQuestion[t].QuestionHint;
                    if (lstBeneficiaryTypeQuestion[t].RegexExpression == 'null') {
                        objectBenQst.RegexExpression = null;
                    } else {
                        objectBenQst.RegexExpression = lstBeneficiaryTypeQuestion[t].RegexExpression;
                    }
                    objectBenQst.IsValidationQuestion = (lstBeneficiaryTypeQuestion[t].IsValidationQuestion == true ? 1 : 0);

                    objectBenQst.BTQstnAnsOptionList = new Array();
                    if (lstOstOption != null && lstOstOption.length > 0) {
                        // lstOstOption.sort((a, b) => a.OptionValue.normalize().localeCompare(b.OptionValue.normalize())); // sorting in alphabeticl ordering
                        for (var qoption = 0; qoption < lstOstOption.length; qoption++) {
                            var objectOptionItem = new Object();
                            objectOptionItem.QuestionID = lstOstOption[qoption].QuestionID;
                            objectOptionItem.OptionID = lstOstOption[qoption].QstnAnsOptionID;
                            objectOptionItem.OptionValue = lstOstOption[qoption].OptionValue;
                            objectBenQst.BTQstnAnsOptionList.push(objectOptionItem);
                        }
                    } else {
                        objectBenQst.BTQstnAnsOptionList = null;
                    }

                    objectBenQst.BTQstnValidationList = new Array();

                    if (lstBenQuestionCondition != null && lstBenQuestionCondition.length > 0) {
                        for (var qCondition = 0; qCondition < lstBenQuestionCondition.length; qCondition++) {
                            var objectConditionItem = new Object();
                            objectConditionItem.QuestionID = lstBenQuestionCondition[qCondition].QuestionID;
                            objectConditionItem.ValidationCondition = lstBenQuestionCondition[qCondition].ValidationCondition;
                            objectConditionItem.MinValue = lstBenQuestionCondition[qCondition].MinValue;
                            objectConditionItem.MaxValue = lstBenQuestionCondition[qCondition].MaxValue;
                            objectConditionItem.ConditionValue = lstBenQuestionCondition[qCondition].ConditionValue;
                            objectConditionItem.LogicOperator = lstBenQuestionCondition[qCondition].LogicOperator;
                            objectBenQst.BTQstnValidationList.push(objectConditionItem);
                        }
                    } else {
                        objectBenQst.BTQstnValidationList = null;
                    }

                    objectBenQst.CascadingQuestionOptions = new Array();
                    if (lstBeneficiaryTypeQuestion[t].AnswerTypeID == 8) {
                        IsCascadingInclude = true;
                        if (lstBeneficiaryQuestionOption != null && lstBeneficiaryQuestionOption.length > 0) {
                            for (var itmcsc = 0; itmcsc < lstBeneficiaryQuestionOption.length; itmcsc++) {
                                var lstitem = await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.where({ OptionValue: lstBeneficiaryQuestionOption[itmcsc].OptionValue }).toArray();

                                if (lstitem != null && lstitem.length > 0) {
                                    var casObjectcss = new Object();
                                    casObjectcss.QuestionID = lstBeneficiaryQuestionOption[itmcsc].QuestionID;
                                    casObjectcss.OptionID = lstBeneficiaryQuestionOption[itmcsc].OptionID;
                                    casObjectcss.OptionValue = lstBeneficiaryQuestionOption[itmcsc].OptionValue;
                                    casObjectcss.OptionGroupID = lstitem[0].OptionGroupID;
                                    casObjectcss.HierarchyOrder = lstitem[0].HierarchyOrder;
                                    objectBenQst.CascadingQuestionOptions.push(casObjectcss);
                                }
                            }
                        } else {
                            objectBenQst.CascadingQuestionOptions = null;
                        }
                    } else {
                        objectBenQst.CascadingQuestionOptions = null;
                    }

                    objectBenQst.DateMenu = false;
                    objectBenQst.DateMenu1 = false;
                    objectBenQst.qRules = "";
                    objectBenQst.uploadedSign = "";
                    objectBenQst.img = "";
                    objectBenQst.uploadedImg = "";
                    objectBenQst.startCamera = false;
                    objectBenQst.camOn = false;
                    objectBenQst.signaturebutton = false;
                    objectBenQst.uploadedFile = "";
                    objectBenQst.Answer = "";
                    objectBenQst.IsUpload = 0;
                    objectSurvey.BeneficiaryRegistrationQstns.push(objectBenQst);
                }
            } else {
                objectSurvey.BeneficiaryRegistrationQstns = null;
            }

            if (lstTaskMasterList != null && lstTaskMasterList.length > 0 && IsCascadingInclude == true) {
                var objectSurveyCascading = new Object();

                var country = lstTaskMasterList.filter(x => x.ItemTable == "Country");
                var state = lstTaskMasterList.filter(x => x.ItemTable == "State");
                var district = lstTaskMasterList.filter(x => x.ItemTable == "District");
                var subDistrict = lstTaskMasterList.filter(x => x.ItemTable == "SubDistrict");

                objectSurveyCascading.Country = country;
                objectSurveyCascading.States = state;
                objectSurveyCascading.District = district;
                objectSurveyCascading.SubDistrict = subDistrict;

                objectSurvey.CascadingList.push(objectSurveyCascading);
            } else {
                objectSurvey.CascadingList = null;
            }

            //  console.log(JSON.stringify(objectSurvey));
            return objectSurvey;
        },
        async create_UUID() {
            var dt = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (dt + Math.random() * 16) % 16 | 0;
                dt = Math.floor(dt / 16);
                return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
            console.log(uuid);
            return uuid;
        },

        //MyTasks Download Summary Excel
        async MyTasksDownloadSummary(type, syncid) {
            this.PreLoader = true;
            var csv = "Submitted date & time,Submitted by,";
            var answerarray = [];
            var lstAnswers = [];
            var SurveyQuestions = [];
            var BenRegQuestions = [];

            console.log(syncid);
            var SyncTaskDetails = ""; //declared this variable for storing the task data
            var result = await tjsummary.FetchAllBeneSurveyQuestDetailAPI(syncid);
            console.log(result.data);
            if (result.status == "Unhandled Exception") {
                this.$router.push({ name: "error" });
            }
            else if (result.data) {
                SyncTaskDetails = result.data; //Now this is the parent array that consists of all task Questions and Answers (Responses)
            }

            // beneficiarylinked download summmary
            if (type == 'beneficiarylinked') {

                //csv += 'Beneficiary Name,';
                var csvcounter = 0;
                var csvexistingbencounter = 0;
                var maxSurveyQuestionsCount = 0;
                var maxBenQuestionsCount = 0;
                var BeneficiaryParentSurveyQtsAnsArray = [];
                var BenRegParentQtsAnsArray = [];
                maxSurveyQuestionsCount = SyncTaskDetails.ResponseDetails[0].SurveyQuestions.length; //finding max question count from the SurveyQuestions of different benef
                maxBenQuestionsCount = SyncTaskDetails.ResponseDetails[0].BeneficiaryQuestions.length;
                BeneficiaryParentSurveyQtsAnsArray = SyncTaskDetails.ResponseDetails[0].SurveyQuestions; //making max question array as parent array
                BenRegParentQtsAnsArray = SyncTaskDetails.ResponseDetails[0].BeneficiaryQuestions;

                for (var BenCnt = 0; BenCnt <= SyncTaskDetails.ResponseDetails.length - 1; BenCnt++) {

                    if (SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions.length > maxBenQuestionsCount) {
                        maxBenQuestionsCount = SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions.length; //finding max question count from the BenRegQuestions of different benef
                        BenRegParentQtsAnsArray = SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions; //making max question array as parent array
                    }

                }

                for (var BenCnt = 0; BenCnt <= SyncTaskDetails.ResponseDetails.length - 1; BenCnt++) {

                    if (SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions.length > maxSurveyQuestionsCount) {
                        maxSurveyQuestionsCount = SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions.length; //finding max question count from the SurveyQuestions of different benef
                        BeneficiaryParentSurveyQtsAnsArray = SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions; //making max question array as parent array
                    }

                }

                for (var i = 0; i < BenRegParentQtsAnsArray.length; i++) {
                    BenRegQuestions.push(BenRegParentQtsAnsArray[i].QuestionId);
                }

                for (var i = 0; i < BeneficiaryParentSurveyQtsAnsArray.length; i++) {
                    SurveyQuestions.push(BeneficiaryParentSurveyQtsAnsArray[i].QuestionId);
                }
                //console.log(maxSurveyQuestionsCount);
                //console.log(SurveyQuestions);
                //console.log(maxBenQuestionsCount);
                //console.log(BenRegQuestions);

                for (var BenCnt = 0; BenCnt <= SyncTaskDetails.ResponseDetails.length - 1; BenCnt++) {
                    lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.SubmittedDateTime));
                    lstAnswers.push(SyncTaskDetails.SubmittedBy);

                    //// Existing beneficiary Reg Question Answers
                    if (SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions.length > 0) {
                        if (csvcounter == 0) {
                            for (var i = 0; i < BenRegQuestions.length; i++) {
                                //beneficiary questions
                                csv += objUtils.common.enclosequotes(BenRegParentQtsAnsArray[i].QuestionName).toString() + ',';
                            }


                            csvexistingbencounter = csvexistingbencounter + 1;
                        }
                        for (var i = 0; i < BenRegQuestions.length; i++) {
                            //beneficiaries
                            var answerchoice = "";
                            for (var j = 0; j < SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions.length; j++) {
                                if (BenRegQuestions[i] == SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions[j].QuestionId) {
                                    if (SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions[j].Answer == undefined || SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions[j].Answer == "") {
                                        answerchoice += " ";
                                    }
                                    else {
                                        answerchoice += SyncTaskDetails.ResponseDetails[BenCnt].BeneficiaryQuestions[j].Answer;
                                    }
                                } else {
                                    continue;
                                }

                            }

                            lstAnswers.push(objUtils.common.enclosequotes(answerchoice));
                        }
                    }


                    // var surveyQuestions = "";
                    //Survey questions Answers
                    if (csvcounter == 0) { // Question headers are required only once.
                        for (var i = 0; i < maxSurveyQuestionsCount; i++) {
                            csv += objUtils.common.enclosequotes(BeneficiaryParentSurveyQtsAnsArray[i].QuestionName).toString() + ',';
                            // surveyQuestions += objUtils.common.enclosequotes(BeneficiaryParentSurveyQtsAnsArray[i].QuestionName).toString() + '|';
                        }

                        csvexistingbencounter = csvexistingbencounter + 1;
                    }

                    var SurSecAnswerChoice = "";
                    var QSTCount = 0;
                    for (var j = 0; j < maxSurveyQuestionsCount; j++) {

                        var questionName = objUtils.common.enclosequotes(BeneficiaryParentSurveyQtsAnsArray[j].QuestionName).toString();

                        if (objUtils.common.enclosequotes(SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[QSTCount].QuestionName) == questionName) {
                            if (SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[QSTCount].Answer == undefined || SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[QSTCount].Answer == "") {
                                SurSecAnswerChoice += " ";
                            } else {
                                SurSecAnswerChoice += SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[QSTCount].Answer;
                            }
                            QSTCount++;

                            lstAnswers.push(objUtils.common.enclosequotes(SurSecAnswerChoice));
                            SurSecAnswerChoice = "";
                        } else {
                            SurSecAnswerChoice += "";
                            lstAnswers.push(objUtils.common.enclosequotes(SurSecAnswerChoice));
                        }
                    }

                    //for (var j = 0; j < SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions.length; j++) {

                    //            if (SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[j].Answer == undefined || SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[j].Answer == "") {
                    //                SurSecAnswerChoice += " ";
                    //            } else {
                    //                SurSecAnswerChoice += SyncTaskDetails.ResponseDetails[BenCnt].SurveyQuestions[j].Answer;
                    //            }
                    //    lstAnswers.push(objUtils.common.enclosequotes(SurSecAnswerChoice));
                    //    SurSecAnswerChoice = "";
                    //    }


                    //
                    lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageName)); // Workflow Stage Name
                    lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageUser)); // Workflow stage User
                    lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStatus)); // Workflow Status
                    //
                    answerarray.push([lstAnswers.toString()]);
                    lstAnswers = [];
                    csvcounter = csvcounter + 1;

                }
            }

            if (type == 'nonbeneficiarylinked') {
                lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.SubmittedDateTime)); //SubmittedDateTime
                lstAnswers.push(SyncTaskDetails.SubmittedBy); //Submitted by
                var NonBenSurveyQtsAnsDetails = SyncTaskDetails.ResponseDetails[0].SurveyQuestions;
                //console.log(NonBenSurveyQtsAnsDetails);


                // Taking Question Name as Heading for excel
                var NoOfQuestions = SyncTaskDetails.ResponseDetails[0].SurveyQuestions.map(function (item) {
                    return item.QuestionId;
                });


                var lstQuestionID = SyncTaskDetails.ResponseDetails[0].SurveyQuestions.filter(x => x.QuestionId == NonBenSurveyQtsAnsDetails[0].QuestionId)

                // Now we will use set to remove duplicate value from array
                var setOfValue = new Set(NoOfQuestions);
                // Now we will convert set to array using spread operator

                var ResponseCount = lstQuestionID //[...setOfValue];

                var QuestionPerSub = [...setOfValue];

                // Getting Question Name as Header
                for (var nQnt = 0; nQnt <= NonBenSurveyQtsAnsDetails.length - 1; nQnt++) {
                    //var QID = QuestionPerSub[nQnt];
                    var QuestionName = NonBenSurveyQtsAnsDetails[nQnt].QuestionName;
                    csv += objUtils.common.enclosequotes(QuestionName).toString() + ',';
                }
                var tempQID = 0
                var answerchoice = "";
                var tempQCnt = NonBenSurveyQtsAnsDetails.length;//QuestionPerSub.length;
                for (var i = 0; i <= NonBenSurveyQtsAnsDetails.length - 1; i++) {
                    lstAnswers.push(objUtils.common.enclosequotes(NonBenSurveyQtsAnsDetails[i].Answer));
                    if (i == (tempQCnt - 1)) {
                        lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageName)); // Workflow Stage Name
                        lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageUser)); // Workflow stage User
                        lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStatus)); // Workflow Status
                        answerarray.push([lstAnswers.toString()]);
                        tempQCnt += QuestionPerSub.length;
                        lstAnswers = [];
                        lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.SubmittedDateTime)); //SubmittedDateTime
                        lstAnswers.push(SyncTaskDetails.SubmittedBy); //Submitted by
                    }
                }
                // old Code
                //for (var i = 0; i <= NonBenSurveyQtsAnsDetails.length - 1; i++)
                //{
                //    var answerchoice = "";
                //     answerchoice += NonBenSurveyQtsAnsDetails[i].Answer;
                //     lstAnswers.push(objUtils.common.enclosequotes(answerchoice));
                // }

                // old Codr ends here

                //
                //lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageName)); // Workflow Stage Name
                //lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStageUser)); // Workflow stage User
                //lstAnswers.push(objUtils.common.enclosequotes(SyncTaskDetails.WorkflowStatus)); // Workflow Status
                //
                //answerarray.push([lstAnswers.toString()]);

            }
            csv += 'Workflow Stage Name,Workflow stage User,Workflow Status,';
            csv += "\n";

            for (var i = 0; i < answerarray.length; i++) {
                csv += answerarray[i].join(",");
                csv += "\n";
            }

            var hiddenElement = document.createElement("a");
            hiddenElement.href = "data:text/csv;charset=utf-8," + encodeURI(csv);
            hiddenElement.target = "_blank";
            hiddenElement.download = "MyTasks_download_summary" + Math.round(Math.random() * 1000) + ".csv";
            hiddenElement.click();

            this.PreLoader = false;
        },

        async ShowOtherOption(item, optionValue, isSelected) {
            if (item.IsOtherOptionSelected == true && optionValue == "OTHERS") {
                if (isSelected == true) {
                    item.isOtherOptionEnabled = true;
                }
                else {
                    item.isOtherOptionEnabled = false;
                    item.OtherOptionAnswer = "";
                }
            }
        },

        async ShowOtherOptionSingleChoice(item, optionValue) {

            if (item.IsOtherOptionSelected == true && optionValue == "OTHERS") {
                item.isOtherOptionEnabled = true;
            }
            else {
                item.isOtherOptionEnabled = false;
                item.OtherOptionAnswer = "";
            }
        },

        fnBindStyle(item, style) {
            //(Qitem.CurrentOrOverdue=='Completed'|| Qitem.CurrentOrOverdue=='Review' || Qitem.CurrentOrOverdue=='Syncing' ? 'font-weight:400' : 'font-weight:600')
            var fontStyle1 = 'font-weight:600';
            var fontStyle2 = 'font-weight:400';
            var dotStyle1 = 'black_dot';
            var dotStyle2 = 'blue_dot';
            if (item.AssignedToIds != null) {
                if (item.AssignedToIds.length > 0) {
                    var lstAssignedToUserIds = item.AssignedToIds.split(',').map(element => { return Number(element); });
                    if (lstAssignedToUserIds.includes(this.LoggedUserID)) {
                        // return 'font-weight:600'
                        return style == 'font' ? fontStyle1 : dotStyle2;
                    }
                    else {
                        //return 'font-weight:400'
                        return style == 'font' ? fontStyle2 : dotStyle1;
                    }
                }
            }
            else {
                //return 'font-weight:400'
                return style == 'font' ? fontStyle2 : dotStyle1;
            }
        },
        //===========Question Calculation ====================//
        async CalculateText(item, QID, benefIndx) {

            var resultVal = 0;
            var fieldVar = '';
            var fieldVar1 = '';
            var lstfieldVar = [];
            var formulaExp = "";
            if (typeof item != 'undefined') {
                if (item.IsFormulaQuestion) {
                    var FormulaQuestion = item;

                    formulaExp = FormulaQuestion.FormulaExpression;

                    if (FormulaQuestion.FormulaFieldVariables != "") {
                        var arr = FormulaQuestion.FormulaFieldVariables.split(",");
                        if (arr.length == 1) {
                            for (var i = 0; i < lstfieldVar.length; i++)
                                fieldVar = fieldVar == "" ? lstfieldVar[i].Answer : fieldVar + "," + lstfieldVar[i].Answer;
                            fieldVar = "'" + fieldVar + "'";
                            formulaExp = formulaExp.replace(arr[0], fieldVar);
                            fieldVar = '';

                        }
                        else {
                            for (var i = 0; i < arr.length; i++) {
                                if (fieldVar == null || fieldVar == '') {
                                    var SectionList = [];
                                    if (this.SurveyBenLinkDlg == true) {
                                        SectionList = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                                    }
                                    else {
                                        SectionList = this.lstSectionsQstns;
                                    }

                                    for (var Cnt = 0; Cnt <= SectionList.length - 1; Cnt++) {
                                        var Questions = SectionList[Cnt].Questions;

                                        if (typeof Questions.filter(x => x.FieldVar == arr[i])[0] != "undefined") {
                                            fieldVar = Questions.filter(x => x.FieldVar == arr[i])[0].Answer;
                                        }

                                        if (typeof this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[i])[0] != "undefined") {
                                            fieldVar = this.RepeatOrSkipSection.filter(x => x.FieldVar == arr[i])[0].Answer;
                                        }
                                        if (fieldVar == null || fieldVar == '') {

                                        }
                                    }
                                }

                                var FieldVarAns = fieldVar == "" ? 0 : fieldVar; //fieldVar ?"":0
                                formulaExp = formulaExp.replace(arr[i], "'" + FieldVarAns + "'");
                                //formulaExp += formulaExp.length > 0 ? +"," + FieldVarAns : FieldVarAns;

                                if (fieldVar1.length > 0) {

                                    fieldVar1 = fieldVar1 + "," + FieldVarAns;
                                } else {
                                    fieldVar1 = FieldVarAns;
                                }
                                fieldVar = '';
                            }
                        }

                        var varStatus = "";
                        if (fieldVar1 != null && fieldVar1.length > 0) {

                            var var1 = "", var2 = "", var3 = "";
                            var splt = fieldVar1.split(",");
                            if (splt != null && splt.length > 0) {
                                var1 = splt[0];
                                var2 = splt[1];
                            }

                            switch (var1) {
                                case "Low":
                                    var1 = "L";
                                    break;
                                case "Medium":
                                    var1 = "M";
                                    break;
                                case "High":
                                    var1 = "H";
                                    break;
                            }

                            switch (var2) {
                                case "Low":
                                    var2 = "L";
                                    break;
                                case "Medium":
                                    var2 = "M";
                                    break;
                                case "High":
                                    var2 = "H";
                                    break;
                            }

                            if (var1 == "H" && var2 == "L") {
                                varStatus = "L";
                            } else if (var1 == "H" && (var2 == "M" || var2 == "H")) {
                                varStatus = "H";
                            } else if (var1 == "M" && var2 == "H") {
                                varStatus = "H";
                            } else if (var1 == "M" && var2 == "M") {
                                varStatus = "M";
                            } else if (var1 == "M" && var2 == "L") {
                                varStatus = "L";
                            } else if (var1 == "L" && (var2 == "H" || var2 == "M" || var2 == "L")) {
                                varStatus = "L";
                            }
                        }

                        //debugger;
                        //var strFn = eval(formulaExp);
                        //debugger;
                        //console.log(strFn);
                        //resultVal = eval(strFn);
                        //item.Answer = resultVal;
                        item.Answer = varStatus;

                    }
                }
            }
        },

        //============Load more Tasks============//
        async fnLoadNewTasks(item, index, mobile) {
            console.log(item)
            if(!item.loadMore) return;
            this.newTaskLoader = true
            try {
                if(!item.loadMore) return
                let { TypeID, PageNumber } = item
                let renderedList = item.TaskLog.map(x => x.myTaskListpkId)
                let newTaskList = []
                let tempTaskLog = []
                let obj = syncData
                await obj.syncData.GetLoadmoreTasks({ row: PageNumber, TypeID })
                    .then(async (val) => {
                        !mobile && (this.TaskCardData[index].PageNumber += 1)
                        mobile && (item.PageNumber += 1)
                        newTaskList = (await dbImworks.MyTaskList.where({ TypeID }).toArray()).filter(x => !renderedList.includes(x.pkId));
                        console.warn(newTaskList)
                    })
                if(!newTaskList.length) {
                    if(mobile) {
                        item.loadMore = false
                        return
                    }
                    this.TaskCardData[index].loadMore = false
                    return
                }
                for (let i = 0; i < newTaskList.length; i++) {
                    let newTask = await this.processTaskLog(newTaskList[i], index)
                    if(newTask) {
                        tempTaskLog.push(newTask)
                    } else {
                        this.AnytimeLog.forEach(x => {
                            if(!renderedList.includes(x.pkId)) {
                                tempTaskLog.push(x)
                            }
                        })
                    }
                }
                if(mobile) {
                    this.$set(this.mobileTaskItem, 'TaskLog', [...item.TaskLog, ...tempTaskLog])
                }
                else {
                    this.$set(this.TaskCardData[index], 'TaskLog', [...item.TaskLog, ...tempTaskLog])
                }
            } catch(error) {
                console.error('Error occured', error)
            } finally {
                this.newTaskLoader = false
            }
        },

        async processTaskLog(Survey, index) {
            let objSurvey = JSON.parse(Survey.TaskData)
            let TaskStatus = "";
            let CurrentOrOverdue = "";
            let dtTaskDate = new Date();
            let Month = "";
            let DataHandledBy = "";
            let TaskUpdatedComment = "";
            let TaskCurrentStatus = "";
            let lstRejectData = [];
            let lstRejectDataTemp = [];
            let WorkflowStage = "";
            let ActionRequired = "";
            let AssignedTo = "";
            let NextAssignToUserId = "";
            let NextLevelAssignedUser = "";

            CurrentOrOverdue = "Current";
            if (Survey.TaskDate.substr(0, 10) == new Date().toISOString().substr(0, 10)) {
                CurrentOrOverdue = "Current";
            } else {
                CurrentOrOverdue = "Overdue";  // Overdue task also shows as Pending
            }
            if (Survey.ActionRequired == "Completed") {
                CurrentOrOverdue = "Completed";
            }
            else if (Survey.WFLevel > 0) {
                CurrentOrOverdue = "Review";
            }
            else if (Survey.ActionRequired == "To Resubmit") {
                CurrentOrOverdue = "Rejected";
            }
            DataHandledBy = Survey.TaskdataAddedBy;
            TaskUpdatedComment = "";
            if (Survey.Command == "ADD" && Survey.WFLevel == 0) {
                dtTaskDate = Survey.TaskDate;
            }
            else {
                dtTaskDate = Survey.TaskSubmittedDate;
            }
            TaskCurrentStatus = Survey.Status;
            TaskStatus = "To Data Collect";

            WorkflowStage = Survey.WorkflowStage,
            ActionRequired = Survey.ActionRequired,
            AssignedTo = Survey.AssignedTo,
            // for updating rejected status
            lstRejectData = [];
            lstRejectDataTemp = [];
            if (Survey.Status == "DataCollected" && Survey.Command == "TaskResponse") {
                lstRejectDataTemp = this.lstTaskList.filter(x => x.ResponseSyncID == Survey.ResponseSyncID);
                lstRejectData = lstRejectDataTemp.filter(x => x.Status == "Rejected");
            }
            var dtMyTask = [];
            if (objSurvey.Periodicity == "Anytime" && Survey.Command == "TaskResponse") {
                dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.selectedTaskpkId == Survey.pkId);
                // Delete if exists from Anytime table using date
                for (var aCnt = 0; aCnt <= this.lstAnyTimeNewSubmission.length - 1; aCnt++) {

                    var data = this.lstAnyTimeNewSubmission[aCnt].TaskData;

                    var SubmittedDate = objUtils.common.formatDateWithTimeAndSec(Survey.TaskAddedDate);
                    var CreateDate = objUtils.common.formatDateWithTimeAndSec(data.CreatedDate);
                    if (SubmittedDate == CreateDate) {

                        await dbImworks.TbAnyTimeData.where("pkId").equals(parseInt(this.lstAnyTimeNewSubmission[aCnt].pkId)).delete();
                    }
                }
                this.lstAnyTimeNewSubmission = [];
                this.lstAnyTimeNewSubmission = await dbImworks.TbAnyTimeData.toArray();
            }
            else if (objSurvey.Periodicity == "Anytime" && Survey.Command == "ADD") {
                dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.SyncDBID == Survey.SyncDBID);
            } else {
                dtMyTask = this.TaskSubmittedList.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.TaskDate == Survey.TaskDate);
            }
            if (Survey.Command == "TaskResponse" && Survey.Status == "DataApproved") {
                DataHandledBy = Survey.TaskdataAddedBy;
            }
            NextAssignToUserId = Survey.AssignedToIds;
            NextLevelAssignedUser = Survey.NextLevelAssignedUsers;
            if (dtMyTask.length > 0) {
                if (typeof dtMyTask[0].NextAssignToUserId != "undefined") {
                    DataHandledBy = dtMyTask[0].TaskSubmittedBy;
                }
                var IsSyncing = this.TaskSyncDb.filter(x => x.TypeID == objSurvey.SurveyID && x.Type == 'Survey' && x.TaskSubmittedDate == dtMyTask[0].TaskSubmittedDate);
                if (IsSyncing.length > 0) { CurrentOrOverdue = "Syncing" }
                else {
                    if (Survey.NextLevelAssignedUsers == null) { //NextLevelAssignedUser
                        CurrentOrOverdue = "Completed"
                        ActionRequired = "Completed";
                        AssignedTo = "NA";
                        WorkflowStage = "NA";
                        NextLevelAssignedUser = "NA";
                        DataHandledBy = Survey.TaskdataAddedBy;
                    }
                    else {
                        CurrentOrOverdue = "Review";
                        dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                    }
                }
                if (typeof dtMyTask[0].NextAssignToUserId != "undefined") {
                    NextAssignToUserId = dtMyTask[0].NextAssignToUserId;
                }
                if (typeof dtMyTask[0].WorkflowStage != "undefined") {
                    WorkflowStage = dtMyTask[0].WorkflowStage;
                }

                if (typeof dtMyTask[0].ActionRequired != "undefined") {
                    ActionRequired = dtMyTask[0].ActionRequired;
                }

                if (typeof dtMyTask[0].NextLevelAssignedUser != "undefined") {
                    if (dtMyTask[0].NextLevelAssignedUser == null) //NextLevelAssignedUser
                    {
                        CurrentOrOverdue = "Completed"
                        if (IsSyncing.length > 0) { CurrentOrOverdue = "Syncing" }
                        ActionRequired = "Completed";
                        AssignedTo = "NA";
                        WorkflowStage = "NA";
                        NextLevelAssignedUser = "NA";
                    }
                    else {
                        CurrentOrOverdue = "Review";
                        if (IsSyncing.length > 0) { CurrentOrOverdue = "Syncing" }
                        NextLevelAssignedUser = dtMyTask[0].NextLevelAssignedUser;
                        AssignedTo = dtMyTask[0].NextLevelAssignedUser;
                        dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                    }
                }
                if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "DataCollected" && dtMyTask[0].WFLevel == 0) {
                    TaskStatus = "For Approval";
                    WorkflowStage = Survey.WorkflowStage;
                    ActionRequired = "To Review";
                }
                else if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "Pending" && dtMyTask[0].WFLevel == 0) {
                    TaskStatus = "For Approval";
                    WorkflowStage = "Level " + (dtMyTask[0].WFLevel + 1);
                    ActionRequired = "To Review";
                    AssignedTo = Survey.NextLevelAssignedUsers;
                    dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                }
                else if (dtMyTask[0].WFLevelStatus == "Saved" && TaskCurrentStatus == "Pending" && dtMyTask[0].WFLevel == 0) {
                    TaskStatus = "To Data Collect";
                    CurrentOrOverdue = "Saved";
                    WorkflowStage = "Survey Submission";
                    ActionRequired = "To Submit";
                    dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                }
                else if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "DataApproved" && dtMyTask[0].WFLevel > 0) {
                    TaskStatus = "Approved";
                    this.TaskCardData[index].IsDataCollector = false;
                }
                else if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "DataApproved" && dtMyTask[0].WFLevel == 0) {
                    TaskStatus = "Approved";
                    WorkflowStage = Survey.WorkflowStage;
                    ActionRequired = Survey.ActionRequired;
                }
                else if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "DataCollected" && dtMyTask[0].WFLevel > 0) {
                    TaskStatus = "Approved";
                    Message = "Survey Data approved";
                    ActionRequired = Survey.ActionRequired;
                    WorkflowStage = "Level " + (dtMyTask[0].WFLevel + 1);
                    AssignedTo = Survey.NextLevelAssignedUsers;
                    dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                }
                else if (dtMyTask[0].WFLevelStatus == "Rejected" && (TaskCurrentStatus == "DataApproved" || TaskCurrentStatus == "DataCollected") && dtMyTask[0].WFLevel > 0) {
                    TaskStatus = "Rejected";
                    WorkflowStage = "Survey Submission";
                    CurrentOrOverdue = "Rejected";
                    AssignedTo = NextLevelAssignedUser;
                    dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                }
                else if (dtMyTask[0].WFLevelStatus == "Completed" && TaskCurrentStatus == "Rejected" && dtMyTask[0].WFLevel == 0) {
                    dtTaskDate = dtMyTask[0].TaskSubmittedDate;
                }
                if (Survey.WorkflowID == 0) {
                    ActionRequired = "Completed";
                    WorkflowStage = "NA";
                    AssignedTo = "NA";
                    CurrentOrOverdue = "Completed";
                }
                //Action Required conditions
                if (dtMyTask[0].WFLevelStatus == "Rejected") {
                    ActionRequired = "To Resubmit";
                    if (dtMyTask[0].WFLevelStatus == "Pending" || dtMyTask[0].WFLevelStatus == "Completed") {
                        ActionRequired = "To Review";
                    }
                }
            }
            else {
                if (Survey.WFLevel > 0 && lstRejectData.length == 0) {
                    var lstDataApproversID = [];
                    var lstDatacollectorsID = [];

                    if (Survey.DataCollectorIds != null && Survey.DataCollectorIds.length > 0) {
                        lstDatacollectorsID = Survey.DataCollectorIds.split(',').map(element => {
                            return Number(element);
                        });
                    }

                    if (Survey.ApproverIds != null && Survey.ApproverIds.length > 0) {
                        lstDataApproversID = Survey.ApproverIds.split(',').map(element => {
                            return Number(element);
                        });
                    }

                    if(lstDataApproversID.includes(this.LoggedUserID) && !this.TaskCardData[index].isApprover) {
                        this.TaskCardData[index].isApprover = true
                    }

                    if (Survey.Status == "DataCollected" && lstDatacollectorsID.includes(this.LoggedUserID)) {
                        if (!lstDataApproversID.includes(this.LoggedUserID)) {
                            CurrentOrOverdue = "Review";
                        }
                        TaskStatus = "For Approval";
                        if (Survey.WorkflowID == 0) {
                            CurrentOrOverdue = "Completed";
                        }

                    } else if (Survey.Status == "DataCollected" && lstDataApproversID.includes(this.LoggedUserID)) {
                        if (lstDatacollectorsID.includes(this.LoggedUserID)) {
                            TaskStatus = "For Approval";
                        }
                        else {
                            TaskStatus = "For Approval";
                            this.TaskCardData[index].IsDataCollector = false; // States that logged user is approver
                        }
                    }
                    else if (Survey.Status == "DataApproved" && Survey.WFLevelStatus == "Completed") {
                        if (!lstDatacollectorsID.includes(this.LoggedUserID)) {
                            this.TaskCardData[index].IsDataCollector = false;
                        }
                        TaskStatus = "Approved";
                    } else if (Survey.Status == "Rejected" && Survey.WFLevelStatus == "Completed") {
                        TaskStatus = "Rejected";
                        CurrentOrOverdue = "Rejected";
                    } else if (Survey.Status == "Rejected" && Survey.WFLevelStatus == "Pending") {
                        TaskStatus = "To Data Collect";
                        CurrentOrOverdue = "Rejected";
                    } else if (Survey.Status == "DataApproved" && Survey.WFLevelStatus == "Pending" && lstDataApproversID.includes(this.LoggedUserID)) {
                        TaskStatus = "For Approval";
                        this.TaskCardData[index].IsDataCollector = false;
                    }
                    else if (Survey.Status == "DataApproved" && Survey.WFLevelStatus == "Pending" && !lstDataApproversID.includes(this.LoggedUserID)) {
                        TaskStatus = "Approved";
                    }
                }
                else {
                    if (lstRejectData.length > 0) {

                        DataHandledBy = lstRejectData[0].TaskdataAddedBy;
                        CurrentOrOverdue = "Rejected"
                        TaskStatus = "To Data Collect";
                    }
                    TaskStatus = "To Data Collect";
                }
            }
            var dateMonth = new Date(Survey.TaskDate);
            if (this.lstmonth.length > 0) {
                Month = this.lstmonth[dateMonth.getMonth()];
            }
            var booltemp = false;
            if (objSurvey.EndDate != null) {
                if (objSurvey.EndDate.substring(0, 10) >= new Date().toISOString().substring(0, 10)) {
                    if (objSurvey.Periodicity == "Anytime" && Survey.Command == "ADD") {
                        var existsanytime = this.lstTaskList.filter(x => x.TypeID == Survey.TypeID && x.Command == "TaskResponse");
                        if (existsanytime.length > 0) {
                            booltemp = true;
                        }
                    }
                }
            }
            if (objSurvey.Periodicity == "Anytime" && Survey.Command == "ADD") {
                this.AnytimeLog = [];
                let AnyTimeTask = {
                    TaskIndex: index + 1,
                    Command: Survey.Command,
                    ProjectName: objSurvey.ProjectName,
                    ProjectId: objSurvey.ProjectId,
                    TaskName: objSurvey.SurveyName,
                    TaskType: 'Survey',
                    TypeID: objSurvey.SurveyID,
                    Name: DataHandledBy,
                    // Message: Message,
                    CreatedDate: dtTaskDate,
                    Comment: TaskUpdatedComment,
                    Status: TaskStatus,
                    CurrentOrOverdue: CurrentOrOverdue,
                    Month: Month, AssignedTo: "",
                    Periodicity: objSurvey.Periodicity,
                    BeneficiaryType: objSurvey.BeneficiaryType,
                    BeneficiaryTypeID: objSurvey.BeneficiaryTypeID,
                    TaskDate: Survey.TaskDate,
                    SyncDBID: Survey.SyncDBID,
                    TypeIDColumn: Survey.TypeIDColumn,
                    WorkflowID: Survey.WorkflowID,
                    TotalWFLevel: Survey.TotalWFLevel,
                    DataCollectedBy: Survey.DataCollectedBy,
                    ResponseSyncID: Survey.ResponseSyncID,
                    WorkspaceID: Survey.WorkspaceID,
                    WFLevel: Survey.WFLevel,
                    WFLevelStatus: Survey.WFLevelStatus,
                    AssignedTo: AssignedTo,
                    WorkflowStage: WorkflowStage,
                    ActionRequired: ActionRequired,
                    NextLevelAssignedUser: Survey.NextLevelAssignedUsers,
                    DataCollectorName: Survey.DataCollectorName,
                    AssignedToIds: NextAssignToUserId,//Survey.AssignedToIds,
                    NextLevelAssignedUsersIds: Survey.NextLevelAssignedUsersIds,
                    DataCollectionSyncDBID: Survey.DataCollectionSyncDBID,
                    BeneficiaryCount: Survey.BeneficiaryCount,
                    ResponseCount: Survey.ResponseCount,
                    CurrentUserWFLevel: Survey.CurrentUserWFLevel,
                    AnswerCount: Survey.AnswerCount,
                    QuestionCount: Survey.QuestionCount,
                    TaskData: objSurvey,
                    myTaskListpkId: Survey.pkId
                }
                this.AnytimeLog.push(AnyTimeTask);

                var Tempdata = await dbImworks.TbAnyTimeTempData.toArray();

                var isExists = Tempdata.filter(x => x.TypeID == Survey.TypeID && x.Type == Survey.Type)
                if (isExists.length == 0) {
                    await dbImworks.TbAnyTimeTempData.put({
                        Type: Survey.Type,
                        TypeID: Survey.TypeID,
                        TaskData: this.AnytimeLog
                    });
                }
            }
            if (lstRejectData.length == 0 && booltemp == false) {
                return {
                    TaskIndex: index + 1,
                    Command: Survey.Command,
                    ProjectName: objSurvey.ProjectName,
                    ProjectId: objSurvey.ProjectId,
                    TaskName: objSurvey.SurveyName,
                    TaskType: 'Survey',
                    TypeID: objSurvey.SurveyID,
                    Name: DataHandledBy,
                    //Message: Message,
                    CreatedDate: dtTaskDate,
                    Comment: TaskUpdatedComment,
                    Status: TaskStatus,
                    CurrentOrOverdue: CurrentOrOverdue,
                    Month: Month,
                    Periodicity: objSurvey.Periodicity,
                    BeneficiaryType: objSurvey.BeneficiaryType,
                    BeneficiaryTypeID: objSurvey.BeneficiaryTypeID,
                    TaskDate: Survey.TaskDate,
                    SyncDBID: Survey.SyncDBID,
                    TypeIDColumn: Survey.TypeIDColumn,
                    WorkflowID: Survey.WorkflowID,
                    TotalWFLevel: Survey.TotalWFLevel,
                    DataCollectedBy: Survey.DataCollectedBy,
                    ResponseSyncID: Survey.ResponseSyncID,
                    WorkspaceID: Survey.WorkspaceID,
                    WFLevel: Survey.WFLevel,
                    WFLevelStatus: Survey.WFLevelStatus,
                    myTaskListpkId: Survey.pkId,
                    AssignedTo: AssignedTo,
                    WorkflowStage: WorkflowStage,
                    ActionRequired: ActionRequired,
                    NextLevelAssignedUser: NextLevelAssignedUser,//Survey.NextLevelAssignedUsers,
                    DataCollectorName: Survey.DataCollectorName,
                    DataCollectorIds: Survey.DataCollectorIds,
                    AssignedToIds: NextAssignToUserId,
                    NextLevelAssignedUsersIds: Survey.NextLevelAssignedUsersIds,
                    DataCollectionSyncDBID: Survey.DataCollectionSyncDBID,
                    BeneficiaryCount: Survey.BeneficiaryCount,
                    ResponseCount: Survey.ResponseCount,
                    CurrentUserWFLevel: Survey.CurrentUserWFLevel,
                    AnswerCount: Survey.AnswerCount,
                    QuestionCount: Survey.QuestionCount,
                    TaskData: objSurvey,
                }
            }
        },
    }

}
</script>

<style scoped lang="scss">
.EditIconHover,
.AcitivityIconHover {
    display: none;
    color: #3D7EFE;
    font-weight: bold;
    position: relative;
    animation: TextOut 1s;
}

.EditActButton:hover~.EditIconHover {
    display: block;
    animation: TextIn 1s;
}

.EditActButton:hover~.PlaceHolderText {
    display: none;
}

.ActivityActButton:hover~.AcitivityIconHover {
    display: block;
    animation: TextIn 1s;
}

.ActivityActButton:hover~.PlaceHolderText {
    display: none;
}

@keyframes TextIn {
    from {
        top: 18px;
    }

    to {
        top: 0px;
    }
}

@keyframes TextOut {
    from {
        top: 18px;
    }

    to {
        top: 25px;
    }
}

.downloadButton:before {
    opacity: 0 !important;
}

.v-ripple__container {
    opacity: 0 !important;
}

.vFileInput {
    width: 35%;
}

/*.dialogHeight {
        height: 85vh;
    }*/

.scrollBoxHeight {
    height: 46vh;
}

.historyHeight {
    height: 52vh;
}

.taskHeight {
    height: 63vh
}

.uploadBoxHeight {
    height: 53vh;
}

.uploadScrollHeight {
    height: 49vh;
}

@media screen and (max-width: 750px) {
    .vFileInput {
        width: 50%;
    }

    .dialogHeight {
        height: 85vh;
    }

    .scrollBoxHeight {
        height: 50vh;
    }

    .historyHeight {
        height: 56vh;
    }

    .taskHeight {
        height: 65vh
    }

    .uploadBoxHeight {
        height: 54vh;
    }

    .uploadScrollHeight {
        height: 48vh;
    }
}

.text-limit {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.text-limit:hover {
    white-space: normal;
    overflow: visible;
}

.task_height {
    height: 66vh;
}

.custom-text-field {
    width: 100%;
    border: 1px solid #c2c2c2;
    color: rgba(0, 0, 0, 0.6);
    border-radius: 4px;
    transition: border 0.3s cubic-bezier(0.25, 0.8, 0.5, 1);
    margin-bottom: 16px;
    -webkit-transition: border 0.3s;
    padding: 5px 10px 5px 10px;
    min-height: 40px;
    font-size: 16px;
    background-color: #fff;
}

.custom-text-field:focus {
    border: 2px solid #3374B9;
    outline: none;
}

.v-expansion-panels.mobile_expansion_panels::after {
    content: '';
    width: 10px;
    height: 100%;
    background-color: #c2c2c2 !important;
    opacity: .4;
    position: absolute;
    top: 0px;
    left: 20px;
    z-index: 1;
}

.v-expansion-panel.expansion_card::before {
    box-shadow: none !important
}

.v-expansion-panel.expansion_card::after {
    display: none;
}



/*------------ Mobile Expansion Card Scoped------------- */

.v-expansion-panel.expansion_card.mobile_expansion_card {
    padding: 3px;
}

.v-expansion-panel.expansion_card.mobile_expansion_card .expansion-header {
    padding: 0px;
    padding-bottom: 10px;
}

.v-expansion-panel.expansion_card.mobile_expansion_card .expansion-header {
    background: url("../../images/Fillers.svg") #D4EBFF;
    background-size: cover;
    background-repeat: no-repeat;
}

.v-expansion-panel.expansion_card.mobile_expansion_card .notif_chip_group {
    float: right;
}

.v-expansion-panel.expansion_card.mobile_expansion_card .expansion-header {
    background-color: #D4EBFF;
    border-radius: 8px
}

.v-expansion-panel.expansion_card.mobile_expansion_card .task-details-row {
    display: none !important;
}

.v-expansion-panel.expansion_card.mobile_expansion_card .notif_chip_group .v-chip.v-chip.bell_chip {
    background: darkslategray
}

.v-expansion-panel.expansion_card.mob_expansion_card .expansion-header {
    background: #FFFFFF;
    border: 1px solid #E2E2E2;
    box-sizing: border-box;
    box-shadow: 0px 0px 4px rgba(0, 0, 0, 0.16) !important;
    border-radius: 10px;
    padding: 0px;
    margin: 0px !important;
    font-size: 14px !important;
}

.v-expansion-panel.expansion_card.mob_expansion_card {
    margin: 0px 2px 8px 2px !important;
    padding: 4px !important;
}

.v-expansion-panel.expansion_card.mob_expansion_card .expansion-content .blue_dot::before {
    content: '';
    position: absolute;
    border-radius: 50%;
    left: 20px;
    top: 80px;
    width: 10px;
    height: 10px;
    background-color: #3D7EFE !important;
    z-index: 2;
}

.v-expansion-panel.expansion_card.mob_expansion_card .expansion-content .black_dot::before {
    content: '';
    position: absolute;
    border-radius: 50%;
    left: 20px;
    top: 80px;
    width: 10px;
    height: 10px;
    background-color: #707070 !important;
    z-index: 2;
}


/*--------------- Desktop Expansion Card Scoped--------------- */

.v-expansion-panel.expansion_card.desktop_expansion_card {
    margin-bottom: 16px !important
}

.v-expansion-panel.expansion_card.desktop_expansion_card .notif_chip_group {
    float: left;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .date-subtext {
    display: none;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .taskname-heading {
    font-family: 'Poppins';
    font-style: normal;
    font-weight: 500;
    font-size: 16px !important;
    //line-height: 32px;
    text-transform: capitalize;
    color: #374151;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .expansion-header {
    background: #FFFFFF;
    border: 1px solid #E2E2E2;
    box-sizing: border-box;
    box-shadow: 0px 0px 4px rgba(0, 0, 0, 0.16) !important;
    border-radius: 5px 5px 0px 0px;
    padding: 0px;
    margin-bottom: 0px !important;
    font-size: 14px !important;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .list-item-container {
    padding-inline-start: 8px;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .list-item-content {
    padding: 0px;
}

.v-expansion-panel.expansion_card.desktop_expansion_card .task-type-strip {
    flex-grow: 0;
}

.notif_chip_group {
    min-width: min-content;
}

.notif_chip_group .v-chip.bell_chip {
    background-color: #3D7EFE;
}

.mobile-view {
    height: 30vh;
    box-shadow: none !important;
}

/* Bulk Import */
.bulkImportContainer{
    width: 100%;
    display: flex;
    justify-content: end;
    padding: 0.25rem;

    .timeline_body{
        display: flex;
        justify-content: space-between;
        width: 100%;
    }


}
</style>

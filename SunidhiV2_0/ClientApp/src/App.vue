<template>
    <div id="app">
        <v-app>
            <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
            <!-- app bar left side view -->
            <v-navigation-drawer v-if="authState && currentWorkspaceID > 0"
                                 v-model="drawer"
                                 :mini-variant="mini"
                                 fixed=""
                                 app
                                 width="190"
                                 mini-variant-width="60">
                <v-list dense>
                    <v-list-item style="min-height: 55px">
                        <v-list-item-icon link=""
                                          @click.stop="mini = !mini"
                                          class="pt-1 pl-1">
                            <v-icon v-show="mini">mdi-menu</v-icon>
                            <v-icon v-show="!mini">mdi-close</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            <v-list-item-title>
                                <img src="../src/images/apurthi_logo.png"
                                     class="main-logo ml-1" />
                            </v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item v-show="showDashBoard"
                                 v-bind:disabled="DashBoardAccess === 1"
                                 active-class="left-nav-btn"
                                 link
                                 :to="{
              path: '/',
              query: { WsID: currentWorkspaceID, Access: DashBoardAccess },
            }"
                                 v-bind:class="{ 'left-nav-btn': $route.path == '/' }"
                                 style="margin-top: -8px">
                        <v-list-item-action>
                            <v-avatar size="30" class="mr-3">
                                <v-icon size="20">icon-dashboard_im_ic</v-icon>
                                <!--<v-img src="../src/assets/nav_bar_icons/svg/ic_dashboard_active.svg" aspect-ratio="1" size="20"></v-img>-->
                            </v-avatar>
                        </v-list-item-action>
                        <v-list-item-content>
                            <v-list-item-title>Dashboard</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <!-- <v-list-item v-show="showInitiatives" v-bind:disabled="InitiativesAccess ===1"
                 active-class="active-primary-button" link :to="{ name: 'initiatives', query: { WsID: currentWorkspaceID, Access: InitiativesAccess } }"
                 v-bind:class="{ 'nav-btn-active': $route.name == 'DraftInitiatives' || $route.name == 'DeployedInitiatives' ||
                                                          $route.name == 'ArchivedInitiatives' ||
                                                          $route.name == 'ManageInitiatives' ||
                                                          $route.name == 'ManageInitiativesProjectTemplate' || $route.name == 'ManageInitiativesProjectWorkflow' ||
                                                          $route.name == 'ManageInitiativesDetails' || $route.name == 'ManageInitiativesUsersGroups' ||
                                                          $route.name == 'Projects' ||
                                                          $route.name == 'DraftProjects' || $route.name == 'DeployedProjects' ||
                                                          $route.name == 'CompletedProjects' || $route.name == 'ArchivedProjects' ||
                                                          $route.name == 'ManageProject' ||
                                                          $route.name == 'ManageProjectDetails' || $route.name == 'ManageProjectTeam' ||
                                                          $route.name == 'ManageProjectFinance' || $route.name == 'ManageProjectFinanceBudget' || $route.name == 'ManageProjectFinancePaymentTerms' || $route.name == 'ManageProjectFinanceAccounts' || $route.name == 'ManageProjectFinanceLedgerBook' || $route.name == 'ManageProjectWorkflow' ||
                                                          $route.name == 'ManageProjectSurveys' || $route.name == 'ManageProjectSurveysIndividual' || $route.name == 'ManageProjectBeneficiaries' || $route.name == 'ManageProjectBeneficiariesTypeIndividual' || $route.name == 'ManageProjectBeneficiariesIndividual' ||
                                                          $route.name == 'ManageProjectActivities' || $route.name == 'ManageProjectActivitiesIndividual' || $route.name == 'ManageProjectKPI' || $route.name == 'ManageProjectOutcomes'|| $route.name == 'ManageProjectKPIActivityLog' ||
                                                          $route.name == 'ManageProjectRisks' || $route.name == 'ManageProjectRiskActivityLog' || $route.name == 'ManageProjectDocuments' ||
                                                          $route.name == 'ManageProjectDiscussion' }">
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">mdi-hexagon-multiple-outline</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content  >
            <v-list-item-title>Initiatives</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                    <v-list-item v-show="showUsers"
                                 v-bind:disabled="UsersAccess === 1"
                                 active-class="left-nav-btn"
                                 link
                                 :to="{
              path: '/UserList',
              query: { WsID: currentWorkspaceID, Access: UsersAccess },
            }"
                                 v-bind:class="{
              'left-nav-btn':
                $route.name == 'userlist' ||
                $route.name == 'userindividual' ||
                $route.name == 'usersgroup' ||
                $route.name == 'usergroupindividual' ||
                $route.name == 'usersroles' ||
                $route.name == 'userroleindividual',
            }">
                        <v-list-item-action>
                            <v-avatar size="30" class="mr-3">
                                <v-icon size="20" color="#707070">icon-profile_im_ic</v-icon>
                            </v-avatar>
                        </v-list-item-action>
                        <v-list-item-content>
                            <v-list-item-title>Users</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <v-list-item name="btnProject" v-show="showProjects"
                                 v-bind:disabled="ProjectsAccess === 1"
                                 active-class="left-nav-btn"
                                 link
                                 :to="{
              path: '/MyprojectsDraft',
              query: { WsID: currentWorkspaceID, Access: ProjectsAccess },
            }"
                                 v-bind:class="{
              'left-nav-btn':
                $route.name == 'MyprojectsDeployed' ||
                $route.name == 'MyprojectsCompleted' ||
                $route.name == 'MyprojectsArchived' ||
                $route.name == 'MyprojectsDraftIndividual' ||
                $route.name == 'MyprojectsDraftIndividualTeam' ||
                $route.name == 'MyprojectsDraftIndividualFinanceDetails' ||
                $route.name ==
                  'MyprojectsDraftIndividualFinanceDetailsBudget' ||
                $route.name == 'MyprojectsDraftIndividualWorkflow' ||
                $route.name == 'MyprojectsDraftIndividualSurveys' ||
                $route.name == 'MyprojectsDraftIndividualBeneficiaries' ||
                $route.name == 'MyprojectsDraftIndividualActivities' ||
                $route.name == 'MyprojectsDraftIndividualKPI' ||
                $route.name == 'MyprojectsDraftIndividualRisks' ||
                $route.name == 'MyprojectsDraftIndividualDocuments' ||
                $route.name == 'MyprojectsDraftIndividualFinancePaymentTerms' ||
                $route.name == 'MyprojectsDraftIndividualSurveysIndividual' ||
                $route.name ==
                  'MyprojectsDraftIndividualBeneficiariesTypeIndividual' ||
                $route.name ==
                  'MyprojectsDraftIndividualBeneficiariesIndividual' ||
                $route.name ==
                  'MyprojectsDraftIndividualActivitiesIndividual' ||
                $route.name == 'MyprojectsDraftIndividualKPIActivityLog' ||
                $route.name == 'MyprojectsDraftIndividualRiskActivityLog' ||
                $route.name == 'MyprojectsDraftIndividualKPIOutcomes' ||
                $route.name == 'MyProjectDraftIndividualOutcomeActivityLog' ||
                $route.name == 'MyprojectsDeployedIndividual' ||
                $route.name == 'MyprojectsDeployedIndividualTeam' ||
                $route.name == 'MyprojectsDeployedIndividualFinanceDetails' ||
                $route.name ==
                  'MyprojectsDeployedIndividualFinanceDetailsBudget' ||
                $route.name ==
                  'MyprojectsDeployedIndividualFinancePaymentTerms' ||
                $route.name == 'MyprojectsDeployedIndividualWorkflow' ||
                $route.name == 'MyprojectsDeployedIndividualSurveys' ||
                $route.name ==
                  'MyprojectsDeployedIndividualSurveysIndividual' ||
                $route.name == 'MyprojectsDeployedIndividualBeneficiaries' ||
                $route.name ==
                  'MyprojectsDeployedIndividualBeneficiariesTypeIndividual' ||
                $route.name ==
                  'MyprojectsDeployedIndividualBeneficiariesIndividual' ||
                $route.name == 'MyprojectsDeployedIndividualActivities' ||
                $route.name ==
                  'MyprojectsDeployedIndividualActivitiesIndividual' ||
                $route.name == 'MyprojectsDeployedIndividualKPI' ||
                $route.name == 'MyprojectsDeployedIndividualKPIOutcomes' ||
                $route.name ==
                  'MyProjectDeployedIndividualOutcomeActivityLog' ||
                $route.name == 'MyprojectsDeployedIndividualKPIActivityLog' ||
                $route.name == 'MyprojectsDeployedIndividualRisks' ||
                $route.name == 'MyprojectsDeployedIndividualRiskActivityLog' ||
                $route.name == 'MyprojectsDeployedIndividualDocuments' ||
                $route.name == 'MyprojectsCompletedIndividual' ||
                $route.name == 'MyprojectsCompletedIndividualTeam' ||
                $route.name == 'MyprojectsCompletedIndividualFinanceDetails' ||
                $route.name ==
                  'MyprojectsCompletedIndividualFinanceDetailsBudget' ||
                $route.name ==
                  'MyprojectsCompletedIndividualFinancePaymentTerms' ||
                $route.name == 'MyprojectsCompletedIndividualWorkflow' ||
                $route.name == 'MyprojectsCompletedIndividualSurveys' ||
                $route.name ==
                  'MyprojectsCompletedIndividualSurveysIndividual' ||
                $route.name == 'MyprojectsCompletedIndividualBeneficiaries' ||
                $route.name ==
                  'MyprojectsCompletedIndividualBeneficiariesTypeIndividual' ||
                $route.name ==
                  'MyprojectsCompletedIndividualBeneficiariesIndividual' ||
                $route.name == 'MyprojectsCompletedIndividualActivities' ||
                $route.name ==
                  'MyprojectsCompletedIndividualActivitiesIndividual' ||
                $route.name == 'MyprojectsCompletedIndividualKPI' ||
                $route.name == 'MyprojectsCompletedIndividualKPIOutcomes' ||
                $route.name ==
                  'MyProjectCompletedIndividualOutcomeActivityLog' ||
                $route.name == 'MyprojectsCompletedIndividualKPIActivityLog' ||
                $route.name == 'MyprojectsCompletedIndividualRisks' ||
                $route.name == 'MyprojectsCompletedIndividualRiskActivityLog' ||
                $route.name == 'MyprojectsCompletedIndividualDocuments' ||
                $route.name == 'MyprojectsArchivedIndividual' ||
                $route.name == 'MyprojectsArchivedIndividualTeam' ||
                $route.name == 'MyprojectsArchivedIndividualFinanceDetails' ||
                $route.name ==
                  'MyprojectsArchivedIndividualFinanceDetailsBudget' ||
                $route.name ==
                  'MyprojectsArchivedIndividualFinancePaymentTerms' ||
                $route.name == 'MyprojectsArchivedIndividualWorkflow' ||
                $route.name == 'MyprojectsArchivedIndividualSurveys' ||
                $route.name ==
                  'MyprojectsArchivedIndividualSurveysIndividual' ||
                $route.name == 'MyprojectsArchivedIndividualBeneficiaries' ||
                $route.name ==
                  'MyprojectsArchivedIndividualBeneficiariesTypeIndividual' ||
                $route.name ==
                  'MyprojectsArchivedIndividualBeneficiariesIndividual' ||
                $route.name == 'MyprojectsArchivedIndividualActivities' ||
                $route.name ==
                  'MyprojectsArchivedIndividualActivitiesIndividual' ||
                $route.name == 'MyprojectsArchivedIndividualKPI' ||
                $route.name == 'MyprojectsArchivedIndividualKPIOutcomes' ||
                $route.name ==
                  'MyProjectArchivedIndividualOutcomeActivityLog' ||
                $route.name == 'MyprojectsArchivedIndividualKPIActivityLog' ||
                $route.name == 'MyprojectsArchivedIndividualRisks' ||
                $route.name == 'MyprojectsArchivedIndividualRiskActivityLog' ||
                $route.name == 'MyprojectsArchivedIndividualDocuments',
            }">
                        <v-list-item-action>
                            <v-avatar size="30" class="mr-3">
                                <v-icon size="20">icon-project_im_ic</v-icon>
                            </v-avatar>
                        </v-list-item-action>
                        <v-list-item-content>
                            <v-list-item-title>Projects</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <!--<v-list-item  v-show="showFindProjects"  active-class="active-primary-button" link="" to="">
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">mdi-file-document-box-search-outline</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Find Projects</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                    <!--<v-list-item v-show="showVenture"
                         v-bind:disabled="VentureAccess === 1"
                         active-class="left-nav-btn"
                         link
                         :to="{
      path: '/SearchVenture',
      query: { WsID: currentWorkspaceID, Access: VentureAccess },
    }"
                         v-bind:class="{
      'left-nav-btn':
        $route.name == 'MyVenture' ||
        $route.name == 'MyFinance' ||
        $route.name == 'ManageVentureDetails' ||
        $route.name == 'ManageVentureFinanceDetails' ||
        $route.name == 'ManageVentureActivities' ||
        $route.name == 'ManageVentureKPI' ||
        $route.name == 'ManageVentureDocuments',
    }">
                <v-list-item-action>
                    <v-avatar size="30" class="mr-3">
                        <v-icon size="20">icon-venture_im_ic</v-icon>-->
                    <!-- <v-img src="../src/images/nav-icon.png" aspect-ratio="1"></v-img>-->
                    <!--</v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Venture</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                    <!--<v-list-item v-show="showMyFinance"
                         v-bind:disabled="MyFinanceAccess === 1"
                         active-class="left-nav-btn"
                         link
                         :to="{
      path: '/SearchFinance',
      query: { WsID: currentWorkspaceID, Access: MyFinanceAccess },
    }"
                         v-bind:class="{
      'left-nav-btn':
        $route.name == 'FinanceBudgetAndFunds' ||
        $route.name == 'ManageFinanceAccount' ||
        $route.name == 'ManageFinanceActivitiesAndWorkflow' ||
        $route.name == 'ManageFinanceLog',
    }">
                <v-list-item-action>
                    <v-avatar size="30" class="mr-3">
                        <v-icon size="20">icon-finance_im_ic</v-icon>-->
                    <!-- <v-img src="../src/images/nav-icon.png" aspect-ratio="1"></v-img>-->
                    <!--</v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Finance</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                    <!--<v-list-item v-show="showTasks" v-bind:disabled="TasksAccess ===1" active-class="active-primary-button" link="" :to="{ path: '/OverdueTasks',query: { WsID: currentWorkspaceID , Access: TasksAccess}}"
                 v-bind:class="{ 'nav-btn-active': $route.name == 'OverdueTasks'}">
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">icon-task_im_ic</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Tasks</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                    <v-list-item v-show="showReport"
                                 v-bind:disabled="ReportAccess === 1"
                                 active-class="left-nav-btn"
                                 link
                                 :to="{
              path: '/MapViewReport',
              query: { WsID: currentWorkspaceID, Access: ReportAccess },
            }"
                                 v-bind:class="{
              'left-nav-btn':
                $route.path == '/ProjectReports' ||
                $route.path == '/SurveryReports' ||
                $route.path == '/OutcomesReports' ||
                $route.path == '/OutputReports' ||
                $route.path == '/InputReports' ||
                $route.path == '/BeneficiariesReports',
            }">
                        <v-list-item-action>
                            <v-avatar size="30" class="mr-3">
                                <!-- <v-img :src="report_active" ></v-img> -->
                                <v-icon size="20">mdi-graph</v-icon>
                            </v-avatar>
                        </v-list-item-action>
                        <v-list-item-content>
                            <v-list-item-title>Report</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <!--<v-list-item v-show="showTasks"
                         v-bind:disabled="TasksAccess === 1"
                         active-class="left-nav-btn"
                         link=""
                         to=""
                         v-bind:class="{
      'left-nav-btn':
        $route.name == 'OverdueTasks' ||
        $route.name == 'CurrentTasks' ||
        $route.name == 'CompletedTasks' ||
        $route.name == 'SavedTasks',
    }"
                         @click.native="fnRouteToMyTask()">
                <v-list-item-action>
                    <v-avatar size="30" class="mr-3">
                        <v-icon size="20">icon-task_im_ic</v-icon>
                    </v-avatar>
                </v-list-item-action>
                <v-list-item-content>
                    <v-list-item-title>Tasks</v-list-item-title>
                </v-list-item-content>
            </v-list-item>-->
                    <v-list-item name="btnMyTask"
                                 v-show="showMyTasks"
                                 v-bind:disabled="TasksAccess === 1"
                                 active-class="left-nav-btn"
                                 link=""
                                 to=""
                                 v-bind:class="{'left-nav-btn':$route.name == 'MyTasks'}"
                                 @click.native="fnRouteToNewTaskPage()">
                        <v-list-item-action>
                            <v-avatar size="30" class="mr-3">
                                <v-icon size="20">icon-task_im_ic</v-icon>
                            </v-avatar>
                        </v-list-item-action>
                        <v-list-item-content>
                            <v-list-item-title>MyTasks</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                    <!--Hiding unnecessary navigations-->
                    <!--<v-list-item v-show="showSettings"
                 v-bind:disabled="SettingsAccess === 1"
                 active-class="left-nav-btn"
                 link=""
                 to="">
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">icon-settings_3_im_ic</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Settings</v-list-item-title>
        </v-list-item-content>
    </v-list-item>
    <v-list-item active-class="left-nav-btn" link>
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">icon-feedback_im_ic</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Feedback</v-list-item-title>
        </v-list-item-content>
    </v-list-item>
    <v-list-item active-class="left-nav-btn" link="">
        <v-list-item-action>
            <v-avatar size="30" class="mr-3">
                <v-icon size="20">icon-help_im_ic</v-icon>
            </v-avatar>
        </v-list-item-action>
        <v-list-item-content>
            <v-list-item-title>Help Desk</v-list-item-title>
        </v-list-item-content>
    </v-list-item>-->
                </v-list>
            </v-navigation-drawer>

            <!-- app bar top view -->
            <v-app-bar v-if="authState" app light dense height="55">
                <v-app-bar-nav-icon @click.stop="drawer = !drawer"
                                    class="mobile-nav-icon" />
                <v-toolbar-title class="pl-0">
                    <img src="../src/images/apurthi_logo.png"
                         class="hidden-md-and-up"
                         width="auto"
                         max-width="300"
                         height="55"
                         v-bind:style="[
              mini == false ? { display: 'none' } : { display: 'block' },
            ]" />
                    <img src="../src/images/apurthi_logo.png"
                         class="hidden-sm-and-down main-logo"
                         v-bind:style="[
              mini == false ? { display: 'none' } : { display: 'block' },
            ]" />
                </v-toolbar-title>

                <!-- Search Work Space Textfeild-->
                <!--<template>
        <v-autocomplete v-model="model"
                        :items="WorkspaceList" :item-text="i => i.WSName" :item-value="i => i.WorkspaceID"
                        :loading="isLoading"
                        chips
                        clearable
                        hide-details
                        hide-selected
                        label="Search for a Workspace..."
                        class="ml-4 d-none d-sm-block" dense append-icon="" prepend-inner-icon="mdi-magnify" outlined
                        @click="fnGetWorkspaceList()">
            <template v-slot:no-data>
                <v-list-item>
                    <v-list-item-title>
                        Search for your
                        <strong>Workspace</strong>
                    </v-list-item-title>
                </v-list-item>
            </template>
            <template v-slot:selection="{ attr, on, item, selected }">
                <v-chip v-bind="attr"
                        :input-value="selected"
                        color="blue-grey"
                        class="white--text">
                    <span v-text="item.WSName"></span>
                </v-chip>
            </template>
            <template v-slot:item="{ item }">
                <v-list-item-avatar color="indigo" class="headline font-weight-light white--text">
                    {{ item.WSName.charAt(0) }}
                </v-list-item-avatar>
                <v-list-item-content>
                    <v-list-item-title v-text="item.WSName"></v-list-item-title>
                    <v-list-item-subtitle v-if="item.WSIsPrivate == 1">
                        <v-icon size="12">mdi-lock</v-icon>
                        Private
                    </v-list-item-subtitle>
                    <v-list-item-subtitle v-if="item.WSIsPrivate == 0">
                        <v-icon size="12">mdi-earth</v-icon>
                        Public
                    </v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-action>
                </v-list-item-action>
            </template>
        </v-autocomplete>
    </template>-->
                <v-spacer></v-spacer>

                <!--Workspace Details-->
                <v-menu v-if="currentWorkspaceID > 0 && isDatacollector== false"
                        :offset-y="true"
                        class="ml-2"
                        :close-on-content-click="false">
                    <template v-slot:activator="{ on }">
                        <v-btn v-on="on"
                               text
                               class="mr-3"
                               width="auto"
                               max-width="300"
                               height="55"
                               @click="fnGetMyWorkspaceList(), fnGetSharedWorkspaceList()">
                            <v-avatar size="40"
                                      tile
                                      class="elevation-1 base-border-radius mr-2"
                                      light
                                      :color="`${getcolor(currentWSName)}`">
                                <v-img v-bind:src="currentWSImage"
                                       height="40"
                                       width="40"
                                       v-if="currentWSImage.length > 0"></v-img>
                                <span class="headline white--text" v-else="">
                                    {{
                    currentWSName == null
                      ? ""
                      : currentWSName.substring(0, 1).toUpperCase()
                                    }}
                                </span>
                            </v-avatar>
                            <p class="text-left mt-1 d-none d-sm-block"
                               style="line-height: 17px">
                                <span class="text-limit mt-4 mb-0"
                                      style="max-width: 250px; display: block">
                                    {{ currentWSName }}
                                </span>
                                <small>
                                    <span v-show="currentWSIsPrivate == 0">
                                        <v-icon size="12">mdi-earth</v-icon> public
                                    </span>
                                    <span v-show="currentWSIsPrivate == 1">
                                        <v-icon size="12">mdi-lock</v-icon> Private
                                    </span>
                                </small>
                            </p>
                        </v-btn>
                    </template>
                    <v-card class="mx-auto" max-width="320" min-width="290">
                        <v-list dense class="">
                            <v-list-item>
                                <v-list-item-content>
                                    <h6 class="text-center">
                                        <v-avatar size="60"
                                                  light
                                                  tile
                                                  :color="`${getcolor(currentWSName)}`"
                                                  class="elevation-1 base-border-radius">
                                            <v-img v-bind:src="currentWSImage"
                                                   height="60"
                                                   width="60"
                                                   v-if="currentWSImage.length > 0"></v-img>
                                            <span class="headline white--text" v-else="">
                                                {{
                          currentWSName == null
                            ? ""
                            : currentWSName.substring(0, 1).toUpperCase()
                                                }}
                                            </span>
                                        </v-avatar>
                                    </h6>
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item>
                                <v-list-item-content class="mt-0" style="cursor: default">
                                    <v-list-item-title class="headline py-2 text-center text-limit">{{ currentWSName }}</v-list-item-title>
                                    <v-list-item-subtitle v-if="currentWSIsPrivate"
                                                          class="text-center">
                                        <v-icon size="12">mdi-lock</v-icon>Private
                                    </v-list-item-subtitle>
                                    <v-list-item-subtitle v-else class="text-center">
                                        <v-icon size="12">mdi-earth</v-icon>Public
                                    </v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item>
                                <v-list-item-content>
                                    <h6 class="text-center">
                                        <v-btn depressed
                                               color="#3374B9"
                                               dark
                                               link
                                               @click="fnManage()"
                                               :to="{
                        name: 'WorkspaceManage',
                        query: { WID: currentWorkspaceID },
                      }">Manage Workspace</v-btn>
                                    </h6>
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item style="min-height: 15px">
                                <v-list-item-content>
                                    <v-divider></v-divider>
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item>
                                <v-tabs color="#52B962" centered>
                                    <v-tab href="#tab-4">Personal</v-tab>
                                    <v-tab href="#tab-5">Shared</v-tab>

                                    <!-- workspace Tab Items -->
                                    <v-tab-item class="mt-1" value="tab-4">
                                        <v-card flat height="20vh" class="v-scrolling-div">
                                            <v-card-text class="pt-0">
                                                <!--<v-container fluid class="pt-0">-->
                                                <v-row>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-row no-gutters>
                                                            <v-list style="max-width: 275px">
                                                                <v-list-item-group v-model="PersonalWorkspace"
                                                                                   color="primary">
                                                                    <v-list-item v-for="(
                                      PersonalWorkspace, i
                                    ) in MyWorkspaceList"
                                                                                 :key="i"
                                                                                 @click="fnEnterWorkspace(PersonalWorkspace.WorkspaceID)">
                                                                        <v-avatar :color="`${getcolor(PersonalWorkspace.WSName)}`" class="my-2 mr-5" size="40">
                                                                            <v-img v-bind:src="imageUrl + PersonalWorkspace.WorkSpaceImage" height="40" width="40" class="ma-4" v-if="PersonalWorkspace.WorkSpaceImage.length>0"></v-img>
                                                                            <v-span class="headline white--text text-center" v-else>{{PersonalWorkspace.WSName.charAt(0)}}</v-span>
                                                                        </v-avatar>

                                                                        <v-list-item-content style="cursor: default">
                                                                            <v-list-item-title v-text="PersonalWorkspace.WSName"></v-list-item-title>
                                                                            <v-list-item-subtitle v-if="
                                          PersonalWorkspace.WSIsPrivate == 1
                                        ">
                                                                                <v-icon size="12">mdi-lock</v-icon>Private
                                                                                <v-icon size="12">mdi-account</v-icon>{{ PersonalWorkspace.WSUsers.length }}
                                                                            </v-list-item-subtitle>
                                                                            <v-list-item-subtitle v-if="
                                          PersonalWorkspace.WSIsPrivate == 0
                                        ">
                                                                                <v-icon size="12">mdi-lock</v-icon>
                                                                                Public
                                                                                <v-icon size="12">mdi-account</v-icon>{{ PersonalWorkspace.WSUsers.length }}
                                                                            </v-list-item-subtitle>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </v-list-item-group>
                                                            </v-list>
                                                        </v-row>
                                                    </v-col>
                                                </v-row>
                                            </v-card-text>
                                        </v-card>
                                    </v-tab-item>
                                    <!-- Shared Workspace tab-->
                                    <v-tab-item class="mt-1" value="tab-5">
                                        <v-card flat height="20vh" class="v-scrolling-div">
                                            <v-card-text class="pt-0">
                                                <!--<v-container fluid class="pt-0">-->
                                                <v-row no-gutters>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <v-container fluid>
                                                            <v-row>
                                                                <v-list style="max-width: 275px">
                                                                    <v-list-item-group v-model="SharedWorkspace"
                                                                                       color="primary">
                                                                        <v-list-item v-for="(
                                        SharedWorkspace, i
                                      ) in SharedWorkspaceList"
                                                                                     :key="i"
                                                                                     @click="fnEnterWorkspace(SharedWorkspace.WorkspaceID)">
                                                                            <v-avatar :color="`${getcolor(SharedWorkspace.WSName)}`" class="mr-5 my-2" size="40">
                                                                                <v-img v-bind:src="imageUrl + SharedWorkspace.WorkSpaceImage" height="40" width="40" class="ma-4" v-if="SharedWorkspace.WorkSpaceImage.length>0"></v-img>
                                                                                <v-span class="headline white--text text-center" v-else>{{SharedWorkspace.WSName.charAt(0)}}</v-span>
                                                                            </v-avatar>

                                                                            <v-list-item-content style="cursor: default">
                                                                                <v-list-item-title v-text="SharedWorkspace.WSName"></v-list-item-title>
                                                                                <v-list-item-subtitle v-if="
                                            SharedWorkspace.WSIsPrivate == 1
                                          ">
                                                                                    <v-icon size="12">mdi-lock</v-icon>Private
                                                                                    <v-icon size="12">mdi-account</v-icon>{{ SharedWorkspace.WSUsers.length }}
                                                                                </v-list-item-subtitle>
                                                                                <v-list-item-subtitle v-if="
                                            SharedWorkspace.WSIsPrivate == 0
                                          ">
                                                                                    <v-icon size="12">mdi-lock</v-icon>
                                                                                    Private
                                                                                    <v-icon size="12">mdi-account</v-icon>{{ SharedWorkspace.WSUsers.length }}
                                                                                </v-list-item-subtitle>
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list-item-group>
                                                                </v-list>
                                                            </v-row>
                                                        </v-container>
                                                    </v-col>
                                                </v-row>
                                                <!--</v-container>-->
                                            </v-card-text>
                                        </v-card>
                                    </v-tab-item>
                                </v-tabs>
                            </v-list-item>

                            <!-- Buttons -->
                            <v-list-item>
                                <v-list-item-content>
                                    <v-row>
                                        <v-col>
                                            <v-btn small
                                                   color="success-btn elevation-0 enter-btn"
                                                   block
                                                   dark
                                                   @click="fnEnter()">
                                                <v-icon size="20" class="mr-2">mdi-location-enter</v-icon>
                                                View All
                                            </v-btn>
                                        </v-col>
                                        <v-col>
                                            <v-btn small
                                                   color="success-btn elevation-0"
                                                   class="manage-btn"
                                                   block
                                                   dark
                                                   @click="fnManage()"
                                                   link
                                                   :to="{ name: 'MyWorkspace', query: { WSdialog: true } }">
                                                <v-icon size="20" class="mr-2">mdi-plus</v-icon>
                                                Create New
                                            </v-btn>
                                        </v-col>
                                    </v-row>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-card>
                </v-menu>

                <!-- Home Button -->
                <v-avatar size="25" class="mr-3" @click="fnEnter()"  v-if="isDatacollector== false">
                    <v-icon color="#707070" size="20">icon-home_im_ic</v-icon>
                </v-avatar>
                <!--<v-badge color="quaternary-bg-light-4" class="header-badge">

            </v-badge>-->
                <!-- Notification Button -->
                <!--<v-menu offset-y transition="scroll-y-transition" left>-->
                <!-- Bell Icon-->
                <!--<template v-slot:activator="{ on }">
            <v-badge overlap=""
                     color="quaternary-bg-light-4"
                     class="header-badge mr-3"
                     v-on="on">
                <template v-slot:badge="">
                    <span class="black--text">2</span>
                </template>
                <v-avatar v-on="on" link size="25">
                    <v-icon color="#707070" size="20">icon-notification_im_ic</v-icon>
                </v-avatar>
            </v-badge>
        </template>
        <v-list color="secondary" dark width="auto" max-width="250">
            <v-list-item link>
                <v-list-item-content> Notification 1 </v-list-item-content>
            </v-list-item>
            <v-list-item link>
                <v-list-item-content> Notification-2 </v-list-item-content>
            </v-list-item>
        </v-list>
    </v-menu>-->
                <!--Help icon-->
                <!--<v-avatar size="25" class="mr-3">
        <v-icon color="#707070" size="20">icon-help_im_ic</v-icon>
    </v-avatar>-->
                <!--Profile Image-->
                <v-menu :offset-y="true" class="ml-2" v-model="ProfileMenu">
                    <template v-slot:activator="{ on }">
                        <v-avatar v-on="on"
                                  size="40"
                                  class="mr-3"
                                  light
                                  v-bind:color="RandomColor">
                            <v-img v-bind:src="uploadedImage"
                                   height="40"
                                   width="40"
                                   class="ma-4"
                                   v-if="uploadedImage.length > 0"></v-img>
                            <span class="headline white--text" v-else>
                                {{
                LoggedUserDetails.FirstName == null
                  ? ""
                  : LoggedUserDetails.FirstName.substring(0, 1).toUpperCase()
                                }}
                            </span>
                        </v-avatar>
                    </template>
                    <v-list color="white"
                            light
                            width="auto"
                            max-width="auto"
                            class="action-button-list">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-avatar size="50"
                                          class="mr-2"
                                          light
                                          v-bind:color="RandomColor">
                                    <v-img v-bind:src="uploadedImage"
                                           height="50"
                                           width="50"
                                           class="ma-4"
                                           v-if="uploadedImage.length > 0"></v-img>
                                    <span class="headline white--text" v-else>
                                        {{
                    LoggedUserDetails.FirstName == null
                      ? ""
                      : LoggedUserDetails.FirstName.substring(
                          0,
                          1
                        ).toUpperCase()
                                        }}
                                    </span>
                                </v-avatar>
                            </v-list-item-icon>
                            <v-list-item-content>
                                <p class="text-medium">{{ LoggedUserDetails.Name }}</p>
                                <h1 style="font-size: 10px; color: #707070">
                                    {{ LoggedUserDetails.EmailId }}
                                </h1>
                            </v-list-item-content>
                        </v-list-item>
                        <!--<v-list-item link>
                <v-list-item-icon>
                    <v-icon>mdi-pencil-outline</v-icon>
                </v-list-item-icon>
                <v-list-item-content @click.stop="(EditAccountUserDailog = true), ChangeTab(1)"
                                     style="font-size: 12px">
                    Edit Account
                </v-list-item-content>
            </v-list-item>-->
                        <v-list-item link @click="">
                            <v-list-item-icon>
                                <v-icon size="20">mdi-tune</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content style="font-size: 12px"
                                                 @click.stop="(EditAccountUserDailog = true), ChangeTab(1)">
                                Account Settings
                            </v-list-item-content>
                        </v-list-item>
                        <!--<v-list-item link @click="">
                <v-list-item-icon>
                    <v-icon size="20">mdi-help-circle-outline</v-icon>
                </v-list-item-icon>
                <v-list-item-content style="font-size: 12px">
                    Support
                </v-list-item-content>
            </v-list-item>-->
                        <v-list-item link @click="fnCheckLogout()">
                            <v-list-item-icon>
                                <v-icon size="20">mdi-logout</v-icon>
                            </v-list-item-icon>
                            <v-list-item-content style="font-size: 12px">
                                Logout
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>

                    <!-- Edit Account User Dailog Starts Here -->
                    <v-dialog v-model="EditAccountUserDailog" width="800">
                        <v-card>
                            <v-card-title primary-title class="page-head pt-4">
                                <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                    <v-icon size="20" color="primary">mdi-account-edit-outline</v-icon>
                                </v-avatar>
                                Profile Details<v-spacer></v-spacer>
                                <v-btn text
                                       @click="
                    (EditAccountUserDailog = false),
                      ChangeTab(1),
                      (ProfileMenu = false),
                      fnGetLoggedUserDetails()
                  ">
                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                </v-btn>
                            </v-card-title>
                            <v-tabs centered v-model="Tab">
                                <v-tab href="#tab-1"> Details </v-tab>
                                <!--<v-tab href="#tab-2">Activity</v-tab>-->
                                <v-tab href="#tab-3">Settings</v-tab>
                                <!-- Details Tab Items -->
                                <v-tab-item value="tab-1">
                                    <v-card flat height="100%">
                                        <v-card-text class="pt-0">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-container fluid>
                                                        <v-row no-gutters
                                                               class="v-scrolling-div px-4"
                                                               style="height: 50vh">
                                                            <v-col cols="12" md="12" class="text-center">
                                                                <v-avatar link=""
                                                                          size="60"
                                                                          class="elevation-3 mr-2"
                                                                          light
                                                                          v-bind:color="RandomColor">
                                                                    <!-- <input type="image" id="idProfileImg" v-bind:src="uploadedImage" height="50" width="50" />-->
                                                                    <v-img v-bind:src="profileeditImage"
                                                                           height="60"
                                                                           width="60"
                                                                           v-if="profileeditImage.length > 0"></v-img>
                                                                    <span class="headline white--text" v-else>
                                                                        {{
                                      LoggedUserDetails.FirstName == null
                                        ? ""
                                        : LoggedUserDetails.FirstName.substring(
                                            0,
                                            1
                                          ).toUpperCase()
                                                                        }}
                                                                    </span>
                                                                    <div class="dp-edit-btn">
                                                                        <input class="upload"
                                                                               type="file"
                                                                               id="file"
                                                                               hide-details
                                                                               accept="image/*"
                                                                               @change="previewImage" />
                                                                    </div>
                                                                </v-avatar>
                                                            </v-col>
                                                            <v-col cols="12" xs="12" sm="12" md="12">
                                                                <v-row :justify="justifyCenter">
                                                                    <v-col md="6" class="pb-0">
                                                                        <v-text-field outlined=""
                                                                                      dense=""
                                                                                      label="First Name"
                                                                                      type="text"
                                                                                      v-model="LoggedUserDetails.FirstName"></v-text-field>
                                                                        <v-text-field outlined=""
                                                                                      dense=""
                                                                                      label="Last Name"
                                                                                      type="text"
                                                                                      v-model="LoggedUserDetails.LastName"></v-text-field>
                                                                        <v-text-field disabled
                                                                                      outlined=""
                                                                                      dense=""
                                                                                      label="Email"
                                                                                      type="text"
                                                                                      v-model="LoggedUserDetails.EmailId"></v-text-field>
                                                                        <v-text-field outlined=""
                                                                                      dense=""
                                                                                      prefix="+91"
                                                                                      label="Phone"
                                                                                      type="number"
                                                                                      v-model="LoggedUserDetails.Phone"
                                                                                      onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                                                      onKeyPress="if(this.value.length==10) return false;"
                                                                                      min="0"></v-text-field>
                                                                        <v-text-field outlined=""
                                                                                      dense=""
                                                                                      label="Organization"
                                                                                      type="text"
                                                                                      v-model="LoggedUserDetails.Organization"></v-text-field>
                                                                        <v-textarea rows="3"
                                                                                    outlined=""
                                                                                    dense=""
                                                                                    label="Address"
                                                                                    type="text"
                                                                                    v-model="LoggedUserDetails.Address">
                                                                        </v-textarea>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row :justify="justifyCenter">
                                                            <v-col md="6">
                                                                <v-btn centered
                                                                       color="success-btn elevation-0"
                                                                       block
                                                                       @click="fnUpdateLoggedUserDetails()">Save</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                    </v-card>
                                </v-tab-item>

                                <!-- Activity Tab Items -->
                                <!--<v-tab-item value="tab-2" height="100%">
                        <v-card flat height="100%">
                            <v-card-text class="pt-0">
                                <v-row no-gutters>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <v-container fluid>
                                            <v-row no-gutters>
                                                <v-list>
                                                    <v-list-item-group v-model="item"
                                                                       color="primary">
                                                        <v-list-item v-for="(item, i) in items"
                                                                     :key="i">
                                                            <v-list-item-icon>
                                                                <v-icon v-text="item.icon"></v-icon>
                                                            </v-list-item-icon>
                                                            <v-list-item-content>
                                                                <v-list-item-title v-text="item.text"></v-list-item-title>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </v-list-item-group>
                                                </v-list>
                                            </v-row>
                                        </v-container>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                        </v-card>
                    </v-tab-item>-->
                                <!-- Settings Tab -->
                                <v-tab-item value="tab-3">
                                    <v-card flat>
                                        <v-card-text class="pt-0">
                                            <v-row no-gutters>
                                                <v-col cols="12" md="12" class="pt-0">
                                                    <v-row :justify="justifyCenter">
                                                        <v-col md="6">
                                                            <v-container fluid>
                                                                <v-expansion-panels centerd>
                                                                    <!-- Notification preference-->
                                                                    <!--<v-expansion-panel>
                                                            <v-expansion-panel-header centerd>
                                                                Notification
                                                                Preferences
                                                            </v-expansion-panel-header>
                                                            <v-expansion-panel-content>
                                                                <v-col cols="12" sm="6" md="12">
                                                                    <v-radio-group column>
                                                                        <v-radio label="I've been assigned to a task"
                                                                                 color="#9B9B9B"
                                                                                 value="1"></v-radio>
                                                                        <v-radio label="My task have been rescheduled"
                                                                                 color="#9B9B9B"
                                                                                 value="2"></v-radio>
                                                                        <v-radio label="My task had progress updated"
                                                                                 color="#9B9B9B"
                                                                                 value="3"></v-radio>
                                                                        <v-radio label="I have task due tomorrow"
                                                                                 color="#9B9B9B"
                                                                                 value="4"></v-radio>
                                                                    </v-radio-group>
                                                                </v-col>
                                                                <v-col cols="12"
                                                                       xs="12"
                                                                       sm="12"
                                                                       md="12"
                                                                       class="text-center">-->
                                                                    <!--<v-btn centered color="secondary" class="mr-2 elevation-0" @click="">Save</v-btn>-->
                                                                    <!--<v-btn centered
                                                                           color="success-btn elevation-0 enter-btn"
                                                                           class="mr-2"
                                                                           dark
                                                                           block
                                                                           @click="">Save</v-btn>
                                                                </v-col>
                                                            </v-expansion-panel-content>
                                                        </v-expansion-panel>-->
                                                                    <!-- Change password expansion panel-->
                                                                    <v-expansion-panel>
                                                                        <v-expansion-panel-header centerd>Change Password</v-expansion-panel-header>
                                                                        <v-expansion-panel-content centered>
                                                                            <v-row :justify="justifyCenter">
                                                                                <v-col cols="12" sm="6" md="12">
                                                                                    <v-text-field v-model="OldPassword"
                                                                                                  :append-icon="
                                              show1 ? 'mdi-eye' : 'mdi-eye-off'
                                            "
                                                                                                  :type="show1 ? 'text' : 'password'"
                                                                                                  name="input-10-1"
                                                                                                  label="Enter old Password"
                                                                                                  autocomplete="new-password"
                                                                                                  required
                                                                                                  @click:append="show1 = !show1"></v-text-field>
                                                                                    <v-text-field v-model="NewPassword"
                                                                                                  :append-icon="
                                              show2 ? 'mdi-eye' : 'mdi-eye-off'
                                            "
                                                                                                  :type="show2 ? 'text' : 'password'"
                                                                                                  :rules="[passwordRules]"
                                                                                                  autocomplete="new-password"
                                                                                                  required
                                                                                                  name="input-10-1"
                                                                                                  label="Enter new Password"
                                                                                                  @click:append="show2 = !show2"></v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-col cols="12"
                                                                                   xs="12"
                                                                                   sm="12"
                                                                                   md="12"
                                                                                   class="text-center">
                                                                                <v-btn v-bind:disabled="
                                            isValidPassword == false
                                          "
                                                                                       centered
                                                                                       color="success-btn elevation-0 enter-btn"
                                                                                       class="mr-2"
                                                                                       block
                                                                                       @click="ChangePassword">Save</v-btn>
                                                                            </v-col>
                                                                        </v-expansion-panel-content>
                                                                    </v-expansion-panel>
                                                                </v-expansion-panels>
                                                            </v-container>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>
                                    </v-card>
                                </v-tab-item>
                            </v-tabs>
                        </v-card>
                    </v-dialog>
                    <!-- Before Logout Confirmation Message-->
                    <v-dialog v-model="warninglogoutdailog" width="800" persistent="">
                        <v-card>
                            <v-card-title primary-title="" class="page-head py-4">
                                <v-avatar size="27" color="#F2C64D" class="elevation-0">
                                    <v-icon size="20" color="#fff">mdi-exclamation</v-icon>
                                </v-avatar>
                                Alert
                                <v-spacer></v-spacer>
                                <v-btn id="AutoTest_U116" text="" @click="warninglogoutdailog=false">
                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                </v-btn>
                            </v-card-title>
                            <v-card-text class="pt-0">
                                <v-container fluid="" class="pt-0">
                                    <v-row no-gutters="">
                                        <v-col cols="12" md="12" class="pt-0">
                                            <v-container fluid="" class="pt-0">
                                                <v-row>
                                                    <v-col cols="12" md="12" class="pt-0">
                                                        <span class="message">
                                                            <h4 class="text-center heading-3">Saved Task data exists,If you continue to logout saved task data will be clear</h4>
                                                            <h4 class="text-center heading-3">Do you want to continue?</h4>
                                                        </span>
                                                    </v-col>
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                        <Secondary-Button id="AutoTest_U117" class="btn-122x36 mr-4" title="Yes" @click.native="warninglogoutdailog = false,logout()"></Secondary-Button>
                                                        <Default-Button-Outlined id="AutoTest_U118" class="btn-122x36" title="No" @click.native="warninglogoutdailog = false"></Default-Button-Outlined>
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
            </v-app-bar>

            <!-- Router view -->
            <v-content>
                <v-container fluid :style="[$route.name == 'home' ? { 'background-color': '#fafafa'} : { 'background-color': '#fff'}]">
                    <v-row align="center" justify="center">
                        <router-view :key="viewKey" />
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
                    <v-snackbar v-model="syncSnackbarMsg"  top :timeout="timeout" centered color="blue-grey darken-3" :max-height="30" class="pa-0">
                        <template>
                            <v-avatar size="25" outlined style="border: 1px solid white;background-color:darkgrey">
                                    <v-icon color="white" size="15">mdi-file</v-icon>
                            </v-avatar>
                            <p class="pa-2 ma-0" >{{SyncMsg}} {{":  " + timer +" secs"}}</p>
                            <v-btn  text @click.stop="syncSnackbarMsg = false">
                                    <v-icon color="white" size="25">mdi-close</v-icon>
                                </v-btn>
                        </template>
                    </v-snackbar>
                    <v-snackbar v-model="syncPendingMsg"  top="" :timeout="3000" centered color="blue-grey darken-3 " :max-height="30" class="pa-0">
                        <template>
                            <v-avatar size="25" outlined style="border: 1px solid white;background-color:darkgrey">
                                    <v-icon color="white" size="15">mdi-file</v-icon>
                            </v-avatar>
                            <p class="pa-2 ma-0" >Synchronization Pending</p>
                            <v-btn  text @click.stop="syncPendingMsg = false">
                                    <v-icon color="white" size="25">mdi-close</v-icon>
                                </v-btn>
                        </template>
                    </v-snackbar>
                    <v-snackbar id="offlineSB" v-model="syncOfflineMsg" top centered color="red">
                        <v-icon class="icon_avatar" color="white" size="16">mdi-alert</v-icon>
                        <p class="pa-2 ma-0" >{{offlineMsg}}</p>
                        <v-btn  text @click.stop="syncOfflineMsg = false">
                            <v-icon color="white" size="25">mdi-close</v-icon>
                        </v-btn>
                    </v-snackbar>
                </v-container>
            </v-content>

            <!-- footer view -->
            <!--
            <v-footer v-if="IsAuthenticated" fixed=""
                      class="font-weight-medium">
                <v-col cols="12">
                    <p class="footer-p">
                        Quick Access
                        <router-link class="ml-4" to=""><v-icon class="mr-2">mdi-message-text-outline</v-icon><span class="text-link">Feedback</span></router-link>
                        <router-link class="ml-4" to=""><v-icon class="mr-2">mdi-help-circle-outline</v-icon><span class="text-link">Help Desk</span></router-link>
                    </p>
                </v-col>
            </v-footer>
            -->
        </v-app>
    </div>
</template>


<style lang="scss">
    @import "src/styles/global.scss";

    #offlineSB {
        .v-snack__wrapper {
            min-width: fit-content;
            max-height: 40px;
            
            .icon_avatar {
                border: 1px solid white;
                background-color:darkgrey;
                border-radius: 50%;
                padding: 2px;
            }

            button {
                margin-left: 0px !important
            }
        }
    }
</style>

<script>
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import { IsAuthenticated } from "./globals.js";
    import syncData from "./ImworksMobApp.js";
    import { dbImworks } from "./ImworksDb";
    import PreLoader from "@/components/pre-loader.vue";
    import {mapGetters} from 'vuex'
    var usr;
    async function importscript() {
        return Promise.all([
            import("./BL/User.js").then(mod => {
                usr = new mod.User();
            })
        ]);
    }
    export default {
        data: function () {
            return {
                SyncMsg: "",
                timeout: 50000,
                RunTimerID: null,
                timer: 55,
                syncSnackbarMsg: false,
                warninglogoutdailog: false,
                viewKey: 1,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                name: "MasterPage",
                IsAuthenticated: false,
                drawer: null,
                mini: true,
                EditAccountUserDailog: false,
                justifyCenter: "center",
                item: 1,
                items: [
                    {
                        text: "Prasant Joseph started impact  Ind Project",
                        icon: "mdi-clock",
                    },
                    {
                        text: "Prasant Joseph completed impact  Ind Project 25% only",
                        icon: "mdi-account",
                    },
                    {
                        text:
                            "Prasant Joseph completed impact  Ind Project 70% only and marked 30% left to be completed",
                        icon: "mdi-flag",
                    },
                ],

                PersonalWorkspace: 1,
                SharedWorkspace: 1,
                OldPassword: "",
                NewPassword: "",
                isValidPassword: false,
                passwordRules: (value) => {
                    const pattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,14})/;
                    if (pattern.test(value)) {
                        this.isValidPassword = true;
                        return true;
                    } else {
                        this.isValidPassword = false;
                        return "Password must be  between 8 and 14 characters with at least one capital letter, a number and a special character.";
                    }
                },
                show1: false,
                show2: false,
                Tab: "Tab-1",
                // For Search
                isLoading: false,
                model: null,
                search: null,
                WorkspaceList: [],
                currentWorkspaceID: 0,
                currentWSName: "",
                currentWSIsPrivate: false,
                currentWSImage: "",
                LoggedUserDetails: {
                    ID: 0,
                    Name: "",
                    FirstName: "",
                    LastName: "",
                    EmailId: "",
                    Phone: 0,
                    Organization: "",
                    Address: "",
                    ProfileImage: "",
                },
                MyWorkspaceList: [],
                SharedWorkspaceList: [],

                clearable: false,
                Search: null,
                ProfileMenu: false,
                uploadedImage: "",
                profileeditImage: "",
                randomColorarray: [
                    "#00B448",
                    "#399CFF",
                    "#2C4258",
                    "#00C2B0",
                    "#6F00B0",
                    "#8307CB",
                ],
                /* ['#87556f', '#206a5d', '#ff5f40', '#438a5e', '#687466'], */
                RandomColor: "",
                imageUrl: "",
                showDashBoard: false,
                DashBoardAccess: 0,
                showInitiatives: false,
                InitiativesAccess: 0,
                showUsers: false,
                UsersAccess: 0,
                showProjects: false,
                ProjectsAccess: 0,
                showMyFinance: false,
                MyFinanceAccess: 0,
                showTasks: false,
                TasksAccess: 0,
                showMyTasks: true,
                MyTasksAccess: 0,
                showWorkflows: false,
                WorkflowsAccess: 0,
                showSettings: false,
                SettingsAccess: 0,
                showVenture: false,
                VentureAccess: 0,
                PreLoader: false,
                showReport: false,
                ReportAccess: 0,
                report_active: require("@/assets/report_asset/ic_reports_active.svg"),
                report_inactive: require("@/assets/report_asset/ic_reports_inactive.svg"),
                //events :['click', 'mousemove','mousedown', 'scroll', 'keypress','load'],
                warningTimer: null,
                logoutTimer: null,
                SyncToServerTimer: null,
                SetTimer: 0,
                syncPendingMsg: false,
                isOnline: navigator.onLine,
                offlineMsg: null,
                syncOfflineMsg: false,
                isDatacollector: false,
            };
        },
        computed: {
            ...mapGetters('SyncBinders',[
                'initState'
            ]),
            ...mapGetters('App', ["authState", "_loader"])
        },
        mounted: async function () {
            //var isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') ;
            //if(isChrome==-1) //!window.chrome
            //{
            //    alert('You are not using chrome. We recommend that you use the latest version of Google Chrome.');
            //}
            if (/Android/.test(navigator.userAgent) && /Chrome/.test(navigator.userAgent)) {
                console.log('Chrome browser on Android');
            }
            else if (/iPhone/.test(navigator.userAgent) && /CriOS/.test(navigator.userAgent)) {
                console.log('Chrome browser on iPhone');
            }
            else if (/Chrome/.test(navigator.userAgent)) {
                console.log('Chrome browser on web');
            } else {
                alert('You are not using chrome. We recommend that you use the latest version of Google Chrome.');
                return false;
            }

            // to redirect to index.html'
            if (!window.location.href.includes("index.html")) {
                var newURL = "";
                // alert(window.location.href);
                if (window.location.href.includes("/app")) {
                    newURL = window.location.href.replace("#/", "").replace("/app", "/app/index.html#");
                }else if (window.location.href.includes("app/")) {
                    newURL = window.location.href.replace("#/", "").replace("app/", "app/index.html#");
                } 
                else {
                    newURL = window.location.href.replace("#/", "app/index.html#");
                }

                window.location.href = newURL;
            }

            await importscript();
            this.imageUrl = window.SERVER_URL + "/Upload";
            this.RandomColor = this.randomColorarray[Math.floor(Math.random() * 5)];
            sessionStorage.setItem("RandomColor", this.RandomColor);


            //this.events.forEach(function (event){
            //        window.addEventListener(event, this.resetTimer);
            //}, this);
            //this.setTimers();
            window.UserUrlpath = window.location.href;//this.$route.from;
            window.BeforelogoutUrl = "";
            //await importscript();
            //await importscriptWorkspace();
            if (!(window.location.href.toLowerCase().includes('surveysharelink'))) // if not sharelink
            {
                setInterval(async () => {
                    if (this.SetTimer == 0) {

                     var loggedUser = 0;
                     var synServerId = 0;

                     var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();
                        if (data != null && data.length > 0) {
                            loggedUser = data[0].UserId;
                        }


                    var lastfetch = await dbImworks.TaskSettings.where("UserId").equals(parseInt(loggedUser)).toArray();
                    if (lastfetch != null && lastfetch.length > 0) {
                        synServerId = lastfetch[lastfetch.length - 1].ServerPKID;
                     }

                     if(synServerId >0 || this.initState) {
                            this.GetMyTaskFromServer();
                        }
                    }
                }, 100000);
                // setInterval(() => {
                // this.UpdateMyTaskToServer();
                //}, 50000);
                this.SyncToServerTimer = setInterval(() => { this.UpdateMyTaskToServer() }, 50000);
                setInterval(() => {
                    this.runSnackBar()
                    this.timer--
                    if (this.timer <= 0) {
                        this.timer = 0;
                        this.syncSnackbarMsg = false;
                        this.toastTimer = 55
                    }
                },1000)
            }
            // to update the variable isOnline by checking internet connectivity

            window.addEventListener('online', () => this.isOnline = true)
            window.addEventListener('offline', ()=> this.isOnline = false)
        },
        destroyed() {
            this.events.forEach(function (event) {
                window.removeEventListener(event, this.resetTimer);
            }, this);
            this.resetTimer();
        },
        components: {
            "Pre-Loader": PreLoader,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
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

            // Auto logout functions
            setTimers: function () {
                this.warningTimer = setInterval(() => { this.warningMessage(); }, 7 * 24 * 60 * 60 * 1000);//setTimeout(this.warningMessage , 7*24*60*60*1000); // 7 days == 7*24*60*60*1000;
                this.logoutTimer = setInterval(() => { this.logoutUser(); }, (7 * 24 * 60 * 60 * 1000 + 60000));//setTimeout(this.logoutUser , (7*24*60*60*1000 + 60000)); // 7 days +1 minutes == 15*60*1000;
                this.Msg = "";
                this.infoSnackbarMsg = false;
            },
            warningMessage: function () {
                //debugger;
                if (this.$route.name != "login") {
                    this.Msg = "Are you still with us?";
                    this.infoSnackbarMsg = true;
                } else {
                    this.resetTimer();
                }
            },

            async checkSyncDataQueue() {
                var SyncDataQueue = await dbImworks.TaskSyncDB.toArray();
                //console.log(SyncDataQueue.length);
                if (SyncDataQueue.length > 0) {
                    this.successSnackbarMsg = false;
                } else {
                    this.syncSnackbarMsg = false;
                    clearInterval(this.RunTimerID);
                    this.timer = 50;
                    this.Msg = "Data Synced";
                    this.successSnackbarMsg = true;
                }
            },

            logoutUser: function () {

                // inserting the Current logout before logout into a local storage as Json
                var JsonUrl = [];
                JsonUrl.push({ UserID: this.LoggedUserDetails.ID, Url: window.location.href });
                window.BeforelogoutUrl = JsonUrl;
                this.logout();
                this.resetTimer();

            },

            resetTimer: function () {
                clearTimeout(this.warningTimer);
                clearTimeout(this.logoutTimer);
                this.setTimers();

            },
            // Auto logout functions -- ends here

            async fnRouteToNewTaskPage() {
                localStorage.setItem("myTaskPageCount", 0);
                this.PreLoader = true;
                this.resetTimer();
                var lstTaskList = await dbImworks.MyTaskList.toArray();
                if (lstTaskList.length == 0) {
                    await this.GetMyTaskFromServer();
                    this.PreLoader = false;
                    this.$router.push({
                        name: "MyTasks",
                        query: { WsID: this.currentWorkspaceID, Access: this.TasksAccess },
                    });
                }
                else {
                    this.PreLoader = false;
                    this.$router.push({
                        name: "MyTasks",
                        query: { WsID: this.currentWorkspaceID, Access: this.TasksAccess },
                    });
                }
            },
            async fnRouteToMyTask() {
                this.PreLoader = true;
                this.resetTimer();
                var lstTaskList = await dbImworks.MyTaskList.toArray();
                if (lstTaskList.length == 0) {
                    await this.GetMyTaskFromServer();
                    this.PreLoader = false;
                    this.$router.push({
                        name: "OverdueTasks",
                        query: { WsID: this.currentWorkspaceID, Access: this.TasksAccess },
                    });
                } else {
                    this.PreLoader = false;
                    this.$router.push({
                        name: "OverdueTasks",
                        query: { WsID: this.currentWorkspaceID, Access: this.TasksAccess },
                    });
                }
            },
            async GetMyTaskFromServer() {
                var obj = syncData;
                await obj.syncData.SyncFromServer(()=>{
                    this.$store.commit('SyncBinders/updateSyncFromFlag')
                });
            },
            async UpdateMyTaskToServer() {
                // reset timer
                clearTimeout(this.SyncToServerTimer);
                var obj = syncData;
                this.SetTimer = 1;
                await obj.syncData.SyncToServer(()=> {
                    this.$store.commit('SyncBinders/updateSyncToFlag')
                });
                this.SetTimer = 0;
                this.SyncToServerTimer = setInterval(() => { this.UpdateMyTaskToServer(); }, 50000);
                this.timer = 55
                if(this.syncSnackbarMsg){
                    this.Msg = "Data Synced";
                    this.successSnackbarMsg = true;
                    this.syncSnackbarMsg = false
                }

            },
            async runSnackBar(){
                    var SyncDataQueue = await dbImworks.TaskSyncDB.toArray();
                    // console.log(SyncDataQueue)
                    if (SyncDataQueue.length > 0) {
                        if(!this.isOnline) {
                            this.syncSnackbarMsg = false;
                            this.offlineMsg = "Offline";//"You're Offline, Data will be synced once you're online";
                            this.syncOfflineMsg = true;
                        } else if(this.isOnline){
                            this.SyncMsg = "Synchronizing...";
                            this.syncSnackbarMsg= true;
                            return;
                        }
                    } else {
                        this.syncSnackbarMsg = false;
                    }
            },
            async fnEnterWorkspace(wsid) {

                this.PreLoader = true;
                await dbImworks.User.where("IsLoggedIn").equals(1).modify({ WorkspaceID: wsid });
                await dbImworks.TaskSettings.clear();
                await dbImworks.MyTaskList.clear();
                await dbImworks.TaskSyncDB.clear();
                await dbImworks.TbMyTask.clear();
                await dbImworks.SavedTaskDB.clear();

                await dbImworks.MyTaskDM.clear();
                await dbImworks.MyTaskSurveySection.clear();
                await dbImworks.MyTaskSurveyQuestion.clear();
                await dbImworks.MyTaskSurveySectionLogic.clear();
                await dbImworks.MyTaskSurveyTrackDate.clear();
                await dbImworks.TbMyTaskTable.clear();
                await dbImworks.MyTaskProjectSurveyQSTCondition.clear();
                await dbImworks.MyTaskProjectBenSurveyQSTCondition.clear();
                await dbImworks.MyTaskProjectSurveyQSTOption.clear();
                await dbImworks.MyTaskSurveyCascadingQuestionOptions.clear();
                await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.clear();
                await dbImworks.TbMyTaskProjectLogicConditions.clear();
                await dbImworks.TbMyTaskProjectBeneficiaryType.clear();
                await dbImworks.TbMyTaskBeneficiaryAnswerType.clear();
                await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.clear();
                await dbImworks.TbMyTaskProjectBeneficiary.clear();
                await dbImworks.MyTaskBeneficiaryTypeQuestion.clear();
                await dbImworks.MyTaskBeneficiaryTypeQuestionOption.clear();
                await dbImworks.MyTaskMasterList.clear();
                await dbImworks.MyTaskQuestionType.clear();
                await dbImworks.MyTaskProjectSurveyResponses.clear();

                this.PreLoader = false;
                //this.$router.go(this.$router.currentRoute);
                this.$router.push({ name: "home", query: { WsID: wsid } });
                this.viewKey += 1;

            },
            fnEnter() {
                this.currentWorkspaceID = 0;
                this.currentWSName = "";
                this.currentWSIsPrivate = false;
                this.$router.push({ name: "workspace" });
            },
            fnManage() {
                this.currentWorkspaceID = 0;
                this.currentWSName = "";
                this.currentWSIsPrivate = false;
            },
            ChangeTab(index) {
                if (index == 1) {
                    this.Tab = "tab-1";
                }
                if (index == 2) {
                    this.Tab = "tab-2";
                }
                if (index == 3) {
                    this.Tab = "tab-3";
                }
            },
            runTimer() {
                if (this.timer <= 0) {
                    this.timer = 50;
                }
                this.RunTimerID = setInterval(() => {
                    this.checkSyncDataQueue();

                    this.timer--;
                    if (this.timer <= 0) {
                        this.timer = 50;
                        this.syncSnackbarMsg = false;
                    }
                }, 1000);
            },
            async logout() {
                localStorage.clear();
                sessionStorage.clear();

                //in case session storage not clearded using sessionStorage.clear();
                var sessionvar = sessionStorage.length;
                for (var i = 0; i < sessionvar; i++) {
                    sessionStorage.removeItem(sessionStorage.key(i));
                }

                var uId = parseInt(this.LoggedUserDetails.ID);

                //clearing all session storage and local storage

                var SyncDataQueue = await dbImworks.TaskSyncDB.toArray();

                if (SyncDataQueue.length > 0) {
                    this.syncPendingMsg = true
                    return
                } else {
                    this.syncPendingMsg = false;
                    //await dbImworks.User.where("UserId").equals(uId).delete();
                     dbImworks.User.clear();
                     dbImworks.TaskSettings.clear();
                     dbImworks.MyTaskList.clear();
                     dbImworks.TaskSyncDB.clear();
                     dbImworks.TbMyTask.clear();
                     dbImworks.SavedTaskDB.clear();
                     dbImworks.TbUpdateTaskData.clear();
                     dbImworks.TbTempResposeData.clear();
                     dbImworks.TbLoggedUserDetails.clear();
                     dbImworks.TbPageAccessList.clear();
                     dbImworks.TbDashboardProjectList.clear();
                     dbImworks.TbNewBeneficiary.clear();
                     dbImworks.TbAnyTimeData.clear();
                     dbImworks.TbAnyTimeTempData.clear();

                     dbImworks.MyTaskDM.clear();
                     dbImworks.MyTaskSurveySection.clear();
                     dbImworks.MyTaskSurveyQuestion.clear();
                     dbImworks.MyTaskSurveySectionLogic.clear();
                     dbImworks.MyTaskSurveyTrackDate.clear();
                     dbImworks.TbMyTaskTable.clear();
                     dbImworks.MyTaskProjectSurveyQSTCondition.clear();
                     dbImworks.MyTaskProjectBenSurveyQSTCondition.clear();
                     dbImworks.MyTaskProjectSurveyQSTOption.clear();
                     dbImworks.MyTaskSurveyCascadingQuestionOptions.clear();
                     dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.clear();
                     dbImworks.TbMyTaskProjectLogicConditions.clear();
                     dbImworks.TbMyTaskProjectBeneficiaryType.clear();
                     dbImworks.TbMyTaskBeneficiaryAnswerType.clear();
                     dbImworks.TbMyTaskBeneficiaryRegistrationDetails.clear();
                     dbImworks.TbMyTaskProjectBeneficiary.clear();
                     dbImworks.MyTaskBeneficiaryTypeQuestion.clear();
                     dbImworks.MyTaskBeneficiaryTypeQuestionOption.clear();
                     dbImworks.MyTaskMasterList.clear();
                     dbImworks.MyTaskQuestionType.clear();
                     dbImworks.MyTaskProjectSurveyResponses.clear();
                    // dbImworks.TbTaskAuditLog.clear();
                     dbImworks.TbTaskResponseDocument.clear();
                     dbImworks.TbTaskBenefRegDocument.clear();

                    window.UserUrlpath = "";
                    var url = window.SERVER_URL + "/UserAccount/Logout";
                    
                    let response = await fetch(url, {
                        method: "POST",
                        credentials: "include",
                    });

                    let result = await response.json();
                    if (result) {
                        //this.$router.push({ name: "login" });
                        this.$store.commit('App/setAuthState', false);
                        this.$router.push({ name: "login" });
                    }
                    sessionStorage.setItem("WorkspaceID", null);
                    this.currentWorkspaceID = 0;
                    this.currentWSName = "";
                    this.currentWSIsPrivate = false;
                    this.uploadedImage = '';

                }
            },
            async fnGetWorkspaceList() {
                var url = window.SERVER_URL + "/Workspace/GetAllWorkspaceList";
                var response = await fetch(url, {
                    method: "GET",
                    credentials: "include",
                });
                var result = await response.json();
                if (result.data) {
                    this.WorkspaceList = result.data;
                }
            },
            async fnGetLoggedUserDetails() {
                var loggeduserdata = await dbImworks.TbLoggedUserDetails.where("ID").equals(window.LoggedUserID).toArray();
                if (loggeduserdata.length == 0) {
                    var url = window.SERVER_URL + "/UserAccount/GetLogUser";

                    var response = await fetch(url, {
                        method: "GET",
                        credentials: "include",
                    });
                    var result = await response.json();
                    this.LoggedUserDetails = result.data;
                    if (result.data) {
                        await dbImworks.TbLoggedUserDetails.put({
                            ID: result.data.ID, Name: result.data.Name, FirstName: result.data.FirstName, LastName: result.data.LastName
                            , EmailId: result.data.EmailId, Phone: result.data.Phone, Organization: result.data.Organization, ProfileImageName: result.data.ProfileImageName
                        });
                    }
                }
                var loggeduserdata = await dbImworks.TbLoggedUserDetails.where("ID").equals(window.LoggedUserID).toArray();
                if (loggeduserdata != null && loggeduserdata.length > 0) {
                    this.LoggedUserDetails.ID = loggeduserdata[0].ID;
                    this.LoggedUserDetails.Name = loggeduserdata[0].Name;
                    this.LoggedUserDetails.FirstName = loggeduserdata[0].FirstName;
                    this.LoggedUserDetails.LastName = loggeduserdata[0].LastName;
                    this.LoggedUserDetails.EmailId = loggeduserdata[0].EmailId;
                    this.LoggedUserDetails.Phone = loggeduserdata[0].Phone;
                    this.LoggedUserDetails.Organization = loggeduserdata[0].Organization;
                    this.LoggedUserDetails.ProfileImage = loggeduserdata[0].ProfileImageName;
                    // this.LoggedUserDetails.Address = loggeduserdata[0].ID;
                }

                if (this.LoggedUserDetails.ProfileImage != "" && this.LoggedUserDetails.ProfileImage != null && this.LoggedUserDetails.ProfileImage != undefined) {
                    this.uploadedImage = window.SERVER_URL + "/Upload/" + this.LoggedUserDetails.ProfileImage;
                    this.profileeditImage = window.SERVER_URL + "/Upload/" + this.LoggedUserDetails.ProfileImage;
                }
            },
            async fnUpdateLoggedUserDetails() {
                this.ProfileMenu = false;

                if (this.LoggedUserDetails.FirstName.length == 0) {
                    this.Msg = "Please enter FirstName";
                    this.infoSnackbarMsg = true;

                    return;
                } else {
                    var input = document.getElementById("file");
                    var data = new FormData();
                    data.append("FirstName", this.LoggedUserDetails.FirstName);
                    data.append("LastName", this.LoggedUserDetails.LastName);
                    data.append("Phone", this.LoggedUserDetails.Phone);
                    data.append("Organization", this.LoggedUserDetails.Organization);
                    data.append("Id", this.LoggedUserDetails.ID);
                    data.append("location", "userprofile");

                    if (input != null && input != undefined) {
                        for (var i = 0; i < input.files.length; i++) {
                            data.append("ProfileImage", input.files[i]);
                        }
                    }
                    var url = window.SERVER_URL + "/UserAccount/UpdateLogUser";
                    var response = await fetch(url, {
                        method: "POST",
                        credentials: "include",
                        body: data,
                    });
                    var result = await response.json();
                    if ((result.status = "success")) {
                        this.Msg = "Successfully Updated";
                        this.successSnackbarMsg = true;
                    } else {
                        this.Msg = "Failed to Update";
                        this.errorSnackbarMsg = true;
                    }
                    this.EditAccountUserDailog = false;
                    await dbImworks.TbLoggedUserDetails.clear();
                    await this.fnGetLoggedUserDetails();
                    this.$router.go();
                }
            },
            async ChangePassword() {
                if (this.OldPassword != "" && this.NewPassword != "") {
                    var data = new FormData();
                    data.append("OldPassword", this.OldPassword);
                    data.append("NewPassword", this.NewPassword);
                    data.append("Id", this.LoggedUserDetails.ID);

                    var url = window.SERVER_URL + "/UserAccount/ChangePassword";
                    var response = await fetch(url, {
                        method: "POST",
                        credentials: "include",
                        body: data,
                    });
                    var result = await response.json();
                    if (result.status == "success") {
                        this.Msg = "Successfully Updated password";
                        this.successSnackbarMsg = true;
                        this.ProfileMenu = false;
                        this.EditAccountUserDailog = false;
                        this.OldPassword = "";
                        this.NewPassword = "";
                    } else {
                        this.Msg = "Failed to Update Password," + result.statusmessage;
                        this.errorSnackbarMsg = true;
                        this.OldPassword = "";
                        this.NewPassword = "";
                    }
                } else {
                    this.Msg = "please enter old password and new password.";
                    this.infoSnackbarMsg = true;
                }
            },

            async GetRoleBasedScreenList() {
                this.isDatacollector = false;
                this.showDashBoard = false;
                this.showUsers = false;
                this.showProjects = false;
                this.showTasks = false;
                this.showReport = false;
                var data = new FormData();
                data.append("WorkspaceID", this.currentWorkspaceID);
                if (this.currentWorkspaceID > 0) {
                    this.ResetMenu();
                    var url = window.SERVER_URL + "/UserRoles/GetRoleBasedScreenList";
                    var response = await fetch(url, {
                        method: "POST",
                        credentials: "include",
                        body: data,
                    });
                    var result = await response.json();
                    if (result.data.length > 0) {
                        this.screenList = result.data;
                        for (var i = 0; i < this.screenList.length; i++) {
                            switch (this.screenList[i].ScreenName) {
                                case "Dashboard":
                                    this.showDashBoard = true;
                                    this.DashBoardAccess = this.screenList[i].AccessId;
                                    break;
                                case "Initiatives":
                                    this.showInitiatives = true;
                                    this.InitiativesAccess = this.screenList[i].AccessId;
                                    break;
                                case "Users":
                                    this.showUsers = true;
                                    this.UsersAccess = this.screenList[i].AccessId;
                                    break;
                                case "Projects":
                                    this.showProjects = true;
                                    this.ProjectsAccess = this.screenList[i].AccessId;
                                    break;
                                case "My Finance":
                                    this.showMyFinance = true;
                                    this.MyFinanceAccess = this.screenList[i].AccessId;
                                    break;
                                case "Tasks":
                                    this.showTasks = true;
                                    this.TasksAccess = this.screenList[i].AccessId;
                                    if (this.screenList.length == 1) {
                                        this.isDatacollector = true;
                                    }
                                    break;

                                case "Venture":
                                    this.showVenture = true;
                                    this.VentureAccess = this.screenList[i].AccessId;
                                    break;
                                case "Settings":
                                    this.showSettings = true;
                                    this.SettingsAccess = this.screenList[i].AccessId;
                                    break;
                                case "Reports":
                                    this.showReport = true;
                                    this.ReportAccess = this.screenList[i].AccessId;
                                    break;
                            }
                        }
                    }
                }
            },
            async ResetMenu() {
                this.showDashBoard = false;
                this.DashBoardAccess = 0;
                this.showInitiatives = false;
                this.InitiativesAccess = 0;
                this.showUsers = false;
                this.UsersAccess = 0;
                this.showProjects = false;
                this.ProjectsAccess = 0;
                this.showMyFinance = false;
                this.MyFinanceAccess = 0;
                this.showTasks = false;
                this.TasksAccess = 0;
                this.showWorkflows = false;
                this.WorkflowsAccess = 0;
                this.showSettings = false;
                this.SettingsAccess = 0;
                this.showVenture = false;
                this.VentureAccess = 0;
            },
            async fnGetMyWorkspaceList() {
                var url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                var response = await fetch(url, {
                    method: "GET",
                    credentials: "include",
                });
                var result = await response.json();
                if (result.data.length > 0) {
                    this.MyWorkspaceList = result.data;
                }
            },
            async fnGetSharedWorkspaceList() {
                var url = window.SERVER_URL + "/Workspace/GetSharedWorkspaceList";
                var response = await fetch(url, {
                    method: "GET",
                    credentials: "include",
                });
                var result = await response.json();
                if (result.data.length > 0) {
                    this.SharedWorkspaceList = result.data;
                }
            },
            // Check validations and condtions before logout
            async fnCheckLogout() {
                if (await this.fnCheckForSavedTask() == false) {
                    this.warninglogoutdailog = false;
                    await this.logout()

                }
            },
            async fnCheckForSavedTask() {
                var lstSavedTaskData = await dbImworks.SavedTaskDB.toArray();

                if (lstSavedTaskData.length > 0) {
                    // Warning message
                    this.warninglogoutdailog = true;
                    return true;
                }
                else {
                    return false;
                }
            },
            async fnGetWorkspaceDetails() {


                var data = new FormData();
                data.append("WorkspaceID", this.currentWorkspaceID);

                var url = window.SERVER_URL + "/Workspace/GetWorkspaceDetails";
                var response = await fetch(url, {
                    method: "POST",
                    credentials: "include",
                    body: data,
                });
                var result = await response.json();

                if (result.data.length > 0) {
                    this.currentWSName = result.data[0].WSName;
                    this.currentWSIsPrivate = result.data[0].WSIsPrivate;

                    if (
                        result.data[0].WorkSpaceImage != null &&
                        result.data[0].WorkSpaceImage.length > 0
                    ) {
                        this.currentWSImage =
                            window.SERVER_URL + "/Upload" + result.data[0].WorkSpaceImage;
                    } else {
                        this.currentWSImage = "";
                    }
                }
            },
            previewImage: function (event) {
                var self = this;
                var input = event.target;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        self.dataURItoBlob(e.target.result, "image/jpeg");
                    };
                    reader.readAsDataURL(input.files[0]);
                }
            },
            dataURItoBlob(dataURI, type) {
                // convert base64 to raw binary data held in a string
                var byteString = atob(dataURI.split(",")[1]);

                // separate out the mime component
                var mimeString = dataURI.split(",")[0].split(":")[1].split(";")[0];

                // write the bytes of the string to an ArrayBuffer
                var ab = new ArrayBuffer(byteString.length);
                var ia = new Uint8Array(ab);
                for (var i = 0; i < byteString.length; i++) {
                    ia[i] = byteString.charCodeAt(i);
                }

                // write the ArrayBuffer to a blob, and you're done
                var bb = new Blob([ab], { type: type });
                this.profileeditImage = URL.createObjectURL(bb);
                //  document.getElementById("idProfileImg").src = URL.createObjectURL(bb);
                this.LoggedUserDetails.ProfileImage = URL.createObjectURL(bb);
                // return bb;
            },
            async ValidateUrl(WorkspaceID) {
  
                var data = new FormData();
                data.append("WorkspaceID", WorkspaceID);
                var result = await usr.FetchUserAccessPagesList(data);
                // debugger;
                if (result.data.length > 0) {
                    //  debugger;
                    await dbImworks.TbPageAccessList.clear();
                    await dbImworks.TbPageAccessList.bulkPut(result.data);
                    return true;
                }
                else {
                    this.Msg = "Authentication failed, Please login again"
                    this.infoSnackbarMsg = true;
                    await this.logout()
                    //this.$router.push({ name: "workspace" });
                }
            },
            // Currently this function is not using , This function is your for to check user to a Page
            async ValidateScreenAccess() {
                //debugger;
                var PageName = this.$router.currentRoute.name;
                //if(PageName.includes('project'))
                if (PageName == 'MyprojectsDeployed' || PageName == 'MyprojectsCompleted' || PageName == 'MyprojectsArchived' || PageName == 'MyprojectsDraftIndividual' || PageName == 'MyprojectsDraftIndividualTeam' || PageName == 'MyprojectsDraftIndividualFinanceDetails' || PageName ==
                    'MyprojectsDraftIndividualFinanceDetailsBudget' || PageName == 'MyprojectsDraftIndividualWorkflow' || PageName == 'MyprojectsDraftIndividualSurveys' ||
                    PageName == 'MyprojectsDraftIndividualBeneficiaries' ||
                    PageName == 'MyprojectsDraftIndividualActivities' ||
                    PageName == 'MyprojectsDraftIndividualKPI' ||
                    PageName == 'MyprojectsDraftIndividualRisks' ||
                    PageName == 'MyprojectsDraftIndividualDocuments' ||
                    PageName == 'MyprojectsDraftIndividualFinancePaymentTerms' ||
                    PageName == 'MyprojectsDraftIndividualSurveysIndividual' ||
                    PageName ==
                    'MyprojectsDraftIndividualBeneficiariesTypeIndividual' ||
                    PageName ==
                    'MyprojectsDraftIndividualBeneficiariesIndividual' ||
                    PageName ==
                    'MyprojectsDraftIndividualActivitiesIndividual' ||
                    PageName == 'MyprojectsDraftIndividualKPIActivityLog' ||
                    PageName == 'MyprojectsDraftIndividualRiskActivityLog' ||
                    PageName == 'MyprojectsDraftIndividualKPIOutcomes' ||
                    PageName == 'MyProjectDraftIndividualOutcomeActivityLog' ||
                    PageName == 'MyprojectsDeployedIndividual' ||
                    PageName == 'MyprojectsDeployedIndividualTeam' ||
                    PageName == 'MyprojectsDeployedIndividualFinanceDetails' ||
                    PageName ==
                    'MyprojectsDeployedIndividualFinanceDetailsBudget' ||
                    PageName ==
                    'MyprojectsDeployedIndividualFinancePaymentTerms' ||
                    PageName == 'MyprojectsDeployedIndividualWorkflow' ||
                    PageName == 'MyprojectsDeployedIndividualSurveys' ||
                    PageName ==
                    'MyprojectsDeployedIndividualSurveysIndividual' ||
                    PageName == 'MyprojectsDeployedIndividualBeneficiaries' ||
                    PageName ==
                    'MyprojectsDeployedIndividualBeneficiariesTypeIndividual' ||
                    PageName ==
                    'MyprojectsDeployedIndividualBeneficiariesIndividual' ||
                    PageName == 'MyprojectsDeployedIndividualActivities' ||
                    PageName ==
                    'MyprojectsDeployedIndividualActivitiesIndividual' ||
                    PageName == 'MyprojectsDeployedIndividualKPI' ||
                    PageName == 'MyprojectsDeployedIndividualKPIOutcomes' ||
                    PageName ==
                    'MyProjectDeployedIndividualOutcomeActivityLog' ||
                    PageName == 'MyprojectsDeployedIndividualKPIActivityLog' ||
                    PageName == 'MyprojectsDeployedIndividualRisks' ||
                    PageName == 'MyprojectsDeployedIndividualRiskActivityLog' ||
                    PageName == 'MyprojectsDeployedIndividualDocuments' ||
                    PageName == 'MyprojectsCompletedIndividual' ||
                    PageName == 'MyprojectsCompletedIndividualTeam' ||
                    PageName == 'MyprojectsCompletedIndividualFinanceDetails' ||
                    PageName ==
                    'MyprojectsCompletedIndividualFinanceDetailsBudget' ||
                    PageName ==
                    'MyprojectsCompletedIndividualFinancePaymentTerms' ||
                    PageName == 'MyprojectsCompletedIndividualWorkflow' ||
                    PageName == 'MyprojectsCompletedIndividualSurveys' ||
                    PageName ==
                    'MyprojectsCompletedIndividualSurveysIndividual' ||
                    PageName == 'MyprojectsCompletedIndividualBeneficiaries' ||
                    PageName ==
                    'MyprojectsCompletedIndividualBeneficiariesTypeIndividual' ||
                    PageName ==
                    'MyprojectsCompletedIndividualBeneficiariesIndividual' ||
                    PageName == 'MyprojectsCompletedIndividualActivities' ||
                    PageName ==
                    'MyprojectsCompletedIndividualActivitiesIndividual' ||
                    PageName == 'MyprojectsCompletedIndividualKPI' ||
                    PageName == 'MyprojectsCompletedIndividualKPIOutcomes' ||
                    PageName ==
                    'MyProjectCompletedIndividualOutcomeActivityLog' ||
                    PageName == 'MyprojectsCompletedIndividualKPIActivityLog' ||
                    PageName == 'MyprojectsCompletedIndividualRisks' ||
                    PageName == 'MyprojectsCompletedIndividualRiskActivityLog' ||
                    PageName == 'MyprojectsCompletedIndividualDocuments' ||
                    PageName == 'MyprojectsArchivedIndividual' ||
                    PageName == 'MyprojectsArchivedIndividualTeam' ||
                    PageName == 'MyprojectsArchivedIndividualFinanceDetails' ||
                    PageName ==
                    'MyprojectsArchivedIndividualFinanceDetailsBudget' ||
                    PageName ==
                    'MyprojectsArchivedIndividualFinancePaymentTerms' ||
                    PageName == 'MyprojectsArchivedIndividualWorkflow' ||
                    PageName == 'MyprojectsArchivedIndividualSurveys' ||
                    PageName ==
                    'MyprojectsArchivedIndividualSurveysIndividual' ||
                    PageName == 'MyprojectsArchivedIndividualBeneficiaries' ||
                    PageName ==
                    'MyprojectsArchivedIndividualBeneficiariesTypeIndividual' ||
                    PageName ==
                    'MyprojectsArchivedIndividualBeneficiariesIndividual' ||
                    PageName == 'MyprojectsArchivedIndividualActivities' ||
                    PageName ==
                    'MyprojectsArchivedIndividualActivitiesIndividual' ||
                    PageName == 'MyprojectsArchivedIndividualKPI' ||
                    PageName == 'MyprojectsArchivedIndividualKPIOutcomes' ||
                    PageName ==
                    'MyProjectArchivedIndividualOutcomeActivityLog' ||
                    PageName == 'MyprojectsArchivedIndividualKPIActivityLog' ||
                    PageName == 'MyprojectsArchivedIndividualRisks' ||
                    PageName == 'MyprojectsArchivedIndividualRiskActivityLog' ||
                    PageName == 'MyprojectsArchivedIndividualDocuments') {
                    //debugger;
                    if (this.showProjects == false) {
                        alert("you dont have access");
                        this.$router.push({ name: "workspace" });
                    }
                }
                //if(PageName.includes('user'))
                if (PageName == 'userlist' || PageName == 'userindividual' || PageName == 'usersgroup' || PageName == 'usergroupindividual' || PageName == 'usersroles' || PageName == 'userroleindividual') {
                    // debugger;
                    if (this.showUsers == false) {
                        alert("you dont have access");
                        this.$router.push({ name: "workspace" });
                    }
                }
                if (PageName == 'OverdueTasks' || PageName == 'CurrentTasks' || PageName == 'CompletedTasks' || PageName == 'SavedTasks') {
                    // debugger;
                    if (this.showTasks == false) {

                        alert("you dont have access");
                        this.$router.push({ name: "workspace" });
                    }
                }
            }
        },
        watch: {
            async $route(to, from) {
                this.ResetMenu();
                this.IsAuthenticated = this.authState
                this.currentWorkspaceID = this.$route.query.WsID;
                
                if (this.currentWorkspaceID > 0)
                    await this.fnGetWorkspaceDetails();
                
                if (this.IsAuthenticated) {
                    if (from.name === null && to.name === "home") {
                        if (!to.query.WsID) {
                            this.$router.push({ name: "workspace" });
                        } else {
                            let WSID = this.$route.query.WsID;
                            await this.ValidateUrl(WSID);
                        }
                    }
                    await this.GetRoleBasedScreenList();
                    await this.fnGetLoggedUserDetails();
                
                    this.resetTimer();

                    let PageName = this.$route.name;
                    let lstAccessPageList = await dbImworks.TbPageAccessList.toArray();
                    
                    if (lstAccessPageList.length > 0 && PageName != 'workspace' && PageName != 'home' && PageName != 'SharedWorkspace' && PageName != 'ArchivedWorkspace') {
                        let find = lstAccessPageList.filter(x => x.PageName == PageName && x.UserID == this.LoggedUserDetails.ID)
                        if (find.length == 0) {
                            //alert("you dont have page access");
                            this.Msg = "you don't have  the permission to access the screen"
                            this.infoSnackbarMsg = true;
                            this.$router.push({ name: "workspace" });
                        }
                    }

                }
            },
        },
    };
</script>


import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import store from "../store";
import TermsAndConditions from '../views/TermsAndConditions.vue'


Vue.use(VueRouter)

async function FunctionToCheckNetwork() {
    var status = true;
    try {
        var url = window.SERVER_URL + "/MobileWebService/FunctionToCheckNetwork";

        let response = await fetch(url, {
            method: 'GET',
            credentials: "include"
        });

        let result = await response.json();

    } catch (err) {
        status = false;
        //console.log(err);
    }

    //console.log("fnIsAuthenticated=", IsAuthenticated, window.LoggedUserID);
    return status;
}

const preAuthRoutes = [
    //{
    //    path: '/Login',
    //    name: 'login',
    //    props: true,
    //    component: () => import('../views/Login.vue')
    //},
    //{
    //    path: '/Signup',
    //    name: 'signup',
    //    component: () => import('../views/Signup.vue')
    //},
    {
        path: '/ResetPassword',
        name: 'resetPassword',
        props: true,
        component: () => import('../views/ResetPassword.vue'),
        meta: {
            auth: false,
            verify: true,
            guestLink: true,
        },
    },
    {
        path: "/Login",
        name: "login",
        component: () => import("../views/LoginAndSignup.vue"),
        meta: {
            auth: false,
            authRoute: true,
        },
    },
    {
        path: '/TermsAndConditions',
        name: 'termsAndConditions',
        //props: true,
        component: TermsAndConditions
        //component: () => import('../views/TermsAndConditions.vue')
    },
];

const WorkspaceRoutes = [
    //Workspace page
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Workspace/Workspace.vue'),
        meta: {
            auth: true,
        },

        children: [
            { path: '/Workspace', name: 'workspace', component: () => import('../views/Workspace/MyWorkspace.vue'), meta: { auth: true }, },
            {
                path: 'MyWorkspace',
                name: 'MyWorkspace',
                props: true,
                component: () => import('../views/Workspace/MyWorkspace.vue'),
                meta: { auth: true },
            },
            {
                path: 'SharedWorkspace',
                name: 'SharedWorkspace',
                props: true,
                component: () => import('../views/Workspace/SharedWorkspace.vue'),
                meta: { auth: true },
            },
            {
                path: 'ArchivedWorkspace',
                name: 'ArchivedWorkspace',
                props: true,
                component: () => import('../views/Workspace/ArchivedWorkspace.vue'),
                meta: { auth: true },
            },

        ]
    },
    //Workspace Manage page
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Workspace/WorkspaceManageHome.vue'),
        children: [
            {
                path: 'WorkspaceManage',
                name: 'WorkspaceManage',
                props: true,
                component: () => import('../views/Workspace/WorkspaceManage.vue'),
                meta: { auth: true },
            }
        ]
    },
];

const InitiativeRoutes = [
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Initiatives/Initiatives.vue'),
        meta: { auth: true },

        children: [
            { path: '/Initiatives', name: 'initiatives', component: () => import('../views/Initiatives/DraftInitiatives.vue'), meta: { auth: true }, },
            {
                path: 'DraftInitiatives',
                name: 'DraftInitiatives',
                props: true,
                component: () => import('../views/Initiatives/DraftInitiatives.vue'),
                meta: { auth: true },
            },
            {
                path: 'DeployedInitiatives',
                name: 'DeployedInitiatives',
                props: true,
                component: () => import('../views/Initiatives/DeployedInitiatives.vue'),
                meta: { auth: true },
            },
            {
                path: 'ArchivedInitiatives',
                name: 'ArchivedInitiatives',
                props: true,
                component: () => import('../views/Initiatives/ArchivedInitiatives.vue'),
                meta: { auth: true },
            },
            //Manage Initiative
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Initiatives/ManageInitiatives.vue'),
                meta: { auth: true },
                children: [
                    { path: '/Initiatives', name: 'ManageInitiatives', component: () => import('../views/Initiatives/ManageInitiativesDetails.vue'), meta: { auth: true }, },
                    {
                        path: 'ManageInitiativesDetails',
                        name: 'ManageInitiativesDetails',
                        props: true,
                        component: () => import('../views/Initiatives/ManageInitiativesDetails.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageInitiativesUsersGroups',
                        name: 'ManageInitiativesUsersGroups',
                        props: true,
                        component: () => import('../views/Initiatives/ManageInitiativesUsersGroups.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageInitiativesProjectTemplate',
                        name: 'ManageInitiativesProjectTemplate',
                        props: true,
                        component: () => import('../views/Initiatives/ManageInitiativesProjectTemplate.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageInitiativesProjectWorkflow',
                        name: 'ManageInitiativesProjectWorkflow',
                        props: true,
                        component: () => import('../views/Initiatives/ManageInitiativesProjectWorkflow.vue'),
                        meta: { auth: true },
                    },
                ]
            },
            //Project page
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Projects/Projects.vue'),
                meta: { auth: true },
                children: [
                    { path: '/Projects', name: 'Projects', component: () => import('../views/Projects/DraftProjects.vue'), meta: { auth: true }, },
                    {
                        path: 'DraftProjects',
                        name: 'DraftProjects',
                        props: true,
                        component: () => import('../views/Projects/DraftProjects.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'DeployedProjects',
                        name: 'DeployedProjects',
                        props: true,
                        component: () => import('../views/Projects/DeployedProjects.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'CompletedProjects',
                        name: 'CompletedProjects',
                        props: true,
                        component: () => import('../views/Projects/CompletedProjects.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ArchivedProjects',
                        name: 'ArchivedProjects',
                        props: true,
                        component: () => import('../views/Projects/ArchivedProjects.vue'),
                        meta: { auth: true },
                    },

                ]
            },
            //Manage Project page
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Projects/ManageProject.vue'),
                meta: { auth: true },
                children: [
                    { path: '/Projects', name: 'ManageProject', component: () => import('../views/Projects/ManageProjectDetails.vue'), meta: { auth: true }, },
                    {
                        path: 'ManageProjectDetails',
                        name: 'ManageProjectDetails',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDetails.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectTeam',
                        name: 'ManageProjectTeam',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectTeam.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectFinance.vue'),
                        meta: { auth: true },
                        children: [
                            {
                                path: 'ManageProjectFinance',
                                name: 'ManageProjectFinance',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLanding.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: '/ManageProjectFinanceBudget',
                                name: 'ManageProjectFinanceBudget',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectFinancePaymentTerms',
                                name: 'ManageProjectFinancePaymentTerms',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinancePaymentTerms.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectFinanceAccounts',
                                name: 'ManageProjectFinanceAccounts',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceAccounts.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectFinanceLedgerBook',
                                name: 'ManageProjectFinanceLedgerBook',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLedgerBook.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },
                    {
                        path: 'ManageProjectWorkflow',
                        name: 'ManageProjectWorkflow',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectWorkflow.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveys.vue'),
                        meta: { auth: true },
                        children: [
                            {
                                path: 'ManageProjectSurveys',
                                name: 'ManageProjectSurveys',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectSurveysLanding.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectSurveysIndividual',
                                name: 'ManageProjectSurveysIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectSurveysIndividual.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiaries.vue'),
                        meta: { auth: true },
                        children: [
                            {
                                path: 'ManageProjectBeneficiaries',
                                name: 'ManageProjectBeneficiaries',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectBeneficiariesLanding.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectBeneficiariesTypeIndividual',
                                name: 'ManageProjectBeneficiariesTypeIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectBeneficiariesTypeIndividual.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectBeneficiariesIndividual',
                                name: 'ManageProjectBeneficiariesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectBeneficiariesIndividual.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectActivities.vue'),
                        meta: { auth: true },
                        children: [
                            {
                                path: 'ManageProjectActivities',
                                name: 'ManageProjectActivities',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesLanding.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: 'ManageProjectActivitiesIndividual',
                                name: 'ManageProjectActivitiesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesIndividual.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },
                    {
                        path: 'ManageProjectKPI',
                        name: 'ManageProjectKPI',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPI.vue'),
                        meta: { auth: true },
                    },
                    // Outcome router
                    {
                        path: '/ManageProjectOutcomes',
                        name: 'ManageProjectOutcomes',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomes.vue'),
                        meta: { auth: true },
                    },
                    // Edit Manage Outcome router
                    {
                        path: '/ManageProjectOutcomeActivityLog', //ManageProjectOutcomeActivityLog
                        name: 'ManageProjectOutcomeActivityLog',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomeActivityLog.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectRisks',
                        name: 'ManageProjectRisks',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRisks.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectDocuments',
                        name: 'ManageProjectDocuments',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDocuments.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectDiscussion',
                        name: 'ManageProjectDiscussion',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDiscussion.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectRiskActivityLog',
                        name: 'ManageProjectRiskActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRiskActivityLog.vue'),
                        meta: { auth: true },
                    },
                    {
                        path: 'ManageProjectKPIActivityLog',
                        name: 'ManageProjectKPIActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPIActivityLog.vue'),
                        meta: { auth: true },
                    },
                ]
            },
        ]
    },
];

const MyProjectsRoutes = [
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Myprojects/Myprojects.vue'),
        meta: { auth: true },
        children: [
            {
                path: '/MyprojectsDraft',
                name: 'MyprojectsDraft',
                component: () => import('../views/Myprojects/MyprojectsDraft.vue'),
                meta: {
                    auth: true,
                    ProjectType: "Draft",
                },
            },
            //MyProject Draft Individual
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsDraftIndividual.vue'),
                meta: {
                    auth: true,
                    ProjectType: "Draft",
                    breadcrumb: [
                        {
                            _name: "Drafts",
                            name: "MyprojectsDraft",
                            path: "/MyprojectsDraft",
                        },
                    ],
                },
                children: [
                    {
                        path: '/MyprojectsDraftIndividual',
                        name: 'MyprojectsDraftIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDetails.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualTeam',
                        name: 'MyprojectsDraftIndividualTeam',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectTeam.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectFinance.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsDraftIndividualFinanceDetails',
                                name: 'MyprojectsDraftIndividualFinanceDetails',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLanding.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: '/MyprojectsDraftIndividualFinanceDetailsBudget',
                                name: 'MyprojectsDraftIndividualFinanceDetailsBudget',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: '/MyprojectsDraftIndividualFinancePaymentTerms',
                                name: 'MyprojectsDraftIndividualFinancePaymentTerms',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinancePaymentTerms.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },

                    {
                        path: '/MyprojectsDraftIndividualWorkflow',
                        name: 'MyprojectsDraftIndividualWorkflow',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectWorkflow.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualSurveys',
                        name: 'MyprojectsDraftIndividualSurveys',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualSurveysIndividual',
                        name: 'MyprojectsDraftIndividualSurveysIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualBeneficiaries',
                        name: 'MyprojectsDraftIndividualBeneficiaries',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualBeneficiariesTypeIndividual',
                        name: 'MyprojectsDraftIndividualBeneficiariesTypeIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesTypeIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualBeneficiariesIndividual',
                        name: 'MyprojectsDraftIndividualBeneficiariesIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectActivities.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsDraftIndividualActivities',
                                name: 'MyprojectsDraftIndividualActivities',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Draft",
                                    breadcrumb: [
                                        {
                                            _name: "Drafts",
                                            name: "MyprojectsDraft",
                                            path: "/MyprojectsDraft",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsDraftIndividualActivitiesIndividual',
                                name: 'MyprojectsDraftIndividualActivitiesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesIndividual.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Draft",
                                    breadcrumb: [
                                        {
                                            _name: "Drafts",
                                            name: "MyprojectsDraft",
                                            path: "/MyprojectsDraft",
                                        },
                                    ],
                                },
                            },
                        ]
                    },
                    {
                        path: '/MyprojectsDraftIndividualKPI',
                        name: 'MyprojectsDraftIndividualKPI',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPI.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    // Outcome router
                    {
                        path: '/MyprojectsDraftIndividualKPIOutcomes',
                        name: 'MyprojectsDraftIndividualKPIOutcomes',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomes.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    // Edit Manage Outcome router
                    {
                        path: '/MyProjectDraftIndividualOutcomeActivityLog', //ManageProjectOutcomeActivityLog
                        name: 'MyProjectDraftIndividualOutcomeActivityLog',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomeActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualKPIActivityLog',
                        name: 'MyprojectsDraftIndividualKPIActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPIActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualRisks',
                        name: 'MyprojectsDraftIndividualRisks',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRisks.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualRiskActivityLog',
                        name: 'MyprojectsDraftIndividualRiskActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRiskActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDraftIndividualDocuments',
                        name: 'MyprojectsDraftIndividualDocuments',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDocuments.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Draft",
                            breadcrumb: [
                                {
                                    _name: "Drafts",
                                    name: "MyprojectsDraft",
                                    path: "/MyprojectsDraft",
                                },
                            ],
                        },
                    },
                ]
            },
            //MyProjec Deployed route
            {
                path: '/MyprojectsDeployed',
                name: 'MyprojectsDeployed',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsDeployed.vue'),
                meta: { auth: true },
            },
            //My Project Deployed Individual
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsDeployedIndividual.vue'),
                meta: {
                    auth: true,
                    ProjectType: "Deployed",
                    breadcrumb: [
                        {
                            _name: "Deployed",
                            name: "MyprojectsDeployed",
                            path: "/MyprojectsDeployed",
                        },
                    ],
                },
                children: [
                    {
                        path: '/MyprojectsDeployedIndividual',
                        name: 'MyprojectsDeployedIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDetails.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualTeam',
                        name: 'MyprojectsDeployedIndividualTeam',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectTeam.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectFinance.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsDeployedIndividualFinanceDetails',
                                name: 'MyprojectsDeployedIndividualFinanceDetails',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Deployed",
                                    breadcrumb: [
                                        {
                                            _name: "Deployed",
                                            name: "MyprojectsDeployed",
                                            path: "/MyprojectsDeployed",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsDeployedIndividualFinanceDetailsBudget',
                                name: 'MyprojectsDeployedIndividualFinanceDetailsBudget',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Deployed",
                                    breadcrumb: [
                                        {
                                            _name: "Deployed",
                                            name: "MyprojectsDeployed",
                                            path: "/MyprojectsDeployed",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsDeployedIndividualFinancePaymentTerms',
                                name: 'MyprojectsDeployedIndividualFinancePaymentTerms',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinancePaymentTerms.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Deployed",
                                    breadcrumb: [
                                        {
                                            _name: "Deployed",
                                            name: "MyprojectsDeployed",
                                            path: "/MyprojectsDeployed",
                                        },
                                    ],
                                },
                            },
                        ]
                    },

                    {
                        path: '/MyprojectsDeployedIndividualWorkflow',
                        name: 'MyprojectsDeployedIndividualWorkflow',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectWorkflow.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualSurveys',
                        name: 'MyprojectsDeployedIndividualSurveys',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualSurveysIndividual',
                        name: 'MyprojectsDeployedIndividualSurveysIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualBeneficiaries',
                        name: 'MyprojectsDeployedIndividualBeneficiaries',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualBeneficiariesTypeIndividual',
                        name: 'MyprojectsDeployedIndividualBeneficiariesTypeIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesTypeIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualBeneficiariesIndividual',
                        name: 'MyprojectsDeployedIndividualBeneficiariesIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectActivities.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsDeployedIndividualActivities',
                                name: 'MyprojectsDeployedIndividualActivities',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Deployed",
                                    breadcrumb: [
                                        {
                                            _name: "Deployed",
                                            name: "MyprojectsDeployed",
                                            path: "/MyprojectsDeployed",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsDeployedIndividualActivitiesIndividual',
                                name: 'MyprojectsDeployedIndividualActivitiesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesIndividual.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Deployed",
                                    breadcrumb: [
                                        {
                                            _name: "Deployed",
                                            name: "MyprojectsDeployed",
                                            path: "/MyprojectsDeployed",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/OutputsReportTable',
                                name: 'OutputReportTable',
                                props: true,
                                component: () => import('../views/Projects/ProjectComponents/OutputReportTable.vue'),
                                meta: { auth: true },
                            },
                            {
                                path: '/OutcomeReportTable',
                                name: 'OutcomeReportTable',
                                props: true,
                                component: () => import('../views/Projects/ProjectComponents/OutcomeReportTable.vue'),
                                meta: { auth: true },
                            }
                        ]
                    },
                    {
                        path: '/MyprojectsDeployedIndividualKPI',
                        name: 'MyprojectsDeployedIndividualKPI',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPI.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    // Outcome router
                    {
                        path: '/MyprojectsDeployedIndividualKPIOutcomes',
                        name: 'MyprojectsDeployedIndividualKPIOutcomes',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomes.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    // Edit Manage Outcome router
                    {
                        path: '/MyProjectDeployedIndividualOutcomeActivityLog', //ManageProjectOutcomeActivityLog
                        name: 'MyProjectDeployedIndividualOutcomeActivityLog',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomeActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualKPIActivityLog',
                        name: 'MyprojectsDeployedIndividualKPIActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPIActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualRisks',
                        name: 'MyprojectsDeployedIndividualRisks',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRisks.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualRiskActivityLog',
                        name: 'MyprojectsDeployedIndividualRiskActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRiskActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsDeployedIndividualDocuments',
                        name: 'MyprojectsDeployedIndividualDocuments',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDocuments.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Deployed",
                            breadcrumb: [
                                {
                                    _name: "Deployed",
                                    name: "MyprojectsDeployed",
                                    path: "/MyprojectsDeployed",
                                },
                            ],
                        },
                    },
                ]
            },
            //My project completed routing
            {
                path: '/MyprojectsCompleted',
                name: 'MyprojectsCompleted',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsCompleted.vue'),
                meta: { auth: true, ProjectType: "Completed" },
            },
            //My project Completed individul
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsCompletedIndividual.vue'),
                meta: { auth: true, ProjectType: "Completed" },
                children: [
                    {
                        path: '/MyprojectsCompletedIndividual',
                        name: 'MyprojectsCompletedIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDetails.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualTeam',
                        name: 'MyprojectsCompletedIndividualTeam',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectTeam.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectFinance.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsCompletedIndividualFinanceDetails',
                                name: 'MyprojectsCompletedIndividualFinanceDetails',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Completed",
                                    breadcrumb: [
                                        {
                                            _name: "Completed",
                                            name: "MyprojectsCompleted",
                                            path: "/MyprojectsCompleted",
                                        },
                                        {
                                            _name: "Finances",
                                            name: "MyprojectsCompletedIndividualFinanceDetails",
                                            path: "/MyprojectsCompletedIndividualFinanceDetails",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsCompletedIndividualFinanceDetailsBudget',
                                name: 'MyprojectsCompletedIndividualFinanceDetailsBudget',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Completed",
                                    breadcrumb: [
                                        {
                                            _name: "Completed",
                                            name: "MyprojectsCompleted",
                                            path: "/MyprojectsCompleted",
                                        },
                                        {
                                            _name: "Finances",
                                            name: "MyprojectsCompletedIndividualFinanceDetails",
                                            path: "/MyprojectsCompletedIndividualFinanceDetails",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsCompletedIndividualFinancePaymentTerms',
                                name: 'MyprojectsCompletedIndividualFinancePaymentTerms',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinancePaymentTerms.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Completed",
                                    breadcrumb: [
                                        {
                                            _name: "Completed",
                                            name: "MyprojectsCompleted",
                                            path: "/MyprojectsCompleted",
                                        },
                                        {
                                            _name: "Finances",
                                            name: "MyprojectsCompletedIndividualFinanceDetails",
                                            path: "/MyprojectsCompletedIndividualFinanceDetails",
                                        },
                                    ],
                                },
                            },
                        ]
                    },

                    {
                        path: '/MyprojectsCompletedIndividualWorkflow',
                        name: 'MyprojectsCompletedIndividualWorkflow',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectWorkflow.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualSurveys',
                        name: 'MyprojectsCompletedIndividualSurveys',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualSurveysIndividual',
                        name: 'MyprojectsCompletedIndividualSurveysIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualBeneficiaries',
                        name: 'MyprojectsCompletedIndividualBeneficiaries',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualBeneficiariesTypeIndividual',
                        name: 'MyprojectsCompletedIndividualBeneficiariesTypeIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesTypeIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualBeneficiariesIndividual',
                        name: 'MyprojectsCompletedIndividualBeneficiariesIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectActivities.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsCompletedIndividualActivities',
                                name: 'MyprojectsCompletedIndividualActivities',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Completed",
                                    breadcrumb: [
                                        {
                                            _name: "Completed",
                                            name: "MyprojectsCompleted",
                                            path: "/MyprojectsCompleted",
                                        },
                                        {
                                            _name: "Activities",
                                            name: "MyprojectsCompletedIndividualActivities",
                                            path: "/MyprojectsCompletedIndividualActivities",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsCompletedIndividualActivitiesIndividual',
                                name: 'MyprojectsCompletedIndividualActivitiesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesIndividual.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Completed",
                                    breadcrumb: [
                                        {
                                            _name: "Completed",
                                            name: "MyprojectsCompleted",
                                            path: "/MyprojectsCompleted",
                                        },
                                        {
                                            _name: "Activities",
                                            name: "MyprojectsCompletedIndividualActivities",
                                            path: "/MyprojectsCompletedIndividualActivities",
                                        },
                                    ],
                                },
                            },
                        ]
                    },
                    {
                        path: '/MyprojectsCompletedIndividualKPI',
                        name: 'MyprojectsCompletedIndividualKPI',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPI.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    // Outcome router
                    {
                        path: '/MyprojectsCompletedIndividualKPIOutcomes',
                        name: 'MyprojectsCompletedIndividualKPIOutcomes',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomes.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    // Edit Manage Outcome router
                    {
                        path: '/MyProjectCompletedIndividualOutcomeActivityLog', //ManageProjectOutcomeActivityLog
                        name: 'MyProjectCompletedIndividualOutcomeActivityLog',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomeActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualKPIActivityLog',
                        name: 'MyprojectsCompletedIndividualKPIActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPIActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualRisks',
                        name: 'MyprojectsCompletedIndividualRisks',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRisks.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualRiskActivityLog',
                        name: 'MyprojectsCompletedIndividualRiskActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRiskActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsCompletedIndividualDocuments',
                        name: 'MyprojectsCompletedIndividualDocuments',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDocuments.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Completed",
                            breadcrumb: [
                                {
                                    _name: "Completed",
                                    name: "MyprojectsCompleted",
                                    path: "/MyprojectsCompleted",
                                },
                            ],
                        },
                    },
                ]
            },
            //Archived Projects route
            {
                path: '/MyprojectsArchived',
                name: 'MyprojectsArchived',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsArchived.vue'),
                meta: { auth: true, ProjectType: "Archived" },
            },
            //Archived Project Individual
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Myprojects/MyprojectsArchivedIndividual.vue'),
                meta: { auth: true, ProjectType: "Archived" },
                children: [
                    {
                        path: '/MyprojectsArchivedIndividual',
                        name: 'MyprojectsArchivedIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDetails.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualTeam',
                        name: 'MyprojectsArchivedIndividualTeam',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectTeam.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectFinance.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsArchivedIndividualFinanceDetails',
                                name: 'MyprojectsArchivedIndividualFinanceDetails',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Archived",
                                    breadcrumb: [
                                        {
                                            _name: "Archived",
                                            name: "MyprojectsArchived",
                                            path: "/MyprojectsArchived",
                                        },
                                        {
                                            _name: "Finance",
                                            name: "MyprojectsArchivedIndividualFinance",
                                            path: "/MyprojectsArchivedIndividualFinance",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsArchivedIndividualFinanceDetailsBudget',
                                name: 'MyprojectsArchivedIndividualFinanceDetailsBudget',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Archived",
                                    breadcrumb: [
                                        {
                                            _name: "Archived",
                                            name: "MyprojectsArchived",
                                            path: "/MyprojectsArchived",
                                        },
                                        {
                                            _name: "Finance",
                                            name: "MyprojectsArchivedIndividualFinance",
                                            path: "/MyprojectsArchivedIndividualFinance",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsArchivedIndividualFinancePaymentTerms',
                                name: 'MyprojectsArchivedIndividualFinancePaymentTerms',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectFinancePaymentTerms.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Archived",
                                    breadcrumb: [
                                        {
                                            _name: "Archived",
                                            name: "MyprojectsArchived",
                                            path: "/MyprojectsArchived",
                                        },
                                        {
                                            _name: "Finance",
                                            name: "MyprojectsArchivedIndividualFinance",
                                            path: "/MyprojectsArchivedIndividualFinance",
                                        },
                                    ],
                                },
                            },
                        ]
                    },

                    {
                        path: '/MyprojectsArchivedIndividualWorkflow',
                        name: 'MyprojectsArchivedIndividualWorkflow',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectWorkflow.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualSurveys',
                        name: 'MyprojectsArchivedIndividualSurveys',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualSurveysIndividual',
                        name: 'MyprojectsArchivedIndividualSurveysIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectSurveysIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualBeneficiaries',
                        name: 'MyprojectsArchivedIndividualBeneficiaries',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesLanding.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualBeneficiariesTypeIndividual',
                        name: 'MyprojectsArchivedIndividualBeneficiariesTypeIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesTypeIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualBeneficiariesIndividual',
                        name: 'MyprojectsArchivedIndividualBeneficiariesIndividual',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectBeneficiariesIndividual.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectActivities.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                        children: [
                            {
                                path: '/MyprojectsArchivedIndividualActivities',
                                name: 'MyprojectsArchivedIndividualActivities',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesLanding.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Archived",
                                    breadcrumb: [
                                        {
                                            _name: "Archived",
                                            name: "MyprojectsArchived",
                                            path: "/MyprojectsArchived",
                                        },
                                    ],
                                },
                            },
                            {
                                path: '/MyprojectsArchivedIndividualActivitiesIndividual',
                                name: 'MyprojectsArchivedIndividualActivitiesIndividual',
                                props: true,
                                component: () => import('../views/Projects/ManageProjectActivitiesIndividual.vue'),
                                meta: {
                                    auth: true,
                                    ProjectType: "Archived",
                                    breadcrumb: [
                                        {
                                            _name: "Archived",
                                            name: "MyprojectsArchived",
                                            path: "/MyprojectsArchived",
                                        },
                                    ],
                                },
                            },
                        ]
                    },
                    {
                        path: '/MyprojectsArchivedIndividualKPI',
                        name: 'MyprojectsArchivedIndividualKPI',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPI.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    // Outcome router
                    {
                        path: '/MyprojectsArchivedIndividualKPIOutcomes',
                        name: 'MyprojectsArchivedIndividualKPIOutcomes',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomes.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    // Edit Manage Outcome router
                    {
                        path: '/MyProjectArchivedIndividualOutcomeActivityLog', //ManageProjectOutcomeActivityLog
                        name: 'MyProjectArchivedIndividualOutcomeActivityLog',
                        props: true,
                        component: () => import('../views/Project_Setup/ManageProjectOutcomeActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualKPIActivityLog',
                        name: 'MyprojectsArchivedIndividualKPIActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectKPIActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualRisks',
                        name: 'MyprojectsArchivedIndividualRisks',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRisks.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualRiskActivityLog',
                        name: 'MyprojectsArchivedIndividualRiskActivityLog',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectRiskActivityLog.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                    {
                        path: '/MyprojectsArchivedIndividualDocuments',
                        name: 'MyprojectsArchivedIndividualDocuments',
                        props: true,
                        component: () => import('../views/Projects/ManageProjectDocuments.vue'),
                        meta: {
                            auth: true,
                            ProjectType: "Archived",
                            breadcrumb: [
                                {
                                    _name: "Archived",
                                    name: "MyprojectsArchived",
                                    path: "/MyprojectsArchived",
                                },
                            ],
                        },
                    },
                ]
            },
        ]

    },
];

const VentureRoutes = [
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Venture/Venture.vue'),
        meta: { auth: true },
        children: [
            { path: '/SearchVenture', name: 'SearchVenture', component: () => import('../views/Venture/SearchVenture.vue'), meta: { auth: true }, },
            { path: '/MyVenture', name: 'MyVenture', component: () => import('../views/Venture/MyVenture.vue'), meta: { auth: true }, },
            { path: '/MyFinance', name: 'MyFinance', component: () => import('../views/Venture/MyFinance.vue'), meta: { auth: true }, },
            // Manage Venture
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Venture/ManageVenture.vue'),
                meta: { auth: true },
                children: [
                    { path: '/ManageVentureDetails', name: 'ManageVentureDetails', component: () => import('../views/Venture/ManageVentureDetails.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureFinanceDetails', name: 'ManageVentureFinanceDetails', component: () => import('../views/Venture/ManageVentureFinanceDetails.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureActivities', name: 'ManageVentureActivities', component: () => import('../views/Venture/ManageVentureActivities.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureKPI', name: 'ManageVentureKPI', component: () => import('../views/Venture/ManageVentureKPI.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureDocuments', name: 'ManageVentureDocuments', component: () => import('../views/Venture/ManageVentureDocuments.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureKPIActivityLog', name: 'ManageVentureKPIActivityLog', component: () => import('../views/Venture/ManageVentureKPIActivityLog.vue'), meta: { auth: true }, },
                    { path: '/ManageVentureActivityIndividual', name: 'ManageVentureActivityIndividual', component: () => import('../views/Venture/ManageVentureActivityIndividual.vue'), meta: { auth: true }, }
                ]
            },
        ]
    },
];

const FinanceRoutes = [
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Finance/Finance.vue'),
        meta: { auth: true },
        children: [
            { path: '/SearchFinance', name: 'SearchFinance', component: () => import('../views/Finance/SearchFinance.vue'), meta: { auth: true }, },
            {
                path: '',
                name: '',
                props: true,
                component: () => import('../views/Finance/ManageFinance.vue'),
                meta: { auth: true },
                children: [
                    { path: '/FinanceBudgetAndFunds', name: 'FinanceBudgetAndFunds', component: () => import('../views/Finance/FinanceBudgetAndFunds.vue'), meta: { auth: true }, },
                    { path: '/ManageFinanceAccount', name: 'ManageFinanceAccount', component: () => import('../views/Finance/ManageFinanceAccount.vue'), meta: { auth: true }, },
                    { path: '/ManageFinancePaymentTerms', name: 'ManageFinancePaymentTerms', component: () => import('../views/Finance/ManageFinancePaymentTerms.vue'), meta: { auth: true }, },
                    { path: '/FinanceBudgetfundsViewAll', name: 'FinanceBudgetfundsViewAll', props: true, component: () => import('../views/Projects/ManageProjectFinanceBudget.vue'), meta: { auth: true }, },
                    {
                        path: '',
                        name: '',
                        props: true,
                        component: () => import('../views/Finance/ManageFinanceActivitiesAndWorkflow.vue'),
                        meta: { auth: true },
                        children: [
                            {
                                path: 'ManageFinanceActivitiesAndWorkflow',
                                name: 'ManageFinanceActivitiesAndWorkflow',
                                props: true,
                                component: () => import('../views/Finance/ManageFinanceActivitiesAndWorkflowLanding.vue'),
                                meta: { auth: true },
                            },
                        ]
                    },
                    { path: '/ManageFinanceLog', name: 'ManageFinanceLog', component: () => import('../views/Finance/ManageFinanceLog.vue'), meta: { auth: true }, },
                ]
            }
        ]
    },
];

const UserRoutes = [
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Users/Users.vue'),
        meta: { auth: true },

        children: [
            {
                path: 'UserList',
                name: 'userlist',
                props: true,
                component: () => import('../views/Users/Userlist.vue'),
                meta: { auth: true },
            },
            {
                path: 'UserGroup',
                name: 'usersgroup',
                props: true,
                component: () => import('../views/Users/UserGroups.vue'),
                meta: { auth: true },
            },
            {
                path: 'UserRoles',
                name: 'usersroles',
                props: true,
                component: () => import('../views/Users/UserRoles.vue'),
                meta: { auth: true },
            },
            {
                path: 'UserIndividual',
                name: 'userindividual',
                props: true,
                component: () => import('../views/Users/UserIndividual.vue'),
                meta: { auth: true },
            },
            {
                path: 'UserRoleIndividual',
                name: 'userroleindividual',
                props: true,
                component: () => import('../views/Users/UserRoleIndividual.vue'),
                meta: { auth: true },
            },
            {
                path: 'UserGroupIndividual',
                name: 'usergroupindividual',
                props: true,
                component: () => import('../views/Users/UserGroupIndividual.vue'),
                meta: { auth: true },
            }
        ]
    },
];

const TasksRoutes = [
    //Task page
    {
        //path: '/Tasks',
        //name: 'Tasks',
        //props: true,
        //component: () => import('../views/Tasks/Tasks.vue'),

        path: '',
        name: '',
        props: true,
        component: () => import('../views/Tasks/ManageTasks.vue'),
        meta: { auth: true },
        children: [
            {
                path: '/OverdueTasks',
                name: 'OverdueTasks',
                props: true,
                component: () => import('../views/Tasks/OverdueTasks.vue'),
                meta: { auth: true },
            },
            {
                path: '/CurrentTasks',
                name: 'CurrentTasks',
                props: true,
                component: () => import('../views/Tasks/CurrentTasks.vue'),
                meta: { auth: true },
            },
            {
                path: '/CompletedTasks',
                name: 'CompletedTasks',
                props: true,
                component: () => import('../views/Tasks/CompletedTasks.vue'),
                meta: { auth: true },
            },
            {
                path: '/SavedTasks',
                name: 'SavedTasks',
                props: true,
                component: () => import('../views/Tasks/SavedTasks.vue'),
                meta: { auth: true },
            },
        ]

    },
    //MyTasks page
    {
        path: "",
        props: true,
        component: () => import('../views/MyTasks/MyTasksIndex.vue'),
        meta: { auth: true },
        children: [
            {
                path: '/MyTasks',
                name: 'MyTasks',
                props: true,
                component: () => import('../views/MyTasks/MyTasksLandingPage.vue'),
                meta: { auth: true },
            },
            {
                path: '/MyTasksManage',
                name: 'MyTasksManage',
                props: true,
                component: () => import('../views/MyTasks/BulkTaskManager.vue'),
                meta: { auth: true },
            }
        ]
    },
];

const ReportRoutes = [
    //report
    {
        path: '',
        name: '',
        props: true,
        component: () => import('../views/Reports/ReportDashboard.vue'),
        meta: { auth: true },
        children: [
            {
                path: '/MapViewReport',
                name: 'MapViewReport',
                props: true,
                component: () => import('../views/Reports/ReportWidget/MapViewReport.vue'),
                meta: {
                    auth: false,
                    verify: true,
                    guestLink: true,
                },
            },
            {
                path: '/ProjectReports',
                name: 'ProjectReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/ProjectReport.vue'),
                meta: { auth: true },
            },
            {
                path: '/SurveryReports',
                name: 'SurveryReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/SurveryReport.vue'),
                meta: { auth: true },
            },
            {
                path: '/OutcomesReports',
                name: 'OutcomesReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/OutcomesReport.vue'),
                meta: { auth: true },
            },
            {
                path: '/OutputReports',
                name: 'OutputReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/OutputReport.vue'),
                meta: { auth: true },
            },
            {
                path: '/InputReports',
                name: 'InputReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/InputReports.vue'),
                meta: { auth: true },
            },
            {
                path: '/BeneficiariesReports',
                name: 'BeneficiariesReports',
                props: true,
                component: () => import('../views/Reports/ReportWidget/BeneficiariesReports.vue'),
                meta: { auth: true },
            },
        ]

    },
];

const ShareLinkRoutes = [
    //SurveyShareLink page
    {
        path: '/SurveyShareLink',
        name: 'SurveyShareLink',
        props: true,
        component: () => import('../views/SurveyShareLink.vue'),
        meta: {
            auth: false,
            verify: true,
            guestLink: true,
        },
    },
];

const defaultRoutes = [
    //Home page
    {
        path: '/',
        name: 'home',
        component: () => import(/* webpackChunkName: "mypage" */ '../views/Home.vue'),
        meta: { auth: true },
    },
    {
        path: '/UiComponents',
        name: 'uicomponents',
        component: () => import('../views/UiComponentSheet.vue'),
    },
];

const fallbackRoutes = [
    {//Error page routed whenever any error from backend server
        path: '/Error',
        name: 'error',
        props: true,
        component: () => import('../views/Error.vue')
    },
    {
        path: "*", component: () => import('../views/404.vue'),  //component: PageNotFound
        meta: {
            auth: false,
        },
    },
];

const routes = [
    ...preAuthRoutes,
    ...defaultRoutes,
    ...WorkspaceRoutes,
    ...ReportRoutes,
    ...InitiativeRoutes,
    ...MyProjectsRoutes,
    ...VentureRoutes,
    ...FinanceRoutes,
    ...UserRoutes,
    ...TasksRoutes,
    ...ShareLinkRoutes,
    ...fallbackRoutes,
];


const router = new VueRouter({
    //mode: 'history',
    routes,
    base: "app",

    scrollBehavior(to, from, savedPosition) {
        return { x: 0, y: 0 };
    }
})

router.beforeEach(async (to, from, next) => {
    let authState = store.getters["App/authState"];
    to.matched.some((rec) => {
        if (!rec.meta.auth && rec.meta.authRoute) {
            if (to.name == 'login') {
                if (!authState) {
                    next()
                }
                else {
                    next(false)
                }
            }
        }
        else if (!rec.meta.auth && rec.meta.guestLink && rec.meta.verify) {
            if (to.name == "SurveyShareLink") {
                store
                    .dispatch("App/getShareLinkAuth")
                    .then((val) => {
                        if (!val.status || val.status != "valid") {
                            throw new Error('invalid');
                        } else next();
                    })
                    .catch(() => {
                        next({ path: "/Error" });
                        setTimeout(() => {
                            next({ path: "/Login" });
                        }, 3000);
                    });
            }
            else if (to.name == "MapViewReport" && to.query.VersionId) {
                store
                    .dispatch("App/getMapViewAuth", { ...to.query })
                    .then((val) => {
                        if (val) {
                            next();
                        } else throw error;
                    })
                    .catch(() => {
                        next({ path: "/Error" });

                        setTimeout(() => {
                            next({ path: "/Login" });
                        }, 3000);
                    });
            }
            else if (to.name == "resetPassword") {
                if (to.query.key) {
                    next();
                } else {
                    next({ path: "/Login" });
                }
            }
        }
        else if (rec.meta.auth) {
            store
                .dispatch("App/getAuthState")
                .then((val) => {
                    if (!val) {
                        throw new Error("nav error");
                    }
                    else next();
                    
                })
                .catch((e) => {
                    console.error(e);
                    next({ path: "/Login" });
                });
        }
        else next()
    })
})

export default router

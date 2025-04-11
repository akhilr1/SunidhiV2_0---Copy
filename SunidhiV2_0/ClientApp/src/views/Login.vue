<template>
    <v-col cols="12" sm="8" md="4">
        <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
        <v-card class="elevation-12 login-box">
            <v-card-text>
                <v-row>
                    <v-col xs="12" sm="12">
                        <h6 class="text-center">
                            <img src="../images/Login-Logo.svg" class="login-logo" />
                        </h6>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-text>
                <v-form>
                    <v-text-field name="username"
                                  label="Enter Email"
                                  v-model="email"
                                  type="text"
                                  id="username"></v-text-field>
                    <v-text-field name="password"
                                  label="Enter Password"
                                  v-model="passwd"
                                  id="password"
                                  type="password"></v-text-field>
                </v-form>
            </v-card-text>
            <v-card-text class="py-0">
                <v-row py-0 no-gutters>
                    <v-col xs="6" sm="6">
                        <v-checkbox label="Remember me"></v-checkbox>
                    </v-col>
                    <v-col xs="6" sm="6" class="text-primary text-right my-auto">
                        <v-btn text @click="resetPassword = true">Forgot Password ?</v-btn>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions class="pb-4 pt-0 text-xs-center d-flex flex-column">
                <v-btn dark name="btnLogin" color="primary" class="text-none login-btn" block @click="fnLogin()">Log In</v-btn>
                <br>
                <v-btn dark class="text-none login-btn" block to="signup">Sign Up</v-btn>
            </v-card-actions>
        </v-card>
        <!-- Password Reset Dialog -->
        <v-dialog v-model="resetPassword" width="600px" class="reset-dlg">
            <v-card>
                <v-card-title>
                    Reset Password
                </v-card-title>
                <v-card-text class="pb-1">
                    <v-card-subtitle class="px-0 pt-0">
                        Please enter your registered email address to reset your password
                    </v-card-subtitle>
                    <v-form lazy-validation ref="forgotPasswordForm">
                        <v-row>
                            <v-col class="py-0 ma-0">
                                <v-text-field :rules="resetEmailRules" outlined label="E-Mail" v-model="resetEmail" required></v-text-field>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card-text>
                <v-card-actions class="d-flex justify-end pr-5 pb-5 pt-0">
                    <v-btn color="blue" dark x-large @click="fnResetPassword()">Send</v-btn>
                </v-card-actions>
                <v-btn type="submit" icon class="reset-close-btn" :ripple="false" @click="resetPassword = false, resetEmail = ''">
                    <v-icon>
                        mdi-close
                    </v-icon>
                </v-btn>
            </v-card>
        </v-dialog>
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
    </v-col>
</template>

<script type="text/javascript">
    import { dbImworks } from '../ImworksDb'
    //import syncData from '../ImworksMobApp.js'
    import syncData from "../ImworksMobApp.js";
    import PreLoader from "@/components/pre-loader.vue";
    var usr;
    var objWorkspace;


    async function importscript() {
        return Promise.all([
            import("../BL/User.js").then(mod => {
                usr = new mod.User();               
            }),
            import("../BL/Workspace.js").then(mod => {
                objWorkspace = new mod.Workspace();
            })
        ]);
    }

    export default {
        data: function () {
            return {
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                Msg: "",
                email: "",
                passwd: "",
                PreLoader: false,
                resetPassword: false,
                resetEmail: '',
                resetEmailRules: [
                    val => !!val || 'Required',
                    val => (val && /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g.test(val)) || 'Enter Valid Email Address'
                ]
            };
        },
        async mounted() {
            await importscript();            
            //usr.fnLogin("dd","dd");
        },
        methods: {
            async fnLogin() {
              //  debugger;
                if (this.email != "" && this.passwd != "") {
                    var notificationtoken = localStorage.getItem("notificationToken");
                    var result = await usr.Login(this.email, this.passwd, notificationtoken);

                    if (result.status) {

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
                        //dbImworks.TbTaskAuditLog.clear();
                        dbImworks.TbTaskResponseDocument.clear();
                        dbImworks.TbTaskBenefRegDocument.clear();

                        window.LoggedUserID = result.userId;
                        sessionStorage.setItem('LoggedUserID', result.userId);


                        var lstUser = await dbImworks.User.toArray();
                        if (lstUser.length == 0)
                            await dbImworks.User.put({ UserId: result.userId, IsLoggedIn: 1, UserName: result.username, Password: '' ,WorkspaceID :0});

                        var UrlJson = "";
                        var NewUrl = "";
                        if (typeof window.BeforelogoutUrl != "undefined") {
                            UrlJson = window.BeforelogoutUrl;
                            if (UrlJson.length > 0) {
                                var temp = UrlJson.filter(x => x.UserID == result.userId)
                                if (temp.length > 0) {
                                    NewUrl = temp[0].Url;
                                }
                            }
                        }
                        
                        var Url = window.UserUrlpath;
                        if (NewUrl.length > 0) {
                            Url = NewUrl;
                        }
                        
                        window.UserUrlpath = "";
                        window.BeforelogoutUrl = "";
                        if (Url != "") {
                            var WSID = Url.split("?")[1];
                            // null check
                            if (typeof WSID != "undefined" && WSID != null) {
                                var UrlParameter = WSID.split("&")[0]; // WsID = 300;
                                var Isvalid = await this.ValidateUrl(UrlParameter);
                                //debugger;
                                if (Isvalid == true) {
                                    if (Url.includes('MyTasks')) {
                                        var selWSID = UrlParameter.split('=');
                                        if (selWSID.length > 1) {
                                            await dbImworks.User.where("UserId").equals(result.userId).modify({ WorkspaceID: selWSID[1] });
                                        }
                                       
                                        this.PreLoader = true;
                                        var lstTaskList = await dbImworks.MyTaskList.toArray();

                                        if (lstTaskList.length == 0) {
                                            await this.GetMyTaskFromServer();
                                        }
                                        this.PreLoader = false;
                                    }

                                    window.location.href = Url;
                                }
                            }
                            else {
                                var url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                                var result = await objWorkspace.GetData(url);
                                if (result.status == "success" && result.data.length > 0) {
                                    this.$router.push({ name: "workspace" });
                                }
                                else if (result.data.length == 0) {
                                    url = window.SERVER_URL + "/Workspace/GetSharedWorkspaceList";
                                    var result = await objWorkspace.GetData(url);
                                    if (result.data.length > 0) {
                                        this.$router.push({ name: "SharedWorkspace" });
                                    } else if (result.data.length == 0) {
                                        this.$router.push({ name: "workspace", query: { WSdialog: true }});
                                    }
                                }
                            }
                        }
                        else {
                            var url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                            var result = await objWorkspace.GetData(url);
                            if (result.status == "success" && result.data.length > 0) {
                                this.$router.push({ name: "workspace" });
                            }
                            else if (result.data.length == 0) {
                                url = window.SERVER_URL + "/Workspace/GetSharedWorkspaceList";
                                var result = await objWorkspace.GetData(url);
                                if (result.data.length > 0) {
                                    this.$router.push({ name: "SharedWorkspace" });
                                } else if (result.data.length == 0) {
                                    this.$router.push({ name: "workspace", query: { WSdialog: true }});
                                }
                            }
                        }
                        
                        //var obj = syncData;
                        //var result = await obj.syncData.SyncFromServer();
                        
                    } else {
                        this.Msg = ("login failed");
                        this.errorSnackbarMsg = true;
                    }
                } else {
                    this.Msg = ("please enter username and password.");
                    this.infoSnackbarMsg = true;
                }
            },
            async ValidateUrl(paramter) {
               // debugger;
                var lstAccessPageList = await dbImworks.TbPageAccessList.toArray();
                if (lstAccessPageList.length > 0) {
                    return true;
                }
                var WorkspaceID = parseInt(paramter.split("=")[1]);
                await dbImworks.User.where("IsLoggedIn").equals(1).modify({ WorkspaceID: WorkspaceID });
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
                    this.Msg = ("You don't have access to the workspace in entered url");
                    this.errorSnackbarMsg = true;
                    return false;
                    //this.$router.push({ name: "workspace" });
                }
            },
            async GetMyTaskFromServer() {
                var obj = syncData;
                var result = await obj.syncData.SyncFromServer();
            },
            async fnResetPassword() { 

                if(this.$refs.forgotPasswordForm.validate()){
                    
                    try {
                        let result = await usr.ForgotPassword({
                            emailId: this.resetEmail
                        })

                        if(!result.status) {
                            this.Msg = result.statusMsg
                            this.errorSnackbarMsg = true
                            throw new Error(result.statusMsg)
                        }
                        if(result.status) {
                            this.Msg = result.statusMsg
                            this.infoSnackbarMsg = true
                            this.resetEmail = ''
                            this.resetPassword = false

                        }
                    } catch (error) {
                        console.error(error)
                    }
                    
                    
                } else {
                    this.Msg = "Please Enter proper email address to reset your password"
                    this.errorSnackbarMsg = true;
                }
            }
            
        },
        components: {
            "Pre-Loader": PreLoader,
        },
    };
</script>

<style scoped>

    .reset-dlg{
        position: relative;
    }
    .reset-close-btn{
        position: absolute;
        top: 3px;
        right: 3px;
    }

</style>

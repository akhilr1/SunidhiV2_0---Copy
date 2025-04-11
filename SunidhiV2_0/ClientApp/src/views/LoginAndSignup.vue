<template>
    <v-container fluid class="main-container">
        <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
        <v-row class="fill-height pa-0 ma-0" style="height: 100vh">
            <!-- SidePanel -->
            <v-col cols="12" sm="5" md="4" class="pa-0 sidePanelCol">
                <v-sheet color="primary2" height="100%" class="sidePanel">
                    <!-- logo -->
                    <div class="logo">
                        <img :src="imwLogo" alt="">
                    </div>
                    <!-- MessageCard -->
                    <v-card flat color="transparent" class=" pb-6 messageCard">
                        <div class="message">
                            <v-card-title class="pb-0 mb-3 title">Welcome to Impact Works!</v-card-title>
                            <v-card-text>
                                <p class="mb-0">Impact Works is a powerful SaaS platform designed to provide end-to-end impact management solutions for both nonprofit and for-profit organizations. With a comprehensive set of features, including project management, data collection and analysis, fund management, impact measurement, secure collaboration, beneficiary information management, real-time monitoring, and detailed fund utilization reports, Impact Works empowers you to drive positive change and measure your contributions towards the Sustainable Development Goals (SDGs).</p>
                                <p class="my-2">Sign-up to avail our 30-day free trial period offer.</p>
                                <p class="mb-0">To subscribe to the paid Impact Works service and enjoy its full range of features, please contact our team at <span style="text-decoration: underline;">impactworks.sales@billionlives.in </span>. We'll be more than happy to assist you in setting up your account and getting started.</p>
                            </v-card-text>
                        </div>
                        <div class="worldImg">
                            <img :src="earth">
                        </div>
                    </v-card>
                    <!-- Footer Actions -->
                    <div class="footerActions">
                        <v-btn dark class="ma-1 footerActionBtn" small text :to="{name: 'termsAndConditions', query: { tab: 'faq'}}" target="_blank">Faq</v-btn>
                        <v-btn dark class="ma-1 footerActionBtn" small text :to="{ name: 'termsAndConditions', query: { tab: 'terms' } }" target="_blank">Terms & Conditions</v-btn>
                        <v-btn dark class="ma-1 footerActionBtn" small text :to="{ name: 'termsAndConditions', query: { tab: 'privacy' } }" target="_blank">Privacy Policy</v-btn>
                    </div>
                </v-sheet>
            </v-col>

            <!-- Main Content -->
            <v-col cols="12" sm="7" md="8" class="pa-0 d-flex flex-column justify-space-between">
                <div class="mainContent">
                    <v-card flat class="loginCard">
                        <v-tabs v-model="tab" slider-color="primary" centered height="36px">
                            <v-tab href="#tab-1">Login</v-tab>
                            <v-tab href="#tab-2">SignUp</v-tab>
                        </v-tabs>
                        <v-card-text>
                            <v-tabs-items v-model="tab">
                                <v-tab-item value="tab-1">
                                    <v-form @submit.prevent="fnLogin" lazy-validation ref="loginForm">
                                        <v-row dense>
                                            <v-col cols="12">
                                                <div class="md-2">Email ID</div>
                                            </v-col>
                                        </v-row>
                                        <v-row dense>
                                            <v-col cols="12">
                                                <v-text-field name="username" v-model="email" type="text" id="username" outlined
                                                    dense="" clearable color="blue"
                                                    :rules="[rules.required, rules.emailValidation]">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                        <v-row dense py-0 no-gutters>
                                            <v-col cols="12">
                                                <div>Password</div>
                                            </v-col>
                                        </v-row>
                                        <v-row dense>
                                            <v-col cols="12">
                                                <v-text-field name="password" v-model="passwd" id="password" outlined dense=""
                                                    :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                                    :type="showPassword ? 'text' : 'password'"
                                                    @click:append="showPassword = !showPassword"
                                                    :rules="[rules.required]"
                                                    >
                                                    <template v-slot:append>
                                                        <v-icon color="blue" @click="showPassword = !showPassword">{{
                                                            showPassword ? 'mdi-eye' : 'mdi-eye-off' }}</v-icon>
                                                    </template>
                                                </v-text-field>
                                            </v-col>
                                        </v-row>

                                        <v-card-text class="py-0 px-0">
                                            <v-row py-0 no-gutters>
                                                <v-col xs="6" sm="6">
                                                    <v-checkbox v-model="rememberMe"
                                                        label="Remember me for 30 days"></v-checkbox>
                                                </v-col>
                                                <v-col xs="6" sm="6" class="text-primary text-right my-auto">
                                                    <v-btn text @click="resetPassword = true" color="blue">Forgot Password
                                                        ?</v-btn>
                                                </v-col>
                                            </v-row>
                                        </v-card-text>

                                        <v-card-actions class="pb-0 px-0 pt-0 text-xs-center d-flex flex-column">
                                            <v-btn dark name="btnLogin" color="#399CFF" class="text-none login-btn" large block
                                                @click="fnLogin()">Get Started</v-btn>
                                        </v-card-actions>


                                    </v-form>
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
                                                            <v-text-field :rules="[rules.required, rules.resetEmailValidation]"
                                                                outlined label="E-Mail" v-model="resetEmail"
                                                                required></v-text-field>
                                                        </v-col>
                                                    </v-row>
                                                </v-form>
                                            </v-card-text>
                                            <v-card-actions class="d-flex justify-end pr-5 pb-5 pt-0">
                                                <v-btn color="blue" dark x-large @click="fnResetPassword()">Send</v-btn>
                                            </v-card-actions>
                                            <v-btn type="submit" icon class="reset-close-btn" :ripple="false"
                                                @click="resetPassword = false, resetEmail = ''">
                                                <v-icon>
                                                    mdi-close
                                                </v-icon>
                                            </v-btn>
                                        </v-card>
                                    </v-dialog>
                                </v-tab-item>
                                <v-tab-item value="tab-2">
                                    <v-stepper v-model="signupStep" class="elevation-0 stepper pa-0 ma-0">
                                        <v-stepper-items>
                                            <v-stepper-content step="1">
                                                <v-form @submit.prevent="signupHandler" v-model="validForm" lazy-validation
                                                        class="mx-0" ref="signupForm">
                                                    <div class="md-2">Name</div>
                                                    <v-text-field v-model="fname" required outlined dense="" clearable flat
                                                                  background-color="white" color="blue"
                                                                  :rules="[(val => (val || '').length > 0 || 'Required field')]"
                                                                  maxlength="48" />

                                                    <div>Email ID</div>
                                                    <v-text-field v-model="signupEmail" required outlined dense="" clearable
                                                                  flat background-color="white" color="blue"
                                                                  :rules="[rules.required, rules.emailValidation]" />
                                                    <div>Password</div>
                                                    <v-text-field v-model="password" required outlined dense="" flat
                                                                  background-color="white"
                                                                  :append-icon="showPassword1 ? 'mdi-eye' : 'mdi-eye-off'"
                                                                  :type="showPassword1 ? 'text' : 'password'"
                                                                  @click:append="showPassword1 = !showPassword1"
                                                                  :rules="[rules.required, rules.min, rules.passwordValidation]">
                                                        <template v-slot:append>
                                                            <v-icon color="blue" @click="showPassword1 = !showPassword1">
                                                                {{
                                                                showPassword1 ? 'mdi-eye' : 'mdi-eye-off'
                                                                }}
                                                            </v-icon>
                                                        </template>
                                                    </v-text-field>
                                                    <div class="password-tooltip-content">
                                                        <ul>
                                                            <li>Password must contain min 8 characters</li>
                                                            <li>Must contain one unique characters like @,_,&, .</li>
                                                            <li>Must contain one number</li>
                                                        </ul>
                                                    </div>
                                                    <br />
                                                    <div>Confirm Password</div>
                                                    <v-text-field v-model="cpassword" required outlined dense="" flat
                                                                  background-color="white"
                                                                  :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                                                  :type="showPassword ? 'text' : 'password'"
                                                                  @click:append="showPassword = !showPassword"
                                                                  :rules="[rules.required, passwordMatch]">
                                                        <template v-slot:append>
                                                            <v-icon color="blue" @click="showPassword = !showPassword">
                                                                {{
                                                                showPassword ? 'mdi-eye' : 'mdi-eye-off'
                                                                }}
                                                            </v-icon>
                                                        </template>
                                                    </v-text-field>
                                                    <div style="display: flex; align-items: center;">
                                                        <v-checkbox v-model="consent" class="pa-0 ma-0"></v-checkbox>
                                                        <div style="margin-bottom: 25px; font-size: 16px;">
                                                            Agree to
                                                            <router-link :to="{ name: 'termsAndConditions', query: { tab: 'terms' } }" target="_blank">
                                                                Terms & Conditions
                                                            </router-link> and
                                                            <router-link :to="{ name: 'termsAndConditions', query: { tab: 'privacy' } }" target="_blank">
                                                                Privacy Policy
                                                            </router-link>
                                                        </div>
                                                    </div>
                                                    <v-btn color="#399CFF" block style="border-radius: 5px;" dark
                                                           class="py-5 px-0 ma-0" type="submit">
                                                        Create Account
                                                    </v-btn>
                                                </v-form>
                                            </v-stepper-content>
                                            <v-stepper-content step="2">
                                                <v-sheet color="transparent">
                                                    <v-card-title class="mb-5 pl-0">
                                                        Email Verification
                                                    </v-card-title>
                                                    <v-card-subtitle class="pl-0">
                                                        We have sent you a code to your email :
                                                        {{ this.signupEmail }}
                                                    </v-card-subtitle>
                                                    <v-sheet width="350px" class="d-flex mx-auto align-center" flat tile
                                                        color="transparent">
                                                        <v-row>
                                                            <v-form lazy-validation @submit.prevent="otpVerificationHandler"
                                                                ref="otpFields">
                                                                <v-sheet class="mx-auto d-flex justify-center align-center"
                                                                    height="200px" color="transparent">
                                                                    <v-col xs="1">
                                                                        <v-text-field outlined background-color="white"
                                                                            color="#A4D1FF" class="otpField" hide-details
                                                                            height="75" id="otpfield1" v-model="otpfield1"
                                                                            @keyup="focusNext('otpfield1')" maxlength="1"
                                                                            :rules="[rules.required]" />
                                                                    </v-col>
                                                                    <v-col xs="1">
                                                                        <v-text-field outlined background-color="white"
                                                                            color="#A4D1FF" class="otpField" hide-details
                                                                            height="75" id="otpfield2" v-model="otpfield2"
                                                                            @keyup="focusNext('otpfield2')" maxlength="1"
                                                                            :rules="[rules.required]" />
                                                                    </v-col>
                                                                    <v-col xs="1">
                                                                        <v-text-field outlined background-color="white"
                                                                            color="#A4D1FF" class="otpField" hide-details
                                                                            height="75" id="otpfield3" v-model="otpfield3"
                                                                            @keyup="focusNext('otpfield3')" maxlength="1"
                                                                            :rules="[rules.required]" />
                                                                    </v-col>
                                                                    <v-col xs="1">
                                                                        <v-text-field outlined background-color="white"
                                                                            color="#A4D1FF" class="otpField" hide-details
                                                                            height="75" id="otpfield4" v-model="otpfield4"
                                                                            @keyup="focusNext('otpfield4')" maxlength="1"
                                                                            :rules="[rules.required]" />
                                                                    </v-col>
                                                                </v-sheet>
                                                                <v-btn class="float-right pa-0" text color="blue"
                                                                    @click.prevent="signupHandler" :disabled="!!otpResetTimer">
                                                                    {{ otpResetTimer ? `Resend Code in ${otpResetTimer}s` :
                                                                        'Resend Code' }}
                                                                </v-btn>
                                                                <v-btn color="#399CFF" block depressed dark class="py-8 ma-0"
                                                                    type="submit">
                                                                    Verify Account
                                                                </v-btn>
                                                            </v-form>
                                                        </v-row>
                                                    </v-sheet>
                                                </v-sheet>
                                            </v-stepper-content>
                                        </v-stepper-items>
                                    </v-stepper>
                                </v-tab-item>
                            </v-tabs-items>
                        </v-card-text>
                    </v-card>
                </div>
                <div class="footerActionsMobile">
                    <v-btn dark class="ma-1 footerActionBtn" small text :to="{ name: 'termsAndConditions', query: { tab: 'faq' } }" target="_blank">Faq</v-btn>
                    <v-btn dark class="ma-1 footerActionBtn" small text :to="{ name: 'termsAndConditions', query: { tab: 'terms' } }" target="_blank">Terms & Conditions</v-btn>
                    <v-btn dark class="ma-1 footerActionBtn" small text :to="{ name: 'termsAndConditions', query: { tab: 'privacy' } }" target="_blank">Privacy Policy</v-btn>
                </div>
            </v-col>
        </v-row>

        <v-snackbar v-model="snackbar.value" :color="snackbar.color" top>
            {{ snackbar.message }}
        </v-snackbar>
    </v-container>
</template>

<script>
import { dbImworks } from '../ImworksDb'
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
    components: {
        "Pre-Loader": PreLoader,
    },
    computed: {
        passwordMatch() {
            return this.password === this.cpassword || 'Password must match'
        }
    },
    async mounted() {
        await importscript();
    },
    data() {
        return {
            imwLogo: require('@/assets/apurthi_login.png'),
            earth: require('@/assets/Signup/Side_Earth.svg'),
            tab: 'tab-1',
            email: "",
            passwd: "",
            rememberMe: false,
            loginForm: null,
            showPassword: false,
            showPassword1: false,
            resetPassword: false,
            buffedPassword: null,
            resetEmail: '',
            signupStep: 1,
            fname: null,
            signupEmail: null,
            password: null,
            cpassword: null,
            consent: false,
            signupForm: null,
            otpfield1: null,
            otpfield2: null,
            otpfield3: null,
            otpfield4: null,
            otpfield5: null,
            validForm: false,
            rules: {
                required: val => !!val || 'Required',
                emailValidation: val => /.+@.+\..+/.test(val) || 'E-mail must be valid',
                resetEmailValidation: val => (val && /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g.test(val)) || 'Enter Valid Email Address',
                min: val => (val && val.length >= 8) || 'Your password must have at least 8 characters',
                passwordValidation: val => (val && /^(?=.*\d)(?=.*[@_&.])/g.test(val)) || 'Your password must have one number and one unique character (@, _, &, .)'
            },
            otpResetTimer: 0,
            PreLoader: false,
            snackbar: {
                message: '',
                value: false,
                color: 'primary1',
            }
        };
    },
    methods: {
        focusNext(e) {
            let n = e.toString().match(/\d+/g)
            let val = this[e]
            if (val.length < 1) {
                e !== 'otpfield1' && document.getElementById(`otpfield${parseInt(n[0]) - 1}`).select()
            }
            if (!val.match(/\d+/g)) {
                this[e] = ""
                return
            }
            if (val.length >= 1) {
                // this[e] = this[e].split('')[0]
                e !== 'otpfield4' && document.getElementById(`otpfield${parseInt(n[0]) + 1}`).select()
            }

        },
        async fnLogin() {
            //  debugger;
            if (this.email != "" && this.passwd != "") {

                // password is passed to the backend as a base64 encoded string instead of plain string
                this.buffedPassword = Buffer.from(this.passwd).toString('base64');
                var notificationtoken = localStorage.getItem("notificationToken");
                var result = await usr.Login(this.email, this.buffedPassword, notificationtoken);

                if (result.status) {

                    // dexie Transaction
                    
                    const tablesToClear = [
                        dbImworks.User,
                        dbImworks.TaskSettings,
                        dbImworks.MyTaskList,
                        dbImworks.TaskSyncDB,
                        dbImworks.TbMyTask,
                        dbImworks.SavedTaskDB,
                        dbImworks.TbUpdateTaskData,
                        dbImworks.TbTempResposeData,
                        dbImworks.TbLoggedUserDetails,
                        dbImworks.TbPageAccessList,
                        dbImworks.TbDashboardProjectList,
                        dbImworks.TbNewBeneficiary,
                        dbImworks.TbAnyTimeData,
                        dbImworks.TbAnyTimeTempData,
                        dbImworks.MyTaskDM,
                        dbImworks.MyTaskSurveySection,
                        dbImworks.MyTaskSurveyQuestion,
                        dbImworks.MyTaskSurveySectionLogic,
                        dbImworks.MyTaskSurveyTrackDate,
                        dbImworks.TbMyTaskTable,
                        dbImworks.MyTaskProjectSurveyQSTCondition,
                        dbImworks.MyTaskProjectBenSurveyQSTCondition,
                        dbImworks.MyTaskProjectSurveyQSTOption,
                        dbImworks.MyTaskSurveyCascadingQuestionOptions,
                        dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions,
                        dbImworks.TbMyTaskProjectLogicConditions,
                        dbImworks.TbMyTaskProjectBeneficiaryType,
                        dbImworks.TbMyTaskBeneficiaryAnswerType,
                        dbImworks.TbMyTaskBeneficiaryRegistrationDetails,
                        dbImworks.TbMyTaskProjectBeneficiary,
                        dbImworks.MyTaskBeneficiaryTypeQuestion,
                        dbImworks.MyTaskBeneficiaryTypeQuestionOption,
                        dbImworks.MyTaskMasterList,
                        dbImworks.MyTaskQuestionType,
                        dbImworks.MyTaskProjectSurveyResponses,
                        //dbImworks.TbTaskAuditLog,
                        dbImworks.TbTaskResponseDocument,
                        dbImworks.TbTaskBenefRegDocument
                    ];

                    dbImworks.transaction('rw', tablesToClear, async () => {
                        for (const table of tablesToClear) {
                            await table.clear();
                        }
                    })
                    .then(() => {
                        console.log("Index Database Cleared");
                    })
                    .catch(error => {
                        console.error("IndexDB Error:", error);
                    });

                    window.LoggedUserID = result.userId;
                    sessionStorage.setItem('LoggedUserID', result.userId);


                    var lstUser = await dbImworks.User.toArray();
                    if (lstUser.length == 0)
                        await dbImworks.User.put({ UserId: result.userId, IsLoggedIn: 1, UserName: result.username, Password: '', WorkspaceID: 0 });

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
                                    this.$router.push({ name: "workspace", query: { WSdialog: true } });
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
                                this.$router.push({ name: "workspace", query: { WSdialog: true } });
                            }
                        }
                    }

                    //var obj = syncData;
                    //var result = await obj.syncData.SyncFromServer();

                } else {
                    this.snackbar = {
                        message: "login failed",
                        value: true,
                        color: 'error'
                    };
                }
            }

            else {
                this.snackbar = {
                    message: "please enter username and password.",
                    value: true,
                    color: 'warning'
                }
            }
        },

        async ValidateUrl(paramter) {
            var lstAccessPageList = await dbImworks.TbPageAccessList.toArray();
            if (lstAccessPageList.length > 0) {
                return true;
            }
            var WorkspaceID = parseInt(paramter.split("=")[1]);
            await dbImworks.User.where("IsLoggedIn").equals(1).modify({ WorkspaceID: WorkspaceID });
            var data = new FormData();
            data.append("WorkspaceID", WorkspaceID);
            var result = await usr.FetchUserAccessPagesList(data);

            if (result.data.length > 0) {

                await dbImworks.TbPageAccessList.clear();
                await dbImworks.TbPageAccessList.bulkPut(result.data);
                return true;
            }
            else {
                this.snackbar = {
                    message: "You don't have access to the workspace in entered url",
                    value: true,
                    color: 'error'
                }
                return false;
                //this.$router.push({ name: "workspace" });
            }
        },
        async GetMyTaskFromServer() {
            var obj = syncData;
            var result = await obj.syncData.SyncFromServer();
        },
        async fnResetPassword() {

            if (this.$refs.forgotPasswordForm.validate()) {

                try {
                    let result = await usr.ForgotPassword({
                        emailId: this.resetEmail
                    })

                    if (!result.status) {
                        this.snackbar = {
                            message: result.statusMsg,
                            value: true,
                            color: 'error'
                        }
                        throw new Error(result.statusMsg)
                    }
                    if (result.status) {
                        this.snackbar = {
                            message: result.statusMsg,
                            value: true,
                            color: 'warning'
                        }
                        this.resetEmail = ''
                        this.resetPassword = false

                    }
                } catch (error) {
                    console.error(error)
                }


            } else {
                this.snackbar = {
                    message: "Please Enter proper email address to reset your password",
                    value: true,
                    color: 'error'
                }
            }
        },

        async signupHandler(e, x) {
            if (!this.consent) {
                this.snackbar = {
                    message: "Please Read and Agree the Terms & Conditions and Privacy Policy",
                    value: true,
                    color: 'error'
                }
            }
            if (this.consent) {
                if (this.$refs.signupForm.validate()) {
                    this.PreLoader = true
                    // password is passed to the backend as a base64 encoded string instead of plain string
                    this.buffedPassword = Buffer.from(this.password).toString('base64')

                    try {
                        let result = await usr.Signup({
                            Name: this.fname,
                            email: this.signupEmail,
                            password: this.buffedPassword
                        })

                        if (!result.status && result.mailerror) {
                            this.snackbar = {
                                message: result.mailerror,
                                value: true,
                                color: 'error'
                            }
                        }

                        if (result && result.status) {
                            this.PreLoader = false
                            this.signupStep < 2 && this.signupStep++

                            this.otpResetTimer = 120
                            let otpTimer = setInterval(() => {
                                this.otpResetTimer--
                                if (this.otpResetTimer == 0) clearInterval(otpTimer)
                            }, 1000)
                        }

                    } catch (error) {
                        console.log(error)
                        this.PreLoader = false
                    }
                } else {
                    this.snackbar = {
                        message: "Please check and fill all the fields",
                        value: true,
                        color: 'error'
                    }
                }

            }
        },

        async otpVerificationHandler() {
            let otpValue = ""
            for (let i = 1; i <= 4; i++) {
                if (!this[`otpfield${i}`]) {
                    this.snackbar = {
                        message: "Please check the otp entered",
                        value: true,
                        color: 'error'
                    }
                    return
                } else {
                    otpValue += this[`otpfield${i}`]
                }
            }

            try {
                this.PreLoader = true
                let result = await usr.VerifySignup({
                    email: this.signupEmail,
                    otpValue: otpValue
                })

                if (result && result.status == "") {
                    this.PreLoader = false
                    this.snackbar = {
                        message: 'Invalid OTP, Please check the otp entered',
                        value: true,
                        color: 'error'
                    }
                }

                if (result && !!result.status) {
                    this.PreLoader = false
                    this.snackbar = {
                        message: result.status,
                        value: true,
                        color: 'error'
                    }
                }

                if (result.status == 'success') {
                    this.snackbar = {
                        message: "Signup successful",
                        value: true,
                        color: 'success'
                    }

                    this.email = this.signupEmail;
                    this.passwd = this.password;
                    await this.fnLogin()

                }

            } catch (error) {
                console.log(error)
                this.PreLoader = false
            }

        },
    },
}
</script>

<style lang="scss" scoped>
.main-container {
    height: 100vh;
    width: 100vw;
    overflow: hidden;
    overflow-y: scroll;
    margin: -12px;
    padding: 0;

    @media screen and (max-width: 768px) {
        &{
            overflow-y: scroll;
            height: auto;
        }
    }
}

.sidePanelCol {
    
    width: 31.56rem;
    .sidePanel {
        border-radius: 0px 16px 16px 0px !important;
        display: flex;
        flex-direction: column;
        justify-content: space-between;

        @media screen and (max-width: 769px) {
            border-radius: 0px !important;
        }
    }
}

.logo {
    width: max-content;
    padding-top: 2rem;
    padding-left: 1rem;

    img {
        padding: 1rem;
        width: auto;
        height: 5.75rem;
        object-fit: contain !important;
    }

    @media screen and (max-width: 768px) {
        padding: 1rem;
        padding-bottom: 0px;

        img {
            padding-bottom: 0px;
        }
    }
}

.messageCard{
    color: white !important;
    // outline: 2px solid red;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    margin: 0px 1.1875rem;
    padding: auto 0px;
    width: fit-content;
    margin-bottom: 2.5rem;

    p{
        color: white !important;
        font-size: 0.8rem;
        line-height: normal;
    }

    .message{
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: center;

        .title {
            font-size: 1.375rem !important;
        }
    }
    .worldImg{
        height: min-content;
        display: grid;
        place-items: center;
        img {
            width: 11.88rem;
        }
    }

    &::before{
        content: url("../assets/Signup/Filler_Top.svg");
        position: absolute;
        top: -50px;
        right: 0px;
        height: 4.25rem;
        width: auto;
    }
    &::after{
        content: url("../assets/Signup/Filler_bottom.svg");
        position: absolute;
        bottom: 0px;
        left: 0px;
        height: 4.25rem;
        width: auto;
    }

    @media screen and (max-width: 768px) {

        margin-bottom: 0px;

        .worldImg{
            display: none;
        }

        &::before{
            display: none;
        }

        &::after{
            display: none;
        }
    }
}

.mainContent{
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;

    .loginCard{
        border: 1px solid #C2E0FF;
        border-radius: 12px !important;
        width: 50%;
    }

    @media screen and (max-width: 768px) {
        &{
            height: auto;
            display: block;
            .loginCard{
                margin: 3px;
                width: 99%;
            }
        }
    }
}

.footerActions {
    display: flex;
    justify-content: space-evenly;
    align-items: baseline;
    flex-wrap: wrap;

    @media screen and (max-width: 768px) {
        display: none;
    }
}

.footerActionsMobile{
    display: none;
    
    @media screen and (max-width: 768px) {
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: #399CFF;
    }
}
</style>
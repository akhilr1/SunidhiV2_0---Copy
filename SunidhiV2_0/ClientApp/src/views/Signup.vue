<template>
    <v-container fluid class="pa-0 ma-0">
        <pre-loader v-show="PreLoader"/>
        <v-sheet class="form-container pa-0 ma-0" height="100vh" tile>
            <v-row class="fill-height">
                <v-col xs="12" md="6" class="pa-0 ma-0">
                    <v-sheet class="fill-height d-flex flex-column justify-space-between" tile>
                        <v-col class=" d-flex flex-column shrink">
                            <v-img
                                :src="logo"
                                height="100px"
                                contain
                                class="float-left"
                                width="50%"
                            ></v-img>
                        </v-col>
                        <v-col class=" mt-5 d-flex flex-column">
                            <v-card-title class="ml-12 ma-0 pa-0 "> 
                                Easy Impact Measurement & Management
                            </v-card-title>
                            <v-carousel 
                                v-model="carouselCursor" 
                                :show-arrows="false" 
                                hide-delimiter-background
                                delimiter-icon="mdi-minus"
                                continuous 
                                cycle
                                interval="3000"
                                class="my-auto"
                                height="410"
                            >
                                <v-carousel-item
                                    v-for="(item, index) in carouselPics"
                                    :key="index"
                                    eager
                                    class=""
                                >
                                    <v-img max-height="350" :src="item" contain class=" ma-1">

                                    </v-img>
                                </v-carousel-item>
                            </v-carousel>
                        </v-col>
                    </v-sheet>
                </v-col> 
                <v-col xs="12" md="6" class="d-flex justify-center align-center">
                    <v-stepper v-model="signupStep" class="elevation-0 stepper pa-0 ma-0">
                        <v-stepper-items>
                            <v-stepper-content step="1">
                                <v-sheet color="transparent">
                                    <v-form @submit.prevent="signupHandler" v-model="validForm" lazy-validation ref="signupForm">
                                        <v-card-title>
                                            Sign up with ImWorks
                                        </v-card-title>
                                        <v-col cols="12"  class="py-0">
                                            <v-text-field 
                                                v-model="fname" 
                                                label="First Name" 
                                                required 
                                                outlined 
                                                clearable 
                                                flat 
                                                background-color="white"
                                                :rules="[(val => (val || '').length > 0 || 'Required field')]"
                                                maxlength="48"
                                            />
                                        </v-col>
                                        <v-col cols="12" class="py-0">
                                            <v-text-field 
                                                v-model="lname" 
                                                label="Last Name" 
                                                required 
 
                                                outlined 
                                                clearable 
                                                flat 
                                                background-color="white"
                                                :rules="[(val => (val || '').length > 0 || 'Required field'), ]"
                                                maxlength="48"
                                            />
                                        </v-col>
                                        <v-col cols="12" class="py-0">
                                            <v-text-field 
                                                v-model="email" 
                                                label="Work Email Address" 
                                                required 
                                                outlined 
                                                clearable 
                                                flat 
                                                background-color="white"
                                                :rules="[
                                                    v => !!v || 'Required',
                                                    v => /.+@.+\..+/.test(v) || 'E-mail must be valid'
                                                ]"
                                            />
                                        </v-col>
                                        <v-tooltip top nudge-top open-delay="1500" close-delay="1000" color="primary" dark>
                                            <template v-slot:activator="{on, attrs}">
                                                <v-col cols="12" class="py-0">
                                                    <v-text-field 
                                                        v-bind="attrs"
                                                        v-on="on"
                                                        v-model="password"
                                                        label="Password" 
                                                        type="password" 
                                                        required 
                                                        outlined 
                                                        clearable 
                                                        flat 
                                                        background-color="white"
                                                        :rules="[rules.required, rules.min, rules.max, rules.passwordValidation]"
                                                    />
                                                </v-col>
                                            </template>
                                            <span class="pa-2">
                                                <ul>
                                                    <li>The Password must be 8 - 32 characters long</li>
                                                    <li>The Password must have atleast 1 Number</li>
                                                    <li>The Password must have atleast 1 Capital Letter</li>
                                                    <li>The Password must have atleast 1 Special Character</li>
                                                </ul>
                                            </span>
                                        </v-tooltip>
                                        <v-col cols="12" class="py-0">
                                            <v-text-field 
                                                v-model="cpassword"
                                                label="Confirm Password" 
                                                type="password" 
                                                required 
 
                                                outlined 
                                                clearable 
                                                flat 
                                                background-color="white"
                                                :rules="[rules.required, passwordMatch ]"
                                            />
                                        </v-col>
                                        <v-col cols="12">
                                            <v-checkbox
                                                v-model="consent"
                                                 class="pa-0"
                                            >
                                            <template v-slot:label>
                                              <div> I have read and agree to the 
                                                <a @click.stop href="/app/TermsAndService.html" target="_blank"> Terms and service </a> and 
                                                <a @click.stop  href="/app/PrivacyPolicy.html" target="_blank"> Privacy Policy</a>
                                              </div> 
                                            </template>
                                        </v-checkbox>
                                        </v-col>
                                        <v-btn 
                                            color="#399CFF" 
                                            block 
                                            depressed 
                                            dark 
                                            class="py-8 ma-0"
                                            type="submit"
                                        >
                                            Sign Up
                                        </v-btn>
                                    </v-form>
                                </v-sheet>
                            </v-stepper-content>
                            <v-stepper-content step="2">
                                <v-sheet color="transparent">
                                    <v-card-title class="mb-5 pl-0">
                                        Email Verification
                                    </v-card-title>
                                    <v-card-subtitle class="pl-0">
                                        We have sent you a code to your email :
                                        {{this.email}}
                                    </v-card-subtitle>
                                    <v-sheet 
                                        width="350px" 
                                        class="d-flex mx-auto align-center"
                                        flat 
                                        tile
                                        color="transparent"
                                    >
                                        <v-row>
                                            <v-form lazy-validation @submit.prevent="otpVerificationHandler" ref="otpFields">
                                                <v-sheet class="mx-auto d-flex justify-center align-center" height="200px"  color="transparent">
                                                    <v-col xs="1">
                                                        <v-text-field
                                                            outlined
                                                            background-color="white"
                                                            color="#A4D1FF"
                                                            class="otpField"
                                                            hide-details
                                                            height="75"
                                                            id="otpfield1"
                                                            v-model="otpfield1"
                                                            @keyup="focusNext('otpfield1')"
                                                            maxlength="1"
                                                            :rules="[rules.required]"
                                                        />
                                                    </v-col>
                                                    <v-col xs="1">
                                                        <v-text-field
                                                            outlined
                                                            background-color="white"
                                                            color="#A4D1FF"
                                                            class="otpField"
                                                            hide-details
                                                            height="75"
                                                            id="otpfield2"
                                                            v-model="otpfield2"
                                                            @keyup="focusNext('otpfield2')"
                                                            maxlength="1"
                                                            :rules="[rules.required]"
                                                        />
                                                    </v-col>
                                                    <v-col xs="1">
                                                        <v-text-field
                                                            outlined
                                                            background-color="white"
                                                            color="#A4D1FF"
                                                            class="otpField"
                                                            hide-details
                                                            height="75"
                                                            id="otpfield3"
                                                            v-model="otpfield3"
                                                            @keyup="focusNext('otpfield3')"
                                                            maxlength="1"
                                                            :rules="[rules.required]"
                                                        />
                                                    </v-col>
                                                    <v-col xs="1">
                                                        <v-text-field
                                                            outlined
                                                            background-color="white"
                                                            color="#A4D1FF"
                                                            class="otpField"
                                                            hide-details
                                                            height="75"
                                                            id="otpfield4"
                                                            v-model="otpfield4"
                                                            @keyup="focusNext('otpfield4')"
                                                            maxlength="1"
                                                            :rules="[rules.required]"
                                                        />
                                                    </v-col>
                                                </v-sheet>
                                                <v-btn class="float-right pa-0" text color="blue" @click.prevent="signupHandler" :disabled="!!otpResetTimer">
                                                    {{otpResetTimer ? `Resend Code in ${otpResetTimer}s` : 'Resend Code'}}
                                                </v-btn>
                                                <v-btn color="#399CFF" block depressed dark class="py-8 ma-0" type="submit">
                                                    Verify Account
                                                </v-btn>
                                            </v-form>
                                        </v-row>
                                    </v-sheet>
                                </v-sheet>
                            </v-stepper-content>
                        </v-stepper-items>
                    </v-stepper>

                    <v-snackbar app v-model="snackStatus1.value" top light :timeout="5000" color="error">
                        {{snackStatus1.message}}
                    </v-snackbar>
                    <v-snackbar app v-model="successSnack.value" top light :timeout="5000" color="success">
                        {{successSnack.message}}
                    </v-snackbar>
                </v-col> 
            </v-row>
        </v-sheet>
    </v-container>
</template>

<script>
import PreLoader from "@/components/pre-loader.vue";
import { dbImworks } from '../ImworksDb'
var user, objWorkspace;
async function importscript() {
    return Promise.all([
        import("../BL/User.js").then(mod => {
            user = new mod.User();               
        }),
        import("../BL/Workspace.js").then(mod => {
            objWorkspace = new mod.Workspace();
        })
    ]);
}

import pic1 from '@/assets/Signup/carousel1.svg'
import pic2 from '@/assets/Signup/carousel2.svg'
import pic3 from '@/assets/Signup/carousel3.svg'
import logo from '@/assets/Signup/imLogo.svg'

export default {
    components: {
        PreLoader,
    },
    data(){
        return {
            carouselPics: [
                pic1,
                pic2,
                pic3
            ],
            logo,
            signupStep: 1,
            fname: null,
            lname: null,
            email: null,
            password: null,
            cpassword: null,
            consent: false,
            signupForm: null,
            carouselCursor: null,
            otpfield1: null,
            otpfield2: null,
            otpfield3: null,
            otpfield4: null,
            otpfield5: null,
            validForm: false,
            rules: {
                required: val => !!val || 'Required',
                min: val => (val && val.length >= 8) || 'Your password must have at least 8 characters',
                max: val => (val && val.length <= 32) || 'Your password must not have more than 32 characters',
                passwordValidation: val => (val && /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{0,}$/g.test(val)) || 'Your password must have 1 Capital letter, 1 Number and 1 Special character'
            },
            snackStatus1: {
                value: false,
                message: "Default Message"
            },
            successSnack: {
                value : false,
                message : "Default success message"
            },
            otpResetTimer : 0,
            PreLoader: false,
        }
    },
    async mounted(){
        await importscript();
    },
    computed: {
        passwordMatch(){
            return this.password === this.cpassword || 'Password must match'
        }
    },
    methods: {
        focusNext(e){
            let n = e.toString().match(/\d+/g)
            let val = this[e]
            if(val.length < 1) {
                e !== 'otpfield1' && document.getElementById(`otpfield${parseInt(n[0])-1}`).select()
            }
            if(!val.match(/\d+/g)){
                this[e] = ""
                return
            }
            if(val.length >= 1) {
                // this[e] = this[e].split('')[0]
                e !== 'otpfield4' && document.getElementById(`otpfield${parseInt(n[0])+1}`).select()
            }
            
        },
        async signupHandler(e,x) {
            if(!this.consent){
                this.snackStatus1.message = "Please Read and Agree the Terms and Service and Privacy Policy "
                this.snackStatus1.value = true
            }
            if(this.consent){
                if(this.$refs.signupForm.validate()){
                    this.PreLoader = true
                    // password is passed to the backend as a base64 encoded string instead of plain string
                    let buffedPassword = Buffer.from(this.password).toString('base64')

                    try {
                        let result = await user.Signup({
                            fullName: this.fname,
                            lastName: this.lname,
                            email: this.email, 
                            password: buffedPassword
                        })

                        if(!result.status && result.mailerror){
                            this.snackStatus1.message = result.mailerror
                            this.snackStatus1.value = true
                        }

                        if(result && result.status) {
                            this.PreLoader = false
                            this.signupStep < 2 && this.signupStep++
                            
                            this.otpResetTimer = 120
                            let otpTimer = setInterval(() => {
                                this.otpResetTimer--
                                if(this.otpResetTimer == 0) clearInterval(otpTimer)
                            }, 1000)
                        }

                    } catch (error) {
                        console.log(error)
                        this.PreLoader = false
                    }
                } else {
                    this.snackStatus1.message = "Please check and fill all the fields"
                    this.snackStatus1.value = true
                }
                
            }
        },
        async otpVerificationHandler(){
            let otpValue = ""
            for(let i=1; i<=4; i++){
                if(!this[`otpfield${i}`]){
                    this.snackStatus1.message = "Please check the otp entered"
                    this.snackStatus1.value = true
                    return
                } else {
                    otpValue += this[`otpfield${i}`]
                }
            }
            
            try {
                this.PreLoader = true
                let result = await user.VerifySignup({
                    email: this.email, 
                    otpValue: otpValue
                })
                
                if(result && result.status == ""){
                    this.PreLoader = false
                    this.snackStatus1.message = 'Invalid OTP, Please check the otp entered'
                    this.snackStatus1.value = true
                }
                
                if(result && !!result.status){
                    this.PreLoader = false
                    this.snackStatus1.message = result.status
                    this.snackStatus1.value = true
                }

                if(result.status == 'success'){
                    this.successSnack.message = "Signup successful"
                    this.successSnack.value = true

                    await this.fnLogin()
                }

            } catch (error) {
                console.log(error)
                this.PreLoader = false
            }
            
        },
        async fnLogin() {
              //  debugger;
                if (this.email != "" && this.password != "") {
                    let notificationtoken = localStorage.getItem("notificationToken");
                    let result = await user.Login(this.email, this.password, notificationtoken)

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


                        let lstUser = await dbImworks.User.toArray();
                        if (lstUser.length == 0)
                            await dbImworks.User.put({ UserId: result.userId, IsLoggedIn: 1, UserName: result.username, Password: '' ,WorkspaceID :0});

                        let UrlJson = "";
                        let NewUrl = "";
                        if (typeof window.BeforelogoutUrl != "undefined") {
                            UrlJson = window.BeforelogoutUrl;
                            if (UrlJson.length > 0) {
                                let temp = UrlJson.filter(x => x.UserID == result.userId)
                                if (temp.length > 0) {
                                    NewUrl = temp[0].Url;
                                }
                            }
                        }
                        
                        let Url = window.UserUrlpath;
                        if (NewUrl.length > 0) {
                            Url = NewUrl;
                        }
                        
                        window.UserUrlpath = "";
                        window.BeforelogoutUrl = "";
                        if (Url != "") {
                            let WSID = Url.split("?")[1];
                            // null check
                            if (typeof WSID != "undefined" && WSID != null) {
                                let UrlParameter = WSID.split("&")[0]; // WsID = 300;
                                let Isvalid = await this.ValidateUrl(UrlParameter);
                                //debugger;
                                if (Isvalid == true) {
                                    if (Url.includes('OverdueTasks') || Url.includes('CurrentTasks') || Url.includes('CompletedTasks') || Url.includes('SavedTasks')) {

                                        this.PreLoader = true;
                                        let lstTaskList = await dbImworks.MyTaskList.toArray();

                                        if (lstTaskList.length == 0) {
                                            await this.GetMyTaskFromServer();
                                        }
                                        this.PreLoader = false;
                                    }

                                    window.location.href = Url;
                                }
                            }
                            else {
                                let url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                                let result = await objWorkspace.GetData(url);
                                if (result.status == "success" && result.data.length > 0) {
                                    this.$router.push({ name: "workspace" });
                                }
                                else if (result.data.length == 0) {
                                    url = window.SERVER_URL + "/Workspace/GetSharedWorkspaceList";
                                    let result = await objWorkspace.GetData(url);
                                    if (result.data.length > 0) {
                                        this.$router.push({ name: "SharedWorkspace" });
                                    } else if (result.data.length == 0) {
                                        this.$router.push({ name: "workspace", query: { WSdialog: true }});
                                    }
                                }
                            }
                        }
                        else {
                            let url = window.SERVER_URL + "/Workspace/GetMyWorkspaceList";
                            let result = await objWorkspace.GetData(url);
                            if (result.status == "success" && result.data.length > 0) {
                                this.$router.push({ name: "workspace" });
                            }
                            else if (result.data.length == 0) {
                                url = window.SERVER_URL + "/Workspace/GetSharedWorkspaceList";
                                let _result = await objWorkspace.GetData(url);
                                if (_result.data.length > 0) {
                                    this.$router.push({ name: "SharedWorkspace" });
                                } else if (_result.data.length == 0) {
                                    this.$router.push({ name: "workspace", query: { WSdialog: true }});
                                }
                            }
                        }
                        
                        //var obj = syncData;
                        //var result = await obj.syncData.SyncFromServer();
                        
                    } else {
                        this.snackStatus1.message = 'Login failed'
                        this.snackStatus1.value = true
                    }
                } else {
                    this.$router.push({ name : "login"})
                }
            },
    },
}
</script>

<style scoped lang="scss">
.check {
    outline: 2px solid black;
}
.form-container{
    background: #EBF5FF;
    /* background: lightcoral; */
    padding: 20px 20px
}
.stepper{
    background: transparent;
}

.otpField{
    border-radius: 10px;
}

:deep(.otpField fieldset){
    border: 1.8px solid #A4D1FF;
    border-radius: 10px;
}

:deep(.otpField .v-text-field__slot input){
    font-size: 2rem;
    text-align: center;
}
</style>
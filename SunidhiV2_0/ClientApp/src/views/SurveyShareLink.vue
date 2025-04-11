<template>
    <v-container fluid style="background:#F5F6F8 !important">
        <Pre-Loader v-show="PreLoader == true"></Pre-Loader>
        <Pre-Loader v-show="InitialPreLoader == true"></Pre-Loader>
        <v-app-bar app light dense height="55">
            <v-toolbar-title class="pl-0">
                <img :src="imWorkLogo"
                     class="hidden-md-and-up"
                     width="auto"
                     max-width="300"
                     height="55"
                     v-bind:style="[
          mini == false ? { display: 'none' } : { display: 'block' },
        ]" />
                <img src="../images/apurthi_logo.png"
                     class="hidden-sm-and-down main-logo"
                     v-bind:style="[
          mini == false ? { display: 'none' } : { display: 'block' },
        ]" />
            </v-toolbar-title>
        </v-app-bar>
        <!--when sharelink is not active-->
        <v-container v-show="sharelinkStatusMesg" class="py-0 my-0" style="background-color: #F5F6F8;min-height:79.2vh">
            <v-row align="center" style="height:70vh">
                <v-col cols="12" align-self="center" class="align-center justify-center">
                    <v-row align="center" style="background-color: white; width:80%;margin-left:10%">
                        <v-col cols="12" md="4" align-self="center" class="align-center justify-center">
                            <div class="text-center pa-0 ma-0">
                                <v-img :src="shareLinkErrorImage" class=" hidden-sm-and-down text-center center-block"
                                       alt="logoImage" height="60%" width="100%"></v-img>
                                <v-img :src="shareLinkErrorImage" class=" hidden-md-and-up center-block"
                                       alt="logoImage" height="70%" width="70%"></v-img>
                            </div>
                        </v-col>
                        <v-col cols="12" sm="0" class="hidden-sm-and-down" md="1"></v-col>
                        <v-col cols="12" md="7" class="text-center text-md-left">
                            <h4 class="hidden-md-and-up">Something went wrong <br /> please check the link provided<br /> with you.</h4>
                            <h2 class="hidden-sm-and-down">Something went wrong <br /> please check the link provided<br /> with you.</h2>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>

        </v-container>
        <!--when share link is active-->
        <v-container v-show="!sharelinkStatusMesg" class="py-0 my-0" style="background-color: #F5F6F8;min-height:79.2vh">
            <v-row no-gutters justify="center" class="pa-0 ma-0">
                <v-col cols="12" class="pa-0 ma-0">
                    <v-card style="background-color: #F5F6F8" elevation="0" class="pa-0 ma-0">
                        <v-card-title class="pa-0 ma-0 pb-3">
                            {{SelectedSurveyName}}
                        </v-card-title>
                        <!---->
                        <v-row no-gutters style="background-color: #F5F6F8;" v-if="IsBeneficiaryLinkedSurvey">
                            <v-col cols="12">
                                <v-stepper v-model="SurveyShareSteps" class="elevation-0">
                                    <v-stepper-items>
                                        <!--select beneficiaries-->
                                        <v-stepper-content step="1" class="pa-0 px-0" style="background-color: #F5F6F8;">
                                            <v-row no-gutters style="background-color: #F5F6F8;">
                                                <v-col class="d-flex">
                                                    <span>Please select the beneficiaries you would want for the survey</span> <v-spacer></v-spacer>
                                                    <v-btn v-on="on" height="39" dense="" style="background-color:white;" class="" @click="GetBenefRegistrationQstns()">
                                                        <v-icon>mdi-plus</v-icon>
                                                        <span>Add</span>
                                                    </v-btn>
                                                </v-col>
                                            </v-row>
                                            <v-row no-gutters style="background-color: #F5F6F8;">
                                                <v-col cols="12" class="pa-0 ma-0">
                                                    <v-col cols="12" class="d-flex flex-wrap pa-0 ma-2">
                                                        <v-col md="5" xs="6" class="pa-0 ma-0">
                                                            <input label="Search Beneficiaries" v-model.lazy="vmSearchBeneficiary"
                                                                   placeholder="Search by age, job"
                                                                   class="custom-text-field elevation-0"
                                                                   append-icon="mdi-magnify" />
                                                        </v-col>
                                                        <v-col md="2" xs="2" align="center" class="pa-0 ma-0">
                                                            <v-btn v-on="on" height="39" dense="" style="background-color:white" class="" @click="">
                                                                <span>Filter</span>
                                                                <v-icon color="primary" class="ml-6">mdi-filter-outline</v-icon>
                                                            </v-btn>
                                                        </v-col>
                                                        <!--<v-col md="2" xs="2" class="pa-0 ma-0">
                                                        <v-btn v-on="on" height="39" dense="" style="background-color:white;" class="" @click="">
                                                            <v-icon>mdi-plus</v-icon>
                                                            <span>Add</span>
                                                        </v-btn>
                                                    </v-col>-->
                                                        <v-col md="3" xs="2" class="pa-0 ma-0">
                                                            <v-card align="center" style="background-color:white;" class="">
                                                                <div class="pa-2">{{lstSelectedBeneficiary.length}} <span class="hidden-sm-and-down">beneficiaries selected</span> <v-icon class="hidden-md-and-up">mdi-account-multiple-outline</v-icon></div>
                                                            </v-card>
                                                            <!--<v-btn v-on="on" height="39" dense="" tile style="background-color:white;" class="hidden-md-and-up" @click="">
                                                            <v-icon>mdi-account-multiple-outline</v-icon>
                                                            <span>{{lstSelectedBeneficiary.length}}</span>
                                                        </v-btn>-->
                                                        </v-col>
                                                    </v-col>
                                                    <v-col cols="12" class="mx-2 pa-0">
                                                        <!--sort-by="CreatedDate"-->
                                                        <v-data-table :headers="BeneficiaryTableheaders" v-model="lstSelectedBeneficiary"
                                                                      :items="lstBeneficiary"
                                                                      item-key="BeneficiaryID"
                                                                      :search="vmSearchBeneficiary"
                                                                      show-select
                                                                      fixed-header height="45vh"
                                                                      class="elevation-0">
                                                        </v-data-table>
                                                    </v-col>
                                                </v-col>
                                            </v-row>
                                            <v-row class="pa-0 ma-0 pt-2">
                                                <v-col cols="12" align="right" class="text-center text-md-right pa-0 ma-0">
                                                    <Default-Button-Outlined class="btn-122x36 ma-2" title="Clear" @click.native="ClearSelectedBenef()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Proceed" @click.native="GetBeneficiayBasedSectionQuestions(),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                        <!--Update survey responses-->
                                        <v-stepper-content step="2" class="pa-0">
                                            <v-row>
                                                <v-col>
                                                    <!--next ben-->
                                                    <v-col cols="12" class="pa-0 ma-0 d-md-inline-flex d-sm-block">
                                                        <!-- Beneficiary Stepper-->
                                                        <v-col md="6" xs="10" class="d-inline-flex pa-0 ma-md-2">
                                                            <div class="text-center" style="background-color:white;padding:7px 8px;width:200px" v-for="(benefItem,Index) in vmSelectedBeneficiaries" :key="Index" v-if="BenefStepper== Index+1">
                                                                <span class="text-center text-limit">{{benefItem.BeneficiaryName}}</span>
                                                            </div>
                                                            <div class="text-center mx-1" style="background-color:white;padding:7px 8px;">
                                                                {{BenefStepper}}/{{vmSelectedBeneficiaries.length}}
                                                            </div>
                                                            <v-btn icon style="border-radius:0px" class="elevation-0" @click="previousBeneficiary()"><v-icon color="primary">mdi-chevron-left</v-icon></v-btn>
                                                            <v-btn icon style="border-radius:0px" class="elevation-0" @click="nextBeneficiary()"><v-icon color="primary">mdi-chevron-right</v-icon></v-btn>
                                                        </v-col>
                                                        <v-spacer class="hidden-sm-and-down"></v-spacer>
                                                        <v-col md="6" xs="10" class="text-center text-md-left pa-0 mx-4 my-2 ">
                                                            <v-btn elevation="1" tile style="border-radius:3px;" color="white" @click="fnResetQuestionsAns()">Reset</v-btn>
                                                        </v-col>
                                                    </v-col>

                                                    <!--next ben-->

                                                    <v-col cols="12">
                                                        <v-col md="8" class="mx-4 my-2 pa-0">
                                                            <div id="scrollToTop"></div>
                                                            <div class="pa-0 ma-0" v-for="(SecItem,Sindex) in lstSectionsQstns" v-if="SecItem.IslogicSection == 0">

                                                                <div class="ma-0 pa-0">
                                                                    <v-avatar size="20" class="mb-1 ma-0 pa-0" style="background-color:#3374B9;color:white">{{Sindex + 1}}</v-avatar>
                                                                    <span class="mx-1" style="font-size:1.5em;font-weight:bolder;">{{SecItem.SectionName}}</span>
                                                                </div>
                                                                <br />
                                                                <div v-for="(item, QIndex) in SecItem.Questions">

                                                                    <label class="text-left subtitle-2 mb-2"><b>{{item.QuestionName}}</b></label>

                                                                    <sub v-if="item.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                                    <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{item.QuestionHint}}</sub>

                                                                    <sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| item.IsValidationError ">{{item.ValidationErrorTest}}</sub>

                                                                    <!-- Single Line Text with validation and Condition -->
                                                                    <div v-if="item.QuestionType=='Single Line Text'">
                                                                        <input type="text" style="background-color: #F5F6F8;" v-if="item.QuestionType=='Single Line Text'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="item.Answer"  @focus="CalculateText(item,item.QuestionID,BenefStepper)" />
                                                                    </div>

                                                                    <!--Multiline Answer-->
                                                                    <textarea type="text" style="background-color: #F5F6F8;" class="custom-text-field elevation-0" v-if="item.QuestionType==='Multi Line Text'" solo label="Type your answer" v-model="item.Answer" />

                                                                    <!--Numeric-->

                                                                    <div v-if="item.QuestionType==='Numeric'">

                                                                        <input type="number" style="background-color: #F5F6F8;" v-if="item.QuestionType==='Numeric'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="item.Answer" :readonly="item.IsFormulaQuestion == true" :maxlength="item.textLength" @change="AddSectionLogic(item,item.QuestionID,BenefStepper)" @focus="Calculate(item,item.QuestionID,BenefStepper)" />

                                                                        <sub class="mr-2 ml-2" style="color:green;font-size:large">{{item.UOM}}</sub>

                                                                    </div>


                                                                    <!--Single Choice Answer-->
                                                                    <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                                        <v-radio-group class="my-0 py-0" v-model.lazy="item.Answer" @change="AddSectionLogic(item,item.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(item,item.Answer.toUpperCase())">
                                                                            <v-radio color="success" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                     :label="optItem.OptionValue"
                                                                                     :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!--Multiple Choice -->
                                                                    <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">

                                                                        <div class="my-0 py-0" v-if="item.QuestionOptions.length>0">

                                                                            <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                        v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,BenefStepper),AddSectionLogicForMultiChoice(optItem,item.QuestionID,BenefStepper,item.QuestionOptions),ShowOtherOption(item,optItem.OptionValue.toUpperCase(),optItem.IsOptSelected)"></v-checkbox>
                                                                        </div>
                                                                    </div>

                                                                    <!--Dropdown -->
                                                                    <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                        <div v-if="item.QuestionOptions.length>0">
                                                                            <v-select dense :items="item.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                                      item-text="OptionValue"
                                                                                      label="Select" outlined hide-details v-model.lazy="item.Answer" @change="AddSectionLogic(item,item.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(item,item.Answer.toUpperCase())">
                                                                            </v-select>
                                                                        </div>
                                                                    </div>

                                                                    <!--Date Picker-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                                        <v-menu v-model="item.DateMenu"
                                                                                :close-on-content-click="false"
                                                                                :nudge-right="40"
                                                                                transition="scale-transition"
                                                                                offset-y
                                                                                min-width="290px">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-text-field v-model.lazy="item.Answer"
                                                                                              prepend-inner-icon="mdi-calendar"
                                                                                              readonly
                                                                                              @click:prepend-inner="item.DateMenu = true"
                                                                                              v-on="on" hide-details></v-text-field>
                                                                            </template>
                                                                            <v-date-picker v-model.lazy="item.Answer" no-title @input="item.DateMenu = false" format="DD-MM-YYYY" @change=""></v-date-picker>
                                                                        </v-menu>
                                                                    </div>

                                                                    <!--Cascading Dropdown-->
                                                                    <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                        <v-row class="my-0 py-0">
                                                                            <v-col class="my-0 py-0">
                                                                                <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                          :label="` ${optItem.OptionValue}`" outlined=""
                                                                                          v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                                          :items="optItem.CascadingList"
                                                                                          item-text="Name"
                                                                                          item-value="ID" @change="FilterCascadingItems(item,optItem,optItem[ind],false,BenefStepper,-1)">
                                                                                </v-select>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!--GPS Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model.lazy="item.Answer">
                                                                        <span class="d-none">{{ GetGPSCoordinates(item,false) }}</span>
                                                                        <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                        <GmapMap :center="Coordinates"
                                                                                 :zoom="13"
                                                                                 style="width: 100%; height: 200px" @click.native="GetGPSCoordinates(item,false)">
                                                                            <GmapMarker :position="Coordinates"
                                                                                        :clickable="true"
                                                                                        :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                                        </GmapMap>
                                                                    </div>

                                                                    <!--File Upload-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='File Upload'">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <v-file-input outlined dense accept="*" v-model="item.AnsBlob"
                                                                                              @change="AnsBlobQID=item.QuestionID,SaveFile(item,'File',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                            </v-col>
                                                                            <v-col md="4" v-if="item.uploadedFile !=null">
                                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                                <v-img :src=item.uploadedFile width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!-- Likert Scale-->
                                                                    <div v-if="item.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                                        <v-radio-group class="my-0 py-0" v-model.lazy="item.Answer">
                                                                            <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                     :label="optItem.OptionValue"
                                                                                     :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!-- Photo Caputure-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Photo Capture" hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Photo Upload" hide-details>
                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <!-- Photo Capture-->
                                                                        <div v-show="item.IsUpload===0">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-btn @click="item.startCamera =true,item.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                                    <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item,item.QuestionID,false,BenefStepper),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                                    <v-btn @click="item.startCamera =false,item.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col md="8" v-if="item.startCamera">
                                                                                    <vue-web-cam ref="webcam"
                                                                                                 :device-id="deviceId"
                                                                                                 width="100%"
                                                                                                 height="200px"
                                                                                                 @started="onStarted"
                                                                                                 @stopped="onStopped"
                                                                                                 @error="onError"
                                                                                                 @cameras="onCameras"
                                                                                                 @camera-change="onCameraChange" />
                                                                                    <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                </v-col>

                                                                                <v-col md="4" v-if="item.img !=null && item.img != ''">
                                                                                    <figure class="figure">
                                                                                        <img :src="item.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                                    </figure>
                                                                                </v-col>

                                                                            </v-row>
                                                                        </div>
                                                                        <!-- Photo Upload-->
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Photo',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4" v-if="item.uploadedImg !=null">
                                                                                    <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                    </div>

                                                                    <!--Signature Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Signature Capture" hide-details></v-radio>
                                                                            <v-radio color="success" label="Signature Upload" hide-details></v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload===0">
                                                                            <v-card outlined>
                                                                                <VueSignaturePad :options="{onBegin: () => { $refs.signaturePad[0].resizeCanvas() }}" width="100%" height="300px" ref="signaturePad" />
                                                                            </v-card>
                                                                            <div>
                                                                                <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=item.signaturebutton @click="saveSign(item.QuestionID,false,BenefStepper,item),item.signaturebutton=true">Save</v-btn>
                                                                                <v-btn default depressed class="mt-2 mr-2" @click="clearSign(item, false,item.QuestionID),item.signaturebutton=false">Clear</v-btn>
                                                                            </div>
                                                                        </div>
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob" @mousedown="item.AnsBlob=null" @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Signature',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                        </div>
                                                                        <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                                                            <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                        </v-card>
                                                                    </div>

                                                                    <!--Audio Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Audio Capture" hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Audio Upload" hide-details>

                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload===0">
                                                                            <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                            :play="playSound"
                                                                                            :attempts="1"
                                                                                            :time="1"
                                                                                            :headers="headers"
                                                                                            :before-recording="callback"
                                                                                            :pause-recording="callback"
                                                                                            :after-recording="SaveAudio"
                                                                                            :before-upload="callback"
                                                                                            :successful-upload="callback"
                                                                                            :failed-upload="callback"
                                                                                            v-model="item.Answer"
                                                                                            @click.native="AnsBlobQID=item.QuestionID,FindRecordedFileItem(item)" />

                                                                        </div>
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                                                          @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Audio',item.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                                        </div>
                                                                    </div>

                                                                    <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                    <div v-if="item.isOtherOptionEnabled==true">
                                                                        <input type="text" solo class="custom-text-field elevation-0" label="Type your answer"
                                                                               v-model.lazy="item.OtherOptionAnswer" />
                                                                    </div>

                                                                    <!-- For Showing repeat or skip section Question Details-->
                                                                    <div v-if="RepeatOrSkipSection.length >0 && Ritem.ParentQuestionID == item.QuestionID && BenefStepper == Ritem.benefIndex" v-for="(Ritem,RIndex) in RepeatOrSkipSection" :key="RIndex">

                                                                        <label class="text-left heading-3 mb-2"><b>{{Ritem.QuestionName}}</b></label>

                                                                        <sub v-if="Ritem.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                                        <sub style="color:red;font-size:smaller" v-if="Ritem.QuestionHint != null && Ritem.QuestionHint != 'null' && Ritem.QuestionHint.length > 0">{{Ritem.QuestionHint}}</sub>

                                                                        <sub style="color:red;font-size:smaller" v-if="(Ritem.IsMandatory==true && Ritem.Answer ==='')|| Ritem.IsValidationError ">{{Ritem.ValidationErrorTest}}</sub>

                                                                        <!-- Single Line Text with validation and Condition -->
                                                                        <div v-if="Ritem.QuestionType=='Single Line Text'">
                                                                            <input type="text" style="background-color: #F5F6F8;" v-if="Ritem.QuestionType=='Single Line Text'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="Ritem.Answer" @focus="CalculateText(Ritem,Ritem.QuestionID,BenefStepper)"/>
                                                                        </div>

                                                                        <!--Multiline Answer-->
                                                                        <input type="text" style="background-color: #F5F6F8;" class="custom-text-field elevation-0" v-if="Ritem.QuestionType==='Multi Line Text'" solo label="Type your answer" v-model="Ritem.Answer" />

                                                                        <!--Numeric-->

                                                                        <div v-if="Ritem.QuestionType==='Numeric'">

                                                                            <input type="number" style="background-color: #F5F6F8;" v-if="Ritem.QuestionType==='Numeric'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="Ritem.Answer" :readonly="Ritem.IsFormulaQuestion == true" @focus="Calculate(Ritem,Ritem.QuestionID,BenefStepper)" />

                                                                            <sub class="mr-2 ml-2" style="color:green;font-size:large">{{Ritem.UOM}}</sub>

                                                                        </div>


                                                                        <!--Single Choice Answer-->
                                                                        <div v-if="Ritem.QuestionType==='Single Choice'" class="mt-0">
                                                                            <v-radio-group class="my-0 py-0" v-model.lazy="Ritem.Answer" @change="AddSectionLogic(Ritem,Ritem.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(Ritem,Ritem.Answer.toUpperCase())">
                                                                                <v-radio color="success" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                         :label="optItem.OptionValue"
                                                                                         :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!--Multiple Choice -->
                                                                        <div v-if="Ritem.QuestionType==='Multi-Choice'" class="mt-0 mb-6">

                                                                            <div class="my-0 py-0" v-if="Ritem.QuestionOptions.length>0">

                                                                                <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                            v-model="optItem.IsOptSelected" @change="GetMultiChoiceRepeat(optItem,optItem.IsOptSelected,optItem.OptionValue,Ritem.RQstInd,Ritem.QuestionID,BenefStepper),ShowOtherOption(Ritem,optItem.OptionValue.toUpperCase(),optItem.IsOptSelected)"></v-checkbox>
                                                                            </div>
                                                                        </div>

                                                                        <!--Dropdown -->
                                                                        <div v-if="Ritem.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                            <div v-if="Ritem.QuestionOptions.length>0">
                                                                                <v-select dense :items="Ritem.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                                          item-text="OptionValue"
                                                                                          label="Select" outlined hide-details v-model.lazy="Ritem.Answer" @change="AddSectionLogic(Ritem,Ritem.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(Ritem,Ritem.Answer.toUpperCase())">
                                                                                </v-select>
                                                                            </div>
                                                                        </div>

                                                                        <!--Date Picker-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Date Picker'">
                                                                            <v-menu v-model="Ritem.DateMenu"
                                                                                    :close-on-content-click="false"
                                                                                    :nudge-right="40"
                                                                                    transition="scale-transition"
                                                                                    offset-y
                                                                                    min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field v-model.lazy="Ritem.Answer"
                                                                                                  prepend-inner-icon="mdi-calendar"
                                                                                                  readonly
                                                                                                  @click:prepend-inner="Ritem.DateMenu = true"
                                                                                                  v-on="on" hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model.lazy="Ritem.Answer" no-title @input="Ritem.DateMenu = false" format="DD-MM-YYYY" @change=""></v-date-picker>
                                                                            </v-menu>
                                                                        </div>

                                                                        <!--Cascading Dropdown-->
                                                                        <div v-if="Ritem.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                            <v-row class="my-0 py-0">
                                                                                <v-col class="my-0 py-0">
                                                                                    <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                              :label="` ${optItem.OptionValue}`" outlined=""
                                                                                              v-for="(optItem,ind) in Ritem.CascadingQuestionOptions" :key="ind"
                                                                                              :items="optItem.CascadingList"
                                                                                              item-text="Name"
                                                                                              item-value="ID" @change="FilterCascadingItems(Ritem,optItem,optItem[ind],false,BenefStepper,-1)">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!--GPS Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='GPS Capture'" v-model.lazy="Ritem.Answer">
                                                                            <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                                            <GmapMap :center="Coordinates"
                                                                                     :zoom="13"
                                                                                     style="width: 100%; height: 200px" @click.native="">
                                                                                <GmapMarker :position="Coordinates"
                                                                                            :clickable="true"
                                                                                            :draggable="true" @dragend="" />
                                                                            </GmapMap>
                                                                        </div>

                                                                        <!--File Upload-->
                                                                        <div class="d-inline-flex pa-0 ma-0" style="width:100%" v-if="Ritem.QuestionType==='File Upload'">

                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <!--<v-btn left elevation="1" tile style="border-radius:3px;" color="white" class="mx-1">UpLoad</v-btn>-->
                                                                                    <v-file-input truncate-length="15" accept="*" v-model.lazy="Ritem.AnsBlob"
                                                                                                  @change="SaveFileRepeat(Ritem,'File',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4" v-if="Ritem.uploadedImg !=null">
                                                                                    <v-img :src=Ritem.uploadedFile width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!-- Likert Scale-->
                                                                        <div v-if="Ritem.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                                            <v-radio-group class="my-0 py-0" v-model.lazy="Ritem.Answer">
                                                                                <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                         :label="optItem.OptionValue"
                                                                                         :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!-- Photo Caputure-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Photo Capture'">
                                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="Ritem.IsUpload=audUpload[QIndex]">
                                                                                <v-radio color="success" label="Photo Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success" label="Photo Upload" hide-details>
                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <!-- Photo Capture-->
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-btn @click="Ritem.startCamera =true,Ritem.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                                        <v-btn class="mr-4" color="secondary" depressed @click="onCapture(Ritem,Ritem.QuestionID,false,BenefStepper),Ritem.startCamera =false,Ritem.camOn=false" v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                                        <v-btn @click="Ritem.startCamera =false,Ritem.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row>
                                                                                    <v-col md="8" v-if="Ritem.startCamera">
                                                                                        <vue-web-cam ref="webcam"
                                                                                                     :device-id="deviceId"
                                                                                                     width="100%"
                                                                                                     height="200px"
                                                                                                     @started="onStarted"
                                                                                                     @stopped="onStopped"
                                                                                                     @error="onError"
                                                                                                     @cameras="onCameras"
                                                                                                     @camera-change="onCameraChange" />
                                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                    </v-col>

                                                                                    <v-col md="4" v-if="Ritem.img !=null && Ritem.img != ''">
                                                                                        <figure class="figure">
                                                                                            <img :src="Ritem.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                                        </figure>
                                                                                    </v-col>

                                                                                </v-row>
                                                                            </div>
                                                                            <!-- Photo Upload-->
                                                                            <div v-show="item.IsUpload===1">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="Ritem.AnsBlob"
                                                                                                      @change="SaveFileRepeat(Ritem,'Photo',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd )"></v-file-input>
                                                                                    </v-col>
                                                                                    <v-col md="4" v-if="item.uploadedImg !=null">
                                                                                        <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                        </div>

                                                                        <!--Signature Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Signature Capture'" v-model="Ritem.Answer">
                                                                            <v-radio-group class="my-0 py-0" row v-model="Ritem.IsUpload" @change="">
                                                                                <v-radio color="success" label="Signature Capture" hide-details></v-radio>
                                                                                <v-radio color="success" label="Signature Upload" hide-details></v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <v-card outlined>
                                                                                    <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                                                </v-card>
                                                                                <div>
                                                                                    <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=Ritem.signaturebutton @click="SaveSignatureRepeatOrSkip(Ritem,false,BenefStepper,Ritem.RQstInd),Ritem.signaturebutton=true">Save</v-btn>
                                                                                    <v-btn default depressed class="mt-2 mr-2" @click="clearSign(Ritem, false,Ritem.QuestionID),Ritem.signaturebutton=false">Clear</v-btn>
                                                                                </div>
                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload===1">
                                                                                <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="Ritem.AnsBlob" @change="SaveFileRepeat(Ritem,'Signature',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd )"></v-file-input>
                                                                            </div>
                                                                            <v-card v-if="Ritem.uploadedSign !=null && Ritem.uploadedSign != '' ">
                                                                                <v-img :src=Ritem.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                            </v-card>
                                                                        </div>

                                                                        <!--Audio Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Audio Capture'">
                                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="Ritem.IsUpload=audUpload[QIndex]">
                                                                                <v-radio color="success" label="Audio Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success" label="Audio Upload" hide-details>

                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                                :play="playSound"
                                                                                                :attempts="1"
                                                                                                :time="1"
                                                                                                :headers="headers"
                                                                                                :before-recording="callback"
                                                                                                :pause-recording="callback"
                                                                                                :after-recording="SaveAudio"
                                                                                                :before-upload="callback"
                                                                                                :successful-upload="callback"
                                                                                                :failed-upload="callback"
                                                                                                v-model="Ritem.Answer"
                                                                                                @click.native="AnsBlobQID=Ritem.QuestionID,FindRecordedFileItem(Ritem)" />

                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload===1">
                                                                                <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="Ritem.AnsBlob"
                                                                                              @change="AnsBlobQID=Ritem.QuestionID,SaveFileRepeat(Ritem,'Audio',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd )"></v-file-input>

                                                                            </div>
                                                                        </div>
                                                                        <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                        <div v-if="Ritem.isOtherOptionEnabled==true">
                                                                            <input type="text" solo class="custom-text-field elevation-0" label="Type your answer"
                                                                                   v-model.lazy="Ritem.OtherOptionAnswer" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </v-col>
                                                    </v-col>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" align="right" style="background-color:#F5F6F8;" class="text-center text-md-right">
                                                    <Default-Button-Outlined class="btn-122x36 ma-2" title="Clear" @click.native="ClearFilledData()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="fnProccedToUpdate(),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                        <!--Comments page-->
                                        <v-stepper-content step="3" class="pa-0">
                                            <v-row>
                                                <v-col>
                                                    <v-row no-gutters>
                                                        <v-col md="12">
                                                            <v-col cols="12">
                                                                <div id="scrollToTop">
                                                                    <v-avatar size="20" style="background-color:#3374B9;color:white">1</v-avatar>
                                                                    <span class="subtitle-2 mx-1">Summarize</span>
                                                                </div>
                                                                <br />
                                                                <v-card>
                                                                    <v-row no-gutters class="text-center">
                                                                        <v-col cols="6" md="4">
                                                                            <h1 style="color:#3D7EFE">{{lstSelectedBeneficiary.length}}</h1>
                                                                            <p>
                                                                                No.of <br />
                                                                                Beneficiaries
                                                                            </p>
                                                                        </v-col><v-spacer></v-spacer>
                                                                        <v-col cols="6" md="4">
                                                                            <h1 style="color:#3D7EFE">{{TotalQstnsPerBeneficiary}}</h1>
                                                                            <p>
                                                                                No.of <br />
                                                                                Questions/Submission
                                                                            </p>
                                                                        </v-col><v-spacer></v-spacer>
                                                                        <v-col cols="6" md="4">
                                                                            <h1 style="color:#3D7EFE">{{CompletionPerc}}%</h1>
                                                                            <p>Completion <br /> Ratio</p>
                                                                        </v-col><v-spacer></v-spacer>
                                                                        <!--<v-col cols="6" md="3">
                                                                        <h1 style="color:#3D7EFE">{{CompletionPerc}}%</h1>
                                                                        <p>Data collected /Beneficiary</p>
                                                                    </v-col>-->
                                                                    </v-row>
                                                                </v-card>
                                                                <br />
                                                                <div>
                                                                    <v-avatar size="20" style="background-color:#3374B9;color:white">2</v-avatar>
                                                                    <span class="subtitle-2 mx-1">Add Comments</span>
                                                                </div>
                                                                <br />
                                                                In Case You Have Any Comments Please Specify Them Below.
                                                                <v-textarea rows="8" background-color="grey lighten-2" color="black" dense label="Summarize Your Comments On The Survey With Few Lines." solo class="elevation-0" type="text">
                                                                </v-textarea>
                                                            </v-col>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" align="right" style="background-color:#F5F6F8;" class="text-center text-md-right">
                                                    <Default-Button-Outlined class="btn-122x36 ma-2" title="Back" @click.native="fnEditQuestionsSection(),scrollToNavbarTop()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Submit" @click.native="UpdateTask(),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                        <!--Final thank you page-->
                                        <v-stepper-content step="4" class="pa-0">
                                            <v-row style="background-color:#F5F6F8">
                                                <v-col cols="12" align="right" class="text-center text-md-right" style="background-color:#F5F6F8;">
                                                    <v-btn color="white" class="px-2" @click="downloadShareLinkSummary('beneficiarylinked')">
                                                        <v-icon>mdi-download</v-icon>
                                                        Download summary
                                                    </v-btn>
                                                </v-col>
                                                <br />

                                            </v-row>
                                            <v-row align="center">
                                                <v-col cols="12" md="4" class="align-center justify-center">
                                                    <div class="text-center pa-0 ma-0">
                                                        <v-img :src="surveyImage" class=" hidden-sm-and-down text-center center-block"
                                                               alt="logoImage" height="60%" width="100%"></v-img>
                                                        <v-img :src="surveyImage" class=" hidden-md-and-up center-block"
                                                               alt="logoImage" height="70%" width="70%"></v-img>
                                                    </div>
                                                </v-col>
                                                <v-col cols="12" md="8" class="align-center text-center">
                                                    <h4 class="hidden-md-and-up">Your response to the survey <br /> has been recorded. Thank you.</h4>
                                                    <h2 class="hidden-sm-and-down">Your response to the survey <br /> has been recorded. Thank you.</h2>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                        <!--add new beneficiary by import and manual-->
                                        <v-stepper-content step="5" class="pa-0">
                                            <v-row no-gutters style="background-color: #F5F6F8;">
                                                <v-col cols="12">
                                                    <v-col cols="12" class="px-4">
                                                        <v-tabs v-model="BenefImportTab">
                                                            <v-tab href='#tab-3'>Upload</v-tab>
                                                            <v-tab href='#tab-4'>Manual</v-tab>
                                                            <!--Upload-->
                                                            <v-tab-item value="tab-3" style="background-color: white;">
                                                                <v-row class="d-flex justify-center">
                                                                    <v-col md="12" class="mt-6">

                                                                        <!-- Drag and Drop Section-->
                                                                        <div class="text-center box_outline" @dragover.prevent @drop.prevent v-if="lstImportBeneficiary.length == 0">
                                                                            <div @drop="dragFile">
                                                                                <div><v-icon color="#374151" size="70">mdi-file</v-icon></div>
                                                                                <div v-if="File.length == 0">
                                                                                    <a>Drag & Drop The Filled Out Form</a><br />
                                                                                    <a>Upload Only Xls File (Not Above 15mb Of Size)</a>
                                                                                </div>
                                                                                <div v-if="File.length" class="mt-4">
                                                                                    <div v-for="file in File" :key="file">
                                                                                        <v-chip close="" label="" color="white">
                                                                                            <v-icon size="20" color="success" height="100">mdi-file-delimited-outline</v-icon>
                                                                                            {{ file.name}}
                                                                                        </v-chip>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <!-- Show vue-excel-editor -->
                                                                        <div no-gutters class="uploadBoxHeight pa-0" v-if="lstImportBeneficiary.length > 0">
                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0">
                                                                                <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                                                            </v-col>

                                                                            <v-col cols="12" xs="12" sm="12" md="12" class="v-scrolling-div text-center uploadScrollHeight pa-0 px-sm-3">
                                                                                <vue-excel-editor no-paging :resetOnClick="resetOnClick" disable-panel-setting disable-panel-filter
                                                                                                  v-model="lstImportBeneficiary">
                                                                                    <div v-for="Hearder in lstImportBeneficiaryHearder">
                                                                                        <vue-excel-column :field="Hearder.text" :type="Hearder.type" :mandatory="Hearder.mandatory" :options="Hearder.options" :validate="onBeforeMandatoryChange" width="80px" />
                                                                                    </div>
                                                                                </vue-excel-editor>
                                                                            </v-col>
                                                                        </div>
                                                                        <div class="mt-4" align="center" v-if="lstImportBeneficiary.length == 0">
                                                                            <Secondary-Button class="btn-122x36 mr-2" title="Download" @click.native="DownloadBenefRegSampleFile()"></Secondary-Button>
                                                                        </div>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-tab-item>
                                                            <!--Manual-->
                                                            <v-tab-item value="tab-4" style="background-color: white;" class="v-scrolling-div" hieght="30vh">
                                                                <v-row>
                                                                    <v-col md="8" class="ma-0 mx-3">
                                                                        <div v-for="(item,index) in lstBenefRegQstns">
                                                                            <label v-bind:id="item.QuestionID.toString()" class="text-left subtitle-2 mb-2">{{item.QuestionName}}</label>
                                                                            <sub v-if="item.IsMandatoryResponse==true" style="color:red;font-size:large"> *</sub>
                                                                            <!-- Question Hint-->
                                                                            <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{ item.QuestionHint }}</sub>
                                                                            <!-- error text-->
                                                                            <sub style="color:red;font-size:smaller" v-if="item.IsValidationError == true">{{ item.ValidationErrorTest }}</sub>
                                                                         
                                                                            <div v-if="item.QuestionType=='Single Line Text' && item.IsValidationQuestion == 1">
                                                                                <input type="text" style="background-color: #F5F6F8;" class="custom-text-field" solo :value="item.Answer" @change="fnBindValue($event, index)" />
                                                                            </div>
                                                                            <div v-if="item.QuestionType=='Single Line Text' && item.IsValidationQuestion == 0">
                                                                                <input type="text" style="background-color: #F5F6F8;" class="custom-text-field" v-if="item.QuestionType=='Single Line Text'" full-width outlined dense label="Type Your answer" :value="item.Answer" @change="fnBindValue($event, index)">

                                                                            </div>
                                                                            <!--Multiline Answer-->
                                                                            <textarea class="custom-text-field" full-width outlined dense v-if="item.QuestionType==='Multi Line Text'" label="Type your answer" :value="item.Answer" @change="fnBindValue($event, index)"></textarea>
                                                                            <!--Numeric-->
                                                                            <div v-if="item.QuestionType==='Numeric'">
                                                                                <input type="number" style="background-color: #F5F6F8;" class="custom-text-field" full-width outlined dense label="Type your answer" :value="item.Answer" @change="fnBindValue($event, index)">
                                                                            </div>
                                                                            <!--Single Choice -->
                                                                            <div v-if="item.QuestionType==='Single Choice'" class="mt-0">

                                                                                <v-radio-group class="my-0 py-0" v-model="item.Answer">
                                                                                    <v-radio color="success" hide-details v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                             :label="optItem.OptionValue"
                                                                                             :value="optItem.OptionValue"></v-radio>
                                                                                </v-radio-group>

                                                                            </div>
                                                                            <!--Multiple Choice -->
                                                                            <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">
                                                                                <div class="my-0 py-0" v-if="item.BTQstnAnsOptionList.length>0">
                                                                                    <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                                                                v-model="optItem.IsOptSelected" @change="GetMultiChoice_BT(optItem,optItem.IsOptSelected,optItem.OptionValue)"></v-checkbox>

                                                                                </div>
                                                                            </div>
                                                                            <!--Dropdown -->
                                                                            <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                                <div v-if="item.BTQstnAnsOptionList.length>0">
                                                                                    <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                                                              item-text="OptionValue"
                                                                                              label="Select" outlined hide-details v-model="item.Answer">
                                                                                    </v-select>
                                                                                </div>
                                                                            </div>
                                                                            <!--Cascading Dropdown-->
                                                                            <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0 py-0 my-0">
                                                                                <v-row class="my-0 py-0">
                                                                                    <v-col class="my-0 py-0">
                                                                                        <v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                                  :label="` ${optItem.OptionValue}`" outlined=""
                                                                                                  v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                                                  :items="optItem.CascadingList"
                                                                                                  item-text="Name"
                                                                                                  item-value="ID" @change="FilterCascadingItems(item,optItem,optItem[ind],true,1,false,-1)">
                                                                                        </v-select>


                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                            <!--Date Picker-->
                                                                            <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                                                <v-menu v-model="item.mBTDate"
                                                                                        :close-on-content-click="false"
                                                                                        :nudge-right="40"
                                                                                        transition="scale-transition"
                                                                                        offset-y
                                                                                        min-width="290px">
                                                                                    <template v-slot:activator="{ on }">
                                                                                        <v-text-field v-model="item.Answer"
                                                                                                      prepend-inner-icon="mdi-calendar"
                                                                                                      outlined
                                                                                                      readonly
                                                                                                      @click:prepend-inner="item.mBTDate = true"
                                                                                                      dense
                                                                                                      v-on="on" hide-details></v-text-field>
                                                                                    </template>
                                                                                    <v-date-picker v-model="item.Answer" no-title @input="item.mBTDate = false" format="DD-MM-YYYY"></v-date-picker>
                                                                                </v-menu>
                                                                            </div>
                                                                            <!--GPS Capture-->
                                                                            <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model="item.Answer">
                                                                                <span class="d-none">{{ GetGPSCoordinates(item,true) }}</span>
                                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                                <GmapMap :center="Coordinates"
                                                                                         :zoom="13"
                                                                                         style="width: 100%; height: 300px" @click.native="GetGPSCoordinates(item,true)">
                                                                                    <GmapMarker :position="Coordinates"
                                                                                                :clickable="true"
                                                                                                :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                                                </GmapMap>
                                                                            </div>
                                                                            <!--Photo Capture-->
                                                                            <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">

                                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                                    <v-radio color="success" label="Photo Capture" hide-details>
                                                                                    </v-radio>
                                                                                    <v-radio color="success" label="Photo Upload" hide-details>
                                                                                    </v-radio>
                                                                                </v-radio-group>
                                                                                <div v-show="item.IsUpload===0">
                                                                                    <v-row>
                                                                                        <v-col md="12" class="my-0 py-0">

                                                                                            <v-btn @click="startCamera =true,camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                                            <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item,item.QuestionID,true,1),startCamera =false,camOn=false" v-bind:disabled="!camOn">Capture</v-btn>
                                                                                            <v-btn @click="startCamera =false,camOn=false,onStop()" v-bind:disabled="!camOn" depressed>Stop</v-btn>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-row>
                                                                                        <v-col md="8" v-if="startCamera">
                                                                                            <vue-web-cam ref="webcam"
                                                                                                         :device-id="deviceId"
                                                                                                         width="100%"
                                                                                                         height="200px"
                                                                                                         @started="onStarted"
                                                                                                         @stopped="onStopped"
                                                                                                         @error="onError"
                                                                                                         @cameras="onCameras1"
                                                                                                         @camera-change="onCameraChange1" />
                                                                                            <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,true,0)">Capture</v-btn>-->

                                                                                        </v-col>
                                                                                        <v-col md="4" v-if="img1 !=null">
                                                                                            <figure class="figure">
                                                                                                <img :src="img1" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                                            </figure>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </div>

                                                                                <div v-show="item.IsUpload===1">
                                                                                    <v-row>
                                                                                        <v-col md="4">

                                                                                            <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.BTAnsBlob"
                                                                                                          @change="BTAnsBlobQID=item.QuestionID,SaveFile(item,'Photo',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                                        </v-col>
                                                                                        <v-col md="4" v-if="benefUploadedImg !=null">
                                                                                            <v-img :src=benefUploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </div>

                                                                            </div>
                                                                            <!--Audio Capture-->
                                                                            <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                                    <v-radio color="success" label="Audio Capture" hide-details>
                                                                                    </v-radio>
                                                                                    <v-radio color="success" label="Audio Upload" hide-details>

                                                                                    </v-radio>
                                                                                </v-radio-group>
                                                                                <div v-show="item.IsUpload===0">
                                                                                    <audio-recorder upload-url="YOUR_API_URL" id="recAudiobenef"
                                                                                                    :play="playSound"
                                                                                                    :attempts="1"
                                                                                                    :time="1"
                                                                                                    :headers="headers"
                                                                                                    :before-recording="callback"
                                                                                                    :pause-recording="callback"
                                                                                                    :after-recording="SaveAudioBT"
                                                                                                    :before-upload="callback"
                                                                                                    :successful-upload="callback"
                                                                                                    :failed-upload="callback"
                                                                                                    v-model="item.Answer"
                                                                                                    @click.native="BTAnsBlobQID=item.QuestionID,FindRecordedFileItem(item)" />

                                                                                </div>
                                                                                <div v-show="item.IsUpload===1">

                                                                                    <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.BTAnsBlob"
                                                                                                  @change="BTAnsBlobQID=item.QuestionID,SaveFile(item,'Audio',item.BTAnsBlob,true,true,0)"></v-file-input>

                                                                                </div>
                                                                            </div>

                                                                            <!--Signature Capture-->
                                                                            <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                                                <v-radio-group class="my-0 py-0" row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                                                    <v-radio color="success" label="Signature Capture" hide-details></v-radio>
                                                                                    <v-radio color="success" label="Signature Upload" hide-details></v-radio>
                                                                                </v-radio-group>
                                                                                <div v-show="item.IsUpload===0">
                                                                                    <v-card outlined>
                                                                                        <VueSignaturePad :options="{onBegin: () => { $refs.signaturePad1[0].resizeCanvas() }}" width="100%" height="300px" ref="signaturePad1" />
                                                                                    </v-card>
                                                                                    <div>
                                                                                        <v-btn color="secondary" depressed class="mt-2 mr-2" @click="saveSign(item.QuestionID,true,0)">Save</v-btn>
                                                                                        <v-btn default depressed class="mt-2 mr-2" @click="clearSign(true,item.QuestionID)">Clear</v-btn>
                                                                                    </div>
                                                                                </div>
                                                                                <div v-show="item.IsUpload===1">

                                                                                    <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.BTAnsBlob"
                                                                                                  @change="BTAnsBlobQID=item.QuestionID,SaveFile(item,'Signature',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                                </div>
                                                                                <v-card v-if="benefUploadedSign !=null && benefUploadedSign !=''">
                                                                                    <v-img :src=benefUploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                                </v-card>
                                                                            </div>
                                                                            <!--Note-->
                                                                            <div v-if="item.QuestionType==='Note'">
                                                                                <!-- <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea> -->
                                                                                <!--<textarea class="custom-text-field" outlined dense hide-details v-model="item.Answer"></textarea>-->
                                                                            </div>
                                                                            <!--File Upload-->
                                                                            <div v-if="item.QuestionType==='File Upload'">

                                                                                <v-file-input label="Upload File" outlined dense accept="*" v-model="item.BTAnsBlob" @mousedown="item.BTAnsBlob=null"
                                                                                              @change="BTAnsBlobQID=item.QuestionID,SaveFile(item,'File',item.BTAnsBlob,true,true,0)"></v-file-input>
                                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                                <div v-if="benefUploadedFile !=null">
                                                                                    <v-img :src=benefUploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-tab-item>
                                                        </v-tabs>
                                                    </v-col>

                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" align="right" style="background:#F5F6F8" class="text-center text-md-right">
                                                    <Default-Button-Outlined class="btn-122x36 mr-2" title="Cancel" @click.native="fnCancelBenefRegistration(BenefImportTab),scrollToNavbarTop()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="RegisterNewBenficiary(BenefImportTab),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-col>
                        </v-row>


                        <!--Non Beneficiary Linked Survey section starts-->


                        <v-row no-gutters style="background-color: #F5F6F8;" v-if="!IsBeneficiaryLinkedSurvey && lstSectionsQstns.length>=1">
                            <v-col cols="12" md="12" sm="12" xs="12">
                                <v-stepper v-model="NonBenefLinkedSurveySteps" class="elevation-0">
                                    <v-stepper-items>
                                        <!-- Survey Question Update Manually -->
                                        <v-stepper-content step="1" class="pa-0 ma-0">
                                            <v-row style="background-color: white;" no-gutters>
                                                <v-col cols="12" class="pa-0 ma-0">

                                                    <v-col cols="12">

                                                        <!-- Survey Sections and Questions-->
                                                        <v-col md="8" xs="10" class="pa-0 ma-0">
                                                            <div id="scrollToTop"></div>
                                                            <div class="pa-0 ma-0" v-for="(SecItem,Sindex) in lstSectionsQstns" v-if="SecItem.IslogicSection == 0">
                                                                <div>
                                                                    <v-avatar size="20" class="mb-1 ma-0 pa-0" style="background-color:#3374B9; color:white">{{Sindex +1}}</v-avatar>
                                                                    <span class="mx-1" style="font-size:1.5em;font-weight:bolder;">{{SecItem.SectionName}}</span>
                                                                </div>
                                                                <br />
                                                                <div v-for="(item, QIndex) in SecItem.Questions">

                                                                    <label class="text-left subtitle-2 mb-2"><b>{{item.QuestionName}}</b></label>

                                                                    <sub v-if="item.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                                    <sub style="color:red;font-size:smaller" v-if="item.QuestionHint != null && item.QuestionHint != 'null' && item.QuestionHint.length > 0">{{item.QuestionHint}}</sub>

                                                                    <sub style="color:red;font-size:smaller" v-if="(item.IsMandatory==true && item.Answer ==='')|| item.IsValidationError ">{{item.ValidationErrorTest}}</sub>

                                                                    <!-- Single Line Text with validation and Condition -->
                                                                    <div v-if="item.QuestionType=='Single Line Text'">
                                                                        <input type="text" style="background-color: #F5F6F8;" v-if="item.QuestionType=='Single Line Text'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="item.Answer" @focus="CalculateText(item,item.QuestionID,BenefStepper)"/>
                                                                    </div>

                                                                    <!--Multiline Answer-->
                                                                    <textarea type="text" style="background-color: #F5F6F8;" class="custom-text-field elevation-0" v-if="item.QuestionType==='Multi Line Text'" solo label="Type your answer" v-model="item.Answer" />

                                                                    <!--Numeric-->

                                                                    <div v-if="item.QuestionType==='Numeric'">

                                                                        <input type="number" style="background-color: #F5F6F8;" v-if="item.QuestionType==='Numeric'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="item.Answer" :readonly="item.IsFormulaQuestion == true" :maxlength="item.textLength" @change="AddSectionLogic(item,item.QuestionID,BenefStepper)" @focus="onFocusCheckQstnValidation(item),Calculate(item,item.QuestionID,BenefStepper)" />

                                                                        <sub class="mr-2 ml-2" style="color:green;font-size:large">{{item.UOM}}</sub>

                                                                    </div>


                                                                    <!--Single Choice Answer-->
                                                                    <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                                        <v-radio-group class="my-0 py-0" v-model.lazy="item.Answer" @change="AddSectionLogic(item,item.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(item,item.Answer.toUpperCase())">
                                                                            <v-radio color="success" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                     :label="optItem.OptionValue"
                                                                                     :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!--Multiple Choice -->
                                                                    <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0 mb-6">

                                                                        <div class="my-0 py-0" v-if="item.QuestionOptions.length>0">

                                                                            <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                        v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,BenefStepper),AddSectionLogicForMultiChoice(optItem,item.QuestionID,BenefStepper,item.QuestionOptions),ShowOtherOption(item,optItem.OptionValue.toUpperCase(),optItem.IsOptSelected)"></v-checkbox>
                                                                        </div>
                                                                    </div>

                                                                    <!--Dropdown -->
                                                                    <div v-if="item.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                        <div v-if="item.QuestionOptions.length>0">
                                                                            <v-select dense :items="item.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                                      item-text="OptionValue"
                                                                                      label="Select" outlined hide-details v-model.lazy="item.Answer" @change="AddSectionLogic(item,item.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(item,item.Answer.toUpperCase())">
                                                                            </v-select>
                                                                        </div>
                                                                    </div>

                                                                    <!--Date Picker-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Date Picker'">
                                                                        <v-menu v-model="item.DateMenu"
                                                                                :close-on-content-click="false"
                                                                                :nudge-right="40"
                                                                                transition="scale-transition"
                                                                                offset-y
                                                                                min-width="290px">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-text-field v-model.lazy="item.Answer"
                                                                                              prepend-inner-icon="mdi-calendar"
                                                                                              readonly
                                                                                              @click:prepend-inner="item.DateMenu = true"
                                                                                              v-on="on" hide-details></v-text-field>
                                                                            </template>
                                                                            <v-date-picker v-model.lazy="item.Answer" no-title @input="item.DateMenu = false" format="DD-MM-YYYY" @change=""></v-date-picker>
                                                                        </v-menu>
                                                                    </div>

                                                                    <!--Cascading Dropdown-->
                                                                    <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                        <v-row class="my-0 py-0">
                                                                            <v-col class="my-0 py-0">
                                                                                <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                          :label="` ${optItem.OptionValue}`" outlined=""
                                                                                          v-for="(optItem,ind) in item.CascadingQuestionOptions" :key="ind"
                                                                                          :items="optItem.CascadingList"
                                                                                          item-text="Name"
                                                                                          item-value="ID" @change="FilterCascadingItems(item,optItem,optItem[ind],false,BenefStepper,-1)">
                                                                                </v-select>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!--GPS Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='GPS Capture'" v-model.lazy="item.Answer">
                                                                        <span class="d-none">{{ GetGPSCoordinates(item,false) }}</span>
                                                                        <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                        <GmapMap :center="Coordinates"
                                                                                 :zoom="13"
                                                                                 style="width: 100%; height: 200px" @click.native="GetGPSCoordinates(item,false)">
                                                                            <GmapMarker :position="Coordinates"
                                                                                        :clickable="true"
                                                                                        :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                                        </GmapMap>
                                                                    </div>

                                                                    <!--File Upload-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='File Upload'">
                                                                        <v-row>
                                                                            <v-col md="12">
                                                                                <v-file-input outlined dense accept="*" v-model="item.AnsBlob"
                                                                                              @change="AnsBlobQID=item.QuestionID,SaveFile(item,'File',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                            </v-col>
                                                                            <v-col md="4" v-if="item.uploadedFile !=null">
                                                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                                                <v-img :src=item.uploadedFile width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <!-- Likert Scale-->
                                                                    <div v-if="item.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                                        <v-radio-group class="my-0 py-0" v-model.lazy="item.Answer">
                                                                            <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in item.QuestionOptions" :key="ind"
                                                                                     :label="optItem.OptionValue"
                                                                                     :value="optItem.OptionValue"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!-- Photo Caputure-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Photo Capture'">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Photo Capture" hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Photo Upload" hide-details>
                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <!-- Photo Capture-->
                                                                        <div v-show="item.IsUpload===0">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-btn @click="item.startCamera =true,item.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                                    <v-btn class="mr-4" color="secondary" depressed @click="onCapture(item,item.QuestionID,false,BenefStepper),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn">Capture</v-btn>
                                                                                    <v-btn @click="item.startCamera =false,item.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                                </v-col>
                                                                            </v-row>
                                                                            <v-row>
                                                                                <v-col md="8" v-if="item.startCamera">
                                                                                    <vue-web-cam ref="webcam"
                                                                                                 :device-id="deviceId"
                                                                                                 width="100%"
                                                                                                 height="200px"
                                                                                                 @started="onStarted"
                                                                                                 @stopped="onStopped"
                                                                                                 @error="onError"
                                                                                                 @cameras="onCameras"
                                                                                                 @camera-change="onCameraChange" />
                                                                                    <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                </v-col>

                                                                                <v-col md="4" v-if="item.img !=null && item.img != ''">
                                                                                    <figure class="figure">
                                                                                        <img :src="item.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                                    </figure>
                                                                                </v-col>

                                                                            </v-row>
                                                                        </div>
                                                                        <!-- Photo Upload-->
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Photo',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4" v-if="item.uploadedImg !=null">
                                                                                    <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                    </div>

                                                                    <!--Signature Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Signature Capture'" v-model="item.Answer">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Signature Capture" hide-details></v-radio>
                                                                            <v-radio color="success" label="Signature Upload" hide-details></v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload===0">
                                                                            <v-card outlined>
                                                                                <VueSignaturePad :options="{onBegin: () => { $refs.signaturePad[0].resizeCanvas() }}" width="100%" height="300px" ref="signaturePad" />
                                                                            </v-card>
                                                                            <div>
                                                                                <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=item.signaturebutton @click="saveSign(item.QuestionID,false,BenefStepper,item),item.signaturebutton=true">Save</v-btn>
                                                                                <v-btn default depressed class="mt-2 mr-2" @click="clearSign(item, false,item.QuestionID),item.signaturebutton=false">Clear</v-btn>
                                                                            </div>
                                                                        </div>
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob" @change=""></v-file-input>
                                                                        </div>
                                                                        <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                                                            <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                        </v-card>
                                                                    </div>

                                                                    <!--Audio Capture-->
                                                                    <div class="mb-6" v-if="item.QuestionType==='Audio Capture'">
                                                                        <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="item.IsUpload=audUpload[QIndex]">
                                                                            <v-radio color="success" label="Audio Capture" hide-details>
                                                                            </v-radio>
                                                                            <v-radio color="success" label="Audio Upload" hide-details>

                                                                            </v-radio>
                                                                        </v-radio-group>
                                                                        <div v-show="item.IsUpload===0">
                                                                            <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                            :play="playSound"
                                                                                            :attempts="1"
                                                                                            :time="1"
                                                                                            :headers="headers"
                                                                                            :before-recording="callback"
                                                                                            :pause-recording="callback"
                                                                                            :after-recording="SaveAudio"
                                                                                            :before-upload="callback"
                                                                                            :successful-upload="callback"
                                                                                            :failed-upload="callback"
                                                                                            v-model="item.Answer"
                                                                                            @click.native="AnsBlobQID=item.QuestionID,FindRecordedFileItem(item)" />

                                                                        </div>
                                                                        <div v-show="item.IsUpload===1">
                                                                            <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                                                          @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Audio',item.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                                        </div>
                                                                    </div>

                                                                    <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                    <div v-if="item.isOtherOptionEnabled==true">
                                                                        <input type="text" solo class="custom-text-field elevation-0" label="Type your answer"
                                                                               v-model.lazy="item.OtherOptionAnswer" />
                                                                    </div>

                                                                    <!-- For Showing repeat or skip section Question Details-->
                                                                    <div v-if="RepeatOrSkipSection.length >0 && Ritem.ParentQuestionID == item.QuestionID && BenefStepper == Ritem.benefIndex" v-for="(Ritem,RIndex) in RepeatOrSkipSection" :key="RIndex">

                                                                        <label class="text-left heading-3 mb-2"><b>{{Ritem.QuestionName}}</b></label>

                                                                        <sub v-if="Ritem.IsMandatory==true" style="color:red;font-size:large"> *</sub>

                                                                        <sub style="color:red;font-size:smaller" v-if="Ritem.QuestionHint != null && Ritem.QuestionHint != 'null' && Ritem.QuestionHint.length > 0">{{Ritem.QuestionHint}}</sub>

                                                                        <sub style="color:red;font-size:smaller" v-if="(Ritem.IsMandatory==true && Ritem.Answer ==='')|| Ritem.IsValidationError ">{{Ritem.ValidationErrorTest}}</sub>

                                                                        <!-- Single Line Text with validation and Condition -->
                                                                        <div v-if="Ritem.QuestionType=='Single Line Text'">
                                                                            <input type="text" style="background-color: #F5F6F8;" v-if="Ritem.QuestionType=='Single Line Text'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="Ritem.Answer" @focus="CalculateText(Ritem,Ritem.QuestionID,BenefStepper)"/>
                                                                        </div>

                                                                        <!--Multiline Answer-->
                                                                        <input type="text" style="background-color: #F5F6F8;" class="custom-text-field elevation-0" v-if="Ritem.QuestionType==='Multi Line Text'" solo label="Type your answer" v-model="Ritem.Answer" />

                                                                        <!--Numeric-->

                                                                        <div v-if="Ritem.QuestionType==='Numeric'">

                                                                            <input type="number" style="background-color: #F5F6F8;" v-if="Ritem.QuestionType==='Numeric'" solo class="custom-text-field elevation-0" label="Type your answer" v-model="Ritem.Answer" :readonly="Ritem.IsFormulaQuestion == true" @focus="Calculate(Ritem,Ritem.QuestionID,BenefStepper)" />

                                                                            <sub class="mr-2 ml-2" style="color:green;font-size:large">{{Ritem.UOM}}</sub>

                                                                        </div>


                                                                        <!--Single Choice Answer-->
                                                                        <div v-if="Ritem.QuestionType==='Single Choice'" class="mt-0">
                                                                            <v-radio-group class="my-0 py-0" v-model.lazy="Ritem.Answer" @change="AddSectionLogic(Ritem,Ritem.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(Ritem,Ritem.Answer.toUpperCase())">
                                                                                <v-radio color="success" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                         :label="optItem.OptionValue"
                                                                                         :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!--Multiple Choice -->
                                                                        <div v-if="Ritem.QuestionType==='Multi-Choice'" class="mt-0 mb-6">

                                                                            <div class="my-0 py-0" v-if="Ritem.QuestionOptions.length>0">

                                                                                <v-checkbox color="success" hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                            v-model="optItem.IsOptSelected" @change="GetMultiChoiceRepeat(optItem,optItem.IsOptSelected,optItem.OptionValue,Ritem.RQstInd,Ritem.QuestionID,BenefStepper),ShowOtherOption(Ritem,optItem.OptionValue.toUpperCase(),optItem.IsOptSelected)"></v-checkbox>  <!--,AddSectionLogicForMultiChoice(optItem,item.QuestionID,BenefStepper,item.QuestionOptions)-->
                                                                            </div>
                                                                        </div>

                                                                        <!--Dropdown -->
                                                                        <div v-if="Ritem.QuestionType==='Dropdown'" class="mt-0 mb-6">
                                                                            <div v-if="Ritem.QuestionOptions.length>0">
                                                                                <v-select dense :items="Ritem.QuestionOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                                          item-text="OptionValue"
                                                                                          label="Select" outlined hide-details v-model.lazy="Ritem.Answer" @change="AddSectionLogic(Ritem,Ritem.QuestionID,BenefStepper),ShowOtherOptionSingleChoice(Ritem,Ritem.Answer.toUpperCase())">
                                                                                </v-select>
                                                                            </div>
                                                                        </div>

                                                                        <!--Date Picker-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Date Picker'">
                                                                            <v-menu v-model="Ritem.DateMenu"
                                                                                    :close-on-content-click="false"
                                                                                    :nudge-right="40"
                                                                                    transition="scale-transition"
                                                                                    offset-y
                                                                                    min-width="290px">
                                                                                <template v-slot:activator="{ on }">
                                                                                    <v-text-field v-model.lazy="Ritem.Answer"
                                                                                                  prepend-inner-icon="mdi-calendar"
                                                                                                  readonly
                                                                                                  @click:prepend-inner="Ritem.DateMenu = true"
                                                                                                  v-on="on" hide-details></v-text-field>
                                                                                </template>
                                                                                <v-date-picker v-model.lazy="Ritem.Answer" no-title @input="Ritem.DateMenu = false" format="DD-MM-YYYY" @change=""></v-date-picker>
                                                                            </v-menu>
                                                                        </div>

                                                                        <!--Cascading Dropdown-->
                                                                        <div v-if="Ritem.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                            <v-row class="my-0 py-0">
                                                                                <v-col class="my-0 py-0">
                                                                                    <v-select class="my-0 py-0" dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                                                              :label="` ${optItem.OptionValue}`" outlined=""
                                                                                              v-for="(optItem,ind) in Ritem.CascadingQuestionOptions" :key="ind"
                                                                                              :items="optItem.CascadingList"
                                                                                              item-text="Name"
                                                                                              item-value="ID" @change="FilterCascadingItems(Ritem,optItem,optItem[ind],false,BenefStepper,-1)">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!--GPS Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='GPS Capture'" v-model.lazy="Ritem.Answer">
                                                                            <label class="text-left heading-1 mt-3">{{Ritem.Answer}}</label>
                                                                            <GmapMap :center="Coordinates"
                                                                                     :zoom="13"
                                                                                     style="width: 100%; height: 200px" @click.native="">
                                                                                <GmapMarker :position="Coordinates"
                                                                                            :clickable="true"
                                                                                            :draggable="true" @dragend="" />
                                                                            </GmapMap>
                                                                        </div>

                                                                        <!--File Upload-->
                                                                        <div class="d-inline-flex pa-0 ma-0" style="width:100%" v-if="Ritem.QuestionType==='File Upload'">

                                                                            <v-row>
                                                                                <v-col md="12">
                                                                                    <!--<v-btn left elevation="1" tile style="border-radius:3px;" color="white" class="mx-1">UpLoad</v-btn>-->
                                                                                    <v-file-input truncate-length="15" accept="*" v-model.lazy="Ritem.AnsBlob" @mousedown="Ritem.AnsBlob=null"
                                                                                                  @change="SaveFile(Ritem,'File',Ritem.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                                </v-col>
                                                                                <v-col md="4" v-if="Ritem.uploadedImg !=null">
                                                                                    <v-img :src=Ritem.uploadedFile width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>

                                                                        <!-- Likert Scale-->
                                                                        <div v-if="Ritem.QuestionType==='Likert Scale'" class="mt-0 my-0 py-0">
                                                                            <v-radio-group class="my-0 py-0" v-model.lazy="Ritem.Answer">
                                                                                <v-radio color="success" class="my-0 py-0" hide-details v-for="(optItem,ind) in Ritem.QuestionOptions" :key="ind"
                                                                                         :label="optItem.OptionValue"
                                                                                         :value="optItem.OptionValue"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>

                                                                        <!-- Photo Caputure-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Photo Capture'">
                                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="Ritem.IsUpload=audUpload[QIndex]">
                                                                                <v-radio color="success" label="Photo Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success" label="Photo Upload" hide-details>
                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <!-- Photo Capture-->
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-btn @click="Ritem.startCamera =true,Ritem.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                                        <v-btn class="mr-4" color="secondary" depressed @click="onCapture(Ritem,Ritem.QuestionID,false,BenefStepper),Ritem.startCamera =false,Ritem.camOn=false" v-bind:disabled="!Ritem.camOn">Capture</v-btn>
                                                                                        <v-btn @click="Ritem.startCamera =false,Ritem.camOn=false,onStop()" v-bind:disabled="!item.camOn" depressed class="mr-4">Stop</v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row>
                                                                                    <v-col md="8" v-if="Ritem.startCamera">
                                                                                        <vue-web-cam ref="webcam"
                                                                                                     :device-id="deviceId"
                                                                                                     width="100%"
                                                                                                     height="200px"
                                                                                                     @started="onStarted"
                                                                                                     @stopped="onStopped"
                                                                                                     @error="onError"
                                                                                                     @cameras="onCameras"
                                                                                                     @camera-change="onCameraChange" />
                                                                                        <!--<v-btn color="secondary" depressed @click="onCapture(item.QuestionID,false,0)">Capture</v-btn>-->

                                                                                    </v-col>

                                                                                    <v-col md="4" v-if="Ritem.img !=null && Ritem.img != ''">
                                                                                        <figure class="figure">
                                                                                            <img :src="Ritem.img" class="center-block my-2" max-width="100%" height="200" aspect-ratio="1.7" />
                                                                                        </figure>
                                                                                    </v-col>

                                                                                </v-row>
                                                                            </div>
                                                                            <!-- Photo Upload-->
                                                                            <div v-show="item.IsUpload===1">
                                                                                <v-row>
                                                                                    <v-col md="12">
                                                                                        <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                                                      @change="AnsBlobQID=item.QuestionID,SaveFile(item,'Photo',item.AnsBlob,false,true,BenefStepper)"></v-file-input>
                                                                                    </v-col>
                                                                                    <v-col md="4" v-if="item.uploadedImg !=null">
                                                                                        <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                        </div>

                                                                        <!--Signature Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Signature Capture'" v-model="Ritem.Answer">
                                                                            <v-radio-group class="my-0 py-0" row v-model="Ritem.IsUpload" @change="">
                                                                                <v-radio color="success" label="Signature Capture" hide-details></v-radio>
                                                                                <v-radio color="success" label="Signature Upload" hide-details></v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <v-card outlined>
                                                                                    <VueSignaturePad :options="{onBegin: () => { $refs.signaturePad[0].resizeCanvas() }}" width="100%" height="300px" ref="signaturePad" />
                                                                                </v-card>
                                                                                <div>
                                                                                    <v-btn color="secondary" depressed class="mt-2 mr-2" :disabled=Ritem.signaturebutton @click="SaveSignatureRepeatOrSkip(Ritem,false,BenefStepper,Ritem.RQstInd),Ritem.signaturebutton=true">Save</v-btn>
                                                                                    <v-btn default depressed class="mt-2 mr-2" @click="clearSign(Ritem, false,Ritem.QuestionID),Ritem.signaturebutton=false">Clear</v-btn>
                                                                                </div>
                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload===1">
                                                                                <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="Ritem.AnsBlob" @change="SaveFileRepeat(Ritem,'Signature',Ritem.AnsBlob,false,true,BenefStepper,Ritem.RQstInd)"></v-file-input>
                                                                            </div>
                                                                            <v-card v-if="Ritem.uploadedSign !=null && Ritem.uploadedSign != '' ">
                                                                                <v-img :src=Ritem.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                                                            </v-card>
                                                                        </div>

                                                                        <!--Audio Capture-->
                                                                        <div class="mb-6" v-if="Ritem.QuestionType==='Audio Capture'">
                                                                            <v-radio-group class="my-0 py-0" row v-model="audUpload[QIndex]" @change="Ritem.IsUpload=audUpload[QIndex]">
                                                                                <v-radio color="success" label="Audio Capture" hide-details>
                                                                                </v-radio>
                                                                                <v-radio color="success" label="Audio Upload" hide-details>

                                                                                </v-radio>
                                                                            </v-radio-group>
                                                                            <div v-show="Ritem.IsUpload===0">
                                                                                <audio-recorder upload-url="YOUR_API_URL" id="recAudioOne"
                                                                                                :play="playSound"
                                                                                                :attempts="1"
                                                                                                :time="1"
                                                                                                :headers="headers"
                                                                                                :before-recording="callback"
                                                                                                :pause-recording="callback"
                                                                                                :after-recording="SaveAudio"
                                                                                                :before-upload="callback"
                                                                                                :successful-upload="callback"
                                                                                                :failed-upload="callback"
                                                                                                v-model="Ritem.Answer"
                                                                                                @click.native="AnsBlobQID=Ritem.QuestionID,FindRecordedFileItem(Ritem)" />

                                                                            </div>
                                                                            <div v-show="Ritem.IsUpload===1">
                                                                                <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="Ritem.AnsBlob" @mousedown="Ritem.AnsBlob=null"
                                                                                              @change="AnsBlobQID=Ritem.QuestionID,SaveFile(item,'Audio',item.AnsBlob,false,true,BenefStepper)"></v-file-input>

                                                                            </div>
                                                                        </div>
                                                                        <!-- if other option in Multi-choice,single choice and dropdown is enabled-->
                                                                        <div v-if="Ritem.isOtherOptionEnabled==true">
                                                                            <input type="text" solo class="custom-text-field elevation-0" label="Type your answer"
                                                                                   v-model.lazy="Ritem.OtherOptionAnswer" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </v-col>
                                                    </v-col>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" align="right" style="background-color:#F5F6F8;" class="text-center text-md-right">
                                                    <Default-Button-Outlined class="btn-122x36 ma-2" title="Clear" @click.native="ClearFilledData()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="fnNonBenefProceedToUpdate(),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>

                                        <!--Final steps-->
                                        <v-stepper-content step="3" class="pa-0">
                                            <!--<v-row no-gutters style="background-color: white;">
                                            <v-col md="12">
                                                <div id="scrollToTop"></div>
                                                <v-col cols="12" class="v-scrolling-div" style="height:66vh">
                                                    <h4>Summarize</h4><br />
                                                    <textarea rows="8" dense style="background-color:#F5F6F8;" label="Summarize Your Comments On The Survey With Few Lines." solo class="elevation-0 custom-text-field" type="text" v-model.lazy="vmUpdateComment">
                                                        </textarea>
                                                </v-col>
                                            </v-col>
                                        </v-row>-->
                                            <v-row>
                                                <v-col class="pa-0 ma-0">
                                                    <v-row no-gutters class="pa-0 ma-0">
                                                        <v-col md="12" class="pa-0 ma-0 px-2">
                                                            <v-col cols="12">
                                                                <div>
                                                                    <v-avatar size="20" style="background-color:#3374B9;color:white">1</v-avatar>
                                                                    <span class="subtitle-2 mx-1">Summarize</span>
                                                                </div>
                                                                <br />
                                                                <v-card>
                                                                    <v-row no-gutters class="text-center">
                                                                        <v-col cols="6" md="3">
                                                                            <h1 style="color:#3D7EFE">1</h1>
                                                                            <p>
                                                                                No.of <br />
                                                                                Responses Added
                                                                            </p>
                                                                        </v-col><v-spacer></v-spacer>
                                                                        <v-col cols="6" md="3">
                                                                            <h1 style="color:#3D7EFE">{{NonBenefTotalQuestion }}</h1>
                                                                            <p>
                                                                                No.of <br />
                                                                                Questions/Submission
                                                                            </p>
                                                                        </v-col><v-spacer></v-spacer>
                                                                        <v-col cols="6" md="3">
                                                                            <h1 style="color:#3D7EFE">{{NonBenefCompletionPerc}}%</h1>
                                                                            <p>Completion <br /> Ratio</p>
                                                                        </v-col>
                                                                        <!--<v-spacer></v-spacer>
                                                                    <v-col cols="6" md="3">
                                                                        <h1 style="color:#3D7EFE">{{CompletionPerc}}%</h1>
                                                                        <p>Data collected /Beneficiary</p>
                                                                    </v-col>-->
                                                                    </v-row>
                                                                </v-card>
                                                                <br />
                                                                <div>
                                                                    <v-avatar size="20" style="background-color:#3374B9;color:white">2</v-avatar>
                                                                    <span class="subtitle-2 mx-1">Add Comments</span>
                                                                </div>
                                                                <br />
                                                                In Case You Have Any Comments Please Specify Them Below.
                                                                <textarea rows="8" dense style="background-color:#F5F6F8;" label="Summarize Your Comments On The Survey With Few Lines." solo class="elevation-0 custom-text-field" type="text" v-model.lazy="vmUpdateComment">
                                                            </textarea>
                                                            </v-col>
                                                        </v-col>
                                                    </v-row>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" align="right" style="background-color:#F5F6F8;" class="text-center text-md-right">
                                                    <Default-Button-Outlined class="btn-122x36 ma-2" title="Back" @click.native="NonBenefLinkedSurveySteps = 1,scrollToNavbarTop(),fnClearMultiChoiceAns_When_Prev_Button_Click()"></Default-Button-Outlined>
                                                    <Secondary-Button class="btn-122x36 mr-2" title="Submit" @click.native="UpdateTask(),scrollToNavbarTop()"></Secondary-Button>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                        <!--if Already updated-->
                                        <v-stepper-content step="4" class="pa-0">
                                            <v-row style="background-color:#F5F6F8">
                                                <v-col cols="12" align="right" class="text-center text-md-right" style="background-color:#F5F6F8;">
                                                    <v-btn color="white" class="px-2" @click="downloadShareLinkSummary('nonbeneficiarylinked')">
                                                        <v-icon>mdi-download</v-icon>
                                                        Download summary
                                                    </v-btn>
                                                </v-col>
                                                <br />

                                            </v-row>
                                            <v-row align="center">
                                                <v-col cols="12" md="4" class="align-center justify-center">
                                                    <div class="text-center pa-0 ma-0">
                                                        <v-img :src="surveyImage" class=" hidden-sm-and-down text-center center-block"
                                                               alt="logoImage" height="60%" width="100%"></v-img>
                                                        <v-img :src="surveyImage" class=" hidden-md-and-up text-center align-center justify-center center-block"
                                                               alt="logoImage" height="70%" width="70%"></v-img>
                                                    </div>
                                                </v-col>
                                                <v-col cols="12" md="8" class="align-center text-center">
                                                    <h4 class="hidden-md-and-up">Your response to the survey <br /> has been recorded. Thank you.</h4>
                                                    <h2 class="hidden-sm-and-down">Your response to the survey <br /> has been recorded. Thank you.</h2>
                                                </v-col>
                                            </v-row>
                                        </v-stepper-content>
                                    </v-stepper-items>
                                </v-stepper>
                            </v-col>
                        </v-row>

                        <!--Non Beneficiary Linked Survey section ends-->
                    </v-card>
                </v-col>
            </v-row>
        </v-container>
        <v-footer padless class="pa-0 ma-0">
            <v-card class="flex pa-0 ma-0 text-center" elevation="0" style="background:transparent">
                <small class="pa-0 ma-0 hidden-md-and-up">This form was created inside ImWorks Powerd by Billion lives. Privacy policy</small>
                <small class="pa-0 ma-0 hidden-sm-and-down">This form was created inside ImWorks Powerd by Billion lives. Privacy policy</small>
                <v-card-text class="py-0 pa-0 ma-0 white--text text-center">
                    <v-row class="pa-0 ma-0">
                        <v-col class="d-flex text-center pa-0 ma-0 align-center justify-center">
                            <div class="text-center pa-0 ma-0">
                                <v-img :src="logoImage" alt="logoImage" class="justify-center pa-0 ma-0 hidden-md-and-up" height="40" width="100"></v-img>
                                <v-img :src="logoImage" alt="logoImage" class="justify-center pa-0 ma-0 hidden-sm-and-down" height="60" width="200"></v-img>
                            </div>
                        </v-col>
                    </v-row>
                </v-card-text>
            </v-card>
        </v-footer>
        <!--<v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
        <template>
            <div class="pa-0 ma-0">
                <v-alert dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
            </div>
        </template>
    </v-snackbar>-->
        <!--<v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
        <template>
            <div class="pa-0 ma-0">
                <v-alert dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
            </div>
        </template>
    </v-snackbar>-->

        <v-dialog v-model="dlgValidationMessage" width="700" persistent>
            <template v-slot:activator="{ on, attrs }">
                <v-btn dark
                       icon
                       v-bind="attrs"
                       v-on="on">
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
                    <v-btn id="AutoTest_U166" text="" @click="dlgValidationMessage=false">
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
                                            <h4 class="text-center heading-3">There are some mandatory questions that are not answered</h4>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <Secondary-Button id="AutoTest_U167" class="btn-122x36 mr-2" title="OK" @click.native="dlgValidationMessage = false"></Secondary-Button>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>

        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
    </v-container>
</template>

<script>
    var usr;
    var objPB;
    import SecondaryButton from '@/components/secondary-button.vue';
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue';
    import { dbImworks } from '../ImworksDb';
    import objUtils from '../utils.js';
    import { Object, Math } from 'core-js';
    import { WebCam } from "vue-web-cam";
    import { v4 as uuidv4 } from 'uuid';
    import PreLoader from "@/components/pre-loader.vue";
    async function importscript() {
        return Promise.all([
            import("../BL/User.js").then(mod => {
                usr = new mod.User();
            }),
            import("../BL/ProjectBeneficiary.js").then(mod => {
                objPB = new mod.ProjectBeneficiary();
            })
        ]);
    }
    export default {
        data() {
            return {
                mini: true,
                imWorkLogo: require("@/images/apurthi_logo.png"),
                logoImage: require("@/images/apurthi_logo.png"),
                surveyImage: require("@/images/surveyShareLinkLastPageImage.svg"),
                shareLinkErrorImage: require("@/images/Sharelink-error-vector.svg"),
                SurveyShareSteps: 1,
                SurveyID: 0,
                WorkspaceID: 0,
                lstSelectedBeneficiary: [
                    {
                        name: 'Frozen Yogurt',
                        age: 22,
                        profession: "doctor",
                        BeneficiaryID: 1,
                    },
                    {
                        name: 'Frozen Yogurt',
                        age: 25,
                        profession: "engineerr",
                        BeneficiaryID: 2,
                    },
                ],
                BeneficiaryTableheaders: [
                    { text: "Name", align: "left", value: "name" },
                    { text: "Age", value: "age" },
                    { text: "ID no.", value: "BeneficiaryID" },
                    { text: "Profession", value: "profession" }
                ],
                lstBeneficiary: [
                    {
                        name: 'Frozen Yogurt',
                        age: 22,
                        profession: "doctor",
                        BeneficiaryID: 1,
                    },
                    {
                        name: 'Frozen Yogurt',
                        age: 25,
                        profession: "engineerr",
                        BeneficiaryID: 2,
                    },
                    {
                        name: 'Frozen Yogurt',
                        age: 22,
                        profession: "doctor",
                        BeneficiaryID: 3,
                    },
                    {
                        name: 'Frozen Yogurt',
                        age: 25,
                        profession: "engineerr",
                        BeneficiaryID: 4,
                    },
                ],
                lstSectionsQstns: [],
                BeneficiariesWithName: [],
                lstSelectedTaskData: [],
                filterNewBenefList: [],
                TaskCardData: [],
                vmSearchBeneficiary: "",

                //
                events: [],
                nonce: 0,
                cardImage: require("@/images/Fillers.svg"),
                calIcon: require("@/assets/Survey/Property 1=Calender.svg"),
                RecIcon: require("@/assets/Survey/Property 1=Recurring.svg"),
                BenIcon: require("@/assets/Survey/Property 1=User.svg"),
                ActIcon: require("@/assets/Survey/Property 1=Activity.svg"),
                WorkspaceID: 0,
                File: [], // Include excel file when we do drag and drop for beneficiary
                SurveyFile: [], //Include excel file when we do drag and drop for Survey
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
                TotalQstnsPerBeneficiary:0,
                CompletionPerc: 0,
                NonBenefCompletionPerc: 0,
                NonBenefTotalQuestion:0,
                vmUpdateComment: '',
                NoOfSelectedBen: 0,
                UploadDocument: null,
                cascadingAnsArray: [],
                lstCountry: [],
                lstStates: [],
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
                infoSnackbarMsg: false,
                sharelinkStatusMesg: false,
                DataSavedSnackBar: false,
                lstImportBeneficiaryHearder: [],
                lstImportBeneficiary: [], // Shows imported Beneficiary List from excel
                BenefImportTab: 'tab-3',
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
                vmSelectedBeneficiaries: [],
                BenefStepper: 1,
                //survey dialog Non - Beneficiary linked
                NonBenLinkSurveyDlg: false,
                NonBenefLinkedSurveySteps: 1,
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
                AuditLog: [],
                PreLoader: false,
                InitialPreLoader: true,
                resetOnClick: 0,
                IsvalidexcelData: true,
                MultiChoiceAnsForRepeat: [],
                delList: [],
                lstAnyTimeNewSubmission: [],
                AnyTimeSubmissionId: 0,
                //mobile
                openMobileDialog: false,
                mobileTaskItem: '',

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
                Msg: "",
                //
                submitteddatetime:'',
                lstAllNewBeneficiariesQstnAns:[],
                WorkflowLevelOneUsers:'',
                WorkflowstageName:'',
                qstnkeyid: 0,
                projid: 0,
                dlgValidationMessage: false

            }
        },

        components: {
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'vue-web-cam': WebCam,
            "Pre-Loader": PreLoader,
        },
        mounted: async function () {
            await importscript();
            //this.SurveyID = this.$route.query.survid;
            //this.WorkspaceID = this.$route.query.wkspid;
            //const urlParams = new URLSearchParams(window.location.search);
            //this.SurveyID = urlParams.get("survid");
            var self = this;
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(position => {
                    self.Coordinates.lat = position.coords.latitude; 
                    self.Coordinates.lng = position.coords.longitude;
                    self.defaultCoordinates.lat = position.coords.latitude;
                    self.defaultCoordinates.lng = position.coords.longitude;
                });
            }
            await this.DecryptShareLinkURL();
            var sharelinkactive = await this.GetShareLinkStatus();
            if (sharelinkactive.status == 'invalid') //global.js, app.vue
            {
                //this.Msg = "Sharelink is not active. Please contact administrator.";
                //this.infoSnackbarMsg = true;
                this.sharelinkStatusMesg = true;
                this.InitialPreLoader=false;
                return false;
            }

            await this.GetWorkflowLevelOneUsers();

            var data = await this.GetShareLinkTaskFromServer();
            if (data != null) {
                if (data.data.MyTaskList != null && data.data.MyTaskList.length > 0) {


                    // Uncomment this if user needs alphabetical sorting in dropdowns

                    //var taskdatadetails = JSON.parse(data.data.MyTaskList[0].TaskData);

                    //if (taskdatadetails.SurveyQstnList != null)
                    //{
                    //    if (taskdatadetails.SurveyQstnList.length > 0)
                    //    {
                    //        for (var i = 0; i < taskdatadetails.SurveyQstnList.length; i++)
                    //        {
                    //            var obj = taskdatadetails.SurveyQstnList[i];

                    //            if (Array.isArray(obj.QuestionOptions)) {
                    //                (taskdatadetails.SurveyQstnList[i]).QuestionOptions.sort((a, b) => (a.OptionValue > b.OptionValue) ? 1 : ((b.OptionValue > a.OptionValue) ? -1 : 0));
                    //            }

                    //        }
                    //    }
                    //}

                    //if (taskdatadetails.BeneficiaryRegistrationQstns != null)
                    //{
                    //    if (taskdatadetails.BeneficiaryRegistrationQstns.length > 0)
                    //    {
                    //        for (var i = 0; i < taskdatadetails.BeneficiaryRegistrationQstns.length; i++)
                    //        {
                    //            var objBen = taskdatadetails.BeneficiaryRegistrationQstns[i];

                    //            if (Array.isArray(objBen.BTQstnAnsOptionList)) {
                    //                (taskdatadetails.BeneficiaryRegistrationQstns[i]).BTQstnAnsOptionList.sort((a, b) => (a.OptionValue > b.OptionValue) ? 1 : ((b.OptionValue > a.OptionValue) ? -1 : 0));
                    //            }
                    //        }
                    //    }
                    //}

                    //data.data.MyTaskList[0].TaskData = JSON.stringify(taskdatadetails);

                    await this.GetSurveyTaskDetails(data.data.MyTaskList[0], 0);
                    await this.GetBeneficiary();
                }
            }

        },

        computed: {
            device: function () {
                return this.devices.find(n => n.deviceId === this.deviceId);
            },
            //dummy function that shows the latest at the top
            timeline() {
                return this.events.slice().reverse();
            },
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
            searchtask: async function () {
                await this.GetTaskData();
            }
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

            async DecryptShareLinkURL() {
                var data = new FormData();
                var encryptedsurvid = decodeURIComponent(this.$route.query.survid);
                var encryptedwkspid = decodeURIComponent(this.$route.query.wkspid);
                data.append("EncryptedSurveyID", encryptedsurvid);
                data.append("EncryptedWorkspaceID", encryptedwkspid);
                var result = await usr.DecryptShareLinkURL(data);
                if (result != null && result.length > 1) {
                    this.SurveyID = result[0];
                    this.WorkspaceID = result[1];
                }
                else {
                    //encryption error
                }

            },

            async GetShareLinkStatus() {

                var result = await usr.GetShareLinkStatus(this.SurveyID);
                return result;
            },

            async GetWorkflowLevelOneUsers() {

                var result = await usr.GetWorkflowLevelOneUsers(this.SurveyID);
                if (result != null && result.workflowleveloneusers!=null) 
                {
                    this.WorkflowLevelOneUsers = result.workflowleveloneusers;
                    this.WorkflowstageName = 'Level 1';
                }
                return result;
            },

            async GetShareLinkTaskFromServer() {
                var result = await usr.GetShareLinkTaskFromServer(this.SurveyID, this.WorkspaceID);
                return result;

                //

                if (result.status == "success") {
                    var TypeID = 0;
                    var CurrentOrOverdue = "";
                    var DataHandledBy = "";
                    var Message = "";
                    var TaskStatus = "";
                    var dtTaskDate = new Date();
                    var Month = "";
                    var TaskUpdatedComment = "";
                    var lstTask = [];
                    lstTask = result.data.MyTaskList;
                    this.lstTaskList.sort(function (a, b) {
                        return parseInt(b.TypeID) - parseInt(a.TypeID);
                    });
                    for (var i = 0; i <= this.lstTaskList.length - 1; i++) {

                        var objSurvey = JSON.parse(this.lstTaskList[i].TaskData);
                        //objSurvey.pkId = this.lstTaskList[i].pkId;
                        if (objSurvey != null) {

                            if (this.lstTaskList[i].Type == 'Survey' && (TypeID != this.lstTaskList[i].TypeID || TypeID == 0)) {
                                this.TaskCardData.push(
                                    {
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
                                        TaskLog: []
                                    })
                            }
                            index = this.TaskCardData.length - 1;
                            CurrentOrOverdue = "Current";
                            DataHandledBy = this.lstTaskList[i].TaskdataAddedBy;
                            TaskUpdatedComment = "";
                            dtTaskDate = new Date(this.lstTaskList[i].TaskDate);

                            var dateMonth = new Date(dtTaskDate);//dtTaskDate.toLocaleString('default', { month: 'short' });
                            Month = this.lstmonth[dateMonth.getMonth()];

                            this.TaskCardData[index].TaskLog.push(
                                {
                                    TaskIndex: i + 1,
                                    ProjectName: objSurvey.ProjectName,
                                    ProjectId: objSurvey.ProjectId,
                                    TaskName: objSurvey.SurveyName,
                                    TaskType: 'Survey',
                                    TypeID: objSurvey.SurveyID,
                                    Name: DataHandledBy,
                                    Message: Message,
                                    CreatedDate: dtTaskDate,
                                    Comment: TaskUpdatedComment,
                                    Status: TaskStatus,
                                    CurrentOrOverdue: CurrentOrOverdue,
                                    Month: Month, AssignedTo: "",
                                    Periodicity: objSurvey.Periodicity,
                                    BeneficiaryType: objSurvey.BeneficiaryType,
                                    BeneficiaryTypeID: objSurvey.BeneficiaryTypeID,
                                    TaskDate: this.lstTaskList[i].TaskDate,
                                    SyncDBID: this.lstTaskList[i].SyncDBID,
                                    TypeIDColumn: this.lstTaskList[i].TypeIDColumn,
                                    WorkflowID: this.lstTaskList[i].WorkflowID,
                                    TotalWFLevel: this.lstTaskList[i].TotalWFLevel,
                                    DataCollectedBy: this.lstTaskList[i].DataCollectedBy,
                                    ResponseSyncID: this.lstTaskList[i].ResponseSyncID,
                                    WorkspaceID: this.lstTaskList[i].WorkspaceID,
                                    WFLevel: this.lstTaskList[i].WFLevel,
                                    WFLevelStatus: this.lstTaskList[i].WFLevelStatus,
                                    TaskData: objSurvey,
                                },
                            );

                        }
                    }
                }

                //if (result.status == "success") {
                //    var lstTask = [], lstCompletedTask = [], lstTaskAuditLog = [], updatedlist = [];
                //    lstTask = result.data.MyTaskList;
                //    lstCompletedTask = result.data.MyCompletedTask;
                //    lstTaskAuditLog = result.data.MyTaskAuditLog;
                //    serverdatetime = result.data.LastSyncDatetime;
                //    updatedlist = result.data.UpdatedTaskList;

                //    if (updatedlist != null && updatedlist.length > 0) {
                //        var MyTaskList = await dbImworks.MyTaskList.toArray();
                //        for (var cnt = 0; cnt < updatedlist.length; cnt++) {
                //            var ans = MyTaskList.filter(x => x.TypeID == updatedlist[cnt].TypeID && x.Type == updatedlist[cnt].Type && x.TaskDate == updatedlist[cnt].TaskDate && x.WFLevelStatus == "Pending");

                //            if (ans != null && ans.length > 0) {
                //                var tasklistjson = JSON.parse(ans[0].TaskData);
                //                if (tasklistjson != null && tasklistjson != undefined && (tasklistjson.Periodicity != "Anytime" || ans[0].WFLevel > 0)) {
                //                    await dbImworks.MyTaskList.where("pkId").equals(ans[0].pkId).modify({ WFLevelStatus: "Completed" }); //  Status: "Completed",
                //                }
                //            }
                //        }
                //    }

                //    var lstSynDBID = [];
                //    if (lstTask != null) {

                //        lstSynDBID = lstTask.lstMyTaskList[0];

                //        var lastfetch1 = await dbImworks.TaskSettings.where("UserId").equals(parseInt(loggedUser)).toArray();

                //        if (lastfetch1 != null && lastfetch1.length > 0) {
                //            if (serverdatetime != null && serverdatetime != undefined && serverdatetime.length > 0) {
                //                await dbImworks.TaskSettings.where("UserId").equals(loggedUser).modify({ ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID, LocalSyncDateTime: new Date(), LastSyncDateTime: serverdatetime });
                //            }
                //        } else {
                //            if (lstTask != null && lstTask.lstMyTaskList != null) {
                //                await dbImworks.TaskSettings.put({
                //                    ServerPKID: lstSynDBID.SyncDBID, TaskType: lstSynDBID.Type, TaskTypeID: lstSynDBID.TypeID,
                //                    LocalSyncDateTime: new Date(), ClientPKID: "", UserId: loggedUser, LastSyncDateTime: serverdatetime
                //                });
                //            }
                //        }

                //        if (lstTask.lstMyTaskList != null && lstTask.lstMyTaskList.length > 0) {
                //            await dbImworks.MyTaskList.bulkPut(lstTask.lstMyTaskList).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskList.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskDM != null && lstTask.lstMyTaskDM.length > 0) {
                //            await dbImworks.MyTaskDM.bulkPut(lstTask.lstMyTaskDM).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskDM.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveySection != null && lstTask.lstMyTaskSurveySection.length > 0) {
                //            await dbImworks.MyTaskSurveySection.bulkPut(lstTask.lstMyTaskSurveySection).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveySection.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveyQuestion != null && lstTask.lstMyTaskSurveyQuestion.length > 0) {
                //            await dbImworks.MyTaskSurveyQuestion.bulkPut(lstTask.lstMyTaskSurveyQuestion).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveyQuestion.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveySectionLogic != null && lstTask.lstMyTaskSurveySectionLogic.length > 0) {
                //            await dbImworks.MyTaskSurveySectionLogic.bulkPut(lstTask.lstMyTaskSurveySectionLogic).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveySectionLogic.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveyTrackDate != null && lstTask.lstMyTaskSurveyTrackDate.length > 0) {
                //            await dbImworks.MyTaskSurveyTrackDate.bulkPut(lstTask.lstMyTaskSurveyTrackDate).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveyTrackDate.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskTable != null && lstTask.lstTbMyTaskTable.length > 0) {
                //            await dbImworks.TbMyTaskTable.bulkPut(lstTask.lstTbMyTaskTable).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskTable.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskProjectSurveyQSTCondition != null && lstTask.lstMyTaskProjectSurveyQSTCondition.length > 0) {
                //            await dbImworks.MyTaskProjectSurveyQSTCondition.bulkPut(lstTask.lstMyTaskProjectSurveyQSTCondition).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskProjectSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskProjectBenSurveyQSTCondition != null && lstTask.lstMyTaskProjectBenSurveyQSTCondition.length > 0) {
                //            await dbImworks.MyTaskProjectBenSurveyQSTCondition.bulkPut(lstTask.lstMyTaskProjectBenSurveyQSTCondition).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskProjectBenSurveyQSTCondition.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskBeneficiarySurveyQSTOption != null && lstTask.lstMyTaskBeneficiarySurveyQSTOption.length > 0) {
                //            await dbImworks.MyTaskBeneficiaryTypeQuestionOption.bulkPut(lstTask.lstMyTaskBeneficiarySurveyQSTOption).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskBeneficiarySurveyQSTOption.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskProjectSurveyQSTOption != null && lstTask.lstMyTaskProjectSurveyQSTOption.length > 0) {
                //            await dbImworks.MyTaskProjectSurveyQSTOption.bulkPut(lstTask.lstMyTaskProjectSurveyQSTOption).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskProjectSurveyQSTOption.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveyCascadingQuestionOptions != null && lstTask.lstMyTaskSurveyCascadingQuestionOptions.length > 0) {
                //            await dbImworks.MyTaskSurveyCascadingQuestionOptions.bulkPut(lstTask.lstMyTaskSurveyCascadingQuestionOptions).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveyCascadingQuestionOptions.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions != null && lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions.length > 0) {
                //            await dbImworks.MyTaskSurveyBeneficiaryCascadingQuestionOptions.bulkPut(lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskBeneficiarySurveyCascadingQuestionOptions.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskProjectLogicConditions != null && lstTask.lstTbMyTaskProjectLogicConditions.length > 0) {
                //            await dbImworks.TbMyTaskProjectLogicConditions.bulkPut(lstTask.lstTbMyTaskProjectLogicConditions).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskProjectLogicConditions.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskProjectBeneficiaryType != null && lstTask.lstTbMyTaskProjectBeneficiaryType.length > 0) {
                //            await dbImworks.TbMyTaskProjectBeneficiaryType.bulkPut(lstTask.lstTbMyTaskProjectBeneficiaryType).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskProjectBeneficiaryType.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskBeneficiaryAnswerType != null && lstTask.lstTbMyTaskBeneficiaryAnswerType.length > 0) {
                //            await dbImworks.TbMyTaskBeneficiaryAnswerType.bulkPut(lstTask.lstTbMyTaskBeneficiaryAnswerType).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskBeneficiaryAnswerType.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskBeneficiaryRegistrationDetails != null && lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.length > 0) {
                //            await dbImworks.TbMyTaskBeneficiaryRegistrationDetails.bulkPut(lstTask.lstTbMyTaskBeneficiaryRegistrationDetails).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskBeneficiaryRegistrationDetails.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstTbMyTaskProjectBeneficiary != null && lstTask.lstTbMyTaskProjectBeneficiary.length > 0) {
                //            await dbImworks.TbMyTaskProjectBeneficiary.bulkPut(lstTask.lstTbMyTaskProjectBeneficiary).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstTbMyTaskProjectBeneficiary.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskBeneficiaryTypeQuestion != null && lstTask.lstMyTaskBeneficiaryTypeQuestion.length > 0) {
                //            await dbImworks.MyTaskBeneficiaryTypeQuestion.bulkPut(lstTask.lstMyTaskBeneficiaryTypeQuestion).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskBeneficiaryTypeQuestion.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskMasterList != null && lstTask.lstMyTaskMasterList.length > 0) {
                //            await dbImworks.MyTaskMasterList.bulkPut(lstTask.lstMyTaskMasterList).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskMasterList.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskQuestionType != null && lstTask.lstMyTaskQuestionType.length > 0) {
                //            await dbImworks.MyTaskQuestionType.bulkPut(lstTask.lstMyTaskQuestionType).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskQuestionType.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //        if (lstTask.lstMyTaskSurveyResponse != null && lstTask.lstMyTaskSurveyResponse.length > 0) {
                //            await dbImworks.MyTaskProjectSurveyResponses.bulkPut(lstTask.lstMyTaskSurveyResponse).then(function (lastKey) {
                //                console.log("Last data's id was: " + lastKey);
                //            }).catch(Dexie.BulkError, function (e) {
                //                console.error("Some data did not succeed. However, " +
                //                    lstTask.lstMyTaskSurveyResponse.length - e.failures.length + " data was added successfully");
                //            });
                //        }

                //    } else {
                //        if (lastfetch != null && lastfetch.length > 0) {
                //            if (serverdatetime != null && serverdatetime != undefined && serverdatetime.length > 0) {
                //                await dbImworks.TaskSettings.where("UserId").equals(loggedUser).modify({ LocalSyncDateTime: new Date(), LastSyncDateTime: serverdatetime });
                //            }
                //        }
                //    }


                //    // Inserting to Completed Task
                //    if (lstCompletedTask != null && lstCompletedTask.length > 0) {
                //        //  await dbImworks.TbMyTask.clear();
                //        await dbImworks.TbMyTask.bulkPut(lstCompletedTask).then(function (lastKey) {
                //            console.log("Last data's id was: " + lastKey);
                //        }).catch(Dexie.BulkError, function (e) {
                //            console.error("Some data did not succeed. However, " +
                //                lstCompletedTask.length - e.failures.length + " data was added successfully");
                //        });
                //    }
                //    // Inserting to Audit Log
                //    if (lstTaskAuditLog != null && lstTaskAuditLog.length > 0) {
                //        //await dbImworks.TbTaskAuditLog.clear();
                //        await dbImworks.TbTaskAuditLog.bulkPut(lstTaskAuditLog).then(function (lastKey) {
                //            console.log("Last data's id was: " + lastKey);
                //        }).catch(Dexie.BulkError, function (e) {
                //            console.error("Some data did not succeed. However, " +
                //                lstTaskAuditLog.length - e.failures.length + " data was added successfully");
                //        });
                //    }

                //}

            },

            async exporttoexcel(myarrayobject) 
            
            {
                const XLSX = require('xlsx');

                // array of objects to save in Excel
                //let binary_univers = [{ 'First Name': 'Rahul', 'Date of Birth': '1988-02-03','pin code':'682030','Your Name':'Ben_R_0402_2','Date of Birth|1':'1959-11-29','Address':'pqrst','Mode of Payment':'\"Paytm,GooglePay\"','QR Code number':'' }, 
                //    { 'First Name': 'Dhanesh', 'Date of Birth': '1988-05-18', 'pin code': '682030', 'Your Name': 'Ben_D_0402_2', 'Date of Birth|1': '1919-08-30', 'Address': 'stuv', 'Mode of Payment': '\"GooglePay,PhonePay\"', 'QR Code number': '' }];
                let binary_univers = myarrayobject;

                let binaryWS = XLSX.utils.json_to_sheet(binary_univers);

                //Same question may be repeating(eg. Date of Birth). for unique identification of question for grouping in json_to_sheet function, appended a number after | symbol. So remove text after | in binaryWS using split below.

                let obj;
                obj = Object.keys(binaryWS).forEach((el) => {
                    if (binaryWS[el].v != undefined) 
                    {
                        binaryWS[el].v = binaryWS[el].v.split('|')[0];
                    }
                    //console.log(binaryWS[el]);
                })

                // Create a new Workbook
                var wb = XLSX.utils.book_new();

                // Name your sheet
                XLSX.utils.book_append_sheet(wb, binaryWS, 'ShareLink');

                // export your excel
                XLSX.writeFile(wb, "sharelink_download_summary" + Math.round(Math.random() * 1000) + '.xlsx');
                return this;
            },

            objectkeyExists(beneficiaryobject,objkey) 
            {
                //if a question repeats, excel function json_to_sheet will group it into one. So to avoid that, if the question already exists in the object, a number after | symbol appends to the next question
                var isKeyExists = beneficiaryobject.hasOwnProperty(objkey);
                var newobjkey = objkey;
                if (isKeyExists) 
                {
                    this.qstnkeyid += 1;
                   //var guid = Math.floor(1000000000 + Math.random() * 9000000000);
                    newobjkey = newobjkey + '|' + this.qstnkeyid;
                }
                return newobjkey;
            },

            async getBeneficiaryRegQstnAnswers() 
            { 
                var BeneficiaryIds = '';
                var BeneficiaryDetails = [];
                BeneficiaryIds += (this.lstSelectedBeneficiary.map(a => a.BeneficiaryID)).toString();
                var result = await objPB.GetBeneficiaryQstnAnsw(BeneficiaryIds);
                if (result.status == "Unhandled Exception") 
                {
                    this.$router.push({ name: "error" });
                }
                else if (result.data) {
                    BeneficiaryDetails = result.data;
                }

                return BeneficiaryDetails;
            },

            async downloadShareLinkSummary(type) 
            {
                this.PreLoader = true;
                var Questions = [];
                var skipsectionsidsUsed = [];
                var qstn_answ_objects_array = [];
                if (type == 'beneficiarylinked') 
                {
                    var BeneficiaryQstnAnswList = await this.getBeneficiaryRegQstnAnswers();

                    var BeneficiarySectionQstnList_COPY = this.BeneficiarySectionQstnList;
                    
                    for (var BenCnt = 0; BenCnt <= this.BeneficiarySectionQstnList.length - 1; BenCnt++) 
                    {
                        var onebeneficiaryobject = new Object();

                        var commonquestion1 = 'Submitted date & time';
                        commonquestion1 = this.objectkeyExists(onebeneficiaryobject, commonquestion1);
                        var commonananswer1 = objUtils.common.enclosequotes(this.submitteddatetime);
                        onebeneficiaryobject[commonquestion1] = commonananswer1;

                        var commonquestion2 = 'Submitted by';
                        commonquestion2 = this.objectkeyExists(onebeneficiaryobject, commonquestion2);
                        var commonananswer2 = objUtils.common.enclosequotes('sharelinkemail@test.com');
                        onebeneficiaryobject[commonquestion2] = commonananswer2;

                        //// Existing beneficiary Reg Question Answers 
                        if (BeneficiaryQstnAnswList.length > 0) 
                        {
                            
                            for (var i = 0; i < BeneficiaryQstnAnswList.length; i++) 
                            { // beneficiaries
                                if (BeneficiaryQstnAnswList[i].BeneficiaryName == this.BeneficiarySectionQstnList[BenCnt].BeneficiaryName) 
                                {

                                    var qstn = objUtils.common.enclosequotes(BeneficiaryQstnAnswList[i].QuestionName);
                                    qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                    var ans = objUtils.common.enclosequotes(BeneficiaryQstnAnswList[i].Answer);
                                    onebeneficiaryobject[qstn] = ans;
                                    
                                }
                            }
                        }

                        //// if new ben registration is there, beneficiary Reg Question Answers 
                        if (this.lstBenefRegQstns.length > 0) 
                        {
                            for (var i = 0; i < this.lstAllNewBeneficiariesQstnAns.length; i++) // beneficiaries
                            {
                                for (var j = 0; j < this.lstBenefRegQstns.length; j++) //questions
                                {
                                    if (this.lstAllNewBeneficiariesQstnAns[i][j].QuestionID == this.lstBenefRegQstns[j].QuestionID) 
                                    {
                                        if (this.lstAllNewBeneficiariesQstnAns[i][0].Answer == this.BeneficiarySectionQstnList[BenCnt].BeneficiaryName) 
                                        {

                                            var qstn = objUtils.common.enclosequotes(this.lstBenefRegQstns[j].QuestionName);
                                            qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                            var ans = objUtils.common.enclosequotes(this.lstAllNewBeneficiariesQstnAns[i][j].Answer);
                                            onebeneficiaryobject[qstn] = ans;
                                        }
                                    }
                                }
                            }
                        }
                        //

                        //Survey questions Answers
                        var Sections = this.BeneficiarySectionQstnList[BenCnt].Sections;
                        for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) 
                        {
                            var issectionused = skipsectionsidsUsed.includes(Sections[SecCnt].SectionID);
                            if (issectionused) 
                            {
                                continue;
                            }
                            Questions = [];
                            if (Sections[SecCnt].IslogicSection == 1) 
                            {
                                Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.benefIndex == Sections[SecCnt].BenefIndex && x.QuestionID > 0);
                                if (Questions.length == 0) //if skip question is not answered, taking section details from main array.
                                {
                                    Questions = Sections[SecCnt].Questions;
                                }
                            }
                            else 
                            {
                                Questions = Sections[SecCnt].Questions;
                            }

                            for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) 
                            {
                                var Obj = new Object();
                                Obj.QuestionID = Questions[QCnt].QuestionID;

                                if (Questions[QCnt].QuestionTypeID == 9) //gps
                                {
                                    if (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer == '') 
                                    {
                                        var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                                        
                                        var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                        qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                        var ans = objUtils.common.enclosequotes(gpsCoordinates);
                                        onebeneficiaryobject[qstn] = ans;
                                    }
                                    else 
                                    {

                                        var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                        qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                        var ans = objUtils.common.enclosequotes(Questions[QCnt].Answer);
                                        onebeneficiaryobject[qstn] = ans;
                                    }
                                }
                                else 
                                {
                                    
                                    var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                    qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                    var ans = objUtils.common.enclosequotes(Questions[QCnt].Answer);
                                    onebeneficiaryobject[qstn] = ans;
                                }

                                var isconditionqstn = Questions[QCnt].IsConditionQuestion;
                                if (isconditionqstn == 1 || isconditionqstn) 
                                {
                                    //var SkipSectionID = [...new Set(Questions[QCnt].SectionLogic.map(a => a.SkipSectionID))];
                                    //var sectiondata = this.RepeatOrSkipSection.filter(x => x.SectionID == SkipSectionID[0] && x.benefIndex == Sections[SecCnt].BenefIndex && x.QuestionID > 0);
                                    var sectiondata = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID == Obj.QuestionID && x.benefIndex == Sections[SecCnt].BenefIndex && x.QuestionID > 0);
                                    if (sectiondata.length > 0) 
                                    {
                                        var SkipSectionID = [...new Set(sectiondata.map(a => a.SectionID))];
                                        var skipsectionquestions = sectiondata;
                                        for (var i = 0; i <= skipsectionquestions.length - 1; i++) 
                                        {
                                            
                                            var qstn = objUtils.common.enclosequotes(skipsectionquestions[i].QuestionName);
                                            qstn = this.objectkeyExists(onebeneficiaryobject,qstn);
                                            var ans = objUtils.common.enclosequotes(skipsectionquestions[i].Answer);
                                            onebeneficiaryobject[qstn] = ans;
                                        }
                                        skipsectionsidsUsed.push(SkipSectionID[0]);
                                    }

                                    if (sectiondata.length == 0) 
                                    {//skip logic questions not attended, copy is used because , main array value changes sometimes.
                                        var SkipSectionID_not = BeneficiarySectionQstnList_COPY[BenCnt].Sections[SecCnt].Questions[QCnt].SectionLogic.filter(item => item.QuestionID === Obj.QuestionID).map(item => item.SkipSectionID);
                                        var sectiondata_not = BeneficiarySectionQstnList_COPY[BenCnt].Sections.filter(x => x.SectionID == SkipSectionID_not[0]);
                                        if (sectiondata_not.length > 0) {
                                            var skipsectionquestions_not = sectiondata_not[0].Questions;
                                            for (var i = 0; i <= skipsectionquestions_not.length - 1; i++) 
                                            {
                                                var qstn = objUtils.common.enclosequotes(skipsectionquestions_not[i].QuestionName);
                                                qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                                var ans = objUtils.common.enclosequotes(skipsectionquestions_not[i].Answer); // blank answer
                                                onebeneficiaryobject[qstn] = ans;
                                            }
                                            skipsectionsidsUsed.push(SkipSectionID_not[0]);
                                        }
                                    }

                                }

                            }
                        }

                        var commonquestion3 = 'Workflow Stage Name';
                        commonquestion3 = this.objectkeyExists(onebeneficiaryobject, commonquestion3);
                        var commonananswer3 = objUtils.common.enclosequotes(this.WorkflowstageName);
                        onebeneficiaryobject[commonquestion3] = commonananswer3;

                        var commonquestion4 = 'Workflow stage User';
                        commonquestion4 = this.objectkeyExists(onebeneficiaryobject, commonquestion4);
                        var commonananswer4 = objUtils.common.enclosequotes(this.WorkflowLevelOneUsers);
                        onebeneficiaryobject[commonquestion4] = commonananswer4;

                        var commonquestion5 = 'Workflow Status';
                        commonquestion5 = this.objectkeyExists(onebeneficiaryobject, commonquestion5);
                        var commonananswer5 = objUtils.common.enclosequotes('WIP');
                        onebeneficiaryobject[commonquestion5] = commonananswer5;

                        this.qstnkeyid = 0;
                        skipsectionsidsUsed = [];
                        qstn_answ_objects_array.push(onebeneficiaryobject);

                    }

                }
                if (type == 'nonbeneficiarylinked') 
                {
                    var onebeneficiaryobject = new Object();

                    var commonquestion1 = 'Submitted date & time';
                    commonquestion1 = this.objectkeyExists(onebeneficiaryobject, commonquestion1);
                    var commonananswer1 = objUtils.common.enclosequotes(this.submitteddatetime);
                    onebeneficiaryobject[commonquestion1] = commonananswer1;

                    var commonquestion2 = 'Submitted by';
                    commonquestion2 = this.objectkeyExists(onebeneficiaryobject, commonquestion2);
                    var commonananswer2 = objUtils.common.enclosequotes('sharelinkemail@test.com');
                    onebeneficiaryobject[commonquestion2] = commonananswer2;

                    var Sections = this.lstSectionsQstns;
                    var lstSectionsQstns_COPY = this.lstSectionsQstns;
                    for (var SecCnt = 0; SecCnt <= Sections.length - 1; SecCnt++) 
                    {
                        var issectionused = skipsectionsidsUsed.includes(Sections[SecCnt].SectionID);
                        if (issectionused) 
                        {
                            continue;
                        }
                        Questions = [];

                        if (Sections[SecCnt].IslogicSection == 1) 
                        {
                            Questions = this.RepeatOrSkipSection.filter(x => x.SectionID == Sections[SecCnt].SectionID && x.QuestionID > 0)
                            if (Questions.length == 0) //if skip question is not answered, taking section details from main array.
                            {
                                Questions = Sections[SecCnt].Questions;
                            }
                        }
                        else 
                        {
                            Questions = Sections[SecCnt].Questions;
                        }

                        // 
                        for (var QCnt = 0; QCnt <= Questions.length - 1; QCnt++) 
                        {
                            var Obj = new Object();
                            Obj.QuestionID = Questions[QCnt].QuestionID;

                            if (Questions[QCnt].QuestionTypeID == 2 && this.multiChoiceAns != null && this.multiChoiceAns != undefined && this.multiChoiceAns.length > 0) 
                            {
                                var filterchoice = this.multiChoiceAns.filter(x => x.QuestionID == Obj.QuestionID);
                                var answerchoice = "";
                                if (filterchoice != null && filterchoice != undefined && filterchoice.length > 0) 
                                {
                                    for (var chccount = 0; chccount < filterchoice.length; chccount++) 
                                    {
                                        if (answerchoice.length > 0) 
                                        {
                                            answerchoice = answerchoice + ',' + filterchoice[chccount].OptionValue;
                                        }
                                        else 
                                        {
                                            answerchoice = filterchoice[chccount].OptionValue;
                                        }
                                    }
                                }

                                //lstAnswers.push(objUtils.common.enclosequotes(answerchoice));

                                var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                var ans = objUtils.common.enclosequotes(answerchoice);
                                onebeneficiaryobject[qstn] = ans;
                            }
                            else 
                            {
                                if (Questions[QCnt].QuestionTypeID == 9) //gps
                                {
                                    if (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer == '') 
                                    {
                                        var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();

                                        //lstAnswers.push(objUtils.common.enclosequotes(gpsCoordinates));

                                        var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                        qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                        var ans = objUtils.common.enclosequotes(gpsCoordinates);
                                        onebeneficiaryobject[qstn] = ans;
                                    }
                                    else 
                                    {
                                        //lstAnswers.push(objUtils.common.enclosequotes(Questions[QCnt].Answer)); 

                                        var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                        qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                        var ans = objUtils.common.enclosequotes(Questions[QCnt].Answer);
                                        onebeneficiaryobject[qstn] = ans;
                                    }
                                }
                                else 
                                {
                                    //lstAnswers.push(objUtils.common.enclosequotes(Questions[QCnt].Answer));

                                    var qstn = objUtils.common.enclosequotes(Questions[QCnt].QuestionName);
                                    qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                    var ans = objUtils.common.enclosequotes(Questions[QCnt].Answer);
                                    onebeneficiaryobject[qstn] = ans;
                                }
                            }

                            var isconditionqstn = Questions[QCnt].IsConditionQuestion;
                            if (isconditionqstn == 1 || isconditionqstn) 
                            {
                                //var SkipSectionID = [...new Set(Questions[QCnt].SectionLogic.map(a => a.SkipSectionID))];
                                //var sectiondata = this.RepeatOrSkipSection.filter(x => x.SectionID == SkipSectionID[0] && x.QuestionID > 0);
                                
                                var sectiondata = this.RepeatOrSkipSection.filter(x => x.ParentQuestionID == Obj.QuestionID && x.QuestionID > 0);
                                if (sectiondata.length > 0) 
                                {
                                    var SkipSectionID = [...new Set(sectiondata.map(a => a.SectionID))];
                                    var skipsectionquestions = sectiondata;
                                    for (var i = 0; i <= skipsectionquestions.length - 1; i++) 
                                    {

                                        var qstn = objUtils.common.enclosequotes(skipsectionquestions[i].QuestionName);
                                        qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                        var ans = objUtils.common.enclosequotes(skipsectionquestions[i].Answer);
                                        onebeneficiaryobject[qstn] = ans;
                                    }
                                    skipsectionsidsUsed.push(SkipSectionID[0]);
                                }

                                if (sectiondata.length == 0) 
                                {//skip logic questions not attended, copy is used because , main array value changes sometimes.
                                    var SkipSectionID_not = lstSectionsQstns_COPY[SecCnt].Questions[QCnt].SectionLogic.filter(item => item.QuestionID === Obj.QuestionID).map(item => item.SkipSectionID);
                                    var sectiondata_not = lstSectionsQstns_COPY.filter(x => x.SectionID == SkipSectionID_not[0]);
                                    if (sectiondata_not.length > 0) 
                                    {
                                        var skipsectionquestions_not = sectiondata_not[0].Questions;
                                        for (var i = 0; i <= skipsectionquestions_not.length - 1; i++) 
                                        {
                                            var qstn = objUtils.common.enclosequotes(skipsectionquestions_not[i].QuestionName); 
                                            qstn = this.objectkeyExists(onebeneficiaryobject, qstn);
                                            var ans = objUtils.common.enclosequotes(skipsectionquestions_not[i].Answer);// blank answer
                                            onebeneficiaryobject[qstn] = ans;
                                        }
                                        skipsectionsidsUsed.push(SkipSectionID_not[0]);
                                    }
                                }

                            }

                        }//
                    }

                    var commonquestion3 = 'Workflow Stage Name';
                    commonquestion3 = this.objectkeyExists(onebeneficiaryobject, commonquestion3);
                    var commonananswer3 = objUtils.common.enclosequotes(this.WorkflowstageName);
                    onebeneficiaryobject[commonquestion3] = commonananswer3;

                    var commonquestion4 = 'Workflow stage User';
                    commonquestion4 = this.objectkeyExists(onebeneficiaryobject, commonquestion4);
                    var commonananswer4 = objUtils.common.enclosequotes(this.WorkflowLevelOneUsers);
                    onebeneficiaryobject[commonquestion4] = commonananswer4;

                    var commonquestion5 = 'Workflow Status';
                    commonquestion5 = this.objectkeyExists(onebeneficiaryobject, commonquestion5);
                    var commonananswer5 = objUtils.common.enclosequotes('WIP');
                    onebeneficiaryobject[commonquestion5] = commonananswer5;

                    this.qstnkeyid = 0;
                    skipsectionsidsUsed = [];
                    qstn_answ_objects_array.push(onebeneficiaryobject);

                }
                
                this.exporttoexcel(qstn_answ_objects_array);
                this.PreLoader = false;
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
                    len = QItem.CascadingQuestionOptions[QItem.CascadingQuestionOptions.length - 1].HierarchyOrder;
                }
                else {
                    // Fetching the count/ length of the cascading question option using RepeatSectionIndexID
                    //len = IsBenefQstn ? this.lstBenefRegQstns.filter(x => x.QuestionID == QItem.QuestionID)[0].CascadingQuestionOptions.length : this.RepeatSection.filter(x => x.QuestionID == QItem.QuestionID && x.RQstInd == RepeatSectionIndexID)[0].CascadingQuestionOptions.length;
                }
                if (len >= nextHierarchyOrder) {
                    var nxtList = [];
                    if (RepeatSectionIndexID == -1) // if the cascading question is not comes under a repeat section , then the value of RepeatSectionIndexID == -1
                    {
                        nxtList = QItem.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder);
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
                        QItem.CascadingQuestionOptions.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].CascadingList = sublist;
                    }

                }

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


            //

            callback(data) {
                //console.debug(data)
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
                const validImageTypes = ['image/gif', 'image/jpeg', 'image/png', 'image/jpg', 'image/jpg"'];
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

            async  SaveSignatureRepeatOrSkip(QItem, IsBenefQstn, benefIndx, RQstInd) {
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

                    ////Filter the Question Based on Signature Question Type
                    //var SignatureQstnList = this.lstSectionsQstns.filter(x => x.SectionID == QItem.SectionID)[0].Questions.filter(x => x.QuestionType == "Signature Capture");
                    //// Getting the index of the signature question
                    //const Qindex = SignatureQstnList.findIndex((element, index) => {
                    //    if (element.QuestionID === this.AnsBlobQID) {
                    //        return true
                    //    }
                    //})
                    signPad = this.$refs.signaturePad[1].signaturePad;
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
                            ProjectID: this.lstSelectedTaskData.ProjectId, DocLinkedTo: this.lstSelectedTaskData.TaskName, IsUploadedFile: isUploadedFile, QuestionID: QuestionID
                        });
                        this.blobFileInputBT = null;
                    }
                    else {

                        await dbImworks.TbTaskResponseDocument.put({

                            SyncTaskID: this.lstSelectedTaskData.SyncDBID, TaskDate: this.lstSelectedTaskData.TaskDate, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0,
                            AttachedDocument: pblobFileInput, FileName: FileName, DocumentTypeIcon: FileName.split('.').pop(),
                            ProjectID: this.lstSelectedTaskData.ProjectId, DocLinkedTo: this.lstSelectedTaskData.TaskName, IsUploadedFile: isUploadedFile,
                            BeneficiaryID: BeneficiaryID, QuestionID: QuestionID
                        });

                        this.blobFileInput = null;
                    }
                }
            },
            //

            //Function used to check section logic for Multi-Choice Questions
            async AddSectionLogicForMultiChoice(item, QuestionID, benefIndex, QuestionOptions) {

                for (var SecCnt = 0; SecCnt <= this.lstSectionsQstns.length - 1; SecCnt++) {
                    var Questions = this.lstSectionsQstns[SecCnt].Questions;
                    if (typeof Questions.filter(x => x.QuestionID == QuestionID)[0] != 'undefined') {

                        if (Questions.filter(x => x.QuestionID == QuestionID)[0].IsConditionQuestion == 1) {

                            var SectionLogic = Questions.filter(x => x.QuestionID == QuestionID)[0].SectionLogic;
                            SectionLogic = SectionLogic.filter(x => x.QuestionID == QuestionID);
                            if (SectionLogic.length > 0) {
                                var SelectedOption = QuestionOptions.filter(x => x.IsOptSelected == true);
                                var IsLogicExists = SectionLogic.filter(function (o1) {
                                    return SelectedOption.some(function (o2) {
                                        return o1.Answer === o2.OptionValue; // return the ones with equal id
                                    });
                                });
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
                                                    var SelectedAndAns = 0;
                                                    for (var i = 0; i < item.QuestionOptions.length; i++) {
                                                        for (var j = 0; j < SectionLogic.length; j++) {
                                                            if (item.QuestionOptions[i].OptionValue == SectionLogic[j].Answer && item.QuestionOptions[i].IsOptSelected == true) {
                                                                SelectedAndAns++
                                                            }
                                                        }
                                                    }
                                                    //if (eval(InputAnswer + SectionLogic[LogicCnt].ConditionValue + InputStringAnswer)) {
                                                    if (SelectedAndAns == SectionLogic.length) {
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
                            SectionID: currSecQstns[0].SectionID, SectionName: currSecQstns[0].SectionName, SectionOrder: currSecQstns[0].SectionOrder,
                            QuestionOrder: 1, QuestionTypeID: 0, QuestionID: 0, QuestionName: currSecQstns[0].SectionName + " : " + j, SectionLogic: [], QuestionType: "",
                            IsFormulaQuestion: 0, IsConditionQuestion: 0, QuestionCondition: null, QuestionOptions: null, QuestionHint: "", IsValidationQuestion: 0,
                            RegEx: "", Answer: null, FieldVar: "", IsUnique: 0, IsUpload: 0, ErrorText: "", DefaultValue: "", IsMandatory: 0, IsSearchable: 0, QuestionCategory: 1,
                            FormulaOperation: "", FormulaExpression: "", FormulaFieldVariables: null, CascadingQuestionOptions: null, UOM: "", IsValidationError: false, ValidationErrorTest: "", DateMenu: false, ParentQuestionID: HirachyParentQID, IndexPos: i + 1, startCamera: false, camOn: false, img: '', uploadedImg: '', uploadedFile: '', benefIndex: benefIndex, BeneficiaryID: BeneficiaryID, isOtherOptionEnabled: false,
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
                                        ObjOptions.IsOptSelected = false;
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
                                    SectionID: lstQuestions[qCnt].SectionID, SectionName: lstQuestions[qCnt].SectionName, SectionOrder: lstQuestions[qCnt].SectionOrder,
                                    QuestionOrder: lstQuestions[qCnt].QuestionOrder, QuestionTypeID: lstQuestions[qCnt].QuestionTypeID, QuestionID: lstQuestions[qCnt].QuestionID,
                                    QuestionName: lstQuestions[qCnt].QuestionName, SectionLogic: lstQuestions[qCnt].SectionLogic, QuestionType: lstQuestions[qCnt].QuestionType,
                                    IsFormulaQuestion: lstQuestions[qCnt].IsFormulaQuestion, IsConditionQuestion: lstQuestions[qCnt].IsConditionQuestion, QuestionCondition: lstQuestions[qCnt].QuestionCondition,
                                    QuestionOptions: lstQuestions[qCnt].QuestionOptions, QuestionHint: lstQuestions[qCnt].QuestionHint, IsValidationQuestion: lstQuestions[qCnt].IsValidationQuestion,
                                    RegEx: lstQuestions[qCnt].RegEx, Answer: lstQuestions[qCnt].Answer, FieldVar: lstQuestions[qCnt].FieldVar, IsUnique: lstQuestions[qCnt].IsUnique, IsUpload: lstQuestions[qCnt].IsUpload,
                                    ErrorText: lstQuestions[qCnt].ErrorText, DefaultValue: lstQuestions[qCnt].DefaultValue, IsMandatory: lstQuestions[qCnt].IsMandatory, IsSearchable: lstQuestions[qCnt].IsSearchable,
                                    QuestionCategory: lstQuestions[qCnt].QuestionCategory, FormulaOperation: lstQuestions[qCnt].FormulaOperation, FormulaExpression: lstQuestions[qCnt].FormulaExpression, UOM: lstQuestions[qCnt].UOM,
                                    FormulaFieldVariables: lstQuestions[qCnt].FormulaFieldVariables, CascadingQuestionOptions: lstQuestions[qCnt].CascadingQuestionOptions, IsValidationError: false, ValidationErrorTest: "", DateMenu: false, ParentQuestionID: HirachyParentQID, IndexPos: i + 1, startCamera: false, camOn: false, img: '', uploadedImg: '', uploadedFile: '', benefIndex: benefIndex, BeneficiaryID: BeneficiaryID,
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

            async GetSurveyTaskDetails(TaskDataitem, pkId) {
                if (typeof TaskDataitem != "undefined") {

                    this.AnyTimeSubmissionId = pkId;

                    this.IsBeneficiaryLinkedSurvey = false;

                    //TaskDataitem.TaskData = await this.GetTaskDataList(TaskDataitem.TypeID, TaskDataitem.SyncDBID, TaskDataitem.TaskData, TaskDataitem.ResponseSyncID, TaskDataitem.BeneficiaryTypeID);
                    TaskDataitem.TaskData = JSON.parse(TaskDataitem.TaskData);
                    this.projid = TaskDataitem.TaskData.ProjectId;
                    this.SelectedTaskType = TaskDataitem.Type;
                    this.SelectedTaskTypeID = TaskDataitem.TypeID;
                    this.SelectedSurveyName = TaskDataitem.TaskData.SurveyName;
                    //this.DataCollectorInfo.DatacollectorName = TaskDataitem.Name;
                    //this.DataCollectorInfo.AssignedTime = this.FormatDateWithTime(TaskDataitem.CreatedDate);
                    this.lstSelectedTaskData = TaskDataitem;
                    this.SelectedTaskDate = TaskDataitem.TaskDate;
                    //this.SelectedTaskIndex = TaskDataitem.TaskIndex;
                    //this.SelectedBenefTypeID = TaskDataitem.BeneficiaryTypeID;

                    this.UnSavedNewBenfList = await dbImworks.TbNewBeneficiary.toArray();
                    this.filterNewBenefList = this.UnSavedNewBenfList.filter(x => x.BeneficiaryTypeID == this.SelectedBenefTypeID && x.WorkspaceID == this.WorkspaceID);

                    if (TaskDataitem.TaskData.BeneficiaryTypeID > 0) {
                        this.IsBeneficiaryLinkedSurvey = true;
                    }
                    else {
                        this.NonBenefLinkedSurveySteps = 1;
                        await this.GetNonBeneficiayBasedSectionQuestions();
                    }


                    // Goto Approval
                    if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Pending" || TaskDataitem.CurrentOrOverdue == "Current") && this.IsBeneficiaryLinkedSurvey) {
                        // Goto Approval Page
                        this.surveyApproveBenfLinkedDialog = true;
                        this.BenefLinkedSurveySteps = 6;
                        await this.fnCalcProgressApprover();
                        await this.fnGetApproverQstnAnswers();
                    }
                    if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Pending" || TaskDataitem.CurrentOrOverdue == "Current") && !this.IsBeneficiaryLinkedSurvey) {

                        // Goto Approval Page
                        this.surveyApproveNonBenfLinkedDialog = true;
                        //await this.fnCalcProgressApprover();
                        await this.fnGetNonBenfApproverQstnAnswers();
                    }
                    // Already Data collected beneficiary linked
                    if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Syncing") && this.IsBeneficiaryLinkedSurvey) {
                        // Goto progress showing page
                        this.SurveyBenLinkDlg = true;
                        this.BenefLinkedSurveySteps = 6;
                        await this.fnCalcProgressApprover();
                    }
                    // Already Data collected non - beneficiary linked
                    if (TaskDataitem.Status == "For Approval" && (TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Syncing") && !this.IsBeneficiaryLinkedSurvey) {
                        // Goto progress showing page
                        this.NonBenLinkSurveyDlg = true;
                        this.NonBenefLinkedSurveySteps = 4;
                        //await this.fnCalcProgressApprover();
                    }
                    // Beneficiary Linked
                    if (TaskDataitem.Status == "Approved" && (TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Syncing") && this.IsBeneficiaryLinkedSurvey) {
                        // Goto progress showing page
                        this.SurveyBenLinkDlg = true;
                        this.BenefLinkedSurveySteps = 6;
                        await this.fnCalcProgressApprover();
                    }
                    // Non Beneficiary Linked
                    if (TaskDataitem.Status == "Approved" && (TaskDataitem.CurrentOrOverdue == "Completed" || TaskDataitem.CurrentOrOverdue == "Syncing") && !this.IsBeneficiaryLinkedSurvey) {
                        // Goto progress showing page
                        this.NonBenLinkSurveyDlg = true;
                        this.NonBenefLinkedSurveySteps = 4
                        this.CustomMessage = "Task Data Approved"
                        //await this.fnCalcProgressApprover();
                    }
                    //Goto Data Collection Page
                    if (TaskDataitem.Status == "To Data Collect" && (TaskDataitem.CurrentOrOverdue == "Pending" || TaskDataitem.CurrentOrOverdue == "Current") && this.IsBeneficiaryLinkedSurvey) {

                        this.BenefLinkedSurveySteps = 1;
                        this.SurveyBenLinkDlg = true;
                    }
                    if (TaskDataitem.Status == "To Data Collect" && (TaskDataitem.CurrentOrOverdue == "Pending" || TaskDataitem.CurrentOrOverdue == "Current") && !this.IsBeneficiaryLinkedSurvey) {
                        this.NonBenefLinkedSurveySteps = 1;
                        this.NonBenLinkSurveyDlg = true;
                        await this.GetNonBeneficiayBasedSectionQuestions();
                    }
                    // Enable Save and Continue Button
                    if (TaskDataitem.Status == "To Data Collect" && TaskDataitem.CurrentOrOverdue == "Saved" && this.IsBeneficiaryLinkedSurvey) {

                        this.BenefLinkedSurveySteps = 1;
                        this.SaveAndContinue = true;
                        this.SearchExistingBeneficiary = false;
                        this.SurveyBenLinkDlg = true;
                    }
                    if (TaskDataitem.Status == "To Data Collect" && TaskDataitem.CurrentOrOverdue == "Saved" && !this.IsBeneficiaryLinkedSurvey) {
                        this.NonBenefLinkedSurveySteps = 1;
                        this.SaveAndContinue = true;
                        this.NonBenLinkSurveyDlg = true;
                    }
                }

            },


            async GetBeneficiary() {
                // Get Beneficiary list based on Type and Type ID
                this.lstSelectedBeneficiary = [];
                if (this.lstBeneficiary.length == 0) {
                    this.BeneficiariesWithName = this.lstSelectedTaskData.TaskData.BeneficiaryList;
                    var BeneficiaryQuestions = this.lstSelectedTaskData.TaskData.BeneficiaryRegistrationQstns;

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
                            //obj['CreatedDate'] = objUtils.common.formatDate(temp[row].CreatedDate);
                        }
                        if (this.BeneficiariesWithName != null && this.BeneficiariesWithName != undefined) {
                            if (this.BeneficiariesWithName.length > 0) {
                                if (uniqueValues[header] == this.BeneficiariesWithName[header].BeneficiaryID) {
                                    //obj["CreatedDate"] = objUtils.common.formatDate(this.BeneficiariesWithName[header].CreatedDate);
                                    obj["CreatedDate"] = this.BeneficiariesWithName[header].CreatedDate;
                                }
                            }
                        }
                        obj["BeneficiaryID"] = uniqueValues[header];
                        obj["TokenValue"] = temp[0].TokenValue;
                        lstBen.push(obj);
                    }

                    lstBen.sort(function (a, b) {
                        return new Date(b.CreatedDate) - new Date(a.CreatedDate);
                    });
                    for (i = 0; i < lstBen.length; i++)
                    {
                        lstBen[i].CreatedDate = objUtils.common.formatDate(lstBen[i].CreatedDate);
                    }

                    this.lstBeneficiary = lstBen;
                    this.InitialPreLoader = false;
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
                debugger;
                if (item.IsOtherOptionSelected == true && optionValue == "OTHERS") {
                    item.isOtherOptionEnabled = true;
                }
                else {
                    item.isOtherOptionEnabled = false;
                    item.OtherOptionAnswer = "";
                }
            },

            // Get Non Beneficiary Linked Survey Question details

            async GetNonBeneficiayBasedSectionQuestions() {
                this.PreLoader = true;
                this.lstSectionsQstns = await this.GetSectionsForEachBeneficiary(0, 0);
                this.PreLoader = false;
                this.InitialPreLoader = false;

            },

            async fnNonBenefProceedToUpdate() {
                if (await this.fnCheckMandatoryNonBenefLinked()) {
                    await this.fnStoreCurrentBenefQstnAns();
                    await this.fnNonBenefCalculateProgress();
                    this.NonBenefLinkedSurveySteps = 3;
                }
            },

            async fnNonBenefCalculateProgress() {
                var NonBenefTotalQuestionsWithoutLogic = 0;
                var NonBenefTotalQuestionsWithLogic = 0;
                var NonBenefAnsweredsQstnCnt = 0;
                var isMultichoiceAnswered = false;
                var isCaseCadingAnswered = false;
                for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                    if (this.lstSectionsQstns[i].IslogicSection === 0) {
                        NonBenefTotalQuestionsWithoutLogic += parseInt(this.lstSectionsQstns[i].Questions.length)
                        //NonBenefAnsweredsQstnCnt += parseInt(this.lstSectionsQstns[i].Questions.filter(x => x.Answer != "").length);
                        for (var j = 0; j < this.lstSectionsQstns[i].Questions.length; j++) {
                            if (this.lstSectionsQstns[i].Questions[j].QuestionType == 'Cascading Dropdown') {

                                if (this.lstSectionsQstns[i].Questions[j].CascadingQuestionOptions.length > 0) {

                                    var CascadingOption = this.lstSectionsQstns[i].Questions[j].CascadingQuestionOptions
                                    for (var k = 0; k <= CascadingOption.length-1; k++) {
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
                                if (this.lstSectionsQstns[i].Questions[j].Answer != "") {
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
            },

            async fnCheckMandatoryNonBenefLinked() {
                var ValidationReturnValue = false;
                var IsValid = true;
                var SectionQstnDetails = this.lstSectionsQstns;
                if (SectionQstnDetails.length > 0) {
                    for (var SecCnt = 0; SecCnt <= SectionQstnDetails.length - 1; SecCnt++) {
                        if (SectionQstnDetails[SecCnt].IslogicSection == 0) { // if Its not a Logic Section

                            ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(SectionQstnDetails[SecCnt].Questions, this.BenefStepper,false)
                            if (ValidationReturnValue == true) {
                                IsValid = false;
                            }
                        }
                        else if (SectionQstnDetails[SecCnt].IslogicSection == 1) {
                            // Logic Section Validations
                            ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(this.RepeatOrSkipSection, this.BenefStepper,true)
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

            async ClearSelectedBenef() {
                 this.lstSelectedBeneficiary = []
            },

            async ClearFilledData() {
                var Sections;
                if (this.IsBeneficiaryLinkedSurvey == true) {
                        Sections = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                        //for (var k = 0; k <= Sections.length - 1; k++) {
                        //    for (var l = 0; l <= Sections[k].Questions.length - 1; l++) {
                        //        Sections[k].Questions[l].Answer = ""
                        //    }
                        //}
                    this.RepeatOrSkipSection = this.RepeatOrSkipSection.filter(item => item.benefIndex !== Sections[0].BenefIndex)
                } else {
                    Sections = this.lstSectionsQstns
                    this.RepeatOrSkipSection=[]
                    }

                for (var i = 0; i <= Sections.length - 1; i++) {
                    for (var j = 0; j <= Sections[i].Questions.length - 1; j++) {
                        
                        if (Sections[i].Questions[j].QuestionType == 'Cascading Dropdown') {

                            if (Sections[i].Questions[j].CascadingQuestionOptions.length > 0) {

                                var CascadingOption = Sections[i].Questions[j].CascadingQuestionOptions
                                for (var k = 0; k <= CascadingOption.length - 1; k++) {
                                    Sections[i].Questions[j].CascadingQuestionOptions[k][0] = null;
                                    Sections[i].Questions[j].CascadingQuestionOptions[k][k] = null;
                                }
                            }
                        }
                        else if (Sections[i].Questions[j].QuestionType == 'Multi-Choice') {
                            for (var k = 0; k <= Sections[i].Questions[j].QuestionOptions.length - 1; k++) {
                                Sections[i].Questions[j].QuestionOptions[k].IsOptSelected = false;
                            }
                        }
                        else if (Sections[i].Questions[j].QuestionType == "Audio Capture") {
                            Sections[i].Questions[j].Answer = "";
                            Sections[i].Questions[j].AnsBlob = [];
                            var audioCatpure = document.getElementById("recAudioOne");

                            if (audioCatpure != null) {
                                audioCatpure.__vue__._data.recordList = [];
                                audioCatpure.__vue__._data.recorder.records = [];
                            }
                            this.savedAudio = "";
                        } else {
                            Sections[i].Questions[j].Answer = ""
                            Sections[i].Questions[j].AnsBlob = []
                            //Sections[i].Questions[j].IsUpload = 0
                            Sections[i].Questions[j].img = "";
                            Sections[i].Questions[j].startCamera = false;
                            Sections[i].Questions[j].signaturebutton = false;
                            Sections[i].Questions[j].uploadedFile = "";
                            Sections[i].Questions[j].uploadedImg = "";
                            Sections[i].Questions[j].uploadedSign = "";
                        }
                    }
                }
            },

            async GetBeneficiayBasedSectionQuestions() {
                this.Coordinates = {
                    lat: this.defaultCoordinates.lat,
                    lng: this.defaultCoordinates.lng,
                };
                if (this.lstSelectedBeneficiary.length == 0) {
                    this.Msg = "Please Select beneficiary";
                    this.infoSnackbarMsg = true;
                } else {
                    this.PreLoader = true;
                    await this.GetSelectedBeneficiaries();
                    await this.FillSurveyQuestionaries();
                    //this.BenefLinkedSurveySteps = 4;
                    this.SurveyShareSteps = 2;
                    await this.scrollToTop(this.lstSectionsQstns);
                    this.PreLoader = false;
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
            async GetSectionsForEachBeneficiary(BeneficiaryID, BenefIndex) {
                this.lstSections = this.lstSelectedTaskData.TaskData.SectionList;
                if (this.lstSections != null && this.lstSections != undefined && this.lstSections.length > 0)
                {
                    
                    this.lstSections.sort((a, b) => (a.SectionOrder > b.SectionOrder) ? 1 : ((b.SectionOrder > a.SectionOrder) ? -1 : 0));
                }
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
                        object.IsUpload = currSecQstns[i].IsUpload;
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
                } else {
                    document.getElementById("scrollToTop").scrollIntoView()
                }
            },
            scrollToNavbarTop() {
                $(document).scrollTop(0, 0);
                //document.getElementById("scrollToTop").scrollIntoView()
            },

            async fnProccedToUpdate() {
                if ((this.BenefStepper != this.vmSelectedBeneficiaries.length) && this.IsBeneficiaryLinkedSurvey == true) {

                    this.Msg = "Please fill all beneficiary details";
                    this.infoSnackbarMsg = true;
                    return false;
                }
                if (await this.fnCheckMandatory()) {

                    await this.fnCalculateProgress();
                    this.IsImportSurvey = false;
                    await this.fnStoreCurrentBenefQstnAns();
                    //this.BenefLinkedSurveySteps = 5;
                    this.SurveyShareSteps = 3;
                    ////this.NonBenefLinkedSurveySteps = 3;
                }

            },
            async fnResetQuestionsAns() {
                this.SurveyShareSteps = 1;
                this.BenefStepper = 1;
                await this.ClearFilledData()
                this.RepeatOrSkipSection=[]
            },

            async fnEditQuestionsSection() {
                this.SurveyShareSteps = 2;
            },

            async onFocusCheckQstnValidation(QItem) {
                var valConditions = QItem.QuestionCondition;
                if (valConditions != null) {
                    this.IsQstnValidation = true;
                    // ValidationCondition
                    for (var i = 0; i < valConditions.length; i++) {
                        if (valConditions[i].ValidationCondition == 'Phone Number') {
                            QItem.textLength = 10;
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
                if (this.IsBeneficiaryLinkedSurvey == true) {
                    Sections = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections.filter(y => y.IslogicSection == 0);
                } else {
                    Sections = Sections = this.lstSectionsQstns.filter(x => x.IslogicSection == 0);
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

                            //if (this.AnsBlob.length > 0) {
                            //    blobFile = this.AnsBlob.filter(x => (x.QuestionID == RepeatSection[Rcount].QuestionID) && (x.BenefID == benefID) && (x.RQstInd == RepeatSection[Rcount].RQstInd))[0];
                            //    this.FileUpload(blobFile.blobData, RepeatSection[Rcount].Answer, false, blobFile.IsUploadedFile, benefID);
                            //}
                            if (this.AnsBlob.length > 0) {
                                blobFile = this.AnsBlob.filter(x => (x.QuestionID == RepeatSection[Rcount].QuestionID) && (x.benefIndex == this.BenefStepper))[0];
                                if (blobFile != null && blobFile != undefined) {
                                    var fileName = blobFile.blobFileName;
                                    this.FileUpload(blobFile.blobData, fileName, false, blobFile.IsUploadedFile, this.BenefStepper, RepeatSection[Rcount].QuestionID);
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
                                    TotalQuestionsWithoutLogic += parseInt(Sections[i].Questions.length)
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
                                        } else if (Sections[i].Questions[j].QuestionType == 'Multi-Choice') {
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
                                            if (Sections[i].Questions[j].Answer != "") {
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
                    } else {

                        var NonBenefTotalQuestionsWithoutLogic = 0;
                        var NonBenefTotalQuestionsWithLogic = 0;
                        var NonBenefAnsweredsQstnCnt = 0;
                        var isMultichoiceAnswered = false;
                        var isCaseCadingAnswered = false;
                        for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                            if (this.lstSectionsQstns[i].IslogicSection === 0) {
                                NonBenefTotalQuestionsWithoutLogic += parseInt(this.lstSectionsQstns[i].Questions.length)
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
                                    } else if (this.lstSectionsQstns[i].Questions[j].QuestionType == 'Multi-Choice') {
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
                                        if (this.lstSectionsQstns[i].Questions[j].Answer != "") {
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
                } else {
                    NonBenefAnsweredsQstnCnt = this.lstBenefSurveyQstnAnswers.filter(x => x.Answer != "").length;
                    this.NonBenefTotalQuestion = this.lstBenefSurveyQstnAnswers.length;
                    this.QstnsPerBeneficiary = this.lstBenefSurveyQstnAnswers.length;
                    this.NonBenefCompletionPerc = Math.round(((NonBenefAnsweredsQstnCnt) / (this.NonBenefTotalQuestion)) * 100);
                    this.CompletionPerc = this.NonBenefCompletionPerc;
                    this.TotalQuestions = this.NonBenefTotalQuestion;
                    var questionCount = 0;
                    for (var i = 0; i <= this.lstSectionsQstns.length - 1; i++) {
                        questionCount += this.lstSectionsQstns[i].Questions.length;
                    }
                    this.Responses = Math.round(this.NonBenefTotalQuestion / questionCount);
                }
            },

            async fnCheckMandatory() {

                var ValidationReturnValue = false;
                var IsValid = true;
                var SectionQstnDetails = this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections;
                if (SectionQstnDetails.length > 0) {
                    for (var SecCnt = 0; SecCnt <= SectionQstnDetails.length - 1; SecCnt++) {
                        if (SectionQstnDetails[SecCnt].IslogicSection == 0) { // if Its not a Logic Section

                            ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(SectionQstnDetails[SecCnt].Questions, this.BenefStepper)
                            if (ValidationReturnValue == true) {
                                IsValid = false;
                            }
                        }
                        else if (SectionQstnDetails[SecCnt].IslogicSection == 1) {
                            // Logic Section Validations
                            ValidationReturnValue = await this.fnCheckMandatoryQuestionValidationAndCondition(this.RepeatOrSkipSection, this.BenefStepper)
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
            FormatDateTodayDate(Date) {
                return objUtils.common.formatDateTodayDate(Date)
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

                                var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.benefIndx == BenefIndex));
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
                        }
                    }
                }

                //REgular expression validation
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

            async UpdateTask() {
                this.PreLoader = true;
                var lstTaskResponse = [];
                var today = new Date().toLocaleString("en-US");
                this.submitteddatetime = today;
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
                                    //Obj.CreatedDate = Questions[QCnt].CreatedDate;
                                    //Obj.UpdatedDate = Questions[QCnt].UpdatedDate;

                                    if (Questions[QCnt].QuestionTypeID == 9 && (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer == '' || Questions[QCnt].Answer == '0,0')) {
                                        var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                                        if (this.Coordinates.lat == 0 && this.Coordinates.lng == 0) {
                                            gpsCoordinates = this.defaultCoordinates.lat.toString() + "," + this.defaultCoordinates.lng.toString();
                                        }
                                        Obj.Answer = gpsCoordinates;
                                    }

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

                                //if (Questions[QCnt].QuestionTypeID == 2 && this.multiChoiceAns != null && this.multiChoiceAns != undefined && this.multiChoiceAns.length > 0) {

                                //    var filterchoice = this.multiChoiceAns.filter(x => x.QuestionID == Obj.QuestionID);

                                //    var answerchoice = "";
                                //    if (filterchoice != null && filterchoice != undefined && filterchoice.length > 0) {
                                //        for (var chccount = 0; chccount < filterchoice.length; chccount++) {
                                //            if (answerchoice.length > 0) {
                                //                answerchoice = answerchoice + ',' + filterchoice[chccount].OptionValue;
                                //            } else {
                                //                answerchoice = filterchoice[chccount].OptionValue;
                                //            }
                                //        }
                                //    }
                                    
                                //    Obj.Answer = answerchoice;
                                //} else {
                                //    Obj.Answer = Questions[QCnt].Answer;
                                //}
                                Obj.Answer = Questions[QCnt].Answer;

                                if (Questions[QCnt].QuestionTypeID == 9 && (Questions[QCnt].Answer == undefined || Questions[QCnt].Answer ==''))
                                { 
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

                    var clientguid = uuidv4(); // generate 36 character client guid

                    await dbImworks.TaskSyncDB.put({
                        Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn,
                        Status: 'DataCollected', WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: this.lstSelectedTaskData.WorkflowID, WFLevel: 0, ClientData: JSON.stringify(lstTaskResponse),
                        lastSyncDBID: this.lstSelectedTaskData.SyncDBID, IsBeneficiaryLinked: this.IsBeneficiaryLinkedSurvey, TaskDate: this.SelectedTaskDate, Periodicity: this.lstSelectedTaskData.Periodicity, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, TaskSubmittedDate: today, ClientGuid: clientguid

                    });

                    var url = window.SERVER_URL + '/MobileWebService/SyncTaskToServer';
                    var data = new FormData();

                    data.append('lstMyTaskSyncToServerData[0].Type', this.SelectedTaskType);
                    data.append('lstMyTaskSyncToServerData[0].WFLevel', 0);
                    data.append('lstMyTaskSyncToServerData[0].IsBeneficiaryLinked', this.IsBeneficiaryLinkedSurvey);
                    data.append('lstMyTaskSyncToServerData[0].ClientData', JSON.stringify(lstTaskResponse));
                    data.append('lstMyTaskSyncToServerData[0].Periodicity', this.lstSelectedTaskData.TaskData.Periodicity);
                    data.append('lstMyTaskSyncToServerData[0].ClientGuid', clientguid);

                    let response = await fetch(url, {
                        method: 'POST',
                        credentials: "include",
                        body: data
                    });
                    let result = await response.json();
                    if (result.status == "success") {


                        if (result.Data != null && result.Data != undefined && result.Data.length > 0) {
                            for (var z = 0; z < result.Data.length; z++) {
                                if (result.Data[z].Status == "success") {
                                    var TaskSyncID = result.Data[z].TaskSyncID;
                                    var tskData = await dbImworks.TaskSyncDB.where("ClientGuid").equals(result.Data[z].ClientGuid).toArray();
                                    await dbImworks.TaskSyncDB.where("ClientGuid").equals(result.Data[z].ClientGuid).delete();

                                    if (tskData != null && tskData.length > 0) {
                                        await dbImworks.TbMyTask.where("TaskSubmittedDate").equals(tskData[0].TaskSubmittedDate).modify({ ToDelete: 2 });

                                        this.UploadTaskDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                        this.UploadTaskResponseDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                        this.UploadTaskBenefRegDocument(TaskSyncID, tskData[0].lastSyncDBID);
                                    }
                                }
                            }
                        }


                        this.Msg = "Survey responses are saved successfully.";
                        this.infoSnackbarMsg = true;
                        if (this.IsBeneficiaryLinkedSurvey == true) {
                            this.SurveyShareSteps = 4
                        } else {
                            this.NonBenefLinkedSurveySteps = 4
                        }
                    }

                    this.PreLoader = false;
                    return false;

                    //below portions are not needed for sharelink

                    await dbImworks.TaskSyncDB.put({
                        Type: this.SelectedTaskType, Command: 'TaskResponse', TypeID: this.SelectedTaskTypeID, TypeIDColumn: this.lstSelectedTaskData.TypeIDColumn,
                        Status: 'DataCollected', WorkflowID: this.lstSelectedTaskData.WorkflowID, TotalWFLevel: this.lstSelectedTaskData.WorkflowID, WFLevel: 0, ClientData: JSON.stringify(lstTaskResponse),
                        lastSyncDBID: this.lstSelectedTaskData.SyncDBID, IsBeneficiaryLinked: this.IsBeneficiaryLinkedSurvey, TaskDate: this.SelectedTaskDate, Periodicity: this.lstSelectedTaskData.Periodicity, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, TaskSubmittedDate: today

                    });
                    

                    var data = await dbImworks.User.where("IsLoggedIn").equals(1).toArray();

                    // Inserting into TbMyTask
                    var lastClientSyncID = await dbImworks.TaskSyncDB.orderBy('pkId').last();
                    await dbImworks.TbMyTask.put({
                        ClientSyncTaskID: lastClientSyncID, Type: this.SelectedTaskType, TypeID: this.SelectedTaskTypeID, WFLevel: 0, WFLevelStatus: "Completed",
                        TaskUpdateComment: this.vmUpdateComment, AttachedDocumentName: '', SyncDBID: this.lstSelectedTaskData.SyncDBID,
                        ProjectName: '', TaskName: '', Description: '',
                        CreatedDate: today, TaskDate: this.SelectedTaskDate, TaskSubmittedDate: today, WorkspaceID: this.lstSelectedTaskData.WorkspaceID, Periodicity: '', TaskSubmittedBy: data[0].UserName, BeneficiarySubmittedList: JSON.stringify(this.vmSelectedBeneficiaries)
                    });

                }


                this.SnackbarMsg = "Data Collection Completed ...";


                if (this.AnyTimeSubmissionId == 0) {

                    // Updating Current Tasks Status
                    var SelectedTaskDetails = this.TaskCardData.filter(x => x.TypeID == this.SelectedTaskTypeID && x.TaskType == this.SelectedTaskType)[0].TaskLog.filter(y => y.TaskIndex == this.SelectedTaskIndex)[0]

                    SelectedTaskDetails.CurrentOrOverdue = "Syncing"
                    SelectedTaskDetails.CreatedDate = today;
                    SelectedTaskDetails.Status = "For Approval";
                    SelectedTaskDetails.Message = "Survey Data Updated";
                    SelectedTaskDetails.Comment = this.vmUpdateComment;
                    SelectedTaskDetails.Name = data[0].UserName;

                }
                else if (this.AnyTimeSubmissionId > 0) {
                    // Update Status to any time dexie table

                    var AnyTimeData = this.lstAnyTimeNewSubmission.filter(x => x.pkId == this.AnyTimeSubmissionId)[0];
                    AnyTimeData.TaskData.CurrentOrOverdue = "Syncing";
                    AnyTimeData.TaskData.CreatedDate = today;
                    AnyTimeData.TaskData.Status = "For Approval";
                    AnyTimeData.TaskData.Message = "Survey Data Updated";
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
                }


                // Deleting the Task fro SavedDb if Exists
                // Delete Saved Data if Exists

                var pkId = [];
                pkId = await dbImworks.SavedTaskDB.where({ SyncDBID: parseInt(this.lstSelectedTaskData.SyncDBID), TaskDate: this.SelectedTaskDate, TypeID: this.SelectedTaskTypeID, Type: this.SelectedTaskType }).toArray();

                if (pkId.length > 0) {
                    await dbImworks.SavedTaskDB.where("pkId").equals(parseInt(pkId[0].pkId)).delete();
                }

                //this.SelectedTaskIndex;
                this.SurveyBenLinkDlg = false;
                this.SuccessSnackbar = true;
                await this.fnClearData();

            },

            async UploadTaskDocument(TaskSyncID, lastSyncDBID) {

                try {

                    var doc = await dbImworks.TbTaskDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                    if (doc != null && doc.length > 0) {
                        for (var i = 0; i < doc.length; i++) {

                            var url = window.SERVER_URL + '/Document/UplaodFiles';
                            var data = new FormData();


                            data.append('myFile', doc[i].AttachedDocument);
                            data.append('DocumentOriginalName', doc[i].DocumentOriginalName);
                            data.append('DocumentSize', doc[i].DocumentSize);
                            data.append('DocumentType', doc[i].DocumentType);
                            data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                            data.append('DocumentLastModifiedDate', doc[i].DocumentLastModifiedDate);
                            data.append('ProjectId', this.projid); //doc[i].ProjectID
                            data.append('UploadedFrom', doc[i].Type);
                            data.append('LinkedTo', doc[i].DocLinkedTo);
                            data.append('LinkedToID', doc[i].TypeID);
                            data.append('TaskSyncID', TaskSyncID);


                            let response = await fetch(url, {
                                method: 'POST',
                                credentials: "include",
                                body: data
                            });
                            let result = await response.json();

                            if (result.status == "success") {

                                var a = [];
                                a = await dbImworks.TbTaskDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate }).toArray();
                                await dbImworks.TbTaskDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                            }


                        }
                    }
                }
                catch (error) {
                    return false;
                    throw error;
                }
                return true;
            },

            async UploadTaskResponseDocument(TaskSyncID, lastSyncDBID) {
                try {
                    var doc = await dbImworks.TbTaskResponseDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                    if (doc != null && doc.length > 0) {
                        for (var i = 0; i < doc.length; i++) {

                            var url = window.SERVER_URL + '/Document/UploadSureyQuestionnaireFiles';
                            var data = new FormData();

                            data.append('myFile', doc[i].AttachedDocument);
                            data.append('ProjectID', this.projid); //doc[i].ProjectID
                            data.append('FileName', doc[i].FileName);
                            data.append('DocumentName', doc[i].FileName);
                            data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                            data.append('ResponseFrom', "SurveyResponse");
                            data.append('UploadedFrom', doc[i].Type);
                            data.append('LinkedTo', doc[i].DocLinkedTo);
                            data.append('LinkedToID', doc[i].TypeID);
                            data.append('TaskSyncID', TaskSyncID);

                            let response = await fetch(url, {
                                method: 'POST',
                                credentials: "include",
                                body: data
                            });
                            let result = await response.json();

                            if (result.status == "success") {

                                var a = [];
                                a = await dbImworks.TbTaskResponseDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate, FileName: doc[i].FileName }).toArray();
                                await dbImworks.TbTaskResponseDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                            }


                        }
                    }
                }
                catch (error) {
                    return false;
                    throw error;
                }
                return true;
            },
            async UploadTaskBenefRegDocument(TaskSyncID, lastSyncDBID) {

                try {

                    var doc = await dbImworks.TbTaskBenefRegDocument.where("SyncTaskID").equals(lastSyncDBID).toArray();

                    if (doc != null && doc.length > 0) {
                        for (var i = 0; i < doc.length; i++) {

                            var url = window.SERVER_URL + '/Document/UplaodQuestionnaireAnsFiles';
                            var data = new FormData();

                            data.append('myFile', doc[i].AttachedDocument);
                            data.append('ProjectID', this.projid); //doc[i].ProjectID
                            data.append('FileName', doc[i].FileName);
                            data.append('DocumentName', doc[i].FileName);
                            data.append('DocumentTypeIcon', doc[i].DocumentTypeIcon);
                            data.append('ResponseFrom', "BeneficiaryResponse");
                            data.append('UploadedFrom', doc[i].Type);
                            data.append('LinkedTo', doc[i].DocLinkedTo);
                            data.append('LinkedToID', doc[i].TypeID);
                            data.append('TaskSyncID', TaskSyncID);


                            let response = await fetch(url, {
                                method: 'POST',
                                credentials: "include",
                                body: data
                            });
                            let result = await response.json();

                            if (result.status == "success") {

                                var a = [];
                                a = await dbImworks.TbTaskBenefRegDocument.where({ SyncTaskID: parseInt(doc[i].SyncTaskID), TaskDate: doc[i].TaskDate, FileName: doc[i].FileName }).toArray();
                                await dbImworks.TbTaskBenefRegDocument.where("pkId").equals(parseInt(a[0].pkId)).delete();

                            }


                        }
                    }
                }
                catch (error) {
                    return false;
                    throw error;
                }
                return true;
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

            },
            async nextBeneficiary() {
                if (this.BenefStepper < this.vmSelectedBeneficiaries.length) {
                    if (await this.fnCheckMandatory()) {

                        await this.fnCalculateProgress();
                        await this.fnStoreCurrentBenefQstnAns();

                        this.BenefStepper++;
                        await this.GetCurrentBenefSectionsQstns();


                        await this.fnPreSavedBenefQstnAns();
                    }
                }
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
                            lstQuestion[QstCnt].Answer = "";
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
                            if (typeof ansBlobData[0] != "undefined") {
                                blbFile = ansBlobData[0].AttachedDocument;
                            }
                            // To Read Photo
                            if (lstQuestion[QstCnt].QuestionType == "Photo Capture" && blbFile != "") {

                                //var UploadIndex = benefSurveyQstn.filter(x => x.QuestionID == currSecQstns[i].QuestionID)[0].IsUpload;
                                lstQuestion[QstCnt].IsUpload = 1;
                                this.audUpload[QstCnt] = 1;
                                if (ansBlobData[0].IsUploadedFile) {
                                    const fr = new FileReader()
                                    fr.readAsDataURL(blbFile)
                                    await fr.addEventListener('loadend', () => {

                                        //this.BeneficiarySectionQstnList[BenCnt].Sections[SecCnt].Questions[QCnt - 1].uploadedImg = fr.result;
                                        this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections[SecCnt].Questions[QstCnt - 1].uploadedImg = fr.result;
                                        //lstQuestion[QstCnt].uploadedImg = fr.result;
                                    });
                                }
                                else {
                                    const fr = new FileReader()
                                    fr.readAsDataURL(blbFile)
                                    await fr.addEventListener('loadend', () => {

                                        //lstQuestion[QstCnt].img = fr.result;
                                        this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == this.BenefStepper)[0].Sections[SecCnt].Questions[QstCnt - 1].img = fr.result;
                                    });
                                }
                            }
                            if (lstQuestion[QstCnt].QuestionType == "File Upload" && blbFile != "") {

                                QuestionID = ansBlobData[0].QuestionID;
                                const fr = new FileReader()
                                fr.readAsDataURL(blbFile)
                                fr.addEventListener('loadend', () => {

                                    var SecIndex = SecCnt;
                                    if (SecCnt > Sections.length - 1) {

                                        SecIndex = Sections.length - 1
                                    }

                                    this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecIndex].Questions.filter(y => y.QuestionID == QuestionID)[0].uploadedFile = fr.result;

                                    //[QstCnt - 1].uploadedFile = fr.result;
                                    //this.BeneficiarySectionQstnList.filter(x => x.BenefIndex == (this.BenefStepper))[0].Sections[SecCnt - 1].Questions[QstCnt - 1].uploadedFile = fr.result;
                                    //var blb = ansBlobData;


                                });
                            }
                            //lstQuestion[QstCnt].Answer = Answer;
                        }

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

            //------ ======= Beneficiary Registrations ====================------ //
            //// Check mandatory and Validation, Condition before save for Beneficiary Registration
            async fnCheckBeneficiaryMandatoryAndValidation() {

                var IsValid = false;
                if (this.lstBenefRegQstns.length > 0) {
                    // Checking for mandatory questions
                    var lstMandatoryQuestion = this.lstBenefRegQstns.filter(x => x.IsMandatoryResponse == 1);
                    //  Checking for Validation Question Min and Max
                    var lstValidationQuestion = this.lstBenefRegQstns.filter(x => x.IsValidationQuestion == 1 && (x.BTQstnValidationList != null));
                    var lstRegEx = this.lstBenefRegQstns.filter(x => (x.RegexExpression != null && x.RegexExpression.length > 0));

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
                                var CascadingAns = this.cascadingAnsBT.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false) && (x.BenefID == benefID));
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
                this.SurveyShareSteps = 5;
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
                    this.SurveyShareSteps = 1;;
                }
                else {

                    this.lstBenefRegQstns = [];
                    this.SurveyShareSteps = 1;;
                }
            },
            async RegisterNewBenficiary(type) {

                if (type == "tab-4") // Register through manual
                {

                    await this.RegisterNewBenficiaryManual()
                }
                else {

                    await this.RegisterNewBeneficiaryByImport();
                }

            },
            // Beneficiary Manual Registration
            async RegisterNewBenficiaryManual() {
                if (await this.fnCheckBeneficiaryMandatoryAndValidation() == false) {


                    if (this.lstBenefRegQstns[0].Answer == "") {

                        this.Msg = "Please enter Beneficiary Name";
                        this.infoSnackbarMsg = true;
                        return false;


                    }
                    this.newBenefID--;
                    var lstBenefRegQstnAnswers = [];
                    var Ans = "";
                    var blobFile = [];

                    for (var i = 0; i < this.lstBenefRegQstns.length; i++) {
                        Ans = "";
                        if (this.lstBenefRegQstns[i].QuestionType == 'Multi-Choice') {

                            for (var j = 0; j < this.multiChoiceAnsBT.length; j++) {
                                Ans = (Ans != '') ? Ans + ',' + this.multiChoiceAnsBT[j] : this.multiChoiceAnsBT[j];
                            }
                        }
                        else if (this.lstBenefRegQstns[i].QuestionType == "Cascading Dropdown") {

                            for (var j = 0; j < this.cascadingAnsBT.length; j++) {
                                Ans = (Ans != '') ? Ans + ',' + this.cascadingAnsBT[j] : this.cascadingAnsBT[j];
                            }
                        }
                        else if ((this.lstBenefRegQstns[i].QuestionType == "Audio Capture") || (this.lstBenefRegQstns[i].QuestionType == "Signature Capture")
                            || (this.lstBenefRegQstns[i].QuestionType == "Photo Capture") || (this.lstBenefRegQstns[i].QuestionType == "File Upload")) {

                            if (this.BTAnsBlob.length != 0) {
                                var bFile = this.BTAnsBlob.filter(x => x.QuestionID == this.lstBenefRegQstns[i].QuestionID)
                                if (bFile.length > 0) {
                                    blobFile = this.BTAnsBlob.filter(x => x.QuestionID == this.lstBenefRegQstns[i].QuestionID)[0].blobData;
                                    if (typeof blobFile != "undefined") {
                                        await this.FileUpload(blobFile, this.lstBenefRegQstns[i].Answer, true, this.lstBenefRegQstns[i].QuestionID);
                                    }
                                }

                            }
                            Ans = this.lstBenefRegQstns[i].Answer;
                        }
                        else {
                            Ans = this.lstBenefRegQstns[i].Answer;
                        }

                        lstBenefRegQstnAnswers.push({ QuestionID: this.lstBenefRegQstns[i].QuestionID, AnswerTypeID: this.lstBenefRegQstns[i].QuestionTypeID, Answer: Ans, CreatedDate: new Date() });
                    }

                    this.lstAllNewBeneficiariesQstnAns.push(lstBenefRegQstnAnswers); // for download summary


                    var tempData = await dbImworks.TbNewBeneficiary.toArray();
                    var BenefIndexID = -(tempData.length + 1);

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
                    this.SurveyShareSteps = 1;

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
                                Answer: this.lstImportBeneficiary[i][this.lstImportBeneficiaryHearder[j].value]
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
                                obj[this.lstBenefRegQstns[Cnt].QuestionName] = temp[0].Answer
                            }
                        }
                        obj["BeneficiaryID"] = BenefIndexID;
                        obj["TokenValue"] = TokenValue;

                        lstBen.push(obj);
                        // Adding new beneficiary to existing beneficiary list
                        this.lstBeneficiary.push(obj);


                        this.lstSelectedBeneficiary.push(lstBen[0]);


                    }

                    this.File = [];
                    this.lstImportBeneficiary = [];
                    for (var i = 0; i < this.lstBenefRegQstns.length; i++)
                        this.lstBenefRegQstns[i].Answer = "";
                    this.SurveyShareSteps = 1;
                    this.PreLoader = false;
                }
                else {
                    this.PreLoader = false;
                    this.Msg = "Please resolve all Invalid Input(s).";
                    this.infoSnackbarMsg = true;
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
                this.lstBenefRegQstns[index].Answer = e.target.value;
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
                                                    if (this.formatExcelDate(dateField) < this.formatExcelDate(today)) {
                                                        obj[this.lstImportSurveyQstnHearderN[j].value] = "Invalid Input";
                                                        this.IsvalidexcelData = false;
                                                    }
                                                }
                                                if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "") {
                                                    if (this.formatExcelDate(dateField) > this.formatExcelDate(today)) {
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
                            this.Msg = "Cannot read file !";
                            this.infoSnackbarMsg = true;
                        }

                    };
                }
                else {
                    this.Msg = 'FileReader is not supported in this browser.';
                    this.infoSnackbarMsg = true;
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
                        this.Msg = "Beneficiary Registration Questionnaire is not matching for the selected file...";
                        this.infoSnackbarMsg = true;
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
                                                    if (this.formatExcelDate(dateField) < this.formatExcelDate(today)) {
                                                        obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                                        this.IsvalidexcelData = false;
                                                    }
                                                }
                                                if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "") {
                                                    if (this.formatExcelDate(dateField) > this.formatExcelDate(today)) {
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
                    this.infoSnackbarMsg = true;
                    //this.regfileInput = null;
                    //this.csvFileName = "";
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
            //async GetBenefRegistrationQstns() {
            //    this.SurveyShareSteps = 5;
            //},
            //async fnCancelBenefRegistration(type) {

            //    if (type == "tab-3") // Cancel Import Registration
            //    {

            //        this.SurveyShareSteps = 1;
            //    }
            //    else {

            //        this.lstBenefRegQstns = [];
            //        this.SurveyShareSteps = 1;
            //    }
            //},
        }
    }
</script>

<style scoped>
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
</style>

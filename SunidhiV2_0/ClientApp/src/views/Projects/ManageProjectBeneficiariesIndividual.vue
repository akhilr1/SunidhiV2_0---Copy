<template>
    <div>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid>
            <v-row>
                <v-col>
                    <h5 class="page-head">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                        </v-avatar>
                        <!--if path is ManageProjectBeneficiariesIndividual-->
                        <span v-show="$route.name=='ManageProjectBeneficiariesIndividual'">
                            <router-link :to="{ name: 'ManageProjectBeneficiaries', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link> >
                            <router-link :to="{ name: 'ManageProjectBeneficiariesTypeIndividual', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID} }" class="page-head text-link">{{BenefTypeName}} > </router-link>
                        </span>
                        <!--_________________-->
                        <!--if path is MyprojectsDraftIndividualBeneficiariesIndividual-->
                        <span v-show="$route.name=='MyprojectsDraftIndividualBeneficiariesIndividual'">
                            <router-link :to="{ name: 'MyprojectsDraftIndividualBeneficiaries', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link> >
                            <router-link :to="{ name: 'MyprojectsDraftIndividualBeneficiariesTypeIndividual', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID} }" class="page-head text-link">{{BenefTypeName}} > </router-link>
                        </span>
                        <!--_________________-->
                        <!--if path is MyprojectsDeployedIndividualBeneficiariesIndividual-->
                        <span v-show="$route.name=='MyprojectsDeployedIndividualBeneficiariesIndividual'">
                            <router-link :to="{ name: 'MyprojectsDeployedIndividualBeneficiaries', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link> >
                            <router-link :to="{ name: 'MyprojectsDeployedIndividualBeneficiariesTypeIndividual', query: {WsID: WorkspaceID , Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID} }" class="page-head text-link">{{BenefTypeName}} > </router-link>
                        </span>
                        <!--_________________-->
                        {{BenefName}}
                    </h5>
                </v-col>
            </v-row>

            <v-row>
                <v-col>
                    <v-tabs centered color="tab-active-line">
                        <v-tab>Basic Details</v-tab>
                        <v-tab>Data Collected</v-tab>

                        <!--Basic Details Tab-->
                        <v-tab-item>
                            <v-row justify="center">
                                <v-col md="3">
                                    <!--Edit button-->
                                    <Secondary-Button id="AutoTest_IP285" v-show="!BenDetailsEdit" title="Edit Details" class="float-right" @click.native="BenDetailsEdit=true" v-bind:disabled="!screenAccess"></Secondary-Button>
                                </v-col>
                            </v-row>
                            <template v-for="(item,index) in lstBeneficiaryDetails">
                                <v-row justify="center">
                                    <v-col md="3" class="py-4">
                                        <!--Details edit panel-->
                                        <div v-if="BenDetailsEdit">
                                            <p class="black--text">{{index +1}}.{{item.QuestionName}}</p>

                                            <sub v-if="item.IsMandatoryResponse==true" style="color:red;font-size:large"> *</sub>
                                            <sub style="color:red;font-size:smaller" class="mr-2 ml-2" v-if="item.QuestionHint.length > 0 && item.QuestionHint != null && item.QuestionHint != 'null'">{{item.QuestionHint}}</sub>
                                            <sub style="color:red;font-size:smaller" v-if="item.IsValidationError">{{item.ValidationErrorTest}}</sub>
                                            <sub style="color:red;font-size:smaller" v-if="item.IsUniqueValidation">{{item.UniqueErrorTest}}</sub>

                                            <!--<v-text-field v-if="item.AnswerType=='Single Line Text'" hide-details outlined dense label="Type your answer" v-model="item.Answer" />-->
                                            <!-- Single line-->
                                            <div v-if="item.AnswerType=='Single Line Text' && (item.IsValidationQuestion == 1) ">
                                                <v-text-field v-if="item.AnswerType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                              :type="item.qType" v-bind:maxlength="item.txtLength" :rules="item.qRules" :min="item.minVal" :max="item.maxVal"
                                                              @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)" />
                                            </div>
                                            <div v-if="item.AnswerType=='Single Line Text' && (item.IsValidationQuestion == 0) ">
                                                <v-text-field v-if="item.AnswerType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                              @focus="onFocusCheckQstnValidation(item)" />

                                            </div>

                                            <!--Multiline Answer-->
                                            <!--<v-textarea v-if="item.AnswerType==='Multi Line Text'" hide-details outlined dense label="Type your answer" v-model="item.Answer" />-->
                                            <div v-if="item.AnswerType=='Multi Line Text' && (item.IsValidationQuestion == 1) ">

                                                <v-textarea v-if="item.AnswerType=='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                            :type="item.qType" v-bind:maxlength="item.txtLength" :rules="item.qRules" :min="item.minVal" :max="item.maxVal"
                                                            @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)" />
                                            </div>
                                            <div v-if="item.AnswerType=='Multi Line Text' && (item.IsValidationQuestion == 0) ">

                                                <v-textarea v-if="item.AnswerType=='Multi Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                            @focus="onFocusCheckQstnValidation(item)" />

                                            </div>

                                            <!--Single Choice Answer-->

                                            <div v-if="item.AnswerType==='Single Choice'" class="mt-0">
                                                <v-radio-group v-model="item.Answer">
                                                    <v-radio v-for="(optItem,ind) in item.BenefAnswerOptions" :key="ind"
                                                             :label="optItem.OptionValue" hide-details
                                                             :value="optItem.OptionValue"></v-radio>
                                                </v-radio-group>

                                            </div>
                                            <!--Likert Scale-->
                                            <div v-if="item.AnswerType==='Likert Scale'" class="mt-0">
                                                <v-radio-group v-model="item.Answer">
                                                    <v-radio hide-details v-for="(optItem,ind) in item.BenefAnswerOptions" :key="ind"
                                                             :label="optItem.OptionValue"
                                                             :value="optItem.OptionValue"></v-radio>
                                                </v-radio-group>
                                            </div>
                                            <!--Multiple Choice -->
                                            <div v-if="item.AnswerType==='Multi-Choice'" class="mt-0">
                                                <div v-if="item.BenefAnswerOptions.length>0">
                                                    <v-checkbox hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BenefAnswerOptions" :key="ind"
                                                                v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,ind)"></v-checkbox>

                                                </div>
                                            </div>
                                            <!--Dropdown -->
                                            <div v-if="item.AnswerType==='Dropdown'" class="mt-0">
                                                <div v-if="item.BenefAnswerOptions.length>0">
                                                    <v-row>
                                                        <v-col md="auto">
                                                            <v-select hide-details dense :items="item.BenefAnswerOptions" :menu-props="{ maxHeight: '300' }" filled
                                                                      item-text="OptionValue"
                                                                      label="Select" outlined="" v-model="item.Answer">
                                                            </v-select>
                                                        </v-col>
                                                    </v-row>
                                                </div>
                                            </div>
                                            <!--Numeric-->
                                            <div v-if="item.AnswerType==='Numeric'">
                                                <v-text-field v-if="item.AnswerType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="item.Answer" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                              onKeyPress="if(this.value.length==10) return false;" :min="item.minVal" :max="item.maxVal" :rules="item.qRules"
                                                              @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)">
                                                </v-text-field>
                                            </div>
                                            <!--Cascading Dropdown -->
                                            <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                                <div v-if="item.BenefAnswerOptions.length>0">
                                                    <v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                              :label="` ${cascadingAns.filter(x=>x.QuestionID == item.QuestionID)[0].SelectedOption[ind]}`" outlined=""
                                                              v-for="(optItem,ind) in item.BTQstnAnsCascadingOptionList" :key="ind"
                                                              :items="optItem.BTQstnAnsCascadingListItems"
                                                              item-text="Name"
                                                              item-value="ID" @change="EditCascadingFilterSubItem(item,optItem,optItem[ind],ind)">
                                                    </v-select>
                                                </div>
                                            </div>

                                            <!--GPS Capture-->
                                            <div v-if="item.AnswerType==='GPS Capture'" v-model="item.Answer">
                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                <GmapMap :center="Coordinates"
                                                         :zoom="13"
                                                         style="width: 100%; height: 300px" @click.native="GetGPSCoordinates(item.QuestionID)">
                                                    <GmapMarker :position="Coordinates"
                                                                :clickable="true"
                                                                :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                                </GmapMap>
                                            </div>
                                            <!--Photo Capture-->
                                            <div v-if="item.AnswerType==='Photo Capture'">
                                                <div v-if="item.IsNew==0">
                                                    <v-row>
                                                        <v-col md="8">
                                                            <v-btn id="AutoTest_IP286" text="" height="25" min-width="25" max-width="25" color="#707070" @click="item.IsNew=1,devices=[],deviceId=null,camera=null">
                                                                <v-icon size="20">mdi-pencil</v-icon>
                                                            </v-btn>
                                                            <v-img :src=fileUrl+item.Answer width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                        </v-col>
                                                    </v-row>

                                                </div>

                                                <div v-if="item.IsNew==1">
                                                    <v-radio-group row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                        <v-radio label="Photo Capture" hide-details>  </v-radio>
                                                        <v-radio label="Photo Upload" hide-details></v-radio>
                                                    </v-radio-group>
                                                    <div v-show="item.IsUpload===0">
                                                        <v-row>
                                                            <v-col md="12">
                                                                <v-btn @click="startCamera =true,camOn=true,onEStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                                <v-btn class="mr-4" id="AutoTest_IP389" color="secondary" depressed @click="onCapture(item.QuestionID,2),startCamera =false,camOn=false" v-bind:disabled="!camOn">Capture</v-btn>
                                                                <v-btn @click="onEStop(),startCamera =false,camOn=false" v-bind:disabled="!camOn" depressed>Stop</v-btn>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col md="8">
                                                                <vue-web-cam ref="webcam" v-if="startCamera"
                                                                             :device-id="deviceId"
                                                                             width="100%"
                                                                             height="200px"
                                                                             @started="onStarted"
                                                                             @stopped="onStopped"
                                                                             @error="onError"
                                                                             @cameras="onCameras"
                                                                             @camera-change="onCameraChange" :autoplay="camOn" />

                                                                <!--<v-btn id="AutoTest_IP287" color="secondary" depressed @click="onCapture(item.QuestionID,2)">Capture</v-btn>-->

                                                            </v-col>
                                                            <v-col md="4" v-if="img !=null">
                                                                <figure class="figure">
                                                                    <img :src="img" class="center-block my-2" width="250" height="180" aspect-ratio="1.7" />
                                                                </figure>
                                                            </v-col>

                                                        </v-row>
                                                    </div>
                                                    <div v-show="item.IsUpload===1">
                                                        <v-row>
                                                            <v-col md="8">
                                                                <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                              @change="AnsBlobQID=item.QuestionID,SaveFile('Photo',item.AnsBlob,2)"></v-file-input>
                                                            </v-col>
                                                            <v-col md="4">
                                                                <v-img :src=uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                            </v-col>
                                                        </v-row>

                                                    </div>
                                                </div>


                                            </div>
                                            <!--Audio Capture-->
                                            <div v-if="item.AnswerType==='Audio Capture'">
                                                <v-radio-group row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                    <v-radio label="Audio Capture" hide-details>
                                                    </v-radio>
                                                    <v-radio label="Audio Upload" hide-details>
                                                    </v-radio>
                                                </v-radio-group>
                                                <div v-show="item.IsUpload===0">
                                                    <audio-recorder upload-url="YOUR_API_URL"
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
                                                                    @click.native="AnsBlobQID=item.QuestionID" />

                                                    <!--<h3 class="text-center mt-4">OR</h3></br>-->

                                                </div>
                                                <div v-show="item.IsUpload===1">
                                                    <!--<v-file-input accept="*" outlined dense label="Upload Audio" id="file" v-model="fileInput" @change="onFileUpload"></v-file-input>-->
                                                    <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile('Audio',item.AnsBlob)"></v-file-input>
                                                </div>
                                            </div>
                                            <!--Signature Capture-->
                                            <div v-if="item.AnswerType==='Signature Capture'" v-model="item.Answer">
                                                <div>
                                                    <v-card>
                                                        <v-img :src=fileUrl+item.Answer width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                    </v-card>
                                                </div>
                                                <v-radio-group row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                                    <v-radio label="Signature Capture" hide-details>

                                                    </v-radio>
                                                    <v-radio label="Signature Upload" hide-details>
                                                    </v-radio>
                                                </v-radio-group>
                                                <div v-show="item.IsUpload===0">
                                                    <v-card outlined>
                                                        <VueSignaturePad width="100%" height="300px" ref="signaturePad" />
                                                    </v-card>
                                                    <div>
                                                        <v-btn id="AutoTest_IP288" color="secondary" depressed class="mt-2 mr-2" @click="saveSign(item.QuestionID)">Save</v-btn>
                                                        <v-btn id="AutoTest_IP289" default depressed class="mt-2 mr-2" @click="clearSign">Clear</v-btn>
                                                    </div>
                                                    <!--<h4 class="text-center">OR</h4>-->
                                                </div>
                                                <div v-show="item.IsUpload===1">
                                                    <!--<v-file-input accept="image/*" outlined dense label="Upload Signature"></v-file-input>-->
                                                    <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile('Signature',item.AnsBlob)"></v-file-input>

                                                </div>
                                            </div>
                                            <!--Date Picker-->
                                            <div v-if="item.AnswerType==='Date Picker'">

                                                <v-menu v-model="item.mDate"
                                                        :close-on-content-click="false"
                                                        :nudge-right="40"
                                                        transition="scale-transition"
                                                        offset-y
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model="item.Answer"
                                                                      label="Select Date"
                                                                      prepend-inner-icon="mdi-calendar"
                                                                      outlined
                                                                      readonly
                                                                      dense
                                                                      v-on="on" hide-details></v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="item.Answer" no-title @input="item.mDate = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(item)"></v-date-picker>
                                                </v-menu>
                                            </div>
                                            <!--File Upload-->
                                            <div v-if="item.AnswerType==='File Upload'" class="mt-0">

                                                <v-file-input label="Upload File" outlined dense accept="*" v-model="item.AnsBlob"
                                                              @change="AnsBlobQID=item.QuestionID,SaveFile('File',item.AnsBlob)"></v-file-input>
                                                <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                                <div v-if="uploadedFile !=null">
                                                    <v-img :src=uploadedFile width="50" height="80" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                </div>
                                            </div>
                                            <!--Note-->
                                            <div v-if="item.AnswerType==='Note'">
                                                <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea>
                                            </div>
                                        </div>

                                        <!--Details Panel-->
                                        <div v-else>
                                            <h4 class="font-weight-regular heading-3-dark">
                                                {{index +1}}.{{item.QuestionName}}
                                            </h4>
                                            <h4 v-if="item.AnswerType!=='Audio Capture' && item.AnswerType!=='Photo Capture' && item.AnswerType!=='GPS Capture'" class="font-weight-regular heading-3">{{item.Answer}}</h4>
                                            <!--Audio Capture-->
                                            <audio class="mt-2" controls v-if="item.AnswerType==='Audio Capture'">
                                                <source :src="fileUrl+item.Answer" type="audio/mp3">

                                            </audio>
                                            <!--Image Capture-->
                                            <v-img class="mt-2" v-if="item.AnswerType==='Photo Capture' || item.AnswerType==='Signature Capture' " :src="fileUrl+item.Answer" width="250" height="180" aspect-ratio="1.7"></v-img>

                                            <GmapMap class="mt-2" v-if="item.AnswerType==='GPS Capture'" v-model="item.Answer"
                                                     :center="Coordinates"
                                                     :zoom="7"
                                                     style="width: 100%; height: 250px" @click.native="GetGPSCoordinates(item.QuestionID)">
                                                <GmapMarker :position="Coordinates"
                                                            :clickable="true"
                                                            :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />

                                            </GmapMap>
                                        </div>
                                    </v-col>
                                </v-row>
                            </template>
                            <v-row justify="center" v-if="BenDetailsEdit">
                                <v-col md="6" class="py-4 text-center">
                                    <Success-Button id="AutoTest_IP290" class="btn-122x36 mr-4" title="Save" @click.native="UpdateBeneficiary()"></Success-Button>
                                    <Default-Button-Outlined id="AutoTest_IP291" class="btn-122x36" title="Cancel" @click.native="BenDetailsEdit=false"></Default-Button-Outlined>
                                </v-col>
                            </v-row>
                        </v-tab-item>
                        <v-tab-item>
                            <v-row justify="center" v-if="lstSurvey.length==0">
                                <v-col md="3">
                                    <h4 class="font-weight-regular heading-3 ">
                                        Please link some survey and come back..
                                    </h4>

                                </v-col>

                            </v-row>
                            <v-row v-if="lstSurvey.length>0">
                                <v-col>
                                    <v-data-table class="mt-4" :headers="BenDataCollectedTableHeaders"
                                                  :items="lstSurvey"
                                                  item-key="SurveyID"
                                                  item-text="SurveyName"
                                                  show-select>
                                        <template v-slot:item.UpdatedDate="{ item }">
                                            <div class="pa-0 text-limit" style="width:150px">
                                                {{item.UpdatedDate.substr(0, 10)}}
                                            </div>
                                        </template>
                                        <template v-slot:item.SurveyName="{item}">
                                            <v-list dense class="py-0 table-v-list">
                                                <v-list-item id="AutoTest_IP292" class="my-0 px-0" @click="GetSurveyResponses(item.SurveyID)">
                                                    <div class="pa-0 text-limit" style="width:350px">
                                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                                            <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                                                        </v-avatar>
                                                        {{item.SurveyName}}
                                                    </div>
                                                </v-list-item>
                                            </v-list>
                                        </template>
                                    </v-data-table>
                                </v-col>
                            </v-row>

                        </v-tab-item>
                    </v-tabs>
                </v-col>
            </v-row>

            <!--Detail edit success snackbar-->
            <!--<v-snackbar v-model="BenDetailsEditSuccessSnackbar" bottom="" color="primary">
            Beneficiary details edited successfully
        </v-snackbar>-->
            <!--Data collected Dialog-->
            <v-dialog v-model="DataCollectedDialog" style="height:550px;">
                <div class="v-scrolling-div" style="height:550px;overflow-x: hidden;">
                    <v-card class="pt-0 px-4 " style="overflow-x: hidden;" elevation="0">
                        <div style="text-align:end;">
                            <v-btn id="AutoTest_IP293" text="" @click="DataCollectedDialog=false">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </div>
                        <v-card-text class="pt-0 px-0">
                            <template v-for="(sectionItem ,index) in QstDtlsForIndividualResponse">
                                <v-row class="mt-4" no-gutters>
                                    <v-col sm="2">
                                        <v-card style="height:4vh;background:#667686;text-align:center;width:70%;" class="pt-1" tile>
                                            <span style="color:white;font-size:16px;font-weight:bold;">Section {{index+1}} of {{QstDtlsForIndividualResponse.length}}</span>
                                        </v-card>
                                    </v-col>
                                </v-row>
                                <v-expansion-panels accordion>
                                    <v-expansion-panel @click="openPanelInd(index)">
                                        <v-expansion-panel-header hide-actions class="pa-0">
                                            <v-card elevation="0">
                                                <v-row>
                                                    <v-col sm="11">
                                                        <h5 class="page-head">
                                                            <v-avatar color="white" size="25" class="elevation-1 mr-2 ml-3 mt-2">
                                                                <v-icon size="15" color="primary">mdi-file-question-outline</v-icon>
                                                            </v-avatar>
                                                            {{sectionItem.sectionName}}
                                                        </h5>
                                                    </v-col>
                                                    <v-col sm="1">
                                                        <Default-Icon-Button-Outlined v-if="(QstDtlsForIndividualResponse[index].sectionArrow == '')" @click.native="(QstDtlsForIndividualResponse[index].sectionArrow == 1)" id="AutoTest_IP666" class="mr-4 mt-2 float-left" icon="mdi-chevron-down"></Default-Icon-Button-Outlined>
                                                        <Default-Icon-Button-Outlined @click.native="(QstDtlsForIndividualResponse[index].sectionArrow == '')" v-else-if="QstDtlsForIndividualResponse[index].sectionArrow == 1" id="AutoTest_IP667" class="mr-4 float-left" icon="mdi-chevron-up"></Default-Icon-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                                <v-row class="mb-1">
                                                    <v-col sm="12">
                                                        <span class=" mr-2 ml-3 mt-2">{{sectionItem.SectionDescription}}</span>
                                                    </v-col>
                                                </v-row>
                                                <v-divider></v-divider>
                                                <v-row>
                                                    <v-col sm="12" v-if="(QstDtlsForIndividualResponse[index].sectionArrow == '')">
                                                        <v-chip color="#ABA8A8" dark="" class=" mr-2 ml-3 mt-2" v-if="(QstDtlsForIndividualResponse[index].sectionArrow == '')">
                                                            <!-- {{sectionItem.SurveySecQuestions.length}} -->
                                                            <!-- {{lstQstnResponses[index].length}}  -->
                                                            <!-- {{sectionItem.lstQstnResponsesInd[0].length}} -->
                                                            Responses
                                                        </v-chip>
                                                    </v-col>
                                                </v-row>
                                            </v-card>
                                            </
                                        </v-expansion-panel-header>
                                        <v-expansion-panel-content class="v-scrolling-div" style="height:350px;overflow-x: hidden;">
                                            <v-card>
                                                <div>
                                                    <template v-for="(respItem ,ind) in lstSurveyResponses">
                                                        <v-col cols="12" class="py-0 px-0">
                                                            <template>
                                                                <v-card v-for="(item,idx) in respItem.QuestionResponses" elevation="0">
                                                                    <v-card-text class="mb-4">
                                                                        <p class="black--text ma-0">{{respItem.QuestionName}}</p>
                                                                        <!--Text type answers-->
                                                                        <!-- <div class="survey-qstn-respons-chip" v-if="((Item.QuestionType==='Single Line Text'
                                                                    || Item.QuestionType=== 'Multi Line Text'
                                                                    || Item.QuestionType==='Cascading Dropdown'
                                                                    || Item.QuestionType==='Date Picker'
                                                                    || Item.QuestionType==='Note'
                                                                    || Item.QuestionType==='Numeric' || Item.QuestionType==='Dropdown'))"> -->
                                                                        <div class="survey-qstn-respons-chip">
                                                                            {{item.Answer}}
                                                                        </div>
                                                                    </v-card-text>
                                                                    <v-divider></v-divider>
                                                                </v-card>
                                                            </template>
                                                        </v-col>
                                                    </template>
                                                </div>

                                            </v-card>
                                        </v-expansion-panel-content>
                                    </v-expansion-panel>
                                </v-expansion-panels>
                            </template>
                        </v-card-text>
                    </v-card>
                </div>
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
    </div>
</template>
<script>
    import SuccessButton from '@/components/success-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import { WebCam } from "vue-web-cam"
    import DefaultIconButtonOutlined from '@/components/default-icon-button-outlined.vue'
    import PreLoader from "@/components/pre-loader.vue";
    import objUtils from '../../utils.js'

    var objPB;
    async function importscript() {
        return Promise.all([
            import("../../BL/ProjectBeneficiary.js").then(mod => {
                objPB = new mod.ProjectBeneficiary();
            })
        ]);
    };
    export default {
        data() {
            return {
                screenAccess: 0,
                RWAccess: 0,
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectID: 0,
                BenefTID: 0,
                BenefID: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,

                BenDetailsEdit: false,
                //BenDetailsEditSuccessSnackbar: false,
                BenDataCollectedTableHeaders: [
                    { text: 'Survey Name', value: 'SurveyName' },
                    { text: 'Last Modified', value: 'UpdatedDate' }
                ],


                DataCollectedDialog: false,
                BenefTypeName: "",
                BenefName: "",
                lstBeneficiaryDetails: [],
                vmDate: new Date().toISOString().substr(0, 10),
                //mDate: "",
                lstSurvey: [],
                lstSurveyResponses: [],
                multiChoiceAns: [],
                headers: {
                    'X-Custom-Header': 'some data'
                },
                blobFileInput: null,
                //BenefName: "",
                //AnsFileName: "",
                AnsBlob: [],//{ QuestionID: 0, blobAudio: "" },
                AnsBlobQID: "",
                audUpload: [],
                IsNew: [],
                blobFilename: "",

                fileUrl: "",
                //editPhoto: true,
                delList: [],
                //GPS Capture
                Coordinates: {
                    lat: 0,
                    lng: 0,
                },

                //Web cam Data
                dialog: false,
                img: null,
                camera: null,
                deviceId: null,
                devices: [],
                Msg: "",
                startCamera: false,
                camOn: false,
                autoPlay: false,
                uploadedImg: "",
                uploadedFile: "",
                PreLoader: false,
                QstDtlsForIndividualResponse:[{
                    SectionDescription:"Please input the basic information of the reviewer",
                    sectionName:"Reviewer information",
                    sectionArrow:''
                },{
                    SectionDescription:"This section will gauge how employees behavioural profile.",
                    sectionName:"Behavioural Analysis",
                    sectionArrow:''
                },{
                    SectionDescription:"Will gauge the performance of the employees for duties they have to perform .",
                    sectionName:"Performance Review",
                    sectionArrow:""
                    },],
                rules: {
                    required: value => !!value || 'Required.',
                },
                numberRules: [
                    v => !v || /\[0-9]$/.test(v) || 'Please enter valid number'
                ],
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'Please enter valid E-mail'
                ],
                phoneNoRules: [
                    v => !v || /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/.test(v) || 'Please enter valid Phone No'
                ],
                cascadingAns :[],
            }
        },

        created() {
            //GPS Capture
            this.$getLocation({}).then((Coordinates) => {
                this.Coordinates = Coordinates;
            });
        },

        components: {
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Success-Button': SuccessButton,
            'vue-web-cam': WebCam,
            'Secondary-Button': SecondaryButton,
            'Default-Icon-Button-Outlined': DefaultIconButtonOutlined,
            "Pre-Loader": PreLoader,
        },
        computed: {
            computedDate() {
                return objUtils.common.formatDate(this.vmDate)
            },
            //Web Cam Functions
            device: function () {
                return this.devices.find(n => n.deviceId === this.deviceId);
            },
            computedUDate() {
                return objUtils.common.formatDate(this.vmUDate)
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
            }
        },

        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            this.BenefTID = this.$route.query.BenfTID;
            this.BenefID = this.$route.query.BenfID;
            this.fileUrl = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.$route.query.ProjID.toString() + "/";
            await importscript();
            await this.GetBeneficiaryDetails();
            await this.GetSurveyDetailsByBenefTypeID(this.BenefTID);
        },
        methods: {
             /// <summary>
             ///  openPanelInd :- Function used to open Individual response panel of question details
             /// </summary>
             /// <param name="index"></param>
             /// <returns> </returns>
             openPanelInd(index) {
                if (this.QstDtlsForIndividualResponse[index].sectionArrow == '') {
                    this.QstDtlsForIndividualResponse[index].sectionArrow = 1
                } else if (this.QstDtlsForIndividualResponse[index].sectionArrow == 1) {
                    this.QstDtlsForIndividualResponse[index].sectionArrow = ''
                }
            },

            /// <summary>
            ///  GetBeneficiaryDetails :- Function used to fetch beneficiary details based on BeneficiaryID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetBeneficiaryDetails() {
               // debugger;
                this.PreLoader = true;
                var AnsOptions = [];
                this.lstBeneficiaryDetails = [];
                this.multiChoiceAns =[];
                var result = await objPB.GetBeneficiaryDetails(this.BenefID);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                }
                else if (result.data) {
                    var BeneficiaryDetails = result.data;
                }

                this.BenefTypeName = BeneficiaryDetails[0].BeneficiaryTypeName;
                this.BenefName = BeneficiaryDetails[0].BeneficiaryName;
                for (var i = 0; i < BeneficiaryDetails.length; i++) {
                    this.lstBeneficiaryDetails.push({
                        QuestionID: BeneficiaryDetails[i].QuestionID,
                        QuestionName: BeneficiaryDetails[i].QuestionName,
                        AnswerTypeID: BeneficiaryDetails[i].AnswerTypeID,
                        AnswerType: BeneficiaryDetails[i].AnswerType,
                        Answer: BeneficiaryDetails[i].Answer,
                        IsUnique: BeneficiaryDetails[i].IsUnique,
                        IsSearchable: BeneficiaryDetails[i].IsSearchable,
                        IsMandatoryResponse: BeneficiaryDetails[i].IsMandatoryResponse,
                        QuestionHint: BeneficiaryDetails[i].QuestionHint,
                        IsValidationQuestion: BeneficiaryDetails[i].IsValidationQuestion,
                        BeneficiaryTypeID: BeneficiaryDetails[i].BeneficiaryTypeID,
                        BeneficiaryID: BeneficiaryDetails[i].BeneficiaryID,
                        BTQstnAnsOptions: BeneficiaryDetails[i].BTQstnAnsOptions,
                        BTQstnAnsOptionList: BeneficiaryDetails[i].BenefAnswerOptions, //BeneficiaryDetails[i].BTQstnAnsOptionList,
                        BTQstnAnsCascadingOptionList: BeneficiaryDetails[i].BenefAnswerCascadingOptionList,//BeneficiaryDetails[i].BTQstnAnsCascadingOptionList,
                        IsRegistrationDone: BeneficiaryDetails[i].IsRegistrationDone,
                        BenefAnswerOptions: BeneficiaryDetails[i].BenefAnswerOptions,
                        BTQstnValidationList: BeneficiaryDetails[i].BTQstnValidationList,
                        IsUpload: 0,
                        IsNew: 0,
                        startCamera: false,
                        camOn: false,
                        img: '',
                        uploadedImg: '',
                        uploadedSign: '',
                        ValidationErrorTest: '',
                        UniqueErrorTest: '',
                        IsValidationError: false,
                        IsUniqueValidation: false,
                        Coordinates: [{ lat: 0, lng: 0, }],
                        qType: "", QstType: "", textLength: 10, minVal: 0, maxVal: 9999999999, qRules: [],
                    });

                    this.audUpload[i] = 0;
                    //this.IsNew[i] = false;

                    if (this.lstBeneficiaryDetails[i].AnswerType == "Date Picker")
                        this.vmUDate = this.lstBeneficiaryDetails[i].Answer;
                    if (this.lstBeneficiaryDetails[i].AnswerType == "Multi-Choice") {

                        if (this.lstBeneficiaryDetails[i].Answer != "") {
                            AnsOptions = this.lstBeneficiaryDetails[i].Answer.split(",");
                           // this.multiChoiceAns = AnsOptions;
                            for (var j = 0; j < AnsOptions.length; j++)
                            {
                                 this.multiChoiceAns.push({OptionValue:AnsOptions[j], QuestionID : this.lstBeneficiaryDetails[i].QuestionID});
                                 this.lstBeneficiaryDetails[i].BenefAnswerOptions.filter((x) => x.OptionValue == AnsOptions[j])[0].IsOptSelected = true;
                            }
                        }
                    }
                    if (this.lstBeneficiaryDetails[i].AnswerType == "Cascading Dropdown") {
                        
                        AnsOptions = this.lstBeneficiaryDetails[i].Answer.split(",");
                        this.cascadingAns.push({QuestionID : BeneficiaryDetails[i].QuestionID , SelectedOption :AnsOptions });

                        for (var j = 1; j < AnsOptions.length; j++) {
                            var pID = this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems.filter((x) => x.Name == AnsOptions[j])[0].ParentID;
                            var sublst = this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems.filter((x) => x.ParentID == pID);
                            this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems = sublst;
                        }
                    }
                    if (this.lstBeneficiaryDetails[i].AnswerType == 'GPS Capture') {

                        var arr = this.lstBeneficiaryDetails[i].Answer.split(",");
                        this.Coordinates.lat = parseInt(arr[0]);
                        this.Coordinates.lng = parseInt(arr[1]);
                    }
                }
                this.PreLoader = false;
            },

            /// <summary>
            ///  EditCascadingFilterSubItem :- Function used to edit cascading filter sub items when changing the option
            /// </summary>
            /// <param name="QItem"></param>
            /// <param name="OptItem"></param>
            /// <param name="parentID"></param>
            /// <param name="edIndx"></param>
            /// <returns> </returns>
            async EditCascadingFilterSubItem(QItem, OptItem, parentID, edIndx) {
               
                var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter(x => x.ID == parentID)[0].Name;

                this.cascadingAns.filter(x=> x.QuestionID == QItem.QuestionID)[0].SelectedOption[edIndx] = selectedOption;

                // this.cascadingAns[edIndx] = selectedOption;
                var nextHierarchyOrder = OptItem.HierarchyOrder + 1;
                var IndexOrder = edIndx + 1;
                if (this.lstBeneficiaryDetails.filter(x => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList.length >= IndexOrder) {
                    var data = new FormData();
                    data.append("optionValue", OptItem.OptionValue);
                    data.append("parentID", parentID);
                    var result = await objPB.GetCascadingOptionsList(data);

                    if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.data) {
                            var sublist = result.data;
                        }
                    
                    this.lstBeneficiaryDetails.filter(x => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList.filter(x => x.HierarchyOrder == nextHierarchyOrder)[0].BTQstnAnsCascadingListItems = sublist[0].BTQstnAnsCascadingListItems;
                    //this.cascadingAns[edIndx + 1] = "Select ";
                    this.cascadingAns.filter(x=> x.QuestionID == QItem.QuestionID)[0].SelectedOption[edIndx + 1] = "Select ";
                }
            },

            /// <summary>
            ///  GetMultiChoice :- Function used to push or remove multichoice option value while selecting or deselecting
            /// </summary>
            /// <param name="optItem"></param>
            /// <param name="optSelected"></param>
            /// <param name="optVal"></param>
            /// <returns> </returns>
            GetMultiChoice(optItem, optSelected, optVal, QuestionID,ItemIndex) {
                if (optSelected){
                        this.multiChoiceAns.push({OptionValue:optVal, QuestionID : QuestionID});}
                else{
                        const index = this.multiChoiceAns.findIndex((element, index) => {
                        if (element.QuestionID === optItem.QuestionID && element.OptionValue == optVal) {
                           return true
                       }});
                    this.multiChoiceAns.splice(index,1);
                }
            },

            /// <summary>
            ///  UpdateBeneficiary :- Function used to update beneficiary under basic details tab
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateBeneficiary() {
                if (await this.fnCheckQuestionValidationAndCondition(this.lstBeneficiaryDetails) == true) {
                    return false;
                }
                this.PreLoader = true;
                var Ans = "";
                var blobFile = "";
                for (var i = 0; i < this.lstBeneficiaryDetails.length; i++) {
                    Ans = "";
                    if (this.lstBeneficiaryDetails[i].AnswerType == 'Multi-Choice') {
                        
                        var MultiChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)
                        for (var j = 0; j < MultiChoiceAns.length; j++)
                            Ans = (Ans != '') ? Ans + ',' + MultiChoiceAns[j].OptionValue : MultiChoiceAns[j].OptionValue;
                        this.lstBeneficiaryDetails[i].Answer = Ans;
                    }
                    if (this.lstBeneficiaryDetails[i].AnswerType == "Cascading Dropdown") {

                        var CasQuestionsAns = this.cascadingAns.filter(x=> x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)
                        for (var j = 0; j < CasQuestionsAns[0].SelectedOption.length; j++){
                                Ans = Ans != "" ? Ans + "," + CasQuestionsAns[0].SelectedOption[j]: CasQuestionsAns[0].SelectedOption[j];
                        }
                        this.lstBeneficiaryDetails[i].Answer = Ans;
                    }
                    if ((this.lstBeneficiaryDetails[i].AnswerType == "Audio Capture") || (this.lstBeneficiaryDetails[i].AnswerType == "Signature Capture")
                        || (this.lstBeneficiaryDetails[i].AnswerType == "Photo Capture")) {

                        if (this.AnsBlob.filter(x => x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)[0] != null) {
                            blobFile = this.AnsBlob.filter(x => x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)[0].blobData;
                            this.FileUpload(blobFile, this.lstBeneficiaryDetails[i].Answer);

                            Ans = this.lstBeneficiaryDetails[i].Answer;
                            this.lstBeneficiaryDetails[i].AnsBlob = null;
                        }
                    }
                }

                var data = new FormData();
                data.append("BenefUpdateList", JSON.stringify(this.lstBeneficiaryDetails));
                var result = await objPB.UpdateBeneficiary(data);
                if (result.status == "Unhandled Exception") {
                    this.PreLoader = false;
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = "Beneficiary updated Successfully ... ";
                        this.successSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in updating Beneficiary  ";
                        this.errorSnackbarMsg = true;
                    }

                    this.PreLoader = false;

                    this.GetBeneficiaryDetails();
                    this.BenDetailsEdit = false;
                    this.RemovePreviousFiles();
                    //this.BenDetailsEdit = false
                }
            },
            //===============Audio Capture=============//

            /// <summary>
            ///  callback :- Function used to callback fires after click the record button/after pause recording/before uploading/after successful uploading/after failure uploading
            /// </summary>
            /// <param name="data"></param>
            /// <returns> </returns>
            callback(data) {
                console.debug(data)
            },

            /// <summary>
            ///  SaveAudio :- Function used to save audio after recording
            /// </summary>
            /// <param name="data"></param>
            /// <returns> </returns>
            async SaveAudio(data) {
                this.blobFileInput = data.blob;
                this.SaveFile("Audio", this.blobFileInput);
            },

            //===============Signature Capture=======================//

            /// <summary>
            ///  clearSign :- Function used to clear signature from signature pad
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            clearSign() {
                this.$refs.signaturePad[0].signaturePad.clear();
                this.$refs.signaturePad[0].signatureData = [];
            },

            /// <summary>
            ///  saveSign :- Function used to save signature
            /// </summary>
            /// <param name="QID"></param>
            /// <returns> </returns>
            saveSign(QID) {
                this.AnsBlobQID = QID;
                var prevAns = this.lstBeneficiaryDetails.filter(x => x.QuestionID == this.AnsBlobQID)[0].Answer;
                    if (prevAns != null)
                        this.delList.push({ QuestionID: QID, Answer: prevAns });

                //var signData = this.$refs.signaturePad[0].signatureData;
                var signPad = this.$refs.signaturePad[0].signaturePad;
                if (signPad.isEmpty()) {
                    this.Msg = "Please provide a signature first.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                else {
                    var dataURL = signPad.toDataURL("image/png");
                    var data = dataURL.split(',')[1];
                    let blobFile = this.b64toBlob(data, 'image/png');
                    this.SaveFile("Signature", blobFile);
                    this.Msg = "Signature saved";
                    this.successSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  b64toBlob :- Function used to convert a Base64 string into a Blob to upload it as a image file
            /// </summary>
            /// <param name="b64Data"></param>
            /// <param name="contentType"></param>
            /// <returns> </returns>
            b64toBlob(b64Data, contentType) {
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
            //===============Photo Capture=======================//
            //Web Cam Functions

            /// <summary>
            ///  onCapture :- Function used to capture photo
            /// </summary>
            /// <param name="QID"></param>
            /// <returns> </returns>
            onCapture(QID) {
                this.AnsBlobQID = QID;
                var prevAns = this.lstBeneficiaryDetails.filter(x => x.QuestionID == this.AnsBlobQID)[0].Answer;
                    if (prevAns != null)
                        this.delList.push({ QuestionID: QID, Answer: prevAns });

                this.img = this.$refs.webcam[0].capture();
                var data = this.img.split(',')[1];
                let blobFile = this.b64toBlob(data, 'image/jpg"');
                this.SaveFile("Photo", blobFile);
            },

            /// <summary>
            ///  onStarted :- Function used for emitting once the stream has started
            /// </summary>
            /// <param name="stream"></param>
            /// <returns> </returns>
            onStarted(stream) {
                console.log("On Started Event", stream);
            },

            /// <summary>
            ///  onStopped :- Function used for emitting once the stream has stopped
            /// </summary>
            /// <param name="stream"></param>
            /// <returns> </returns>
            onStopped(stream) {
                console.log("On Stopped Event", stream);
            },

            /// <summary>
            ///  onStop :- Function used to stop the camera
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            onStop() {
                this.$refs.webcam.stop();
            },

            /// <summary>
            ///  onStart :- Function used to start the camera after stopping it
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            onStart() {
                this.$refs.webcam.start();
            },

            /// <summary>
            ///  onError :- Function used for emitting if the stream failed to start with the error returned
            /// </summary>
            /// <param name="error"></param>
            /// <returns> </returns>
            onError(error) {
                console.log("On Error Event", error);
            },

            /// <summary>
            ///  onCameras :- Function used for emitting when a list of all cameras available is loaded
            /// </summary>
            /// <param name="cameras"></param>
            /// <returns> </returns>
            onCameras(cameras) {
                this.devices = cameras;
                console.log("On Cameras Event", cameras);
            },

            /// <summary>
            ///  onCameraChange :- Function used for emitting when camera change occurs
            /// </summary>
            /// <param name="deviceId"></param>
            /// <returns> </returns>
            onCameraChange(deviceId) {
                this.deviceId = deviceId;
                this.camera = deviceId;
                console.log("On Camera Change Event", deviceId);
            },


            //===================================================//
            /// <summary>
            ///  SaveFile :- Function used to save audio/signature/photo file
            /// </summary>
            /// <param name="fileType"></param>
            /// <param name="blbFile"></param>
            /// <returns> </returns>
            async SaveFile(fileType, blbFile) {
                var fname = "";
                var ansType = "";
                var timestamp = new Date().valueOf();

                var prevData = this.AnsBlob.filter(x => x.QuestionID == this.AnsBlobQID);
                if (prevData.length > 0)
                    this.AnsBlob.splice(this.AnsBlob.indexOf(this.AnsBlobQID), 1);

                if (fileType == "Audio") {
                    fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
                    ansType = "Audio Capture";
                }
                if (fileType == "Signature") {
                    fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Signature Capture";
                }
                if (fileType == "Photo") {
                    fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split('.').pop() : "jpg");
                    ansType = "Photo Capture";
                }

                this.AnsBlob.push({ QuestionID: this.AnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname });
                this.lstBeneficiaryDetails.filter(x => x.QuestionID == this.AnsBlobQID)[0].Answer = fname;

            },

            /// <summary>
            ///  FileUpload :- Function used to upload file based on blobFileInput,ProjectID,FileName
            /// </summary>
            /// <param name="blobFileInput"></param>
            /// <param name="FileName"></param>
            /// <returns> </returns>
            async FileUpload(blobFileInput, FileName) {
                const formData = new FormData()
                formData.append('myFile', blobFileInput);
                formData.append('ProjectID', this.ProjectID);
                formData.append('FileName', FileName);
                var result = await objPB.UploadFiles(formData);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                this.blobFileInput = null;
            },

            /// <summary>
            ///  RemovePreviousFiles :- Function used to delete previous files based on ProjectID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async RemovePreviousFiles() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("DocList", JSON.stringify(this.delList));
                var result = await objPB.RemoveQuestionnaireFiles(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
            },
            //==========GPS Capture=============================//
            /// <summary>
            ///  GetGPSCoordinates :- Function used to get GPS coordinates
            /// </summary>
            /// <param name="QID"></param>
            /// <returns> </returns>
            GetGPSCoordinates(QID) {
                var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
                this.lstBeneficiaryDetails.filter(x => x.QuestionID == QID)[0].Answer = gpsCoordinates;
            },

            //========Survey===========================//
            /// <summary>
            ///  GetSurveyDetailsByBenefTypeID :- Function used to fetch survey details based on BeneficiaryTypeID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetSurveyDetailsByBenefTypeID() {
                var result = await objPB.GetSurveyDetailsByBenefTypeID(this.BenefTID);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        this.lstSurvey = result.data;
            },

            /// <summary>
            ///  GetSurveyResponses :- Function used to fetch survey responses based on SurveyID
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSurveyResponses(SurveyID) {
                this.DataCollectedDialog = true;
                var data = new FormData();
                data.append("SurveyID", SurveyID);
                data.append("offset", 0);
                var result = await objPB.GetSurveyResponses(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.SummaryResponse)

                this.lstSurveyResponses = result.SummaryResponse;
            },

            /// <summary>
            ///  updateCoordinates :- Function used to update coordinates
            /// </summary>
            /// <param name="evnt"></param>
            /// <param name="Qitem"></param>
            /// <returns> </returns>
            updateCoordinates(evnt, Qitem) {
                this.Coordinates = {
                    lat: evnt.lat(),
                    lng: evnt.lng(),
                };
                Qitem.Answer = Coordinates.lat + "," + Coordinates.lng;
            },

            /// <summary>
            ///  onFocusCheckQstnValidation :- Function used to check question validation for question types (Single line,Multi line,Numeric,DatePicker)
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            onFocusCheckQstnValidation(QItem) {
                var valConditions = QItem.BTQstnValidationList;
                var NewErrorMsg = "";
                var OldErrorMsg = "";
                var errMsg = "";
                //this.qType = "";
                //this.QstType = "";
                // this.txtLength = 0;
                //this.minVal = 0;
                //this.maxVal = 9999999999;
                // this.qRules = [];
                var today = new Date().toLocaleDateString('en-GB');
                if (valConditions != null) {
                    this.IsQstnValidation = true;
                    // ValidationCondition
                    for (var i = 0; i < valConditions.length; i++) {
                        if (valConditions[i].ValidationCondition == 'Phone Number') {
                            QItem.qType = "number";
                            QItem.QstType = "PhoneNumber";
                            QItem.txtLength = 10;
                            QItem.minVal = 0;
                            QItem.maxVal = 9999999999;
                            QItem.qRules = this.phoneNoRules;
                            // QItem.ErrorText ="";
                            QItem.IsValidationError = false;
                        }
                        else if (valConditions[i].ValidationCondition == 'Email') {
                            QItem.textLength = 1000;
                            QItem.minVal = 0;
                            QItem.qType = "email";
                            QItem.qRules = this.emailRules;
                            // QItem.ErrorText ="";
                            QItem.IsValidationError = false;

                        }
                        else if (valConditions[i].ValidationCondition == 'Min & Max Range') {
                            QItem.textLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = valConditions[i].MinValue;
                            QItem.maxVal = valConditions[i].MaxValue;
                            QItem.qType = "number";
                            //this.qRules = [
                            //    v => !v || /\[0-9]{txtLength}$/.test(v) || 'Please enter valid number'
                            //];//this.numberRules;
                            if (QItem.Answer != "") {
                                if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {
                                    if (valConditions[i].LogicOperator == 1 && QItem.ValidationErrorTest == "") //-- 1--> Or Condition
                                    {

                                    }
                                    NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                                    //OldErrorMsg = QItem.ErrorText;
                                    if (OldErrorMsg == "") {
                                        QItem.ValidationErrorTest = NewErrorMsg;
                                    }
                                    else {
                                        QItem.ValidationErrorTest = OldErrorMsg
                                    }
                                    QItem.IsValidationError = true;
                                }
                                else {
                                    QItem.IsValidationError = false;
                                    QItem.ValidationErrorTest = "";
                                }
                            }

                        }
                        else if (valConditions[i].ValidationCondition == 'Should be less than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            //this.minVal = new Date().toISOString().substr(0, 10);
                            QItem.maxVal = new Date().toISOString().substr(0, 10);
                            // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer > objUtils.common.formatDateTodayDate(today)) {
                                NewErrorMsg = "Should be less than Current date";
                                //OldErrorMsg = QItem.ErrorText;

                                if (OldErrorMsg == "") {
                                    QItem.ValidationErrorTest = NewErrorMsg;
                                }
                                else {
                                    QItem.ValidationErrorTest = OldErrorMsg
                                }
                                QItem.IsValidationError = true;
                            }
                            else {
                                QItem.IsValidationError = false;
                                QItem.ValidationErrorTest = "";
                            }
                        }
                        else if (valConditions[i].ValidationCondition == 'Should be greater than Current date') {
                            // this.txtLength = valConditions[i].MaxValue.toString().length;
                            QItem.minVal = new Date().toISOString().substr(0, 10);
                            //this.maxVal = valConditions[i].MaxValue;
                            // this.qType = "date";
                            //this.qRules = '';
                            if (QItem.Answer < objUtils.common.formatDateTodayDate(today)) {
                                NewErrorMsg = "Should be greater than Current date";
                                //OldErrorMsg = QItem.ErrorText;

                                if (OldErrorMsg == "") {
                                    QItem.ValidationErrorTest = NewErrorMsg;
                                }
                                else {
                                    QItem.ValidationErrorTest = OldErrorMsg
                                }
                                QItem.IsValidationError = true;
                            }
                            else {
                                QItem.IsValidationError = false;
                                QItem.ValidationErrorTest = "";
                            }
                        }
                    }
                }
            },

            /// <summary>
            ///  fnCheckQuestionValidationAndCondition :- Function used to check validation and condition for mandatory/unique questions
            /// </summary>
            /// <param name="BTQuestions"></param>
            /// <returns> </returns>
            async fnCheckQuestionValidationAndCondition(BTQuestions)
            {
                //  Checking for mandatory questions
                var lstMandatoryQuestion = BTQuestions.filter(x => x.IsMandatoryResponse == true);
                //  Checking unique
                var lstunique = BTQuestions.filter(x => x.IsUnique == true);
                //  Checking for Validation Question Min and Max
                var lstValidationQuestion = BTQuestions.filter(x => x.IsValidationQuestion == 1);

                var IsValid = false;

                if (lstMandatoryQuestion.length > 0) {
                    // Selecting from Mandatory Questions without answer
                    var IsQAnswers = lstMandatoryQuestion.filter(x => x.Answer == "" || x.Answer == null);

                    var QstnWithAnswer = lstMandatoryQuestion.filter(x => x.Answer != "" || x.Answer != null);
                    QstnWithAnswer.forEach(function (part, index) {
                        QstnWithAnswer[index].ValidationErrorTest = "";
                        QstnWithAnswer[index].IsValidationError = false;
                    });
                    if (IsQAnswers.length > 0) {
                        for (var i = 0; i <= IsQAnswers.length - 1; i++) {
                            if (IsQAnswers[i].AnswerType != "Cascading Dropdown" && IsQAnswers[i].AnswerType != "Photo Capture" && IsQAnswers[i].AnswerType != "Multi-Choice") {
                                BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                IsValid = true;
                            }
                            else if (IsQAnswers[i].AnswerType == "Cascading Dropdown") {
                                var CascadingAns = this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false));
                                if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].CascadingQuestionOptions.length)) {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                            else if (IsQAnswers[i].AnswerType == "Multi-Choice") {
                                
                                var SelectedmultiChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)
                                if (typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0) {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                }
                            }
                            else if (IsQAnswers[i].AnswerType == "Photo Capture") {
                                 if (typeof IsQAnswers[i].AnsBlob != "undefined") {
                                    if (IsQAnswers[i].AnsBlob.name == "") {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                        IsValid = true;
                                    }
                                    else {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                                    }
                                 }
                                 else {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                 }
                            }
                            else {
                                BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                            }
                        }
                    }
                }

                // Code for validations checking
                if (lstValidationQuestion.length > 0) {
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
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }


                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;

                                }
                                else {
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }// if ends here
                            if (valConditions[i].ValidationCondition == "Should be less than Current date") {
                                if (IsQAnswers[intValQstCnt].Answer > objUtils.common.formatDateTodayDate(today)) {
                                    NewErrorMsg = "Should be less than Current date";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    if (OldErrorMsg == "") {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    this.qRules = "";
                                    //this.chkQstnValid = true;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }// if ends here

                            if (valConditions[i].ValidationCondition == "Should be greater than Current date") {
                                if (IsQAnswers[intValQstCnt].Answer < objUtils.common.formatDateTodayDate(today)) {
                                    NewErrorMsg = "Should be greater than Current date";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    if (OldErrorMsg == "") {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }

                            // Checking Phone number validation
                            if (valConditions[i].ValidationCondition == "Phone Number") {

                                var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                if (!IsQAnswers[intValQstCnt].Answer.match(phoneNum)) {

                                    NewErrorMsg = "Please enter a valid Phone Number";
                                    //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                    if (OldErrorMsg == "") {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }
                            // Checking email  validation
                            if (valConditions[i].ValidationCondition == "Email") {

                                var email = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                if (!IsQAnswers[intValQstCnt].Answer.match(email)) {
                                    NewErrorMsg = "Please enter a valid Email";
                                    //OldErrorMsg = this.lstBTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules = "";
                                    if (OldErrorMsg == "") {
                                        BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                                    } else { BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg }
                                    // this.qRules = "";
                                    //this.chkQstnValid = true;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                                }
                                else {
                                    //this.chkQstnValid = false;
                                    BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                                }
                            }
                            //

                        }
                    }
                }
                if (lstunique.length > 0) {
                    // Selecting from unique answer
                    var IsuniqueAnswers = lstunique.filter(x => x.Answer != "" || x.Answer != null);
                    for (var i = 0; i <= IsuniqueAnswers.length - 1; i++) {
                        var data = new FormData();
                        data.append("Answer", IsuniqueAnswers[i].Answer);
                        var result = await objPB.IsUniqueResponse(data);
                        if (result.status == "Unhandled Exception")
                            this.$router.push({ name: "error" });
                        else {
                            if (result.status == "success") {
                                BTQuestions.filter(x => x.QuestionID == IsuniqueAnswers[i].QuestionID)[0].UniqueErrorTest = "Must be a unique one";
                                BTQuestions.filter(x => x.QuestionID == IsuniqueAnswers[i].QuestionID)[0].IsUniqueValidation = true;
                                IsValid = true;
                            }
                        }
                    }
                }
                return IsValid;
            },
        },

    }
</script>
<style scoped>
    .text-limit {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

        .text-limit:hover {
            white-space: normal;
            overflow: visible;
        }
</style>
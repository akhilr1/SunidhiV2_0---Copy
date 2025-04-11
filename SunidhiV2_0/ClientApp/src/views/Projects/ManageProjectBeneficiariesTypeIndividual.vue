<template>
    <div>
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <v-container fluid>
            <v-row>
                <v-col md="auto">
                    <h5 class="page-head">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="15" color="primary">mdi-account-multiple</v-icon>
                        </v-avatar>
                        <!--if path is ManageProjectBeneficiariesTypeIndividual-->
                        <router-link v-show="$route.name=='ManageProjectBeneficiariesTypeIndividual'" :to="{ name: 'ManageProjectBeneficiaries', query: {WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link>
                        <!--____________-->
                        <!--if path is MyprojectsDraftIndividualBeneficiaries-->
                        <router-link v-show="$route.name=='MyprojectsDraftIndividualBeneficiariesTypeIndividual'" :to="{ name: 'MyprojectsDraftIndividualBeneficiaries', query: {WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link>
                        <!--____________-->
                      <!--if path is MyprojectsDeployedIndividualBeneficiaries-->
                      <router-link v-show="$route.name=='MyprojectsDeployedIndividualBeneficiariesTypeIndividual'" :to="{ name: 'MyprojectsDeployedIndividualBeneficiaries', query: {WsID: WorkspaceID, Access:RWAccess, InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Beneficiaries</router-link>
                      <!--____________-->
                        > {{benefTypeName}}
                    </h5>
                </v-col>
                <v-spacer></v-spacer>
                <v-col md="auto" v-if="lstAllBeneficiaries.length!=0">
                    <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn v-on="on" class="success-btn mr-4 float-left" depressed dark height="37">
                                Add Benificiary
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list dense="" color="white" light="" width="auto" max-width="250" class="action-button-list">
                            <v-list-item id="AutoTest_IP366" link @click="RegBeneficiaryDialog=true">
                                <v-list-item-content>
                                    Register
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item id="AutoTest_IP367" link @click="dlgImportRegisterBeneficiary=true">
                                <v-list-item-content>
                                    Import & Register
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                    <v-menu offset-y="" transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn id="AutoTest_IP368" height="38" v-on="on" color="grey" outlined="" class="elevation-0 mr-4 float-left" @click="isBulk=true,snackbarBulkActions=true">
                                Bulk Actions
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list color="white" light="" width="auto" max-width="250" class="action-button-list">
                            <v-list-item id="AutoTest_IP369" @click.stop="dlgRemoveBeneficiary = true" v-bind:disabled="vmBeneficiary.length==0">
                                <v-list-item-icon>
                                    <v-icon>mdi-trash-can-outline</v-icon>
                                </v-list-item-icon>
                                <v-list-item-content>
                                    Remove
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                    <v-text-field label="Search" class="mr-4 float-left" v-model="searchBeneficiary"
                                  dense="" prepend-inner-icon="mdi-magnify"
                                  outlined="" hide-details="" style="max-width:150px"></v-text-field>

                </v-col>
            </v-row>
            <v-row v-if="lstAllBeneficiaries.length==0">
                <!--If there is no Beneficiaries-->
                <v-col class="text-center">
                    <v-img src="../../images/ic_no_workspace.svg" width="50" height="50" aspect-ratio="1.7" class="center-block my-2"></v-img>
                    <h4 style="color:#585858" class="title text-center">No Beneficiary available</h4>
                    <v-menu offset-y transition="scroll-y-transition" v-bind:disabled="!screenAccess">
                        <template v-slot:activator="{ on }">
                            <v-btn v-on="on" class="success-btn mt-4" depressed dark height="37">
                                Add Benificiary
                                <v-icon>mdi-chevron-down</v-icon>
                            </v-btn>
                        </template>
                        <v-list dense="" color="white" light="" width="auto" max-width="250" class="action-button-list">
                            <v-list-item id="AutoTest_IP370" link @click="RegBeneficiaryDialog=true">
                                <v-list-item-content>
                                    Register
                                </v-list-item-content>
                            </v-list-item>
                            <v-list-item id="AutoTest_IP371" link @click="dlgImportRegisterBeneficiary=true">
                                <v-list-item-content>
                                    Import & Register
                                </v-list-item-content>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                </v-col>
                <!--______________________________________-->
            </v-row>
            <!--<v-row no-gutters>
                <v-col cols="12" xs="12" sm="12" md="12">
                    <v-chip label outlined dark color="red"
                            v-if="(isBulk && vmBeneficiary.length ==0)">
                        * Please select Beneficiary from the list
                    </v-chip>
                </v-col>
            </v-row>-->
            <v-row v-if="lstAllBeneficiaries.length>0">
                <v-col>
                    <v-data-table :headers="BenTableheaders"
                                  :items="lstAllBeneficiaries"
                                  :search="searchBeneficiary"
                                  item-key="BeneficiaryID"
                                  v-model="vmBeneficiary"
                                  show-select>

                        <template v-slot:item.CreatedDate="{ item }">
                            <div class="pa-0 text-limit" style="width:150px">
                                {{formatDate(new Date(item.CreatedDate).toISOString().substr(0, 10))}}
                            </div>
                        </template>
                        <template v-slot:item.UpdatedDate="{ item }">
                            <div class="pa-0 text-limit" style="width:150px">
                                {{formatDate(new Date(item.UpdatedDate).toISOString().substr(0, 10))}}
                            </div>
                        </template>
                        <template v-slot:item.BeneficiaryName="{item}">
                            <v-list dense class="py-0 table-v-list">
                                <!--if path is ManageProjectBeneficiariesIndividual-->
                                <v-list-item v-if="$route.name=='ManageProjectBeneficiariesTypeIndividual' " link :to="{ name: 'ManageProjectBeneficiariesIndividual', query: { WsID: WorkspaceID, Access:RWAccess , InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID, BenfID :item.BeneficiaryID}}" class="my-0 px-0">
                                     <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.BeneficiaryName}}
                                    </div>
                                </v-list-item>
                                <!--__________________-->
                                <!--if path is MyprojectsDraftIndividualBeneficiariesTypeIndividual-->
                                <v-list-item v-if="$route.name=='MyprojectsDraftIndividualBeneficiariesTypeIndividual' " link :to="{ name: 'MyprojectsDraftIndividualBeneficiariesIndividual', query: { WsID: WorkspaceID, Access:RWAccess , InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID, BenfID :item.BeneficiaryID}}" class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.BeneficiaryName}}
                                    </div>
                                </v-list-item>
                                <!--__________________-->
                                <!--if path is MyprojectsDeployedIndividualBeneficiariesTypeIndividual-->
                                <v-list-item v-if="$route.name=='MyprojectsDeployedIndividualBeneficiariesTypeIndividual' " link="" :to="{ name: 'MyprojectsDeployedIndividualBeneficiariesIndividual', query: { WsID: WorkspaceID, Access:RWAccess , InitID :  InitiativeID, ProjID: ProjectID,BenfTID :BenefTID, BenfID :item.BeneficiaryID}}" class="my-0 px-0">
                                    <div class="pa-0 text-limit" style="width:250px">
                                        <v-avatar size="25" color="#fff" class="elevation-1">
                                            <v-icon size="15" color="primary">mdi-database</v-icon>
                                        </v-avatar>
                                        {{item.BeneficiaryName}}
                                    </div>
                                </v-list-item>
                                <!--__________________-->
                            </v-list>
                        </template>
                        <template v-slot:item.BenActions="{item}">
                            <v-menu close-on-content-click transition="scroll-y-transition" left="" offset-y v-bind:disabled="!screenAccess">
                                <template v-slot:activator="{ on }">
                                    <v-btn min-width="27" height="29" v-on="on" color="grey" outlined dark class="elevation-0 mr-2 px-0">
                                        <v-icon>mdi-dots-vertical</v-icon>
                                    </v-btn>
                                </template>
                                <v-list color="white" light width="auto" class="action-button-list">
                                    <v-list-item id="AutoTest_IP372" link @click.stop="GetBeneficiaryDetails(item.BeneficiaryID)">
                                        <v-list-item-icon><v-icon>mdi-pencil-outline</v-icon></v-list-item-icon>
                                        <v-list-item-content>Edit</v-list-item-content>
                                    </v-list-item>
                                    <v-list-item id="AutoTest_IP373" link @click.stop="dlgRemoveBeneficiary = true,BenefName =item.BeneficiaryName,BenefID= item.BeneficiaryID,isBulk=false">
                                        <v-list-item-icon><v-icon>mdi-trash-can-outline</v-icon></v-list-item-icon>
                                        <v-list-item-content>Remove</v-list-item-content>
                                    </v-list-item>
                                </v-list>
                            </v-menu>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>

            <!--Register Beneficiary Dialog-->
            <v-dialog v-model="RegBeneficiaryDialog" width="800" persistent>
                <v-card >
                    <v-card-title primary-title class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-database</v-icon>
                        </v-avatar>
                        Register
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP374" text="" @click="RegBeneficiaryDialog=false,CancelRegisterDialogue()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="v-scrolling-div" style="max-height:50vh">
                        <h4 class="text-center heading-3 my-4">Please enter basic details to create a new beneficiary.</h4>
                        <template v-for="(item,index) in lstBTQuestions">
                            <v-row justify="center">
                                <v-col md="6">
                                    <!--<p class="black--text">{{index +1}}.{{item.QuestionName}}</p>-->
                                    <label class="text-left heading-3 mb-2" v-if="item.QuestionID==0"><b>{{index +1}}.{{item.QuestionName}}</b></label>
                                    <label class="text-left heading-3 mb-2" v-if="item.QuestionID>0">{{index +1}}.{{item.QuestionName}}</label>
                                    <sub v-if="item.IsMandatoryResponse==true" style="color:red;font-size:large"> *</sub>
                                    <sub style="color:red;font-size:smaller" class="mr-2 ml-2" v-if="item.QuestionHint.length > 0 && item.QuestionHint != null && item.QuestionHint != 'null'">{{item.QuestionHint}}</sub>
                                    <sub style="color:red;font-size:smaller" v-if="item.IsValidationError">{{item.ValidationErrorTest}}</sub>
                                    <sub style="color:red;font-size:smaller" v-if="item.IsUniqueValidation">{{item.UniqueErrorTest}}</sub>

                                    <!--<v-text-field v-if="item.AnswerType=='Single Line Text'" hide-details outlined dense label="Type your answer" v-model="item.Answer" />-->
                                    <div v-if="item.AnswerType=='Single Line Text' && (item.IsValidationQuestion) ">

                                        <v-text-field v-if="item.AnswerType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                      :type="item.qType" v-bind:maxlength="item.txtLength" :rules="item.qRules" :min="item.minVal" :max="item.maxVal"
                                                      @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)" />
                                    </div>
                                    <div v-if="item.AnswerType=='Single Line Text' && !(item.IsValidationQuestion) ">

                                        <v-text-field v-if="item.AnswerType=='Single Line Text'" full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                      @focus="onFocusCheckQstnValidation(item)" />

                                    </div>

                                    <!--Multiline Answer-->
                                    <!--<v-textarea v-if="item.AnswerType==='Multi Line Text'" hide-details outlined dense label="Type your answer" v-model="item.Answer" />-->

                                    <div v-if="item.AnswerType=='Multi Line Text' && (item.IsValidationQuestion) ">

                                        <v-textarea v-if="item.AnswerType=='Multi Line Text'"  hide-details outlined dense label="Type your answer" v-model="item.Answer"
                                                      :type="item.qType" v-bind:maxlength="item.txtLength" :rules="item.qRules" :min="item.minVal" :max="item.maxVal"
                                                      @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)" />
                                    </div>
                                    <div v-if="item.AnswerType=='Multi Line Text' && !(item.IsValidationQuestion) ">

                                        <v-textarea v-if="item.AnswerType=='Multi Line Text'"  hide-details outlined dense label="Type your answer" v-model="item.Answer"
                                                      @focus="onFocusCheckQstnValidation(item)" />

                                    </div>


                                    <!--Single Choice Answer-->
                                    <div v-if="item.AnswerType==='Single Choice'" class="mt-0">

                                        <v-radio-group v-model="item.Answer">
                                            <v-radio hide-details v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                     :label="optItem.OptionValue"
                                                     :value="optItem.OptionValue"></v-radio>
                                        </v-radio-group>

                                    </div>
                                    <!--Multiple Choice -->
                                    <div v-if="item.AnswerType==='Multi-Choice'" class="mt-0">
                                        <div v-if="item.BTQstnAnsOptionList.length>0">
                                            <v-checkbox hide-details input-value="true" :label="optItem.OptionValue" v-for="(optItem,ind) in item.BTQstnAnsOptionList" :key="ind"
                                                        v-model="optItem.IsOptSelected" @change="GetMultiChoice(optItem,optItem.IsOptSelected,optItem.OptionValue,item.QuestionID,ind)"></v-checkbox>

                                        </div>
                                    </div>
                                    <!--Dropdown -->
                                    <div v-if="item.AnswerType==='Dropdown'" class="mt-0">
                                        <div v-if="item.BTQstnAnsOptionList.length>0">
                                            <v-select dense :items="item.BTQstnAnsOptionList" :menu-props="{ maxHeight: '300' }" filled
                                                      item-text="OptionValue"
                                                      label="Select" outlined hide-details v-model="item.Answer">
                                            </v-select>
                                        </div>
                                    </div>
                                    <!--Numeric-->
                                    <div v-if="item.AnswerType==='Numeric'">
                                        <v-text-field v-if="item.AnswerType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="item.Answer" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;" :min="item.minVal" :max="item.maxVal" :rules="item.qRules"
                                                      @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)">
                                        </v-text-field>
                                    </div>
                                    <!--Cascading Dropdown-->
                                    <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                        <v-row>
                                            <v-col>
                                                <!--<v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="vmCascadingOptionItem[{index:index,optIndexVal:optItem}]"-->
                                                <v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                          :label="` ${optItem.OptionValue}`" outlined=""
                                                          v-for="(optItem,ind) in item.BTQstnAnsCascadingOptionList" :key="ind"
                                                          :items="optItem.BTQstnAnsCascadingListItems"
                                                          item-text="Name"
                                                          item-value="ID" @change="FilterSubItem(item,optItem,optItem[ind],ind)">
                                                </v-select>

                                            </v-col>
                                        </v-row>
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
                                    <div v-show="item.AnswerType==='Photo Capture'">

                                        <v-radio-group row v-model="audUpload[index]" @change="item.IsUpload=audUpload[index]">
                                            <v-radio label="Photo Capture" hide-details>
                                            </v-radio>
                                            <v-radio label="Photo Upload" hide-details>
                                            </v-radio>
                                        </v-radio-group>
                                        <div v-show="item.IsUpload===0">
                                            <v-row>
                                                <v-col md="12">

                                                    <v-btn @click="item.startCamera =true,item.camOn=true,onStart()" color="primary" depressed class="mr-4">Start</v-btn>
                                                    <v-btn id="AutoTest_IP375" class="mr-4" color="secondary" depressed @click="onCapture(item.QuestionID,1),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn">Capture</v-btn>
                                                    <v-btn @click="onStop(),item.startCamera =false,item.camOn=false" v-bind:disabled="!item.camOn" depressed>Stop</v-btn>
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
                                                                 @camera-change="onCameraChange"
                                                                 :autoplay="item.camOn" />
                                                    <!--<v-btn id="AutoTest_IP375" color="secondary" depressed @click="onCapture(item.QuestionID,1)">Capture</v-btn>-->
                                                </v-col>
                                                <v-col md="4" class="pt-4 mt-3">
                                                    <figure class="figure">
                                                        <img :src="item.img" class="full-size" />
                                                    </figure>
                                                </v-col>
                                            </v-row>

                                        </div>
                                        <div v-show="item.IsUpload===1">

                                            <v-row>
                                                <v-col md="8">
                                                    <v-file-input label="Upload Image" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                                  @change="AnsBlobQID=item.QuestionID,SaveFile('Photo',item.AnsBlob,1)"></v-file-input>
                                                </v-col>
                                                <v-col md="4" class="pt-4 mt-3">
                                                    <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                </v-col>

                                            </v-row>
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
                                                            @click.native="AnsBlobQID=item.QuestionID" />

                                            <!--<h3 class="text-center mt-4">OR</h3></br>-->

                                        </div>
                                        <div v-show="item.IsUpload===1">
                                            <v-file-input label="Upload Audio" outlined dense accept="audio/*" v-model="item.AnsBlob"
                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Audio',item.AnsBlob,1)"></v-file-input>

                                        </div>
                                    </div>
                                    <!--Signature Capture-->
                                    <div v-if="item.AnswerType==='Signature Capture'" v-model="item.Answer">
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
                                                <v-btn id="AutoTest_IP376" color="secondary" depressed class="mt-2 mr-2" @click="saveSign(item.QuestionID,1)">Save</v-btn>
                                                <v-btn id="AutoTest_IP377" default depressed class="mt-2 mr-2" @click="clearSign">Clear</v-btn>
                                            </div>

                                        </div>
                                        <div v-show="item.IsUpload===1">
                                            <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Signature',item.AnsBlob,1)"></v-file-input>

                                        </div>
                                        <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                            <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>
                                        </v-card>
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
                                    <!--Note-->
                                    <div v-if="item.AnswerType==='Note'">
                                        <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea>
                                    </div>


                                </v-col>
                            </v-row>
                        </template>
                    </v-card-text>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Default-Button-Outlined id="AutoTest_IP378" class="float-right btn-122x36" title="Cancel" @click.native="CancelRegisterDialogue()"></Default-Button-Outlined>
                            <Secondary-Button id="AutoTest_IP379" class="float-right btn-122x36 mr-4" title="Save" @click.native="RegisterBeneficiary()"></Secondary-Button>
                        </v-col>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <v-snackbar v-model="snackbarBulkActions" top color="primary" v-if="isBulk && vmBeneficiary.length ==0">
                <template>
                    <div class="pa-0 ma-0">
                        <v-alert dense type="info" color="primary" class="pa-0 ma-0">* Please select Beneficiaries from the list</v-alert>
                    </div>
                </template>
            </v-snackbar>
            <!--__________________________________-->
            <!--Import and Register Dialog-->
            <v-dialog v-model="dlgImportRegisterBeneficiary" width="800" persistent>
                <v-card class="elevation-0" width="100%" style="overflow:hidden;">
                    <v-card-title primary-title="" class="page-head pa-4 pb-0">
                        <v-avatar color="white" size="25" class="elevation-1">
                            <v-icon size="20" color="primary">mdi-database</v-icon>
                        </v-avatar>Import & Register
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP380" text @click="dlgImportRegisterBeneficiary=false,CancelImport()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-divider class="mt-3"></v-divider>
                    <v-card-text class="pa-3 v-scrolling-div" style="54vh">
                        <v-stepper v-model="ImportRegBenefSteps" class="elevation-0 py-0">
                            <v-row :justify="justifyCenter">
                                <v-col md="6" class="px-6 pa-0">
                                    <v-stepper-header class="elevation-0">
                                        <v-stepper-step :complete="ImportRegBenefSteps > 1" step="1" color="inherit">Upload </v-stepper-step>
                                        <v-divider></v-divider>
                                        <v-stepper-step :complete="ImportRegBenefSteps > 2" step="2" color="inherit">Format Check</v-stepper-step>
                                    </v-stepper-header>
                                </v-col>
                            </v-row>
                            <v-stepper-items>
                                <v-stepper-content step="1" class="px-3 py-0">
                                    <v-container fluid class="pa-0">
                                        <v-row no-gutters class="uploadBoxHeight pa-0">
                                            <v-col cols="12" md="12" class="pa-0">
                                                <h4 class="text-center heading-3">Step 1: Upload your CSV file set create a new beneficiary.</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="text-center">
                                                <v-card class="elevation-0 v-scrolling-div uploadScrollHeight">
                                                    <v-card-text class="py-0">
                                                        <v-row no-gutters>
                                                            <v-col>
                                                                <v-avatar size="100" tile>
                                                                    <img src="../../images/Import-icon.svg" alt="Import" />
                                                                </v-avatar>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row no-gutters>
                                                            <v-col>
                                                                <p class="text-medium">
                                                                    Your CSV file must map all our mandatory fields</br>and your file cannot exceed more than 1000 rows
                                                                </p>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row>
                                                            <v-col>
                                                                <v-btn id="AutoTest_IP381" color="grey" outlined dark class="elevation-0" height="40" @click="DownloadSampleFile()">
                                                                    <v-icon size="26" color="grey">mdi-file-download-outline</v-icon>Download an example file
                                                                </v-btn>
                                                            </v-col>
                                                            <v-col align-center justify-center>
                                                                <v-file-input prepend-icon="" label="Upload File" outlined dense
                                                                                id="file" v-model="fileInput"
                                                                                prepend-inner-icon="mdi-file-upload-outline" accept=".csv,x-csv,application/vnd.ms-excel,text/plain"
                                                                                @change="OnFileUpload($event)">
                                                                </v-file-input>
                                                            </v-col>
                                                        </v-row>

                                                    </v-card-text>
                                                </v-card>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>
                                <v-stepper-content step="2" class="pa-0 px-3">
                                    <v-row no-gutters class="pa-0">
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0 pb-3">
                                            <h4 class="heading-3">We are running a format check this will take a moment</h4>
                                        </v-col>

                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-center pa-0 px-sm-3">
                                            <vue-excel-editor no-paging :resetOnClick="resetOnClick" disable-panel-setting disable-panel-filter
                                                                v-model="lstImportBeneficiary">
                                                <div v-for="Hearder in lstImportBeneficiaryHearder">
                                                    <vue-excel-column :field="Hearder.text" :type="Hearder.type" :mandatory="Hearder.mandatory" :options="Hearder.options" :validate="onBeforeNameChange" width="80px" />
                                                </div>
                                            </vue-excel-editor>
                                        </v-col>
                                    </v-row>
                                </v-stepper-content>
                                <v-stepper-content step="3" class="pa-0">
                                    <v-container fluid>
                                        <v-row>
                                            <v-col md="12">
                                                <h4 class="text-center heading-3">Super !</h4>
                                            </v-col>
                                            <v-col md="12" class="text-center">
                                                <v-avatar size="150" tile="">
                                                    <img src="../../images/Invite-Sent-icon.svg" alt="success mail" />
                                                </v-avatar>
                                            </v-col>
                                            <v-col md="12">
                                                <p class="text-medium text-center">
                                                    Your Beneficiary has been created successfully. Bask in your glory !
                                                </p>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-stepper-content>
                            </v-stepper-items>
                        </v-stepper>
                    </v-card-text>
                    <v-card-actions class="pb-4 pr-4">
                        <v-row class="my-0 py-0">
                            <template v-if="ImportRegBenefSteps == 1">
                                <!-- step1 -->
                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                    <Default-Button-Outlined id="AutoTest_IP382" class="float-right btn-122x36" title="Cancel" @click.native="CancelImport()"></Default-Button-Outlined>
                                    <Secondary-Button id="AutoTest_IP383" class="mr-2 float-right btn-122x36" title="Next" @click.native="(ImportRegBenefSteps = 2)" v-bind:disabled="lstImportBeneficiary.length == 0"></Secondary-Button>
                                </v-col>
                            </template>
                            <template v-if="ImportRegBenefSteps == 2">
                                <!-- step 2 -->
                                <v-col cols="12" xs="12" sm="12" md="12" class="text-center text-md-right py-0 my-0">
                                    <Default-Button-Outlined id="AutoTest_IP384" class="float-right btn-122x36" title="Cancel" @click.native="CancelImport()"></Default-Button-Outlined>
                                    <Secondary-Button id="AutoTest_IP385" class="mr-2 float-right btn-122x36" v-bind:disabled="IsvalidexcelData == false" title="Next" @click.native="ImportRegisterBeneficiary()"></Secondary-Button>
                                </v-col>
                            </template>
                            <template v-if="ImportRegBenefSteps == 3">
                                <!-- step 3 -->
                                <v-col md="12" class="text-center">
                                    <Secondary-Button id="AutoTest_IP386" class="btn-122x36" title="Done" @click.native="ImportRegBenefSteps = 1, dlgImportRegisterBeneficiary = false"></Secondary-Button>
                                </v-col>
                            </template>
                        </v-row>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <!--_______________________________________________________________-->
            <!--Edit Beneficiary Dialog-->
            <v-dialog v-model="EditBeneficiaryDialog" width="800" persistent="">
                <v-card>
                    <v-card-title primary-title="" class="page-head pb-0">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-database</v-icon>
                        </v-avatar>
                        Edit Beneficiary
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP387" text="" @click="EditBeneficiaryDialog=false,GetAllBeneficiaries(),startCamera =false,camOn=false,CancelRegisterDialogue()">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="v-scrolling-div" style="max-height:60vh">
                        <h4 class="text-center heading-3 my-4">Please edit basic details of beneficiary.</h4>

                        <template v-for="(item,index) in lstBeneficiaryDetails">
                            <v-row justify="center">
                                <v-col md="6">
                                    <p class="black--text">{{index +1}}.{{item.QuestionName}}</p>
                                    <sub style="color:red;font-size:smaller" class="mr-2 ml-2" v-if="item.QuestionHint.length > 0 && item.QuestionHint != null && item.QuestionHint != 'null'">{{item.QuestionHint}}</sub>
                                    <sub style="color:red;font-size:smaller" v-if="item.IsValidationError">{{item.ValidationErrorTest}}</sub>
                                    <sub style="color:red;font-size:smaller" v-if="item.IsUniqueValidation">{{item.UniqueErrorTest}}</sub>

                                    <!--<v-text-field v-if="item.AnswerType=='Single Line Text'" outlined dense label="Type your answer" v-model="item.Answer" />-->
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

                                    <div v-if="item.AnswerType=='Multi Line Text' && (item.IsValidationQuestion) ">

                                        <v-textarea v-if="item.AnswerType=='Multi Line Text'" hide-details full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                    :type="item.qType" v-bind:maxlength="item.txtLength" :rules="item.qRules" :min="item.minVal" :max="item.maxVal"
                                                    @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)" />
                                    </div>
                                    <div v-if="item.AnswerType=='Multi Line Text' && !(item.IsValidationQuestion) ">

                                        <v-textarea v-if="item.AnswerType=='Multi Line Text'" hide-details full-width outlined dense label="Type your answer" v-model="item.Answer"
                                                    @focus="onFocusCheckQstnValidation(item)" />

                                    </div>


                                    <!--Single Choice Answer-->
                                    <div v-if="item.AnswerType==='Single Choice'" class="mt-0">

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
                                            <v-select dense :items="item.BenefAnswerOptions" :menu-props="{ maxHeight: '300' }" outlined hide-details
                                                      item-text="OptionValue"
                                                      label="Select" v-model="item.Answer">
                                            </v-select>
                                        </div>
                                    </div>
                                    <!--Cascading Dropdown -->
                                    <div v-if="item.AnswerType==='Cascading Dropdown'" class="mt-0">
                                        <div v-if="item.BenefAnswerOptions.length>0">
                                            <!--<v-select dense :items="item.BenefAnswerOptions" :menu-props="{ maxHeight: '300' }" outlined hide-details
                                                      item-text="OptionValue"
                                                      label="Select" outlined v-model="item.Answer">
                                            </v-select>-->
                                            <v-select dense :menu-props="{ maxHeight: '300' }" filled v-model="optItem[ind]"
                                                      :label="GetCascadingName(item.QuestionID,ind)" outlined=""
                                                      v-for="(optItem,ind) in item.BTQstnAnsCascadingOptionList " :key="ind"
                                                      :items="optItem.BTQstnAnsCascadingListItems"
                                                      item-text="Name"
                                                      item-value="ID" @change="EditCascadingFilterSubItem(item,optItem,optItem[ind],ind)">
                                            </v-select>
                                        </div>
                                    </div>
                                    <!--Numeric-->
                                    <div v-if="item.AnswerType==='Numeric'">
                                        <v-text-field v-if="item.AnswerType==='Numeric'" full-width outlined dense type="number" label="Type your answer" v-model="item.Answer" onkeydown="javascript: return (event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189)? false : true"
                                                      onKeyPress="if(this.value.length==10) return false;" :min="item.minVal" :max="item.maxVal" :rules="item.qRules"
                                                      @focus="onFocusCheckQstnValidation(item)" @change="onFocusCheckQstnValidation(item)">
                                        </v-text-field>
                                    </div>

                                    <!--GPS Capture-->
                                    <div v-if="item.AnswerType==='GPS Capture'" v-model="item.Answer">
                                        <label class="text-left heading-1 mt-3">{{item.Answer}}</label>
                                        <GmapMap :center="Coordinates"
                                                 :zoom="13"
                                                 style="width: 100%; height: 300px" @click.native="GetGPSCoordinatesEdit(item.QuestionID)">
                                            <GmapMarker :position="item.Coordinates"
                                                        :clickable="true"
                                                        :draggable="true" @dragend="updateCoordinates($event.latLng, item)" />
                                        </GmapMap>
                                    </div>

                                    <!--Photo Capture-->
                                    <div v-show="item.AnswerType==='Photo Capture'">
                                        <div v-if="item.IsNew==0">
                                            <!--<div v-if="!IsNew[index]">-->
                                            <v-row>
                                                <v-col md="8">
                                                    <v-btn id="AutoTest_IP388" text="" height="25" min-width="25" max-width="25" color="#707070" @click="item.IsNew=1,devices=[],deviceId=null,camera=null">
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
                                                        <vue-web-cam ref="ewebcam" v-if="startCamera"
                                                                     :device-id="deviceId"
                                                                     width="100%"
                                                                     height="200px"
                                                                     @started="onStarted"
                                                                     @stopped="onStopped"
                                                                     @error="onError"
                                                                     @cameras="onCameras"
                                                                     @camera-change="onCameraChange" :autoplay="camOn" />
                                                        <!--<v-btn id="AutoTest_IP389" color="secondary" depressed @click="onCapture(item.QuestionID,2)">Capture</v-btn>-->

                                                    </v-col>
                                                    <v-col md="4">
                                                        <figure class="figure">
                                                            <img :src="item.img" class="full-size" />
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
                                                        <v-img :src=item.uploadedImg width="250" height="180" aspect-ratio="1.7" class="center-block my-2"></v-img>
                                                    </v-col>
                                                </v-row>
                                            </div>
                                        </div>


                                    </div>
                                    <!--Audio Capture-->
                                    <div v-if="item.AnswerType==='Audio Capture'">
                                        <audio class="mt-0 mb-6" controls v-if="item.AnswerType==='Audio Capture'">
                                            <source :src="fileUrl+item.Answer" type="audio/mp3">
                                        </audio>
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
                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Audio',item.AnsBlob,2)"></v-file-input>

                                        </div>



                                    </div>
                                    <!--Signature Capture-->
                                    <div v-if="item.AnswerType==='Signature Capture'" v-model="item.Answer">
                                        <div>
                                            <v-card v-if="item.Answer != 'null' && item.Answer != '' && item.uploadedSign == ''">
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
                                                <v-btn id="AutoTest_IP390" color="secondary" depressed class="mt-2 mr-2" @click="saveSign(item.QuestionID,2)">Save</v-btn>
                                                <v-btn id="AutoTest_IP391" default depressed class="mt-2 mr-2" @click="clearSign">Clear</v-btn>
                                            </div>
                                            <!--<h4 class="text-center">OR</h4>-->
                                        </div>
                                        <div v-show="item.IsUpload===1">
                                            <!--<v-file-input accept="image/*" outlined dense label="Upload Signature"></v-file-input>-->
                                            <v-file-input label="Upload Signature" outlined dense accept="image/*" v-model="item.AnsBlob"
                                                          @change="AnsBlobQID=item.QuestionID,SaveFile('Signature',item.AnsBlob,2)"></v-file-input>

                                        </div>
                                        <v-card v-if="item.uploadedSign !=null && item.uploadedSign != '' ">
                                            <v-img :src=item.uploadedSign max-width="50%" height="150" class="my-4" aspect-ratio="1.7"></v-img>

                                        </v-card>
                                    </div>
                                    <!--Date Picker-->
                                    <div v-if="item.AnswerType==='Date Picker'">

                                        <v-menu v-model="item.mUDate"
                                                :close-on-content-click="false"
                                                :nudge-right="40"
                                                transition="scale-transition"
                                                offset-y=""
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="item.Answer"
                                                              label="Select Date"
                                                              prepend-inner-icon="mdi-calendar"
                                                              outlined
                                                              readonly hide-details
                                                              dense
                                                              v-on="on"></v-text-field>
                                            </template>
                                            <v-date-picker v-model="item.Answer" no-title="" @input="item.mUDate = false" format="DD-MM-YYYY" @change="onFocusCheckQstnValidation(item)"></v-date-picker>
                                        </v-menu>
                                    </div>
                                    <!--Note-->
                                    <div v-if="item.AnswerType==='Note'">
                                        <v-textarea outlined dense hide-details v-model="item.Answer"></v-textarea>
                                    </div>
                                </v-col>
                            </v-row>
                        </template>
                    </v-card-text>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Default-Button-Outlined id="AutoTest_IP392" class="float-right btn-122x36" title="Cancel" @click.native="EditBeneficiaryDialog=false,GetAllBeneficiaries(),CancelRegisterDialogue()"></Default-Button-Outlined>
                            <Secondary-Button id="AutoTest_IP393" class="float-right btn-122x36 mr-4" title="Save" @click.native="UpdateBeneficiary()"></Secondary-Button>
                        </v-col>
                    </v-card-actions>
                </v-card>
            </v-dialog>

            <!--__________________________________-->
            <!-- Remove Beneficiary Type-->
            <v-dialog v-model="dlgRemoveBeneficiary" width="800">
                <v-card>
                    <v-card-title primary-title class="page-head py-4">
                        <v-avatar color="white" size="25" class="elevation-1 mr-2">
                            <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                        </v-avatar>
                        Remove Beneficiary
                        <v-spacer></v-spacer>
                        <v-btn id="AutoTest_IP394" text="" @click="dlgRemoveBeneficiary=false,vmBeneficiary=[],isBulk=false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <v-card-text class="pt-0">
                        <v-container fluid class="pt-4">
                            <v-row no-gutters>
                                <v-col cols="12" md="12" class="pt-0">
                                    <v-container fluid class="pt-0">
                                        <v-row>

                                            <v-col cols="12" md="12" class="pt-0" v-if="!isBulk">
                                                <h4 class="text-center heading-3">Are you sure you want to remove this beneficiary ?</h4>
                                            </v-col>
                                            <v-col cols="12" md="12" class="pt-0" v-if="isBulk">
                                                <h4 class="text-center heading-3">Are you sure you want to remove the selected beneficiaries ?</h4>
                                            </v-col>
                                            <v-col v-if="!isBulk">
                                                <h4 class="text-center heading-3">
                                                    <v-icon class="tmr-2" color="#707070" size="25">mdi-database</v-icon>
                                                    {{BenefName}}
                                                </h4>
                                            </v-col>

                                            <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                                <Secondary-Button id="AutoTest_IP395" class="btn-122x36 mr-4" title="Remove" @click.native="RemoveBeneficiary(isBulk)"></Secondary-Button>
                                                <Default-Button-Outlined id="AutoTest_IP396" class="btn-122x36" title="Cancel" @click.native="dlgRemoveBeneficiary = false,vmBeneficiary=[],isBulk=false"></Default-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
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
    </div>
</template>
<script>
import SecondaryButton from "@/components/secondary-button.vue";
import DefaultButtonOutlined from "@/components/default-button-outlined.vue";
import { WebCam } from "vue-web-cam";
import PreLoader from "@/components/pre-loader.vue";
import objUtils from '../../utils.js';

var objPB;
async function importscript() {
  return Promise.all([
    import("../../BL/ProjectBeneficiary.js").then((mod) => {
      objPB = new mod.ProjectBeneficiary();
    }),
  ]);
}
export default {
  data() {
    return {

      resetOnClick:0,
      abc: 1,
      screenAccess: 0,
      RWAccess: 0,
      PreLoader: false,
      snackbarBulkActions: false,
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
      startCamera: false,
      camOn: false,
      autoPlay: false,

      EditBeneficiaryDialog: false,
      searchBeneficiary: "",

      RegBeneficiaryDialog: false,
      successSnackbarMsg: false,
      errorSnackbarMsg: false,
      infoSnackbarMsg: false,
      dlgImportRegisterBeneficiary: false,
      justifyCenter: "center",

      ImportRegBenefSteps: 1,

      BenTableheaders: [
        { text: "Name", value: "BeneficiaryName" },
        { text: "Created Date", value: "CreatedDate" },
        { text: "Last Modified", value: "UpdatedDate" },
        { text: "Action",align:"center", value: "BenActions",width:"100px" },
      ],

      dlgRemoveBeneficiary: false,

      BenefTID: 0,
      WorkspaceID: 0,
      InitiativeID: 0,
      ProjectID: 0,
      lstBeneficiaryTypeDetails: [],
      lstAllBeneficiaries: [],
      vmBeneficiary: [],
      CountOfCountrySelected: 0,
      CountOfStateSelected: 0,
      CountOfDistrictSelected: 0,
      CountOfSubDistSelected: 0,

      Msg: "",
      lstBTQuestions: [],
      benefTypeName: "",
      vmDate: new Date().toISOString().substr(0, 10),
      vmUDate: new Date().toISOString().substr(0, 10),
      mDate: "",
      mUDate: "",
      fileInput: null,
      lstImportBeneficiaryHearder: [],
      lstImportBeneficiary: [],
      csvFileName: "",
      isBulk: false,
      removeList: [],
      lstBeneficiaryDetails: [],
      multiChoiceAns: [],
      headers: {
        "X-Custom-Header": "some data",
      },
      blobFileInput: null,
      BenefName: "",
      AnsBlob: [], //{ QuestionID: 0, blobAudio: "" },
      AnsBlobQID: "",
      audUpload: [],
      blobFilename: "",
      fileUrl: "",
      delList: [],
      cascadingAns: [],
      uploadedImg: "",
     uploadedSign:"",
    ValidationErrorTest:"",
    UniqueErrorTest:"",
    savedAudio:"",

    qType: 'text',
    qRules: [],
    txtLength: 10000,
    IsvalidexcelData:true,
    //=========validation========= //
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
    };
  },

  created() {
    //GPS Capture
    this.$getLocation({}).then((Coordinates) => {
      this.Coordinates = Coordinates;
    });
  },

  components: {
    "Secondary-Button": SecondaryButton,
    "Default-Button-Outlined": DefaultButtonOutlined,
    "vue-web-cam": WebCam,
    "Pre-Loader": PreLoader,
  },
  computed: {
    //Web Cam Functions
    device: function () {
      return this.devices.find((n) => n.deviceId === this.deviceId);
    },

    computedDate(index) {
      return this.formatDate(this.vmDate);
    },
    computedUDate(index) {
      return this.formatDate(this.vmUDate);
    },
  },
  mounted: async function () {
    this.WorkspaceID = this.$route.query.WsID;
    this.RWAccess = this.$route.query.Access;
    this.screenAccess = this.$route.query.Access == 3 ? true : false;
    this.InitiativeID = this.$route.query.InitID;
    this.ProjectID = this.$route.query.ProjID;
    this.BenefTID = this.$route.query.BenfTID;
    this.fileUrl = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.$route.query.ProjID.toString() + "/";
    await importscript();
    this.PreLoader = true;
    await this.GetBeneficiaryTypeDetails();
    await this.GetAllBeneficiaries();
    this.PreLoader = false;
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
  },

  methods: {
      /// <summary>
      ///  EditCascadingFilterSubItem :- Function used to edit cascading filter sub items when changing the option in edit mode
      /// </summary>
      /// <param name="QItem"></param>
      /// <param name="OptItem"></param>
      /// <param name="parentID"></param>
      /// <param name="edIndx"></param>
      /// <returns> </returns>
      async EditCascadingFilterSubItem(QItem, OptItem, parentID, edIndx) {

          var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
         // this.cascadingAns[edIndx] = selectedOption;
             this.cascadingAns.filter(x=> x.QuestionID == QItem.QuestionID)[0].selectedOption[edIndx] = selectedOption;

          var nextHierarchyOrder = OptItem.HierarchyOrder + 1;
          var IndexOrder =   edIndx+1;
          if (this.lstBeneficiaryDetails.filter((x) => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList.length >= IndexOrder) {
              var data = new FormData();
              data.append("optionValue", OptItem.OptionValue);
              data.append("parentID", parentID);
              var result = await objPB.GetCascadingOptionsList(data);

              if (result.status == "Unhandled Exception")
                  this.$router.push({ name: "error" });
              else if (result.data)
                  var sublist = result.data;

              this.lstBeneficiaryDetails.filter((x) => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList .filter(
                 (x) => x.HierarchyOrder == nextHierarchyOrder)[0].BTQstnAnsCascadingListItems = sublist[0].BTQstnAnsCascadingListItems;
             // this.cascadingAns[edIndx + 1] = "Select ";
                this.cascadingAns.filter(x=> x.QuestionID == QItem.QuestionID)[0].selectedOption[edIndx + 1] = "Select ";
          }
      },

      /// <summary>
      ///  FilterSubItem :- Function used to add cascading filter sub items while register beneficiary
      /// </summary>
      /// <param name="QItem"></param>
      /// <param name="OptItem"></param>
      /// <param name="parentID"></param>
      /// <returns> </returns>
      async FilterSubItem(QItem, OptItem, parentID, CurrIndex) {
          if (OptItem.OptionValue == 'Country' && this.CountOfCountrySelected == 0) {
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue});
              this.CountOfCountrySelected += 1;
          }
          if (OptItem.OptionValue == 'States' && this.CountOfStateSelected == 0) {
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
              this.CountOfStateSelected += 1;
          }
          if (OptItem.OptionValue == 'District' && this.CountOfDistrictSelected == 0) {
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
              this.CountOfDistrictSelected += 1;
          }
          if (OptItem.OptionValue == 'SubDistrict' && this.CountOfSubDistSelected == 0) {
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
              this.CountOfSubDistSelected += 1;
          }
          if (OptItem.OptionValue == 'Country' && this.CountOfCountrySelected != 0) {
              const index = this.cascadingAns.findIndex((element, index) => {
                  if (element.QuestionID === QItem.QuestionID && element.OptionValue == OptItem.OptionValue) {
                      return true;
                  }
              });
              this.cascadingAns.splice(index, 1);
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
          }
          if (OptItem.OptionValue == 'States' && this.CountOfStateSelected != 0) {
              const index = this.cascadingAns.findIndex((element, index) => {
                  if (element.QuestionID === QItem.QuestionID && element.OptionValue == OptItem.OptionValue) {
                      return true;
                  }
              });
              this.cascadingAns.splice(index, 1);
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue }); 
          }
          if (OptItem.OptionValue == 'District' && this.CountOfDistrictSelected != 0) {
              const index = this.cascadingAns.findIndex((element, index) => {
                  if (element.QuestionID === QItem.QuestionID && element.OptionValue == OptItem.OptionValue) {
                      return true;
                  }
              });
              this.cascadingAns.splice(index, 1);
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
          }
          if (OptItem.OptionValue == 'SubDistrict' && this.CountOfSubDistSelected != 0) {
              const index = this.cascadingAns.findIndex((element, index) => {
                  if (element.QuestionID === QItem.QuestionID && element.OptionValue == OptItem.OptionValue) {
                      return true;
                  }
              });
              this.cascadingAns.splice(index, 1);
              var selectedOption = OptItem.BTQstnAnsCascadingListItems.filter((x) => x.ID == parentID)[0].Name;
              this.cascadingAns.push({ selectedOption: selectedOption, QuestionID: QItem.QuestionID, OptionValue: OptItem.OptionValue });
          }

          var nextHierarchyOrder = OptItem.HierarchyOrder + 1;
          var IndexOrder =   CurrIndex+1;
          if (this.lstBTQuestions.filter((x) => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList.length >= IndexOrder) {
              var data = new FormData();
              data.append("optionValue", OptItem.OptionValue);
              data.append("parentID", parentID);
              var result = await objPB.GetCascadingOptionsList(data);

              if (result.status == "Unhandled Exception")
                  this.$router.push({ name: "error" });
              else if (result.data)
                  var sublist = result.data;

              if (sublist[0].BTQstnAnsCascadingListItems != undefined) {
                  this.lstBTQuestions.filter((x) => x.QuestionID == QItem.QuestionID)[0].BTQstnAnsCascadingOptionList.filter(
                      (x) => x.HierarchyOrder == nextHierarchyOrder)[0].BTQstnAnsCascadingListItems = sublist[0].BTQstnAnsCascadingListItems;
              }
          }
      },

      /// <summary>
      ///  formatDate :- Function used to formatting a date
      /// </summary>
      /// <param name="date"></param>
      /// <returns> </returns>
      formatDate(date) {
          return objUtils.common.formatDate(date);
      },

      /// <summary>
      ///  GetBeneficiaryTypeDetails :- Function used to fetch individual beneficiary type details based on BeneficiaryTypeID
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async GetBeneficiaryTypeDetails() {
          var colValue = "";
          this.lstBTQuestions=[];
          this.lstImportBeneficiaryHearder = [];
          var result = await objPB.GetBeneficiaryTypeDetails(this.BenefTID);
          if (result.status == "Unhandled Exception")
              this.$router.push({ name: "error" });
          else if (result.data) {
              this.lstBeneficiaryTypeDetails = result.data;
              this.benefTypeName = this.lstBeneficiaryTypeDetails[0].Name;
              for (var i = 0; i < this.lstBeneficiaryTypeDetails[0].BTQuestions.length; i++) {
                  var BTQuestion=this.lstBeneficiaryTypeDetails[0].BTQuestions[i];

                  this.lstBTQuestions.push({
                      QuestionID:BTQuestion.QuestionID,
                      QuestionName:BTQuestion.QuestionName,
                      AnswerTypeID:BTQuestion.AnswerTypeID,
                      AnswerType:BTQuestion.AnswerType,
                      Answer:BTQuestion.Answer,
                      IsUnique:BTQuestion.IsUnique,
                      IsSearchable:BTQuestion.IsSearchable,
                      IsMandatoryResponse:BTQuestion.IsMandatoryResponse,
                      IsValidationQuestion :BTQuestion.IsValidationQuestion,
                      QuestionHint:BTQuestion.QuestionHint,
                      BeneficiaryTypeID:BTQuestion.BeneficiaryTypeID,
                      BeneficiaryID:BTQuestion.BeneficiaryID,
                      BTQstnAnsOptions:BTQuestion.BTQstnAnsOptions,
                      BTQstnAnsOptionList:BTQuestion.BTQstnAnsOptionList,
                      BTQstnAnsCascadingOptionList:BTQuestion.BTQstnAnsCascadingOptionList,
                      IsRegistrationDone:BTQuestion.IsRegistrationDone,
                      BTQstnValidationList :BTQuestion.BTQstnValidationList,
                      IsUpload :0,
                      IsNew : 0,
                      startCamera:false,
                      camOn:false,
                      img:'',
                      uploadedImg:'',
                      uploadedSign:'',
                      ValidationErrorTest:'',
                      IsValidationError:false,
                      UniqueErrorTest:'',
                      IsUniqueValidation:false,
                      Coordinates:[{lat: 0,lng: 0,}],
                      qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules:[],
                  });
                  this.audUpload[i] = 0;
                  //this.IsNew[i] = false;
                  colValue = "Answer" + (i + 1).toString();
                  if(this.lstBTQuestions[i].AnswerType =="Date Picker") {
                      this.lstImportBeneficiaryHearder.push({ text: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                          value: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                          type:"date", mandatory:this.lstBTQuestions[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                          QuestionCondition:this.lstBTQuestions[i].BTQstnValidationList,
                      });
                  }
                  else if(this.lstBTQuestions[i].AnswerType =="Numeric") {
                       this.lstImportBeneficiaryHearder.push({ text: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           value: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           type:"number", mandatory:this.lstBTQuestions[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                           QuestionCondition:this.lstBTQuestions[i].BTQstnValidationList,
                       });
                  }
                  else if(this.lstBTQuestions[i].BTQstnAnsOptionList.length>0) {
                       var optionlist=[];
                       for (var j = 0; j < this.lstBTQuestions[i].BTQstnAnsOptionList.length; j++) {
                           optionlist.push(this.lstBTQuestions[i].BTQstnAnsOptionList[j].OptionValue)
                       }
                       this.lstImportBeneficiaryHearder.push({
                           text: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           value: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           type:"select",
                           mandatory:this.lstBTQuestions[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                           QuestionCondition:this.lstBTQuestions[i].BTQstnValidationList,
                           options:optionlist,
                       });
                  }
                  else {
                       this.lstImportBeneficiaryHearder.push({
                           text: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           value: this.lstBTQuestions[i].QuestionName.concat(' ',this.lstBTQuestions[i].QuestionHint),
                           type:"string",
                           mandatory:this.lstBTQuestions[i].IsMandatoryResponse == 1 ? 'Yes' : 'No',
                           QuestionCondition:this.lstBTQuestions[i].BTQstnValidationList,
                       });
                  }
              }
          }
      },

      /// <summary>
      ///  GetAllBeneficiaries :- Function used to fetch all beneficiaries based on BeneficiaryTypeID
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async GetAllBeneficiaries() {
          var data = new FormData();
          data.append("BeneficiaryTypeID", this.BenefTID);
          var result = await objPB.GetAllBeneficiaries(this.BenefTID); //await objPB.GetAllBeneficiaries(data);

          if (result.status == "Unhandled Exception")
              this.$router.push({ name: "error" });
          else if (result.data)
              this.lstAllBeneficiaries = result.data;
      },

      /// <summary>
      ///  GetBeneficiaryDetails :- Function used to fetch beneficiary details for edit based on BeneficiaryID
      /// </summary>
      /// <param name="BenefID"></param>
      /// <returns> </returns>
      async GetBeneficiaryDetails(BenefID) {
          this.PreLoader = true;
          var AnsOptions = [];
          this.lstBeneficiaryDetails=[];
          this.multiChoiceAns =[];
          var result = await objPB.GetBeneficiaryDetails(BenefID);

          if (result.status == "Unhandled Exception")
              this.$router.push({ name: "error" });
          else if (result.data)
              var BeneficiaryDetails = result.data;
          for (var i = 0; i < BeneficiaryDetails.length; i++) {
              this.lstBeneficiaryDetails.push({
                  QuestionID:BeneficiaryDetails[i].QuestionID,
                  QuestionName:BeneficiaryDetails[i].QuestionName,
                  AnswerTypeID:BeneficiaryDetails[i].AnswerTypeID,
                  AnswerType:BeneficiaryDetails[i].AnswerType,
                  Answer:BeneficiaryDetails[i].Answer,
                  IsUnique:BeneficiaryDetails[i].IsUnique,
                  IsSearchable:BeneficiaryDetails[i].IsSearchable,
                  IsMandatoryResponse:BeneficiaryDetails[i].IsMandatoryResponse,
                  QuestionHint:BeneficiaryDetails[i].QuestionHint,
                  IsValidationQuestion :BeneficiaryDetails[i].IsValidationQuestion,
                  BeneficiaryTypeID:BeneficiaryDetails[i].BeneficiaryTypeID,
                  BeneficiaryID:BeneficiaryDetails[i].BeneficiaryID,
                  BTQstnAnsOptions:BeneficiaryDetails[i].BTQstnAnsOptions,
                  BTQstnAnsOptionList:BeneficiaryDetails[i].BTQstnAnsOptionList,
                  BTQstnAnsCascadingOptionList:BeneficiaryDetails[i].BenefAnswerCascadingOptionList,
                  IsRegistrationDone:BeneficiaryDetails[i].IsRegistrationDone,
                  BenefAnswerOptions:BeneficiaryDetails[i].BenefAnswerOptions,
                  BTQstnValidationList :BeneficiaryDetails[i].BTQstnValidationList,
                  IsUpload :0,
                  IsNew : 0,
                  startCamera:false,
                  camOn:false,
                  img:'',
                  uploadedImg:'',
                  uploadedSign:'',
                  ValidationErrorTest:'',
                  UniqueErrorTest:'',
                  IsValidationError:false,
                  IsUniqueValidation:false,
                  Coordinates:[{lat: 0,lng: 0,}],
                  qType : "",QstType :"",textLength : 10,minVal : 0 ,maxVal : 9999999999,qRules:[],
              });

              this.audUpload[i] = 0;
              //this.IsNew[i] = false;

              if (this.lstBeneficiaryDetails[i].AnswerType == "Date Picker")
                  this.vmUDate = this.lstBeneficiaryDetails[i].Answer;
              if (this.lstBeneficiaryDetails[i].AnswerType == "Multi-Choice") {

                  if(this.lstBeneficiaryDetails[i].Answer != "") {
                      AnsOptions = this.lstBeneficiaryDetails[i].Answer.split(",");
                      //this.multiChoiceAns = AnsOptions;

                      for (var j = 0; j < AnsOptions.length; j++)
                      {
                          this.multiChoiceAns.push({OptionValue:AnsOptions[j], QuestionID : this.lstBeneficiaryDetails[i].QuestionID});
                          this.lstBeneficiaryDetails[i].BenefAnswerOptions.filter((x) => x.OptionValue == AnsOptions[j])[0].IsOptSelected = true;
                      }
                  }
              }
              if (this.lstBeneficiaryDetails[i].AnswerType == "Cascading Dropdown")
              {

                  AnsOptions = this.lstBeneficiaryDetails[i].Answer.split(",");
                  //this.cascadingAns.push({QuestionID : this.lstBeneficiaryDetails[i].QuestionID , SelectedOption :AnsOptions });
                    this.cascadingAns.push({selectedOption :AnsOptions, QuestionID : this.lstBeneficiaryDetails[i].QuestionID});
                  //this.cascadingAns = AnsOptions;
                  for (var j = 1; j < AnsOptions.length; j++) {
                      var pID = this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems.filter((x) => x.Name == AnsOptions[j])[0].ParentID;
                      var sublst = this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems.filter((x) => x.ParentID == pID);
                      this.lstBeneficiaryDetails[i].BTQstnAnsCascadingOptionList[j].BTQstnAnsCascadingListItems = sublst;
                  }
              }
          }
          this.PreLoader = false;
          this.EditBeneficiaryDialog = true;
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
    //===============Audio Capture=============//

      /// <summary>
      ///  callback :- Function used to callback fires after click the record button/after pause recording/before uploading/after successful uploading/after failure uploading
      /// </summary>
      /// <param name="data"></param>
      /// <returns> </returns>
      callback(data) {
          console.debug(data);
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

      /// <summary>
      ///  playSound :- Function used to play audio
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async playSound() {
          if (this.savedAudio) {
              var audio = new Audio(this.savedAudio);
              audio.play();
          }
      },

    //===============Signature Capture=======================//

      /// <summary>
      ///  clearSign :- Function used to clear signature from signature pad
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      clearSign() {
          if(typeof this.$refs.signaturePad != "undefined" ) {
              this.$refs.signaturePad[0].signaturePad.clear();
              this.$refs.signaturePad[0].signatureData = [];
          }
      },

      /// <summary>
      ///  saveSign :- Function used to save signature
      /// </summary>
      /// <param name="QID"></param>
      /// <param name="IsEdit"></param>
      /// <returns> </returns>
      saveSign(QID, IsEdit) {
          var signPad = "";
          this.AnsBlobQID = QID;
          if(IsEdit==2) {
              var prevAns = this.lstBeneficiaryDetails.filter((x) => x.QuestionID == this.AnsBlobQID)[0].Answer;
              if (prevAns != null)
                 this.delList.push({ QuestionID: QID, Answer: prevAns });
                 //Filter the Question Based on Signature Question Type
                    var SignatureQstnList  =  this.lstBeneficiaryDetails.filter(x => x.AnswerType == "Signature Capture")
                    // Getting the index of the signature question
                    const Qindex = SignatureQstnList.findIndex((element, index) => {
                          if (element.QuestionID === this.AnsBlobQID) {
                            return true
                          }
                      })
                    signPad = this.$refs.signaturePad[Qindex].signaturePad;

          }
          else
          {
              var prevAns = this.lstBTQuestions.filter((x) => x.QuestionID == this.AnsBlobQID)[0].Answer;
              if (prevAns != null)
                  this.delList.push({ QuestionID: QID, Answer: prevAns });
                  signPad = this.$refs.signaturePad[0].signaturePad;
          }

          //var signData = this.$refs.signaturePad[0].signatureData;

          if (signPad.isEmpty()) {
              this.Msg = "Please provide a signature first.";
              this.infoSnackbarMsg = true;
              return;
          } else {
              var dataURL = signPad.toDataURL("image/png");
              var data = dataURL.split(",")[1];
              let blobFile = this.b64toBlob(data, "image/png");
              this.SaveFile("Signature", blobFile, IsEdit);
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
          contentType = contentType || "";
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
      /// <param name="IsEdit"></param>
      /// <returns> </returns>
      onCapture(QID, IsEdit) {
          this.AnsBlobQID = QID;
          if (IsEdit == 2) {
              var prevAns = this.lstBeneficiaryDetails.filter((x) => x.QuestionID == this.AnsBlobQID)[0].Answer;
              if (prevAns != null)
                  this.delList.push({ QuestionID: QID, Answer: prevAns });
                  this.lstBeneficiaryDetails.filter(x=> x.QuestionID == QID)[0].img = this.$refs.ewebcam[0].capture();
                  this.img = this.$refs.ewebcam[0].capture();
          } else
          {
              this.lstBTQuestions.filter(x=> x.QuestionID == QID)[0].img = this.$refs.webcam[0].capture();
              this.img = this.$refs.webcam[0].capture();
          }
          var data = this.img.split(",")[1];
          let blobFile = this.b64toBlob(data, 'image/jpg"');
          this.SaveFile("Photo", blobFile, IsEdit);
          if (IsEdit == 1)
              this.onStop();
          else
              this.onEStop();
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
          this.$refs.webcam[0].stop();
      },

      /// <summary>
      ///  onStart :- Function used to start the camera after stopping it
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      onStart() {
          if(typeof this.$refs.webcam != "undefined" && this.$refs.webcam.length > 0) {
               this.$refs.webcam[0].start();
          }
      },

      /// <summary>
      ///  onEStop :- Function used to stop the camera in edit beneficiary dialog
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      onEStop() {
          this.$refs.ewebcam[0].stop();
      },

      /// <summary>
      ///  onEStart :- Function used to start the camera after stopping it in edit beneficiary dialog
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      onEStart() {
          this.$refs.ewebcam[0].start();
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
      /// <param name="IsEdit"></param>
      /// <returns> </returns>
      async SaveFile(fileType, blbFile, IsEdit) {
          var fname = "";
          var ansType = "";
          var timestamp = new Date().valueOf();

          var prevData =
          this.AnsBlob != [] ? this.AnsBlob.filter((x) => x.QuestionID == this.AnsBlobQID) : "";
          if (prevData.length > 0)
              this.AnsBlob.splice(this.AnsBlob.indexOf(this.AnsBlobQID), 1);

          if (fileType == "Audio") {
              fname = "Audio_" + timestamp + ".mp3"; // "Audio_" + blbFile.lastModified + ".mp3";
              ansType = "Audio Capture";
              this.savedAudio = window.URL.createObjectURL(blbFile);
          }
          if (fileType == "Signature") {
              fname = "Signature_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split(".").pop() : "jpg");
              ansType = "Signature Capture";
              const fr = new FileReader();
              fr.readAsDataURL(blbFile);
              if (IsEdit == 2) {
                  fr.addEventListener("load", () => {
                      //this.uploadedImg = fr.result;
                      this.lstBeneficiaryDetails.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedSign = fr.result;
                  });
              }
              else
              {
                  fr.addEventListener("load", () => {
                      this.lstBTQuestions.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedSign = fr.result;
                  });
              }
          }
          if (fileType == "Photo") {
              fname = "Photo_" + timestamp + "." + (blbFile.name != null ? blbFile.name.split(".").pop() : "jpg");
              ansType = "Photo Capture";
              const fr = new FileReader();
              fr.readAsDataURL(blbFile);
              if (IsEdit == 2)
              {
                  fr.addEventListener("load", () => {
                      //this.uploadedImg = fr.result;
                      this.lstBeneficiaryDetails.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedImg = fr.result;
                  });
              }
              else
              {
                  fr.addEventListener("load", () => {
                      // this.lstBTQuestions[1].uploadedImg = fr.result;
                      this.lstBTQuestions.filter(x => (x.QuestionID == this.AnsBlobQID))[0].uploadedImg = fr.result;
                  });
              }
          }

          this.AnsBlob.push({ QuestionID: this.AnsBlobQID, AnswerType: ansType, blobData: blbFile, blobFileName: fname, });
          this.lstBTQuestions.filter((x) => x.QuestionID == this.AnsBlobQID)[0].Answer = fname;
          if (IsEdit == 2)
              this.lstBeneficiaryDetails.filter((x) => x.QuestionID == this.AnsBlobQID)[0].Answer = fname;
      },

      /// <summary>
      ///  FileUpload :- Function used to upload file based on blobFileInput,ProjectID,FileName
      /// </summary>
      /// <param name="blobFileInput"></param>
      /// <param name="FileName"></param>
      /// <returns> </returns>
      async FileUpload(blobFileInput, FileName) {
          const formData = new FormData();
          formData.append("myFile", blobFileInput);
          formData.append("ProjectID", this.ProjectID);
          formData.append("FileName", FileName);
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
          this.lstBTQuestions.filter((x) => x.QuestionID == QID)[0].Answer = gpsCoordinates;
      },

      /// <summary>
      ///  GetGPSCoordinatesEdit :- Function used to get GPS coordinates in edit beneficiary
      /// </summary>
      /// <param name="QID"></param>
      /// <returns> </returns>
      GetGPSCoordinatesEdit(QID) {
          var gpsCoordinates = this.Coordinates.lat.toString() + "," + this.Coordinates.lng.toString();
          this.lstBeneficiaryDetails.filter((x) => x.QuestionID == QID)[0].Answer = gpsCoordinates;
      },
    //==================================================//

      /// <summary>
      ///  RegisterBeneficiary :- Function used to add beneficiary while register
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async RegisterBeneficiary() {
          if(await this.fnCheckQuestionValidationAndCondition(this.lstBTQuestions)==true)
	      {
              return false;
          }

          this.PreLoader = true;
          var lstQstnAnswers = [];
          var Ans = "";
          var blobFile = "";

          for (var i = 0; i < this.lstBTQuestions.length; i++) {
              Ans = "";
              if (this.lstBTQuestions[i].AnswerType == "Multi-Choice")
              {
                  var MultiChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == this.lstBTQuestions[i].QuestionID)
                  for (var j = 0; j < MultiChoiceAns.length; j++)
                      Ans = Ans != "" ? Ans + "," + MultiChoiceAns[j].OptionValue : MultiChoiceAns[j].OptionValue;
              }
              else if (this.lstBTQuestions[i].AnswerType == "Cascading Dropdown")
              {

                  var CasQuestionsAns = this.cascadingAns.filter(x=> x.QuestionID == this.lstBTQuestions[i].QuestionID)
                  for (var j = 0; j < CasQuestionsAns.length; j++)
                      Ans = Ans != "" ? Ans + "," + CasQuestionsAns[j].selectedOption: CasQuestionsAns[j].selectedOption;
              }
              else if (this.lstBTQuestions[i].AnswerType == "Audio Capture" || this.lstBTQuestions[i].AnswerType == "Signature Capture" || this.lstBTQuestions[i].AnswerType == "Photo Capture") {
                  if (this.AnsBlob.length != 0) {
                      var FileBlob=this.AnsBlob.filter((x) => x.QuestionID == this.lstBTQuestions[i].QuestionID)[0];
                      if(typeof FileBlob != "undefined"){
                          blobFile = FileBlob.blobData;
                          this.FileUpload(blobFile, this.lstBTQuestions[i].Answer);
                      }
                  }
                  this.lstBTQuestions[i].AnsBlob = null;
                  Ans = this.lstBTQuestions[i].Answer;
              } else
                  Ans = this.lstBTQuestions[i].Answer;
              lstQstnAnswers.push({
                  QuestionID: this.lstBTQuestions[i].QuestionID,
                  AnswerTypeID: this.lstBTQuestions[i].AnswerTypeID,
                  Answer: Ans,
              });
          }
          var data = new FormData();
          data.append("BeneficiaryTypeID", this.BenefTID);
          data.append("BeneficiaryName", lstQstnAnswers[0].Answer);
          data.append("BeneficiaryQstnAnswers", JSON.stringify(lstQstnAnswers));
          var result = await objPB.RegisterBeneficiary(data);
          if (result.status == "Unhandled Exception"){
              this.PreLoader = false;
              this.$router.push({ name: "error" });
          }
          else {
              if (result.status == "success") {
                  this.Msg = "Beneficiary Added Successfully ... ";
                  this.successSnackbarMsg = true;
                  this.GetAllBeneficiaries();
              } else {
                  this.Msg = "Failed in adding new Beneficiary Type ";
                  this.errorSnackbarMsg = true;
              }
              this.CancelRegisterDialogue();
              this.GetAllBeneficiaries();
          }
          this.PreLoader = false;
      },

      /// <summary>
      ///  CancelRegisterDialogue :- Function used to cancel register beneficiary dialog
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      CancelRegisterDialogue() {
          for (var i = 0; i < this.lstBTQuestions.length; i++)
          this.lstBTQuestions[i].Answer = "";
          this.RegBeneficiaryDialog = false;
          this.AnsBlob = [];
          this.AnsBlobQID = "";
          this.multiChoiceAns = [];
          this.cascadingAns = [];
          this.uploadedImg = "";
          this.uploadedSign="";
          this.img = null;
          this.startCamera = false;
          this.camOn = false;
          this.GetBeneficiaryTypeDetails();
          this.clearSign();
      },

      /// <summary>
      ///  RemoveBeneficiary :- Function used to delete beneficiary as bulk/individual based on BeneficiaryID
      /// </summary>
      /// <param name="isBulk"></param>
      /// <returns> </returns>
      async RemoveBeneficiary(isBulk) {
          if (!isBulk)
             this.removeList.push({
                 BeneficiaryID: this.BenefID,
                 BeneficiaryName: this.BenefName,
             });
          else
             for (var i = 0; i < this.vmBeneficiary.length; i++)
                this.removeList.push({
                    BeneficiaryID: this.vmBeneficiary[i].BeneficiaryID,
                    BeneficiaryName: this.vmBeneficiary[i].BeneficiaryName,
                });

           var str = JSON.stringify(this.removeList);
           var data = new FormData();
           data.append("BeneficiaryList", JSON.stringify(this.removeList));
           var result = await objPB.RemoveBeneficiary(data);

           if (result.status == "Unhandled Exception")
           this.$router.push({ name: "error" });
           else {
               if (result.status == "success") {
                   this.Msg = "Beneficiary removed successfully..";
                   this.successSnackbarMsg = true;
               }
               else {
                   this.Msg = "Failed in removing Beneficiary ";
                   this.errorSnackbarMsg = true;
               }
           }

           this.removeList = [];
           this.dlgRemoveBeneficiary = false;
           this.GetAllBeneficiaries();
           this.vmBeneficiary = [];
           this.isBulk = false;
      },

    //=========== Import & Register ==========//

      /// <summary>
      ///  DownloadSampleFile :- Function used to download sample file while import and register beneficiary
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      DownloadSampleFile() {
          var ColumnHeader = "";
          var ColumnData1 = [];
          var ColumnData = [];
          for (var i = 0; i < this.lstImportBeneficiaryHearder.length; i++) {
              if(this.lstImportBeneficiaryHearder[i].text.includes(','))
              {
                  ColumnHeader += ColumnHeader == "" ? "\""+this.lstImportBeneficiaryHearder[i].text+"\"" : "," + "\""+this.lstImportBeneficiaryHearder[i].text+"\"";
              }
              else
              {
                  ColumnHeader += ColumnHeader == "" ? this.lstImportBeneficiaryHearder[i].text : "," + this.lstImportBeneficiaryHearder[i].text;
              }
              ColumnData1.push("Registration Response here");
          }
          ColumnHeader = ColumnHeader + "\n";
          ColumnData.push(ColumnData1);

          ColumnData.forEach(function (row) {
              ColumnHeader += row.join(",");
              ColumnHeader += "\n";
          });

          var hiddenElement = document.createElement("a");
          hiddenElement.href = "data:text/csv;charset=utf-8," + encodeURI(ColumnHeader);
          hiddenElement.target = "_blank";
          hiddenElement.download = "sample_beneficiary_register_" + Math.round(Math.random() * 1000) + ".csv";
          hiddenElement.click();
      },

      /// <summary>
      ///  OnFileUpload :- Function used to upload file for import and register beneficiary
      /// </summary>
      /// <param name="event"></param>
      /// <returns> </returns>
      OnFileUpload(event) {
          this.csvFileName = this.fileInput.name;
          var vm = this;
          if (window.FileReader) {
              var reader = new FileReader();
              reader.readAsText(this.fileInput);
              // Handle errors load
              reader.onload = function (event) {
                 var csv = event.target.result;
                 vm.ReadCsvFile(csv);
              };
              reader.onerror = function (evt) {
                 if (evt.target.error.name == "NotReadableError") {
                    this.Msg = "Cannot read file !";
                    this.infoSnackbarMsg = true;
                 }
              };
          } else {
              this.Msg = "FileReader is not supported in this browser.";
              this.infoSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  ReadCsvFile :- Function used to read a csv file
      /// </summary>
      /// <param name="csv"></param>
      /// <returns> </returns>
      ReadCsvFile(csv) {
        var today = new Date().toLocaleString();
        var lines = csv.split(/\r\n|\n/);
        //var headers = lines[0].split(",");
          var headers = objUtils.common.MergeCommas(lines[0]);
          
        if (this.lstImportBeneficiaryHearder.length == headers.length) {
            for (var i = 1; i < lines.length; i++) {
                var obj = {};
                // var currentline = lines[i].split(",");
                var currentline;
                currentline = objUtils.common.MergeCommas(lines[i]);
                if (currentline != "") {
                    for (var j = 0; j < headers.length; j++)
                    {
                        var dataType=this.lstImportBeneficiaryHearder[j].type
                        var QuestionCondition =[];
                        QuestionCondition = this.lstImportBeneficiaryHearder[j].QuestionCondition != null ? this.lstImportBeneficiaryHearder[j].QuestionCondition : [];
                        if (dataType =="number")
                        {
                            if(isNaN(currentline[j]) ||(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes'))
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                            }
                            else
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                            }
                            // validating Question Condition
                            this.IsvalidexcelData = true;
                            if(QuestionCondition.length > 0)
                            {
                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                {
                                    if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                    {
                                        var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                        if(!currentline[j].match(phoneNum))
                                        {
                                            obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                            this.IsvalidexcelData = false;
                                        }
                                    }
                                    if (QuestionCondition[Count].ValidationCondition == "Min & Max Range" && currentline[j] != "")
                                    {
                                        if(!(parseInt(currentline[j]) > parseInt(QuestionCondition[Count].MinValue) && parseInt(currentline[j]) < parseInt(QuestionCondition[Count].MaxValue)))
                                        {
                                             obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                             this.IsvalidexcelData = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if(dataType =="date")
                        {
                            var dateField=currentline[j];
                            var timestamp = function(dateField) {
                            if ( Object.prototype.toString.call(dateField) === "[object Date]" )
                                return true;
                                return false;
                            };
                            //var timestamp = Date.parse(currentline[j]);

                            //if (isNaN(timestamp) ||(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory==true))
                            if (timestamp==false ||(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes'))
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                            }
                            else
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                            }
                            // validating Question Condition
                            this.IsvalidexcelData = true;
                            if(QuestionCondition.length > 0)
                            {
                                for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                                {
                                    // Date Validation
                                    if (QuestionCondition[Count].ValidationCondition == "Should be greater than Current date" && currentline[j] != "")
                                    {
                                         if (objUtils.common.formatExcelDate(dateField) < objUtils.common.formatExcelDate(today))
                                         {
                                               obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                               this.IsvalidexcelData = false;
                                          }
                                     }
                                     if (QuestionCondition[Count].ValidationCondition == "Should be less than Current date" && currentline[j] != "")
                                     {
                                          if (objUtils.common.formatExcelDate(dateField) > objUtils.common.formatExcelDate(today))
                                          {
                                               obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                               this.IsvalidexcelData = false;
                                          }
                                     }
                                 }
                            }
                        }
                        else if(dataType =="select" && this.lstBTQuestions[j].BTQstnAnsOptionList.length>0)
                        {
                            var QstnOptionList=[];
                            for (var k = 0; k < this.lstBTQuestions[j].BTQstnAnsOptionList.length; k++) {
                                QstnOptionList.push(this.lstBTQuestions[j].BTQstnAnsOptionList[k].OptionValue)
                            }

                            if((currentline[j] !="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes' && !QstnOptionList.includes(currentline[j])))
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                            }
                            else
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                            }
                        }
                        else
                        {
                            if(currentline[j]=="" && this.lstImportBeneficiaryHearder[j].mandatory=='Yes')
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input"
                            }
                            else
                            {
                                obj[this.lstImportBeneficiaryHearder[j].value] = currentline[j];
                            }
                        }
                        // validating Question Condition
                        this.IsvalidexcelData = true;
                        if(QuestionCondition.length > 0)
                        {
                            for(var Count = 0; Count <= QuestionCondition.length-1 ; Count++)
                            {
                                if (QuestionCondition[Count].ValidationCondition == "Phone Number" && currentline[j] != "")
                                {
                                     var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                                     if(!currentline[j].match(phoneNum))
                                     {
                                          obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                          this.IsvalidexcelData = false;
                                     }
                                }
                                if (QuestionCondition[Count].ValidationCondition == "Email" && currentline[j] != "")
                                {
                                     var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                                     if(!currentline[j].match(email))
                                     {
                                          obj[this.lstImportBeneficiaryHearder[j].value] = "Invalid Input";
                                          this.IsvalidexcelData = false;
                                      }
                                 }
                            }
                        }
                    }
                    this.lstImportBeneficiary.push(obj);
                }
            }
        } else {
            this.Msg = "Beneficiary Type format is not matching for the selected file...  ";
            this.infoSnackbarMsg = true;
        }
      },

      /// <summary>
      ///  onBeforeNameChange :- Function used to validate Beneficiary imported excel data
      /// </summary>
      /// <param name="...args"></param>
      /// <returns> </returns>
      onBeforeNameChange (...args) {
          var today = new Date().toLocaleString();
          this.IsvalidexcelData=true;
          var list= this.lstImportBeneficiary;
          var header=this.lstImportBeneficiaryHearder;
          var IsMadatoryHearder=header.filter(x=>x.mandatory=='Yes')

          for(var i=0;i<=IsMadatoryHearder.length-1;i++)
          {
              for(var j=0 ;j<= list.length-1 ; j++)
              {
                  var Ans =   list[j][IsMadatoryHearder[i].value];
                  if(Ans == "")
                  {
                      this.IsvalidexcelData=false;
                  }
              }
          }
          var argument=args;
          if(argument[3].mandatory=='Yes')
          {
              if(argument[0]=="")
              {
                  // this.IsvalidexcelData=false;
                  argument[0]="Invalid Input";
                  return 'Required field'

              }
          }
          var tempData=this.lstImportBeneficiaryHearder.filter(x=>x.value==argument[3].name)
          if(tempData.length>0)
          {
              if(tempData[0].QuestionCondition != null)
              {
                  var phoneNoValidation=tempData[0].QuestionCondition;
                  for (var j = 0; j < phoneNoValidation.length; j++)
                  {
                      if (phoneNoValidation[j].ValidationCondition == "Phone Number" && argument[0] !="")
                      {
                          var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                          if(!argument[0].match(phoneNum))
                          {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Phone Number'
                          }
                      }
                      if (phoneNoValidation[j].ValidationCondition == "Email" && argument[0] !="")
                      {
                          var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                          if(!argument[0].match(email))
                          {
                              this.IsvalidexcelData=false;
                              return 'Enter a valid Email'
                          }
                      }
                      if (phoneNoValidation[j].ValidationCondition == "Should be greater than Current date" && argument[0] !="")
                      {
                          if (objUtils.common.formatExcelDate(argument[0]) < objUtils.common.formatExcelDate(today))
                          {
                              this.IsvalidexcelData=false;
                              return 'Should be greater than Current date'
                          }
                      }
                      if (phoneNoValidation[j].ValidationCondition == "Should be less than Current date" && argument[0] !="")
                      {
                          if (objUtils.common.formatExcelDate(argument[0]) > objUtils.common.formatExcelDate(today))
                          {
                              this.IsvalidexcelData=false;
                              return 'Should be less than Current date'
                          }
                      }
                      if (phoneNoValidation[j].ValidationCondition == "Min & Max Range" && argument[0] !="")
                      {
                          if(!(parseInt(argument[0]) > parseInt(phoneNoValidation[j].MinValue) && parseInt(argument[0]) < parseInt(phoneNoValidation[j].MaxValue)))
                          {
                              this.IsvalidexcelData=false;
                              return "Value should be between " + phoneNoValidation[j].MinValue + " and " + phoneNoValidation[j].MaxValue;
                          }
                      }
                  }
              }
          }
          return ''
      },

      /// <summary>
      ///  searchStringInArray :- Function used to search an array which contains any invalid input string
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      searchStringInArray () {
          var newList = this.lstImportBeneficiary.filter(element => {
              for (var property in element) {
                  if (element.hasOwnProperty(property)) {
                      if(element[property] == "Invalid Input") {
                            return true;
                      }
                  }
              }

          });
          if(newList.length>0)
          {
              return false;
          }
          else
          {
              return true;
          }
      },

      /// <summary>
      ///  ImportRegisterBeneficiary :- Function used to import register beneficiary
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async ImportRegisterBeneficiary() {
          if (this.searchStringInArray() == true) {
              var lstImportList = [];
              for (var i = 0; i < this.lstImportBeneficiary.length; i++) {
                  var lstQstnAns = [];
                  for (var j = 0; j < this.lstImportBeneficiaryHearder.length; j++)
                      lstQstnAns.push({
                          QuestionID: this.lstBTQuestions[j].QuestionID,
                          AnswerTypeID: this.lstBTQuestions[j].AnswerTypeID,
                          Answer: this.lstImportBeneficiary[i][
                             this.lstImportBeneficiaryHearder[j].value
                          ],
                      });

                   lstImportList.push({
                      BeneficiaryTypeID: this.BenefTID,
                      BeneficiaryName: this.lstImportBeneficiary[i][
                       this.lstImportBeneficiaryHearder[0].value
                      ],
                      BenefQuestions: lstQstnAns,
                   });
              }
              var data = new FormData();
              data.append("BenefImportList", JSON.stringify(lstImportList));
              var result = await objPB.ImportRegisterBeneficiary(data);
              if (result.status == "Unhandled Exception")
                  this.$router.push({ name: "error" });
              else {
                  if (result.status == "success") this.ImportRegBenefSteps = 3;
                      this.fileInput = null;
                      this.lstImportBeneficiary = [];
                      this.GetAllBeneficiaries();
              }
          }
          else
          {
               this.Msg = "Please resolve all Invalid Input(s).";
               this.infoSnackbarMsg = true;
          }
      },

      /// <summary>
      ///  CancelImport :- Function used to cancel import register beneficiary
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      CancelImport() {
          this.resetOnClick = 2
          this.dlgImportRegisterBeneficiary = false;
          this.ImportRegBenefSteps = 1;
          this.fileInput = null;
          this.lstImportBeneficiary = [];
          this.resetOnClick = 0
          this.lstImportBeneficiary = []
      },
    //=======================================//
      /// <summary>
      ///  UpdateBeneficiary :- Function used to update beneficiary
      /// </summary>
      /// <param name=""></param>
      /// <returns> </returns>
      async UpdateBeneficiary() {
          if(await this.fnCheckQuestionValidationAndCondition(this.lstBeneficiaryDetails)==true)
	      {
              return false;
          }
          this.PreLoader = true;
          var Ans = "";
          var blobFile = "";

          for (var i = 0; i < this.lstBeneficiaryDetails.length; i++) {
              Ans = "";
              if (this.lstBeneficiaryDetails[i].AnswerType == "Multi-Choice")
              {

                  var MultiChoiceAns = this.multiChoiceAns.filter(x => x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)
                  for (var j = 0; j < MultiChoiceAns.length; j++)
                      Ans = Ans != "" ? Ans + "," + MultiChoiceAns[j].OptionValue : MultiChoiceAns[j].OptionValue;

                  this.lstBeneficiaryDetails[i].Answer = Ans;
              }
              if (this.lstBeneficiaryDetails[i].AnswerType == "Cascading Dropdown") {
                   //for (var j = 0; j < this.cascadingAns.length; j++)
                   //   Ans = Ans != "" ? Ans + "," + this.cascadingAns[j] : this.cascadingAns[j];

                   var CasQuestionsAns = this.cascadingAns.filter(x=> x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID)
                   for (var j = 0; j < CasQuestionsAns[0].selectedOption.length; j++)
                   {
                       Ans = Ans != "" ? Ans + "," + CasQuestionsAns[0].selectedOption[j]: CasQuestionsAns[0].selectedOption[j];
                   }
                  this.lstBeneficiaryDetails[i].Answer = Ans;
              }
              if ( this.lstBeneficiaryDetails[i].AnswerType == "Audio Capture" || this.lstBeneficiaryDetails[i].AnswerType == "Signature Capture" || this.lstBeneficiaryDetails[i].AnswerType == "Photo Capture" ) {
                  if (this.AnsBlob.length != 0) {
                       var FileBlob=this.AnsBlob.filter( (x) => x.QuestionID == this.lstBeneficiaryDetails[i].QuestionID )[0];
                       if(typeof FileBlob != "undefined"){
                           blobFile = FileBlob.blobData;
                           this.FileUpload(blobFile, this.lstBeneficiaryDetails[i].Answer);
                       }
                  }
                  this.lstBeneficiaryDetails[i].AnsBlob = null;
              }
          }

          var data = new FormData();
          data.append("BenefUpdateList", JSON.stringify(this.lstBeneficiaryDetails));
          var result = await objPB.UpdateBeneficiary(data);
          if (result.status == "Unhandled Exception"){
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
              this.EditBeneficiaryDialog = false;
              this.uploadedImg = "";
              this.img = null;
              this.startCamera = false;
              this.camOn = false;
              this.RemovePreviousFiles();
              this.GetAllBeneficiaries();
          }
      },

      /// <summary>
      ///  updateCoordinates :- Function used to update coordinates while GPS capture
      /// </summary>
      /// <param name="evnt"></param>
      /// <param name="Qitem"></param>
      /// <returns> </returns>
      updateCoordinates(evnt, Qitem) {
         this.Coordinates = {
            lat: evnt.lat(),
            lng: evnt.lng(),
         };
         Qitem.Answer = this.Coordinates.lat + "," + this.Coordinates.lng;
      },

      /// <summary>
      ///  fnCheckQuestionValidationAndCondition :- Function used to check validation and condition for mandatory/unique questions
      /// </summary>
      /// <param name="BTQuestions"></param>
      /// <returns> </returns>
      async fnCheckQuestionValidationAndCondition(BTQuestions){
          //  Checking for mandatory questions
          var lstMandatoryQuestion = BTQuestions.filter(x=> x.IsMandatoryResponse == true);
          //  Checking unique
          var lstunique = BTQuestions.filter(x=> x.IsUnique == true);
          //  Checking for Validation Question Min and Max
          var lstValidationQuestion = BTQuestions.filter(x=> x.IsValidationQuestion == 1);

          var IsValid = false;
          if(lstMandatoryQuestion.length > 0 ){
              // Selecting from Mandatory Questions without answer
              var IsQAnswers = lstMandatoryQuestion.filter(x=> x.Answer == "" || x.Answer == null);

              var QstnWithAnswer =lstMandatoryQuestion.filter(x=> x.Answer != ""|| x.Answer != null);
              QstnWithAnswer.forEach(function(part, index) {
                  QstnWithAnswer[index].ValidationErrorTest = "";
                  QstnWithAnswer[index].IsValidationError = false;
              });
              if(IsQAnswers.length > 0){
                  for( var i = 0; i <= IsQAnswers.length -1 ; i++){
                      if(IsQAnswers[i].AnswerType != "Cascading Dropdown" && IsQAnswers[i].AnswerType !="Photo Capture" && IsQAnswers[i].AnswerType != "Multi-Choice"){
                          BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                          BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                          IsValid = true;
                      }
                      else if(IsQAnswers[i].AnswerType == "Cascading Dropdown")
                      {
                          
                          var CascadingAns = this.cascadingAns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)//this.cascadingAnsArray.filter(x => (x.QID == IsQAnswers[i].QuestionID) && (x.IsBenefQstn == false));
                          if ((CascadingAns == null || CascadingAns.length == 0 || CascadingAns.length < IsQAnswers[i].BTQstnAnsCascadingOptionList.length)){
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else {
                              BTQuestions.filter(x => x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                          }
                      }
                      else if(IsQAnswers[i].AnswerType == "Multi-Choice")
                      {
                          var SelectedmultiChoiceAns = this.multiChoiceAns.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)
                          if(typeof SelectedmultiChoiceAns == "undefined" || SelectedmultiChoiceAns.length == 0)
                          {
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else
                          {
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                          }
                      }
                      else if(IsQAnswers[i].AnswerType == "Photo Capture")
                      {
                           if(typeof IsQAnswers[i].AnsBlob != "undefined")
                           {
                               if(IsQAnswers[i].AnsBlob.name == "")
                               {
                                    BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                                    BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                                    IsValid = true;
                               }
                               else
                               {
                                    BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                                    BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                               }
                           }
                           else
                           {
                               BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "Required Field";
                               BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = true;
                               IsValid = true;
                           }
                      }
                      else
                      {
                           BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].ValidationErrorTest = "";
                           BTQuestions.filter(x=> x.QuestionID == IsQAnswers[i].QuestionID)[0].IsValidationError = false;
                      }
                  }
                  // return IsValid;
              }
          }

          // Code for validations checking
          if(lstValidationQuestion.length > 0)
          {
              var IsQAnswers = lstValidationQuestion.filter(x=> x.Answer != "" && x.Answer != null);
              var valConditions
              var NewErrorMsg ="";
              var OldErrorMsg ="";
              var today = new Date().toLocaleDateString('en-GB');
              for(var intValQstCnt = 0 ; intValQstCnt<=IsQAnswers.length-1 ; intValQstCnt++)
              {
                  valConditions = IsQAnswers[intValQstCnt].BTQstnValidationList;
                  for (var i = 0; i < valConditions.length; i++)
                  {
                      if (valConditions[i].ValidationCondition == 'Min & Max Range') // if Starts here
                      {
                          if (!(IsQAnswers[intValQstCnt].Answer > parseInt(valConditions[i].MinValue) && IsQAnswers[intValQstCnt].Answer < parseInt(valConditions[i].MaxValue)))
                          {
                              NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                              //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;

                              if(OldErrorMsg == ""){
                                   BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                              } else {
                                  BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg
                              }
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else
                          {
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                          }
                      }// if ends here
                      if (valConditions[i].ValidationCondition == "Should be less than Current date")
                      {
                          if (IsQAnswers[intValQstCnt].Answer > objUtils.common.formatDateTodayDate(today))
                          {
                               NewErrorMsg = "Should be less than Current date";
                               //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                               if(OldErrorMsg == ""){
                                   BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                               } else {
                                   BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg
                               }
                               this.qRules = "";
                               //this.chkQstnValid = true;
                               BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                               IsValid = true;
                          }
                          else
                          {
                               //this.chkQstnValid = false;
                               BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                          }
                      }// if ends here
                      if (valConditions[i].ValidationCondition == "Should be greater than Current date")
                      {
                          if (IsQAnswers[intValQstCnt].Answer < objUtils.common.formatDateTodayDate(today))
                          {
                              NewErrorMsg = "Should be greater than Current date";
                              //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                              if(OldErrorMsg == ""){
                                  BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                              } else {
                                  BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg
                              }
                              // this.qRules = "";
                              //this.chkQstnValid = true;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else
                          {
                              //this.chkQstnValid = false;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                          }
                      }

                      // Checking Phone number validation
                      if (valConditions[i].ValidationCondition == "Phone Number")
                      {
                          var phoneNum = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                          if(!IsQAnswers[intValQstCnt].Answer.match(phoneNum))
                          {
                              NewErrorMsg = "Please enter a valid Phone Number";
                              //OldErrorMsg = this.lstSectionQstns.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                              if(OldErrorMsg == ""){
                                  BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                              } else {
                                  BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg
                              }
                              // this.qRules = "";
                              //this.chkQstnValid = true;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else
                          {
                              //this.chkQstnValid = false;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                          }
                      }
                      // Checking email  validation
                      if (valConditions[i].ValidationCondition == "Email")
                      {
                          var email =/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
                          if(!IsQAnswers[intValQstCnt].Answer.match(email))
                          {
                              NewErrorMsg = "Please enter a valid Email";
                              //OldErrorMsg = this.lstBTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ErrorText;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].qRules ="";
                              if(OldErrorMsg == ""){
                                  BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = NewErrorMsg;
                              } else {
                                  BTQuestions.filter(x => x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].ValidationErrorTest = OldErrorMsg
                              }
                              // this.qRules = "";
                              //this.chkQstnValid = true;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = true;
                              IsValid = true;
                          }
                          else
                          {
                              //this.chkQstnValid = false;
                              BTQuestions.filter(x=> x.QuestionID == IsQAnswers[intValQstCnt].QuestionID)[0].IsValidationError = false
                          }
                      }
                  }
              }
          }
          if(lstunique.length > 0 )
          {
              // Selecting from unique answer
              var IsuniqueAnswers = lstunique.filter(x=> x.Answer != "" || x.Answer != null);
              for( var i = 0; i <= IsuniqueAnswers.length -1 ; i++)
              {
                  var data = new FormData();
                  data.append("Answer", IsuniqueAnswers[i].Answer);
                  var result = await objPB.IsUniqueResponse(data);
                  if (result.status == "Unhandled Exception")
                      this.$router.push({ name: "error" });
                  else
                  {
                      if (result.status == "success")
                      {
                          BTQuestions.filter(x=> x.QuestionID == IsuniqueAnswers[i].QuestionID)[0].UniqueErrorTest = "Must be a unique one";
                          BTQuestions.filter(x=> x.QuestionID == IsuniqueAnswers[i].QuestionID)[0].IsUniqueValidation = true;
                          IsValid = true;
                      }
                  }
              }
          }
          return IsValid;
      },
      GetCascadingName(QuestionID , ItemIndex)
      {
         if(this.cascadingAns.length > 0)
          {
               return this.cascadingAns.filter(x=>x.QuestionID == QuestionID)[0].selectedOption[ItemIndex];
          }
          else{ return "";}
      },
      /// <summary>
      ///  onFocusCheckQstnValidation :- Function used to check question validation for question types (Single line,Multi line,Numeric,DatePicker)
      /// </summary>
      /// <param name="QItem"></param>
      /// <returns> </returns>
      onFocusCheckQstnValidation(QItem) {
          var valConditions = QItem.BTQstnValidationList;
          var NewErrorMsg ="";
          var OldErrorMsg ="";
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
                      if(QItem.Answer !="")
                      {
                          if (!(QItem.Answer > parseInt(valConditions[i].MinValue) && QItem.Answer < parseInt(valConditions[i].MaxValue))) {
                              if(valConditions[i].LogicOperator == 1 && QItem.ValidationErrorTest == "") //-- 1--> Or Condition
                              {

                              }
                              NewErrorMsg = "Value should be between " + valConditions[i].MinValue + " and " + valConditions[i].MaxValue;
                              //OldErrorMsg = QItem.ErrorText;
                              if(OldErrorMsg == "")
                              {
                                  QItem.ValidationErrorTest = NewErrorMsg;
                              }
                              else
                              {
                                  QItem.ValidationErrorTest= OldErrorMsg
                              }
                               QItem.IsValidationError = true;
                          }
                          else
                          {
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
                      if (QItem.Answer > objUtils.common.formatDateTodayDate(today))
                      {
                          NewErrorMsg = "Should be less than Current date";
                          //OldErrorMsg = QItem.ErrorText;
                          if(OldErrorMsg == "")
                          {
                              QItem.ValidationErrorTest = NewErrorMsg;
                          }
                          else
                          {
                              QItem.ValidationErrorTest = OldErrorMsg
                          }
                          QItem.IsValidationError = true;
                      }
                      else
                      {
                          QItem.IsValidationError = false;
                          QItem.ValidationErrorTest="";
                      }
                  }
                  else if (valConditions[i].ValidationCondition == 'Should be greater than Current date') {
                      // this.txtLength = valConditions[i].MaxValue.toString().length;
                      QItem.minVal = new Date().toISOString().substr(0, 10);
                      //this.maxVal = valConditions[i].MaxValue;
                      // this.qType = "date";
                      //this.qRules = '';
                      if (QItem.Answer < objUtils.common.formatDateTodayDate(today))
                      {
                          NewErrorMsg = "Should be greater than Current date";
                          //OldErrorMsg = QItem.ErrorText;
                          if(OldErrorMsg == "")
                          {
                              QItem.ValidationErrorTest = NewErrorMsg;
                          }
                          else
                          {
                              QItem.ValidationErrorTest = OldErrorMsg
                          }
                          QItem.IsValidationError = true;
                      }
                      else
                      {
                          QItem.IsValidationError = false;
                          QItem.ValidationErrorTest="";
                      }
                  }
              }
          }
      },
  }
};
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

    .dialogHeight {
        height: 70vh;
    }

    .uploadBoxHeight {
        height: 39vh;
    }

    .uploadScrollHeight {
        height: 35vh;
    }

    @media screen and (max-width: 750px) {

        .dialogHeight {
            height: 85vh;
        }


        .uploadBoxHeight {
            height: 54vh;
        }

        .uploadScrollHeight {
            height: 48vh;
        }
    }

</style>
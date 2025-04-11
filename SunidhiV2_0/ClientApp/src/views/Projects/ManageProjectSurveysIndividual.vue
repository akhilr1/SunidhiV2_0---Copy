<template>
    <div>
        <!--Pre-loader-->
        <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
        <!--________________________-->
        <v-row>
            <v-col md="auto">
                <h5 class="page-head pl-3">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                    </v-avatar>
                    <!--if path is ManageProjectSurveysIndividual-->
                    <router-link v-show="$route.name=='ManageProjectSurveysIndividual'" :to="{ name: 'ManageProjectSurveys', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Surveys</router-link>
                    <!--__________________-->
                    <!--if path is MyprojectsDraftIndividualSurveysIndividual-->
                    <router-link v-show="$route.name=='MyprojectsDraftIndividualSurveysIndividual'" :to="{ name: 'MyprojectsDraftIndividualSurveys', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Surveys</router-link>
                    <!--________________-->
                    <!--if path is MyprojectsDeployedIndividualSurveysIndividual-->
                    <router-link v-show="$route.name=='MyprojectsDeployedIndividualSurveysIndividual'" :to="{ name: 'MyprojectsDeployedIndividualSurveys', query: {WsID: WorkspaceID, Access:RWAccess,InitID :  InitiativeID, ProjID: ProjectID} }" class="page-head text-link">Surveys</router-link>
                    <!--________________-->
                    > {{SurveyName}}
                </h5>
            </v-col>
            <v-spacer></v-spacer>
            <v-col cols="2" v-if="ActiveTab == 'tab-2'">
                <v-select 
                    outlined  
                    dense 
                    hide-details="auto" 
                    v-model="currentResponseSectionType" 
                    :items="responseSectionTypes"
                    @change="switchResponseViewType"
                    item-text="name"
                    item-value="id"
                ></v-select>
            </v-col>
            <!--<v-col md="auto">
            <Success-Button class="btn-122x36 mr-4 float-left" title="Save" @click.native="SaveSurveySectionQuestion()"></Success-Button>
            <Default-Button-Outlined class="btn-122x36 mr-4 float-left" title="Cancel" @click.native=""></Default-Button-Outlined>
        </v-col>-->
        </v-row>

        <v-row justify="center">
            <v-col md="12">
                <v-card elevation="0">
                    <v-tabs centered class="pt-0" color="tab-active-line" @change="GetAllBeneficiaries()" v-model="ActiveTab">
                        <v-tab href='#tab-1'>Questions</v-tab>
                        <v-tab href='#tab-2' @click="fnMountPivotTab()">Responses</v-tab>
                        <!--Questions Tab Content-->
                        <v-tab-item value="tab-1">
                            <v-card tile elevation="0">
                                <v-card class="ma-4" elevation="0">
                                    <v-card-title class="pb-0">
                                        <v-row class="">
                                            <v-col md="auto">
                                                <h5 class="page-head">
                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                        <v-icon size="15" color="primary">mdi-file-document-edit-outline</v-icon>
                                                    </v-avatar>
                                                    {{SurveyName}}
                                                </h5>
                                            </v-col>
                                            <v-spacer></v-spacer>
                                            <v-col md="auto">
                                                <v-btn name="btnSendSurvey" @click.native="shareLinkPopup=true" class="mr-4 float-left primary">Send</v-btn>
                                                <Default-Icon-Button-Outlined name="btnEditSurvey" id="AutoTest_IP662" class="mr-4 float-left" icon="mdi-pencil-outline" @click.native="dlgEditSurvey=true" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                                <Default-Icon-Button-Outlined name="btnDeleteSurvey" id="AutoTest_IP663" class="mr-4 float-left" icon="mdi-trash-can-outline" @click.native="dlgRemoveSurvey=true" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-card-title>
                                    <v-card-text style="padding-right:30vw">
                                        <p class="ml-1">
                                            {{SurveyDescription}}
                                        </p>
                                    </v-card-text>
                                </v-card>
                                <v-card-text class="py-0">
                                    <draggable v-model="lstSections" @update="UpdateSectionOrder(lstSections)">
                                        <transition-group type="transition" name="flip-list">
                                            <div class="drag-cursor" v-for="(sectionItem,indexs) in lstSections" :key="sectionItem.SectionID">
                                                <v-row class="mt-4" no-gutters>
                                                    <v-col sm="2">
                                                        <v-card style="height:4vh;background:#667686;text-align:center;width:85%;padding-top:7px;" tile>                                                           
                                                            <span style="color:white;font-size:16px;font-weight:bold;">Section {{indexs+1}} of {{sectionTotalCount}}</span>
                                                        </v-card>
                                                    </v-col>
                                                </v-row>
                                                <v-card class="mb-4" style="border-top: 4px solid #667686;border-radius: 0px;" tile>
                                                    <!--<v-card class="mt-4" v-for="sectionItem in lstSurveySections">-->
                                                    <v-card-title class="pb-0">
                                                        <v-row>
                                                            <v-col md="auto">
                                                                <h5 class="page-head">
                                                                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                                                                        <v-icon size="15" color="primary">mdi-file-question-outline</v-icon>
                                                                    </v-avatar>
                                                                    {{sectionItem.SectionName}}
                                                                </h5>
                                                            </v-col>
                                                            <v-spacer></v-spacer>
                                                            <v-col md="auto">
                                                                <Default-Icon-Button-Outlined name="btnEditSurveySection" id="AutoTest_IP663A" class="mr-4 float-left" icon="mdi-pencil-outline" @click.native="dlgEditSection=true,GetSurveySectionDetails(sectionItem.SectionID),sectionItem.viewSave=false" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                                                <Default-Icon-Button-Outlined name="btnDeleteSurveySection" id="AutoTest_IP664" class="mr-4 float-left" icon="mdi-trash-can-outline" @click.native="dlgRemoveSection=true,SectionID=sectionItem.SectionID,SectionName=sectionItem.SectionName,sectionItem.viewSave=false" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                                                <!--<Default-Icon-Button-Outlined id="AutoTest_IP665" class="mr-4 float-left" icon="mdi-dots-vertical" @click.native="SectionID=sectionItem.SectionID,GetSectionsLogic(sectionItem.SectionID),GetAllLogicConditions(),GetAllQstnsForSectionsLogic(sectionItem.SectionID),dlgSecSettings=true,sectionItem.viewSave=false"></Default-Icon-Button-Outlined>-->
                                                                <Default-Icon-Button-Outlined name="btnSectionSettings" id="AutoTest_IP665" class="mr-4 float-left" icon="mdi-settings" @click.native="SectionID=sectionItem.SectionID,GetSectionsLogic(sectionItem.SectionID),GetAllLogicConditions(),dlgSecSettings=true,sectionItem.viewSave=false,SectionSettingTab = 'tab-2'" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>

                                                                <Default-Icon-Button-Outlined name="btnExpandSection" id="AutoTest_IP666" v-show="sectionItem.questionPanel===false" class="mr-4 float-left" icon="mdi-chevron-down" @click.native="sectionItem.questionPanel=true,sectionItem.viewSave=false"></Default-Icon-Button-Outlined>
                                                                <Default-Icon-Button-Outlined name="btnCollapseSection" id="AutoTest_IP667" v-show="sectionItem.questionPanel===true" class="mr-4 float-left" icon="mdi-chevron-up" @click.native="sectionItem.questionPanel=false,sectionItem.viewSave=false"></Default-Icon-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-title>
                                                    <v-card-text style="padding-right:30vw; padding-left:22px;">
                                                        {{sectionItem.SectionDescription}}
                                                    </v-card-text>
                                                    <v-divider></v-divider>
                                                    <v-card-text v-show="sectionItem.questionPanel===false && sectionItem.UpdateQstnSection===false">
                                                        <v-chip name="txtCountOfQuestions" color="#ABA8A8" dark="">
                                                            {{sectionItem.SurveySecQuestions.length}} Questions
                                                        </v-chip>
                                                    </v-card-text>

                                                    <v-card-text class="pb-0" v-show="sectionItem.questionPanel===true">
                                                        <draggable v-model="sectionItem.SurveySecQuestions" @update="UpdateQstnOrder(sectionItem.SurveySecQuestions)">
                                                            <transition-group type="transition" name="flip-list">
                                                                <div class="drag-cursor" v-for="(item,index) in sectionItem.SurveySecQuestions" :key="item.QuestionID">
                                                                    <v-row class="pt-4">
                                                                        <v-col md="4" class="pl-4 pt-4">
                                                                            <p class="font-weight-medium">{{item.QuestionName}}</p>
                                                                            <!--Singleline Answer-->
                                                                            <v-text-field v-if="item.QuestionType==='Single Line Text'" dense label="Type your answer" v-bind:disabled="!item.IsEditQstn">
                                                                            </v-text-field>
                                                                            <!--Multiline Answer-->
                                                                            <v-textarea v-if="item.QuestionType==='Multi Line Text'" dense label="Type your answer" v-bind:disabled="!item.IsEditQstn">
                                                                            </v-textarea>

                                                                            <!--Single Choice -->
                                                                            <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                                                <v-radio-group>
                                                                                    <v-radio :label="optItem.OptionValue" disabled v-for="(optItem,ind) in item.lstQuestionOptions" :key="ind"></v-radio>
                                                                                </v-radio-group>
                                                                            </div>
                                                                            <!--Multiple Choice -->
                                                                            <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0">
                                                                                <v-checkbox hide-details input-value="true" :label="optItem.OptionValue" disabled="" v-for="(optItem,ind) in item.lstQuestionOptions" :key="ind"></v-checkbox>

                                                                            </div>
                                                                            <!--Dropdown -->
                                                                            <div v-if="item.QuestionType==='Dropdown'" class="mt-0">
                                                                                <v-row>
                                                                                    <v-col md="auto">
                                                                                        <v-select disabled dense :items="item.lstQuestionOptions" :menu-props="{ maxHeight: '300' }"
                                                                                                  item-text="OptionValue"
                                                                                                  label="Select">
                                                                                        </v-select>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>

                                                                            <!--Cascading Dropdown -->
                                                                            <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                                <v-row>
                                                                                    <v-col md="auto">
                                                                                        <v-select dense="" :menu-props="{ maxHeight: '300' }" disabled
                                                                                                  :label="` ${optItem.OptionValue}`"
                                                                                                  v-for="(optItem,ind) in item.lstCascadingOptions" :key="ind">
                                                                                        </v-select>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>

                                                                            <!--Numeric-->
                                                                            <v-text-field v-if="item.QuestionType==='Numeric'" dense type="number" label="Type your answer" disabled="">
                                                                            </v-text-field>

                                                                            <!--GPS Capture-->
                                                                            <div v-if="item.QuestionType==='GPS Capture'">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                                    Location
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Photo Capture-->
                                                                            <div v-if="item.QuestionType==='Photo Capture'">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                                    Photo Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Audio Capture-->
                                                                            <div v-if="item.QuestionType==='Audio Capture'" disabled="">
                                                                                <v-btn outlined disabled>
                                                                                    <v-icon class="mr-2">mdi-audiobook</v-icon>
                                                                                    Audio Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Signature Capture-->
                                                                            <div v-if="item.QuestionType==='Signature Capture'">
                                                                                <v-btn outlined="" disabled="">
                                                                                    <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                                    Signature Capture
                                                                                </v-btn>
                                                                            </div>
                                                                            <!--Note-->
                                                                            <div v-if="item.QuestionType==='Note'">
                                                                                <v-textarea disabled outlined></v-textarea>
                                                                            </div>
                                                                            <!--Date Picker-->
                                                                            <div v-if="item.QuestionType==='Date Picker'">
                                                                                <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                        v-model="SelectAnswerTypeDatepicker"
                                                                                        :close-on-content-click="false"
                                                                                        transition="scale-transition"
                                                                                        :return-value.sync="SelectAnswerTypeDate"
                                                                                        offset-y=""
                                                                                        min-width="290px">
                                                                                    <template v-slot:activator="{ on }">
                                                                                        <v-text-field v-model="SelectAnswerTypeDate"
                                                                                                      prepend-inner-icon="mdi-calendar" dense
                                                                                                      readonly="" clearable=""
                                                                                                      v-on="on" disabled=""></v-text-field>
                                                                                    </template>
                                                                                    <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                        <v-spacer></v-spacer>
                                                                                        <v-btn id="AutoTest_IP668" text="" color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                        <v-btn id="AutoTest_IP669" text="" color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(SelectAnswerTypeDate)">OK</v-btn>
                                                                                    </v-date-picker>
                                                                                </v-menu>
                                                                            </div>

                                                                            <!--Likert -->
                                                                            <div v-if="item.QuestionType==='Likert Scale'" class="mt-0">
                                                                                <v-radio-group>
                                                                                    <v-radio :label="likerts.OptionValue" disabled v-for="(likerts,ind) in item.lstQuestionOptions" :key="ind"></v-radio>
                                                                                </v-radio-group>
                                                                            </div>
                                                                            <!--File Upload-->
                                                                            <div v-if="item.QuestionType==='File Upload'">
                                                                                <v-btn outlined="" disabled="">
                                                                                    <v-icon class="mr-2">mdi-file-document-box-outline</v-icon>
                                                                                    Upload File
                                                                                </v-btn>
                                                                            </div>
                                                                        </v-col>

                                                                        <v-col md="3">
                                                                            <v-select dense="" :menu-props="{ maxHeight: '300' }" filled=""
                                                                                      :label="` ${item.QuestionType}`" outlined="" disabled="" hide-details>
                                                                            </v-select>
                                                                            <div v-if="item.QuestionType==='Numeric'">
                                                                                <v-text-field :placeholder="item.UOM" dense="" disabled=""></v-text-field>
                                                                            </div>
                                                                        </v-col>
                                                                        <v-col md="5" align="right">
                                                                            <Default-Icon-Button-Outlined name="btnEditQuestion" id="AutoTest_IP670" class="mr-4" icon="mdi-pencil-outline" @click.native="EditQuestion(item,index,indexs)" v-bind:disabled="RespCount>0 || !screenAccess"></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined name="btnCopyQuestion" id="AutoTest_IP671" class="mr-4" icon="mdi-content-copy" @click.native="QuestionID=item.QuestionID,dlgCopySectionQuestion=true,sectionItem.viewSave=false" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined name="btnDeleteQuestion" id="AutoTest_IP672" class="mr-4" icon="mdi-trash-can-outline" @click.native="SectionID=item.SectionID,QuestionID=item.QuestionID,dlgRemoveSectionQuest=true,sectionItem.viewSave=false" v-bind:disabled="!screenAccess"></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined name="btnQuestionSettings" id="AutoTest_IP673" class="mr-4" icon="mdi-settings" v-bind:disabled="RespCount>0 || !screenAccess" @click.native="SectionID=item.SectionID,QuestionID=item.QuestionID,GetSectionQstnSetting(SectionID,QuestionID),dlgSectionQstnSetting=true,sectionItem.viewSave=false"></Default-Icon-Button-Outlined>
                                                                        </v-col>
                                                                        <v-col cols="12" class="px-0 py-0">
                                                                            <v-divider></v-divider>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <div class="indicator">
                                                                        <v-icon>mdi-blur</v-icon>
                                                                    </div>
                                                                </div>

                                                            </transition-group>
                                                        </draggable>
                                                    </v-card-text>

                                                    <!-- Update an existing question-->
                                                    <v-card-text v-show="sectionItem.UpdateQstnSection===true" class="px-3 pt-0">
                                                        <template v-for="(item,ind) in sectionItem.SurveySecQuestions">
                                                            <v-row v-bind:class="{ 'active-section py-4 pl-4': item.IsEditQstn }">
                                                                <v-col md="4">
                                                                    <div v-if="!item.IsEditQstn">
                                                                        <p class="font-weight-medium">{{item.QuestionName}}</p>
                                                                    </div>
                                                                    <div v-if="item.IsEditQstn">
                                                                        <v-text-field name="txtEditQuestionName" filled dense v-model="vmEQuestionName" label="Question">
                                                                        </v-text-field>
                                                                    </div>
                                                                    <!--Singleline Answer-->
                                                                    <v-text-field v-if="item.QuestionType==='Single Line Text'" dense label="Single Line Text" disabled>
                                                                    </v-text-field>
                                                                    <!--Multiline Answer-->
                                                                    <v-textarea v-if="item.QuestionType==='Multi Line Text'" dense label="Type your answer" disabled="">
                                                                    </v-textarea>

                                                                    <!--Single Choice / Multi choice Answer / DropDown-->
                                                                    <div v-if="!item.IsEditQstn">
                                                                        <!--Single Choice -->
                                                                        <div v-if="item.QuestionType==='Single Choice'" class="mt-0">
                                                                            <v-radio-group>
                                                                                <v-radio :label="optItem.OptionValue" disabled="" v-for="(optItem,ind) in item.lstQuestionOptions" :key="ind"></v-radio>
                                                                            </v-radio-group>
                                                                        </div>
                                                                        <!--Multiple Choice -->
                                                                        <div v-if="item.QuestionType==='Multi-Choice'" class="mt-0">
                                                                            <v-checkbox input-value="true" :label="optItem.OptionValue" disabled="" v-for="(optItem,ind) in item.lstQuestionOptions" :key="ind"></v-checkbox>
                                                                        </div>
                                                                        <!--Dropdown -->
                                                                        <div v-if="item.QuestionType==='Dropdown'" class="mt-0">
                                                                            <v-row>
                                                                                <v-col md="auto">
                                                                                    <v-select id="txtEnterDropdownOption" dense :items="item.lstQuestionOptions" :menu-props="{ maxHeight: '300' }"
                                                                                              item-text="OptionValue"
                                                                                              label="Select">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                        <!--Cascading Dropdown -->
                                                                        <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                            <v-row>
                                                                                <v-col md="auto">
                                                                                    <v-select id="drpCascadingList" dense="" :menu-props="{ maxHeight: '300' }" filled disabled
                                                                                              :label="` ${optItem.OptionValue}`"
                                                                                              v-for="(optItem,ind) in item.lstCascadingOptions" :key="ind">
                                                                                    </v-select>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </div>
                                                                    </div>
                                                                    <!--Single Choice / Multi choice Answer / DropDown in Edit mode-->
                                                                    <div v-if="item.IsEditQstn">
                                                                        <div v-if="item.QuestionType==='Single Choice' || item.QuestionType==='Multi-Choice' || item.QuestionType==='Dropdown'" class="mt-0">
                                                                            <div v-if="item.lstQuestionOptions.length>0">
                                                                                <v-row v-for="(SingleChoice, index) in item.lstQuestionOptions" :key="index">
                                                                                    <v-col v-if="item.QuestionType !='Cascading Dropdown'">
                                                                                        <v-text-field name="txtEditEnterChoiceOption" dense label="Enter Option" v-model="SingleChoice.OptionValue" hide-details>
                                                                                        </v-text-field>
                                                                                    </v-col>
                                                                                    <v-col md="auto">
                                                                                        <Button-Light-24x24 name="btnDeleteOption" id="AutoTest_IP674" v-show="item.lstQuestionOptions.length>1" class="mr-4 mt-3" icon="mdi-trash-can-outline" @click.native="EditDeleteOptionItem(SingleChoice,item)"></Button-Light-24x24>
                                                                                    </v-col>
                                                                                    <v-col cols="12" class="py-0" align="right">
                                                                                        <Button-Light-24x24 name="btnEditAddOption" id="AutoTest_IP675" v-show="index == item.lstQuestionOptions.length - 1" class="mt-3 mr-4" icon="mdi-plus" @click.native="EditOptionItems(item)"></Button-Light-24x24>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                            <v-checkbox color="info" label="Add Others" v-model="IsOtherOptionSelected"></v-checkbox>
                                                                        </div>
                                                                        <!--Cascading Dropdown -->
                                                                        <div v-if="item.QuestionType==='Cascading Dropdown'" class="mt-0">
                                                                            <div v-if="item.lstCascadingOptions!=null">
                                                                                <v-row v-for="(optItem, index) in item.lstCascadingOptions.slice(0, 1)" :key="index">
                                                                                    <v-col>
                                                                                        <v-select id="drpEditCascadingList" v-model="vmECascadingOption"
                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                  item-text="OptionValue"
                                                                                                  item-value="ID"
                                                                                                  dense="" :menu-props="{ maxHeight: '300' }"
                                                                                                  :label="` ${optItem.OptionValue}`"
                                                                                                  return-object="" @change="SaveCascadingOptions(vmECascadingOption.ID),preCasOption=1">
                                                                                        </v-select>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                            <div v-else="">
                                                                                <v-row>
                                                                                    <v-col>
                                                                                        <v-select id="drpEditCascadingList" v-model="vmECascadingOption"
                                                                                                  :items="lstCascadingQstnOptions"
                                                                                                  item-text="OptionValue"
                                                                                                  item-value="ID"
                                                                                                  dense="" :menu-props="{ maxHeight: '300' }" filled=""
                                                                                                  label="Select " outlined=""
                                                                                                  return-object="" @change="SaveCascadingOptions(vmECascadingOption.ID)">
                                                                                        </v-select>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <!--Numeric-->
                                                                    <v-text-field v-if="item.QuestionType==='Numeric'" dense="" type="number" label="Type your answer" disabled>
                                                                    </v-text-field>

                                                                    <!--GPS Capture-->
                                                                    <div v-if="item.QuestionType==='GPS Capture'" disabled="">
                                                                        <v-btn outlined disabled>
                                                                            <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                            Capture Location
                                                                        </v-btn>
                                                                    </div>
                                                                    <!--Photo Capture-->
                                                                    <div v-if="item.QuestionType==='Photo Capture'" disabled="">
                                                                        <v-btn outlined disabled>
                                                                            <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                            Photo Capture
                                                                        </v-btn>
                                                                    </div>
                                                                    <!--Audio Capture-->
                                                                    <div v-if="item.QuestionType==='Audio Capture'" disabled="">
                                                                        <audio controls></audio>
                                                                    </div>
                                                                    <!--Signature Capture-->
                                                                    <div v-if="item.QuestionType==='Signature Capture'" disabled="">
                                                                        <v-btn outlined disabled>
                                                                            <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                            Upload Signature
                                                                        </v-btn>
                                                                    </div>
                                                                    <!--Note-->
                                                                    <div v-if="item.QuestionType==='Note'" disabled="">
                                                                        <v-textarea disabled outlined></v-textarea>
                                                                    </div>
                                                                    <!--Date Picker-->
                                                                    <div v-if="item.QuestionType==='Date Picker'">
                                                                        <v-menu ref="SelectAnswerTypeDatepicker"
                                                                                v-model="SelectAnswerTypeDatepicker"
                                                                                :close-on-content-click="false"
                                                                                transition="scale-transition"
                                                                                :return-value.sync="SelectAnswerTypeDate"
                                                                                offset-y=""
                                                                                min-width="290px">
                                                                            <template v-slot:activator="{ on }">
                                                                                <v-text-field v-model="SelectAnswerTypeDate"
                                                                                              label="Select Date"
                                                                                              prepend-inner-icon="mdi-calendar" dense=""
                                                                                              readonly="" clearable=""
                                                                                              v-on="on" disabled=""></v-text-field>
                                                                            </template>
                                                                            <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                                <v-spacer></v-spacer>
                                                                                <v-btn id="AutoTest_IP676" text="" color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                                <v-btn id="AutoTest_IP677" text="" color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(SelectAnswerTypeDate)">OK</v-btn>
                                                                            </v-date-picker>
                                                                        </v-menu>
                                                                    </div>

                                                                    <!--Likert-->
                                                                    <div v-if="item.QuestionType==='Likert Scale' && likerSection == indexs && likerIndex == ind">
                                                                        <v-row v-for="( LikertItem, index) in item.lstQuestionOptions" :key="index">
                                                                            <v-col>
                                                                                <v-text-field name="txtEditEnterLikertScaleOption" dense="" label="Enter Option" v-model="LikertItem.OptionValue">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                            <v-col md="auto">
                                                                                <Button-Light-24x24 name="btnDeleteOption" v-show="item.lstQuestionOptions.length>1" class="mr-4 mt-3" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(LikertItem,true,item)"></Button-Light-24x24>
                                                                            </v-col>
                                                                            <v-col cols="12" align="right" class="py-0">
                                                                                <Button-Light-24x24 name="btnEditAddOption" v-show="index == item.lstQuestionOptions.length - 1" class="mt-3 mr-4" icon="mdi-plus" @click.native="EditOptionItemsLIkert(item)"></Button-Light-24x24>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </div>

                                                                    <div v-if="(item.QuestionType==='Likert Scale' && likerSection == indexs && likerIndex != ind )|| (item.QuestionType==='Likert Scale' && likerSection != indexs && likerIndex == ind ) || (item.QuestionType==='Likert Scale' && likerSection != indexs && likerIndex != ind )" class="mt-0">
                                                                        <v-radio-group>
                                                                            <v-radio :label="likerts.OptionValue" disabled v-for="(likerts,ind) in item.lstQuestionOptions" :key="ind"></v-radio>
                                                                        </v-radio-group>
                                                                    </div>

                                                                    <!--File Upload-->
                                                                    <div v-if="item.QuestionType==='File Upload'">
                                                                        <v-file-input multiple label="Upload File" disabled></v-file-input>
                                                                    </div>
                                                                </v-col>
                                                                <v-col md="3" v-show="item.IsEditQstn">
                                                                    <v-select dense :items="lstQstnTypes" :menu-props="{ maxHeight: '300' }"
                                                                              item-text="Type"
                                                                              item-value="ID" return-object=""
                                                                              :label="` ${item.QuestionType}`" outlined
                                                                              v-model="vmEQuestionType" @change="EditQuestionType(item)">
                                                                    </v-select>
                                                                    <div v-if="item.QuestionType==='Numeric'">
                                                                        <v-text-field :placeholder="item.UOM" class="mt-4" dense="" label="Unit of Measurement" v-model="vmEUOM" v-bind:disabled="!item.IsEditQstn"></v-text-field>
                                                                        <!--<div v-if="item.IsEditQstn">
                                                                        <v-text-field :placeholder="item.UOM" outlined="" dense="" v-model="vmEUOM"></v-text-field>
                                                                    </div>
                                                                    <div v-if="!item.IsEditQstn">
                                                                        <v-text-field :placeholder="item.UOM" outlined="" dense="" disabled></v-text-field>
                                                                    </div>-->
                                                                    </div>
                                                                </v-col>
                                                                <v-col md="3" v-show="!item.IsEditQstn">
                                                                    <v-select dense="" :menu-props="{ maxHeight: '300' }" filled=""
                                                                              :label="` ${item.QuestionType}`" outlined="" disabled="">
                                                                    </v-select>
                                                                </v-col>
                                                                <v-col md="5" align="right">
                                                                    <v-row>
                                                                        <v-col cols="12">
                                                                            <Default-Icon-Button-Outlined name="" class="mr-4" icon="mdi-pencil-outline" v-if="RespCount==0" disabled=""></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined class="mr-4" icon="mdi-content-copy" disabled=""></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined name="btnDeleteOption" class="mr-4" icon="mdi-trash-can-outline" disabled=""></Default-Icon-Button-Outlined>
                                                                            <Default-Icon-Button-Outlined class="mr-4" icon="mdi-settings" disabled=""></Default-Icon-Button-Outlined>
                                                                        </v-col>
                                                                        <!-- Save and Cancel Button for updating question-->
                                                                        <!-- The button section only visible when the question try to edit-->
                                                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-5" v-show="item.IsEditQstn">

                                                                            <Secondary-Button name="btnUpdatEditQuestion" id="AutoTest_IP678" class="btn-122x36 mr-4" title="Save" @click.native="UpdateQuestion(item.QuestionID,vmEQuestionName,vmEQuestionType,item,item.UOM,item.SurveyID,item.SectionID),
                                                                   item.IsEditQstn =false,sectionItem.UpdateQstnSection=false,sectionItem.questionPanel=true"
                                                                                              v-bind:disabled="fnCheckMandatoryQuestion(vmEQuestionName,vmEQuestionType.Type,true,item) || !screenAccess"></Secondary-Button>
                                                                            <Default-Button-Outlined name="btnCancelEditQuestion" id="AutoTest_IP679" class="btn-122x36 mr-4" title="Cancel" @click.native="CancelEditQuestion(item)"></Default-Button-Outlined>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-col>
                                                            </v-row>
                                                        </template>
                                                    </v-card-text>

                                                    <!--Add a new Question section-->
                                                    <v-card-text v-if="sectionItem.addQstnSection===true" class="active-section">
                                                        <v-row>
                                                            <v-col md="4">
                                                                <v-text-field name="txtQuestionName" filled dense label="Untitled Question" v-model="vmQstn">
                                                                </v-text-field>
                                                                <!--Singline Answer-->

                                                                <v-text-field v-if="vmQuestionType.Type==='Single Line Text'" dense="" label="Type your answer" disabled="">
                                                                </v-text-field>
                                                                <!--Multiline Answer-->
                                                                <v-textarea v-if="vmQuestionType.Type==='Multi Line Text'" dense="" label="Type your answer" disabled="">
                                                                </v-textarea>

                                                                <!--Single Choice / Multi choice Answer / DropDown-->

                                                                <div v-if="vmQuestionType.Type==='Single Choice' || vmQuestionType.Type==='Multi-Choice' || vmQuestionType.Type==='Dropdown'" class="mt-0">
                                                                    <div v-if="SingleChoices.length>0">
                                                                        <v-row v-for="(SingleChoice, index) in SingleChoices" :key="index">
                                                                            <v-col>
                                                                                <v-text-field name="txtEnterOption" dense="" label="Enter Option" v-model="SingleChoice.OptionValue">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                            <v-col md="auto">
                                                                                <Button-Light-24x24 name="btnDeleteOption" id="AutoTest_IP680" v-show="SingleChoices.length>1" class="mr-4 mt-3" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(SingleChoice)"></Button-Light-24x24>
                                                                            </v-col>
                                                                            <v-col cols="12" class="py-0" align="right">
                                                                                <Button-Light-24x24 name="btnAddOption" id="AutoTest_IP681" v-show="index == SingleChoices.length - 1" class="mt-3 mr-4" icon="mdi-plus" @click.native="AddOptionItem(false,SingleChoice)"></Button-Light-24x24>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-checkbox color="info" label="Add Others" v-model="IsOtherOptionSelected"></v-checkbox>
                                                                    </div>
                                                                </div>
                                                                <!--Cascading Dropdown -->
                                                                <div v-if="vmQuestionType.Type==='Cascading Dropdown'" class="mt-0">
                                                                    <v-row>
                                                                        <v-col md="auto">
                                                                            <v-select id="drpCascadingList" v-model="vmCascadingOption" single-line
                                                                                      :items="lstCascadingQstnOptions"
                                                                                      item-text="OptionValue"
                                                                                      item-value="ID"
                                                                                      :menu-props="{ maxHeight: '300' }"
                                                                                      dense
                                                                                      label="Select" return-object @change="SaveCascadingOptions(vmCascadingOption.ID)">
                                                                            </v-select>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>
                                                                <!--Numeric-->
                                                                <v-text-field v-if="vmQuestionType.Type==='Numeric'" dense type="number" label="Type your answer" disabled="">
                                                                </v-text-field>

                                                                <!--GPS Capture-->
                                                                <div v-if="vmQuestionType.Type==='GPS Capture'">
                                                                    <v-btn outlined disabled>
                                                                        <v-icon class="mr-2">mdi-map-marker</v-icon>
                                                                        Capture Location
                                                                    </v-btn>
                                                                </div>
                                                                <!--Photo Capture-->
                                                                <div v-if="vmQuestionType.Type==='Photo Capture'">
                                                                    <v-btn outlined disabled>
                                                                        <v-icon class="mr-2">mdi-camera-image</v-icon>
                                                                        Upload Photo
                                                                    </v-btn>
                                                                </div>
                                                                <!--Audio Capture-->
                                                                <div v-if="vmQuestionType.Type==='Audio Capture'">
                                                                    <audio controls></audio>
                                                                </div>
                                                                <!--Signature Capture-->
                                                                <div v-if="vmQuestionType.Type==='Signature Capture'">
                                                                    <v-btn outlined disabled>
                                                                        <v-icon class="mr-2">mdi-fountain-pen-tip</v-icon>
                                                                        Upload Signature
                                                                    </v-btn>
                                                                </div>

                                                                <!--Note-->
                                                                <div v-if="vmQuestionType.Type==='Note'">
                                                                    <!-- <v-textarea disabled="" outlined=""></v-textarea>-->
                                                                </div>
                                                                <!--Date Picker-->
                                                                <div v-if="vmQuestionType.Type==='Date Picker'">
                                                                    <v-menu ref="SelectAnswerTypeDatepicker"
                                                                            v-model="SelectAnswerTypeDatepicker"
                                                                            :close-on-content-click="false"
                                                                            transition="scale-transition"
                                                                            :return-value.sync="SelectAnswerTypeDate"
                                                                            offset-y=""
                                                                            min-width="290px">
                                                                        <template v-slot:activator="{ on }">
                                                                            <v-text-field v-model="SelectAnswerTypeDate"
                                                                                          prepend-inner-icon="mdi-calendar" dense=""
                                                                                          readonly="" clearable=""
                                                                                          v-on="on" disabled=""></v-text-field>
                                                                        </template>
                                                                        <v-date-picker v-model="SelectAnswerTypeDate" no-title="" scrollable="">
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP682" text color="primary" @click="SelectAnswerTypeDatepicker = false">Cancel</v-btn>
                                                                            <v-btn id="AutoTest_IP683" text color="primary" @click="$refs.SelectAnswerTypeDatepicker.save(SelectAnswerTypeDate)">OK</v-btn>
                                                                        </v-date-picker>
                                                                    </v-menu>
                                                                </div>
                                                                <!--Likert Scale-->
                                                                <div v-if="vmQuestionType.Type==='Likert Scale'">
                                                                    <v-row v-for="(LikertItem, index) in Likerts" :key="index">
                                                                        <v-col>
                                                                            <v-text-field name="txtEnterLikertScaleOption" dense="" label="Enter Option" v-model="LikertItem.OptionValue">
                                                                            </v-text-field>
                                                                        </v-col>
                                                                        <v-col md="auto">
                                                                            <Button-Light-24x24 name="btnDeleteOption" v-show="Likerts.length>1" class="mr-4 mt-3" icon="mdi-trash-can-outline" @click.native="DeleteOptionItem(LikertItem,true,item)"></Button-Light-24x24>
                                                                        </v-col>
                                                                        <v-col cols="12" align="right" class="py-0">
                                                                            <Button-Light-24x24 name="btnAddOption" v-show="index == Likerts.length - 1" class="mt-3 mr-4" icon="mdi-plus" @click.native="AddOptionItem(true,LikertItem)"></Button-Light-24x24>
                                                                        </v-col>
                                                                    </v-row>
                                                                </div>
                                                                <!--File Upload-->
                                                                <div v-if="vmQuestionType.Type==='File Upload'">
                                                                    <v-file-input multiple label="Upload File" disabled></v-file-input>
                                                                </div>
                                                            </v-col>
                                                            <v-col md="3">
                                                                <v-select id="drpQuestionType" v-model="vmQuestionType" single-line=""
                                                                          :items="lstQstnTypes"
                                                                          item-text="Type"
                                                                          item-value="ID"
                                                                          :menu-props="{ maxHeight: '300' }"
                                                                          outlined="" dense=""
                                                                          label="Select Question Type" @change="QuestionCategory(),lstOptions=[],vmCascadingOption=[],SingleChoices = [{ OptionValue: '' }],lstOptions = SingleChoices"
                                                                          return-object>
                                                                </v-select>
                                                                <div v-if="vmQuestionType.Type==='Numeric'">
                                                                    <v-text-field class="mt-4" placeholder="Unit of Measurement" dense="" v-model="vmUOM"></v-text-field>
                                                                </div>
                                                            </v-col>
                                                            <v-col md="5" align="right">
                                                                <v-row>
                                                                    <v-col cols="12">
                                                                        <Default-Icon-Button-Outlined id="AutoTest_IP684" class="mr-4" icon="mdi-settings" @click.native="QuestionID=0,GetSectionQstnSetting(sectionItem.SectionID,QuestionID),dlgSectionQstnSetting=true"
                                                                                                      v-bind:disabled="vmQstn.length==0 || vmQuestionType==null "></Default-Icon-Button-Outlined>
                                                                    </v-col>
                                                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-5">
                                                                        <Secondary-Button name="btnSaveSurveyQuestion" id="AutoTest_IP685" class="btn-122x36 mr-4" title="Save" @click.native="AddNewQuestion(sectionItem.SectionID),sectionItem.addQstnSection=true,sectionItem.viewSave=false"
                                                                                          v-bind:disabled="vmQstn.length ==0 ||vmQuestionType.length ==0 ||  fnCheckMandatoryQuestion(vmQstn,vmQuestionType.Type,false) || !screenAccess"></Secondary-Button>
                                                                        <Default-Button-Outlined name="btnCancelSurveyQuestion" id="AutoTest_IP686" class="btn-122x36 mr-4" title="Cancel" @click.native="ClearAddQstn(),sectionItem.addQstnSection=false,sectionItem.viewSave =false"></Default-Button-Outlined>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-card-text>
                                                    <v-card-actions>
                                                        <!--<Primary-Button class="center-block mb-2" title="Add question" @click.native="AddQuestion(sectionItem.SectionID),sectionItem.addQstnSection=true"></Primary-Button>-->
                                                        <Primary-Button name="btnAddQuestion" id="AutoTest_IP687" class="center-block btn-122x36 mb-2 mt-2" title="Add question" @click.native="sectionItem.addQstnSection=true,sectionItem.viewSave =true" v-bind:disabled="sectionItem.viewSave || !screenAccess"></Primary-Button>
                                                    </v-card-actions>
                                                </v-card>
                                                <div class="indicator">
                                                    <v-icon>mdi-blur</v-icon>
                                                </div>
                                            </div>
                                        </transition-group>
                                    </draggable>
                                </v-card-text>
                                <v-card-actions>
                                    <Primary-Button name="btnAddNewSurveySection" id="AutoTest_IP688" class="center-block btn-122x36 mb-2" title="Add Section" @click.native="dlgAddSection=true" v-bind:disabled="!screenAccess"></Primary-Button>
                                </v-card-actions>
                            </v-card>
                        </v-tab-item>
                        <!--Response Tab Content-->
                        <v-tab-item class="mt-4" value="tab-2">
                            <v-card elevation="0" v-if="currentResponseSectionType === 1"> 
                                <v-card-text>
                                    <!-- Pagination -->
                                    <v-row>
                                        <!-- Drop down-->
                                        <!-- <v-col cols="12" md="4" class="pb-0">
                                            <v-select v-model="ResponseType" outlined dense :items="ResponseTypeItems"
                                                      label="Select View Type" @change="vmSearchResponse = '',GetResponseByBeneficiaryID()"></v-select>
                                        </v-col> -->
                                        <v-col class="pb-0">
                                            <v-row>
                                                <v-col class="py-0" v-show="ResponseType!=='Pivot Table'">
                                                    <!--<v-select dense label="Select Beneficiary" outlined></v-select>-->
                                                    <v-select id="drpBeneficiary" dense :items="lstAllBeneficiaries" :item-text="i => i.BeneficiaryName" :item-value="i => i.BeneficiaryID" label="Select Beneficiary" outlined="" v-model="BeneficiaryID" @change="vmSearchResponse = '',GetResponseByBeneficiaryID()">
                                                    </v-select>
                                                </v-col>
                                            </v-row> 
                                            <v-row align="center" justify="center">
                                                <v-col cols="auto" v-show="ResponseType==='Individual'">
                                                    <p class="black--text">
                                                        <v-btn name="btnPreviousPage" id="AutoTest_IP689" text max-width="20" min-width="20" @click="Pageindex-=1,resIndex =Pageindex-1" v-bind:disabled="Pageindex==1">
                                                            <v-icon>
                                                                mdi-chevron-left
                                                            </v-icon>
                                                        </v-btn>
                                                        <!--{{index }} of {{lstQstnResponsesInd.length}}-->
                                                        {{Pageindex}} of {{PageCount}}
                                                        <v-btn name="btnNextPage" id="AutoTest_IP690" text max-width="20" min-width="20" @click="Pageindex+=1,resIndex =Pageindex-1" v-bind:disabled="Pageindex==PageCount">
                                                            <v-icon>
                                                                mdi-chevron-right
                                                            </v-icon>
                                                        </v-btn>
                                                    </p>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <!-- Search Text-->
                                        <v-col class="pb-0" v-show="ResponseType!=='Pivot Table'">
                                            <v-text-field name="txtSearchByQuestion" outlined dense prepend-inner-icon="mdi-magnify" label="Search by Question" v-model="vmSearchResponse" v-on:keypress="GetSearchResponse($event)"></v-text-field>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col cols="12" class="pt-0 py-0">
                                            <!--If Response Type Pivot Table-->
                                            <v-row v-show="ResponseType==='Pivot Table'" class="py-0">
                                                <v-col cols="12" class="py-0">
                                                    <v-row justify="start" class="py-0">
                                                        <v-col cols="12" md="4" class="py-0">
                                                            <v-select :items="lstRepeatSections"
                                                                      v-model="presetSelected"
                                                                      item-value="presetName"
                                                                      item-text="presetName"
                                                                      @change="RestorePivotPreset()"
                                                                      label="Preset"
                                                                      dense
                                                                      outlined></v-select>
                                                        </v-col>
                                                        <v-col cols="12" md="4" class="pt-0">
                                                            <v-row class="pt-0 mt-0">
                                                                <v-col cols="4" class="pt-0">
                                                                    <v-btn outlined block depressed @click="addPresetDialogOpen()"><v-icon dense>mdi-plus</v-icon> Preset</v-btn>
                                                                </v-col>
                                                                <v-col cols="4" class="pt-0">
                                                                    <v-btn outlined block depressed @click="editPresetDialogOpen()"><v-icon dense>mdi-pencil-outline</v-icon>Edit</v-btn>
                                                                </v-col>
                                                                <v-col cols="4" class="pt-0">
                                                                    <v-btn outlined block depressed @click="deletePresetDialog=true"><v-icon dense>mdi-trash-can</v-icon>Delete</v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                    </v-row>
                                                    <v-row class="py-0">
                                                        <v-col cols="12" md="4" class="py-0">
                                                            <v-row class="pa-0 ma-0">
                                                                <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                                                                    <v-menu v-model="submittedmStartDate"
                                                                            :close-on-content-click="false"
                                                                            :nudge-right="40"
                                                                            transition="scale-transition"
                                                                            offset-y
                                                                            style="border:solid; border-color:red; width:100px;">
                                                                        <template v-slot:activator="{ on }">
                                                                            <v-text-field name="dtStartDate" v-model="submittedStartDate" label="Start Date"
                                                                                          outlined dense
                                                                                          readonly
                                                                                          v-on="on"></v-text-field>
                                                                        </template>
                                                                        <v-date-picker v-model="submittedbystartDate" no-title @input="submittedmStartDate = false" format="DD-MM-YYYY"
                                                                                       :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                                                    </v-menu>

                                                                </v-col>
                                                                <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                                                                    <v-menu v-model="submittedmEndDate"
                                                                            :close-on-content-click="false"
                                                                            :nudge-right="40"
                                                                            transition="scale-transition"
                                                                            offset-y
                                                                            min-width="290px">
                                                                        <template v-slot:activator="{ on }">
                                                                            <v-text-field name="dtEndDate" v-model="submittedEndDate" label="End Date"
                                                                                          outlined dense
                                                                                          readonly
                                                                                          v-on="on"></v-text-field>
                                                                        </template>
                                                                        <v-date-picker v-model="submittedbyendDate" no-title @input="submittedmEndDate = false"
                                                                                       :min="submittedbystartDate" :max="ProjectEndDate"></v-date-picker>
                                                                    </v-menu>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        <v-col md="2" class="py-0" v-if="lstRepeatLogic.length">
                                                            <v-select
                                                                outlined
                                                                label="Sections"
                                                                dense
                                                                v-model="repeatLogic"
                                                                :items="computedLstRepeatLogic"
                                                                :item-value="i => i.Key"
                                                                :item-text="i => i.Value"
                                                                :menu-props="{
                                                                    closeOnContentClick: true,
                                                                }"
                                                            >
                                                                <template v-slot:item="{ item, on, attrs }">
                                                                    <v-list-item v-on="on" v-bind="attrs">
                                                                        <v-list-item-content>
                                                                            <v-list-item-title>
                                                                                Section - {{item.Value}}
                                                                            </v-list-item-title>
                                                                        </v-list-item-content>
                                                                    </v-list-item>
                                                                </template>
                                                            </v-select>
                                                        </v-col>
                                                        <v-col cols="12" md="4" class="pt-0">
                                                            <v-row class="pa-0 ma-0">
                                                                <v-col cols="4" class="py-0">
                                                                    <v-btn outlined block depressed @click="submitpivottable()">Submit</v-btn>
                                                                </v-col>
                                                                <v-col cols="4" class="py-0 px-2">
                                                                    <v-btn outlined block depressed @click="clearDateSelection()">Clear Preset</v-btn>
                                                                </v-col>
                                                                <v-col cols="4" class="text-center py-0">
                                                                    <v-btn outlined depressed @click="ExportToExcel('xlsx')">
                                                                        <v-icon dense class="px-2">mdi-export-variant</v-icon> Export
                                                                    </v-btn>
                                                                </v-col>
                                                            </v-row>
                                                        </v-col>
                                                        
                                                    </v-row>
                                                </v-col>
                                                <!--<v-col cols="4" class="pr-0 pb-0">
        <v-btn outlined depressed class="mr-9" @click="ExportToExcel('xlsx')">
            Export as excel
        </v-btn>
        <v-btn outlined depressed @click="addPresetDialogOpen()">Add Preset</v-btn>
    </v-col>
    <v-col cols="2" class="pl-0 pb-0" style="position:relative;left:-65px">
        <v-select :items="presetLocalStorage"
                  v-model="presetSelected"
                  item-value="presetName"
                  item-text="presetName"
                  @change="RestorePivotPreset()"
                  label="Preset"
                  dense
                  outlined></v-select>
    </v-col>
    <v-col cols="4" class="pr-0 pb-0">
        <v-row class="pa-0 ma-0">
            <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                <v-menu v-model="submittedmStartDate"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        style="border:solid; border-color:red; width:100px;">
                    <template v-slot:activator="{ on }">
                        <v-text-field v-model="submittedStartDate" label="Start Date"
                                      outlined dense
                                      readonly
                                      v-on="on"></v-text-field>
                    </template>
                    <v-date-picker v-model="submittedbystartDate" no-title @input="submittedmStartDate = false" format="DD-MM-YYYY"
                                   :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                </v-menu>

            </v-col>
            <v-col cols="12" class="pa-0 col-md-6 ma-0 px-1">
                <v-menu v-model="submittedmEndDate"
                        :close-on-content-click="false"
                        :nudge-right="40"
                        transition="scale-transition"
                        offset-y
                        min-width="290px">
                    <template v-slot:activator="{ on }">
                        <v-text-field v-model="submittedEndDate" label="End Date"
                                      outlined dense
                                      readonly
                                      v-on="on"></v-text-field>
                    </template>
                    <v-date-picker v-model="submittedbyendDate" no-title @input="submittedmEndDate = false"
                                   :min="submittedbystartDate" :max="ProjectEndDate"></v-date-picker>
                </v-menu>
            </v-col>
        </v-row>
    </v-col>
    <v-col cols="2" class="pr-0 pb-0">
        <v-btn outlined depressed class="mr-5" @click="submitpivottable()">
            Submit
        </v-btn>
        <v-btn outlined depressed @click="clearDateSelection()">Clear</v-btn>
    </v-col>-->
                                                <v-col cols="12" class="pt-0">
                                                    <div id="output" class="pivot_table_container">
                                                    </div>
                                                </v-col>
                                                <!--dialod for add preset functionality-->
                                                <v-dialog v-model="addPresetDialog"
                                                          persistent
                                                          max-width="600">
                                                    <v-form ref="entryForm" @submit.prevent="submitHandler">
                                                        <v-card>
                                                            <v-card-title primary-title class="page-head pb-0">
                                                                <span style="font-weight:900;">Preset</span>
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP641"
                                                                       text
                                                                       @click="addPresetDialogCloseButton()">
                                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-card-text class="py-0">
                                                                <v-container fluid class="pt-0">
                                                                    <v-row no-gutters justify="center">
                                                                        <v-col cols="12" md="12" class="pt-7">
                                                                            <v-text-field v-model="presetName" :rules="rules.name" label="Name this Preset"
                                                                                          outlined>
                                                                                <template v-slot:append>
                                                                                    <span class="error--text">*</span>
                                                                                </template>
                                                                            </v-text-field>
                                                                        </v-col>
                                                                    </v-row>

                                                                    <v-row no-gutters justift="center" style="height:50px;" class="mb-4 v-scrolling-div">
                                                                        <v-col cols="2" class="pl-0 pb-0 pt-1" style="text-align:center;">
                                                                            <span style="font-size:15px;color:black;">X-Axis</span>
                                                                        </v-col>
                                                                        <v-col cols="9" class="pl-0 ">
                                                                            <v-menu v-for="(i,ind) in rowDialog" open-on-click="xAxisButton == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                                <template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue(i)" class="mr-2 mb-2" :key="ind" color="#439bde" id="AutoTest_I145" small
                                                                                            label>
                                                                                        <span style="color:white;">{{i}}</span>
                                                                                        <v-icon id="AutoTest_I154" right color="white" size="20">mdi-chevron-down</v-icon>
                                                                                    </v-chip>
                                                                                </template>
                                                                                <v-card>
                                                                                    <v-list>
                                                                                        <v-list-item @click="changeSelectedPivotValue(item,ind,'x')" v-for="(item, index) in nonSelectedData"
                                                                                                     :key="index">
                                                                                            <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                        </v-list-item>
                                                                                    </v-list>

                                                                                </v-card>
                                                                            </v-menu>
                                                                            <v-btn @click="changeSelectedPivotValue(null,null,'lastIndexX')" class="mb-2" :disabled="nonSelectedData.length == 0 ? true : false" outlined style="width:40px;height:30px;min-width:40px;" small><v-icon>mdi-plus</v-icon></v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters justift="center" style="height:50px;" class="mb-4 v-scrolling-div">
                                                                        <v-col cols="2" class="pl-0  pt-1" style="text-align:center;">
                                                                            <span style="font-size:15px;color:black;">Y-Axis</span>
                                                                        </v-col>
                                                                        <v-col cols="9" class="pl-0 ">
                                                                            <v-menu v-for="(i,ind) in colsDialog" open-on-click="yAxisButton == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                                <template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue(i)" class="mr-2 mb-2" :key="ind" color="#439bde" id="AutoTest_I145" small
                                                                                            label>
                                                                                        <span style="color:white;">{{i}}</span>
                                                                                        <v-icon id="AutoTest_I154" right color="white" size="20">mdi-chevron-down</v-icon>
                                                                                    </v-chip>
                                                                                </template>
                                                                                <v-card>
                                                                                    <v-list>
                                                                                        <v-list-item @click="changeSelectedPivotValue(item,ind,'y')" v-for="(item, index) in nonSelectedData"
                                                                                                     :key="index">
                                                                                            <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                        </v-list-item>
                                                                                    </v-list>

                                                                                </v-card>
                                                                            </v-menu>
                                                                            <v-btn class="mb-2" @click="changeSelectedPivotValue(null,null,'lastIndexY')" :disabled="nonSelectedData.length == 0" outlined style="width:40px;height:30px;min-width:40px;" small><v-icon>mdi-plus</v-icon></v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters justify="end">
                                                                        <v-col cols="6" md="3" class="pl-0 pb-0 pt-1">
                                                                            <v-btn centered
                                                                                   color="success-btn elevation-0 enter-btn"
                                                                                   class="mr-2"
                                                                                   dark
                                                                                   block
                                                                                   type="submit">Save</v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-container>
                                                            </v-card-text>
                                                        </v-card>
                                                    </v-form>
                                                </v-dialog>
                                                <!--dialog for edit functionality-->
                                                <v-dialog v-model="editPresetDialog"
                                                          persistent
                                                          max-width="800">
                                                    <v-form ref="editForm" @submit.prevent="submitEditHandler">
                                                        <v-card>
                                                            <v-card-title primary-title class="page-head pb-0">
                                                                <span style="font-weight:900;">Preset</span>
                                                                <v-spacer></v-spacer>
                                                                <v-btn id=""
                                                                       text
                                                                       @click="editPresetDialog=false">
                                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-card-text class="py-5 ma-0" style="background:#f5f5f5">
                                                                    <v-row no-gutters justify="center">
                                                                        <v-col cols="12" md="12" class="pt-7 px-3">
                                                                            <v-select :items="lstRepeatSections"
                                                                                      v-model="presetSelected_Edit"
                                                                                      item-value="presetName"
                                                                                      item-text="presetName"
                                                                                      @change="GetPivotPresetData_Edit()"
                                                                                      label="Select Preset"
                                                                                      dense
                                                                                      background-color="white"
                                                                                      outlined></v-select>
                                                                        </v-col>
                                                                    </v-row>

                                                                    <v-row no-gutters justift="center" align="center" class="mb-4 px-3">
                                                                        <v-col cols="2" class="pa-2" style="text-align:center;background:#e6EEEE">
                                                                            <span style="font-size:15px;color:black;">X-Axis</span>
                                                                        </v-col>
                                                                        <v-col cols="10" class="pa-2 ma-0 v-scrolling-div"  style="background:white;max-height:70px">
                                                                            <v-menu v-for="(i,ind) in rowDialog_Edit" open-on-click="xAxisButton_Edit == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                                <!--<template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue_Edit(i)" class="mr-2" :key="ind" color="#e6EEEE" id="" small
                                                                                            label>
                                                                                        <span style="color:black;">{{i}}</span>
                                                                                        <v-icon id="" right color="black" size="20">mdi-menu-down</v-icon>
                                                                                        <v-icon id="" right color="black" size="20">mdi-minus-circle-outline</v-icon>
                                                                                    </v-chip>


                                                                                </template>-->
                                                                                <template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" class="mr-2" :key="ind" color="#e6EEEE" id="" small label>
                                                                                        <span style="color:black;">{{i}}</span>
                                                                                        <v-icon id="" v-on="on" @click="nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-chevron-down</v-icon>
                                                                                        <v-icon id="" @click="Remove_nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-minus-circle-outline</v-icon>
                                                                                    </v-chip>
                                                                                </template>
                                                                                <v-card style="height:400px;" class=" v-scrolling-div">
                                                                                    <v-list>
                                                                                        <v-list-item @click="changeSelectedPivotValue_Edit(item,ind,'x')" v-for="(item, index) in nonSelectedData_Edit"
                                                                                                     :key="index">
                                                                                            <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                        </v-list-item>
                                                                                    </v-list>

                                                                                </v-card>
                                                                            </v-menu>
                                                                            <v-btn @click="changeSelectedPivotValue_Edit(null,null,'lastIndexX')" :disabled="nonSelectedData_Edit.length == 0 ? true : false"  style="width:5%;height:20px;min-width:5%;background:#e6EEEE" small><v-icon size="20">mdi-plus</v-icon></v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters justift="center" align="center" class="mb-4 px-3">
                                                                        <v-col cols="2" class="pa-2 ma-0" style="text-align:center;background:#e6EEEE">
                                                                            <span style="font-size:15px;color:black;">Y-Axis</span>
                                                                        </v-col>
                                                                        <v-col cols="10" class="pa-2 ma-0 v-scrolling-div"  style="background:white;max-height:70px">
                                                                            <v-menu v-for="(i,ind) in colsDialog_Edit" open-on-click="yAxisButton_Edit == i" absolute :close-on-click="true" :close-on-content-click="true">
                                                                                <!--<template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" v-on="on" @click="nonSelectedPivotValue_Edit(i)" class="mr-2" :key="ind" color="#e6EEEE" id="" small
                                                                                            label>
                                                                                        <span style="color:black;">{{i}}</span>
                                                                                        <v-icon id="" right color="black" size="20">mdi-chevron-down</v-icon>
                                                                                        <v-icon id="" right color="black" size="20">mdi-minus-circle-outline</v-icon>

                                                                                    </v-chip>
                                                                                </template>-->
                                                                                <template v-slot:activator="{ on, attrs }">
                                                                                    <v-chip v-bind="attrs" class="mr-2" :key="ind" color="#e6EEEE" id="" small label>
                                                                                        <span style="color:black;">{{i}}</span>
                                                                                        <v-icon id="" v-on="on" @click="nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-chevron-down</v-icon>
                                                                                        <v-icon id="" @click="Remove_nonSelectedPivotValue_Edit(i)" right color="black" size="20">mdi-minus-circle-outline</v-icon>
                                                                                    </v-chip>
                                                                                </template>
                                                                                <v-card style="height:400px;" class=" v-scrolling-div">
                                                                                    <v-list>
                                                                                        <v-list-item @click="changeSelectedPivotValue_Edit(item,ind,'y')" v-for="(item, index) in nonSelectedData_Edit"
                                                                                                     :key="index">
                                                                                            <v-list-item-title>{{ item}}</v-list-item-title>
                                                                                        </v-list-item>
                                                                                    </v-list>

                                                                                </v-card>
                                                                            </v-menu>
                                                                            <v-btn  @click="changeSelectedPivotValue_Edit(null,null,'lastIndexY')" :disabled="nonSelectedData_Edit.length == 0" style="width:5%;height:20px;min-width:5%;background:#e6EEEE" small><v-icon size="20">mdi-plus</v-icon></v-btn>
                                                                        </v-col>
                                                                    </v-row>
                                                            </v-card-text>
                                                            <v-card-actions>
                                                                <v-row no-gutters justify="end">
                                                                    <v-col cols="6" md="3" class="pl-0 pb-4 pt-1 pr-md-3">
                                                                        <v-btn centered
                                                                               color="primary elevation-0 enter-btn"
                                                                               class="mr-2"
                                                                               dark
                                                                               block
                                                                               type="submit">Save Preset</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-form>
                                                </v-dialog>
                                                <!--dialog for delete functionality-->
                                                <v-dialog v-model="deletePresetDialog"
                                                          persistent
                                                          max-width="800">
                                                    <v-form ref="deleteForm" @submit.prevent="submitDeleteHandler">
                                                        <v-card>
                                                            <v-card-title primary-title class="page-head pb-0">
                                                                <span style="font-weight:900;">Preset</span>
                                                                <v-spacer></v-spacer>
                                                                <v-btn id=""
                                                                       text
                                                                       color="black"
                                                                       @click="deletePresetDialog=false">
                                                                    <v-icon color="#707070" size="20">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                             <v-card-text class="py-5 ma-0" style="background:#f5f5f5">
                                                                    <v-row class="pa-0 ma-0">
                                                                        <v-col class="text-center pa-0 ma-0">
                                                                            <span>Presets once deleted cannot be restored. Are you sure want to delete the presets ?</span>
                                                                        </v-col>
                                                                    </v-row>
                                                                    <v-row no-gutters justify="center">
                                                                        <v-col cols="12" md="12" class="py-7">
                                                                            <v-select :items="lstRepeatSections"
                                                                                      v-model="presetSelected_Delete"
                                                                                      item-value="presetName"
                                                                                      item-text="presetName"
                                                                                      label="Select Preset"
                                                                                      dense
                                                                                      background-color="white"
                                                                                      outlined></v-select>
                                                                        </v-col>
                                                                    </v-row>
                                                            </v-card-text>
                                                            <v-card-actions>
                                                                <v-row no-gutters justify="end">
                                                                    <v-col cols="6" md="3" class="pl-0 pb-4 pt-1 pr-md-3">
                                                                        <v-btn centered
                                                                               color="primary elevation-0 enter-btn"
                                                                               class="mr-2"
                                                                               dark
                                                                               block
                                                                               type="submit">Delete Preset</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-form>
                                                </v-dialog>
                                            </v-row>
                                            <!--If Response Type Summary-->
                                            <v-card elevation="0" class="mb-4" v-show="ResponseType==='Summary'">
                                                <v-card-text class="px-0 pt-0">
                                                    <template v-for="(sectionItem ,index) in questionDetailsForResponse">
                                                        <v-row class="mt-4" no-gutters>
                                                            <v-col sm="2">
                                                                <v-card style="height:4vh;background:#667686;text-align:center;width:70%;" class="pt-1" tile>
                                                                    <span style="color:white;font-size:16px;font-weight:bold;">Section {{index+1}} of {{questionDetailsForResponse.length}}</span>
                                                                </v-card>
                                                            </v-col>
                                                        </v-row>
                                                        <v-expansion-panels accordion>
                                                            <v-expansion-panel @click="openPanelSummary(index)">
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
                                                                                <Default-Icon-Button-Outlined v-if="(arrowsUpArrayIndividual[index].sectionArrow == '')" @click.native="(arrowsUpArrayIndividual[index].sectionArrow == 1)" id="AutoTest_IP666" class="mr-4 mt-2 float-left" icon="mdi-chevron-down"></Default-Icon-Button-Outlined>
                                                                                <Default-Icon-Button-Outlined @click.native="(arrowsUpArrayIndividual[index].sectionArrow == '')" v-else-if="arrowsUpArrayIndividual[index].sectionArrow == 1" id="AutoTest_IP667" class="mr-4 float-left" icon="mdi-chevron-up"></Default-Icon-Button-Outlined>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row class="mb-1">
                                                                            <v-col sm="12">
                                                                                <span class=" mr-2 ml-3 mt-2">{{sectionItem.SectionDescription}}</span>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-divider></v-divider>
                                                                        <v-row>
                                                                            <v-col sm="12" v-if="(arrowsUpArrayIndividual[index].sectionArrow == '')">
                                                                                <v-chip color="#ABA8A8" dark="" class=" mr-2 ml-3 mt-2" v-if="(arrowsUpArrayIndividual[index].sectionArrow == '')">
                                                                                    <!-- {{sectionItem.SurveySecQuestions.length}} -->
                                                                                    <!--{{lstQstnResponses[index].length}} -->
                                                                                    <!--{{responseCountSummary[index]}}  Responses-->
                                                                                    {{fnSectionResponseCount(sectionItem.lstQstnResponses[0])}} Responses

                                                                                </v-chip>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card>
                                                                </v-expansion-panel-header>
                                                                <v-expansion-panel-content style="height:568px; overflow-y: auto !important;
    overflow-x: hidden !important;">
                                                                    <template v-for="(respItem ,ind) in sectionItem.lstQstnResponses[0]">
                                                                        <v-card elevation="0">
                                                                            <v-card-text>
                                                                                <p class="black--text ma-0">{{respItem.QuestionName}}</p>
                                                                                <small v-show="respItem.QuestionType !='Multi-Choice'">{{fnResponseCount(respItem.QuestionResponses,index)}} responses</small>
                                                                                <small v-show="respItem.QuestionType ==='Multi-Choice'">{{respItem.MultichoiceResponse}} responses</small>
                                                                            </v-card-text>
                                                                            <!--<v-card-text v-if="respItem.QuestionResponses.length>0">-->
                                                                            <v-card-text>
                                                                                <v-row v-if="respItem.QuestionResponses.length <= 10">
                                                                                    <v-col>
                                                                                        <template v-for="(item,idx) in respItem.QuestionResponses">
                                                                                            <!--Text type answers-->
                                                                                            <div class="survey-qstn-respons-chip"
                                                                                                 v-if="IsMandatoryChecking(respItem,item)">
                                                                                                {{item.Answer}}
                                                                                            </div>
                                                                                        </template>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <v-row v-else>
                                                                                    <v-col class="v-scrolling-div" style="height:250px">
                                                                                        <template v-for="(item,idx) in respItem.QuestionResponses">
                                                                                            <!--Text type answers-->
                                                                                            <div class="survey-qstn-respons-chip"
                                                                                                 v-if="IsMandatoryChecking(respItem,item)">
                                                                                                {{item.Answer}}
                                                                                            </div>
                                                                                        </template>
                                                                                    </v-col>
                                                                                </v-row>
                                                                                <template>
                                                                                    <!--Graph Summary-->
                                                                                    <v-row v-if="respItem.QuestionType==='Likert Scale'
                                                                                                               || respItem.QuestionType==='Single Choice'
                                                                                                               || respItem.QuestionType==='Multi-Choice'
                                                                                                               || respItem.QuestionType==='Numeric'" align="center" justify="center">
                                                                                        <v-col md="6">
                                                                                            <Line-Chart v-if="respItem.QuestionType==='Numeric'" :linedata="respItem.ChartData" :lineOptions="responsesLineOptions" style="width:100%; height:auto"></Line-Chart>
                                                                                            <Bar-Chart v-else="" :barData="respItem.ChartData" :barOptions="responsesBarOptions" style="width:100%; height:auto"></Bar-Chart>

                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </template>
                                                                                <template v-for="(item,idx) in respItem.QuestionResponses">
                                                                                    <!--Text type answers-->
                                                                                    <!-- <div class="survey-qstn-respons-chip"
                                                                                     v-if="IsMandatoryChecking(respItem,item)">
                                                                                    {{item.Answer}}
                                                                                </div> -->
                                                                                    <!--Audio answers-->
                                                                                    <v-row v-if="respItem.QuestionType==='Audio Capture'">
                                                                                        <v-col cols="12">
                                                                                            <h4 v-if="((idx > 0 && formatDT(item.UpdatedDate)!= formatDT(respItem.QuestionResponses[idx-1].UpdatedDate))||idx==0)" class="font-weight-medium black--text">{{formatDT(item.UpdatedDate)}}</h4>
                                                                                        </v-col>
                                                                                        <v-col cols="12" class="d-md-flex">
                                                                                            <div class="px-4 py-4">
                                                                                                <h5 class="text-center font-weight-medium">{{formatMonth(item.UpdatedDate)}}</h5>
                                                                                                <h2 class="text-center black--text font-weight-medium">{{formatDay(item.UpdatedDate)}}</h2>
                                                                                            </div>
                                                                                            <div style="width:100%" class="mx-md-6">
                                                                                                <v-divider class="mb-4"></v-divider>
                                                                                                <div class="d-md-flex">
                                                                                                    <div class="pb-4">
                                                                                                        <audio controls="" style="min-width:150px"><source :src="fileUrl+item.Answer" type="audio/mp3"></audio>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <!--Gps answers-->
                                                                                    <v-row v-if="respItem.QuestionType==='GPS Capture'">
                                                                                        <v-col cols="12">
                                                                                            <h4 v-if="((idx > 0 && formatDT(item.UpdatedDate)!= formatDT(respItem.QuestionResponses[idx-1].UpdatedDate))||idx==0)" class="font-weight-medium black--text">
                                                                                                {{formatDT(item.UpdatedDate)}}
                                                                                            </h4>
                                                                                        </v-col>
                                                                                        <v-col cols="12" class="d-md-flex">
                                                                                            <div class="px-4 py-4">
                                                                                                <h5 class="text-center font-weight-medium">{{formatMonth(item.UpdatedDate)}}</h5>
                                                                                                <h2 class="text-center black--text font-weight-medium">{{formatDay(item.UpdatedDate)}}</h2>
                                                                                            </div>
                                                                                            <div style="width:100%" class="mx-md-6">
                                                                                                <v-divider class="mb-4"></v-divider>
                                                                                                <div class="d-md-flex">
                                                                                                    <div class="pb-4">
                                                                                                        <GmapMap :center="{lat:parseInt(item.Answer.split(',')[0]), lng:parseInt(item.Answer.split(',')[1])}"
                                                                                                                 :zoom="7"
                                                                                                                 style="width: 350px; height: 175px">
                                                                                                            <GmapMarker :position="{lat:parseInt(item.Answer.split(',')[0]), lng:parseInt(item.Answer.split(',')[1])}"
                                                                                                                        :clickable="true"
                                                                                                                        :draggable="true" />
                                                                                                        </GmapMap>
                                                                                                    </div>
                                                                                                    <div class="pl-md-4">
                                                                                                        <h4 class="font-weight-medium black--text">GPS Capture</h4>
                                                                                                        <div class="d-flex pt-4">
                                                                                                            <div>
                                                                                                                <h5 class="font-weight-regular black--text">Latitude</h5>
                                                                                                                <h5 class="font-weight-regular">{{item.Answer.split(',')[0]}}</h5>
                                                                                                            </div>
                                                                                                            <div class="pl-9">
                                                                                                                <h5 class="font-weight-regular black--text">Longitude</h5>
                                                                                                                <h5 class="font-weight-regular">{{item.Answer.split(',')[1]}}</h5>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </v-col>

                                                                                    </v-row>
                                                                                    <!--Image Capture answers-->
                                                                                    <v-row v-if="respItem.QuestionType==='Photo Capture'">
                                                                                        <v-col cols="12">
                                                                                            <h4 v-if="((idx > 0 && formatDT(item.UpdatedDate)!= formatDT(respItem.QuestionResponses[idx-1].UpdatedDate))||idx==0)" class="font-weight-medium black--text">{{formatDT(item.UpdatedDate)}}</h4>
                                                                                        </v-col>
                                                                                        <v-col cols="12" class="d-md-flex">
                                                                                            <div class="px-4 py-4">
                                                                                                <h5 class="text-center font-weight-medium">{{formatMonth(item.UpdatedDate)}}</h5>
                                                                                                <h2 class="text-center black--text font-weight-medium">{{formatDay(item.UpdatedDate)}}</h2>
                                                                                            </div>
                                                                                            <div style="width:100%" class="mx-md-6">
                                                                                                <v-divider class="mb-4"></v-divider>
                                                                                                <div class="d-md-flex">
                                                                                                    <div class="pb-4" style="cursor: pointer">
                                                                                                        <!-- <img src="../../images/ic_no_workspace.svg" style="max-width:100px" />-->
                                                                                                        <v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <!-- File Upload-->
                                                                                    <v-row v-if="respItem.QuestionType==='File Upload'">
                                                                                        <v-col cols="12">
                                                                                            <h4 v-if="((idx > 0 && formatDT(item.UpdatedDate)!= formatDT(respItem.QuestionResponses[idx-1].UpdatedDate))||idx==0)" class="font-weight-medium black--text">{{formatDT(item.UpdatedDate)}}</h4>
                                                                                        </v-col>
                                                                                        <v-col cols="12" class="d-md-flex">
                                                                                            <div class="px-4 py-4">
                                                                                                <h5 class="text-center font-weight-medium">{{formatMonth(item.UpdatedDate)}}</h5>
                                                                                                <h2 class="text-center black--text font-weight-medium">{{formatDay(item.UpdatedDate)}}</h2>
                                                                                            </div>
                                                                                            <div style="width:100%" class="mx-md-6">
                                                                                                <v-divider class="mb-4"></v-divider>
                                                                                                <div class="d-md-flex">
                                                                                                    <div class="pb-4" style="cursor: pointer" v-if="fnDoumentFileType(item.Answer) == 'PDF'">
                                                                                                        {{item.Answer}}
                                                                                                        <v-img src="../../images/pdf_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                        <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                    </div>
                                                                                                    <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(item.Answer) == 'XLSX' || fnDoumentFileType(item.Answer) == 'CSV'">
                                                                                                        {{item.Answer}}
                                                                                                        <v-img src="../../images/excel_thumbnail.png" style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                        <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                    </div>
                                                                                                    <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(item.Answer) == 'TIFF' || fnDoumentFileType(item.Answer) == 'JPEG' || fnDoumentFileType(item.Answer) == 'JPG' || fnDoumentFileType(item.Answer) == 'GIF' || fnDoumentFileType(item.Answer) == 'PNG' || fnDoumentFileType(item.Answer) == 'JFIF'">
                                                                                                        {{item.Answer}}
                                                                                                        <v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                    </div>
                                                                                                    <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(item.Answer) == 'DOCX'">
                                                                                                        {{item.Answer}}
                                                                                                        <v-img src="../../images/doc_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                        <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                    </div>
                                                                                                    <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(item.Answer) == 'TXT'">
                                                                                                        {{item.Answer}}
                                                                                                        <v-img src="../../images/txt_thumbnail.png" style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                        <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                    </div>
                                                                                                    <div class="pb-4" style="cursor: pointer" v-else>
                                                                                                        {{item.Answer}}
                                                                                                        <v-img src="../../images/no_image_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                        <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->

                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <!--Signature Capture answers-->
                                                                                    <v-row v-if="respItem.QuestionType==='Signature Capture'">
                                                                                        <v-col cols="12">
                                                                                            <h4 v-if="((idx > 0 && formatDT(item.UpdatedDate)!= formatDT(respItem.QuestionResponses[idx-1].UpdatedDate))||idx==0)" class="font-weight-medium black--text">{{formatDT(item.UpdatedDate)}}</h4>
                                                                                        </v-col>
                                                                                        <v-col cols="12" class="d-md-flex">
                                                                                            <div class="px-4 py-4">
                                                                                                <h5 class="text-center font-weight-medium">{{formatMonth(item.UpdatedDate)}}</h5>
                                                                                                <h2 class="text-center black--text font-weight-medium">{{formatDay(item.UpdatedDate)}}</h2>
                                                                                            </div>
                                                                                            <div style="width:100%" class="mx-md-6">
                                                                                                <v-divider class="mb-4"></v-divider>
                                                                                                <div class="d-md-flex">
                                                                                                    <div class="pb-4" style="cursor: pointer">
                                                                                                        <!--<img src="../../images/ic_no_workspace.svg" style="max-width:100px" />-->
                                                                                                        <v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </template>
                                                                            </v-card-text>
                                                                            <v-divider></v-divider>
                                                                        </v-card>
                                                                    </template>
                                                                </v-expansion-panel-content>
                                                            </v-expansion-panel>
                                                        </v-expansion-panels>
                                                    </template>
                                                </v-card-text>
                                            </v-card>

                                            <!--If Response Type Individual-->
                                            <v-card class="mb-4 pt-0" elevation="0">
                                                <v-card-text v-show="ResponseType==='Individual'" class="pt-0 px-0">
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
                                                                                    {{fnIndividualResCount(sectionItem.lstQstnResponsesInd[0])}}
                                                                                    Responses
                                                                                </v-chip>
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-card>
                                                                </v-expansion-panel-header>
                                                                <v-expansion-panel-content style="height:550px;overflow-y: auto !important;
    overflow-x: hidden !important;">
                                                                    <v-card>
                                                                        <div v-show="((typeof sectionItem.lstQstnResponsesInd[0] !='undefined') && sectionItem.lstQstnResponsesInd[0].length >0)">
                                                                            <template v-for="(Item1,ind) in sectionItem.lstQstnResponsesInd[0]">
                                                                                <v-col v-if="Item1.SectionName == sectionItem.sectionName" cols="12" class="py-0 px-0">
                                                                                    <template v-for="(Item,i) in Item1.QuestionResponses">
                                                                                        <v-card v-if="i == Pageindex-1" elevation="0">
                                                                                            <v-card-text class="mb-4" v-if="Item.Answer != null && Item.Answer !=''">
                                                                                                <p class="black--text ma-0">{{Item.QuestionName}}</p>
                                                                                                <!--Text type answers-->
                                                                                                <div class="survey-qstn-respons-chip" v-if="((Item.QuestionType==='Single Line Text'
                                                                                    || Item.QuestionType=== 'Multi Line Text'
                                                                                    || Item.QuestionType==='Cascading Dropdown'
                                                                                    || Item.QuestionType==='Date Picker'
                                                                                    || Item.QuestionType==='Note'
                                                                                    || Item.QuestionType==='Numeric' || Item.QuestionType==='Dropdown'))">
                                                                                                    {{Item.Answer}}
                                                                                                </div>
                                                                                                <!--Single Choice -->
                                                                                                <div v-if="Item.QuestionType==='Single Choice'" class="mt-0">
                                                                                                    <div v-for="(optItem,id) in Item1.QuestionOptionList" :key="id">
                                                                                                        <label>
                                                                                                            <input type="radio" disabled=""
                                                                                                                   :checked="optItem.OptionValue == Item.Answer" />                                                                             {{optItem.OptionValue}}
                                                                                                        </label>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <!--Likert -->
                                                                                                <div v-if="Item.QuestionType==='Likert Scale'" class="mt-0">
                                                                                                    <div v-for="(optItem,id) in Item1.QuestionOptionList" :key="id">
                                                                                                        <label style="font-size:16px">
                                                                                                            <input type="radio" :label="optItem.OptionValue" disabled=""
                                                                                                                   :checked="optItem.OptionValue == Item.Answer" />                                                                             {{optItem.OptionValue}}
                                                                                                        </label>

                                                                                                    </div>
                                                                                                </div>
                                                                                                <!--Multiple Choice -->
                                                                                                <div v-if="Item.QuestionType==='Multi-Choice'" class="mt-0">
                                                                                                    <div v-for="(optItem,ind) in fnCommaseperatedMultichoice(Item1.QuestionOptionList,Item.Answer)" :key="ind">
                                                                                                        <label style="font-size:16px">
                                                                                                            <input type="checkbox" disabled="" :checked="optItem.IsChecked" />
                                                                                                            {{optItem.OptionValue}}
                                                                                                        </label>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <!---Audio answers -->
                                                                                                <v-row v-if="Item.QuestionType==='Audio Capture'">
                                                                                                    <v-col cols="12" class="d-md-flex ">
                                                                                                        <div class="d-md-flex">
                                                                                                            <div class="pb-4">
                                                                                                                <audio controls="" style="min-width:150px"><source :src="fileUrl+Item.Answer" type="audio/mp3"></audio>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <!--Gps answers-->
                                                                                                <v-row v-if="Item.QuestionType==='GPS Capture'">
                                                                                                    <v-col cols="12" class="d-md-flex">
                                                                                                        <div class="d-md-flex">
                                                                                                            <div class="pb-4">
                                                                                                                <GmapMap :center="{lat:parseInt(Item.Answer.split(',')[0]), lng:parseInt(Item.Answer.split(',')[1])}"
                                                                                                                         :zoom="7"
                                                                                                                         style="width: 350px; height: 175px">
                                                                                                                    <GmapMarker :position="{lat:parseInt(Item.Answer.split(',')[0]), lng:parseInt(Item.Answer.split(',')[1])}"
                                                                                                                                :clickable="true"
                                                                                                                                :draggable="true" />
                                                                                                                </GmapMap>
                                                                                                            </div>
                                                                                                            <div class="pl-md-4">
                                                                                                                <h4 class="font-weight-medium black--text">GPS Capture</h4>
                                                                                                                <div class="d-flex pt-4">
                                                                                                                    <div>
                                                                                                                        <h5 class="font-weight-regular black--text">Latitude</h5>
                                                                                                                        <h5 class="font-weight-regular">{{Item.Answer.split(',')[0]}}</h5>
                                                                                                                    </div>
                                                                                                                    <div class="pl-9">
                                                                                                                        <h5 class="font-weight-regular black--text">Longitude</h5>
                                                                                                                        <h5 class="font-weight-regular">{{Item.Answer.split(',')[1]}}</h5>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </v-col>
                                                                                                </v-row>

                                                                                                <!--Image Capture answers-->
                                                                                                <v-row v-if="Item.QuestionType==='Photo Capture'">
                                                                                                    <v-col cols="12" class="d-md-flex">
                                                                                                        <div class="d-md-flex">
                                                                                                            <div class="pb-4">
                                                                                                                <v-img :src=fileUrl+Item.Answer style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <!-- file upload-->
                                                                                                <v-row v-if="Item.QuestionType==='File Upload'">
                                                                                                    <v-col cols="12" class="d-md-flex">
                                                                                                        <div class="d-md-flex">
                                                                                                            <div class="pb-4" style="cursor: pointer" v-if="fnDoumentFileType(Item.Answer) == 'PDF'">
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img src="../../images/pdf_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                                <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                            </div>
                                                                                                            <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(Item.Answer) == 'XLSX' || fnDoumentFileType(Item.Answer) == 'CSV'">
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img src="../../images/excel_thumbnail.png" style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                                <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                            </div>
                                                                                                            <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(Item.Answer) == 'TIFF' || fnDoumentFileType(Item.Answer) == 'JPEG' || fnDoumentFileType(Item.Answer) == 'JPG' || fnDoumentFileType(Item.Answer) == 'GIF' || fnDoumentFileType(Item.Answer) == 'PNG' || fnDoumentFileType(Item.Answer) == 'JFIF'">
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img :src=fileUrl+Item.Answer style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                            </div>
                                                                                                            <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(Item.Answer) == 'DOCX'">
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img src="../../images/doc_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                                <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                            </div>
                                                                                                            <div class="pb-4" style="cursor: pointer" v-else-if="fnDoumentFileType(Item.Answer) == 'TXT'">
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img src="../../images/txt_thumbnail.png" style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                                <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->
                                                                                                            </div>
                                                                                                            <div class="pb-4" style="cursor: pointer" v-else>
                                                                                                                {{Item.Answer}}
                                                                                                                <v-img src="../../images/no_image_thumbnail.jpg" style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                                <!--<v-img :src=fileUrl+item.Answer style="max-width:100px" @click="GotoNewTab(item.Answer)"></v-img>-->

                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                                <!--Signature Capture answers-->
                                                                                                <v-row v-if="Item.QuestionType==='Signature Capture'">
                                                                                                    <v-col cols="12" class="d-md-flex">
                                                                                                        <div class="d-md-flex">
                                                                                                            <div class="pb-4">
                                                                                                                <v-img :src=fileUrl+Item.Answer style="max-width:100px" @click="GotoNewTab(Item.Answer)"></v-img>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </v-col>
                                                                                                </v-row>
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
                                        </v-col>
                                    </v-row>
                                </v-card-text>
                            </v-card>
                            <template v-else>
                                <ResponsesGridView :responseMode="true" class="mx-2" height="500px"/>
                            </template>
                        </v-tab-item>
                    </v-tabs>
                </v-card>
            </v-col>
        </v-row>

        <!--survey share link pop-up-->
        <v-dialog v-model="shareLinkPopup" width="600" persistent="">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    Send Survey
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseShareLink" id="" text="" @click="shareLinkPopup=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                      
                    </v-btn>
                </v-card-title>
                <v-card-text style="background:#F7F6EF">
                       <v-row class="pa-0 ma-0">
                           <v-col class="pa-0 ma-0">
                               <!--<v-checkbox class="pa-0 ma-0" v-model="collectEmailCheckBox" color="primary"
                                           label="Collect Emails"></v-checkbox>-->
                               <small class="pa-0">This link is set public anyone with this link can complete the surevey</small>
                           </v-col>
                       </v-row>
                       <v-row class="pa-0 ma-0">
                           <v-col cols="12" class="pa-0 ma-0">
                               <p> Share Link</p>
                           </v-col>
                           <v-col class="pa-0 ma-0 d-flex">
                               <v-text-field v-model="ShareLinkURL"
                                             outlined
                                             readonly
                                             dense
                                             ref="shareLinkText"></v-text-field>
                               <v-btn name="btnCopyShareLink" id="" lable="" @click="copyText" class="pa-0 ma-0 ml-4" outlined>
                                   <v-icon size="15">mdi-content-copy</v-icon>
                                   <span>Copy</span>
                               </v-btn>
                           </v-col>
                       </v-row>
                    <v-row class="pa-0 ma-0">
                        <v-col class="pa-0 ma-0">
                            <v-switch name="toggleBtnActiveLink" v-model="LinkActiveOrNot" color="primary" class="pa-0 ma-0" dense flat
                                      label="Link Active"></v-switch>
                            <small>This toggle controls when the survey is allowed to be filled in Switching it off will not allow anyone with the survey link to fill</small>
                        </v-col>
                    </v-row>

                   
                </v-card-text>
                <v-card-actions class="pa-0 ma-0">
                    <v-col>
                        <!--<v-btn @click.native="fnRouteToNewTaskPage()" class="mr-4 float-left primary">go to sharelinkpage</v-btn>-->
                        <Secondary-Button name="btnSaveShareLink" id="" class="btn-122x36 float-right mr-4" title="Save" @click.native="SaveShareLinkStatus(),shareLinkPopup=false"></Secondary-Button>
                        <Default-Button-Outlined name="btnCancelShareLink" id="" class="btn-122x36 float-right mr-4" title="Cancel" @click.native="shareLinkPopup=false"></Default-Button-Outlined>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <!--Edit Survey -->
        <v-dialog v-model="dlgEditSurvey" width="800" persistent="">
            <v-card>
                <v-card-title primary-title="" class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                        <v-icon size="20" color="primary">mdi-cash</v-icon>
                    </v-avatar>
                    Edit Survey
                    <v-spacer></v-spacer>
                    <v-btn name="btnEditSurveyClose" id="AutoTest_IP691" text="" @click="dlgEditSurvey=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Please enter the basic details to edit a survey</h4>
                    <v-row justify="center">
                        <v-col md="6" class="v-scrolling-div mt-4" style="height:50vh">

                            <v-text-field name="txtEditSurveyName" outlined="" dense="" label="Survey Name" :rules="[rules.required]" v-model=" SurveyName">
                            </v-text-field>
                            <v-textarea name="txtEditSurveyDescription" outlined="" dense="" label="Enter Description" value="" :rules="[rules.required]" v-model="SurveyDescription"></v-textarea>

                            <v-select id="drpEditSurveyWorkflow" dense="" :items="lstWorkFlow" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Workflow" outlined="" v-model="WorkflowID" :rules="[rules.required]">
                            </v-select>
                            <v-row>
                                <v-col md="6">
                                    <v-menu v-model="mStartDate"
                                            :close-on-content-click="false"
                                            :nudge-right="40"
                                            transition="scale-transition"
                                            offset-y
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field name="dtEditSurveyStartDate" v-model="computedStartDate"
                                                          label="Start Date"
                                                          outlined dense
                                                          readonly
                                                          v-on="on"></v-text-field>
                                        </template>
                                        <v-date-picker v-model="startDate" no-title @input="mStartDate = false,IsValidDate()" format="DD-MM-YYYY"
                                                       :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                    </v-menu>
                                </v-col>
                                <v-col md="6">
                                    <v-menu v-model="mEndDate"
                                            :close-on-content-click="false"
                                            :nudge-right="40"
                                            transition="scale-transition"
                                            offset-y
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field name="dtEditSurveyEndDate" v-model="computedEndDate"
                                                          label="End Date"
                                                          outlined dense
                                                          readonly
                                                          v-on="on"></v-text-field>
                                        </template>
                                        <v-date-picker v-model="endDate" no-title @input="mEndDate = false,IsValidDate()"
                                                       :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                    </v-menu>
                                </v-col>
                            </v-row>
                            <v-select id="drpEditReportingFrequency" dense :items="lstPeriodicity" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Reporting Frequency" outlined="" v-model="ReportingFrequency" :rules="[rules.required]">
                            </v-select>

                            <!-- Once -->
                            <div v-show="ReportingFrequency === 2">
                                <v-menu v-model="FrmDateMenu"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y
                                        max-width="290px"
                                        min-width="290px">
                                    <template v-slot:activator="{ on }">
                                        <v-text-field v-model="FormattedDate"
                                                      label="Choose a Date" dense
                                                      persistent-hint outlined hide-details
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="FrmDate" no-title @input="FrmDateMenu = false,IsValidPeriodicityDate()" dense></v-date-picker>
                                </v-menu>
                            </div>

                            <!-- Weekly-->
                            <div v-show="ReportingFrequency === 4">
                                <v-select dense="" dense :items="lstWeek" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="ReportingWeek" hide-details>
                                </v-select>
                            </div>

                            <!-- Monthly-->
                            <div v-show="ReportingFrequency === 5">
                                <v-select dense :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDay" hide-details>
                                </v-select>
                            </div>

                            <!-- Quaterly , Halyearly and Yearly-->
                            <div class="d-inline-flex">
                                <v-select class="mr-1" hide-details dense :items="lstMonth" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="ReportingMonth" v-show="ReportingFrequency === 6|| ReportingFrequency === 7 ||  ReportingFrequency === 8">
                                </v-select>
                                <v-select hide-details dense :items="lstMonthDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="ReportingMonthDateDay" v-show="ReportingFrequency === 6|| ReportingFrequency === 7 ||  ReportingFrequency === 8">
                                </v-select>
                            </div>
                            <p>
                                Beneficiary Linked
                                <v-switch name="toggleBtnBeneficiaryLinked" hide-details class="ml-2" style="display:inline-block"
                                          color="secondary" v-model="BeneficiaryLinked"></v-switch>
                            </p>
                            <v-select id="drpBeneficiaryTypeFilter" v-show="BeneficiaryLinked" dense="" :items="lstBeneficiaryType" :item-text="i => i.Name" :item-value="i => i.ID" label="Select Beneficiary Type" outlined="" v-model="BeneficiaryTypeID">
                            </v-select>

                            <!-- Select User -->
                            <span class="small-text mr-1">
                                <v-icon size="15">mdi-account</v-icon> Users
                            </span>
                            <!--<v-avatar size="27" class="team-avatar" v-if="selectedteamUsers == 0">
                            <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="Notify" />
                        </v-avatar>
                        <v-avatar size="27" class="team-avatar" color="red" v-if="selectedteamUsers.length == 1">
                            <span class="text--white">{{selectedteamUsers[0].Name.charAt(0)}}</span>
                        </v-avatar>
                        <v-avatar size="27" class="team-avatar" color="green" v-if="selectedteamUsers.length >= 2">
                            <span class="text--white">{{selectedteamUsers[0].Name.charAt(0)}}</span>
                        </v-avatar>
                        <v-avatar size="27" class="team-avatar" color="red" v-if="selectedteamUsers.length >= 2">
                            <span class="text--white">  {{selectedteamUsers[1].Name.charAt(0)}}</span>
                        </v-avatar>

                        <v-avatar size="27" class="team-avatar" color="red" v-if="selectedteamUsers.length > 2">
                            <span class="text--white"> + {{selectedteamUsers.length -2}}</span>
                        </v-avatar>-->
                            <span v-if="selectedteamUsers.length != 0">
                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in selectedteamUsers.slice(0,3)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                    <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                </v-avatar>
                                <v-avatar v-if="selectedteamUsers.length>=4" size="25" class="team-count">
                                    +{{selectedteamUsers.length-3}}
                                </v-avatar>
                            </span>

                            <v-menu style="height:500px" absolute="" v-model="AssignAccountUserPopUp" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                <template v-slot:activator="{ on }">
                                    <v-btn name="btnAddSurveyUser" id="AutoTest_IP692" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" @click="OpenAssignUserDialog()">
                                        <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                    </v-btn>
                                </template>
                                <!--Assign Account User pop up-->
                                <v-card>
                                    <v-card-title class="heading-3">
                                        Add People to Notify
                                        <v-spacer></v-spacer>
                                        <v-btn id="AutoTest_IP693" text="" height="25" min-width="25" max-width="25" color="#707070" @click="CloseAssignUserDialog()">
                                            <v-icon size="20">mdi-close</v-icon>
                                        </v-btn>
                                    </v-card-title>
                                    <v-divider></v-divider>
                                    <v-card-text style="height:150px" class="v-scrolling-div">
                                        <v-data-table :headers="lstuserTableheaders"
                                                      :items="lstUsers"
                                                      show-select
                                                      item-key="UserID"
                                                      sort-by="Name"
                                                      v-model="selectedteamUsers1">
                                            <template v-slot:item.Name="{item}">
                                                <v-list class="py-0 table-v-list">
                                                    <v-list-item class="my-0 px-0">
                                                        <v-list-item-icon>
                                                            <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                            </v-avatar>
                                                        </v-list-item-icon>
                                                        <v-list-item-content>
                                                            {{ item.Name }}
                                                        </v-list-item-content>
                                                    </v-list-item>
                                                </v-list>
                                            </template>
                                        </v-data-table>
                                    </v-card-text>
                                    <v-divider></v-divider>
                                    <v-card-actions>
                                        <v-row no-gutters="">
                                            <v-col md="12" class="py-0" align="center">
                                                <Success-Button name="btnSaveUser" id="AutoTest_IP694" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AssignUserToSurvey()"></Success-Button>
                                            </v-col>
                                        </v-row>
                                    </v-card-actions>
                                </v-card>
                                <!--_______________________________________-->
                            </v-menu>


                            <!-- Tags-->
                            <v-row class="mb-1">
                                <v-col class="text-left d-inline-flex" md="12">
                                    <v-icon size="16">mdi-tag-multiple</v-icon> <span class="mr-1">Tags</span>
                                    <v-menu absolute v-model="NewTagMenu" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                        <template v-slot:activator="{ on }">
                                            <v-btn id="AutoTest_IP696" v-on="on" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey"
                                                   @click="fnInitializeSteper()" class="mr-1">
                                                <v-icon size="15">mdi-plus</v-icon>
                                            </v-btn>
                                        </template>
                                        <v-card>
                                            <v-card-text class="pt-0 pb-1">
                                                <v-col cols="12" md="12">
                                                    <!-- Header Text  -->
                                                    <v-col cols="12" md="12" class="d-inline-flex">
                                                        <v-btn id="AutoTest_IP697" text @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                            <v-icon size="15">mdi-chevron-left</v-icon>
                                                        </v-btn>
                                                        <v-spacer></v-spacer>
                                                        <h3 class="text-left page-head pt-2">
                                                            {{TagHeaderName}}
                                                        </h3>
                                                        <v-spacer></v-spacer>
                                                        <v-btn id="AutoTest_IP698" text @click="NewTagMenu = false,SearchTagDetails=[]">
                                                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                        </v-btn>
                                                    </v-col>
                                                    <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                        <v-stepper-items>

                                                            <!-- Create New Tag Option when there is no Tags are -->
                                                            <v-stepper-content step="1" class="pa-0">
                                                                <v-row>
                                                                    <v-col class="pb-1 ml-2">
                                                                        <v-text-field outlined type="text" dense
                                                                                      label="Name" v-model="NewTagName" hide-details>
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row class="pt-0">
                                                                    <v-col class="pb-1">
                                                                        <span class="ml-3">Select Color</span>
                                                                        <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                        :swatches="swatches"
                                                                                        hide-canvas
                                                                                        hide-inputs
                                                                                        hide-mode-switch
                                                                                        show-swatches
                                                                                        v-model="color"
                                                                                        @input="">
                                                                        </v-color-picker>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col cols="12" md="12" class="text-center">
                                                                        <v-btn id="AutoTest_IP699" depressed color="success-btn" dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn> <!-- Adding To Existing Tag List-->
                                                                    </v-col>
                                                                </v-row>
                                                            </v-stepper-content>

                                                            <!-- Select from existing Tags-->
                                                            <v-stepper-content step="2" class="pa-0">

                                                                <v-row style="max-height:30vh;overflow-y:scroll">
                                                                    <v-col col="12" md="12" class="mb-0 pt-0">
                                                                        <p class="mb-2">Search Tags</p>
                                                                        <v-text-field outlined="" dense="" v-model="SearchTagDetails" label="Search tag here" hide-details class="mb-3">
                                                                        </v-text-field>
                                                                        <v-chip-group dark multiple column active-class="primary--text" style="max-width:350px">
                                                                            <v-chip id="AutoTest_IP700" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small
                                                                                    label @click.native="fnAddToSelectedTag(index)">
                                                                                {{ index.TagName }}
                                                                                <v-icon id="AutoTest_IP701" right color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                            </v-chip>
                                                                        </v-chip-group>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-col cols="12" md="12" class="text-center mt-3">
                                                                    <v-btn id="AutoTest_IP702" depressed color="success-btn" @click="AllTagListSteps = 1" dark>Create New</v-btn>
                                                                </v-col>
                                                            </v-stepper-content>

                                                            <!-- Edit Tag-->
                                                            <v-stepper-content step="3" class="pa-0">
                                                                <v-row>
                                                                    <v-col class="pb-1 ml-2">
                                                                        <v-text-field outlined type="text" dense
                                                                                      v-model="EditTagName" hide-details>
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row class="pt-0">
                                                                    <v-col class="pb-1">
                                                                        <span class="ml-3">Select Color</span>
                                                                        <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                        :swatches="swatches"
                                                                                        hide-canvas
                                                                                        hide-inputs
                                                                                        hide-mode-switch
                                                                                        show-swatches
                                                                                        v-model="color"
                                                                                        @input="">
                                                                        </v-color-picker>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col cols="12" md="12" class="text-center">
                                                                        <v-btn id="AutoTest_IP703" depressed color="success-btn" dark @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-stepper-content>

                                                        </v-stepper-items>
                                                    </v-stepper>
                                                </v-col>
                                            </v-card-text>
                                        </v-card>
                                    </v-menu>
                                    <div v-if="!allTags">
                                        <!--<v-chip v-for="basicTag in AllTagList" :key="basicTag.TagID" close @click:close="(basicTag.TagID = false)" v-show ="basicTag.TagID" :color="basicTag.TagColor" label small class="tags mr-2">{{basicTag.TagName}}</v-chip>-->
                                        <v-chip dark v-if="SelectedTag.length>0" :color="SelectedTag[0].TagColor" label small class="tags mr-1">{{SelectedTag[0].TagName}}</v-chip>
                                        <v-chip dark v-if="SelectedTag.length>1" :color="SelectedTag[1].TagColor" label small class="tags mr-1">{{SelectedTag[1].TagName}}</v-chip>
                                        }<
                                        <v-chip dark v-if="SelectedTag.length>2" label small class="tags tag-count mr-1">+{{SelectedTag.length-2}}</v-chip>
                                    </div>
                                    <v-btn id="AutoTest_IP704" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                        <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                    </v-btn>
                                    <v-btn id="AutoTest_IP705" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                        <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                    </v-btn>
                                </v-col>
                            </v-row>
                            <v-row no-gutter v-if="allTags && SelectedTag.length > 0" class="mt-0">
                                <v-col md="12" class="chip-container mb-0 pt-0">
                                    <v-chip-group dark v-for="TagItem in SelectedTag" :key="TagItem.TagID" multiple column active-class="primary--text" class="d-inline-flex">
                                        <v-chip id="AutoTest_IP706" small :color="TagItem.TagColor" close @click:close="fnRemoveFromSelectedTagItem(TagItem)" class="mb-0">
                                            {{TagItem.TagName}}
                                        </v-chip>
                                    </v-chip-group>
                                </v-col>
                            </v-row>
                        </v-col>

                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>

                        <!--<Cancel-Button-Outlined-Sm class="float-right" @click.native="dlgEditSurvey=false"></Cancel-Button-Outlined-Sm>-->
                        <Default-Button-Outlined name="btnCancelSurvey" id="AutoTest_IP707" class="btn-122x36 float-right mr-4" title="Cancel" @click.native="ClearBindData(),dlgEditSurvey=false"></Default-Button-Outlined>
                        <Secondary-Button name="btnUpdateSurvey" id="AutoTest_IP708" class="btn-122x36 float-right mr-4" title="Save" @click.native="UpdateProjectSurvey(),dlgEditSurvey=false" v-bind:disabled="fnCheckMandatory()"></Secondary-Button>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!--Add Section-->
        <v-dialog v-model="dlgAddSection" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi mdi-file-document-box-check-outline</v-icon>
                    </v-avatar>
                    Add Section
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseSurveySection" id="AutoTest_IP709" text="" @click="dlgAddSection=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-row :justify="justifyCenter">
                        <v-col md="12">
                            <h4 class="text-center heading-3">Please enter basic details to add a new section.</h4>
                        </v-col>
                        <v-col md="6">
                            <v-text-field name="txtSurveySectionName" outlined="" dense="" label="Enter Section Name" v-model="vmSectionName"></v-text-field>
                            <v-responsive w="100%"></v-responsive>
                            <v-textarea name="txtSurveySectionDescription" outlined dense label="Enter Description" hide-details v-model="vmSectionDesc"></v-textarea>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions>
                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right pt-0 pb-4">
                        <Default-Button-Outlined name="btnCancelSurveySection" id="AutoTest_IP710" title="Cancel" class="float-right btn-122x36" @click.native="dlgAddSection=false,vmSectionName = '',vmSectionDesc = ''"></Default-Button-Outlined>
                        <Secondary-Button name="btnConfirmSurveySection" id="AutoTest_IP711" title="Confirm" class="float-right mr-4 btn-122x36" @click.native="SaveSurveySection(0)"
                                          v-bind:disabled="vmSectionName.length == 0 || vmSectionName.trim() == ''"></Secondary-Button>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-snackbar v-model="successSnackbarMsg" top color="success" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert name="successAlert" dense type="success" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="errorSnackbarMsg" top color="red" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert name="errorAlert" dense type="error" color="red" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <v-snackbar v-model="infoSnackbarMsg" top color="primary" :max-height="30" class="pa-0">
            <template>
                <div class="pa-0 ma-0">
                    <v-alert name="infoAlert" dense type="info" color="primary" class="pa-0 ma-0">{{Msg}}</v-alert>
                </div>
            </template>
        </v-snackbar>
        <!-- Remove Survey-->
        <v-dialog v-model="dlgRemoveSurvey" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Survey
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP712" text="" @click="dlgRemoveSurvey=false">
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
                                            <h4 class="text-center heading-3">Are you sure you want to remove this Survey ?</h4>
                                        </v-col>
                                        <v-col>
                                            <h4 class="text-center heading-3">
                                                <v-icon class="tmr-2" color="#707070" size="25">mdi-file-document-box-check-outline</v-icon>
                                                {{SurveyName}}
                                            </h4>
                                        </v-col>

                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn name="btnRemoveSurvey" id="AutoTest_IP713" color="secondary elevation-0" class="mr-2" @click="RemoveProjectSurvey(),dlgRemoveSurvey=false">Remove</v-btn>
                                            <v-btn id="AutoTest_IP714" outlined color="grey" @click="dlgRemoveSurvey=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <!-- Edit Section Dialog-->
        <v-dialog v-model="dlgEditSection" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-file-document-edit-outline</v-icon>
                    </v-avatar>
                    Edit Section
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseSurveySection" id="AutoTest_IP715" text @click="dlgEditSection=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text>
                    <h4 class="text-center heading-3 mt-4">Edit the basic details of the section.</h4>
                    <v-row :justify="justifyCenter" class="pt-4">
                        <v-col md="6" style="height:250px" class="v-scrolling-div">
                            <v-text-field name="txtEditSurveySectionName" placeholder="Enter Section Name" class="mt-4" outlined dense v-model="vmSectionName"></v-text-field>
                            <v-textarea name="txtEditSurveySectionDescription" placeholder="Enter Section Description" outlined v-model="vmSectionDesc"></v-textarea>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Default-Button-Outlined name="btnCancelSurveySection" id="AutoTest_IP716" title="Cancel" class="float-right btn-122x36" @click.native="dlgEditSection=false"></Default-Button-Outlined>
                        <Secondary-Button name="btnConfirmSurveySection" id="AutoTest_IP717" title="Confirm" class="float-right mr-4 btn-122x36" @click.native="SaveSurveySection(SectionID),dlgEditSection=false"></Secondary-Button>
                    </v-col>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!-- Remove Section-->
        <v-dialog v-model="dlgRemoveSection" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Section
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseSectionOperation" id="AutoTest_IP718" text="" @click="dlgRemoveSection=false">
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
                                            <h4 class="text-center heading-3">Are you sure you want to remove this section ?</h4>
                                        </v-col>
                                        <v-col>
                                            <h4 class="text-center heading-3">
                                                <v-icon class="tmr-2" color="#707070" size="25">mdi-file-document-box-check-outline</v-icon>
                                                {{SectionName}}
                                            </h4>
                                        </v-col>

                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn name="btnRemoveSection" id="AutoTest_IP719" color="secondary elevation-0" class="mr-2" @click="RemoveSurveySection(SectionID),dlgRemoveSection=false">Remove</v-btn>
                                            <v-btn name="btnCancelSectionOperation" id="AutoTest_IP720" outlined color="grey" @click="dlgRemoveSection=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <!--Section Setting Dialog-->
        <v-dialog v-model="dlgSecSettings" width="800" persistent>
            <v-card>
                <v-card-text>
                    <v-tabs fixed-tabs color="tab-active-line" v-model="SectionSettingTab">
                        <v-tab href='#tab-1'>Setting</v-tab>
                        <v-tab href='#tab-2'>Logic</v-tab>
                        <v-tab-item value="tab-1">
                            Setting content
                        </v-tab-item>
                        <!--Logic Tab-->
                        <v-tab-item class="mt-4" value="tab-2">
                            <h3 class="heading-3 black--text mb-4">Please select logic type :</h3>
                            <v-select id="drpLogic" v-model="vmLogicType" label="Select Logic"
                                      :items="sectionSettingLogicItems" outlined dense style="width:200px;" @change="GetAllQstnsForSectionsLogic(SectionID)"></v-select>
                            <!-- Skip logic-->
                            <div v-show="vmLogicType==='Skip Logic'">
                                <v-row class="v-scrolling-div" style="max-height:30vh">
                                    <v-col>
                                        <div v-if="lstLogicConditionItems.length >0">
                                            <div v-for="(item, index) in lstLogicConditionItems" :key="item.QuestionID">

                                                <!-- And / Or Radio Button-->
                                                <v-row :justify="justifyCenter" no-gutters v-show="index >0">

                                                    <v-radio-group name="radioBtn" row v-model="item.LogicOperator" @change="ChangeLogicOperator(item.LogicOperator,index)">
                                                        <v-radio name="radioBtnAnd" label="AND" hide-details></v-radio>
                                                        <v-radio name="radioBtnOR" label="OR" hide-details></v-radio>
                                                    </v-radio-group>

                                                </v-row>

                                                <v-row class="d-inline-flex" no-gutters>

                                                    <v-col>
                                                        <v-select id="drpQuestions" dense :items="lstLogicQstns" v-model="item.QuestionID"
                                                                  item-text="QuestionName"
                                                                  item-value="QuestionID"
                                                                  label="Select Question" @change="GetLogicAnswer(item.QuestionID,index)" hide-details>
                                                        </v-select>

                                                    </v-col>
                                                    <v-col>

                                                        <v-select id="drpCondition" dense :items="lstLogicConditions" v-model="item.ConditionID"
                                                                  item-text="Condition"
                                                                  item-value="ConditionID"
                                                                  label="Select Condition" hide-details>
                                                        </v-select>
                                                    </v-col>

                                                    <v-col>
                                                        <v-select id="drpValueSkipLogic" dense :items="lstLogicAnswers" v-model="item.Answer"
                                                                  item-text="OptionValue"
                                                                  item-value="OptionID"
                                                                  :label="item.OptionValue" hide-details v-if="logicAnsType[index]=='Single Choice' || logicAnsType[index]=='Multi-Choice'||logicAnsType[index]=='Dropdown' ">
                                                        </v-select>
                                                        <v-text-field name="txtValueSkipLogic" dense v-model="item.Answer" v-else></v-text-field>
                                                    </v-col>
                                                    <v-col md="auto">
                                                        <Default-Icon-Button-Outlined name="btnDeleteSkipLogicCondition" id="AutoTest_IP721" hide-details class="mr-4 float-left" icon="mdi-trash-can-outline"
                                                                                      @click.native="DeleteLogicItem(item)"></Default-Icon-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </div>
                                        </div>

                                        <div v-if="lstLogicConditionItems.length ==0 || AddNew==true">
                                            <v-row :justify="justifyCenter" no-gutters v-if="lstLogicConditionItems.length >0">

                                                <v-radio-group name="radioBtn" row v-model="vmOperatorValue">
                                                    <v-radio name="radioBtnAnd" label="AND" hide-details></v-radio>
                                                    <v-radio name="radioBtnOR" label="OR" hide-details></v-radio>
                                                </v-radio-group>

                                            </v-row>
                                            <v-row class="d-inline-flex" no-gutters>

                                                <v-col>
                                                    <v-select id="drpQuestions" dense :items="lstLogicQstns" v-model="vmLogicQstn"
                                                              item-text="QuestionName"
                                                              item-value="QuestionID"
                                                              label="Select Question" @change="GetLogicAnswer(vmLogicQstn)" hide-details>
                                                    </v-select>

                                                </v-col>
                                                <v-col>

                                                    <v-select id="drpCondition" dense :items="lstLogicConditions" v-model="vmLogicCondition"
                                                              item-text="Condition"
                                                              item-value="ConditionID"
                                                              label="Select Condition" hide-details>
                                                    </v-select>
                                                </v-col>

                                                <v-col>
                                                    <v-select id="drpValueSkipLogic" v-if="logicAnsType1=='Single Choice' || logicAnsType1=='Multi-Choice'||logicAnsType1=='Dropdown' " dense :items="lstLogicAnswers" v-model="vmLogicAnswer"
                                                              item-text="OptionValue"
                                                              item-value="OptionID"
                                                              label="Select Answer" hide-details return-object>
                                                    </v-select>
                                                    <v-text-field name="txtValueSkipLogic" v-else hide-details dense v-model="vmLogicAnswer1"></v-text-field>


                                                </v-col>
                                                <v-col md="auto">
                                                    <Default-Icon-Button-Outlined name="btnDeleteSkipLogicCondition" id="AutoTest_IP722" hide-details class="mr-4 float-left" icon="mdi-trash-can-outline"
                                                                                  @click.native="DeleteLogicItem(item)"></Default-Icon-Button-Outlined>
                                                </v-col>
                                            </v-row>
                                        </div>
                                        <v-responsive width="100%"></v-responsive>
                                        <v-row>
                                            <v-col>
                                                <Primary-Button name="btnAddConditionSkipLogic" id="AutoTest_IP723" title="Add Condition" @click.native="AddNew=true,AddLogicItem()"></Primary-Button>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>

                            </div>
                            <!-- Repeat Logic-->
                            <div v-show="vmLogicType==='Repeat Logic'">
                                <v-row>
                                    <v-col class="py-0">
                                        <div v-for="(item, index) in lstRepeatLogicConditionItems" :key="item.QuestionID">
                                            <v-row>
                                                <v-col>
                                                    <!--<v-select dense outlined label="Select Question"></v-select>-->
                                                    <v-select id="drpQuestions" dense :items="lstLogicQstns" v-model="item.QuestionID"
                                                              item-text="QuestionName"
                                                              item-value="QuestionID"
                                                              label="Select Question">
                                                    </v-select>
                                                </v-col>
                                                <v-col md="auto">
                                                    <Default-Icon-Button-Outlined name="btnDeleteOption" hide-details class="mr-4 float-left" icon="mdi-trash-can-outline"
                                                                                  @click.native="DeleteRepeatLogicItem(item)"></Default-Icon-Button-Outlined>
                                                </v-col>
                                            </v-row>
                                        </div>
                                    </v-col>
                                </v-row>
                                <v-row v-show="lstRepeatLogicConditionItems.length===0">
                                    <v-col>
                                        <!--<v-select dense outlined label="Select Question"></v-select>-->
                                        <v-select dense :items="lstLogicQstns" v-model="vmRLogicQstn"
                                                  item-text="QuestionName"
                                                  item-value="QuestionID"
                                                  label="Select Question">
                                        </v-select>
                                    </v-col>
                                    <v-col md="auto">
                                        <Default-Icon-Button-Outlined name="btnDeleteOption" class="mr-4 float-left" icon="mdi-trash-can-outline" @click.native="vmRLogicQstn=''"></Default-Icon-Button-Outlined>
                                    </v-col>
                                </v-row>
                                <v-responsive width="100%"></v-responsive>
                                <!--<v-row>
                                <v-col class="py-0">
                                    <Primary-Button title="Add Condition" @click.native="AddRepeatLogicItem()"></Primary-Button>
                                </v-col>
                            </v-row>-->
                            </div>
                        </v-tab-item>
                    </v-tabs>
                </v-card-text>
                <v-card-actions>

                    <v-col>
                        <Default-Button-Outlined name="btnCancelLogic" id="AutoTest_IP724" title="Cancel" class="float-right btn-122x36" @click.native="ClearLogic()"></Default-Button-Outlined>
                        <Secondary-Button name="btnSaveLogic" id="AutoTest_IP725" title="Save" class="float-right mr-4 btn-122x36" @click.native="UpdateSectionLogic(SectionID)" v-bind:disabled="vmLogicType.length ==0"></Secondary-Button>
                    </v-col>

                </v-card-actions>
            </v-card>
        </v-dialog>
        <!-- Remove Section Question-->
        <v-dialog v-model="dlgRemoveSectionQuest" width="800" persistent>
            <v-card>
                <v-card-title primary-title="" class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Question
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseQuestionOperation" id="AutoTest_IP726" text @click="dlgRemoveSectionQuest=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid="" class="pt-4">
                        <v-row no-gutters="">
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this question ?</h4>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn name="btnRemoveQuestion" id="AutoTest_IP727" color="secondary elevation-0" class="mr-2" @click="RemoveSurveySectionQuestion(SectionID,QuestionID),dlgRemoveSectionQuest=false">Remove</v-btn>
                                            <v-btn name="btnCancelQuestionOperation" id="AutoTest_IP728" outlined="" color="grey" @click="dlgRemoveSectionQuest=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <!-- Question Copy Section-->
        <v-dialog v-model="dlgCopySectionQuestion" width="800" persistent>
            <v-card>
                <v-card-title primary-title="" class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-content-copy</v-icon>
                    </v-avatar>
                    Copy Question
                    <v-spacer></v-spacer>
                    <v-btn name="btnCloseCopyQuestion" id="AutoTest_IP729" text @click="dlgCopySectionQuestion=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid="" class="pt-4">
                        <v-row no-gutters="">
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Please select the details you want to copy to create a duplicate question</h4>
                                        </v-col>
                                        <v-row :justify="justifyCenter">
                                            <v-col cols="6" md="6" class="pt-0">
                                                <v-checkbox name="checkboxCopyQuestionSettings" hide-details input-value="true" label="Copy same settings" v-model="vmChkSettings"></v-checkbox>
                                                <v-checkbox name="checkboxCopyQuestionCalculation" hide-details input-value="true" label="Copy same calculation" v-model="vmChkCalculation"></v-checkbox>
                                                <v-checkbox name="checkboxCopyQuestionValidations" hide-details input-value="true" label="Copy same validations" v-model="vmChkValidation"></v-checkbox>
                                            </v-col>
                                        </v-row>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn name="btnConfirmCopyQuestion" id="AutoTest_IP730" color="secondary elevation-0" class="mr-2" @click="CopyQuestion(QuestionID),dlgCopySectionQuestion=false">Confirm</v-btn>
                                            <v-btn name="btnCancelCopyQuestion" id="AutoTest_IP731" outlined="" color="grey" @click="dlgCopySectionQuestion=false">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>
        <!--Question Setting Dialog-->
        <v-dialog v-model="dlgSectionQstnSetting" width="800" persistent>
            <v-card>
                <v-card-text>
                    <v-tabs fixed-tabs v-model="Tab" color="tab-active-line">
                        <v-tab href='#tab-1'>Setting</v-tab>
                        <v-tab href='#tab-2' v-bind:disabled="IsCalcavailable == false">Calculation</v-tab>
                        <v-tab href='#tab-3' v-bind:disabled="IsValavailable == false">Validation</v-tab>
                        <!--Setting Tab Content-->
                        <v-tab-item class="mt-4" value="tab-1">
                            <v-row :justify="justifyCenter">
                                <v-col md="6">
                                    <v-card elevation="0" class="v-scrolling-div" style="max-height:60vh">

                                        <v-card-text class="pa-0">
                                            <v-list dense>
                                                <v-list-item>
                                                    <h3 class="heading-4 ">Question Hint</h3>
                                                </v-list-item>
                                                <v-list-item>
                                                    <v-text-field name="txtSurveyQuestionHint" filled dense label="Hint" v-model="vmQuestionHint"></v-text-field>
                                                </v-list-item>
                                                <v-list-item>
                                                    <v-list-item-content>
                                                        Make the Question receive a mandatory response
                                                    </v-list-item-content>
                                                    <v-list-item-action>
                                                        <v-switch name="toggleBtnMandatoryQuestion" color="#707070" v-model="vmIsMandatory"></v-switch>
                                                    </v-list-item-action>
                                                </v-list-item>
                                                <v-list-item>
                                                    <h3 class="heading-4 ">Default Response</h3>
                                                </v-list-item>
                                                <v-list-item>
                                                    <v-text-field name="txtQuestionDefaultResponse" filled dense label="Response" v-model="vmDefaultVal"></v-text-field>
                                                </v-list-item>
                                                <v-list-item>
                                                    <v-list-item-content>
                                                        Make the Question and its answers searchable.
                                                    </v-list-item-content>
                                                    <v-list-item-action>
                                                        <v-switch name="toggleBtnSearchableQuestions" color="#707070" v-model="vmIsSearchable"></v-switch>
                                                    </v-list-item-action>
                                                </v-list-item>
                                                <v-list-item>
                                                    <v-list-item-content>
                                                        Make the Question and its answers unique
                                                    </v-list-item-content>
                                                    <v-list-item-action>
                                                        <v-switch name="toggleBtnUniqueQuestions" color="#707070" v-model="vmIsUnique"></v-switch>
                                                    </v-list-item-action>
                                                </v-list-item>
                                            </v-list>
                                        </v-card-text>
                                    </v-card>
                                </v-col>
                            </v-row>
                        </v-tab-item>
                        <!--Calculation Tab Content-->
                        <v-tab-item class="mt-4" value="tab-2" v-bind:disabled="vmQstnCategory!=1">
                            <v-row :justify="justifyCenter" v-bind:disabled="vmQstnCategory!=1">
                                <v-col md="10" class="pb-0">
                                    <h3 class="heading-4 mb-2">Formula Operation</h3>
                                    <v-select id="drpFromulaOperation" dense outlined :items="FormulaOperationItems" label="Select formula operation" v-model="vmFormulaOperation" item-text="Value" @change="CalculateFormulaExpression()"></v-select>
                                </v-col>
                                <v-responsive width="100%"></v-responsive>
                                <template v-if="vmFormulaOperation === 'Generic_Formula'">
                                    <v-col md="10" class="v-scrolling-div" style="max-height:50vh">
                                        <v-simple-table fixed-header height="20vh">
                                            <template v-slot:default>
                                                <thead>
                                                    <tr>
                                                        <th class="text-left">
                                                            Questions
                                                        </th>
                                                        <th class="text-left">
                                                            Variables
                                                        </th>
                                                        <th class="text-right">
                                                            Action
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr v-for="item in QstnCalcVarItems"
                                                        :key="item.FieldVar">

                                                        <td>{{item.QuestionName}}</td>
                                                        <td>
                                                            {{item.FieldVar}}
                                                        </td>
                                                        <td>
                                                            <Default-Icon-Button-Outlined name="btnCopyVariable" hide-details class="float-right" v-clipboard="item.FieldVar" icon="mdi mdi-content-copy" v-clipboard:success="onCopy" v-clipboard:error="onError"></Default-Icon-Button-Outlined>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </template>
                                        </v-simple-table>
                                        <v-textarea name="txtFormulaExpression" label="Formula Expression" v-model="vmFormulaExp" outlined dense hide-details class="mt-4"></v-textarea>
                                    </v-col>
                                </template>
                                <template v-else>
                                    <v-col md="10" class="v-scrolling-div" style="max-height:60vh">
                                        <!--<h3 class="heading-4 mb-2">Formula Operation</h3>
                                    <v-select dense outlined :items="FormulaOperationItems" label="Select formula operation" v-model="vmFormulaOperation"
                                              item-text="Value"
                                              @change="CalculateFormulaExpression()"></v-select>-->
                                        <h3 class="heading-4 mb-2">Variable</h3>

                                        <!--<v-select dense outlined :items="QstnCalcVarItems"
                                              item-text="QuestionName"
                                              item-value="FieldVar"
                                              label="Select formula operation" v-model="vmQstnCalcVar">
                                        <template v-slot:append-outer>
                                            <Default-Icon-Button-Outlined class="mr-2" icon="mdi-trash-can-outline"></Default-Icon-Button-Outlined>
                                            <Default-Icon-Button-Outlined icon="mdi-plus" @click.native="AddQstnCalcVariables()"></Default-Icon-Button-Outlined>
                                        </template>
                                    </v-select>-->
                                        <div v-if="lstFieldVars.length>0">
                                            <div v-for="(item, index) in lstFieldVars" :key="item.FieldVar">
                                                <v-row>
                                                    <v-col>
                                                        <v-select id="drpVariables" dense :items="QstnCalcVarItems" v-model="item.FieldVar"
                                                                  item-text="QuestionName"
                                                                  item-value="FieldVar"
                                                                  label="Select Question" outlined="" hide-details>
                                                        </v-select>
                                                    </v-col>
                                                    <v-col md="auto">
                                                        <Default-Icon-Button-Outlined name="btnDeleteVariable" id="AutoTest_IP732" hide-details class="float-left" icon="mdi-trash-can-outline"
                                                                                      @click.native="DeleteFieldVarItem(item)"></Default-Icon-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </div>
                                        </div>
                                        <v-row>
                                            <v-col>
                                                <v-select id="drpVariables" dense :items="QstnCalcVarItems" v-model="vmQstnCalcVar"
                                                          item-text="QuestionName"
                                                          item-value="FieldVar"
                                                          label="Select Question" outlined="" hide-details return-object>
                                                </v-select>
                                            </v-col>
                                            <v-col md="auto">
                                                <Default-Icon-Button-Outlined name="btnAddVariable" id="AutoTest_IP733" icon="mdi-plus" @click.native="AddQstnCalcVariables()"></Default-Icon-Button-Outlined>
                                            </v-col>
                                        </v-row>
                                    </v-col>

                                    <v-responsive width="100%"></v-responsive>
                                    <v-col md="10" class="py-0">
                                        <Primary-Button name="btnConfirmFormula" id="AutoTest_IP734" title="Confirm Formula" @click.native="CalculateFormulaExpression()"
                                                        v-bind:disabled="vmFormulaOperation =='null' ||vmFormulaOperation =='' ||vmFormulaOperation ==null || vmFormulaOperation.length ==0"></Primary-Button>
                                        <br></br>
                                        <!--Formulation Progress section-->
                                        <!--<template>
                                        <v-progress-circular indeterminate=""
                                                             color="primary"></v-progress-circular> Formulating...
                                    </template>-->
                                        <br></br>
                                        <!--Formulation Result section-->
                                        <template>
                                            <v-chip color="#EEEEEE" label v-if="(vmFormulaExp !=null && vmFormulaExp.length > 0)">
                                                {{vmFormulaExp}}
                                            </v-chip>
                                        </template>
                                    </v-col>
                                </template>
                            </v-row>
                        </v-tab-item>
                        <!--Validation Tab Content-->
                        <v-tab-item class="mt-4" value="tab-3" v-bind:disabled="vmQstnCategory!=1">
                            <v-row :justify="justifyCenter" v-bind:disabled="vmQstnCategory!=1">
                                <v-col md="6" class="v-scrolling-div" style="max-height:60vh">
                                    <h3 class="heading-4 mb-2">Validation REGEX Expression</h3>
                                    <v-textarea name="txtValidationRegexExpression" filled label="Enter validation REGEX expression..." v-model="vmRegEx"></v-textarea>
                                    <h3 class="heading-4 mb-2">Validation Condition</h3>
                                    <div>
                                        <div v-if="lstValidationItems.length>0">
                                            <div v-for="(item, index) in lstValidationItems" :key="index">
                                                <!-- And / Or Radio Button-->
                                                <v-row :justify="justifyCenter" no-gutters v-show="index >0">
                                                    <v-col md="6" class="d-inline-flex">
                                                        <v-radio-group row v-model="item.LogicOperator" @change="ChangeLogicOperator(item.LogicOperator,index)">
                                                            <v-radio label="AND" hide-details></v-radio>
                                                            <v-radio label="OR" hide-details></v-radio>
                                                        </v-radio-group>
                                                    </v-col>
                                                </v-row>
                                                <v-row no-gutters>
                                                    <v-col>
                                                        <v-select id="drpValidationCondition" dense :items="QstValidationItems" v-model="item.ValidationCondition"
                                                                  item-text="ValidationCondition"
                                                                  label="Select validation condition" outlined="" hide-details>
                                                        </v-select>
                                                        <v-row v-show="item.ValidationCondition==='Min & Max Range'">
                                                            <v-col>
                                                                <v-text-field filled dense label="Enter min range" type="number" v-model="item.MinValue" hide-details></v-text-field>
                                                            </v-col>
                                                            <v-col>
                                                                <v-text-field hide-details filled dense label="Enter max range" type="number" v-model="item.MaxValue"></v-text-field>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>

                                                    <v-col md="auto">
                                                        <Default-Icon-Button-Outlined name="btnDeleteValidationCondition" id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline"
                                                                                      @click.native="DeleteValidationItem(item)"></Default-Icon-Button-Outlined>
                                                        <Default-Icon-Button-Outlined name="btnAddValidationCondition" id="AutoTest_IP736" icon="mdi-plus" @click.native="AddNewValidation = true, AddValidationItem()" v-if="index == lstValidationItems.length-1 && AddNewValidation == false" class="ml-1 mr-1"></Default-Icon-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </div>
                                        </div>
                                        <v-row no-gutters v-if="lstValidationItems.length ==0 || AddNewValidation == true">
                                            <v-col>
                                                <v-row :justify="justifyCenter" no-gutters v-if="lstValidationItems.length >0">
                                                    <v-col md="6" class="d-inline-flex">
                                                        <v-radio-group row v-model="vmOperatorValue">
                                                            <v-radio label="AND" hide-details></v-radio>
                                                            <v-radio label="OR" hide-details></v-radio>
                                                        </v-radio-group>
                                                    </v-col>
                                                </v-row>

                                                <v-row>
                                                    <v-col>
                                                        <v-select id="drpValidationCondition" dense :items="QstValidationItems" v-model="vmQstValidationConditions"
                                                                  item-text="ValidationCondition"
                                                                  label="Select validation condition" outlined="" hide-details return-object>
                                                        </v-select>
                                                        <v-row v-show="vmQstValidationConditions==='Min & Max Range'">
                                                            <v-col>
                                                                <v-text-field filled dense label="Enter min range" type="number" v-model="vmMinValue"></v-text-field>
                                                            </v-col>
                                                            <v-col>
                                                                <v-text-field filled dense label="Enter max range" type="number" v-model="vmMaxValue"></v-text-field>
                                                            </v-col>
                                                        </v-row>
                                                    </v-col>
                                                    <v-col md="auto">
                                                        <Default-Icon-Button-Outlined name="btnDeleteValidationCondition" id="AutoTest_IP735" hide-details class="float-right" icon="mdi-trash-can-outline"
                                                                                      @click.native="DeleteValidationItem(item)"></Default-Icon-Button-Outlined>
                                                        <Default-Icon-Button-Outlined name="btnAddValidationCondition" id="AutoTest_IP736" icon="mdi-plus" @click.native="AddValidationItem(),AddNewValidation = true" class="ml-1 mr-1"></Default-Icon-Button-Outlined>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                        </v-row>
                                    </div>
                                    <!--<div>
                                    <v-select v-model="vmQstValidationConditions" :items="QstValidationItems" dense outlined label="Select validation condition" clearable>
                                            <template v-slot:append-outer>
                                                <Default-Icon-Button-Outlined class="mr-2" icon="mdi-trash-can-outline" @click.native="DeleteValidationItem(item)"></Default-Icon-Button-Outlined>
                                                <Default-Icon-Button-Outlined icon="mdi-plus" @click.native="AddValidationItem(item)"></Default-Icon-Button-Outlined>
                                            </template>
                                    </v-select>

                                    <v-row v-show="vmQstValidationConditions==='Min & Max Range'">
                                        <v-col>
                                            <v-text-field filled dense label="Enter min range" type="number" v-model="vmMinValue"></v-text-field>
                                        </v-col>
                                        <v-col>
                                            <v-text-field filled dense label="Enter max range" type="number" v-model="vmMaxValue"></v-text-field>
                                        </v-col>
                                    </v-row>
                                </div>-->


                                    <v-textarea name="txtValidationErrorMessage" filled label="Enter error message..." v-model="vmErrorText"></v-textarea>
                                </v-col>
                            </v-row>
                        </v-tab-item>
                    </v-tabs>
                </v-card-text>
                <v-card-actions>

                    <v-col>
                        <Default-Button-Outlined name="btnCancelValidation" id="AutoTest_IP737" title="Cancel" class="float-right btn-122x36" @click.native="ClearQstnSettingsData(),dlgSectionQstnSetting=false"></Default-Button-Outlined>
                        <Secondary-Button name="btnSaveValidation" id="AutoTest_IP738" title="Save" class="float-right mr-4 btn-122x36" @click.native="UpdateQuestionSettings(QuestionID,SectionID)"></Secondary-Button>
                    </v-col>

                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>
<script>
    import Vue from 'vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import PrimaryButton from '@/components/primary-button-normal.vue'
    import DefaultIconButtonOutlined from '@/components/default-icon-button-outlined.vue'
    import SuccessButton from '@/components/success-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import draggable from 'vuedraggable'
    import ButtonLight24x24 from '@/components/button-light-24x24.vue'
    import BarChart from '@/components/BarChart.vue'
    import LineChart from '@/components/LineChart.vue'
    import DoughnutChart from '@/components/DoughnutChart.vue'
    import { Object } from 'core-js';
    import VueClipboard from 'vue-clipboard2'
    import { debuglog } from 'util';
    import objUtils from '../../utils.js'
    import ResponsesGridView from '@/views/MyTasks/MyTaskComponents/ResponsesGridView.vue'

    Vue.use(VueClipboard)

    VueClipboard.config.autoSetContainer = true;

    var objSurvey;
    async function importscript() {
        return Promise.all([
            import("../../BL/ProjectSurvey.js").then(mod => {
                objSurvey = new mod.ProjectSurvey();
            })
        ]);
    };
    var objPB;
    async function importscriptBeneficiary() {
        return Promise.all([
            import("../../BL/ProjectBeneficiary.js").then((mod) => {
                objPB = new mod.ProjectBeneficiary();
            }),
        ]);
    }
    export default {

        data() {
            return {
                rules: [],
                nonSelectedData: [],
                nonSelectedData_Edit:[],
                colsDialog: [],
                colsDialog_Edit:[],
                rowDialog: [],
                rowDialog_Edit: [],
                xAxisButton: '',
                xAxisButton_Edit:'',
                yAxisButton: '',
                yAxisButton_Edit:'',
                addPresetDialog: '',
                editPresetDialog: '',
                deletePresetDialog: '',
                items: [],
                //Pivot table dummy data
                valueFilter: {
                    Meal: {
                        Dinner: true
                    }
                },
                config: {},
                filteredData: [],
                presetName: '',
                presetSelected: {},
                presetSelected_Edit: {},
                presetSelected_Delete: {},
                pivotjsondata: [],
                presetLocalStorage: [],
                presetStoreSave: false,
                attributes: [],
                rows: [],
                cols: [],
                vals: [],
                disabledFromDragDrop: [],
                hiddenFromDragDrop: [],
                sortonlyFromDragDrop: [],
                pivotColumns: [],
                loading: false,
                aggregatorName: 'Count',
                rendererName: 'Table',
                localeStrings: {
                    en: {
                        renderError: 'An error occurred rendering the PivotTable results.',
                        computeError: 'An error occurred computing the PivotTable results.',
                        uiRenderError: 'An error occurred rendering the PivotTable UI.',
                        selectAll: 'Select All',
                        selectNone: 'Select None',
                        tooMany: 'too many values to show',
                        filterResults: 'Filter values',
                        totals: 'Totals',
                        only: 'only',
                        rendererNames: {
                            Table: 'Table',
                            'Table Heatmap': 'Table Heatmap',
                            'Table Col Heatmap': 'Table Col Heatmap',
                            'Table Row Heatmap': 'Table Row Heatmap',
                            'Export Table TSV': 'Export Table TSV',
                            'Grouped Column Chart': 'Grouped Column Chart',
                            'Stacked Column Chart': 'Stacked Column Chart',
                            'Grouped Bar Chart': 'Grouped Bar Chart',
                            'Stacked Bar Chart': 'Stacked Bar Chart',
                            'Line Chart': 'Line Chart',
                            'Dot Chart': 'Dot Chart',
                            'Area Chart': 'Area Chart',
                            'Scatter Chart': 'Scatter Chart',
                            'Multiple Pie Chart': 'Multiple Pie Chart'
                        },
                        aggregatorMap: {
                            Count: 'Count',
                            'Count Unique Values': 'Count Unique Values',
                            'List Unique Values': 'List Unique Values',
                            Sum: 'Sum',
                            'Integer Sum': 'Integer Sum',
                            Average: 'Average',
                            Median: 'Median',
                            'Sample Variance': 'Sample Variance',
                            'Sample Standard Deviation': 'Sample Standard Deviation',
                            Minimum: 'Minimum',
                            Maximum: 'Maximum',
                            First: 'First',
                            Last: 'Last',
                            'Sum over Sum': 'Sum over Sum',
                            'Sum as Fraction of Total': 'Sum as Fraction of Total',
                            'Sum as Fraction of Rows': 'Sum as Fraction of Rows',
                            'Sum as Fraction of Columns': 'Sum as Fraction of Columns',
                            'Count as Fraction of Total': 'Count as Fraction of Total',
                            'Count as Fraction of Rows': 'Count as Fraction of Rows',
                            'Count as Fraction of Columns': 'Count as Fraction of Columns'
                        }
                    }
                },
                locale: 'en',
                //Bar Chart Dummy Data
                likerIndex: '',
                likerSection: '',
                responsesBarData: {
                    labels: ['Organic Method', 'Organic Method', 'Other Method'],
                    datasets: [{
                        label: 'Options',
                        barThickness: 35,
                        data: [10, 60, 85],
                        backgroundColor: [
                            '#F1C100',
                            '#F1C100',
                            '#F1C100',
                        ],
                    }]
                },
                responsesBarOptions: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }

                },
                //_____________
                //Line Chart Dummy Data
                responsesLineData: {
                    labels: ['Organic Method', 'Organic Method', 'Other Method'],
                    datasets: [{
                        label: 'Options',
                        data: [10, 60, 85],
                        backgroundColor: 'transparent',
                        borderColor: '#F1C100'
                    }]
                },
                responsesLineOptions: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }

                },
                //_____________
                //Likert default
                Likerts: [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ],
                LikertsDefault: [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ],
                screenAccess: 0,
                RWAccess: 0,
                PreLoader: false,

                ResponseType: 'Pivot Table',
                ResponseTypeItems: [
                    'Pivot Table','Summary', 'Individual'
                ],

                // Tags ------------
                NewTagMenu: false,
                AllTagListSteps: 0,
                AllTagList: [],
                TagHeaderName: "Add Tag",
                SearchTag: "",
                NewTagName: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],

                ],
                allTags: false,
                EditTagName: "",
                type: 'hex',
                hex: '#FF00FF',

                color: "",
                filterAllTagList: [],
                SearchTagDetails: "",
                closeonClick: false,
                closeOnContentClick: false,
                offset: false,

                UpdatingTagItem: "",
                SelectedTag: [],

                SelectAnswerTypeDate: new Date().toISOString().substr(0, 10),
                SelectAnswerTypeDatepicker: false,

                rules: {
                    required: value => !!value || 'Required.',
                },

                lstuserTableheaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],

                AssignUserDialog: false,
                lstPeriodicity: [],
                lstWorkFlow: [],
                lstBeneficiaryType: [],
                lstUsers: [],

                WorkflowID: 0,
                ReportingFrequency: 0,
                ReportingMonthDay: 0,
                ReportingMonth: 0,
                ReportingMonthDateDay: 0,
                ReportingWeek: 0,
                BeneficiaryTypeID: 0,
                rules: {
                    required: value => !!value || 'Required.',
                },
                FrmDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,
                lstMonthDate: [],
                lstMonth: [],
                lstWeek: [],
                selectedteamUsers1: [],

                AssignAccountUserPopUp: false,

                BeneficiaryLinked: false,
                //PaymentLinked: "",
                selectedteamUsers: [],
                //==============================//
                justifyCenter: 'center',
                WorkspaceID: 0,
                InitiativeID: 0,
                ProjectID: 0,
                SurveyID: 0,

                dlgEditSurvey: false,
                dlgRemoveSurvey: false,
                dlgAddSection: false,
                dlgEditSection: false,
                dlgRemoveSection: false,
                dlgSecSettings: false,

                dlgSectionQstnSetting: false,
                dlgRemoveSectionQuest: false,

                lstSurveyDetails: [],
                SurveyName: "",
                SurveyDescription: "",

                SectionID: 0,
                SectionName: "",
                vmSectionName: "",
                vmSectionDesc: "",
                lstSurveySections: [],
                lstSections: [],
                sectionSettingLogicItems: ['Skip Logic', 'Repeat Logic'],

                //=== Qstn Variables ===//

                QuestionID: 0,
                vmQstn: "",
                vmQuestionType: [],
                vmDefaultVal: "",
                vmQuestionHint: "",
                vmIsUnique: false,
                vmIsSearchable: false,
                vmIsMandatory: false,

                vmFormulaOperation: "",
                //vmIsFormulaQstn: "",
                vmFormulaExp: "",
                vmFormulaFieldVar: "",
                vmQstnCalcVar: [],
                //FieldVarCount: 0,
                formulaVariables: [],
                tempExp: "",
                //lstFieldVars: [{ FieldVar: "", QuestionName: "" }],
                lstFieldVars: [],

                vmMinValue: 0,
                vmMaxValue: 0,
                vmRegEx: "",
                vmErrorText: "",

                lstQstnTypes: [],
                lstQstns: [],
                lstOptions: [],
                SingleChoices: [{ OptionValue: "" }],
                QstnCalcVarItems: [],
                FormulaOperationItems: [],//['Average', 'Sum','Percentage'],
                vmQstValidationConditions: null,
                QstValidationItems: [
                    'Email', 'Phone Number', 'Should be less than Current date', 'Should be greater than Current date', 'Min & Max Range', 'Aadhaar Number'
                ],
                //===Copy Qstn===//
                dlgCopySectionQuestion: false,
                vmChkSettings: true,
                vmChkValidation: true,
                vmChkCalculation: true,

                Msg: "",
                lstQstnResponses: [],
                lstQstnResponsesInd: [],
                Pageindex: 1,
                resIndex: 0,

                //lstQnSettings: [],
                vmLogicType: "",
                vmLogicCondition: "",
                vmLogicQstn: "",
                vmRLogicQstn: "",
                lstLogicQstns: [],
                lstLogicConditions: [],
                logicAnsType: [],
                logicAnsType1: "",
                vmLogicAnswer: [],
                vmLogicAnswer1: "",
                vmOperatorValue: 2,
                lstLogicAnswers: [],
                lstLogicConditionItems: [],
                lstRepeatLogicConditionItems: [],
                lstValidationItems: [],

                AddNew: true,
                vmSearchResponse: "",
                lstIndResponse: [],
                lstSummResponse: [],
                vmQstnCategory: "",
                vmQstnCategory1: "",

                IsCalcavailable: 0,
                IsValavailable: 0,

                Tab: 'tab-1',
                vmQstValidationVal: "",

                mStartDate: false,
                mEndDate: false,

                endDate: null,
                startDate: null,
                ProjectStartDate: "",
                ProjectEndDate: "",
                vmEQuestionType: [{ ID: 0, Type: "" }],
                vmEQuestionName: "",
                vmEQstnCategory: "",

                EditQstn: [],
                RespCount: 0,
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                lstCascadingQstnOptions: [],
                lstCascadingQstnSubOptions: [],
                vmCascadingOption: [],
                vmECascadingOption: [],
                preQstn: "",
                preAnsType: "",
                preCasOption: 0,
                vmUOM: "",
                vmEUOM: "",
                SectionSettingTab: "",
                SectionLogic: false,
                SectionSkipLogicSavedCount: 1,

                AddNewValidation: true,
                offset: 0,
                fileUrl: "",
                labelsArray: [],
                //GPS Capture
                //Coordinates: {
                //    lat: 0,
                //    lng: 0,
                //},
                mycolor: "",
                date: "",
                ActiveTab: "tab-1",
                userimage: "",
                questionDetailsForResponse: [],
                questionDetailsForResponseMainData: [],
                arrowsUpArrayIndividual: [{ sectionArrow: '', sectionId: 0 }],
                responseCountSummary: [],
                responseSummaryIndex: false,
                lstAllBeneficiaries: [],
                BeneficiaryID: 0,
                QstDtlsForIndividualResponse: [],
                PageCount: 0,
                PageCountTemp: 0,

                copyMessage: "",
                submittedbystartDate:null,
                submittedbyendDate:null,
                submittedmStartDate: false,
                submittedmEndDate: false,
                shareLinkPopup: false,
                collectEmailCheckBox: true,
                LinkActiveOrNot:true,
                ShareLinkURL: '',
                EncryptedSurveyID:'',
                EncryptedWorkspaceID: '',
                IsOtherOptionSelected: false,
                sectionTotalCount: 0,
                IsOtherOptionSelected:false,
                rows_Edit: [],
                cols_Edit: [],
                pivotdata:[],
                repeatLogic: null,
                lstRepeatLogic: [],
                lstRepeatSections: [],
                responseSectionTypes: [
                    {
                        name: 'Pivot Table',
                        id: 1,
                    },
                    {
                        name: 'Grid View',
                        id: 2
                    }
                ],
                currentResponseSectionType: 1
            }
        },
        components: {
            'Secondary-Button': SecondaryButton,
            'Primary-Button': PrimaryButton,
            'Default-Icon-Button-Outlined': DefaultIconButtonOutlined,
            'Success-Button': SuccessButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Button-Light-24x24': ButtonLight24x24,
            'Pre-Loader': PreLoader,
            draggable,
            'Bar-Chart': BarChart,
            'Line-Chart': LineChart,
            ResponsesGridView,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.userimage = window.SERVER_URL + "/Upload";
            this.RWAccess = this.$route.query.Access;
            this.screenAccess = this.$route.query.Access == 3 ? true : false;
            this.InitiativeID = this.$route.query.InitID;
            this.ProjectID = this.$route.query.ProjID;
            this.SurveyID = this.$route.query.SID;
            this.RespCount = this.$route.query.ResCount;
            

            await importscript();
            await importscriptBeneficiary();
            await this.EncryptShareLinkURL();
            this.ShareLinkURL = window.SERVER_URL + "/app/index.html#/SurveyShareLink?survid=" + encodeURIComponent(this.EncryptedSurveyID) + "&wkspid=" + encodeURIComponent(this.EncryptedWorkspaceID);
            await this.GetSurveyDetails();
            await this.GetAllSections();
            await this.GetAllQuestionTypes();
            await this.GetCascadingQuestionOptions();

            await this.fnGetAllTags();
            await this.fnGetWorkflows();
            await this.GetProjectDate();
            await this.GetReportingFrequencyList();
            await this.GetProjectTeamList();
            await this.GetProjectBeneficiaryType();
            // await this.GetAllResponsesBySurveyID();

            // this.scroll();

          
        },

        computed: {
            FormattedDate() {
                return objUtils.common.formatDate(this.FrmDate)
            },
            computedStartDate() {
                return objUtils.common.formatDate(this.startDate)
            },
            computedEndDate() {
                return objUtils.common.formatDate(this.endDate)
            },
            
            unusedAttrs() {
                return this.config.unusedAttrs
            },
            submittedStartDate() { 
                return objUtils.common.formatDate(this.submittedbystartDate)
            },
            submittedEndDate() {
                return objUtils.common.formatDate(this.submittedbyendDate)
            },
            computedLstRepeatLogic() {
                return [{Key:null, Value: 'Main Form'},...this.lstRepeatLogic]
            },
        },
        watch: {
            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.toLowerCase().includes(this.SearchTagDetails.toLowerCase()));
            }
        },
        methods: {
            switchResponseViewType() {
                switch(this.currentResponseSectionType) {
                    case 1: 
                        console.log('mount pivot tab')
                        this.fnMountPivotTab();
                        break;
                    case 2:
                        break;
                }
            },
            //async getWeek() {
            //    var firstWeekday = new Date(this.getFullYear(), this.getMonth(), 1).getDay();
            //    var offsetDate = this.getDate() + firstWeekday - 1;
            //    return Math.floor(offsetDate / 7) + 1;
            //},
            async EncryptShareLinkURL() { 
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objSurvey.EncryptShareLinkURL(data);
                if (result != null && result.length > 1) {
                    this.EncryptedSurveyID = result[0];
                    this.EncryptedWorkspaceID = result[1];
                }
                else 
                {
                    //encryption error
                }
            
            },
            copyText() {
                const input = this.$refs.shareLinkText.$refs.input
                input.select()
                document.execCommand('copy')
                input.setSelectionRange(0, 0) // unselect
                this.infoSnackbarMsg = true;
                this.Msg = 'link Copied';
            },
            async SaveShareLinkStatus() { 
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                //data.append("UserID", 582); 
                data.append("ShareLinkStatus", this.LinkActiveOrNot);
                var result = await objSurvey.SaveShareLinkStatus(data);
            },

            async  ExportToExcel(type, fn, dl) 
            {
                    var elt = document.querySelector("#output .pvtTable")
                    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
                    return dl ?
                        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
                        XLSX.writeFile(wb, fn || ('Pivot_Table.' + (type || 'xlsx')));
            },

            

            async appendFilePath() {
                var surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectID.toString() + "/";
                var beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.ProjectID.toString() + "/";

                if (this.pivotdata != null && this.pivotdata != undefined)
                {
                    for (var i = 0; i < this.pivotdata.length; i++) {
                        for (let key in this.pivotdata[i]) {
                            if (this.pivotdata[i].hasOwnProperty(key)) {
                                var allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar'];
                                if (this.pivotdata[i][key] != null && this.pivotdata[i][key] != undefined && this.pivotdata[i][key] != 0 && typeof this.pivotdata[i][key] === 'string') {
                                    let filextension = this.pivotdata[i][key].split('.').pop();
                                    if (allowedExtensions.indexOf(filextension) >= 0) {
                            //var data = new FormData();
                            //data.append("FileName", this.pivotdata[i][key]);
                            //data.append("ProjectID", this.ProjectID);
                            //console.log('key :' + key);
                            //console.log('i :' + i);
                            //console.log('this.pivotdata[i][key] :' + this.pivotdata[i][key]);
                            //console.log('ProjectID :' + this.ProjectID);
                            //var isFileExists = await objSurvey.checkFileExists(data);
                            //console.log('isFileExists :' + isFileExists);
                            //switch (isFileExists) {
                            //    case 0: // file not exists
                            //        this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                            //        break;
                            //    case 1:
                            //        this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                            //        console.log('this.pivotdata[i][key] in switch :' + this.pivotdata[i][key]);
                            //        break;
                            //    case 2:
                            //        this.pivotdata[i][key] = beneficiaryfilesURL + this.pivotdata[i][key];
                            //        break;
                            //    default: // file not exists

                            //}

                                        //temporary

                                        var questionname = key;

                                        if (questionname.indexOf('(Beneficiary)') == -1) {
                                            this.pivotdata[i][key] = surveyfilesURL + this.pivotdata[i][key];
                                        }

                                        if (questionname.indexOf('(Beneficiary)') >= 0)
                                        {
                                            this.pivotdata[i][key] = beneficiaryfilesURL + this.pivotdata[i][key];
                                        }

                                        //var surveyfilePath = surveyfilesURL + this.pivotdata[i][key];
                                        //var beneficiaryfilePath = beneficiaryfilesURL + this.pivotdata[i][key];
                                        //if (this.UrlExists(surveyfilePath))
                                        //{
                                        //    this.pivotdata[i][key] = surveyfilePath;
                                        //}
                                        //else if (this.UrlExists(beneficiaryfilePath)) {
                                        //    this.pivotdata[i][key] = beneficiaryfilePath;
                                        //}
                                        


                                        //temporary ends
                                    }

                                    if (objUtils.common.checkIfValidlatitudeAndlongitude(this.pivotdata[i][key])) {
                                        if (this.pivotdata[i][key] != '0,0') {
                                            this.pivotdata[i][key] = objUtils.GPS_TARGET_URL + this.pivotdata[i][key] + "?hl=en";
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            },

            async getpivottable() {
                
                //if ($.pivotUtilities != undefined) {
                //    var dateFormat = $.pivotUtilities.derivers.dateFormat;
                //    var sortAs = $.pivotUtilities.sortAs;
                //}
                var renderers = $.extend($.pivotUtilities.renderers,
                    $.pivotUtilities.plotly_renderers);
                
                var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                url += "?url=getpivotdata?surveyid=" + this.SurveyID + "&repeatenable=1";

                var self = this;
                

                $.getJSON(url, function (tdata) {
                    var data = tdata;
                    self.pivotdata = [];
                    self.pivotdata = tdata;
                    self.appendFilePath();
                    
                    var temparray = [];
                    for (var i = 0; i < data.length;i++)
                    { 
                        temparray.push(...Object.keys(data[i]));
                    }
                    let arr2 = [...new Set(temparray.flat(1))];
                    //self.rows = Object.keys(data[0]); 
                    //self.rows = arr2; 
                    //self.cols = arr2; 
                    self.attributes = arr2;
                    self.nonSelectedData = arr2;
                    self.nonSelectedData_Edit = arr2;
                    if (data != null) {
                        $("#output").pivotUI(data, {

                            //hiddenAttributes: ["SubmittedDate"],
                            hiddenAttributes: ["BeneficiaryName"],
                            //derivedAttributes: {
                            //    "Year": dateFormat("SubmittedDate", "%y", true),
                            //    "Quarter": function (record) {
                            //        var d = new Date(record.SubmittedDate);
                            //        var q = [4, 1, 2, 3];
                            //        return q[Math.floor(d.getMonth() / 3)];
                            //    },

                            //    "Month": dateFormat("SubmittedDate", "%n", true),
                            //    "Week": function (record) {
                            //        var d = new Date(record.SubmittedDate);
                            //        return d.getWeek()
                            //    },
                            //    "Day": dateFormat("SubmittedDate", "%w", true)
                            //},
                            //sorters: {
                            //    "Month": sortAs(["Jan", "Feb", "Mar", "Apr", "May",
                            //        "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]),
                            //    "Day": sortAs(["Mon", "Tue", "Wed", "Thu", "Fri",
                            //        "Sat", "Sun"])
                            //},

                            renderers: renderers,

                            rows: ["Answer"],
                            cols: ["QuestionName"],
                            rendererName: "Table",
                            rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                        });
                    }

                })
                

            },

          async  SavePivotPreset() {

                var config = $("#output").data("pivotUIOptions");
                var config_copy = JSON.parse(JSON.stringify(config));
                //delete some values which will not serialize to JSON
                delete config_copy["aggregators"];
                delete config_copy["renderers"];
                //localStorage.setItem('pivotpreset', JSON.stringify(config_copy));

                var data = new FormData();
                data.append("PresetName", this.presetName);
                data.append("SurveyID", this.SurveyID);
                data.append("PresetData", JSON.stringify(config_copy));
                var result = await objSurvey.SavePivotPreset(data);

                this.Msg = "Pivot table preset is saved successfully.";
                this.successSnackbarMsg = true;

            },

            async RestorePivotPreset() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("PresetName", this.presetSelected);
                data.append("SurveyID", this.SurveyID);
                var result = await objSurvey.RestorePivotPreset(data);

                if (result !== null && result != undefined && result !='') 
                {
                    var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                    url += "?url=getpivotdata?surveyid=" + this.SurveyID + "&repeatenable=1";

                    var self = this;
                    
                    //$.getJSON(url, function (tdata) {
                    //    var data = tdata;
                    //    self.pivotdata = tdata;
                    //    self.appendFilePath();
                    //    if (data != null) {
                    //        $("#output").pivotUI(data, result, true);
                    //    }
                    //})

                    $.getJSON(url, function (tdata) {
                        self.pivotdata = [];
                        self.pivotdata = tdata;
                        self.appendFilePath();
                        
                    })
                    if (self.pivotdata != null) {
                        $("#output").pivotUI(self.pivotdata, result, true);
                    }
                }
                else {
                    this.Msg = "Preset does not exist.";
                    this.infoSnackbarMsg = true;
                }

                this.PreLoader = false;

            },

            async GetPivotPresetList() 
            {
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                var result = await objSurvey.GetPivotPresetList(data);
                if (result !== null && result != undefined && result != '') 
                {
                    this.presetLocalStorage = result;
                }
            
            },

            changeSelectedPivotValue: function (newData, oldIndex, axis) {
                if (axis == 'y') {
                    this.colsDialog[oldIndex] = newData
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.yAxisButton = '';
                } else if (axis == 'x') {
                    this.rowDialog[oldIndex] = newData
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.xAxisButton = '';
                } else if (axis == 'lastIndexX') {
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.rowDialog.push(this.nonSelectedData[this.nonSelectedData.length - 1])
                    this.nonSelectedData.pop()
                } else if (axis == 'lastIndexY') {
                    let totalSelected = this.rowDialog + this.colsDialog;
                    this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                    this.colsDialog.push(this.nonSelectedData[this.nonSelectedData.length - 1])
                    this.nonSelectedData.pop()
                }

            },

            nonSelectedPivotValue: function (index) {
                let totalSelected = this.rowDialog + this.colsDialog;
                this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton = index;
            },

            changeSelectedPivotValue_Edit: function (newData, oldIndex, axis) {
                if (axis == 'y') {
                    this.colsDialog_Edit[oldIndex] = newData;
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.yAxisButton_Edit = '';
                } else if (axis == 'x') {
                    this.rowDialog_Edit[oldIndex] = newData;
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.xAxisButton_Edit = '';
                } else if (axis == 'lastIndexX') {
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.rowDialog_Edit.push(this.nonSelectedData_Edit[this.nonSelectedData_Edit.length - 1]);
                    this.nonSelectedData_Edit.pop();
                } else if (axis == 'lastIndexY') {
                    let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                    this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                    this.colsDialog_Edit.push(this.nonSelectedData_Edit[this.nonSelectedData_Edit.length - 1]);
                    this.nonSelectedData_Edit.pop();
                }

            },

            nonSelectedPivotValue_Edit: function (index) {
                let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton_Edit = index;
            },

            Remove_nonSelectedPivotValue_Edit: function (index) {
                this.rowDialog_Edit = this.rowDialog_Edit.filter(e => e !== index);
                this.colsDialog_Edit = this.colsDialog_Edit.filter(e => e !== index);
                let totalSelected = this.rowDialog_Edit + this.colsDialog_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.yAxisButton_Edit = index;
            },

            onCopy: function (e) {
                this.infoSnackbarMsg = true;
                this.Msg = 'You just copied : ' + e.text;
            },
            addPresetDialogOpen: function () {
                this.rows = $("#output").data("pivotUIOptions").rows
                this.cols = $("#output").data("pivotUIOptions").cols
                let totalSelected = this.rows + this.cols;
                this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                this.colsDialog = [...this.cols];
                this.rowDialog = [...this.rows];
                this.presetStoreSave = false;
                this.addPresetDialog = true;
            },
            addPresetDialogCloseButton: function () {
                this.colsDialog = this.cols
                this.rowDialog = this.rows
                this.cols = [];
                this.rows = [];
                this.cols = [...this.colsDialog];
                this.rows = [...this.rowDialog];
                this.rules = [];
                this.presetName = '';
                this.addPresetDialog = false;
            },
            addPresetDialogSaveButton: async function () {
                this.PreLoader = true;
                
                this.cols = [];
                this.rows = [];
                this.cols = [...this.colsDialog];
                this.rows = [...this.rowDialog];

                //
                $("#output").data("pivotUIOptions").rows = this.rows;
                $("#output").data("pivotUIOptions").cols = this.cols;
                var config = $("#output").data("pivotUIOptions");
                var config_copy = JSON.parse(JSON.stringify(config));
                //delete some values which will not serialize to JSON
                delete config_copy["aggregators"];
                delete config_copy["renderers"];
                //localStorage.setItem('pivotpreset', JSON.stringify(config_copy));

                var data = new FormData();
                data.append("PresetName", this.presetName);
                data.append("SurveyID", this.SurveyID);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objSurvey.SavePivotPreset(data);

                if (result) {
                    //this.presetLocalStorage.push(this.presetName); 
                    this.Msg = "Pivot table preset is saved successfully.";
                    this.successSnackbarMsg = true;
                    this.GetPivotPresetList();
                }
                else { 
                    this.Msg = "An unknown error has occured.";
                    this.infoSnackbarMsg = true;
                }
                //

                this.rules = [];
                this.presetName = '';
                this.addPresetDialog = false;
                this.PreLoader = false;
            },
            submitHandler() {
                this.rules = {
                    name: [(v) => !!v || "Preset Name is required"]
                };

                var isPresetExists = this.presetLocalStorage.includes(this.presetName);
                if (isPresetExists) 
                {
                    this.Msg = "Preset name already exists.";
                    this.infoSnackbarMsg = true;
                    return false;
                }
                let self = this;
                setTimeout(() => {
                    if (self.$refs.entryForm.validate()) {
                        this.addPresetDialogSaveButton()
                    }
                });
            },

            //for edit preset
            editPresetDialogOpen: function () {
                //this.rows = $("#output").data("pivotUIOptions").rows
                //this.cols = $("#output").data("pivotUIOptions").cols

                
                //let totalSelected = this.rows + this.cols;
                //this.nonSelectedData = this.attributes.filter(e => !totalSelected.includes(e));
                //this.colsDialog = [...this.cols];
                //this.rowDialog = [...this.rows];
                //this.presetStoreSave = false;
                this.editPresetDialog = true;

                this.presetSelected_Edit = '';
                this.colsDialog_Edit = [];
                this.rowDialog_Edit = [];
                this.rows_Edit = [];
                this.cols_Edit = [];

            },

            GetPivotPresetData_Edit()
            {
                var selectedpreset_Edit = this.presetSelected_Edit;

                this.rows_Edit = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData).rows;
                this.cols_Edit = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData).cols;
                let totalSelected = this.rows_Edit + this.cols_Edit;
                this.nonSelectedData_Edit = this.attributes.filter(e => !totalSelected.includes(e));
                this.colsDialog_Edit = [...this.cols_Edit];
                this.rowDialog_Edit = [...this.rows_Edit];
            },

            async submitEditHandler() {

                var selectedpreset_Edit = this.presetSelected_Edit;

                //var ExistingPresetData = this.presetLocalStorage.find(obj => {
                //    return obj.presetName === selectedpreset_Edit;
                //});
                var ExistingPresetData = JSON.parse(this.presetLocalStorage.filter(a => a.presetName == selectedpreset_Edit)[0].presetData);

                ExistingPresetData.rows = this.rowDialog_Edit;
                ExistingPresetData.cols = this.colsDialog_Edit;
                var config_copy = ExistingPresetData;
                var data = new FormData();
                data.append("PresetName", selectedpreset_Edit);
                data.append("SurveyID", this.SurveyID);
                data.append("PresetData", JSON.stringify(config_copy));
                this.repeatLogic && data.append("sectionId", this.repeatLogic)
                var result = await objSurvey.EditPivotPreset(data);

                this.Msg = "Pivot table preset is saved successfully.";
                this.successSnackbarMsg = true;
                this.GetPivotPresetList();
                this.editPresetDialog = false;

            },

            //for Delete preset
           async submitDeleteHandler() {
                this.PreLoader = true;
                var data = new FormData();
                data.append("PresetName", this.presetSelected_Delete);
                data.append("SurveyID", this.SurveyID);
                var result = await objSurvey.DeletePivotPreset(data);

                if (result) {
                    //var x = this.presetLocalStorage.filter(a => a.PresetName !== this.presetSelected_Delete);
                    //this.presetLocalStorage = x;
                    this.presetLocalStorage = [];
                    this.GetPivotPresetList();
                    this.clearDateSelection();
                    this.Msg = "Pivot table preset is deleted successfully.";
                    this.successSnackbarMsg = true;
                    this.deletePresetDialog = false;
                    
                }
                else {
                    this.Msg = "Preset does not exist.";
                    this.infoSnackbarMsg = true;
                }

                this.PreLoader = false;
            },
            
            clearDateSelection() { 
                this.submittedbystartDate = null;
                this.submittedbyendDate = null;
                this.submittedmStartDate = false;
                this.submittedmEndDate = false;
                this.presetSelected = '';
                this.repeatLogic = null;
                $("#output").pivotUI([], {});
                this.lstRepeatSections = this.fnFilterPresets();
            },
            async submitpivottable() {
                this.PreLoader = true
                //if ($.pivotUtilities != undefined) {
                //    var dateFormat = $.pivotUtilities.derivers.dateFormat;
                //    var sortAs = $.pivotUtilities.sortAs;
                //}
                var renderers = $.extend($.pivotUtilities.renderers,
                    $.pivotUtilities.plotly_renderers);

                var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                url += "?url=getpivotdata?surveyid=" + this.SurveyID + "&repeatenable=1";

                if(this.repeatLogic){
                    url += `&sectionid=${this.repeatLogic}`
                }

                var self = this;

                let response = await fetch(url)
                let data = await response.json()
                self.pivotdata = data
                self.appendFilePath()

                var temparray = [];
                for (var i = 0; i < data.length; i++) {
                    temparray.push(...Object.keys(data[i]));
                }
                let arr2 = [...new Set(temparray.flat(1))];
                self.attributes = arr2;
                self.nonSelectedData = arr2;
                if(self.submittedbystartDate && self.submittedbyendDate){
                    var newArray = data.filter(function (el) {
                    return objUtils.common.formatExcelDate(el.SubmittedDate) <= self.submittedbyendDate &&
                        objUtils.common.formatExcelDate(el.SubmittedDate) >= self.submittedbystartDate;
                    });
                    if (newArray != null) {
                        $("#output").pivotUI(newArray, {
                            renderers: renderers,
                            rows: ["Answer"],
                            cols: ["QuestionName"],
                            rendererName: "Table",
                            rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                        });
                    }
                }
                else if (data.length) {
                    $("#output").pivotUI(data, {
                        renderers: renderers,
                        rows: ["Answer"],
                        cols: ["QuestionName"],
                        rendererName: "Table",
                        rowOrder: "value_a_to_z", colOrder: "value_z_to_a",
                    });
                }
                this.lstRepeatSections = this.fnFilterPresets()
                this.PreLoader = false
            },

            /// <summary>
            ///  onError :- Function used to display error message if failed to copy variable
            /// </summary>
            /// <param name="e"></param>
            /// <returns> </returns>
            onError: function (e) {
                this.errorSnackbarMsg = true;
                this.Msg = 'Failed to copy texts';
            },

            /// <summary>
            ///  openPanelSummary :- Function used to open Summary response panel
            /// </summary>
            /// <param name="index"></param>
            /// <returns> </returns>
            openPanelSummary(index) {
                if (this.arrowsUpArrayIndividual[index].sectionArrow == '') {
                    this.arrowsUpArrayIndividual[index].sectionArrow = 1
                } else if (this.arrowsUpArrayIndividual[index].sectionArrow == 1) {
                    this.arrowsUpArrayIndividual[index].sectionArrow = ''
                }
            },

            /// <summary>
            ///  openPanelInd :- Function used to open Individual response panel
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
            ///  commonGetColor :- Function used to get color
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            ///  formatDT :- Function used to show month and year from a date when question type as image/audio/GPS/signature capture in response page
            /// </summary>
            /// <param name="date"></param>
            /// <returns> month and year </returns>
            formatDT(date) {
                return objUtils.common.formatDT(date);
            },

            /// <summary>
            ///  formatMonth :- Function used to show month from a date when question type as image/audio/GPS/signature capture in response page
            /// </summary>
            /// <param name="date"></param>
            /// <returns> month </returns>
            formatMonth(date) {
                return objUtils.common.formatMonth(date);
            },

            /// <summary>
            ///  formatDay :- Function used to show day from a date when question type as image/audio/GPS/signature capture in response page
            /// </summary>
            /// <param name="date"></param>
            /// <returns> day </returns>
            formatDay(date) {
                return objUtils.common.formatDay(date);
            },

            //==============Survey=====================//

            //Tag related methods

            /// <summary>
            ///  fnInitializeSteper :- Function used to initialize stepper for tag
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSteper() {
                if (this.AllTagList.length == 0) {
                    this.AllTagListSteps = 1;
                }
                else {
                    this.AllTagListSteps = 2;
                }
            },

            /// <summary>
            ///  fnGetAllTags :- Function used to fetch all project tag list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllTags() {
                var result = await objSurvey.GetAllProjectTags(this.InitiativeID);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AllTagList = result.data;
                    this.filterAllTagList = result.data;
                } else {
                    this.Msg = "Failed to get tag list.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnEditTagDetails :- Function used to get edit tag details
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnEditTagDetails(TagItem) {
                this.AllTagListSteps = 3;
                this.TagHeaderName = 'Edit Tag Name';
                this.EditTagName = TagItem.TagName;
                this.color = TagItem.TagColor;
                this.UpdatingTagItem = TagItem;

            },

            /// <summary>
            ///  UpdateTagDetails :- Function used to update tag details after edit
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {

                if (this.UpdatingTagItem == "") {
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();

                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objSurvey.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags()
                        this.UpdatingTagItem = "";
                    }
            },

            /// <summary>
            ///  CreateNewTag :- Function used to create new tag
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color.hex);
                data.append("InitiativeId", this.InitiativeID);
                var result = await objSurvey.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllTags()
                    }
            },

            /// <summary>
            ///  fnAddToSelectedTag :- Function used to add a tag to selected tag items
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToSelectedTag(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.SelectedTag.length > 0) {
                        var temp = this.SelectedTag.filter(x => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                        }
                    } else {
                        this.SelectedTag.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }

                    this.NewTagMenu = false;
                }
            },

            /// <summary>
            ///  fnRemoveFromSelectedTagItem :- Function used to remove a tag from selected tag items
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnRemoveFromSelectedTagItem(TagItem) {
                this.SelectedTag.splice(this.SelectedTag.indexOf(TagItem), 1);
                this.SelectedTag = [...this.SelectedTag];
                if (this.SelectedTag.length == 0) {
                    this.allTags = false;
                }

            },

            //---------------------------------------------//

            /// <summary>
            ///  GetProjectDate :- Function used to fetch project startDate and project endDate
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectDate() {

                var result = await objSurvey.GetProjectDate(this.ProjectID);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {

                    this.ProjectStartDate = result.PrjStartDate;
                    this.ProjectEndDate = result.PrjEndDate;
                }
            },

            /// <summary>
            ///  GetSurveyDetails :- Function used to fetch survey details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetSurveyDetails() {
                this.PreLoader = true;
                var result = await objSurvey.GetProjectSurveyBySurveyId(this.SurveyID);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {

                        this.lstSurveyDetails = result.data;
                        this.SurveyName = result.data.SurveyName;
                        this.SurveyDescription = result.data.SurveyDescription;

                        //------For Edit-----//
                        //if (result.data.StartDate != null)
                        //    this.startDate = this.formatDate(result.data.StartDate.substr(0, 10));
                        //if (result.data.EndDate != null)
                        //    this.endDate = this.formatDate(result.data.EndDate.substr(0, 10));

                        //if (result.data.StartDate != null)
                        //    this.startDate = this.formatDate(result.data.StartDate.toString("dd-MM-yyyy").substr(0, 10));
                        //if (result.data.EndDate != null)
                        //    this.endDate = this.formatDate(result.data.EndDate.toString("dd-MM-yyyy").substr(0, 10));

                        if (result.data.StartDate != null)
                            this.startDate = objUtils.common.formatDate(result.data.StartDate);
                        if (result.data.EndDate != null)
                            this.endDate = objUtils.common.formatDate(result.data.EndDate);

                        this.WorkflowID = result.data.WorkflowID;
                        this.ReportingFrequency = result.data.ReportingFrequency;
                        this.ReportingMonth = result.data.ReportingMonth;
                        this.ReportingMonthDateDay = result.data.ReportingMonthDateDay;
                        this.ReportingMonthDay = result.data.ReportingMonthDay;
                        this.ReportingWeek = result.data.ReportingWeek;

                        if (result.data.ReportingOnceDate != null && result.data.ReportingOnceDate != "") {
                            this.FrmDate = objUtils.common.formatDate(result.data.ReportingOnceDate);
                        }

                        if (result.data.BeneficiaryTypeID > 0) {
                            this.BeneficiaryLinked = true;
                            this.BeneficiaryTypeID = result.data.BeneficiaryTypeID;
                        }

                        if (result.data.AssignedUsers != null && result.data.AssignedUsers != undefined && result.data.AssignedUsers != "") {
                            this.selectedteamUsers = JSON.parse(result.data.AssignedUsers);
                        }
                        if (result.data.AssignedTags != null && result.data.AssignedTags != undefined && result.data.AssignedTags != "") {
                            this.SelectedTag = JSON.parse(result.data.AssignedTags);
                        }

                    }
                this.PreLoader = false;
            },

            /// <summary>
            ///  GetProjectTeamList :- Function used to fetch project team list to display users
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectTeamList() {
                var data = new FormData();
                data.append("ProjectID", this.ProjectID);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objSurvey.GetProjectTeam(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)

                        this.lstUsers = result.data;
            },

            /// <summary>
            ///  fnGetWorkflows :- Function used to fetch project workflows
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetWorkflows() {
                var result = await objSurvey.GetProjectWorkFlow(this.ProjectID);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data)
                    this.lstWorkFlow = result.data;
            },
            async fnRouteToSurveyShareLink() {
                    this.$router.push({
                        name: "SurveyShareLink",
                        query: {},
                    });
            },

            /// <summary>
            ///  GetReportingFrequencyList :- Function used to fetch reporting frequency list (Week, Month, MonthDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetReportingFrequencyList() {
                var result = await objSurvey.GetReportingFrequencyList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.status = "success") {

                    if (result.data.RptFrequencyList.length > 0) {
                        this.lstPeriodicity = result.data.RptFrequencyList;
                    }
                    if (result.data.WeekList.length > 0) {
                        this.lstWeek = result.data.WeekList;
                    }
                    if (result.data.MonthList.length > 0) {
                        this.lstMonth = result.data.MonthList;
                    }
                    if (result.data.MonthDate.length > 0) {
                        this.lstMonthDate = result.data.MonthDate;
                    }
                }
            },

            /// <summary>
            ///  GetProjectBeneficiaryType :- Function used to fetch project beneficiary type list when survey is beneficiary linked survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetProjectBeneficiaryType() {
                var data = new FormData();
                data.append("projectId", this.ProjectID);
                data.append("surveyId", this.SurveyID)
                this.lstBeneficiaryType = [];
                var result = await objSurvey.GetProjectBeneficiaryType(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data)
                    this.lstBeneficiaryType = result.data;
            },

            /// <summary>
            ///  RemoveProjectSurvey :- Function used to remove survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async RemoveProjectSurvey() {
                var data = new FormData();
                data.append("SurveyID", JSON.stringify(this.SurveyID));
                data.append("Type", 2);

                var result = await objSurvey.DeleteProjectSurvey(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else {
                    if (result.status == "success") {
                        this.Msg = "Survey is removed Successfully ... ";
                        this.successSnackbarMsg = true;
                    }

                    else {
                        this.Msg = "Failed in removing Survey... ";
                        this.errorSnackbarMsg = true;
                    }


                }
                this.$router.push({ name: 'ManageProjectSurveys', query: { WsID: this.WorkspaceID, Access: RWAccess, InitID: this.InitiativeID, ProjID: this.ProjectID } });
            },

            /// <summary>
            ///  fnCheckMandatory :- Function used to check any mandatory field is showing blank or null
            /// </summary>
            /// <param name=""></param>
            /// <returns> true/false </returns>
            fnCheckMandatory() {
                if (this.SurveyName == "" ||
                    this.SurveyDescription == "" ||
                    //  this.WorkflowID == 0 || this.selectedteamUsers == 0||
                    this.ReportingFrequency == 0 ||
                    (this.ReportingFrequency == 2 && (this.FrmDate == "" || this.FrmDate == null)) ||
                    (this.BeneficiaryLinked == true && this.BeneficiaryTypeID == 0))
                    return true;
                else
                    return false;
            },

            /// <summary>
            ///  IsValidPeriodicityDate :- Function used to check periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidPeriodicityDate() {
                if (this.startDate != null) {
                    if (Date.parse(this.FrmDate) < Date.parse(this.startDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.FrmDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }

                if (this.endDate != null) {
                    if (Date.parse(this.FrmDate) > Date.parse(this.endDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.FrmDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                return true;

            },

            /// <summary>
            ///  IsValidDate :- Function used to check survey start date and end date is valid (between project start date and project end date)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.startDate = "";
                    return;
                }
                if (Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + objUtils.common.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + objUtils.common.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    this.endDate = "";
                    return;
                }
                // startdate/ Enddate  not < todays date
                //enddate  any valid range - Max value
            },

            /// <summary>
            ///  UpdateProjectSurvey :- Function used to update survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async UpdateProjectSurvey() {
                if (this.ReportingFrequency == 2) {
                    if (!this.IsValidPeriodicityDate()) {
                        this.Msg = " Invalid PeriodicityDate ";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }

                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                data.append("SurveyName", this.SurveyName);
                data.append("SurveyDescription", this.SurveyDescription);
                data.append("StartDate", this.startDate);
                data.append("EndDate", this.endDate);
                data.append("ProjectId", this.ProjectID);
                data.append("WorkflowID", this.WorkflowID);
                data.append("ReportingFrequency", this.ReportingFrequency);
                data.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                data.append("AssignedUsers", JSON.stringify(this.selectedteamUsers));
                data.append("AssignedTags", JSON.stringify(this.SelectedTag));

                data.append("ReportingMonth", this.ReportingMonth);
                data.append("ReportingMonthDay", this.ReportingMonthDay);
                data.append("ReportingWeek", this.ReportingWeek);
                data.append("ReportingOnceDate", this.FormattedDate);
                data.append("ReportingMonthDateDay", this.ReportingMonthDateDay);

                var result = await objSurvey.SaveProjectSurvey(data);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.dlgEditSurvey = false;
                        this.Msg = " Survey deatils updated successfully...";
                        this.selectedteamUsers = [];
                        await this.ClearBindData();


                    }
                    else {
                        if (result.status == "nameexists")
                            this.infoSnackbarMsg = "Survey Name Exists";
                        else if (result.status == "rptfrequencyfailed") {
                            this.Msg = "Reporting Frequency Failed";
                            this.errorSnackbarMsg = true;
                        }


                        else {
                            this.Msg = " Failed in updating  Survey deatils ...";
                            this.errorSnackbarMsg = true;
                        }

                    }

                }
            },

            /// <summary>
            ///  ClearBindData :- Function used to clear all data
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ClearBindData() {
                this.SurveyId = 0;
                this.SurveyName = "";
                this.SurveyDescription = "";
                this.startDate = null,
                    this.endDate = null,
                    this.WorkflowID = 0;
                this.ReportingFrequency = 0;
                this.BeneficiaryTypeID = 0;
                this.selectedteamUsers = [];
                this.selectedteamUsers1 = [];
                this.BeneficiaryLinked = false;
                this.SelectedBeneficiaryTypeList = [];
                this.SelectedTag = [];
                this.ReportingMonth = 0;
                this.ReportingMonthDateDay = 0;
                this.ReportingMonthDay = 0;
                this.ReportingWeek = 0;
                this.GetSurveyDetails();
            },

            /// <summary>
            ///  CloseAssignUserDialog :- Function used to close assignuser dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CloseAssignUserDialog() {
                this.AssignAccountUserPopUp = false;
                this.selectedteamUsers1 = [];
            },

            /// <summary>
            ///  AssignUserToSurvey :- Function used to assign users to survey
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AssignUserToSurvey() {
                this.AssignAccountUserPopUp = false;
                this.selectedteamUsers = this.selectedteamUsers1;
                this.selectedteamUsers1 = [];
            },

            /// <summary>
            ///  OpenAssignUserDialog :- Function used to open assignuser dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            OpenAssignUserDialog() {
                this.AssignAccountUserPopUp = true;
                this.selectedteamUsers1 = this.selectedteamUsers;
            },



            //==============Section=====================//

            /// <summary>
            ///  GetAllSections :- Function used to fetch all sections details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllSections() {
                this.PreLoader = true;
                var result = await objSurvey.GetAllSectionsBySurveyID(this.SurveyID);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstSurveySections = result.data;
                        this.lstSections = [];

                        for (var i = 0; i < this.lstSurveySections.length; i++) {
                            for (var j = 0; j < this.lstSurveySections[i].SurveySecQuestions.length; j++) {
                                this.lstSurveySections[i].SurveySecQuestions[j].IsEditQstn = 0;
                                if (this.lstSurveySections[i].SurveySecQuestions[j].QuestionType == "Cascading Dropdown") {
                                    this.lstSurveySections[i].SurveySecQuestions[j].lstCascadingOptions = this.lstSurveySections[i].SurveySecQuestions[j].lstQuestionOptions;
                                    this.lstSurveySections[i].SurveySecQuestions[j].lstQuestionOptions = [{ OptionValue: "" }];
                                }
                                if (this.lstSurveySections[i].SurveySecQuestions[j].lstQuestionOptions.length == 0)
                                    this.lstSurveySections[i].SurveySecQuestions[j].lstQuestionOptions = [{ OptionValue: "" }];
                            }

                            this.lstSections.push({
                                SectionID: this.lstSurveySections[i].SectionID, SectionName: this.lstSurveySections[i].SectionName,
                                SectionDescription: this.lstSurveySections[i].SectionDescription, SectionOrder: this.lstSurveySections[i].SectionOrder,
                                SurveySecQuestions: this.lstSurveySections[i].SurveySecQuestions, questionPanel: true, addQstnSection: false, viewSave: false
                                , UpdateQstnSection: false
                            });
                        }

                        this.sectionTotalCount = this.lstSurveySections.length;
                    }

                this.PreLoader = false;
            },

            /// <summary>
            ///  GetSectionsLogic :- Function used to fetch section logic type (Skip logic/Repeat logic)
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async GetSectionsLogic(sectionID) {
                var result = await objSurvey.GetSectionsLogic(this.SurveyID, sectionID);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data.length != 0) {
                        this.vmLogicType = result.data[0].LogicType;
                        this.AddNew = false;
                        if (result.data[0].LogicType == "Skip Logic") {
                            this.lstLogicConditionItems = result.data;
                            this.SectionSkipLogicSavedCount = this.lstLogicConditionItems.length;
                        }
                        else {
                            this.lstRepeatLogicConditionItems = result.data;
                        }
                    }
                this.GetAllQstnsForSectionsLogic(sectionID);


            },

            /// <summary>
            ///  GetAllQstnsForSectionsLogic :- Function used to fetch all questions for section logic type (Skip logic/Repeat logic)
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async GetAllQstnsForSectionsLogic(sectionID) {
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                data.append("SectionID", sectionID);
                data.append("LogicType", this.vmLogicType == null || this.vmLogicType == "" ? "Skip Logic" : this.vmLogicType);
                var result = await objSurvey.GetAllQstnsForSectionLogic(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        this.lstLogicQstns = result.data;

            },

            /// <summary>
            ///  GetAllFormulaFunctions :- Function used to get all formula operations in question calculation settings page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllFormulaFunctions() {
                var result = await objSurvey.GetAllFormulaFunctions();
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        this.FormulaOperationItems = result.data;
            },

            /// <summary>
            ///  GetAllLogicConditions :- Function used to fetch all logic conditions
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllLogicConditions() {
                var result = await objSurvey.GetAllLogicConditions();

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        this.lstLogicConditions = result.data;

            },

            /// <summary>
            ///  GetLogicAnswer :- Function used to get logic answer when logic question type as single choice/multi choice/dropdown
            /// </summary>
            /// <param name="QstnID"></param>
            /// <param name="indx"></param>
            /// <returns> </returns>
            async GetLogicAnswer(QstnID, indx) {
                if (indx == null) {
                    if (this.lstLogicQstns.length > 0) {
                        this.logicAnsType1 = this.lstLogicQstns.filter(x => x.QuestionID == QstnID)[0].QuestionType;
                        if (this.logicAnsType1 == 'Single Choice' || this.logicAnsType1 == 'Multi-Choice' || this.logicAnsType1 == 'Dropdown') {
                            var result = await objSurvey.GetLogicAnswers(QstnID);

                            if (result.status == "Unhandled Exception")
                                this.$router.push({ name: "error" });
                            else
                                if (result.data)
                                    this.lstLogicAnswers = result.data;
                        }
                    }
                }
                else {
                    this.logicAnsType[indx] = this.lstLogicQstns.filter(x => x.QuestionID == QstnID)[0].QuestionType;
                    if (this.logicAnsType[indx] == 'Single Choice' || this.logicAnsType[indx] == 'Multi-Choice' || this.logicAnsType[indx] == 'Dropdown') {
                        var result = await objSurvey.GetLogicAnswers(QstnID);

                        if (result.status == "Unhandled Exception")
                            this.$router.push({ name: "error" });
                        else
                            if (result.data)
                                this.lstLogicAnswers = result.data;
                    }
                }
            },

            /// <summary>
            ///  AddLogicItem :- Function used to add new logic condition
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddLogicItem() {
                this.SectionSkipLogicSavedCount = this.SectionSkipLogicSavedCount + 1;

                if (this.vmLogicQstn != "") {

                    var Ans = (this.vmLogicAnswer1 == "") ? this.vmLogicAnswer.OptionValue : this.vmLogicAnswer1;
                    if ((typeof Ans != "undefined") && (this.vmLogicCondition != null && this.vmLogicCondition != "")) {
                        // && (this.vmOperatorValue != 2)
                        this.lstLogicConditionItems.push({
                            SectionLogicID: 0, QuestionID: this.vmLogicQstn, ConditionID: this.vmLogicCondition, Answer: Ans, LogicOperator: this.vmOperatorValue
                        });
                        this.vmLogicQstn = "";
                        this.vmLogicCondition = null;
                        this.vmLogicAnswer = [];
                        this.vmLogicAnswer1 = "";
                        this.vmOperatorValue = 2;
                    }
                }
            },
            //AddRepeatLogicItem() {

            //    if (this.vmLogicQstn != "") {
            //        this.lstRepeatLogicConditionItems.push({ SectionLogicID: 0, QuestionID: this.vmLogicQstn });
            //        this.vmLogicQstn = "";
            //        this.vmLogicCondition = "";
            //        this.vmLogicAnswer = [];
            //        this.vmLogicAnswer1 = "";
            //        this.vmOperatorValue = 2;
            //    }
            //},

            /// <summary>
            ///  DeleteLogicItem :- Function used to delete section skip logic condition items
            /// </summary>
            /// <param name="conditionItem"></param>
            /// <returns> </returns>
            async DeleteLogicItem(conditionItem) {
                if (typeof conditionItem != "undefined") {
                    this.lstLogicConditionItems.splice(this.lstLogicConditionItems.indexOf(conditionItem), 1);
                    this.SectionSkipLogicSavedCount = this.SectionSkipLogicSavedCount - 1;
                    if (conditionItem.SectionLogicID > 0) {
                        this.DeleteSectionLogicItem(conditionItem.SectionLogicID);
                    }
                } else {
                    this.AddNew = false;
                    this.vmLogicQstn = "";
                    this.vmLogicCondition = "";
                    this.vmLogicAnswer = "";
                    this.vmLogicAnswer1 = "";
                    this.vmOperatorValue = 2;
                    if (this.SectionSkipLogicSavedCount > 1) {
                        this.SectionSkipLogicSavedCount = this.SectionSkipLogicSavedCount - 1;
                    }
                }
            },

            /// <summary>
            ///  DeleteRepeatLogicItem :- Function used to delete section repeat logic condition items
            /// </summary>
            /// <param name="conditionItem"></param>
            /// <returns> </returns>
            async DeleteRepeatLogicItem(conditionItem) {
                this.lstRepeatLogicConditionItems.splice(this.lstRepeatLogicConditionItems.indexOf(conditionItem), 1);
                this.vmRLogicQstn = "";
                if (conditionItem.SectionLogicID > 0)
                    this.DeleteSectionLogicItem(conditionItem.SectionLogicID);
            },

            /// <summary>
            ///  DeleteSectionLogicItem :- Function used to delete section logic items based on SectionLogicID
            /// </summary>
            /// <param name="SectionLogicID"></param>
            /// <returns> </returns>
            async DeleteSectionLogicItem(SectionLogicID) {

                var data = new FormData();
                data.append("SectionLogicID", SectionLogicID);
                var result = await objSurvey.DeleteSectionLogicItem(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "SurveySection Logic Condition removed Successfully ... ";
                        this.successSnackbarMsg = true;
                    }


                    else {
                        this.Msg = "Failed in removing new SurveySection Logic Condition.. ";
                        this.errorSnackbarMsg = true;
                    }

                }
            },

            /// <summary>
            ///  DeleteValidationItem :- Function used to delete question validation condition items
            /// </summary>
            /// <param name="valItem"></param>
            /// <returns> </returns>
            async DeleteValidationItem(valItem) {
                if (typeof valItem != "undefined") {
                    this.lstValidationItems.splice(this.lstValidationItems.indexOf(valItem), 1);
                    //if (conditionItem.SectionLogicID > 0) {
                    //    this.DeleteSectionLogicItem(conditionItem.SectionLogicID);
                    //}

                } else {
                    this.AddNewValidation = false;
                    this.vmQstValidationConditions = "";
                    this.vmOperatorValue = 2;
                }

            },

            /// <summary>
            ///  AddValidationItem :- Function used to add question validation condition items
            /// </summary>
            /// <param name="valItem"></param>
            /// <returns> </returns>
            async AddValidationItem(valItem) {

                //this.lstValidationItems.push({QstValidationCondition :this.vmQstValidationConditions});
                //this.vmQstValidationConditions = [];
                if (this.vmQstValidationConditions != null && this.vmQstValidationConditions != "" && (this.vmOperatorValue != 2 || this.lstValidationItems.length == 0)) {

                    this.lstValidationItems.push({
                        QuestionID: 0, ValidationCondition: this.vmQstValidationConditions, ConditionValue: this.vmQstValidationVal,
                        MinValue: this.vmMinValue, MaxValue: this.vmMaxValue, LogicOperator: this.vmOperatorValue
                    });
                    this.vmQstValidationConditions = [];
                    this.vmQstValidationVal = "";
                    this.vmMinValue = 0;
                    this.vmMaxValue = 0;
                    this.vmOperatorValue = 2;
                }

            },

            /// <summary>
            ///  ChangeLogicOperator :- Function used to change logic operator in validations and logic tab (AND/OR)
            /// </summary>
            /// <param name="oprVal"></param>
            /// <param name="indx"></param>
            /// <returns> </returns>
            async ChangeLogicOperator(oprVal, indx) {
                if (oprVal == 0)
                    this.lstLogicConditionItems[indx].LogicOperator = 0;
                else if (oprVal == 1)
                    this.lstLogicConditionItems[indx].LogicOperator = 1;
            },

            /// <summary>
            ///  ClearLogic :- Function used to cancel logic details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async ClearLogic() {
                this.lstLogicConditionItems = [];
                this.dlgSecSettings = false;
                this.GetAllLogicConditions();
                this.GetAllQstnsForSectionsLogic();

                this.GetLogicAnswer();
                this.logicAnsType = [];
                this.logicAnsType1 = "";
                this.vmLogicCondition = "";
                this.vmLogicQstn = "";
                this.vmRLogicQstn = "";
                this.vmLogicType = "";
                this.vmLogicAnswer = "";
                this.vmLogicAnswer1 = "";
                this.SectionSkipLogicSavedCount = 1;

            },

            /// <summary>
            ///  GetSurveySectionDetails :- Function used to fetch survey section details for edit section
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            GetSurveySectionDetails(sectionID) {
                this.SectionID = sectionID;
                this.vmSectionName = this.lstSections.filter(x => x.SectionID == sectionID)[0].SectionName;
                this.vmSectionDesc = this.lstSections.filter(x => x.SectionID == sectionID)[0].SectionDescription;
            },

            /// <summary>
            ///  SaveSurveySection :- Function used to add new survey section
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async SaveSurveySection(sectionID) {

                if (this.vmSectionName == "") {
                    this.Msg = "Section Name is required";
                    this.infoSnackbarMsg = true;
                    return;
                }

                /*Chk Duplicate*/

                for (var i = 0; i < this.lstSurveySections.length; i++) {
                    if (this.lstSurveySections[i].SectionName == this.vmSectionName && this.lstSurveySections[i].SectionDescription == this.vmSectionDesc) {
                        this.Msg = "Duplicate Entry - Same Section Name existis..";
                        this.errorSnackbarMsg = true;
                        return;
                    }
                }
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                data.append("SectionID", sectionID);
                data.append("SectionName", this.vmSectionName);
                data.append("SectionDescription", this.vmSectionDesc);
                var result = await objSurvey.SaveProjectSurveySection(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "SurveySection Updated Successfully ... ";
                        this.successSnackbarMsg = true;
                    }

                    else {
                        this.Msg = "Failed in updating new SurveySection.. ";
                        this.errorSnackbarMsg = true;
                    }
                    this.dlgAddSection = false;
                    this.vmSectionName = "";
                    this.vmSectionDesc = "";
                    this.GetAllSections();
                }
            },

            /// <summary>
            ///  RemoveSurveySection :- Function used to remove survey section
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async RemoveSurveySection(SectionID) {

                var data = new FormData();
                data.append("SectionID", SectionID);
                var result = await objSurvey.RemoveSurveySection(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {

                    if (result.status == "success") {
                        this.Msg = "SurveySection removed Successfully ... ";
                        this.successSnackbarMsg = true;
                    }

                    else {
                        this.Msg = "Failed in removing new SurveySection.. ";
                        this.errorSnackbarMsg = true;
                    }

                    this.dlgRemoveSection = false;
                    this.vmSectionName = "";
                    this.vmSectionDesc = "";
                    this.GetAllSections();

                }
            },

            /// <summary>
            ///  UpdateSectionLogic :- Function used to update section logic based on SectionID
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async UpdateSectionLogic(SectionID) {

                if (this.vmLogicType == "Skip Logic") {
                    if (this.lstLogicConditionItems.length == 0) {
                        var Ans = (this.vmLogicAnswer1 == "") ? this.vmLogicAnswer.OptionValue : this.vmLogicAnswer1;
                        if (this.vmLogicQstn != "" && (typeof Ans != "undefined") && (this.vmLogicCondition != null && this.vmLogicCondition != "")) {
                            this.lstLogicConditionItems.push({
                                SectionLogicID: 0,
                                QuestionID: this.vmLogicQstn, ConditionID: this.vmLogicCondition, Answer: Ans, LogicOperator: this.vmOperatorValue
                            });
                        }
                        else {
                            this.Msg = "Please select all the required feilds";
                            this.infoSnackbarMsg = true;
                            return;
                        }
                        this.lstLogicItem = this.lstLogicConditionItems;
                    }
                    if (this.lstLogicConditionItems.length > 0) {
                        this.lstLogicItem = this.lstLogicConditionItems;
                        var Ans = (this.vmLogicAnswer1 == "") ? this.vmLogicAnswer.OptionValue : this.vmLogicAnswer1;
                        if ((this.SectionSkipLogicSavedCount > this.lstLogicConditionItems.length)) {
                            if (this.vmLogicQstn != "" && (typeof Ans != "undefined") && (this.vmLogicCondition != null && this.vmLogicCondition != "")) {
                                this.lstLogicConditionItems.push({
                                    SectionLogicID: 0,
                                    QuestionID: this.vmLogicQstn, ConditionID: this.vmLogicCondition, Answer: Ans, LogicOperator: this.vmOperatorValue
                                });
                            }
                            else {
                                this.Msg = "Please select all the required feilds";
                                this.infoSnackbarMsg = true;
                                return;
                            }
                        }
                    }

                }
                else if (this.vmLogicType == "Repeat Logic") {
                    if (this.lstRepeatLogicConditionItems.length == 0) {
                        if (this.vmRLogicQstn != "") {
                            this.lstRepeatLogicConditionItems.push({ SectionLogicID: 0, QuestionID: this.vmRLogicQstn });
                            this.lstLogicItem = this.lstRepeatLogicConditionItems;
                        }
                        else {
                            this.SectionLogic = true;
                            this.SectionLogicValidationMsg = "Please select a question";
                            return;
                        }
                    }
                    else if (this.lstRepeatLogicConditionItems.length > 0) {
                        this.lstLogicItem = this.lstRepeatLogicConditionItems;
                    }
                }

                var data = new FormData();
                data.append("SectionID", SectionID);
                data.append("SurveyID", this.SurveyID);
                data.append("LogicType", this.vmLogicType);
                data.append("LogicConditionList", JSON.stringify(this.lstLogicItem));
                var result = await objSurvey.UpdateSectionLogic(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "SurveySection Logic updated Successfully ... ";
                        this.successSnackbarMsg = true;
                    } else if (result.status == "nestedQuestion") {
                        this.Msg = "Nested skip logics or repeat logic not allowed..";
                        this.errorSnackbarMsg = true;
                    }
                    else {
                        this.Msg = "Failed in updating SurveySection Logic.. ";
                        this.errorSnackbarMsg = true;
                    }

                    this.GetAllSections();

                }
                this.dlgSecSettings = false;
                this.vmLogicQstn = "";
                this.vmLogicCondition = "";
                this.vmLogicAnswer = [];
                this.vmLogicAnswer1 = "";
                this.vmOperatorValue = 2;
                this.lstLogicConditionItems = [];
                this.lstRepeatLogicConditionItems = [];
                this.SectionSkipLogicSavedCount = 1;
                this.ClearLogic();

            },

            /// <summary>
            ///  UpdateSectionOrder :- Function used to change section order by dragging sections
            /// </summary>
            /// <param name="SecItem"></param>
            /// <returns> </returns>
            async UpdateSectionOrder(SecItem) {
                var lstSecOrder = []
                for (var i = 0; i < SecItem.length; i++)
                    lstSecOrder.push({ SectionID: SecItem[i].SectionID, SectionOrder: (i + 1) })
                var data = new FormData();
                data.append("SectionOrderList", JSON.stringify(lstSecOrder));
                var result = await objSurvey.UpdateSectionOrder(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Section Order Changed";
                        this.successSnackbarMsg = true;
                    }

            },
            //==============Questions=====================//

            /// <summary>
            ///  GetAllQuestionTypes :- Function used to fetch all question type while adding new question
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllQuestionTypes() {
                var result = await objSurvey.GetAllQuestionTypes();

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {

                        this.lstQstnTypes = result.data;
                    }
            },

            /// <summary>
            ///  GetCascadingQuestionOptions :- Function used to fetch cascading question options
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetCascadingQuestionOptions() {
                var result = await objSurvey.GetCascadingQuestionOptions();

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstCascadingQstnOptions = result.data;
                    }
            },

            /// <summary>
            ///  SaveCascadingOptions :- Function used to save cascading question options
            /// </summary>
            /// <param name="OptionID"></param>
            /// <returns> </returns>
            async SaveCascadingOptions(OptionID) {
                var result = await objSurvey.GetCascadingQuestionSubOptions(OptionID);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data) {
                        this.lstCascadingQstnSubOptions = result.data;
                    }
                this.lstOptions = [];
                for (var i = 0; i < this.lstCascadingQstnSubOptions.length; i++)
                    this.lstOptions.push({ OptionValue: this.lstCascadingQstnSubOptions[i].OptionValue });

            },

            /// <summary>
            ///  GetSectionQstnSetting :- Function used to get section question settings
            /// </summary>
            /// <param name="sectionID"></param>
            /// <param name="QuestionID"></param>
            /// <returns> </returns>
            GetSectionQstnSetting(sectionID, QuestionID) {
                this.GetAllFormulaFunctions();
                this.lstQstns = this.lstSections.filter(x => x.SectionID == sectionID)[0].SurveySecQuestions;
                if (QuestionID > 0) {
                    (this.vmQuestionHint != "null" || this.vmQuestionHint != null) ? this.vmQuestionHint : "";

                    this.vmDefaultVal = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].DefaultValue;
                    // this.vmDefaultVal = (vaDefaultVal != null) ? vaDefaultVal : "";

                    this.vmQuestionHint = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].QuestionHint;
                    this.vmIsUnique = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].IsUnique;
                    this.vmIsSearchable = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].IsSearchable;
                    this.vmIsMandatory = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].IsMandatory;
                    this.vmFormulaExp = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].FormulaExpression;
                    this.vmFormulaOperation = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].FormulaOperation;

                    this.vmRegEx = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].RegEx;
                    this.vmErrorText = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].ErrorText;

                    this.lstValidationItems = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].lstQuestionCondition;
                    this.vmQstnCategory = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].QuestionCategory;

                    this.IsCalcavailable = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].IsCalcavailable;
                    this.IsValavailable = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].IsValavailable;

                    this.vmFormulaFieldVar = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].FormulaFieldVariables;

                    if (this.vmFormulaFieldVar != null && this.vmFormulaFieldVar != "") {
                        var arr = this.vmFormulaFieldVar.split(",");
                        var Qname = this.lstQstns.filter(x => (x.SectionID == sectionID) && (x.QuestionID == QuestionID))[0].QuestionName;
                        for (var i = 0; i < arr.length; i++) {
                            this.lstFieldVars.push({ FieldVar: arr[i], QuestionName: Qname });
                        }
                    }
                }
                /*
                //this.QstnCalcVarItems = this.lstQstns.filter(x => (x.QuestionTypeID == 1) || (x.QuestionTypeID == 5));
                this.QstnCalcVarItems = this.lstQstns.filter(x => (x.QuestionTypeID == 5));
                */
                this.AddNewValidation = false;

                var lstAllCalQstns = [];
                this.QstnCalcVarItems = [];
                for (var i = 0; i < this.lstSections.length; i++) {
                    lstAllCalQstns.push(this.lstSections[i].SurveySecQuestions);
                    if (typeof lstAllCalQstns[0].filter(x => (x.QuestionTypeID == 5 || x.QuestionTypeID == 16 || x.QuestionTypeID == 3))[0] !== "undefined") {
                        var temp = lstAllCalQstns[0].filter(x => (x.QuestionTypeID == 5 || x.QuestionTypeID == 16 || x.QuestionTypeID == 3) && x.QuestionID != QuestionID);
                        if (temp.length > 0) {
                            for (var j = 0; j <= temp.length - 1; j++) {
                                this.QstnCalcVarItems.push(temp[j]);
                            }
                        }

                    }
                    lstAllCalQstns = [];
                }
            },

            /// <summary>
            ///  AddNewQuestion :- Function used to add new question section based on sectionID
            /// </summary>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async AddNewQuestion(sectionID) {
                this.PreLoader = true;
                this.SectionID = sectionID;
                // this.vmFormulaFieldVar = [];
                //for (var i = 0; i < this.formulaVariables.length; i++) {
                //    this.vmFormulaFieldVar = this.vmFormulaFieldVar == "" ? this.formulaVariables[i].FieldVar : this.vmFormulaFieldVar + "," + this.formulaVariables[i].FieldVar;
                //}

                if (this.vmQstn != "" && this.vmQuestionType != []) {
                    if (this.vmQuestionType.Type == "Likert Scale")
                        this.lstOptions = this.Likerts;
                    var lstOpts = this.lstOptions.filter(x => x.OptionValue != null);

                    this.lstQstns.push({
                        SurveyID: this.SurveyID, SectionID: sectionID, QuestionID: 0, QuestionName: this.vmQstn, QuestionTypeID: this.vmQuestionType.ID,
                        lstQuestionOptions: lstOpts, QuestionHint: this.vmQuestionHint, DefaultValue: this.vmDefaultVal, IsUnique: this.vmIsUnique,
                        IsSearchable: this.vmIsSearchable, IsMandatory: this.vmIsMandatory, FormulaOperation: this.vmFormulaOperation, FormulaExpression: this.vmFormulaExp,
                        IsFormulaQuestion: this.vmFormulaExp != "" ? true : false, FormulaFieldVariables: this.vmFormulaFieldVar, RegEx: this.vmRegEx,
                        ErrorText: this.vmErrorText, lstQuestionValidations: this.lstValidationItems, IsValidationQuestion: this.lstValidationItems.length > 0 ? true : false
                        , UOM: this.vmUOM, IsOtherOptionSelected: this.IsOtherOptionSelected
                        //MinValue: this.vmMinValue, MaxValue: this.vmMaxValue, ValidationCondition: this.vmQstValidationConditions,
                    });
                }

                this.SaveQuestion(this.lstQstns);
                this.PreLoader = false;
            },

            /// <summary>
            ///  SaveQuestion :- Function used to save new question
            /// </summary>
            /// <param name="lstQstn"></param>
            /// <returns> </returns>
            async SaveQuestion(lstQstn) {
                var lstNewQstn = lstQstn.filter(x => x.QuestionID == 0);

                if (lstNewQstn != null && lstNewQstn != undefined && lstNewQstn.length >0 && lstNewQstn[0].IsOtherOptionSelected == true) {
                    var other = new Object();
                    other.OptionValue = "Others";
                    lstNewQstn[0].lstQuestionOptions.push(other);
                }

                var data = new FormData();
                data.append("SecQstnList", JSON.stringify(lstNewQstn));
                var result = await objSurvey.AddSurveySectionQstn(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success") {
                        this.Msg = "Survey Section Questions added Successfully ... ";
                        this.successSnackbarMsg = true;
                    }

                    else {
                        this.Msg = "Duplicate QuestionName/Failed to update SurveySection Questions.. ";
                        this.errorSnackbarMsg = true;
                    }

                    this.ClearAddQstn();
                    this.GetAllSections();

                }
            },

            /// <summary>
            ///  CopyQuestion :- Function used to copy question details based on QuestionID
            /// </summary>
            /// <param name="QuestionID"></param>
            /// <returns> </returns>
            async CopyQuestion(QuestionID) {
                var lstQstn = [];
                var lstNQstn = [];

                var result = await objSurvey.GetQuestionDetails(QuestionID);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.data)
                        lstQstn = result.data;

                this.CopyQstnSettings(lstQstn);
                this.CopyQstnValidation(lstQstn);
                this.CopyQstnCalculation(lstQstn);

                // Question Name
                var QuestionName = lstQstn[0].QuestionName
                var Sections = this.lstSections.filter(x => x.SectionID == lstQstn[0].SectionID);
                if (Sections.length > 0) {
                    var QuestionsInSections = Sections[0].SurveySecQuestions.filter(x => x.QuestionName.includes(lstQstn[0].QuestionName));
                    QuestionName = QuestionName + "-Copy(" + QuestionsInSections.length + ")";
                }
                lstNQstn.push({
                    SurveyID: this.SurveyID, SectionID: lstQstn[0].SectionID, QuestionID: 0, QuestionName: QuestionName,
                    QuestionTypeID: lstQstn[0].QuestionTypeID, lstQuestionOptions: lstQstn[0].lstQuestionOptions, QuestionHint: this.vmQuestionHint,
                    DefaultValue: this.vmDefaultVal, IsUnique: this.vmIsUnique, IsSearchable: this.vmIsSearchable, IsMandatory: this.vmIsMandatory,
                    FormulaOperation: this.vmFormulaOperation, FormulaExpression: this.vmFormulaExp, IsFormulaQuestion: this.vmFormulaExp != "" ? true : false,
                    FormulaFieldVariables: this.vmFormulaFieldVar, RegEx: this.vmRegEx,
                    //MinValue: this.vmMinValue, MaxValue: this.vmMaxValue,  ValidationCondition: this.vmQstValidationConditions,
                    ErrorText: this.vmErrorText, lstQuestionValidations: this.lstValidationItems, IsValidationQuestion: this.lstValidationItems.length > 0 ? true : false
                    , UOM: this.vmUOM, IsOtherOptionSelected: lstQstn[0].IsOtherOptionSelected
                });

                this.SaveQuestion(lstNQstn);
                lstNQstn = [];
            },

            /// <summary>
            ///  CopyQstnSettings :- Function used to copy question settings
            /// </summary>
            /// <param name="lstQstn"></param>
            /// <returns> </returns>
            CopyQstnSettings(lstQstn) {
                if (this.vmChkSettings) {
                    this.vmQuestionHint = lstQstn[0].QuestionHint;
                    this.vmDefaultVal = lstQstn[0].DefaultValue;
                    this.vmIsUnique = lstQstn[0].IsUnique;
                    this.vmIsSearchable = lstQstn[0].IsSearchable;
                    this.vmIsMandatory = lstQstn[0].IsMandatory;
                }
                else {
                    this.vmQuestionHint = ""
                    this.vmDefaultVal = "";
                    this.vmIsUnique = 0;
                    this.vmIsSearchable = 0;
                    this.vmIsMandatory = 0;
                }
            },

            /// <summary>
            ///  CopyQstnValidation :- Function used to copy question validation
            /// </summary>
            /// <param name="lstQstn"></param>
            /// <returns> </returns>
            CopyQstnValidation(lstQstn) {
                if (this.vmChkValidation) {

                    //this.vmMinValue = lstQstn[0].MinValue;
                    //this.vmMaxValue = lstQstn[0].MaxValue;
                    //this.vmQstValidationConditions = lstQstn[0].ValidationCondition;

                    this.lstValidationItems = lstQstn[0].lstQuestionCondition;
                    this.vmRegEx = lstQstn[0].RegEx;
                    this.vmErrorText = lstQstn[0].ErrorText;
                }
                else {
                    this.vmMinValue = 0;
                    this.vmMaxValue = 0;
                    this.vmRegEx = "";
                    this.vmQstValidationConditions = "";
                    this.vmErrorText = "";
                }
            },

            /// <summary>
            ///  CopyQstnCalculation :- Function used to copy question calculation
            /// </summary>
            /// <param name="lstQstn"></param>
            /// <returns> </returns>
            CopyQstnCalculation(lstQstn) {
                if (this.vmChkCalculation) {
                    this.vmFormulaOperation = lstQstn[0].FormulaOperation;
                    this.vmFormulaExp = lstQstn[0].FormulaExpression;
                    this.vmFormulaFieldVar = lstQstn[0].FormulaFieldVariables;
                }
                else {
                    this.vmFormulaOperation = lstQstn[0].FormulaOperation;
                    this.vmFormulaExp = lstQstn[0].FormulaExpression;
                    this.vmFormulaFieldVar = lstQstn[0].FormulaFieldVariables;
                }
            },

            /// <summary>
            ///  ClearAddQstn :- Function used to cancel add question dialog
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            ClearAddQstn() {
                this.lstQstns = [];
                this.vmQstn = "";
                this.vmQuestionType = [];
                this.lstOptions = [];
                this.lstQuestionCondition = [];
                this.SingleChoices = [{ OptionValue: "" }];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.vmUOM = "";
                this.IsOtherOptionSelected = false;

                this.Likerts = [];
                this.Likerts = [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ];
                this.ClearQstnSettingsData();
            },

            /// <summary>
            ///  ClearQstnSettingsData :- Function used to clear question settings data
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            ClearQstnSettingsData() {
                this.Tab = 'tab-1';
                this.vmQuestionHint = "";
                this.vmDefaultVal = "";
                this.vmIsUnique = 0;
                this.vmIsSearchable = 0;
                this.vmIsMandatory = 0;

                this.vmMinValue = 0;
                this.vmMaxValue = 0;
                this.vmRegEx = "";
                this.vmErrorText = "";

                this.vmFormulaFieldVar = "";
                this.vmQstValidationConditions = "";
                this.vmFormulaExp = "";
                this.vmFormulaOperation = "";
                this.lstFieldVars = [];
                this.FormulaOperationItems = [];//FormulaOperationItems: ['Average', 'Sum','Percentage'];
                QstValidationItems: [
                    'Email', 'Phone Number', 'Should be less that Current date', 'Should be greater that Current date', 'Min & Max Range', 'Aadhaar Number'
                ];
                this.lstValidationItems = [];
                this.AddNewValidation = false;
                this.vmQstnCalcVar = [];

            },

            /// <summary>
            ///  AddOptionItem :- Function used to add option value when question type as single choice/multi choice/dropdown
            /// </summary>
            /// <param name="IsLikert"></param>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            AddOptionItem(IsLikert, QItem) {
                if (!IsLikert) {
                    if (typeof QItem.OptionValue != "undefined" && QItem.OptionValue != "") {

                        this.SingleChoices.push({ OptionValue: "" });
                        this.lstOptions = this.SingleChoices.filter(x => x.OptionValue != "" || x.OptionValue == "");
                    }
                }
                else {
                    if (typeof QItem.OptionValue != "undefined" && QItem.OptionValue != "") {
                        this.Likerts.push({});
                        this.lstOptions = this.Likerts.filter(x => x.OptionValue != "")
                    }
                }
            },

            /// <summary>
            ///  DeleteOptionItem :- Function used to delete option items
            /// </summary>
            /// <param name="SingleChoice"></param>
            /// <param name="IsLikert"></param>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            DeleteOptionItem(SingleChoice, IsLikert, QItem) {
                if (!IsLikert) {

                    if (typeof QItem != "undefined") {
                        this.SingleChoices.splice(this.SingleChoices.indexOf(SingleChoice), 1);
                    } else {
                        this.SingleChoices.splice(this.SingleChoices.indexOf(SingleChoice), 1);
                        this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);
                    }
                }
                else {

                    //this.Likerts.splice(this.Likerts.indexOf(SingleChoice), 1);
                    //this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);.
                    if (typeof QItem != "undefined") {
                        var optList = QItem.lstQuestionOptions;
                        this.lstOptions = optList.filter(x => x.OptionValue != "");
                        var index = 0;
                        for (var i = 0; i <= this.lstOptions.length - 1; i++) {
                            if (this.lstOptions[i].OptionValue == SingleChoice.OptionValue) {
                                index = i;
                                break;
                            }
                        }
                        //var temp = this.lstOptions.filter(x => x.OptionValue == "Bad");
                        optList.splice(optList.indexOf(this.lstOptions[index]), 1);
                        this.lstOptions.splice(this.lstOptions.indexOf(this.lstOptions[index]), 1);

                        this.lstSections.filter(x => x.SectionID == this.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].lstQuestionOptions = optList;
                        this.Likerts.splice(this.Likerts.indexOf(SingleChoice), 1);
                    } else {
                        this.Likerts.splice(this.Likerts.indexOf(SingleChoice), 1);
                        this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);
                    }
                }
            },

            /// <summary>
            ///  EditOptionItems :- Function used to edit option items
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            EditOptionItems(QItem) {

                var OptionListCount = 0;
                OptionListCount = QItem.lstQuestionOptions.length - 1;
                var optList = QItem.lstQuestionOptions;
                if (typeof optList[OptionListCount].OptionValue != "undefined" && optList[OptionListCount].OptionValue != null && optList[OptionListCount].OptionValue != "") {
                    optList.push({});
                    this.lstOptions = optList.filter(x => x.OptionValue != "");
                    this.lstSections.filter(x => x.SectionID == this.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].lstQuestionOptions = optList;
                }
            },

            /// <summary>
            ///  EditOptionItemsLIkert :- Function used to edit option items of likert question type
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            EditOptionItemsLIkert(QItem) {

                var OptionCount = QItem.lstQuestionOptions.length - 1;
                if (typeof QItem.lstQuestionOptions[OptionCount].OptionValue != "undefined" && QItem.lstQuestionOptions[OptionCount].OptionValue != "") {
                    var optList = QItem.lstQuestionOptions;
                    optList.push({});
                    this.lstOptions = optList.filter(x => x.OptionValue != "");
                    this.lstSections.filter(x => x.SectionID == this.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].lstQuestionOptions = optList;
                }
            },

            /// <summary>
            ///  EditDeleteOptionItem :- Function used to delete option items of single choice answer in edit mode
            /// </summary>
            /// <param name="SingleChoice"></param>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            EditDeleteOptionItem: function (SingleChoice, QItem) {
                var optList = QItem.lstQuestionOptions;
                this.lstOptions = optList.filter(x => x.OptionValue != "");
                optList.splice(optList.indexOf(SingleChoice), 1);
                this.lstOptions.splice(this.lstOptions.indexOf(SingleChoice), 1);
                this.lstSections.filter(x => x.SectionID == this.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].lstQuestionOptions = optList;

            },

            /// <summary>
            ///  AddQstnCalcVariables :- Function used to add question calculation variables
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddQstnCalcVariables() {
                if (typeof this.vmQstnCalcVar.FieldVar != "undefined" && typeof this.vmQstnCalcVar.QuestionName != "undefined") {
                    this.lstFieldVars.push({ FieldVar: this.vmQstnCalcVar.FieldVar, QuestionName: this.vmQstnCalcVar.QuestionName });
                    this.vmQstnCalcVar = [];
                }
            },

            /// <summary>
            ///  DeleteFieldVarItem :- Function used to remove variable items in question calculation tab
            /// </summary>
            /// <param name="fieldVarItem"></param>
            /// <returns> </returns>
            DeleteFieldVarItem(fieldVarItem) {

                this.lstFieldVars.splice(this.lstFieldVars.indexOf(fieldVarItem), 1);
                if (this.formulaVariables.length > 0) {
                    var vaDeleteItem = this.formulaVariables.filter(x => x.FieldVar == fieldVarItem.FieldVar);
                    this.formulaVariables.splice(this.formulaVariables.indexOf(vaDeleteItem), 1);
                }
                this.CalculateFormulaExpression();
                if (this.lstFieldVars.length == 0) {
                    this.vmFormulaExp = "";
                    this.tempExp = "";
                    this.vmFormulaExp = "";
                    this.formulaVariables = [];
                }
            },

            /// <summary>
            ///  CalculateFormulaExpression :- Function used to calculate formula expression
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            CalculateFormulaExpression() {

                this.vmFormulaExp = "";
                this.tempExp = "";
                this.formulaVariables = [];
                if (this.vmFormulaOperation == null) {
                    this.Msg = "Please Select formula Operation ";
                    this.infoSnackbarMsg = true;
                    return;
                }
                else {
                    if (this.vmQstnCalcVar != "") {
                        this.lstFieldVars.push({ FieldVar: this.vmQstnCalcVar.FieldVar, QuestionName: this.vmQstnCalcVar.QuestionName });
                        this.vmQstnCalcVar = [];
                    }

                    var action = "+";

                    switch (this.vmFormulaOperation) {
                        case "Subtraction":
                            action = "-";
                            break;
                        case "Multiplication":
                            action = "*";
                            break;
                        case "RepeatVal_Aggregate_Multiplication":
                            action = "*";
                            break;
                        case "RepeatVal_Aggregate_Subtraction":
                            action = "-";
                            break;
                        default:
                    }

                    for (var i = 0; i < this.lstFieldVars.length; i++) {
                        this.tempExp = this.tempExp == "" ? this.lstFieldVars[i].FieldVar : this.tempExp + action + this.lstFieldVars[i].FieldVar;
                        this.formulaVariables.push({ FieldVar: this.lstFieldVars[i].FieldVar });
                    }

                    if (this.tempExp != "") {
                        if (this.vmFormulaOperation == 'Average') {
                            this.vmFormulaExp = "(" + this.tempExp + ") /" + this.lstFieldVars.length;
                        } else if (this.vmFormulaOperation == 'Sum' || this.vmFormulaOperation == 'Subtraction' || this.vmFormulaOperation == 'Multiplication') {
                            this.vmFormulaExp = this.tempExp;
                        } else if (this.vmFormulaOperation != 'Generic_Formula') {
                            this.tempExp = "";
                            for (var i = 0; i < this.lstFieldVars.length; i++) {
                                this.tempExp = this.tempExp == "" ? this.lstFieldVars[i].FieldVar : this.tempExp + "," + this.lstFieldVars[i].FieldVar;
                            }

                            this.vmFormulaExp = this.vmFormulaOperation + "(" + this.tempExp + ")";
                        }
                    }
                }
            },

            /// <summary>
            ///  UpdateQuestionSettings :- Function used to update question settings data
            /// </summary>
            /// <param name="QuestionID"></param>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            async UpdateQuestionSettings(QuestionID, sectionID) {

                if (this.vmFormulaOperation == "Generic_Formula") {
                    if (this.vmFormulaExp.length < 10) {  // eg:- var_1 * var_2 , minimum length for a transaction
                        this.infoSnackbarMsg = true;
                        this.copyMessage = "Please fill with the proper formula expression.";
                        return false;
                    }

                    var calcuation = "+,-,*,%,(,),/";
                    var splt = calcuation.split(",");

                    var variablefromformula = this.vmFormulaExp;

                    for (var j = 0; j < splt.length; j++) {
                        variablefromformula = variablefromformula.replaceAll(splt[j], " ");
                    }
                    variablefromformula = variablefromformula.trim().replace(/  +/g, ' ');

                    this.formulaVariables = variablefromformula.split(' ');

                    if (this.formulaVariables.length >= 0) {

                        this.vmFormulaFieldVar = "";
                        for (var i = 0; i < this.formulaVariables.length; i++) {
                            if (this.formulaVariables[i].includes("var")) {
                                this.vmFormulaFieldVar = this.vmFormulaFieldVar == "" ? this.formulaVariables[i] : this.vmFormulaFieldVar + "," + this.formulaVariables[i];
                            }
                        }
                    }
                } else {
                    if (this.formulaVariables.length >= 0) {
                        this.vmFormulaFieldVar = "";
                        for (var i = 0; i < this.formulaVariables.length; i++) {
                            this.vmFormulaFieldVar = this.vmFormulaFieldVar == "" ? this.formulaVariables[i].FieldVar : this.vmFormulaFieldVar + "," + this.formulaVariables[i].FieldVar;
                        }
                    }
                }

                //if (this.formulaVariables.length == 0 && this.lstFieldVars.length > 0)
                //    for (var i = 0; i < this.lstFieldVars.length; i++)
                //        this.formulaVariables.push({ FieldVar: this.lstFieldVars[i].FieldVar });



                if (this.vmQstValidationConditions != null) {
                    if (this.vmQstValidationConditions.length != 0) {

                        this.lstValidationItems.push({
                            QuestionID: 0, ValidationCondition: this.vmQstValidationConditions, ConditionValue: this.vmQstValidationVal,
                            MinValue: this.vmMinValue, MaxValue: this.vmMaxValue, LogicOperator: this.vmOperatorValue
                        });
                    }
                }

                this.vmQstValidationConditions = [];
                this.vmOperatorValue = 2;

                var vaFormulaOperation = (this.vmFormulaOperation != "null" || this.vmFormulaOperation != null) ? this.vmFormulaOperation : "";
                if (this.vmFormulaExp == "") {
                    vaFormulaOperation = "";
                }

                if (QuestionID > 0) {

                    var data = new FormData();
                    data.append("QuestionID", QuestionID);
                    data.append("QuestionHint", (this.vmQuestionHint != "null" || this.vmQuestionHint != null) ? this.vmQuestionHint : "");
                    data.append("IsMandatory", this.vmIsMandatory);
                    data.append("DefaultValue", (this.vmDefaultVal != "null" || this.vmDefaultVal != null) ? this.vmDefaultVal : "");
                    data.append("IsSearchable", this.vmIsSearchable);
                    data.append("IsUnique", this.vmIsUnique);
                    data.append("RegEx", (this.vmRegEx != "null" || this.vmRegEx != null) ? this.vmRegEx : "");
                    data.append("ErrorText", (this.vmErrorText != "null" || this.vmErrorText != null) ? this.vmErrorText : "");
                    data.append("ValidationConditions", JSON.stringify(this.lstValidationItems));

                    //data.append("ValidationCondition", this.vmQstValidationConditions);
                    //data.append("MinValue", this.vmMinValue);
                    //data.append("MaxValue", this.vmMaxValue);

                    data.append("FormulaExpression", (this.vmFormulaExp != "null" || this.vmFormulaExp != null) ? this.vmFormulaExp : "");
                    data.append("IsFormulaQuestion", (this.vmFormulaExp != "" ? true : false));
                    data.append("FormulaOperation", vaFormulaOperation);
                    data.append("FormulaFieldVariables", (this.vmFormulaFieldVar != "null" || this.vmFormulaFieldVar != null) ? this.vmFormulaFieldVar : "");
                    data.append("IsValidationQuestion", this.lstValidationItems.length > 0 ? true : false);

                    var result = await objSurvey.UpdateQuestionSettings(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.status == "success") {
                            this.Msg = "Survey Section Questions Settings updated Successfully ... ";
                            this.successSnackbarMsg = true;
                        }

                        else {
                            this.Msg = "Failed in updating SurveySection Question Settings.. ";
                            this.errorSnackbarMsg = true;
                        }

                        this.GetAllSections();
                    }

                    this.ClearQstnSettingsData();
                }
                this.dlgSectionQstnSetting = false;

            },

            /// <summary>
            ///  RemoveSurveySectionQuestion :- Function used to remove survey section questions
            /// </summary>
            /// <param name="sectionID"></param>
            /// <param name="QuestionID"></param>
            /// <returns> </returns>
            async RemoveSurveySectionQuestion(sectionID, QuestionID) {
                if (QuestionID < 0) {
                    this.lstQstns = this.lstSections.filter(x => x.SectionID == sectionID)[0].SurveySecQuestions.filter(x => x.QuestionID != QuestionID);
                    this.lstSections.filter(x => x.SectionID == this.SectionID).SurveySecQuestions = this.lstQstns;
                }
                else {
                    var data = new FormData();
                    data.append("SectionID", sectionID);
                    data.append("QuestionID", QuestionID);
                    var result = await objSurvey.RemoveSurveySectionQuestion(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.status == "success") {
                            this.Msg = "Survey Section Questions removed Successfully ... ";
                            this.successSnackbarMsg = true;
                        }


                        else {
                            this.Msg = "Failed in updating SurveySection Questions.. ";
                            this.errorSnackbarMsg = true;
                        }

                        this.dlgRemoveSection = false;
                        this.ClearQstnSettingsData();
                        this.GetAllSections();
                    }
                }
            },

            /// <summary>
            ///  EditQuestion :- Function used to edit section questions
            /// </summary>
            /// <param name="QItem"></param>
            /// <param name="idx"></param>
            /// <param name="sectionID"></param>
            /// <returns> </returns>
            EditQuestion(QItem, idx, sectionID) {
                this.IsOtherOptionSelected = false;
                if (QItem.QuestionType == "Likert Scale") {
                    this.likerIndex = idx;
                    this.likerSection = sectionID;
                }
                this.preQstn = this.lstSections.filter(x => x.SectionID == QItem.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].QuestionName;
                this.preAnsType = this.lstSections.filter(x => x.SectionID == QItem.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].QuestionType;

                this.vmEQuestionName = QItem.QuestionName;
                this.vmEQuestionType.ID = QItem.QuestionTypeID;
                this.vmEQuestionType.Type = QItem.QuestionType;
                this.vmQstnCategory = QItem.QuestionCategory;
                this.IsCalcavailable = QItem.IsCalcavailable;
                this.IsValavailable = QItem.IsValavailable;
                this.SectionID = QItem.SectionID;
                this.lstSections.filter(x => x.SectionID == QItem.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].IsEditQstn = true;

                for (var i = 0; i < this.lstSections.length; i++) {
                    this.lstSections[i].UpdateQstnSection = true;
                    this.lstSections[i].questionPanel = false;
                }

                if (QItem.IsOtherOptionSelected || QItem.IsOtherOptionSelected == "true") {
                    this.IsOtherOptionSelected = true;
                }

            },

            /// <summary>
            ///  EditQuestionType :- Function used to edit question type
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            EditQuestionType(QItem) {
                QItem.QuestionType = this.vmEQuestionType.Type;
                this.preCasOption = 0;
                this.vmQstnCategory = this.vmEQuestionType.QuestionCategory;
                this.IsCalcavailable = this.vmEQuestionType.IsCalcavailable;
                this.IsValavailable = this.vmEQuestionType.IsValavailable;
                this.lstOptions = [];
                QItem.lstQuestionOptions = [{ OptionValue: '' }];
                if (QItem.QuestionType == 'Cascading Dropdown') {
                    this.vmECascadingOption = [];
                    QItem.lstCascadingOptions = [{ OptionValue: '' }];

                }
            },

            /// <summary>
            ///  CancelEditQuestion :- Function used to cancel question in edit mode
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            CancelEditQuestion(QItem) {
                this.vmEQuestionName = "";
                this.vmEQuestionType = [{ ID: 0, Type: "" }];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.preCasOption = 0;
                this.lstSections.filter(x => x.SectionID == QItem.SectionID)[0].SurveySecQuestions.filter(x => (x.QuestionID == QItem.QuestionID))[0].IsEditQstn = false;

                for (var i = 0; i < this.lstSections.length; i++) {
                    this.lstSections[i].UpdateQstnSection = false;
                    this.lstSections[i].questionPanel = true;
                }
                this.GetAllSections();

            },

            /// <summary>
            ///  fnCheckMandatoryQuestion :- Function used to check mandatory question
            /// </summary>
            /// <param name="NewQstn"></param>
            /// <param name="QstnType"></param>
            /// <param name="IsEdit"></param>
            /// <param name="QItem"></param>
            /// <returns> true/false </returns>
            fnCheckMandatoryQuestion(NewQstn, QstnType, IsEdit, QItem) {
                var IsNotValid = false;
                var OptionCount = 0;
                if (typeof QstnType != "undefined") {

                    if (!IsEdit) {
                        if (QstnType == 'Cascading Dropdown' || QstnType == 'Dropdown' || QstnType == 'Multi-Choice' || QstnType == 'Single Choice') {

                            //if (this.lstOptions.filter(x => x.OptionValue != null).length == 0 || this.lstOptions.filter(x => x.OptionValue != []).length == 0) {

                            //    IsNotValid = true;
                            //} else { IsNotValid = false;}
                            OptionCount = this.lstOptions.length - 1;
                            if (this.lstOptions[OptionCount].OptionValue == null || this.lstOptions[OptionCount].OptionValue == "") {
                                IsNotValid = true;
                            } else {
                                IsNotValid = false;
                            }

                        }
                        if (QstnType == 'Likert Scale') {
                            if (this.Likerts.filter(x => x.OptionValue != null).length == 0 || this.Likerts.filter(x => x.OptionValue != []).length == 0) {
                                IsNotValid = true;
                            } else {
                                IsNotValid = false;
                            }
                        }
                    }
                    else {
                        if (!(QstnType == 'Cascading Dropdown' || QstnType == 'Dropdown' || QstnType == 'Multi-Choice' || QstnType == 'Single Choice')) {

                            if (this.preQstn == NewQstn && this.preAnsType == QstnType) {

                                if (QstnType == "Numeric") {

                                    if (this.vmEUOM.length > 0) {
                                        IsNotValid = false;
                                    } else {
                                        IsNotValid = true;
                                    }
                                } else {
                                    IsNotValid = true;
                                }

                            }
                        }

                        if (QstnType == 'Dropdown' || QstnType == 'Multi-Choice' || QstnType == 'Single Choice') {

                            //if (QItem.lstQuestionOptions.filter(x => x.OptionValue != null).length == 0 || QItem.lstQuestionOptions.filter(x => x.OptionValue != []).length == 0) {
                            //    IsNotValid = true;
                            //} else {IsNotValid = false;}

                            OptionCount = QItem.lstQuestionOptions.length - 1;
                            if (QItem.lstQuestionOptions[OptionCount].OptionValue == null || QItem.lstQuestionOptions[OptionCount].OptionValue == "") {
                                IsNotValid = true;
                            } else { IsNotValid = false; }


                        }
                        if (QstnType == 'Cascading Dropdown') {
                            if (this.preAnsType != QstnType) {
                                if (this.lstOptions.filter(x => x.OptionValue != null).length == 0 || this.lstOptions.filter(x => x.OptionValue != []).length == 0)
                                    IsNotValid = true;
                            }
                            else if (this.preQstn == NewQstn && this.preCasOption == 0)
                                IsNotValid = true;
                        }
                        if (QstnType == 'Likert Scale') {

                            if (QItem.lstQuestionOptions.filter(x => x.OptionValue != null).length == 0 || QItem.lstQuestionOptions.filter(x => x.OptionValue != []).length == 0) {
                                IsNotValid = true;
                            } else {
                                IsNotValid = false;
                            }
                        }
                        if (QstnType == 'GPS Capture' || QstnType == 'Audio Capture' || QstnType == 'Date Picker' || QstnType == 'Numeric' || QstnType == 'File Upload' || QstnType == 'Multi Line Text' || QstnType == 'Note' || QstnType == 'Photo Capture' || QstnType == 'Signature Capture' || QstnType == 'Single Line Text') {

                            IsNotValid = false;
                        }

                    }
                    if (NewQstn == '') {
                        IsNotValid = true
                    }
                    return IsNotValid;
                }
            },

            /// <summary>
            ///  UpdateQuestion :- Function used to update question in edit mode
            /// </summary>
            /// <param name="QID"></param>
            /// <param name="QName"></param>
            /// <param name="QType"></param>
            /// <param name="QItem"></param>
            /// <param name="QUOM"></param>
            /// <param name="SurveyID"></param>
            /// <param name="SectionID"></param>
            /// <returns> </returns>
            async UpdateQuestion(QID, QName, QType, QItem, QUOM, SurveyID, SectionID) {
                this.PreLoader = true;

                if ((QType.Type == 'Dropdown' || QType.Type == 'Multi-Choice' || QType.Type == 'Single Choice' || QType.Type == 'Likert Scale') && (this.lstOptions.length == 0))
                    this.lstOptions = QItem.lstQuestionOptions;
                else if ((QType.Type == 'Cascading Dropdown') && (this.lstOptions.length == 0))
                    this.lstOptions = QItem.lstCascadingOptions;

                var lstQOptions = this.lstOptions.filter(x => x.OptionValue != "")
                if ((QType.Type == 'Dropdown' || QType.Type == 'Multi-Choice' || QType.Type == 'Single Choice' || QType.Type == 'Cascading Dropdown' || QType.Type == 'Likert Scale') && (lstQOptions.length == 0)) {
                    this.Msg = "Please Add/Select  Options"
                    this.infoSnackbarMsg = true;
                    this.PreLoader = false;
                    return;
                }
                if (QType.Type == 'Likert Scale') {

                    var OptionCount = this.lstOptions.length - 1;
                    if (this.lstOptions[OptionCount].OptionValue == null) {

                        this.Msg = "Please Add/Select  Options"
                        this.infoSnackbarMsg = true;
                        this.PreLoader = false;
                        //QItem.lstQuestionOptions
                        this.lstOptions.splice(this.lstOptions.indexOf(this.lstOptions[OptionCount]), 1);
                        QItem.lstQuestionOptions.splice(QItem.lstQuestionOptions.indexOf(QItem.lstQuestionOptions[OptionCount]), 1);
                        return;
                    }
                }

                var data = new FormData();
                data.append("QuestionID", QID);
                data.append("QuestionName", QName);
                data.append("QuestionTypeID", QType.ID);
                data.append("QuestionOptionList", JSON.stringify(lstQOptions));
                data.append("UOM", QUOM);
                data.append("SurveyID", SurveyID);
                data.append("SectionID", SectionID);
                data.append("IsOtherOptionSelected", this.IsOtherOptionSelected);
                var result = await objSurvey.UpdateQuestion(data);

                await this.GetAllSections();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });

                }

                else if (result.status == "success") {
                    this.Msg = "Question details Updated successfully ...";
                    this.successSnackbarMsg = true;
                }

                else {
                    this.Msg = "Duplicate QuestionName/Updating Question details failed ...";
                    this.errorSnackbarMsg = true;
                }
                this.lstOptions = [];
                this.SingleChoices = [{ OptionValue: "" }];
                this.vmEQuestionType = [{}];
                this.vmCascadingOption = [];
                this.vmECascadingOption = [];
                this.preCasOption = 0;
                this.Likerts = [
                    { OptionValue: 'Bad' }, { OptionValue: 'Average' }, { OptionValue: 'Good' }, { OptionValue: 'Very good' }, { OptionValue: 'Excellent' }
                ];
                this.PreLoader = false;
            },

            /// <summary>
            ///  QuestionCategory :- Function used to cancel question in edit mode
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            QuestionCategory() {
                this.IsOtherOptionSelected = false;
                this.vmQstnCategory = this.vmQuestionType.QuestionCategory;
                this.IsCalcavailable = this.vmQuestionType.IsCalcavailable;
                this.IsValavailable = this.vmQuestionType.IsValavailable;
            },

            /// <summary>
            ///  UpdateQstnOrder :- Function used to update question order
            /// </summary>
            /// <param name="QItem"></param>
            /// <returns> </returns>
            async UpdateQstnOrder(QItem) {
                var lstQstnOrder = []
                for (var i = 0; i < QItem.length; i++)
                    lstQstnOrder.push({ QuestionID: QItem[i].QuestionID, QuestionOrder: (i + 1) })
                var data = new FormData();
                data.append("QstnList", JSON.stringify(lstQstnOrder));
                var result = await objSurvey.UpdateQuestionOrder(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else
                    if (result.status == "success") {
                        this.Msg = "Question Order Changed";
                        this.successSnackbarMsg = true;
                    }

            },
            //==============Questions Responses====================//

            /// <summary>
            ///  GetAllResponsesBySurveyID :- Function used to fetch all responses by survey ID
			///  Currently this functions is not using but we can use this function for reference
            ///  This Function Fetch Both Summary and Individual, at Same time, now we splited this functionality in to two functions				  
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllResponsesBySurveyID() {
                if (this.ActiveTab == 'tab-2') {
                    this.PreLoader = true;
                    this.fileUrl = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectID.toString() + "/";
                    this.lstQstnResponses = [];
                    var data = new FormData();
                    data.append("SurveyID", this.SurveyID);
                    data.append("offset", this.offset);
                    var result = await objSurvey.GetAllResponsesBySurveyID(data);
                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else {
                        if (result.SummaryResponse) {
                            this.lstQstnResponses.push(result.SummaryResponse);
                            this.offset += 5;

                            var labelsArrays = [];
                            var dataArray = [];
                            var bgColor = [];
                            var MultichoiceResponse = 0;
                            for (var i = 0; i < result.SummaryResponse.length; i++) {

                                var lstAnswerList = result.SummaryResponse[i].AnswerList;
                                if (result.SummaryResponse[i].QuestionType != "Numeric") {

                                    var lstOptionList = result.SummaryResponse[i].QuestionOptionList
                                    // var lstAnswerList = result.SummaryResponse[i].AnswerList;
                                    for (var j = 0; j < lstOptionList.length; j++) {
                                        this.generator()
                                        labelsArrays.push(lstOptionList[j].OptionValue ? lstOptionList[j].OptionValue : '');
                                        dataArray.push(0);
                                        bgColor.push(this.mycolor)
                                    }
                                    for (var k = 0; k < lstAnswerList.length; k++) {
                                        var index = labelsArrays.indexOf(lstAnswerList[k].Answer)
                                        if (index >= 0) {
                                            dataArray[index] = lstAnswerList[k].count ? lstAnswerList[k].count : 0
                                        }
                                        MultichoiceResponse = MultichoiceResponse + lstAnswerList[k].count
                                    }
                                    this.responsesBarData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            barThickness: 35,
                                            data: dataArray,
                                            backgroundColor: bgColor
                                        }]
                                    }
                                    result.SummaryResponse[i].ChartData = this.responsesBarData;
                                    result.SummaryResponse[i].MultichoiceResponse = MultichoiceResponse;
                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesBarData = [];
                                    MultichoiceResponse = 0;
                                }
                                else if (result.SummaryResponse[i].QuestionType == "Numeric") {
                                    if (result.SummaryResponse[i].QuestionResponses.length > 0) {
                                        var temp = result.SummaryResponse[i].QuestionResponses.filter(x => x.Answer != null && x.Answer != '');
                                        for (var k = 0; k < temp.length; k++) {
                                            this.generator()
                                            var labelText = "Response " + (k + 1).toString();
                                            labelsArrays.push(labelText);
                                            dataArray.push(temp[k].Answer ? temp[k].Answer : 0);

                                        }
                                    }
                                    this.responsesLineData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            data: dataArray,
                                            backgroundColor: 'transparent',
                                            borderColor: '#F1C100'
                                        }]
                                    }
                                    result.SummaryResponse[i].ChartData = this.responsesLineData;
                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesLineData = [];
                                }
                            }

                            this.questionDetailsForResponse = [];
                            this.questionDetailsForResponseMainData = [];
                            if (this.lstSurveySections.length > 0) {
                                for (var SecI = 0; SecI <= this.lstSurveySections.length - 1; SecI++) {
                                    var Response = [];
                                    if (this.lstQstnResponses[0].filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID).length > 0) {
                                        Response.push(this.lstQstnResponses[0].filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID))
                                    }

                                    this.questionDetailsForResponse.push({
                                        sectionArrow: '',
                                        sectionId: this.lstSurveySections[SecI].SectionID,
                                        sectionName: this.lstSurveySections[SecI].SectionName,
                                        SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                        lstQstnResponses: Response,
                                    })
                                    this.questionDetailsForResponseMainData.push({
                                        sectionArrow: '',
                                        sectionId: this.lstSurveySections[SecI].SectionID,
                                        sectionName: this.lstSurveySections[SecI].SectionName,
                                        SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                        lstQstnResponses: Response,
                                    })
                                    // just pushing to another array .

                                    this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[SecI].SectionID })
                                }
                                // this.questionDetailsForResponseMainData = this.questionDetailsForResponse;
                            }

                        }
                        if (result.IndResponses) {

                            this.lstQstnResponsesInd = result.IndResponses;
                            this.lstIndResponse = this.lstQstnResponsesInd;
                            this.arrowsUpArrayIndividual = [];
                            //adding section details
                            this.QstDtlsForIndividualResponse = [];
                            if (this.lstSurveySections.length > 0) {
                                for (var SecI = 0; SecI <= this.lstSurveySections.length - 1; SecI++) {
                                    var Response = [];
                                    if (this.lstQstnResponsesInd.filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID).length > 0) {
                                        Response.push(this.lstQstnResponsesInd.filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID))
                                    }
                                    this.QstDtlsForIndividualResponse.push({
                                        sectionArrow: '',
                                        sectionId: this.lstSurveySections[SecI].SectionID,
                                        sectionName: this.lstSurveySections[SecI].SectionName,
                                        SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                        lstQstnResponsesInd: Response,
                                    });

                                    if (Response.length > 0) {
                                        this.PageCount = Response[0][0].QuestionResponses.length;
                                    }
                                    this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[SecI].SectionID })
                                }
                            }
                        }
                    }
                    this.PreLoader = false;
                }
            },

            /// <summary>
            ///  GetSummaryResponseByBeneficiaryID :- Function used to fetch summary response by Beneficiary ID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetSummaryResponseByBeneficiaryID() {
                this.PreLoader = true;
                this.questionDetailsForResponse = [];
                //await this.GetAllResponsesBySurveyID();
				await this.GetAllSummaryResonseBySurveyID();	
                //this.questionDetailsForResponse = this.questionDetailsForResponseMainData;
                var labelsArrays = [];
                var dataArray = [];
                var bgColor = [];
                var MultichoiceResponse = 0;
                if (this.questionDetailsForResponse.length > 0) {
                    for (var i = 0; i <= this.questionDetailsForResponse.length - 1; i++) {
                        var QstnResponsesStep1 = this.questionDetailsForResponse[i].lstQstnResponses[0];
                        for (var j = 0; j <= QstnResponsesStep1.length - 1; j++) {
                            var QstnResponsesStep2 = QstnResponsesStep1[j].QuestionResponses;
							if (this.BeneficiaryID > 0) {	 
                                var temp = QstnResponsesStep2.filter(x => x.ResponseID == this.BeneficiaryID);
                                if (temp.length > 0) {
                                    this.questionDetailsForResponse[i].lstQstnResponses[0][j].QuestionResponses = temp;
                                } else {
                                    this.questionDetailsForResponse[i].lstQstnResponses[0][j].QuestionResponses = [];
                                }
							}
                        }

                        // -- to check response of a question is exist or not
                        var IsResponseExist = this.questionDetailsForResponse[i].lstQstnResponses[0].filter(x => x.QuestionResponses.length > 0);
                        // -- chart section  i = section Count
                        if (IsResponseExist.length > 0) {
                            var SummaryResponse = this.questionDetailsForResponse[i].lstQstnResponses[0];
                            for (var lnt = 0; lnt < SummaryResponse.length; lnt++) {

                                var lstAnswerList = SummaryResponse[lnt].AnswerList.filter(x => x.ResponseID == SummaryResponse[lnt].QuestionResponses[0].ResponseID);//result.SummaryResponse[i].AnswerList;

                                if (SummaryResponse[lnt].QuestionType != "Numeric") {

                                    var lstOptionList = SummaryResponse[lnt].QuestionOptionList
                                    // var lstAnswerList = result.SummaryResponse[i].AnswerList;
                                    for (var j = 0; j < lstOptionList.length; j++) {
                                        this.generator()
                                        labelsArrays.push(lstOptionList[j].OptionValue ? lstOptionList[j].OptionValue : '');
                                        dataArray.push(0);
                                        bgColor.push(this.mycolor)
                                    }
                                    for (var k = 0; k < lstAnswerList.length; k++) {
                                        var index = labelsArrays.indexOf(lstAnswerList[k].Answer)
                                        if (index >= 0) {
                                            dataArray[index] = lstAnswerList[k].count ? lstAnswerList[k].count : 0
                                        }
                                        MultichoiceResponse = MultichoiceResponse + lstAnswerList[k].count
                                    }
                                    this.responsesBarData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            barThickness: 35,
                                            data: dataArray,
                                            backgroundColor: bgColor
                                        }]
                                    }
                                    SummaryResponse[lnt].ChartData = this.responsesBarData;
                                    SummaryResponse[lnt].MultichoiceResponse = MultichoiceResponse;
                                    this.questionDetailsForResponse[i].lstQstnResponses[0].filter(x => x.QuestionID == SummaryResponse[lnt].QuestionID)[0].ChartData = this.responsesBarData; //[j].ChartData = this.responsesBarData;
                                    this.questionDetailsForResponse[i].lstQstnResponses[0].filter(x => x.QuestionID == SummaryResponse[lnt].QuestionID)[0].MultichoiceResponse = this.MultichoiceResponse;

                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesBarData = [];
                                    MultichoiceResponse = 0;
                                }
                                else if (SummaryResponse[lnt].QuestionType == "Numeric") {
                                    if (SummaryResponse[lnt].QuestionResponses.length > 0) {
                                        var temp = SummaryResponse[lnt].QuestionResponses.filter(x => x.Answer != null && x.Answer != '');
                                        for (var k = 0; k < temp.length; k++) {
                                            this.generator()
                                            var labelText = "Response " + (k + 1).toString();
                                            labelsArrays.push(labelText);
                                            dataArray.push(temp[k].Answer ? temp[k].Answer : 0);

                                        }
                                    }
                                    this.responsesLineData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            data: dataArray,
                                            backgroundColor: 'transparent',
                                            borderColor: '#F1C100'
                                        }]
                                    }
                                    SummaryResponse[lnt].ChartData = this.responsesLineData;
                                    this.questionDetailsForResponse[i].lstQstnResponses[0].filter(x => x.QuestionID == SummaryResponse[lnt].QuestionID)[0].ChartData = this.responsesLineData;
                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesLineData = [];
                                }
                            }
                        }
                        else {
                            this.questionDetailsForResponse[i].lstQstnResponses[0] = [];
                        }
                    }

                }
                this.PreLoader = false;
            },

            /// <summary>
            ///  GetIndividualRespByBeneficiaryID :- Function used to fetch Individual response by Beneficiary ID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetIndividualRespByBeneficiaryID() {
                this.PreLoader = true;
                //before beneficiary filter . resetting the array with all data
                await this.fnInitializeIndividualResponse()
				this.PageCount = 0;								   
                if (this.QstDtlsForIndividualResponse.length > 0) {
                    for (var i = 0; i <= this.QstDtlsForIndividualResponse.length - 1; i++) {
                        var QstnResponsesStep1 = this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0];
                        if (typeof QstnResponsesStep1 != "undefined") {
							for (var Qcnt = 0; Qcnt <= QstnResponsesStep1.length - 1; Qcnt++) {
                                if (this.BeneficiaryID > 0) {
                                    var temp = QstnResponsesStep1[Qcnt].QuestionResponses.filter(x => x.ResponseID == this.BeneficiaryID);
									this.Pageindex = 1;
                                    if (this.PageCount < temp.length) {
                                        this.PageCount = temp.length;
                                    }		  
                                    if (temp.length > 0) {
                                        this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0][Qcnt].QuestionResponses = temp;
                                    } else {
                                        this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0] = [];
                                    }
								}
								else {
                                    this.PageCount = this.PageCountTemp;
                                }
                            }
                        }
                    }
				}
                this.PreLoader = false;
            },

            /// <summary>
            ///  fnInitializeIndividualResponse :- Function used to initialize Individual response
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeIndividualResponse() {
                this.QstDtlsForIndividualResponse = [];
				this.lstQstnResponsesInd = [];
                await this.GetAllIndividualResponseBySurveyID();
            },
			async GetAllIndividualResponseBySurveyID() {
                var data = new FormData();
                data.append("SurveyID", this.SurveyID);
                data.append("offset", this.offset);
                var result = await objSurvey.GetAllIndividualResponseBySurveyID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    this.lstQstnResponsesInd = result.IndResponses;
                    this.arrowsUpArrayIndividual = [];
                    //adding section details
                    this.QstDtlsForIndividualResponse = [];
                    if (this.lstSurveySections.length > 0) {
                        for (var SecI = 0; SecI <= this.lstSurveySections.length - 1; SecI++) {
                            var Response = [];
                            if (this.lstQstnResponsesInd.filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID).length > 0) {
                                Response.push(this.lstQstnResponsesInd.filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID))
							}
                            this.QstDtlsForIndividualResponse.push({
                                sectionArrow: '',
                                sectionId: this.lstSurveySections[SecI].SectionID,
                                sectionName: this.lstSurveySections[SecI].SectionName,
                                SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                lstQstnResponsesInd: Response,
                            });

							if (Response.length > 0) {
                                this.Pageindex = 1;
                                this.PageCountTemp = this.RespCount;
                                //if (this.PageCountTemp < Response[0][0].QuestionResponses.length) {
                                //    this.PageCountTemp = Response[0][0].QuestionResponses.length;
                                //}
                            }
                            this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[SecI].SectionID })
                        }
                    }
				}
			},
                    
			// To Get all Summary responses
            async GetAllSummaryResonseBySurveyID() {

                if (this.ActiveTab == 'tab-2') {
                    this.PreLoader = true;
                    this.lstQstnResponses = [];
                    var data = new FormData();
                    data.append("SurveyID", this.SurveyID);
                    data.append("offset", this.offset);
                    var result = await objSurvey.GetAllSummaryResponseBySurveyID(data);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else {

                        if (result.SummaryResponse) {
                            this.lstQstnResponses.push(result.SummaryResponse);
                            this.offset += 5;

                            var labelsArrays = [];
                            var dataArray = [];
                            var bgColor = [];
                            var MultichoiceResponse = 0;
                            for (var i = 0; i < result.SummaryResponse.length; i++) {

                                var lstAnswerList = result.SummaryResponse[i].AnswerList;
                                if (result.SummaryResponse[i].QuestionType != "Numeric") {

                                    var lstOptionList = result.SummaryResponse[i].QuestionOptionList
                                    // var lstAnswerList = result.SummaryResponse[i].AnswerList;
                                    for (var j = 0; j < lstOptionList.length; j++) {
                                        this.generator()
                                        labelsArrays.push(lstOptionList[j].OptionValue ? lstOptionList[j].OptionValue : '');
                                        dataArray.push(0);
                                        bgColor.push(this.mycolor)
                                    }
                                    for (var k = 0; k < lstAnswerList.length; k++) {
                                        var index = labelsArrays.indexOf(lstAnswerList[k].Answer)
                                        if (index >= 0) {
                                            dataArray[index] = lstAnswerList[k].count ? lstAnswerList[k].count : 0
                                        }
                                        MultichoiceResponse = MultichoiceResponse + lstAnswerList[k].count
                                    }
                                    this.responsesBarData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            barThickness: 35,
                                            data: dataArray,
                                            backgroundColor: bgColor
                                        }]
                                    }
                                    result.SummaryResponse[i].ChartData = this.responsesBarData;
                                    result.SummaryResponse[i].MultichoiceResponse = MultichoiceResponse;
                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesBarData = [];
                                    MultichoiceResponse = 0;
                                }
                                else if (result.SummaryResponse[i].QuestionType == "Numeric") {
                                    if (result.SummaryResponse[i].QuestionResponses.length > 0) {
                                        var temp = result.SummaryResponse[i].QuestionResponses.filter(x => x.Answer != null && x.Answer != '');
                                        for (var k = 0; k < temp.length; k++) {
                                            this.generator()
                                            var labelText = "Response " + (k + 1).toString();
                                            labelsArrays.push(labelText);
                                            dataArray.push(temp[k].Answer ? temp[k].Answer : 0);

                                        }
                                    }
                                    this.responsesLineData = {
                                        labels: labelsArrays,
                                        datasets: [{
                                            label: 'Options',
                                            data: dataArray,
                                            backgroundColor: 'transparent',
                                            borderColor: '#F1C100'
                                        }]
                                    }
                                    result.SummaryResponse[i].ChartData = this.responsesLineData;
                                    labelsArrays = [];
                                    dataArray = [];
                                    this.responsesLineData = [];
                                }
                            }

                            this.questionDetailsForResponse = [];
                            this.questionDetailsForResponseMainData = [];
                            if (this.lstSurveySections.length > 0) {
                                for (var SecI = 0; SecI <= this.lstSurveySections.length - 1; SecI++) {
                                    var Response = [];
                                    if (this.lstQstnResponses[0].filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID).length > 0) {
                                        Response.push(this.lstQstnResponses[0].filter(x => x.SectionID == this.lstSurveySections[SecI].SectionID))
                                    }
                                    this.questionDetailsForResponse.push({
                                        sectionArrow: '',
                                        sectionId: this.lstSurveySections[SecI].SectionID,
                                        sectionName: this.lstSurveySections[SecI].SectionName,
                                        SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                        lstQstnResponses: Response,
                                    })
                        
									this.questionDetailsForResponseMainData.push({
                                        sectionArrow: '',
                                        sectionId: this.lstSurveySections[SecI].SectionID,
                                        sectionName: this.lstSurveySections[SecI].SectionName,
                                        SectionDescription: this.lstSurveySections[SecI].SectionDescription,
                                        lstQstnResponses: Response,
                                    })
                                    // just pushing to another array .

						            this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[SecI].SectionID })
                                }
                                // this.questionDetailsForResponseMainData = this.questionDetailsForResponse;
                            }
                        }
                    }
				}
			   this.PreLoader = false;	  
            },

            /// <summary>
            ///  GetResponseByBeneficiaryID :- Function used to fetch response by Beneficiary ID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetResponseByBeneficiaryID() {
                if (this.ResponseType == "Summary") {
                    await this.GetSummaryResponseByBeneficiaryID();
                }
                else if (this.ResponseType == "Individual") {
                    await this.GetIndividualRespByBeneficiaryID();
                }
                else if (this.ResponseType == "Pivot Table") {
                    try {
                        this.PreLoader = true;
                        this.getpivottable();
                        //var response = await objSurvey.GetPivotTableData(this.SurveyID);// this.SurveyID;
                        ////debugger;
                        ////console.log(Object.keys(response[0]));
                        //if (response != null && response != undefined && response.length > 0) {
                        //    this.attributes = Object.keys(response[0]);
                        //    //this.rows = Object.keys(response[0]);
                        //    //this.cols = Object.keys(response[0]);
                        //    this.pivotColumns = Object.keys(response[0]);

                        //    this.pivotjsondata = response;
                        //}
                    }
                    catch (err) {
                        this.PreLoader = false;
                        alert("Error while generating pivot table data");
                    }
                    //const myArrayFromLocalStorage = localStorage.getItem('preset')
                    //if (myArrayFromLocalStorage && myArrayFromLocalStorage.length) {
                    //    this.presetLocalStorage = JSON.parse(myArrayFromLocalStorage)
                    //}
                    this.PreLoader = false;
                }
            },

            /// <summary>
            ///  generator :- Function used to select random colors for graphical representation in response page
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            generator: function () {
                this.mycolor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
            },

            /// <summary>
            ///  GetSearchResponse :- Function used to get search response (Summary/Individual)
            /// </summary>
            /// <param name="event"></param>
            /// <returns> </returns>
            async GetSearchResponse(event) {
                var keycode = event.code;
                if (keycode == "Enter") {
                    if (this.ResponseType == "Summary") {
                        await this.SearchSummaryResponse();
                    }
                    else if (this.ResponseType == "Individual") {
                        await this.SearchIndividualResponse();
                    }
                }
            },

            /// <summary>
            ///  SearchSummaryResponse :- Function used to search Summary response
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async SearchSummaryResponse() {
                await this.GetResponseByBeneficiaryID()
                this.arrowsUpArrayIndividual = [];
                if (this.questionDetailsForResponse.length > 0) {
                    for (var SectionCnt = 0; SectionCnt <= this.questionDetailsForResponse.length - 1; SectionCnt++) {
                        // Fetching Respposnse (Questions response) under a section
                        var lstQuestionResponse = this.questionDetailsForResponse[SectionCnt].lstQstnResponses[0];
                        if (lstQuestionResponse.length > 0) {
                            var lstsummResponse = lstQuestionResponse.filter(x => (x.QuestionName.toLowerCase().includes(this.vmSearchResponse.toLowerCase())));
                            if (lstsummResponse.length > 0) {
                                this.questionDetailsForResponse[SectionCnt].lstQstnResponses[0] = lstsummResponse;
                                //  this.arrowsUpArrayIndividual[SectionCnt].sectionArrow == 1

                            }
                            else {
                                this.questionDetailsForResponse[SectionCnt].lstQstnResponses[0] = [];
                                //this.questionDetailsForResponse[SectionCnt] = [];
                            }
                            // await this.fnSectionResponseCount(this.questionDetailsForResponse[SectionCnt].lstQstnResponses[0]);
                            this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[SectionCnt].SectionID })
                        }
                    }
                }
            },

            /// <summary>
            ///  SearchIndividualResponse :- Function used to search Individual response
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async SearchIndividualResponse() {
                this.arrowsUpArrayIndividual = [];
                await this.GetIndividualRespByBeneficiaryID();
                if (this.QstDtlsForIndividualResponse.length > 0) {
                    for (var i = 0; i <= this.QstDtlsForIndividualResponse.length - 1; i++) {
                        var QstnResponsesStep1 = this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0];
                        var temp = QstnResponsesStep1.filter(x => (x.QuestionName.toLowerCase().includes(this.vmSearchResponse.toLowerCase())));
                        if (temp.length > 0) {
                            this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0] = temp;
                        } else {
                            this.QstDtlsForIndividualResponse[i].lstQstnResponsesInd[0] = [];
                        }
                        //  }
                        this.arrowsUpArrayIndividual.push({ sectionArrow: '', sectionId: this.lstSurveySections[i].SectionID })
                    }
                }
            },

            /// <summary>
            ///  fnCommaseperatedMultichoice :- Function used to show multi choice response based on comma seperated string
            /// </summary>
            /// <param name="OptionList"></param>
            /// <param name="CommaSeperatedString"></param>
            /// <returns> OutOptionList </returns>
            fnCommaseperatedMultichoice(OptionList, CommaSeperatedString) {

                var CommaSepoptionList = CommaSeperatedString.split(",");
                var QuestionOptionList = OptionList;
                var OutOptionList = [];
                if (typeof OptionList != "undefined") {
                    for (var Cnt = 0; Cnt <= QuestionOptionList.length - 1; Cnt++) {
                        var object = new Object();
                        object.OptionValue = QuestionOptionList[Cnt].OptionValue;
                        object.OptionID = QuestionOptionList[Cnt].OptionID;
                        object.IsChecked = false;
                        OutOptionList.push(object);
                    }

                    for (var i = 0; i <= CommaSepoptionList.length - 1; i++) {
                        var OptionItem = OutOptionList.filter(x => x.OptionValue == CommaSepoptionList[i]);
                        if (OptionItem.length > 0) {
                            OptionItem[0].IsChecked = true;
                        }
                    }
                }
                return OutOptionList;
            },

            /// <summary>
            ///  GetAllBeneficiaries :- Function used to fetch all beneficiary details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async GetAllBeneficiaries() {
                var data = new FormData();
                data.append("BeneficiaryTypeID", this.BeneficiaryTypeID);
                var result = await objPB.GetAllBeneficiaries(this.BeneficiaryTypeID); //await objPB.GetAllBeneficiaries(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else if (result.data) {
                    this.lstAllBeneficiaries = result.data;
                    if (this.lstAllBeneficiaries.length > 0) {
                        // this.BeneficiaryID = this.lstAllBeneficiaries[0].BeneficiaryID;
                        // await this.GetResponseByBeneficiaryID();
                    }

                }
            },
            //scroll() {
            //    window.onscroll = () => {
            //        let bottomOfWindow = Math.ceil(document.documentElement.scrollTop + window.innerHeight) === document.documentElement.offsetHeight;
            //        if (bottomOfWindow) {
            //            if (this.ResponseType == 'Summary') {
            //                this.GetAllResponsesBySurveyID()
            //            }

            //        }
            //    };
            //},

            /// <summary>
            ///  fnResponseCount :- Function used to fetch Summary response count of question
            /// </summary>
            /// <param name="lstReponse"></param>
            /// <param name="index"></param>
            /// <returns> responseCount </returns>
            fnResponseCount(lstReponse, index) {
                var responseCount = 0;
                if (lstReponse.length > 0) {

                    var temp = lstReponse.filter(x => x.Answer != null && x.Answer != '');
                    responseCount = temp.length;
                }
                if (this.responseCountSummary.length == this.questionDetailsForResponse.length && index < this.responseCountSummary.length - 1) {
                    this.responseSummaryIndex = true
                }
                else if (this.responseCountSummary.length <= this.questionDetailsForResponse.length && this.responseSummaryIndex == false) {
                    if (this.responseCountSummary[index] == undefined) {
                        this.responseCountSummary.push(responseCount)
                    } else {
                        this.responseCountSummary[index] = parseInt(this.responseCountSummary[index]) + parseInt(responseCount)
                    }
                }
                return responseCount;
            },

            /// <summary>
            ///  fnSectionResponseCount :- Function used to fetch section response count
            /// </summary>
            /// <param name="lstReponse"></param>
            /// <returns> Count </returns>
            fnSectionResponseCount(lstReponse) {
                var Count = 0;
                for (var i = 0; i <= lstReponse.length - 1; i++) {

                    Count += lstReponse[i].QuestionResponses.length;
                }
                return Count;
            },

            /// <summary>
            ///  IsMandatoryChecking :- Function used to avoid blank and Null answers from showing
            /// </summary>
            /// <param name="QItemResponse"></param>
            /// <param name="QItem"></param>
            /// <returns> true/false </returns>
            IsMandatoryChecking(QItemResponse, QItem) {
                if (typeof QItemResponse != "undefined") {
                    var IsValid = true;
                    if ((QItemResponse.QuestionType === 'Single Line Text' || QItemResponse.QuestionType === 'Multi Line Text' || QItemResponse.QuestionType === 'Cascading Dropdown' || QItemResponse.QuestionType === 'Date Picker' || QItemResponse.QuestionType === 'Note' || QItemResponse.QuestionType === 'Dropdown')) {
                        IsValid = true;

                        if (QItem.Answer == null || QItem.Answer == '') {
                            IsValid = false;
                        }
                    } else {
                        IsValid = false;
                    }
                }
                return IsValid;
            },

            /// <summary>
            ///  fnIndividualResCount :- Function used to fetch Individual response count
            /// </summary>
            /// <param name="item"></param>
            /// <returns> Count </returns>
            fnIndividualResCount(item) {
                var Count = 0;
                if (typeof item == "undefined") {
                    Count = 0;
                } else {
                    Count = item.length;
                }
                return Count;
            },
            async DownloadDocument(docName) {
                var downloadFileName = docName;
                var docUrl = this.fileUrl + docName;

                fetch(docUrl).then(function (t) {
                    return t.blob().then((b) => {
                        var a = document.createElement("a");
                        a.href = URL.createObjectURL(b);
                        a.setAttribute("download", downloadFileName);
                        a.click();
                    });
                });
            },
            GotoNewTab(docName) {
                //debugger;
                var docUrl = this.fileUrl + docName;
                //let route = this.$router.resolve({ path: docUrl });
                window.open(docUrl);
            },
            fnDoumentFileType(filename) {

                var FileType = "Default";
                var colFilename = filename.split(".")
                if (colFilename.length > 0) {
                    FileType = colFilename[colFilename.length - 1];
                }
                return FileType.toUpperCase();
            },
            async fnMountPivotTab() {
                if(this.currentResponseSectionType === 2) return
                this.clearDateSelection();
                await this.GetPivotPresetList();
                await this.GetSurveyRepeatSection()
                $("#output").pivotUI([], {});
                await this.getpivottable()
                this.lstRepeatSections = this.fnFilterPresets()
            },
            async GetSurveyRepeatSection(){
                let { data } = await objSurvey.GetSurveyRepeatSection(this.SurveyID)
                this.lstRepeatLogic = data
            },
            fnFilterPresets() {
                if(this.repeatLogic){
                    return this.presetLocalStorage.filter((x) => x.sectionId == this.repeatLogic)
                }
                return this.presetLocalStorage.filter((y) => y.sectionId === 0)
            }
        },

    }
</script>

<style>
    .v-expansion-panel-content__wrap {
        padding: 0 !important;
    }

    .table-responsive {
        display: block;
        width: 100%;
        overflow-x: auto;
    }
</style>
<style>

    .pvtUnused.pvtVertList {
        max-width: 500px;
        display: inline-block;
        overflow: scroll;
    }
    .pivot_table_container {
        overflow-x: scroll;
        border: 1px solid rgb(162, 177, 198);
        border-radius: 5px;
    }
</style>

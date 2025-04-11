   <template>
        <v-container fluid>
            <v-row justify="center">
                <Pre-Loader v-show="PreLoader==true"></Pre-Loader>
                <v-col cols="12" xs="12" sm="12" md="11">
                    <v-card elevation="1">
                        <v-row>
                            <v-col cols="12" xs="12" sm="12" md="12" class="pb-0">
                                <template>
                                    <v-container class="pb-0" fluid>
                                        <v-snackbar v-model="snackbarMsg" top color="primary">
                                            {{Msg}}
                                        </v-snackbar>
                                        <v-row>
                                            <v-col sm="12" cols="12" class="pt-1 pb-0">
                                                <v-card-title class="pb-0" style="color: #2D2F40; font-size: 17px; font-weight: bold">
                                                    Survey Overview
                                                </v-card-title>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                <template>
                                                    <v-container class="py-0" fluid>
                                                        <v-row>
                                                            <v-col cols="12" class=" pb-0">
                                                                <v-row class="pl-4">
                                                                    <v-col sm="5" cols="12">
                                                                        <v-select dense :items="lstproject" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Project" outlined hide-details class="mr-2" v-model="ProjectId" @change="mShowChartRow = false,fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(0),fnGetBeneficiaryData(0),GetSurveyProgress(ProjectId),FetchAllProjectDet(ProjectId),GetAllResponsesBySurveyID(0)">
                                                                        </v-select>
                                                                    </v-col>
                                                                    <!--<v-col cols="6" sm="6">
                                                                        <button type="button"
                                                                                class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V20"
                                                                                style="background-color: rgb(255, 255, 255); border-color: rgb(255, 255, 255);" 
                                                                                @click="mShowChartRow = false,fnProjectDataLeftRight(ProjectId,0)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                        <span>{{Projectmin}} of {{lstproject.length}}</span>
                                                                        <button type="button"
                                                                                class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V21"
                                                                                style="background-color: rgb(255, 255, 255);border-color: rgb(255, 255, 255);" 
                                                                                @click="mShowChartRow = false,fnProjectDataLeftRight(ProjectId,1)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                    </v-col>-->
                                                                </v-row>
                                                            </v-col>
                                                            
                                                            <v-col sm="2" cols="12" class="pt-8 pb-0">
                                                                <v-row>
                                                                    <v-col sm="5" cols="12" class="pt-0" style="text-align: end">
                                                                        <v-avatar>
                                                                            <img :src="survey" alt="John" style="font-size: 60px" />
                                                                        </v-avatar>
                                                                    </v-col>
                                                                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                                                                        <v-col sm="12" class="py-0 pl-0">
                                                                            <span style="font-size: 21px; font-weight: bold">{{lstsurveyTot.length}}</span>
                                                                        </v-col>
                                                                        <v-col sm="12" class="pt-0 pl-0">
                                                                            <span style="font-size: 12px">Total Surveys</span>
                                                                        </v-col>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <v-col sm="3" cols="12" class="pt-8 pb-0">
                                                                <v-row>
                                                                    <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
                                                                        <v-avatar>
                                                                            <img :src="response" alt="John" style="font-size: 60px" />
                                                                        </v-avatar>
                                                                    </v-col>
                                                                    <v-col sm="7" cols="12" class="pt-0 pl-0 ">
                                                                        <v-col sm="12" class="py-0 pl-0">
                                                                            <span style="font-size: 21px; font-weight: bold">{{ TotResponseCount }}</span>
                                                                        </v-col>
                                                                        <v-col sm="12" class="pt-0 pl-0">
                                                                            <span style="font-size: 12px">Total Responses</span>
                                                                        </v-col>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                            <v-col sm="3" cols="12" class="pt-8 pb-0">
                                                                <v-row>
                                                                    <v-col sm="5" cols="12" class="pt-0 pb-0" style="text-align: end">
                                                                        <v-avatar>
                                                                            <img :src="completed" alt="John" style="font-size: 60px" />
                                                                        </v-avatar>
                                                                    </v-col>
                                                                    <v-col sm="7" cols="12" class="pt-0 pl-0 pb-0">
                                                                        <v-col sm="12" class="py-0 pl-0">
                                                                            <span style="font-size: 21px; font-weight: bold">{{ TotCompleteCount
                                                                            }}</span>
                                                                        </v-col>
                                                                        <v-col sm="12" class="pt-0 pl-0">
                                                                            <span style="font-size: 12px">Total Completed</span>
                                                                        </v-col>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </template>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                <template>
                                                    <v-container class="pt-0 pb-0" fluid>
                                                        <v-row>
                                                            <v-col sm="4" cols="12" class="pl-8 pt-0">
                                                                <v-row>
                                                                    <v-col class="pb-0" sm="12" cols="12">
                                                                        <span style="color: #2d2f40; font-size: 17px; font-weight: bold">Individual Survey Analysis</span>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-col>
                                                        </v-row>
                                                        <v-row class="pl-4 pt-0">
                                                            <v-col class="pt-0" sm="2" cols="12">
                                                                <!--<v-select dense label="Start Date" outlined hide-details></v-select>-->

                                                                <v-menu v-model="mStartDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">

                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="computedStartDate"
                                                                                    label="Start Date"
                                                                                    outlined dense
                                                                                    readonly
                                                                                    v-on="on"></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="startDate" no-title @input="mShowChartRow = false,mStartDate = false,IsValidDate(),fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(0),fnGetBeneficiaryData(0),GetAllResponsesBySurveyID(0)" format="DD-MM-YYYY"
                                                                                :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>

                                                            </v-col>
                                                            <v-col class="pt-0" sm="2" cols="12">
                                                                <!--<v-select dense label="End Date" outlined hide-details></v-select>-->
                                                                <v-menu v-model="mEndDate"
                                                                        :close-on-content-click="false"
                                                                        :nudge-right="40"
                                                                        transition="scale-transition"
                                                                        offset-y
                                                                        min-width="290px">
                                                                    <template v-slot:activator="{ on }">
                                                                        <v-text-field v-model="computedEndDate"
                                                                                    label="End Date"
                                                                                    outlined dense
                                                                                    readonly
                                                                                    v-on="on"></v-text-field>
                                                                    </template>
                                                                    <v-date-picker v-model="endDate" no-title @input="mShowChartRow = false,mEndDate = false,IsValidDate(),fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(0),fnGetBeneficiaryData(0),GetAllResponsesBySurveyID(0)"
                                                                                :min="startDate" :max="ProjectEndDate"></v-date-picker>
                                                                </v-menu>
                                                            </v-col>

                                                            <!--<v-col cols="4" class="px-2">
                                                    <h3 style="text-align:center">Start Date</h3>
                                                    <v-date-picker color="primary" v-model="FiterStartDate" no-title @input="IsValidDate()"
                                                                class="elevation-0"></v-date-picker>
                                                </v-col>
                                                <v-col cols="4" class="px-2">
                                                    <h3 style="text-align:center">End Date</h3>
                                                    <v-date-picker color="primary" v-model="FiterEndDate" no-title :min="FiterStartDate" @input="IsValidDate()"
                                                                class="elevation-0"></v-date-picker>
                                                </v-col>-->


                                                            <v-col class="pt-0" sm="4" cols="12">
                                                                <v-select dense :items="lstsurvey" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Survey" outlined hide-details class="mr-2" v-model="SurveyID" @change="mShowChartRow = false,fnGetQuestionData(SurveyID),fnGetBeneficiaryData(SurveyID)">
                                                                </v-select>

                                                                <!--<v-select v-model="select"
                                                            dense
                                                            :items="data_survey_list"
                                                            label="Standard"
                                                            outlined
                                                            hide-details
                                                            item-text="name">
                                                        <template v-slot:selection="{ item, index }">
                                                            <img :src="item.image" />
                                                            <span class="ml-3"
                                                                style="font-size: 14px">{{ item.name }}</span>
                                                        </template>
                                                        <template v-slot:item="prop">
                                                            <img :src="prop.item.image" />
                                                            <span class="ml-3"
                                                                style="font-size: 14px">{{ prop.item.name }}</span>
                                                        </template>
                                                    </v-select>-->
                                                            </v-col>
                                                            <v-col class="pt-0 d-flex justify-space-around" sm="3" cols="12">
                                                                <Secondary-Button title="Generate" @click.native="ShowChartRow(1),fnGetQuestionname(QuestionID),FetchAllSurveyQuestDetail(SurveyID, QuestionID),GetAllResponsesBySurveyID(QuestionID),GetPivotPresetList()"></Secondary-Button>
                                                                <Default-Button-Outlined title="Clear All" @click.native="ShowChartRow(0),fnGetSurveyData(ProjectId,startDate,endDate),fnGetQuestionData(SurveyID),fnGetBeneficiaryData(SurveyID)"></Default-Button-Outlined>
                                                            </v-col>
                                                        </v-row>
                                                    </v-container>
                                                </template>
                                            </v-col>
                                        </v-row>
                                        <v-tabs v-model="tabList" class="pl-4 mb-5" v-if="mShowChartRow" >
                                            <v-tab href="#sec-preset">Preset</v-tab>
                                            <!--<v-tab href="#sec-summary">Summary</v-tab>-->
                                        </v-tabs>

                                        <v-tabs-items v-model="tabList" v-if="mShowChartRow">
                                            <v-tab-item value="sec-summary">
                                                <v-row  v-if="mShowChartRow===true">
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                        <template>
                                                            <v-container fluid>
                                                                <v-row>
                                                                    <v-col sm="5" class="pl-7" cols="12">

                                                                        <v-select dense :items="lstbeneficiary" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Beneficiary" outlined hide-details class="mr-2" v-model="BeneficiaryID" @change="fnGetBeneficiaryPos(BeneficiaryID)">
                                                                        </v-select>

                                                                        <!--<v-select v-model="select"
                                                                    dense
                                                                    :items="data_survey_list"
                                                                    label="Standard"
                                                                    outlined
                                                                    hide-details
                                                                    item-text="name">
                                                                <template v-slot:selection="{ item, index }">
                                                                    <img :src="item.image" />
                                                                    <span class="ml-3"
                                                                        style="font-size: 14px">{{ item.name }}</span>
                                                                </template>
                                                                <template v-slot:item="prop">
                                                                    <img :src="prop.item.image" />
                                                                    <span class="ml-3"
                                                                        style="font-size: 14px">{{ prop.item.name }}</span>
                                                                </template>
                                                            </v-select>-->
                                                                    </v-col>
                                                                    <v-col cols="6" sm="6">
                                                                        <button type="button"
                                                                                class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V20"
                                                                                style="background-color: rgb(255, 255, 255);border-color: rgb(255, 255, 255);" 
                                                                                @click="fnBeneficiaryDataLeftRight(BeneficiaryID,0)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                        <span>{{Beneficiarymin}} of {{lstbeneficiary.length}}</span>
                                                                        <button type="button"
                                                                                class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V21"
                                                                                style="background-color: rgb(255, 255, 255);border-color: rgb(255, 255, 255);" 
                                                                                @click="fnBeneficiaryDataLeftRight(BeneficiaryID,1)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>
                                                                    <v-col sm="5" class="pl-7" cols="12">
                                                                        <v-select dense :items="lstquestion" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Question" outlined hide-details class="mr-2" v-model="QuestionID" @change="fnGetQuestionname(QuestionID),FetchAllSurveyQuestDetail(SurveyID, QuestionID),GetAllResponsesBySurveyID(QuestionID)">
                                                                        </v-select>

                                                                        <!--<v-select v-model="select"
                                                                                        dense
                                                                                        :items="items1"
                                                                                        label="Standard"
                                                                                        outlined
                                                                                        hide-details
                                                                                        item-text="name">
                                                                                    <template v-slot:selection="{ item, index }">
                                                                                        <v-avatar color="red" size="25">
                                                                                            <span class="white--text">C</span>
                                                                                        </v-avatar>
                                                                                        <span class="ml-3" style="font-size: 14px">{{ item.title }}</span>
                                                                                    </template>
                                                                                    <template v-slot:item="prop" class="pl-0">
                                                                                        <v-avatar color="red" size="35">
                                                                                            <span class="white--text">C</span>
                                                                                        </v-avatar>
                                                                                        <span class="ml-3" style="font-size: 14px">
                                                                                            {{
                                                            prop.item.title
                                                                                            }}
                                                                                        </span>
                                                                                    </template>
                                                                                </v-select>-->

                                                                    </v-col>
                                                                    <v-col cols="6" sm="6">
                                                                        <button type="button"
                                                                                class="elevation-1 mr-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V20"
                                                                                style="background-color: rgb(255, 255, 255);border-color: rgb(255, 255, 255);" 
                                                                                @click="fnQuestionDataLeftRight(QuestionID,0),fnGetQuestionname(QuestionID),FetchAllSurveyQuestDetail(SurveyID, QuestionID),GetAllResponsesBySurveyID(QuestionID)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-left theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                        <span>{{Questionmin}} of {{lstquestion.length}}</span>
                                                                        <button type="button"
                                                                                class="elevation-1 ml-2 v-btn v-btn--contained v-btn--fab v-btn--round theme--light v-size--x-small"
                                                                                id="AutoTest_V21"
                                                                                style="background-color: rgb(255, 255, 255);border-color: rgb(255, 255, 255);" 
                                                                                @click="fnQuestionDataLeftRight(QuestionID,1),fnGetQuestionname(QuestionID),FetchAllSurveyQuestDetail(SurveyID, QuestionID),GetAllResponsesBySurveyID(QuestionID)">
                                                                            <span class="v-btn__content">
                                                                                <i aria-hidden="true"
                                                                                class="v-icon notranslate mdi mdi-arrow-right theme--light primary--text"
                                                                                style="font-size: 25px"></i>
                                                                            </span>
                                                                        </button>
                                                                    </v-col>
                                                                </v-row>
                                                                <v-row>

                                                                    <v-col class="pt-7" sm="7" cols="12">

                                                                        <v-row v-if="QuestionTypeBarLine==='Likert Scale'
                                                                            || QuestionTypeBarLine==='Single Choice'
                                                                            || QuestionTypeBarLine==='Multi-Choice'
                                                                            || QuestionTypeBarLine==='Numeric'" align="center" justify="center"
                                                                        >
                                                                            <v-col sm="10" cols="12">
                                                                                <v-row>
                                                                                    <v-col sm="7" cols="12" class="pl-8">
                                                                                        <span style="font-size: 13px; font-weight: bold">{{ QuestionN1 }}</span>
                                                                                    </v-col>
                                                                                    <v-col sm="5" cols="12" class="pt-3">
                                                                                        <v-row>
                                                                                            <v-col sm="12" cols="12" class="pt-0 px-0">
                                                                                                <v-chip color="#2196F3" label dark small> Surveys </v-chip>
                                                                                                <v-chip color="black" label dark small class="ml-2">
                                                                                                    Beneficiaries
                                                                                                </v-chip>
                                                                                            </v-col>
                                                                                        </v-row>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col sm="2" cols="12">
                                                                                <v-row justify="start">
                                                                                    <v-col sm="5" cols="12" class="pt-2 pl-0">
                                                                                        <v-btn elevation="1"
                                                                                            tile
                                                                                            style="border-radius: 3px"
                                                                                            color="#AFAFAF"
                                                                                            icon>
                                                                                            <v-avatar size="18">
                                                                                                <img :src="excel" />
                                                                                            </v-avatar>
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                    <v-col sm="5" cols="12" class="pt-2 pl-0">
                                                                                        <v-btn elevation="1"
                                                                                            style="border-radius: 3px"
                                                                                            color="#EEEEEE"
                                                                                            tile
                                                                                            icon>
                                                                                            <v-avatar size="18">
                                                                                                <img :src="printer" />
                                                                                            </v-avatar>
                                                                                        </v-btn>
                                                                                    </v-col>
                                                                                </v-row>
                                                                            </v-col>
                                                                            <v-col sm="12" cols="12" class="pt-0">
                                                                                <v-row>
                                                                                    <v-col sm="12" cols="12">
                                                                                        <Line-Chart v-if="QuestionTypeBarLine==='Numeric'" :linedata="responsesLineData" :lineOptions="responsesLineOptions" style="width:100%; height:auto"></Line-Chart>
                                                                                        <Bar-Chart v-else :barData="responsesBarData" :barOptions="responsesBarOptions"></Bar-Chart>

                                                                                    </v-col>
                                                                                </v-row>

                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-col>
                                                                    <v-col sm="4" cols="12" class="pt-11 ml-2">
                                                                        <v-card style="height: 480px">
                                                                            <v-row justify-md="center">
                                                                                <v-col sm="11">
                                                                                    <v-tabs grow class="pt-0">
                                                                                        <v-tabs-slider color="teal lighten-3"></v-tabs-slider>
                                                                                        <v-tab href="#tabs-1"> Details </v-tab>
                                                                                        <v-tab href="#tabs-2"> Activity </v-tab>
                                                                                        <v-tab-item value="tabs-1">
                                                                                            <v-card elevation="0"
                                                                                                    class="v-scrolling-div"
                                                                                                    style="overflow-x: hidden; height: 400px">
                                                                                                <v-row justify="center">
                                                                                                    <v-col cols="12" sm="11">
                                                                                                        <v-row>
                                                                                                            <v-col cols="12" sm="12" class="pb-0">
                                                                                                                <span style="font-weight: bold; font-size: 17px">Question</span>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" sm="12" class="pt-0">
                                                                                                                <span style="color: #585858">
                                                                                                                    {{ QuestionN2 }}
                                                                                                                </span>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-row>
                                                                                                            <v-col cols="12" sm="6" class="">
                                                                                                                <v-row>
                                                                                                                    <v-col class="py-0" sm="12">
                                                                                                                        <span style="font-weight: bold; font-size: 17px">Progress</span>
                                                                                                                    </v-col>
                                                                                                                    <v-col sm="8" cols="12" class="pt-2 pr-0 pb-0">
                                                                                                                        <v-progress-linear color="#20C997" rounded
                                                                                                                                        height="9"
                                                                                                                                        value=SurveyProgress></v-progress-linear>
                                                                                                                    </v-col>
                                                                                                                    <v-col sm="4" cols="12" class="py-0 pr-0">
                                                                                                                        <span style="font-size: 14px;color: #000000;font-weight: bold;">{{ SurveyProgress }}%</span>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                            <v-col sm="6" cols="12" class="pl-0">
                                                                                                                <v-col class="py-0 pl-0" sm="12">
                                                                                                                    <span style="font-weight: bold; font-size: 17px">Total Responses</span>
                                                                                                                </v-col>
                                                                                                                <v-col sm="12"
                                                                                                                    cols="12"
                                                                                                                    class="pt-0 pr-0 pb-0 pl-0">
                                                                                                                    <v-col sm="12"
                                                                                                                        cols="12"
                                                                                                                        style="color: #585858"
                                                                                                                        class="pl-0 pt-0 pr-0 pb-0">{{ QuestTotResponse }}</v-col>
                                                                                                                </v-col>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-row>
                                                                                                            <v-col cols="12" sm="12">
                                                                                                                <span style="font-weight: bold; font-size: 17px">Enumerator</span>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" sm="2" class="py-0">
                                                                                                                <v-avatar size="40" :color="`${getcolor(EnumName)}`">
                                                                                                                    <img v-bind:src="userimage + EnumImage" alt="User DP" v-if="EnumImage.length >0" />
                                                                                                                    <span class="white--text" v-else>{{EnumName == null ? "" : EnumName.substring(0, 1).toUpperCase()}}</span>
                                                                                                                </v-avatar>
                                                                                                            </v-col>
                                                                                                            <v-col cols="12" sm="10" class="pb-0 pt-4">
                                                                                                                <span>{{EnumName}}</span>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-row>
                                                                                                            <v-col cols="12" sm="6">
                                                                                                                <v-row>
                                                                                                                    <v-col class="py-0" sm="12">
                                                                                                                        <span style="font-weight: bold; font-size: 17px">Created Date</span>
                                                                                                                    </v-col>
                                                                                                                    <v-col sm="12" cols="12" class="pt-0 pr-0 pb-0">
                                                                                                                        <v-col sm="12"
                                                                                                                            cols="12"
                                                                                                                            style="color: #585858"
                                                                                                                            class="pt-0 pr-0 pb-0 pl-0">{{ QuestCreateDate }}</v-col>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                            <v-col sm="6" cols="12" class="pl-0">
                                                                                                                <v-col class="py-0 pl-0" sm="12">
                                                                                                                    <span style="font-weight: bold; font-size: 17px">Last Updated</span>
                                                                                                                </v-col>
                                                                                                                <v-col sm="12"
                                                                                                                    cols="12"
                                                                                                                    class="pt-0 pr-0 pb-0 pl-0">
                                                                                                                    <v-col sm="12"
                                                                                                                        cols="12"
                                                                                                                        class="pt-0 pr-0 pb-0 pl-0">{{ QuestUpdateDate }}</v-col>
                                                                                                                </v-col>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                        <v-row>
                                                                                                            <v-col cols="12" sm="6">
                                                                                                                <v-row>
                                                                                                                    <v-col class="py-0" sm="12">
                                                                                                                        <span style="font-weight: bold; font-size: 17px">Periodicity</span>
                                                                                                                    </v-col>
                                                                                                                    <v-col sm="12" cols="12" class="pt-0 pr-0 pb-0">
                                                                                                                        <v-col sm="12"
                                                                                                                            cols="12"
                                                                                                                            style="color: #585858"
                                                                                                                            class="pt-0 pr-0 pb-0 pl-0">{{ SurveyPeriodicity }}</v-col>
                                                                                                                    </v-col>
                                                                                                                </v-row>
                                                                                                            </v-col>
                                                                                                            <v-col sm="6" cols="12" class="pl-0">
                                                                                                                <v-col class="py-0 pl-0" sm="12">
                                                                                                                    <span style="font-weight: bold; font-size: 17px">Beneficiary Type</span>
                                                                                                                </v-col>
                                                                                                                <v-col sm="12"
                                                                                                                    cols="12"
                                                                                                                    class="pt-0 pr-0 pb-0 pl-0">
                                                                                                                    <v-col sm="12"
                                                                                                                        cols="12"
                                                                                                                        style="color: #585858"
                                                                                                                        class="pt-0 pr-0 pb-0 pl-0">{{ SurveyBeneficiary }}</v-col>
                                                                                                                </v-col>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-card>
                                                                                        </v-tab-item>
                                                                                        <v-tab-item value="tabs-2">
                                                                                            <v-card elevation="0"
                                                                                                    class="v-scrolling-div"
                                                                                                    style="overflow-x: hidden; height: 400px">
                                                                                                <v-row justify="center">
                                                                                                    <v-col cols="12" sm="11">
                                                                                                        <v-row>
                                                                                                            <v-col sm="12" cols="12">
                                                                                                                <v-list three-line>
                                                                                                                    <template v-for="(item, index) in lstLogFrame">
                                                                                                                        <v-list-item class="py-0">
                                                                                                                            <v-list-item-icon>
                                                                                                                                <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                                                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                                                                </v-avatar>
                                                                                                                            </v-list-item-icon>
                                                                                                                            <v-list-item-content>
                                                                                                                                <v-list-item-title class="py-0 pb-0">
                                                                                                                                    <v-row class="d-lg-inline-flex py-0 pb-0">
                                                                                                                                        <v-col class="px-0 pl-3 py-0">
                                                                                                                                            <h4 class="font-weight-large   py-0 pb-0 px-0">{{item.Name}}</h4>
                                                                                                                                        </v-col>
                                                                                                                                        <v-col class="ml-3 px-0 py-0">
                                                                                                                                            <h4 class="font-weight-regular  py-0 pb-0">{{item.Message}}</h4>
                                                                                                                                        </v-col>
                                                                                                                                    </v-row>
                                                                                                                                </v-list-item-title>
                                                                                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0">{{formatDateAndTime(item.CreatedDate)}}</h4>
                                                                                                                                </v-list-item-subtitle>
                                                                                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                                                                    <h4 class="font-weight-regular py-0 pb-0 px-0">{{item.TaskUpdateComment}}</h4>
                                                                                                                                </v-list-item-subtitle>
                                                                                                                                <v-list-item-subtitle class="py-0 pb-0 mt-1 pt-0">
                                                                                                                                    <v-row>
                                                                                                                                        <v-col md="8">
                                                                                                                                            <v-btn v-show="item.AttachedDocumentName.length >0 " color="red" outlined="" dark="" class="elevation-0" height="35" @click="DownloadDocument(docName=item.AttachedDocumentName,docUName=item.UniqueDocumentName)">
                                                                                                                                                <v-icon size="26" color="red">mdi-file-download-outline</v-icon>{{item.AttachedDocumentName}}
                                                                                                                                            </v-btn>
                                                                                                                                        </v-col>
                                                                                                                                    </v-row>
                                                                                                                                </v-list-item-subtitle>
                                                                                                                            </v-list-item-content>
                                                                                                                        </v-list-item>
                                                                                                                    </template>
                                                                                                                </v-list>
                                                                                                            </v-col>
                                                                                                        </v-row>
                                                                                                    </v-col>
                                                                                                </v-row>
                                                                                            </v-card>
                                                                                        </v-tab-item>
                                                                                    </v-tabs>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-container>
                                                        </template>
                                                    </v-col>
                                                </v-row>
                                            </v-tab-item>
                                            <v-tab-item value="sec-preset">
                                                <!--<h1>Preset</h1>-->
                                                <v-row v-if="mShowChartRow===true">
                                                    <v-col cols="12" xs="12" sm="12" md="12" class="py-0">
                                                        <template>

                                                            <v-card class="preset-card">
                                                                <template v-if="loader" class="mx-auto">
                                                                    <v-progress-linear :size="70"
                                                                                       color="primary"
                                                                                       indeterminate></v-progress-linear>
                                                                </template>
                                                                <div class="d-flex flex-row">
                                                                    <v-card-title>
                                                                        {{loader ? 'Loading...' : presetName}}
                                                                    </v-card-title>
                                                                    <div id="controlx" class="ml-auto mt-4 mr-2" v-if="!loader">
                                                                        <v-btn icon outlined @click="prevPreset" class="mr-1" color="blue" small :disabled="Boolean(!cursor)">
                                                                            <v-icon>mdi-chevron-left</v-icon>
                                                                        </v-btn>
                                                                        <v-btn icon outlined @click="nextPreset" class="mr-1" color="blue" small :disabled="Boolean(!presetOptions.length) || Boolean(presetOptions.length && cursor == presetOptions.length - 1)">
                                                                            <v-icon>mdi-chevron-right</v-icon>
                                                                        </v-btn>
                                                                    </div>
                                                                </div>
                                                                <v-divider /> 
                                                                <div id="output" class="pa-5 v-scrolling-div" style="overflow-x:scroll"></div>
                                                            </v-card>
                                                        </template>
                                                    </v-col>
                                                </v-row>
                                            </v-tab-item>
                                        </v-tabs-items>
                                    </v-container>
                                </template>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-col>
            </v-row>
            <v-snackbar app top v-model="snackStatus.value" :color="snackStatus.color">
                {{snackStatus.message}}
            </v-snackbar>
        </v-container>
    </template>
    <script>
        //import SurveyOverview from "../ReportWidget/SurveyOverview";
        //import OverallSurveyProgress from "../ReportWidget/OverallSurveyProgress";
        //import IndividualSurveyAnalysis from "../ReportWidget/IndividualSurveyAnalysis";
        //import SurveyCard from "../ReportWidget/SurveyCard";
        import SecondaryButton from '@/components/secondary-button.vue'
        import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
        import BarChart from "@/components/BarChart.vue";
        import LineChart from "@/components/LineChart.vue";
        import { dbImworks } from '../../../ImworksDb'
        import PreLoader from '@/components/pre-loader.vue'
        import objUtils from '@/utils.js';

        var objManageProjectSurvey;
        var objDashboard;
        async function importscript() {
            return Promise.all([
                import("../../../BL/ProjectSurvey.js").then((mod) => {
                    objManageProjectSurvey = new mod.ProjectSurvey();
                }),
                import("../../../BL/Dashboard.js").then(mod => {
                    objDashboard = new mod.Dashboard();
                })
            ]);
        }
        export default {
            data() {
                return {
                    skill: 20,
                    knowledge: 33,
                    power: 0,
                    SurveyProgress: 0,
                    lstproject: [],
                    lstsurvey: [],
                    lstsurveyTot: [],
                    EnumName: "",
                    PreLoader:"",
                    EnumImage: "",
                    EnumImagePath: "",
                    mShowChartRow: false,
                    snackbarMsg: false,
                    lstquestion: [],
                    lstbeneficiary: [],
                    TotResponse: 0,
                    TotComplete: 0,
                    TotResponseCount: 0,
                    TotCompleteCount: 0,
                    lstLogFrame: [],
                    AuditLog: [],
                    FiterStartDate: "",
                    FiterEndDate: "",
                    ProjectStartDate: "",
                    ProjectEndDate: "",
                    userimage: "",
                    QuestTotResponse: 0,
                    QuestCreateDate: "",
                    QuestUpdateDate: "",
                    SurveyBeneficiary: "",
                    SurveyPeriodicity: "",
                    QuestionTypeBarLine: "",
                    mStartDate: false,
                    mEndDate: false,
                    endDate: null,
                    startDate: null,
                    Msg: "",
                    SurveyProgress:0,
                    SurveyID:0,
                    BeneficiaryID:0,
                    QuestionID:0,
                    Numb: 0,
                    PositionLR:0,
                    offset: 0,
                    QuestionN1: "",
                    QuestionN2: "",
                    Beneficiarymin: 0,
                    Projectmin: 0,
                    Questionmin: 0,
                    WorkspaceID: 0,
                    ProjectId: 0,
                    survey: require("@/assets/Survey/Total_Surveys.svg"),
                    response: require("@/assets/Survey/Total_Responses.svg"),
                    completed: require("@/assets/Survey/Total_completed.svg"),
                    data_survey_list: [
                        {
                            name: "Baseline Farmer Survey",
                            image: require("@/assets/Survey/Survey_Icon.svg"),
                        },
                        { name: "Baseline ", image: require("@/assets/Survey/Survey_Icon.svg") },
                        {
                            name: "Baseline Farmer ",
                            image: require("@/assets/Survey/Survey_Icon.svg"),
                        },
                    ],
                    printer: require("@/assets/Survey/Print.svg"),
                    excel: require("@/assets/Survey/Export_Excel.svg"),
                    data_survey_list: [
                        {
                            name: "Type of agricultural produce/crop cultivated",
                            image: require("@/assets/Survey/Question_Icon.svg"),
                        },
                        {
                            name: "Planned",
                            image: require("@/assets/Survey/Question_Icon.svg"),
                        },
                        {
                            name: "Planned v/s ",
                            image: require("@/assets/Survey/Question_Icon.svg"),
                        },
                    ],
                    //responsesBarData: {
                    //    labels: ["Teach Each", "Mission Home", "Funraisers"],
                    //    datasets: [
                    //        {
                    //            label: "Total Budget",
                    //            data: [500000, 900000, 700000],
                    //            backgroundColor: ["#F1C100", "#F1C100", "#F1C100"],
                    //        },
                    //    ],
                    //},
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
                    //responsesBarOptions: {
                    //    responsive: true,
                    //    maintainAspectRatio: false,
                    //    legend: {
                    //        display: false,
                    //    },
                    //    scales: {
                    //        yAxes: [
                    //            {
                    //                barPercentage: 1,
                    //                categoryPercentage: 1,
                    //                ticks: {
                    //                    beginAtZero: true,
                    //                    min: 0,
                    //                    max: 1000000,
                    //                    stepSize: 200000,
                    //                },
                    //            },
                    //        ],
                    //        xAxes: [
                    //            {
                    //                barThickness: 15,
                    //                categoryPercentage: 0.55,
                    //            },
                    //        ],
                    //    },
                    //},
                    responsesLineData: {
                        labels: ["Organic Method", "Organic Method", "Other Method"],
                        datasets: [
                            {
                                label: "Options",
                                data: [10, 60, 85],
                                backgroundColor: "transparent",
                                borderColor: "#F1C100",
                            },
                        ],
                    },
                    responsesLineOptions: {
                        responsive: true,
                        maintainAspectRatio: false,
                        legend: {
                            display: false,
                        },
                        scales: {
                            yAxes: [
                                {
                                    ticks: {
                                        beginAtZero: true,
                                    },
                                },
                            ],
                        },
                    },
                    msExcel: require("@/assets/microsoft-excel.png"),
                    items: [
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                        { divider: true, inset: true },
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                        { divider: true, inset: true },
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                        { divider: true, inset: true },
                    ],
                    items1: [
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                        {
                            avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
                            title: "Varun Mittal",
                            subtitle: "Updated Task",
                        },
                    ],
                    tabList: 'sec-preset',
                    presetName: "No Presets",
                    presetOptions: [],
                    cursor: 0,
                    currentPreset: '',
                    loader: false,
                    plotlyLayout: {
                        width: 500,
                        height: 400,
                    },
                    snackStatus : {
                    value: false,
                    message: "",
                    color: ""
                }
                };

            },
            mounted: async function () {
                await importscript();
                this.WorkspaceID = this.$route.query.WsID;
                this.userimage = window.SERVER_URL + "/Upload";
                await this.fnGetProjectData(this.WorkspaceID);

            },
            computed: {
                FormattedDate() {
                    return this.formatDate(this.FrmDate)
                },
                computedStartDate() {
                    return this.formatDate(this.startDate)
                },
                computedEndDate() {
                    return this.formatDate(this.endDate)
                },
                computedPresetResponse(){

                    let data = this.PresetResponseData
                    let format = objUtils.common.formatExcelDate

                    if(data.length) {
                        if(this.startDate && this.endDate){
                            let filteredData = data.filter((e)=> {
                                return format(e.SubmittedDate) <= this.endDate && format(e.SubmittedDate) >= this.startDate
                            })
                            return filteredData
                        }
                        if(this.startDate) {
                            let filteredData = data.filter((e)=> format(e.SubmittedDate) >= this.startDate)
                            return filteredData
                        }
                        if(this.endDate){
                            let filteredData = data.filter((e)=> format(e.SubmittedDate) <= this.endDate)
                            return filteredData
                        }
                    }
                    return data
                }
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

                async fnGetProjectData(WorkspaceID) {
                    this.PreLoader = true;
                    this.Projectmin = 0;
                    this.Questionmin = 0;
                    this.Beneficiarymin = 0;
                    var result = await objManageProjectSurvey.GetProjectData(WorkspaceID);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.lstproject = result.data;
                        if (this.lstproject.length != 0) {
                            this.Projectmin = 1;
                        }
                    }
                    this.PreLoader = false;
                },
                formatDate(date) {
                    if (!date) return null
                    const [year, month, day] = date.split('-')
                    return `${day}-${month}-${year}`
                },
                IsValidDate() {
                    if (Date.parse(this.endDate) < Date.parse(this.startDate)) {
                        this.Msg = "Invalid Date : End date should be greater than Start Date";
                        this.snackbarMsg = true;
                        this.endDate = "";
                        return;
                    }
                    if (Date.parse(this.startDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg =
                            "Invalid Date : Start date should not be less than Project Start Date - " +
                            this.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.startDate = "";
                        return;
                    }
                    if (Date.parse(this.startDate) > Date.parse(this.ProjectEndDate)) {
                        this.Msg =
                            "Invalid Date : Start date should not be greater than Project End Date - " +
                            this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.startDate = "";
                        return;
                    }
                    if (
                        Date.parse(this.endDate) > Date.parse(this.ProjectEndDate.substr(0, 10))
                    ) {
                        this.Msg =
                            "Invalid Date : End date should not be greater than Project End Date - " +
                            this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.snackbarMsg = true;
                        this.endDate = "";
                        return;
                    }
                    if (Date.parse(this.endDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg =
                            "Invalid Date : End date should be greater than Project Start Date (" +
                            this.formatDate(this.ProjectStartDate.substr(0, 10)) +
                            ")";
                        this.snackbarMsg = true;
                        this.endDate = "";
                        return;
                    }
                    if (Date.parse(this.endDate) == Date.parse(this.startDate)) {
                        this.Msg = "Invalid Date : Start Date and End date should not be same";
                        this.snackbarMsg = true;
                        this.endDate = "";
                        return;
                    }
                    // startdate/ Enddate  not < todays date
                    //enddate  any valid range - Max value
                },
                async fnGetSurveyData(ProjectId, startDate, endDate) {
                    this.PreLoader = true;
                    this.SurveyID = 0;
                    this.QuestionID = 0;
                    this.BeneficiaryID = 0;
                    this.Questionmin = 0;
                    this.Beneficiarymin = 0;

                    var result = await objManageProjectSurvey.GetSurveyData(ProjectId, startDate, endDate);
                   
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        const index = this.lstproject.findIndex((element, index) => {
                            if (element.Key === ProjectId) {
                                return true
                            }
                        });
                        this.Projectmin = (index + 1);
                        this.lstsurvey = result.data;
                    }
                    this.PreLoader = false;
                },
                async GetSurveyProgress(ProjectId) {
                    this.PreLoader = true;
                    this.SurveyProgress = 0;                    
                    var result = await objManageProjectSurvey.GetSurveyProgress(ProjectId);
                    
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.data) {

                            this.SurveyProgress = result.data.toFixed();
                        }
                    }
                    this.PreLoader = false;
                },
                async fnGetQuestionData(SurveyID) {
                    this.PreLoader = true;
                    this.QuestionID = 0;
                    this.BeneficiaryID = 0;
                    this.Questionmin = 0;
                    var result = await objManageProjectSurvey.GetQuestionData(SurveyID);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.lstquestion = result.data;
                        if (this.lstquestion.length != 0) {
                            this.Questionmin = 1;
                            this.QuestionID = this.lstquestion[0].Key;
                        }
                    }
                    this.PreLoader = false;
                    
                },
                async fnGetQuestionname(QuestionID) {
                    
                    if (this.QuestionID != 0) {
                        if (this.lstquestion.length != 0) {
                    
                            const index = this.lstquestion.findIndex((element, index) => {
                                if (element.Key === QuestionID) {
                                    return true
                                }
                            });
                            this.Questionmin = (index + 1);

                            var QuestTemp = this.lstquestion.filter(x => x.Key == this.QuestionID);
                            this.QuestionN1 = QuestTemp[0].Value;
                            this.QuestionN2 = QuestTemp[0].Value;
                        }
                    }
                    
                },
                async fnGetBeneficiaryPos(BeneficiaryID) {
                    if (this.BeneficiaryID != 0) {
                        if (this.lstbeneficiary.length != 0) {
                            const index = this.lstbeneficiary.findIndex((element, index) => {
                                if (element.Key === BeneficiaryID) {
                                    return true
                                }
                            });
                            this.Beneficiarymin = (index + 1);
                        }
                    }
                },
                async FetchAllProjectDet(ProjectId) {
                    this.PreLoader = true;
                    var result = await objManageProjectSurvey.GetDetailProjectData(ProjectId);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.data) {
                            this.TotResponse = result.data.SurveyTotResponse;
                            this.TotComplete = result.data.SurveyTotComplete;
                            this.TotResponseCount = result.data.SurveyTotResponseCount;
                            this.TotCompleteCount = result.data.SurveyTotCompleteCount;
                        }

                    var resultA = await objManageProjectSurvey.GetSurveyData(ProjectId, null, null);
                    this.lstsurveyTot = resultA.data;

                    this.PreLoader = false;
                },

                async fnQuestionDataLeftRight(QuestionID, PositionLR) {
                    
                    if ((this.lstquestion.length > 0) && (this.QuestionID != 0)) {
                        const index = this.lstquestion.findIndex((element, index) => {
                            if (element.Key === QuestionID) {
                                return true
                            }
                        });
                        var mQuestionID = 0;
                        if ((index != 0) && (PositionLR == 0)) {
                            mQuestionID = this.lstquestion[index - 1].Key;
                            this.Questionmin = (index - 1) + 1;
                            this.QuestionID = mQuestionID;
                            
                        }
                        else if ((index != this.lstquestion.length) && (PositionLR == 1)) {
                            mQuestionID = this.lstquestion[index + 1].Key;
                            this.Questionmin = (index + 1) + 1;
                            this.QuestionID = mQuestionID;
                            
                        }
                        if (mQuestionID != 0) {
                            
                            this.fnGetQuestionname(mQuestionID);
                            this.FetchAllSurveyQuestDetail(this.SurveyID, mQuestionID);
                            this.GetAllResponsesBySurveyID(mQuestionID);                           
                        }

                    }


                    
                },

                async fnProjectDataLeftRight(ProjectId,PositionLR) {
                    
                    if ((this.lstproject.length > 0) && (this.ProjectId !=0))
                    {
                        const index = this.lstproject.findIndex((element, index) => {
                            if (element.Key === ProjectId) {
                                return true
                            }
                        });
                        var mProjectID = 0;
                        if ((index != 0) && (PositionLR == 0)) {
                            mProjectID = this.lstproject[index - 1].Key;
                            this.Projectmin = (index - 1)+1;
                            this.ProjectId = mProjectID;
                            
                        }
                        else if ((index != this.lstproject.length) && (PositionLR == 1))
                        {
                            mProjectID = this.lstproject[index + 1].Key;
                            this.Projectmin = (index + 1)+1;
                            this.ProjectId = mProjectID;
                            
                        }
                        if (mProjectID != 0) {
                            this.fnGetSurveyData(mProjectID);
                            this.fnGetQuestionData(0);
                            this.fnGetBeneficiaryData(0);
                            this.GetSurveyProgress(mProjectID);
                            this.FetchAllProjectDet(mProjectID);
                        }

                    }
                  
                   
                    
                },

                async fnBeneficiaryDataLeftRight(BeneficiaryID, PositionLR) {
                    
                    if ((this.lstbeneficiary.length > 0) && (this.BeneficiaryID != 0)) {
                        const index = this.lstbeneficiary.findIndex((element, index) => {
                            if (element.Key === BeneficiaryID) {
                                return true
                            }
                        });
                        var mBeneficiaryID = 0;
                        if ((index != 0) && (PositionLR == 0)) {
                            mBeneficiaryID = this.lstbeneficiary[index - 1].Key;
                            this.Beneficiarymin = (index - 1) + 1;
                            this.BeneficiaryID = mBeneficiaryID;
                            
                        }
                        else if ((index != this.lstbeneficiary.length) && (PositionLR == 1)) {
                            mBeneficiaryID = this.lstbeneficiary[index + 1].Key;
                            this.Beneficiarymin = (index + 1) + 1;
                            this.BeneficiaryID = mBeneficiaryID;
                            
                        }                       

                    }


                    
                },

                formatDateAndTime(date) {
                    if (!date) return null;
                    const [day, month, year] = date.substr(0, 10).split("-");
                    const [hh, mm, ss] = date.substr(11, 19).split(":");
                    if (month === "01") {
                        if (hh == 12) {
                            return `${"JAN"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"JAN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"JAN"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "02") {
                        if (hh == 12) {
                            return `${"FEB"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"FEB"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"FEB"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "03") {
                        if (hh == 12) {
                            return `${"MAR"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"MAR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"MAR"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "04") {
                        if (hh == 12) {
                            return `${"APR"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"APR"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"APR"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "05") {
                        if (hh == 12) {
                            return `${"MAY"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"MAY"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"MAY"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "06") {
                        if (hh == 12) {
                            return `${"JUN"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"JUN"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"JUN"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "07") {
                        if (hh == 12) {
                            return `${"JUL"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"JUL"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"JUL"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "08") {
                        if (hh == 12) {
                            return `${"AUG"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"AUG"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"AUG"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "09") {
                        if (hh == 12) {
                            return `${"SEP"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"SEP"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"SEP"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "10") {
                        if (hh == 12) {
                            return `${"OCT"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"OCT"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"OCT"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "11") {
                        if (hh == 12) {
                            return `${"NOV"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"NOV"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"NOV"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                    if (month === "12") {
                        if (hh == 12) {
                            return `${"DEC"} ${day}, ${year} at ${hh}:${mm} PM`;
                        } else if (hh > 12) {
                            return `${"DEC"} ${day}, ${year} at ${hh - 12}:${mm} PM`;
                        } else {
                            return `${"DEC"} ${day}, ${year} at ${hh}:${mm} AM`;
                        }
                    }
                },

                async ShowChartRow(Numb) {
                    
                    this.snackbarMsg = false;
                    if (Numb == 1) {
                        if (this.SurveyID != 0) {
                            this.mShowChartRow = true;
                        }
                        else
                        {
                            this.Msg = "Please Select Survey";
                            this.snackbarMsg = true;
                        }
                    }
                    else 
                    {
                        this.mShowChartRow = false;
                    }
                },

                async FetchAllSurveyQuestDetail(SurveyID, QuestionID) {
                    this.PreLoader = true;
                    var result = await objManageProjectSurvey.FetchAllSurveyQuestDetail(SurveyID, QuestionID);
                    
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    }
                    else
                        if (result.data) {
                            this.QuestTotResponse = result.data[0].SurveyQuestionTotResponse;
                            this.QuestCreateDate = result.data[0].QuestionCreateDate;
                            this.QuestUpdateDate = result.data[0].QuestionUpdateDate;
                            this.SurveyBeneficiary = result.data[0].SurveyBeneficiary;
                            this.SurveyPeriodicity = result.data[0].SurveyPeriodicity;
                            this.SurveyProgress = result.data[0].SurveyProgress;
                            this.EnumName = result.data[0].EnumName;
                            this.EnumImage = result.data[0].EnumImage; 
                            this.EnumImagePath = this.userimage + this.EnumImage;
                        }
                    this.PreLoader = false;
                },

                async GetAllResponsesBySurveyID(QuestionID) {
                    this.PreLoader = true;
                    // Activity //
                    var resultA = await objManageProjectSurvey.GetSurveyAuditLog(this.SurveyID);
                    this.lstLogFrame = resultA.data;
                                      
                    this.lstQstnResponses = [];
                    var QuestionIDPass = QuestionID;
                    var data = new FormData();
                    data.append("SurveyID", this.SurveyID);
                    data.append("offset", this.offset);
                    data.append("QuestionID", QuestionIDPass);
                    var result = await objManageProjectSurvey.GetAllResponsesBySurveyReport(data);
                    
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    }
                    else {
                        if (result.SummaryResponse) {
                            
                            this.lstQstnResponses.push(result.SummaryResponse);
                            this.offset += 5;

                            var labelsArrays = [];
                            var dataArray = [];
                            var bgColor = [];
                            var MultichoiceResponse = 0;
                            for (var i = 0; i < result.SummaryResponse.length; i++) {
                                this.QuestionTypeBarLine = result.SummaryResponse[i].QuestionType;
                                
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
                                        /*MultichoiceResponse = MultichoiceResponse + lstAnswerList[k].count*/
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
                                    //result.SummaryResponse[i].ChartData = this.responsesBarData;
                                    //result.SummaryResponse[i].MultichoiceResponse = MultichoiceResponse;
                                    //labelsArrays = [];
                                    //dataArray = [];
                                    //this.responsesBarData = [];
                                   /* MultichoiceResponse = 0;*/
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
                                  /*  result.SummaryResponse[i].ChartData = this.responsesLineData;*/
                                    //labelsArrays = [];
                                    //dataArray = [];
                                    // this.responsesLineData = [];
                                }
                            }
                           
                        }                        
                    }
                    this.PreLoader = false;
                     
                },

                generator: function () {
                    this.mycolor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
                }, 

                async fnGetBeneficiaryData(SurveyID) {
                    this.PreLoader = true;
                    this.BeneficiaryID = 0;
                    this.Beneficiarymin = 0;
                    var result = await objManageProjectSurvey.GetBeneficiaryData(SurveyID);
                    if (result.status == "Unhandled Exception") {
                        this.PreLoader = false;
                        this.$router.push({ name: "error" });
                    } else if (result.data) {
                        this.lstbeneficiary = result.data;
                        
                        if (this.lstbeneficiary.length != 0) {
                            this.Beneficiarymin = 1;
                            this.BeneficiaryID = this.lstbeneficiary[0].Key;
                        }

                    }
                    this.PreLoader = false;
                },
                async GetPivotPresetList() {
                    this.loader = true;
                    this.presetOptions = [];
                    this.cursor = 0;
                    var data = new FormData();
                    data.append("SurveyID", this.SurveyID);
                    var result = await objManageProjectSurvey.GetPivotPresetList(data);
                    if (result !== null && result != undefined && result != '') {
                        result.forEach((r) => {
                            this.presetOptions.push({
                                PresetName: r.presetName,
                                PresetData: JSON.parse(r.presetData),
                            });
                        });

                        if (this.presetOptions.length) {
                            this.presetName = this.presetOptions[this.cursor].PresetName;
                            this.currentPreset = this.presetOptions[this.cursor].PresetData;
                            await this.loadPivotTable();
                            this.loader = false;
                        }
                        else {
                            this.loader = false;
                        }
                    }
                    else {
                        this.presetName = "No Presets";
                        this.loader = false;
                    }

                },
                async loadPivotTable() {
                    let renderers = {};
                    $.extend(renderers, $.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                    this.loader = true;

                    var url = window.SERVER_URL + '/ProjectSurvey/GetPivotTableData';
                    url += "?url=getpivotdata?surveyid=" + this.SurveyID;

                    try {
                        let data = await fetch(url, {
                            method: "POST",
                            body: ''
                        })
                        if(!data.ok) {
                            throw new Error(data.statusText)
                        }
                        let res = await data.json();
                        this.PresetResponseData = res;

                        if (this.PresetResponseData.length) {
                            await this.appendFilePath(this.PresetResponseData);
                            let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                            this.$nextTick(() => {
                                $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                            })
                            this.loader = false
                        } else {
                            this.presetName = "No Presets";
                            this.loader = false;
                        }
                    } catch (error) {
                        console.error(error)
                        this.presetOptions = []
                        this.presetName = "Error";
                        this.snackStatus = {
                            value : true,
                            message: "An Error occurred while fetching Pivot Table Data",
                            color: 'error'
                        }
                        this.loader = false
                    }
                },
                nextPreset() {
                    if(this.cursor == this.presetOptions.length - 1){
                        return
                    }
                    this.cursor++;
                    if (this.presetOptions.length && this.PresetResponseData.length) {
                        this.currentPreset = this.presetOptions[this.cursor].PresetData;
                        this.presetName = this.presetOptions[this.cursor].PresetName;
                        let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                        let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                        this.$nextTick(() => {
                            $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                        })
                    }
                },
                prevPreset() {
                    if(this.cursor == 0) {
                        return
                    }
                    this.cursor--;
                    if (this.presetOptions.length && this.PresetResponseData.length) {
                        this.currentPreset = this.presetOptions[this.cursor].PresetData;
                        this.presetName = this.presetOptions[this.cursor].PresetName;
                        let renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.plotly_renderers);
                        let runOptions = { ...this.currentPreset, renderers, rendererOptions: { ...this.currentPreset.rendererOptions, plotly: this.plotlyLayout }, showUI: false }
                        this.$nextTick(() => {
                            $("#output").pivotUI(this.computedPresetResponse, runOptions, true);
                        })
                    }
                },
                async appendFilePath(input) {
                    const surveyfilesURL = window.SERVER_URL + "/Upload/SureyQuestionnaire/" + this.ProjectId.toString() + "/";
                    const beneficiaryfilesURL = window.SERVER_URL + "/Upload/BeneficiaryRegistration/" + this.ProjectId.toString() + "/";
                    const allowedExtensions = ['jpg', 'jpeg', 'gif', 'png', 'txt', 'mp3', 'pdf', 'doc', 'docx', 'xlx', 'xlsx', 'csv', 'sveg','zip','rar'];

                    for (let [i, v] of input.entries()) {
                        for (let key of Object.keys(v)) {
                            if (v[key]) {
                                let cursor = v[key].toString()
                                let filextension = cursor.split('.').pop();
                                if (objUtils.common.checkIfValidlatitudeAndlongitude(v[key])) {
                                    v[key] = objUtils.GPS_TARGET_URL + v[key] + "?hl=en";
                                }
                                if (filextension && allowedExtensions.includes(filextension)) {
                                    
                                    if (key.includes('Beneficiary')) {
                                        v[key] = beneficiaryfilesURL + v[key]
                                    } else {
                                        v[key] = surveyfilesURL + v[key]
                                    }
                                }
                            }
                        }
                    }
                },
            },
            watch: {
                cursor: function (val) {
                    if (val > this.presetOptions.length - 1) {
                        this.cursor = this.presetOptions.length - 1;
                    }

                    if (val <= 0) {
                        this.cursor = 0;
                    }
                },
            },
            components: {
                //SurveyOverview: SurveyOverview,
                //OverallSurveyProgress: OverallSurveyProgress,
                //IndividualSurveyAnalysis: IndividualSurveyAnalysis,
                //SurveyCard:SurveyCard
                "Bar-Chart": BarChart,
                "Line-Chart": LineChart,
                'Secondary-Button': SecondaryButton,  
                'Default-Button-Outlined': DefaultButtonOutlined,
                'Pre-Loader': PreLoader,
            },
        };
    </script>



     

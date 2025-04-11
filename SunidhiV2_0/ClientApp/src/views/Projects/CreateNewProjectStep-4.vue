<template>
  <v-row class="mx-4">
    <v-col sm="12" md="6">
      <h3 class="create-project-steps-title text-center">Define Outputs</h3>
      <v-row>
        <v-col cols="12" align="center" class="pt-9">
          <v-carousel
          cycle="" height="200px"
          hide-delimiter-background=""
          show-arrows-on-hover=""
        >
            <v-carousel-item>
              <img src="../../images/Create-Project-Step4-Vector-1.svg" class="create-project-corousal-vector"/>
            </v-carousel-item>
          </v-carousel>
        </v-col>
        <v-col cols="12">
          <p class="text-center">
            Here you can define outputs &<br>activities for the project
          </p>
        </v-col>
      </v-row>
   </v-col>
    <v-col sm="12" md="6">
        <v-row>
            <v-col md="auto">
                <h6 class="heading-3">
                    <v-avatar size="30" color="#EDF6FF" class="mr-2">
                        <v-icon color="secondary" size="15">mdi-clipboard-list</v-icon>
                    </v-avatar> Outputs
                </h6>
            </v-col>
            <v-spacer></v-spacer>
            <v-col md="auto">
                <v-menu offset-y transition="scroll-y-transition">
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn class="success-btn" depressed dark height="37" v-bind="attrs" v-on="on">
                            Add Outputs
                            <v-icon>
                                mdi-chevron-down
                            </v-icon>
                        </v-btn>
                    </template>
                    <v-list dense>
                        <v-list-item id="AutoTest_IP1201" link @click="AddEditActivityDialog=true,AddEditActivityDialogHeader='Create Output'">
                            <v-list-item-content>
                                Add Output
                            </v-list-item-content>
                        </v-list-item>
                        <v-divider></v-divider>
                        <v-list-item id="AutoTest_IP202" link="" @click="CreateSubActivityDailog = true">
                            <v-list-item-content>
                                Add Activity
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>
                </v-menu>


            </v-col>
        </v-row>
        <!--Create Activity Dialog-->
        <v-dialog v-model="AddEditActivityDialog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-chart-areaspline</v-icon>
                    </v-avatar>
                    {{ AddEditActivityDialogHeader }}
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP221" text @click="AddEditActivityDialog=false,fnInitializeAddEditActivity(null),$refs.Form.resetValidation()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-form ref="Form"
                            v-model="valid"
                            @submit.prevent="fnAddActivity">
                <v-card-text style="height:60vh" class="v-scrolling-div">
                    <h4 class="text-center heading-3 mt-4">Please enter the details of the output.</h4>
                    <v-row justify="center">
                        <v-col md="10" class="mt-4">
                            <!--Basic Details Section-->
                            <h4 class="heading-3 mt-4">Basic Details</h4>
                            <v-row>
                                <v-col>
                                    <sub style="color:red;font-size:large"></sub>
                                    <v-text-field outlined dense label="Output Name" v-model="AddEditActivity.ActivityName"
                                                  :rules="alphanumericRules">
                                        <template v-if="AddEditActivity.ActivityName.length==0" v-slot:append>
                                            <span class="mandatory-star">*</span>
                                        </template>
                                    </v-text-field>

                                </v-col>
                                <v-col>
                                    <v-text-field hide-details outlined dense label="Description" v-model="AddEditActivity.Description">
                                    </v-text-field>
                                </v-col>
                                <v-responsive width="100%"></v-responsive>
                                <v-col>
                                    <v-menu v-model="mStartDate"
                                            :close-on-content-click="false"
                                            :nudge-right="40"
                                            transition="scale-transition"
                                            offset-y
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field v-model="computedStartDate"
                                                          label="Start Date"
                                                          outlined
                                                          :rules="[(value) => !!value || 'Required field']"
                                                          readonly dense
                                                          prepend-inner-icon="mdi-calendar"
                                                          v-on="on" hide-details>
                                                <template v-if="AddEditActivity.StartDate== ''" v-slot:append="">
                                                    <span class="mandatory-star">*</span>
                                                </template>
                                            </v-text-field>
                                        </template>
                                        <v-date-picker v-model="AddEditActivity.StartDate" color="primary" no-title @input="mStartDate = false,IsValidDate(),IsValidPeriodicityDate()" format="DD-MM-YYYY"
                                                       :min="ProjectStartDate" :max="ProjectEndDate"></v-date-picker>

                                    </v-menu>

                                </v-col>
                                <v-col>
                                    <v-menu v-model="mEndDate"
                                            :close-on-content-click="false"
                                            :nudge-right="40"
                                            transition="scale-transition"
                                            offset-y
                                            min-width="290px">
                                        <template v-slot:activator="{ on }">
                                            <v-text-field v-model="computedEndDate"
                                                          label="End Date"
                                                          outlined
                                                          :rules="[(value) => !!value || 'Required field']"
                                                          readonly dense
                                                          prepend-inner-icon="mdi-calendar"
                                                          v-on="on" hide-details>
                                                <template v-if="AddEditActivity.EndDate== ''" v-slot:append="">
                                                    <span class="mandatory-star">*</span>
                                                </template>
                                            </v-text-field>
                                        </template>
                                        <v-date-picker v-model="AddEditActivity.EndDate" color="primary" no-title @input="mEndDate = false,IsValidDate(),IsValidPeriodicityDate()"
                                                       :min="AddEditActivity.StartDate" :max="ProjectEndDate"></v-date-picker>
                                    </v-menu>

                                </v-col>
                                <v-responsive width="100%"></v-responsive>
                                <v-col>
                                    <v-text-field type="number" hide-details outlined dense label="Weightage (100 %)" v-model="AddEditActivity.WeightagePercentage"
                                                  onkeydown="javascript: return(event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189) ? false : true"
                                                  onKeyPress="if(this.value.length==3) return false;" min="0" max="100" :rules="percentageRules">
                                    </v-text-field>
                                </v-col>
                                <v-col>
                                    <!--Tag select-->
                                    <v-row class="mb-1">
                                        <v-col class="text-left d-inline-flex" md="12">
                                            <v-icon size="16">mdi-tag-multiple</v-icon>
                                            <span class="mr-1">Tags</span>
                                            <v-menu absolute="" v-model="AddEditActivityNewTagMenu" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                <template v-slot:activator="{ on }">
                                                    <v-btn id="AutoTest_IP222" v-on="on" max-width="15" min-width="15" height="25" outlined="" color="grey"
                                                           @click="fnInitializeSteper()" class="mr-1">
                                                        <v-icon size="15">mdi-plus</v-icon>
                                                    </v-btn>
                                                </template>
                                                <v-card>
                                                    <v-card-text class="pt-0 pb-1">
                                                        <v-col cols="12" md="12">
                                                            <!-- Header Text  -->
                                                            <v-col cols="12" md="12" class="d-inline-flex">
                                                                <v-btn id="AutoTest_IP223" text="" @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                    <v-icon size="15">mdi-chevron-left</v-icon>
                                                                </v-btn>
                                                                <v-spacer></v-spacer>
                                                                <h3 class="text-left page-head pt-2">
                                                                    {{TagHeaderName}}
                                                                </h3>
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP224" text="" @click="AddEditActivityNewTagMenu = false">
                                                                    <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-col>
                                                            <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                                <v-stepper-items>
                                                                    <!-- Create New Tag Option when there is no Tags are -->
                                                                    <v-stepper-content step="1" class="pa-0">
                                                                        <v-row>
                                                                            <v-col class="pb-1 ml-2">
                                                                                <v-text-field outlined label="Name" type="text" dense=""
                                                                                              v-model="NewTagName" hide-details="">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row class="pt-0">
                                                                            <v-col class="pb-1">
                                                                                <span class="ml-3">Select Color</span>
                                                                                <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                :swatches="swatches"
                                                                                                hide-canvas=""
                                                                                                hide-inputs=""
                                                                                                hide-mode-switch=""
                                                                                                show-swatches=""
                                                                                                v-model="color"
                                                                                                @input="">
                                                                                </v-color-picker>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col cols="12" md="12" class="text-center">
                                                                                <v-btn id="AutoTest_IP225" color="success-btn" depressed="" dark="" @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn>
                                                                                <!-- Adding To Existing Tag List-->
                                                                            </v-col>
                                                                        </v-row>
                                                                    </v-stepper-content>

                                                                    <!-- Select from existing Tags-->
                                                                    <v-stepper-content step="2" class="pa-0">
                                                                        <v-row style="max-height:30vh;overflow-y:scroll">
                                                                            <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                <p class="mb-2">Search Tags</p>
                                                                                <v-text-field outlined="" dense="" v-model="SearchTagDetails" label="Search tag here" hide-details="" class="mb-3">
                                                                                </v-text-field>
                                                                                <v-chip-group dark="" multiple="" column="" active-class="primary--text" style="max-width:350px">
                                                                                    <v-chip id="AutoTest_IP226" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small=""
                                                                                            label="" @click.native="fnAddToAddEditActivityTags(index)">
                                                                                        {{ index.TagName }}
                                                                                        <v-icon id="AutoTest_IP227" right="" color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                    </v-chip>
                                                                                </v-chip-group>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-col cols="12" md="12" class="text-center mt-3">
                                                                            <v-btn id="AutoTest_IP228" color="success-btn" depressed="" @click="AllTagListSteps = 1" dark="">Create New</v-btn>
                                                                        </v-col>
                                                                    </v-stepper-content>

                                                                    <!-- Edit Tag-->
                                                                    <v-stepper-content step="3" class="pa-0">
                                                                        <v-row>
                                                                            <v-col class="pb-1 ml-2">
                                                                                <v-text-field outlined="" type="text" dense=""
                                                                                              v-model="EditTagName" hide-details="">
                                                                                </v-text-field>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row class="pt-0">
                                                                            <v-col class="pb-1">
                                                                                <span class="ml-3">Select Color</span>
                                                                                <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                :swatches="swatches"
                                                                                                hide-canvas=""
                                                                                                hide-inputs=""
                                                                                                hide-mode-switch=""
                                                                                                show-swatches=""
                                                                                                v-model="color"
                                                                                                @input="">
                                                                                </v-color-picker>
                                                                            </v-col>
                                                                        </v-row>
                                                                        <v-row>
                                                                            <v-col cols="12" md="12" class="text-center">
                                                                                <v-btn id="AutoTest_IP229" depressed color="success-btn" dark="" @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                <v-chip dark="" v-if="AddEditActivity.ActivityTags && AddEditActivity.ActivityTags.length>0" :color="AddEditActivity.ActivityTags[0].TagColor" label="" small="" class="tags mr-1">{{AddEditActivity.ActivityTags[0].TagName}}</v-chip>
                                                <v-chip dark="" v-if="AddEditActivity.ActivityTags && AddEditActivity.ActivityTags.length>1" :color="AddEditActivity.ActivityTags[1].TagColor" label="" small="" class="tags mr-1">{{AddEditActivity.ActivityTags[1].TagName}}</v-chip>
                                                <v-chip dark="" v-if="AddEditActivity.ActivityTags && AddEditActivity.ActivityTags.length>2" label="" small="" class="tags tag-count mr-1">+{{AddEditActivity.ActivityTags.length-2}}</v-chip>
                                            </div>
                                            <v-btn id="AutoTest_IP230" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                            </v-btn>
                                            <v-btn id="AutoTest_IP231" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                            </v-btn>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutter="" v-if="allTags" class="mt-0">
                                        <v-col v-if="AddEditActivity.ActivityTags && AddEditActivity.ActivityTags.length > 0" md="12" class="chip-container mb-0 pt-0">
                                            <v-chip-group dark="" v-for="(TagItem, key) in AddEditActivity.ActivityTags" :key="TagItem.TagID" multiple="" column="" active-class="primary--text" class="d-inline-flex">
                                                <v-chip id="AutoTest_IP232" small="" :color="TagItem.TagColor" close="" @click:close="$delete(AddEditActivity.ActivityTags, key)" class="mb-0">
                                                    {{TagItem.TagName}}
                                                </v-chip>
                                            </v-chip-group>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>

                            <!--Datacollection Type Section-->
                            <h4 class="heading-3 my-2">Datacollection Type</h4>
                            <v-row>
                                <!-- Data collection Type-->
                                <v-col>
                                    <v-select dense v-model="AddEditActivity.ActivityType"
                                              :items="ActivityTypeItems"
                                              label="Select Datacollection Type"
                                              :rules="[(value) => !!value || 'Required field']"
                                              outlined hide-details @change="fnClearSurveydetails()">
                                        <template v-if="AddEditActivity.ActivityType== ''" v-slot:append="">
                                            <span class="mandatory-star">*</span>
                                        </template>
                                    </v-select>

                                </v-col>
                                <!-- Survey Combo-->
                                <v-col>

                                    <v-select v-show="AddEditActivity.ActivityType==='Quantitative'||AddEditActivity.ActivityType==='Qualitative'" dense
                                              :items="SurveyList" :rules="[(value) => !!value || 'Required field']" hide-details :item-text="i => i.SurveyName" :item-value="i => i.SurveyID"
                                              label="Select Survey" v-model="AddEditActivity.SurveyId" @change="GetSectionsBySurveyID(AddEditActivity.SurveyId)"
                                              outlined :disabled="SubActivityExists==1">
                                    </v-select>

                                </v-col>
                                <v-responsive width="100%"></v-responsive>
                                <template v-if="AddEditActivity.ActivityType==='Quantitative'">
                                    <v-col>

                                        <v-select :items="SectionList"
                                                  label="Select Section"
                                                  item-text="SectionName"
                                                  item-value="SectionID"
                                                  outlined
                                                  dense
                                                  @change="GetQstnBySectionID(AddEditActivity.SurveySectionId)"
                                                  v-model="AddEditActivity.SurveySectionId" hide-details v-bind:disabled="SubActivityExists==1"></v-select>
                                    </v-col>
                                    <v-col>

                                        <v-select :items="QstnList"
                                                  label="Select Question"
                                                  item-text="QuestionName"
                                                  item-value="QuestionID"
                                                  outlined
                                                  dense
                                                  @change="GetResponsesByQstnid(AddEditActivity.SurveyQuestionId)"
                                                  v-model="AddEditActivity.SurveyQuestionId" hide-details v-bind:disabled="SubActivityExists==1"></v-select>
                                    </v-col>
                                    <v-responsive width="100%"></v-responsive>

                                    <v-col>
                                        <v-select dense
                                                  :items="LogicItems" hide-details
                                                  item-text="Type"
                                                  item-value="ID"
                                                  label="Define Logic"
                                                  v-model="AddEditActivity.SurveyLogic"
                                                  outlined="" v-bind:disabled="SubActivityExists==1">
                                        </v-select>
                                    </v-col>
                                    <v-col></v-col>
                                    <v-responsive width="100%"></v-responsive>
                                    <v-col>
                                        <v-row>
                                            <v-col class="py-0" v-if="AddEditActivity.SurveyLogic==1 || AddEditActivity.SurveyLogic==2 || logicAnsType=='Single Line Text' || logicAnsType=='Numeric' ">
                                                <v-text-field outlined dense v-model="AddEditActivity.SurveyTarget1"></v-text-field>
                                            </v-col>
                                            <v-col class="py-0" v-else>
                                                <v-select dense :items="lstLogicAnswers" v-model="AddEditActivity.SurveyTarget"
                                                          item-text="OptionValue"
                                                          item-value="OptionID"
                                                          label="Define Target" outlined="" hide-details v-bind:disabled="SubActivityExists==1">
                                                </v-select>

                                            </v-col>

                                        </v-row>
                                    </v-col>
                                    <v-col></v-col>
                                </template>
                            </v-row>

                            <!--Payment Section-->
                            <h4 class="heading-3 mt-4">Payment</h4>
                            <v-row>
                                <v-col class="py-0">
                                    <p class="black--text">
                                        Payment Linked
                                        <v-switch hide-details v-model="AddEditActivity.IsPaymentLinked" class="ml-2" style="display:inline-block"
                                                  color="secondary"></v-switch>
                                    </p>
                                </v-col>
                                <v-col class="py-0"></v-col>
                                <v-responsive width="100%"></v-responsive>
                                <template v-if="AddEditActivity.IsPaymentLinked===true">
                                    <v-col>
                                        <v-text-field hide-details outlined dense label="Define Budget" v-model="AddEditActivity.PaymentLinkedBudget"></v-text-field>
                                    </v-col>
                                    <v-col></v-col>
                                    <v-responsive width="100%"></v-responsive>
                                    <!-- Expense Tags -->
                                    <v-col md="12">
                                        <span class="small-text mr-1">
                                            <v-icon size="12">mdi-tag</v-icon> Expense Tags
                                        </span>
                                        <v-chip id="AutoTest_IP233" v-for="(expenseTag, key) in AddEditActivity.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label close
                                                @click:close="$delete(AddEditActivity.ExpenseTags, key)">
                                            {{expenseTag.TagName}}
                                        </v-chip>

                                        <v-menu max-width="300" absolute v-model="AddEditActivityExpenseTagPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                            <template v-slot:activator="{ on }">
                                                <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                                    <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                                </v-btn>
                                            </template>
                                            <!--Expense Tag pop up-->
                                            <v-stepper v-model="AddEditActivityExpenseTagStepper">
                                                <v-stepper-items>
                                                    <v-stepper-content step="1" class="pa-0">
                                                        <v-card>
                                                            <v-card-title class="heading-3">
                                                                Add Expense Tags
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP234" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddEditActivityExpenseTagPopUp=false">
                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-divider></v-divider>
                                                            <v-card-text class="v-scrolling-div">
                                                                <v-row>
                                                                    <p class="mb-2">Search Tags</p>
                                                                    <v-col md="12" align="center">
                                                                        <v-text-field outlined="" dense hide-details label="Search Tag" v-model="AddEditActivitySearchExpenseTag">
                                                                        </v-text-field>
                                                                    </v-col>
                                                                    <v-col md="12">
                                                                        <v-chip id="AutoTest_IP235" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
                                                                                @click="fnAddExpenseTagToList(expenseTag)" class="ma-2" small label close close-icon="mdi-pencil" @click:close="AddEditActivityExpenseTagStepper=3,fnInitializeEditExpenseTagItem(expenseTag)">
                                                                            {{expenseTag.TagName}}
                                                                        </v-chip>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row justify="center">
                                                                    <v-col md="6">
                                                                        <Success-Button id="AutoTest_IP236" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddEditActivityExpenseTagStepper=2"></Success-Button>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-stepper-content>
                                                    <v-stepper-content step="2" class="pa-0">
                                                        <v-card>
                                                            <v-card-title class="heading-3">
                                                                <v-btn id="AutoTest_IP237" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="AddEditActivityExpenseTagStepper=1,AddEditActivityNewExpenseTagName=''">
                                                                    <v-icon size="20">mdi-arrow-left</v-icon>
                                                                </v-btn>
                                                                Create Expense Tags
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP238" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddEditActivityExpenseTagPopUp=false,AddEditActivityExpenseTagStepper=1,AddEditActivityNewExpenseTagName=''">
                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-divider></v-divider>
                                                            <v-card-text class="v-scrolling-div">
                                                                <v-row>
                                                                    <v-col md="12" align="center">
                                                                        <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="AddEditActivityNewExpenseTagName">
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row justify="center">
                                                                    <v-col md="6">
                                                                        <Success-Button id="AutoTest_IP239" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="AddEditActivityExpenseTagStepper=1,fnAddNewExpenseTag()"></Success-Button>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-stepper-content>
                                                    <v-stepper-content step="3" class="pa-0">
                                                        <v-card>
                                                            <v-card-title class="heading-3">
                                                                <v-btn id="AutoTest_IP240" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="AddEditActivityExpenseTagStepper=1,fnInitializeEditExpenseTagItem()">
                                                                    <v-icon size="20">mdi-arrow-left</v-icon>
                                                                </v-btn>
                                                                Edit Tags
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP241" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddEditActivityExpenseTagPopUp=false,AddEditActivityExpenseTagStepper=1,fnInitializeEditExpenseTagItem()">
                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-divider></v-divider>
                                                            <v-card-text class="v-scrolling-div">
                                                                <v-row>
                                                                    <v-col md="12" align="center">
                                                                        <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="AddEditActivityEditExpenseTagItem.TagName">
                                                                        </v-text-field>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row justify="center">
                                                                    <v-col md="6">
                                                                        <Success-Button id="AutoTest_IP242" style="width:130px" class="mb-2 center-block" title="Save" @click.native="AddEditActivityExpenseTagStepper=1,fnEditExpenseTag()"></Success-Button>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-stepper-content>
                                                </v-stepper-items>
                                            </v-stepper>
                                        </v-menu>
                                    </v-col>

                                    <!-- Notify User -->
                                    <v-col md="12">
                                        <span class="small-text mr-1">
                                            <v-icon size="12">mdi-account</v-icon> Notify Users
                                        </span>
                                        <v-avatar size="25" class="team-avatar" v-if="key>=4" v-for="(item,key) in AddEditActivity.ExpenseTagsNotifyUsers" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                        <v-avatar v-if="AddEditActivity.ExpenseTagsNotifyUsers != null && AddEditActivity.ExpenseTagsNotifyUsers.length>=4" size="25" class="team-count">
                                            +{{AddEditActivity.ExpenseTagsNotifyUsers.length-3}}
                                        </v-avatar>

                                        <v-menu min-width="100" absolute v-model="AddEditActivityNotifyPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                            <template v-slot:activator="{ on }">
                                                <v-btn id="AutoTest_IP243" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                       @click="if(AddEditActivity.ExpenseTagsNotifyUsers) selectedNotifyUsers=AddEditActivity.ExpenseTagsNotifyUsers">
                                                    <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                </v-btn>
                                            </template>
                                            <!-- user pop up-->
                                            <v-card>
                                                <v-card-title class="heading-3">
                                                    Select Users
                                                    <v-spacer></v-spacer>
                                                    <v-btn id="AutoTest_IP244" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddEditActivityNotifyPopUp=false,selectedNotifyUsers=[]">
                                                        <v-icon size="20">mdi-close</v-icon>
                                                    </v-btn>
                                                </v-card-title>
                                                <v-divider></v-divider>
                                                <v-card-text style="max-height:150px" class="v-scrolling-div">
                                                    <v-data-table :headers="ProjectUserListTableHeaders"
                                                                  :items="ProjectUserList"
                                                                  item-key="ID"
                                                                  hide-default-header
                                                                  sort-by="Name"
                                                                  show-select
                                                                  class="elevation-0"
                                                                  v-model="selectedNotifyUsers">
                                                        <template v-slot:item.Name="{item}">
                                                            <v-list class="py-0 table-v-list">
                                                                <v-list-item class="my-0 px-0">
                                                                    <v-list-item-icon>
                                                                        <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                            <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                        </v-avatar>
                                                                    </v-list-item-icon>
                                                                    <v-list-item-content>
                                                                        {{item.Name}}
                                                                    </v-list-item-content>
                                                                </v-list-item>
                                                            </v-list>
                                                        </template>
                                                        <template v-slot:no-data="">
                                                            <v-alert :value="true" color="error" icon="mdi-warning">
                                                                Sorry, nothing to display here :(
                                                            </v-alert>
                                                        </template>
                                                    </v-data-table>
                                                </v-card-text>
                                                <v-divider></v-divider>
                                                <v-card-actions>
                                                    <v-row no-gutters>
                                                        <v-col md="12" class="py-0" align="center">
                                                            <Success-Button id="AutoTest_IP245" v-bind:disabled="selectedNotifyUsers.length==0" style="width:130px" class="mb-2 center-block" title="Save"
                                                                            @click.native="AddEditActivityNotifyPopUp=false,AddEditActivity.ExpenseTagsNotifyUsers=selectedNotifyUsers,selectedNotifyUsers=[]"></Success-Button>
                                                        </v-col>
                                                    </v-row>
                                                </v-card-actions>
                                            </v-card>
                                        </v-menu>
                                    </v-col>
                                </template>
                            </v-row>

                            <!--Assignment section-->
                            <h4 class="heading-3 mt-4">Assignment</h4>
                            <v-row>
                                <!--<v-col>
                                    <v-select dense
                                              :items="ProjectWorkflowList" :item-text="i => i.WfName" :item-value="i => i.WorkflowID"
                                              label="Select Workflow" v-model="AddEditActivity.WorkflowId"
                                              outlined hide-details>
                                    </v-select>
                                </v-col>-->
                                <v-col></v-col>
                                <v-responsive width="100%"></v-responsive>
                                <v-col class="pb-0">
                                    <v-select dense="" :items="PeriodicityList" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Periodicity" outlined=""
                                              v-model="AddEditActivity.Periodicity" :rules="[(value) => !!value || 'Required field']">
                                        <template v-if="AddEditActivity.Periodicity== ''" v-slot:append>
                                            <span class="mandatory-star">*</span>
                                        </template>
                                    </v-select>

                                </v-col>
                                <v-col>
                                    <!-- Once -->
                                    <div v-show="AddEditActivity.Periodicity === 2">

                                        <v-menu v-model="AddEditActivityPeriodicityDatePicker"
                                                :close-on-content-click="false"
                                                :nudge-right="40"
                                                transition="scale-transition"
                                                offset-y
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-text-field v-model="computedPeriodicityDate"
                                                              label="Periodicity Date"
                                                              outlined
                                                              readonly
                                                              dense
                                                              prepend-inner-icon="mdi-calendar"
                                                              v-on="on" hide-details></v-text-field>
                                            </template>
                                            <v-date-picker v-model="AddEditActivity.PeriodicityOnceDate" no-title @input="AddEditActivityPeriodicityDatePicker = false,IsValidDate(),IsValidPeriodicityDate()"
                                                           :min="AddEditActivity.StartDate" :max="AddEditActivity.EndDate"></v-date-picker>
                                        </v-menu>

                                    </div>

                                    <!-- Weekly-->
                                    <div v-show="AddEditActivity.Periodicity === 4">
                                        <v-select dense="" :items="PeriodicityWeekList" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="AddEditActivity.PeriodicityWeek" hide-details>
                                        </v-select>
                                    </div>

                                    <!-- Monthly-->
                                    <div v-show="AddEditActivity.Periodicity === 5">
                                        <v-select dense="" :items="PeriodicityMonthListDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="AddEditActivity.PeriodicityMonthDay" hide-details>
                                        </v-select>
                                    </div>

                                    <!-- Quaterly , Halyearly and Yearly-->
                                    <div class="d-inline-flex">
                                        <v-select class="mr-1" hide-details dense="" :items="PeriodicityMonthList" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="AddEditActivity.PeriodicityMonth" v-show="AddEditActivity.Periodicity === 6|| AddEditActivity.Periodicity === 7 ||  AddEditActivity.Periodicity === 8">
                                        </v-select>
                                        <v-select hide-details dense="" :items="PeriodicityMonthListDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="AddEditActivity.PeriodicityMonthDateDay" v-show="AddEditActivity.Periodicity === 6|| AddEditActivity.Periodicity === 7 ||  AddEditActivity.Periodicity === 8">
                                        </v-select>
                                    </div>
                                </v-col>
                                <v-responsive width="100%"></v-responsive>
                                <v-col md="12" class="py-0">
                                    <span class="small-text mr-1">
                                        <v-icon size="12">mdi-account</v-icon> Users
                                    </span>
                                    <v-avatar size="25" class="team-avatar" v-for="(item,index) in AddEditActivity.ActivityUsers" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`" v-if="index <=2">
                                        <v-img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="((typeof item.ProfileImage != null ) && item.ProfileImage.length >0)" />
                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                        <!--<span class="white--text">{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>-->
                                    </v-avatar>
                                    
                                    <v-avatar v-if="AddEditActivity.ActivityUsers.length>=4" size="25" class="team-count">
                                        +{{AddEditActivity.ActivityUsers.length-3}}
                                    </v-avatar>
                                    <v-menu min-width="100" absolute="" v-model="AddEditActivityUserPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                        <template v-slot:activator="{ on }">
                                            <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" v-bind:disabled="SubActivityExists==1">
                                                <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                            </v-btn>
                                        </template>
                                        <!-- user pop up-->
                                        <v-card>
                                            <v-card-title class="heading-3">
                                                Select Users
                                                <v-spacer></v-spacer>
                                                <v-btn id="AutoTest_IP246" text="" height="25" min-width="25" max-width="25" color="#707070" @click="AddEditActivityUserPopUp=false">
                                                    <v-icon size="20">mdi-close</v-icon>
                                                </v-btn>
                                            </v-card-title>
                                            <v-divider></v-divider>
                                            <v-card-text style="max-height:150px" class="v-scrolling-div">
                                                <v-data-table :headers="ProjectUserListTableHeaders"
                                                              :items="ProjectUserList"
                                                              item-key="ID"
                                                              hide-default-header
                                                              sort-by="Name"
                                                              show-select
                                                              class="elevation-0"
                                                              v-model="selectedProjectUserList">
                                                    <template v-slot:item.Name="{item}">
                                                        <v-list class="py-0 table-v-list">
                                                            <v-list-item class="my-0 px-0">
                                                                <v-list-item-icon>
                                                                    <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                        <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                        <!--{{item.ProfileImage}}-->
                                                                    </v-avatar>
                                                                </v-list-item-icon>
                                                                <v-list-item-content>
                                                                    {{ item.Name }}
                                                                </v-list-item-content>
                                                            </v-list-item>
                                                        </v-list>
                                                    </template>
                                                    <template v-slot:no-data="">
                                                        <v-alert :value="true" color="error" icon="mdi-warning">
                                                            Sorry, nothing to display here :(
                                                        </v-alert>
                                                    </template>
                                                </v-data-table>
                                            </v-card-text>
                                            <v-divider></v-divider>
                                            <v-card-actions>
                                                <v-row no-gutters>
                                                    <v-col md="12" class="py-0" align="center">
                                                        <Success-Button id="AutoTest_IP247" v-bind:disabled="selectedProjectUserList.length==0" style="width:130px" class="mb-2 center-block" title="Save"
                                                                        @click.native="AddEditActivity.ActivityUsers=selectedProjectUserList,AddEditActivityUserPopUp=false"></Success-Button>
                                                    </v-col>
                                                </v-row>
                                            </v-card-actions>
                                        </v-card>
                                    </v-menu>
                                </v-col>
                            </v-row>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="pb-4">
                    <v-col>
                        <Default-Button-Outlined id="AutoTest_IP248" class="float-right btn-122x36" title="Cancel" @click.native="AddEditActivityDialog=false,fnInitializeAddEditActivity(null),$refs.Form.resetValidation()"></Default-Button-Outlined>
                        <!--<Secondary-Button id="AutoTest_IP249" v-bind:disabled="fnCheckMandatoryAddEditActivity()" class="float-right btn-122x36 mr-4" title="Save" @click.native="fnAddActivity('AddActivity')"></Secondary-Button>-->
                        <Secondary-Button id="AutoTest_IP249"  class="float-right btn-122x36 mr-4" title="Save" @click.native="fnAddActivity('AddActivity')"></Secondary-Button>
                    </v-col>
                </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>

        <!-- Created Sub Activity-->
        <v-dialog v-model="CreateSubActivityDailog" width="800" persistent>
            <v-card>
                <v-card-title primary-title class="page-head pb-0">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-chart-areaspline</v-icon>
                    </v-avatar>
                    Create Activities
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP256" text @click="CreateSubActivityDailog=false,CreateSubActivitySteps=1,fnInitializeSubActivity(),$refs.Form.resetValidation()">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-stepper v-model="CreateSubActivitySteps" class="elevation-0">
                    <v-stepper-items>
                        <v-stepper-content step="1" class="pa-0">
                            <h4 class="text-center heading-3 mt-4">step 1 : Please select under which output you want to create an activity</h4>
                            <v-row justify="center">
                                <v-col md="6">
                                    <v-row no-gutters>
                                        <v-col md="12">
                                            <v-select dense :items="ParentActivityList" :item-text="i => i.ActivityName" :item-value="i => i.ActivityId"
                                                      v-model="SubActivity.ParentActivityId"
                                                      label="Select an output" outlined hide-details>
                                            </v-select>
                                        </v-col>
                                        <v-col md="12">
                                            <p class="black--text">
                                                Copy the fields of the output
                                                <v-switch hide-details="" v-model="CopyParentActivitySwitch" class="ml-2 mt-1" style="display:inline-block"
                                                          color="secondary" @change="fnCopyParentActivitySwitch()"></v-switch>
                                            </p>
                                        </v-col>
                                    </v-row>
                                    <v-row no-gutters class="mt-0" v-show="CopyParentActivitySwitch == true">
                                        <v-col md="12">
                                            <h4 class="text-center heading-3 mt-4">Select fields of the output you want to copy.</h4>
                                        </v-col>
                                        <v-col>
                                            <v-checkbox color="#52B962" label="Basic Details" class="mr-5" hide-details="" v-model="CopyBasicDetails"></v-checkbox>
                                        </v-col>
                                        <v-col>
                                            <v-checkbox color="#52B962" label="Payment Details" hide-details="" v-model="CopyPaymnetDetails"></v-checkbox>
                                        </v-col>
                                        <v-responsive width="100%"></v-responsive>
                                        <v-col>
                                            <v-checkbox color="#52B962" label="Output Type" class="mr-5" hide-details="" v-model="CopyActivityType"></v-checkbox>
                                        </v-col>
                                        <v-col>
                                            <v-checkbox color="#52B962" label="Assignment" hide-details="" v-model="CopyAssignment"></v-checkbox>
                                        </v-col>

                                    </v-row>
                                </v-col>
                            </v-row>
                        </v-stepper-content>
                        <v-stepper-content step="2" class="pa-0">
                            <v-form ref="Form"
                                            v-model="valid"
                                            @submit.prevent="fnAddActivity">
                            <v-card-text style="height:60vh" class="v-scrolling-div">
                                <h4 class="text-center heading-3 mt-4">Please enter the details of the new  activities.</h4>
                                <v-row justify="center">
                                    <v-col md="10" class="mt-4">
                                        <!--Basic Details Section-->
                                        <h4 class="heading-3 my-2">Basic Details</h4>
                                        <v-row>
                                            <v-col>
                                                <v-text-field hide-details outlined dense label="Activity Name" v-model="SubActivity.ActivityName" :rules="alphanumericRules">
                                                    <template v-if="SubActivity.ActivityName.length==0" v-slot:append="">
                                                        <span class="mandatory-star">*</span>
                                                    </template>
                                                </v-text-field>
                                            </v-col>
                                            <v-col>
                                                <v-text-field hide-details outlined dense label="Description" v-model="SubActivity.Description">
                                                </v-text-field>
                                            </v-col>
                                            <v-responsive width="100%"></v-responsive>
                                            <v-col>

                                                <v-menu v-model="SubActivityStartDatePicker"
                                                        :close-on-content-click="false"
                                                        :nudge-right="40"
                                                        transition="scale-transition"
                                                        offset-y
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model="computedSubStartDate"
                                                                      label="Start Date"
                                                                      outlined
                                                                      :rules="[(value) => !!value || 'Required field']"
                                                                      readonly dense
                                                                      prepend-inner-icon="mdi-calendar"
                                                                      v-on="on" hide-details>
                                                            <template v-if="SubActivity.StartDate== ''" v-slot:append="">
                                                        <span class="mandatory-star">*</span>
                                                    </template>
                                                        </v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="SubActivity.StartDate" no-title @input="SubActivityStartDatePicker = false,IsValidSubActivityDate(),IsValidSubActivityPeriodicityDate()" format="DD-MM-YYYY"
                                                                   :min="parentActivityStartDate" :max="parentActivityEndDate"></v-date-picker>
                                                </v-menu>
                                            </v-col>
                                            <v-col>
                                                <v-menu v-model="SubActivityEndDatePicker"
                                                        :close-on-content-click="false"
                                                        :nudge-right="40"
                                                        transition="scale-transition"
                                                        offset-y
                                                        min-width="290px">
                                                    <template v-slot:activator="{ on }">
                                                        <v-text-field v-model="computedSubEndDate"
                                                                      label="End Date"
                                                                      outlined dense
                                                                      readonly
                                                                      :rules="[(value) => !!value || 'Required field']"
                                                                      prepend-inner-icon="mdi-calendar"
                                                                      v-on="on" hide-details>
                                                            <template v-if="SubActivity.EndDate== ''" v-slot:append="">
                                                                <span class="mandatory-star">*</span>
                                                            </template>
                                                        </v-text-field>
                                                    </template>
                                                    <v-date-picker v-model="SubActivity.EndDate" no-title @input="SubActivityEndDatePicker = false,IsValidSubActivityDate(),IsValidSubActivityPeriodicityDate()" format="DD-MM-YYYY"
                                                                   :min="SubActivity.StartDate" :max="parentActivityEndDate"></v-date-picker>
                                                </v-menu>
                                            </v-col>
                                            <v-responsive width="100%"></v-responsive>
                                            <!-- Weightage -->
                                            <v-col>
                                                <v-text-field type="number" hide-details outlined dense label="Weightage (100 %)" v-model="SubActivity.WeightagePercentage"
                                                              onkeydown="javascript: return(event.keyCode == 69 ||event.keyCode == 101 ||event.keyCode == 109||event.keyCode == 189) ? false : true"
                                                              onKeyPress="if(this.value.length==3 ) return false;" min="0" max="100" :rules="percentageRules">
                                                </v-text-field>
                                            </v-col>
                                            <v-col>
                                                <!--Tag select-->
                                                <v-row class="mb-1">
                                                    <v-col class="text-left d-inline-flex" md="12">
                                                        <v-icon size="16">mdi-tag-multiple</v-icon>
                                                        <span class="mr-1">Tags</span>
                                                        <v-menu absolute="" v-model="SubActivityNewTagMenu" top="" :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                            <template v-slot:activator="{ on }">
                                                                <v-btn id="AutoTest_IP257" v-on="on" max-width="15" min-width="15" height="25" outlined="" color="grey"
                                                                       @click="fnInitializeSteper()" class="mr-1">
                                                                    <v-icon size="15">mdi-plus</v-icon>
                                                                </v-btn>
                                                            </template>
                                                            <v-card>
                                                                <v-card-text class="pt-0 pb-1">
                                                                    <v-col cols="12" md="12">
                                                                        <!-- Header Text  -->
                                                                        <v-col cols="12" md="12" class="d-inline-flex">
                                                                            <v-btn id="AutoTest_IP258" text="" @click="AllTagListSteps = 2 ,TagHeaderName ='Add Tag' " v-if="AllTagListSteps != 2 || AllTagList.length ==0">
                                                                                <v-icon size="15">mdi-chevron-left</v-icon>
                                                                            </v-btn>
                                                                            <v-spacer></v-spacer>
                                                                            <h3 class="text-left page-head pt-2">
                                                                                {{TagHeaderName}}
                                                                            </h3>
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP259" text="" @click="SubActivityNewTagMenu = false">
                                                                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                                                                            </v-btn>
                                                                        </v-col>
                                                                        <v-stepper v-model="AllTagListSteps" class="elevation-0">
                                                                            <v-stepper-items>
                                                                                <!-- Create New Tag Option when there is no Tags are -->
                                                                                <v-stepper-content step="1" class="pa-0">
                                                                                    <v-row>
                                                                                        <v-col class="pb-1 ml-2">
                                                                                            <v-text-field outlined="" type="text" dense=""
                                                                                                          v-model="NewTagName" hide-details="">
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
                                                                                            <v-btn id="AutoTest_IP260" color="success-btn" depressed dark @click="CreateNewTag(color,NewTagName)" v-bind:disabled="NewTagName.length==0">Create</v-btn>
                                                                                            <!-- Adding To Existing Tag List-->
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                </v-stepper-content>

                                                                                <!-- Select from existing Tags-->
                                                                                <v-stepper-content step="2" class="pa-0">
                                                                                    <v-row style="max-height:30vh;overflow-y:scroll">
                                                                                        <v-col col="12" md="12" class="mb-0 pt-0">
                                                                                            <p class="mb-2">Search Tags</p>
                                                                                            <v-text-field outlined="" dense="" v-model="SearchTagDetails" label="Search tag here" hide-details="" class="mb-3">
                                                                                            </v-text-field>
                                                                                            <v-chip-group dark multiple="" column="" active-class="primary--text" style="max-width:350px">
                                                                                                <v-chip id="AutoTest_IP261" v-for="index in filterAllTagList" :key="index.TagID" :color="index.TagColor" small=""
                                                                                                        label="" @click.native="fnAddToSubActivityTags(index)">
                                                                                                    {{ index.TagName }}
                                                                                                    <v-icon id="AutoTest_IP262" right="" color="#FFFFFF" size="20" @click="fnEditTagDetails(index)">mdi-pencil-circle</v-icon>
                                                                                                </v-chip>
                                                                                            </v-chip-group>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-col cols="12" md="12" class="text-center mt-3">
                                                                                        <v-btn id="AutoTest_IP263" color="success-btn" @click="AllTagListSteps = 1" dark="">Create New</v-btn>
                                                                                    </v-col>
                                                                                </v-stepper-content>

                                                                                <!-- Edit Tag-->
                                                                                <v-stepper-content step="3" class="pa-0">
                                                                                    <v-row>
                                                                                        <v-col class="pb-1 ml-2">
                                                                                            <v-text-field outlined="" type="text" dense=""
                                                                                                          v-model="EditTagName" hide-details="">
                                                                                            </v-text-field>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-row class="pt-0">
                                                                                        <v-col class="pb-1">
                                                                                            <span class="ml-3">Select Color</span>
                                                                                            <v-color-picker class="ma-2 elevation-1" label="Select Color"
                                                                                                            :swatches="swatches"
                                                                                                            hide-canvas=""
                                                                                                            hide-inputs=""
                                                                                                            hide-mode-switch=""
                                                                                                            show-swatches=""
                                                                                                            v-model="color"
                                                                                                            @input="">
                                                                                            </v-color-picker>
                                                                                        </v-col>
                                                                                    </v-row>
                                                                                    <v-row>
                                                                                        <v-col cols="12" md="12" class="text-center">
                                                                                            <v-btn id="AutoTest_IP264" color="success-btn" dark="" @click="UpdateTagDetails(color,EditTagName)" v-bind:disabled="EditTagName.length== 0">Update</v-btn>
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
                                                            <v-chip dark v-if="SubActivity.ActivityTags && SubActivity.ActivityTags.length>0" :color="SubActivity.ActivityTags[0].TagColor" label="" small="" class="tags mr-1">{{SubActivity.ActivityTags[0].TagName}}</v-chip>
                                                            <v-chip dark v-if="SubActivity.ActivityTags && SubActivity.ActivityTags.length>1" :color="SubActivity.ActivityTags[1].TagColor" label="" small="" class="tags mr-1">{{SubActivity.ActivityTags[1].TagName}}</v-chip>
                                                            <v-chip dark v-if="SubActivity.ActivityTags && SubActivity.ActivityTags.length>2" label="" small="" class="tags tag-count mr-1">+{{SubActivity.ActivityTags.length-2}}</v-chip>
                                                        </div>
                                                        <v-btn id="AutoTest_IP265" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=true" class="mr-1" v-if="allTags == false">
                                                            <v-icon size="15">mdi-arrow-down-bold-outline</v-icon>
                                                        </v-btn>
                                                        <v-btn id="AutoTest_IP266" max-width="15" min-width="15" height="25" outlined="" dark="" color="grey" @click="allTags=false" class="mr-1" v-if="allTags == true">
                                                            <v-icon size="15">mdi-arrow-up-bold-outline</v-icon>
                                                        </v-btn>
                                                    </v-col>
                                                </v-row>
                                                <v-row no-gutter="" v-if="allTags" class="mt-0">
                                                    <v-col v-if="SubActivity.ActivityTags && SubActivity.ActivityTags.length > 0" md="12" class="chip-container mb-0 pt-0">
                                                        <v-chip-group dark v-for="(TagItem, key) in SubActivity.ActivityTags" :key="TagItem.TagID" multiple="" column="" active-class="primary--text" class="d-inline-flex">
                                                            <v-chip id="AutoTest_IP267" small="" :color="TagItem.TagColor" close="" @click:close="$delete(SubActivity.ActivityTags, key)" class="mb-0">
                                                                {{TagItem.TagName}}
                                                            </v-chip>
                                                        </v-chip-group>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                        </v-row>
                                        <!--Activity Type Section-->
                                        <h4 class="heading-3 my-2">Datacollection Type</h4>
                                        <v-row>
                                            <v-col>
                                                <v-select dense v-model="SubActivity.ActivityType"
                                                          :items="ActivityTypeItems"
                                                          :rules="[(value) => !!value || 'Required field']"
                                                          label="Select Datacollection Type"
                                                          outlined hide-details>
                                                    <template v-if="SubActivity.ActivityType== ''" v-slot:append="">
                                                        <span class="mandatory-star">*</span>
                                                    </template>
                                                </v-select>
                                            </v-col>
                                            <v-col>
                                                <v-select dense :items="SurveyList" hide-details :item-text="i => i.SurveyName" :item-value="i => i.SurveyID"
                                                          label="Select Survey" :rules="[(value) => !!value || 'Required field']" v-model="SubActivity.SurveyId" @change="GetSectionsBySurveyID(SubActivity.SurveyId)"
                                                          outlined v-bind:disabled="SubActivity.SubActivityExists==1">
                                                    <template v-if="((SubActivity.ActivityType==='Quantitative'||SubActivity.ActivityType==='Qualitative')&&SubActivity.SurveyId=='')" v-slot:append>
                                                        <span class="mandatory-star">*</span>
                                                    </template>
                                                </v-select>
                                            </v-col>
                                            <v-responsive width="100%"></v-responsive>
                                            <template v-if="SubActivity.ActivityType==='Quantitative'">
                                                <v-col>
                                                    <v-select :items="SectionList"
                                                              label="Select Section"
                                                              item-text="SectionName"
                                                              item-value="SectionID"
                                                              outlined
                                                              dense
                                                              @change="GetQstnBySectionID(SubActivity.SurveySectionId)"
                                                              v-model="SubActivity.SurveySectionId" hide-details v-bind:disabled="SubActivity.SubActivityExists==1"></v-select>
                                                </v-col>
                                                <v-col>
                                                    <v-select :items="QstnList"
                                                              label="Select Question"
                                                              item-text="QuestionName"
                                                              item-value="QuestionID"
                                                              outlined
                                                              dense
                                                              @change="GetResponsesByQstnid(SubActivity.SurveyQuestionId)"
                                                              v-model="SubActivity.SurveyQuestionId" hide-details v-bind:disabled="SubActivity.SubActivityExists==1"></v-select>
                                                </v-col>
                                                <v-responsive width="100%"></v-responsive>
                                                <v-col>
                                                    <v-select dense
                                                              :items="LogicItems" hide-details
                                                              item-text="Type"
                                                              item-value="ID"
                                                              label="Define Logic"
                                                              v-model="SubActivity.SurveyLogic"
                                                              outlined="">
                                                    </v-select>
                                                </v-col>
                                                <v-col></v-col>
                                                <v-responsive width="100%"></v-responsive>
                                                <v-col class="py-0">
                                                    <v-row>
                                                        <v-col class="py-0" v-if="SubActivity.SurveyLogic==1 || SubActivity.SurveyLogic==2 || logicAnsType=='Single Line Text' || logicAnsType=='Numeric' ">
                                                            <v-text-field outlined dense v-model="SubActivity.SurveyTarget1"></v-text-field>
                                                        </v-col>
                                                        <v-col class="py-0" v-else>
                                                            <v-select dense :items="lstLogicAnswers" v-model="SubActivity.SurveyTarget"
                                                                      item-text="OptionValue"
                                                                      item-value="OptionID"
                                                                      label="Define Target" outlined="" hide-details v-bind:disabled="SubActivity.SubActivityExists==1">
                                                            </v-select>
                                                        </v-col>
                                                        <!--<v-col v-else>
                                                    <v-text-field outlined dense v-model="SubActivity.SurveyTarget1"></v-text-field>
                                                </v-col>-->
                                                    </v-row>
                                                </v-col>
                                                <v-col></v-col>
                                            </template>
                                        </v-row>

                                        <!--Payment Section-->
                                        <h4 class="heading-3 my-2">Payment</h4>
                                        <v-row>
                                            <v-col class="py-0">
                                                <p class="black--text">
                                                    Payment Linked
                                                    <v-switch hide-details v-model="SubActivity.IsPaymentLinked" class="ml-2" style="display:inline-block"
                                                              color="secondary"></v-switch>
                                                </p>
                                            </v-col>
                                            <v-col class="py-0"></v-col>
                                            <v-responsive width="100%"></v-responsive>
                                            <template v-if="SubActivity.IsPaymentLinked===true">
                                                <v-col>
                                                    <v-text-field hide-details outlined dense label="Define Budget" v-model="SubActivity.PaymentLinkedBudget"></v-text-field>
                                                </v-col>
                                                <v-col></v-col>
                                                <v-responsive width="100%"></v-responsive>
                                                <!-- Expense Tags -->
                                                <v-col md="12">
                                                    <span class="small-text mr-1">
                                                        <v-icon size="12">mdi-tag</v-icon> Expense Tags
                                                    </span>
                                                    <v-chip id="AutoTest_IP268" v-for="(expenseTag, key) in SubActivity.ExpenseTags" :key="expenseTag.TagID" class="ma-2" small label close
                                                            @click:close="$delete(SubActivity.ExpenseTags, key)">
                                                        {{expenseTag.TagName}}
                                                    </v-chip>

                                                    <v-menu max-width="300" absolute v-model="SubActivityExpenseTagPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                        <template v-slot:activator="{ on }">
                                                            <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF">
                                                                <v-icon size="15" light color="#707070">mdi-plus</v-icon>
                                                            </v-btn>
                                                        </template>
                                                        <!--Expense Tag pop up-->
                                                        <v-stepper v-model="SubActivityExpenseTagStepper">
                                                            <v-stepper-items>
                                                                <v-stepper-content step="1" class="pa-0">
                                                                    <v-card>
                                                                        <v-card-title class="heading-3">
                                                                            Add Expense Tags
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP269" text="" height="25" min-width="25" max-width="25" color="#707070" @click="SubActivityExpenseTagPopUp=false">
                                                                                <v-icon size="20">mdi-close</v-icon>
                                                                            </v-btn>
                                                                        </v-card-title>
                                                                        <v-divider></v-divider>
                                                                        <v-card-text class="v-scrolling-div">
                                                                            <v-row>
                                                                                <p class="mb-2">Search Tags</p>
                                                                                <v-col md="12" align="center">
                                                                                    <v-text-field outlined="" dense hide-details label="Search Tag" v-model="SubActivitySearchExpenseTag">
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                                <v-col md="12">
                                                                                    <v-chip id="AutoTest_IP270" v-for="expenseTag in filterProjectExpenseTagList" :key="expenseTag.TagID"
                                                                                            @click="fnAddExpenseTagToList(expenseTag,'AddSubActivity')" class="ma-2" small label close close-icon="mdi-pencil"
                                                                                            @click:close="SubActivityExpenseTagStepper=3,fnInitializeEditExpenseTagItem(expenseTag,'AddSubActivity')">
                                                                                        {{expenseTag.TagName}}
                                                                                    </v-chip>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                        <v-divider></v-divider>
                                                                        <v-card-actions>
                                                                            <v-row justify="center">
                                                                                <v-col md="6">
                                                                                    <Success-Button id="AutoTest_IP271" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="SubActivityExpenseTagStepper=2"></Success-Button>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-actions>
                                                                    </v-card>
                                                                </v-stepper-content>
                                                                <v-stepper-content step="2" class="pa-0">
                                                                    <v-card>
                                                                        <v-card-title class="heading-3">
                                                                            <v-btn id="AutoTest_IP272" height="25" min-width="25" max-width="25" small depressed text class="mr-4" color="#707070" @click="SubActivityExpenseTagStepper=1,SubActivityNewExpenseTagName=''">
                                                                                <v-icon size="20">mdi-arrow-left</v-icon>
                                                                            </v-btn>
                                                                            Create Expense Tags
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP273" text="" height="25" min-width="25" max-width="25" color="#707070" @click="SubActivityExpenseTagPopUp=false,SubActivityExpenseTagStepper=1,SubActivityNewExpenseTagName=''">
                                                                                <v-icon size="20">mdi-close</v-icon>
                                                                            </v-btn>
                                                                        </v-card-title>
                                                                        <v-divider></v-divider>
                                                                        <v-card-text class="v-scrolling-div">
                                                                            <v-row>
                                                                                <v-col md="12" align="center">
                                                                                    <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="SubActivityNewExpenseTagName">
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                        <v-divider></v-divider>
                                                                        <v-card-actions>
                                                                            <v-row justify="center">
                                                                                <v-col md="6">
                                                                                    <Success-Button id="AutoTest_IP274" style="width:130px" class="mb-2 center-block" title="Create New" @click.native="SubActivityExpenseTagStepper=1,fnAddNewExpenseTag('AddSubActivity')"></Success-Button>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-actions>
                                                                    </v-card>
                                                                </v-stepper-content>
                                                                <v-stepper-content step="3" class="pa-0">
                                                                    <v-card>
                                                                        <v-card-title class="heading-3">
                                                                            <v-btn id="AutoTest_IP275" height="25" min-width="25" max-width="25" small="" depressed="" text="" class="mr-4" color="#707070" @click="SubActivityExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'AddSubActivity')">
                                                                                <v-icon size="20">mdi-arrow-left</v-icon>
                                                                            </v-btn>
                                                                            Edit Tags
                                                                            <v-spacer></v-spacer>
                                                                            <v-btn id="AutoTest_IP275" text="" height="25" min-width="25" max-width="25" color="#707070" @click="SubActivityExpenseTagPopUp=false,SubActivityExpenseTagStepper=1,fnInitializeEditExpenseTagItem(null,'AddSubActivity')">
                                                                                <v-icon size="20">mdi-close</v-icon>
                                                                            </v-btn>
                                                                        </v-card-title>
                                                                        <v-divider></v-divider>
                                                                        <v-card-text class="v-scrolling-div">
                                                                            <v-row>
                                                                                <v-col md="12" align="center">
                                                                                    <v-text-field outlined dense hide-details label="Enter Tag Name" v-model="SubActivityEditExpenseTagItem.TagName">
                                                                                    </v-text-field>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-text>
                                                                        <v-divider></v-divider>
                                                                        <v-card-actions>
                                                                            <v-row justify="center">
                                                                                <v-col md="6">
                                                                                    <Success-Button id="AutoTest_IP276" style="width:130px" class="mb-2 center-block" title="Save" @click.native="SubActivityExpenseTagStepper=1,fnEditExpenseTag('AddSubActivity')"></Success-Button>
                                                                                </v-col>
                                                                            </v-row>
                                                                        </v-card-actions>
                                                                    </v-card>
                                                                </v-stepper-content>
                                                            </v-stepper-items>
                                                        </v-stepper>
                                                    </v-menu>
                                                </v-col>

                                                <!-- Notify User -->
                                                <v-col md="12">
                                                    <span class="small-text mr-1">
                                                        <v-icon size="12">mdi-account</v-icon> Notify Users
                                                    </span>
                                                    <!--<v-avatar size="26" class="team-avatar" v-if="SubActivity.ExpenseTagsNotifyUsers != null && SubActivity.ExpenseTagsNotifyUsers.length == 0">
                                                <img src="https://cdn.vuetifyjs.com/images/john.jpg" class="team-image" alt="user name" />
                                            </v-avatar>-->
                                                    <!--<v-avatar size="26" class="team-avatar" color="red" v-if="SubActivity.ExpenseTagsNotifyUsers != null && SubActivity.ExpenseTagsNotifyUsers.length >= 1">
                                                <span>{{SubActivity.ExpenseTagsNotifyUsers[0].Name.charAt(0)}}</span>
                                            </v-avatar>
                                            <v-avatar size="26" class="team-avatar" color="blue" v-if="SubActivity.ExpenseTagsNotifyUsers != null && SubActivity.ExpenseTagsNotifyUsers.length >= 2">
                                                <span>{{SubActivity.ExpenseTagsNotifyUsers[1].Name.charAt(0)}}</span>
                                            </v-avatar>
                                            <v-avatar size="26" class="team-avatar" color="green" v-if="SubActivity.ExpenseTagsNotifyUsers != null && SubActivity.ExpenseTagsNotifyUsers.length >= 3">
                                                +{{ SubActivity.ExpenseTagsNotifyUsers.length-2 }}
                                            </v-avatar>-->
                                                    <v-avatar size="25" class="team-avatar" v-if="key>=4" v-for="(item,key) in SubActivity.ExpenseTagsNotifyUsers" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                        <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                    </v-avatar>
                                                    <v-avatar v-if="SubActivity.ExpenseTagsNotifyUsers != null && SubActivity.ExpenseTagsNotifyUsers.length>=4" size="25" class="team-count">
                                                        +{{SubActivity.ExpenseTagsNotifyUsers.length-3}}
                                                    </v-avatar>
                                                    <v-menu min-width="100" absolute v-model="SubActivityNotifyPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                        <template v-slot:activator="{ on }">
                                                            <v-btn id="AutoTest_IP276A" v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF"
                                                                   @click="if(SubActivity.ExpenseTagsNotifyUsers) selectedNotifyUsers=SubActivity.ExpenseTagsNotifyUsers">
                                                                <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                            </v-btn>
                                                        </template>
                                                        <!-- user pop up-->
                                                        <v-card>
                                                            <v-card-title class="heading-3">
                                                                Select Users
                                                                <v-spacer></v-spacer>
                                                                <v-btn id="AutoTest_IP277" text="" height="25" min-width="25" max-width="25" color="#707070" @click="SubActivityNotifyPopUp=false,selectedNotifyUsers=[]">
                                                                    <v-icon size="20">mdi-close</v-icon>
                                                                </v-btn>
                                                            </v-card-title>
                                                            <v-divider></v-divider>
                                                            <v-card-text style="max-height:150px" class="v-scrolling-div">
                                                                <v-data-table :headers="ProjectUserListTableHeaders"
                                                                              :items="ProjectUserList"
                                                                              item-key="ID"
                                                                              hide-default-header
                                                                              sort-by="Name"
                                                                              show-select
                                                                              class="elevation-0"
                                                                              v-model="selectedNotifyUsers">
                                                                    <template v-slot:item.Name="{item}">
                                                                        <v-list class="py-0 table-v-list">
                                                                            <v-list-item class="my-0 px-0">
                                                                                <v-list-item-icon>
                                                                                    <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                                        <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                        <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                    </v-avatar>
                                                                                </v-list-item-icon>
                                                                                <v-list-item-content>
                                                                                    {{item.Name}}
                                                                                </v-list-item-content>
                                                                            </v-list-item>
                                                                        </v-list>
                                                                    </template>
                                                                    <template v-slot:no-data="">
                                                                        <v-alert :value="true" color="error" icon="mdi-warning">
                                                                            Sorry, nothing to display here :(
                                                                        </v-alert>
                                                                    </template>
                                                                </v-data-table>
                                                            </v-card-text>
                                                            <v-divider></v-divider>
                                                            <v-card-actions>
                                                                <v-row no-gutters>
                                                                    <v-col md="12" class="py-0" align="center">
                                                                        <Success-Button id="AutoTest_IP278" v-bind:disabled="selectedNotifyUsers.length==0" style="width:130px" class="mb-2 center-block" title="Save"
                                                                                        @click.native="SubActivityNotifyPopUp=false,SubActivity.ExpenseTagsNotifyUsers=selectedNotifyUsers,selectedNotifyUsers=[]"></Success-Button>
                                                                    </v-col>
                                                                </v-row>
                                                            </v-card-actions>
                                                        </v-card>
                                                    </v-menu>
                                                </v-col>
                                            </template>
                                        </v-row>

                                        <!--Assignment section-->
                                        <h4 class="heading-3 my-2">Assignment</h4>
                                        <v-row>
                                            <!--<v-col>
                                                <v-select dense
                                                          :items="ProjectWorkflowList" :item-text="i => i.WfName" :item-value="i => i.WorkflowID"
                                                          label="Select Workflow" v-model="SubActivity.WorkflowId"
                                                          outlined hide-details>
                                                </v-select>
                                            </v-col>-->
                                            <v-col></v-col>
                                            <v-responsive width="100%"></v-responsive>
                                            <v-col>

                                                <v-select dense="" :rules="[(value) => !!value || 'Required field']" :items="PeriodicityList" :item-text="i => i.Value" :item-value="i => i.Key" label="Select Periodicity" outlined="" v-model="SubActivity.Periodicity">
                                                    <template v-if="SubActivity.Periodicity== ''" v-slot:append="">
                                                        <span class="mandatory-star">*</span>
                                                    </template>
                                                </v-select>
                                            </v-col>
                                            <v-col>
                                                <!-- Once -->
                                                <div v-show="SubActivity.Periodicity === 2">

                                                    <v-menu v-model="SubActivityPeriodicityDatePicker"
                                                            :close-on-content-click="false"
                                                            :nudge-right="40"
                                                            transition="scale-transition"
                                                            offset-y
                                                            min-width="290px">
                                                        <template v-slot:activator="{ on }">
                                                            <v-text-field v-model="computedSubPeriodicityDate"
                                                                          label="Periodicity Date"
                                                                          outlined
                                                                          readonly
                                                                          dense
                                                                          prepend-inner-icon="mdi-calendar"
                                                                          v-on="on" hide-details></v-text-field>
                                                        </template>
                                                        <v-date-picker v-model="SubActivity.PeriodicityOnceDate" no-title @input="SubActivityPeriodicityDatePicker = false" format="DD-MM-YYYY"
                                                                       :min="SubActivity.StartDate" :max="SubActivity.EndDate"></v-date-picker>
                                                    </v-menu>
                                                </div>

                                                <!-- Weekly-->
                                                <div v-show="SubActivity.Periodicity === 4">
                                                    <v-select dense="" :items="PeriodicityWeekList" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a day" outlined="" v-model="SubActivity.PeriodicityWeek" hide-details="">
                                                    </v-select>
                                                </div>

                                                <!-- Monthly-->
                                                <div v-show="SubActivity.Periodicity === 5">
                                                    <v-select dense="" :items="PeriodicityMonthListDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="SubActivity.PeriodicityMonthDay" hide-details="">
                                                    </v-select>
                                                </div>

                                                <!-- Quaterly , Halyearly and Yearly-->
                                                <div class="d-inline-flex">
                                                    <v-select class="mr-1" hide-details="" dense="" :items="PeriodicityMonthList" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Month" outlined="" v-model="SubActivity.PeriodicityMonth" v-show="SubActivity.Periodicity === 6|| SubActivity.Periodicity === 7 ||  SubActivity.Periodicity === 8">
                                                    </v-select>
                                                    <v-select hide-details="" dense="" :items="PeriodicityMonthListDate" :item-text="i => i.Value" :item-value="i => i.Key" label="Choose a Date" outlined="" v-model="SubActivity.PeriodicityMonthDateDay" v-show="SubActivity.Periodicity === 6|| SubActivity.Periodicity === 7 ||  SubActivity.Periodicity === 8">
                                                    </v-select>
                                                </div>
                                            </v-col>
                                            <v-col md="12">
                                                <span class="small-text mr-1">
                                                    <v-icon size="12">mdi-account</v-icon> Users
                                                </span>
                                                <v-avatar size="25" class="team-avatar" v-for="(item,key) in SubActivity.ActivityUsers.slice(0,3)" v-bind:key="item.ID" :color="`${commonGetColor(item.Name)}`">
                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                    <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                                </v-avatar>
                                                <v-avatar v-if="SubActivity.ActivityUsers.length>=4" size="25" class="team-count">
                                                    +{{SubActivity.ActivityUsers.length-3}}
                                                </v-avatar>
                                                <v-menu min-width="100" absolute="" v-model="SubActivityUserPopUp" top :offset-y="offset" :close-on-click="closeonClick" :close-on-content-click="closeOnContentClick">
                                                    <template v-slot:activator="{ on }">
                                                        <v-btn v-on="on" max-height="24" max-width="24" min-width="24" min-height="24" depressed="" fab="" light="" color="#E5F2FF" v-bind:disabled="SubActivity.SubActivityExists==1">
                                                            <v-icon size="15" light="" color="#707070">mdi-plus</v-icon>
                                                        </v-btn>
                                                    </template>
                                                    <!-- user pop up-->
                                                    <v-card>
                                                        <v-card-title class="heading-3">
                                                            Select Users
                                                            <v-spacer></v-spacer>
                                                            <v-btn id="AutoTest_IP279" text="" height="25" min-width="25" max-width="25" color="#707070" @click="SubActivityUserPopUp=false,selectedProjectUserList=SubActivity.ActivityUsers">
                                                                <v-icon size="20">mdi-close</v-icon>
                                                            </v-btn>
                                                        </v-card-title>
                                                        <v-divider></v-divider>
                                                        <v-card-text style="max-height:150px" class="v-scrolling-div">
                                                            <v-data-table :headers="ProjectUserListTableHeaders"
                                                                          :items="ProjectUserList"
                                                                          item-key="ID"
                                                                          hide-default-header
                                                                          sort-by="Name"
                                                                          show-select
                                                                          class="elevation-0"
                                                                          v-model="selectedProjectUserList">
                                                                <template v-slot:item.Name="{item}">
                                                                    <v-list class="py-0 table-v-list">
                                                                        <v-list-item class="my-0 px-0">
                                                                            <v-list-item-icon>
                                                                                <v-avatar size="27" light :color="`${commonGetColor(item.Name)}`">
                                                                                    <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                                                                    <span class="white--text" v-else>  {{item.Name == null ? "" :item.Name.substring(0, 1).toUpperCase()}}</span>
                                                                                </v-avatar>
                                                                            </v-list-item-icon>
                                                                            <v-list-item-content>
                                                                                {{ item.Name }}
                                                                            </v-list-item-content>
                                                                        </v-list-item>
                                                                    </v-list>
                                                                </template>
                                                                <template v-slot:no-data="">
                                                                    <v-alert :value="true" color="error" icon="mdi-warning">
                                                                        Sorry, nothing to display here :(
                                                                    </v-alert>
                                                                </template>
                                                            </v-data-table>
                                                        </v-card-text>
                                                        <v-divider></v-divider>
                                                        <v-card-actions>
                                                            <v-row no-gutters>
                                                                <v-col md="12" class="py-0" align="center">
                                                                    <Success-Button id="AutoTest_IP280" v-bind:disabled="selectedProjectUserList.length==0" style="width:130px" class="mb-2 center-block" title="Save"
                                                                                    @click.native="SubActivityUserPopUp=false,SubActivity.ActivityUsers=selectedProjectUserList,selectedProjectUserList=[]"></Success-Button>
                                                                </v-col>
                                                            </v-row>
                                                        </v-card-actions>
                                                    </v-card>
                                                </v-menu>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                            </v-form>
                        </v-stepper-content>
                    </v-stepper-items>
                    <v-card-actions class="pb-4">
                        <v-col>
                            <Default-Button-Outlined id="AutoTest_IP281" class="float-right btn-122x36 mr-4" title="Cancel" @click.native="CreateSubActivityDailog=false,CreateSubActivitySteps=1,HidePreviousButton=true,fnInitializeSubActivity(),$refs.Form.resetValidation()"></Default-Button-Outlined>
                            <Default-Button-Outlined id="AutoTest_IP282" v-show="HidePreviousButton==false" class="float-right btn-122x36 mr-4" title="Previous" @click.native="CreateSubActivitySteps=1,HidePreviousButton=true"></Default-Button-Outlined>
                            <Secondary-Button id="AutoTest_IP283" v-bind:disabled="SubActivity.ParentActivityId==0" v-show="CreateSubActivitySteps==1||CreateSubActivitySteps==0" class="float-right btn-122x36 mr-4" title="Next" @click.native="CreateSubActivitySteps=2,HidePreviousButton=false,fnInitializeSubActivityDetails()"></Secondary-Button>
                            <!--<Secondary-Button id="AutoTest_IP284" class="btn-122x36 mr-2 float-right" v-show="CreateSubActivitySteps==2" title="Confirm" v-bind:disabled="fnCheckMandatorySubActivity()" @click.native="fnAddActivity('AddSubActivity')"></Secondary-Button>-->
                            <Secondary-Button id="AutoTest_IP284" class="btn-122x36 mr-2 float-right" v-show="CreateSubActivitySteps==2" title="Confirm"  @click.native="fnAddActivity('AddSubActivity')"></Secondary-Button>
                        </v-col>
                    </v-card-actions>
                </v-stepper>
            </v-card>
        </v-dialog>

        <!-- Remove Output-->
        <v-dialog v-model="RemoveOutputDialog" width="800">
            <v-card>
                <v-card-title primary-title="" class="page-head py-4">
                    <v-avatar color="white" size="25" class="elevation-1 mr-2">
                        <v-icon size="20" color="primary">mdi-trash-can-outline</v-icon>
                    </v-avatar>
                    Remove Activity
                    <v-spacer></v-spacer>
                    <v-btn id="AutoTest_IP253" text="" @click="RemoveOutputDialog=false">
                        <v-icon color="#707070" size="15">mdi-close</v-icon>
                    </v-btn>
                </v-card-title>
                <v-card-text class="pt-0">
                    <v-container fluid class="pt-4">
                        <v-row no-gutters>
                            <v-col cols="12" md="12" class="pt-0">
                                <v-container fluid class="pt-0">
                                    <v-row>
                                        <v-col  cols="12" md="12" class="pt-0">
                                            <h4 class="text-center heading-3">Are you sure you want to remove this Output ?</h4>
                                        </v-col>
                                        <v-col v-if="RemoveActivityItem!=null">
                                            <h4 class="text-center heading-3">
                                                <v-icon class="tmr-2" color="#707070" size="25">mdi-chart-areaspline</v-icon>
                                                {{ OutputName }}
                                            </h4>
                                        </v-col>
                                        <v-col cols="12" xs="12" sm="12" md="12" class="text-right py-0">
                                            <v-btn id="AutoTest_IP254" color="secondary elevation-0" class="mr-2" @click="fnRemoveActivity()">Remove</v-btn>
                                            <v-btn id="AutoTest_IP255" outlined="" color="grey" @click="RemoveOutputDialog=false,RemoveActivityItem=null">Cancel</v-btn>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-card-text>
            </v-card>
        </v-dialog>

        <v-row>
            <v-col cols="12" class="v-scrolling-div" style="max-height:40vh">
                <v-expansion-panels flat="">
                    <v-expansion-panel class="mb-4 pa-2"
                                       v-for="(item,index) in ActivityList"
                                       :key="index">
                        <v-expansion-panel-header class="text--secondary py-0 px-0 inline-flex" @click.native="fnGetGrandChildrenActivity(item.ActivityId)">
                            <v-avatar max-width="20">
                                <v-icon color="secondary" size="20" class="mr-2">
                                    mdi-calendar-text
                                </v-icon>
                            </v-avatar>
                            {{item.ActivityName}}
                            <v-spacer></v-spacer>
                            <v-btn fab x-small depressed class="mr-2" max-width="20" @click.native="AddEditActivityDialog = true,fnGetActivityDetailsByID(item.ActivityId),fnInitializeAddEditActivity(item)">
                                <v-icon color="#707070" size="15">mdi-pencil</v-icon>
                            </v-btn>
                            <v-btn fab x-small depressed class="mr-2" max-width="20" @click.native ="RemoveOutputDialog = true ,OutputName = item.ActivityName, OutputId = item.ActivityId">
                                <v-icon color="#707070" size="15">mdi-close</v-icon>
                            </v-btn>
                        </v-expansion-panel-header>
                        <v-expansion-panel-content class="px-9">
                            <v-row>
                                <v-col cols="auto">
                                    <p class="black--text mb-0">Start Date</p>
                                    <p class="text--secondary mb-0">{{formatDate(item.StartDate.substr(0, 10))}}</p>
                                </v-col>

                                <v-col cols="auto">
                                    <p class="black--text mb-0">End Date</p>
                                    <p class="text--secondary mb-0">{{formatDate(item.EndDate.substr(0, 10))}}</p>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="auto">
                                    <p class="black--text mb-0">Periodicity</p>
                                    <p class="text--secondary mb-0">{{item.ReportingFrequencyName}}</p>
                                </v-col>

                                <v-col cols="auto">
                                    <p class="black--text mb-0">Survey</p>
                                    <p class="text--secondary mb-0">{{item.SurveyName}}</p>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <p class="black--text mb-0">Type</p>
                                    <p class="text--secondary mb-0">{{item.ActivityType}}</p>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col md="auto">
                                    <p class="black--text mb-0 mt-1">Sub Activities</p>
                                </v-col>
                                <v-col md="auto">
                                    <Success-Button title="Add Activity" @click.native="CreateSubActivityDailog = true, SubActivity.ParentActivityId = item.ActivityId"></Success-Button>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" class="pt-0">
                                    <ol class="text--secondary pl-4">
                                        <li v-for="(item,index) in SubActivityList"
                                            :key="index">
                                            {{item.ActivityName}}
                                            <v-btn fab x-small depressed class="ml-2" max-width="20" @click.native="AddEditActivityDialog = true,fnGetActivityDetailsByID(item.ActivityId),fnInitializeAddEditActivity(item)">
                                                <v-icon color="#707070" size="15">mdi-pencil</v-icon>
                                            </v-btn>
                                        </li>

                                        <!--<li>
                                        Register new activity2
                                        <v-btn fab="" x-small="" depressed="" class="ml-2" max-width="20">
                                            <v-icon color="#707070" size="15">mdi-pencil</v-icon>
                                        </v-btn>
                                    </li>-->
                                    </ol>
                                </v-col>
                            </v-row>
                        </v-expansion-panel-content>
                    </v-expansion-panel>
                </v-expansion-panels>

            </v-col>
        </v-row>
    </v-col>
    <v-col cols="12" align="right">
      <Secondary-Button title="Next" class="btn-122x36 mr-4" v-on:click.native="GoToCreateNewProjectStepsFiveFn"></Secondary-Button>
      <v-btn depressed="" class="btn-122x36 mr-4" height="37"  v-on:click="GoToCreateNewProjectStepsThreeFn">Previous</v-btn>
      <v-btn depressed="" class="btn-122x36" height="37"  v-on:click="GoToCreateNewProjectStepsFiveFn">Skip for Now</v-btn>
    </v-col>
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
  </v-row>
</template>
<script>
    import SuccessButtonSm from'@/components/success-button-sm.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import SuccessButton from '@/components/success-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    import PreLoader from '@/components/pre-loader.vue'
    import objUtils from '../../utils.js'

    var objManageProjectActivity;
    async function importscript() {
        return Promise.all([
            import("../../BL/ManageProjectActivity.js").then(mod => {
                objManageProjectActivity = new mod.ManageProjectActivity();
            })
        ]);
    };

    export default {
        props: [
            'CreateNewProjectStepsProp'
        ],

        data: function () {
            return {
                valid: true,
                alphanumericRules: [
                    v => !v || /^[ a-z0-9A-Z()_-]+$/.test(v) || 'Please enter valid name',
                    (value) => !!value || "Required field.",
                ],
                percentageRules: [
                    v => !v || /^\d{1,2}\.\d{1,2}$|^\d{1,3}$/.test(v) || 'Please enter valid percentage value'
                ],
                PreLoader: false,
                //Table Pagination
                currentPage: 0,
                recordsPerPage: 10,
                //General parameters
                WorkspaceID: 0,
                initiativeInfo: {
                    InitiativeId: 0,
                },
                Msg: "",
                successSnackbarMsg: false,
                errorSnackbarMsg: false,
                infoSnackbarMsg: false,
                ProjectStartDate: "",
                ProjectEndDate: "",
                mStartDate: false,
                mEndDate: false,
                parentActivityStartDate: "",
                parentActivityEndDate: "",

                projectInfo: {
                    ProjectId: 0,
                    ProjectName: "",
                    Description: "",
                    Code: "",
                    Budget: 0,
                    StartDate: "",
                    EndDate: "",
                    CountryID: 0,
                    StateID: 0,
                    DistrictID: 0,
                    ProjectTags: "",
                   GroupId : 0,
                },

                //Listing parameters
                ActivityList: [],
                filterActivityList: [],
                selectedActivityList: [],

                //parameters used for both Add and Edit Activity
                AddEditActivityDialog: false,
                AddEditActivityDialogHeader: "",
                AddEditActivity: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: 0,
                    SurveyTarget: 0,
                    SurveyTarget1: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    //WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                    SubActivityExists : 0,
                },
                AddEditActivityNotifyPopUp: false,
                AddEditActivityUserPopUp: false,
                AddEditActivityExpenseTagStepper: 1,
                AddEditActivityExpenseTagPopUp: false,
                AddEditActivityNewExpenseTagName: "",
                AddEditActivitySearchExpenseTag: "",
                AddEditActivityEditExpenseTagItem: { TagID: 0, TagName: "" },
                AddEditActivityPeriodicityDatePicker: false,
                AddEditActivityNewTagMenu: false,
                rules: {
                    required: (value) => !!value || "Required.",
                },
                 ActivityTypeItems: ["Quantitative", "Qualitative"], //['Target', 'Non Target'],
                SurveyList: [],
                PeriodicityList: [],
                PeriodicityMonthListDate: [],
                PeriodicityMonthList: [],
                PeriodicityWeekList: [],
                ProjectExpenseTagList: [],
                filterProjectExpenseTagList: [],

                //parameters used in AddActivity, EditActivity and Assign Users
                ProjectUserListTableHeaders: [
                    { text: "Name", align: "left", value: "Name" },
                    { text: "User Role", value: "UserRole" },
                ],
                ProjectUserList: [],
                filterProjectUserList: [],
                selectedProjectUserList: [],
                selectedNotifyUsers: [],

                //Remove Activity parameters
                RemoveActivityItem: null,

                //Assign Users parameters
                AssignUserDialog: false,
                AssignUser: {
                    ActivityId: 0,
                    selectedProjectUserList: []
                },

                expenseTag: false,
                stepper: 1,
                LogicItems: [],
                QuestionItems: [],
                SectionItems: [],
                EditActivityDialog: false,
                offset: true,
                closeonClick: false,
                closeOnContentClick: false,
                // Create Sub Activity

                //Create Sub Activity parameters
                CreateSubActivityDailog: false,
                CreateSubActivitySteps: 0,
                ParentActivityList: [],
                SubActivityList: [],
                CopyParentActivitySwitch: false,
                CopyBasicDetails: false,
                CopyPaymnetDetails: false,
                CopyActivityType: false,
                CopyAssignment: false,
                SubActivity: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: 0,
                    SurveyTarget1: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    //WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },
                SubActivityStartDatePicker: false,
                SubActivityEndDatePicker: false,
                SubActivityNotifyPopUp: false,
                SubActivityUserPopUp: false,
                SubActivityExpenseTagStepper: 1,
                SubActivityExpenseTagPopUp: false,
                SubActivityNewExpenseTagName: "",
                SubActivitySearchExpenseTag: "",
                SubActivityEditExpenseTagItem: { TagID: 0, TagName: "" },
                SubActivityPeriodicityDatePicker: false,
                HidePreviousButton: true,

                SectionList: [],
                QstnList: [],
                logicAnsType: "Dropdown",
                lstLogicAnswers: [],
                logicType: "",


                // Tags ------------
                SubActivityNewTagMenu: false,
                SearchTagDetails: "",
                AllTagListSteps: 0,
                AllTagList: [],
                filterAllTagList: [],
                TagHeaderName: "Add Tag",
                NewTagName: "",
                color: "",
                swatches: [
                    ['#00FF00', '#FF0000'],
                    ['#F0EB06', '#4B0D64'],
                    ['#00FFFF'],
                    ['#0000FF'],
                ],
                allTags: false,
                EditTagName: "",
                tableDataTest: [],
                step: 50,
                ParentActivityOfSubAct: "",

             // Edit-------
              editActivity: {
                    ActivityId: 0,
                    ParentActivityId: 0,
                    ActivityName: "",
                    Description: "",
                    StartDate: "",
                    EndDate: "",
                    WeightagePercentage: 0,
                    ActivityTags: [],
                    ActivityType: "",
                    SurveyId: 0,
                    SurveySectionId: 0,
                    SurveyQuestionId: 0,
                    SurveyResponseId: 0,
                    SurveyLogic: "",
                    SurveyTarget: "",
                    IsPaymentLinked: false,
                    PaymentLinkedBudget: 0,
                    ExpenseTags: [],
                    ExpenseTagsNotifyUsers: [],
                    //WorkflowId: "",
                    Periodicity: "",
                    PeriodicityOnceDate: "", //once periodicity
                    PeriodicityWeek: 0, //week periodicity
                    PeriodicityMonthDay: 0, //monthly periodicity
                    PeriodicityMonth: 0, //quarterly, half-yearly, yearly periocicity
                    PeriodicityMonthDateDay: 0,  //quarterly, half-yearly, yearly periocicity
                    ActivityUsers: [],
                },
                OutputId : 0,
                OutputName : '',
                RemoveOutputDialog : false,
                userimage :'',
                SubActivityExists :0,
            }
        },

        components: {
            'Success-Button-Sm': SuccessButtonSm,
            'Secondary-Button': SecondaryButton,
            'Success-Button': SuccessButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
            'Pre-Loader': PreLoader,
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.initiativeInfo.InitiativeId = this.$route.query.InitID;
            //this.projectInfo.ProjectId = this.$route.query.ProjID;
            await importscript();
            this.parentActivityStartDate = window.ProjectStartDate;
            this.parentActivityEndDate =  window.ProjectEndDate;
            this.userimage = window.SERVER_URL + "/Upload";

        },
        computed: {

            //Pagination
            paginatedData: function () {
                const firstIndexOnPage = this.currentPage * this.recordsPerPage
                const firstIndexOnNextPage = (this.currentPage + 1) * this.recordsPerPage
                return this.filterActivityList.slice(firstIndexOnPage, firstIndexOnNextPage)
            },

            computedPeriodicityDate() {
                return this.formatDate(this.AddEditActivity.PeriodicityOnceDate)
            },
            computedStartDate() {
                return this.formatDate(this.AddEditActivity.StartDate)
            },
            computedEndDate() {
                return this.formatDate(this.AddEditActivity.EndDate)
            },
            computedSubStartDate() {
                return this.formatDate(this.SubActivity.StartDate)
            },
            computedSubEndDate() {
                return this.formatDate(this.SubActivity.EndDate)
            },
            computedSubPeriodicityDate() {
                return this.formatDate(this.SubActivity.PeriodicityOnceDate)
            },
        },
        methods: {
            /// <summary>
            ///  GoToCreateNewProjectStepsFiveFn :- Function used to go to new project step five (next step)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            GoToCreateNewProjectStepsFiveFn() {
                this.CreateNewProjectStepsProp = 5;
                this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
            },

            /// <summary>
            ///  GoToCreateNewProjectStepsThreeFn :- Function used to go to new project step three (previous step)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            GoToCreateNewProjectStepsThreeFn() {
                this.CreateNewProjectStepsProp = 3;
                this.$emit('CreateNewProjectStepsPropChange', this.CreateNewProjectStepsProp)
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
            ///  commonGetColor :- Function used to get color
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            commonGetColor(name) {
                return objUtils.common.getcolor(name);
            },

            /// <summary>
            ///  fnGetProjectDetailsByID :- Function used to fetch project details based on prjID
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectDetailsByID() {
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);
                var result = await objManageProjectActivity.GetProjectDetailsByID(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.projectInfo = result.data;
                    this.ProjectStartDate = result.data.StartDate;
                    this.ProjectEndDate = result.data.EndDate;
                } else {
                    this.Msg = "Failed to get details of Project " + this.projectInfo.ProjectId;
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetProjectActivityList :- Function used to fetch activity list based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectActivityList() {
                var result = await objManageProjectActivity.GetProjectActivityList(this.projectInfo.ProjectId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.filterActivityList = result.data;
                    //this.filterActivityList[0].children = [{ ActivityName: 'Activity 1.1'}];
                    this.ActivityList = result.data;
                    for (var i = 0; i < this.filterActivityList.length; i++) {
                        if (this.filterActivityList[i].children.length > 0)
                            this.filterActivityList[i].SubActivityExists = 1;
                        else
                            this.filterActivityList[i].SubActivityExists = 0;
                    }

                    this.ParentActivityList = this.ActivityList.filter(a => a.ParentActivityId == 0);
                } else {
                    this.Msg = "Failed to get activity list of Project " + this.projectInfo.ProjectName;
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetGrandChildrenActivity :- Function used to fetch sub activity list based on ActivityId
            /// </summary>
            /// <param name="ActivityId"></param>
            /// <returns> </returns>
            async fnGetGrandChildrenActivity(ActivityId) {
                var result = await objManageProjectActivity.GetSubActivityListByID(ActivityId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.SubActivityList = result.data;
                }
            },

            /// <summary>
            ///  fnGetLogicType :- Function used to fetch logic type
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetLogicType() {
                var result = await objManageProjectActivity.GetLogicType();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.LogicItems = result.data;
                }
            },

            /// <summary>
            ///  fnGetProjectSurveyList :- Function used to fetch project survey list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetProjectSurveyList() {
                var result = await objManageProjectActivity.GetProjectSurveyList(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {

                    this.filterPaymentList = result.data;
                    this.SurveyList = result.data;
                } else {
                    this.Msg = "Failed to get survey list of Project " + this.projectInfo.ProjectName;
                    this.errorSnackbarMsg= true;
                }
            },

            /// <summary>
            ///  GetSectionsBySurveyID :- Function used to fetch sections based on SurveyID
            /// </summary>
            /// <param name="SurveyID"></param>
            /// <returns> </returns>
            async GetSectionsBySurveyID(SurveyID) {
                var data = new FormData();
                data.append("SurveyID", SurveyID);
                var result = await objManageProjectActivity.GetSectionsBySurveyID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.SectionList = result.data;
                }
            },

            /// <summary>
            ///  GetQstnBySectionID :- Function used to fetch questions based on SectionID
            /// </summary>
            /// <param name="SectionID"></param>
            /// <returns> </returns>
            async GetQstnBySectionID(SectionID) {
                var data = new FormData();
                data.append("SectionID", SectionID);
                var result = await objManageProjectActivity.GetQstnBySectionID(data);
                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data)
                        this.QstnList = result.data;
                    if (this.AddEditActivity.SurveyQuestionId > 0) {
                        this.GetResponsesByQstnid(this.AddEditActivity.SurveyQuestionId);
                    }
                }
            },

            /// <summary>
            ///  GetResponsesByQstnid :- Function used to fetch responses based on Qstnid
            /// </summary>
            /// <param name="Qstnid"></param>
            /// <returns> </returns>
            async GetResponsesByQstnid(Qstnid) {
                this.logicAnsType = this.QstnList.filter(x => x.QuestionID == Qstnid)[0].QuestionType;
                if (this.logicAnsType == 'Single Choice' || this.logicAnsType == 'Multi-Choice' || this.logicAnsType == 'Dropdown') {
                    var result = await objManageProjectActivity.GetLogicAnswers(Qstnid);

                    if (result.status == "Unhandled Exception")
                        this.$router.push({ name: "error" });
                    else
                        if (result.data)
                           this.lstLogicAnswers = result.data;
                }
            },

            /// <summary>
            ///  fnGetUsersListByGroupId :- Function used to fetch users list based on GroupId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetUsersListByGroupId() {
                var data = new FormData();
                data.append("GroupId", this.projectInfo.GroupId);
                data.append("WorkspaceID", this.WorkspaceID);
                var result = await objManageProjectActivity.GetUsersListByGroupId(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectUserList = result.data;
                    this.filterProjectUserList = result.data;
                }
            },

            /// <summary>
            ///  fnGetExpenseTagList :- Function used to fetch all expense tag list based on ProjectId
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetExpenseTagList() {
                var result = await objManageProjectActivity.GetAllExpenseTags(this.projectInfo.ProjectId);

                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.ProjectExpenseTagList = result.data;
                    this.filterProjectExpenseTagList = result.data;
                } else {
                    this.Msg = "Failed to get all expense tag list of Project " + this.projectInfo.ProjectName;
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnGetPeriodicityList :- Function used to fetch periodicity list
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetPeriodicityList() {
                var result = await objManageProjectActivity.GetPeriodicityList();
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } if (result.status = "success") {

                    if (result.data.RptFrequencyList.length > 0) {
                        this.PeriodicityList = result.data.RptFrequencyList;
                    }
                    if (result.data.WeekList.length > 0) {
                        this.PeriodicityWeekList = result.data.WeekList;
                    }
                    if (result.data.MonthList.length > 0) {
                        this.PeriodicityMonthList = result.data.MonthList;
                    }
                    if (result.data.MonthDate.length > 0) {
                        this.PeriodicityMonthListDate = result.data.MonthDate;
                    }
                }
            },

            /// <summary>
            ///  fnInitializeAddEditActivity :- Function used to initialize activity variables for edit/update activity
            /// </summary>
            /// <param name="item"></param>
            /// <returns> </returns>
            fnInitializeAddEditActivity(item) {
                if (item != null) {
                    this.AddEditActivity.ActivityId = item.ActivityId;
                    this.AddEditActivity.ParentActivityId = item.ParentActivityId;
                    this.AddEditActivity.ActivityName = item.ActivityName;
                    this.AddEditActivity.Description = item.Description;
                    this.AddEditActivity.SubActivityExists = item.SubActivityExists;
                    //this.AddEditActivity.StartDate = this.formatDate(item.StartDate);
                    //this.AddEditActivity.EndDate = this.formatDate(item.EndDate);
                    this.AddEditActivity.StartDate = item.StartDate;
                    this.AddEditActivity.EndDate = item.EndDate;
                    this.AddEditActivity.WeightagePercentage = item.WeightagePercentage;
                    if (item.ActivityTags) {
                        item.ActivityTags.forEach(u => {
                            this.AddEditActivity.ActivityTags.push({ TagID: u.TagID, TagName: u.TagName })
                        });
                    }
                    this.AddEditActivity.ActivityTags = item.ActivityTags;
                    this.AddEditActivity.ActivityType = item.ActivityType;


                    this.AddEditActivity.SurveyId = item.SurveyId;
                    if (item.SurveyId != 0) {
                        this.GetSectionsBySurveyID(item.SurveyId)
                    }
                    this.AddEditActivity.SurveySectionId = item.SurveySectionId;
                    this.AddEditActivity.SurveyQuestionId = item.SurveyQuestionId;

                    if (item.SurveySectionId != 0) {
                        this.GetQstnBySectionID(item.SurveySectionId)
                    }
                    this.AddEditActivity.SurveyLogic = item.SurveyLogic;

                    //AddEditActivity.SurveyLogic==1 || AddEditActivity.SurveyLogic==2 || logicAnsType=='Single Line Text' || logicAnsType=='Numeric'
                    if (this.AddEditActivity.SurveyLogic==1 || this.AddEditActivity.SurveyLogic==2 || this.logicAnsType=='Single Line Text' || this.logicAnsType=='Numeric') {
                         this.AddEditActivity.SurveyTarget1 = item.SurveyTarget;
                        //
                    }
                    else if (!(this.AddEditActivity.SurveyLogic==1 || this.AddEditActivity.SurveyLogic==2 || this.logicAnsType=='Single Line Text' || this.logicAnsType=='Numeric')) {
                        this.AddEditActivity.SurveyTarget = parseInt(item.SurveyTarget);
                    }

                    this.AddEditActivity.IsPaymentLinked = item.IsPaymentLinked;
                    this.AddEditActivity.PaymentLinkedBudget = item.PaymentLinkedBudget;
                    if (item.ExpenseTags) {
                        item.ExpenseTags.forEach(u => {
                            this.AddEditActivity.ExpenseTags.push({ TagID: u.TagID, TagName: u.TagName })
                        });
                    }
                    this.AddEditActivity.ExpenseTagsNotifyUsers = [];
                    if(item.ExpenseTagsNotifyUsers != null)
                    {
                        this.AddEditActivity.ExpenseTagsNotifyUsers = item.ExpenseTagsNotifyUsers;
                    }
                    
                    //this.AddEditActivity.WorkflowId = item.WorkflowId;
                    this.AddEditActivity.Periodicity = item.Periodicity;
                    this.AddEditActivity.PeriodicityOnceDate = this.formatDate(item.PeriodicityOnceDate);
                    this.AddEditActivity.PeriodicityWeek = item.PeriodicityWeek;
                    this.AddEditActivity.PeriodicityMonthDay = item.PeriodicityMonthDay;
                    this.AddEditActivity.PeriodicityMonth = item.PeriodicityMonth;
                    this.AddEditActivity.PeriodicityMonthDateDay = item.PeriodicityMonthDateDay;
                    this.AddEditActivity.ActivityUsers = item.ActivityUsers;
                    
                    this.selectedProjectUserList = item.ActivityUsers;
                    this.SubActivityExists =  item.SubActivityExists;
                } else {
                    this.AddEditActivity.ActivityId = 0;
                    this.AddEditActivity.ParentActivityId = 0;
                    this.AddEditActivity.ActivityName = "";
                    this.AddEditActivity.Description = "";
                    this.AddEditActivity.StartDate = "";
                    this.AddEditActivity.EndDate = "";
                    this.AddEditActivity.WeightagePercentage = 0;
                    this.AddEditActivity.ActivityTags = [];
                    this.AddEditActivity.ActivityType = "";
                    this.AddEditActivity.SurveyId = 0;
                    this.AddEditActivity.SurveySectionId = 0;
                    this.AddEditActivity.SurveyQuestionId = 0;
                    this.AddEditActivity.SurveyResponseId = 0;
                    this.AddEditActivity.SurveyLogic = "";
                    this.AddEditActivity.SurveyTarget = 0;
                    this.AddEditActivity.SurveyTarget1 = "";
                    this.AddEditActivity.IsPaymentLinked = false;
                    this.AddEditActivity.PaymentLinkedBudget = 0;
                    this.AddEditActivity.ExpenseTags = [];
                    this.AddEditActivity.ExpenseTagsNotifyUsers = [];
                    //this.AddEditActivity.WorkflowId = "";
                    this.AddEditActivity.Periodicity = 0;
                    this.AddEditActivity.PeriodicityOnceDate = "";
                    this.AddEditActivity.PeriodicityWeek = 0;
                    this.AddEditActivity.PeriodicityMonthDay = 0;
                    this.AddEditActivity.PeriodicityMonth = 0;
                    this.AddEditActivity.PeriodicityMonthDateDay = 0;
                    this.AddEditActivity.ActivityUsers = [];
                    this.logicAnsType = "";
                    this.QstnList = [];
                    this.AddEditActivity.SubActivityExists = 0;
                    this.SubActivityExists = 0;
                }
            },

            /// <summary>
            ///  fnAddNewExpenseTag :- Function used to create new expense tag for payment linked activity based on ProjectId and tagname
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnAddNewExpenseTag(type) {
                var tagname = "";
                switch (type) {
                    case "AddSubActivity":
                        tagname = this.SubActivityNewExpenseTagName;
                        break;
                    default:
                        tagname = this.AddEditActivityNewExpenseTagName;
                        break;
                }
                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var data = new FormData();
                data.append("ProjectId", this.projectInfo.ProjectId);
                data.append("TagName", tagname);
                var result = await objManageProjectActivity.AddNewExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        switch (type) {
                            case "AddSubActivity":
                                this.SubActivityNewExpenseTagName = "";
                                break;
                            default:
                                this.AddEditActivityNewExpenseTagName = "";
                                break;
                        }
                        this.fnGetExpenseTagList();
                    } else {
                        this.Msg = "Failed to add expense tag.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  fnAddExpenseTagToList :- Function used to add expense tag to the list
            /// </summary>
            /// <param name="item"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            fnAddExpenseTagToList(item, type) {
                switch (type) {
                    case "AddSubActivity":
                        if (this.SubActivity.ExpenseTags.length == 0)
                            this.SubActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                        else {
                            var temp = this.SubActivity.ExpenseTags.filter(x => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.SubActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                            }
                        }
                        break;
                    default:
                        if (this.AddEditActivity.ExpenseTags.length == 0)
                            this.AddEditActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                        else {
                            var temp = this.AddEditActivity.ExpenseTags.filter(x => x.TagID == item.TagID);
                            if (temp.length == 0) {
                                this.AddEditActivity.ExpenseTags.push({ TagID: item.TagID, TagName: item.TagName });
                            }
                        }
                        break;
                }
            },

            /// <summary>
            ///  fnInitializeEditExpenseTagItem :- Function used to open edit expense tag item box, when clicking on the edit button from the list
            ///  initialize expense tag variables
            /// </summary>
            /// <param name="exptagitem = null"></param>
            /// <param name="type"></param>
            /// <returns> </returns>
            fnInitializeEditExpenseTagItem(exptagitem = null, type) {
                switch (type) {
                    case "AddSubActivity":
                        if (exptagitem == null) {
                            this.SubActivityEditExpenseTagItem.TagID = 0;
                            this.SubActivityEditExpenseTagItem.TagName = "";
                        } else {
                            this.SubActivityEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.SubActivityEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                    default:
                        if (exptagitem == null) {
                            this.AddEditActivityEditExpenseTagItem.TagID = 0;
                            this.AddEditActivityEditExpenseTagItem.TagName = "";
                        } else {
                            this.AddEditActivityEditExpenseTagItem.TagID = exptagitem.TagID;
                            this.AddEditActivityEditExpenseTagItem.TagName = exptagitem.TagName;
                        }
                        break;
                }
            },

            /// <summary>
            ///  fnEditExpenseTag :- Function used to edit expense tag item, when clicking on the save button
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnEditExpenseTag(type) {
                var tagid = 0; var tagname = "";
                switch (type) {
                    case "AddSubActivity":
                        tagid = this.SubActivityEditExpenseTagItem.TagID;
                        tagname = this.SubActivityEditExpenseTagItem.TagName;
                        break;
                    default:
                        tagid = this.AddEditActivityEditExpenseTagItem.TagID;
                        tagname = this.AddEditActivityEditExpenseTagItem.TagName;
                        break;
                }

                var temp = this.ProjectExpenseTagList.filter(x => x.TagName.includes(tagname));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }

                var data = new FormData();
                data.append("TagID", tagid);
                data.append("TagName", tagname);
                var result = await objManageProjectActivity.EditExpenseTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else {
                    if (result.status == "success") {
                        this.fnInitializeEditExpenseTagItem(null, type);
                        this.fnGetExpenseTagList();
                    } else {
                        this.Msg = "Failed to edit expense tag.";
                        this.errorSnackbarMsg = true;
                    }
                }
            },

            /// <summary>
            ///  IsValidPeriodicityDate :- Function used to check activity periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidPeriodicityDate() {
                if (this.AddEditActivity.StartDate != null) {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) < Date.parse(this.AddEditActivity.StartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Start Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) < Date.parse(this.ProjectStartDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be greater than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }

                if (this.AddEditActivity.EndDate != null) {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) > Date.parse(this.AddEditActivity.EndDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.AddEditActivity.PeriodicityOnceDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than Project End Date- " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        this.FrmDate = "";
                        return;
                    }
                }
                return true;

            },

            /// <summary>
            ///  IsValidDate :- Function used to check activity start date and activity end date is valid (between project start date and project end date) 
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidDate() {
                if (Date.parse(this.AddEditActivity.EndDate) < Date.parse(this.AddEditActivity.StartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.StartDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : Start date should not be less than Project Start Date - " + this.formatDate(this.ProjectStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.StartDate) > Date.parse(this.ProjectEndDate)) {
                    this.Msg = "Invalid Date : Start date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.StartDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) > Date.parse(this.ProjectEndDate.substr(0, 10))) {
                    this.Msg = "Invalid Date : End date should not be greater than Project End Date - " + this.formatDate(this.ProjectEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) < Date.parse(this.ProjectStartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Project Start Date (" + this.formatDate(this.ProjectStartDate.substr(0, 10)) + ")";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
                if (Date.parse(this.AddEditActivity.EndDate) == Date.parse(this.AddEditActivity.StartDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;
                }
            },

            /// <summary>
            ///  IsValidSubActivityPeriodicityDate :- Function used to check sub activity periodicity date is valid when periodicity as once (Between startDate and endDate)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidSubActivityPeriodicityDate() {
                if (this.SubActivity.StartDate != null) {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) < Date.parse(this.SubActivity.StartDate)) {
                        this.Msg ="Please choose a Report Frequency Date between Start Date and End Date";
                        this.infoSnackbarMsg = true;
                        this.SubActivity.PeriodicityOnceDate = "";
                        return;
                    }
                }
                else {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) < Date.parse(this.parentActivityStartDate)) {
                        this.Msg ="Please choose a Report Frequency Date between Start Date and End Date";
                        this.infoSnackbarMsg = true;
                        this.SubActivity.PeriodicityOnceDate = "";
                        return;
                    }
                }

                if (this.SubActivity.EndDate != null) {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) > Date.parse(this.SubActivity.EndDate)) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than End Date";
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }
                else {
                    if (Date.parse(this.SubActivity.PeriodicityOnceDate) > Date.parse(this.parentActivityEndDate.substr(0, 10))) {
                        this.Msg = "Invalid Date : Report Frequency Date should be less than parent activity End Date- " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                        this.infoSnackbarMsg = true;
                        return;
                    }
                }
                return true;

            },

            /// <summary>
            ///  IsValidSubActivityDate :- Function used to check sub activity start date and sub activity end date is valid (between parent activity start date and parent activity end date)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            IsValidSubActivityDate() {
               if (Date.parse(this.formatDate(this.parentActivityEndDate)) < Date.parse(this.SubActivity.StartDate)) {
                    this.Msg = "Invalid Date : End date should be greater than Start Date";
                    this.infoSnackbarMsg = true;
                    this.AddEditActivity.EndDate = "";
                    return;//this.SubActivity.StartDate
                }
                if (Date.parse(this.SubActivity.StartDate) < Date.parse(this.formatDate(this.parentActivityStartDate))) {
                    this.Msg = "Invalid Date : Start date should not be less than Parent activity Start Date - " + this.formatDate(this.parentActivityStartDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                   // this.AddEditActivity.StartDate = "";
                    this.SubActivity.StartDate = this.formatDate(this.parentActivityStartDate);
                    return;
                }
                if (Date.parse(this.SubActivity.StartDate) > Date.parse(this.formatDate(this.parentActivityEndDate))) {
                    this.Msg = "Invalid Date : Start date should not be greater than Parent activity End Date - " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                   // this.AddEditActivity.StartDate = "";
                     this.SubActivity.StartDate = this.formatDate(this.parentActivityStartDate);
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) > Date.parse(this.formatDate(this.parentActivityEndDate.substr(0, 10)))) {
                    this.Msg = "Invalid Date : End date should not be greater than parent activity End Date - " + this.formatDate(this.parentActivityEndDate.substr(0, 10));
                    this.infoSnackbarMsg = true;
                   // this.AddEditActivity.EndDate = "";
                    this.SubActivity.EndDate = this.formatDate(this.parentActivityEndDate);
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) < Date.parse(this.formatDate(this.parentActivityStartDate))) {
                    this.Msg = "Invalid Date : End date should be greater than parent activity Start Date (" + this.formatDate(this.parentActivityStartDate.substr(0, 10)) + ")";
                    this.infoSnackbarMsg = true;
                  //  this.AddEditActivity.EndDate = "";
                    this.SubActivity.EndDate = this.formatDate(this.parentActivityEndDate);
                    return;
                }
                if (Date.parse(this.SubActivity.EndDate) == Date.parse(this.SubActivity.StartDate)) {
                    this.Msg = "Invalid Date : Start Date and End date should not be same";
                    this.infoSnackbarMsg = true;
                    //this.AddEditActivity.EndDate = "";
                    this.SubActivity.StartDate = this.formatDate(this.parentActivityStartDate);
                    this.SubActivity.EndDate = this.formatDate(this.parentActivityEndDate);
                    return;
                }
            },

            /// <summary>
            ///  fnAddActivity :- Function used for AddActivity, EditActivity, AddSubActivity and EditSubActivity
            /// </summary>
            /// <param name="type"></param>
            /// <returns> </returns>
            async fnAddActivity(type) {
                var item;
                if (this.$refs.Form.validate() === true) {
                    switch (type) {
                         case "AddSubActivity":
                             item = this.SubActivity;
                             if (item.Periodicity == 2) {
                                 if (!this.IsValidSubActivityPeriodicityDate()) {
                                     this.Msg = " Invalid PeriodicityDate ";
                                     this.infoSnackbarMsg = true;
                                     this.SubActivity.PeriodicityOnceDate = "";
                                     return;
                                 }
                             }

                             break;
                         default:
                              item = this.AddEditActivity;
                              if (item.Periodicity == 2) {
                                  if (!this.IsValidPeriodicityDate()) {
                                      this.Msg = " Invalid PeriodicityDate ";
                                      this.infoSnackbarMsg = true;
                                      this.AddEditActivity.PeriodicityOnceDate = "";
                                      return;
                                  }
                              }
                              break;
                    }
                    if (this.ActivityList.length > 0 && item.ActivityId == 0) {
                         if (this.ActivityList.find((x) => x.ActivityName == item.ActivityName)) {
                             this.Msg ="ActivityName you entered already exists. Please enter a new activity name.";
                             this.infoSnackbarMsg = true;
                             return;
                         }
                    }

                    switch (type) {
                         case "AddSubActivity":
                             this.CreateSubActivityDailog = false;
                             this.CreateSubActivitySteps = 1;
                             break;
                         default:
                             this.AddEditActivityDialog = false;
                             break;
                    }

                    this.PreLoader = true;
                    var data = new FormData();

                    data.append("ProjectId", this.projectInfo.ProjectId);
                    data.append("ActivityId", item.ActivityId);
                    data.append("ParentActivityId", item.ParentActivityId);
                    data.append("ActivityName", item.ActivityName);
                    data.append("Description", item.Description);
                    data.append("StartDate", item.StartDate);
                    data.append("EndDate", item.EndDate);
                    data.append("WeightagePercentage", item.WeightagePercentage);
                    data.append("ActivityTagsList", JSON.stringify(item.ActivityTags));
                    data.append("ActivityType", item.ActivityType);
                    data.append("SurveyId", item.SurveyId);
                   if (item.ActivityType == "Quantitative") {
                        data.append("SurveySectionId", item.SurveySectionId);
                        data.append("SurveyQuestionId", item.SurveyQuestionId);
                        data.append("SurveyResponseId", item.SurveyResponseId);
                        data.append("SurveyLogic", item.SurveyLogic);
                        if (item.SurveyLogic == 1 || item.SurveyLogic == 2 || this.logicAnsType == "Single Line Text" || this.logicAnsType == "Numeric") {
                            data.append("SurveyTarget", item.SurveyTarget1);
                        } else {
                            data.append("SurveyTarget", item.SurveyTarget);
                        }
                   } else {
                        data.append("SurveySectionId", 0);
                        data.append("SurveyQuestionId", 0);
                        data.append("SurveyResponseId", 0);
                        data.append("SurveyLogic", "");
                        data.append("SurveyTarget", "");
                   }
                   data.append("IsPaymentLinked", item.IsPaymentLinked);
                   if (item.IsPaymentLinked) {
                        data.append("PaymentLinkedBudget", item.PaymentLinkedBudget);
                        data.append("ExpenseTagsList", JSON.stringify(item.ExpenseTags));
                        data.append("ExpenseTagsNotifyUsersList", JSON.stringify(item.ExpenseTagsNotifyUsers));
                   } else {
                        data.append("PaymentLinkedBudget", 0);
                        data.append("ExpenseTagsList", null);
                        data.append("ExpenseTagsNotifyUsersList", null);
                   }
                   //data.append("WorkflowId", item.WorkflowId);
                   data.append("Periodicity", item.Periodicity);
                   data.append("PeriodicityOnceDate", item.PeriodicityOnceDate);
                   data.append("PeriodicityWeek", item.PeriodicityWeek);
                   data.append("PeriodicityMonthDay", item.PeriodicityMonthDay);
                   data.append("PeriodicityMonth", item.PeriodicityMonth);
                   data.append("PeriodicityMonthDateDay", item.PeriodicityMonthDateDay);
                   data.append("ActivityUsersList", JSON.stringify(item.ActivityUsers));

                   var result = await objManageProjectActivity.AddActivity(data);

                   if (result.status == "Unhandled Exception") {
                        this.$router.push({ name: "error" });
                   } else {
                       if (result.status == "success") {
                           if (type == "AddSubActivity") {
                               var result = await objManageProjectActivity.GetSubActivityListByID(item.ParentActivityId);

                               if (result.status == "Unhandled Exception") {
                                   this.$router.push({ name: "error" });
                               } else if (result.data) {
                                   if (this.ParentActivityOfSubAct != "") {
                                      this.ParentActivityOfSubAct.children = result.data;
                                      this.ParentActivityOfSubAct.isOpen = false;
                                   } else {
                                      this.fnGetProjectActivityList();
                                   }
                               } else {
                                     this.Msg ="Failed to get sub-activity list of Activity " + oneact.ActivityName;
                                     this.errorSnackbarMsg = true;
                               }
                               this.Msg = "Sub Activity created successfully.";
                               this.successSnackbarMsg = true;
                               this.fnInitializeSubActivity(null);
                               this.$refs.Form.resetValidation()
                           } else if (item.ActivityId > 0) {
                                this.Msg = "Activity Successfully updated";
                                this.successSnackbarMsg = true;
                                this.fnInitializeAddEditActivity(null);
                                this.fnGetProjectActivityList();
                           } else {
                                this.Msg = "Activity Successfully added";
                                this.successSnackbarMsg = true;
                                this.fnInitializeAddEditActivity(null);
                                this.fnGetProjectActivityList();
                                this.$refs.Form.resetValidation()
                           }
                       } else if (result.status == "PeriodicityFailed") {
                           this.Msg = "Periodicity Failed";
                           this.errorSnackbarMsg = true;
                           this.AddEditActivityDialog = true;
                       } else {
                           this.Msg = "Failed to add an activity.";
                           this.errorSnackbarMsg = true;
                       }  
                       this.PreLoader = false;
                   }
                }
            },
    

            //Add SubActivity functions

            /// <summary>
            ///  fnCopyParentActivitySwitch :- Function used to clear all the checkboxes while copy parent activity (Basic Details, Payment Details,Activity Type, Assignment)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnCopyParentActivitySwitch() {
                if (this.CopyParentActivitySwitch == false) {
                    this.CopyBasicDetails = false;
                    this.CopyPaymnetDetails = false;
                    this.CopyActivityType = false;
                    this.CopyAssignment = false;
                }
            },

            /// <summary>
            ///  fnInitializeSubActivity :- Function used to set default values in sub activity variables
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnInitializeSubActivity() {
                this.CopyParentActivitySwitch = false;
                this.CopyBasicDetails = false;
                this.CopyPaymnetDetails = false;
                this.CopyActivityType = false;
                this.CopyAssignment = false;

                this.SubActivity.ActivityId = 0;
                this.SubActivity.ParentActivityId = 0;
                this.SubActivity.ActivityName = "";
                this.SubActivity.Description = "";
                this.SubActivity.StartDate = "";
                this.SubActivity.EndDate = "";
                this.SubActivity.WeightagePercentage = 0;
                this.SubActivity.ActivityTags = [];
                this.SubActivity.ActivityType = "";
                this.SubActivity.SurveyId = 0;
                this.SubActivity.SurveySectionId = 0;
                this.SubActivity.SurveyQuestionId = 0;
                this.SubActivity.SurveyResponseId = 0;
                this.SubActivity.SurveyLogic = "";
                this.SubActivity.SurveyTarget = 0;
                this.SubActivity.SurveyTarget1 = "";
                this.SubActivity.IsPaymentLinked = false;
                this.SubActivity.PaymentLinkedBudget = 0;
                this.SubActivity.ExpenseTags = [];
                this.SubActivity.ExpenseTagsNotifyUsers = [];
                //this.SubActivity.WorkflowId = "";
                this.SubActivity.Periodicity = 0;
                this.SubActivity.PeriodicityOnceDate = "";
                this.SubActivity.PeriodicityWeek = 0;
                this.SubActivity.PeriodicityMonthDay = 0;
                this.SubActivity.PeriodicityMonth = 0;
                this.SubActivity.PeriodicityMonthDateDay = 0;
                this.SubActivity.ActivityUsers = [];
            },

            /// <summary>
            ///  fnInitializeSubActivityDetails :- Function used to add parent output details to create a new sub activity
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnInitializeSubActivityDetails() {
                var temp = this.ActivityList.find(a => a.ActivityId == this.SubActivity.ParentActivityId);
    
                if (temp != null) {
                    if (this.CopyBasicDetails == true) {
                        this.SubActivity.ActivityName = temp.ActivityName;
                        this.SubActivity.Description = temp.Description;
                        this.SubActivity.StartDate = temp.StartDate
                        this.SubActivity.EndDate = temp.EndDate;
                        this.SubActivity.WeightagePercentage = temp.WeightagePercentage;
                        this.parentActivityStartDate = temp.StartDate;
                        this.parentActivityEndDate = temp.EndDate;
                        if (temp.ActivityTags) {
                            temp.ActivityTags.forEach(u => {
                                this.SubActivity.ActivityTags.push({ TagID: u.TagID, TagName: u.TagName, TagColor: u.TagColor })
                            });
                        }
                    } else {
                        this.SubActivity.ActivityName = "";
                        this.SubActivity.Description = "";
                        this.SubActivity.StartDate = "";
                        this.SubActivity.EndDate = "";
                        this.SubActivity.WeightagePercentage = 0;
                        this.SubActivity.ActivityTags = [];
                    }

                    if (this.CopyActivityType == true) {
                        this.SubActivity.ActivityType = temp.ActivityType;
                        this.SubActivity.SurveyId = temp.SurveyId;
                        if (this.SubActivity.SurveyId  != 0) {
                          await this.GetSectionsBySurveyID(this.SubActivity.SurveyId);
                        }
                        this.SubActivity.SurveySectionId = temp.SurveySectionId;
                        if (this.SubActivity.SurveySectionId != 0) {
                          await this.GetQstnBySectionID(this.SubActivity.SurveySectionId);
                        }
                        this.SubActivity.SurveyQuestionId = temp.SurveyQuestionId;
                        if(this.SubActivity.SurveyQuestionId > 0){
                        await this.GetResponsesByQstnid(this.SubActivity.SurveyQuestionId);
                        }
                        this.SubActivity.SurveyLogic = temp.SurveyLogic;

                        if (temp.SurveyLogic == 1 || temp.SurveyLogic == 2 || this.logicAnsType == "Single Line Text" || this.logicAnsType == "Numeric") {
                          this.SubActivity.SurveyTarget1 = temp.SurveyTarget;
                        } else {
                          this.SubActivity.SurveyTarget = parseInt(temp.SurveyTarget);
                        }
                        this.SubActivity.SurveyResponseId = temp.SurveyResponseId;
                        //this.SubActivity.SurveyTarget = temp.SurveyTarget;
                    } else {
                        this.SubActivity.ActivityType = "";
                        this.SubActivity.SurveyId = 0;
                        this.SubActivity.SurveySectionId = 0;
                        this.SubActivity.SurveyQuestionId = 0;
                        this.SubActivity.SurveyResponseId = 0;
                        this.SubActivity.SurveyLogic = "";
                        this.SubActivity.SurveyTarget = "";
                    }

                    if (this.CopyPaymnetDetails == true) {
                        this.SubActivity.IsPaymentLinked = temp.IsPaymentLinked;
                        this.SubActivity.PaymentLinkedBudget = temp.PaymentLinkedBudget;
                        if (temp.ExpenseTags) {
                            temp.ExpenseTags.forEach(u => {
                                this.SubActivity.ExpenseTags.push({ TagID: u.TagID, TagName: u.TagName })
                            });
                        }
                        this.SubActivity.ExpenseTagsNotifyUsers = temp.ExpenseTagsNotifyUsers;
                    } else {
                        this.SubActivity.IsPaymentLinked = false;
                        this.SubActivity.PaymentLinkedBudget = 0;
                        this.SubActivity.ExpenseTags = [];
                        this.SubActivity.ExpenseTagsNotifyUsers = [];
                    }

                    if (this.CopyAssignment == true) {
                        //this.SubActivity.WorkflowId = temp.WorkflowId;
                        this.SubActivity.Periodicity = temp.Periodicity;
                        this.SubActivity.PeriodicityOnceDate = temp.PeriodicityOnceDate;
                        this.SubActivity.PeriodicityWeek = temp.PeriodicityWeek;
                        this.SubActivity.PeriodicityMonthDay = temp.PeriodicityMonthDay;
                        this.SubActivity.PeriodicityMonth = temp.PeriodicityMonth;
                        this.SubActivity.PeriodicityMonthDateDay = temp.PeriodicityMonthDateDay;
                        this.SubActivity.ActivityUsers = temp.ActivityUsers;
                        this.selectedProjectUserList = temp.ActivityUsers;
                    } else {
                        //this.SubActivity.WorkflowId = "";
                        this.SubActivity.Periodicity = 0;
                        this.SubActivity.PeriodicityOnceDate = "";
                        this.SubActivity.PeriodicityWeek = 0;
                        this.SubActivity.PeriodicityMonthDay = 0;
                        this.SubActivity.PeriodicityMonth = 0;
                        this.SubActivity.PeriodicityMonthDateDay = 0;
                        this.SubActivity.ActivityUsers = [];
                    }
                }
            },

            //Add Tag functions

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
            ///  CreateNewTag :- Function used to create new tag based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async CreateNewTag(color, TagName) {
                var temp = this.AllTagList.filter(x => x.TagName.includes(TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }

                var data = new FormData();
                data.append("TagName", TagName);
                data.append("TagColor", color);
                data.append("InitiativeId", this.initiativeInfo.InitiativeId);

                var result = await objManageProjectActivity.CreateNewTag(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllProjectTags()
                    }
            },

            /// <summary>
            ///  fnGetAllProjectTags :- Function used to fetch all project tags
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnGetAllProjectTags() {
                var result = await objManageProjectActivity.GetAllProjectTags(this.initiativeInfo.InitiativeId);
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
            ///  fnAddToSubActivityTags :- Function used for add sub activity tag to the selected tag items based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToSubActivityTags(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.SubActivity.ActivityTags.length > 0) {
                        var temp = this.SubActivity.ActivityTags.filter(x => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.SubActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                        }
                    } else {
                        this.SubActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                }
            },

            /// <summary>
            ///  fnAddToAddEditActivityTags :- Function used for add activity tag to the selected tag items based on TagItem
            /// </summary>
            /// <param name="TagItem"></param>
            /// <returns> </returns>
            async fnAddToAddEditActivityTags(TagItem) {
                // check for existence . if Tag already exist do not add
                if (this.AllTagListSteps != 3) {
                    if (this.AddEditActivity.ActivityTags.length > 0) {
                        var temp = this.AddEditActivity.ActivityTags.filter(x => x.TagID == TagItem.TagID);
                        if (temp.length == 0) {
                            this.AddEditActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                        }
                    } else {
                        this.AddEditActivity.ActivityTags.push({ TagID: TagItem.TagID, TagName: TagItem.TagName, TagColor: TagItem.TagColor });
                    }
                }
            },

            /// <summary>
            ///  fnEditTagDetails :- Function used to get tag details for edit
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
            ///  UpdateTagDetails :- Function used to update tag details in edit mode based on color and TagName
            /// </summary>
            /// <param name="color"></param>
            /// <param name="TagName"></param>
            /// <returns> </returns>
            async UpdateTagDetails(color, TagName) {
                if (this.UpdatingTagItem == "") {
                    return;
                }
                var temp = this.AllTagList.filter(x => x.TagName.includes(TagName));
                if (temp.length > 0) {
                    this.Msg = "TagName you entered already exists. Please enter a new tag name.";
                    this.infoSnackbarMsg = true;
                    return;
                }
                var TagID = this.UpdatingTagItem.TagID;
                var data = new FormData();
                data.append("TagID", TagID);
                data.append("TagName", TagName);
                data.append("TagColor", color);

                var result = await objManageProjectActivity.UpdateTagDetails(data);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                }
                else
                    if (result.status == "success") {
                        this.TagHeaderName = 'Add Tag';
                        this.AllTagListSteps = 2;
                        this.fnGetAllProjectTags()
                        this.UpdatingTagItem = "";
                    }
            },
    //  Edit-------------
            /// <summary>
            ///  fnGetActivityDetailsByID :- Function used to fetch activity details based on ActivityId
            /// </summary>
            /// <param name="ActivityId"></param>
            /// <returns> </returns>
            async fnGetActivityDetailsByID(ActivityId) {
                var result = await objManageProjectActivity.GetActivityDetailsByID(ActivityId);
                if (result.status == "Unhandled Exception") {
                    this.$router.push({ name: "error" });
                } else if (result.data) {
                    this.AddEditActivity = result.data;
                } else {
                    this.Msg = "Failed to get activity details.";
                    this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnRemoveActivity :- Function used to remove activity
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            async fnRemoveActivity() {
                this.RemoveOutputDialog = false;
                var data = new FormData();
                var actlist = [];
                data.append("ActivityIdList", this.OutputId);
                var result = await objManageProjectActivity.RemoveActivity(data);

                if (result.status == "Unhandled Exception") {
                     this.$router.push({ name: "error" });
                } else if (result.status == "success") {
                     this.Msg = "Output Successfully removed";
                     this.successSnackbarMsg = true;
                     this.RemoveOutputDialog = false;
                     this.OutputName ='';
                     this.fnGetProjectActivityList();
                } else {
                     this.Msg = "Failed to remove an activities.";
                     this.errorSnackbarMsg = true;
                }
            },

            /// <summary>
            ///  fnClearSurveydetails :- Function used to clear survey details
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            fnClearSurveydetails() {
              this.AddEditActivity.SurveyId = 0;
              this.AddEditActivity.SurveySectionId = 0;
              this.AddEditActivity.SurveyQuestionId = 0;
              this.AddEditActivity.SurveyLogic = 0;
              this.AddEditActivity.SurveyTarget1 = 0;
              this.AddEditActivity.SurveyTarget = 0;
            },
        },

        watch: {

            CreateNewProjectStepsProp: function () {
                this.projectInfo.ProjectId  = window.ProjectId;//this.$route.query.ProjID;
                this.projectInfo.GroupId=window.GroupId ;
                this.PreLoader = true;
                this.fnGetProjectDetailsByID();
                this.fnGetProjectActivityList();
                this.PreLoader = false;
                this.fnGetProjectSurveyList();
                this.fnGetUsersListByGroupId();
                this.fnGetAllProjectTags();
                this.fnGetExpenseTagList();
                this.fnGetPeriodicityList();
                this.fnGetLogicType();
            },

            /// <summary>
            ///  AddEditActivitySearchExpenseTag :- Function used to search activity expense tags based on TagName (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            AddEditActivitySearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.AddEditActivitySearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.AddEditActivitySearchExpenseTag));
            },

            /// <summary>
            ///  SubActivitySearchExpenseTag :- Function used to search sub activity expense tags based on TagName (in expense tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SubActivitySearchExpenseTag: function () {
                this.filterProjectExpenseTagList = this.ProjectExpenseTagList;
                if (this.SubActivitySearchExpenseTag.length > 0)
                    this.filterProjectExpenseTagList = this.filterProjectExpenseTagList.filter(x => x.TagName.includes(this.SubActivitySearchExpenseTag));
            },

            /// <summary>
            ///  SearchTagDetails :- Function used to search tags based on TagName (in tag menu pop up)
            /// </summary>
            /// <param name=""></param>
            /// <returns> </returns>
            SearchTagDetails: function () {
                this.filterAllTagList = this.AllTagList;
                if (this.SearchTagDetails.length > 0)
                    this.filterAllTagList = this.filterAllTagList.filter(x => x.TagName.includes(this.SearchTagDetails));
            },
        }
    }

</script>
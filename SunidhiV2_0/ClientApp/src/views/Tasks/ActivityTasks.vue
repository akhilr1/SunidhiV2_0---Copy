<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0">
        <v-row justify="center">
            <v-col cols="12" xs="12" sm="12" md="12">
                <div>
                    <v-card class="mb-4" @click.native="fnClick(item)" v-for="(item,index) in MyTask" :key="index">
                        <v-card-title>
                            <v-row no-gutters>
                                <v-col>
                                    <h3 class="text-left page-head">
                                        {{item.ActivityName}}
                                    </h3>
                                </v-col>
                                <v-col>
                                    <v-chip :color="item.TypeColor" class="ml-0 mr-2" label dark small>
                                        Activity
                                    </v-chip>
                                </v-col>
                                <v-col md="auto">
                                    <h1 class="page-head text-right">
                                        {{new Date(item.CreatedDate).toLocaleDateString('en-GB')}}
                                    </h1>
                                </v-col>
                                <v-col md="auto">
                                    <v-btn min-width="27" height="29" color="grey" outlined dark class="elevation-0 px-0 ml-2" style="float:right" @click.stop="fnButton()">
                                        <v-icon>
                                            mdi-dots-vertical
                                        </v-icon>
                                    </v-btn>
                                </v-col>
                            </v-row>
                        </v-card-title>
                        <v-card-text>
                            <h3 class="text-left page-head">
                                {{item.Description}}
                            </h3>
                        </v-card-text>
                        <v-card-subtitle>
                            <h3 class="text-left page-head">
                                Project Name : {{item.ProjectName}}
                            </h3>
                        </v-card-subtitle>
                    </v-card>
                </div>
            </v-col>

            <!-- Activity DataCollector Dialog-->
            <v-dialog v-model="ActvityDataCollectordlg" width="800" persistent>
                <v-card class="elevation-0">
                    <v-card-title style="height:50px">
                        <h5 class="page-head mr-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                            </v-avatar>
                            {{MyActivity.ActivityName}}
                        </h5>
                        <v-chip color="#4CAF50" class="ml-0" label dark small>
                            Activity
                        </v-chip>
                        <v-spacer></v-spacer>
                        <v-btn text="" @click="ActvityDataCollectordlg = false,IsUpdate = false, IsInputFinished = false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsUpdate == false && IsInputFinished == false">
                        <v-tabs grow v-model="ActivityTabs" class="pt-0 elevation-1">
                            <v-tab href='#tab-1'>Task Details</v-tab>
                            <v-tab href='#tab-2'>History</v-tab>

                            <!-- Task Details-->
                            <v-tab-item value="tab-1">

                                <v-row justify="center" class="mt-4">
                                    <v-col md="12" class="v-scrolling-div" style="height:40vh">
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Task Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3"> {{MyActivity.ActivityName}}</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyActivity.CreateDate}}</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            {{MyActivity.Description}}
                                        </h4>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyActivity.ProjectName }}</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Target
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">50</h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Periodicity
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">{{MyActivity.Periodicity}}</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-arrow-right</v-icon>
                                                </v-avatar>
                                                Pending
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                        <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="GetSections(),IsUpdate = true,QuestionStepper=1"></Secondary-Button>
                                        <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native="ActvityDataCollectordlg = false"></Default-Button-Outlined>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                            <v-tab-item value="tab-2">
                                <v-row justify="center">
                                    <v-col md="6">
                                        <v-card flat>
                                            <v-card-text style="height:300px" class="v-scrolling-div">
                                                <v-list three-line dense>
                                                    <template v-for="(item, index) in lstLogFrame">
                                                        <v-list-item>
                                                            <v-list-item-avatar color="red">
                                                                <span class="text--white">
                                                                    {{item.Name.charAt(0)}}
                                                                </span>
                                                            </v-list-item-avatar>
                                                            <v-list-item-content>
                                                                <v-list-item-title class="pb-0 pt-0">
                                                                    <v-row class="d-lg-inline-flex pb-0">
                                                                        <v-col>
                                                                            <h4 class="font-weight-regular heading-3-dark">{{item.Name}}</h4>
                                                                        </v-col>
                                                                        <v-col>
                                                                            <h4 class="font-weight-regular heading-3">{{item.CreatedDate}}</h4>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-list-item-title>
                                                                <v-list-item-subtitle>
                                                                    <h4 class="font-weight-regular heading-3-dark">{{item.Message}}</h4>
                                                                </v-list-item-subtitle>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </template>
                                                </v-list>
                                            </v-card-text>
                                        </v-card>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                        </v-tabs>
                    </v-card-text>
                    <!-- Task Questions and input feilds -->
                    <v-card-text v-show="IsUpdate == true && IsInputFinished == false" v-for="items in CurrentSectionQuestions" :key="items.SectionID">
                        <v-card>
                            <v-card-title>
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center page-head">Section {{QuestionStepper}} of {{selectionList.length}}</h4>
                                    </v-col>
                                </v-row>
                            </v-card-title>
                            <v-card-subtitle>
                                <v-row>
                                    <v-col md="auto">
                                        <h4 class="text-left page-head mb-2"> {{items.SectionName}}</h4>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col md="auto">
                                        <v-select :items="selectionList" :item-text="i => i.SectionName" :item-value="i => i.SectionID" v-model="selectedsection" hide-details dense="" outlined
                                                  @change="fnEditQuestion(selectedsection)"></v-select>
                                    </v-col>
                                </v-row>
                            </v-card-subtitle>
                            <v-card-text>
                                <v-row class="v-scrolling-div" style="max-height:50vh">
                                    <v-col>
                                        <div v-for="question in items.Questions">
                                            <!-- Text -->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Single Line Text'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" full-width outlined dense v-model="question.Answers"></v-text-field>
                                            </v-flex>
                                            <!-- Number-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Numeric'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" type="number" full-width outlined dense v-model="question.Answers"></v-text-field>
                                            </v-flex>
                                            <!-- Date-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Date'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" type="Date" full-width outlined dense v-model="question.Answers"></v-text-field>
                                            </v-flex>
                                            <!-- Multiline-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Multiline'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-textarea class="mt-3" full-width outlined dense v-model="question.Answers"></v-textarea>
                                            </v-flex>
                                            <!-- Checkbox-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Checkbox'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <ul>

                                                    <li v-for="option in question.Options">
                                                        <v-checkbox hide-details :label="option.OptionText" :id="option.OptionID"
                                                                    class="my-0" v-model="question.Answers">
                                                        </v-checkbox>
                                                    </li>
                                                </ul>
                                            </v-flex>

                                        </div>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                            <v-card-actions>
                                <v-col md="12" class="text-right">
                                    <v-divider class="mb-4"></v-divider>
                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="fnGetNextSurvey(items)"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsUpdate = false"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsInputFinished == true ">
                        <v-row>
                            <v-col cols="12" md="12" class="mt-3">
                                <h4 class="text-left page-head">In Case you have any comments please specify them below</h4>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <h4 class="text-left heading-3 mb-2">Add comments</h4>
                                <v-textarea rows="5" outlined dense label="Description" v-model="TaskDescription" type="text">
                                </v-textarea>
                                <v-file-input label="Upload Attachment" v-model="TaskAttachment" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""></v-file-input>
                            </v-col>
                        </v-row>
                        <v-row>
                            <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="SaveActivitySurvey()"></Secondary-Button>
                            <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsUpdate = true, IsInputFinished = false"></Default-Button-Outlined>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <v-snackbar v-model="SaveActivitySucessSnackBar" top="" color="primary">
               Task Updated Successfully
            </v-snackbar>

            <!-- Activity data collector Payment  linked-->
            <v-dialog v-model="PaymentActvityDataCollectordlg" width="800" persistent>
                <v-card class="elevation-0">
                    <v-card-title style="height:50px">
                        <h5 class="page-head mr-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                            </v-avatar>
                            Medical Camp
                        </h5>
                        <v-chip color="#4CAF50" class="ml-0" label dark small>
                            Activity
                        </v-chip>
                        <v-spacer></v-spacer>
                        <v-btn text="" @click="PaymentActvityDataCollectordlg = false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsUpdate == false">
                        <v-tabs grow v-model="ActivityTabs" class="pt-0 elevation-1">
                            <v-tab href='#tab-1'>Task Details</v-tab>
                            <v-tab href='#tab-2'>History</v-tab>

                            <!-- Task Details-->
                            <v-tab-item value="tab-1">

                                <v-row justify="center" class="mt-4">
                                    <v-col md="12" class="v-scrolling-div" style="height:40vh">
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Task Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Medical Camp</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">05/08/2020</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
                                        </h4>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Sample Project 1</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Target
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">50</h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Beneficiary Type
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Beneficiary</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-arrow-right</v-icon>
                                                </v-avatar>
                                                Pending
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                        <Secondary-Button class="btn-122x36 mr-2" title="Update" @click.native="IsUpdate = true"></Secondary-Button>
                                        <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native=""></Default-Button-Outlined>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                            <v-tab-item value="tab-2">
                                <v-row justify="center">
                                    <v-col md="6">
                                        <v-card flat>
                                            <v-card-text style="height:300px" class="v-scrolling-div">
                                                <v-list three-line dense>
                                                    <template v-for="(item, index) in lstLogFrame">
                                                        <v-list-item>
                                                            <v-list-item-avatar color="red">
                                                                <span class="text--white">
                                                                    {{item.Name.charAt(0)}}
                                                                </span>
                                                            </v-list-item-avatar>
                                                            <v-list-item-content>
                                                                <v-list-item-title class="pb-0 pt-0">
                                                                    <v-row class="d-lg-inline-flex pb-0">
                                                                        <v-col>
                                                                            <h4 class="font-weight-regular heading-3-dark">{{item.Name}}</h4>
                                                                        </v-col>
                                                                        <v-col>
                                                                            <h4 class="font-weight-regular heading-3">{{item.CreatedDate}}</h4>
                                                                        </v-col>
                                                                    </v-row>
                                                                </v-list-item-title>
                                                                <v-list-item-subtitle>
                                                                    <h4 class="font-weight-regular heading-3-dark">{{item.Message}}</h4>
                                                                </v-list-item-subtitle>
                                                            </v-list-item-content>
                                                        </v-list-item>
                                                    </template>
                                                </v-list>
                                            </v-card-text>
                                        </v-card>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                        </v-tabs>
                    </v-card-text>
                    <!-- Task Questions and input feilds -->
                    <v-card-text v-show="IsUpdate == true">
                        <v-card>
                            <v-card-title>
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center page-head">Section {{QuestionStepper}} of 3</h4>
                                    </v-col>
                                </v-row>
                            </v-card-title>
                            <v-card-subtitle>
                                <v-row>
                                    <v-col md="auto">
                                        <h4 class="text-left page-head mb-2"> Section Name 1</h4>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col md="auto">

                                        <v-select :items="selectionList" :item-text="i => i.SectionName" :item-value="i => i.SectionID" v-model="selectedsection" hide-details dense="" outlined
                                                  @change="fnEditQuestion(selectedsection)"></v-select>
                                    </v-col>
                                </v-row>
                            </v-card-subtitle>
                            <v-card-text>
                                <v-row class="v-scrolling-div" style="max-height:50vh">
                                    <v-col>
                                        <div v-for="question in Questions">
                                            <!-- Text -->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Text'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" full-width outlined dense></v-text-field>
                                            </v-flex>
                                            <!-- Number-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Number'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" type="number" full-width outlined dense></v-text-field>
                                            </v-flex>
                                            <!-- Date-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Date'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-text-field class="mt-3" type="Date" full-width outlined dense></v-text-field>
                                            </v-flex>
                                            <!-- Multiline-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Multiline'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <v-textarea class="mt-3" full-width outlined dense v-model="question.Answers"></v-textarea>
                                            </v-flex>
                                            <!-- Checkbox-->
                                            <v-flex xs12 sm12 md12 v-if="question.QuestionType=='Checkbox'">
                                                <label class="text-left heading-3 mb-2">{{question.QuestionName}}</label><sub v-if="question.IsMandatory==true" style="color:red;font-size:large">*</sub>
                                                <ul>

                                                    <li v-for="option in question.Options">
                                                        <v-checkbox hide-details :label="option.OptionText" :id="option.OptionID"
                                                                    class="my-0" v-model="question.Answers">
                                                        </v-checkbox>
                                                    </li>
                                                </ul>
                                            </v-flex>
                                        </div>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                            <v-card-actions>
                                <v-col md="12" class="text-right">
                                    <v-divider class="mb-4"></v-divider>
                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="IsInputFinished = true,IsUpdate = false"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsUpdate = false"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsInputFinished == true ">
                        <v-row>
                            <v-col cols="12" md="12" class="pt-0">
                                <h4 class="text-center page-head">In Case you have any comments please specify them below</h4>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <h4 class="text-left heading-3 mb-2">Add comments</h4>
                                <v-textarea rows="5" outlined dense label="Description" type="text">
                                </v-textarea>
                                <v-file-input label="Upload Attachment" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""></v-file-input>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-dialog>

            <!-- Actvity Approver dialog-->
            <v-dialog v-model="ActvityApproverdlg" width="800" persistent>
                <v-card class="elevation-0">
                    <v-card-title style="height:50px">
                        <h5 class="page-head mr-4">
                            <v-avatar color="white" size="25" class="elevation-1 mr-2 mb-1">
                                <v-icon size="15" color="primary">mdi-cash-multiple</v-icon>
                            </v-avatar>
                            Medical Camp
                        </h5>
                        <v-chip color="#4CAF50" class="ml-0" label dark small>
                            Activity
                        </v-chip>
                        <v-spacer></v-spacer>
                        <v-btn text="" @click="ActvityApproverdlg = false">
                            <v-icon color="#707070" size="15">mdi-close</v-icon>
                        </v-btn>
                    </v-card-title>
                    <!-- Task Details and History-->
                    <v-card-text v-show="IsToReview == false">
                        <v-tabs grow v-model="ActivityTabs" class="pt-0 elevation-1">
                            <v-tab href='#tab-1'>Task Details</v-tab>
                            <v-tab href='#tab-2'>History</v-tab>

                            <!-- Task Details-->
                            <v-tab-item value="tab-1">

                                <v-row justify="center" class="mt-4">
                                    <v-col md="12" class="v-scrolling-div" style="height:40vh">
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Task Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Medical Camp</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Date
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">05/08/2020</h4>
                                            </v-col>
                                        </v-row>
                                        <h4 class="font-weight-regular heading-3-dark mt-4">
                                            Task Description
                                        </h4>
                                        <h4 class="font-weight-regular heading-3">
                                            Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
                                        </h4>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Project Name
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Sample Project 1</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Target
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">50</h4>
                                            </v-col>
                                        </v-row>
                                        <v-row>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Periodicity
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Once</h4>
                                            </v-col>
                                            <v-col md="6">
                                                <h4 class="font-weight-regular heading-3-dark mt-4">
                                                    Status
                                                </h4>
                                                <v-avatar color="#FEEEC2" size="20" class="mr-1">
                                                    <v-icon size="10" color="##FCC735">mdi-tick</v-icon>
                                                </v-avatar>
                                                Updated
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" xs="12" sm="12" md="12" class="text-right">
                                        <Secondary-Button class="btn-122x36 mr-2" title="Review" @click.native="IsToReview = true"></Secondary-Button>
                                        <Default-Button-Outlined class="btn-122x36" title="Cancel" @click.native=""></Default-Button-Outlined>
                                    </v-col>
                                </v-row>
                            </v-tab-item>
                            <v-tab-item value="tab-2">

                            </v-tab-item>
                        </v-tabs>
                    </v-card-text>
                    <!-- Task Questions and Answer -->
                    <v-card-text v-show="IsToReview == true">
                        <v-card>
                            <v-card-title>
                                <v-row>
                                    <v-col cols="12" md="12" class="pt-0">
                                        <h4 class="text-center page-head">Section {{QuestionStepper}} of 3</h4>
                                    </v-col>
                                </v-row>
                            </v-card-title>
                            <v-card-subtitle>
                                <v-row>
                                    <v-col md="auto">
                                        <h4 class="text-left page-head mb-2"> Section Name 1</h4>
                                    </v-col>
                                    <v-spacer></v-spacer>
                                    <v-col md="auto">
                                        <v-select :items="selectionList" v-model="selectedsection" hide-details dense="" outlined></v-select>
                                    </v-col>
                                </v-row>
                            </v-card-subtitle>
                            <v-card-text>
                                <v-row class="v-scrolling-div" style="max-height:50vh">
                                    <v-col>
                                        <div v-for="question in EditedQuestions">
                                            <!-- Text -->
                                            <v-flex xs12 sm12 md12 class="mt-3">
                                                <label class="text-left heading-3 mb-2 mt-3">{{question.QuestionName}}</label></br>
                                                <label class="text-left heading-1 mt-3">{{question.Answer}}</label>
                                            </v-flex>
                                        </div>
                                    </v-col>
                                </v-row>
                            </v-card-text>
                            <v-card-actions>
                                <v-col md="12" class="text-right">
                                    <v-divider class="mb-4"></v-divider>
                                    <Secondary-Button class="btn-122x36 mr-2" title="Next" @click.native="IsReviewFinished = true,IsToReview = false"></Secondary-Button>
                                    <Default-Button-Outlined class="btn-122x36" title="Previous" @click.native="IsToReview = false"></Default-Button-Outlined>
                                </v-col>
                            </v-card-actions>
                        </v-card>
                    </v-card-text>
                    <!-- Document upload-->
                    <v-card-text v-show="IsReviewFinished == true ">
                        <v-row>
                            <v-col cols="12" md="12" class="pt-0">
                                <h4 class="text-center page-head">In Case you have any comments please specify them below</h4>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <h4 class="text-left heading-3 mb-2">Add comments</h4>
                                <v-textarea rows="5" outlined dense label="Description" type="text">
                                </v-textarea>
                                <v-file-input label="Upload Attachment" prepend-inner-icon="mdi-paperclip" prepend-icon="" outlined="" dense=""></v-file-input>
                            </v-col>
                        </v-row>
                        <v-col md="12">
                            <v-col md="12" class="text-right">
                                <v-divider class="mb-4"></v-divider>
                                <Secondary-Button class="btn-122x36 mr-2" title="Approve" @click.native=""></Secondary-Button>
                                <Default-Button-Outlined class="btn-122x36" title="Reject" @click.native=""></Default-Button-Outlined>
                            </v-col>
                        </v-col>
                    </v-card-text>
                </v-card>
            </v-dialog>

        </v-row>
    </v-card>
    <!--</v-col>-->
</template>
<script>
    ///  import ActivityDataCollector from '@/views/Tasks/DataCollector/ActivityDataCollector.vue'
    import SecondaryButton from '@/components/secondary-button.vue'
    import DefaultButtonOutlined from '@/components/default-button-outlined.vue'
    //import { ImWorksdb } from '../../../public/Script/Sunidhiapp.js'
    import { dbImworks } from '../../ImworksDb'

    //var objImWorksService;
    //async function importscript() {
    //    return Promise.all([
    //        import("../../../public/Script/ImWorksService.js").then(mod => {
    //            objImWorksService = new mod.ImWorksService();
    //        })
    //    ]);
    //}
    export default {
        data() {
            return {
                WorkspaceID: 0,
                lstTask: [],
                lstActivity: [],
                lstSurveyQstns: [],
                lstTasks: [],

                // ------------ Activity Data Collector
                MyTask: [],
                ActvityDataCollectordlg: false,
                ActivityTabs: 'tab-1',
                IsUpdate: false,
                QuestionStepper: 1,
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,
                selectionList: [],
                selectedsection: "section 1",
                Questions: [],
                IsInputFinished: false,
                MyActivity: {
                    ActivityId: '',
                    ActivityName: '',
                    Description: '',
                    ProjectName: '',
                    CreateDate: '',
                    BeneficiaryType: '',
                    Periodicity: 'Once',
                    Target: 50,
                    IsPaymentLined: false,
                    WorkflowId: '',
                    Status: '',
                },
                CurrentSectionQuestions: [],
                 SaveActivitySucessSnackBar : false,
                //---------------- Activity Apporover

                ActvityApproverdlg: false,
                IsToReview: false,
                IsReviewFinished: false,
                EditedQuestions: [
                    {
                        QuestionID: 1,
                        QuestionName: "What is your name ?",
                        QuestionType: 'Text',
                        IsMandatory: true,
                        Answer: "John"
                    },
                    {
                        QuestionID: 2,
                        QuestionName: "What is your age ?",
                        QuestionType: 'Number',
                        IsMandatory: true,
                        Answer: "31"
                    },
                    {
                        QuestionID: 3,
                        QuestionName: "What is your date of birth ?",
                        QuestionType: 'Date',
                        IsMandatory: true,
                        Answer: "02/08/1991"
                    },
                    {
                        QuestionID: 3,
                        QuestionName: "Where do you leave?",
                        QuestionType: 'checkbox',
                        IsMandatory: true,
                        Answer: "New Delhi"
                    }
                ],

                // Payemnt Activity
                PaymentActvityDataCollectordlg: false,
                lstLogFrame: [],
                TaskAttachment: null,
                TaskDescription: "",
                TransactionDocName: "",
            }
        },
        mounted: async function () {
          //  debugger;
            this.WorkspaceID = this.$route.query.WsID;
            //await importscript();
            //await this.GetActivity();
            await this.GetActivityTaskData();
        },
        methods: {
            fnClick(item) {
                this.Questions = [];
                this.selectionList = [];
                this.MyActivity.ActivityId = item.ActivityId;
                this.MyActivity.ActivityName = item.ActivityName;
                this.MyActivity.Description = item.Description;
                this.MyActivity.ProjectName = item.ProjectName;
                this.MyActivity.CreateDate = item.CreateDate;
                this.MyActivity.BeneficiaryType = item.BeneficiaryTypeName;
                this.MyActivity.Periodicity = "Once",
                this.MyActivity.Target = 50,
                this.MyActivity.IsPaymentLined = item.IsPaymentLined;
                this.MyActivity.WorkflowId = item.WorkflowId;
                //this.MyActivity.Status = item.Status;
                //if (this.MyActivity.Status == 'Pending') {
                    this.ActvityDataCollectordlg = true;
                //} else if (this.MyActivity.Status == 'Completed') {
                    //this.ActvityApproverdlg = true;

                //}
                this.Questions = item.SurveyQstnList

                var lstSection = [];
                const map = new Map();
                for (const item of this.Questions) {
                    if (!map.has(item.SectionID)) {
                        map.set(item.SectionID, true);
                        this.selectionList.push({
                            SectionID: item.SectionID,
                            SectionName: item.SectionName
                        });
                    }
                }

                this.lstLogFrame = this.lstLogFrame.filter(x => x.ActivityId == item.ActivityId);
            },
            fnButton() {
                alert("Button Clicked");
            },
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${day}-${month}-${year}`
            },
            fnStepperIncrement() {
                this.QuestionStepper = this.QuestionStepper + 1;
            },

            async GetActivity() {
                this.MyTask = await ImWorksdb.TbActivity.toArray();
                this.lstLogFrame = await ImWorksdb.TbAuditLog.toArray();
                this.selectionList = await ImWorksdb.TbSection.toArray();
                this.Questions = await ImWorksdb.TbQuestion.toArray();

            },

            async GetActivityTaskData() {
               // debugger;
                this.lstTask = await dbImworks.MyTaskList.toArray();
                for (var i = 0; i < this.lstTask.length; i++) {
                    if (this.lstTask[i].Type == 'Activity') {
                        var objActivity = JSON.parse(this.lstTask[i].TaskData);
                        this.lstActivity.push(objActivity);
                        this.MyTask.push({
                            ActivityId: this.lstActivity[i].ActivityId,
                            ActivityName: this.lstActivity[i].ActivityName, TaskType: 'Activity', CreatedDate: this.lstActivity[i].CreatedDate,
                            Description: this.lstActivity[i].Description, ProjectName: this.lstActivity[i].ProjectName, SurveyQstnList: this.lstActivity[i].SurveyQstnList
                        });
                   
             

                    }

                }
            },

            async GetSections() {
                this.SurveySectionList = [];
                for (var SectionCount = 0; SectionCount <= this.selectionList.length - 1; SectionCount++) {
                    var object = new Object();
                    object.SurveyId = this.selectionList[SectionCount].SurveyId;
                    object.SurveyId = this.selectionList[SectionCount].SurveyId;
                    object.SectionID = this.selectionList[SectionCount].SectionID;
                    object.SectionName = this.selectionList[SectionCount].SectionName;
                    object.SectionOrder = SectionCount + 1;
                    object.Questions = await this.GetSectionQuestions(object.SectionID)

                    this.SurveySectionList.push(object);

                }
                await this.GetCurrentSection(1);
            },
            async GetSectionQuestions(SectionID) {
                var SectionQuestionList = [];
                var SurveyQuestion = this.Questions.filter(x => x.SectionID == SectionID);
                for (var i = 0; i <= SurveyQuestion.length - 1; i++) {
                    var object = new Object();
                    object.ActivityId = this.MyActivity.ActivityId;
                    object.SurveyId = SurveyQuestion[i].SurveyID;
                    object.SectionId = SurveyQuestion[i].SectionID;
                    object.QuestionID = SurveyQuestion[i].QuestionID;
                    object.QuestionName = SurveyQuestion[i].QuestionName;
                    object.QuestionTypeID = SurveyQuestion[i].QuestionTypeID;
                    object.QuestionType = SurveyQuestion[i].QuestionType;
                    object.IsMandatory = SurveyQuestion[i].IsMandatory;

                    object.Answers = [];
                    SectionQuestionList.push(object);
                }
                return SectionQuestionList;
            },
            async GetCurrentSection(SectionOrder) {

                this.CurrentSectionQuestions = this.SurveySectionList.filter(x => x.SectionOrder == SectionOrder);
                this.selectedsection = this.CurrentSectionQuestions[0].SectionID;
            },
            async fnGetNextSurvey(item) {

                if (item.SectionOrder < this.selectionList.length) {
                    var CurrentOrder = item.SectionOrder;
                    CurrentOrder = CurrentOrder + 1;
                    this.QuestionStepper = CurrentOrder;

                    await this.GetCurrentSection(CurrentOrder);
                } else {
                    this.IsInputFinished = true,
                        this.IsUpdate = false
                }

            },
            async fnEditQuestion(sectionId) {

                var section = this.SurveySectionList.filter(x => x.SectionID == sectionId);
                await this.GetCurrentSection(section[0].SectionOrder);
                this.QuestionStepper = section[0].SectionOrder;
            },
            async SaveActivitySurvey() {
               
                if (this.TaskAttachment) {
                    this.TransactionDocName = this.TaskAttachment.name;

                }
                
                var AnswerList = [];
               
                for (var SurveyCnt = 0; SurveyCnt <= this.SurveySectionList.length - 1; SurveyCnt++) {

                    var SurveySection = this.SurveySectionList.filter(x => x.SectionID == this.SurveySectionList[SurveyCnt].SectionID);

                    for (var QstnCnt = 0; QstnCnt <= SurveySection[0].Questions.length - 1; QstnCnt++) {
                        var item = SurveySection[0].Questions[QstnCnt];

                        if (item.Answers > 0 || item.Answers.length > 0) {

                            var AnswerID = await dbImworks.TbSurveyAnswer.put({
                                Answer: item.Answers, QuestionID: item.QuestionID, SurveyID: item.SurveyId,sectionId:item.SectionId
                            });
                            AnswerList.push({ Answer: item.Answers, QuestionID: item.QuestionID, SurveyID: item.SurveyId,sectionId:item.SectionId });
                        }
                    }
                }
             
               // var s = JSON.stringify(await dbImworks.TbSurveyAnswer.toArray());
                this.lstTasks.push({ Type: 'Activity', TypeIDColumn: 'ActivityID',TypeID: this.MyActivity.ActivityId, Status: 'Completed', TaskWFStatus: 'Pending', TransactionDocName: this.TransactionDocName, TaskUpdateComment: this.TaskDescription, SurveyAnswer: AnswerList });
                var clientdata = JSON.stringify(this.lstTasks);
                var result = await dbImworks.TbSyncDB.put({
                    Type: 'Activity', TaskUpdateComment: this.TaskDescription, TypeIDColumn: 'ActivityID',
                    TypeID: this.MyActivity.ActivityId, Status: 'Completed', TaskWFStatus: 'Pending', ClientData: clientdata
                });
                
              this.ActvityDataCollectordlg =false;
              this.SaveActivitySucessSnackBar = true;
            },
            async onFileUpload(addfile, actname) {
                var lstModDate = new Date(addfile.lastModifiedDate).toISOString().substr(0, 10) + ' ' + new Date(addfile.lastModifiedDate).toISOString().substr(11, 5);
                const formData = new FormData()
                formData.append('myFile', addfile);
                formData.append('DocumentOriginalName', addfile.name.split('.').slice(0, -1).join('.'));
                formData.append('DocumentSize', this.formatFileSize(addfile.size, 0));
                formData.append('DocumentType', addfile.type);
                formData.append('DocumentTypeIcon', addfile.name.split('.').pop());
                formData.append('DocumentLastModifiedDate', lstModDate);
                formData.append('ProjectId', this.projectInfo.ProjectId);
                formData.append('UploadedFrom', "Payment");
                formData.append('LinkedTo', actname);

                var result = await objManageProjectLedgerBook.UploadDocument(formData);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.status == "success")
                        return true;
                    else
                        return false;
                }
            },
            formatFileSize(bytes, decimalPoint) {
                if (bytes == 0) return '0 Bytes';
                var k = 1000,
                    dm = decimalPoint || 2,
                    sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
                    i = Math.floor(Math.log(bytes) / Math.log(k));
                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            },

        },
        components: {
            //  'Activity-DataCollector': ActivityDataCollector,
            'Secondary-Button': SecondaryButton,
            'Default-Button-Outlined': DefaultButtonOutlined,
        },
        computed: {
            FrmDate() {
                return this.formatDate(this.startDate)
            },
        }
    }
</script>
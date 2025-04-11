<template>
    <!--<v-col cols="12" xs="12" sm="12" md="12">-->
    <v-card class="elevation-0">
        <v-row justify="center">
            <v-col md="12">
                <div>
                    <v-card class="mb-4" @click.native="fnClick()" v-for="item in MyTask" :key="item.TaskID">
                        <v-card-title>
                            <v-row no-gutters>
                                <v-col>
                                    <h3 class="text-left page-head">
                                        {{item.TaskName}}
                                    </h3>
                                </v-col>
                                <v-col>
                                    <v-chip :color="item.TypeColor" class="ml-0 mr-2" label dark small>
                                        {{item.TaskType}}
                                    </v-chip>
                                </v-col>
                                <v-col md="auto">
                                    <h1 class="page-head text-right">
                                        {{item.CreatedDate}}
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
                                {{item.TaskDescription}}
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
                    <v-card-title>
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
                        <v-btn text="" @click="ActvityDataCollectordlg = false">
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
                                                    Periodicity
                                                </h4>
                                                <h4 class="font-weight-regular heading-3">Once</h4>
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
                                        <v-select :items="selectionList" v-model="selectedsection" hide-details dense="" outlined></v-select>
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
    export default {
        data() {
            return {
                WorkspaceID: 0,

                // ------------ Activity Data Collector
                MyTask: [
                    {
                        TaskID : 1,
                        TaskName: 'Rounds KPI',
                        TaskType: 'KPI',
                        TypeColor: '#FF5722',
                        TaskDescription: 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the',
                        ProjectName: 'Sample Project 1',
                        CreatedDate: '15/08/2020',
                    },
                ],
                ActvityDataCollectordlg: false,
                ActivityTabs: 'tab-1',
                IsUpdate: false,
                QuestionStepper: 1,
                startDate: new Date().toISOString().substr(0, 10),
                FrmDateMenu: false,
                selectionList: ["section 1", "section 2", "section 3"],
                selectedsection: "section 1",
                Questions: [
                    {
                        QuestionID: 1,
                        QuestionName: "What is your name ?",
                        QuestionType: 'Text',
                        IsMandatory: true,
                        Answer:""
                    },
                    {
                        QuestionID: 2,
                        QuestionName: "What is your age ?",
                        QuestionType: 'Number',
                        IsMandatory: true,
                        Answers:""
                    },
                    {
                        QuestionID: 3,
                        QuestionName: "What is your Date of Birth",
                        QuestionType: 'Date',
                        IsMandatory: true,
                        Answers:""
                    },
                    {
                        QuestionID: 4,
                        QuestionName: "What is your current Address?",
                        QuestionType: 'Multiline',
                        IsMandatory: false,
                        Answers: ""
                    },
                    {
                        QuestionID: 5,
                        QuestionName: "Where do you leave?",
                        QuestionType: 'Checkbox',
                        IsMandatory: false,
                        Answers: "",
                        Options: [{
                            OptionID: 1,
                            OptionText:"New Delhi"
                        },
                            {
                                OptionID: 2,
                                OptionText: "Mumbai"
                            },
                            {
                                OptionID: 3,
                                OptionText: "Banglure"
                            },
                            {
                                OptionID: 4,
                                OptionText: "Others"
                            }
                        ]
                    }
                ],
                IsInputFinished: false,

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
                ]
            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
        },
        methods: {
            fnClick() {
               // this.ActvityDataCollectordlg = true;
                this.ActvityApproverdlg = true;
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
            }
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
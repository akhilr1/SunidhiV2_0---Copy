<template>
    <v-card flat color="#F5F6F8" height="90vh">
      <v-card-title class="titleBar">
        Activity Log
        <v-spacer />
        <v-btn color="primary2" icon @click="getActivityLogMap" :loading="isLoading" :disabled="isLoading"><v-icon>mdi-refresh</v-icon></v-btn>
      </v-card-title>

      <v-card-text class="activityContainer v-scrolling-div">
        <v-card width="100%" class="card" v-for="logItem in syncTaskLog">
          <v-card-title>
            <template v-if="logItem.WorkflowStageStatus === 'Approved' && logItem.isBulk">
              <v-img :src="ApprovalSign" :max-width="35" contain class="mr-2"></v-img>
              <v-img :src="LayersIcon" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has approved the survey submittedby <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <template v-if="logItem.WorkflowStageStatus === 'Rejected' && logItem.isBulk">
              <v-img :src="RejectionSign" :max-width="35" contain class="mr-2"></v-img>
              <v-img :src="LayersIcon" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has Rejected the survey submittedby <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <template v-else-if="logItem.WorkflowStageStatus === 'Approved'">
              <v-img :src="ApprovalSign" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has approved the survey submittedby <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <template v-else-if="logItem.WorkflowStageStatus === 'Rejected'">
              <v-img  :src="RejectionSign" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has Rejected the survey submittedby <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <template v-else-if="logItem.WorkflowStageStatus === 'Submitted'">
              <v-img  :src="SubmissionIcon" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has Submitted a Survey to <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <template v-else-if="logItem.WorkflowStageStatus === 'Assigned'">
              <v-img  :src="SendSign" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="userName">{{ logItem.SubmittedBy }}</span> has Sent a Survey to <span class="userName">{{ logItem.DefaultText }}</span></p>
            </template>
            <!-- <template v-else>
              <v-img  :src="SubmissionIcon" :max-width="35" contain class="mr-2"></v-img>
              <p class="titleBase mb-0"><span class="titleBase userName">{{ logItem.SubmittedBy }}</span> has Sent a Survey to <span class="titleBase">{{ logItem.DefaultText }}</span></p>
            </template> -->
          </v-card-title>
          <v-card-text>
            <div v-if="logItem.Comments" class="subCard mb-3">
              {{ logItem.Comments }}
            </div>
            <div class="mx-1">
              <p class="mb-0">{{ new Date(logItem.SubmissionDate).toLocaleString('en-IN') }}</p>
            </div>
          </v-card-text>
        </v-card>
      </v-card-text>

      <!-- snackbar -->
      <v-snackbar top v-model="snackbar.value" :color="snackbar.color">
        {{ snackbar.message }}
      </v-snackbar>
    </v-card>
</template>

<script>
import TaskBulkActions from '../../BL/TaskBulkActions';
export default {
  props: {
    syncTaskIDList: {
      type: Array,
      required: true,
      default: () => [],
    }
  },
  data() {
    return {
      TaskBulkController: new TaskBulkActions(),
      syncTaskLog: [],
      ApprovalSign: require('@/assets/ActivityLog/ApprovalSign.svg'),
      SendSign: require('@/assets/ActivityLog/SendSign.svg'),
      RejectionSign: require('@/assets/ActivityLog/RejectionSign.svg'),
      LayersIcon: require('@/assets/ActivityLog/LayersIcon.svg'),
      SubmissionIcon: require('@/assets/ActivityLog/SubmissionIcon.svg'),
      isLoading: false,
      snackbar: {
        value: false,
        message: '',
        color: '',
      }
    }
  },
  async mounted(){
    if(this.syncTaskIDList.length) {
      this.syncTaskLog = await this.getActivityLogMap(this.syncTaskIDList)
    }
    console.log(this.syncTaskLog)
  },
  methods:{
    async getActivityLogMap() {
      try {
        this.isLoading = true
        let data = []
        const result = await this.TaskBulkController.getTaskActivityLog(this.syncTaskIDList)
        if (result) {
          data.push(...result.data)
        }

        data.sort((a, b) => {
          return new Date(b.SubmissionDate) - new Date(a.SubmissionDate);
        })
        return data
      } catch (error) {
        console.error(error)
        this.snackbar = {
          value: true,
          message: 'An error occured',
          color: 'error',
        }
        return false
      } finally {
        setTimeout(() => {
          this.isLoading = false
        }, 1000)
      }
    }
  },
};
</script>

<style lang="scss" scoped>
.card{
  background: #FFFFFF;
  box-shadow: 0px 0px 4px rgba(0, 0, 0, 0.16);
  border-radius: 10px !important;
  margin-bottom: 1rem;
}
.subCard {
  background: #EBF5FF;
  border-radius: 8px;
  padding: 8px;
}

.activityContainer{
  padding-top: 5px;
  height: 90%;
}

.titleBar {
  background-color: white;
  margin: 0 -1rem;
  margin-bottom: 0.75rem;
  padding: 6px 2rem;
}

.titleBase {
  font-size: 16px;
  line-height: 24px;
  color: #374151;
  font-family: 'Poppins';
  letter-spacing: 0.0015em;
  
  .userName {
    font-weight: 600;
    text-transform: capitalize;
  }
}
</style>
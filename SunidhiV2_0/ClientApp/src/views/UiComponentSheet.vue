<template>
 <v-container fluid>
   <v-row>
     <v-col>
       <v-card>
         <v-card-title>Button styles</v-card-title>
         <v-card-text>
           <p>
            Primary button
            <Primary-Button title="Label"></Primary-Button>
           </p>
           <p>
             Secondary button
            <Secondary-Button title="label"></Secondary-Button>
           </p>
           <p>
             Success Button
             <Success-Button title="label"></Success-Button>
           </p>
           <p>
             Default button outlined
             <Default-Button-Outlined title="Lable"></Default-Button-Outlined>
           </p>
           <p>
             Primary button outlined
             <Primary-Button-Outlined title="Label"></Primary-Button-Outlined>
           </p>
           <p>
             Success button large
             <Success-Button-Lg title="Label"></Success-Button-Lg>
           </p>
           <p>
             Success button Small
             <Success-Button-Sm title="Label"></Success-Button-Sm>
           </p>
           <p>
             Dropdown button default oulined
             <Default-Dropdown-Button-Outlined title="Label"></Default-Dropdown-Button-Outlined>
           </p>
           <p>
             Fab button light small
             <Fab-Button-Light-Small icon="mdi-pencil"></Fab-Button-Light-Small>
           </p>
           <p>
             Button light 24x24
             <Button-Light-24x24 icon="mdi-pencil"></Button-Light-24x24>
           </p>
           <p>
             <add-tag></add-tag>
           </p>
           <p>
             Icon button outlined
             <Default-Icon-Button-Outlined icon="mdi-account"></Default-Icon-Button-Outlined>
           </p>
         </v-card-text>
       </v-card>
     </v-col>
     <v-col>
        <v-card>
          <v-card-title>Button styles</v-card-title>
          <v-card-text>
            <p>
              Assign users button green
              <Assign-Users-Button-Success></Assign-Users-Button-Success>
            </p>
            <p>
              Action button green
              <Action-Button-Success></Action-Button-Success>
            </p>
            <p>
              Save button green small
              <Save-Button-Success-Small></Save-Button-Success-Small>
            </p>
            <p>
              Create workspace button
              <Create-Workspace-Button></Create-Workspace-Button>
            </p>
            <p>
              View workflow button
              <View-Workflow-Button></View-Workflow-Button>
            </p>
            <p>
              View all button
              <View-All-Button></View-All-Button>
            </p>
            <p>
              Create workspace button plus
              <Create-Workspace-Button-Plus></Create-Workspace-Button-Plus>
            </p>
          </v-card-text>
        </v-card>
     </v-col>
     <v-col>
        <v-card>
          <v-card-title>Button styles</v-card-title>
          <v-card-text>
            <p>
              Assign users button green
              <Assign-Users-Button-Success></Assign-Users-Button-Success>
            </p>
            <p>
              View template button
              <View-Template-Button></View-Template-Button>
            </p>
            <p>
              Save button success large
              <Save-Button-Success-Large></Save-Button-Success-Large>
            </p>
            <p>
              Manage workspace button primary
              <Manage-Workspace-Button-Primary></Manage-Workspace-Button-Primary>
            </p>
            <p>
              Primary button rounded
              <Primary-Button-Rounded></Primary-Button-Rounded>
            </p>
            <p>
              Add button fab
              <Add-Button-Fab></Add-Button-Fab>
            </p>
          </v-card-text>
        </v-card>
     </v-col>
     <v-responsive width="100%"></v-responsive>
     <v-col>
       <v-card>
         <v-card-title>Button styles</v-card-title>
         <v-card-text>
           <p>
             Create button green
             <Creat-Button-Green-Small></Creat-Button-Green-Small>
           </p>
           <p>
             Add tag button
             <Add-Tag-Button></Add-Tag-Button>
           </p>
           <p>
             Create button blue
             <Creat-button-primary-light class="mt-2"></Creat-button-primary-light>
           </p>
           <p>
             Next button blue
             <Next-button-primary-light class="mt-2"></Next-button-primary-light>
           </p>
           <p>
             Done button
             <Done-Button-Success class="mt-2"></Done-Button-Success>
           </p>
           <p>
             Enter button
             <Enter-Button-Success class="mt-2"></Enter-Button-Success>
           </p>
           <p>
             Manage button
             <Manage-Button-Normal class="mt-2"></Manage-Button-Normal>
           </p>
         </v-card-text>
       </v-card>
     </v-col>
     <v-col>
       <v-card>
         <v-card-title>Button styles</v-card-title>
         <v-card-text>
           <p>
             Tune button
             <Tune-Button-Outlined></Tune-Button-Outlined>
           </p>
           <p>
             Cancel button outlined small
             <Cancel-Button-Outlined-Sm></Cancel-Button-Outlined-Sm>
           </p>
           <p>
             Cancel button outlined Large
             <Cancel-Button-Outlined-Lg></Cancel-Button-Outlined-Lg>
           </p>
           <p>
             Previous button outlined
             <Previous-Button-Outlined></Previous-Button-Outlined>
           </p>
           <p>
             Filter button
             <Filter-Button-Outlined></Filter-Button-Outlined>
           </p>
           <p>
             Bulk action button
             <Bulk-Action-Button-Outlined></Bulk-Action-Button-Outlined>
           </p>
           <p>
             Outlined router button
             <Outlined-Route-Button-Rounded-Avatar :to="{name:'home'}"></Outlined-Route-Button-Rounded-Avatar>
           </p>
         </v-card-text>
       </v-card>
     </v-col>
     <v-col>
       <v-card>
         <v-card-title>Dropdown menus</v-card-title>
         <v-card-text>
           <p>
             Dropdown white menu large
             <Dropdown-Menu-White-Lg></Dropdown-Menu-White-Lg>
           </p>
           <p>
             Success button Medium
             <Success-Button-Md title="Save"></Success-Button-Md>
           </p>
           <p>
             Outlined button
             <Outlined-Button-Dark title="Outlined button"></Outlined-Button-Dark>
           </p>
         </v-card-text>
       </v-card>
     </v-col>
   </v-row>
 </v-container>
</template>

<script>
  import DefaultIconButtonOutlined from'@/components/default-icon-button-outlined.vue'
  import AddTag from'@/components/add-tag.vue'
  import PrimaryButton from'@/components/primary-button-normal.vue'
  import SecondaryButton from'@/components/secondary-button.vue'
  import SuccessButton from'@/components/success-button.vue'
  import DefaultButtonOutlined from'@/components/default-button-outlined.vue'
  import PrimaryButtonOutlined from'@/components/primary-button-outlined.vue'
  import SuccessButtonLg from'@/components/success-button-lg.vue'
  import SuccessButtonSm from'@/components/success-button-sm.vue'
  import DefaultDropdownButtonOutlined from'@/components/default-dropdown-button-outlined.vue'
  import FabButtonLightSmall from'@/components/fab-button-light-small.vue'
  import ButtonLight24x24 from'@/components/button-light-24x24.vue'


  import CreatButtonGreenSmall from'@/components/create-button-green-small.vue'
  import AddTagButton from'@/components/add-tag-button.vue'
  import CreatButtonPrimaryLight from'@/components/create-button-blue.vue'
  import NextButtonPrimaryLight from'@/components/next-button-blue.vue'
  import DoneButtonSuccess from'@/components/done-button-success.vue'
  import EnterButtonSuccess from'@/components/enter-button-success.vue'
  import ManageButtonNormal from'@/components/manage-button-normal.vue'
  import AssignUsersButtonSuccess from'@/components/assign-users-button-success.vue'
  import ActionButtonSuccess from'@/components/action-button-success.vue'
  import SaveButtonSuccessSmall from'@/components/save-button-success-small.vue'
  import CreateWorkspaceButton from'@/components/create-workspace-button.vue'
  import ViewWorkflowButton from'@/components/view-workflow-button.vue'
  import ViewAllButton from'@/components/view-all-button.vue'
  import CreateWorkspaceButtonPlus from'@/components/create-workspace-button-plus.vue'
  import ViewTemplateButton from'@/components/view-template-button.vue'
  import SaveButtonSuccessLarge from'@/components/save-button-success-large.vue'
  import ManageWorkspaceButtonPrimary from'@/components/manage-workspace-button-primary.vue'
  import PrimaryButtonRounded from'@/components/primary-button-rounded.vue'
  import AddButtonFab from'@/components/add-button-fab.vue'
  import TuneButtonOutlined from'@/components/tune-button-outlined.vue'
  import CancelButtonOutlinedSm from'@/components/cancel-button-outlined-sm.vue'
  import CancelButtonOutlinedLg from'@/components/cancel-button-outlined-lg.vue'
  import PreviousButtonOutlined from'@/components/previous-button-outlined.vue'
  import FilterButtonOutlined from'@/components/filter-button-outlined.vue'
  import OutlinedRouteButtonRoundedAvatar from'@/components/outlined-route-button-rounded-avatar.vue'
  import DropdownMenuWhiteLg from'@/components/dropdown-menu-white-lg.vue'
  import BulkActionButtonOutlined from'@/components/bulk-action-button-outlined.vue'
  import SuccessButtonMd from'@/components/success-button-md.vue'
  import PrimaryButtonLight from'@/components/primary-button-light.vue'
  import OutlinedButtonDark from'@/components/outlined-button-dark.vue'




  export default {

  components: {
  'Default-Icon-Button-Outlined':DefaultIconButtonOutlined,
  'add-tag':AddTag,
  'Primary-Button':PrimaryButton,
  'Secondary-Button':SecondaryButton,
  'Default-Button-Outlined':DefaultButtonOutlined,
  'Success-Button':SuccessButton,
  'Primary-Button-Outlined':PrimaryButtonOutlined,
  'Success-Button-Lg':SuccessButtonLg,
  'Success-Button-Sm':SuccessButtonSm,
  'Default-Dropdown-Button-Outlined':DefaultDropdownButtonOutlined,
  'Fab-Button-Light-Small':FabButtonLightSmall,
  'Button-Light-24x24':ButtonLight24x24,


  'Creat-Button-Green-Small':CreatButtonGreenSmall,
  'Add-Tag-Button':AddTagButton,
  'Creat-button-primary-light': CreatButtonPrimaryLight,
  'Next-button-primary-light': NextButtonPrimaryLight,
  'Done-Button-Success': DoneButtonSuccess,
  'Enter-Button-Success': EnterButtonSuccess,
  'Manage-Button-Normal':ManageButtonNormal,
  'Assign-Users-Button-Success':AssignUsersButtonSuccess,
  'Action-Button-Success':ActionButtonSuccess,
  'Save-Button-Success-Small':SaveButtonSuccessSmall,
  'Create-Workspace-Button':CreateWorkspaceButton,
  'View-Workflow-Button':ViewWorkflowButton,
  'View-All-Button':ViewAllButton,
  'Create-Workspace-Button-Plus':CreateWorkspaceButtonPlus,
  'View-Template-Button':ViewTemplateButton,
  'Save-Button-Success-Large':SaveButtonSuccessLarge,
  'Manage-Workspace-Button-Primary':ManageWorkspaceButtonPrimary,
  'Primary-Button-Rounded':PrimaryButtonRounded,
  'Add-Button-Fab':AddButtonFab,
  'Tune-Button-Outlined':TuneButtonOutlined,
  'Cancel-Button-Outlined-Sm':CancelButtonOutlinedSm,
  'Cancel-Button-Outlined-Lg':CancelButtonOutlinedLg,
  'Previous-Button-Outlined':PreviousButtonOutlined,
  'Filter-Button-Outlined':FilterButtonOutlined,
  'Outlined-Route-Button-Rounded-Avatar':OutlinedRouteButtonRoundedAvatar,
  'Dropdown-Menu-White-Lg':DropdownMenuWhiteLg,
  'Bulk-Action-Button-Outlined':BulkActionButtonOutlined,
  'Success-Button-Md':SuccessButtonMd,
  'Primary-Button-Light':PrimaryButtonLight,
  'Outlined-Button-Dark':OutlinedButtonDark,
  },
  data: function(){
  return{
  }
  },

  }
</script>
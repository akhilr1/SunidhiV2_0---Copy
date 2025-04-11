<template>
    <v-row>
        <v-col class="pt-0">
            <FinanceActivity></FinanceActivity>
        </v-col>
        <v-responsive width="100%"></v-responsive>
        <v-col>
            <FinanceWorkflow></FinanceWorkflow>
        </v-col>
    </v-row>
</template>
<script>
    import FinanceActivity from '@/views/Finance/ManageFinanceActivity.vue'
    import FinanceWorkflow from '@/views/Finance/ManageFinanceWorkflow.vue'
    export default {
        data() {
            return {
            }
        },
        components: {
            'FinanceActivity': FinanceActivity,
            'FinanceWorkflow': FinanceWorkflow,
        },

    }
</script>
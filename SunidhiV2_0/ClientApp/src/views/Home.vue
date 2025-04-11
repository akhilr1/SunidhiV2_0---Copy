<template>
    <div class="home" v-if="loggedUser">
        <!--<h2>Home Page - Dashboard</h2>-->
        <AdminDashboard></AdminDashboard>
    </div>
</template>
<script>
    import AdminDashboard from "@/views/Dashboard/AdminDashboard.vue"

    export default {
        data() {
            return {
                loggedUser: false,
            }
        },
        methods: {
            getLoggedUser(){
                this.loggedUser = sessionStorage.getItem('LoggedUserID')
            }
        },
        mounted() {
            this.getLoggedUser();
        },
        components: {
            'AdminDashboard': AdminDashboard,
        }
    }
</script>

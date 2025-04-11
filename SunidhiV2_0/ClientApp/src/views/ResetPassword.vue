<template>
    <v-row class="" style="height: 95vh">
        <v-card style="height:fit-content" class="my-auto" elevation="10">
            <v-card-title>Reset Password</v-card-title>
            <v-card-text class="gap-5">
                <v-form ref="passwordForm" lazy-validation>
                    <v-row>
                        <v-tooltip bottom open-delay="1000" close-delay="300" color="primary" dark>
                            <template v-slot:activator="{on, attrs}">
                                <v-col cols="12" class="py-0">
                                    <v-text-field
                                        v-bind="attrs"
                                        v-on="on" 
                                        :rules="[
                                                passwordRules.required, 
                                                passwordRules.min, 
                                                passwordRules.max, 
                                                passwordRules.passwordValidation
                                            ]" 
                                        outlined 
                                        hide-details 
                                        label="Enter Password" 
                                        v-model="password"
                                        type="password"
                                    >
                                    </v-text-field>
                                </v-col>
                            </template>
                            <span class="pa-2">
                                <ul>
                                    <li>The Password must be 8 - 32 characters long</li>
                                    <li>The Password must have atleast 1 Number</li>
                                    <li>The Password must have atleast 1 Capital Letter</li>
                                    <li>The Password must have atleast 1 Special Character</li>
                                </ul>
                            </span>
                        </v-tooltip>
                        <v-col cols="12">
                            <v-text-field :rules="confirmRules" outlined hide-details label="Confirm Password" v-model="cnf_password" type="password"></v-text-field>
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions class="d-flex justify-end mr-3 pb-5">
                <v-btn x-large @click="submitHandler" color="blue" dark>Submit</v-btn>
                <v-btn x-large @click="clearAll()">Clear</v-btn>
            </v-card-actions>
        </v-card>

        <v-snackbar app v-model="snackStatus.value" :color="snackStatus.type">
            {{snackStatus.message}}
        </v-snackbar>
    </v-row>
</template>

<script>
var User
async function importscript() {
    return Promise.all([
        import("../BL/User.js").then(mod => {
            User = new mod.User();               
        })
    ]);
}
export default {
    data() {
        return {
            key: String,
            password: '',
            cnf_password: '',
            passwordRules: {
                required: val => !!val || 'Required',
                min: val => (val && val.length >= 8) || 'Your password must have at least 8 characters',
                max: val => (val && val.length <= 32) || 'Your password must not have more than 32 characters',
                passwordValidation: val => (val && /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{0,}$/g.test(val)) || 'Your password must have 1 Capital letter, 1 Number and 1 Special character'
            },
            confirmRules: [
                val => val === this.password || 'Your passwords must match'
            ],
            snackStatus: {
                value: false,
                message: '',
                type: ''
            }
        }    
    },
    async mounted() {
        await importscript()
        this.key = this.$route.query.key
    },
    methods: {
        async submitHandler() {
            if(this.$refs.passwordForm.validate()){

                try {
                    let result = await User.ResetPassword({
                        password: Buffer.from(this.password).toString('base64'), 
                        key: this.key
                    })
                    if(!result.status){
                        throw new Error(result.statusMsg)
                    }
                    else if(result.status) {
                        this.snackStatus = {
                            value: true,
                            message: "Password reset successfully",
                            type: "success"
                        }
                        setTimeout(() => {
                            this.$router.push({ name : "Login"})
                        }, 1000);
                    }
                } catch (error) {
                    console.error(error)
                }

            }
            else {
                this.snackStatus = {
                    value: true,
                    message: "Please enter all the Password fields",
                    type: 'error'
                }
            }
        },
        clearAll() {
            this.password = '',
            this.cnf_password = ''
            this.snackStatus = {
                value : true,
                message: 'Fields Cleared',
                type: "success"
            }
        }
    }
}
</script>
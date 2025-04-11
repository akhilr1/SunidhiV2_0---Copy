<template>
    <v-card class="elevation-0 pa-0">
        <v-card-title style="height:73px">
            <h5 class="page-head">Audit Frame</h5>
        </v-card-title>
        <v-card-text>
            <v-row justify="center">
            <v-col md="8" class="py-0">
                <v-card flat class="py-0">
                    <v-card-text style="height:360px" class="v-scrolling-div">
                        <v-list  dense class="py-0">
                            <template v-for="(item, index) in lstLogFrame">
                                <v-list-item class="py-0">
                                    <v-list-item-icon>
                                        <v-avatar size="40" :color="`${getcolor(item.Name)}`">
                                            <img v-bind:src="userimage + item.ProfileImage" alt="User DP" v-if="item.ProfileImage.length >0" />
                                            <span class="white--text" v-else>{{item.Name == null ? "" : item.Name.substring(0, 1).toUpperCase()}}</span>
                                        </v-avatar>
                                    </v-list-item-icon>
                                    <v-list-item-content>
                                        <v-list-item-title  class="py-0 pb-0">
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
                                    </v-list-item-content>
                                </v-list-item>
                            </template>
                        </v-list>
                    </v-card-text>
                </v-card>
            </v-col>
           </v-row>
        </v-card-text>
    </v-card>
       
</template>
<script>

    import objUtils from '../../utils.js'

    var objFinance;
    async function importMyFinancescript() {
        return Promise.all([
            import("../../BL/Finance.js").then(mod => {
                objFinance = new mod.Finance();
            })
        ]);
    }
    export default {

        data() {
            return {
                WorkspaceID: 0,
                ProjectID:0,
                lstLogFrame: [],
                userimage: "",
            }
        },
        mounted: async function () {
            this.WorkspaceID = this.$route.query.WsID;
            this.ProjectID = this.$route.query.ProjID;
            this.userimage = window.SERVER_URL + "/Upload";
            await importMyFinancescript();
            await this.fnGetAuditLog();
        },
        methods: {

            // <summary>
            /// getcolor :- Function used to get color of users
            /// </summary>
            /// <param name="name"></param>
            /// <returns> </returns>
            getcolor(name) {
                return objUtils.common.getcolor(name);
            },

            // <summary>
            /// fnGetAuditLog :- Function used to fetch Audit Log Details based on ProjectID
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            async fnGetAuditLog() {
               
                var data = new FormData();
                data.append("ProjectId", this.ProjectID);
                var result = await objFinance.GetAuditLog(data);

                if (result.status == "Unhandled Exception")
                    this.$router.push({ name: "error" });
                else {
                    if (result.data) {
                        this.lstLogFrame = result.data;
                    }
                }
            },

            // <summary>
            /// getcolor :- Function used to format date and time
            /// </summary>
            /// <param name="date"></param>
            /// <returns> </returns>
            formatDateAndTime(date) {
                return objUtils.common.formatDateAndTime(date);
            },
        }
    }
</script>
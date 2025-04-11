<template>
    <v-card style="height:490px" elevation="1">
        <v-row class="pt-1">
            <v-col md="auto" class="pt-5">
                <h1 class="page-head-dark font-weight-medium pl-4">
                    <!--<v-avatar color="white" size="20" class="elevation-1 mr-2 mb-1">
                    <v-icon size="15" color="primary">mdi-bell-outline</v-icon>
                </v-avatar>-->
                    Notifications
                </h1>
            </v-col>
            <v-spacer></v-spacer>
            <v-col md="auto">
                <v-select dense
                          label="Today"
                          outlined hide-details style="max-width:150px" class="mr-2"></v-select>
            </v-col>
        </v-row>
        <v-divider></v-divider>
        <v-row>
            <v-col md="12" cols="12" class="pt-1">
                <div class="v-scrolling-div pa-0" style="height:416px;">
                    <v-list three-line>
                        <template v-for="(item, index) in items">
                            <v-list-item :key="item.title">
                                <v-list-item-avatar color="blue lighten-5">
                                    <v-icon color="#0099FF" dark outline>
                                        {{item.icon}}
                                    </v-icon>
                                </v-list-item-avatar>

                                <v-list-item-content>
                                    <v-list-item-title v-html="item.title"></v-list-item-title>
                                    <v-list-item-subtitle v-html="item.subtitle"></v-list-item-subtitle>
                                </v-list-item-content>
                            </v-list-item>
                            <v-divider></v-divider>
                        </template>
                    </v-list>
                </div>
            </v-col>
        </v-row>
    </v-card>
        
</template>
<script>
    
    export default {
        data() {
            return {
                items: [
                    {
                        icon: 'mdi-folder-outline',
                        title: 'Subscription',
                        subtitle: 'Your Subscription about to expire in 3 days',
                    },
                    {
                        icon: 'mdi-settings-outline',
                        title: 'IN-Southern Region',
                        subtitle: `<span class="text--primary">to Alex, Scott, Jennifer</span> &mdash; Wish I could come, but I'm out of town this weekend.`,
                    },
                    {
                        icon: 'mdi-account-outline',
                        title: 'Event Planning checklist',
                        subtitle: '<span class="text--primary">Sandra Adams</span> &mdash; Do you have Paris recommendations? Have you ever been?',
                    },
                    {
                        icon: 'mdi-google-circles-extended',
                        title: 'Birthday gift',
                        subtitle: '<span class="text--primary">Trevor Hansen</span> &mdash; Have any ideas about what we should get Heidi for her birthday?',
                    },
                    {
                        icon: 'mdi-folder-outline',
                        title: 'Recipe to try',
                        subtitle: '<span class="text--primary">Britta Holt</span> &mdash; We should eat this: Grate, Squash, Corn, and tomatillo Tacos.',
                    },
                ],
            }
        },
        components: {
            
        },
    }
</script>
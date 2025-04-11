<template>
    <v-card style="height:490px" elevation="1">
        <v-row class="pt-1">
            <v-col class="pt-5">
                <h1 class="page-head-dark font-weight-medium pl-4">
                    Help Documentation
                </h1>
            </v-col>
        </v-row>
        <v-divider></v-divider>
        <v-row>
            <v-col md="12" cols="12" class="pt-1">
                <div class="v-scrolling-div" style="height:420px;">
                    <v-list three-line>
                        <template v-for="(item, index) in items">
                            <v-list-item :key="item.title">
                                <v-list-item-avatar rounded tile color="blue lighten-5">
                                    <v-icon color="#0099FF" dark outline>
                                        {{item.icon}}
                                    </v-icon>
                                </v-list-item-avatar>

                                <v-list-item-content>
                                    <v-list-item-title v-html="item.name"></v-list-item-title>
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
                    { name: 'Getting Started', icon: 'mdi-format-list-bulleted-square', subtitle: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus condimentum leo est, in bibendum nisi dignissim nec.' },
                    { name: 'Getting Started', icon: 'mdi-format-list-bulleted-square', subtitle: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus condimentum leo est, in bibendum nisi dignissim nec.' },
                    { name: 'Getting Started', icon: 'mdi-format-list-bulleted-square', subtitle: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus condimentum leo est, in bibendum nisi dignissim nec.' },
                    { name: 'Getting Started', icon: 'mdi-format-list-bulleted-square', subtitle: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus condimentum leo est, in bibendum nisi dignissim nec.' },
                    { name: 'Getting Started', icon: 'mdi-format-list-bulleted-square', subtitle: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus condimentum leo est, in bibendum nisi dignissim nec.' },
                ],
            }
        },
        components: {

        },
    }
</script>
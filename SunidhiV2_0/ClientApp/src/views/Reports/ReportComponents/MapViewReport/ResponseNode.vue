<template>
    <v-list-item class="preactive" ref="coord_list_item" @click="toggleResponseWindow(data)">
        <v-list-item-icon>
            <v-icon color="white" small class="navigation-icon">
                mdi-navigation
            </v-icon>
        </v-list-item-icon>
        <v-list-item-content>
            <v-list-item-title>
            {{formatCoords(data.Geo_coordinates)}}
            </v-list-item-title>
            <v-list-item-subtitle class="mt-2">
                <span class="mr-2"><v-icon small> mdi-calendar-blank-outline </v-icon> {{createdDateTime(data.CreatedDate, 'date')}}</span>
                <span class="ml-1"><v-icon small> mdi-clock-outline </v-icon> {{createdDateTime(data.CreatedDate, 'time')}}</span>
            </v-list-item-subtitle>
        </v-list-item-content>
        <v-list-item-action>
            <v-btn icon small class="toggle-btn">
                <v-icon small color="black">
                    {{ responseToggle ? 'mdi-close' : 'mdi-chevron-right' }}
                    <!-- {{'mdi-chevron-right' }} -->
                </v-icon>
            </v-btn>
        </v-list-item-action>
    </v-list-item>
</template>

<script>
import { createNamespacedHelpers } from "vuex";
const { mapGetters, mapActions } = createNamespacedHelpers("MapReport");
export default {
    props: ['data'],
    data() {
        return {
            responseToggle: false
        }
    },
    computed:{
        ...mapGetters([
            "GeoDataList",
        ])
    },
    mounted() {
        this.$bus.$on('closeResponseWindow', () => {
            this.responseToggle = false
            this.$refs.coord_list_item?.$el.classList.remove('active')
        })
        this.$bus.$on('openResponseWindow', (val) => {
            let d = val.getPosition().toString().replace(/\s/g, "");
            let data = d.slice(1, d.length-1)
            if(data == this.data.Geo_coordinates) {
                this.toggleResponseWindow(this.data)
            }
        })
    },
    methods: {
        ...mapActions([
            "fnSetCardState"
        ]),
        formatCoords(i) {
            let dat = i.split(",");
            return `${dat[0]}°N, ${dat[1]}°E`;
        },
        createdDateTime(i, x) {
            let dateObj = new Date(i);
            if(x == 'date'){
            let date = `${dateObj.getDate()}/${dateObj.getMonth() + 1}/${dateObj.getFullYear()}`
            return date
            }else{
            let Time = `${dateObj.getHours()}:${dateObj.getMinutes()} ${dateObj.getHours() > 12 ? "PM" : "AM"}`;
            return Time
            }
        },
        toggleResponseWindow(item){
            this.responseToggle = !this.responseToggle;
            this.responseToggle ?  this.$refs.coord_list_item?.$el.classList.add('active') : this.$refs.coord_list_item?.$el.classList.remove('active')
            this.fnSetCardState({
                ...item,
                type: 'leaf'
            })
            this.$bus.$emit('toggleResponseWindow', item)
            this.$store.commit('MapReport/CALL_CARD', null)
        }
    }
}
</script>

<style scoped>
    .navigation-icon{
        background: #399cff;
        border-radius: 50%;
        padding: 5px;
    }
    .preactive:hover{
        background: #EBF5FF;
        border-radius: 8px;
    }
    .active{
        background: #EBF5FF;
        border-radius: 8px;
    }
</style>
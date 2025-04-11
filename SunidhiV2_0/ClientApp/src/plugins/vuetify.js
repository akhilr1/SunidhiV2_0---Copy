import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        themes: {
            light: {
                primary2: '#399CFF',
                primary: '#3374B9',
                secondary: '#439BDE',
                accent: '#C8E9FF',
                error: '#ff5252',
                success:"#52B962"
            },
        },
    },
});

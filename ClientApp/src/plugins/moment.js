import Vue from 'vue'
import moment from 'moment';

Plugin.install = function (Vue, /*options*/) {
    Vue.moment = moment;
    window.axios = moment;
    Object.defineProperties(Vue.prototype, {
        moment: {
            get() {
                return moment;
            }
        },
        $moment: {
            get() {
                return moment;
            }
        },
    });
};

Vue.use(Plugin);
export default Plugin;

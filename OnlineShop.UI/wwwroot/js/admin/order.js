var app = new Vue({
    el: "#app",
    data: {
        status: 0,
        loading: false,
        orders: [],
        selectedOrder: null
    },

    mounted() {
        this.getOrders();
        console.log("Mounted");
    },
    
    watch: {
        status: function () {
            this.getOrders();
        }
    },

    methods: {
        getOrders() {
            this.loading = true;
            axios.get('/orders?status=' + this.status)
                .then(result => {
                    this.orders = result.data;
                    this.loading = false;
                })
                .catch(error => {
                    console.log(error);
                })
                .then(() => {
                    console.log(this.orders)
                    console.log("GotOrders");
                });
        },
        selectOrder(id) {
            this.loading = true;
            axios.get('/orders/' + id)
                .then(result => {
                    this.selectedOrder = result.data;
                    this.loading = false;
                    console.log(this.selectedOrder);
                });
        },
        updateOrder() {
            this.loading = true;
            axios.put('/orders/' + this.selectedOrder.id, null)
                .then(result => {
                    this.loading = false;
                    this.exitOrder();
                    this.getOrders();
                })
        },
        exitOrder() {
            this.selectedOrder = null;
        }
    },
    computed: {
    }
});

const pendingTab = document.getElementById('pendingBtn');
const packedTab = document.getElementById('packedBtn');
const shippedTab = document.getElementById('shippedBtn');

pendingTab.addEventListener('click', () => {
    app.status = 0;
    app.exitOrder();
});
packedTab.addEventListener('click', () => {
    app.status = 1;
    app.exitOrder();
});
shippedTab.addEventListener('click', () => {
    app.status = 2;
    app.exitOrder();
});

app.$watch('status', (val) => {
    pendingTab.classList.toggle('is-static', val === 0);
    packedTab.classList.toggle('is-static', val === 1);
    shippedTab.classList.toggle('is-static', val === 2);
});
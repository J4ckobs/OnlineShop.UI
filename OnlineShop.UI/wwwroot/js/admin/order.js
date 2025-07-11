﻿var app = new Vue({
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
                });
        },
        updateOrder() {
            this.loading = false;
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
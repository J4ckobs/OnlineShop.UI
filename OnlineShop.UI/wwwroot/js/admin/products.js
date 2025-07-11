﻿var app = new Vue({

    el: '#app',
    data: {
        menu: 0,
        editing: false,
        loading: false,
        objectIndex: 0,
        productModel: {
            id: 0,
            name: 'Product Name',
            description: "Product desc",
            value: 1.99
        },
        products: []
    },
    computed: {},

    mounted() {
        this.getProducts();
    },

    methods: {
        getProduct(id) {
            this.loading = true;
            axios.get('/products/' + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value
                    };
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })

        },

        getProducts() {
            this.loading = true;
            axios.get('/products')
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })
        },

        createProduct() {
            this.loading = true;
            axios.post('/products', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                    this.setDefaultValues();
                })
        },

        updateProduct() {
            this.loading = true;
            axios.put('/products', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.splice(this.objectIndex, 1, res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                    this.setDefaultValues();
                })
        },

        deleteProduct(id, index) {
            this.loading = true;
            axios.delete('/products/' + id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })

        },

        newProduct() {
            this.editing = true;
            this.productModel.id = 0;
        },

        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },

        cancelUpdateProduct() {
            this.editing = false;
            this.setDefaultValues();
        },

        setDefaultValues() {
            this.productModel = {
                id: 0,
                name: 'Product Name',
                description: "Product desc",
                value: 1.99
            };
        }
    }
});
var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        selectedProduct: null,
        products: [],
        newStock: {
            productId: 0,
            name: "Name",
            description: "Size",
            quantity: 10
        }
    },

    methods: {
        getStock() {
            this.loading = true;
            axios.get('/Admin/stocks')
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

        selectProduct(product) {
            this.selectedProduct = product;
            this.newStock.productId = product.id;
        },

        updateStock() {
            this.loading = true;

            axios.put('/Admin/stocks/', {
                stock: this.selectedProduct.stock.map(x => {
                    return {
                        id: x.id,
                        description: x.description,
                        quantity: x.quantity,
                        productId: this.selectedProduct.id
                    }
                })
            })
                .then(res => {
                    console.log("Stock updated.");
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })
        },

        addStock() {
            this.loading = true;

            axios.post('/Admin/stocks', this.newStock)
                .then(res => {
                    console.log(res.data);
                    this.selectedProduct.stock.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })
        },

        deleteStock(id, index) {
            this.loading = true;

            axios.delete('/Admin/stocks/' + id)
                .then(res => {
                    this.selectedProduct.stock.splice(index, 1)
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                })
        }
    },

    mounted() {
        this.getStock();
    }
})
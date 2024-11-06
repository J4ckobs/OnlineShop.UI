var app = new Vue({
    el: "#app",
    data: {
        username: "",
        password: "",
        users: []
    },

    methods: {
        createUser() {
            axios.post('/users', {
                username: this.username,
                password: this.password
            })
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
        },
        getUsers() {
            axios.get('/users')
                .then(result => {
                    this.users = result.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    console.log(this.users)
                })
        },
        deleteUser(id, index) {
            axios.delete('/users/' + id)
                .then(res => {
                    console.log(res);
                    this.users.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
        },
        //To add later
/*        editUser(id, index) { 
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },*/
    },

    mounted() {
        this.getUsers();
    }
});

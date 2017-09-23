// app.js
requirejs.config({
    baseUrl: 'lib',
    paths: {
        app: '../app',
        airbrakeJs: 'node_modules/airbrake-js/dist',
        book: 'book'
    }
});

requirejs(['app/main']);
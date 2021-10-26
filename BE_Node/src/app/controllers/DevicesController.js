class DevicesController {
    // [GET] /
    index(req, res, next) {
        res.render('detail/home', {
            title: 'Home',
            isLoginPage: false,
            haveChart: false,
            styles: '',
        });
    }
    
}

module.exports = new DevicesController();
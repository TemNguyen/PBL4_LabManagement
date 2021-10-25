class LoginController {
    // [GET] /news
    // index(req, res) {
    //     res.render('login/login');
    // }

    index(req, res) {
        res.render('login/login', {
            title: 'Đăng nhập',
            isLoginPage: true,
            haveChart: false,
            styles: 'login',
        });
    };
}

module.exports = new LoginController();
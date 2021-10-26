const Account = require('../models/Account');

class LoginController {
    // [GET] /
    index(req, res, next) {
        res.render('login/login', {
            title: 'Đăng nhập',
            isLoginPage: true,
            haveChart: false,
            styles: 'login',
        });
    }

    // [POST] /login
    checkLogin(req, res, next) {
        Account.findOne({ username: req.body.username, password: req.body.password })
            .then(account => {
                if (account == null) {
                    res.send('false');
                } else {
                    // Lưu
                    res.redirect('/dashboard');
                }
            })
            .catch(next);
    }
}

module.exports = new LoginController();
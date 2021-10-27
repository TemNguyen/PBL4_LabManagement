const Account = require('../models/Account');

class LoginController {
    // [GET] /
    index(req, res, next) {
        console.log({
            title: 'Đăng nhập',
            isLoginPage: true,
            haveChart: false,
            styles: 'login',
            message: {
                status: req.query.status || '',
                content: String(req.query.content).replaceAll('-', ' ') || '',
            },
        });
        res.render('login/login', {
            title: 'Đăng nhập',
            isLoginPage: true,
            haveChart: false,
            styles: 'login',
            message: {
                status: req.query.status || '',
                content: req.query.content || '',
            },
        });
    }

    // [POST] /login
    checkLogin(req, res, next) {
        Account.findOne({ username: req.body.username, password: req.body.password })
            .then(account => {
                if (account == null) {
                    res.redirect('/?status=alert-danger&content=Sai-tài-khoản-hoặc-mật-khẩu');
                } else {
                    req.session.isAuthenticated = true;
                    req.session.username = req.body.username;
                    res.redirect('/dashboard');
                }
            })
            .catch(next);
    }

    // [GET] /logout
    logout(req, res, next) {
        req.session.isAuthenticated = false;
        req.session.username = null;
        res.redirect('/');
    }
}

module.exports = new LoginController();
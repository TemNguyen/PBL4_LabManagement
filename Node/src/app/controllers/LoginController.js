const jwt = require('jsonwebtoken');
const Account = require('../models/Account');

class LoginController {
    // [GET] /
    index(req, res, next) {
        res.render('login/login', {
            title: 'Đăng nhập',
            isLoginPage: true,
            haveChart: false,
            styles: 'login',
            message: {
                status: req.query.status || '',
                content: req.query.content ? String(req.query.content).replaceAll('-', ' ') : '',
            },
        });
    }

    // [POST] /login
    checkLogin(req, res, next) {
        var day = 14;
        Account.findOne({ username: req.body.username, password: req.body.password })
            .then(account => {
                if (account == null) {
                    res.redirect('/?status=alert-danger&content=Sai-tài-khoản-hoặc-mật-khẩu');
                } else {
                    try {
                        var data = jwt.sign({
                            isAuthenticated: true,
                            accountID: account._id,
                            username: req.body.username,
                        }, global.keyCookie);
                    } catch (error) {}
                    res.cookie('data', data, { expires: new Date(Date.now() + 3600000 * 24 * day) });
                    res.redirect('/dashboard');
                }
            })
            .catch(next);
    }

    // [GET] /logout
    logout(req, res, next) {
        res.clearCookie('data');
        res.redirect('/');
    }
}

module.exports = new LoginController();
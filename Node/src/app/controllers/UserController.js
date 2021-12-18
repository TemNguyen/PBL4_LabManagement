const jwt = require('jsonwebtoken');
const Account = require('../models/Account');

class UserController {
    // [GET] /user
    index(req, res, next) {
        res.render('user/user', {
            title: 'Tài khoản cá nhân',
            isLoginPage: false,
            haveChart: false,
            styles: 'user',
            user: 'active',
            username: res.locals.localUsername,
            message: {
                status: req.query.status || '',
                content: req.query.content ? String(req.query.content).replaceAll('-', ' ') : '',
            },
        });
    }

    // [PUT] /user/repass
    repass(req, res, next) {
        try {
            var accountID = jwt.verify(req.cookies.data, 'hana').accountID;
        } catch (error) {}
        Account.findById(accountID)
            .then(account => {
                if (account.password == req.body.oldPass) {
                    account.password = req.body.newPass;
                    account.save(next);
                    res.redirect('/user/?status=alert-success&content=Đổi-mật-khẩu-thành-công');
                } else {
                    res.redirect('/user/?status=alert-danger&content=Sai-mật-khẩu');
                }
            })
            .catch(next);
    }
}

module.exports = new UserController();
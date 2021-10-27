const Account = require('../models/Account');

class UserController {
    // [GET] /User
    index(req, res, next) {
        res.render('user/user', {
            title: 'Tài khoản cá nhân',
            isLoginPage: false,
            haveChart: false,
            styles: 'user',
            user: 'active',
            username: res.locals.localUsername,
        });
    }
}

module.exports = new UserController();
const express = require('express');
const router = express.Router();
const isLogin = require('../app/middlewares/isLoginMiddleware');

const loginController = require('../app/controllers/LoginController');

router.post('/login', loginController.checkLogin);
router.get('/logout', loginController.logout);
router.get('/', isLogin.isLogged, loginController.index);

module.exports = router;

const express = require('express');
const router = express.Router();
const isLogin = require('../app/middlewares/isLoginMiddleware');

const userController = require('../app/controllers/UserController');

router.get('/', isLogin.isNotLogged, userController.index);

module.exports = router;

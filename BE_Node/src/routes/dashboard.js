const express = require('express');
const router = express.Router();
const isLogin = require('../app/middlewares/isLoginMiddleware');

const dashboardController = require('../app/controllers/DashboardController');

router.get('/', isLogin.isNotLogged, dashboardController.index);

module.exports = router;

const express = require('express');
const router = express.Router();

const loginController = require('../app/controllers/LoginController');

router.post('/login', loginController.checkLogin);
router.get('/', loginController.index);

module.exports = router;

const express = require('express');
const router = express.Router();

const userController = require('../app/controllers/UserController');

router.put('/repass', userController.repass);
router.get('/', userController.index);

module.exports = router;

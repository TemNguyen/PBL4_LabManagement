const express = require('express');
const router = express.Router();

const devicesController = require('../app/controllers/DevicesController');

router.post('/devices/detail', devicesController.index);

module.exports = router;

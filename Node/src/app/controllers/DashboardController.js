const Device = require("../models/Device");
const mongoose = require('../../util/mongoose');

class DashboardController {
    // [GET] /dashboard
    index(req, res, next) {
        Device.find({})
            .then(devices => {
                devices = mongoose.multipleMongooseToObject(devices);
                var countActive = 0;
                var countTime = 0;
                devices.forEach(device => {
                    if (device.isOnline == true) {
                        countActive++;
                    }
                    countTime += device.activeTime;
                });
                var hour = Math.floor(countTime / 3600);
                var min = Math.floor(countTime % 3600 / 60);
                var second = Math.floor(countTime % 3600 % 60);

                res.render('dashboard', {
                    title: 'Trang chủ',
                    isLoginPage: false,
                    haveChart: true,
                    styles: 'dashboard',
                    dashboard: 'active',
                    username: res.locals.localUsername,
                    data: {
                        percent: parseInt(countActive / devices.length * 100),
                        hour,
                        min,
                        second,
                    },
                });

            })
            .catch(next);
    }
}

module.exports = new DashboardController();
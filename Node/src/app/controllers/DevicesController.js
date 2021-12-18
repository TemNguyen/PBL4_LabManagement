const delay = require('delay');
const mongoose = require('../../util/mongoose');
const timer = require('../../util/timer');
const Device = require('../models/Device');
const { exec } = require('child_process');

class DevicesController {
    // [GET] /
    index(req, res, next) {
        Device.find({})
            .then(_devices => {
                res.render('devices/devices', {
                    title: 'Thiết bị',
                    isLoginPage: false,
                    haveChart: false,
                    styles: 'devices',
                    devices: 'active',
                    username: res.locals.localUsername,
                    message: {
                        status: req.query.status || '',
                        content: req.query.content ? String(req.query.content).replaceAll('-', ' ') : '',
                    },
                    _devices: mongoose.multipleMongooseToObject(_devices),
                });
            })
            .catch(next);
    }

    // [GET] /:slug
    async detail(req, res, next) {
        var cpuActive = '';
        var moreActive = '';
        var cpuContent = '';
        var moreContent = '';
        if (req.query.activeTab == 'more') {
            moreActive = ' active';
            moreContent = ' show active';
        } else {
            cpuActive = ' active';
            cpuContent = ' show active';
        }
        var moreStatus = timer.getTimer(req, req.params.slug) ? timer.getTimer(req, req.params.slug).content : '';
        Device.findOne({ _id: req.params.slug })
            .then(device => {
                res.render('devices/detail', {
                    title: 'Chi tiết',
                    isLoginPage: false,
                    haveChart: true,
                    styles: 'detail',
                    devices: 'active',
                    username: res.locals.localUsername,
                    cpuActive,
                    moreActive,
                    cpuContent,
                    moreContent,
                    message: {
                        status: req.query.status || '',
                        content: req.query.content ? String(req.query.content).replaceAll('-', ' ') : '',
                    },
                    device: mongoose.mongooseToObject(device),
                    moreStatus,
                });
            })
            .catch(next);
        
        while (true) {
            // console.log(global.socketActive);
            Device.findOne({ _id: req.params.slug })
                .then(device => {
                    device = mongoose.mongooseToObject(device);
                    global.io.emit('data', {
                        data: {
                            id: req.params.slug,
                            cpuUsage: device.CPUUsage,
                            numProcess: device.numProcess,
                            numThread: device.numThread,
                            ramUsage: device.RAMUsage,
                            ramFree: parseFloat(device.RAMSize - device.RAMUsage).toFixed(1),
                            diskUsage: device.diskUsage,
                            gpuUsage: device.GPUUsage,
                        },
                    });
                });
            await delay(2000);
            if (global.socketActive == false) {
                // console.log(global.socketActive, ' ', req.params.slug);
                return;
            }
        }
    }

    // [POST] /:slug
    async timer(req, res, next) {
        var time = parseInt(req.body.min) * 60;
        Device.findById(req.params.slug)
            .then(device => {
                if (device.computerName.includes('CLIENT') == true) {
                    var q1 = 'NET USE \\\\' + req.query.name + ' /user:ADMIN tem1412';
                    var q2 = 'shutdown -s -m \\\\' + req.query.name + ' -t ' + time;
                    exec(q1, (err1, stdout1, stderr1) => {
                        if (err1) {
                            console.log(err1.message);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        if (stderr1) {
                            console.log(stderr1);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        exec(q2, (err2, stdout2, stderr2) => {
                            if (err2) {
                                console.log(err2.message);
                                res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                                return;
                            }
                            if (stderr2) {
                                console.log(stderr2);
                                res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                                return;
                            }
                            timer.addTimer(res, req.params.slug, req.body.min);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-success&content=Hẹn-giờ-tắt-máy-thành-công&activeTab=more`);
                        });
                    });
                } else {
                    var q = 'shutdown -s -t ' + time;
                    exec(q, (err, stdout, stderr) => {
                        if (err) {
                            console.log(err.message);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        if (stderr) {
                            console.log(stderr);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        timer.addTimer(res, req.params.slug, req.body.min);
                        res.redirect(`/devices/${req.params.slug}/?status=alert-success&content=Hẹn-giờ-tắt-máy-thành-công&activeTab=more`);
                    });
                }
            })
            .catch(next);
    }

    // [POST] /cancel/:slug
    async cancelTimer(req, res, next) {
        Device.findById(req.params.slug)
            .then(device => {
                if (device.computerName.includes('CLIENT') == true) {
                    var q1 = 'NET USE \\\\' + req.query.name + ' /user:ADMIN tem1412';
                    var q2 = 'shutdown -a -m \\\\' + req.query.name;
                    exec(q1, (err1, stdout1, stderr1) => {
                        if (err1) {
                            console.log(err1.message);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        if (stderr1) {
                            console.log(stderr1);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        exec(q2, (err2, stdout2, stderr2) => {
                            if (err2) {
                                console.log(err2.message);
                                res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                                return;
                            }
                            if (stderr2) {
                                console.log(stderr2);
                                res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                                return;
                            }
                            timer.delTimer(res, req.params.slug);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-success&content=Hủy-hẹn-giờ-tắt-máy-thành-công&activeTab=more`);
                        });
                    });
                } else {
                    var q = 'shutdown -a';
                    exec(q, (err, stdout, stderr) => {
                        if (err) {
                            console.log(err.message);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        if (stderr) {
                            console.log(stderr);
                            res.redirect(`/devices/${req.params.slug}/?status=alert-danger&content=Có-lỗi-xảy-ra-vui-lòng-thử-lại&activeTab=more`);
                            return;
                        }
                        timer.delTimer(res, req.params.slug);
                        res.redirect(`/devices/${req.params.slug}/?status=alert-success&content=Hủy-hẹn-giờ-tắt-máy-thành-công&activeTab=more`);
                    });
                }
            })
            .catch(next);


    }
}

module.exports = new DevicesController();

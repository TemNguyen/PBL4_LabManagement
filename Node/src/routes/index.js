const dashboardRouter = require('./dashboard');
const devicesRouter = require('./devices');
const userRouter = require('./user');
const loginRouter = require('./login');
const aboutRouter = require('./about');

const isLogin = require('../app/middlewares/isLoginMiddleware');

function route(app) {
    app.use('/devices', isLogin.isNotLogged, devicesRouter);
    app.use('/user', isLogin.isNotLogged, userRouter);
    app.use('/dashboard', isLogin.isNotLogged, dashboardRouter);
    app.use('/about-contact', aboutRouter);
    app.use('/', loginRouter);
}

module.exports = route;
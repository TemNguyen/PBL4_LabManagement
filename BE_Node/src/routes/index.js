const dashboardRouter = require('./dashboard');
const devicesRouter = require('./devices');
const userRouter = require('./user');
const loginRouter = require('./login');

function route(app) {
    // app.use('/dashboard', dashboardRouter);
    // app.use('/devices', devicesRouter);
    // app.use('/user', userRouter);
    app.use('/', loginRouter);
}

module.exports = route;
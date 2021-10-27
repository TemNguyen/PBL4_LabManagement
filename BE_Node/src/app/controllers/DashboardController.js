class DashboardController {
    // [GET] /dashboard
    index(req, res, next) {
        res.render('dashboard', {
            title: 'Trang chủ',
            isLoginPage: false,
            haveChart: true,
            styles: 'dashboard',
            dashboard: 'active',
            username: res.locals.localUsername,
        });
    }
}

module.exports = new DashboardController();
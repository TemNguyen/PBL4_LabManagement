const jwt = require('jsonwebtoken');

module.exports = {
    isLogged(req, res, next) {
        try {
            var data = jwt.verify(req.cookies.data, global.keyCookie);
        } catch (error) {}
        if (data !== undefined) {
            return res.redirect('/dashboard');
        }
        next();
    },
    isNotLogged(req, res, next) {
        try {
            var data = jwt.verify(req.cookies.data, global.keyCookie);
        } catch (error) {}
        if (data === undefined) {
            return res.redirect('/');
        }
        next();
    },
    clearCacheBack(req, res, next) {
        res.set('Cache-Control', 'no-cache, private, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0');
        next();
    },
};
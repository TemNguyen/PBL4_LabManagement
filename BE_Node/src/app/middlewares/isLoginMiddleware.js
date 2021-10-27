module.exports = {
    isLogged(req, res, next) {
        if (req.session.isAuthenticated) {
            return res.redirect('/dashboard');
        }
        next();
    },
    isNotLogged(req, res, next) {
        if (!req.session.isAuthenticated) {
            return res.redirect('/');
        }
        next();
    },
    clearCacheBack(req, res, next) {
        res.set('Cache-Control', 'no-cache, private, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0');
        next();
    },
};
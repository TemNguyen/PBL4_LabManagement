const jwt = require('jsonwebtoken');

module.exports = {
    addTimer(res, id, time) {
        try {
            var data = jwt.sign({
                content: 'Máy sẽ tắt sau ' + time + ' phút kể từ ' + new Date().toLocaleString()
            }, global.keyCookie);
        } catch (error) {}
        res.cookie(id, data, { expires: new Date(Date.now() + 60000 * time) });
    },
    delTimer(res, id) {
        res.clearCookie(id);
    },
    getTimer(req, id) {
        try {
            var data = jwt.verify(req.cookies[id], global.keyCookie);
        } catch (error) {}
        return data;
    }
}
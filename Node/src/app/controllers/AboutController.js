class AboutController {
    // [GET] /about-contact
    index(req, res, next) {
        var content = false;
        var title = 'Liên hệ';
        if (req.query.content == 'about') {
            title = 'Giới thiệu';
            content = true;
        }
        res.render('about', {
            layout: false,
            title,
            content,
        });
    }
}

module.exports = new AboutController();
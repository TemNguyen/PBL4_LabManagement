const Handlebars = require('handlebars');

Handlebars.registerHelper('switch', function (value, options) {
    this.switch_value = value;
    return options.fn(this);
});

Handlebars.registerHelper('case', function (value, options) {
    if (value == this.switch_value) {
        return options.fn(this);
    }
});

Handlebars.registerHelper('sum', function (a, b) {
    return a + b;
});

Handlebars.registerHelper('sub', function (a, b) {
    return (parseFloat(a) -parseFloat(b)).toFixed(0);
});

Handlebars.registerHelper('fixed', function (a) {
    return parseFloat(a).toFixed(1);
});

const express = require('express');
const path = require('path');
const handlebars  = require('express-handlebars');

const route = require('./routes');
const db = require('./config/db');

const app = express();
const port = 3000;

// Connect to DB
db.connect();

// static file
app.use(express.static(path.join(__dirname, 'public')));

// template engine
app.engine(
  'hbs',
  handlebars({
      extname: '.hbs',
      helpers: require('./helpers/switch_case'),
  }),
);
app.set('view engine', 'hbs');
app.set('views', path.join(__dirname, 'resources', 'views'));


// routes init
route(app);

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});
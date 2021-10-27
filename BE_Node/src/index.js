const express = require('express');
const path = require('path');
const handlebars  = require('express-handlebars');
const session = require('express-session');

const route = require('./routes');
const db = require('./config/db');

// middleware
const isLogin = require('./app/middlewares/isLoginMiddleware');
const saveAccountLogged = require('./app/middlewares/saveAccountMiddleware');

const app = express();
const port = 3000;

// Connect to DB
db.connect();

// static file
app.use(express.static(path.join(__dirname, 'public')));

app.use(
  express.urlencoded({
      extended: true,
  }),
);
app.use(express.json());

// express session
app.set('trust proxy', 1);
app.use(session({
  secret: 'keyboard idol',
  resave: false,
  saveUninitialized: true,
  cookie: { 
    // secure: true, 
  }
}));

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

app.use(saveAccountLogged);

app.use(isLogin.clearCacheBack);

// routes init
route(app);

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});
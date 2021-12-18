const express = require('express');
const path = require('path');
const handlebars  = require('express-handlebars');

const route = require('./routes');
const db = require('./config/db');

// middleware
const cookieParser = require('cookie-parser');
const methodOverride = require('method-override');
const isLogin = require('./app/middlewares/isLoginMiddleware');
const saveAccountLogged = require('./app/middlewares/saveAccountMiddleware');

const app = express();
const port = process.env.PORT || 3000;

// socket
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
global.io = new Server(server);
global.socketActive = true;

global.keyCookie = 'hana';

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

// template engine
app.engine(
  'hbs',
  handlebars({
      extname: '.hbs',
      helpers: require('./helpers/handlebars'),
  }),
);
app.set('view engine', 'hbs');
app.set('views', path.join(__dirname, 'resources', 'views'));

// use middleware
app.use(cookieParser('hanaidol'));
app.use(methodOverride('_method'));
app.use(saveAccountLogged);
app.use(isLogin.clearCacheBack);

// routes init
route(app);

io.on('connection', (socket) => {
  global.socketActive = true;
  console.log('connected');

  socket.on('disconnect', () => {
    global.socketActive = false;
    console.log('disconnected');
  });
});

server.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`);
});

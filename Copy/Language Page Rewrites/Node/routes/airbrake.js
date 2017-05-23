var express = require('express');
var router = express.Router();

/* GET users listing. */
router.get('/', function(req, res, next) {
  throw new Error('Oh noes, something broke!');
  res.send('respond with a brake');
});

module.exports = router;

// var airbrake = require('airbrake').createClient(
//     '144163', // Project ID
//     'da01bb6595df56f4610420efaf735fda' // Project key
// );
// airbrake.handleExceptions();
//
// throw new Error('I am an uncaught exception');
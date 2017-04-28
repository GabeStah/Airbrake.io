// Direct Example
var airbrake = new airbrakeJs.Client({
  projectId: 141379, 
  projectKey: '52e0659442daa64cd52d8fd9110f66c1'
})

airbrake.addFilter(function(notice) {
  // if (notice.environment.server == 'development') {
  //   // Ignore errors from the development server.
  //   return null;
  // }
  return notice;
});

try {
  throw new Error('Direct example error.');
} catch (err) {
  // Notify Airbrake of error.
  promise = airbrake.notify({
    error:       err,
    context:     { component: 'library' },
    environment: { server: 'development' },
    params:      { name: 'Bob Smith' },
    session:     { user: 'bobsmith@example.com' },
  });
  promise.then(function(notice) {
    console.log('Id:', notice.id);
  }, function(err) {
    console.log('Airbrake failed:', err);
  });
}

// Express Example
// var express = require('express');
// var app = express();
// var AirbrakeClient = require('airbrake-js');
// var makeErrorHandler = require('airbrake-js/dist/instrumentation/express');

// var airbrake = new AirbrakeClient({
//   projectId: 141379, 
//   projectKey: '52e0659442daa64cd52d8fd9110f66c1'
// });

// app.get('/', function hello (req, res) {
//   throw new Error('Express example error.');
//   res.send('Hello World!');
// })

// // Use the express instrumentation to catch and notify errors.
// app.use(makeErrorHandler(airbrake));

// app.listen(3000, function () {
//   console.log('Example app listening on port 3000!');
// })

// RequireJS Example
// require.config({
//   paths: {
//     airbrakeJs: 'node_modules/airbrake-js/dist'
//   }
// });

// require(['airbrakeJs/client'], function (AirbrakeClient) {
//   var airbrake = new AirbrakeClient({
//     projectId: 141379, 
//     projectKey: '52e0659442daa64cd52d8fd9110f66c1'
//   });

//   try {
//     throw new Error('Require.js example error.');
//   } catch (err) {
//     promise = airbrake.notify(err);
//     promise.then(function(notice) {
//       console.log('notice id:', notice.id);
//     }, function(err) {
//       console.log('airbrake failed:', err);
//     });
//   }
// });
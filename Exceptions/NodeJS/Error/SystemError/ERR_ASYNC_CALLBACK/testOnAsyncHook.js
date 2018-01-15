/**
 * testOnAsyncHook.js
 */

/**
 * Performs basic tracing for async_hooks.
 * See: https://github.com/lrlna/on-async-hook
 *
 * @type {onAsyncHook}
 */
const onAsyncHook = require('on-async-hook');

// Create asyncHook instance and log data to console.
let stopAsyncHook = onAsyncHook(function (data) {
  console.log(data)
});

// Create server and respond with 'Hello world' to incoming connections on port 8080.
require('http').createServer(function (request, response) {
  response.end('Hello world')
}).listen(8080);

// Stop asyncHook after 2 seconds.
setTimeout(() => {
  stopAsyncHook();
}, 2000);
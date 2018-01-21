/**
 * asyncResourceTests.js
 */
const async_hooks = require('async_hooks');
const { AsyncResource, executionAsyncId } = async_hooks;
const logging = require('logging');

function executeTests () {
  //logging.lineSeparator("getAsyncResource('MyNetResource', ...)", 60);
  //getAsyncResource('MyNetResource', { triggerAsyncId: executionAsyncId() } );

  //logging.lineSeparator("getAsyncResource(24601)", 60);
  //getAsyncResource(24601);
  //
  logging.lineSeparator("getAsyncResource('')", 60);
  getAsyncResource('');
}

/**
 * Creates an AsyncResource instance using passed options, performing basic net.server connection test.
 *
 * @param options Type and other arguments.
 */
function getAsyncResource (options) {
  try {
    // Create AsyncResource.
    let resource = new AsyncResource(options);

    // Create server and listen on port 8080.
    let server = require('net').createServer(() => {
    }).listen(8080, () => {
      // Invoke resource.emitBefore().
      logging.logSync(`resource.emitBefore(): ${resource.emitBefore()}`);
      resource.emitBefore()

      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
        logging.logSync(`resource.asyncId(): ${resource.asyncId()}`);
        logging.logSync(`resource.triggerAsyncId(): ${resource.triggerAsyncId()}`);
      }, 1000);

      // Invoke resource.emitAfter().
      logging.logSync(`resource.emitAfter(): ${resource.emitAfter()}`);
      resource.emitAfter()

      // Close connection after 3 seconds.
      setTimeout(() => {
        server.close();
      }, 3000);
    });

    // Invoke resource.emitDestroy() when server closed.
    server.on('close', function () {
      logging.logSync(`resource.emitDestroy(): ${resource.emitDestroy()}`);
      resource.emitDestroy()
    })

    return resource;
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_TYPE') {
      // Output expected ERR_ASYNC_TYPE TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
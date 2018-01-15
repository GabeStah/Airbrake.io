/**
 * createAsyncHook.js
 */

const async_hooks = require('async_hooks');
const logging = require('logging');

function executeTests () {
  let callbacks = {
    init: init,
    before: before,
    after: after,
    destroy: destroy,
    promiseResolve: promiseResolve
  };
  logging.lineSeparator('async_hooks.createHook(...)', 60);
  getAsyncHook(callbacks);
}

/**
 * Gets an AsyncHook instance.
 *
 * @param callbacks
 * @returns {*}
 */
function getAsyncHook (callbacks) {
  try {
    // Get instance from constructor, log to console, and return object.
    let asyncHook = async_hooks.createHook(callbacks).enable();

    // Create server and listen on port 8080.
    require('net').createServer(() => {}).listen(8079, () => {
      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
      }, 1000);
    });

    return asyncHook;
  } catch (e) {
    // Catch TypeError with code property of ERR_ASYNC_CALLBACK.
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_CALLBACK') {
      // Output expected ERR_ASYNC_CALLBACK TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Called during object construction.
 *
 * @param asyncId
 * @param type
 * @param triggerAsyncId
 * @param resource
 */
function init(asyncId, type, triggerAsyncId, resource) {
  const executionId = async_hooks.executionAsyncId();
  logging.logSync(`(INIT) Id: ${asyncId}, TriggerId: ${triggerAsyncId}, ExecutionId: ${executionId}, Type: ${type}`);
}

/**
 * Called before resource's callback.
 *
 * @param asyncId
 */
function before(asyncId) {
  logging.logSync(`(BEFORE) Id: ${asyncId}`);
}

/**
 * Called after resource's callback.
 *
 * @param asyncId
 */
function after(asyncId) {
  logging.logSync(`(AFTER) Id: ${asyncId}`);
}

/**
 * Called when AsyncWrap instance is destroyed.
 *
 * @param asyncId
 */
function destroy(asyncId) {
  logging.logSync(`(DESTROY) Id: ${asyncId}`);
}

/**
 * Called when a Promise resource's resolve function is passed to the Promise constructor.
 *
 * @param asyncId
 */
function promiseResolve(asyncId) {
  logging.logSync(`(PROMISE) Id: ${asyncId}`);
}

executeTests();
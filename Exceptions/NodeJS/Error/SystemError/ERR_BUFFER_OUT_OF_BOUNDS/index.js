/**
 * index.js
 */
const logging = require('logging');

function executeTests () {
  // Create int8 array populated with first 5 keys.
  const data = Uint8Array.from(new Uint8Array(5).keys());

  logging.lineSeparator('bufferFromValue(data.buffer)', 60);
  bufferFromValue(data.buffer);

  logging.lineSeparator('bufferFromValue(data.buffer, 3)', 60);
  bufferFromValue(data.buffer, 3);

  logging.lineSeparator('bufferFromValue(data.buffer, 6)', 60);
  bufferFromValue(data.buffer, 6);

  logging.lineSeparator('bufferFromValue(data.buffer, 2, 7)', 60);
  bufferFromValue(data.buffer, 2, 7);
}

/**
 * Retrieves a buffer from passed value, using optional encoding/offset and length.
 *
 * @param value String, buffer, array-like Object to get buffer from.
 * @param encodingOrOffset Encoding (for Strings) or offset (for array-like Objects) at which to offset returned buffer.
 * @param length Length of buffer to retrieve.
 * @returns {Buffer2} Generated buffer.
 */
function bufferFromValue (value, encodingOrOffset, length) {
  try {
    // Invokes Buffer.from method with passed parameters.
    let buffer = Buffer.from(value, encodingOrOffset, length);
    // Log generated buffer.
    logging.log(buffer);
    // Return generated buffer.
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_OUT_OF_BOUNDS') {
      // Output expected ERR_BUFFER_OUT_OF_BOUNDS RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
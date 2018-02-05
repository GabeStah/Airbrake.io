/**
 * index.js
 */
const { kMaxLength } = require('buffer');
const logging = require('logging');

function executeTests () {
  logging.lineSeparator(`instantiateBuffer(1)`, 60);
  instantiateBuffer(1)

  logging.lineSeparator(`instantiateBuffer(kMaxLength)`, 60);
  instantiateBuffer(kMaxLength)

  logging.lineSeparator(`instantiateBuffer(kMaxLength + 1)`, 60);
  instantiateBuffer(kMaxLength + 1);

  logging.lineSeparator(`allocateBuffer(1)`, 60);
  allocateBuffer(1);

  logging.lineSeparator(`allocateBuffer(kMaxLength)`, 60);
  allocateBuffer(kMaxLength);

  logging.lineSeparator(`allocateBuffer(kMaxLength + 1)`, 60);
  allocateBuffer(kMaxLength + 1);
}

/**
 * Allocates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to allocate.
 * @returns {Buffer} Allocated Buffer.
 */
function allocateBuffer (size) {
  try {
    let buffer = Buffer.alloc(size);
    logging.log(`Successfully allocated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Instantiates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to instantiate.
 * @returns {Buffer} Instantiated Buffer.
 */
function instantiateBuffer (size) {
  try {
    let buffer = new Buffer(size);
    logging.log(`Successfully instantiated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
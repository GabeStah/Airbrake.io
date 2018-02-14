/**
 * index.js
 */
const logging = require('logging');
const { TextDecoder } = require('util');

function executeTests () {
  logging.lineSeparator(`DECODE: Airbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Airbrake.io'));

  logging.lineSeparator(`DECODE: Âirbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Âirbrake.io'));

  logging.lineSeparator(`DECODE: [0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]`, 100, '=');
  decodeBuffer(Buffer.from([0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]));

  logging.lineSeparator(`NON-FATALLY DECODE: Âirbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Âirbrake.io'), 'utf-8', false);
}

/**
 * Decodes passed Buffer via TextDecoder, using passed encoding and fatal option.
 * Iterates and outputs each byte segment of full buffer.
 *
 * @param buffer Buffer to be decoded.
 * @param encoding Encoding to use.
 * @param fatal Determines if decoder will throw errors if an issue occurs.
 */
function decodeBuffer (buffer, encoding='utf-8', fatal=true) {
  try {
    // Create text decoder with proper encoding and fatal option.
    const decoder = new TextDecoder(encoding, { fatal: fatal });

    // Iterate through buffer, slicing from start to each index, then outputting and decoding.
    for (let i = buffer.length; i >= 1; i--) {
      let slice = buffer.slice(0, i);

      logging.lineSeparator(`slice(0, ${i})`, 23, '_');
      logging.log(slice);
      logging.log(`Decoded slice: ${decoder.decode(slice)}`);
    }
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ENCODING_INVALID_ENCODED_DATA') {
      // Output expected ERR_ENCODING_INVALID_ENCODED_DATA TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
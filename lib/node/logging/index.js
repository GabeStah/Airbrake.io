// logging module - index.js
const fs = require("fs");
const SeparatorCharacterDefault = '-';
const SeparatorLengthDefault = 40;

module.exports = {
  /**
   * Outputs a line separator via console.log, with optional first argument text centered in the middle.
   */
  lineSeparator: function () {
    // Check if at least one argument of string type is passed.
    if (arguments.length >= 1 && typeof(arguments[0]) === 'string') {
      lineSeparatorWithInsert(arguments[0], arguments[1], arguments[2]);
    } else {
      // Otherwise, assume default separator without insertion.
      lineSeparator(arguments[0], arguments[1]);
    }
  },

  /**
   * Log the passed object or value.
   *
   * @param value Value to be logged to the console.
   */
  log: function (value) {
    if (value instanceof Error) {
      logError(value, arguments[1]);
    } else {
      logValue(value);
    }
  },

  /**
   * Synchronously log the passed object or value.
   *
   * @param value Value to be logged to the console.
   */
  logSync: function (value) {
    if (value instanceof Error) {
      logError(value, arguments[1], true);
    } else {
      logValue(value, true);
    }
  }
};

/**
 * Outputs a line separator via console.log.
 *
 * @param length Total separator length.
 * @param char Separator character.
 */
function lineSeparator (length = SeparatorLengthDefault, char = SeparatorCharacterDefault) {
  // Default output to insertion.
  logValue(Array(length).join(char));
}

/**
 * Outputs a line separator via console.log with inserted text centered in the middle.
 *
 * @param insert Inserted text to be centered.
 * @param length Total separator length.
 * @param char Separator character.
 */
function lineSeparatorWithInsert (insert, length = SeparatorLengthDefault, char = SeparatorCharacterDefault) {
  // Default output to insertion.
  let output = insert;

  if (insert.length < length) {
    // Update length based on insert length, less a space for margin.
    length -= insert.length + 2;
    // Halve the length and floor left side.
    let left = Math.floor(length / 2);
    let right = left;
    // If odd number, add dropped remainder to right side.
    if ((length % 2) !== 0) {
      right += 1;
    }

    // Surround insert with separators.
    output = `${Array(left).join(char)} ${insert} ${Array(right).join(char)}`;
  }

  logValue(output);
}

/**
 * Logs an Error with explicit/inexplicit tag, error name, and message.
 *
 * @param error Error to be logged.
 * @param explicit Determines if passed Error was explicit (intended) or not.
 * @param synchronous Determines if output must be synchronous.
 */
function logError(error, explicit = true, synchronous = false) {
  let message = `[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`;
  // To avoid duplication get stack without initial error message line.
  let stack = error.stack.slice(error.stack.indexOf("\n") + 1);
  // Use fs.writeSync for synchronous output, otherwise console.log.
  if (synchronous) {
    fs.writeSync(1, message + '\n');
    fs.writeSync(1, stack);
  } else {
    console.log(message);
    console.log(stack);
  }
}

/**
 * Logs a value (string, object, number, etc).
 *
 * @param value Value to be logged.
 * @param synchronous Determines if output must be synchronous.
 */
function logValue(value, synchronous = false) {
  // Use fs.writeSync for synchronous output, otherwise console.log.
  if (synchronous) {
    fs.writeSync(1, value + '\n');
  } else {
    console.log(value);
  }
}
/**
 * index.js
 */
const Book = require('book');
const { Console } = require('console');
const fs = require('fs');
const logging = require('logging');

function executeTests () {
  // Create new Book instance.
  let bookA = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
  logging.lineSeparator('BOOK A', 60);
  logging.log(bookA.toString());

  // Output Book A to stream.
  outputBookToStream(bookA, fs.createWriteStream('books.json'));

  let bookB = new Book('The Wise Man\'s Fear', 'Patrick Rothfuss', 994, new Date(2011, 2, 1));
  logging.lineSeparator('BOOK B', 60);
  logging.log(bookB.toString());

  // Output Book B to stream.
  outputBookToStream(bookB, fs.createWriteStream('books.json'));

  // Add both Books to new stream in order to properly format JSON.
  logging.lineSeparator('ADDING BOOKS A & B SIMULTANEOUSLY', 60);
  addValueToStream(JSON.stringify([
    bookA,
    bookB
  ]), fs.createWriteStream('books.json'));

  // Create Book C instance.
  let bookC = new Book('Doors of Stone', 'Patrick Rothfuss', 0);
  logging.lineSeparator('BOOK C', 60);
  logging.log(bookC.toString());

  // Output Book C to null stream.
  outputBookToStream(bookC);
}

/**
 * Adds passed value to passed writeStream by creating new Console instance.
 *
 * @param value Value to be added.
 * @param writeStream (Optional) Write stream to add value to.
 * @param errorStream (Optional) Error stream to output errors to.
 */
function addValueToStream (value, writeStream = null, errorStream = null) {
  try {
    // Get console instance using passed streams.
    let streamConsole = getConsole(writeStream, errorStream);
    if (!streamConsole) return;

    // Log passed value to stream.
    streamConsole.log(value);

    // Confirm value addition.
    logging.log(`Successfully added ${value} to stream.`);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Instantiates a new Console using passed Writables.
 *
 * @param {Writable} stdout (Optional) Writable to be written to.
 * @param {Writable} stderr (Optional) Writable for error to be written to.
 * @returns {Console.Console} Console instance.
 */
function getConsole (stdout = null, stderr = null) {
  try {
    if (!stderr) {
      stderr = stdout;
    }

    return new Console(stdout, stderr);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Converts passed Book to JSON and outputs it to passed stream via Console.
 *
 * @param book Book to be added.
 * @param stream Stream into which Book should be output.
 */
function outputBookToStream (book, stream = null) {
  // Convert book to JSON.
  let json = JSON.stringify(book);
  logging.lineSeparator('BOOK TO JSON', 60);
  logging.log(json);

  // Output JSON to stream.
  addValueToStream(json, stream);
}

executeTests();
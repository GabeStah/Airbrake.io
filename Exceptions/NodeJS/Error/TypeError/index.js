const Book = require('book');
const logging = require('logging');
const path = require('path');

function executeTests () {
  logging.lineSeparator("parsePath('/Error/TypeError/index.js')", 60);
  parsePath('/Error/TypeError/index.js');

  logging.lineSeparator("parsePath(12345)", 60);
  parsePath(12345);

  logging.lineSeparator("createValidBook()", 60);
  createValidBook();

  logging.lineSeparator("createInvalidBook()", 60);
  createInvalidBook();
}

function parsePath (value) {
  try {
    logging.log(path.parse(value));
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function createValidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function createInvalidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book("The Wise Man's Fear", new String('Patrick Rothfuss'), 994, new Date(2011, 2, 1));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
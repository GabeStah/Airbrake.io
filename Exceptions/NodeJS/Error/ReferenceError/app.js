const logging = require('logging');
const Book = require('book');

function executeTests () {
  logging.lineSeparator('EXECUTING PASSING TEST');
  passingTest();

  logging.lineSeparator('EXECUTING FAILING TEST');
  failingTest();
}

function passingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output valid Book instance, 'book'.
    logging.log(book);
    logging.log(book.getTagline());
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function failingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output invalid Book instance, 'boo'.
    logging.log(boo);
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();

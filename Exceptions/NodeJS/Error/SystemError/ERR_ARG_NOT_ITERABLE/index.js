const logging = require('logging');
const { URL, URLSearchParams } = require('url');

function executeTests () {
  let params;

  params = 'title=The Hobbit&author=J.R.R. Tolkien&pageCount=366';
  logging.lineSeparator('STRING TEST', 60);
  getURLSearchParams(params);

  params = {
    title: 'The Stand',
    author: 'Stephen King',
    pageCount: 1153
  };
  logging.lineSeparator('OBJECT TEST', 60);
  getURLSearchParams(params);

  params = [
    ['title', 'The Name of the Wind'],
    ['author', 'Patrick Rothfuss'],
    ['pageCount', 662]
  ];
  logging.lineSeparator('ITERABLE TEST', 60);
  getURLSearchParams(params);

  params = new Map([
    ['title', "The Wise Man's Fear"],
    ['author', 'Patrick Rothfuss'],
    ['pageCount', 994]
  ]);
  logging.lineSeparator('MAP TEST', 60);
  getURLSearchParams(params);

  params = {
    [Symbol.iterator]: [
      ['title', 'The Slow Regard of Silent Things'],
      ['author', 'Patrick Rothfuss'],
      ['pageCount', 159]
    ]
  };
  logging.lineSeparator('NON-WELL-FORMED ITERABLE FUNCTION TEST', 60);
  getURLSearchParams(params);

  params = {
    [Symbol.iterator]: function* () {
      yield ['title', 'The Slow Regard of Silent Things'];
      yield ['author', 'Patrick Rothfuss'];
      yield ['pageCount', 159];
    }
  };
  logging.lineSeparator('WELL-FORMED ITERABLE FUNCTION TEST', 60);
  getURLSearchParams(params);
}

/**
 * Gets a URLSearchParams instance object by passing the value param to the constructor.
 * @param value Value to be passed to URLSearchParams constructor.
 * @returns {*} URLSearchParams instance, or undefined.
 */
function getURLSearchParams (value) {
  try {
    // Get instance from constructor, log to console, and return object.
    let instance = new URLSearchParams(value);
    logging.log(instance);
    return instance;
  } catch (e) {
    // Catch TypeError with code property of ERR_ARG_NOT_ITERABLE.
    if (e instanceof TypeError && e.code === 'ERR_ARG_NOT_ITERABLE') {
      // Output expected ERR_ARG_NOT_ITERABLE TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
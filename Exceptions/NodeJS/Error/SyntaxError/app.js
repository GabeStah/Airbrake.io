const logging = require('logging');
const vm = require('vm');

function executeTests () {
  logging.lineSeparator("functionTest(2, 5, 'return x * y')", 60);
  functionTest(2, 5, 'return x * y');

  logging.lineSeparator("functionTest(2, 5, 'return x  y')", 60);
  functionTest(2, 5, 'return x  y');

  logging.lineSeparator("evalTest('3 * 6')", 60);
  evalTest('3 * 6');

  logging.lineSeparator("evalTest('3 | 6')", 60);
  evalTest('3 _ 6');

  logging.lineSeparator("requireTest(4, 7, './multiply.js')", 60);
  requireTest(4, 7, './multiply.js');

  logging.lineSeparator("requireTest(4, 7, './multiply_invalid.js')", 60);
  requireTest(4, 7, './multiply_invalid.js');

  logging.lineSeparator("vmTest('5 * 8')", 60);
  vmTest('5 * 8');

  logging.lineSeparator("vmTest('5 # 8')", 60);
  vmTest('5 # 8');
}

function evalTest (body) {
  try {
    // Execute eval(body) and output result.
    logging.log(eval(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function functionTest (x, y, body) {
  try {
    // Create function body.
    let f = new Function ('x', 'y', body);
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function requireTest (x, y, path) {
  try {
    // Get multiply function from require path.
    let f = require(path).multiply;
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function vmTest (body) {
  try {
    // Run the body code in current context.
    logging.log(vm.runInThisContext(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();

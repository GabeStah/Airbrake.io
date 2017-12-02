const assert = require('assert');
const AssertionError = require('assert').AssertionError;

function executeTests () {
  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(0, 1);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(0, 1, "0 and 1 are not equal!");

  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(1, 1);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 4);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 4, "4 and 4 are equivalent!");

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 5);
}

function assertStrictEquality (a, b, message = null) {
  try {
    // Output test.
    console.log(`----- ASSERTING: ${a} === ${b} -----`);
    // Assert equality of a and b parameters.
    assert.strictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} === ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}

function assertStrictInequality (a, b, message = null) {
  try {
    console.log(`----- ASSERTING: ${a} !== ${b} -----`);
    // Assert inequality of a and b parameters.
    assert.notStrictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} !== ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}

executeTests();

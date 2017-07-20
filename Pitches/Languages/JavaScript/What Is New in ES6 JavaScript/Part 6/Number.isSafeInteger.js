// Get minimum safe number.
console.log(Number.MIN_SAFE_INTEGER);
// Get maximum safe number.
console.log(Number.MAX_SAFE_INTEGER);

// Check if 12345 is safe.
console.log(Number.isSafeInteger(12345)); // true
// Check if 2^53 is safe.
console.log(Number.isSafeInteger(Math.pow(2, 53))); // false
// Check if 2^53 - 1 is safe.
console.log(Number.isSafeInteger(Math.pow(2, 53) - 1)); // true
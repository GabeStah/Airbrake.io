// ES6 Array.prototype.find()
// Define some data.
let data = [1, 6, 3, 4, 2, 5, 7];

// Find first value greater than or equal to 5.
console.log(data.find(x => x >= 5)); // 6

// Find index of first value equal to 4.
console.log(data.findIndex(x => x == 4)); // 3
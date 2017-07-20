// ES6 String searching
let name = "Chris Decker";

// Check if name starts with 'Chr'.
console.log(name.startsWith("Chr")); // true
// Check if name ends with 'Chr'.
console.log(name.endsWith("Chr")); // false

// Check if name includes 'ecker'.
console.log(name.includes("ecker")); // true
// Check if name includes 'ecker' starting at position 10.
console.log(name.includes("ecker", 10)); // false
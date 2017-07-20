// ES6 Mapping Example
// Create book objects.
let book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
let book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create library map object.
let library = new Map();

// Set published dates using book objects as keys.
library.set(book1, new Date(1960, 6, 11));
library.set(book2, new Date(2005, 0, 1));

// Get published dates.
console.log(`'${book1.title}' by ${book1.author} published on ${library.get(book1).toDateString()}.`);
console.log(`'${book2.title}' by ${book2.author} published on ${library.get(book2).toDateString()}.`);
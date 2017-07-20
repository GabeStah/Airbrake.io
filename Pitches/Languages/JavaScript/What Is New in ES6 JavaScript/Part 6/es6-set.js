// ES6 Set Example
// Create book objects.
let book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
let book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create library set object.
let library = new Set();

// Add book1 to set.
library.add(book1);

// Determine if books exists in set.
console.log(`'Is ${book1.title}' by ${book1.author} in the library? ${library.has(book1) ? 'YES' : 'NO'}`);
// 'Is To Kill a Mockingbird' by Harper Lee in the library? YES
console.log(`'Is ${book2.title}' by ${book2.author} in the library? ${library.has(book2) ? 'YES' : 'NO'}`);
// 'Is The Book Thief' by Markus Zusak in the library? NO

// Add book2 to set.
library.add(book2);
// Delete book1 from set.
library.delete(book1);

// Determine if books exists in set.
console.log(`'Is ${book1.title}' by ${book1.author} in the library? ${library.has(book1) ? 'YES' : 'NO'}`);
// 'Is To Kill a Mockingbird' by Harper Lee in the library? NO
console.log(`'Is ${book2.title}' by ${book2.author} in the library? ${library.has(book2) ? 'YES' : 'NO'}`);
// 'Is The Book Thief' by Markus Zusak in the library? YES
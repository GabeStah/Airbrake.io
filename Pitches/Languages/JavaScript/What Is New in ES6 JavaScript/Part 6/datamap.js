class Book {
    constructor(title, author, pageCount, wordCount) {
        this.title = title;
        this.author = author;
        this.pageCount = pageCount;
        this.wordCount = wordCount;                
    }

    get wordsPerPage() {
        return this.wordCount / this.pageCount;
    }

    static wordsPerPage(pageCount, wordCount) {
        return wordCount / pageCount;
    }
}

// ES5 Mapping Example
// Create book objects.
var book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
var book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create parent library object.
var library = {};

// Set published dates using book objects as keys.
library[book1] = new Date(1960, 6, 11);
library[book2] = new Date(2005, 0, 1);

// Get published dates.
console.log(`'${book1.title}' by ${book1.author} published on ${library[book1].toDateString()}.`);
console.log(`'${book2.title}' by ${book2.author} published on ${library[book2].toDateString()}.`);

// Output all key/value pairs in library object.
for (var key in library) {
    if (library.hasOwnProperty(key)) {
        console.log(`${key}: ${library[key]}`);
    }
}
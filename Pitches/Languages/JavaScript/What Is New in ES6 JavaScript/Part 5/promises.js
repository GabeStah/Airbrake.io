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

    // Perform basic CRUD queries regarding this instance.
    queryDatabase(type) {
        type = type || 'CREATE';
        // Return a new Promise to be asynchronously handled elsewhere.
        return new Promise((resolve, reject) => {
            // Perform async task here.
            // Using setTimeout() to simulate an async call by delaying for 1 second.            
            if (type == 'CREATE') {
                setTimeout(() => {
                    resolve(`Creating [${this.title} by ${this.author}] in database.`);
                }, 1000);                
            } else if (type == 'READ') {
                setTimeout(() => {
                    resolve(`Reading [${this.title} by ${this.author}] from database.`);
                }, 1000);  
            } else if (type == 'UPDATE') {
                setTimeout(() => {
                    resolve(`Updating [${this.title} by ${this.author}] in database.`);
                }, 1000);  
            } else if (type == 'DESTROY') {
                setTimeout(() => {
                    reject(new Error(`Destruction of [${this.title} by ${this.author}] failed. Book not found.`));
                }, 1000);  
            }
        });
    }
}

// Create a new Book instance.
let book = new Book("The Stand", "Stephen King", 823, 472376);

// Query the database for book.
// First READ the existing record.
// Once the read is successful then perform an UPDATE.
// Output the resolve messages for each call.
book.queryDatabase('READ').then((message) => {
    console.log(message);
    return book.queryDatabase('UPDATE');
}).then(
    (message) => console.log(message)
);

// Create a new Book instance.
let book2 = new Book("The Hobbit", "J.R.R. Tolkien", 320, 95022);

// Try to destroy book database record.
book2.queryDatabase('DESTROY').then(
    // Output the resolve message, if applicable.
    (message) => console.log(message),
    // Catch any errors or failures and output that message instead.
    (errorMessage) => console.log(errorMessage)
);

// Create a new Book instance.
let book3 = new Book("The Name of the Wind", "Patrick Rothfuss", 662, 250000);

// Ensure that CREATE, READ, and UPDATE are all successful.
Promise.all([
    book3.queryDatabase('CREATE'),
    book3.queryDatabase('READ'),
    book3.queryDatabase('UPDATE')
]).then(
    (message) => console.log(`CREATE, READ, and UPDATE succeeded for [${book3.title} by ${book3.author}].`),
    (errorMessage) => console.log(errorMessage)
);

Promise.all([
    book3.queryDatabase('CREATE'),
    book3.queryDatabase('READ'),
    book3.queryDatabase('UPDATE'),
    book3.queryDatabase('DESTROY')
]).then(
    (message) => console.log(`CREATE, READ, UPDATE, and DESTROY succeeded for [${book3.title} by ${book3.author}].`),
    (errorMessage) => console.log(errorMessage)
);

// Create a new Book instance.
let book4 = new Book("Seveneves", "Neal Stephenson", 880, 272800);

// Check which operation is fulfilled (or rejected) first.
Promise.race([
    book4.queryDatabase('CREATE'),
    book4.queryDatabase('READ'),
    book4.queryDatabase('UPDATE'),
    book4.queryDatabase('DESTROY')
]).then(
    (message) => console.log(message),
    (errorMessage) => console.log(errorMessage)
);
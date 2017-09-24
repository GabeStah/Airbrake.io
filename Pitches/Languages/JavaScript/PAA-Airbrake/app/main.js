class Book {
    get author() {
        return this.get('_author');
    }

    set author(value) {
        this.set('_author', value);
    }

    get pageCount() {
        return this.get('_pageCount');
    }

    set pageCount(value) {
        this.set('_pageCount', value);
    }

    get title() {
        return this.get('_title');
    }

    set title(value) {
        this.set('_title', value);
    }
    constructor(title, author, pageCount) {
        this.author = author;
        this.pageCount = pageCount;
        this.title = title;
    }

    /**
     * General getter.
     *
     * @param property
     * @returns {*}
     */
    get(property) {
        return this[property];
    }

    /**
     * General setter.
     *
     * Used to simulate different property set results,
     * depending on property that is modified and existing values.
     *
     * @param property
     * @param value
     */
    set(property, value) {
        return new Promise((resolve, reject) => {
            if (property === '_title') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    this[property] = value;
                    // Set title to new value no matter what.
                    if (typeof previousValue === 'undefined') {
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        resolve(`Updated Title from '${previousValue}' to '${this.title}'.`);
                    }
                }, 1000);
            } else if (property === '_author') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    // Set author to new value, if no author property is defined.
                    if (typeof previousValue === 'undefined') {
                        this[property] = value;
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        // If author is already defined, reject update
                        // and throw new Error.
                        reject(new Error(`Cannot update Author from ${previousValue} to ${value}.`));
                    }
                }, 1000);
            }
        });
    }

    /**
     * Output Book to formatted string.
     *
     * @returns {string}
     */
    toString() {
        return `'${this.title}' by ${this.author} (${this.pageCount} pgs)`;
    }
}

// main.js
require(['airbrakeJs/client'], function (AirbrakeClient) {
    let airbrake = new AirbrakeClient({
        projectId: 157536,
        projectKey: 'e6b2c1bd63c0c26ab5751a7ce89d2757'
    });

    //testPromise();
    //testPromiseWithAirbrake(airbrake);
    // testAsyncAwait();
    testAsyncAwaitWithAirbrake(airbrake);
});

const testPromise = () => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => console.log(err)
    );
};

const testPromiseWithAirbrake = (airbrake) => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise w/ Airbrake Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise w/ Airbrake Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => {
            // Handle error with Airbrake.
            let promise = airbrake.notify(err);
            promise.then(
                (notice) => console.log('Airbrake Notice Id:', notice.id),
                (noticeError) => console.log('Airbrake Notification Failed:', noticeError)
            );
        }
    );
};

const testAsyncAwait = async () => {
    // With Async/Await, can use inline try-catch block.
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await Title');
        await book.set('_author', 'Await Author');
    } catch (err) {
        console.log(err);
    }
};

const testAsyncAwaitWithAirbrake = async (airbrake) => {
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await w/ Airbrake Title')
        await book.set('_author', 'Await w/ Airbrake Author');
    } catch (err) {
        // Handle error with Airbrake, by awaiting promise from notify.
        await airbrake.notify(err).then(
            (notice) => console.log('Airbrake Notice Id:', notice.id)
        );
    }
};
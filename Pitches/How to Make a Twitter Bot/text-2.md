# Create a Serverless Twitter Bot with Airbrake and AWS Lambda - Part 2

In [Part 1](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-1) we setup our development environment, created our `twitter-bot` project, integrated it with the [Twitter API](https://developer.twitter.com/en/docs) so it could send out programmatic tweets, and performed a basic `Atom` feed retrieval for some actual content to tweet.

Today, we'll continue refining our Twitter bot by integrating automatic error handling with <a class="js-cta-utm" href="https://airbrake.io/languages/nodejs-error-monitoring?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-serverless-twitter-bot">Airbrake's Node.js</a> software.  This will ensure our Twitter bot is always stable and executing as expected, since we'll be alerted immediately if something goes wrong.  We'll then begin the process of getting our application into a serverless environment with AWS Lambda, before we finish everything up in Part 3 later this week.  You can also view the full source code for this project at anytime [on GitHub](https://github.com/GabeStah/twitter-bot), so with that let's jump right in!

## Getting Started with Airbrake

To begin using the [`Airbrake-JS`](https://github.com/airbrake/airbrake-js) module we need to [Create an Airbrake account](https://airbrake.io/account/new), sign in, and make a new project.

Now we just need to locally install `Airbrake-JS` package.  We'll do this in a console from our project root directory using the `npm install --save airbrake-js` command:

```bash
$ npm install --save airbrake-js
npm WARN twitter-bot@1.0.0 No repository field.

+ airbrake-js@1.0.0
added 1 package in 0.879s
```

Alternatively, we could also install it by adding `airbrake-js` to our `package.json` dependencies:

```json
{
  "dependencies": {
    "airbrake": "^2.0.1"
  }
}
```

### Handling Secret Keys

We need to instantiate the Airbrake client object in our application by passing the `projectId` and `projectKey` values obtained from the `twitter-bot` project we created on the Airbrake dashboard.  However, we don't want to publicly expose these values, so we'll implement the same technique we used in [Part 1](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-1) to hide the Twitter API secret keys.

Let's start by adding `airbrake-credentials.js` to our `.gitignore` file, since this is the file we'll use to hold our private `projectId` and `projectKey` values.  Our `.gitignore` file should look something like this:

```
### .gitignore ###
# Airbrake credentials
airbrake-credentials.js

# Twitter API credentials
twitter-api-credentials.js

# IDEA directory
.idea/
```

Now, let's create the `airbrake-credentials.js` file, then copy and paste the `projectId` and `projectKey` values into the appropriate properties, similar to what we did in `twitter-api-credentials.js`:

```js
// airbrake-credentials.js
module.exports = {
    projectId: "PROJECT_ID",
    projectKey: "PROJECT_API_KEY",
};
```

### Integrating Airbrake-JS

We can now `require` the `airbrake-credentials.js` file in our application code and pass the exported object to the `AirbrakeClient` constructor.  We'll start by requiring `airbrake-js` and `airbrake-credentials.js`.  We'll then pass the credentials to `new AirbrakeClient(...)` to create the actual client object we can use in our code:

```js
// index.js
const airbrake_credentials = require('./airbrake-credentials');
const AirbrakeClient = require('airbrake-js');

// Use exported secret credentials.
let airbrake = new AirbrakeClient(airbrake_credentials);
```

That's all there is to using `airbrake-js` with default settings!  All thrown `Errors` will be detected by `airbrake-js` and will be instantly, automatically reported to you via the Airbrake project dashboard (and also via email or other service integrations you may have setup).  For our purposes we'll add a simple `tweet(message)` function that attempts to tweet the passed `message` argument:

```js
/**
 * Tweet the passed string message.
 *
 * @param message String to be tweeted.
 */
function tweet(message) {
    if (message === null || message === '') return;
    twitter.post(
        'statuses/update',
        {
            status: message
        },
        tweetCallback
    );
}
```

We've also started to clean up the code a bit by creating the `tweetCallback(error, tweet, response)` function, which is invoked as the callback for the `twitter.post` method call used throughout our code:

```js
/**
 * Callback for tweet attempts.
 *
 * @param error Caught error.
 * @param tweet Tweet.
 * @param response Response.
 */
function tweetCallback(error, tweet, response) {
    if(error) {
        console.log(error);
        throw new Error(error[0].message);
    }
    console.log('---- TWEETED ----');
    console.log(tweet);
}
```

The `error` object passed to `tweetCallback(error, tweet, response)` is actually a one-dimensional array, so we explicitly `throw` a `new Error` and pass the `message` property of the underlying error object that was caught, if applicable.  Otherwise a successful tweet was posted, so we log that to the console.

We can test this by calling `tweet(message)`:

```js
tweet('Hello world.');
```

This produces a [successful tweet](https://twitter.com/AirbrakeArticle/status/947983527877885952):

```
---- TWEETED ----
{ created_at: 'Tue Jan 02 00:11:00 +0000 2018',
  id: 947983527877886000,
  id_str: '947983527877885952',
  text: 'Hello world.',
...
```

However, let's see what happens if we try to send the same tweet again:

```js
tweet('Hello world.');
```

Something has gone wrong.  The console shows the following error array object:

```
[ { code: 187, message: 'Status is a duplicate.' } ]
```

More importantly, `airbrake-js` immediately picked up on the error and reported the issue to me via the Airbrake project dashboard (and via email, which is the default behavior).  The Airbrake dashboard shows a great deal of useful information about the error:

- `First Seen`: 8 seconds ago
- `Last Seen`: 8 seconds ago
- `Occurrences`: 1
- `Error Type`: Error
- `Severity`: error
- `File`: index.js:99
- `Error Message`: Status is a duplicate.


Clicking on the specific `Occurrence` tab in the dashboard shows the full details of the caught error.  For example, `airbrake-js` picks up the full `backtrace`, which shows us that the error occurred in the `tweetCallback(error, tweet, response)` function in our `index.js` file on line `99`:

```
index.js:99:15 in tweetCallback
node_modules/twitter/lib/twitter.js:215:14 in Request._callback
node_modules/request/request.js:186:22 in Request.self.callback
events.js:126:13 in emitTwo
events.js:214:7 in Request.emit
node_modules/request/request.js:1163:10 in Request.<anonymous>
events.js:116:13 in emitOne
events.js:211:7 in Request.emit
node_modules/request/request.js:1085:12 in IncomingMessage.<anonymous>
events.js:313:30 in Object.onceWrapper
```

It also reports a great deal of contextual info, including all recent `console.log` messages.  With <a class="js-cta-utm" href="https://airbrake.io/languages/nodejs-error-monitoring?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-serverless-twitter-bot">Airbrake-JS</a> added to our project we can now ensure all future errors, whether during development or in production, are instantly reported.

We'll finish up our `Airbrake-JS` integration by committing the recent changes to Git:

```bash
$ git add .
$ git commit -am "Added and integrated Airbrake-JS."
[master 873f9c5] Added and integrated Airbrake-JS.
 20 files changed, 2753 insertions(+), 12 deletions(-)
...

$ git push
Counting objects: 26, done.
...
   b0773cf..873f9c5  master -> master
```

## AWS Lambda

Everything is working so far as expected.  Our application is able to successfully retrieve a random Airbrake article, connect to Twitter, and submit a new tweet.  However, we want to be able to let our bot go and have it execute its own code when appropriate, such as on a regular schedule.  To accomplish this we'll be using [AWS Lambda](https://aws.amazon.com/lambda/), which allows you to run code without worrying about servers or infrastructure.

Unlike the typical scenario, where server hardware is paid for and configured to host application code, AWS Lambda is a `serverless` technology that hosts your code within tiny, self-contained functions, which can be triggered by a wide variety of events.  Instead of paying for servers, you just pay for compute time.  This sort of setup is perfect for infrequent, small applications like our little Twitter bot.

### Lambda Function Handler Basics (for Node.js)

To actually exceute your code, AWS Lambda invokes the [Node function](https://docs.aws.amazon.com/lambda/latest/dg/nodejs-prog-model-handler.html) you specify as the `handler`.  This handler is simply a function in your code that should be exported, using the standard `exports` statement.  The typical syntax for an exported `handler` is:

```js
exports.myHandler = function(event, context) {
   // ...
}
```

The `event` parameter is used by AWS Lambda to pass event data to the handler.  The `context` parameter is used by AWS Lambda to [provide runtime information](https://docs.aws.amazon.com/lambda/latest/dg/nodejs-prog-model-context.html), such as logging info, timeout duration, request id, and so forth.  The `context` object also includes `succeed` and `fail` methods, which can be explicitly invoked in your own code to inform Lambda that a function call has succeeded or failed, respectively.

Since you must explicitly tell AWS Lambda which particular `handler` to invoke within any given Lambda function, it's a smart practice to `export` and expose multiple `handler` functions, and then you can invoke whatever `handler` is relevant.

### Coding Lambda-Compatible Handler Functions

We now need to modify our existing application code so that it can effectively export the `handler` functions we want to expose to AWS Lambda.  These functions need to have a few capabilities.  The first capability our function needs is a way to ensure that execution of a `handler` function is completely self-contained, meaning that any outside libraries can also be accessed by AWS Lambda.

The second requirement is that the code is _contextually sound_, meaning that it will make proper use of the AWS Lambda-specific `context` parameter we briefly discussed above.  Since AWS Lambda is priced based on computation time and memory, it's critical that our code only executes for as long as absolutely necessary.  If something goes wrong or a request is successful, we want to immediately inform AWS Lambda of the result so we can quickly halt execution.  It may seem minor, but a savings of even a few milliseconds for each function call will add up quickly for computations that are performed hundreds or thousands of times.

Therefore, the first change we need to make to our application code is to streamline the `tweet(message)` function and allow it to provide `context` information during the underlying `twitter.post(...)` method callback.  Here is the modified `tweet(...)` function:

```js
/**
 * Tweet the passed string message.
 *
 * @param message String to be tweeted.
 * @param succeed Success callback function.
 * @param fail Failure callback function.
 */
function tweet(message, succeed, fail) {
    if (message === null || message === '') {
        fail('Blank message cannot be tweeted.');
        return;
    }
    twitter.post(
        'statuses/update',
        {
            status: message
        },
        // Callback function after response or failure.
        function (error, tweet, response) {
            if (error) {
                // If error, output to log.
                console.log(error);
                // Report error to Airbrake.
                airbrake.notify(error[0]).then(() => {
                    // Return fail promise, to inform Lambda of failure.
                    return fail(error[0].message);
                });
            } else {
                // If success, output to log.
                console.log('---- TWEETED ----');
                console.log(tweet);
                // Inform Lambda of success.
                succeed(`'${tweet.text}' tweeted.`);
            }
        // Bind passed succeed/fail parameters to callback function params.
        }.bind( {succeed: succeed, fail: fail} )
    );
}
```

The first change is at the top when we check if `message` is valid.  If no `message` is provided we invoke the `fail(...)` function, which you'll see in a moment is the `context.fail(...)` function provided by AWS Lambda.  Invoking this will immediately halt execution of our function and inform Lambda of the stop.

We've also reverted the `twitter.post(...)` callback function to an anonymous function, as this allows us to use the built-in [`Function.prototype.bind()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function/bind) method, so we can include the two new `succeed` and `fail` parameters in the callback function context.  Since we don't want to modify the `twitter` package code, this is the next best option for being able to invoke the `succeed` and `fail` functions within the callback code block.

Within said callback function we check if an `error` parameter exists.  If so, we output it to the log and then explicitly invoke the `airbrake.notify(...)` method and pass in the generated error.  Since `airbrake.notify(...)` returns a [`promise`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise) instance, we can chain the `then()` method, which will invoke the returned `fail(...)` promise _after_ `airbrake.notify(...)` processes the error.  The result of this code is that we are able to immediately report the generated error via Airbrake while this code is running remotely on AWS Lambda, and once the asynchronous Airbrake notification has completed, we then halt our Lambda function execution and report the error to Lambda.

On the other hand, if no error exists we merely output the tweet to the log and invoke the `succeed(...)` function, informing Lambda that the function has completed.

The first AWS Lambda `handler` function we're exporting is a simple test function called `tweetHelloWorld(event context)`, which does just what the name suggests:

```js
/**
 * Tweets Hello world! via AWS Lambda.
 *
 * @param event AWS Lambda event data for the handler.
 * @param context AWS Lambda runtime information.
 */
exports.tweetHelloWorld = function(event, context) {
    tweet('Hello World', context.succeed, context.fail);
};
```

As mentioned, it's a good idea to export multiple `handler` functions that you think you might want to use for testing or what not, since we can explicitly choose which `handler` is executed by AWS Lambda.  As such, let's also create the `tweetTime(event, context)` function:

```js
/**
 * Tweets the current time via AWS Lambda.
 *
 * @param event AWS Lambda event data for the handler.
 * @param context AWS Lambda runtime information.
 */
exports.tweetTime = function(event, context) {
    let currentdate = new Date();
    tweet(currentdate.toLocaleTimeString(), context.succeed, context.fail);
};
```

Both of these basic `handler` functions will be useful for testing, since one of them will attempt to tweet the same value each time, while the other will always be something new.

The final `handler` function is, of course, `tweetRandomArticle(event, context)`, which is the primary function we're looking to have AWS Lambda execute.  This function needs to be able to perform the entire process of retrieving articles, selecting a random one, and tweeting the selected article.  As such, we've moved a lot of the event handler code that previously existed _outside_ the context of a function, and stuck it directly inside this function:

```js
/**
 * Tweets a random Airbrake article via AWS Lambda.
 *
 * @param event AWS Lambda event data for the handler.
 * @param context AWS Lambda runtime information.
 */
exports.tweetRandomArticle = function(event, context) {
    let feedparser = new FeedParser();
    let feed = request('https://airbrake.io/blog/feed/atom');

    /**
     * Fires when feed request receives a response from server.
     */
    feed.on('response', function (response) {
        if (response.statusCode !== 200) {
            let error = new Error('Bad status code.');
            this.emit('error', error);
        } else {
            // Pipes request to feedparser for processing.
            this.pipe(feedparser);
        }
    });

    /**
     * Invoked when feedparser completes processing request.
     */
    feedparser.on('end', function () {
        tweetArticle(getRandomArticle(), context.succeed, context.fail);
    });

    /**
     * Executes when feedparser contains readable stream data.
     */
    feedparser.on('readable', function () {
        let article;

        // Iterate through all items in stream.
        while (article = this.read()) {
            // Output each Article to console.
            console.log(`Gathered '${article.title}' published ${article.date}`);
            // Add Article to collection.
            articles.push(article);
        }
    });
};
```

We also have to move the `FeedParser()` and `request(...)` calls into this function context, since we don't want these calls to be invoked at any other time _except_ when `tweetRandomArticle(event, context)` is called.  Other than that, most of the code is the same as before -- we start by gathering article from the feed, select a random one, and then pass it to `tweetArticle(article, succeed, fail)`:

```js
/**
 * Tweet the passed Article object.
 *
 * @param article Article to be tweeted.
 * @param succeed Success callback function.
 * @param fail Failure callback function.
 */
function tweetArticle(article, succeed, fail) {
    if (article === null) return;
    // Invoke base tweet function.
    tweet(`${article.title} ${article.link}`, succeed, fail);
}
```

---

That'll do for Part 2 of this series.  Stay tuned for Part 3 later this week, where we'll finish up by packaging and integrating our application code into AWS Lambda by creating functions and handlers, testing everything, and then finally getting a fully automated, error-managed, serverless Twitter bot up and running!

---

__META DESCRIPTION__

Part 2 of a full walkthrough for how to create a completely serverless Twitter bot in Node, with automatic error tracking via Airbrake and AWS Lambda.

---

__SOURCES__

- http://joelgrus.com/2015/12/29/polyglot-twitter-bot-part-1-nodejs/
- https://www.npmjs.com/package/twitter
- https://medium.com/@emckean/create-a-simple-free-text-driven-twitterbot-with-aws-lambda-node-js-b80e26209f5
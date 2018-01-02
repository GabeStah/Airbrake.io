TODO: Check intro article coverage.
TODO: Add outro CTA.

# Create a Serverless Twitter Bot with Airbrake and AWS Lambda - Part 2

In [Part 1](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-1) we setup our development environment, created our `twitter-bot` project, integrated it with the [Twitter API](https://developer.twitter.com/en/docs) so it could send out programmatic tweets, and performed a basic `Atom` feed retrieval for some actual content to tweet.

Today, we'll continue refining our Twitter bot by integrating automatic error handling with <a class="js-cta-utm" href="https://airbrake.io/languages/nodejs-error-monitoring?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-serverless-twitter-bot">Airbrake's Node.js</a> software.  This will ensure our Twitter bot is always stable and executing as expected, since we'll be alerted immediately if something goes wrong.  Let's jump right in!

## Getting Started with Airbrake

To begin using the [`Airbrake-JS`](https://github.com/airbrake/airbrake-js) module we need to [Create an Airbrake account](https://airbrake.io/account/new), sign in, and make a new project.

Now we just need to locally install `Airbrake-JS` package.  We'll do this in a console from our project root directory using the `npm install --save airbrake-js` command:

```bash
$ npm install --save airbrake-js
npm WARN twitter-bot@1.0.0 No repository field.

+ airbrake-js@1.0.0
added 1 package in 0.879s
```

Alternatively, we could also install by adding `airbrake-js` to our `package.json` dependencies:

```json
{
  "dependencies": {
    "airbrake": "^2.0.1"
  }
}
```

### Handling Secret Keys

We need to instantiate the Airbrake client object in our application by passing the `projectId` and `projectKey` values obtained from the `twitter-bot` project we created on the Airbrake dashboard.  However, we don't want to publically expose these values, so we'll implement the same technique we used in [Part 1](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-1) to hide the Twitter API secret keys.

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

We can now `require` the `airbrake-credentials.js` file in our application code and pass the exported object to the `AirbrakeClient` instantiation.  We'll start by requiring `airbrake-js` and `airbrake-credentials.js`.  We'll then pass the credentials to `new AirbrakeClient(...)` to create the actual client object we can use in our code:

```js
// index.js
const airbrake_credentials = require('./airbrake-credentials');
const AirbrakeClient = require('airbrake-js');

// Use exported secret credentials.
let airbrake = new AirbrakeClient(airbrake_credentials);
```

That's all there is to using `airbrake-js` with default settings!  All thrown `Errors` will be detected by `airbrake-js` and will be instantly, automatically reported to you via the Airbrake project dashboard (along with email or other service integrations you may have setup).  For our purposes we'll add a simple `tweet(message)` function that attempts to tweet the passed `message` argument:

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

The `error` object passed to `tweetCallback(error, tweet, response)` is actually an single-dimension array, so we explicitly `throw` a `new Error` and pass the `message` property of the underlying error object that was caught, if applicable.  Otherwise a successful tweet was posted, so we log that to the console.

We can test this by calling `tweet(message)`:

```js
tweet('Hello world.');
```

This produces a [succesful tweet](https://twitter.com/AirbrakeArticle/status/947983527877885952):

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

Now something has gone wrong.  The console shows the following error array object:

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

We'll finish up our `Airbrake-JS` integration by commiting the recent changes to Git:

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

Everything is working so far as expected.  Our application is able to successfully retrieve a random Airbrake article, connect to Twitter, and submit a new tweet 

### Lambda Function Handler Basics (for Node.js)

AWS Lambda invokes the [Node function](https://docs.aws.amazon.com/lambda/latest/dg/nodejs-prog-model-handler.html) you specify as the `handler`.  This handler is simply a function in your code that should be exported, using the standard `exports` statement.  The typical syntax for an exported `handler` is:

```js
exports.myHandler = function(event, context) {
   // ...
}
```

The `event` parameter is used by AWS Lambda to pass event data to the handler.  The `context` parameter is used by AWS Lambda to [provide runtime information](https://docs.aws.amazon.com/lambda/latest/dg/nodejs-prog-model-context.html), such as logging info, timeout duration, request id, and so forth.  The `context` object also includes `succeed` and `fail` methods, which can be explicitly invoked in your own code to inform Lambda that a function call has succeeded or failed, respectively.

Since you must explicitly tell AWS Lambda which particular `handler` to invoke within any given Lambda function, it's a smart practice to `export` and expose multiple `handler` functions, and then you can invoke just the particular `handler` that is relevant.

### Coding Lambda-Compatible Handler Functions

We now need to modify our existing application code so that it can effectively export the `handler` function(s) we want to expose to AWS Lambda.  These functions need to have a few capabilities.  The first of which is we need to ensure that execution of a `handler` function is completely self-contained, meaning that any outside libraries can also be accessed by AWS Lambda.  The second requirement is that the code is _contextually sound_, meaning that it makes proper use of the AWS Lambda-specific `context` parameter we briefly discussed above.  Since AWS Lambda is priced based on computation time and memory, it's critical that our code only executes for as long as absolutely necessary.  If something goes wrong or a request is successfuly, we want to immediately inform AWS Lambda of the success or failure so execution is halted.  It may seem minor, but a savings of even a couple milliseconds for each function call will add up quickly for functions executed many thousands of times.

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

The first change is at the top when we check if `message` is valid.  If no `message` is provided we invoke the `fail(...)` function, which you'll see in a moment is the `context.fail(...)` function passed in by AWS Lambda.  Invoking this will immediately halt execution of our function and inform Lambda of the stop.

We've also reverted the `twitter.post(...)` callback function to an anonymous function, as this allows us to use the built-in [`Function.prototype.bind()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function/bind) method to include the two new `succeed` and `fail` parameters in the callback function context.  Since we don't want to modify the `twitter` package code, this is the next best option for being able to invoke the `succeed` and `fail` functions within the callback code block.

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

The final `handler` function is, of course, `tweetRandomArticle(event, context)`, which is the primary function we're looking to have AWS Lambda execute.  This function needs to be able to perform the entire process of retrieving articles, selecting a random one, and tweeting the selected article.  As such, we've moved a lot of the event handler code that existing outside the context of a function before directly into this function:

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

We also have to move the `FeedParser()` and `request(...)` calls into this function context, since we don't want these calls to be invoked at any other time _except_ when `tweetRandomArticle(event, context)` is called.  Otherwise, most of the code is the same as before, where we start by gathering article from the feed, select a random one, and then pass it to `tweetArticle(article, succeed, fail)`:

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

### Packaging Our Application Code

AWS Lambda allows us to write code directly on their dashboard, but we _must_ ensure that all dependencies that our Node.js application relies upon are included and available to AWS Lambda.  Therefore, the solution is to package our entire project directory into a `zip` file, which we can then upload directly to AWS Lambda.  This will ensure that the `node_modules` directory is included with our `index.js` and other application code.

Navigate to your project directory in the console and enter the following command:

```bash
$ zip -r twitter-bot.zip *
  adding: airbrake-credentials.js (deflated 9%)
  adding: index.js (deflated 64%)
  adding: node_modules/ (stored 0%)
...
```

### Create AWS Lambda Function

To create an AWS Lambda function, start by navigating to the AWS Lambda console and then click `Create Function`.

Make sure `Author from scratch` is selected, since we'll be using our own custom code and exported `handler`.  For this example my function is named `airbrake-article-twitter-bot`.

For the `Runtime` dropdown you'll want to use the latest version of `Node.js`, or the one closest to your own version.  At the time of writing `Node.js 6.10` is the latest supported version.

If this is your first Lambda function you'll likely need to `Create a new role from template(s)` in the `Role` dropdown.  Then enter a descriptive `Role name`, such as `airbrakeArticleTwitterBotRole`.  For this basic bot we don't need any advanced permissions at present, so leave `Policy templates` blank and click `Create function`.

Once created you'll be looking at the `airbrake-article-twitter-bot` function dashboard, which includes the `Designer` where a graphical layout of your function is displayed.  Below that is the `Function code` screen, which defaults to a default `handler`.  From there, most of the sections can be ignored, for the moment.

To upload the `zip` package we created simply click on the `Function code > Code entry type` dropdown box and select `Upload a .ZIP file`, then click `Upload` and select the `twitter-bot.zip` file.

Next, we need to tell AWS Lambda which particular `handler` function it should invoke when executing this function, so we'll change `Function code > Handler` to `index.tweetHelloWorld`.  The `index` portion is the name of the app file (`index.js`), and the second portion is the `handler` function.  Now click `Save` at the top right and the `zip` will be uploaded!

### Testing a Lambda Function

Our code is uploaded and ready to go, but we need to actually tell AWS Lambda when and how to invoke our `handler` function.  In this case, it's easiest to start with a manual execution test.

Click the `Test` button at the top right, select `Create new test event`, choose `Hello World` from the `Event template` dropdown, then enter any `Event name` you wish and click `Create`.  The actual parameters of the test aren't relevant to this particular example, but feel free to change them before creating your test.

Now your new test will be selected in the test dropdown, so just click the `Test` button and this will invoke the AWS Lambda function, which will call the `handler` function in your code (`index.tweetHelloWorld`, in this case).

Once execution completes you'll see the result dialog at the top, which can be expanded for more detail.  In this case, the function result shows:

```
"'Hello World' tweeted."
```

And the log output, which is automatically tracked by AWS Lambda and stored in Amazon CloudWatch Logs, shows:

```
START RequestId: b4ed494c-ef6e-11e7-a6f0-f73d0ba7827e Version: $LATEST
2018-01-02T03:40:43.288Z	b4ed494c-ef6e-11e7-a6f0-f73d0ba7827e	---- TWEETED ----
2018-01-02T03:40:43.325Z	b4ed494c-ef6e-11e7-a6f0-f73d0ba7827e	{ created_at: 'Tue Jan 02 03:40:43 +0000 2018',
  id: 948036303739789300,
  id_str: '948036303739789312',
  text: 'Hello World',
  ...
END RequestId: b4ed494c-ef6e-11e7-a6f0-f73d0ba7827e
REPORT RequestId: b4ed494c-ef6e-11e7-a6f0-f73d0ba7827e	Duration: 904.52 ms	Billed Duration: 1000 ms 	Memory Size: 128 MB	Max Memory Used: 41 MB	
```

As expected, our tweet was successfully created by AWS Lambda and shows up on the [AirbrakeArticles Twitter page](https://twitter.com/AirbrakeArticle/status/948036303739789312)!

### AWS Lambda and Airbrake Error Handling

As you may recall from before, trying to send out a second, identical tweet results in a rejected request from the Twitter API, which indicates that the status is a duplicate.  Therefore, this is a great opportunity to test our `Airbrake-JS` integration with AWS Lambda.  To do so just execute the same `Test` once again, which will invoke the `index.tweetHelloWorld` handler a second time and attempt to tweet `"Hello World"` again.

As expected, the result of execution in AWS Lambda is:

```
{
  "errorMessage": "Status is a duplicate."
}
```

The log output confirms the issue:

```
START RequestId: 5b12662e-ef6f-11e7-b472-21ffb7560a15 Version: $LATEST
2018-01-02T03:45:21.006Z	5b12662e-ef6f-11e7-b472-21ffb7560a15	[ { code: 187, message: 'Status is a duplicate.' } ]
2018-01-02T03:45:21.520Z	5b12662e-ef6f-11e7-b472-21ffb7560a15	{"errorMessage":"Status is a duplicate."}
END RequestId: 5b12662e-ef6f-11e7-b472-21ffb7560a15
```

Most importantly, looking over at the Airbrake project dashboard for our `twitter-bot` Node project immediately displays a `Status is a duplicate.` error, with all the same detailed contextual information that we saw previously, when this error occurred in our local development environment.

### Repeating Schedule Lambda Functions

While this is cool so far that we're able to use AWS Lambda to execute our Node.js application code, it's not very useful unless execution can be triggered from another event.  There are many possible ways to trigger Lambda functions, but we'll keep it simple and simply setup a schedule.  To add a scheduled trigger open the `Designer` section in the `airbrake-article-twitter-bot` dashboard and select `CloudWatch Events`.  This will add `CloudWatch Events` to the trigger side of the visual display, while also opening the `Configure triggers` panel where you'll actually specify how the trigger will behave.

Select `Create a new rule` from the `Rule` dropdown and enter a descriptive name.  For testing purposes we'll be creating a trigger that occurs once a minute, so we'll name this rule `every-minute`.  We can now specify the schedule in the `Schedule expression` box, using `Cron` or `rate` expressions.  We don't need anything complex, so we'll just use `rate(1 minute)` to trigger this function every 60 seconds.  Finally, you may want to uncheck `Enable trigger` for now so the trigger is only enabled when you wish.  Click `Add` to add the trigger.

We also want to change the `handler` function that is being executed to something that won't automatically produce an error.  To modify our function code again just click on the visual `airbrake-article-twitter-bot` box in the `Designer` panel.  Under `Function code > Handler` enter `index.tweetTime`, then click `Save`.  If you want to manually test the function again you can click `Test` and ensure the result is successful:

```
"'3:56:56 AM' tweeted."
```

However, our goal is to schedule the function to trigger on its own, so select `CloudWatch Events` in the `Designer` panel, `Enable` the `every-minute` trigger, then click `Save` to confirm the changes.  Now. we just sit back and wait a minute or two and we'll have an automatically scheduled trigger causing our AWS Lambda function to execute, which will generate a tweet of the current time every minute.  We can confirm this either by checking for the [produced tweets](https://twitter.com/AirbrakeArticle/status/948040754030448646), or open the `CloudWatch Logs` page on AWS Lambda:

```
2018-01-02T03:58:24.279Z	2dd869f5-ef71-11e7-bace-1515e5380db4	---- TWEETED ----
2018-01-02T03:58:24.285Z	2dd869f5-ef71-11e7-bace-1515e5380db4	{ created_at: 'Tue Jan 02 03:58:24 +0000 2018', id: 948040754030448600, id_str: '948040754030448646', text: '3:58:23 AM', truncated: false, entities: { hashtags: [], symbols: [], user_mentions: [], urls: [] }, source: '<a href="https://github.com/GabeStah/twitter-bot" rel="nofollow">AirbrakeArticles</a>', in_reply_to_statu
END RequestId: 2dd869f5-ef71-11e7-bace-1515e5380db4
REPORT RequestId: 2dd869f5-ef71-11e7-bace-1515e5380db4	Duration: 453.04 ms	Billed Duration: 500 ms Memory Size: 128 MB	Max Memory Used: 42 MB
START RequestId: 51862ce1-ef71-11e7-b70b-05b293363b49 Version: $LATEST
2018-01-02T03:59:24.125Z	51862ce1-ef71-11e7-b70b-05b293363b49	---- TWEETED ----
2018-01-02T03:59:24.125Z	51862ce1-ef71-11e7-b70b-05b293363b49	{ created_at: 'Tue Jan 02 03:59:24 +0000 2018', id: 948041005034438700, id_str: '948041005034438661', text: '3:59:23 AM', truncated: false, entities: { hashtags: [], symbols: [], user_mentions: [], urls: [] }, source: '<a href="https://github.com/GabeStah/twitter-bot" rel="nofollow">AirbrakeArticles</a>', in_reply_to_statu
END RequestId: 51862ce1-ef71-11e7-b70b-05b293363b49
REPORT RequestId: 51862ce1-ef71-11e7-b70b-05b293363b49	Duration: 433.53 ms	Billed Duration: 500 ms Memory Size: 128 MB	Max Memory Used: 43 MB
START RequestId: 7548af02-ef71-11e7-af26-3903d36c0918 Version: $LATEST
2018-01-02T04:00:24.150Z	7548af02-ef71-11e7-af26-3903d36c0918	---- TWEETED ----
2018-01-02T04:00:24.150Z	7548af02-ef71-11e7-af26-3903d36c0918	{ created_at: 'Tue Jan 02 04:00:24 +0000 2018', id: 948041256709496800, id_str: '948041256709496833', text: '4:00:23 AM', truncated: false, entities: { hashtags
```

**Note: If you are experiencing timeout errors indicating that the AWS Lambda function concluded before execution completed, you may need to increase the `timeout` period under the `Basic settings` panel.**

### Finalizing Our Serverless Twitter Bot

Now that we know everything works as expected, the last step is to use the `tweetRandomArticle` `handler` for our AWS Lambda function, and to modify the schedule to something more appropriate.  We'll begin by testing that the `handler` works as expected and actually retrieves and posts a random article.  After changing the `handler` field to `index.tweetRandomArticle` and clicking `Save` we'll check the logs and our AirbrakeArticles Twitter account.

Sure enough, it works as expected and a random article was retrieved and [tweeted automatically](https://twitter.com/AirbrakeArticle/status/948043576490958849)!  Plus, when our bot inevitably chooses a random article that is a duplicate from a previous recent tweet, Airbrake catches and reports the error immediately.

The article retrieval process isn't very efficient, so we don't want to execute this all that often.  Therefore, we'll change our schedule trigger to occur once every twelve hours.

---

__META DESCRIPTION__

A full walkthrough for how to create a completely serverless Twitter bot in Node, with automatic error tracking via Airbrake and AWS Lambda.

---

__SOURCES__

- http://joelgrus.com/2015/12/29/polyglot-twitter-bot-part-1-nodejs/
- https://www.npmjs.com/package/twitter
- https://medium.com/@emckean/create-a-simple-free-text-driven-twitterbot-with-aws-lambda-node-js-b80e26209f5
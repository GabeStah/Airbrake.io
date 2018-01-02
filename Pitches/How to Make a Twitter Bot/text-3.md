# Create a Serverless Twitter Bot with Airbrake and AWS Lambda - Part 3

In [Part 1](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-1) of this series we setup our development environment, created our `twitter-bot` project, integrated it with the [Twitter API](https://developer.twitter.com/en/docs) so it could send out programmatic tweets, and performed a basic `Atom` feed retrieval for some actual content to tweet.  In [Part 2](https://airbrake.io/blog/nodejs/serverless-twitter-bot-airbrake-lambda-part-2) we integrated <a class="js-cta-utm" href="https://airbrake.io/languages/nodejs-error-monitoring?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-serverless-twitter-bot">Airbrake's Node.js</a> software to handle automatic error reporting, plus, we also started the AWS Lambda integration.

Today, we'll finish up this series and project by completing the integration into AWS Lambda by packaging our application code, uploading it to AWS Lambda, creating functions and handlers, testing, and eventually getting a fully automated, error-managed, serverless Twitter bot running, so let's get to it!

### Packaging Our Application Code

AWS Lambda allows us to write code directly within the Lambda dashboard, but we _must_ ensure that all dependencies that our Node.js application relies upon are included and available to AWS Lambda.  Therefore, the solution is to package our entire project directory into a `zip` file, which we can then upload directly to AWS Lambda.  This will ensure that the `node_modules` directory is included with our `index.js`, along with all other application code.

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

Make sure `Author from scratch` is selected, since we'll be using our own custom code and exported `handler`.  For this example I've named my function `airbrake-article-twitter-bot`.

For the `Runtime` dropdown you'll want to use the latest version of `Node.js`, or the one closest to your own version.  At the time of writing `Node.js 6.10` is the latest supported version.

If this is your first Lambda function you'll likely need to `Create a new role from template(s)` in the `Role` dropdown.  Then enter a descriptive `Role name`, such as `airbrakeArticleTwitterBotRole`.  For this basic bot we don't need any advanced permissions, so leave `Policy templates` blank and click `Create function`.

Once created you'll be looking at the `airbrake-article-twitter-bot` function dashboard, which includes the `Designer` panel, where a graphical layout of your function is displayed.  Below that is the `Function code` screen, which includes a default `handler` function.  From there, most of the sections can be ignored for now.

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

Most importantly, looking over at the Airbrake project dashboard for our `twitter-bot` Node project immediately displays a `Status is a duplicate.` error, with all the same detailed contextual information that we saw previously from when this error occurred in our local development environment.

### Repeating Schedule Lambda Functions

While this is cool so far that we're able to use AWS Lambda to execute our Node.js application code, it's not very useful unless execution can be triggered from another event.  There are many possible ways to trigger Lambda functions, but we'll keep it simple by setting up a schedule.  To add a scheduled trigger, open the `Designer` panel in the `airbrake-article-twitter-bot` dashboard and select `CloudWatch Events`.  This will add `CloudWatch Events` to the trigger side of the visual display, while also opening the `Configure triggers` panel where you'll actually specify how the trigger will behave.

Select `Create a new rule` from the `Rule` dropdown and enter a descriptive name.  For testing purposes we'll be creating a trigger that occurs once a minute, so we'll name this rule `every-minute`.  We can now specify the schedule in the `Schedule expression` box, using `Cron` or `rate` expressions.  We don't need anything complex, so we'll just use `rate(1 minute)` to trigger this function every 60 seconds.  Finally, you may want to uncheck `Enable trigger` for now, so the trigger won't be enabled until you do so yourself.  Click `Add` to create the trigger.

We also want to change the `handler` function that is being executed to something that won't automatically produce an error.  To modify our function code again just click on the visual `airbrake-article-twitter-bot` box in the `Designer` panel.  Under `Function code > Handler` enter `index.tweetTime`, then click `Save`.  If you want to manually test the function again you can click `Test` and ensure the result is successful:

```
"'3:56:56 AM' tweeted."
```

Our goal is to schedule the function to trigger on its own, so select `CloudWatch Events` in the `Designer` panel, `Enable` the `every-minute` trigger, then click `Save` to confirm the changes.  Now. we just sit back and wait a minute or two and we'll have an automatically scheduled trigger causing our AWS Lambda function to execute, which will generate a tweet of the current time every minute.  We can confirm this either by checking for the [produced tweets](https://twitter.com/AirbrakeArticle/status/948040754030448646), or open the `CloudWatch Logs` page on AWS Lambda:

```
2018-01-02T03:58:24.279Z	2dd869f5-ef71-11e7-bace-1515e5380db4	---- TWEETED ----
2018-01-02T03:58:24.285Z	2dd869f5-ef71-11e7-bace-1515e5380db4	{ created_at: 'Tue Jan 02 03:58:24 +0000 2018', id: 948040754030448600, id_str: '948040754030448646', text: '3:58:23 AM', truncated: false, entities: { hashtags: [], symbols: [], user_mentions: [], urls: [] }, source: '<a href="https://github.com/GabeStah/twitter-bot" rel="nofollow">AirbrakeArticles</a>', in_reply_to_status
END RequestId: 2dd869f5-ef71-11e7-bace-1515e5380db4
REPORT RequestId: 2dd869f5-ef71-11e7-bace-1515e5380db4	Duration: 453.04 ms	Billed Duration: 500 ms Memory Size: 128 MB	Max Memory Used: 42 MB
START RequestId: 51862ce1-ef71-11e7-b70b-05b293363b49 Version: $LATEST
2018-01-02T03:59:24.125Z	51862ce1-ef71-11e7-b70b-05b293363b49	---- TWEETED ----
2018-01-02T03:59:24.125Z	51862ce1-ef71-11e7-b70b-05b293363b49	{ created_at: 'Tue Jan 02 03:59:24 +0000 2018', id: 948041005034438700, id_str: '948041005034438661', text: '3:59:23 AM', truncated: false, entities: { hashtags: [], symbols: [], user_mentions: [], urls: [] }, source: '<a href="https://github.com/GabeStah/twitter-bot" rel="nofollow">AirbrakeArticles</a>', in_reply_to_status
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

The article retrieval process isn't very efficient, so we don't want to execute this all that often.  Therefore, we'll change our schedule trigger to occur once every twelve hours, but otherwise we're all set.  We now have a fully functional, serverless Twitter bot written with Node.js and automatically triggered and executed within AWS Lambda!

---

__META DESCRIPTION__

Part 3 of a full walkthrough for how to create a completely serverless Twitter bot in Node, with automatic error tracking via Airbrake and AWS Lambda.

---

__SOURCES__

- http://joelgrus.com/2015/12/29/polyglot-twitter-bot-part-1-nodejs/
- https://www.npmjs.com/package/twitter
- https://medium.com/@emckean/create-a-simple-free-text-driven-twitterbot-with-aws-lambda-node-js-b80e26209f5
# How to Make a Serverless Twitter Bot with Airbrake Error Monitoring and AWS Lambda

```bash
~/work$ mkdir twitter-bot
~/work$ cd twitter-bot/
~/work/twitter-bot$ git init
Initialized empty Git repository in /home/gabe/work/twitter-bot/.git
~/work/twitter-bot$ npm init
This utility will walk you through creating a package.json file.
It only covers the most common items, and tries to guess sensible defaults.

See `npm help json` for definitive documentation on these fields
and exactly what they do.

Use `npm install <pkg>` afterwards to install a package and
save it as a dependency in the package.json file.

Press ^C at any time to quit.
package name: (twitter-bot) 
version: (1.0.0) 
description: Basic Twitter bot.
entry point: (index.js)
test command: 
git repository: 
keywords: 
author: Gabe Wyatt <gabe@gabewyatt.com> (http://gabewyatt.com)
license: (ISC) 
About to write to /home/gabe/work/twitter-bot/package.json:

{
  "name": "twitter-bot",
  "version": "1.0.0",
  "description": "Basic Twitter bot.",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "author": "Gabe Wyatt <gabe@gabewyatt.com> (http://gabewyatt.com)",
  "license": "ISC"
}


Is this ok? (yes) 
```

We'll be using the [`twitter`](https://www.npmjs.com/package/twitter) NPM package, so we'll install that (along with its own dependents):

```bash
npm install twitter --save
npm notice created a lockfile as package-lock.json. You should commit this file.
npm WARN twitter-bot@1.0.0 No repository field.

+ twitter@1.7.1
added 54 packages in 1.217s
```

As with most Git repositories, there are some files or filetypes we don't want to expose to the public, so we'll add a [`.gitignore`](https://git-scm.com/docs/gitignore) file to the `twitter-bot` project directory.  __Note: I am using VS Code and WebStorm to develop this project, so the `code` command can be replaced with whatever command you use to open your favorite text editor.__

```bash
$ code .gitignore
```

We'll need to get some authentication API keys from Twitter to be authorized to performated automated tweets and other functionality, but some of this information should be kept private.  We'll be storing these Twitter API keys in the `twitter-api-credentials.js` file, so let's add that explicit file to the `.gitignore` file before we add some secret keys and accidentally commit them to the Git repository:

```
### .gitignore ###
# Twitter API credentials
twitter-api-credentials.js
```

Now, let's double-check that we've setup everything correctly and that our secrets won't be committed by creating the `twitter-api-credentials.js` file and adding some irrelevant text to it.  Here we're using the `>>` Unix command to append the text `12345` to the `twitter-api-credentials.js` file, then outputting the contents of the file using the `cat` command:

```bash
$ touch twitter-api-credentials.js
$ echo '12345' >> twitter-api-credentials.js
$ cat twitter-api-credentials.js
12345
```

Our credentials file is created, so now we should double-check that `.gitignore` is working correctly and won't add `twitter-api-credentials.js` to the repository.  A simple `git status` command should do the trick and show what untracked files are waitined to be added:

```bash
$ git status
On branch master

Initial commit

Untracked files:
  (use "git add <file>..." to include in what will be committed)

	.gitignore
	.idea/
	node_modules/
	package-lock.json
	package.json

nothing added to commit but untracked files present (use "git add" to track)
```

That looks great, since everything except the ignored `twitter-api-credentials.js` is listed there.  Let's get into the smart habit of commiting changes to Git by making our initial commit:

```bash
$ git add .
$ git commit -am "Initial commit."
[master (root-commit) 2f0f181] Initial commit.
 590 files changed, 89356 insertions(+)
 create mode 100644 .gitignore
...
```

We'll also be storing this project on a remote, public [GitHub repository](https://github.com/GabeStah/twitter-bot), so I need to set the origin and use the `git push` command to ensure our changes are uploaded:

```bash
$ git remote add origin git@github.com:GabeStah/twitter-bot.git
$ git push -u origin master
Counting objects: 700, done.
Delta compression using up to 8 threads.
Compressing objects: 100% (652/652), done.
Writing objects: 100% (700/700), 1.14 MiB | 0 bytes/s, done.
Total 700 (delta 94), reused 0 (delta 0)
remote: Resolving deltas: 100% (94/94), done.
To https://github.com/GabeStah/twitter-bot.git
 * [new branch]      master -> master
Branch master set up to track remote branch master from origin.
```

Now we need to actually obtain some Twitter API credentials, so we start by logging into our Twitter account and then visiting [https://apps.twitter.com/](https://apps.twitter.com/).  Here, click `Create New App` then fill out the form as you see fit.  For this example we'll use the following:

- `Name`: AirbrakeArticles
- `Description`: Tweeting random Airbrake.io articles!
- `Website`: https://github.com/GabeStah/twitter-bot

Agree to the agreement then create your app!

Next, we need to create and copy the API keys to the `twitter-api-credentials.js` file for use in our project.  Click on the `Keys and Access Tokens` tab at the top, then click `Create my access token` at the bottom.  Per the [Twitter NPM](https://www.npmjs.com/package/twitter) documentation we need the `consumer_key`, `consumer_secret`, `access_token_key`, and `access_token_secret` values from Twitter.  Open the `twitter-api-credentials.js` and paste the following template into it:

```js
// twitter-api-credentials.js
module.exports = {
  consumer_key: "",
  consumer_secret: "",
  access_token_key: "",
  access_token_secret: ""
};
```

Now, copy and paste each value from the Twitter App page into the appropriate field and save the file, which now has all the secret keys necessary to connect to and use Twitter programatically.

At this point we can actually begin creating our application logic by first testing that our Twitter API connection works.  Start by creating the base application file for your Node project.  The default is usually `index.js`:

```bash
$ code index.js
```

We'll start by requiring the `twitter-api-credentials.js` file that exports our credentials, along with the `twitter` NPM module.  We'll then instantiate a new `Twitter` object and pass the credentials to it, which will perform authentication and authorization for us while using the `Twitter` object:

```js
// index.js
const twitter_credentials = require('./twitter-api-credentials');
const Twitter = require('twitter');

// Use exported secret credentials.
let twitter = new Twitter(twitter_credentials);
```

So, everything is ready to go, but how do we actually _use_ the `twitter` module?  It primarily provides convenience methods for sending `GET` and `POST` HTTP method requests to the [Twitter API](https://developer.twitter.com/en/docs).  For example, if we want to post a tweet we need to send a `POST` request to the `statuses/update` API endpoint, as shown in the [official documentation](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-update).  The API accepts a number of required (and optional) parameters to be included with the request.

To illustrate we'll perform a simple test tweet of `"Am I a robot?"` from our bot account.  Add the following to `index.js`:

```js
// Perform a test tweet.
twitter.post(
    'statuses/update',
    {
        status: 'Am I a robot?'
    },
    function(error, tweet, response) {
        if(error) {
            console.log(error);
            throw error;
        }
        console.log('---- TWEET ----');
        console.log(tweet);
        console.log('---- RESPONSE ----');
        console.log(response);
    }
);
```

Now, to test that everything works just run your Node application.  If all was setup correctly you'll see the output in the console log to confirm it works, as can refresh your Twitter account page to see the new [`"Am I a robot?"` tweet](https://twitter.com/AirbrakeArticle/status/946880037424209921):

```bash
$ node index.js
{ created_at: 'Fri Dec 29 23:12:53 +0000 2017',
  id: 946881740039057400,
  id_str: '946881740039057408',
  text: 'Am I a robot?',
  truncated: false,
  ...
```

## Customizing the Bot

Alright, we've got a working bot, but right now it only does exactly what we tell it.  Let's improve things by customizing it a bit and getting it to do something more interesting.  For this example, we'll use our `AirbrakeArticle` Twitter bot account to tweet out random `Airbrake.io` articles that have been published in the past.  There are many ways to accomplish this, but we'll start with the easiest technique.  Since `https://airbrake.io/blog` uses Wordpress, we can access the `RSS` or `Atom` feed by appending `/feed` or `/feed/atom` to the base URL.  For example, opening [https://airbrake.io/blog/feed/atom](https://airbrake.io/blog/feed/atom) will provides the `Atom` feed of the most recent articles.  Unfortunately, this doesn't give us programmatic access to _historical_ data, since RSS feeds are limited to only the most recent information, but it's a good starting point.

There's little reason to reinvent the wheel, so we'll be using the [feedparser](https://www.npmjs.com/package/feedparser) NPM module to simplify the process of retrieving the latest Airbrake article feed.  The use of the `--save` flag within the `npm install` command forces the package being installed to be automatically added to the `package.json` `dependencies` field, so we don't have to manually add it ourselves:

```bash
$ npm install --save feedparser
npm WARN twitter-bot@1.0.0 No repository field.

+ feedparser@2.2.7
added 15 packages in 0.733s
```

We then need to `require('feedparser')` at the top of `index.js`, along with the `request` built-in module for handling our request to the feed URL (`https://airbrake.io/blog/feed/atom`).  The top of `index.js` should look something like this now:

```js
// index.js
const FeedParser = require('feedparser');
const request = require('request');
const twitter_credentials = require('./twitter-api-credentials');
const Twitter = require('twitter');

let feedparser = new FeedParser();
let feed = request('https://airbrake.io/blog/feed/atom');

// Use exported secret credentials.
let twitter = new Twitter(twitter_credentials);

// Article collection.
let articles = [];
```

We need to respond to a few different events to process the incoming feed request, since it happens asynchronously:

```js
/**
 * Fires when feed request receives a response from server.
 */
feed.on('response', function (response) {
    if (response.statusCode !== 200) {
        this.emit('error', new Error('Bad status code'));
    } else {
        // Pipes request to feedparser for processing.
        this.pipe(feedparser);
    }
});

/**
 * Invoked when feedparser completes processing request.
 */
feedparser.on('end', function () {
    tweetRandomArticle(articles);
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
```

We start with `feed.on('response')`, which fires when the `request(...)` made to the feed URL receives a response.  If we get a `200` error code back, we know it was successful so we pipe the response to the `feedparser` object.  From there, `feedparser.on('readable')` fires when there's a readable stream available, which means we have some feed data to parse.  Within this function we're outputting each article to the log and then adding them to the `articles` collection array.  Finally, `feedparser.on('end')` is invoked when `feedparser` completes the task of reading the stream, so we want to actually produce a tweet of a random `Article` when that occurs.

We've also added the `tweetArticle(article)` and `tweetRandomArticle()` helper functions as simple wrappers for using the `twitter` module to `POST` to the `statuses/update` API endpoint:

```js
/**
 * Tweet the passed Article object.
 *
 * @param article Article to be tweeted.
 */
function tweetArticle(article) {
    if (article == null) return;
    twitter.post(
        'statuses/update',
        {
            status: `${article.title} ${article.link}`
        },
        function(error, tweet, response) {
            if(error) {
                console.log(error);
                throw error;
            }
            console.log('---- TWEETED ARTICLE ----');
            console.log(tweet);
        }
    );
}

/**
 * Tweet a random Article.
 */
function tweetRandomArticle() {
    // Tweet a random article.
    tweetArticle(articles[Math.floor(Math.random()*articles.length)])
}
```

Alright, everything is setup, so let's try running our application again and see what happens:

```bash
$ node index.js
Gathered '410 Gone Error: What It Is and How to Fix It' published Thu Dec 28 2017 19:09:32 GMT-0800 (PST)
Gathered 'Python Exception Handling &#8211; EOFError' published Wed Dec 27 2017 19:21:25 GMT-0800 (PST)
Gathered 'Techniques for Preventing Software Bugs' published Tue Dec 26 2017 14:23:52 GMT-0800 (PST)
...
---- TWEETED ARTICLE ----
{ created_at: 'Sat Dec 30 00:10:26 +0000 2017',
  id: 946896220181561300,
  id_str: '946896220181561344',
  text: '303 See Other: What It Is and How to Fix It https://t.co/UaIf0uYeUS',
...
```

Awesome!  Everything works as intended.  We parsed the `Atom` feed from Wordpress to capture the latest articles, then selected a random article and tweeted the title and the URL on our Twitter Bot account, [AirbrakeArticle](https://twitter.com/AirbrakeArticle/status/946896220181561344).

We now have the basic structure of our application up and running, so next week we'll refine it and make it more real-world ready by implementing realtime error monitoring via Airbrake's NodeJS package, then we'll ensure this entire application can run automatically and serverlessly by using AWS Lambda.  Stay tuned!

---

__META DESCRIPTION__

A full walkthrough for how to create a completely serverless Twitter bot in Node, with automatic error tracking via Airbrake and AWS Lambda.

---

__SOURCES__

- http://joelgrus.com/2015/12/29/polyglot-twitter-bot-part-1-nodejs/
- https://www.npmjs.com/package/twitter
- https://medium.com/@emckean/create-a-simple-free-text-driven-twitterbot-with-aws-lambda-node-js-b80e26209f5
TODO: Change title.
TODO: Split into two parts.

# How to Make a Twitter Bot

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
entry point: (index.js) app.js
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
  "main": "app.js",
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

```bash
$ git remote add origin https://github.com/GabeStah/twitter-bot.git
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




---

__META DESCRIPTION__

---

__SOURCES__

- http://joelgrus.com/2015/12/29/polyglot-twitter-bot-part-1-nodejs/
- https://www.npmjs.com/package/twitter
- https://medium.com/@emckean/create-a-simple-free-text-driven-twitterbot-with-aws-lambda-node-js-b80e26209f5
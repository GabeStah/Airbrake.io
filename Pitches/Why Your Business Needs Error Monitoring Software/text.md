# Why Your Business Needs Error Monitoring Software

Modern software applications are flexible, capable tools.  These applications empower users with a wide range of abilities, from communicating with loved ones and coworkers, to gleaning knowledge from millions of crowdsourced articles, to diagnosing afflictions and potentially saving lives, and even to building the next great application, which might provide even more fantastic possibilities.

Yet the Facebooks, Wikipedias, and Watsons of the world haven't become the huge successes that they are without many bumps along the way.  Developing software is difficult, but for most modern applications, releasing your project out into the wild for the public is only the first step.  Monitoring and maintenance are critical components for most public-facing applications after release, to ensure the product can handle the load, is performing as expected, is financially viable, and, of course, doesn't collapse under the weight of unexpected errors.

While not particularly common, even within projects where the entire software development life cycle was smooth sailing up until production launch, nothing can sink the ship quicker than a slew of errors, which may grind the service itself to a halt, or in the worst case, crash the application entirely.

Enter the power of error monitoring software.  Even during development, but particularly after release, error monitoring software can provide that life line your organization needs to ensure your software remains fully functional.  Any unforeseen errors can quickly be identified, examined, and resolved, without the need for user-generated feedback or error reports.

To better examine why your business could benefit from error monitoring software, we'll explore the advantages by answering questions related to the `Six Ws`: `Who`, `What`, `When`, `Where`, `Why`, and `How`.  By the end, you should have a better understanding of how your business may benefit by utilizing error monitoring software for your own projects.

## Who is receiving this error?

Error monitoring software is designed to immediately inform you and your team when an error occurs, whether through email, via code hooks, or integrations with other services.  Best of all, since the error monitor can be easily hooked into your own application code, you'll receive detailed reporting information on the error or issue that occurred, including appropriate user data.  This might include associated account information for the afflicted user, the user's location when the error occurred, device the user was using, operating system version, browser version, session data, and so forth.  In short, error monitoring software can automatically provide you with detailed information about the individuals who are experiencing errors, so you can better serve your users.

## Who is assigned to resolving the error?

Modern monitoring software includes tools for tracking and resolving issues throughout the entire team.  This ensures that not only will you be aware of new exceptions as they occur, but you can monitor which team members are working on fixes for which errors.  By employing common prioritization techniques across the suite of errors you've received, your team can easily track issues and formulate the best plan to tackling them in order of importance.

## What is the exact error that occurred?

Error monitoring software allows you to see the exact nature of the error, in addition to the detailed metadata included with the exception.  For example, when an error occurs using [Airbrake.io's error monitoring software](https://airbrake.io/), Airbrake is able to report the error type/object, error message, parameters, a detailed backtrace, the environment in which the error occurred, the client machine data, the called API method that generated the error, the file that caused the error, and much more.

Using an error monitoring service allows for real-world debugging to occur without the need for user-generated feedback or reports.  When issues arise, the monitor will provide the same level of detail (and often more) than would be available to a developer who was performing step-by-step debugging on the source code.

## What programming languages and platforms are supported?

Integrating error monitoring software into your development life cycle is great and all, but it needs to be compatible with the language or platform that your team is working with.  Thankfully, most error monitoring services are compatible with all the popular languages, and even a handful of less-popular choices as well.  For example, [Airbrake](https://airbrake.io/languages) is currently compatible with **20** officially supported languages and platforms, with more always being added, including the big names like `.NET`, `Ruby`, `PHP`, `JavaScript`, `Node.js`, and all mobile platforms.  In nearly every case, if your team is working with it, chances are the error monitoring software you choose is compatible.

## What service integrations can I use?

Most monitoring services are designed to slide right into your existing workflow, without the need for additional work or added headaches.  To simplify this process, most error monitors are compatible with the most popular development services, like `Slack`, `JIRA`, `GitHub`, `BitBucket`, `Campfire`, `Trello`, and many more.  Moreover, a good error monitoring service will provide access to webhooks and an integration API, allowing your team to create a simple integration that enhances your workflow.

## When did the error occur?

Error monitoring software allows you and your team to be informed immediately when errors occur.  Yet, just as importantly, the software will also keep historical, timestamped records of all errors.  Not only does this allow you to pinpoint exactly when a particular issue or type of error occurred, but a friendly interface aggregates similar errors based on criteria you specify, or even automatically.  Thus, not only can you see when any single instance of an error occurred, but you can view charts and graphs with information about issues over time, providing you with a bird's-eye view of the progress, and how particular builds or patches may have improved or impacted the occurrence of that issue.

## When is error monitoring appropriate?

The short answer: throughout the entire software development life cycle.  The longer answer is that it depends on the needs of your team and your project.  At the very least, error monitoring services excel once a public beta or release is underway, as the core power of such a service is providing detailed error feedback from users in the wild, without the need for intervention on your part.

## Where in the code did this error originate?

Knowing _that_ an error occurred is useful, but knowing exactly _where_ the error occurred within your code is exactly the kind of detail necessary to quickly and accurately find the cause and get it resolved.  Error monitoring software provides detailed reports that include not just the methods or functions that generated the error, but the detailed backtrace, with the exact lines of code that led to this issue.  Furthermore, when integrated with `GitHub` or `GitLab`, any part of the backtrace can be clicked through to take you to that exact line in the source code.

## Why is error monitoring important?

Creating functional and effective software without an error monitoring service to assist you is certainly possible.  However, particularly for projects with growing user bases, doing so requires a great deal of foresight and a massive amount of upfront development time.  Not only must you heavily emphasize bug squashing throughout the life cycle, but you must also have components in place within your released code that can provide detailed error feedback from users in the wild, so when something (inevitably) goes wrong after production, your team can be alerted and quickly respond.  Certainly not an impossible task, but this requires a great deal of additional work that can be handled, easily and inexpensively, by error monitoring software.

Put simply, error monitoring software is a safety net.  It doesn't allow your team to forego all levels of quality assurance leading up to and after release, but instead, it provides some breathing room.  With an error monitor in place, you'll know that any unforeseen bugs, which do slip through the cracks and pop up after launch, will be immediately found and sent your way, so your team can quickly respond and get fixes out ASAP.

## Why did the error occur?

In most cases, once software is launched to production, getting detailed error reports often requires asking users affected by the error to take time out of their day to dig through directories for a crash dump or error log, and email or post it to your support forums.  Even in the case where your application can provide automated error dump reports to your team, without the need for user intervention, a team member then needs to either manually comb through those reports, or to develop a software component that can perform that task for you.

With error monitoring software, those problems are largely eliminated.  To figure out why an error occurred, you simply open that particular report within the user-friendly interface, and scan through the plethora of information attached to that error instance.  Rather than asking a user to provide a crash dump, you can simply look over the error, moments after it occurred and you were alerted, to see exactly what part of the source code caused the issue, and begin working toward a resolution.

## How many errors does our software application have?

Most error monitoring services provide detailed statistics, charts, and historical graphs about the errors within your software.  Not only can you view aggregate numbers for the overall system, but you can drill down with filtered queries and searches, to get data about errors that may have occurred only in `production`, within the `last 1 month`, that contain the word `IO`.  This makes it easy to evaluate trends, so you can quickly determine if a particular type of error is occurring less frequently, as a result of a recent patch aimed at fixing it.

## How do I get started with error monitoring software?

Integrating and beginning to use error monitoring software is very simple.  In most cases, simply [sign up for a free trial account](https://airbrake.io/account/new), locate the programming language or platform that suits your needs, follow the simple instructions to integrate with your project, and test it out by intentionally creating an error.  Most monitoring services allow you to be up and running in under five minutes.

From there, you can improve the workflow by integrating the error monitoring software with other third-party services your team uses, such as source control and issue tracking.  Once complete, you can breath easier, knowing that all future errors will be automatically tracked and reported across your team, providing you with a great deal of additional quality control throughout development and well after launch to production.

---

__SOURCES__

- https://airbrake.io/
- https://sentry.io/welcome/
- https://rollbar.com/features/
- https://raygun.com/error-monitoring-software
- http://www.nytimes.com/2012/12/04/health/quest-to-eliminate-diagnostic-lapses.html
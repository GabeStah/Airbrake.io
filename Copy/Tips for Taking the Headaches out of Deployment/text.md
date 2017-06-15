# Tips for Taking the Headaches out of Deployment

Months and often years of work go into most modern application development life cycles, so when it comes to deployment (whether for testing or even production) it can be a momentous occasion.  While many continuous deployment-style practices try to reduce the difficulty and stress involved in deploying by keeping the process a frequent occurrence, it can still be a major challenge to deploy without introducing any number of problems.

To try to alleviate some of these deployment headaches in this article we'll explore a bunch of tips and techniques that your teams can implement with varying levels of effort, but all of which can have a hugely positive impact on the deployment process throughout the entirety of the development life cycle.  Let's take a look!

## Plan Early

One of the most helpful practices right out of the gate is to begin your planning as early as possible.  You'll need to plan for all sorts of variables; everything from development and production platforms to languages and frameworks to database structures and schemas to release cycles and story handling to exception management and tracking and more!  Since it can be daunting to try to sit down and plan everything from the beginning, don't try to tackle the entirety of the scope or project at once, particularly if your organization plans to use some form of agile development where things can (and should) change during the process.  Instead, just get in the habit of trying to plan how things will go -- particularly when it comes to deployment -- as early as you can safely manage so there are fewer surprises later on down the road.

## Release Regularly

The next broad tip is to iterate often and release regularly.  Unless your project or business requirements absolutely forbid it or don't allow for more than one massive product launch and release, it's almost always going to be an easier road to travel if your team can get in the habit of frequent and regular releases.  Whether these occur in localized or remote staging environments or are full-blown public releases, getting releases out there into a deployed state will reveal where things are working and where other aspects might need to be fixed or improved upon.

Moreover, it's typical to have stubs and temporary solutions in place during development for interfacing with shared services like security, data, CDNs, and the like.  Regularly releasing deployments forces your hand a bit into nailing down how all those integrations and services will work with your application almost right out of the gate, which will ensure stability in the future.

## The Continuity Trifecta: Integration, Delivery, and Deployment

`Continuous X` remains a huge buzzword in the industry right now and for good reason: continuous practices generally empower your team and strengthen your application so you can be assured everything works as expected across all environments and even after deployments.  To that end, it may be beneficial to consider using some of the common continuous practices within your own project life cycle, depending which practices best fit the needs of your business.

Continuous integration (`CI`) is the practice of automatically building and testing your application on a consistent and regular basis.  The frequency of testing will depend on your business needs, but with the many powerful tools that are now available this process can occur for every build or even for every single commit to the shared repository.

Continuous delivery (`CD`) is less of a practice and more of a concept; the idea that your code base should always be release-ready.  What this means is debatable, but the basic idea in most implementations is that your application should always be ready for a single-click (or scheduled and automated) full release into some form of staging or production environment.

Finally, continuous deployment (also `CD`) is the culmination of these continuous practices and is where the actual deployment of releases or patches take place and are available for wider use (staging, production, etc).  Some businesses choose to streamline these processes so much that they are deploying new, updated builds to the public release environment on a daily basis.

## Make Use of Tools and Services

Don't be afraid to take advantage of tools and services that already exist to improve and streamline your deployment practices.  Many modern applications successfully rely on these tools as they can immediately improve deployment speed and stability.  There are far too many deployment tools out there to list them all here so we've listed just a handful of the popular choices below.  With a bit of time and research early in the development life cycle your team can find which tools best meet you business requirements needs and fit into the structure of your project.

- [Jenkins](https://jenkins.io): An open source continuous integration tool written in Java, which features pipelines as code and numerous plugins, allowing for simplified deployment.
- [Chef](https://www.chef.io/): Chef allows for easy manipulation of servers and deployment environments.
- [Octopus Deploy](https://octopus.com/): A .NET- and Windows-focused automatic deployment tool.
- [Travis CI](https://travis-ci.org/): A continuous integration tool for syncing with code repositories for automatic testing and deployment.
- [Codeship](https://codeship.com/): A customizable, hosted continuous integration platform service.
- [Capistrano](http://capistranorb.com/): A Ruby-based remote server automation tool.

## Automating Code Review

Deployment is made much easier and less stress-inducing when the stability of the code can be relied on, so another simple yet highly effective practice to improve deployment is to practice automatic code reviews.  There are many tried-and-true tools available that can automatically evaluate your application's code to check for complexity, style, security, duplication, coverage, and much more.  The extra level of assurance that the code is well-built and maintained helps ensure that deployments are more reliable and less error-prone.

## Automate Most Things, Don't Worry About All Things

Deployment and testing tools these days make it relatively simple to automate the vast majority of the deployment process, but therein lies a dangerous trap you may be caught in if you're not careful: The temptation to automate _everything_.  While this may be a valiant goal -- and one that is certainly possible, as many of the largest and most successful applications we all use today have shown us -- it's not always a smart move to spend the often excessive amount of time and resources necessary to try and automate that last little bit of the deployment process.

Instead, if automating some things just isn't as easy as you had hoped, don't be afraid to go ahead with automating the core 95% of the process while leaving the last little bit as a manual process for now.  Over time and as your deployment practices improve it's likely your team will eventually figure out how to automate that last part as well, but in the meantime you can save a lot of effort by foregoing it.

## Test Early, Test Often

Test as frequently as you can and as early in the development life cycle as possible.  Whether that means using a test-driven development methodology or not is up to your team and based on your own requirements, but the important point is that the more frequently the code base is tested and the earlier this occurs in the process, the better the application will perform and the easier frequent deployments will become.

## Practice Gradual Feature Rollouts

Wherever possible try to gradually rollout features instead of pushing a new feature to the entire userbase all at once.  A simple way to implement this is to release features that are _disabled_ by default.  The new code can be live and in production, but most users aren't going to be using the new feature.  This allows your own team members, or even a select group of beta testers, to test out and vet the new feature in the wilds of the production environment before you toggle the switch and enable the feature globally in a future release.

## Enforce Go/No-Go Decision Points

During the planning process, as well as throughout development, try to maintain a list of go/no-go inflection points that will determine if a build is ready to be deployed.  These decision points can be anything that fits your business requirements, but having some formal metrics on which to base the continuation of deployment (or subsequent rollback, if necessary) can really improve the stability and simplicity of releases overall.

## Consider Blue-Green Deployment

Blue-green deployment is a practice that aims to reduce downtime and improve stability by running two identical production environments, which are commonly referred to as `blue` and `green`.  The idea is that only one environment (either `blue` or `green`) is live at any moment and is serving all traffic and meeting all requirements.  For example, if `blue` is the currently active environment then `green` is idle and not in use.

When a new release is ready it should be deployed to and tested on the currently-idle environment (`green` in this case).  This allows your team to run all tests and diagnostics on the `green` production environment to ensure everything runs smoothly.  Once confirmed simply flip the switch on routing to move the live production service from the previous `blue` to the new version of `green`.  Not only will this reduce downtime for end-users but it dramatically reduces risk by allowing for _immediate_ rollback to the previous `blue` version and server with a simple route switch.

## Utilize Content Delivery Networks

There are many well-tested and reliable content delivery network services out there, allowing your application to delivery high-bandwidth or frequent media from a reliable and stable location without the need to worry about where that data comes from between one deployment and the next.

## Always Hard-Code Dependencies

While this is often (mistakenly) considered a practice solely for developers it's critical that dependencies are hard-coded to ensure that deployments never run into troubles because of incompatible versions or software changes made in the future.  If a library your application relies on is modified in the future it can have dramatic and negative effects on your software if your deployment isn't forcing explicit, hard-coded versions.  

## Monitor All the Things

Even after your application has been deployed and seems to be working the entirety of the deployment process is far from over.  One of the most critical stages to any successful deployment is the post-deploy process of monitoring your application to ensure everything is working well and remains stable.

### Monitor Exceptions

Even with the best testing procedures and quality assurance practices you can't always be certain that no bugs or issues made their way into the newly-deployed release.  Therefore, it's critical that you monitor exceptions in a manner that suits your team.  Error monitoring services like <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-bugs">Airbrake</a> provide an easy and inexpensive way to be automatically alerted of any exceptions which may occur.  Airbrake can be quickly integrated and will alert you and your team of issues within the deployed application without the need for user-generated error reports.

In addition to monitoring for exceptions you'll also want an efficient way to handle errors once they're discovered.  Even a simple error-tracker will make it a breeze to manage those post-deploy exceptions that may pop up.

### Monitor Load Times

If your deployed application fails to load or feels slow and bogged down then users are likely to become frustrated and may even leave entirely.  Be sure to monitor load times so you can be immediately alerted if something isn't working correctly.

### Monitor Logs

Sometimes an issue will pop up that isn't severe enough to raise an exception, but it's still important enough to take note of it and investiage.  In such cases the issue may still be tracked by application or server logs, so keep a close on those after a new release has been deployed.

### Monitor Key Performance Indicators

Key performance indicators (`KPIs`) are simply quantitative ways to measure how your application performs within the various business objectives your organization has laid out.  Whether you want to monitor user signup rates, sales figures, click-through frequency, support requests, or anything else you may need, it can be extremely beneficial to understand what is working and what could be improved during this deployment, which can dramatically alter the direction of your application and future deployments as well.

---

__META DESCRIPTION__

Check out these critical tips for taking the headaches out of deployment including continuous practices, monitoring techniques, and automation.

---

__SOURCES__

- http://www.ambysoft.com/essays/deploymentTips.html
- https://blog.codeship.com/continuously-deploying-single-page-apps/
- https://www.cukeragency.com/blog/2015/06/12/tips-automating-deployment/
- https://www.atlassian.com/blog/continuous-delivery/practical-continuous-deployment
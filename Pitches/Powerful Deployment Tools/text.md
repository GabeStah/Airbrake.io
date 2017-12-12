# 9 Powerful Deployment Tools

Deployment isn't usually the sexiest stage of application development, but performing it properly and efficiently is one of the most important factors in producing stable, successful software.  To simplify this process, numerous deployment tools have recently emerged that can handle everything from automatically building and testing code to instantly deploying it and reporting errors back to the team when they occur.  However, determining what tools are the best can be a bit challenging, especially if you or your team haven't been keeping up on the latest "continuous XYZ" trends.

Throughout this article we'll explore a hand-picked selection of 9 of the most powerful deployment tools on the market today, all of which can help your organization with every aspect of the development life cycle, including integration, delivery, deployment, testing, and error monitoring.  Let's take a look!

## Continuous Integration Tools

Continuous integration is the practice of automatically building and testing code after every single code commit across the team.  Implementing continuous integration provides numerous benefits, including the ability to near-instantly determine if pushed changes are compatible with the existing code base, or whether issues crop up that must be addressed.  As such, we'll take a look at a few of the most popular continuous integration tools that have emerged, all of which aim to streamline this validation process.

### Jenkins

[Jenkins](https://jenkins-ci.org/) is probably the most well-known _open source_ continuous integration tool on the market.  It boasts a long track record and relatively large userbase of successful projects and organizations.  Its most powerful feature is the availability of **thousands** of [plugins](https://plugins.jenkins.io/), which allow Jenkins to be incredibly flexible and well-suited for just about any project, team, organization, or use case.  As a simple example, Jenkins isn't pre-configured for any particular source control management (SCM) software.  Instead, you can simply go to the SCM category plugins page and install a plugin for Git, Subversion, Mercurial, Team Foundation Server, or whatever SCM your team is using.

### JetBrains TeamCity

[JetBrains TeamCity](https://www.jetbrains.com/teamcity/) is a free, enterprise-level, cross-platform continuous integration and continuous deployment tool, which supports unlimited users, build times, and builds.  It can be used for up to 100 build configurations, 3 concurrent build agents, and provides a public issue tracker and forum.  On-the-fly build progress reporting provides feedback throughout the build process, without the need to wait for each build to complete.

TeamCity also includes project hierarchies for configuration and permissions inheritance across multiple projects.  Templates can be created and customized to quickly copy common settings from other build configurations.  Additionally, build chains and dependencies make it easy to break down a larger build process into smaller procedures, which can be run sequentially or simultaneously.

### CircleCI

[CircleCI](https://circleci.com/) is the scalable, enterprise-level continuous integration platform that has powered some of the most well-known web applications and companies on Earth, including Facebook, Spotify, and Kickstarter.  CircleCI provides a simple YAML-based configuration syntax for defining exceptionally complex job execution instructions, so you and your team have complete control over the development, testing, and integration processes.  Since the service is based on scalable architecture, you can start with the minimal computational power and memory required for your project, then scale things up as more power is needed.

CircleCI is also language-agnostic, and includes support for C++, JavaScript, .NET, PHP, Python, and Ruby.  Integrated Docker integration also allows you run any image from a Docker registry, while customizing said image on a per-job basis.

## Automated Deployment Software

Manual deployment may have been viable back in the 90s, before "web application" was a common term and most sites consisted of a handful of HTML pages.  These days, modern applications require more precise and automated procedures.  Enter the need for automated deployment tools.  Using such a tool makes it simple for just about any member of your team to safely deploy new releases, or to roll back a problematic release with the touch of a button.  Moreover, many deployment tools can be configured to _automatically_ roll back a troublesome release, with no user intervention at all.  Below we take a closer look three of the most powerful and popular automated deployment tools on the market.

### Google Cloud Deployment Manager

[Google Cloud Deployment Manager](https://cloud.google.com/deployment-manager/) is an infrastructure deployment service that automates the creation and management of Google Cloud Platform resources.  It provides flexible deployments through custom templates and configuration files, which can be used across the wide range of Google Cloud platforms including Google Cloud Storage, Google Compute Engine, Google Cloud SQL, and many more.

The primary advantage to Google Cloud Deployment Manager is that all resources and requirements for an application deployment can be specified and customized within a simple YAML text format, allowing virtually anyone on the team to successfully deploy projects.  

### AWS CodeDeploy

[AWS CodeDeploy](https://aws.amazon.com/codedeploy/) automates software deployments onto a variety of compute services including Amazon EC2, AWS Lambda, or even on-premises instances.  It fully automates software deployments, providing a rapid and reliable method for frequent release schedules.  Moreover, you can choose to deploy your application across multiple environments, from development and testing to staging and production.  Since AWS CodeDeploy is powered by the ubiquitous AWS platform it can easily scale with your infrastructure needs, from single Lambda functions all the way up to thousands of EC2 instances.

AWS CodeDeploy provides mechanisms for application health monitoring _during_ the deployment process, so your application is made as readily available as possible throughout the deployment process.  Deployment rules can be configured to provide alerts for application health metrics during deployment, in order to halt or even revert deployments, if issues arise.

AWS CodeDeploy also gives detailed reports showing when and where each application revision was deployed.  You can even create push notifications in order to get live updates about deployments as they progress.

### Octopus Deploy

[Octopus Deploy](https://octopus.com/) aims to pick up where your continuous integration tool's work ends by automating even the most complex application deployments -- whether cloud based or on-premises.  Octopus Deploy's primary benefit is the strict requirements and restrictions you can place on various stages of deployment.  Development, quality assurance, testing, staging, production deployments, and other deployment phases are separately tracked and performed.  You can restrict access to certain phases, so untested code cannot be deployed to production.

Octopus Deploy also allows you to _build_ your application once, but deploy it as many times as necessary, ensuring the deployed software is exactly what was tested.  In short, Octopus Deploy tries to ensure nothing changes between development and production, so you'll be confident it will "Just Work™".

Octopus Deploy provides an intuitive dashboard for performing high level deployment steps for .NET, Java, and many other platforms.  Moreover, custom scripts can be used to manage sensitive data or implement complex deployment patterns.

## Error Monitoring Software

Even the most diligently tested software may run into unforeseen errors after deployment.  Unfortunately, many organizations merely rely on user reports to determine if there are errors in production releases.  Not only do the vast majority of users fail to report errors in the first place, but relying on user reporting is dangerous and foolish for a multitude of reasons.

Therefore, error reporting software is a crucial tool for any deployment process, as it will provide you and your team with automatic exception tracking and alerts, _without_ the need for user intervention.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-powerful-development-tools">Airbrake's error monitoring software</a> reports the error type, error message, parameters, the API method that generated the error, the detailed backtrace, the client machine data, the environment in which the error occurred, the file that caused the error, and much more.  There's no longer a need to sift through complicated logs or await user-provided feedback and bug reports.  Moreover, <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-powerful-development-tools">Airbrake</a> includes deployment tracking and error trend data through the powerful web dashboard, so managing and triaging errors has never been easier.

![airbrake-aggregated-data](https://d1mj7kpaxms69g.cloudfront.net/assets/marketing/aggregated-error-data-540-64426766b3ea9642d77a88ba6559fee5e7dcd2aa472d7fc1760267d7050f0407.png)

Errors are intelligently grouped so you can review and resolve similar errors within a single, easy-to-use interface.  Errors can be tracked over time and can be collated using custom grouping rules.

![airbrake-error-grouping](https://d1mj7kpaxms69g.cloudfront.net/assets/marketing/intelligent-error-grouping-540-c16d4e3e6d1d031569277d6bf460a46e172cfc69bdb55d312c11babad55d134e.png)

<a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-powerful-development-tools">Airbrake</a> also includes advanced filtering capabilities, allowing you and your team to find the exact errors you care about.  With GitHub or GitLab integration, backtrace line markers can be clicked through to view the exact source code from where the error originated.

![airbrake-filtering](https://d1mj7kpaxms69g.cloudfront.net/assets/marketing/search-and-filter-540-8358e4e5c92f61f87fe1e6cd9a7b14012c2aac09acd5bb4d5d42be60b08cec2c.png)

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-powerful-development-tools">Airbrake's error monitoring software</a> today and see why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

## Communication Tools

If your organization is looking to streamline communication, then a cloud-based communication platform like [Slack](https://slack.com/) or [Stride](https://www.stride.com/) (previously HipChat) is a great option.  Boasting over [6 million daily users](https://www.forbes.com/sites/alexkonrad/2017/09/12/slack-passes-6-million-daily-users-and-opens-up-channels-to-multi-company-use/#77ad2d557fdb), Slack is the current front runner and de facto choice for most companies.  However, Stride should not be ignored, as it comes from [Atlassian](https://www.atlassian.com/), the creators of Jira, Trello, and Bitbucket -- if your team uses these tools, Stride's integration might be an ideal fit.

Either way, the advantage of a cloud-based communication tool is the ability for everyone in the team -- and across the entire company -- to easily communicate with one another.  Every conceivable discussion configuration can be realized, from one-on-one private messaging to massive team chats through channels specific to any need you might have.  For example, a `#deployment` channel could be created and dedicated to solely tracking and communicating deployment statuses throughout the week.  Team members that need to monitor such information can join the `#deployment` channel, while all other members can remain oblivious and need not know such a channel even exists.

Additionally, both Slack and Stride allow for third-party app integrations, making it easier to configure push notifications to specific team members or channels.  For example, you can [integrate Slack with Airbrake](https://airbrake.io/docs/integrations/slack/) so that an automatic Slack notification is generated when an error occurs within your application.  Or, you can get an automatic notification through automated deployment tools like [AWS CodeDeploy](https://aws.amazon.com/blogs/aws/new-slack-integration-blueprints-for-aws-lambda/) when a deployment occurs.  The potential for integration is near-limitless.

---

__META DESCRIPTION__

A brief overview of 9 of the most powerful deployment tools available on the market today, to help you with deployment, testing, and error reporting.

---

__SOURCES__

- https://xebialabs.com/the-ultimate-devops-tool-chest/deployment/
- https://stackify.com/software-deployment-tools/
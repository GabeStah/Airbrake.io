---
categories: [devops]
date: 2018-01-23
published: true
title: "APM: What You Need to Know"
---

Application performance management (`APM`) is the practice of monitoring and managing the performance, availability, and capability of modern software applications.  `APM` doesn't have an explicit, dictionary definition, but is rather an industry-created term that encompasses nearly everything to do with monitoring and managing code, application performance, transaction times, and end user experience.

The overall goal if application performance management is to detect, diagnose, and assist in resolving complicated application performance problems, in order to maintain a pre-determined "minimal level of service."  Of course, every organization and application will have a different definition of what that minimal level of service can be.  However, by following a basic set of overall application performance management principles, a team and project of virtually any size should be able to implement a workable and beneficial monitoring solution.

In this article we'll explore the basic principles of application performance management, along with a handful of the best tools and techniques for implementing these principles into your own project.  Let's take a look!

## Application Monitoring Management Principles

In late 2016 [Gartner Research](https://www.gartner.com), one of the world's leading technology research groups, proposed the latest iteration of their [APM Conceptual Framework](https://www.gartner.com/doc/3551918) specification, which proposes five fundamental application performance management principles.  By working to implement and abide by these principles, most APM models should be stable enough to provide ample coverage against all potential pitfalls.

### End User Experiences

Commonly referred to as `real-time application monitoring` the goal of the end user experience principle is to ensure that communication between a client/user request and a server/remote response is efficient and effective.  `Passive monitoring` is the first of two components that make up the end user experience, and should perform an agentless monitoring of ports and services used within the application (such as HTTP requests, database queries, and so forth).  `Active monitoring`, on the other hand, uses explicit agents that should each actively probe and test systems, while the system is performing basic business transactions.

As the name suggests, the overall goal of tracking the end user experience is to maximize responsiveness to actual users, regardless of the incoming traffic or application activity at that particular moment in time.  Proper APM practices should give your team insight into the application's performance and the end user experience at all times of day.

### Runtime Application Architecture

This principle aims to ensure `bottom-up monitoring` is active and functional, which provides a direct and measurable correlation between the topology of the network and your application's architecture.  Ideally, you should implement some form of automated transaction mapping, in order to track each of the application's infrastructure components and see how each interaction that said components perform is carried out, relative to the rest of the application infrastructure.  With such capabilities in place, your team can better perform impact assessment for upcoming features, releases, and other major system changes.

### Business Transactions

The goal here is to try to categorize the multitude of potential detailed transactions your application may be capable of making into a smaller, more manageable high-level categories.  For example, consider the many different URL endpoints an application might have related to the `/user` domain:

- `/user/create`
- `/user/#id`
- `/user/#id/delete`
- `/user/#id/feed`
- `/user/#id/message`
- `/user/#id/subscribe`
- `/users/`
- And so forth...

Modern applications will likely have dozens if not hundreds of potential endpoints (and, therefore, transactions) that each resource can perform, so keeping track of these can be difficult, even using the best application monitoring tools.  Therefore, it is generally beneficial to try to categorize this large assortment of transactions into broader categories, which can then be monitored and more easily referenced in reports and other business communications.  In the example above, a resource-based `user` category may serve as a good starting point for all user-related transactions.

### Deep Dive Component Monitoring

Through the use of explicit agents and tools, your monitoring solution should be capable of providing real-time feedback of the particular language-specific application stack your software is using, and relate to the team how each component within said stack is connected to and performs user-defined business transactions.  Ideally, the result should show a clear, traceable path traversing from source code execution up through the component stack and back to the final source of the transaction request (i.e. from the client/user).  Since this piece should involve everything from the local program runtime to any application middleware, the benefits include better code reviewing, more accurate quality assurance and testing, and overall performance insights.

### Analytics and Reporting

The final and perhaps most important component of a successful application performance management model is adequate and robust analytics reporting.  This involves first determining what common set of metrics should be collected and reported within the application, or even for each individual component of said application.  From there, you'll need to establish a standardized view for how the data should be presented, so everyone on the team is on the same page when viewing or discussing said data.

In most cases, it is better to have _too much_ source data than not enough -- collect as much raw data as possible, and from there you and your team can decide what useful insights can be pulled from that dataset to actually create useful reports and analytics.

## Tools

Creating insightful, useful, and visually appealing APM reports can be a massive challenge unto itself.  Not only are there a million ways to approach the actual gathering of raw data, but combining data into a useful collection that can provide actionable feedback is a major undertaking.

Thankfully, many different tools exist to aid your team with application perform management implementations.  In general these fall into a few broad categories of tools, including tools aimed at helping developers with actual code creation, tools aimed at helping administrators handle server infrastructure management, and tools aimed at helping _everyone_ monitor the results of a production application.

### Code Profilers

Code profilers allow developers (and other team members) to quickly analyze written or executing source code, in order to detect potential bottlenecks, stack issues, or other performance related problems specific to the code itself.  There are dozens of powerful and popular tools, but below are a handful listed for various common languages and platforms.

- [ReSharper for Visual Studio](https://www.jetbrains.com/resharper/) - A Visual Studio extension for analyzing and profiling C#, VB.NET, XAML, ASP.NET, ASP.NET MVC, JavaScript, TypeScript, CSS, HTML, and XML code.  Includes `dotTrace` code profiler, `dotMemory` memory profiler, and `dotCover` unit test runner and code coverage tool.  Code style and formatting functionality with fine-grained, language-specific settings will help you get rid of unused code and create a common coding standard for your team.  You can instantly navigate and search in the whole solution.  Jump to any file, type, or type member, or navigate from a specific symbol to its usages, base and derived symbols, or implementations.
- [VisualVM](https://visualvm.github.io/) - A visual tool integrating commandline JDK tools and lightweight profiling capabilities, designed for both development and production time use.  VisualVM automatically detects and lists locally and remotely running Java applications.  VisualVM monitors application CPU usage, GC activity, heap and metaspace / permanent generation memory, number of loaded classes and running threads.  VisualVM provides basic profiling capabilities for analyzing application performance and memory management.  VisualVM takes and displays thread, heap, and core dumps, for an immediate insight of what is going on in the target process.
- [cProfile](https://docs.python.org/3.6/library/profile.html) - Provides deterministic profiling of Python programs.  A profile is a set of statistics that describes how often and for how long various parts of the program executed.  These statistics can be formatted into reports via the [`pstats`](https://docs.python.org/3.6/library/profile.html#module-pstats) module.
- [V8 CPU & Memory Profiler](https://www.jetbrains.com/help/webstorm/v8-cpu-and-memory-profiling.html) - Captures and analyzes CPU profiles and heap snapshots for Node.js applications.  With V8 CPU profiling you can get a better understanding of which parts of your code take up the most CPU time, and how your code is executed and optimized by the V8 JavaScript engine.  You can also open and explore profiles and snapshots captured in Google Chrome DevTools for your client-side code.

### Application Monitoring

Once your application is actually up and running an application monitoring tool or service will help your team keep track of critical health metrics within the app, such as transaction traces, database and server responsiveness, cloud resource performance, mobile telemetry, and much more.  Again, there are a multitude of powerful services and tools available, so we've highlighted just a few of the most established and powerful options below.

- [ManageEngine Applications Manager](https://www.manageengine.com/products/applications_manager/) - .NET, Java, and Ruby on Rails code-level diagnostics.  Provides a single, integrated platform to monitor your entire application ecosystem - end user, applications, and underlying infrastructure components such as application servers, databases, big data stores, middleware & messaging components, web servers, web services, ERP packages, virtual systems and cloud resources.
- [Scout](https://scoutapp.com/) - Ruby on Rails, Elixir, and Python application monitoring for developers.  Scout tracks the key health metrics you'd expect for every web endpoint and background job. Long-running metrics are kept for common Ruby dependencies as well: ActiveRecord, Redis, Elasticsearch, and more.  Transaction traces break down time and memory allocations with precision to include critical context with each trace like backtraces, flags on N+1 queries, number of ActiveRecord rows returned, and more  .Pricing is based on your app's transaction volume, decoupling your architecture from per-server pricing.
- [Microsoft Azure Application Insights](https://azure.microsoft.com/en-us/services/application-insights/) - .NET, Java, and Node.js app monitoring with advanced and customizable querying and reporting tools.  Automatically detects performance anomalies.  Application Insights includes powerful analytics tools to help you diagnose issues and to understand what users actually do with your app.  It's designed to help you continuously improve performance and usability.  It integrates with your DevOps process, and has connection points to a variety of development tools.  It can monitor and analyze telemetry from mobile apps by integrating with Visual Studio App Center and HockeyApp.
- [Amazon CloudWatch](https://aws.amazon.com/cloudwatch/) - A monitoring service for AWS cloud resources and the applications you run on AWS.  Use Amazon CloudWatch to collect and track metrics, collect and monitor log files, set alarms, and automatically react to changes in your AWS resources.  Amazon CloudWatch can monitor AWS resources such as Amazon EC2 instances, Amazon DynamoDB tables, and Amazon RDS DB instances, as well as custom metrics generated by your applications and services, and any log files your applications generate.  Use Amazon CloudWatch to gain system-wide visibility into resource utilization, application performance, and operational health.

### Error Monitoring

- <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-apm-what-you-need-to-know">Airbrake</a> - Powerful, language-agnostic <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-devops-best-practices">error monitoring software</a> that guarantees that your team will never need to worry about losing track of production errors.  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.  Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-apm-what-you-need-to-know">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An examination of what you need to know to implement proper application performance management practices and principles in your own software projects.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Application_performance_management
- http://www.apmdigest.com/12-things-you-need-to-know-about-application-performance-management-in-the-cloud
- https://stackify.com/what-is-apm/

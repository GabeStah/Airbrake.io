---
categories: [devops]
date: 2018-01-16
published: true
title: Improve Your Deployments with DevOps
---

[DevOps](https://airbrake.io/blog/what-is/devops) has emerged in recent years as a direct attempt to counteract the typical problem of slow software releases.  Every day that passes between the inception of an a great software idea and its eventual release into the wild is another day in which that idea loses relevance and value to users.  In business the concept the [first-mover advantage](https://en.wikipedia.org/wiki/First-mover_advantage) sums this up nicely: The first occupant of a market segment is provided a _significant_ advantage over the rest of the competition.  Even if the software your organization is developing doesn't have any competitors, there are still major incentives (financial and otherwise) to releasing in a timely manner.  

It is for these basic reasons that DevOps has become such a dominant practice in modern software development.  It forces a transition from slower release cycles into those that occur in a _continuous_ manner, commonly referred to as `continuous delivery` or `continuous deployment`.  In this article we'll explore a handful of these core concepts and see how you can dramatically improve your deployments with DevOps.

## Why Use DevOps?

The primary aim of DevOps is to smooth out the transition between active development and a functional release.  In an ideal world, this process should be completely frictionless and automated, requiring little or not manual human intervention.  If this is the end goal, then it stands to reason that DevOps can be used to modify and improve the standard release and deployment process.  In most cases, a typically release process contains four overall stages:

1. `Scheduling` - Document and plan on a release schedule, whether that be a singular event or multiple lesser releases.  This can be as broad or as detailed a process as is required, but every team or organization that is involved and has a stake in the release should be included in this decision-making process.  Operations team members should also be involved in this planning stage, to ensure that releases will be properly supported and are technically feasible with current or expected resources.

2. `Compatibility` - Confirm that all components of the upcoming release are compatible with one another.  These may include everything from individual software methods and APIs to third-party services and libraries.  This isn't entirely software related either -- hardware, platforms, and architecture requirements and capabilities must also be checked, so there aren't any unexpected problems or incompatibilities immediately following or during release.

3. `Environments` - Maintain integrity of the software when transitioning from development/testing/staging to the release environment (e.g. production).  It is quite common for components in an overall software application to experience minor adjustments from one environment to the next, so this critical stage of the deployment process aims to ensure nothing untoward has happened during the transition.  This might include verifying and maintaining dependency versions, and should include some form of tracking every component in the release.

4. `Rollbacks` - Allow for any given software release to be rolled back to a previous version.  In the event of an unexpected error or severe hardware failures, a software rollback to a prior release is often the only safe and prudent solution.

There's no mention of DevOps practices in any of the release stages above since they're all standard techniques for most software releases.  However, the inclusion of DevOps practices allows for most (if not all) of these steps to be highly automated.  Automation dramatically reduces the likelihood of human error and saves a great deal of resources in the long run.  Just as importantly, automating the majority of the deployment process dramatically speeds up the entire act, which allows for all future releases to occur far faster and at much greater frequency.

## Advantages of Automation

To further elaborate on why automation is so vital let's briefly consider the overall stages of a full software development life cycle:

- `Requirements` - Discuss and document software requirements with customers and other team members.
- `Development` - Develop the majority of the functional application, including unit tests.
- `Build` - Build the software for whatever environment is currently required.
- `Testing` - Test the software, on any and all environments.
- `Deployment` - Deploy the software to the target environment.
- `Execution` - Monitor the active release and respond to errors or other events.

This is a basic explanation of the stages, but performing all of these manually is a massive burden, which is largely what caused software projects in previous years to turn into massive undertakings that spanned years of effort before a release.  DevOps aims to dramatically reduce the time spent in these stages by automating nearly everything following the `development` stage.  Building, testing, deploying, and executing can all be automated, often to such a degree that a single command can be issued to build, test, and release a new version of the software.

Automating deployment takes many forms, but a common and powerful practice is stick [everything inside version control](https://airbrake.io/blog/devops/devops-best-practices).  This allows for an entire software version to be accessed from a central location, while also forcing developers and operators to create scripts and use tools to automate every aspect of the process.  These scripts are modified over time, along with the source code of the actual application, so the deployment process is in a constant state of improvement.

## Shrink Your Team Sizes

DevOps practices are not all tangible techniques related to how software is developed or deployed.  In many cases, integrating DevOps is about shifting the outlook and philosophies of the organization, by changing the beliefs and practices of those people that make up said organization.  One such philosophical shift that may be difficult for some more traditional groups to implement is the shrinking of team sizes.  While the explicit number of people that should be allowed on a team will differ from one expert to the next, the important thing to consider is that a smaller team provides many advantages over more traditional, larger teams:

- `Improved Communication` - When a massive department meets there's little room for discussion, and often one or a handful of individuals are "presenting" to the group.  On the flip side, a small team has few enough people that everyone involved can express opinions, concerns, or ideas that will impact the project or the rest of the team.  Plus, meetings tend to be shorter since everything moves faster and more efficiently.

- `Rapid Decision Making` - Since it's easier for members of smaller teams to communicate with one another, this also provides another massive advantage: it's easier for the team to reach decisive conclusions in relatively shorter periods of time.  A consensus from a dozen or so people is far easier (and even plausible) than reaching one for fifty or more team members.

- `Better Group Dynamics` - Not everyone will get along with or love everyone else, no matter the size of the team.  However, a team with fewer people will typically lead to closer relationships and cohesiveness when compared to a larger team.

All that said, even though there are clear advantages to smaller teams, there are certainly tasks or releases that will require many more people than a typical team can take on.  With DevOps practices in place, it's much easier for smaller teams to work together on larger components of a project.  Plus, if everything is version controlled and most everything is automated, members of different teams are still working on and referencing the same primary source at all times.  This act of _coordination_ is another key aspect of DevOps.

## Mitigate Unnecessary Coordination

The act of coordinating takes on many forms throughout a full software development life cycle.  Team members can coordinate directly in conversation, indirectly by working on shared components, in real time, or in delayed time via tangible elements such as email or chat messages.  A version control system is a powerful coordinating tool that ensures a project always maintains forward momentum and historical records of all previous changes.

The tricky part about development and deployment is the need to perform a cost/benefit analysis for each coordinating effort throughout the process.  While it may not seem obvious at first, every act of coordination has a real cost and (hopefully) a real benefit.  For example, a typical morning meeting involving a half-dozen team members forces direct coordination between those people.  It may not seem significant, but if said meeting interrupts an active developer who was in a state of flow just prior to being interrupted by this meeting, that may "cost" half a days worth of man hours for that individual developer to get back to the same state of active progress he or she was at before.  Multiple this cost by the number of people involved in the meeting and it can quickly become a very costly endeavor.

On the other hand, this morning meeting may result in dramatic benefits, such as determining the next major software feature the team should work on.  The challenge that DevOps practices tries to improve is finding that sweet spot between too much and too little coordination.  The goal can be summed up as "minimizing unnecessary coordination."  For example, maybe the topics covered in the aforementioned morning meeting could actually be handled using indirect coordination (email, Slack, etc), rather than forcing everyone to attend a direct, in-person meeting.  This would allow the flow-experiencing developer to continue working uninterrupted, but he or she can still provide input on the topics at a more convenient time.  Since the entire goal of DevOps is to reduce time to market for each deployment and release, a major hurdle for many organizations will be determining how to reduce unnecessary coordination practices.

## Error Detection and Reporting

Even the most diligently tested software may run into unforeseen errors during or after deployment.  Unfortunately, many organizations merely rely on user reports to determine if there are errors in production releases.  Not only do the vast majority of users fail to report errors in the first place, but relying on user reporting is both dangerous and foolish, for a variety of reasons.

Therefore, error reporting software is a crucial tool for any deployment process, as it will provide you and your team with automatic exception tracking and alerts, _without_ the need for user intervention.  Since everything else in the deployment process is automated, there's no reason that error monitoring and reporting shouldn't also be automated as much as possible.  That's why Airbrake's powerful <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-improve-your-deployment-process">error monitoring software</a> guarantees that your team won't need to worry about losing track of production errors!  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-improve-your-deployment-process">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An overview of how to improve your deployments with DevOps practices and techniques, including automation, smaller teams, and error reporting.

---

__SOURCES__

- https://en.wikipedia.org/wiki/DevOps
- https://gist.github.com/jpswade/4135841363e72ece8086146bd7bb5d91
- https://devops.com/devops-best-practice/
- https://www.amazon.com/DevOps-Software-Architects-Perspective-Engineering/dp/0134049845
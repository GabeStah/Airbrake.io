# 7 Tips for Reducing Production Defects

We all make mistakes.  Such a mistake might be forgetting to turn the lights off before leaving the house, or unintentionally cutting off another driver on the way to work.  Or, as a software developer, it might be accidentally introducing a defect into the newest software project the team is working on.  While defects are inevitable during development, they can largely be identified, fixed, or prevented entirely long before they reach a production environment.  Throughout this article we'll explore a few tips for reducing production defects, which will boost overall software quality, reduce regressive issues, improve inter-team communication, and increase customer satisfaction.  Let's take a look!

## Change the Groupthink Regarding Defects

Many businesses and organizations have a strong tendency to view defects in a skewed and unhealthy light.  Defects are inevitable at some point throughout the overall software development life cycle, but many traditionally-managed organizations view defect and error management as a basically binary battle to beat back the bugs.  There is rarely acknowledgement that defects _will_ crop up, and that healthy processes to combat their arrival must be planned out and executed.  Instead, the ultimatum from the executives and managers on high can often be summarized thusly: "Attention maggots!  Proceed with elimination of X% of defects.  TAKE NO PRISONERS!"

While defects during development are a necessarily evil, the goal shouldn't be to just eliminate bugs, but rather to develop practices and procedures that simplify the identification, debugging, and resolution of defects.  Just as importantly, these processes should be implemented early in the development life cycle, and constantly improved upon.  With healthy practices in place, defects can become something that are no longer considered [an inevitability](https://airbrake.io/blog/devops/production-defects-are-not-inevitable), but are instead a rare surprise, like seeing a digital unicorn on a distant hill -- you didn't expect it, but you're glad you spotted it, and have a desire to investigate it further.

This change in attitude, particularly for well-established organizations that may be accustomed to more traditional outlooks on defects, may be difficult, but the shift in attitude must be a top down process.  Executives and managers need to alter their perceptions that defects are inevitable (particularly in production), and instead, view bugs as exceptional.  This change in perception will eventually traverse downward throughout the company, eventually leading to a paradigm shift in the groupthink attitudes about defects.  This will open an avenue toward changes in how the organization handles problems, which will eventually lead to a dramatic reduction in production defects.

## Thoroughly Analyze Software Requirements

Set aside an adequate block of time on a regular basis (weekly, monthly, quarterly, etc) and meet with the managers and development leads throughout the team to discuss the detailed software requirements of the project.  This process should identify exactly what requirements are necessary for the overall application, as well as detailed component- or feature-specific requirements.  The major benefit of this practice is uncovering potential pitfalls and preventing a large portion of unnecessary defects that may otherwise crop up down the road.

For example, during a software requirements analysis meeting your team may come to the conclusion that the data layer implementation that was previously planned (like Azure) may not work with a required third-party component, so the requirements must be changed to another solution (such as AWS).  Failing to identify such an issue early in the development life cycle will often lead to an assortment of painful issues, and if ignored long enough, could cause a slew of production defects that could have been easily prevented.  Since the vast majority of defects that crop up during an application's development are related to failures during software requirement planning -- as opposed to actual coding and implementation issues -- it's critical that this process be taken seriously, and be performed both early and often.

## Practice Frequent Code Refactoring

With a solid plan for establishing sound software requirements laid out, the next habit to get into is to implement organization-wide code refactoring practices.  Refactoring aims to improve and redesign the structure of already existing code, without modifying its fundamental behavior.  Simple examples of refactoring include fixing improperly names variables or methods, and reducing repeated code down to a single method or function.

This self-review process on code should also include peer review.  Many companies find great success in pair programming techniques, whereby two individuals sit together during actual code development, with one developer writing code and the other watching as an observer.  While this practice increases the man-hours required to complete any given line of code [studies show](https://collaboration.csc.ncsu.edu/laurie/Papers/XPSardinia.PDF) that code produced from pair programming will contain approximately 15% fewer defects than code produced by solo programmers.

The best way to reduce production defects through any form of code review is to ensure the processes your organization has established are practiced _frequently_ -- repetition will create habitual processes that invariably catch potential problems and existing defects well before they reach the production environment.

## Perform Aggressive Regression Testing

Regression testing is a form of software testing which confirms or denies the functionality of software components after these components undergo alterations.  In the event that a change is made to a software component and a defect is discovered, regression testing is required to confirm the issue, and to attempt to resolve it.  Regression testing should ideally be performed on a regular schedule or basis, such as after every change, at the end of every day, weekly, bi-weekly, and so forth.  Regression tests should also typically be executed anytime a previously discovered issue has been fixed.  Generally speaking, the more often regression testing can occur, the more issues can be discovered and resolved, and the more stable the application will become, which will reduce production defects dramatically.

## Execute Defect Analysis

Invariably, some defects will appear at some point in the software development life cycle, so it's important that your team takes full advantage of the benefits these provide.  Specifically, a defect presents the opportunity to perform deep analysis on the affected components of the software and make improvements to all areas that were impacted.  How your organization chooses to perform analysis will be specific to your team and application, but there are a few key principles to keep in mind when sussing out the root cause of any given defect:

- **Aim to Improve Quality**: Above all else, all the tips and practices laid out in this article are aimed at improving the quality of the software throughout future iterations and into production releases.  Thus, defect analysis should aim to both _prevent_ defects or, for those that still slip through the cracks, to _detect_ them as early as possible.
- **Rely on Expert Team Members**: Quality Assurance departments are beneficial and often necessary for particularly large projects, but don't neglect the members of your development team that were involved with actually producing the code or components that caused the defect.  Identifying these members is not to pass down any judgment or shame them, but rather to empower these individuals to analyze the problem and come up with the most elegant solution.
- **Prioritize Systematic Defects**: Throughout the typical development life cycle there will tend to be a handful of defects that are regressive -- issues that repeatedly appear again and again, in spite of the team's best efforts to squash them.  Such systematic defects should be prioritized during the analysis process and heavily focused on, as doing so will have the biggest impact on overall defect rates.

## Consider Continuous Changes

The concepts of [continuous integration](https://airbrake.io/blog/software-development/efficiency-continuous-integration-cloud), [continuous delivery](https://airbrake.io/blog/devops/take-headaches-out-of-deployment), [continuous deployment](https://airbrake.io/blog/devops/take-headaches-out-of-deployment), and the like are not merely buzzwords; they are highly effective practices that can dramatically improve software quality by taking much of the headache out of iterative releases and deployments.

Continuous integration is the practice of automatically building and testing your application on a regular basis.  The frequency of testing will depend on your business needs, but with the many powerful tools that are now available this process can occur for every build, or even for every single commit to the shared repository.

Continuous delivery is less of a practice and more of a concept; the idea that your code base should always be release-ready.  What this means is debatable, but the basic idea in most implementations is that your application should always be ready for a single-click (or scheduled and automated) full release into a staging or production environment.

Lastly, continuous deployment is the culmination of these continuous practices and is where the actual deployment of releases or patches take place and are available for wider use (staging, production, etc).  Some organizations choose to streamline these processes so much that they are deploying new, updated builds to the production environment on a daily basis.

## Error Monitoring Software

Automated exception tracking and reporting tools, like <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-how-to-reduce-production-defects">Airbrake</a>, ensure that your team is immediately aware of exceptions the moment they occur.  Airbrake's powerful <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-how-to-reduce-production-defects">error monitoring software</a> guarantees that your team won't need to worry about losing track of that rare production defect that slips through the cracks.  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize defect parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-how-to-reduce-production-defects">Airbrake's defect monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

7 top tips for reducing production defects during development, including continuous integration, regression testing, and error monitoring software.

---

__SOURCES__

- https://collaboration.csc.ncsu.edu/laurie/Papers/XPSardinia.PDF
- https://hbr.org/1983/09/product-defects-and-productivity
- https://www.isixsigma.com/industries/software-it/defect-prevention-reducing-costs-and-enhancing-quality/
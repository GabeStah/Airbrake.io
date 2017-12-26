# Techniques for Preventing Software Bugs

Preventing software bugs can be a tricky business.  Bugs can occur as a result of all sorts of problems, from improper testing or messy code to lack of communication or inadequate specifications documents.  In this article we'll explore the three main categories of software bug, along with the various causes of each, and how implementing a few simple techniques can help you and your team prevent such software bugs in your own projects.  Let's take a look!

## Implementation Bugs

These types of bugs occur when an accurate specification documentation exists, but the actual code implementation is faulty in some way.  Such bugs might pop up for a variety of reasons including regression, messy code, and inadequate testing.

### Regression

`Regression` is when an application works correctly at first, but a problem later arises within what was previously-tested and valid code.  These types of bugs are fairly common across the development industry, so it is not abnormal for a regression to suddenly appear and cause bugs to surface, throughout the development life cycle.

#### Prevention

[Test-driven development](https://airbrake.io/blog/sdlc/test-driven-development) is a common practice in which development works backwards from the normal order of "code, test, debug, repeat".  Instead, `test-driven development` (or `TDD`) focuses on initially creating _failing_ tests that define and test for the exact functionality the software should handle.  Once the tests are in place, _only then_ is code written that can successfully _pass_ the previously-failing tests.  If the code fails to pass a test, it is modified until all tests pass, which provides a strong indicator that the code is doing what it should.

Test-driven development is an extremely beneficial technique when trying to prevent or reduce implementation bugs in your software.  In the case of `regression` bugs, `TDD` is a primary means by which your team can maintain a stable code base throughout the development life cycle.  By properly creating tests first, and _then_ writing code that passes those tests, the chances of regressive bugs popping up drops dramatically.

### Messy Code

`Messy code` (commonly known as `smelly code` or [`code smell`](https://en.wikipedia.org/wiki/Code_smell)) is a term used to describe a minor, surface-level problem with a code base that hints at a larger issue deeper down in the code.  In many cases, developers and other team members can pick up on a code smell with experience and practice, preventing much larger issues well before they become a problem.  However, it's common for development to be kicked into overdrive to meet deadlines, which often leads to the unintentional introduction of bugs, as messy code begins to grow and impact larger features in the application.

#### Prevention

[Test-driven development](https://airbrake.io/blog/sdlc/test-driven-development) is another incredible technique for preventing bugs that are result of `code smell`.  `TDD` encourages development to break out the code base into cleaner, separated concerns.  It also heavily favors frequent code review and refactoring.  Combined, these techniques dramatically reduce the frequency of bugs occuring due to smelly code.

### Inadequate Testing

If you or your team are unable to dedicate enough time and resources to testing, it's only a matter of time before new bugs crop up as a result.  While it can often feel like a waste of time to a developer and a waste of money to a manager/executive, the truth is that adequate testing procedures will be one of the most beneficial practices your team can implement to help quell the potential onslaught of bugs.

#### Prevention

[Continuous integration](https://airbrake.io/blog/software-development/efficiency-continuous-integration-cloud) is the practice of automatically building and testing code after every single code commit is made, across the entire team. Implementing continuous integration provides numerous benefits, including the ability to near-instantly determine if pushed changes are compatible with the existing code base, or whether issues crop up that must be addressed.  Consequently, `continuous integration` is a powerful technique that can largely prevent bugs that might otherwise occur due to inadequate testing.  It will typically reduce turnaround time between builds, which will improve overall development and implementation speed, providing more time for testing and quality assurance purposes.

Another proven technique to help make up for inadequate testing is implementing automated exception tracking and reporting tools like <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-techniques-preventing-bugs">Airbrake</a>, which ensures that your team is immediately aware of exceptions the moment they occur.  Airbrake's powerful <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-techniques-preventing-bugs">error monitoring software</a> guarantees that your team won't need to worry about losing track of that rare production defect that slips through the cracks.  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize defect parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-how-to-reduce-production-defects">Airbrake's defect monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices.

## Specification Bugs

A [`specifications document`](https://en.wikipedia.org/wiki/Functional_specification) is typically used in software development to define what functions the software should be capable of performing.  This documentation is often the _technical_ portion of a pairing with the `requirements document`.  Since the specification defines what functionality the software (or components within said software) should provide, it stands to reason that an improper specification will eventually lead to the introduction of preventable bugs within the code base.  

### Failures in Communication

The most common direct cause of a specification problem that eventually results in a bug is a simple lack of proper communication somewhere within the team.  Whether it's developer to developer, manager to developer, executive to department head, or any other combination therein, a failure in communication will often cause a breakdown in the produced specification document.  This may take the form of a missing specification section, or a totally incorrect specification that is aimed at dealing with a critical component of the software.  In such cases, bugs will crop up all over the place as the software matures beyond the initial stages and other better-defined specifications are implemented alongside lesser, broken features.

#### Prevention

[Behavior-driven development](https://airbrake.io/blog/software-design/behavior-driven-development), or `BDD`, is a software development methodology that combines practices from [test-driven development](https://airbrake.io/blog/sdlc/test-driven-development) and [domain-driven design](https://airbrake.io/blog/software-design/domain-driven-design).  `BDD` aims to simplify development through the use of a common domain-specific language (`DSL`), which is used to adapt natural language sentences and phrases into executable tests.  

It many cases, `BDD` implementation starts at the broadest levels of expertise and works toward the deepest, lowest points of understanding.  By crossing through these thresholds, the team has a much better overall understanding of the scenarios and domains to be implemented, which leads to better-designed, more robust software.  This _forces_ and, therefore, improves communication across the team, dramatically reducing the potential for bugs that might result from an improper specification document due to lack of communication.

### Conflicts

`Specifications` are only as useful and as accurate as the team members writing them.  While some people may consider themselves without flaw, the reality is we all make mistakes from time to time, and the process of creating specification documents is no exception.  As such, there may be instances where a single specification document is created that circumvents or takes precedent over an existing specification, without the team fully knowing or understanding which specification is proper and which should be thrown into the trash.  In these scenarios where multiple specifications exist and cover the same component or functionality, it's common for bugs to crop up later in the development life cycle, as a result of such conflicts.

#### Prevention

The best technique for preventing specification conflicts is to dedicate one or more team members to vigilantly managing and reviewing specifications throughout the development process.  Since specification documents tend to be fairly technical in nature, it's critical that the person(s) assigned to this duty have a technical background and a clear understanding of the overall software functionality.  Failing that possibility, such as when the project is massive or spans multiple teams, the specification specialist should understand the intricacies of the software _component_ for which his or her team is assigned.

Additionally, `behavior-driven development` techniques will also help prevent conflict-related specification bugs, since it encourages a common `domain-specific language` (`DSL`) to be created prior to and during development.  The existence of a `DSL` ensures everyone on the team can easily communicate (and, thus, document) all the specifications necessary to create the final application.

## Absent Specification Bugs

The last general type of bug occurs when `specifications` are simply absent.  In other words, if a bug appears as a result of the software being asked to do something completely outside the realm of what you and your team originally envisioned as possible or plausible, it falls into the category of `absent specification bugs`.  Such unknown scenarios are unknown for a _reason_: They are completely outside of the scope of what was considered likely to affect the software, so they were never introduced into an existing specification document.

### Improper Planning

The most likely cause of an absent specification bug is a simple lack of planning on the part of the team prior to development.  Such planning failures may manifest themselves in the form of improper specification documentation, inadequate requirements, under-performing operations infrastructure, poorly written code, or many other potential pitfalls.

#### Prevention

It's not necessary that you and your team accurately estimate each and every particular area that could cause a problem in the future.  Instead, it's critical that you "have a plan to plan" -- create a simple procedure by which _any_ team member can begin the process of outlining and documenting a potential problem area well ahead of time.  This will dramatically improve the chance that the team adequately plans for and covers most troublesome details, since it encourages everyone working on the project to be open and vocal about things they might foresee as a problem, without relying on a handful (or even one) individual to come up with _everything_.

As you may suspect, [behavior-driven development](https://airbrake.io/blog/software-design/behavior-driven-development) is a powerful technique here as well, since it promotes the heavy reliance on crowdsourcing questions (and solutions) throughout the entire software development life cycle.

### Incorrect Assumptions

Similar to, yet slightly different from, improper planning is the concept of making incorrect assumptions about the software, the requirements, the specifications, teams' capabilities, and so forth.  There are many possible assumptions that _must_ be made throughout software development, so a few incorrect assumptions are certainly going to occur.  In such cases, a specification will often go unwritten, which may lead to bugs later on in development (or even in production).

#### Prevention

Preventing incorrect assumptions throughout the software development life cycle starts with open communication and dialogue.  If too much bureaucracy and "red tape" exists within the organization, it may prevent intelligent individuals from broaching/discussing critical issues that may dramatically impact the software and the overall development process in the long run.

Therefore, it is vital that your organization maintains open communication within and, if possible, _across_ all departments.  `BDD` practices can help here as well, but they are certainly not a requirement.  Instead, merely adopting an open door and open communication policy will allow discussion of critical topics and ensure nothing comes as a major surprise after many incorrect assumptions may have already been made.

---

__META DESCRIPTION__

An examination of the primary types of software bugs, including a handful of useful techniques for preventing software bugs in your own applications.

---

__SOURCES__

- https://medium.com/quality-faster/preventing-software-bugs-13f1cb2c7103
- https://blog.smartbear.com/code-review/reduce-software-defects/
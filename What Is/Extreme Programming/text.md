Extreme Programming is a software development methodology designed to improve the quality of software and its ability to properly adapt to the changing needs of the customer or client.  During the mid and late nineties, while working on the Chrysler Comprehensive Compensation System (C3) to help manage the company's payroll, software engineer [Ken Beck](https://en.wikipedia.org/wiki/Kent_Beck) first developed the Extreme Programming methodology.  In October 1999, he published _Extreme Programming Explained_, detailing the entire method for others, and shortly thereafter the [official website](http://www.extremeprogramming.org/) was launched as well.

Similar to other `Agile Methods` of development, Extreme Programming aims to provide iterative and frequent small releases throughout the project, allowing both team members and customers to examine and review the project's progress throughout the entire [`SDLC`].

Throughout this article, we'll examine exactly what Extreme Programming is and how it works, from the values and principles that are behind it, to the rules and procedural best practices that are used to implement a new Extreme Programming project, so let's get started!

## Extreme Values

These five fundamental values provide the foundation on which the entirety of the Extreme Programming paradigm is built, allowing the people involved in the project to feel confident in the direction the project is taking and to understand their personal feedback and insight is as necessary and welcome as anyone else.

__Simplicity__: We will do what is needed and asked for, but no more. This will maximize the value created for the investment made to date. We will take small simple steps to our goal and mitigate failures as they happen. We will create something we are proud of and maintain it long term for reasonable costs.

__Communication__: Everyone is part of the team and we communicate face to face daily. We will work together on everything from requirements to code. We will create the best solution to our problem that we can together.

__Feedback__: We will take every iteration commitment seriously by delivering working software. We demonstrate our software early and often then listen carefully and make any changes needed. We will talk about the project and adapt our process to it, not the other way around.

__Respect__: Everyone gives and feels the respect they deserve as a valued team member. Everyone contributes value even if it's simply enthusiasm. Developers respect the expertise of the customers and vice versa. Management respects our right to accept responsibility and receive authority over our own work.

__Courage__: We will tell the truth about progress and estimates. We don't document excuses for failure because we plan to succeed. We don't fear anything because no one ever works alone. We will adapt to changes when ever they happen.

## Extreme Rules

Initially published by Don Wells in 1999, the proprietor of the Extreme Programming website, this set of [`Extreme Programming Rules`] were originally intended to help to counter the claims that Extreme Programming fails to support some of the prominent disciplines necessary for modern development.

__Planning__

- User stories are written.
- Release planning creates the release schedule.
- Make frequent small releases.
- The project is divided into iterations.
- Iteration planning starts each iteration.

__Managing__

- Give the team a dedicated open work space.
- Set a sustainable pace.
- A stand up meeting starts each day.
- The Project Velocity is measured.
- Move people around.
- Fix Extreme Programming when it breaks.

__Designing__

- Simplicity.
- Choose a system metaphor.
- Use CRC cards for design sessions.
- Create spike solutions to reduce risk.
- No functionality is added early.
- Refactor whenever and wherever possible.

__Coding__

- The customer is always available.
- Code must be written to agreed standards.
- Code the unit test first.
- All production code is pair programmed.
- Only one pair integrates code at a time.
- Integrate often.
- Set up a dedicated integration computer.
- Use collective ownership.

__Testing__

- All code must have unit tests.
- All code must pass all unit tests before it can be released.
- When a bug is found tests are created.
- Acceptance tests are run often and the score is published.

## Extreme Practices

Created using what were considered the best practices of software development at the time, these twelve `Extreme Programming Best Practices` detail the specific procedures that should be followed when implementing a project using Extreme Programming.

### Fine-scale feedback

__Pair programming__

In essence, pair programming means that two people work in tandem on the same system when developing any production code.  By frequently rotating partners throughout the team, Extreme Programming promotes better communication and team-building.

__Planning game__

Often this takes the form of a meeting at a frequent and well-defined interval (every one or two weeks), where the majority of planning for the project takes place.

Within this procedure exists the `Release Planning` stage, where determinations are made regarding what is required for impending releases.  Sections of `Release Planning` include:

- `Exploration Phase`: Story cards are used to detail the most valuable requirements from customers.
- `Commitment Phase`: Planning and commitments from the team are made to meet the needs of the next schedule release and get it out on time.
- `Steering Phase`: This allows for previously developed plans to be adjusted based on the evolving needs of the project, similar to many other `Agile Model` methodologies.

Following the `Release Planning` is also the `Iteration Planning` section, which consists of the same three sub-phases of its own, but with variants on their implementations:

- `Exploration Phase`: All project requirements are written down.
- `Commitment Phase`: Necessary tasks yet to be completed to meet the upcoming iteration release are assigned to developers and scheduled appropriately.
- `Steering Phase`: Development takes place and, upon completion, the resulting iteration is compared to the outlined story cards created at the start of the `Planning` procedure.

__Test-driven development__

While an entire article could be written about test-driven development, the concept is fairly well known among developers and effectively means that tests are generated for each and every requirement of the project, and _only then_ is code developed that will successfully pass those tests.

__Whole team__

As with many other [`SDLC`] methods and practices, Extreme Programming promotes the inclusion of customers and clients throughout the entire process, using their feedback to help shape the project at all times.

### Continuous process

__Continuous integration__

Another common practice in modern development, the idea behind continuous integration is that all code developed across the entire team is merged into one common repository many times a day.  This ensures that any issues with integration across the entire project are noticed and dealt with as soon as possible.

__Code refactoring__

Another very common practice, the idea behind code refactoring is simply to improve and redesign the structure of already existing code, without modifying its fundamental behavior.  Simple examples of refactoring include fixing improperly names variables or methods, and reducing repeated code down to a single method or function.

__Small releases__

Very much in line with the practices of the `Iterative Model`, this concept ensures that the project will feature iterated, small releases on a frequent basis, allowing the customer as well, as all team members, to get a sense of how the project is developing.

### Shared understanding

__Coding standards__

The coding standard is simply a set of best practices within the code itself, such as formatting and style, which the entire team abides by throughout the life cycle of the project.  This promotes better understanding and readability of the code not only for current members, but for future developers as well.

__Collective code ownership__

This practice allows for any developer across the team to change any section of the code, as necessary.  While this practice may sound dangerous to some, it speeds up development time, and any potential issues can be quelled with proper unit testing.

__Simple design__

There's little reason to complicate things whenever a simpler option is available.  This basic practice of keeping all components and code as simple as can be ensures that the entire team is always evaluating whether things could be done in an easier way.

__System metaphor__

Best thought of as part of the `coding standards`, the system metaphor is the idea that every person on the team should be able to look at the high-level code that is developed, and have a clear understanding of what functionality that code is performing.

### Programmer welfare

__Sustainable pace__

A key concept for better work-life balance with developers on an Extreme Programming project is the notion that nobody should be required to work in excess of the normal scheduled work week.  Overtime is frowned upon, as is the concept of "crunch time", where developers are expected to work extreme hours near the end of a release to get everything completed on time.


[`SDLC`]: https://airbrake.io/blog/category/sdlc
[`Rapid Application Development`]: https://airbrake.io/blog/sdlc/rapid-application-development
[`Extreme Programming Rules`]: http://www.extremeprogramming.org/rules.html


---

__SOURCES__

- http://www.extremeprogramming.org/
- http://www.extremeprogramming.org/rules.html
- http://www.extremeprogramming.org/values.html
- https://en.wikipedia.org/wiki/Extreme_programming
- https://en.wikipedia.org/wiki/Extreme_programming_practices

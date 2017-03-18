# Kaizen Model - What is it and how do you use it?

`Kaizen` is the Japanese word for `improvement`.  In the realm of software development life cycles, `kaizen` expands on the notions of `continuous integration` and `continuous development` with its core concept of `continuous improvement`.  This applies to all aspects of the organization, including both the project and the employees themselves.

Throughout this article we'll fully explore what the `kaizen model` is, how it is typically implemented in modern development life cycles, and the advantages or disadvantages of implementing a `kaizen model` in your own projects.  Let's jump right in!

## What is Kaizen?

Long before software development and modern `SDLC` methodologies became a necessity, `kaizen` was used to improve standardized business practices.  These practices initially sprung up throughout the Japanese business world after World War II, most notably as the fundamental component for Toyota Motor Corporation's [`The Toyota Way`](https://en.wikipedia.org/wiki/The_Toyota_Way) managerial and production style.  While a full exploration of `The Toyota Way` methodology is outside the scope of this article, that method is a great real-world example of the potential of `kaizen`, with its core principles derived from the following simple concepts:

- Long-Term Philosophy
- The Right Process Will Produce the Right Results
- Add Value to the Organization by Developing Your People
- Continuously Solving Root Problems Drives Organizational Learning

These principles, which have helped bring Toyota great success over the years, illustrate the fundamental concepts of `kaizen` practices.  The idea of `continuous improvement` through `kaizen` focuses on enacting change as quickly as possible.  In most traditional implementations, this means workers are looking for minor ideas and improvements which can be implemented immediately, or within a relatively short timespan (ideally the same day).  The goal is to humanize the workplace, while also teaching team members how to experiment and apply scientific methodology, in order to notice and eliminate waste throughout business processes.

## What is the Kaizen Model?

Jumping ahead to present day, let's take a look at how `kaizen` practices have influenced and improved the software development life cycle, via the `kaizen model`.  At the core, the `kaizen model` also emphasizes quality through `continuous improvement`.  Developers (and other team members) are held accountable for the aspects of the application under their umbrella of responsibility.  Moreover, rather than being held accountable by their superiors higher up the corporate ladder, the `kaizen model` emphasizes the importance of _peer-to-peer_ accountability.  Being accountable to the others you work with on a daily basis, who perform the same tasks and have similar responsibilities, is a far more powerful incentive than typical judgment from your superiors.

This means that in practice, throughout the software development life cycle, the `kaizen model` constantly asks team members to evaluate their own work, and to help review that work of their peers.  Fundamentally, this creates a culture of intelligent individuals working toward a common goal of `continuous improvement` -- a stark contrast to Waterfall-esque models, which often emphasize the importance and goals of a select few individuals during the initial phases of the development life cycle.

## Implementation of the Kaizen Model

When applied to a software development life cycle, the `kaizen model` is implemented in a few different ways, depending on the particular needs of your organization or project.

### Day-to-Day vs Special Event

Typical day-to-day approaches to the `kaizen model` involve the development team gathering on a regular basis (daily or once a week) to discuss a previously identified issue.  Using input from everyone involved -- or, at least, those who wish to participate -- the team can gather ideas and potential solutions to resolve the issue at hand.  It's not necessary to perform special meetings explicitly for this purpose, but rather, these practices can (and should) be incorporated into already established meetings that the team is performing anyway.

By contrast, a special event approach to the `kaizen model` emphasizes a bit more planning, in order to execute an improvement over the course of a few days (if necessary).  This improvement should be implemented by the developer(s) who are most tightly linked to the component where the improvement is to be made, since those individuals are in the best position to make changes without introducing additional issues or bugs.

### Individual vs Team

In general, `kaizen model` practices emphasize the team, and invariably provide `continuous improvement` for that team as a whole.  However, there is a subset of `kaizen` practices known as `teian kaizen`.  `Teian` (or `提案`) roughly translates to `proposal` in English.  Therefore, the practice of `teian kaizen` is when, rather than focusing on the team, individual team members discover and _propose_ improvements during their day-to-day activities.  With this method, typically the proposer isn't charged with __applying__ said improvement, but once that improvement is noted and discussed around the team, it can be implemented in the best possible way.

### Process vs Sub-Process

In the realm of `kaizen`, the `process` is the overarching business goal or value stream that your software aims to provide.  Therefore, the entire software development life cycle -- from inception to production release and beyond -- is loosely considered to be the full `process` within the `kaizen model`.  `Kaizen` techniques and practices are rarely applied directly to the `process` at large; instead, they are more commonly applied to `sub-processes`.

A `sub-process`, as the name implies, is simply a child component of the `process`.  In the case of software development, a `sub-process` might be anything from the `database layer` to the `content delivery network` to individual `classes` or `methods`.  No matter what, when an improvement is suggested, it should be clearly applicable to one or more specific sub-processes.

For example, imagine that you think of a way to improve the response time when a user makes a request for "Latest Posts" in your blogging application.   When you make that suggestion to the team at the next meeting, the team should be able to specifically detail all the `sub-processes` that would be impacted (and thus may need to be changed) to see this improvement enacted (e.g. User class, Post class, database layer, etc).  From there, individual team members best able to alter those `sub-processes` can help to see this improvement made in a timely manner.

## Advantages of the Kaizen Model

At the core, the `kaizen model` is based around a few fundamental principles, which should be very familiar to those who have used other `Agile` or `LEAN` models in the past.  As it happens, many of these are also obvious advantages to using the `kaizen model` as a whole:

- **Emphasizes Iterative Development**: The `kaizen model` focuses on iteration and incremental improvement.  Rather than planning far ahead and attempting to get everything right during that critical planning period, the `kaizen model` allows for rapid, incremental improvements to your software, so the project can appropriately adapt when something (invariably) goes wrong.
- **Promotes `Continuous Integration`**: By keeping all development work merged into one central location, all team members can better analyze and discover areas for further improvement.
- **Reduces Waste**: Through `continuous improvements` made multiple times a week, or even daily, the `kaizen model` tries to largely eliminate waste.
- **Empowers Individuals**: The fundamental practice of receiving and implementing improvements suggested by members of the team is a great motivator, not only by keeping team members pushing ahead, but as acknowledgement of everyone's importance and relevance throughout the entire software development life cycle.
- **Emphasizes Multi-Headed Decision Making**: Since the majority of improvements and iterations made throughout the software development life cycle are discussed by the team as a whole, this leads to the majority of critical design decisions also being made by more than one person.  While there is some potential for danger of having too many cooks in the development kitchen, if handled properly, this can be a large advantage, where every voice can introduce both problems and solutions for each component along the way.

## Disadvantages of the Kaizen Model

Since the `kaizen model` is so open-ended, and largely just a mentality shift rather than a specific step-by-step methodology, it can be difficult to pin down any specific disadvantages.  Both positives and negatives largely come down to implementation.  That said, the potential issues with using `kaizen` typically revolve around an organization's (in)ability to adapt to the new paradigm, which empowers everyone on the team.

- **Requires Open, Company-Wide Communication**: Many organizations, particularly development studios who have been embroiled in `waterfall` methodologies for some time, may find it challenging to adapt to the open communication style of the `kaizen model`.  Proper implementation of `kaizen` requires that the organization both allow (and embrace) the input and potential improvements coming from every individual across the team.
- **Flattens Company Power Structures**: While implementation of the `kaizen model` doesn't directly require a change in authority or managerial structures, it _does_ emphasize the need to reduce the importance of those dynamics throughout the team.  Developers, and other team members, cannot be afraid of or intimidated by their superiors, otherwise potentially drastic improvements could be left by the wayside.

---

__META DESCRIPTION__

A detailed examination of kaizen practices, its origins, and the kaizen model throughout the software development life cycle.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Kaizen
- https://www.slideshare.net/ZakharPrykhoda/kaizen-software-development-model
- https://www.lynda.com/Operating-Systems-tutorials/Kaizen-Continuous-improvement/508618/555089-4.html
- https://m4i3.wordpress.com/kaizen/
- https://en.wikipedia.org/wiki/The_Toyota_Way
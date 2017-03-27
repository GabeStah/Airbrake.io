# Scrum - What is it and how do you use it?

`Scrum` is easily one of the most well-known and commonly implemented software development frameworks in the world.  At its core, `scrum` is actually an `Agile` framework.  However, `scrum` is also technically a _subset_ of `Agile`, since `scrum` adheres to a rather strict set of processes and practices.

In this article we'll dive deeper into the origins of `scrum`, what the framework is in the realm of software development, how it is typically implemented, and a few potential advantages and disadvantages you may experience while using `scrum` on your next project, so let's get started!

## What is Scrum?

The word `"scrum"` is most commonly used in the sport of rugby football.  In rugby, a `scrum` is a method of restarting play after a minor rule infringement has occurred, specifically in a situation where no advantage would be gained for the team that obeyed the rules.  A `scrum` is formed with eight players from each team linking together into three tight rows on each side, before facing one another with heads down and attempting to push the opposing team out of position.  The ball is thrown into the `tunnel` (the gap under the players), and the as the `scrum` begins, both teams attempt to move the ball using their legs and by repositioning themselves, such that the ball emerges behind their team's formation and can be taken into possession.  I'm no expert and this is a very rudimentary explanation compared to the [full rules of a `scrum`](http://laws.worldrugby.org/?law=20&language=EN), which are far more in-depth and interesting, but this origin of `scrum` from rugby transitions well into the use of the word within software development.

In the January 1986 issue of Harvard Business Review, two professors at Hitotsubashi University in Tokyo Japan, Hirotaka Takeuchi and Ikujiro Nonaka, published an article titled [_The New New Product Development Game_](https://hbr.org/1986/01/the-new-new-product-development-game), which emphasizes the importance of speed and flexibility when developing new products.  In the article, Takeuchi and Nonaka detail the lessons learned from manufacturing practices, which they refer to as the `"rugby approach,"` in which the team ["tries to go the distance as a unit, passing the ball back and forth."](https://hbr.org/1986/01/the-new-new-product-development-game)  The article even goes on to explicitly make use of the term `scrum` to define these practices of pushing the team forward, which is considered the introduction of the term to modern software and project development.

A few years later, a software developer named Ken Schwaber began implementing the `scrum` framework into software development practices within his company.  Over the next few decades, the `scrum` framework began to take shape, with the publication of books and guides outlining its use, including the definitive book by Schwaber and colleague Jeff Sutherland, [_The Scrum Guide_](https://www.scrum.org/resources/scrum-guide).  As it happens, the _Guide_ provides us with a definition of what `scrum` is: "A framework within which people can address complex adaptive problems, while productively and creatively delivering products of the highest possible value."

### The Scrum Theory, Pillars, and Values

The theory of `scrum` is focused on being an _empirical_ process: a framework that attempts to gain experienced-based knowledge, then make decisions based on that learned knowledge.  To meet that goal, `scrum` is founded on three core pillars:

- **Transparency**: All relevant aspects of the project must be well-defined and visible to everyone on the team that shares in the responsibility for said aspects.
- **Inspection**: `Scrum Artifacts` must be frequently inspected, to measure progress toward `Sprint Goals` (both of which we'll discuss shortly).
- **Adaptation**: If an inspector decides that an aspect of the project is failing to meet its intended goals, that aspect should be adjusted as quickly as possible.

_The Scrum Guide_ also lays out the five key `scrum values`:

- **Commitment**: Team members individually commit to achieving their team goals, each and every `Sprint`.
- **Courage**: Team members know they have the courage to work through conflict and challenges together so that they can do the right thing.
- **Focus**: Team members focus exclusively on their team goals and the `Sprint Backlog`; there should be no work done other than through their `Backlog`.
- **Openness**: Team members and their stakeholders agree to be transparent about their work and any challenges they face.
- **Respect**: Team members respect each other to be technically capable and to work with good intent.

### Scrum Artifacts

A `scrum artifact` is simply a representation of value or work to be completed, which is well-defined and should be transparently visible to all team members.  There are three main _types_ of `scrum artifacts`:

- `Product Backlog`: This is the `scrum` equivalent of a `work in progress` list.  The `Product Backlog` is simply an ordered list of all items that are (or may be) necessary throughout the entirety of the software development life cycle.  The `Backlog` acts as the definitive requirements documentation for the project.  The `Product Backlog` is maintained by the `Product Owner`.
- `Sprint Backlog`: A `Sprint Backlog` is a subset of items from the `Product Backlog` that have been explicitly selected to be part of a `Sprint`.
- `Increment`: The `Increment` is the summation of all `Product Backlog` items that were successfully completed during a particular `Sprint`, _added to_ the value of all previous `Increments`.

### Scrum Team Roles

There are three primary roles within the whole of the `scrum team`:

- `Product Owner`: This individual can best be thought of as the project lead or producer, and his or her aim is to maximize the value of the product by ensuring the `Development Team` produces the best possible work.  The `Product Owner` largely focuses on maintaining the `Product Backlog`
- `Development Team`: The group of developers that actually produce the work defined in the `Product Backlog` into a functional and releasable iteration.  The `Development Team` should be self-organizing and fully independent from the `Scrum Master`.
- `Scrum Master`: Acts as both the referee and coach for the whole team when it comes to the proper use and implementation of `scrum` practices and processes.

### Scrum Events and Workflow

Day-to-day activities within the `scrum` framework are all based around particular `scrum events`.  All `events` are "time-boxed", meaning they each have a maximum duration.  This helps to ensure that the development life cycle remains constantly adaptive and properly `Agile`.

The `scrum` framework defines five types of events:

- `Sprint Planning`: During this event, the entire team collaborates to define what the `Sprint Goal` will be for the upcoming `Sprint`.  This is accomplished by answering two simple questions: 1) What work should be accomplished within the next month?  2) How can that work be completed?  The `Sprint Planning` session should be kept to a maximum of eight hours per month.
- `Sprint`: The bread and butter of `scrum` practices, a `Sprint` is a one-month period in which a potentially releasable iteration is created (commonly referred to as a "Done").  In spite of the name, a `Sprint` is not anything like "crunch time."  Instead, a `Sprint` is always active: when one `Sprint` ends, the next immediately begins.
- `Daily Scrum`: Every day, the `Development Team` meets for a maximum of 15 minutes to discuss the planned work for the next day.
- `Sprint Review`: Following the completion of a `Sprint`, the `Sprint Review` is an event with a maximum duration of four hours, in which the entire team discusses the `Increment` results and makes any necessary changes to the `Product Backlog`.
- `Sprint Retrospective`: At a maximum of three hours, the `Sprint Retrospective` occurs after the `Sprint Review`, but prior to the next `Sprint`, and is a meeting for the entire team to decide on potential improvements that can be made general to practices or procedures for the next `Sprint`.

## Advantages of Scrum

- **Allows for Rapid Prototyping**: With a maximum of only one month to devote to any particular `Sprint Goal`, `scrum` allows for rapid coding and development of ideas or components that may be experimental or may even fail, without severe worry or potential downsides.
- **Keeps Customers in the Loop**: Since `scrum` is an `Agile` framework and is highly iterative, customers are able to quickly assess progress and provide feedback throughout the entirety of the development life cycle.
- **Encourages Consistent Productivity**: The daily meetings of `Daily Scrums` are a guaranteed way to get insight from all team members about their progress, so suggestions and guidance can be provided where necessary to keep development on track.

## Disadvantages of Scrum

- **Abundance of Meetings**: Many people, particularly developers trying to maintain that sweet `flow` state, are unlikely to appreciate the need for numerous meetings, particularly the `Daily Scrums`.
- **Potential Difficulty With Estimations**: As with other `Agile` frameworks, it's quite easy to simply jump into a project and begin development without much in the way of planning.  While this is often a benefit, it also means that `scrum` can often obfuscate the actual time and monetary costs of a project (or even aspects of said project), often until a few months down the line.
- **Requires Lenient Leadership**: Since proper `scrum` practices emphasize the importance of separating the management of `Development Teams` from roles like `Scrum Master` and `Product Owner`, successful implementation of `scrum` requires that managers and leadership are able to trust the `Development Team` and give them the freedom they need to work independently.  

---

__META DESCRIPTION__

An detailed analysis of the scrum framework, including its origins and common implementation practices in modern software development life cycles.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Scrum_(software_development)
- https://www.scrum.org
- https://www.scrumalliance.org/
- https://hbr.org/1986/01/the-new-new-product-development-game
- https://www.scrum.org/resources/scrum-guide
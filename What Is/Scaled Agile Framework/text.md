TODO: Replace `SAFE` or `safe` with `SAFe`.

# Scaled Agile Framework - What is it and how do you use it? [v1]

Scaled Agile Framework, also known as `SAFe`, is an enterprise-scale development methodology, developed by [Scaled Agile](http://www.scaledagile.com/), which combines `Lean` and `Agile` principles within a templated framework.  Proponents of `SAFe` claim that `safe` provides a significant increase in employee engagement, increased productivity, faster times to market, and overall higher quality.

In this article, we'll dive deeper into what, exactly, `safe` is, how it is typically implemented, and both the advantages and disadvantages of using the `safe` methodology throughout the software development life cycle.  As we explore, always keep in mind that detailed information is readily available, free of charge, from the [`Scaled Agile Framework website`](http://www.scaledagileframework.com/).  Let's get to it!

## What is Scaled Agile Framework?

For a visual overview of `safe`, the flowchart on the [`SAFe homepage`](http://www.scaledagileframework.com/) is a great tool.  Yet to really understand how `safe` works, we need to dig a bit more into some of the fundamental components.

`safe` heavily relies on the core principles of `Lean` and `Agile`, which it adapts to form the nine [`SAFe Lean-Agile Principles`](http://www.scaledagileframework.com/safe-lean-agile-principles/):

1. **Take an economic view**: Delivering the best value and quality to people and society in the sustainably shortest lead time requires a fundamental understanding of the economics of the system builder’s mission.
2. **Apply systems thinking**: In `SAFe`, systems thinking is applied to the organization that builds the system, as well as the system under development, and further, how that system operates in its end user environment.
3. **Assume variability; preserve options**: Lean systems developers maintain multiple requirements and design options for a longer period in the development cycle. Empirical data is then used to narrow focus, resulting in a design that creates better economic outcomes.
4. **Build incrementally with fast, integrated learning cycles**: Increments provide the opportunity for fast customer feedback and risk mitigation, and also serve as minimum viable solutions or prototypes for market testing and validation.
5. **Base milestones on objective evaluation of working systems**: In `Lean-Agile` development, each integration point provides an objective milestone to evaluate the solution, frequently and throughout the development life cycle. This objective evaluation assures that a continuing investment will produce a commensurate return.
6. **Visualize and limit WIP, reduce batch sizes, and manage queue lengths**: Three primary keys to implementing flow are to: 1) Visualize and limit the amount of work-in-process so as to limit demand to actual capacity, 2) Reduce the batch sizes of work items to facilitate reliable flow though the system, and 3) Manage queue lengths so as to reduce the wait times for new capabilities.
7. **Apply cadence, synchronize with cross-domain planning**: Cadence transforms unpredictable events into predictable ones, and provides a rhythm for development. Synchronization causes multiple perspectives to be understood, resolved and integrated at the same time.
8. **Unlock the intrinsic motivation of knowledge workers**: Providing autonomy, mission and purpose, and minimizing constraints, leads to higher levels of employee engagement, and results in better outcomes for customers and the enterprise.
9. **Decentralize decision-making**: Decentralized decision-making reduces delays, improves product development flow and enables faster feedback and more innovative solutions. However, some decisions are strategic, global in nature, and have economies of scale sufficient enough to warrant centralized decision-making.

`safe` attempts to incorporate the various lessons from `Lean` and `Agile` methodologies into the basic principles, which are then used to bring substantial improvements to time to market, employee engagement, quality, and productivity.

### Agile Release Trains

An `Agile Release Train`, or `ART`, is a fundamental concept within `safe`.  The `ART` is the primary value delivery method of `safe`.  [`Agile Teams`](http://www.scaledagileframework.com/agile-teams/) are a small group of individuals focused on defining, building, and testing solutions within a short time frame.  An `ART` is a self-organizing, long-lived group of `Agile Teams` (a team of teams, if you will), whose purpose is to plan, commit, and execute solutions together.  Built around the organization's core [`Value Streams`](http://www.scaledagileframework.com/value-streams/), an `Agile Release Train` exists solely to deliver on promised value by building beneficial solutions for the customer.

Using tools like a common `Vision`, `Roadmap`, and `Program Backlog`, and `ART` aims to complete goals within a specific period of time, known in `safe` as `Program Increments` (e.g. a 10 week period).  Furthermore, `ARTs` 

### Program Level

[`Program Level`](http://www.scaledagileframework.com/program-level/) is another key concept within `safe`.  Simply put, the `Program Level` is where development teams and other resources are applied to an important, ongoing development mission.  Most `Program Levels` -- such as teams, roles, and activities -- revolve around a specific `ART`, ensuring a constant flow of incremental, value-generating releases.

### Core Values

`safe` focuses on four fundamental, core values:

- **Alignment**: Fundamentally, global focus is more valuable than local focus.  Individuals on a `safe` team should value the team's goals above personal tasks and responsibilities.  Extending off that, members of `Agile Release Trains` should emphasize vision and program objectives over team goals.  `ARTs` should focus on `Value Stream` objectives over `ART` objectives.  `Value Streams` value contributions to the business portfolio.  Finally, `Management` should focuses on establishing a mission, but with few constraints.
- **Built-in Quality**: `safe` contains a number of built-in quality practices to help ensure that every element, within every incremental build, is up to a high standard of quality.
- **Transparency**: Large-scale development is a challenge.  Transparency establishes trust throughout the project by sharing facts and progress openly across all levels.  This extra level of trust enables decentralized decision-making and additional employee empowerment.
- **Program Execution**: Each `Agile Release Train` should predictably generate value.  The `Program Level` within `safe` provides responsibilities and guidance to various member roles within `ARTs`, to assist with the generation of value.

## SAFe Implementation and Best Practices

Given the sheer size and scope of `safe`, proper implementation can be rather daunting, especially starting out.  Since a full explanation of `safe` implementation would require tens of thousands of words -- and because more detailed information is available on the website -- we'll cover a brief overview of implementation here:

1. **Train Implementers**: Due to the sheer scope and challenge required in adopting `safe`, most organizations will need a combination of internet and external mentors and coaches.  These people should be capable of easily teaching and delivering `safe` techniques to others throughout the organization.
2. **Train Executes, Managers, and Leaders**: The initial batch of `Implementers` should first focus on training all executives, managers, and leaders.  Once these fundamental team members understand the `Lean-Agile` mindset, core `safe` principles, and implementation techniques, the process will become much smoother for the entire organization.
3. **Train Teams**: Individuals should initially be organized into `Agile Teams`, who can then all be trained on the various `Lean`, `Agile`, and `safe` principles.
4. **Launch `Agile Release Trains`**: Finally, once the organization has been properly trained, it's time to group `Agile Teams` together into `ARTs`, and then generate models for objective planning, program execution, program increment planning, and all the other components required for a successful `Agile Release Train`.

`safe` defines three levels within the organization:

- [`Portfolio Level`](http://www.scaledagileframework.com/portfolio-level/): Focuses on the `Portfolio Vision`, creating `Investment Themes` with assigned funding, and makes use of `Kanban`.  [`Epics`](http://www.scaledagileframework.com/epic/) are also devised at this level, which contain significant initiatives to help guide value streams toward the larger portfolio goals.
- [`Program Level`](http://www.scaledagileframework.com/program-level/): As we discussed earlier, `Program Level` focuses on specific business value streams.  One key aspect of `Program Level` is the process of breaking down `Epics` into smaller features that form the [`Program Backlog`](http://www.scaledagileframework.com/program-and-value-stream-backlogs/).
- [`Team Level`](http://www.scaledagileframework.com/team-level/): At the `Team Level`, features from the `Program Level` are broken down further into `Stories`, forming the `Team Backlog`.  `Scrum` is then utilized, over the course of typical iteration lengths (2 weeks), to complete the features of `Stories`.

## Advantages of SAFe

- Highlight the emphasis of aligning team-level development with business strategy.
- Discuss the evolving nature of `SAFe` to better support Agile practices.
- Detail how `SAFe` is _arguably_ [free of charge](http://www.scaledagileframework.com/usage-and-permissions/), though any help with implementation is charged.
- Provides specific, tangible advice for implementing software development practices.
- Best suited for large organizations with sizeable teams.
- Emphasizes people and skills over technology.
- Aims to support teams which exist for more than one project.

> - https://intland.com/blog/agile/safe/scaled-agile-framework-safe-in-a-nutshell/
> - https://www.linkedin.com/pulse/benefits-scaled-agile-framework-herb-bowie
> - https://barrettsteve.wordpress.com/2014/04/09/scaling-agile-using-the-scaled-agile-framework-safe/

## Disadvantages of SAFe

- Discuss common criticisms of `SAFe`:
  - licensing
  - certification/training requirements
  - highly prescriptive
- Overbearing and extremely complex.
- Ill-suited for smaller teams or organizations.
- Separates functional and non-functional work.

> - https://barrettsteve.wordpress.com/2014/04/09/scaling-agile-using-the-scaled-agile-framework-safe/
> - https://en.wikipedia.org/wiki/Scaled_Agile_Framework#Criticism
> - https://dzone.com/articles/method-wars-scrum-vs-safe
> - http://www.cio.com/article/2974436/agile-development/comparing-scaling-agile-frameworks.html

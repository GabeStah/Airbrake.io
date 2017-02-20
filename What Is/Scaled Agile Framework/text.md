# Scaled Agile Framework - What is it and how do you use it? [v1]

Scaled Agile Framework, also known as `SAFe`, is an enterprise-scale development methodology, developed by [Scaled Agile](http://www.scaledagile.com/), which combines `Lean` and `Agile` principles within a templated framework.  Proponents of `SAFe` claim that `SAFe` provides a significant increase in employee engagement, increased productivity, faster times to market, and overall higher quality.

In this article, we'll dive deeper into what, exactly, `SAFe` is, how it is typically implemented, and both the advantages and disadvantages of using the `SAFe` methodology throughout the software development life cycle.  As we explore, always keep in mind that detailed information is readily available, free of charge, from the [`Scaled Agile Framework website`](http://www.scaledagileframework.com/).  Let's get to it!

## What is Scaled Agile Framework?

For a visual overview of `SAFe`, the flowchart on the [`SAFe homepage`](http://www.scaledagileframework.com/) is a great tool.  Yet to really understand how `SAFe` works, we need to dig a bit more into some of the fundamental components.

`SAFe` heavily relies on the core principles of `Lean` and `Agile`, which it adapts to form the nine [`SAFe Lean-Agile Principles`](http://www.scaledagileframework.com/safe-lean-agile-principles/):

1. **Take an economic view**: Delivering the best value and quality to people and society in the sustainably shortest lead time requires a fundamental understanding of the economics of the system builder’s mission.
2. **Apply systems thinking**: In `SAFe`, systems thinking is applied to the organization that builds the system, as well as the system under development, and further, how that system operates in its end user environment.
3. **Assume variability; preserve options**: Lean systems developers maintain multiple requirements and design options for a longer period in the development cycle. Empirical data is then used to narrow focus, resulting in a design that creates better economic outcomes.
4. **Build incrementally with fast, integrated learning cycles**: Increments provide the opportunity for fast customer feedback and risk mitigation, and also serve as minimum viable solutions or prototypes for market testing and validation.
5. **Base milestones on objective evaluation of working systems**: In `Lean-Agile` development, each integration point provides an objective milestone to evaluate the solution, frequently and throughout the development life cycle. This objective evaluation assures that a continuing investment will produce a commensurate return.
6. **Visualize and limit WIP, reduce batch sizes, and manage queue lengths**: Three primary keys to implementing flow are to: 1) Visualize and limit the amount of work-in-process so as to limit demand to actual capacity, 2) Reduce the batch sizes of work items to facilitate reliable flow though the system, and 3) Manage queue lengths so as to reduce the wait times for new capabilities.
7. **Apply cadence, synchronize with cross-domain planning**: Cadence transforms unpredictable events into predictable ones, and provides a rhythm for development. Synchronization causes multiple perspectives to be understood, resolved and integrated at the same time.
8. **Unlock the intrinsic motivation of knowledge workers**: Providing autonomy, mission and purpose, and minimizing constraints, leads to higher levels of employee engagement, and results in better outcomes for customers and the enterprise.
9. **Decentralize decision-making**: Decentralized decision-making reduces delays, improves product development flow and enables faster feedback and more innovative solutions. However, some decisions are strategic, global in nature, and have economies of scale sufficient enough to warrant centralized decision-making.

`SAFe` attempts to incorporate the various lessons from `Lean` and `Agile` methodologies into the basic principles, which are then used to bring substantial improvements to time to market, employee engagement, quality, and productivity.

### Agile Release Trains

An `Agile Release Train`, or `ART`, is a fundamental concept within `SAFe`.  The `ART` is the primary value delivery method of `SAFe`.  [`Agile Teams`](http://www.scaledagileframework.com/agile-teams/) are a small group of individuals focused on defining, building, and testing solutions within a short time frame.  An `ART` is a self-organizing, long-lived group of `Agile Teams` (a team of teams, if you will), whose purpose is to plan, commit, and execute solutions together.  Built around the organization's core [`Value Streams`](http://www.scaledagileframework.com/value-streams/), an `Agile Release Train` exists solely to deliver on promised value by building beneficial solutions for the customer.

Using tools like a common `Vision`, `Roadmap`, and `Program Backlog`, and `ART` aims to complete goals within a specific period of time, known in `SAFe` as `Program Increments` (e.g. a 10 week period).  Furthermore, `ARTs` 

### Program Level

[`Program Level`](http://www.scaledagileframework.com/program-level/) is another key concept within `SAFe`.  Simply put, the `Program Level` is where development teams and other resources are applied to an important, ongoing development mission.  Most `Program Levels` -- such as teams, roles, and activities -- revolve around a specific `ART`, ensuring a constant flow of incremental, value-generating releases.

### Core Values

`SAFe` focuses on four fundamental, core values:

- **Alignment**: Fundamentally, global focus is more valuable than local focus.  Individuals on a `SAFe` team should value the team's goals above personal tasks and responsibilities.  Extending off that, members of `Agile Release Trains` should emphasize vision and program objectives over team goals.  `ARTs` should focus on `Value Stream` objectives over `ART` objectives.  `Value Streams` value contributions to the business portfolio.  Finally, `Management` should focuses on establishing a mission, but with few constraints.
- **Built-in Quality**: `SAFe` contains a number of built-in quality practices to help ensure that every element, within every incremental build, is up to a high standard of quality.
- **Transparency**: Large-scale development is a challenge.  Transparency establishes trust throughout the project by sharing facts and progress openly across all levels.  This extra level of trust enables decentralized decision-making and additional employee empowerment.
- **Program Execution**: Each `Agile Release Train` should predictably generate value.  The `Program Level` within `SAFe` provides responsibilities and guidance to various member roles within `ARTs`, to assist with the generation of value.

## SAFe Implementation

Given the sheer size and scope of `SAFe`, proper implementation can be rather daunting, especially starting out.  Since a full explanation of `SAFe` implementation would require tens of thousands of words -- and because more detailed information is available on the website -- we'll cover a brief overview of implementation here:

1. **Train Implementers**: Due to the sheer scope and challenge required in adopting `SAFe`, most organizations will need a combination of internet and external mentors and coaches.  These people should be capable of easily teaching and delivering `SAFe` techniques to others throughout the organization.
2. **Train Executes, Managers, and Leaders**: The initial batch of `Implementers` should first focus on training all executives, managers, and leaders.  Once these fundamental team members understand the `Lean-Agile` mindset, core `SAFe` principles, and implementation techniques, the process will become much smoother for the entire organization.
3. **Train Teams**: Individuals should initially be organized into `Agile Teams`, who can then all be trained on the various `Lean`, `Agile`, and `SAFe` principles.
4. **Launch `Agile Release Trains`**: Finally, once the organization has been properly trained, it's time to group `Agile Teams` together into `ARTs`, and then generate models for objective planning, program execution, program increment planning, and all the other components required for a successful `Agile Release Train`.

`SAFe` defines three levels within the organization:

- [`Portfolio Level`](http://www.scaledagileframework.com/portfolio-level/): Focuses on the `Portfolio Vision`, creating `Investment Themes` with assigned funding, and makes use of `Kanban`.  [`Epics`](http://www.scaledagileframework.com/epic/) are also devised at this level, which contain significant initiatives to help guide value streams toward the larger portfolio goals.
- [`Program Level`](http://www.scaledagileframework.com/program-level/): As we discussed earlier, `Program Level` focuses on specific business value streams.  One key aspect of `Program Level` is the process of breaking down `Epics` into smaller features that form the [`Program Backlog`](http://www.scaledagileframework.com/program-and-value-stream-backlogs/).
- [`Team Level`](http://www.scaledagileframework.com/team-level/): At the `Team Level`, features from the `Program Level` are broken down further into `Stories`, forming the `Team Backlog`.  `Scrum` is then utilized, over the course of typical iteration lengths (2 weeks), to complete the features of `Stories`.

## Advantages of SAFe

Regardless of any possible downsides, there are clearly a number of positive benefits to using `SAFe`:

- **Promotes `Lean` and `Agile` practices into traditional corporate organizations**: Since `SAFe` focuses on `Lean` and `Agile` principles, this promotes a dramatic cultural shift for many organizations looking to adopt `SAFe`.  While it doesn't require actual restructuring within an organization, `SAFe` does require the creation of "virtual teams," who can then be assigned to `Agile Teams` and then `Agile Release Trains` to fulfill business goals.
- **Emphasizes short term deliveries**: Most traditional organizations may have projects with delivery goals that are months if not years in the future.  `SAFe` focuses on a default period of `10 weeks` for most `Agile Release Trains`, which emphasizes regular feedback loops and adaptive planning.
- **Free of charge**: While `Scaled Agile`, the company behind `SAFe`, does offer numerous training courses to help individuals or organizations get up to speed on `SAFe`, at the fundamental level, the use of `SAFe` is [free of charge](http://www.scaledagileframework.com/usage-and-permissions/).
- **Advocates long-lived teams**: In many organizations, teams are created only for the length of a single project, after which time they are disbanded.  `SAFe` promotes teams that remain together for long periods of time, scaling as necessary across numerous projects.
- **Suited for large organizations**: `SAFe` focuses heavily on supporting very large organizations, through both the practices and implementation itself, to the availability of training and courses from `SAFe` professionals.
- **Emphasizes people over technology**: Due to the reliance on `Lean` and `Agile` principles, `SAFe` heavily focuses on the importance of people and their knowledge over technology.  This recognition of the power and decision-making skills of individuals often leads to products which are better-suited for the customer.

## Disadvantages of SAFe

Critics of `SAFe` will contend that the methodology suffers from a few flaws, and depending on the size and needs of your organization, would argue that other methodologies would better serve you throughout the software development life cycle:

- **Pushes Certification and Training**: The `SAFe` website, which is basically the only real source of information on `SAFe` and proper usage, places a very heavy focus on pushing certification and training courses to organizations looking to implement `SAFe`.  Many critics maintain that this focus on revenue takes away from the potential growth and implementation of the methodology, compared to other, more open methods.
- **Highly Prescriptive**: `SAFe` heavily emphasizes the use of its particular practices and rules, without leaving much room for customization on the part of the organization.  This strictness can be rather stifling unless organizations are willing to swallow the whole `SAFe` pill.
- **"Manager-oriented Agile"**: Many critics argue that `SAFe` fails to truly implement `Agile` principles, but instead gives the _illusion_ of true `Lean` and `Agile` principles by layering `Agile` on top of a pre-existing organizational hierarchy.  In short, this allows managers and executives to make many of the fundamental decisions that trickle down through `Agile Teams` and `ARTs` into `Stories` that must be implemented by the developers and other team members that truly understand the issues first-hand.
The V-Model is a unique, linear development methodology used during a software development life cycle (SDLC).  The V-Model focuses on a fairly typical `waterfall-esque` method that follows strict, step-by-step stages.  While initial stages are broad design stages, progress proceeds down through more and more granular stages, leading into implementation and coding, and finally back through all testing stages prior to completion of the project.

In this article we'll examine just what the V-Model actually entails, and why it may (or may not) be suitable for certain types of projects or organizations.

# The Process of the V-Model

Much like the traditional `waterfall model`, the V-Model specifies a series of linear stages that should occur across the life cycle, one at a time, until the project is complete.  For this reason V-Model is not considered an `agile` development method, and due to the sheer volume of stages and their integration, understanding the model in detail can be challenging for everyone on the team, let alone clients or users.

To begin, it's best to visualize the rough stages of the V-Model, as seen in the diagram below.

![v-model graphic](https://upload.wikimedia.org/wikipedia/commons/e/e8/Systems_Engineering_Process_II.svg)
_Image courtesy of [Wikipedia.org](https://upload.wikimedia.org/wikipedia/commons/thumb/e/e8/Systems_Engineering_Process_II.svg/440px-Systems_Engineering_Process_II.svg.png)_

The V-shape of the V-Model method represents the various stages that will be passed through during the software development life cycle.  Beginning at the top-left stage and working, over time, toward the top-right tip, the stages represent a linear progression of development similar to the `waterfall model`.

Below we'll briefly discuss each of roughly nine stages involved in the typical V-Model and how they all come together to generate a finished product.

__Requirements__

During this initial phase, system requirements and analysis are performed to determine the feature set and needs of users.  Just as with the same phase from the `waterfall model` or other similar methods, spending enough time and creating thorough user requirement documentation is critical during this phase, as it only occurs once.

Another component unique to the V-Model is that during each `design` stage, the corresponding tests are __also__ designed to be implemented later during the `testing` stages.  Thus, during the `requirements` phase, `acceptance tests` are designed.

__System Design__

Utilizing feedback and user requirement documents created during the `requirements` phase, this next stage is used to generate a specification document that will outline all technical components such as the data layers, business logic, and so on.

`System Tests` are also designed during this stage for later use.

__Architecture Design__

During this stage, specifications are drawn up that detail how the application will link up all its various components, either internally or via outside integrations.  Often this is referred to as [high-level design](https://en.wikipedia.org/wiki/High-level_design).

`Integration tests` are also developed during this time.

__Module Design__

This phase consists of all the [low-level design](https://en.wikipedia.org/wiki/Low-level_design) for the system, including detailed specifications for how all functional, coded business logic will be implemented, such as models, components, interfaces, and so forth.

`Unit tests` should also be created during the `module design` phase.

__Implementation/Coding__

At this point, halfway through the stages along the process, the actual coding and implementation occur.  This period should allot for as much time as is necessary to convert all previously generated design and specification docs into a coded, functional system.  This stage should be fully complete once the testing phases begin.

__Unit Testing__

Now the process moves back up the far side of the V-Model with inverse testing, starting with the `unit tests` developed during the `module design` phase.  Ideally, this phase should eliminate the vast majority of potential bugs and issues, and thus will be the lengthiest testing phase of the project.

That said, just as when performing unit testing with other development models, unit tests cannot (or should not) cover every possible issue that can occur in the system, so the less granular testing phases to follow should fill in these gaps.

__Integration Testing__

Testing devised during the `architecture design` phase are executed here, ensuring that the system functions across all components and third-party integrations.

__System Testing__

The tests created during `system design` are next executed, largely focusing on performance and regression testing.

__Acceptance Testing__

Lastly, `acceptance testing` is the process of implementing all tests created during the initial `requirements` phase and should ensure that the system is functional in a live environment with actual data, ready for deployment.

# Advantages of the V-Model

- __Suited for Restricted Projects__: Due to the stringent nature of the V-Model and its linear design, implementation, and testing phases, it's perhaps no wonder that the V-Model has been heavily adopted by the [medical device industry](http://eprints.dkit.ie/144/) in recent years.  In situations where the project length and scope are well-defined, the technology is stable, and the documentation & design specifications are clear, the V-Model can be a great method.
- __Ideal for Time Management__: Along the same vein, V-Model is also well-suited for projects that must maintain a strict deadline and meet key milestone dates throughout the process.  With fairly clear and well understood stages that the whole team can easily comprehend and prepare for, it is relatively simple to create a time line for the entire development life cycle, while generating milestones for each stage along the way.  Of course, the use of BM in no way ensures milestones will always be met, but the strict nature of the model itself enforces the need to keep to a fairly tight schedule.


# Disadvantages of the V-Model

- __Lacks Adaptability__: Similar to the issues facing the traditional `waterfall` model on which the V-Model is based, the most problematic aspect to the V-Model is its inability to adapt to any necessary changes during the development life cycle.  For example, an overlooked issue within some fundamental `system design`, that is then only discovered during the `implementation` phase, can present a severe setback in terms of lost man-hours as well as increased costs.
- __Timeline Restrictions__: While not an inherent problem with the V-Model itself, the focus on testing at the end of the life cycle means that it's all too easy to be pigeonholed at the end of the project into performing tests in a rushed manner to meet a particular deadline or milestone.
- __Ill-Suited for Lengthy Life Cycles__: Like the `waterfall model`, the V-Model is completely linear and thus projects cannot be easily altered once the development train has left the station.  V-Model is therefore poorly suited to handle long-term projects that may require many versions or constant updates/patches.
- __Encourages 'Design-by-Committee' Development__: While V-Model is certainly not the only development model to fall under this criticism, it cannot be denied that the strict and methodical nature of the V-Model and its various linear stages tend to emphasize a development cycle befitting managers and users, rather than developers and designers.  With a method like V-Model, it can be all too easy for project managers or others to overlook the vast complexities of software development in favor of trying to meet deadlines, or to simply feel overly confident in the process or current progress, based solely on what stage in the life cycle is actively being developed.

---

__SOURCES__

- http://eprints.dkit.ie/144/
- https://en.wikipedia.org/wiki/V-Model_(software_development)

# Conceptual Model - What is it and how do you use it?

A `conceptual model` is a representation of a system that uses concepts and ideas to form said representation.  `Conceptual modeling` is used across many fields, ranging from the sciences to socioeconomics to software development.

When using a `conceptual model` to represent abstract ideas, it's important to distinguish between a model _of a concept_ versus a model that _is conceptual_.  That is to say, a model is **intrinsically a thing unto itself**, but that model also contains a concept of **what that model represents** -- what a model _is_, as opposed to what a model _represents_.

Without diving too deep into the philosophical, recognizing these differences between the model itself and what it represents is critical to understanding the proper use of `conceptual models` in the first place.  It should come as no surprise, then, that `conceptual models` are often used as an abstract representation of real world objects.

Throughout this article we'll explore what `conceptual models` are, how they are most commonly implemented, as well as a few advantages and disadvantages of using `conceptual models` in the realm of software development, so let's get to it!

## What is a Conceptual Model?

As touched on above, `conceptual modeling` is used as a way to describe physical or social aspects of the world in an abstract way.  For example, in the realm of software development, a `conceptual model` may be used to represent the relationships of entities within a database.  Whether written down via text or diagrammed visually, such a `conceptual model` can easily represent abstract concepts of the relationships between objects in the system, such as `Users` and their relationship to `Accounts`.

In fact, `conceptual models` within a typical software development life cycle are often referred to as `domain models`.  A `domain model` is a _type_ of `conceptual model` that incorporates representations of both behavior and data at the same time.  As illustrated above, this often represents database entities, using simple diagramming techniques to illustrate `1-to-1`, `1-to-many`, and `many-to-many` relationships within the system.

Overall, a `conceptual model` should fulfill four fundamental objectives:

- Enhance understanding of the representative system.
- Promote efficient conveyance of system details between team members.
- Provide a point of reference for system designers to gather system specifications.
- Document the system for future reference.

Applying these objectives to our example data model above, we can ascertain that a `conceptual data model` should capture the key business entities (a person, place, concept, event, or thing about which the organization wants to collect data), as well as the relationships between these entities.

## Conceptual Model Implementation and Best Practices


Due to the broad spectrum of concepts and inherent abstractness that it can represent, `conceptual modeling` is used in a wide range of projects, across dozens of fields.  Within the realm of software development, as we illustrated above, `conceptual modeling` is most commonly used as a form of `data modeling`; representing abstract business entities and the relationships therein.

The rise of `rapid application development` represents another very common implementation of `conceptual modeling`, which uses abstract models to represent development processes that are rapidly changing and being iterated upon.  Furthermore, within each phase of `rapid application development`, `conceptual models` are typically used to communicate sub-concepts as well.

Even fundamental modeling practices are, by their very nature, forms of `conceptual models`.  One such example is the [`Unified Modeling Language`](https://en.wikipedia.org/wiki/Unified_Modeling_Language), which was created in the mid-90s, and is a general purpose modeling language that attempts to provide a standard method to visualize system design.

Throughout all the various implementations of `conceptual models`, a few best practices have emerged.  A `conceptual model` should:

- ... be available to all team members, to facilitate collaboration and iteration.
- ... be easily changeable, as a continuous reflection of up-to-date information.
- ... contain both visual and written forms of diagramming, to better explain the abstract concepts it may represent.
- ... establish relevant terms and concepts that will be used throughout the project.
- ... define said terms and concepts.
- ... provide a basic structure for entities of the project.

In spite of the name, `conceptual models` are not merely _conceptual_, they are frequently put to use in [real-world scenarios](https://www.acm-sigsim-mskr.org/Courseware/Balci/Slides/BalciSlides-08-ConceptualModeling.pdf).  The `Federal Emergency Management Agency` (`FEMA`) used `conceptual modeling` to develop `Emergency Response Management` systems, as have other vital institutions including the `U.S. Missile Defense Agency` and the `National Institute of Standards and Technology`.

## Advantages of Conceptual Modeling

Since `conceptual models` are merely representations of abstract concepts and their respective relationships, the potential advantages of implementing a `conceptual model` are many, but largely depend on your own ability to devise a strong model in the first place.  Generally speaking, the primary advantages of a `conceptual model` include:

- **Establishes Entities**: By establishing and defining all the various entities and concepts that are likely to come up throughout the course of a software development life cycle, a `conceptual model` can help ensure that there are fewer surprises down the road, where entities or relationships might otherwise have been neglected or forgotten.
- **Defines Project Scope**: A solid `conceptual model` can be used as a way to define project scope, which assists with time management and scheduling.
- **Base Model for Other Models**: For most projects, additional, less abstract models will need to be generated beyond the rough concepts defined in the `conceptual model`.  `Conceptual models` serve as a great jumping-off point from which more concrete models can be created, such as `logical data models` and the like.
- **High-Level Understanding**: `Conceptual models` serve as a great tool by providing a high-level understanding of a system throughout the software development life cycle.  This can be particularly beneficial for managers and executives, who may not be dealing directly with coding or implementation, but require a solid understanding of the system and the relationships therein.

## Disadvantages of Conceptual Modeling

Since a `conceptual model` is so abstract, and thus, is only as useful as you make it, there _can be_ a few disadvantages or caveats to watch out for when implementing your own `conceptual model`:

- **Creation Requires Deep Understanding**: While `conceptual models` can (and should) be adaptive, proper creation and maintenance of a `conceptual model` requires a fundamental and robust understanding of the project, along with all associated entities and relationships.
- **Potential Time Sink**: Improper modeling of entities or relationships within a `conceptual model` may lead to massive time waste and potential sunk costs, where development and planning have largely gone astray of what was actually necessary in the first place.
- **Possible System Clashes**: Since `conceptual modeling` is used to represent such abstract entities and their relationships, it's possible to create clashes between various components.  In this case, a clash simply indicates that one component may conflict with another component, somewhere down the line.  This may be seen when `design` or `coding` clash with `deployment`, as the initial assumptions of scaling during `design` and `coding` were proven wrong when actual `deployment` occurred.
- **Implementation Challenge Scales With Size**: While `conceptual models` are not inherently ill-suited for large applications, it can be challenging to develop and maintain a proper `conceptual model` for particularly complex projects, as the number of potential issues, or `clashes`, will grow exponentially as the system size increases.
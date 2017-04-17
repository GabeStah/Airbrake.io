# Object-Oriented Analysis and Design - What is it and how do you use it?

Most developers are well-aware of the concepts of `object-oriented` development, but those same concepts originate from a broader approach to the entire software development life cycle known as `object-oriented analysis and design` (`OOAD`).  `OOAD` is a technical method of analyzing and designing an application based on that system's `object models` (the logical components of the system that interact with one another).

We'll take the time in this article to explore exactly what `object-oriented analysis` and `object-oriented design` are, how these techniques are typically used in modern development, and any potential advantages or disadvantages you may consider when implementing `OOAD` into your own work.  Let's get going!

## Origins of Object-Oriented Analysis and Design

During the software development life cycle, development is typically broken up into `stages`, which are loose, abstract concepts used to separate the activities taking place within each phase of development.  Often, these stages might include `requirements`, `planning`, `design`, `coding/development`, `testing`, `deployment`, `maintenance`, and so forth.  

In the case of stringent development methodologies, such as the [waterfall method](https://airbrake.io/blog/sdlc/waterfall-model), these stages are sequential and intended to be completely separate from one another.  Thus, when creating an application using the waterfall method, it's unlikely that discoveries made during the `testing` or `deployment` phases can impact the decisions already made during the `planning` or `design` phases.  These limitations, along with the strict step-by-step staging process of waterfall-esque models, led to the rise of `iterative` models like `object-oriented analysis and design`.

While `OOAD` practices have been around for a number of decades, the core ideas and techniques were largely cemented in the collective mind of the development community in the 1990s.  An assortment of practitioners and authorities in the industry, working together and on solo endeavors, began to publish a number of books, articles, and techniques that all relied heavily on `OOAD` concepts.  Some of these publications and methodologies are still well-known and in use today, including the [`Unified Modeling Language`](https://en.wikipedia.org/wiki/Unified_Modeling_Language) and the [`Rational Unified Process`](https://airbrake.io/blog/sdlc/rational-unified-process).

## What is Object-Oriented Analysis?

To define `object-oriented analysis` we must first define what we mean by an `object`.  The definition of an `object`, according to most dictionaries, is "a tangible, material thing."  Drilling down a bit more to the realm of computer science, an `object` can be most anything in a programmatic sense, from a variable or data model to a function, class, or method.  Moving even deeper into the realm of `object-oriented programming`, an `object` is an instance of a thing that typically represents a real world object and has all the same types of characteristics (`properties`), behaviors (`methods`), and states (`data`).  When discussing `OOAD` concepts, an `object` most closely resembles the `object-oriented programming` version of an `object`, in that it is a representation of a real world object with behaviors, characteristics, and states.

With that out of the way, we can define `object-oriented analysis` (`OOA`).  In short, `OOA` is an iterative stage of analysis, which takes place during the software development life cycle, that aims to model the **functional** requirements of the software while remaining completely independent of any potential **implementation** requirements.  To accomplish this task via `OOAD` practices, an `object-oriented analysis` will focus everything through the lens of `objects`.  This forces `OOA` to combine _all_ `behaviors`, `characteristics`, and `states` together into one analysis process, rather than splitting them up into separate stages, as many other methodologies would do.

To accomplish this goal, a typical `OOA` phase consists of five stages:

- Find and define the objects.
- Organize the objects.
- Describe how the objects interact with one another.
- Define the external behavior of the objects.
- Define the internal behavior of the objects.

For example, a typical implementation of `OOA` is to create an `object model` for an application.  The `object model` might describe the `names`, `relationships`, `behaviors`, and `characteristics` of each object in the system.  With this information established for each object, the design process that follows is much simpler.

## What is Object-Oriented Design?

The process of `object-oriented design` is really just an extension of the `object-oriented analysis` process that preceded it, except with a critical caveat: the consideration and implementation of `constraints`.  For example, with an `analyzed` object in hand, such as an `object model`, we must now consider how that object would actually be _designed_ and implemented, which will often require the application of constraints, such as software or hardware platforms, time and budgetary limitations, performance requirements, developer aptitude, and so forth.

Put another way, the `OOD` process takes the _theoretical_ concepts and ideas planned out during the `OOA` stage, and tries to find a way to design and tangibly implement them, usually via code using whatever language and platforms the development team has settled upon.  If `OOA` is the **what**, then `OOD` is the **how**.

## Advantages of Object-Oriented Analysis and Design

- **Encourages Encapsulation**: Since everything within `OOAD` revolves around the concept of `objects` (specifically, the `object-oriented` variety), one of the biggest advantages of `OOAD` is that it encourages planning and development of systems that are truly independent of one another.  Just like a `class` written using `object-oriented` techniques, all the systems and objects produced during an `OOAD` development life cycle can be mixed and matched as necessary, since they will ideally be built as completely self-contained entities.
- **Easy to Understand**: Since `OOAD` principles are fundamentally based on real world objects, it's quite easy for everyone on the team to quickly understand what an object name means or how a particular behavior, well, behaves.  This makes the overall development life cycle a much smoother process, particularly if your team needs to frequently interact with customers or other non-technical users about the objects and components in the system.  In such cases, most people still understand how system components and modelled objects work when they're based on real world objects and ideas.

## Disadvantages of Object-Oriented Analysis and Design

- **Ill-Suited to Procedural Applications**: Given the object-oriented nature of `OOAD`, it is quite difficult (although not impossible) to practice `OOAD` techniques within a procedural programming language, or often to apply the techniques to non-object business logic.  Whereas procedural applications are often logically bound by concepts of scope and modularity, object-oriented applications, of course, emphasize _objects_ that simulate the real world, making `OOAD` methods ill-suited for procedural languages and applications.
- **Too Complex for Simple Applications**: While arguably not a disadvantage that is applicable to all projects, it's certainly the case that `OOAD` practices are generally not ideal for simpler projects.  Many developers have their own personal hard and fast rules to help when deciding whether a project should be procedural or object-oriented, but in most cases, the more basic the needs of the application, the more likely a less-structured, procedural approach is the best fit.  As always, we must always use our own best judgment.

---

__META DESCRIPTION__

A close look at object-oriented design and analysis in software development, including what it is, how it's used, and a few pros and cons.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Object-oriented_design
- https://en.wikipedia.org/wiki/Object-oriented_analysis_and_design
- https://www.amazon.com/Object-Oriented-Analysis-Design-Applications-3rd/dp/020189551X
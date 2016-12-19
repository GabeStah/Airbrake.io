Today we'll be taking a closer look at a unique, yet rather popular, software development model, colloquially known as the `Big Bang model`.  While the tenets of the `Big Bang model`, such as they are, are almost excessively simple, use of the model should not be wholly discarded or ignored, as it can be a powerful tool to manage the software development life cycle ([`SDLC`]) of a wide range of projects.

Loosely based on the cosmological model and theory of the same name, the `Big Bang model` of software development is designed around the notion that, beginning with nothing, a rapid growth and expansion of code will quickly emerge, thus producing a finished product in a mere instant (relatively speaking).

In this article, we'll take a closer look at what exactly the `Big Bang model` is within the context of software development, how it is typically implemented, some cautions for using it as a deployment method, and both the advantages and disadvantages of the method overall.  Let the bangin' begin!

## The Only Step: Bang

Unlike nearly all other popular [`SDLC`] models, the `Big Bang model` is unique in that it requires virtually no planning, organization, best practices, or typical procedures.  Instead, the `Big Band model` is fundamentally about simply starting the project __right now__, at this instant, with no formal development structure or organization.  It is typical that very few if any team members, let alone the customer, are completely familiar with what the requirements are for the project, nor what is necessary to meet those goals, and thus every single aspect of the project is developed and implemented on the fly.

Day-to-day development occurs at the whim of the team members and is loosely based on the requirements that are generally known and accepted at that present moment, without much regard for consequences or future requirements that may arise.  Everything about the project is completed with a laissez-faire attitude, with little regard or care for what may come next, and instead simply enjoying the work that is being performed at that very moment.  In short, the `Big Bang model` is the heavy metal of software development methodologies.

## Big Bang Deployment

As a lesser component of software development, the `deployment` phase of a product or release is typically one of the final and most critical steps in the software development life cycle.  While most projects will opt for a typical, incremental rollout for a new deployment, in some instances it can be infeasible to slowly introduce a new system, particularly when creating a dramatically new version of a product or release.

In such cases, the only logical solution is a deployment that occurs instantly, like flipping a switch.  This method is often referred to as [`big bang adoption`], which requires that all users of a system are moved to the new system at a specific date and time.

It should come as no surprise that implementing any system via big bang deployment -- in particular a new software system -- is fraught with numerous risks.  In many cases, unforeseen issues can cause quirks in the system or failures to properly handle migrated information that may exist when coming from a previous version of the system.

In the worst cases, such instantaneous deployment can lead to bugs in the code that are so severe, the system virtually fails to function on all fronts.  One such real-world example of a big bang deployment that went terribly wrong was in 1992 with the [`Computer Aided Dispatch`] program for the `London Ambulance Service`.  The software was intended to facilitate the rapid allocation of responding ambulances to emergency services calls.

Unfortunately, within just a few hours of the system going live, numerous problems arose, such as the software sending multiple units to one location while other locations were completely ignored.  The application began producing such as massive number of error messages on dispatchers' terminals that incoming emergency calls were completely lost.  A mere eight days after the system was launched, it was completely shut down in favor of the old, manual system.

## Advantages of the Big Bang Model

- __No Planning Requirements__: Perhaps the biggest advantage to the `Big Bang model` of software development is the ability to __just start coding__.  With no formal need to write out tons of requirements, story cards, or specification documents, developers can simply sit down, open their favorite editor, and begin hacking away at the code itself, potentially producing something tangible and functional in a relatively short period time.
- __No Management Requirements__: With no planning documentation or formalized requirements, there's also very little need for managerial staff.  In most cases with a `Big Bang model` project, everyone on the team will effectively be peers, with no formal management hierarchy.  
- __Well-Suited for Small Projects__: Given the chaotic nature of a project implemented using the `Big Bang model`, it should come as no surprise that the kind of application most suited to this method is that of a particularly small venture.  The `Big Bang model` is actually quite common, whether developers know or acknowledge they're using it or not, within small teams of just a handful of developers.  [`Hackathons`], where collaborators get together over the course of a few days to rapidly develop a piece of functional software, are a great example of where the `Big Bang model` really shines.
- __Great Introduction to Software Development__: While certainly not always relevant to most projects, the `Big Bang model` is particularly great at introducing newcomers to the core concepts of software development, as it allows them to get straight into the code and focus on the development aspects, without being bogged down in the minutiae of most typical `SDLC` models, like documentation, testing, and so forth.
- __Ideal of Multi-Disciplined Developers__: Since everyone involved in the project is likely to be a developer, `Big Bang model` projects are ideal at catering to the skills of multi-disciplined or full-stack developers, who are capable of working with a wide range of technologies.

## Disadvantages of the Big Bang Model

- __Extremely Risky__: Perhaps it goes without saying, but the use of the `Big Bang model` for all but the simplest of projects comes with some very real and very dangerous risks.  Without proper planning, formal leadership, or even standard coding practices and procedures, it is all too easy to run into a potentially serious problem later down the line once deep into the project, which may require a massive if not total rewrite of the entire code base up to that point.  Still, if you want to code like a rock star, there's no better way to do it!
- __Too Simple for Complex Projects__: While the `Big Bang model` works fairly well with smaller projects and tiny teams, on the flip side, it is categorically disastrous for larger or long-term projects.  Even in the best case for a lengthy project, problems would be discovered frequently but relatively soon after they were created, which would already require hours if not days of necessary refactoring and alterations to existing components.  In the worst case, as illustrated earlier with the `London Ambulance Service's` debacle, the entire system can come crashing down.
- __Potentially Expensive__: While the `Big Bang model` is about as inexpensive a model as it gets when the project first gets out of the gates, it also comes with some risks, which can cause it to unintentionally become one of the most expensive `SDLC` models in the long run.  While initial funds and budget will be spent on a whim for whatever is necessary to get the ball rolling, such lack of planning can quickly spiral out of control when requirements of the project dramatically change without notice.




[`SDLC`]: https://airbrake.io/blog/category/sdlc
[`Hackathons`]: https://en.wikipedia.org/wiki/Hackathon
[`Big bang adoption`]: https://en.wikipedia.org/wiki/Big_bang_adoption
[`Computer Aided Dispatch`]: http://erichmusick.com/writings/technology/1992-london-ambulance-cad-failure.html

---

__SOURCES__

- https://lameguy.wordpress.com/2013/05/29/the-big-bang-software-development-lifecycle-model/
- https://www.tutorialspoint.com/sdlc/sdlc_bigbang_model.htm
- https://books.google.com/books?id=5wEQCwAAQBAJ&printsec=frontcover
- https://en.wikipedia.org/wiki/Big_bang_adoption
- https://www.wired.com/2009/10/1026london-ambulance-computer-meltdown/

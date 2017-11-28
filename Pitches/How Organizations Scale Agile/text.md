# How Organizations Scale Agile

Many organizations are looking at ways to scale Agile implementations, but doing so can be a particularly challenging for businesses not well-versed in Agile practices.  Properly scaling more traditional Agile frameworks can be difficult for organizations with excessive personnel or an abundance of individual teams working on one or more products simultaneously.

However, not all hope is lost.  In this article we'll explore a number of techniques and existing frameworks aimed at helping you and your organization scale your Agile implementations as large and as wide as they need to be.  Let's get started!

## Why Scaling Agile Matters

Understanding how to scale Agile requires an understanding of the key concepts and practices your organization should strive for and try to implement:

- **Maintain Small Team Sizes** - Keeping smaller teams ensures every team member to absorbs all relevant information, and allows members to effectively contribute to the work.
- **Reduce Iteration Durations** - A particular challenge for many organizations trying to scale Agile practices is finding the correct balance between iteration length and actual production.  However, successful Agile implementations almost universally focus on _shorter_ iteration durations wherever possible, so implementing what may seem a rather strict limitation at first will pay off in spades later down the road.
- **Practice Organization-Wide Synchronicity** - Many aggressively-scaled Agile implementations will be performing simultaneous work across multiple teams, and likely on multiple products.  It can be challenging to coordinate the contributions from across the entire company when each team is working with their own specific iteration duration and schedule.  Thus, it is critical to the overall organization's success that plans are made to synchronize the end-of-iteration periods across multiple teams.  Failing to do so can lead to one team implementing a feature before another team is prepared for it, causing a negative cascade effect as teams throughout the organization must devote resources to adjust to said new feature.
- **Utilize Specialized Roles** - Many Agile practices have historically recommended a broadening of skill sets throughout the team, allowing for more generic, less specialized work to be performed.  However, many managers and organizations are finding benefits from moving in a direction toward more specialization in roles, allowing for faster iteration and turnaround.
- **Evaluate Release Window Scheduling** - Most Agile implementations are structured around a series of releases, each of which is built up of a series of iterations.  It is common practice for each release to coincide with the calendar quarter (e.g. six 2-week iterations, four 3-week iterations, etc).  However, since a scaled Agile implementation is likely to be implemented by a larger organization, it may be worth synchronizing release cycles with _business_ cycles, such as quarterly earnings cycles and the like.  This allows the organization to better adapt to external factors between each release.
- **Assign Product Owner Roles** - Even if your organization isn't using Scrum there are numerous benefits to assigning a product ownership role to at least one team member per product.  This individual should be the go-to contact and line of communication with users, able to communicate value-based priorities to the rest of the team throughout development.
- **Select Elevated User Roles** - Just as a product owner role benefits the team, at least one user should be selected to represent the larger user base throughout the development life cycle.  This ensures that communication lines stay open and that the team is able to adapt to user feedback throughout the process.
- **Increase Turnaround** - Scaling Agile dramatically benefits from performing a multitude of value-based iterations, as opposed to longer duration iterations in traditional models.  This quick turnaround allows for frequent, constant user feedback throughout the entire product life cycle.

## Exploring Possible Frameworks

There are a few reliable and well-tested frameworks that are explicitly aimed at scaling Agile beyond the needs of most smaller organizations.  Below we've selected a handful of the most popular of these frameworks and provided a brief overview of each.

### Scrumming Your Scrums

For organizations already implementing the Scrum framework, one obvious way to scale Agile is to create a _scrum of scrums_.  Since multiple teams across the organization are each using scrum, a scrum of scrums is a meeting used to keep people across the organization informed about important issues across the entire company.  However, this meeting should not be a simple status meeting.  Instead, each team elects a representative to attend these meetings.  Just like a typical `daily stand-up` in scrum, the scrum of scrums meeting should be a short (~15 minute) meeting every day where teams can share knowledge and discuss important integration issues that may affect other teams.

If this practice feels beneficial and proves successful, it may also benefit your organization to perform similar cross-team meetings to perform sprint planning and sprint retrospectives.  This allows representatives attending these meetings to inform the rest of their teams about potential roadblocks within upcoming sprints.

### The Scaled Agile Framework

[Scaled Agile Framework](http://www.scaledagileframework.com/) (SAFe) attempts to create a more structured approach to scaling Agile practices than something like a scrum of scrums.  It accomplishes this by defining three different levels of activity within the organization: `portfolio`, `program`, and `team`.  The `portfolio` level contains principles, practices, and roles needed to initiate and govern a set of value streams.  The `program` level contains the roles and activities needed to continuously deliver solutions using an Agile Release Train.  The `team` level contains the value-based roles, activities, events, and processes which the team creates and delivers.

Each area of related work is known as a `theme`, which maps business and architectural epics throughout the project.  Business epics are user-facing, such as launching a new product.  Architectural epics are company-facing, such as altering the server back end of a software application.  The combination of these epics make up the `portfolio backlog`.

Team management and technical leadership prioritizes elements in the portfolio backlog, which transforms each business and architectural epic into its own Agile program with its own Agile Release Train (ART).  Since multiple teams can work together on the same program (and, thus, within the same Agile Release Train), the Scaled Agile Framework is well-suited to larger organizations looking to scale Agile.

### Large Scale Scrum (LeSS)

[Large Scale Scrum](https://less.works/less/framework/index.html) (LeSS) is focused on directing the attention of all teams throughout the organization onto the product as a whole, rather than on their individual or team-exclusive responsibilities.  To accomplish this LeSS actually has two different frameworks that can be implemented, depending on overall team size:

- **LeSS**: Up to eight teams (of eight people each).
- **LeSS Huge**: Up to a few thousand people on one product.

LeSS is similar to comparable to a one-team Scrum in a variety of ways, including:

- A single Product Backlog (because it’s for a product, not a team).
- One Definition of Done for all teams.
- One Potentially Shippable Product Increment at the end of each Sprint.
- One Product Owner.
- Many complete, cross-functional teams (with no single-specialist teams).
- One Sprint.

On the other hand, LeSS attempts to improve upon traditional scrum practices in a number of ways:

- **Sprint Planning**: In addition to a single Product Owner, the Sprint Plan should include people from all teams. This allows team members to manage themselves and decide upon their own division of `product backlog items`.  Team members should also discuss opportunities to cooperate on shared work.  Sprint Planning should also occur independently within each team, though it may be useful and occasionally necessary to coordinate between multiple teams.
- **Daily Scrum**: These should be performed independently by each team, though elected members may observe other teams' daily scrums to improve sharing of information across team lines.
- **Product Backlog Refinement**: There may be an optional and short overall Product Backlog Refinement (PBR) meeting that includes the one Product Owner and people from all teams.  The key purpose is to decide which teams are likely to implement which items and therefore select those items for later in-depth single-team PBR.  It is also a chance to increase alignment with the Product Owner and all teams.  Typically, a LeSS implementation will utilize a single-team PBR.  However, it may be beneficial for your organization to perform multi-team PBRs in which multiple teams meet together to improve communication and coordination.
- **Sprint Review**: In addition to the one Product Owner, a Sprint Review should include people from all teams, along with any relevant users, customers, and/or stakeholders.  For the phase of inspecting the product increment and new items, consider using large room (virtual or otherwise) that contains multiple areas each staffed by team members, in which the items developed by said teams are displayed, demoed, and discussed.
- **Overall Retrospective**: This is a new meeting not found in one-team Scrum, and its purpose is to explore improving the overall system, rather than focusing on one team.  The maximum duration is 45 minutes per week of Sprint.  It should include the Product Owner, Scrum Masters, and rotating representatives from each Team.

---

__META DESCRIPTION__

An exploration of the techniques and common frameworks used by organizations to properly scale Agile implementations when team sizes start to expand.

---

__SOURCES__

- https://insights.sei.cmu.edu/sei_blog/2017/02/five-perspectives-on-scaling-agile.html
- http://www.scaledagileframework.com/
- https://www.atlassian.com/agile/ways-to-scale-agile
- http://agileforall.com/case-study/whole-foods-market/
- https://less.works/less/framework/index.html
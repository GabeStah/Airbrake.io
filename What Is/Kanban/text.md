# Kanban Methodology - What is it and how do you use it?

`Kanban` (or `看板`, literally meaning `a sign` in Japanese), is used today to signify a form of manufacturing in which all necessary components are managed through the use of a card that indicates missing components.  The purpose of `kanban` is to control inventory throughout the supply chain, within a manufacturing setting, via a practice known as `just-in-time` (`JIT`) manufacturing.  This ensures that the `supply` and `demand` of manufacturing components are perfectly balanced with one another, largely eliminating wasted materials and time.

In modern software development, `kanban` (or the `kanban methodology`) expands on those same `just-in-time` concepts by ensuring that the level of required work at any given time roughly equates to the work capabilities of the team.  Thus, development work is performed in a `just-in-time` fashion, with similarly minimal waste.

In this article we'll further explore what the `kanban methodology` is, how it is most commonly implemented in the software development life cycle, as well as any advantages and disadvantages you might experience when trying to implement `kanban` yourself.  Let's get to it!

## What is Kanban?

Just as with the `kaizen model` [that we explored in a previous article](https://airbrake.io/blog/sdlc/kaizen-model), the `kanban method` was originally developed inside Toyota Motor Corporation, by an industrial engineer named [`Taiichi Ohno`](https://en.wikipedia.org/wiki/Taiichi_Ohno).  The goal was to drastically reduce waste across the manufacturing floor at Toyota, and `kanban methods` proved to do exactly that.  `Toyota's Six Rules`, as described by Taiichi Ohno in his 1998 book [_Toyota Production System: Beyond Large-Scale Production_](https://www.amazon.com/Toyota-Production-System-Beyond-Large-Scale/dp/0915299143), describe the company's methods for applying `kanban` principles:

- Later process picks up the number of items indicated by the kanban at the earlier process.
- Earlier process produces items in the quantity and sequence indicated by the kanban.
- No items are made or transported without a kanban.
- Always attach a kanban to the goods.
- Defective products are not sent on to the subsequent process. The result is 100% defect-free goods.
- Reducing the number of kanban increases the sensitivity.

### Kanban Cards

A `kanban card` is a fundamental component of the `kanban` process.  A `kanban card` is simply a written message that indicates the need to replenish a particular component using in the manufacturing process, such as parts or materials.  In Toyota's case, a `kanban card` delivered to one section of the factory may have indicated a depletion of `wheels`, for example.  This would, in turn, trigger a replenishment process for a new batch of `wheels` to be sent (also through the use of a `kanban card`).  To keep things running smoothly, there would typically already be a batch of `wheels` waiting for pickup when a depletion `kanban card` was being delivered.  This would ensure that the process kept running smoothly across the entire factory, and further guaranteed that the consumption of one component (such as `wheels`) was always the driving force and clear indicator for more of that product.

## What is the Kanban Methodology?

Moving into the realm of modern software development, `kanban` has been adapted to fit the needs of most development life cycles.  As previously discussed, the `kanban methodology` aims to balance the level of `active` or `in progress` work with the capabilities of the team to handle said work.  If your team is handling all current `work in progress` with ease, more tasks can be added to that `in progress` list, to hopefully ensure there's a constant balance of efficiency.

If executed properly, the `kanban methodology` can make planning and adapting to changes easier for your team.  This will also result in producing better code and at a faster rate, ensuring a focused agenda, and providing organization-wide transparency throughout the entire software development life cycle.

## Implementation of the Kanban Methodology

When applied to a software development life cycle, the `kanban methodology` has a few different styles of implementation, depending on the particular needs of your project or organization.

### The Kanban Board

A `kanban board` is a workflow management and visualization tool, on which `kanban cards` are placed and moved around to indicate where in the process those `cards` reside.  For older implementations, a `kanban board` would've been a physical board, but most modern implementations use a virtual board, through their own project management software, such as [`Trello`](https://trello.com/).  Obviously, modern software development teams can still opt for a physical board, but the benefits of a virtual board are tough to beat, so it's highly recommended you go that route if you're giving `kanban` a try.

Functionally, the `kanban board` serves one simple purpose: To visualize and standardize the workflow throughout the project into easy-to-use categories.  The actual categories that `kanban cards` can be placed inside your `board` are up to you and dependent on the needs of your team, but at the most basic level, these categories should consist of:

- `To Do`: Tasks that have yet to be worked on.
- `In Progress`: Tasks that are actively being worked on.
- `Complete`: Tasks that are complete.

Thus, the path of a particular task (or `kanban card`) is from left to right, always beginning in the `To Do` category, then to `In Progress`, before finally ending at `Complete`.

Of course, many projects will require additional categories.  For software development, we might also want to slip in phases such as `Planned`, `In Development`, and `In Testing`, but the same principles apply, moving from one side to the other.

### The Kanban Card

A `kanban card`, like the `board`, can be physical or virtual, but it always represents a task to be worked on.  Different implementations of `kanban` will include different fields within the `card`, but at the simplest level you'll want:

- `ID`: The unique identifier of this particular card.
- `Assignees`: People working on the task.
- `Description`: What the task is.
- `Type`: Type of task, such as `feature`, `defect`, `user story`, etc.
- `Timeframe`: Estimated time to complete the task (either this phase or in total).
- `Blocked Indicator`: Whether or not this is a `blocked card` (see below).

The freedom of `kanban` is that `cards` can (and should) be created and managed by anyone and everyone on the team.  `Cards` are often color-coded to indicate the `type` of task.  That color coordination, along with their placement in the relative columns or categories on the `board`, provides a clear, visual way to evaluate the progress of individual tasks or the entire project as a whole.

#### Blocked Cards

`Kanban` also typically includes an indicator for `blocked cards`, usually with a red marker at the corner of the `card`.  A `blocked card` is a `card` that _cannot be progressed_ until some other action is taken.  Often, this blockage is due to the lack of progress on a different `card`.  For example, the `testing user login card` will likely remain `blocked` until the `develop user login functionality card` is finished.

## Advantages of the Kanban Methodology

- **Reduces Waste**: Even the original implementation of `kanban` was focused on eliminating waste during the manufacturing process, and those same practices apply to its use throughout the software development life cycle.  `Kanban` promotes transparency and an `agile` workflow, allowing the whole team to stay abreast of the progress of everything in the project, and therefore what any one individual can jump onto and focus on for the next day or week.
- **Forces Event-Driven Workflow**: Since everything in `kanban` is based on tasks (`cards`), and their relative progress throughout the stages of development on the `board`, `kanban` is entirely event-driven.  This allows the team to constantly shift and adapt, as some `cards` become prominent while others are completed and can be ignored.
- **Allows for Flexibility**: Aside from the basic concepts of using `boards` and `cards`, actual implementation of the `kanban methodology` is very flexible.  Unlike some SDLC methods, which are strict in their implementation, `kanban` allows your team to modify the `boards`, `cards`, and the fields therein, to best meet the needs of your project.
- **Encourages Adaptive Workloads**: Since `cards` can always be shifted to different categories, there's always a simple way to adjust the workload whenever required.  If work capacity is available, more `cards` can be added, whereas if capacity is limited, `cards` can be removed or adjusted.

## Disadvantages of the Kanban Methodology

- **Required Constant Board Monitoring**: While many teams may consider this a benefit, it cannot be denied that a `kanban board` must be constantly surveilled, to ensure that the `cards` do not become outdated, thus causing more harm than good.
- **Potential for Complexity**: Since `kanban` is such a flexible methodology, it's entirely possible for a `board` to be created that is dangerously over-engineered and complicated.  Perhaps there are too many category `types` or card `types`, or simply an excess of `cards`, but no matter the cause, the result can often be that the system is too confusing to easily parse and make use of.
- **Possible Bottlenecks**: If your team doesn't plan for and deal with `blocked cards` well, it's also possible to be "stuck" for a long time, waiting on one particular `card` or component to be completed.  While this could arguably occur using other methodologies, `kanban` presents a particular danger since its focus on `cards` makes scheduling and milestoning rather difficult.

---

__META DESCRIPTION__

An in-depth exploration of kanban practices, its beginnings, and using the kanban methodology throughout the software development life cycle.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Kanban
- https://www.atlassian.com/agile
- https://www.amazon.com/Toyota-Production-System-Beyond-Large-Scale/dp/0915299143
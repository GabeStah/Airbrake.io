# How Your Software Methodology Affects Production Behavior

Dozens of development methodologies have popped up over the relatively few decades in which modern software development has taken place.  Each method provides its own benefits and challenges throughout the software development life cycle, but it can be particularly difficult to determine how your software methodology affects production behavior of your application.

In today's article we examine a handful of the most popular software development methodologies by summarizing what they accomplish, how they work, and what benefits (and possible detriments) using each particular model will have in a production environment.  Away we go!

## Agile Model

The `agile methodology` encourages every project to be handled differently, based on the individual needs of the project, the schedule, and the team behind it.  Unlike other software methodologies, the agile model focuses less on specific requirements or guidelines, and far more on abstraction of these best practices to allow for greater flexibility, or _agility_, during the development process.

### Agile Fundamentals

- **Individuals and interactions**: Rather than solely emphasizing systems and tools, the focus should be on the people within the team and their professional interactions with one another.  For a project to be successful, it should adapt to not just the systems or budget available, but most importantly to the people working on it.  If team members are efficient and effective at working together, the end result will be a polished and optimized product.
- **Working software**: While documentation can certainly be very beneficial during development, it is far better to produce a working product, or even a simple prototype, that illustrates the design goals or the components used throughout the application.  This is beneficial not only to other team members working on development, management, and marketing, but especially to clients or testers who would otherwise be forced to rely on a handful of documents and illustrations to understand how the application is expected to function.
- **Customer collaboration**: It is critical that the project be constantly open, willing, and able to respond to customer feedback and behavior.  By keeping customers or clients in the loop throughout the entire life cycle, everyone involved will be on the same page and there will not be any surprises at the end or massive rewrites necessary because a module or integration wasn't clear for all parties involved.
- **Responding to change**: Perhaps the most critical principle across the entirety of the `agile model` is the ability for the project to adapt and respond to the ever-changing needs of everyone and everything involved.  As development progresses, software technologies will change, the team will shift, clients will hem and haw, and throughout it all, the project should remain malleable and remain capable of adapting along with these needs.

### Agile in Production

- **Constant User Feedback**: Although the constant communication with customers and clients requires a dedicated team member to take on this responsibility, it is a critical component to ensuring the success of the product from both a development perspective as well as from that of the client.  During production, an `agile` project can maintain a constant state of user feedback, which can be parsed and iterated on during future release cycles.
- **Highly Adaptive**: Once the project has been released to a production environment, new information will come pouring in about how the software is performing and where improvements can be made.  The `agile method` makes it particularly easy to adapt to these necessary changes on a whim, without the abundance of bureaucracy or paperwork that may be required with other methodologies.
- **Typically Minimal Documentation**:  The `agile model` largely forgoes initial efforts to heavily design and document the project requirements or scope, in favor of getting into the meat of the project and beginning that iterative process.  This can be a challenge for some projects, particularly after production launch when customers, users, and even other team members may not be as well-versed in how to use the application as the primary developers.  Be sure that your project is adequately documented _before_ (or shortly after) production release.
- **Risk for Increased Technical Debt**: `Agile` projects tend to rack up higher levels of `technical debt` than projects developed with other methodologies, which can cause the application to contain numerous "easy fixes" to solutions, as opposed to more robust but also more difficult to implement solutions.  Prior to production, make sure there is as little technical debt hanging around the necks of the development team for this application.

## Big Bang Model

The `Big Bang model` is encourages a team to start on the project _immediately_, with no formal development structure or organization in place.  The model is unique in that it requires virtually no planning, organization, best practices, or typical procedures.  It is rare that any team members, let alone users, should be completely familiar with what the requirements are for the project, nor what is necessary to meet those goals, and thus every single aspect of the project is developed and implemented on the fly.

### Big Bang Fundamentals

- **Start With a Bang**: The `Big Bang` model really has only one tenet.  Day-to-day development should occur at the whim of team members, and should be loosely based on the requirements that are generally known and accepted at the time, without much regard for consequences or future requirements that may arise.  Everything about the project is completed with a laissez-faire attitude, with little regard for what may come next.  In short, the `Big Bang model` is an explosive, chaotic form of software development.

### Big Bang in Production

- **Ideal of Multi-Disciplined Developers**: Since everyone involved in the project is likely to be a developer, a `Big Bang model` project must be supported by full-stack or multi-disciplined developers.  This is particularly true when the project is released to production, since the documentation and overall understanding of the project will be fairly limited to only a handful of the most prolific members of the team.
- **Problematic for Complex Projects**: While the `Big Bang model` works fairly well with smaller projects and tiny teams, on the flip side, it is categorically disastrous for large or long-term projects.  The production behavior for small projects is fairly predictable, but for a large or even moderately-sized project developed using the `Big Bang` method, it's likely to be too difficult for the team to account for and track what's going wrong and how to fix it.
- **Prone to Expensive Problems**: If your team is not exceptionally careful and meticulous, a `Big Bang` project is likely to result in one or more issues that will be _extremely_ expensive to fix.  While initial funds and budget will be spent on a whim for whatever is necessary to get the ball rolling, this lack of planning can quickly spiral out of control when requirements of the project dramatically change without notice, which will be amplified once outside parties and users are also involved.

## Conceptual Model

`Conceptual modeling` is the practice of describing physical or social aspects of the world in an abstract way.  For example, in the realm of software development, a `conceptual model` may be used to represent the relationships of entities within a database.  Whether written down via text or diagrammed visually, such a `conceptual model` can easily represent abstract concepts of the relationships between objects in the system, such as `Users` and their relationship to `Accounts`.  In fact, `conceptual models` within a typical software development life cycle are often referred to as `domain models`.  A `domain model` is a _type_ of `conceptual model` that incorporates representations of both behavior and data at the same time.

### Conceptual Model Fundamentals

- **Transparent**: All aspects of a `conceptual` project should be open to all team members, to better facilitate collaboration and iteration.
- **Adaptive**: The project should be easily changeable, as a continuous reflection of up-to-date information.
- **Documented**: A `conceptual model` application should consist of both visual and written forms of diagramming, to better explain the abstract concepts represented within it.  
- **Defined**: All relevant terms and concepts used throughout the project should be described and written down in an accessible format.
- **Descriptive**: Finally, a `conceptual model` should provide a basic structure for all entities defined in the project.

### Conceptual Model in Production

- **Defines Critical Entities**: By establishing and defining all the various entities and concepts that are likely to come up throughout the course of the software development life cycle, a `conceptual model` can help ensure that there are fewer surprises down the road, where entities or relationships might otherwise have been neglected or forgotten.  This is particularly beneficial within production where unexpected, massive changes can be both expensive and painful.
- **Possible System Clashes**: Since `conceptual modeling` is used to represent such abstract entities and their relationships, it's possible to create clashes between various components.  In this case, a clash simply indicates that one component may conflict with another component, somewhere down the line.  This may be seen when design clashes with DevOps, as the initial assumptions of scaling during the design process were proven wrong when actual deployment occurred.  It's critical that such potential clashes are recognized and dealt with (or, at least, planned for) prior to production launch.
- **Scaling Challenges**: While `conceptual models` are not inherently ill-suited for large applications, it can be difficult to develop and maintain an ongoing for particularly complex projects, as the number of potential "clashes" may occur.  Such problems will only become more apparent in a production environment and as the project size increases.

## Rapid Application Development Model

`Rapid application development` (`RAD`) is a method of software development that heavily emphasizes rapid prototyping and iterative delivery.  One of the biggest advantages of `rapid application development` is how well its fundamental concepts synergize with the practice of software development as a whole.  While many forms of creation, like architecture or space flight, require meticulous planning and logical development, the essence of software is both malleable and in constant evolution.  Since code itself -- and by extension, the software which is powered by said code -- can be easily morphed throughout development, software is inherently adaptable, lending itself well to iteration and experimentation.  By utilizing a `rapid application development method`, designers and developers can aggressively utilize knowledge and discoveries gleaned during the development process itself to shape the design and or alter the software direction entirely.  

### Rapid Application Development Model Fundamentals

- **Planning Requirements**: During this initial stage designers, developers, and users come to a rough agreement on project scope and application requirements, making it possible and easy to transition to future that require prototyping.
- **User Design**: User feedback is gathered, with a heavy emphasis on determining the system architecture.  This allows initial modeling and prototypes to be created.  This step is repeated as often as necessary as the project evolves.
- **Rapid Construction**: Once basic user and system design has begun, the construction phase is where most of the actual application coding, testing, and integration takes place.  Along with the user design phase, the rapid construction phase is repeated as often as necessary, as new required components are created in order to meet the needs of the project.
- **Cutover**: This final stage allows the development team time to move components to a live production environment, where any necessary full-scale testing or team training can take place.

### Rapid Application Development Model in Production

- **Quickly Generate Productive Code**: As more and more full-stack software developers emerge, projects that use the `rapid application development` methodology will allow skilled team members to quickly produce prototypes, which can illustrate working examples that might otherwise take weeks or months to see the light of day using a slower development method.  This benefit is particularly noticeable in production, where quick iterations are often necessary to push quick bug fixes or new A-B test components.
- **Early Systems Integration**: While most `waterfall method` software projects must, by their very nature, wait until the tail end of the development life cycle to integrate other systems and services, a rapidly developed application will be integrated almost immediately.  By requiring early integrations within a prototype, a `rapid application development` project is able to quickly identify any errors or complications within said integrations, and begin the process of immediately resolving any issues.  This leads to a dramatic reduction in overall post-production integration problems.
- **Requires Frequent User Feedback**: Getting constant, frequent user insight and feedback is obviously a benefit from a design perspective, but this double-edged sword requires that the team be both willing and able to communicate with users on a much more frequent basis, in comparison to more traditional `waterfall model` methods.  This is particularly challenging while live in production, where user feedback can be both abundant and overwhelming.  Plan for this eventuality (and necessity) by establishing clear and easy-to-use communication channels with users, so the jump to production isn't as big of a shock to the system.

## Waterfall Model

The `waterfall model` ensures that a logical progression of steps are taken throughout the software development life cycle, much like a series of cascading steps moving down an incremental waterfall.  While the popularity of the waterfall model has waned over recent years in favor of more `agile` methodologies, the logical nature of the sequential process used in the waterfall method cannot be denied, and it remains a common design process in the industry.

### Waterfall Model Fundamentals

- **Requirements**: During this initial phase, the potential requirements of the application are methodically analyzed and written down in a specification document that serves as the basis for all future development.  The result should be a requirements document that defines _what_ the application should do, but not _how_ it should do it.
- **Analysis**: During this second stage, the system is analyzed in order to properly generate the models and business logic that will be used in the application.
- **Design**: This stage largely covers technical design requirements, such as programming language, data layers, services, etc.  A design specification will typically be created that outlines how exactly the business logic covered in analysis will be technically implemented.
- **Coding**: The actual source code is finally written in this fourth stage, implementing all models, business logic, and service integrations that were specified in the prior stages.
- **Testing**: During this stage, quality assurance, beta testers, and all other testers systematically discover and report issues within the application that need to be resolved.  It is not uncommon for this phase to cause a "necessary repeat" of the previous coding phase, in order for revealed bugs to be properly squashed.
- **Operations**: Finally, the application is ready for deployment to production.  The operations stage entails not just the deployment of the application, but also subsequent support and maintenance that may be required to keep it functional and up-to-date.

### Waterfall Model in Production

- **Ideal for Milestone-Focused Development**: Due to the inherent linear structure of `waterfall` projects such applications are well-suited for teams that work well under a milestone-based paradigm.  With clear, concrete, and well understood stages that everyone on the team can understand and prepare for, it is relatively simple to develop a time line for the entire process and assign particular markers and milestones for each stage, including launch to production and post-production releases.  On the other hand, the `waterfall method` tends to be create much stricter projects than more agile methodologies, making it more difficult to implement rapid changes or iterations within a production environment.
- **Minimal Customer Feedback**: Due to the strict step-by-step process that the `waterfall model` enforces, user and/or client feedback is typically provided quite late into the development life cycle.  Such insights are often too little, too late, and cannot be easily adapted into an existing project.  Thus, if your team is working within a `waterfall model`, make sure to collect user feedback throughout the development process.  Additionally, plan for explicit patch releases after production launch to implement necessary changes.
- **Delayed Testing Period**: The `waterfall model` largely shies away from testing until quite late into the software development life cycle.  This not only means that most bugs (and often design issues) won't be discovered until very late into the process, but it can also encourage somewhat lackadaisical coding practices, since testing is only an afterthought.  `Waterfall-style` projects are particularly well-suited to implementing additional testing and error management tools, such as <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-methodology-production-behavior">Airbrake's real-time, automatic error monitoring software</a>.

No matter what software methodology your team is using, you'll need a solid plan and willingness to adjust your trajectory once the launch to production occurs.  However, with <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-methodology-production-behavior">Airbrake's error monitoring software</a>, your team won't need to worry about losing track of production errors!  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-methodology-production-behavior">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

**META DESCRIPTION**

A quick look at how popular software methodologies affect production behavior, with insight into the agile model, waterfall model, and others.
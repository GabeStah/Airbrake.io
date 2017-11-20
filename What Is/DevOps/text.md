# What is DevOps?

DevOps is the practice of combining the philosophies and tools of software development (`Dev`) and software operation (`Ops`).  The term was first introduced during the 2008 `Agile Toronto` conference by developers and technologists Patrick Debois and Andrew Shafer.  Yet, as with many relatively new technological terms thrown around the industry, it can be difficult to pin down exactly what DevOps is and how its functions are commonly put into practice.

In this article we'll explore what DevOps is by looking at the generally accepted goals of a typical DevOps model, along with the most beneficial practices of implementing the model throughout your own organization.  Without further ado, let's get into it! 

## The Goals of DevOps

DevOps can largely be thought of as a [software development methodology](https://airbrake.io/blog/category/sdlc), similar to other techniques like the [`Agile model`](https://airbrake.io/blog/sdlc/agile-model).  For better or worse, there is no ultimate manifesto that explicitly defines and describes a DevOps model.  Instead, DevOps is a somewhat abstract concept that focuses on a few key principles that we'll look at below.

### Speed

Your application, development process, and release capabilities need to remain extremely fast in order to better adapt to customer needs, changes in the market, and new business goals.  DevOps attempts to keep things moving along rapidly, so you can maintain that constant pressure necessary for modern development, including rapid releases.  Practices like [`continuous delivery`](#continuous-delivery) and [`continuous integration`](#continuous-integration) make it possible to keep that speed up throughout the development and operations stages of your project.

### Reliability

[`Continuous integration`](#continuous-integration) and [`continuous delivery`](#continuous-delivery) techniques also improve the overall stability and reliability of your software and platform.  This ensures that user experiences are as optimal as possible, while allowing you and your team to rest easy, knowing your application is as robust as it can be at every stage throughout the software development life cycle.  Integrating automated testing suites and powerful <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-what-is-devops">exception handling software</a> helps your team identify problems immediately, so they are alerted of any unforeseen issues right away.

### Rapid Delivery

DevOps also aims to increase the pace and frequency of new releases, allowing you and your team to improve your software as often as possible.  Similar to the benefits brought about from and increase in overall speed, performing frequent, fast-paced releases ensures that the turnaround time on any given bug fix or new feature release is as short as possible, providing a maximum benefit to your users.  Moreover, a quick delivery turnaround makes it easier for the development and quality assurance teams to easily identify problematic issues right away, giving those teams the best chance at reducing development and testing costs.  The common practices of [`continuous integration`](#continuous-integration) and [`continuous delivery`](#continuous-delivery) are paramount for meeting the rapid delivery needs of most DevOps models.

### Scalability

DevOps also heavily focuses on creating applications and infrastructure platforms that quickly and easily scale with the ever-changing demands of your users or your business needs.  Ideally, such scaling processes should be automated and reliable, giving your software the ability to adapt on-the-fly when a sudden marketing effort goes viral or a big new feature is released.

Practices like [`infrastructure as code`](#infrastructure-as-code) are becoming evermore popular, which emphasizes the importance of provisioning and managing infrastructure components using code and other development techniques.  The goal is to allow administrators, and even developers, to quickly and easily propagate new hardware and services on a whim using programmatic, simple-to-use cloud-based APIs -- a dramatic improvement over the traditional console-based deployment and setup practices of the past.

By implementing these scaling practices, DevOps aims to improve the creation of all development, testing, staging, and production environments, and makes the process efficient and easily repeatable.

### Security

DevOps encourages strong security practices by automating compliance policies, simplifying configuration processes, and introducing detailed security controls.  [`Infrastructure as code`](#infrastructure-as-code) practices improve security by largely removing the human element when provisioning new hardware and services.  Since additional scaling should be largely automated and handled by code APIs within the cloud, creating new infrastructure that matches existing configurations is a breeze and far less error-prone.

Beyond that, the practice of [`policy as code`](#policy-as-code) is a similar concept in which security policies and compliance can be configured, executed, and monitored via cloud-based APIs.  This programmatic and automatic nature ensures that any resources that fall out of compliance are immediately noticed and can be evaluated by the team, in order to get them back into compliance as quickly as possible.

### Collaboration

Similar to other Agile-based software development methodologies like the [`conceptual model`](https://airbrake.io/blog/sdlc/conceptual-model), the DevOps model strongly encourages collaboration throughout the entire development life cycle.  Every team member should be encouraged to take ownership and be accountable for aspects of the project they are involved in.  This leads to more effective teams and has been shown to increase productivity, reduce mistakes, and even improve job satisfaction.  Both system administrators and developers should collaborate throughout the project by sharing responsibilities and workflows.

As it happens, collaboration also extends beyond just the organization and the team within that is working on the project.  Improved collaboration should also take place with users, customers, clients, and other third-parties.  A DevOps model that encourages your team to work closely with users throughout the development life cycle will produce a more polished final product, while dramatically reducing time and money that might be wasted by re-routing production features based on user feedback.

## Implementing a DevOps Model

Understanding what makes a DevOps model is all well and good, but actually implementing such a model can be difficult, particularly for organizations that are accustomed to more traditional development models.  However, making the effort to get into the DevOps mindset will provide massive, immediate benefits and put your team on the path to a streamlined software development life cycle.

### A Cultural Shift

The first hurdle for most organizations is getting rid of the engrained philosophical belief that the development and operations teams are -- and must forever remain -- two completely independent teams that never have need to work together.  As the DevOps name implies, these teams need to work in tandem to really see the benefits that such a method provides.  By promoting a cultural shift throughout the organization that encourages operations and development team members to collaborate, you'll see an overall improvement in development productivity and operational reliability.

As part of the focus on collaboration, both development and operations team members are encouraged to take ownership of and accountability for their work.  Members are encouraged to look beyond their "traditional" roles and scopes of responsibility to consider how their contributions can improve the product for the end user.

This cultural shift need not only apply to development and operations teams, either.  If your organization traditionally quarantines separate quality assurance teams as well, they should also be integrated into the melting pot of developers and operation team members, in order to add their expertise and feedback into the mix.

### Common Practices

While a DevOps model doesn't follow a strict rule book to indicate what should (and should not) be implemented, there are a handful of common practices and techniques that most DevOps methods utilize.  Overall, when considering whether any one of these concepts is right for your organization or project, keep the [overall goals of DevOps](#the-goals-of-devops) in mind.  If only a few of these practices fit the needs of your team or project, ignoring the others is perfectly acceptable and you should still see the many benefits a DevOps model can provide.

#### Small and Frequent Updates

One particularly powerful benefit to integrating a DevOps model is the ability to easily push minute software updates at a rather frequent pace.  Doing so will dramatically reduce the potential risk from any given deployment, since rolling back to a previous update will be just as small of a relative change.  Moreover, rapid, minor updates make it much easier for your team to identify problematic areas and newly-introduced bugs, since <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-what-is-devops">error monitoring tools</a> can automatically distinguish one release build from the next.

#### Continuous Integration

`Continuous integration` is a development practice that encourages developers to frequently merge code into a central repository that is shared across the entire team.  Automated testing is performed on this singular central repository, which ensures that bugs are identified and resolved as quickly as possible, improving the overall quality of the software throughout the entire development life cycle.  Since a central repository is used as the basis for all future patches, releases, and modifications, it's relatively easy to validate new releases as quickly as possible.

#### Continuous Delivery

Whereas `continuous integration` focuses on the "initial" stages of code development, `continuous delivery` is the practice of automatically building, testing, and preparing code for production release.  `Continuous delivery` often works in tandem with `continuous integration` by deploying the code found in the central repository to a testing, staging, and/or production environment once everything is built and verified.  As the name suggests, the purpose of this practice is to ensure that there is a constant production-ready build that has been fully tested and confirmed to be a viable release candidate.

#### Microservices

A `microservice` is a small service that can run on its own, independent of other services.  A collection of `microservices` are then typically combined to create a larger software application by communicating with one another over simple interfaces like web-based APIs.  By creating individual `microservices` that are purpose-built for a discrete purpose, your team can use the most ideal programming languages, frameworks, and tools to create and distribute each service.  Moreover, when a larger application consists of a series of smaller services, it's much easier to identify the root cause of bugs or issues that may arise, since the failure of a particular `microservice` should not impact the stability of other services.

#### Infrastructure as Code

`Infrastructure as code` is a practice that emphasizes the importance of provisioning and managing infrastructure components using code and other development techniques.  The goal is to allow administrators and even developers to quickly and easily propagate new hardware and services using programmatic, simple-to-use cloud-based APIs.  Interacting with infrastructure through programmatic code allows standardized patterns to be used for updating hardware, patching software, and even duplicating entire server arrays.

#### Policy as Code

`Policy as code` is the practice of creating and managing security policies and compliance via cloud-based APIs.  Such policies can be configured, executed, and monitored programmatically, ensuring that any resources that fall out of compliance are immediately noticed and can be evaluated by the team to get them working again as quickly as possible.

#### Logging and Error Monitoring

As most modern applications become 24/7 services, and because `continuous delivery` and `continuous integration` practices encourage rapid releases, it's critical that your organization logs as much application data and as many metrics as possible.  Logs provide insight into the performance of individual releases, as well as the trajectory and common behaviors of your end users.  Alerts and real-time monitoring are vital components for creating and maintaining a healthy application that takes advantage of the best DevOps model practices.  In fact, <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-what-is-devops">Airbrake's error monitoring software</a> guarantees that your team won't need to worry about losing track of production errors!  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-what-is-devops">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief overview of exactly what DevOps is, including its goals, principles, and practices needed to add a DevOps model into your own organization.

---

__SOURCES__

- https://aws.amazon.com/devops/what-is-devops/
- https://medium.com/devopslinks/the-15-point-devops-check-list-8cd2afb4a448
- https://en.wikipedia.org/wiki/DevOps
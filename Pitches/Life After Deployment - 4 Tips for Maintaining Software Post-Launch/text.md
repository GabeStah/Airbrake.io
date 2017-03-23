# Life After Deployment: 4 Tips for Maintaining Software Post-Launch

Finally deploying your application, after months (or sometimes years) of development and toil, is an exciting time.  However, there are numerous additional responsibilities and considerations, which are necessary in order to properly monitor and support your application once it has been pushed live and is in the hands of your users.  An application that is deployed and then forgotten is an application that is destined to fail, sometimes catastrophically so.

In this article we'll explore a few crucial tips to help you and your team plan for, and eventually deal with, the post-launch period in the life cycle of your shiny new application, so let's get started!

## Devise Proper Software Architecture

`Software architecture` is a critical component that will have a dramatic impact on your (in)ability to properly support your production application.  Software architecture is a rather broad term, which loosely describes the structure of the entire `system` of an application.  That `system` is made up of numerous `components`, all working in tandem to provide the overall functional and technical requirements of the application.

For example, a modern web application's software architecture might be made up of a variety of `components`, including: `business logic`, `database layers`, `integrated services`, `interface`, `monitoring`, and so forth.  Each of these `components` may consist of a variety of sub-components, which may include numerous physical servers, configurations, and actual source code.

All together, software architecture should fully define and encapsulate every aspect of your application's functional and technical needs, such that all `components` can work in harmony once the time for deployment arrives.  After launch -- once your application is in production -- the software architecture should allow your team to easily monitor the system, as well as make any necessary updates, without negatively impacting the now-established userbase.

While there are far too many potential architectural styles to cover them all here, we'll instead list a few common styles for consideration:

- **Client/Server**: A common style used in many web applications, in which two systems (the `client` and the `server`) communicate information back and forth.
- **Component-Based Architecture**: Facebook's [`React.js`](https://facebook.github.io/react/) library is a popular example of `component-based architecture`, which aims to divide application functionality into individual, self-contained components, which can communicate with one another.
- **Model-View-Controller Architecture**: A three-tiered method which splits application functionality into three parts: The `model` handles data and business logic, the `view` handles output of information (UI), and the `controller` takes input of some kind and sends appropriate commands to the `model` and `view`.
- **Domain-Driven Design**: Uses object-oriented techniques to define objects that handle business logic, based on the business domain.
- **Message Bus Architecture**: Allows for sending/receiving of encapsulated messages that inform the application's business logic.
- **Service-Oriented Architecture**: Often in the form of APIs, this style of architecture allows the application to send/receive various requests, as a service.

The key takeaway is that one single architectural style is not (nor should not) makeup the entirety of your application's software architecture.  Styles are meant to be combined in the manner that best suits the needs of your application.

## Monitor Application Health

After deployment, there's a bevy of components and systems that need to be carefully monitored, to properly ensure that your application is running efficiently and effectively.  Obviously, these are just generalized suggestions, and the specific needs will differ from one application to the next.  However, if a particular example component is applicable to your application, chances are it's worth monitoring.

#### Monitor Errors

No matter how thorough your testing procedures and quality assurance practices may have been, it's near-impossible for a recently deployed application not to experience unexpected issues or throw a series of new errors your way.  The best way to keep these under wraps is two-fold.

First, consider implementing an error monitoring service such as [`Airbrake.io`](https://airbrake.io/), which is an easy and inexpensive way to be automatically alerted to any errors or unexpected issues that may occur inside your recently-deployed application.  `Airbrake.io` can be easily integrated into your application with just a few lines of code, which will automatically alert your team of any errors the moment they occur, without the need for user feedback or reporting.

Second, employ an efficient system of handling errors, from altering their status to making necessary fixes to pushing changes back to production.  Error monitoring software can help with error status management, but it's up to your team to implement a method for quickly patching fixes and deploying those changes.

#### Monitor Load Times

The perception of your application's performance is critical, particularly after a big deployment where users are actively seeking you out and have expectations.  Nothing quenches that fiery excitement quite like a page that just won't load.  Ideally, your `server architecture` should be built in such a way that the system can grow automatically and immediately, without the need for intervention from the IT department.

#### Monitor Server Utilization

Ensure that all servers are online, functioning as expected, and not being overly strained by traffic.  Be sure to check on the network, disk, memory, and CPU usage, to see that they fall within the expected ranges.

#### Monitor Logs

Not all issues raise an error; many are far more subtle, yet could be just as damaging if left unchecked.  One great tool to keep track of less-dramatic issues with your application is to keep a close eye on the `logs` throughout your system.

#### Monitor Key Performance Indicators (KPIs)

`Key Performance Indicators` (`KPIs`) are measurable values that quantitatively indicate how your application is performing relative to particular business objectives.  For example, common `KPIs` might include things like `rate of new users`, `revenue/sales figures`, `advertisement click-through rates`, `support requests`, or `social media mentions`.

No matter which metrics you decide are important to your application and business, it's a great idea to keep tabs on them all, so your team can quickly evaluate what is working and what could use with some improvement.

## Begin the Support, Maintenance and Improvement Phase

Feeling that relief of a successful deployment and launch to production is wonderful, but for many modern applications, this is just the beginning of a new chapter in the development life cycle of your application.  In many ways, the old phases of development leading up to release are gone, but now you're entering the `support, maintenance, and improvement phase`.

### Support

We've all had to contact a `support` department at one time or another to get assistance with a problem, whether it was a late shipment, an overcharge on a bill, or to praise a particular employee (hah!).  Just as with all those other organizations, your application and organization will need an active form of support.  The exact implementation of support is largely based on what your software demands, but whether it's email, telephone, live chat, or some form of personal house call, your customers will need a way to efficiently receive support and have their questions answered, no matter how silly those questions may seem.

You'll also likely want to establish the level of support provided, as it pertains to the various versions or releases of your application.

### Maintenance

No modern software application exists in a bubble; it is built upon, and therefore relies on, a wide range of other software, components, and services, all of which may frequently change over time.  Your application will likely need to handle upgrades to other components it is integrated with at some point during the post-deployment life cycle.  This might be changes to hardware, operating system, database versions, language versions, framework versions, and so forth.

In many cases, it is preferable not to change components that you don't have to, which is why most developers will initially design using a specific version of a component or library.  Down the road, even if new versions of that component are released, your application can remain mostly the same, without updating to all the latest versions.

Even still, some change will be inevitably be required.  This is particularly true for security patches and updates.  When a security flaw is discovered within a component your application, you absolutely must update it, so your team should be prepared for and capable of making those changes, then adapting to any consequences this may incur on the application as a whole.

### Improvement

Beyond support and required maintenance, you may also wish to implement new features or changes to your application after it has already been deployed.  In many ways, these should be handled in much the same way as pre-deployment development, with all the same practices in place.  Once a new release is ready and is fully tested on a staging environment, it can be deployed to production (either directly overwriting your previous version, or using some form of `A/B testing`, if necessary).

## Plan Your Application's End-of-Life

While arguably a somewhat somber topic to consider just after deployment, it's important to plan for the eventual end of your application.  Known as `end-of-life` (or sometimes `end-of-service`, depending on the type of software in question), this is the moment in time when your application has reached the end of its useful life for users.

Determining when end-of-life should occur is specific to each project, but establishing this moment earlier rather than later is a good idea, as it sets a clear line in the sand for both your team and your customers.  End-of-life may be when a new major release is launched, or years down the line, when aspects your application are expected to be obsolete.

---

__META DESCRIPTION__

Four crucial tips to help you plan for and handle the maintenance of your software toward the end of the development life cycle.

---

__SOURCES__

- https://developer.android.com/distribute/marketing-tools/launch-checklist.html#support-users
- https://stackify.com/ultimate-checklist-app-deployment-success/
- https://en.wikipedia.org/wiki/Product_lifecycle
- https://en.wikipedia.org/wiki/Software_maintenance
- https://msdn.microsoft.com/en-us/library/ee658093.aspx
- https://en.wikipedia.org/wiki/End-of-life_(product)
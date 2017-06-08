# Production Defects Are Not Inevitable

Let's be upfront about this right out of the gate: Defects are an inevitability at _some point_ during the development life cycle, particularly for larger and more complicated projects.  There's no benefit to be gained by fooling ourselves into the unhealthy belief that _all_ bugs and defects can be avoided.  This attitude is dangerous, not only for technical leadership in the company but for customers and even the bottom line of the business as a whole.  However, even though some defects are a given throughout the course of development, defects in production can often be avoided with intelligent planning, coding practices, and -- most importantly -- thorough testing.

Throughout this article we'll explore why, even after decades of advancement in programming and development, creating modern zero-bug applications is still so challenging.  We'll then examine a handful of tried-and-true techniques that can help contain all defects within the confines of safe environments like development and staging, while largely eliminating defects from your full production releases.

## The Standard Is: There Is No Standard

As many technically-minded individuals like myself can attest, we often like to try simplifying abstract or challenging concepts by quantifying them in some way -- if it can be measured, counted, or computed it becomes easier to work with.  Thus, when it comes to creating powerful, robust, and (hopefully) defect-free applications it helps to understand what level others in the industry are at.  This information can help us answer some important questions: How many defects are acceptable in our application?  How much should severity level factor in?  What is the industry average for bugs?

That last question is difficult to answer due to the sheer volume of projects that are being produced across the industry, not to mention the dramatically different goals and scopes of each project.  One resource that can help with an answer is the renowned book [_Code Complete_](https://books.google.com/books?id=LpVCAwAAQBAJ) by the highly-respected software engineer [Steve McConnell](https://en.wikipedia.org/wiki/Steve_McConnell).  While his work is somewhat dated at present, the trends in defect rates seem to be hold steady over time, even over the course of decades of advancement -- as tools and written code get better, the defects we produce just become more complicated.

According to McConnell's research, the industry average defect rate is around 1-25 bugs for every 1,000 lines of code.  Yet this isn't to suggest that all applications or all organizations will average out to creating a bug every 80 lines of code.  In many cases defect rates are directly proportionate to the potential cost or danger of producing said defects.

In his stellar article [_They Write the Right Stuff_](https://www.fastcompany.com/28121/they-write-right-stuff), author and business journalist Charles Fishman concludes that much of the success of NASA's now-retired Space Shuttle program came from the excellence of the software that largely controlled the spacecraft.  The Space Shuttle's software was as near to bug-free as humans have ever produced.  Fishman lays the stats out in all their glory: "The last three versions of the program -- each 420,000 lines long -- had just one error each.  The last 11 versions of this software had a total of 17 errors.  Commercial programs of equivalent complexity would have 5,000 errors."

Granted, the vast majority of software in development these days isn't aimed at controlling multi-billion dollar, space-faring, meticulously-engineered feats of wonder, but there's certainly a happy middle ground to be had.  If too many defects will see your proverbial ship crash and burn, yet zero defects is merely a flight of fancy, what is the reasonable compromise that can produce quality software with as few bugs as possible?

## Development and Staging: Where the Wild Things Are

Development (and staging) is the playground where anything and everything can go wrong and yet you can always recover.  Perhaps the most critical notion necessary for creating robust applications is to make every effort to contain all the defects to the development environment.  No matter how hard we try or how diligent our development teams may be there will always be bugs popping up from time to time.  Keeping these wild things contained to the confines and the safety of the development or even staging environments means defects are recognized and resolved before a single end-user ever experiences them.

A multitude of techniques and tools exist to help detect and resolve bugs during development, but a handful of the tried-and-true practices are:

- [Test-driven development](https://airbrake.io/blog/sdlc/test-driven-development) (`TDD`) aims to create tests _before_ software is written, ideally to ensure all written code is aimed at meeting business requirements.
- The [Agile model](https://airbrake.io/blog/sdlc/agile-model) encourages iterative development and high adaptability to changing requirements.
- Refactor early and often to improve code stability and reduce the potential for defects while keeping the external behavior the same.
- Code reviews and pair programming are simple techniques to get more than one person looking at code before it is committed to the repository.
- Regression testing should also be used to confirm that recent changes haven't broken previously functional code.
- Automated exception tracking and reporting through tools like <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-bugs">Airbrake</a> ensures your team is immediately aware of exceptions the moment they occur.

While many other best practices and techniques exist to help your team recognize and respond to defects during development and staging, the most important factor is the development of a basic set of standards and requirements for use throughout the project.  Obviously these requirements can (and will) change over the development life cycle, but having a single source of information that everyone on the team can look to makes it much more likely that many potential defects will be avoided altogether.

## Production: We Are Go for Launch

Once you're ready for production it's time to launch into the orbit of customers who can (and will) invariably find ways to potentially break your application.  Thus it's critical that all the planning that was put into place and the best practices that were executed upon during development were sound and ideally followed to a T.  Thankfully, with the proper development methodologies, solid testing techniques, and powerful exception reporting it's quite feasible to enter into production with very few if any defects.

That said, even the best laid plans of mice and men often go awry.  While production defects are far from inevitable it's critical to have a safety net in the unlikely occurrence that something unexpected happens in your application after it's already out there.  This is where the power of error monitoring software comes into play.  Even during development, but particularly after production release, error monitoring software provides that life line your organization needs to ensure your software remains fully functional.  Any unforeseen defects are immediately identified and reported to your team, without the need for user-generated feedback or awkward error reports.  Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-bugs">Airbrake's</a> exception handling tools today to see how your organization can keep on top of any defects that slipped through the cracks during production.

---

__META DESCRIPTION__

Exploration of why production defects are not inevitable, and can be largely eliminated with planning, requirements, and exception monitoring tools.

---

__SOURCES__

- https://books.google.com/books?id=LpVCAwAAQBAJ
- https://spin.atomicobject.com/2015/03/17/inevitability-of-bugs/
- https://www.fastcompany.com/28121/they-write-right-stuff
- https://m.signalvnoise.com/software-has-bugs-this-is-normal-f64761a262ca
# Who Discovers Your Production Errors? Don't Tell Me It's Your Users

After months (or even years) of blood, sweat, and tears, you and your team have finally launched your latest application into production and are basking in the glow of a job well done.  And yet, there's that tiny itch in the back of your minds, nervously waiting and wondering if the other shoe will drop.  Nothing seems terribly wrong for some time, as hours turn to days, which eventually turn to weeks without any major issues.  Sure, production errors crop up from time to time, but nothing too impactful.  

The real question is, how are these production errors being discovered?  Are you and your team finding them due to diligent software testing practices?  Or, are your _users_ running into problems and either reporting them to you directly, or worse, assuming your application is faulty and no longer using it at all?  Production errors are the bane of many otherwise well-designed applications.  After all that effort to get a solid project built and launched, it can all be brought down (or, at the least, dramatically degraded) by only a handful of unexpected and impactful production errors.

Things don't have to be this way.  By practicing just a bit of diligence in how you go about software testing, your team can be confident that you'll have a smooth production launch with few, if any, error reports coming in from users that you aren't already aware of and actively fixing.

Making every effort to avoid user-reported productions errors can be challenging, but doing so is critical for building up and maintaining the reputation of your team, your business, and your product.  In the rest of this article we'll explore a few tips and techniques, which are used by various business leaders around the world, that will help you and your team reduce your production error rates, while completely eliminating the need for user error reports.

## Establish Robust Automated Unit Tests

Every software testing suite should include some form of automated unit tests.  While functional tests are focused entirely on the _what_ of your application, unit tests are concerned with the _how_, allowing them to verify that each line of source code and each component in the overall application behaves exactly as expected.

Since unit tests tend to be among the most "low-level" types of tests your team will implement in your overall software testing practices, these should usually be created by developers or other team members closely acquainted with the exact inner-workings of the application code.  However, developer man hours are vitally important, so it is paramount that unit tests be **automated**, allowing developers to get back to the important work of improving the application code, rather than focusing an abundance of their work hours on executing and monitoring unit tests.

## Create Detailed Automated Functional Tests

As discussed, functional tests are intended to verify that your software is doing _what_ it should be.  Functional tests _do not_ concern themselves with _how_ the application goes about completing a particular goal.  Thus, functional testing is often used to confirm application pages, user interfaces, and other end-to-end components are behaving as expected, without explicitly taking a look at the source code under the hood that powers said functionality.

Automated functional tests extend the normal benefits of functional tests by allowing your team to consistently and automatically execute tests and compare results without human intervention.  Non-developers are particularly well-suited to creating and managing automated functional tests, since they can often be generated with an easy user interface or script, then executed on a consistent basis, such as following every build or release.

## Design and Develop Reliable Fail-Safes 

Another critical component to a robust software testing suite is thinking, planning, and developing outside the bounds of a typical testing practices.  Planning for unexpected behaviors, on both the part of the application as well as your end users, will lead to a much healthier application and dramatically reduce the number of errors once production finally comes around.  Build in intentional fail-safes into your application, along with your software testing suite, to ensure that these rare but possible events don't lead to dramatic problems down the road.

For example, it should be assumed that users will misuse your application, whether they do so intentionally or not.  A user is likely to click on a series of UI components out of the "expected order."  If your application has an order confirmation button element, at the very least your application should have a built-in fail-safe that prevents the user from clicking the order button multiple times, or ignores subsequent clicks after the first.  Whether you choose to inform the user of their improper action is more about business practices and beyond the realm of software testing, but it's critical that any and all such unintended behaviors be assumed and accounted for.

Similarly, even attempting to plan for every eventuality will leave gaps here and there in the application code -- something will occur that wasn't expected nor planned for.  In such cases, it's often smart to ensure your application code catches and handles all unexpected exceptions.  These should be logged, reported, or monitored via error handling software, but no matter how you choose to deal with them, better that your application code handles things then for your user to experience an ugly or even application-breaking error.

## Integrate Error Monitoring Software

Error monitoring software provides a constant pulse on the health and reliability of your application, whether its running in development or even out in the wilds of production.  All errors are quickly identified, examined, and are ready to be fixed, without the need for convoluted user-generated reports or expensive, time-consuming quality assurance support.

The best error monitoring services allow you and your team to see the exact nature of every error, including a plethora of detailed metadata.  For example, when an error occurs using <a class="js-cta-utm" href="https://airbrake.io/?utm_source=sitepoint&amp;utm_medium=end-post&amp;utm_campaign=airbrake-software-testing">Airbrake.io's error monitoring software</a>, Airbrake is able to report the error type, error message, parameters, the API method that generated the error, the detailed backtrace, the client machine data, the environment in which the error occurred, the file that caused the error, and much more.

Thus, not only does error monitoring software allow you to track and immediately identify exceptions when they occur, it also provides a substantial safety net, particularly during production releases.  While you'll still want to plan accordingly and establish a sound software testing suite, error monitoring services provide a bit of breathing room by promising to inform you of any unforeseen issues.

Check out <a class="js-cta-utm" href="https://airbrake.io/?utm_source=sitepoint&amp;utm_medium=end-post&amp;utm_campaign=airbrake-software-testing">Airbrake.io's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams are using `Airbrake` to revolutionize their exception handling practices!

__META DESCRIPTION__

A handful of tips and tricks for creating a more robust software testing suite, to better plan for and handle unexpected productions errors.

**Keyword:** software testing

---

__SOURCES__

- 
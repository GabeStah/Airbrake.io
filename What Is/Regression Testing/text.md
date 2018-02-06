---
categories: ["What Is", Testing]
date: 2018-02-06
published: true
title: "Regression Testing: What It Is and How to Use It"
---

**Regression testing** is a form of software testing that confirms or denies a software's functionality after the software undergoes changes.  The term `regression` actually means "the act of reverting back to a previous state."  By extrapolating that definition into the realm of software development we can infer that `regression testing` is performed to verify that software has not unintentionally regressed to a previous state.  `Regression testing` is ideally performed every time a software component or feature is modified, to help identify (and resolve) any newly discovered or regressed issues.

Throughout this article we'll further examine what `regression testing` is and provide a handful of useful tips for how to properly implement `regression testing` into your team's software development life cycles.  Let's get to it!

## What is Regression Testing?

`Regression testing` simply confirms that modified software hasn't unintentionally changed and it is typically performed using any combination of the following techniques:

- `Retest All`: A `retest all` practice, as the name implies, aims to re-test the entire software suite.  In most cases, the majority of testing is actually automated using assorted tools and [test-driven development practices](https://airbrake.io/blog/sdlc/test-driven-development), since it's neither feasible nor economical for humans to perform such a massive quantity of testing.  However, this lack of human intervention can also be problematic, so it's critical to have a backup plan like an <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-regression-testing">error monitoring tool</a>, which will ensure anything that slips through the cracks is caught and dealt with.
- `Regression Test Selection`: As a slightly toned-down alternative to `retest all`, `regression test selection` encourages the team to extract a `representative selection` of tests from the full test suite that will _approximate_ the average test case of the entire testing suite as a whole.  The primary advantage to this practice is that it requires far less time and effort to perform.  This kind of `regression testing` is ideally handled by human testers -- such as those from the QA or development teams -- who will typically have better insight into the nuances of test edge-cases and unexpected behaviors.
- `Test Case Prioritization`: The goal here is to prioritize a limited set of test cases such that the more potentially impactful tests are executed ahead of all less critical tests.  The act of _how_ your team prioritizes test cases is outside the scope of this article, but many of the techniques used during [`defect triage`](https://airbrake.io/blog/devops/easing-the-pain-of-defect-triage) are applicable during `test case prioritization`.

## Why is Regression Testing Important?

In the world of software quality assurance there exists a term known as `software regression`, which simply refers to a software bug which causes some kind of unintended non-functionality when a change is made to the system, such as a patch or new release.  There are three common categories of `software regression`:

- `Local`: When a new bug is located within the same software component that was updated.
- `Remote`: When a new bug is located within a _different_ software component than the one that was updated.
- `Unmasked`: When the bug already existed, but it had no effect prior to the software component update.

The overall purpose of `regression testing` is to easily and effectively uncover all possible `software regressions`, whether they were newly created (`local` / `remote`) or previously undiscovered (`unmasked`).

## How is Regression Testing Performed?

No matter which techniques your team opts to use to handle `regression testing` there are a few important best practices you should consider implementing:

- **Maintain a Strict Testing Schedule**: Always maintain a continual testing schedule throughout the entire software development life cycle.  Not only will this quickly force the team to adapt to a frequent testing regimen, it will also ensure the finished product is as well-tested as possible.
- **Use Test Management Software**: Unless your current software project is a simple self-developed side project, chances are you'll have such an abundance of tests that tracking each will be well beyond the capabilities of a single individual or a spreadsheet.  Thankfully, there are many different test management tools on the market designed to simplify the process of creating, managing, tracking, and reporting on all the tests in your entire testing suite.
- **Categorize Your Tests**: Imagine a test suite of hundreds or thousands of tests that are merely identified by a single `name` or `id` field.  How on Earth would anyone ever sort through that massive list in order to identify tests that are related?  The solution is to categorize tests into smaller groups based on whatever criteria is appropriate for your team.  Most test management tools will provide the means of categorizing or tagging tests, which will make it easier for everyone on the team to identify and reference a certain _type_ of test.
- **Prioritize Tests Based on Customer Needs**: One useful way to prioritize tests is to consider the needs of the customer or user.  Consider how a given test case impacts the end user's experience or the customer's business requirements.

## Who Should Perform Regression Testing?

Ideally, your tests should be robust and simple enough to execute that _anyone_ on the development team is able to run the full `regression test` suite by issuing a single command.  Even better, using the proper [continuous deployment tools](https://airbrake.io/blog/devops/9-powerful-deployment-tools) will allow the full suite of `regression tests` to be performed automatically, per the testing schedule you and your team have devised.

## Where Are Regression Tests Appropriate?

The short and sweet answer for where a `regression test` should be implemented is "everywhere you can afford it!"  However, in the real world, designing and maintaining a near-infinite set of `regression tests` is just not feasible, so it is important to determine where in the software a new test should be generated and how it should be designed.

To illustrate the challenge of deciding when and where a test should be added, consider a simple application that contains just two text boxes, each of which can hold a number up to `20` characters in length.  Upon submission both numbers are added together and their sum is output to the user.  It doesn't get much simpler than this, but let's now consider how we'd go about testing for what seems like a simple scenario: "Are there any numbers that can be entered into either box that produce an error?"  We'll assume there are already restraints and sanity checks in place such that only valid numbers from zero through 10<sup>20</sup>-1 can be entered.

We could write a test that just starts iterating through numbers and checking the result without too much trouble.  The problem here is that computers are fast, but [they just aren't _that_ fast](https://computers-are-fast.github.io/).  Even if we could blaze through `~550,000,000` numbers every second, it would still take [thousands of years](https://www.wolframalpha.com/input/?i=(10+%5E+20+-+1)+%2F+(5.5+%C3%97+10%5E8)+seconds) to run through every possible valid number -- and this doesn't even account for the _combination_ of adding numbers together.

Now obviously, we don't go to these extremes for most applications because it's generally assumed that if a function can successfully add two numbers together it can probably do the same with a _different_ pair of numbers.  Therefore, the obvious solution here (which can be applied to `regression testing` in general) is to create a `representative sample` of tests that will approximate the larger collection of _possible_ or theoretical values.  In the case of our two numeric text boxes, we'd probably test extreme values (`-2,147,483,648`, `-2,147,483,647`, `0`, `2,147,483,647`, `2,147,483,648`), special values (`Infinity`, `NaN`, `null`, etc), and then maybe a handful of randomized values to ensure stability between test executions.  In 99.99% of scenarios, we can safely assume that this level of test coverage will suffice, and there's no need to test all 99 quintillion values we could theoretically input.

## When Should Regression Testing Occur?

`Regression testing` should be performed after any change is made to the code base.  Additionally, `regression tests` should also be executed anytime a previously discovered issue has been marked as `fixed` and must be verified.

Your team will need to decide the `regression testing` schedule that best meets your needs, but most organizations find it useful to perform `regression testing` on a strict schedule.  This may be at the end of every work day, weekly, bi-weekly, or even after every single repository commit is pushed.  The more often your team is able to perform `regression testing`, the more issues can be discovered and fixed, which will lead to a more stable and functional piece of software at the end of development and leading into production.

That said, it's important not to rely _exclusively_ on your `regression testing` practices to catch all errors or potential bugs.  No matter how diligent you and your team may be, some defects will eventually slip through the cracks, so make sure you implement an extra layer of security beyond testing such as <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-regression-testing">Airbrake's error monitoring software</a>.  Airbrake guarantees that your team won't need to worry about losing track of production errors, because Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-regression-testing">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at what regression testing is, including tips for where, when, why, and how it should be used in your own development projects.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Regression_testing
- https://smartbear.com/learn/automated-testing/what-is-regression-testing/
- http://www.guru99.com/regression-testing.html
- https://test.io/software-testing-guide/what-is-regression-testing/
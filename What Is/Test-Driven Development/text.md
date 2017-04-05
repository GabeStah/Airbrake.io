# Test-Driven Development - What is it and how do you use it?

In the world of software development, `test-driven development` (commonly shortened to `tdd`) is an well-known and commonly-used development methodology by which (failing) tests are initially created, and _only then_ is the actual software code created, which aims pass the newly-generated tests.

Today we'll take some time to explore the fundamental components of `test-driven development`, including the basic `test-driven development` life cycle, some best practices, and potential advantages and disadvantages to implemented `test-driven development` in your own projects.  Let's get to it!

## What is Test-Driven Development?

The core of the `test-driven development` cycle revolves around five simple steps, which are repeated ad nauseam throughout the software development life cycle.  The goal of these steps (and the whole of `test-driven development` in general) is to ensure that code is simple and efficient, while fulfilling all functional business requirements.

#### 1. Write a Test

Since development is _driven_ by tests, the obvious first step is to create a new test.  This test should be as succinct and as simple as possible, testing a very specific aspect or component of a larger feature.  For example, if you're creating a `user registration` feature, you _could_ create a single test that encompasses every aspect of registration, from the generation of the form elements, to the user entry, to the database connection, to the data models, and so forth.  However, it would be quite difficult to properly create a single test that encompasses all those aspects, let alone one that does so efficiently and effectively from the outset.

Instead, `test-driven development` encourages you to write the smallest possible test that is necessary to meet the needs of the actively developed feature.  Over time, many tests are created, until enough tests exist to cover every aspect of the much larger feature.

Thus, a single test that relates to the `user registration` feature might be something as simple as: "`email` field is not blank."  Most tests created in `test-driven development` use some sort of language-specific framework that allows tests to be "titled" or "named", typically be writing a simple phrase that defines the behavior (commonly referred to as a `user story`).  Thus, our first test might be titled "`email` field is not blank", and its purpose is simply to test that the `email` field is not empty when the form is submitted.

#### 2. Confirm the Test Fails

Once the test is created, the next step is to confirm that the test fails.  The entire purpose of the `test-driven development` methodology is to force you to think about the _requirements_ of a feature or a section of code, such that a created test will not only be necessary in order to confirm when the feature is finally working as expected, but also that the test will fail prior to implementing said feature.  By confirming that the new test fails -- and does so for the reason(s) you expect -- you can be confident that the test is useful, and will be beneficial once you write the code necessary to pass the test.

#### 3. Write Code to Pass Test

After confirming that the test fails, you now must write the code that will allow the test to pass.  One key idea here is that you _should not_ write any additional or extraneous code that goes beyond the scope of the test.  Just as we focused on creating the simplest test possible in `Step 1`, here we want to write the simplest code possible that allows our test to pass.

Code written here will likely be rough and not finalized, but that's OK.  The entire `test-driven development` process encourages constant refactoring, meaning your current code is likely to change numerous times in the future.

#### 4. Confirm the Test Passes

Once your new code is written, confirm that the test now passes.  In our example case, we've confirmed that submitting our registration form with a blank `email` field causes our test to fail, while entering text in the `email` field allows the test to pass.  Believe it or not, that's all there is to the the basic `test-driven development` process.

#### 5. Refactor

During the fifth step, even though you now have a test that passes, the process of writing the code necessary to allow said test to pass may have introduced some duplications or inconsistencies.  That's perfectly normal and expected, but the importance of the `refactoring` step is to take the time to locate those problem areas and to focus on simplifying the codebase.

This process should also include frequent re-running of all previous tests, to confirm that you haven't accidentally introduced any additional bugs, or changed something that now causes a previously passing test to fail.  Most developers will know this practice as `regression testing` -- confirming that functional code doesn't break due to new changes.

#### 6. Repeat All Steps

Arguably, there is a sixth step in this five-step `test-driven development` process: `repeat`.  If everything is kept small (small use cases, small tests, small code changes, small confirmations, etc), then the entire process, from writing a failing test to confirming a passing test and refactoring, can often take only a few minutes.  Thus, the process is repeated over and over, with each new test slowly incrementing the entire codebase, progressing closer and closer to a fully-realized and completed feature.

`Red-green-refactor` is a simplified, shorthand version of `test-driven development` that you may hear from time to time, and it's just another way of referring to the basic steps outlined above.  A `red test` is a failing test, while a `green test` is a passing test, so the process of `red-green-refactor` just means to create a failing test, make it pass, and then refactor.

## Tool-Assisted Test-Driven Development

For most modern software development projects, `test-driven development` is largely based around the use of other tools that dramatically assist with the process.  The most beneficial of these tools is easily the `automated test runner` or `automated task runner`.  There are too many such tools to list here, but depending on the language or framework you're using, there's bound to be multiple choices available to you.

The obvious benefit to an `automated test runner` is the ability to have all your tests executed automatically without your direct intervention.  For developer-centric testing, such as most unit testing that is the core of `test-driven development` practices, not having to worry about manually executing tests yourself is a lifesaver.  Many `automated test runners` can even execute tests when changes are made to a desired set of files.  This way, your tests can always be running in the background, and if a test fails due to a recent change in the code, you can be alerted immediately, while otherwise focusing entirely on the code you're working on.

## Advantages of Test-Driven Development

- **Reduces the Reliance on Debugging**: Since `test-driven development` focuses on writing tests first, and only _then_ creating code intended to pass said tests, many developers find that a `test-driven development` life cycle can dramatically reduce the need for debugging.  Since `test-driven development` encourages a much deeper understanding of logic and functional requirements during test writing and coding, the cause of a failing test can often be quickly recognized and remedied.
- **Considers the User Experience**: The process of initially thinking about and writing a test fundamentally forces your brain to work backwards: You first consider how the function will be used, then how it might be implemented, and then how a test to encompass that should be written.  This encourages you to consider the user experience aspect of the feature (and therefore the entire project as a whole).
- **Can Decrease Overall Development Time**: [According to some](http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.7.4232), `test-driven development` practices have been shown to reduce the total development time for a project when compared to a traditional, non-test-driven model.  While total lines of code increase (due to the extra lines in tests), frequent testing often prevents bugs and catches existing bugs much earlier in the process, preventing them from becoming problematic later down the line.

## Disadvantages of Test-Driven Development

- **Discourages Big Picture Design**: Since `test-driven development` encourages developers to write the simplest possible test, then resolve that test with the simplest possible code, this can often lead to a severe lack of scope regarding the overall design of a feature or the whole project.  When using `test-driven development` practices, it's all too easy to miss the forest for the trees, since your entire focus is on the minutiae of the problem at hand.  Repeating these focused practices during minute-to-minute development often leads to designs that are too specific in nature, rather than stepping back and looking at the big picture.
- **Difficult to Apply in All Cases**: `Test-driven development` is great at handling smaller projects, or even small components and features of larger projects.  Unfortunately, `test-driven development` practices may begin to falter when applied to incredibly massive or complex projects.  Writing tests for a complex feature that you may not yet fully understand can be difficult if not impossible.  Writing tests is great, but if those new tests don't accurately represent the requirements of the feature, they serve no purpose (or may even actively hinder development).  Moreover, some projects -- particularly those dealing with legacy code or third-party systems -- simply don't lend themselves to `test-driven development` practices, because it may be near-impossible to create tests that properly integrate with those systems or that legacy code.
- **Requires Additional Time Investment**: While arguably the time spent on generating tests upfront is saved later on in the development life cycle (see `Advantages` above), the fact remains that `test-driven development` requires a significant upfront output of time and energy to come up with and write tests.  [For many developers](http://beust.com/weblog/2008/03/03/tdd-leads-to-an-architectural-meltdown-around-iteration-three/), that time may be better spent simply writing new or refactoring existing code, 

---

__META DESCRIPTION__

An overview of test-driven development practices, including the steps for common implementation, along with a handful of advantages and disadvantages.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Test-driven_development
- http://www.eecs.yorku.ca/course_archive/2003-04/W/3311/sectionM/case_studies/money/KentBeck_TDD_byexample.pdf
- http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.7.4232
- http://david.heinemeierhansson.com/2014/tdd-is-dead-long-live-testing.html
- http://beust.com/weblog/2014/05/11/the-pitfalls-of-test-driven-development/
- http://beust.com/weblog/2008/03/03/tdd-leads-to-an-architectural-meltdown-around-iteration-three/
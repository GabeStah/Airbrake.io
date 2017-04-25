# Behavior-Driven Development - What is it and how do you use it?

The other week we covered the first part of a miniature series covering a trifecta of software development life cycle articles beginning with [Object-Oriented Analysis and Design - What is it and how do you use it?](https://airbrake.io/blog/design-patterns/object-oriented-analysis-and-design).  Shortly after its introduction, `Object-Oriented Analysis and Design` was the stepping off point for another new software design pattern, which we covered a few days later in [Domain-Driven Design - What is it and how do you use it?](https://airbrake.io/blog/software-design/domain-driven-design).  A few weeks prior, we also explored [Test-Driven Development](https://airbrake.io/blog/sdlc/test-driven-development), which is also related to today's topic.

The next logical step in this series of models is `behavior-driven development` (`BDD`), which is the topic we'll be exploring today.  Throughout this article we'll examine just what `BDD` is, how it is commonly put into practice, and any potential advantages or disadvantages you might encounter when implementing `behavior-driven development` techniques into your own projects.  Let's get crackin'!

## What is Behavior-Driven Development?

At its core, `behavior-driven development` is a software development methodology that combines practices from [`test-driven development`](https://airbrake.io/blog/sdlc/test-driven-development) (`TDD`) and [`domain-driven design`](https://airbrake.io/blog/software-design/domain-driven-design) (`DDD`).

As we saw in our [previous article](https://airbrake.io/blog/sdlc/test-driven-development), `test-driven development` focuses development on short, cyclical iterations in which (failing) tests are initially created that define the desired functionality, and _only then_ is actual code written that ensures those previously-failing tests can now pass.  When performed consistently and rapidly, this `TDD` structure can dramatically reduce overall development time and lower reported bug counts, since code bases tend to be stronger and more stable throughout the software development life cycle.

As we also explored in a [prior article](https://airbrake.io/blog/software-design/domain-driven-design), `domain-driven design` centers on the concept of `domain` and `domain logic`, which simply encompass the overall "sphere of knowledge or activity around which application logic revolves."  Thus, `DDD` practices attempt to simplify the terminology within the project scope by focusing and defining everything in the application as real-world objects or concepts that most people are familiar with.  This largely simplifies communication and encourages the team to develop an application that precisely fits the needs of the particular `domain` in question.

Thus, by combining parts of both `TDD` and `DDD`, `behavior-driven development` aims to simplify development through the use of a common `domain-specific language` (`DSL`), which is used to adapt natural language sentences and phrases into executable tests.  Whether it is recognized or not, this practice of using natural language sentences to describe and define tests is used in a plethora of programming languages and testing suites.  As a simple example, the built-in testing framework for `Ruby on Rails` allows tests to be defined like so:

```ruby
test "User should have a valid email address." do
  # Test code to be executed to check for valid email address.
end
```

While this is treated behind-the-scenes as a normal method, to the end-user when executing the test it's easy to quickly differentiate between this test and others, while also immediately recognizing what this test aims to prove or disprove.

## The Principles of Behavior-Driven Development

At its core, `behavior-driven development` expands on `TDD` and `DDD` by narrowing in on the notion of **behavior**.  While "loose" `TDD` allows for tests to focus on all requirement levels within the application, `BDD` states that tests should be defined in terms of the "desired behavior of the unit."  As we learned about from our `DDD` article, this `behavior` is best defined as the relevant `business logic` or `domain logic` for that particular software unit.

Beyond that overall focus on behavior, `BDD` also specifies a handful of principles that should be put into practice.

### Behavioral Specifications

Typically, defining behaviors within `BDD` is accomplished through `user stories`.  These are written-out scenarios that include some sort of baseline title that summarizes the intent, a `narrative` section that describes the `who` and `what` that should be involved in achieving this story requirement, and the `scenarios` section that describes a series of specific scenarios via `if-then`-style conditions.

While `BDD` doesn't enforce any one particular syntax or format for these `user stories`, it does suggest that your team standardize a format to abide by.  This will ensure that your team can continue to easily discuss and modify stories, and multiple team members can create stories without the need to work closely together.

Here is a typical `user story` format used in `BDD` projects, as recommended by Dan North, who is considered to more or less be the "founder" of `BDD`:

```
Title (one line describing the story)
 
Narrative:
As a [role]
I want [feature]
So that [benefit]
 
Acceptance Criteria: (presented as Scenarios)
 
Scenario 1: Title
Given [context]
  And [some more context]...
When  [event]
Then  [outcome]
  And [another outcome]...
 
Scenario 2: ...
```

Expanding on this template, Mr. North provides a filled out `user story` example in his [Introducing BDD](https://dannorth.net/introducing-bdd/) article:

```
Story: Account Holder withdraws cash
 
As an Account Holder
I want to withdraw cash from an ATM
So that I can get money when the bank is closed
 
Scenario 1: Account has sufficient funds
Given the account balance is \$100
 And the card is valid
 And the machine contains enough money
When the Account Holder requests \$20
Then the ATM should dispense \$20
 And the account balance should be \$80
 And the card should be returned
 
Scenario 2: Account has insufficient funds
Given the account balance is \$10
 And the card is valid
 And the machine contains enough money
When the Account Holder requests \$20
Then the ATM should not dispense any money
 And the ATM should say there are insufficient funds
 And the account balance should be \$20
 And the card should be returned
 
Scenario 3: Card has been disabled
Given the card is disabled
When the Account Holder requests \$20
Then the ATM should retain the card
And the ATM should say the card has been retained
```

### Ubiquitous Language

Just as we discussed in our [Domain-Driven Design](https://airbrake.io/blog/software-design/domain-driven-design) article, `behavior-driven development` heavily emphasizes the importance of a `ubiquitous language`, which is also commonly referred to as `domain-specific language` or `DSL`.  The `DSL` should be clearly defined and agreed upon by all team members early in the development life cycle.  `DSL` allows for easy communication about the `domain` of the project, and should be both simple and robust enough to support discussion between all types of personnel, from developers and team leaders to customers and business executives.

### Using Specialized Tools

`Behavior-driven development` is heavily supported by specialized tools that aid in the creation and execution of testing suites.  Just like automated testing tools used in `test-driven development`, `BDD` tools will similarly perform automated tests in an aim to streamline the development process.  The big difference, however, between `TDD` and `BDD` testing tools is that `BDD` tools are tightly linked to the `DSL` that has been defined for the project.  As such, test specifications inside typical `BDD` testing tools will aim to directly copy the language and phrases from the `DSL` `user stories` that have already been defined.

Looking back at `Scenario 1` from Dan North's `Account Holder withdraws cash` story above, for example, we can see how that scenario might be translated into actual automated test code (using the Ruby programming language, in this particular case):

```
Scenario 1: Account has sufficient funds
Given the account balance is \$100
 And the card is valid
 And the machine contains enough money
When the Account Holder requests \$20
Then the ATM should dispense \$20
 And the account balance should be \$80
 And the card should be returned
```

And here is the pseudocode that tests for that scenario in Ruby (using [`Cucumber`](https://cucumber.io/) syntax):

```ruby
Given /^the account balance is \$100$/ do
  Account.balance = 100
end

And /^the card is valid$/ do
  Account.card.valid = true
end

And /^the machine contains enough money$/ do
  Machine.balance = 10000
end

When /^the Account Holder requests \$20$/ do
  request_money(20)
end

Then /^the ATM should dispense \$20$/ do
  expect(Account.dispensation).to be 20
end

And /^the account balance should be \$80$/ do
  expect(Account.balance).to be 80
end

And /^the card should be returned$/ do
  expect(Machine.card?).to be false
end
```

## Advantages of Behavior-Driven Development

Since `behavior-driven development` is heavily derived from and influenced by `test-driven development`, many of the same benefits that apply to `TDD` also apply to `BDD`.

- **Reduces Regression**: With a full suite of tests being continually executed, and with new tests always being added, `BDD` dramatically reduces the likelihood of regression bugs popping up, since the codebase will be in a constant state of monitoring and testing.  
- **Improves Team Communication**: The reliance on a well-defined `ubiquitous language`/`DSL` means that `BDD` can often improve communication across the entirety of the team, or even among organizations, since there is a common, real-life basis for phrases and terminology when discussing the project.

## Disadvantages of Behavior-Driven Development

- **Requires Specification Before Development**: For better or worse, `behavior-driven development` requires that the team sit down and write out both the `DSL` and in-depth specification documentation (`user stories`) for each particular scenario or feature, before even a single line of functional code can be written.  For many teams, particularly dealing with larger projects, this restriction may not be an issue, but for smaller teams and rapid projects, that extra effort and requirement may be more hindrance than help.
- **Relies on Constant Outside Feedback**: While staying in touch with users, customers, or domain experts may not be a problem for some teams, for many organizations this requirement of constant contact with outside people can have a negative impact on development time.  In cases where feedback is required to flesh out a new `user story` or scenario prior to tests being written -- let alone functional code -- if the relevant domain expert is unavailable at that time, development can grind to a halt (or be forcibly redirected, which is effectively the same thing in many cases).

---

__META DESCRIPTION__

A close examination of the behavior-driven development software development methodology, with example user stories, scenarios, and more.

---

__SOURCES__

- https://dannorth.net/introducing-bdd/
- https://en.wikipedia.org/wiki/Behavior-driven_development
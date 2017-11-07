# Planning Your Department's Production Monitoring Budget

Managing budgets and other important accounting practices for software development projects can be tricky.  Many of the standards and practices that have been used for decades are rapidly becoming severely outdated and obsolete, so it can be challenging to properly plan your department's production monitoring budget, along with the other critical components necessary to produce a successful application; one that remains healthy well after it's in the hands of your customers or clients.

In this article we'll explore some of the most important accounting practices, particularly as they pertain to modern software development life cycles.  We'll then jump into some budgeting techniques to help you plan for the implementation of production monitoring services into your post-launch maintenance budget.  Away we go!

## Generally Accepted Accounting Principles

Before we get into the specifics of budgeting, let's briefly look at overall financial, budgeting, and accounting principles.  In the United States most organizations will abide by the set of accounting rules and practices known as the [`generally accepted accounting principles`](https://en.wikipedia.org/wiki/Generally_Accepted_Accounting_Principles_(United_States)) (or `GAAP`).  The purpose of following `GAAP` guidelines is, among other things, to provide useful information to executives and investors necessary for making long-term financial decisions.  This includes forming appropriate judgment about budgetary requirements and allotments.

To accomplish its goals, `GAAP` follows a handful of assumptions, principles, and constraints.

### Assumptions

- `Business Entity`: Your business, including its revenue and expenses, is an entity unto itself, separate from owners or other businesses.
- `Monetary Unit`: A specific monetary currency is used for all record keeping.  For `GAAP` and most US-based organizations, the US Dollar is the assumed monetary unit.
- `Periodicity`: Your business's economic activities can be divided into arbitrary time periods (e.g. `fiscal quarter`).

### Principles

- `Historical Cost Principle`: Your company must report the costs of assets and liabilities at the time of acquisition, as opposed to fair market value.  However, most debts and securities are reported at market value.
- `Revenue Recognition Principle`: Your business must record revenue when it is _earned_, but not when it is _received_.  On the flip side, losses must recorded when their occurrence becomes "probable," regardless of whether the loss has already occurred.
- `Matching Principle`: Revenues and expenses must be matched to one another when reasonable to do so.  As such, expenses are recognized when the work resulting from said expense contributes to revenue.
- `Full Disclosure Principle`: Your company should disclose enough information for others to make sound decisions based on said information, while also maintaining reasonable costs.  The trade-off between providing more information and the additional costs to provide that information must be made.  Relevant financial information should be provided in the body of financial statements.

### Constraints

- `Objectivity Principle`: Your business's financial statements must be factual and based on objective evidence.
- `Materiality Principle`: An item should be disclosed and considered "significant" if that item would affect the decision of a reasonable person.
- `Consistency Principle`: Your company should maintain the same accounting principles and practices from one time period to the next.
- `Conservatism Principle`: When choosing between two financial decisions, your organization should choose the option that is least favorable (i.e. more conservative).
- `Cost Constraint`: The benefit of reporting financial information should exceed the cost of supplying said information.

## Budgeting Challenges With an Agile Methodology

Organizations that implement a more traditional [`waterfall` software development methodology](https://airbrake.io/blog/sdlc/waterfall-model) are often able to rely on more traditional accounting and budgeting techniques.  However, as modern development methodologies embrace more [`agile` models](https://airbrake.io/blog/sdlc/agile-model), it can be challenging to apply those same financial practices to an organization or project that is constantly changing.

`Capital expenditures` are funds that are generally used to purchase major assets or services that improve your company's ability to generate profit.  Such expenditures typically include physical goods like computer hardware, software, or a new office building.  In general, capitalized expenditures are not recorded on a yearly or quarterly basis like normal `operating expenses`, but are instead recorded as long-term assets, using depreciation to spread the cost of the asset over its useful life, which is typically between five and ten years.

Operating expenses, on the other hand, cover all the ongoing expenses required to run your business.  This includes things like payroll, employee benefits, rent, transportation, and so forth.

Things get a bit more complicated for software development organizations, particularly during agile development life cycles.  For most US-based businesses, according to the [`Statement of Financial Accounting Standards No. 86`](http://www.fasb.org/jsp/FASB/Document_C/DocumentPage?cid=1218220127961&acceptedDisclaimer=true), "all costs incurred to establish the technological feasibility of a computer software product
to be sold, leased, or otherwise marketed are research and development costs" and "shall be charged to expense when incurred."  Proving that your product is "technically feasible" requires that your organization has completed all "planning, designing, coding, and testing" necessary to meet the software's design specifications and technical performance requirements.

Once production actually begins, however, the "costs of producing `product masters` incurred subsequent to establishing technological
feasibility shall be capitalized."  That is to say, after you've proven your software is technically feasible, all production related costs (coding, testing, and so forth) can be considered capital expenditures.

Finally, capitalization of costs must halt once your software is released to customers in a production environment.  Moreover, costs of "maintenance and customer support shall be charged to expense when related revenue is recognized or when those costs are incurred, whichever occurs first."

In other words, the actual period in which your team is developing and producing the software can be capitalized, while all other costs (including those for post-production maintenance and monitoring) are considered normal operating expenses.

## Calculating an Overall Budget

Many of the standard accounting and financial practices used by the `FASB` -- and via `GAAP` -- can often be a bit outdated, particularly as they pertain to the rapidly-changing realm of software development.  For example, the [`Statement of Financial Accounting Standards No. 86`](http://www.fasb.org/jsp/FASB/Document_C/DocumentPage?cid=1218220127961&acceptedDisclaimer=true) document we examined in the previous section -- while generally applicable to software development organizations -- was originally published over 30 years ago in August 1985.

To cope with modern business practices, the `FASB` occasionally releases updated standards documents.  One such document is [`Intangibles — Goodwill and Other Internal-Use Software (Subtopic 350-40)`](https://asc.fasb.org/imageRoot/74/64938874.pdf), which covers the accounting practices for cloud computing arrangements and was published in April 2015.  This document outlines some critical accounting scenarios such as software as a service, platform as a service, infrastructure as a service, and so forth.

One such `intangible asset` that is crucial to a robust and well-supported application after release is production monitoring software.  Coming up with the appropriate production monitoring budget can be difficult, but the newer standards and accounting guidelines linked above can help.  Here are a handful of tips to get you started on overall budgeting, which will help you determine your organization's production monitoring budget.

### 1. Identify Your Key Decisions

Start by brainstorming a list of decisions you want to make with this budgeting process.  A few examples might be:

- Begin development of project `X` or not?
- What is the budget for project `X`?
- Is it time to start advertising?
- Are we ready to launch to production?
- How will we monitor our application after launch?

With the critical decision points in hand, you can move onto the next step.

### 2. Break Down Software Features

Next, make a list of all the major features the application should have.  For example, a tic-tac-toe knockoff might require:

- Players
- Turns
- Winning/Losing
- UI
- Art
- Music/Sound
- Leaderboards
- Cloud Syncing
- Production Monitoring

And so forth.  All these features should be high-level concepts at first, but from each concept you can start to break it down into smaller, more tangible pieces of functionality.

### 3. Budget Each High-Level Feature

Now we select a single high-level feature and break it down as much as possible, which will allow us to roughly estimate the manhours (and therefore cost) of implementing said feature.  To illustrate, let's break down the `UI` component of our tic-tac-toe game:

- Launch screen
- Main menu
- Buttons
- Score
- Game board
- Leaderboard
- Dialog popups
- Text/Localization

We can dig down as much as we want, but with our component extrapolated we can estimate the manhours or weekly work necessary to complete each sub-section:

- Launch screen - 1.5 hours
- Main menu - 3 hours
- Buttons - 1 hour
- Score - 0.5 hours
- Game board - 4 hours
- Leaderboard - 5 hours
- Dialog popups - 1 hour
- Text/Localization - 6 hours

These are just made up numbers, of course, but with this estimate in hand we can start to budget for each high-level feature.  In total, we've estimated that our `UI` feature will require around `22 hours` of work.  We can now apply our weekly operating expenses for a `~22` hour period and come up with an overall cost for this feature.  For larger projects, we'd likely use weeks or even months as our time period to estimate, but the same principles apply.

Let's imagine we've determined that it'll cost us around `$15,000` to operate the company over the `22 hour` period necessary to complete our `UI` feature.  We can then make note of that on the high-level feature list and, once all features are budgeted, come up with a grand total budget.  We won't bother doing so for this example, but this concept makes it easy to determine which features are within budget, and which may need to be scaled back (or removed altogether).

## Planning Your Production Monitoring Budget

One of the high-level features that should be calculated and budgeted for, alongside the normal maintenance costs, is the production monitoring budget.  The best error monitoring services allow you and your team to see the exact nature of every error, including a plethora of detailed metadata.  When an error occurs using <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-production-monitoring-budget">Airbrake.io's error monitoring software</a>, Airbrake is able to report the error type, error message, parameters, the API method that generated the error, the detailed backtrace, the client machine data, the environment in which the error occurred, the file that caused the error, and much more.

Thus, not only does error monitoring software allow you to track and immediately identify exceptions when they occur, it also provides a substantial safety net, particularly during production releases.  While you'll still want to plan accordingly and establish a sound security testing suite, error monitoring services provide a bit of breathing room by promising to inform you of any unforeseen issues.

Best of all, <a class="js-cta-utm" href="https://airbrake.io/pricing?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-production-monitoring-budget">Airbrake.io's error monitoring software</a> is competitively priced so it won't ruin your development or post-production budget.  Get started today and see for yourself why so many of the world's best engineering teams are using `Airbrake` to revolutionize their production monitoring practices!

---

__META DESCRIPTION__

A close look at planning your department's production monitoring budget, with an overview of generally accepted accounting principles and standards.

---

__SOURCES__

- https://hbr.org/2011/09/why-your-it-project-may-be-riskier-than-you-think
- http://www.investopedia.com/ask/answers/020915/what-difference-between-capex-and-opex.asp
- http://www.accounting.com/resources/gaap/
- https://hbr.org/2014/12/your-agile-project-needs-a-budget-not-an-estimate
- http://www.fasb.org/resources/ccurl/286/565/fas2.pdf
- http://www.fasb.org/jsp/FASB/Document_C/DocumentPage?cid=1218220127961&acceptedDisclaimer=true
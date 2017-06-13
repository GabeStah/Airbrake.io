# What Code Quality Metrics Should Management Monitor?

Let's face it: It can sometimes feel downright impossible to properly measure code quality throughout the entirety of the development life cycle, particularly for the some of the massive applications we see produced today.  There are nearly as many different _types_ of metrics and best practices out there as there are applications of which to measure.  This abundance of choices and numerous complexities often lead to the question of what code quality metrics should management monitor?

In this article we'll aim to help break down that complexity by establishing a few general categories of code quality metrics.  Within each category we'll list a few of the best and most widespread choices in hopes that you and your team will have a better idea of which to focus on and which you should ignore.  Let's get started!

## Quality Metrics

Quality metrics aim to keep the code base in tip-top condition by ensuring that quality is maintained throughout the entire development life cycle.

### Average Percentage of Faults Detected

Average percentage of faults detected (or `APFD`) is a fairly well-known code quality  metric in the industry and for good reason: It aims to measure the rate of faults or bugs relative to the percentage of the test suite that is being executed. 

While there are certainly more complex calculations, which can be [found online](http://www.iosrjournals.org/iosr-jce/papers/Vol16-issue4/Version-1/G016414751.pdf) and which show specifically how APFD can be calculated, the simplest explanation of AFPD is it's a ratio of the **number of faults** relative to the **number of tests** in the test suite.  This metric is a quick and easy way to evaluate the quality of specific components in the application and allows you to focus testing, refactoring, and coding efforts where they're most needed.

### Fault Severity

The next quality metric we'll discuss is fault severity, which looks at each fault that is uncovered during the development life cycle and assigns the fault a `severity` rating from low to high.  The qualities and attributes used to determine the severity will depend on your organization and the business requirements of your application, but even with a rudimentary severity rating applied to each bug it becomes much easier to prioritize test cases based on both the requirements and the severity level of the faults that those tests aim to cover.

### Production Incidents

While launching into production might be a long way off for most beginning projects it's critical to establish a simple yet effective way to measure the application quality and performance once it nears production-readiness.  To that end it's extremely beneficial to measure production incidents, both in number and their rate of occurrence over time.

The actual numbers you aim for will be specific to your team, but obviously the higher percentage of stories and releases that are pushed to production without incident the better.  To assist with this metric make sure to record the initial cause of the issue along with what corrective measures were taken to resolve it.  This allows your organization to effectively perform recursion testing for the particular issue so it's less likely to pop up again.

This metric style also works well in organizations with multiple teams, since root causes can often be traced to specific teams, which provides insight into which teams are working well and which could use improvement.

### Quality Over Release Life Cycles

Another useful metric is to actively track the life cycle of a release from story creation to code by examining it from the perspective of a single individual and point of entry, such as a developer and his or her system.  Tough choices must often be made regarding how to reduce costs while increasing productivity and quality of releases, so this practice makes it easier to identify where resources should be invested during the life cycle of a release.  Human labor and resources can be invested in one area while automation can be focused on other sections.

Once the process is in place and been practiced a few times it will become faster and faster to produce a quality measurement for a single release life cycle.  Whether your team is able to produce a quality metric result within weeks, days, hours, or even just a few minutes will depend on many factors, but lowering that time investment will make all future development work easier and cheaper.

## Coverage Metrics

Coverage metrics are all focused on determining to what degree the source code of an application is executed each time a test suite runs.  There are typically four levels of coverage metrics that can be measured so we'll briefly cover each to give you an idea of which, if any, may be suitable in your next project.

### Functional Coverage

The total _functional_ coverage metric aims to measure the quantity of functions, methods, classes, and the like that are covered by the test suite.  Less cumbersome to implement than total statement coverage, total functional coverage can also be more focused and better suited for testing actual business requirements.

Even though just focusing on functions can be easier than testing coverage for all statements, in some cases it may still may not be feasible (or even necessary) to build tests that cover every function in the code base.  Therefore it's often a smart choice to implement total functional coverage metrics alongside some form of prioritization.  This will feel similar to the prioritization of `fault severity` that was discussed previously except, rather than prioritizing faults, you're prioritizing the importance of functions and methods instead.  Figure out which are the most critical to the current release or to the story that is being worked on and prioritize their coverage in the test suite coverage.

### Statement Coverage

Drilling down a bit from total functional coverage is the simple yet sometimes controversial total statement coverage metric, which aims to measure how much of the entire code base -- either line-by-line or statement-by-statement, depending on the language in use -- is actually covered by the various test suites you have in place.  While some companies may shun these sorts of rudimentary metrics, others find it useful to be able to automatically and accurately measure a rough percentage of the code base that is currently being tested.

On the other hand, total statement coverage can be extremely cumbersome to implement and may require a massive effort on the part of the developers and testers alike, so it's best to first try this technique early in the development life cycle to see if it fits the needs of your team.

### Branch Coverage

Another form of coverage metric is that of branch coverage, which aims to measure how many branches (i.e. inflection points) in the code base have been executed during testing.  Branch coverage is a somewhat middle ground choice between total statement coverage and total functional coverage, since it will be less verbose than the former but more so than the latter.  It is common practice to aim for branch coverage of at least 95% for new features and 75% for previously-tested code.

### Condition Coverage

The last coverage metric to consider is condition coverage, which measures how many boolean expressions were independently evaluated to be both `true` and `false`, and whether those changes directly impacted the outcome of said boolean decisions.

This metric is not commonly implemented within most test suites but it can be useful for highly-sensitive software like financial services or safety systems.

## Complexity Metrics

Just as the name implies the category of complexity metrics tend to contain more advanced metrics that may take more time and effort to implement, but consequently may also produce exceptional results.

### Cyclomatic Complexity

[Cyclomatic complexity](https://en.wikipedia.org/wiki/Cyclomatic_complexity) measures the number of linearly independent paths within the source code of your application.  For example, executing one statement after another without any `goto`-style jumps -- like `if-else` blocks, function or method calls, object instantiations, and so forth -- follows an execution path that is linear; that is, it doesn't branch at any point.  However, most modern languages, particularly object-oriented varieties, tend to feature a plethora of branching execution paths that can be followed depending on the application state from one line to the next.

Cyclomatic complexity tries to measure just how much the code base branches into functions, methods, control flow statements, and the like.  The general goal with this metric is to reduce cyclomatic complexity as much as possible, which will generally reduce the overall complexity of the code making it easier to modify or debug when something goes wrong.

### Essential Complexity

[Essential complexity](https://en.wikipedia.org/wiki/Essential_complexity) is used in conjunction with the cyclomatic complexity metric to determine how much the code base can (or cannot) be _reduced_ down to only a single entry and exit point during execution.  In other words, how many cyclomatic complexities (methods, functions, control flow blocks, etc) can actually be removed through refactoring until execution flow has the fewest possible independent paths it can traverse.

To understand this concept it helps to look at an example.  Here we have a simple `for` loop and an `if` statement:

```cs
for (i = 0; i < 10; i++)
{
    if (i == 3)
    {
        foo = bar;
    }
}
```

While this is a rather obvious example the idea of essential complexity is to look at a snippet like this one and realize that we can effectively _remove_ both the `for` loop and the `if` statement entirely because,  in this specific situation and barring any outside changes to the variables seen here, the `for` and `if` statements serve no purpose and the `foo = bar` assignment will always execute.  This gives the above snippet an essential complexity measurement of `1` since we can reduce it to one single linear path.

As might be obvious, a higher essential complexity rating generally means more difficulty using and testing that code so the suggested complexity level is `4` or less.

### Integration Complexity

The integration complexity metric simply measures the level of interaction between different modules or components within the application.  This is typically calculated by using the size and overall complexity of a target module while ignoring the internal logic of said module.  This size/complexity measurement is summarized and then compared as a ratio to the number of methods in the code base.  This metric can then be used to help determine roughly how many and what types of integration tests are needed in the test suite.

### Cyclomatic Density

Cyclomatic density is the ratio of logical decision points (i.e. linear paths which can be taken) to the number of lines of code in the code base.  The higher this metric the more complicated the code will likely be, making future work and testing more difficult.

## Object-Oriented Metrics

The last category we'll cover today is object-oriented metrics, which are focused on measuring the relationships and coverage between independent classes, modules, and the like.

### Average Cyclomatic Complexity

This metric merely breaks down the cyclomatic complexity into class-specific measurements and averages the value over the entire collection of classes in the system.  The higher the average, the more complex the code base and the more likely refactoring should take place.

### Average Essential Complexity

This time we're averaging the essential complexity metric of classes in the code base.  Since the goal of essential complexity is to reduce the number of possible execution paths as bear to one as possible that same goal applies to the average measurement.  If the average essential complexity for classes is much greater than one then refactoring may be required.

### Number of Parents/Children

As the name states this metric merely measures how many parents (or children, depending how you wish to evaluate it) each class has.  While some instances of multi-inheritance may be necessary the more parents an object has the more complex it becomes.

### Responses

The responses for class metric adds the number of methods the target class implements to the number of _custom_ methods that are accessible to an instance of the target class.  Obviously when working with frameworks you wouldn't add methods that are accessible merely because they're from the framework's public API, so instead the focus should be only on methods your application implements in some way.

As usual, the higher the number of possible method invocations the more complex the class.

### Coupling Between Objects

Coupling between objects is a crucial measurement that aims to count how many non-inherited classes a target class depends on.  Simply put, when the coupling between objects is high that typically means the particular target class is too intertwined with other classes and isn't considered truly object-oriented or reusable.

As projects tend to expand in breadth and scope this metric tends to increase, so keep a close eye on it throughout the life cycle to ensure that classes remain slim and agile.

### Class Hierarchy Level

Class hierarchy level measures how many objects inherit from the target class.  If this number exceeds `six` then it typically indicates the class will be too difficult to properly test.  Conversely, if the class hierarchy level is at `one or fewer` that indicates an improper use of object-oriented techniques.  The recommended saturation level across the board is somewhere between `two and three` levels for each class.

### Number of Methods

This metric is quite straightforward and measures the number of methods implemented in a class.  The suggested limit is no more than `20` methods within a single class.

### Method Cohesion

The method cohesion metric measures the average percentage of methods within a class that are using a particular attribute or property of that class.  If this percentage is low because not many methods are using certain attributes then refactoring could help improve reusability.  On the other hand if method cohesion is high then attributes are being used effectively and thoroughly.

---

This concludes our brief glimpse a few of the many possible code quality metrics that can be implemented into modern development life cycles.  Hopefully these gave you a few good ideas for the next project your team takes on, and just to make sure your organization is fully prepared check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-bugs">Airbrake's</a> exception handling tools and to see how your team can keep on top of any defects that slipped through the cracks during production.

---

__META DESCRIPTION__

A brief overview of the most common and effective code quality metrics that managers should look to for all their upcoming projects.

---

__SOURCES__

- http://www.mccabe.com/pdf/McCabeCodeQualityMetrics-OutsourcedDev.pdf
- https://techbeacon.com/top-5-software-quality-metrics-matter-right-now
- https://en.wikipedia.org/wiki/Software_metric
- https://en.wikipedia.org/wiki/Code_coverage
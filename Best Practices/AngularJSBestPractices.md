AngularJS has rapidly become one of the hotest new trends in development for creating powerful web or local applications on virtually any platform.  While this extraordinary platform heralds itself as a `MVW` (Model-View-Whatever) framework - and is therefore suited for whatever your team or project requires - it can also be an overwhelmingly daunting foundation to build off of.

To help alleviate any potential vertigo as you gaze down from the mountain of potential that is AngularJS, we've put together a handful of best practices to keep your code clean and your mind (somewhat) sane.

## Cultivate a Tidy Project Architecture

Perhaps the best place to begin with any development project, but even more so with AngularJS, is in the creation of a properly structured project.  The configuration of project files, directories, and components plants the seeds of the entire life cycle of the project.  Components that are illogically structured will make for a miserable development experience as new components must be added or code must be altered.

The challenge is that the sheer scope of the code base for even the most rudimentary AngularJS project can be overwhelming, to say the least.  Therefore, it's strongly recommended that you relieve yourself of a modicum of this burden by establishing and maintaining proper project architecture, particularly when your venture is just taking off.

A great place to start is to consider how the business logic of your project will be configured utilizing the primary [building blocks](https://angular.io/docs/ts/latest/guide/architecture.html) of Angular.  While some of these are designed specifically for Angular 2, they can be implemented within Angular 1 even, and generally represent the community-accepted method for structuring your project.

- __Modules__: At the basic level, Angular `modules` are simply `classes` that use the `@NgModule` decorator and allow you to disperse your application into logical blocks of functionality.
- __Components__: Similar to `controllers` in other popular `MVC` frameworks, `components` are used to create the user interface (i.e. `views`) within your application.
- __Templates__: As with `views` in other `MVCs`, `templates` in Angular are altered forms of HTML that describe how the application renders the interface.
- __Metadata__: Typically a short and sweet collection of data that precedes a class, which is used to _explicitly_ inform Angular that the class definition which follows is a particular type of class, such as a `component`.
- __Data Binding__: Angular allows for simple binding of data between the `DOM` and the underlying `components`.  Depending on the syntax used within the `templates`, this binding can be one way from `DOM` to `component`, `component` to `DOM`, or simultaneously in both directions.
- __Directives__: Markers attached to particular `DOM` elements which apply a specific behavior or transformation to the element.
- __Services__: Perhaps the most generic part of an Angular application, a `service` is typically a class with well-defined, logical functionality, such as configuration, logging, or data management.
- __Dependency Injection__: Finally, with a number of `services` powering your application, you can utilize an `injector` to inform a particular `component` about which `services` it requires to be functional.

## Exploit a Task Runner

If you're at all experienced with modern JavaScript application development, you've undoubtedly heard of (if not used) [`Grunt`](http://gruntjs.com/).  For those unfamiliar, `Grunt` is a `task runner`, which essentially means it is a tool that will __automate__ a number of menial tasks you would otherwise need to perform yourself on a fairly regular basis.  Not only does the automation provided by a task runner like `Grunt` reduce your workflow requirement as a developer, but it also eliminates many unintentional mistakes that may be made by human hands.

`Grunt` is capable of many, many types of tasks (inherently or custom-designed), but a few of the common uses include:

- __Minification__: To speed up browser load times, most web applications "minify" their combined JavaScript files, a task `Grunt` can handle automatically.
- __Live Reload__: During development, can automatically reload your browser when changes are made to the code.
- __Best Practice Hints__: Like the live reload function, when code is altered, `Grunt` will analyze and output any hints about potential changes to conform to JavaScript best practices.
- __SASS/LESS Compilation__: Will automatically compile any CSS-style framework files into actual CSS.
- __THOUSANDS More__: `Grunt` has a [`massive plugin directory`](http://gruntjs.com/plugins) that contains a plethora of potential tasks depending on your needs.

## Exercise Proper Style Guidelines

As with any programming language, AngularJS (and the JavaScript that effectively powers it) can be written in a variety of ways and styles, but "breaking the mold" of the established style guides used by the AngularJS community is never advisable.

Instead, wherever possible, it is highly recommended that you try to follow the popular best practice style guidelines laid out in [`this documentation`](https://github.com/johnpapa/angular-styleguide).  More specifically, if you're using the latest version of AngularJS (`Angular 2`), these guidelines are now actually part of the [`official AngularJS documentation`](https://angular.io/docs/ts/latest/guide/style-guide.html).

## Automate Your Testing

Just as `Grunt` can be used to help you automate a number of menial tasks during AngularJS development, with the growing popularity of heavy focus on test-driven development (TDD), there's never been a better time to get in the habit of using a `test runner` tool to automate and speed up all your testing requirements.

Best of all, the AngularJS team has taken it upon themselves to develop and support an extremely powerful `test running` tool known as [`Karma`](http://karma-runner.github.io/) (previously named `Testacular`).

At the outside, it should be understood that `Karma` _is not_ a `test framework`, meaning you will not directly write code in a specific format or syntax to then instruct `Karma` to perform a few tests.  Instead, as a `test runner`, similar to the way `Grunt` is a `task runner`, `Karma` is used to __automatically execute your entire test suite on real browsers and devices__.  The power of this simple differentiation from most testing that is not performed on actual browsers/devices cannot be overstated.

The `Karma` homepage includes a nice little introductory video on how `Karma` works, but the simple explanation is that once you've written a few tests in your favorite `test framework` ([`Jasmine`](https://jasmine.github.io/) is a popular and powerful choice), _anytime_ you make a change to your codebase and save the file, `Karma` will recognize that change, immediately execute all your tests, and display the results in your terminal.  This saves loads of time and headache during development, ensuring that you never have to remember to run tests after changes are made.

## Utilize Helper Modules

One final tip we'd like to pass along to improve your AngularJS development experience is to make heavy use of the work of others that have come before you, in the form of the numerous and often exceptionally powerful `helper modules` available to AngularJS.

Just as with libraries or plugins for other programming frameworks, `modules` in AngularJS are packaged code components that provide additional functionality out of the box.  `Modules` can greatly speed up your own development and improve productivity by allowing you to focus on the core business logic of your code that is unique to the project at hand.

There are a number of sources where you can find helpful modules, but a few examples are [`ngmodules.org`](http://ngmodules.org/) and [`AngularUI`](http://angular-ui.github.io/).  `AngularUI` will be of particular interest to anyone with experience in (and fondness for) the jQuery framework and it's plethora of popular UI components.

Beyond that, the [`UI-Router`](https://ui-router.github.io/) module is a popular way to easily implement client-side routing, whereby the browser's displayed URL automatically updates based on the server-side routing (e.g. `app/#/users`, `app/#/login`, etc).

The [`Restangular`](https://github.com/mgonto/restangular) module is another proven service that allows for simple implementation of a standard RESTful API through standard `GET`, `POST`, `UPDATE`, and `DELETE` requests.

---

__SOURCES__

- https://github.com/ngbp/ngbp
- https://github.com/johnpapa/angular-styleguide
- https://artandlogic.com/2013/05/ive-been-doing-it-wrong-part-1-of-3/
- http://gruntjs.com/
- http://karma-runner.github.io/
- https://jasmine.github.io/
- https://docs.angularjs.org/guide/unit-testing
- http://angular-ui.github.io/
- https://ui-router.github.io/
- https://github.com/mgonto/restangular
- https://artandlogic.com/2013/05/angularjs-best-practices-ive-been-doing-it-wrong-part-3-of-3/
- https://angular.io/docs/ts/latest/guide/architecture.html

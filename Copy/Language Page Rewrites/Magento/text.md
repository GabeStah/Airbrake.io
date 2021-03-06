# Magento Exception Handling

Magento is one of the leading eCommerce platforms used by many PHP applications around the world, and with the capabilities of the `Airbrake-Magento` module, you can quickly and easily bring the power of real-time error monitoring and automatic exception reporting to your entire Magento application in just a few minutes.  With the `Airbrake-Magento` module there's no more need for manually tracking exceptions or digging around through massive log files.  Best of all, all errors are reported to `Airbrake's` dynamic web dashboard the moment they occur, providing you and your team with immediate alerts and powerful search tools designed to help you find out exactly what went wrong, no matter when and where the problem occurred.

Browse through some of the great features `Airbrake-Magento` has to offer and see why organizations running some of the most demanding Magento applications around are using `Airbrake` to streamline their exception handling practices!

## Features

`Airbrake-Magento` is built to be extremely easy to install, yet it also equips your team with powerful tools.  With `Airbrake-Magento`, all exceptions generated by your Magento application are immediately delivered to you via email, `Airbrake's` powerful web dashboard, and propagated to the many third-party service integrations you may have configured such as `GitHub`, `GitLab`, `JIRA`, `Slack`, and many more.  Once installed and configured via the Magento Administration Panel, the `Airbrake-Magento` module will detect all exceptions and report them via `Airbrake`, including a robust set of metadata aimed at helping your team pin down exactly what caused this error in the first place.

### Dramatically Reduces Logging Requirements

With most Magento applications tracking and handling exceptions is typically a time-consuming and taxing affair.  It often requires writing custom classes and code to track and route all exceptions to the appropriate place, then exporting them to cumbersome log files or sending them out via email chains.  However, with `Airbrake-Magento` installed you no longer need to dig through the dreaded `/var/log` or `/var/report` directories, searching for specific case number files when a particular error occurs.  Instead, Magento will automatically route exceptions through the `Airbrake-Magento` module, thereby immediately reporting them to you and your team via the `Airbrake` dashboard.

### Robust Field Filtering

The `Airbrake-Magento` module includes customizable fields within the Magento Admin Panel, allowing you to change how exceptions are reported and stored.  For example, changing the `Environment` field from `development` to `staging` to `production` throughout the life cycle of your application's development allows you to effortlessly aggregate exception reports on the `Airbrake` dashboard, making it easy to see how your application's health has changed over time.  Moreover, rich search and filtering capabilities on the dashboard allow you to drill down the search results to view only the subset of errors you care about most at that moment, making it a walk in the park to find the exact source of the issue and get to solving it immediately.

### Simple Module-Based Installation

If you've ever installed another Magento before then installing `Airbrake-Magento` will be a breeze, even for individuals that might be less technically-minded.  With the popular and incredibly helpful [`Modman`](https://github.com/colinmollenhour/modman) module manager installed, you can get `Airbrake-Magento` installed, configured, and reporting exceptions with just four commands and a few clicks.

## Quick Setup

1. To begin using `Airbrake-Magento` start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.
2. Open a terminal window, navigate to your Magento project directory, initialize `Modman` (if necessary), clone the `Airbrake-Magento` `git` repository, then deploy it via Modman:

```bash
$ cd <your-magento-project>
$ modman init
$ git clone https://github.com/airbrake/Airbrake-Magento.git .modman/CodebaseExceptions
$ modman deploy CodebaseExceptions --force
```

3. [Create an Airbrake account](https://airbrake.io/account/new) and sign in.
4. Create a new project and copy the `Project API Key` to your clipboard, which can be found on the right-hand side of the `Project Settings` page.
5. Visit your Magento Administration Panel (`[APPLICATION-DOMAIN]/admin` by default) and navigate to `System > Configuration > Elgentos > CodebaseExceptions`.
6. Paste the `Project API Key` into the `Airbrake API key` field, make any other adjustments you'd like, then click `Save Config`:

![magento admin panel airbrake](https://camo.githubusercontent.com/8153d690c29da28305aac3c537b93706825ab522/68747470733a2f2f696d672d666f746b692e79616e6465782e72752f6765742f343030302f39383939313933372e31662f305f62376662615f33303136303430635f6f726967)

7. You're all set!  To confirm that `Airbrake-Magento` is working correctly visit the `[APPLICATION-DOMAIN]/exceptions/index/test` URL, which will generate a new test exception and send it to your `Airbrake` project dashboard.

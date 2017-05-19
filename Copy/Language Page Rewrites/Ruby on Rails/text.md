# Rails Exception Handling

Airbrake's Rails exception handling gem simplifies the error-reporting process for all of your Ruby web framework projects, including `Rails`, `Sinatra`, `Rack`, and more.  Built on top of Airbrake's powerful and robust [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, `Airbrake` provides your team with real-time error monitoring and reporting across your entire application.  Receive instant feedback on the health of your application, without the need for user-generated reports or filling out issue tracker forms.  With built-in integration for Ruby web frameworks, `Heroku` support, and even job processing libraries like `ActiveJob`, `Resque`, and `Sidekiq`, `Airbrake` can be integrated into your application and begin revolutionizing your debugging workflow in just a few minutes.  Have a look below at all the features packed into `Airbrake` and start improving your exception handling practices today!

## Features

`Airbrake` is loaded with features designed with the Ruby framework developer in mind.  Whether you're using the tried-and-true `Rails`, `Sinatra`, or even a `Rack` application, `Airbrake` will seamlessly integrate and provide your team with instant, beautiful error reports available at the `Airbrake` dashboard.  Plus, if you need additional integrations to suit your workflow, `Airbrake` seamlessly integrates with all the most popular job processing libraries.  From there, you can even expand that integration into outside services such as `GitHub`, `Trello`, `Slack`, `JIRA`, and many more, giving you and your team the most up-to-date alerts when something goes wrong inside your application.

Check out just a few of the amazing features below, or take a look at the full [documentation](https://github.com/airbrake/airbrake#key-features) to find out everything `Airbrake` brings to the table for you and your team!

### Broad Framework Support

The `Airbrake` gem integrates easily with the most popular Ruby web frameworks such as [Rails](#integrating-with-rails), [Sinatra](#integrating-with-sinatra), and [Rack](#integrating-with-rack).  After just a few minutes and with only a couple lines of code, you can get `Airbrake` quickly up and running within your existing framework application.  `Airbrake` handles all exceptions across your entire app, automatically and instantly logging and reporting them to your `Airbrake` dashboard.  Of course, you can also customize `Airbrake` using the detailed [configuration API](https://github.com/airbrake/airbrake-ruby#configuration), so it precisely suits your application stack and particular project requirements.

### Easily Integrates With Most Job Processors

Whether you use `Sidekiq`, `ActiveJob`, `Resque`, `DelayedJob`, or even `Shoryuken`, the `Airbrake` gem has you covered with built-in support and easy (often automatic) integration.  In most cases, if your application loaded your job processor prior to loading the `Airbrake` gem, then you don't need to perform any additional steps or configure any other settings -- `Airbrake` will automatically detect errors that occur within your processed jobs and report them to your `Airbrake` dashboard right away.  Best of all, `Airbrake` attempts to provide as much relevant information with the exception report as possible, helping you and your team quickly identify which job caused an issue and why.

For example, here's a sample of the `Parameters` JSON that `Airbrake` automatically generated after a `ZeroDivisionError` occurred during a simple `Sidekiq` job execution:

```json
{
  "context": "Job raised exception",
  "job": {
    "args": [
      "dole",
      5
    ],
    "class": "HardWorker",
    "created_at": 1493342517.8676329,
    "enqueued_at": 1493342606.95342,
    "error_class": "ZeroDivisionError",
    "error_message": "divided by 0",
    "failed_at": 1493342527.5855296,
    "jid": "ca23d933fd6ab9b618eb8543",
    "queue": "default",
    "retried_at": 1493342606.9559603,
    "retry": true,
    "retry_count": 2
  },
  "jobstr": "{\"class\":\"HardWorker\",\"args\":[\"dole\",5],\"retry\":true,\"queue\":\"default\",\"jid\":\"ca23d933fd6ab9b618eb8543\",\"created_at\":1493342517.8676329,\"enqueued_at\":1493342606.95342,\"error_message\":\"divided by 0\",\"error_class\":\"ZeroDivisionError\",\"failed_at\":1493342527.5855296,\"retry_count\":1,\"retried_at\":1493342557.8936946}"
}
```

Check out the [full documentation](https://github.com/airbrake/airbrake#sidekiq) to see how easy it is to integrate `Airbrake` with the job processor of your choice!

### Automatic Deployment Tracking

`Airbrake` makes it easy to automatically track the deployments of all your Ruby-based projects.  By alerting `Airbrake` of your deploys, all existing errors in the specified `environment` can be marked `Resolved`, providing you and your team with a clean slate, making it easy to determine which errors have been fixed by this new release, or which regressions might have recently popped up as a result.  Plus, you can `filter errors by deploy` on the `Airbrake` dashboard and follow simple click through links in your exception backtrace logs to the relevant revision > file > line using a `GitHub` or `GitLab` integration.

For example, to integrate with `Capistrano` deployments, simply `require` the `Airbrake` Capistrano file in your `Capfile`:

```ruby
# Capfile
require 'airbrake/capistrano/tasks'
```

If you're using Capistrano v3, add a call to the `airbrake:deploy` task using an `#after :finished` hook:

```ruby
# config/deploy.rb
namespace :deploy do
  after :finished, 'airbrake:deploy'
end
```

Of course, you're not limited to only Capistrano for deployment tracking, so feel free to check out the [documentation](https://github.com/airbrake/airbrake#deploy-tracking) to see just how `Airbrake` easily integrates into your project deploys.

### Plus Unlimited Custom Parameters, Exception Filtering, Promise Callbacks, and More!

Since the `Airbrake` gem is based on the powerful [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, in additional to all the framework- and gem-related features already mentioned, `Airbrake` provides all the additional functionality of `Airbrake-Ruby`:

- [`Custom Parameters`](https://github.com/airbrake/airbrake-ruby#custom-exception-attributes) allow you to associate an unlimited set of parameters to your exceptions reports, giving you the utmost control over the information your team has access to.
- [`Exception Filtering`](https://github.com/airbrake/airbrake-ruby#airbrakeadd_filter) ensures you send only the errors that matter most.  With a robust API at your fingertips, you can filter out any exceptions that aren't relevant to a particular application, module, or even deployment.
- [`Promise Callbacks`](https://github.com/airbrake/airbrake-ruby#promise) grant your application the ability to perform additional tasks after an exception is sent to `Airbrake` (or even if a report failed to go through).
- And more!  Check out the [`Airbrake-Ruby` language page](https://airbrake.io/languages/ruby_exception_handling) or the [official `Airbrake-Ruby` documentation](https://github.com/airbrake/airbrake-ruby#introduction) for full details on the extra capabilities provided by this powerful gem.

## Quick Setup

### Install via `Bundler`

To install `Airbrake` through `Bundler`, first create a `Gemfile`, if necessary, with `bundle init`:

```bash
$ bundle init
Writing new Gemfile to /home/dev/apps/Airbrake
```

Then add `Airbrake` to your `Gemfile`:

```ruby
gem 'airbrake', '~> 6.0'
```

Finally, tell `Bundler` to install the newly-added gem:

```bash
$ bundle install
```

### Install Gem Manually

If you aren't using `Bundler` and a `Gemfile` for your project, you can easily install `Airbrake` manually from your terminal:

```bash
$ gem install airbrake
```

## Usage

Once the gem is installed, using `Airbrake` is a breeze.  Just find the appropriate framework that matches your application and follow the few easy steps.  Or, if you aren't using a framework and are just creating a plain Ruby application, then check out the [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, which was explicitly developed for baseline Ruby projects.

### Integrating with Rails

The `Airbrake` gem needs access to your `Project ID` and `Project API Key`, both of which can be found on the right-hand side of the `Project Settings` page while being logged into the `Airbrake` dashboard.  With both of these values in hand, open a terminal to your Rails application root directory and enter the `rails generate airbrake` command, followed by your `Project ID` and `Project API Key`:

```bash
$ rails generate airbrake PROJECT_ID PROJECT_API_KEY
Running via Spring preloader in process 10796
      create  config/initializers/airbrake.rb
```

This will automatically generate a configuration file at `config/initializers/airbrake.rb`.  Feel free to take a look at this file as it is well documented and allows you to modify the settings of your `Airbrake` integration, if needed.  The [documentation](https://github.com/airbrake/airbrake-ruby#config-options) includes a full list of configuration values and options.

Lastly, you can test the Rails `Airbrake` integration by invoking the `rake airbrake:test` command (or `rails airbrake:test` for newer version of Rails) from your terminal:

```bash
$ rake airbrake:test
[ruby]
description: ruby 2.4.0p0 (2016-12-24 revision 57164) [x86_64-linux]
...
The test exception was sent. Find it here: https://airbrake.io/locate/00054e1b-5288-186b-045b-123456789000
```

If the integration was configured properly, you should see a confirmation that a test exception was generated and sent to `Airbrake`.  Click the provided link or login to your `Airbrake` dashboard to see the newly generated exception report under your project's `Errors` page.

#### Controller Helpers

By default, `Airbrake` will monitor your entire application and report all exceptions that may occur.  However, for a more manual approach, `Airbrake` also provides two helper methods that can be used inside your Rails controllers: `#notify_airbrake` and `#notify_airbrake_sync`.  Using these methods is ideal when generating manual exception reports, since they will automatically include `Rack` environment information with the produced error reports.

To use `#notify_airbrake` (which is asynchronous) or `#notify_airbrake_sync` (which is synchronous), simply add them to the execution call somewhere in the relevant Controller code:

```ruby
class UsersController < ApplicationController
  def show
    # Find user by :id.
    @user = User.find_by(id: params[:id])
    # Check if user was found.
    if !@user
      # If not found, notify Airbrake of issue with associated hash params.
      notify_airbrake('User lookup not found.', { id: params[:id] })
    end
  end
end
```

The above example asynchronously generates an error and sends it to `Airbrake` if no valid `User` can be found with an `:id`, as provided in the request parameters.

### Integrating with Sinatra

Just as with Rails integration, you'll need to configure the `Airbrake` gem to use your `Project ID` and `Project API Key`, both of which are found on your `Airbrake` project dashboard.  Add the `Airbrake#configure` method block to set configuration values, then make sure your Sinatra app `uses` `Airbrake::Rack::Middleware`, which will ensure exceptions are sent to Airbrake.

For example, here's a basic Sinatra app located in the `myapp.rb` file:

```ruby
# myapp.rb
require 'sinatra/base'
require 'airbrake'

Airbrake.configure do |c|
  c.project_id = YOUR_PROJECT_ID,
  c.project_key = 'YOUR_PROJECT_API_KEY'

  # Display debug output.
  c.logger.level = Logger::DEBUG
end

class MyApp < Sinatra::Base
  use Airbrake::Rack::Middleware

  get('/') {
    # Indirectly raise a ZeroDivisionError for testing.
    1/0
  }
end

run MyApp.run!
```

Make sure your `Rack` configuration file (`config.ru`) requires your Sinatra application, if necessary:

```ruby
# config.ru
require_relative 'myapp'
```

Then call `rackup` from your application root directory to execute your application:

```bash
$ rackup
== Sinatra (v1.4.8) has taken the stage on 4567 for development with backup from Thin
Thin web server (v1.7.0 codename Dunder Mifflin)
Maximum connections set to 1024
Listening on localhost:4567, CTRL+C to stop
```

Visit the localhost web server (or use the `curl` utility) to cause the intended `ZeroDivisionError` and confirm that `Airbrake` is now working with your Sinatra application.

### Integrating with Rack

Integrating `Airbrake` into a Rack application is similar to Sinatra.  Start by `requiring` the `Airbrake` gem, configure your `Project ID` and `Project API Key` values, then make a `#use` call to `Airbrake::Rack::Middleware`:

```ruby
require 'airbrake'
require 'airbrake/rack'

Airbrake.configure do |c|
  c.project_id = YOUR_PROJECT_ID,
  c.project_key = 'YOUR_PROJECT_API_KEY'
end

use Airbrake::Rack::Middleware
```
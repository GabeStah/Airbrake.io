# Ruby Exception Handling

Airbrake's `Ruby` exception handling tool easily integrates into all your new or existing Ruby applications.  Designed from the ground up as the stable and robust framework used to power all of `Airbrake.io's` Ruby-related error handling tools, `Airbrake-Ruby` provides your team with real-time error reporting for applications of any size and scope.  Ideal for plain Ruby v2.0 (and higher) projects, `Airbrake-Ruby` is extremely lightweight and dependency-free, so it won't negatively impact application performance.  Best of all, `Airbrake-Ruby` comes packed with features that developers from teams of all sizes will appreciate including: asynchronous exception reporting, flexible logging capabilities, multiple notifiers to help you precisely categorize errors, Java exceptions through JRuby, custom error parameters, simple filtering tools, and much more!

## Features

`Airbrake-Ruby` comes equipped with a plethora of features suited to your specific Ruby project and workflow.  Check out some of the top features below, or take a look at the [documentation](https://github.com/airbrake/airbrake-ruby) to see everything that `Airbrake-Ruby` can do for your next project!

### Powerful, Easy-to-Use API

The [`Airbrake-Ruby API`](https://github.com/airbrake/airbrake-ruby#api) offers a great deal of functionality, including custom `notifiers` and `notices`, detailed exception filters, custom parameters, promises, and more.  We've detailed the use of a few of these features below, but feel free to check out the [documentation](https://github.com/airbrake/airbrake-ruby#api) for full details on using the `Airbrake-Ruby API`.

### Unlimited Custom Parameters

While `Airbrake-Ruby` will report all the critical, basic exception information (message, call stack, file, etc), you are not constrained to only these default values.  By adding a second parameter to the `#notify` method call, you can include unlimited custom parameters via a `Hash`, ensuring you and your team have access to all relevant exception data:

```ruby
rescue IOError => ex
  # Notify by passing a direct Exception, with a custom parameter Hash.
  Airbrake.notify(ex, {
    user: 'foo@bar.com',
    context: 'lib',
    environment: 'production'
  })
end
```

### Robust Exception Filtering

`Airbrake-Ruby` makes it easy to filter out particular types of exceptions, or even ignore specific `notices`.  By using a `Airbrake#add_filter` method code block, `notices` can be evaluated and ignored, if necessary, using the `notice#ignore!` method.  The `Airbrake#add_filter` method runs a callback before the `Airbrake#notify` method is called, allowing you to easily intercept notices that need further evaluation.

```ruby
# Create a filter callback.
Airbrake.add_filter do |notice|
  # Find any notices with an error class type of RuntimeError.
  if notice[:errors].any? { |error| error[:type] == 'RuntimeError' }
    # Ignore all RuntimeErrors.
    notice.ignore!
  end
end
```

Head over to the [documentation](https://github.com/airbrake/airbrake-ruby#airbrakeadd_filter) for more details on filtering.

### Dynamic Promise Callbacks

Similar to promises found in JavaScript, `Airbrake-Ruby` provides `Airbrake::Promises`, which enable additional callbacks to be chained onto `Airbrake#notify` method calls.  These callbacks will execute when the promise is either successfully resolved (a `notice` is sent) or is rejected (a `notice` is returned by the `Airbrake.io API`).

To create a callback when the promise is successful simply chain the `#then` method onto your `Airbrake#notify` call, with an appropriate code block to handle the `response` that is provided:

```ruby
Airbrake.notify("Open the pod bay doors please, HAL.").then { |response| puts response }
```

Conversely, to create a callback for promises that fail (in which the `Airbrake.io API` rejects the `notice` for some reason), chain the `#rescue` method onto `Airbrake#notify`:

```ruby
Airbrake.notify("I'm sorry Dave, I'm afraid I can't do that.").rescue { |error| puts error }
```

Further details can be found in the [documentation](https://github.com/airbrake/airbrake-ruby#promise).

## Installation

#### Install via `Bundler`

To install `Airbrake-Ruby` through `Bundler`, first create a `Gemfile`, if necessary, with `bundle init`:

```bash
$ bundle init
Writing new Gemfile to /home/dev/apps/Airbrake-Ruby
```

Then add `Airbrake-Ruby` to your `Gemfile`:

```ruby
gem 'airbrake-ruby', '~> 2.0'
```

Finally, tell `Bundler` to install the newly-added gem:

```bash
$ bundle install
```

#### Install Gem Manually

If you aren't using `Bundler` and a `Gemfile` for your project, you can easily install `Airbrake-Ruby` manually from your terminal:

```bash
$ gem install airbrake-ruby
```

## Usage

Once installed, using `Airbrake-Ruby` only requires a few lines of code to quickly get up and running.

#### Create a Notifier

Each `notifier` is associated with a particular `Airbrake` `Project ID` and `Project API Key`, which can be found on the right-hand side of the `Project Settings` page while logged into your `Airbrake.io` dashboard.

Initialize a `notifier` by ensuring that the `Airbrake-Ruby` gem is available, then make a call to the `Airbrake#configure` method, including a block to set the `Project ID` and `Project API Key` properties:

```ruby
# Require the gem where necessary.
require 'airbrake-ruby'

# Create a single notifier.
Airbrake.configure do |c|
  c.project_id = YOUR_PROJECT_ID,
  c.project_key = 'YOUR_PROJECT_API_KEY'
end
```

Alternatively, you may create as many `notifiers` as you wish, each of which can be associated with a unique project through the `Airbrake.io` dashboard.  To create multiple `notifiers`, simply pass an (optional) `Symbol` parameter to the `Airbrake#configure` method, indicating the `name` of the `notifier` you wish to initialize:

```ruby
# Require the gem where necessary.
require 'airbrake-ruby'

# Create a notifier named :staging.
Airbrake.configure(:staging) do |c|
  c.project_id = YOUR_PROJECT_ID,
  c.project_key = 'YOUR_PROJECT_API_KEY'
end

# Create a notifier named :production.
Airbrake.configure(:production) do |c|
  c.project_id = YOUR_PROJECT_ID,
  c.project_key = 'YOUR_PROJECT_API_KEY'
end
```

#### Asynchronous Exception Notification

With a `notifier` configured, now you can send asynchronous notifications to Airbrake using the `Airbrake#notify` method:

```ruby
begin
  puts [1, 2, 3, 4, 5].sample(-3)
rescue ArgumentError => ex
  # Notify by passing direct Exception.
  Airbrake.notify(ex)
end

# Notify by passing a String, via the :staging notifier.
Airbrake[:staging].notify('There was an error!')

# Notify by passing an object which can be converted to a String via a #to_s method call.
Airbrake.notify(3.1415926535)

# Create an instance of Airbrake::Notice.
notice = Airbrake.build_notice('Uh oh, something broke!')
# Add custom parameters to this particular notice.
notice[:params][:username] = 'admin'
# Notify by passing the generated instance of Airbrake::Notice, via the :production notifier.
Airbrake[:production].notify(notice)
```

#### Synchronous Exception Notifications

Alternatively, you can send synchronous notifications using the `Airbrake#notify_sync` method, passing all the same parameter types, while also attaching it to a particular `notifier`, if you wish:

```ruby
begin
  puts [1, 2, 3, 4, 5].sample(-3)
rescue ArgumentError => ex
  # Synchronous notification by passing a direct Exception.
  Airbrake.notify_sync(ex)
end

# Synchronous notification by passing a String, via the :staging notifier.
Airbrake[:staging].notify_sync('There was an error!')
```
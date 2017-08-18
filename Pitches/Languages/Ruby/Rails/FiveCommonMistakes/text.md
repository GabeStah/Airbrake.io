# 5 Common Mistakes in Rails Development

Rails is a powerful and relatively easy to use framework.  Its dominant [`principles`](http://rubyonrails.org/doctrine/) heavily emphasize convention over configuration, allowing new Rails applications to be up and running in a fraction of the time for applications created in many other, more verbose languages.  However, the simplicity that Rails provides can also be a pitfall, particularly for newer developers just getting into the design space.  In this article we'll examine a handful of some of the most common mistakes in Rails development, and provide guidance on how these can best be avoided in your own coding adventures, so let's get to it!

## Application Logic Inside Views

Rails is, by default, a [model-view-controller](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller) (`MVC`) framework.  While other similar `MV_` patterns have been introduced and become more popular as of late, the fundamental nature of Rails is to rely on `models` to handle business logic, `views` to render visual components to the user, and `controllers` to manage the connection between the other two.  

Most developers strongly encourage data access logic inside your `models`, leaving your `views` free to focus purely on the layout and visual aspect of your application.  Unfortunately, `views` are typically written using a templating engine such as `ERB`, which is the default selection for new Rails applications.  `ERB` and the like allow _inline Ruby_ within the `ERB` file itself.  This often leads to the common mistake that some newer Rails developers make: _including application logic inside `views`_.

For example, consider this snippet of the `views/books/index.html.erb` file, which displays a header indicating the `title` property of the `favorite_book` variable, which stores the user's favorite book.  However, if `favorite_book` doesn't exist, the `view` cannot display the title, so it defaults to `UNKNOWN`:

```erb
<h2>
  My Favorite Book: 
  <% if favorite_book %>
    <%= favorite_book.title %>
  <% else %>
    UNKNOWN
  <% end %>
</h2>
```

There are two issues going on here.  The first is that there is application logic occurring within the `view`, which can be difficult to manage as the application grows, not to mention a nightmare to debug.  The second issue is that we're effectively using a constant, hard-coded value of `UNKNOWN`, which is never a good idea.

To improve this simple example we'll move the majority of the logic out of the `view` file and into a `controller` -- in this case, as a `helper_method`.  Normally this method shouldn't be in the `ApplicationController`, but we're throwing it in there for the sake of this example.  Either way, we can now perform logic to check if the `favorite_book` variable exists:

```ruby
# book.rb
class Book < ApplicationRecord
  DEFAULT_TITLE = 'UNKNOWN'.freeze
end

# application_controller.rb
class ApplicationController < ActionController::Base
  protect_from_forgery with: :exception

  helper_method :favorite_book

  def favorite_book
    @favorite_book ||= Book.find session[:favorite_book_id] if session[:favorite_book_id]
    if @favorite_book
      @favorite_book
    else
      Book.new(title: Book::DEFAULT_TITLE)
    end
  end
end
```

Now we have the `#favorite_book` helper method that attempts to find the `Book` in the database based on the `session[:favorite_book_id]` value, if it exists.  If not, a new `Book` instance is created and the `title` is set to the default value of `UNKNOWN`.  With the logic moved out of the `view`, our `views/books/index.html.erb` code has been dramatically simplified:

```erb
<h2>My Favorite Book: <%= favorite_book.title %></h2>
```

Now we don't need to worry about whether the user has already picked a favorite book or not inside our `view`.  Instead, we just access the `title` property no matter what, and the appropriate value is returned regardless.

## Improper Predicate Method Usage

Another common mistake during Rails development is _improperly using predicate methods_.  For those unaware, a [`predicate method`](http://ruby-for-beginners.rubymonstas.org/objects/predicates.html) is any method that syntactically ends with a question mark (`?`) and that should return _only_ a `truthy` result.  A `truthy` result is commonly known as `true` or `false`, but it's important to understand the slight difference in some potential values.  Specifically, both `false` and `nil` are considered `truthy-false` values in Ruby, meaning both will effectively be considered a false or negative result when returned by a predicate method.  _Any other value_ will be considered `truthy-true`, which is effectively indicates `true` or a positive result.

When creating your own `predicate methods`, it's important to understand their main purpose.  To that end, a predicate method should _never_ perform any action or alter any data.  For example, this predicate method determines if the favorite book is currently within the library, and, _if not_, it changes the `book.library` property to `true`:

```ruby
def favorite_book_in_library?
  book = favorite_book
  # Confirm that book is in library.
  unless book.library
    # If not, add library.
    book.library = true
    true
  end
end
```

This is a direct manipulation of data that should never occur in a `predicate method`.  Instead, the predicate should only examine existing data and return a boolean result.  In the case above, we should remove the `book.library` assignment and return the result of the property check.  We can accomplish this with a single line:

```ruby
# Check if favorite book is in library.
def favorite_book_in_library?
  return favorite_book.library
end
```

## Obese Models

The phrase "fat model, skinny controller" is a well-known and often divisive best practice.  It suggests that your `models` should be sizeable and contain the majority of your domain logic, while your `controllers` should remain small and only perform the basic functionality of linking `models` and `views`.  However, as Rails has matured and been used for larger and more complex applications, the notion that a `fat model` is always the proper avenue to take has fallen by the wayside.

`ActiveRecord` (which is now technically `ApplicationRecord`, which extends `ActiveRecord`) is the base class for all `models` in Rails.  The purpose of `ActiveRecord` is to closely map to the underlying data objects in a persistent storage medium; most commonly a database.  In fact, `ActiveRecord` is itself an implementation of an `object relational mapping` (`ORM`) system.

As your application grows in size and complexity, it can be all too easy to throw any and all business logic into the `models` and call it a day.  Obviously, you don't want to put extra logic into the `controllers` or `views`, so the `model` is the most obvious dumping ground for most newer Rails developers.  However, given the tight integration between the data layer and your `models`, its easy to quickly fall into the trap of _enormous_ `models` that go well beyond the "single responsibility" principle they should focus on.  After all, every `model` is created and named after a corresponding data table in the database, so when a `model` extends its functionality and logic outside the scope of that particular domain, things start to get messy.

The question then remains: _How do we avoid obese `models`?_  There are a few schools of thought, but the simplest answer is to vigilantly refactor your existing `models` to keep them thin and relevant to their singular responsibility.  This can be accomplished through the extraction and creation of a number of secondary object types:

- `Value Objects` - Immutable classes that are compared using their _value_ rather than their _identity_.
- `Service Objects` - Classes that encompass complex behaviors.  `Service objects` often make use of multiple `models` at once, lending themselves to the notion that no _single_ `model` should "own" this logic or behavior.
- `Form Objects` - Classes which manipulate multiple `models` when a user submits a form.  These are often referred to as `presenter objects`.
- `Query Objects` - Classes that handle complicated database queries.

There are many more options available to trim down extraordinarily fat `models`.  In fact, we'll go into much greater detail and provide code examples in a future article dedicated to refactoring obese models in Rails, so be on the lookout for that!

## Extreme Reliance on Gems

Most Rails developers have gone through an all-too-familiar experience at some point in their career.  As you're getting into Rails and discovering all the magical things it can do, you find the [plethora of gems](https://rubygems.org/) and their rich ecosystem, which supports millions of developers around the world.  Gems can accomplish so much, with so little effort.  Need to solve a particular problem?  Chances are there's already a gem for that.  But beware!  The call of the RubyGems repository is a dangerous siren song if not properly considered and managed.

The biggest potential drawback when adding a new gem to your application is application-breaking bugs.  While smart developers will only use the most stable and well-tested gems, even those can cause unexpected behaviors or have their own bugs that slip through the cracks.

Putting aside the potential that the gem itself is flawed in some way, adding a gem to perform a task is often more resource intensive than if you were to code the solution yourself.  A gem that can help your application perform one task is often going to provide additional functionality (and thus, resource usage) that your application simply doesn't need.  For example, if you need an easy way to parse some XML data you might consider the popular [`nokogiri`](https://rubygems.org/gems/nokogiri) gem, which is a powerful parsing API that has been around for many years now.  While your application may only ever use a small portion of a gem's full power, you're often still loading the entirety of the gem and uses resources that you could be saving or using elsewhere.

Lastly, when your `Gemfile` indicates a new gem dependency required by your application, that gem itself often includes gem dependencies of its own, which may, in turn, have their own dependencies, and so on.  Again, the better gems will avoid dependencies as much as possible, but if you aren't particularly careful, your application can quickly grow from a couple dozen base gems for most Rails applications to hundreds in a very short period of time.

The bottom line is, think carefully before adding gems that you may not need.  Whenever possible, roll your own coded solution to perform tasks that are specific to your business requirements.  This will give you better control over how those tasks are performed, while also dramatically improving debugging capabilities when something (invariably) goes wrong.

## Directly Calling External Services

Another common mistake when first getting into Rails developer is _making direct application calls to third-party services_.  For example, when a new user signs up and provides their telephone number, your application may use a third-party service, like [`Twilio`](https://www.twilio.com/), to automatically text the user a multi-factor authentication confirmation code.  The relevant code might look something like this `#send_authentication_code` method:

```ruby
require 'twilio-ruby'

def send_authentication_code(number, code)
  account_sid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" # Your Account SID from www.twilio.com/console
  auth_token = "your_auth_token"   # Your Auth Token from www.twilio.com/console

  @client = Twilio::REST::Client.new account_sid, auth_token
  message = @client.messages.create(
    body: code,
    to: number,
    from: ENV['TWILIO_NUMBER'])

  puts message.sid
end
```

If the `#send_authentication_code` method is directly called in your application, you risk your application hanging while the third-party service is contacted and the request is executed.

To avoid potentially hanging your application while accessing outside services, it's best to integrate a job system that allows a queue to store and execute work behind the scenes, independent of your main application threads.  In fact, Rails 4.2 introduced the [`ActiveJob`](http://edgeguides.rubyonrails.org/active_job_basics.html) class, which was specifically built for this purpose and can be easily integrated with other popular queuing backends like `Sidekiq`, `Resque`, and more.

Check out <a class="js-cta-utm" href="https://airbrake.io/languages/rails_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-rails">Airbrake's Rails exception handling gem</a>, which simplifies the error-reporting process for all of your Ruby web framework projects, including `Rails`, `Sinatra`, `Rack`, and more.  Built on top of Airbrake's powerful and robust [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, `Airbrake` provides your team with real-time error monitoring and reporting across your entire application.  Receive instant feedback on the health of your application, without the need for user-generated reports or filling out issue tracker forms.  With built-in integration for Ruby web frameworks, `Heroku` support, and even job processing libraries like `ActiveJob`, `Resque`, and `Sidekiq`, `Airbrake` can be integrated into your application and begin revolutionizing your debugging workflow in just a few minutes.  Have a look below at all the features packed into `Airbrake` and start improving your exception handling practices today!

---

__META DESCRIPTION__


A handful of 5 common mistakes in rails development for new developers, with code relevant code samples and guidance on avoiding these issues.

---

__SOURCES__

- https://www.toptal.com/ruby-on-rails/top-10-mistakes-that-rails-programmers-make
- https://jetruby.com/expertise/common-rails-mistakes-ruby-way/
- http://blog.codeclimate.com/blog/2012/10/17/7-ways-to-decompose-fat-activerecord-models/
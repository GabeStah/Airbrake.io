# Refactoring Fat Models in Rails

For most Rails developers, a massive benefit of using the Rails framework is the power of its core components, like the dominant [`Active Record`](http://guides.rubyonrails.org/active_record_basics.html).  Acting as the `model` in the core `model-view-controller` paradigm that Rails is built around, according to the [official documentation](http://guides.rubyonrails.org/active_record_basics.html), an `Active Record` is the "layer of the system responsible for representing business data and logic.  Active Record facilitates the creation and use of business objects whose data requires persistent storage to a database."  In other words, an `Active Record` `model` should represent a data layer component (typically a database `table`), and each instance of a particular `model` is a one-to-one representation of an object in the database.  This relationship is typically known as [`Object Relational Mapping`](https://en.wikipedia.org/wiki/Object-relational_mapping) (or `ORM`).

As Uncle Ben (Spider-Man's kin) is oft-quoted, "With great power comes great responsibility."  This is certainly applicable in the nerdy realm of Rails' `models`, because it's all too easy to code with the assumption that all "business data and logic", for which `models` are intended, encompasses anything and everything that doesn't directly relate to the `view` or `controller` components of the `MVC` architecture.  For many developers, particularly those new to the Rails platform, projects can quickly spiral out of control with rapidly growing `models`, as they take on more responsibility as the application scope increases.  Without explicit care and exacting refactoring, you may quickly find you've got one or more `"fat models"` (as they're colloquially known) on your hands.  Like tiny little bulls made up of ones and zeroes, these monstrosities can wreak havoc in the china shop that is your codebase.

In our [5 Common Mistakes in Rails Development](https://airbrake.io/blog/rails/5-common-mistakes-rails-development) article we looked at a handful of issues that frequently trip up both new and seasoned Rails developers.  One topic we briefly discussed is `obese models`, promising a follow-up article taking a deeper dive into this issue and how to manage it.  In today's article we'll tackle just that problem, examining the nature of `fat models`, the goals you should strive for in your own `model` design, and a few tips and tricks to help you refactor `models` that have already grown a little to big for their britches.  Let's get this party started!

## What is a Fat Model?



The phrase "fat model, skinny controller" is a well-known and often divisive best practice.  It suggests that your `models` should be sizeable and contain the majority of your domain logic, while your `controllers` should remain small and only perform the basic functionality of linking `models` and `views`.  However, as Rails has matured and been used for larger and more complex applications, the notion that a `fat model` is always the proper avenue to take has fallen by the wayside.

`ActiveRecord` (which is now technically `ApplicationRecord`, which extends `ActiveRecord`) is the base class for all `models` in Rails.  The purpose of `ActiveRecord` is to closely map to the underlying data objects in a persistent storage medium; most commonly a database.  In fact, `ActiveRecord` is itself an implementation of an `object relational mapping` (`ORM`) system.

As your application grows in size and complexity, it can be all too easy to throw any and all business logic into the `models` and call it a day.  Obviously, you don't want to put extra logic into the `controllers` or `views`, so the `model` is the most obvious dumping ground for most newer Rails developers.  However, given the tight integration between the data layer and your `models`, its easy to quickly fall into the trap of _enormous_ `models` that go well beyond the "single responsibility" principle they should focus on.  After all, every `model` is created and named after a corresponding data table in the database, so when a `model` extends its functionality and logic outside the scope of that particular domain, things start to get messy.

The question then remains: _How do we avoid obese `models`?_  There are a few schools of thought, but the simplest answer is to vigilantly refactor your existing `models` to keep them thin and relevant to their singular responsibility.  This can be accomplished through the extraction and creation of a number of secondary object types:

- `Value Objects` - Immutable classes that are compared using their _value_ rather than their _identity_.
- `Service Objects` - Classes that encompass complex behaviors.  `Service objects` often make use of multiple `models` at once, lending themselves to the notion that no _single_ `model` should "own" this logic or behavior.
- `Form Objects` - Classes which manipulate multiple `models` when a user submits a form.  These are often referred to as `presenter objects`.
- `Query Objects` - Classes that handle complicated database queries.

There are many more options available to trim down extraordinarily fat `models`.  In fact, we'll go into much greater detail and provide code examples in a future article dedicated to refactoring obese models in Rails, so be on the lookout for that!





Check out <a class="js-cta-utm" href="https://airbrake.io/languages/rails_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-rails">Airbrake's Rails exception handling gem</a>, which simplifies the error-reporting process for all of your Ruby web framework projects, including `Rails`, `Sinatra`, `Rack`, and more.  Built on top of Airbrake's powerful and robust [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, `Airbrake` provides your team with real-time error monitoring and reporting across your entire application.  Receive instant feedback on the health of your application, without the need for user-generated reports or filling out issue tracker forms.  With built-in integration for Ruby web frameworks, `Heroku` support, and even job processing libraries like `ActiveJob`, `Resque`, and `Sidekiq`, `Airbrake` can be integrated into your application and begin revolutionizing your debugging workflow in just a few minutes.  Have a look below at all the features packed into `Airbrake` and start improving your exception handling practices today!

---

__META DESCRIPTION__


A handful of 5 common mistakes in rails development for new developers, with code relevant code samples and guidance on avoiding these issues.

---

__SOURCES__

- http://guides.rubyonrails.org/active_record_basics.html
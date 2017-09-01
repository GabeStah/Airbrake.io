# Top Tips for Refactoring Fat Models in Rails

For most Rails developers, a massive benefit of using the Rails framework is the power of its core components, like the dominant [`Active Record`](http://guides.rubyonrails.org/active_record_basics.html).  Acting as the `model` in the core `model-view-controller` paradigm that Rails is built around, according to the [official documentation](http://guides.rubyonrails.org/active_record_basics.html), an `Active Record` is the "layer of the system responsible for representing business data and logic.  Active Record facilitates the creation and use of business objects whose data requires persistent storage to a database."  In other words, an `Active Record` `model` should represent a data layer component (typically a database `table`), and each instance of a particular model is a one-to-one representation of an object in the database.  This relationship is typically known as [`Object Relational Mapping`](https://en.wikipedia.org/wiki/Object-relational_mapping) (or `ORM`).

As Uncle Ben (Spider-Man's very own kin) is oft-quoted, "With great power comes great responsibility."  This is certainly applicable in the nerdy realm of Rails' models, because it's all too easy to write code with the assumption that the notion of all "business data and logic", for which models are intended, encompasses anything and everything that doesn't directly relate to the `view` or `controller` components of the `MVC` architecture.  For many developers, particularly those new to the Rails platform, projects can quickly spiral out of control with rapidly growing models, taking on more and more responsibility as the application scope increases.  Without explicit care and precise refactoring, you may quickly find you've got one or more `"fat models"` (as they're colloquially known) on your hands.  Like tiny little bulls made up of ones and zeroes, these monstrosities can wreak havoc in the china shop that is your codebase.

In our [5 Common Mistakes in Rails Development](https://airbrake.io/blog/rails/5-common-mistakes-rails-development) article, we looked at a handful of issues that frequently trip up both new and seasoned Rails developers.  One topic we briefly discussed is `obese models`, promising a follow-up article taking a deeper dive into this issue and how to manage it.  In today's article we'll tackle just that problem, examining the nature of `fat models`, the goals you should strive for in your own model design, and a few tips and tricks to help you refactor models that have already grown a little to big for their britches.  Let's get this party started!

## What Makes a Model Fat?

There are many indicators that your models have grown too big, but the most indicative is when your model breaks the [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle).  First introduced by software engineer and author [Robert Cecil Martin](https://en.wikipedia.org/wiki/Robert_Cecil_Martin) in his book [_Agile Software Development, Principles, Patterns, and Practices_](https://www.amazon.com/Software-Development-Principles-Patterns-Practices/dp/0135974445), the `single responsibility principle` states that every class in an application should have responsibility over _one, and only one_ aspect of the overall software.  Martin defines "responsibility" as a "reason to change," indicating that each class should only ever have one reason to change.

For example, imagine a class that handles sending out email confirmations to users after sign up.  This class might include just a few fields like `email` and `content`.  Perhaps, when initially creating this class, it was decided that each user has a unique email address, so the `email` field (which is a `string)` is sufficient.  However, later down the road, there's eventually a need to associate this class with more complex relationships, because, perhaps users can register through some identifier _besides_ their email address.

Now we're in a situation where this simple class intended to send out email confirmations actually has _more than one_ responsibility or reason to change.  One possible reason is when emails need to be sent out in different ways, perhaps based on their frequency, recipients, or email protocol type.  Another reason to change is when we want to move from the old `email` identification to a more robust means, such as an association with a `User` class object found elsewhere in the application.  This will allow users to be contacted not just by email, but by text message or otherwise.

Anyway, we could get into a deep rabbit hole of theoretical examples, but the same principles apply: If a model has grown to the point that some part of it is handling multiple responsibilities, it's best to try to refactor it and trim it down so it focuses solely on its single, independent job.

## Extract Value Objects

A `value object` is an immutable class that is compared using its _value_ rather than its _identity_.  Common examples of `value objects` include numbers, dates, monetary values, and strings.  For example, a `penny` is a `value object`.  I can have five unique copies of a `penny` in my pocket at once, but their usefulness as currency is not dependant on their unique identification.  Instead, they are identified based on their `value` as a representation of one cent.  Similarly, the string `"ABC"` is _equal to_ a second copy of the string `"ABC"`.  Even though these are two unique objects, their equivalence is always based on their `value`, rather than their identify.

As it happens, looking for `value objects` within your model is a great way to refactor and slim things down.  For example, consider an application with a `Book` class.  We allow users to rate a `Book` from 1 to 5, and we keep track of the total number of ratings in an array.  We can then retrieve the current `#rating` by averaging all the values thus far:

```ruby
class Book
  def initialize(title, author, ratings)
    @author = author
    @ratings = ratings
    @title = title
  end

  def add_rating(rating)
    @ratings.push(rating)
  end

  def rating
    # Average ratings.
    @ratings.inject{:+}.to_f / @ratings.size
  end
end
```

While this method works, its rather limiting.  What if we want a more complex form of rating?  What if we need to perform additional logic based on the ratings that were provided?  We can accomplish these goals, while also slimming down our primary `Book` class, by extracting the rating-related methods and placing them into their own `value object` class, `Rating`:

```ruby
class Rating
  def initialize(value)
    @value = value || 1
  end

  def <=>(other)
    other.to_s <=> to_s
  end

  def eql?(other)
    to_s == other.to_s
  end

  def hash
    @value.hash
  end

  def to_s
    @value.to_s
  end
end
```

Now the `Book` class can reference the `Rating` class, rather than just a plain integer value, and we can perform more advanced logic within `Rating` as needed.

## Define Service Objects

Another useful type of class to create, particularly as part of model refactoring, is a `service object`.  A `service object` is a class that encompasses complex behaviors.  `Service objects` often make use of multiple `models` at once, lending themselves to the notion that no _single_ `model` should "own" this particular logic or behavior.

For example, consider an infrequent maintenance activity like cleaning up the `Book` records in the database every quarter.  Perhaps the system should look at all `Books` and determine if their `Ratings` are below a certain threshold.  If not, such `Books` should no longer be featured elsewhere in the application.  While this code _could_ be placed inside the `Book` model, it wouldn't make logical sense because the `Book` class represents a single instance of a book, rather than the whole collection of books.  Therefore, this is a perfect scenario in which to use a `service object` class:

```ruby
class BookCleanup
  def initialize
  end

  # Remove featured flag from all books with rating below 3.
  def cleanup
    Book.where('rating < ?', 3).update_all(featured: false)
  end
end
```

As a simple example, here the `BookCleanup` `service object` is used to change the `featured` flag to `false` for all `Books` with `Ratings` below `3`.

## Create Form Objects

Somewhat similar to a `service object`, a `form object` is ideal when handling a form request that manipulates multiple models at once.  For example, when a `User` submits a rating on a book, this (may) require manipulation of the `Book` model and the `User` model.  While we _could_ choose to handle this within either the `Book` or the `User` model, doing either would break the `single responsibility principle` we're aiming for, so creating a separate `form object` class to handle this update is ideal:

```ruby
class RatingForm
  extend ActiveModel::Naming
  include ActiveModel::Conversion
  include ActiveModel::Validations

  attribute :user, Integer
  attribute :book, Integer
  attribute :rating, Integer

  validates :book,
            presence: true
  validates :rating,
            presence: true,
            numericality: {
                only_integer: true,
                greater_than_or_equal_to: 1,
                less_than_or_equal_to: 5
            }

  def persisted?
    false
  end

  def save
    if valid?
      persist!
      true
    else
      false
    end
  end

  private

    def persist!
      rating = Rating.new(:rating)
      user = User.find(:user)
      book = Book.find(:book)
      book.add_rating!(rating, user)
      user.add_rating!(rating, book)
    end
end
```

There are many more options available to trim down extraordinarily fat `models`, but hopefully this brief look at a few critical tips and ways to slim down these models can help you in your own future Rails projects.

Check out <a class="js-cta-utm" href="https://airbrake.io/languages/rails_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-rails">Airbrake's Rails exception handling gem</a>, which simplifies the error-reporting process for all of your Ruby web framework projects, including `Rails`, `Sinatra`, `Rack`, and more.  Built on top of Airbrake's powerful and robust [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, `Airbrake` provides your team with real-time error monitoring and reporting across your entire application.  Receive instant feedback on the health of your application, without the need for user-generated reports or filling out issue tracker forms.  With built-in integration for Ruby web frameworks, `Heroku` support, and even job processing libraries like `ActiveJob`, `Resque`, and `Sidekiq`, `Airbrake` can be integrated into your application and begin revolutionizing your debugging workflow in just a few minutes.  Have a look below at all the features packed into `Airbrake` and start improving your exception handling practices today!

---

__META DESCRIPTION__


A collection of tips and tricks to help you with refactoring fat models in rails to ensure they live up to the single responsibility principle.

---

__SOURCES__

- http://guides.rubyonrails.org/active_record_basics.html
- http://blog.codeclimate.com/blog/2012/10/17/7-ways-to-decompose-fat-activerecord-models/
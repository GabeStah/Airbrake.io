# Even More Tips for Refactoring Rails Models

There are many indicators that your models have grown too big, but the most indicative is when your model breaks the [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle).  First introduced by software engineer and author [Robert Cecil Martin](https://en.wikipedia.org/wiki/Robert_Cecil_Martin) in his book [_Agile Software Development, Principles, Patterns, and Practices_](https://www.amazon.com/Software-Development-Principles-Patterns-Practices/dp/0135974445), the `single responsibility principle` states that every class in an application should have responsibility over _one, and only one_ aspect of the overall software.  Martin defines "responsibility" as a "reason to change," indicating that each class should only ever have one reason to change.

In our [Top Tips for Refactoring Fat Models in Rails](https://airbrake.io/blog/rails/top-tips-for-refactoring-rails-models) article we looked at a handful of suggestions for cleaning up and streamlining bloated models.  Today, we'll be looking at _even more_ tips for refactoring Rails models, so let's jump right in!

## Create View Objects

In many cases, your Rails application may need to perform some logic to determine what `views` and other UI elements are active for the user.  However, while it's relatively easy to start throwing such logic directly into `.erb` files, refactoring this logic into `view objects` can save a lot of time and headaches down the road.

For example, consider a scenario where our application needs to present a series of navigation menu links (i.e. `tabs`) to the user.  The initial implementation might look something like this, within the `header.html.erb` view:

```erb
<!-- header.html.erb -->
<div class="tabs header-tabs">
  <%= link_to 'Home', root_path, class: "tabs--link #{'is-active' if is_tab_active?(:index)}" %>
  <% if can? :view, Book %>
    <%= link_to 'Books', books_path, class: "tabs--link #{'is-active' if is_tab_active?(:book)}" %>
  <% end %>
  <%= link_to 'About', about_path, class: "tabs--link #{'is-active' if is_tab_active?(:about)}" %>
  <%= link_to 'Contact', contact_path, class: "tabs--link #{'is-active' if is_tab_active?(:contact)}" %>
</div>
```

We're using the popular and powerful [`CanCan`](https://github.com/CanCanCommunity/cancancan) gem to make it easy to check permissive behaviors, but doing so also adds some additional logic within our view.  We also reference the same `ApplicationHelper#is_tab_active?` method numerous times:

```ruby
# application_helper.rb
module ApplicationHelper
  def is_tab_active?(tab)
    case tab
      when :about
        return true if controller_name == 'about'
      when :book
        return true if controller_name == 'books'
      when :index
        return true if controller_name == 'index'
      when :contact
        return true if controller_name == 'contact'
      else
        false
    end
  end
end
```

Arguably, this is maintainable code, even with the no-no of including `CanCan` logic in the `header` view.  However, this code is also far from beautiful and will eventually lead to problems down the road.  What if we want to add additional tabs that are found in a different `view`?  Maybe administrators on the site need to view some different tabs, which are created within the `admin.html.erb` view:

```erb
<!-- admin.html.erb -->
<div class="tabs admin-tabs">
  <% if can? :edit, Profile %>
    <%= link_to 'Edit Profile', edit_profile_path, class: "tabs--link #{'is-active' if is_tab_active?(:profile)}" %>
  <% end %>
  <% if can? :edit, Setting %>
    <%= link_to 'Settings', edit_setting_path, class: "tabs--link #{'is-active' if is_menu_tab_active?(:settings)}" %>
  <% end %>
</div>
```

Now, not only do we have to modify two different `.erb` view files when changing tabs, but the HTML structure of how tabs are created (i.e. `<div class="tabs tab-XYZ">`) is spread over multiple files as well.

The solution is to create our own `view object`, which is just a plain old Ruby object that acts as an `interface`, which can be extended through other classes to handle the actual view-based logic we need.

```rb
# view_objects/view_object.rb
class ViewObject
  attr_reader :context

  include Rails.application.routes.url_helpers
  include ActionView::Helpers
  include ActionView::Context

  # CanCan integration.
  include CanCan::ControllerAdditions
  delegate :current_ability, :to => :context

  def initialize(context, args = {})
    @context = context
    after_init(args)
  end

  def after_init(args = {})
  end
end
```

Now, let's create a generic `Tabs` view object, which can be inherited by specific implementations (i.e. application components).

```rb
# view_objects/tabs.rb
class Tabs < ViewObject
  def html
    content_tag :div, tabs.join('').html_safe, class: 'tabs'
  end

  private
    # Interfaced.
    def tabs
      fail 'must be implemented by subclass'
    end

    # Interfaced.
    def active?(tab)
      fail 'must be implemented by subclass'
    end

    # Gets formatted tab link CSS.
    def tab_class(tab)
      active_class = active?(tab) ? 'is-active' : nil
      ['tabs--link', active_class].compact.join(' ')
    end

    # Get full tab link_to.
    def tab(text, path, tab)
      link_to text, path, class: tab_class(tab)
    end
end
```

```rb
# view_objects/tabs/header_tabs.rb
class HeaderTabs < Tabs
  private

    def tabs
      [about_tab, book_tab, contact_tab, index_tab].compact
    end

    def active?(tab)
      case tab
        when :about
          return true if controller_name == 'about'
        when :book
          return true if controller_name == 'books'
        when :contact
          return true if controller_name == 'contact'
        when :index
          return true if controller_name == 'index'
        else
          false
      end
    end

    def about_tab
      tab('About', about_path, :about)
    end

    def book_tab
      # Perform privilege check inside method.
      return nil unless can?(:edit, Book)
      tab('Books', books_path, :book)
    end

    def contact_tab
      tab('Contact', contact_path, :contact)
    end

    def index_tab
      tab('Home', root_path, :index)
    end
end
```

While the number of lines of code has slightly increased from the original implementation, we've dramatically reduced the complexity of how navigation tabs are handled in the codebase.  We can now simply invoke the appropriate `view object` class in the actual view.  For example, the `header.html.erb` code goes from this:

```erb
<!-- header.html.erb -->
<div class="tabs home-tabs">
  <%= link_to 'Home', root_path, class: "tabs--link #{'is-active' if is_menu_tab_active?(:index)}" %>
  <% if can? :view, Book %>
    <%= link_to 'Books', books_path, class: "tabs--link #{'is-active' if is_menu_tab_active?(:book)}" %>
  <% end %>
  <%= link_to 'About', about_path, class: "tabs--link #{'is-active' if is_menu_tab_active?(:about)}" %>
  <%= link_to 'Contact', contact_path, class: "tabs--link #{'is-active' if is_menu_tab_active?(:contact)}" %>
</div>
```

... to this:

```erb
<!-- header.html.erb -->
<%= HeaderTabs.new.html %>
```

Plus, encapsulating all the logic into separate `view objects` makes it _much_ easier to modify existing tabs in the future, or even add new sections.  For example, to add the `admin` tabs section we just need to implement the base `Tabs` interface:

```rb
# view_objects/tabs/admin_tabs.rb
class AdminTabs < Tabs
  private

    def tabs
      [profile_tab, setting_tab].compact
    end

    def active?(tab)
      case tab
        when :profile
          return true if controller_name == 'profile'
        when :setting
          return true if controller_name == 'setting'
        else
          false
      end
    end

    def profile_tab
      # Check if user can edit Profile.
      return nil unless can?(:edit, Profile)
      tab('Edit Profile', edit_profile_path(context.current_user), :profile)
    end

    def book_tab
      # Check if user can edit Settings.
      return nil unless can?(:edit, Setting)
      tab('Settings', edit_setting_path, :setting)
    end
end
```

```erb
<!-- admin.html.erb -->
<%= AdminTabs.new.html %>
```

## Extract Policy Objects

As you may recall, in our [previous article with refactoring fat models in Rails](https://airbrake.io/blog/rails/top-tips-for-refactoring-rails-models) we discussed creating `service objects`.  A `service object` is essentially a class that encompasses complex behaviors, typically across multiple models.  More importantly, `service objects` inherently deal directly with data (i.e. `ActiveRecord` objects), so invoking a service object will usually force actual changes to the database.  However, there are some scenarios where you may not need to _change_ data, but must still invoke multiple models or complex behavior to retrieve some particular information.  For such scenarios, a `policy object` is ideal.

A `policy object` can be extracted from your application code by identifying anywhere where one or more business rules are used to determine something about data that is _already in memory_.  For example, consider the following snippet from our previous article, in which we created a simple `service object` called `BookCleanup` that updates the `featured` flag of all `Books` that have a low `rating` score:

```rb
class BookCleanup
  def initialize
  end
 
  # Remove featured flag from all books with rating below 3.
  def cleanup
    Book.where('rating < ?', 3).update_all(featured: false)
  end
end
```

Clearly, we can see that `update_all(featured: false)` causes this `#cleanup` method to modify the database.  However, what if we just wanted to determine if a particular `Book` is considered "highly rated" (i.e. it has an average `rating` score of at least `4.5`)?  We might implement a `BookPolicy` object:

```rb
class BookPolicy
  def initialize(book)
    @book = book
  end

  def highly_rated?
    @book.featured? && @book.rating >= 4.5
  end
end
```

Now, wherever we need to check if a `Book` is highly rated, we can create a `BookPolicy` instance and invoke the `#highly_rated?` method.  Moreover, we can expand the functionality of the `BookPolicy` object when we want to include additional checks, such as if a `Book` was published in the last year:

```rb
class BookPolicy
  def initialize(book)
    @book = book
  end

  def highly_rated?
    @book.featured? && @book.rating >= 4.5
  end

  def published_within_last_year?
    @book.published_on >= (DateTime.now - 1.year)
  end
end
```

## Decorate with Decorators

The last refactoring technique we'll cover today is making `decorators` out of existing callbacks.  A `decorator object` essentially allows you to modify or "add onto" the behavior of existing objects, such as `ActiveRecord` models, without adjusting the behavior of outside code.  Extracting decorators from existing code is typically useful when your model has been given too much responsibility, or where some behavior must only execute under certain circumstances.

For example, consider what happens when a user creates a new `Book` in our book-based application.  We might want to trigger a tweet on `Twitter` on the user's behalf, letting their friends know they just read the newly-added `Book`.  Such logic doesn't belong in the `Book` model, since `Twitter` handling falls outside its purview.  However, a `decorator` object is perfect for this scenario:

```rb
class TwitterBookNotifier
  def initialize(book)
    @book = book
  end

  def save
    @book.save && tweet
  end

private

  def tweet
    Twitter.tweet(text: "I just finished reading '#{@book.title}' by #{@book.author}!")
  end
end
```

Here we've created the `TwitterBookNotifier` class.  As you can see, it's quite simple.  It expects a `Book` argument passed during initialization, and it provides a `#save` method, which is similar to the normal `Book#save` method we'd be using.  However, in addition to calling `#save` on the passed `Book` instance, it also invokes the `#tweet` method, which connects to the Twitter API and sends out a tweet.

Now, to make use of the `TwitterBookNotifier` object we can use it in our controller, just as we'd use an actual `Book` instance that is being created and saved:

```rb
class BooksController < ApplicationController
  # ...

  def create
    @book = TwitterBookNotifier.new(Book.new(book_params))

    if @book.save
      redirect_to root_path, notice: "Your read book has been saved."
    else
      render "new"
    end
  end

  # ...
end
```

There we have it!  A handful of cool, new tips for refactoring rails models! Also, be sure to check out <a class="js-cta-utm" href="https://airbrake.io/languages/rails_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-rails">Airbrake's Rails exception handling gem</a>, which simplifies the error-reporting process for all of your Ruby web framework projects, including `Rails`, `Sinatra`, `Rack`, and more.  Built on top of Airbrake's powerful and robust [`Airbrake-Ruby`](https://github.com/airbrake/airbrake-ruby) gem, `Airbrake` provides your team with real-time error monitoring and reporting across your entire application.  Receive instant feedback on the health of your application, without the need for user-generated reports or filling out issue tracker forms.  With built-in integration for Ruby web frameworks, `Heroku` support, and even job processing libraries like `ActiveJob`, `Resque`, and `Sidekiq`, `Airbrake` can be integrated into your application and begin revolutionizing your debugging workflow in just a few minutes.  Have a look below at all the features packed into `Airbrake` and start improving your exception handling practices today!

---

__META DESCRIPTION__


Even more tips for refactoring rails models, with sample code to help you ensure your Rails application is as slim and robust as it can possibly be.

---

__SOURCES__

- https://launchpadlab.com/blog/view-objects-in-ruby-on-rails
- http://guides.rubyonrails.org/active_record_basics.html
- http://blog.codeclimate.com/blog/2012/10/17/7-ways-to-decompose-fat-activerecord-models/
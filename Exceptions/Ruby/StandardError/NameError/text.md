# Ruby Exception Handling: NameError

Making our way through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll take a dive into Ruby's `NameError`.  Simply put, the `NameError` is raised when a provided variable name or symbol is invalid or undefined.

We'll take some time throughout this article to examine the `NameError` in more detail, exploring where it resides within the Ruby `Exception` class hierarchy, as well as checking out some sample code to see how `NameErrors` might occur in your own Ruby coding endeavors, so let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `NameError` is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class.

## When Should You Use It?

There are only a couple situations where a `NameError` might occur when running your own Ruby code.  The first is simply when trying to call an undefined variable or symbol name which hasn't been previously declared.  As an example, below we have an `invalid_name_example` method that attempts to output the value of `title`, which has not been declared:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    invalid_name_example
    valid_name_example
    invalid_constant_example
    valid_constant_example
end

def invalid_name_example
    begin
        # Output a title value which is undeclared.
        puts title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

# Execute all examples.
execute_examples
```

This is not allowed, and thus Ruby raises a `NameError` for our troubles:

```
[EXPLICIT] NameError: undefined local variable or method `title' for main:Object
```

If we alter the code slightly, as we've done for `valid_name_example`, we can initially declare the `title` variable prior to calling it:

```ruby
def valid_name_example
    begin
        title = 'The Stand'
        # Output a title value, after declaration.
        puts title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end
```

This outputs the expected `title` value of: `The Stand`

It's also possible to raise a `NameError` when attempting to assign a `constant` value that does not __begin with__ a capital letter.  This is because Ruby requires all `constants` to at least [_begin_ with a capital letter](http://rubylearning.com/satishtalim/ruby_constants.html).  It is common practice to name most `constants` entirely in uppercase using underscore word separators.  In our `invalid_constant_example` method below we're calling the [`const_set`](https://ruby-doc.org/core-2.4.0/Module.html#method-i-const_set) method, which sets the name of a constant to the given value:

```ruby
def invalid_constant_example
    begin
        # Assign an invalid (lowercase) constant value of :title.
        String.const_set :title, 'The Shining'
        puts String::title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

In this case, we're trying to declare and assign the constant `String::title` to a value of `The Shining`.  However, Ruby doesn't care for this and raises another `NameError`, since our constant must begin with an uppercase letter:

```
[EXPLICIT] NameError: wrong constant name title
```

The easy fix, of course, is to change our constant name to either `Title` or `TITLE`.  Since the latter is the common convention, we'll use `TITLE`:

```ruby
def valid_constant_example
    begin
        # Assign an valid constant value of :title.
        String.const_set :TITLE, 'The Shining'
        puts String::TITLE
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end
```

Sure enough, this works just fine and outputs our new `String::TITLE` constant value of `The Shining` to the console:

```
The Shining
```

Lastly, while related naming issues will not _directly_ produce a `NameError` that we can rescue, in the wonderful world of Ruby, it happens that all `class` names are _also_ constants as well.  This means that, when declaring a class, the class name must also begin with an uppercase letter.  Unlike value-constants, it is common practice for Ruby class name constants to be CamelCase.

For example, here we've declared a new class named `author` which doesn't begin with the required uppercase letter:

```ruby
class author
    def name
        puts 'Stephen King'
    end
end
```

The Ruby parser will catch this issue immediately upon execution, rather than stepping through any other code, since this is basically a syntax error.  Therefore, while not directly raising a `NameError`, Ruby reports the issue to the console during execution:

```
class/module name must be CONSTANT
```

The simple fix is to change the class name from `author` to `Author`, and all is well:

```ruby
class Author
    def name
        puts 'Stephen King'
    end
end
puts Author.new.name
```

As expected, this class definition is just fine, so our `puts` statement outputs the `name` of our `Author`: `Stephen King`

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close look at the NameError in Ruby, with code examples and a short glimpse at the proper declaration syntax for Ruby constants.
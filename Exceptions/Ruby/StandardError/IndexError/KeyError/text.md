# Ruby Exception Handling: KeyError

Next up in our deep dive through the __Ruby Exception Handling__ series we'll be dog-paddling our way around the `KeyError`.  The `KeyError` is a descendant of the `IndexError`.  Unlike the `IndexError`, which deals with `Arrays`, the `KeyError` deals with `Hashes` instead.  Specifically, a `KeyError` occurs when a reference to a `key` within a hash is invalid or missing.

Throughout today's article we'll examine exactly what the `KeyError` means, see where it fits within the Ruby `Exception` class hierarchy, and analyze how to handle `KeyErrors` when they're raised, so let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- [`IndexError`](https://ruby-doc.org/core-2.3.3/IndexError.html) is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.3.3/StandardError.html) class, and is a superclass to a few descendants of its own.
- `KeyError` is a direct descendant of the [`IndexError`](https://ruby-doc.org/core-2.3.3/IndexError.html).

## When Should You Use It?

Unlike the `IndexError` that is associated with improper use of `Arrays`, the `KeyError` is associated with improper use of `Hashes`.  A Ruby [`Hash`](https://docs.ruby-lang.org/en/2.4.0/Hash.html) is effectively a `dictionary` object that contains a series of `key/value` pairs.  This data structure is also commonly referred to as an `associative array`.

Just as with `Arrays`, a `Hash` can be created in a number of ways:

```ruby
# Implicit and inline.
book = {
    "title" => "The Stand",
    "author" => "Stephen King",
    "page_count" => 823,
}

# Alternative symbol syntax for keys.
book = {
    title: "The Stand",
    author: "Stephen King",
    page_count: 823,
}

# Using Hash.new for initialization, then inline key/value assignment.
book = Hash.new
book[:title] = "The Stand"
book[:author] = "Stephen King"
book[:page_count] = 823
```

Also like `Arrays`, there are [many methods](https://docs.ruby-lang.org/en/2.4.0/Hash.html) that can be used to manipulate our `Hash` once initialized.  We can use similar syntax to that of assignment to also retrieve values:

```ruby
book = {
    title: "The Stand",
    author: "Stephen King",
    page_count: 823,
}

puts book[:title] #=> "The Stand"
puts book["author"] #=> "Stephen King"
puts book.fetch(:page_count) #=> 823
```

As with the `Array.fetch` method, we can also call the [`Hash.fetch`](https://docs.ruby-lang.org/en/2.4.0/Hash.html#method-i-fetch) method to retrieve a value from our `Hash` by passing the `key` value as the first argument, as seen above.  However, if we pass an invalid `key` to `Hash.fetch`, let's see what happens:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published)
rescue KeyError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

As expected, we raised a `KeyError`, since `:published` was not specified as a `key` in our `Hash`:

```
[EXPLICIT] KeyError: key not found: :published
```

As a workaround, we can pass a second argument to `Hash.fetch`, which specifies a `default value` to return if the `Hash` doesn't contain a matching `key` for the value we provided:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published, Time.now.to_s)
rescue KeyError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Now, rather than raising a `KeyError`, our call to `Hash.fetch` notices a default value of `Time.now.to_s`, and returns that as our output instead:

```
2017-03-16 18:02:14 -0700
```

Finally, just as with `Array.fetch`, we can also initialize our call to `Hash.fetch` with an associated block, which forces Ruby to execute the block code if, and only if, the passed `key` value is invalid:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published) { |key|
        "Key '#{key}' not found."
    }
rescue KeyError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Now, instead of raising a `KeyError`, our output shows we gracefully failed out of that `Hash.fetch` method call:

```
Key 'published' not found.
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A detailed dive into the KeyError in Ruby, a direct descendant of the IndexError class, including a short foray into Ruby Hashes.
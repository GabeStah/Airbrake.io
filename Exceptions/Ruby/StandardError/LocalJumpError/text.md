# Ruby Exception Handling: LocalJumpError

Continuing along through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we're going to take a closer look at the `LocalJumpError`.  Put simply, a `LocalJumpError` is raised when a `yield` call is made inside a method that has code `block` associated with it.

Throughout this article we'll explore the `LocalJumpError` in more detail, looking at where it sits within the Ruby `Exception` class hierarchy, briefly going over Ruby code `blocks` and the `yield` statement, and then looking at actual code to see how a `LocalJumpError` is raised.  Let's get crackin'!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `LocalJumpError` is a direct descendant of the [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) class.

## When Should You Use It?

As mentioned in the introduction, a `LocalJumpError` can only be raised in a very specific scenario: When a `yield` call is made within a method that doesn't have an associated code `block`.  To understand just what that means, let's take a moment to refresh ourselves on what `blocks` are in Ruby, and then we can take a look at how `yield` is used in conjunction with them.

In Ruby, a `block` (also known as a `closure` in some other languages) is a group of statements that can be **associated with** a `method` call.  A code `block` is associated with a method when the `block` immediately follows the method call.  A `block` can be written using one of two possible syntaxes:

```ruby
# Braces syntax:
my_method { ... }
# Braces syntax, multiline:
my_method {
    ...
}

# do...end syntax:
my_method do ... end
# do...end syntax, multiline:
my_method do
    ...
end
```

Generally speaking, most developers use the braces syntax if the `block` is a single statement and can, therefore, easily fit on one line.  On the other hand, the `do...end` syntax is commonly used for multiline `blocks`.

To further illustrate, let's give a real working example of defining a method and then associated a `block` with our method call:

```ruby
# Create a book hash.
book = {
    title: 'The Stand',
    author: 'Stephen King',
    published_at: 1978
}

# Returns the title of the book argument.
def get_title(book)
    puts 'Retrieving title...'
    puts book[:title]
    return book[:title]
end

# Call the get_title method, with an associated code block.
get_title(book) { puts 'Block has been executed!' }
```

As you can see, we created a `book` hash with a handful of values.  We then defined the `get_title` method, which simply returns the `title` value of our hash.  Finally, we make a call to `get_title(book)`, but we've **also** associated a single line `block` with our method call: `{ puts 'Block has been executed!' }`

Let's try running this code and see what happens in the output:

```
Retrieving title...
The Stand
```

As it happens, we only get the `Retrieving title...` indicator, and then output the title that we retrieved with our method call.  Our code `block` is not executed.  Why?  Because we didn't include the `yield` statement anywhere inside our `get_title` method.

In Ruby, the `yield` statement allows you to temporarily `halt` or `step out of` the execution of the inner method code, alternatively stepping into the currently associated code `block` attached to that method.  This has many potential benefits across Ruby, but the most common use is with `Enumerators`, which all use `block` and `yield` to retrieve the current iterative value, no matter how many items or iterations are involved.

To see `yield` in action, let's modify our initial example above by adding a few more lines:

```ruby
# Create a book hash.
book = {
    title: 'The Stand',
    author: 'Stephen King',
    published_at: 1978
}

# Returns the title of the book argument.
def get_title(book)
    puts 'Yielding...'
    # Yields to the code block associated with method call.
    yield book[:author]
    puts 'Retrieving title...'
    puts book[:title]
    return book[:title]
end

# Call the get_title method, with an associated code block.
get_title(book) { puts 'Block has been executed!' }
```

We added two new lines inside our `get_title` method.  The first is an output to indicate that we're calling `yield`, and the second is the `yield` statement itself.  Now our output looks different from before:

```
Yielding...
Block has been executed!
Retrieving title...
The Stand
```

We can now see that calling `yield` did its job.  During execution of `get_title`, in between the `Yielding...` and `Retrieving title...` lines, our execution stepped out due to the `yield` statement, and executed the code `block` that we associated with our method call, which produced the `Block has been executed!` output.

Keen observers may have also noticed that we passed an argument to our `yield` call, in this case `book[:author]`.  We can pass arguments to `yield` calls, which are then passed along to the associated code `block` when it is executed.  We need to define some parameters within our `block` to make use of this, so let's change our method call to the following code:

```ruby
get_title(book) do |author|
    puts "Author is: #{author}"
    puts 'Block has been executed!'
end
```

Now, when we make this call to our `get_title` method, the associated `block` has an `author` parameter that we can call inside the `block` code.  The result of our output shows that this passing of values via `yield` works just fine:

```
Yielding...
Author is: Stephen King
Block has been executed!
Retrieving title...
The Stand
```

Whew!  Now that we've refreshed ourselves on how Ruby `blocks` and `yield` statements work, we can quickly see what causes a `LocalJumpError` in this context.  Simply put, it's when we issue a `yield` statement inside a method, but that method call **doesn't** have a code `block` associated with it.  For example, here we have the same code as before, but we've removed the `block` from our `get_title` method call:

```ruby
begin
    # Create a book hash.
    book = {
        title: 'The Stand',
        author: 'Stephen King',
        published_at: 1978
    }

    # Returns the title of the book argument.
    def get_title(book)
        puts 'Yielding...'
        # Yields to the code block associated with method call.
        yield book[:author]
        puts 'Retrieving title...'
        puts book[:title]
        return book[:title]
    end

    # Call the get_title method, WITHOUT an associated code block.
    get_title(book)
rescue LocalJumpError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
```

Sure enough, we raise a `LocalJumpError` as expected, with the error message explaining problem:

```
Yielding...
[EXPLICIT] LocalJumpError: no block given (yield)
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

An examination of the LocalJumpError in Ruby, including a brief overview of using code `blocks` and the `yield` statement.
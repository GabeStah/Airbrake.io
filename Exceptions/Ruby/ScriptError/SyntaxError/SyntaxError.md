Today we continue our in-depth **Ruby Exception Handling** series with a dive into the `SyntaxError` exception class. `SyntaxError` is a subclass descendant of the `ScriptError` superclass, and pops up anytime Ruby attempts to execute invalid code syntax.

In this piece we'll examine the `SyntaxError` class, looking at exactly where it fits into Ruby's `Exception` class hierarchy, how to deal with `SyntaxErrors`, and the best practices to avoid this exception altogether.  Let's get to it!

# The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- [`ScriptError`] is a direct descendant of the [`Exception`] class.
- `SyntaxError` is a direct descendant of the [`ScriptError`] class.

# When Should You Use It?

Perhaps the first thing to consider when examining `SyntaxErrors` is that, by their very nature, a `SyntaxError` __cannot be directly `rescued` within a single chunk of code__.

For example, let's run the following simple code snippet, whereby the broken line in question is `1=2`.  Since Ruby expects `1` in this case to be an object to which we're assigning the value of `2`, and not the `Fixnum` object type that it actually is, our code fails and we produce a `SyntaxError`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    1=2
rescue SyntaxError => e
    print_exception(e, true)
end
```

This produces the following output:

```
code.rb:8: syntax error, unexpected '=', expecting keyword_end
    1=2
      ^
```

As you may notice, the issue here is that even though we've added our `rescue` clause and explicitly grab any `SyntaxErrors` -- _and_ the output shows a `SyntaxError` is actually produced -- our `rescue` clause never actually fires.

The reason for this behavior is that Ruby encounters our `SyntaxError` _during the initial parsing_, which is where Ruby converts the code into a `parse tree` used to perform actual execution.  To dive into more detail on how Ruby converts your code into a parse tree, a gem like [`ParseTree`](https://rubygems.org/gems/ParseTree/versions/3.0.9) can be very handy.

Ultimately, this means that you can almost never expect to be able to actually trigger a `rescue` block for a `SyntaxError`.  That said, there are a handful of exceptions to this rule.

The most likely scenario where a `SyntaxError` can actually be `rescued` is when the code that produces the `SyntaxError` is in an outside file that is then `required` by an inner file where the `rescue` clause can then capture it during execution.

For example, here we've taken our above `1=2` code and moved it into a separate file called `invalid.rb`:

```ruby
# invalid.rb
1=2
```

Now, instead of directly executing our invalid code within our `begin` block as before, we _offset_ that execution one layer by using the [`require_relative()`](https://ruby-doc.org/core-2.3.3/Kernel.html#method-i-require_relative) method to include our invalid code into the execution chain:

```ruby
# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    require_relative "invalid"
rescue SyntaxError => e
    print_exception(e, true)
end
```

Now we effectively delay Ruby's processing of our invalid code, thus allowing our `rescue` clause to properly detect the `SyntaxError` and the block to execute our `print_exception()` function as desired.  While the actual error is the same, the output is quite different since our `rescue` block actually fires:

```
[EXPLICIT] SyntaxError: invalid.rb:2: syntax error, unexpected '=', expecting end-of-input
```

It is also possible to `rescue` generated `SyntaxErrors` from within an [`eval()`](https://ruby-doc.org/core-2.3.3/Kernel.html#method-i-eval) method, since this effectively offsets execution ordering the same way `require` does:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    eval("1=2")
rescue SyntaxError => e
    print_exception(e, true)
end
```

This produces an expected, captured error:

```
[EXPLICIT] SyntaxError: (eval):1: syntax error, unexpected '=', expecting end-of-input
```

As with other subclass `Exceptions` that are not direct descendants of `StandardError`, it is important that you do not rely on an inexplicit class within your `rescue` clause to capture `SyntaxErrors`:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    eval("1=2")
rescue => e
    print_exception(e, false)
end
```

While our `SyntaxError` is still produced, of course, the error is not captured by our `rescue` clause and thus the `rescue` block and `print_exception` function do not fire:

```
code.rb:44:in `eval': (eval):1: syntax error, unexpected '=', expecting end-of-input (SyntaxError)
        from code.rb:44:in `<main>'
```

[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html
[`ScriptError`]: https://ruby-doc.org/core-2.3.3/ScriptError.html

--------------------------------------------------------------------------------

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/SyntaxError.html

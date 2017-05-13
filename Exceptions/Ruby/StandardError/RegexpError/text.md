# Ruby Exception Handling: RegexpError

The next stop along our trek through the [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series brings us to the realm of regular expressions and the `RegexpError`.  Quite logically, the `RegexpError` occurs when trying to define a new regular expression that contains some form of an invalid pattern.

We'll spend some time in this article exploring the `RegexpError` in greater detail, seeing where it resides in the Ruby `Exception` class hierarchy, along with some sample code snippets to illustrate how `RegexpErrors` might be thrown, so let's get crackin'!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `RegexpError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).

## When Should You Use It?

Since the `RegexpError` deals directly with regular expressions, this is a good time to remind ourselves what those are and how they are used in Ruby.  A regular expression (often called a `regex` or `regexp` for short) is simply a series of characters that define a search pattern.  This pattern is then used to parse a target string, performing searches for matching characters within the target string.

Most programming languages provide a means of using regular expressions, and Ruby is no different.  A great deal of information about Ruby's particular implementation of regular expressions can be found in the [official documentation](https://ruby-doc.org/core-2.4.1/Regexp.html), so we'll just go over some basic concepts here.

In most cases the syntax for creating a new regular expression is to bound the expression in forward slashes: `/.../`.  Alternatively, a regular expression can be defined by using the `%r{...}` literal syntax instead or by creating a new instance with the `Regexp#new` method call.

The two main purposes of a regular expression are:

- To determine if a target string _contains_ characters which match a given pattern.
- To extract portions of the target string which match a given pattern.

While regular expression patterns can be very complex, the simplest form of a pattern is just to search for literal characters.  Here we're searching for the word `fox` somewhere in our sentence:

```ruby
def regex_example
    begin
        phrase = 'The quick brown fox jumps over the lazy dog.'
        # Define regex.
        regex = /fox/
        # Check for match result.
        result = regex.match(phrase)
        # Output result.
        puts result #=> fox
        # Show the class.
        puts result.class #=> MatchData
        # Get the regexp.
        puts result.regexp #=> (?-mix:fox)
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end
```

As we can see by checking the `#class` of our returned result, the `#match` method of the `Regexp` class returns an instance of the `MatchData` type.  This class provides a number of methods itself that may come in handy.

A few more important concepts of regular expressions are `escape sequences` and `metacharacters`.  Most of the time, characters appearing in a regex pattern are literal representations of themselves.  For example, our regexp of `/fox/` has no escape sequences or special characters, so we're literally searching for the characters `f`, `o`, and `x`, one after the other -- that is, we're searching for the word `fox`.

However, regular expressions in Ruby allow for a number of special metacharacters, which often begin with a single backslash (`\`) escape sequence to indicate what follows is special and should therefore not be taken as a _literal_ character.

For example, the metacharacter of `\w` is a simple way to indicate that we want to search for a "word character."  This means any lowercase letter, uppercase letter, or number.  Let's test this out on our `quick brown fox` sentence from before:

```ruby
result = /\w/.match('The quick brown fox jumps over the lazy dog.')
puts result #=> T
```

As we can see, the result is just a single capital letter `T`.  Why?  Because the `\w` metacharacter merely seeks for the first instance of a character that matches the pattern which, in this case, is the very first letter in the sentence.  However, what if we want to find _whole_ words rather than just single characters.  This is accomplished with `quantifiers`.

There are a number of quantifiers that can be used in a regex pattern and each represents a search for a particular "quantitative range" of the preceeding character class in the pattern:

- `*` - Zero or more times
- `+` - One or more times
- `?` - Zero or one times (optional)
- `{n}` - Exactly n times
- `{n,}` - n or more times
- `{,m}` - m or less times
- `{n,m}` - At least n and at most m times

Thus, if we append any metacharacter in our pattern with one of these `quantifiers`, we're able to tell the parser that we're seeking a particular number (or range of possible numbers) of that character type in sequence.

For example, let's append the `+` quantifier to the end of our `\w` metacharacter, like so: `/\w+/`.  This tells the regex parser that we're looking for all matches in the target string that contain _at least one_ -- or possibly more than one -- word character in sequential order.  In other words, it will look for sequences of letters and/or numbers that are not separated by other characters (such as spaces).

Plugging that into our search reveals an easy way to find _whole words_:

```ruby
result = /\w+/.match('The quick brown fox jumps over the lazy dog.')
puts result #=> The
```

The last common technique we'll discuss for general regular expression usage is `capturing`.  As discussed earlier, regular expressions are either used to search for patterns in a string, or to search for and _extract_ matches from said string.  This is what capturing can accomplish.

For Ruby's regex implementation, capturing is accomplished by surrounding the pattern to be captured in parentheses, like so: `(...)`.  Thus, extending our example above we can try capturing whole words from our sentence:

```ruby
def capture_example
    begin
        # Capture a full word.
        result = /(\w+)/.match('The quick brown fox jumps over the lazy dog.')
        # Output captures.
        puts result.captures #=> The
        # Show that captures is an array.
        puts result.captures.class #=> Array
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end
```

Here we can see that our captured text is the same as before, but can be accessed using the `#captured` method of the `MatchData` result.

There's plenty more to learn about regular expressions, applicable to Ruby and for other languages as well, but with a bit more understanding of how regular expressions work we can take a look at what might cause a `RegexpError` to occur.  As mentioned in the introduction, the basic cause of a `RegexpError` is when trying to define a new regular expression that Ruby cannot parse (and thus considers invalid).

For example, here we're creating a new regular expression using the `Regexp#new` method call.  In this case, the full expression is merely a single `+` symbol:

```ruby
def invalid_pattern_example
    begin
        regex = Regexp.new('+');
        result = regex.match('The quick brown fox jumps over the lazy dog.')
        puts result
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end
```

Sure enough, this raises a `RegexpError`, telling us that our repeat operator (`+`) doesn't have a valid target on which to apply itself:

```
[EXPLICIT] RegexpError: target of repeat operator is not specified: /+/
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close look at the RegexpError in Ruby, with working code examples and a brief exploration of regular expressions in Ruby.
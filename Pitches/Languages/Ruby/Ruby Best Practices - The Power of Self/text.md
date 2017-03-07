# Ruby Best Practices: The Power of Self

For a variety of programming languages, the `self` (or `this`) keyword is a powerful tool, allowing developers to **contextually** refer to a particular class or object.  This capability to refer to the _current_ object, no matter the context, can be difficult to fully grasp, regardless of the language, and Ruby is no different.

In this article we'll explore exactly what the `self` keyword is within Ruby and the various methods in which it is commonly used, so you'll have a better understanding of how Ruby (and other object-oriented programming languages) handle self-referential keywords.  Let's get crackin'!

# Contexts of Self

The purpose (and thus behavior) of `self` within Ruby is dependant on the situation, or the context in which the `self` keyword is used.  In other words, within one context a call to `self` may refer to the parent `class`, whereas in a different context, `self` may refer to a particular _instance_ of a `class`.  To better illustrate the various contexts, we'll outline each and provide code examples as well.

No matter which context the code finds itself, one important rule about `self` must be remembered: _`self` always refers to one and only one object at any given time._  The trick is to understand (and thus plan around) which particular object that will is, given the current context.  Properly doing so provides a great deal of additional development power over the course of most any Ruby project.

## Top Level Self

`Top level` context is for any Ruby code that is **not** executing inside any `class`, `module`, `method`, or otherwise any sub-level code block.  Typically, `top level` code is just naked code in an `.rb` file, executed as a simple script or part of a larger application, without the need for object-oriented components.

For example, here is a _very_ simple script that consists of four lines: creating a `name` variable at the `top level` of execution, then outputting three lines to the console to display both our `name` value and show us what `self` is.

```ruby
# top-level.rb
name = "Jane"
puts "Name is: #{name}"
puts "Self is: #{self}"
puts "Self class is: #{self.class}"
```

The output is:

```
Name is: Jane
Self is: main
Self class is: Object
```

What's critical here is that Ruby has told us that `self`, at least within the `top level` context, is an object called `main`.  This is because all execution within Ruby occurs within the context of an object.  When working inside a particular `class`, it's obvious that that parent `class` is the object of that context, but when Ruby first begins execution, it automatically creates the `main` object.  Thus, our call to `self` above refers to the `main` object instance that Ruby generates for us.

## Class Definition Self

When defining a `class` within Ruby, we can also then refer to `self` within that context as well.  Here we have an `Author` class, inside which we're outputting the value of `self`:

```ruby
# class-definition.rb
class Author
    puts "Self is: #{self}"
end
```

The result is that, directly inside the context of a `class definition`, `self` is equivalent to the parent `class` in which it was defined; `Author`, in this case.  In fact, in this context, the keyword `self` can be thought of as a substitute for the actual `class` name.  Instead of using `self` here, we could use `Author`, and Ruby would treat it the exact same:

```
Self is: Author
```

## Module Definition Self

The use of `self` inside a `module definition` is very similar to that of a `class definition`.  In fact, as far as Ruby is concerned, the reference to `self` doesn't care whether you're in the context of a `class`, or a `module`, it will treat both basically the same.

For example, here we're enclosed our `Author` `class` inside the `Library` `module`, then output the value of `self` within both contexts:

```ruby
# module-definition.rb
module Library
    puts "Self inside Library is: #{self}"

    class Author
        puts "Self inside Library::Author is: #{self}"
    end
end
```

While Ruby treats the `module definition` call to `self` just as it did with the `class definition`, representing the parent level object of `Library` in this case, for the child call it recognizes that there's a hierarchy here, so we get the `module` plus the `class`:

```
Self inside Library is: Library
Self inside Library::Author is: Library::Author
```

## Class Method Self

We won't get into the full details of what distinguishes a `class method` from an `class instance method` in this article, but the simple answer is:

- A `class method` is a method that refers only to that class in all contexts, but not to any individual `instances` of that class.
- A `class instance method` is a method that applies to all `instances` of that class, but not for the `class` object itself.

So, for our example of `self`, we've added the `name` `class method` to our `Author` class (defined by the `self.` object that precedes the definition).  We've also added a `@@name` `class variable`, which isn't necessary for this example, but is a more common practice since it allows our `name` method to act as a property `getter` method for the `@@name` value:


```ruby
# class-method.rb
class Author
    # Define class variable
    @@name = "John Doe"

    # Getter method
    def self.name
        puts "Self inside class method is: #{self}"
        return @@name
    end
end

puts "Author class method 'name' is: #{Author.name}"
```

The fact that `self.name` is used to define a `class method` is a bit of a give away, but sure enough, `self` inside a `class method` definition refers to that parent `class` object -- `Author` in this case.  We also call `Author.name` in the output, to show that our `class method` getter behaves as expected:

```
Self inside class method is: Author
Author class method 'name' is: John Doe
```

## Class Instance Method Self

As discussed in the previous section, a `class instance method` is a method that can be referenced by all instances of that `class`, but not directly by the `class` object itself.  In Ruby, defining a `class instance method` is as simple as excluding the `self.` object prefix within the method definition; in this case, using just `def name` does the trick:

```ruby
# class-instance-method.rb
class Author

    # Instance method
    def name
        puts "Self inside class instance method is: #{self}"
        puts "Self.class inside class instance method is: #{self.class}"
        return "John Doe"
    end
end

# Define instance
author = Author.new
puts "Author class instance method 'name' is: #{author.name}"
```

Since this is an instance method, we can't call it until we create a new instance of our `Author` class.  After that, once we call the `name` method on that instance, we get the full output.  Unlike a direct `class method` reference to `self`, a `class instance method` reference to `self` actually points to the particular `instance` that is being executed, thus our output shows an instance of the `Author` class, as indicated by its memory address:

```
Self inside class instance method is: #<Author:0x00000002bd77c8>
Self.class inside class instance method is: Author
Author class instance method 'name' is: John Doe
```

## Class Singleton Method Self

The final context you're likely to use `self` within is the `class singleton method`.  A `singleton method` is a method that is attached to only one specific `instance` of an object -- it cannot be called by any other `instances` of that object, nor directly by the class object itself.

To illustrate this, we've got our `Author` class again, but we haven't created any methods directly within the `Author` class definition.  Instead, we create a new instance called `author`, then define a `singleton method` by using the `def [INSTANCE].[METHOD]` syntax, or `def author.name` in our case:

```ruby
# class-singleton-method.rb
class Author

end

# Define instance
author = Author.new

# Singleton method
def author.name
    puts "Self inside class singleton method is: #{self}"
    puts "Self.class inside class singleton method is: #{self.class}"
    return "John Doe"
end

puts "Author class singleton method 'name' is: #{author.name}"

# Define second instance without singleton method
new_author = Author.new
puts "New class method 'name' should be undefined: #{new_author.name}"
```

Not only are we outputting the `self` values within our `singleton method`, but we're also generating a second instance of our `Author` class (`new_author`), which we use to confirm that the `name` method is, in fact, a `singleton method` only associated with our original `author` instance of the class.  The full output, once again, shows that inside the `singleton method` itself, `self` refers to _that particular instance_ of the class:

```
Self inside class singleton method is: #<Author:0x00000002d36fd8>
Self.class inside class singleton method is: Author
Author class singleton method 'name' is: John Doe
class-singleton-method.rb:20:in `<main>': undefined method `name' for #<Author:0x00000002d36d80> (NoMethodError)
```

---

__SOURCES__

- https://en.wikipedia.org/wiki/This_(computer_programming)
- http://rubylearning.com/satishtalim/ruby_self.html
- https://www.jimmycuadra.com/posts/self-in-ruby/
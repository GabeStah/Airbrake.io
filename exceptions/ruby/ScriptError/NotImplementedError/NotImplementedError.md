Next on the docket for our __Ruby Exception Handling__ series, we're taking a look at the `NIE` exception class.  `NIE` is a subclass descendant of the `ScriptError` superclass, and occurs when Ruby attempts to utilize a feature or method that isn't implemented for the current platform or Ruby installation.

In this post we'll explore the `NIE` class, examining where it lands within Ruby's `Exception` class hierarchy, how to handle `NIEs`, and best practices to avoid this exception from appearing in the first place!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`] class, or a subclass therein.
- `ScriptError` is a direct descendant of the [`Exception`] class.
- `NIE` is a direct descendant of the `ScriptError` class.

## When Should You Use It?


[`Exception`]: https://ruby-doc.org/core-2.3.3/Exception.html

---

__SOURCES__

- https://ruby-doc.org/core-2.3.3/Exception.html
- https://ruby-doc.org/core-2.3.3/Kernel.html

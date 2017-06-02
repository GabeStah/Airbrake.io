# Ruby Exception Handling: SystemCallError

Moving along through our [__Ruby Exception Handling__](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) series, today we'll be checking out the `SystemCallError` and it's closely associated `Errno` subclasses.  `SystemCallErrors` are raised during any low-level, platform-dependent exceptions like invalid IO calls and the like.

In this article we'll explore the `SystemCallError` in more detail, including where it resides in the Ruby `Exception` class hierarchy.  We'll also take a look at a few simple code examples to illustrate how `SystemCallErrors` and their corresponding `Errnos` show up in the first place, so let's get to it!

## The Technical Rundown

- All Ruby exceptions are descendants of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, or a subclass therein.
- [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html) is a direct descendant of the [`Exception`](https://airbrake.io/blog/ruby-exception-handling/ruby-exception-classes) class, and is also a superclass with many descendants of its own.
- `SystemCallError` is the direct descendant of [`StandardError`](https://ruby-doc.org/core-2.4.0/StandardError.html).
- `Errno` is a module with dozens of subclasses contained within it, all of which are direct descendants of `SystemCallError`.

## When Should You Use It?

`SystemCallError` is somewhat unique in the realm of Ruby exception classes because it isn't raised directly.  Instead, `SystemCallError` is the base class for dozens of `Errno` module subclasses.  The variety of `Errno` module classes is dependant on the operating system that Ruby is running on, but you can view the list of possible `Errno` classes by viewing the output of `Errno.constants` through an interactive Ruby console (`irb`) or similar tools.  As it happens, testing on both Windows 7 and Ubuntu 16.04 (Unix) show that, at least for Ruby 2.3, the list is identical:

**Unix `Errno` constants:**

```ruby
>> Errno.constants
=> [:NOERROR, :EPERM, :ENOENT, :ESRCH, :EINTR, :EIO, :ENXIO, :E2BIG, :ENOEXEC, :EBADF, :ECHILD, :EAGAIN, :ENOMEM, :EACCES, :EFAULT, :ENOTBLK, :EBUSY, :EEXIST, :EXDEV, :ENODEV, :ENOTDIR, :EISDIR, :EINVAL, :ENFILE, :EMFILE, :ENOTTY, :ETXTBSY, :EFBIG, :ENOSPC, :ESPIPE, :EROFS, :EMLINK, :EPIPE, :EDOM, :ERANGE, :EDEADLK, :ENAMETOOLONG, :ENOLCK, :ENOSYS, :ENOTEMPTY, :ELOOP, :EWOULDBLOCK, :ENOMSG, :EIDRM, :ECHRNG, :EL2NSYNC, :EL3HLT, :EL3RST, :ELNRNG, :EUNATCH, :ENOCSI, :EL2HLT, :EBADE, :EBADR, :EXFULL, :ENOANO, :EBADRQC, :EBADSLT, :EDEADLOCK, :EBFONT, :ENOSTR, :ENODATA, :ETIME, :ENOSR, :ENONET, :ENOPKG, :EREMOTE, :ENOLINK, :EADV, :ESRMNT, :ECOMM, :EPROTO, :EMULTIHOP, :EDOTDOT, :EBADMSG, :EOVERFLOW, :ENOTUNIQ, :EBADFD, :EREMCHG, :ELIBACC, :ELIBBAD, :ELIBSCN, :ELIBMAX, :ELIBEXEC, :EILSEQ, :ERESTART, :ESTRPIPE, :EUSERS, :ENOTSOCK, :EDESTADDRREQ, :EMSGSIZE, :EPROTOTYPE, :ENOPROTOOPT, :EPROTONOSUPPORT, :ESOCKTNOSUPPORT, :EOPNOTSUPP, :EPFNOSUPPORT, :EAFNOSUPPORT, :EADDRINUSE, :EADDRNOTAVAIL, :ENETDOWN, :ENETUNREACH, :ENETRESET, :ECONNABORTED, :ECONNRESET, :ENOBUFS, :EISCONN, :ENOTCONN, :ESHUTDOWN, :ETOOMANYREFS, :ETIMEDOUT, :ECONNREFUSED, :EHOSTDOWN, :EHOSTUNREACH, :EALREADY, :EINPROGRESS, :ESTALE, :EUCLEAN, :ENOTNAM, :ENAVAIL, :EISNAM, :EREMOTEIO, :EDQUOT, :ECANCELED, :EKEYEXPIRED, :EKEYREJECTED, :EKEYREVOKED, :EMEDIUMTYPE, :ENOKEY, :ENOMEDIUM, :ENOTRECOVERABLE, :EOWNERDEAD, :ERFKILL, :EAUTH, :EBADRPC, :EDOOFUS, :EFTYPE, :ENEEDAUTH, :ENOATTR, :ENOTSUP, :EPROCLIM, :EPROCUNAVAIL, :EPROGMISMATCH, :EPROGUNAVAIL, :ERPCMISMATCH, :EIPSEC, :EHWPOISON, :ECAPMODE, :ENOTCAPABLE]
>> Errno.constants.count
=> 149
```

**Windows `Errno` constants:**

```ruby
>> Errno.constants
=> [:NOERROR, :EPERM, :ENOENT, :ESRCH, :EINTR, :EIO, :ENXIO, :E2BIG, :ENOEXEC, :EBADF, :ECHILD, :EAGAIN, :ENOMEM, :EACCES, :EFAULT, :ENOTBLK, :EBUSY, :EEXIST, :EXDEV, :ENODEV, :ENOTDIR, :EISDIR, :EINVAL, :ENFILE, :EMFILE, :ENOTTY, :ETXTBSY, :EFBIG, :ENOSPC, :ESPIPE, :EROFS, :EMLINK, :EPIPE, :EDOM, :ERANGE, :EDEADLK, :ENAMETOOLONG, :ENOLCK, :ENOSYS, :ENOTEMPTY, :ELOOP, :EWOULDBLOCK, :ENOMSG, :EIDRM, :ECHRNG, :EL2NSYNC, :EL3HLT, :EL3RST, :ELNRNG, :EUNATCH, :ENOCSI, :EL2HLT, :EBADE, :EBADR, :EXFULL, :ENOANO, :EBADRQC, :EBADSLT, :EDEADLOCK, :EBFONT, :ENOSTR, :ENODATA, :ETIME, :ENOSR, :ENONET, :ENOPKG, :EREMOTE, :ENOLINK, :EADV, :ESRMNT, :ECOMM, :EPROTO, :EMULTIHOP, :EDOTDOT, :EBADMSG, :EOVERFLOW, :ENOTUNIQ, :EBADFD, :EREMCHG, :ELIBACC, :ELIBBAD, :ELIBSCN, :ELIBMAX, :ELIBEXEC, :EILSEQ, :ERESTART, :ESTRPIPE, :EUSERS, :ENOTSOCK, :EDESTADDRREQ, :EMSGSIZE, :EPROTOTYPE, :ENOPROTOOPT, :EPROTONOSUPPORT, :ESOCKTNOSUPPORT, :EOPNOTSUPP, :EPFNOSUPPORT, :EAFNOSUPPORT, :EADDRINUSE, :EADDRNOTAVAIL, :ENETDOWN, :ENETUNREACH, :ENETRESET, :ECONNABORTED, :ECONNRESET, :ENOBUFS, :EISCONN, :ENOTCONN, :ESHUTDOWN, :ETOOMANYREFS, :ETIMEDOUT, :ECONNREFUSED, :EHOSTDOWN, :EHOSTUNREACH, :EALREADY, :EINPROGRESS, :ESTALE, :EUCLEAN, :ENOTNAM, :ENAVAIL, :EISNAM, :EREMOTEIO, :EDQUOT, :ECANCELED, :EKEYEXPIRED, :EKEYREJECTED, :EKEYREVOKED, :EMEDIUMTYPE, :ENOKEY, :ENOMEDIUM, :ENOTRECOVERABLE, :EOWNERDEAD, :ERFKILL, :EAUTH, :EBADRPC, :EDOOFUS, :EFTYPE, :ENEEDAUTH, :ENOATTR, :ENOTSUP, :EPROCLIM, :EPROCUNAVAIL, :EPROGMISMATCH, :EPROGUNAVAIL, :ERPCMISMATCH, :EIPSEC, :EHWPOISON, :ECAPMODE, :ENOTCAPABLE]
>> Errno.constants.count
=> 149
```

Since all low-level errors produce an appropriate `Errno` subclass from the constant list above, there are clearly many possible ways to raise `SystemCallErrors` in the first place.  We don't have time to cover them all here so we'll just give a brief example and see how we can best `rescue` the specific errors we're after.

First, it's important to remember that since `SystemCallError` is the base class of which all `Errno` classes inherit, we can actually capture all possible `Errno` errors by `rescuing` `SystemCallError`.  For example, here we are trying to open a `File` object with an invalid file path provided as the argument.  This raises a `Errno::ENOENT` error, but since all `Errno` subclasses inherit from `SystemCallError`, we can `rescue` them all using that base class:

```ruby
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def invalid_path_example
    begin
        File.open('missing/file/path')        
    rescue SystemCallError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end
```

As expected, our explicit `rescue` of `SystemCallError` works just fine and outputs the `EXPLICIT` result we expected:

```
[EXPLICIT] Errno::ENOENT: No such file or directory @ rb_sysopen - missing/file/path
```

While we _can_ `rescue` the `Errno::ENOENT` class directly, it's important to remember that Ruby will systematically check each `rescue` clause in order until it finds one that matches the error that was raised.  This means that even if we explicitly `rescue` `Errno::ENOENT`, if such a `rescue` comes _after_ `SystemCallError`, the base class of `SystemCallError` still qualifies as a match and occurs first:

```ruby
def invalid_path_example_2
    begin
        File.open('missing/file/path')        
    rescue SystemCallError => e
        puts 'Rescued by SystemCallError statement.'
        print_exception(e, true)        
    rescue Errno::ENOENT => e
        puts 'Rescued by Errno::ENOENT statement.'
        print_exception(e, true)        
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end
```

To illustrate this ordering we've added some simple output messages inside each `rescue` block above to indicate which particular `rescue` statement was responsible for catching our error.  This helps us show that, sure enough, that first `rescue` catches our `Errno::ENOENT` exception, even though we explicitly `rescue` that class later on in the same `begin-rescue-end` block:

```
Rescued by SystemCallError statement.
[EXPLICIT] Errno::ENOENT: No such file or directory @ rb_sysopen - missing/file/path
```

This shows the importance of ordering your `rescue` classes properly, usually beginning with the lowest-level classes and working up toward the basest classes.  Thus, for our example, starting with `Errno:ENOENT` that ensures that the `Errno::ENOENT` errorr is caught by that statement and processed in that block.  Any other `Errno` classes can then be caught by the second `SystemCallError` `rescue`, while all other errors are caught by the plain `rescue` block at the last:

```ruby
def invalid_path_example_3
    begin
        File.open('missing/file/path')        
    rescue Errno::ENOENT => e
        puts 'Rescued by Errno::ENOENT statement.'
        print_exception(e, true)        
    rescue SystemCallError => e
        puts 'Rescued by SystemCallError statement.'
        print_exception(e, true)
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end
```

As expected, this shows an `EXPLICIT` error output from `rescue Errno:ENOENT`:

```
Rescued by Errno::ENOENT statement.
[EXPLICIT] Errno::ENOENT: No such file or directory @ rb_sysopen - missing/file/path
```

It may also be useful to combine multiple classes into a single `rescue` statement.  For example, we could combine `Errno::ENOENT` with `SystemCallError` in a single `rescue` statement like so:

```ruby
def invalid_path_example_4
    begin
        File.open('missing/file/path')        
    rescue Errno::ENOENT, SystemCallError => e
        puts 'Rescued by combined statement.'
        print_exception(e, true)        
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end
```

The produced output:

```
Rescued by combined statement.
[EXPLICIT] Errno::ENOENT: No such file or directory @ rb_sysopen - missing/file/path
```

To get the most out of your own applications and to fully manage any and all Ruby Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-ruby">Airbrake Ruby</a> exception handling tool, offering real-time alerts and instantaneous insight into what went wrong with your Ruby code, including integrated support for a variety of popular Ruby gems and frameworks.

---

__META DESCRIPTION__

A close examination of the `SystemCallError` in Ruby and the variety of child Errno subclasses, with functional code samples.
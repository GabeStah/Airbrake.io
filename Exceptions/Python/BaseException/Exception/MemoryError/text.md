---
categories: [Python Exception Handling]
date: 2018-01-24
published: true
title: "Python Exception Handling - MemoryError"
---

Continuing along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll dig into Python's **MemoryError**.  As with all programming languages, Python includes a fallback exception for when the interpreter completely runs out of memory and must abort current execution.  In these (hopefully rare) instances, Python raises a `MemoryError`, giving the script a chance to catch itself and break out of the current memory draught and recover.  However, since Python uses the C language's `malloc()` function for its memory management architecture, it is not guaranteed that all processes will be able to recover -- in some cases, a `MemoryError` will result in an unrecoverable crash.

In today's article we'll examine the `MemoryError` in more detail, starting with where it sits in the larger [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also examine a simple code sample that illustrates how large memory allocations can occur, how the behavior of using massive objects differs depending on the particular computer architecture and Python version in use, and how `MemoryErrors` may be raised and handled.  Let's get into it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `MemoryError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import os
import psutil
import sys
import traceback

PROCESS = psutil.Process(os.getpid())
MEGA = 10 ** 6
MEGA_STR = ' ' * MEGA


def main():
    try:
        print_memory_usage()
        alloc_max_str()
        alloc_max_array()
    except MemoryError as error:
        # Output expected MemoryErrors.
        log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        log_exception(exception, False)


def alloc_max_array():
    """Allocates memory for maximum array.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    collection = []
    while True:
        try:
            collection.append(MEGA_STR)
        except MemoryError as error:
            # Output expected MemoryErrors.
            log_exception(error)
            break
        except Exception as exception:
            # Output unexpected Exceptions.
            log_exception(exception, False)
    print('Maximum array size:', len(collection) * 10)
    print_memory_usage()


def alloc_max_str():
    """Allocates memory for maximum string.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    i = 0
    while True:
        try:
            a = ' ' * (i * 10 * MEGA)
            del a
        except MemoryError as error:
            # Output expected MemoryErrors.
            log_exception(error)
            break
        except Exception as exception:
            # Output unexpected Exceptions.
            log_exception(exception, False)
        i += 1
    max_i = i - 1
    print('Maximum string size:', (max_i * 10 * MEGA))
    print_memory_usage()


def log_exception(exception: BaseException, expected: bool = True):
    """Prints the passed BaseException to the console, including traceback.

    :param exception: The BaseException to output.
    :param expected: Determines if BaseException was expected.
    """
    output = "[{}] {}: {}".format('EXPECTED' if expected else 'UNEXPECTED', type(exception).__name__, exception)
    print(output)
    exc_type, exc_value, exc_traceback = sys.exc_info()
    traceback.print_tb(exc_traceback)


def print_memory_usage():
    """Prints current memory usage stats.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    total, available, percent, used, free = psutil.virtual_memory()
    total, available, used, free = total / MEGA, available / MEGA, used / MEGA, free / MEGA
    proc = PROCESS.memory_info()[1] / MEGA
    print('process = %s total = %s available = %s used = %s free = %s percent = %s'
          % (proc, total, available, used, free, percent))


if __name__ == "__main__":
    main()
```

## When Should You Use It?

In most situations, a `MemoryError` indicates a major flaw in the current application.  For example, an application that accepts files or user data input could run into `MemoryErrors` if the application has insufficient sanity checks in place.  There are tons of scenarios where memory limits can be problematic, but for our code illustration we'll just stick with a simple allocation in local memory using strings and arrays.

The _most important_ factor in whether your own applications are likely to experience `MemoryErrors` is actually the computer architecture the executing system is running on.  Or, even more specifically, the architecture your version of Python is using.  If you're using a 32-bit Python then the maximum memory allocation given to the Python process is exceptionally low.  The specific maximum memory allocation limit varies and depends on your system, but it's usually around 2 GB and certainly no more than 4 GB.

On the other hand, 64-bit Python versions are more or less limited only by your available system memory.  In practical terms, a 64-bit Python interpreter is unlikely to experience memory issues, or if it does, the issue is a much bigger deal since it's likely impacting the rest of the system anyway.

To test this stuff out we'll be using the [`psutil`](https://pypi.python.org/pypi/psutil) to retrieve information about the active process, and specifically, the [`psutil.virtual_memory()`](http://psutil.readthedocs.io/en/latest/#memory) method, which provides current memory usage stats when invoked.  This information is printed within the `print_memory_usage()` function:

```py
def print_memory_usage():
    """Prints current memory usage stats.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    total, available, percent, used, free = psutil.virtual_memory()
    total, available, used, free = total / MEGA, available / MEGA, used / MEGA, free / MEGA
    proc = PROCESS.memory_info()[1] / MEGA
    print('process = %s total = %s available = %s used = %s free = %s percent = %s'
          % (proc, total, available, used, free, percent))
```

We'll start by using the Python `3.6.4` 32-bit version and appending `MEGA_STR` strings (which contain one million characters each) onto the end of an array until the process catches a `MemoryError`:

```py
PROCESS = psutil.Process(os.getpid())
MEGA = 10 ** 6
MEGA_STR = ' ' * MEGA

def alloc_max_array():
    """Allocates memory for maximum array.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    collection = []
    while True:
        try:
            collection.append(MEGA_STR)
        except MemoryError as error:
            # Output expected MemoryErrors.
            log_exception(error)
            break
        except Exception as exception:
            # Output unexpected Exceptions.
            log_exception(exception, False)
    print('Maximum array size:', len(collection) * 10)
    print_memory_usage()

def log_exception(exception: BaseException, expected: bool = True):
    """Prints the passed BaseException to the console, including traceback.

    :param exception: The BaseException to output.
    :param expected: Determines if BaseException was expected.
    """
    output = "[{}] {}: {}".format('EXPECTED' if expected else 'UNEXPECTED', type(exception).__name__, exception)
    print(output)
    exc_type, exc_value, exc_traceback = sys.exc_info()
    traceback.print_tb(exc_traceback)    
```

After we run out of memory and `break` out of the loop we output the memory usage of the array, along with overall memory usage stats.  The result of running this function is the following output:

```
process = 14.577664 total = 17106.767872 available = 9025.814528 used = 8080.953344 free = 9025.814528 percent = 47.2

[EXPECTED] MemoryError: 
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/MemoryError/main.py", line 33, in alloc_max_array
    collection.append(MEGA_STR)

Maximum array size: 1074655510

process = 446.603264 total = 17106.767872 available = 8769.028096 used = 8337.739776 free = 8769.028096 percent = 48.7
```

This shows our base memory usage at the top, and the array size we created at the bottom.  As expected, after about 15 seconds of execution on my system we experienced a `MemoryError`.  The `alloc_max_str()` function test creates a large string instead of an array, but we should see similar results:

```py
def alloc_max_str():
    """Allocates memory for maximum string.
    See: https://stackoverflow.com/a/15495136

    :return: None
    """
    i = 0
    while True:
        try:
            a = ' ' * (i * 10 * MEGA)
            del a
        except MemoryError as error:
            # Output expected MemoryErrors.
            log_exception(error)
            break
        except Exception as exception:
            # Output unexpected Exceptions.
            log_exception(exception, False)
        i += 1
    max_i = i - 1
    print('Maximum string size:', (max_i * 10 * MEGA))
    print_memory_usage()
```

Sure enough, executing `alloc_max_str()` results in a raised `MemoryError` after a relatively short execution period:

```
[EXPECTED] MemoryError: 
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/MemoryError/main.py", line 54, in alloc_max_str
    a = ' ' * (i * 10 * MEGA)

Maximum string size: 1110000000

process = 14.966784 total = 17106.767872 available = 9240.141824 used = 7866.626048 free = 9240.141824 percent = 46.0
```

As mentioned, there is a huge difference between 32- and 64-bit versions of Python.  If we swap to Python `3.6.4` `64-bit` and execute the same code no `MemoryError` has been thrown after 5+ minutes of iteration.  As discussed, this is because 64-bit Python isn't artificially limited, but can more or less use most of the available system memory!

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep look at the MemoryError in Python, with code samples illustrating how different Python versions may handle MemoryErrors differently.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html
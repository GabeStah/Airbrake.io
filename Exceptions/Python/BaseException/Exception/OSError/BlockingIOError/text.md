---
categories: 
  - Python Exception Handling
date: 2018-02-13
description: "A close look at the BlockingIOError in Python, with code samples illustrating how to perform basic socket requests with and without blocking."
published: true
sources:
  - https://docs.python.org/3/library/exceptions.html
title: "Python Exception Handling - BlockingIOError"  
---

Moving along through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be getting into the **BlockingIOError**.  A `BlockingIOError` is raised when an operation would block an object (such as a `socket`) that is attempting to perform a non-blocking operation.  The `BlockingIOError` is the first of many subclasses in Python for the [`OSError`](https://docs.python.org/3/library/exceptions.html#OSError), all of which indicate a problem at the system level during execution.

We'll further explore the `BlockingIOError` throughout this article, starting with where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look over some fully functional code samples that illustrate how basic `HTTP` requests can be made within Python through the use of the built-in [`socket`](https://docs.python.org/3/library/socket.html) module, so let's get down to it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`OSError`](https://docs.python.org/3/library/exceptions.html#OSError)
            - `BlockingIOError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import socket
import ssl

from gw_utility.logging import Logging


def main():
    try:
        host = 'airbrake.io'

        Logging.line_separator('AIRBRAKE.IO HTTP REQUEST', 60)
        s = get_socket(host, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST', 60)
        s = get_socket(host, True)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTP REQUEST w/o BLOCKING', 60)
        s = get_socket(host, False, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST w/o BLOCKING', 60)
        s = get_socket(host, True, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def get_socket(host, is_ssl=False, is_blocking=True):
    """Retrieves a socket connection to host.

    :param host: Host to connect to.
    :param is_ssl: Determines if SSL connection should be established.
    :param is_blocking: Determines if socket should be blocking.
    :return: Socket or SSLSocket.
    """
    try:
        if is_ssl:
            # If SSL is necessary then wrap socket in SSLContext object.
            context = ssl.SSLContext(ssl.PROTOCOL_SSLv23)
            s = context.wrap_socket(socket.socket(socket.AF_INET, socket.SOCK_STREAM))
            s.setblocking(is_blocking)
            s.connect((host, 443))
            return s
        else:
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.setblocking(is_blocking)
            s.connect((host, 80))
            return s
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def get_socket_response(s, request, max_bytes=4096):
    """Retrieves and logs request response from passed socket, up to maximum bytes.

    :param s: Socket with which to send request.
    :param request: Request (as bytes).
    :param max_bytes: Maximum number of bytes to receive.
    :return: Response data (as bytearray).
    """
    try:
        # Confirm that socket exists.
        if s is None:
            return None

        data = bytearray()

        # Send request.
        s.send(request)

        while True:
            # Get response and extend data array.
            response = s.recv(max_bytes)
            data.extend(response)

            # Break if no bytes, otherwise loop until max_bytes (or all available bytes) received.
            if len(response) == 0 or len(data) >= max_bytes or len(data) == len(response):
                break

        # Close socket.
        s.close()

        # Output decoded response.
        if data is not None:
            Logging.log(data.decode())

        # Return data.
        return data
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# logging.py
import math
import sys
import traceback


class Logging:
    separator_character_default = '-'
    separator_length_default = 40

    @classmethod
    def __output(cls, *args, sep: str = ' ', end: str = '\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        :return: None
        """
        print(*args, sep=sep, end=end, file=file)

    @classmethod
    def line_separator(cls, value: str = None, length: int = separator_length_default,
                       char: str = separator_character_default):
        """Print a line separator with inserted text centered in the middle.

        :param value: Inserted text to be centered.
        :param length: Total separator length.
        :param char: Separator character.
        """
        output = value

        # If no value passed, output separator of length.
        if value == None or len(value) == 0:
            output = f'{char * length}'
        elif len(value) < length:
            #   Update length based on insert length, less a space for margin.
            length -= len(value) + 2
            #   Halve the length and floor left side.
            left = math.floor(length / 2)
            right = left
            #   If odd number, add dropped remainder to right side.
            if length % 2 != 0:
                right += 1

            # Surround insert with separators.
            output = f'{char * left} {value} {char * right}'

        cls.__output(output)

    @classmethod
    def log(cls, *args, sep: str = ' ', end: str = '\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        """
        cls.__output(*args, sep=sep, end=end, file=file)

    @classmethod
    def log_exception(cls, exception: BaseException, expected: bool = True):
        """Prints the passed BaseException to the console, including traceback.

        :param exception: The BaseException to output.
        :param expected: Determines if BaseException was expected.
        """
        output = "[{}] {}: {}".format('EXPECTED' if expected else 'UNEXPECTED', type(exception).__name__, exception)
        cls.__output(output)
        exc_type, exc_value, exc_traceback = sys.exc_info()
        traceback.print_tb(exc_traceback)

```

## When Should You Use It?

As mentioned the `BlockingIOError` is raised when performing a blocking operation on an object that is set to perform non-blocking operations.  In programming, the term `blocking` is used to indicate that the calling execution thread will _wait_ for the `blocking` code to complete execution before moving on to the next instruction.  This is commonly seen in I/O operations where execution may need to temporarily halt (`block`) while the local operating system retrieves a file or data from a database query.  Blocking is also common in multi-threaded applications, since it can be used to halt execution of entire threads while another thread completes some action.  This often makes for challenging code creation, which is why many languages have recently introduced the ability to perform asynchronous instructions, which effectively use non-blocking instructions to _eventually_ process code that would otherwise cause execution to halt.

Now that we have a basic understanding of what blocking is we can evaluate how it relates to Python and the `BlockingIOError`.  There are a handful of different types of objects that can cause `BlockingIOErrors`, but the most likely is a `socket` that is used to connect to a remote host or server.  For our code sample we'll be performing a few direct socket HTTP connections to `airbrake.io`, with and without blocking enabled, and then we'll evaluate the results of each request.  We begin with the `get_socket(host, is_ssl=False, is_blocking=True)` helper method:

```py
def get_socket(host, is_ssl=False, is_blocking=True):
    """Retrieves a socket connection to host.

    :param host: Host to connect to.
    :param is_ssl: Determines if SSL connection should be established.
    :param is_blocking: Determines if socket should be blocking.
    :return: Socket or SSLSocket.
    """
    try:
        if is_ssl:
            # If SSL is necessary then wrap socket in SSLContext object.
            context = ssl.SSLContext(ssl.PROTOCOL_SSLv23)
            s = context.wrap_socket(socket.socket(socket.AF_INET, socket.SOCK_STREAM))
            s.setblocking(is_blocking)
            s.connect((host, 443))
            return s
        else:
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.setblocking(is_blocking)
            s.connect((host, 80))
            return s
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

This method merely creates a `socket` (or `SSLSocket`) instance and connects to the passed `host` on our behalf.  We can then use the generated socket object for actually performing requests, as seen in our `get_socket_response(s, request, max_bytes=4096)` method:

```py
def get_socket_response(s, request, max_bytes=4096):
    """Retrieves and logs request response from passed socket, up to maximum bytes.

    :param s: Socket with which to send request.
    :param request: Request (as bytes).
    :param max_bytes: Maximum number of bytes to receive.
    :return: Response data (as bytearray).
    """
    try:
        # Confirm that socket exists.
        if s is None:
            return None

        data = bytearray()

        # Send request.
        s.send(request)

        while True:
            # Get response and extend data array.
            response = s.recv(max_bytes)
            data.extend(response)

            # Break if no bytes, otherwise loop until max_bytes (or all available bytes) received.
            if len(response) == 0 or len(data) >= max_bytes or len(data) == len(response):
                break

        # Close socket.
        s.close()

        # Output decoded response.
        if data is not None:
            Logging.log(data.decode())

        # Return data.
        return data
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Here we use the passed `s` socket parameter to send the passed `request` and loop through byte chunks from the response, adding them to the `data` `bytearray` before outputting and returning said data.

With everything setup we can try establishing a connection and sending a simple `GET` HTTP request to `airbrake.io`:

```py
host = 'airbrake.io'

Logging.line_separator('AIRBRAKE.IO HTTP REQUEST', 60)
s = get_socket(host, False)
get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())
```

The only thing that may look strange is the formatting and explicit return (`\r`) and newline (`\n`) characters we've added to our request string.  These are necessary to generate a properly formatted request.  In human-readable format the above request string would look like this:

```
GET / HTTP/1.1
Host: airbrake.io
```

Executing this first test successfully connects to `airbrake.io`, but we're given a [`301 Moved Permanently`](https://airbrake.io/blog/http-errors/301-moved-permanently) response (which you can read more about in our [article over here](https://airbrake.io/blog/http-errors/301-moved-permanently)):

```
----------------- AIRBRAKE.IO HTTP REQUEST -----------------
HTTP/1.1 301 Moved Permanently
Location: https://airbrake.io/
Content-Length: 0
Connection: keep-alive
```

As you can probably deduce from the `Location: https://airbrake.io/` response header, the problem here is that we're attempting to make an `HTTP` (unencrypted) request, but `airbrake.io` is actually hosted on (and redirects to) an `HTTPS` address.  To resolve this we'll try the same test again, but this time we'll set `is_ssl` to `True` when calling `get_socket(host, is_ssl=False, is_blocking=True)`:

```py
Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST', 60)
s = get_socket(host, True)
get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())
``` 

Running this test works perfectly by connecting to `airbrake.io` via an SSL context and retrieving the first `4096` bytes of the response, including headers and a bit of HTML:

```
---------------- AIRBRAKE.IO HTTPS REQUEST -----------------
HTTP/1.1 200 OK
Cache-Control: max-age=0, private, must-revalidate
Content-Type: text/html; charset=utf-8
Date: Tue, 13 Feb 2018 09:36:45 GMT
ETag: []
Set-Cookie: []; domain=.airbrake.io; path=/; expires=Tue, 20 Feb 2018 09:36:45 -0000; secure; HttpOnly
Strict-Transport-Security: max-age=31536000
X-Content-Type-Options: nosniff
X-Frame-Options: SAMEORIGIN
X-Request-Id: er1181d9-c548-4c0f-a7a9-5389c348e124
X-Runtime: 0.016832
X-XSS-Protection: 1; mode=block
Content-Length: 31111
Connection: keep-alive

<!DOCTYPE html>
<html>
<head lang='en'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1'>
  <meta charset='utf-8'>

  <title>Error Monitoring and Detection Software | Airbrake</title>

  <link rel="canonical" href="https://airbrake.io/" />

<meta name="description" content="Cut debugging time in half with Airbrake error monitoring software. Real-time alerts help you find and fix bugs as they happen. Supports all major languages." />
```

Cool!  Next, let's see what happens if we set the `is_blocking` argument to `False`, which will inform the (normally blocking) `socket` that it should become non-blocking.  We'll try a non-blocking test for both `HTTP` and `HTTPS`:

```py
Logging.line_separator('AIRBRAKE.IO HTTP REQUEST w/o BLOCKING', 60)
s = get_socket(host, False, False)
get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST w/o BLOCKING', 60)
s = get_socket(host, True, False)
get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())
```

No matter what protocol, both attempts to complete an immediate operation (i.e. `connect` to our host) via our non-blocking `socket` fail and raise `BlockingIOErrors`:

```
---------- AIRBRAKE.IO HTTP REQUEST w/o BLOCKING -----------
[EXPECTED] BlockingIOError: [WinError 10035] A non-blocking socket operation could not be completed immediately
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/OSError/BlockingIOError/main.py", line 53, in get_socket
    s.connect((host, 80))

---------- AIRBRAKE.IO HTTPS REQUEST w/o BLOCKING ----------
[EXPECTED] BlockingIOError: [WinError 10035] A non-blocking socket operation could not be completed immediately
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/OSError/BlockingIOError/main.py", line 48, in get_socket
    s.connect((host, 443))
  File "C:\Program Files\Python36\lib\ssl.py", line 1100, in connect
    self._real_connect(addr, False)
  File "C:\Program Files\Python36\lib\ssl.py", line 1087, in _real_connect
    socket.connect(self, addr)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!
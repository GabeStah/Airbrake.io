# Java Exception Handling - ClassNotFoundException

Today we continue our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series with a closer look at the java.lang.ClassNotFoundException.  Unlike many Java errors that can occur in a variety of scenarios, the `ClassNotFoundException` can only be thrown as a result of three different method calls, all of which handling loading classes by name.

In this article we'll dig into the `ClassNotFoundException` in more detail, looking at where it sits in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), along with some functional code samples illustrating how `ClassNotFoundExceptions` are thrown, so let's get this party started!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.ReflectiveOperationException`](https://docs.oracle.com/javase/8/docs/api/java/lang/ReflectiveOperationException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- Finally, `java.lang.ClassNotFoundException` inherits from [`java.lang.ReflectiveOperationException`](https://docs.oracle.com/javase/8/docs/api/java/lang/ReflectiveOperationException.html).

## When Should You Use It?

As mentioned, the `ClassNotFoundException` can only occur as a result of three different methods failing to load the specified class.  The three methods in question are as follows (we've obviously excluded their respective overloads):

- [`Class.forName(String className)`](https://docs.oracle.com/javase/8/docs/api/java/lang/Class.html#forName-java.lang.String-)
- [`ClassLoader.findSystemClass(String name)`](https://docs.oracle.com/javase/8/docs/api/java/lang/ClassLoader.html#findSystemClass-java.lang.String-)
- [`ClassLoader.loadClass(String name)`](https://docs.oracle.com/javase/8/docs/api/java/lang/ClassLoader.html#loadClass-java.lang.String-)

Therefore, the only time a `ClassNotFoundException` should be raised is when calling one of the above methods with an invalid class name argument.  We'll start with our full code sample below, after which we'll break it down in a bit more detail and look at how each method is used and might potentially throw a `ClassNotFoundException`:

```java
// Main.java
package io.airbrake;

import io.airbrake.utility.*;

public class Main {
    private static String className;

    public static void main(String[] args) {
        // Set className to Integer class.
        className = "java.lang.Integer";
        Logging.lineSeparator(className);

        // Output result of each getClassX method.
        Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
        Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
        Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));

        // Set className to InvalidClassName (an unknown class).
        className = "io.airbrake.InvalidClassName";
        Logging.lineSeparator(className);
        // Output result of each getClassX method.
        Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
        Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
        Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));
    }

    /**
     * Get Class by name via Class.forName(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getClassByName(String name) {
        try {
            // Retrieve class by name.
            return Class.forName(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Get Class by name via MyClassLoader.loadClass(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getClassFromLoaderByName(String name) {
        try {
            // Create new loader instance.
            MyClassLoader loader = new MyClassLoader();
            // Retrieve class by name.
            return loader.loadClass(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Get Class by name via MyClassLoader.findSystemClassByName(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getSystemClassFromLoaderByName(String name) {
        try {
            // Create new loader instance.
            MyClassLoader loader = new MyClassLoader();
            // Retrieve system class by name.
            return loader.findSystemClassByName(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}

// MyClassLoader.java
package io.airbrake;

public class MyClassLoader extends ClassLoader {

    public MyClassLoader() {  }

    public Class<?> findSystemClassByName(String name)
        throws ClassNotFoundException
    {
        return findSystemClass(name);
    }
}

// Logging.java
package io.airbrake.utility;

import java.util.Arrays;
import org.apache.commons.lang3.builder.*;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {
    private static final char separatorCharacterDefault = '-';
    private static final String separatorInsertDefault = "";
    private static final int separatorLengthDefault = 40;

    /**
     * Get a String of passed char of passed length size.
     * @param character Character to repeat.
     * @param length Length of string.
     * @return Created string.
     */
    private static String getRepeatedCharString(char character, int length) {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, character);
        // Return generated string.
        return new String(characters);
    }

    /**
     * Outputs any kind of Object.
     * Uses ReflectionToStringBuilder from Apache commons-lang library.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        if (value instanceof String)
        {
            System.out.println(value);
        }
        else
        {
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
        }
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Can be overloaded if expected parameter should be specified.
     *
     * @param throwable Throwable instance to output.
     */
    public static void log(Throwable throwable)
    {
        // Invoke call with default expected value.
        log(throwable, true);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Includes Throwable class type, message, stack trace, and expectation status.
     *
     * @param throwable Throwable instance to output.
     * @param expected Determines if this Throwable was expected or not.
     */
    public static void log(Throwable throwable, boolean expected)
    {
        System.out.println(String.format("[%s] %s", expected ? "EXPECTED" : "UNEXPECTED", throwable.toString()));
        throwable.printStackTrace();
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator() {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert) {
        lineSeparator(insert, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length) {
        lineSeparator(separatorInsertDefault, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length, char separator) {
        lineSeparator(separatorInsertDefault, length, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(char separator) {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, int length) {
        lineSeparator(insert, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, char separator) {
        lineSeparator(insert, separatorLengthDefault, separator);
    }

    /**
     * Outputs a dashed line separator with
     * inserted text centered in the middle.
     *
     * @param insert Inserted text to be centered.
     * @param length Length of line to be output.
     * @param separator Separator character.
     */
    public static void lineSeparator(String insert, int length, char separator)
    {
        // Default output to insert.
        String output = insert;

        if (insert.length() == 0) {
            output = getRepeatedCharString(separator, length);
        } else if (insert.length() < length) {
            // Update length based on insert length, less a space for margin.
            length -= (insert.length() + 2);
            // Halve the length and floor left side.
            int left = (int) Math.floor(length / 2);
            int right = left;
            // If odd number, add dropped remainder to right side.
            if (length % 2 != 0) right += 1;

            // Surround insert with separators.
            output = String.format("%s %s %s", getRepeatedCharString(separator, left), insert, getRepeatedCharString(separator, right));
        }

        System.out.println(output);
    }
}
```

---

Since we have three built-in `java.lang` methods to test, we've written three of our own testing methods to coincide with each.  We start with the `getClassByName(String name)` method:

```java
/**
* Get Class by name via Class.forName(String).
* @param name Class name.
* @return Class|null
*/
private static Class<?> getClassByName(String name) {
    try {
        // Retrieve class by name.
        return Class.forName(name);
    } catch (ClassNotFoundException exception) {
        // Output exception ClassNotFoundExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

This is an extremely basic method (most of the code is for error handling, in fact).  As you can see, the purpose is to act as a wrapper and exception catcher for the `Class.forName(String className)` method call.

Next, we have the `getSystemClassFromLoaderByName(String name)` method, which is a wrapper to call the `ClassLoader.findSystemClass(String name)` method:

```java
/**
* Get Class by name via MyClassLoader.findSystemClassByName(String).
* @param name Class name.
* @return Class|null
*/
private static Class<?> getSystemClassFromLoaderByName(String name) {
    try {
        // Create new loader instance.
        MyClassLoader loader = new MyClassLoader();
        // Retrieve system class by name.
        return loader.findSystemClassByName(name);
    } catch (ClassNotFoundException exception) {
        // Output exception ClassNotFoundExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

As it happens the `ClassLoader.findSystemClass(String name)` method is `protected`, which means it can only be accessed from within the `class` itself, its `package`, or a `subclass` that extends `ClassLoader`.  Since our calling code is within a different package (`io.airbrake` compared to `java.lang`), we _cannot_ make a direct call to `ClassLoader.findSystemClass(String name)`.

The solution here is to create our own class that extends `ClassLoader`, which will inherit all the methods from the base `ClassLoader` class, and allowing us to create our own class method that wraps the base `ClassLoader.findSystemClass(String name)` call.  The `MyClassLoader` class serves this purpose by extending `ClassLoader`:

```java
// MyClassLoader.java
package io.airbrake;

public class MyClassLoader extends ClassLoader {

    public MyClassLoader() {  }

    public Class<?> findSystemClassByName(String name)
        throws ClassNotFoundException
    {
        return findSystemClass(name);
    }
}

```

Just like the `ClassLoader.findSystemClass(String name)` method, our `findSystemClassByName(String name)` method `throws ClassNotFoundException`, which will propagate up through the call stack to our calling code.

Finally, the `getClassFromLoaderByName(String name)` method is a wrapper for the `ClassLoader.loadClass(String name)` method.  Unlike the previous built-in method, this one is not `protected`, but it _is_ an instance method, so we need to instantiate a new `MyClassLoader` instance on which we can call `loadClass(String name)`:

```java
/**
* Get Class by name via MyClassLoader.loadClass(String).
* @param name Class name.
* @return Class|null
*/
private static Class<?> getClassFromLoaderByName(String name) {
    try {
        // Create new loader instance.
        MyClassLoader loader = new MyClassLoader();
        // Retrieve class by name.
        return loader.loadClass(name);
    } catch (ClassNotFoundException exception) {
        // Output exception ClassNotFoundExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

To test our code out we are passing a class name to each of our three methods, then outputting the returned class name.  If no class was found, the return value is `null`:

```java
public class Main {
    private static String className;

    public static void main(String[] args) {
        // Set className to Integer class.
        className = "java.lang.Integer";
        Logging.lineSeparator(className);

        // Output result of each getClassX method.
        Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
        Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
        Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));

        // ...
    }

    // ...
}
```

We first start by passing a very common and widely used class: `java.lang.Integer`.  This is just to verify that all our methods are working and that classes are retrievable via the underlying, wrapped methods.  The output confirms that all three work as expected:

```
---------- java.lang.Integer -----------
getClassByName(String) result: class java.lang.Integer

getClassFromLoaderByName(String) result: class java.lang.Integer

getSystemClassFromLoaderByName(String) result: class java.lang.Integer
```

Now we change the class name to something that doesn't exist; in this case, `io.airbrake.InvalidClassName`:

```java
// Set className to InvalidClassName (an unknown class).
className = "io.airbrake.InvalidClassName";
Logging.lineSeparator(className);
// Output result of each getClassX method.
Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));
```

As you can probably guess, all three of our methods run into trouble and are unable to retrieve a class by that name, resulting in three `ClassNotFoundExceptions` in the output (along with `null` return values):

```
----- io.airbrake.InvalidClassName -----
[EXPECTED] java.lang.ClassNotFoundException: io.airbrake.InvalidClassName
getClassByName(String) result: null

[EXPECTED] java.lang.ClassNotFoundException: io.airbrake.InvalidClassName
getClassFromLoaderByName(String) result: null

[EXPECTED] java.lang.ClassNotFoundException: io.airbrake.InvalidClassName
getSystemClassFromLoaderByName(String) result: null
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep exploration of the Java ClassNotFoundException, with sample code illustrating how to call all built-in API methods that result in these errors.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
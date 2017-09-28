# Java Exception Handling - UnsatisfiedLinkError

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, in today's article we'll be looking over the **UnsatisfiedLinkError**, which is thrown when attempting to dynamically load a native library that cannot be located by the Java Runtime Environment (`JVM`).

We'll examine the `UnsatisfiedLinkError` by looking at where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), as well as exploring some functional sample code illustrating how one might load native libraries during runtime, and how that might lead to `UnsatisfiedLinkErrors`.  Let's get going!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - `UnsatisfiedLinkError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

_Note: As mentioned in the code documentation, some code used in this sample was obtained from [this StackOverflow comment](https://stackoverflow.com/questions/1007861/how-do-i-get-a-list-of-jni-libraries-which-are-loaded#comment10270887_1008631)_.

```java
// Main.java
package io.airbrake;

import io.airbrake.utility.*;

import java.lang.reflect.Method;

public class Main {
    public static void main(String[] args) {
        Logging.lineSeparator("LIST LOADED LIBRARIES");
        listLoadedLibraries();

        Logging.lineSeparator("UNLOAD ZIPFILE CLASS");
        unloadLibraryByName("java.util.zip.ZipFile", "size");

        Logging.lineSeparator("LOAD INVALID LIBRARY");
        loadLibraryByName("InvalidLibrary");
    }

    /**
     * Lists all currently loaded native libraries.
     *
     * See: https://stackoverflow.com/questions/1007861/how-do-i-get-a-list-of-jni-libraries-which-are-loaded#comment10270887_1008631
     * See: https://pastebin.com/aDgGqjEr
     * See: https://pastebin.com/eVXFdgr9
     */
    private static void listLoadedLibraries() {
        AllLoadedNativeLibrariesInJVM.listAllLoadedNativeLibrariesFromJVM();
    }

    /**
     * Loads a native library using the passed name.
     *
     * @param name Name of library to load.
     */
    private static void loadLibraryByName(String name) {
        try {
            // Attempt to load library.
            System.loadLibrary(name);
        } catch (UnsatisfiedLinkError error) {
            // Output expected UnsatisfiedLinkErrors.
            Logging.log(error);
        } catch (Error | Exception error) {
            // Output unexpected Errors and Exceptions.
            Logging.log(error, false);
        }
    }

    /**
     * Unloads native library using passed libraryName.
     *
     * @param libraryName Name of library to unload.
     */
    private static void unloadLibraryByName(String libraryName, String methodName) {
        try {
            CustomClassLoader cl = new CustomClassLoader();
            Class ca = null;
            ca = cl.findClass(libraryName);
            Object a = ca.newInstance();
            Method p = ca.getMethod(methodName);
            p.invoke(a);
            p = null;
            ca = null;
            a = null;
            cl = null;
            System.gc();
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

```java
// Logging.java
package io.airbrake.utility;

import java.util.Arrays;

import org.apache.commons.lang3.ClassUtils;
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
        if (value == null) return;
        // If primitive or wrapper object, directly output.
        if (ClassUtils.isPrimitiveOrWrapper(value.getClass()))
        {
            System.out.println(value);
        }
        else
        {
            // For complex objects, use reflection builder output.
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
        }
    }

    /**
     * Outputs any kind of String.
     *
     * @param value String to be output.
     */
    public static void log(String value)
    {
        if (value == null) return;
        System.out.println(value);
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

## When Should You Use It?

To understand why an `UnsatisfiedLinkError` might be thrown, we should first briefly talk about native libraries in Java.  Most code that is created and executed within a given Java application is written _using Java code_, which can then be interpreted by the `JVM` to create a running application.  Additionally, the [`Java Native Interface`](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/intro.html) (`JNI`) was introduced to support code written in _other programming languages_, which can then be interpolated and run alongside standard Java code.  For example, a library written and compiled in the `C` programming language could be converted into a native `.dll` file that most Windows users are familiar with, and this "native library" can then be loaded by the `JNI` and executed within a normal Java application.

Now, the potential for a thrown `UnsatisfiedLinkError` occurs when attempting to load such a native library that doesn't exist, or is otherwise incompatible with the executing Java application.  To illustrate, our sample code is somewhat complex, but much of it is broken up into a series of helper classes.  Thus, we start with the simple `listLoadedLibraries()` method, which does just as the name suggests, listing all currently loaded native libraries:

```java
/**
* Lists all currently loaded native libraries.
*
* See: https://stackoverflow.com/questions/1007861/how-do-i-get-a-list-of-jni-libraries-which-are-loaded#comment10270887_1008631
* See: https://pastebin.com/aDgGqjEr
* See: https://pastebin.com/eVXFdgr9
*/
private static void listLoadedLibraries() {
    AllLoadedNativeLibrariesInJVM.listAllLoadedNativeLibrariesFromJVM();
}
```

Executing the above outputs the following, showing that my current JVM isn't running much in the way of additional libraries:

```
-------- LIST LOADED LIBRARIES ---------
C:\Program Files\Java\jdk1.8.0_141\jre\bin\zip.dll
```

Now, you may run into a scenario where you wish to dynamically _unload_ an already-loaded library.  There isn't a direct API method that allows for this, because the JVM instead only unloads classes loaded by a class loader when garbage collection takes place.  For example, the following `unloadLibraryByName(String libraryName, String methodName)` method attempts to unload the passed library by assigning it to a new class loader, then nullifying it and performing garbage collection:

```java
/**
* Unloads native library using passed libraryName.
*
* @param libraryName Name of library to unload.
*/
private static void unloadLibraryByName(String libraryName, String methodName) {
    try {
        CustomClassLoader cl = new CustomClassLoader();
        Class ca = null;
        ca = cl.findClass(libraryName);
        Object a = ca.newInstance();
        Method p = ca.getMethod(methodName);
        p.invoke(a);
        p = null;
        ca = null;
        a = null;
        cl = null;
        System.gc();
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Since we only have the `zip` library currently loaded, let's see what happens if we try to unload it:

```java
Logging.lineSeparator("UNLOAD ZIPFILE CLASS");
unloadLibraryByName("java.util.zip.ZipFile", "size");
```

Attempting to unload the `java.util.zip` library above actually produces a `SecurityException`, indicating that the `java.util.zip` package name is prohibited:

```
--------- UNLOAD ZIPFILE CLASS ---------
[UNEXPECTED] java.lang.SecurityException: Prohibited package name: java.util.zip
```

This particular restriction is intended to prevent user code from modifying standard Java packages, which could potentially present significant security threats, since the native classes have very low-level system access.  Therefore, while the `unloadLibraryByName(...)` method can be used to unload a custom native library, it can't unload a standard Java package, as we saw above.

Alright, so unloading a native library is all well and good, but how do we actually _load_ a library?  There _is_ actually an API method for that, as we can see in the `loadLibraryByName(String name)` method:

```java
/**
* Loads a native library using the passed name.
*
* @param name Name of library to load.
*/
private static void loadLibraryByName(String name) {
    try {
        // Attempt to load library.
        System.loadLibrary(name);
    } catch (UnsatisfiedLinkError error) {
        // Output expected UnsatisfiedLinkErrors.
        Logging.log(error);
    } catch (Error | Exception error) {
        // Output unexpected Errors and Exceptions.
        Logging.log(error, false);
    }
}
```

Simply calling [`System.loadLibrary(String libname)`](https://docs.oracle.com/javase/8/docs/api/java/lang/System.html#loadLibrary-java.lang.String-) will attempt to load the passed native library file (`.dll`, `.so`, etc).  To illustrate, let's try loading a native library:

```java
Logging.lineSeparator("LOAD INVALID LIBRARY");
loadLibraryByName("InvalidLibrary");
```

As you may suspect, attempting to load a library named `InvalidLibrary` throws an `UnsatisfiedLinkError`, indicating that such a library cannot be found in the `java.library.path`:

```
--------- LOAD INVALID LIBRARY ---------
[EXPECTED] java.lang.UnsatisfiedLinkError: no InvalidLibrary in java.library.path
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A detailed look at the Java UnsatisfiedLinkError, with functional code samples illustrating how to load and unload native libraries.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
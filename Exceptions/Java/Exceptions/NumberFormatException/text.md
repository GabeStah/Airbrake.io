# Java Exception Handling - NumberFormatException

Making our way through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we're taking a look at the java.lang.NumberFormatException.  As you may suspect, the `NumberFormatException` is thrown when code attempts to convert an invalid `String` into one of the other generic numeric `wrapper` classes, such as `Integer`, `Byte`, `Long`, and so forth.

Throughout this article we'll explore the `NumberFormatException` in greater detail, looking at where it resides in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), as well as looking at some basic and functional sample code that illustrates how `NumberFormatExceptions` might be commonly thrown.  Let's get going!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- [`java.lang.IllegalArgumentException`](https://docs.oracle.com/javase/8/docs/api/java/lang/IllegalArgumentException.html) then inherits from [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html).
- Finally, `java.lang.NumberFormatException` inherits from [`java.lang.ReflectiveOperationException`](https://docs.oracle.com/javase/8/docs/api/java/lang/ReflectiveOperationException.html).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("STRING TO BYTE");
        Logging.log(convertStringToByte("20"));
        Logging.log(convertStringToByte("200"));

        Logging.lineSeparator("STRING TO DOUBLE");
        Logging.log(convertStringToDouble("3.14e7"));
        Logging.log(convertStringToDouble(Double.toString(Double.MAX_VALUE)));
        Logging.log(convertStringToDouble("3.14x"));

        Logging.lineSeparator("STRING TO FLOAT");
        Logging.log(convertStringToFloat("3.14e7"));
        Logging.log(convertStringToFloat("3.14e39"));
        Logging.log(convertStringToFloat("3.14x39"));

        Logging.lineSeparator("STRING TO INTEGER");
        Logging.log(convertStringToInteger("10"));
        Logging.log(convertStringToInteger("10x"));

        Logging.lineSeparator("STRING TO LONG");
        Logging.log(convertStringToLong("20"));
        // 2^63 - 1
        Logging.log(convertStringToLong("9223372036854775807"));
        // 2^63
        Logging.log(convertStringToLong("9223372036854775808"));

        Logging.lineSeparator("STRING TO SHORT");
        Logging.log(convertStringToShort("20"));
        // 2^15 - 1
        Logging.log(convertStringToShort("32767"));
        // 2^15
        Logging.log(convertStringToShort("32768"));
    }

    /**
     * Convert String to Byte.
     *
     * @param string String to be converted.
     * @return Converted Byte.
     */
    private static Byte convertStringToByte(String string) {
        try {
            return Byte.parseByte(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Double.
     *
     * @param string String to be converted.
     * @return Converted Double.
     */
    private static Double convertStringToDouble(String string) {
        try {
            return Double.parseDouble(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Float.
     *
     * @param string String to be converted.
     * @return Converted Float.
     */
    private static Float convertStringToFloat(String string) {
        try {
            return Float.parseFloat(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Integer.
     *
     * @param string String to be converted.
     * @return Converted Integer.
     */
    private static Integer convertStringToInteger(String string) {
        try {
            return Integer.parseInt(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Long.
     *
     * @param string String to be converted.
     * @return Converted Long.
     */
    private static Long convertStringToLong(String string) {
        try {
            return Long.parseLong(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Short.
     *
     * @param string String to be converted.
     * @return Converted Short.
     */
    private static Short convertStringToShort(String string) {
        try {
            return Short.parseShort(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}

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

The abstract [`Number`](https://docs.oracle.com/javase/8/docs/api/java/lang/Number.html) class is the core superclass that is used to represent numeric values which can be converted into the primitive types of `byte`, `double`, `float`, `int`, `long`, and `short`.  As such, Java throws a `NumberFormatException` when a failed attempt is made to convert into one of those types.  In addition, each of the numeric primitive types has its own wrapper class, namely [`Byte`](https://docs.oracle.com/javase/8/docs/api/java/lang/Byte.html), [`Double`](https://docs.oracle.com/javase/8/docs/api/java/lang/Double.html), [`Float`](https://docs.oracle.com/javase/8/docs/api/java/lang/Float.html), [`Integer`](https://docs.oracle.com/javase/8/docs/api/java/lang/Integer.html), [`Long`](https://docs.oracle.com/javase/8/docs/api/java/lang/Long.html), and [`Short`](https://docs.oracle.com/javase/8/docs/api/java/lang/Short.html).  Like most wrappers, these classes can be used to represent a value object of the underlying primitive type, while also providing additional functionality and methods (such as the `MAX_VALUE` field).

For our sample code we're testing conversion from `String` to each of the wrapper class types using their built in `parseType()` method (e.g. [`Integer.parseInt()`](https://docs.oracle.com/javase/8/docs/api/java/lang/Integer.html#parseInt-java.lang.String-) for the `Integer` class).  Just to keep things tidy we'll traverse through them in alphabetical order, starting with `Byte`, which we test in the `convertStringToByte(String string)` method:

```java
/**
* Convert String to Byte.
*
* @param string String to be converted.
* @return Converted Byte.
*/
private static Byte convertStringToByte(String string) {
    try {
        return Byte.parseByte(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

As you can see, nothing fancy going on here.  We merely take our passed `String` parameter and attempt to parse it via `Byte.parseByte(String string)`.  If we catch an `Exception` we output it.

To call this method we have two different `String` values we're trying: `20` and `200`.

```java
Logging.lineSeparator("STRING TO BYTE");
Logging.log(convertStringToByte("20"));
Logging.log(convertStringToByte("200"));
```

As you're probably aware, the maximum positive value of a `Byte` is `127`, so the first call works, but the second throws a `NumberFormatException`, indicating that the value of `200` is out of range:

```
------------ STRING TO BYTE ------------
20
[EXPECTED] java.lang.NumberFormatException: Value out of range. Value:"200" Radix:10
```

Next we have our `Double` testing method and executing code:

```java
/**
    * Convert String to Double.
    *
    * @param string String to be converted.
    * @return Converted Double.
    */
private static Double convertStringToDouble(String string) {
    try {
        return Double.parseDouble(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}

Logging.lineSeparator("STRING TO DOUBLE");
Logging.log(convertStringToDouble("3.14e7"));
Logging.log(convertStringToDouble(Double.toString(Double.MAX_VALUE)));
Logging.log(convertStringToDouble("3.14x"));
```

Here we're trying three different values to show how we can use those build-in fields, like `Double.MAX_VALUE`.  However, an unrecognizable character of `x` at the end of our third `String` value results in another `NumberFormatException`:

```
----------- STRING TO DOUBLE -----------
3.14E7
1.7976931348623157E308
[EXPECTED] java.lang.NumberFormatException: For input string: "3.14x"
```

Here's our test for conversion to a `Float` object:

```java
/**
    * Convert String to Float.
    *
    * @param string String to be converted.
    * @return Converted Float.
    */
private static Float convertStringToFloat(String string) {
    try {
        return Float.parseFloat(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}

Logging.lineSeparator("STRING TO FLOAT");
Logging.log(convertStringToFloat("3.14e7"));
Logging.log(convertStringToFloat("3.14e39"));
Logging.log(convertStringToFloat("3.14x39"));
```

Our first value call works fine, as does the second, except the output is `Infinity`.  This is because the maximum positive value of a `Float` is a little less than `2^39`, so while there's no parse error, a `Float` object cannot handle that value so it wraps to an infinite representation.  Our third call, however, again uses an invalid character of `x` instead of the exponent `e`, so another `NumberFormatException` is thrown:

```
3.14E7
Infinity
[EXPECTED] java.lang.NumberFormatException: For input string: "3.14x39"
```

For our `Integer` test we again use an invalid character of `x`:

```java
/**
* Convert String to Integer.
*
* @param string String to be converted.
* @return Converted Integer.
*/
private static Integer convertStringToInteger(String string) {
    try {
        return Integer.parseInt(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}

Logging.lineSeparator("STRING TO INTEGER");
Logging.log(convertStringToInteger("10"));
Logging.log(convertStringToInteger("10x"));
```

As expected, the second call throws another `NumberFormatException`:

```
---------- STRING TO INTEGER -----------
10
[EXPECTED] java.lang.NumberFormatException: For input string: "10x"
```

Next we have the `Long` value, which is essentially just the much larger form of an `Integer`:

```java
/**
* Convert String to Long.
*
* @param string String to be converted.
* @return Converted Long.
*/
private static Long convertStringToLong(String string) {
    try {
        return Long.parseLong(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}

Logging.lineSeparator("STRING TO LONG");
Logging.log(convertStringToLong("20"));
// 2^63 - 1
Logging.log(convertStringToLong("9223372036854775807"));
// 2^63
Logging.log(convertStringToLong("9223372036854775808"));
```

As you can see by the comment, the maximum positive value of a `Long` is `2e63 - 1`, which we've converted to its decimal format as a `String`.  Both these first two conversions work fine, but increasing the value to one above the `MAX_VALUE` produces another `NumberFormatException`:

```
------------ STRING TO LONG ------------
20
9223372036854775807
[EXPECTED] java.lang.NumberFormatException: For input string: "9223372036854775808"
```

Finally, we have the `Short` conversion.  While `Long` is a much larger `Integer`, `Short` is limited to a far smaller value:

```java
/**
* Convert String to Short.
*
* @param string String to be converted.
* @return Converted Short.
*/
private static Short convertStringToShort(String string) {
    try {
        return Short.parseShort(string);
    } catch (NumberFormatException exception) {
        // Output expected NumberFormatException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}

Logging.lineSeparator("STRING TO SHORT");
Logging.log(convertStringToShort("20"));
// 2^15 - 1
Logging.log(convertStringToShort("32767"));
// 2^15
Logging.log(convertStringToShort("32768"));
```

Again we're testing using the maximum positive value of a `Short` (`32,767`), which works fine, but the increase to one more than that throws yet another `NumberFormatException`:

```
----------- STRING TO SHORT ------------
20
32767
[EXPECTED] java.lang.NumberFormatException: Value out of range. Value:"32768" Radix:10
```

As we can see, `NumberFormatExceptions` can occur in a variety of scenarios, but typically they're due to either typos in the numeric `String` values that are being parsed, _or_ because the resultant value would exceed the bounds of the target object type.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep exploration of the Java NumberFormatException, with sample code illustrating limitations on converting into different primitive numeric types.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
---
categories: [Java Exception Handling]
date: 2018-02-02
published: true
title: "Java Exception Handling - InputMismatchException"
---

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be examining the **InputMismatchException**.  The `InputMismatchException` is thrown when attempting to retrieve a `token` using the text [`Scanner`](https://docs.oracle.com/javase/9/docs/api/java/util/Scanner.html) class that doesn't match the expected pattern or type.

In this article we'll explore the `InputMismatchException` in more detail by first looking at where it sits in the larger [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also explore the basic purpose and usage of the built-in [`Scanner`](https://docs.oracle.com/javase/9/docs/api/java/util/Scanner.html) class, and see how improper use of this class can result in unintended `InputMismatchExceptions` in your own code, so let's dig in!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.RuntimeError`](https://docs.oracle.com/javase/9/docs/api/java/lang/RuntimeException.html)
                - [`java.util.NoSuchElementException`](https://docs.oracle.com/javase/9/docs/api/java/util/NoSuchElementException.html)
                    - `InputMismatchException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.File;
import java.util.InputMismatchException;
import java.util.List;
import java.util.Scanner;
import java.util.regex.MatchResult;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Main {

    private static final String FILE_PATH = "2014_world_gdp_with_codes.csv";
    // Pads trio of values by 35 spaces, with explicit alignment of floats on decimal point.
    private static final String OUTPUT_FORMAT = "%-35s%10.2f%25s";
    // Matches CSVs, with first value including optional quotations, spaces, etc.
    private static final String REGEX_PATTERN = "(\"?[-()',\\w\\s]+\"?),(\\d*\\.?\\d*?),(\\w+)\n";

    public static void main(String[] args) {
        Logging.lineSeparator("DELIMITER TEST, BY TYPES", 70);
        delimiterTestByTypes();

        Logging.lineSeparator("DELIMITER TEST, WITH INVALID TYPES", 70);
        delimiterTestByDirectTypes();

        Logging.lineSeparator("RESULT STREAM TEST", 70);
        resultStreamTest();
    }

    /**
     * Retrieves and outputs Scanner results using Java 9 findAll() pattern matching method.
     */
    private static void resultStreamTest() {
        try {
            // Create Scanner to parse passed file.
            Scanner scanner = new Scanner(new File(FILE_PATH));
            // Find all regex matches from REGEX_PATTERN.
            Stream<MatchResult> resultStream = scanner.findAll(REGEX_PATTERN);
            // Use ResultStream to collect results into a list.
            List<MatchResult> list = resultStream.collect(Collectors.toList());
            // Iterate MatchResults to extract and output values.
            for (MatchResult result : list) {
                String country = result.group(1);
                Double gdp = Double.valueOf(result.group(2));
                String code = result.group(3);
                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves and outputs Scanner results using Scanner delimiter and getScannerValueByType() results.
     */
    private static void delimiterTestByTypes() {
        try {
            // Create Scanner to parse passed file, using either comma- or newline-delimiter.
            Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

            // Iterate through new lines when scanner has a next value.
            while (scanner.hasNextLine() && scanner.hasNext()) {
                // Get next values indirectly through getScannerValueByType() method.
                Object country = getScannerValueByType(String.class, scanner);
                Double gdp = (Double) getScannerValueByType(Double.class, scanner);
                Object code = getScannerValueByType(String.class, scanner);

                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves and outputs Scanner results using Scanner delimiter and direct next[TYPE] type method calls.
     */
    private static void delimiterTestByDirectTypes() {
        try {
            // Create Scanner to parse passed file, using either comma- or newline-delimiter.
            Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

            // Iterate through new lines when scanner has a next value.
            while (scanner.hasNextLine() && scanner.hasNext()) {
                // Get next values directly, without sanity checks.
                Object country = scanner.next();
                Double gdp = scanner.nextDouble();
                Object code = scanner.next();

                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves the appropriate Scanner.next[Type] method result based on passed <T>type</T>.
     *
     * @param clazz Class matching type to retrieve.
     * @param scanner Scanner instance from which to retrieve value.
     * @param <T> Type to retrieve.
     * @return Retrieved value, or null.
     */
    private static <T> Object getScannerValueByType(Class<T> clazz, Scanner scanner) {
        switch (clazz.getSimpleName()) {
            case "Byte":
                if (scanner.hasNextByte())
                    return scanner.nextByte();
                break;
            case "Double":
                if (scanner.hasNextDouble())
                    return scanner.nextDouble();
                break;
            case "Float":
                if (scanner.hasNextFloat())
                    return scanner.nextFloat();
                break;
            case "Integer":
                if (scanner.hasNextInt())
                    return scanner.nextInt();
                break;
            case "Long":
                if (scanner.hasNextLong())
                    return scanner.nextLong();
                break;
            case "Short":
                if (scanner.hasNextShort())
                    return scanner.nextShort();
                break;
            case "String":
                if (scanner.hasNext())
                    return scanner.next();
                break;
            default:
                if (scanner.hasNext())
                    return scanner.next();
                break;
        }
        return null;
    }
}
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

To understand what might cause an `InputMismatchException` we need to briefly explore what the [`Scanner`](https://docs.oracle.com/javase/9/docs/api/java/util/Scanner.html) class is and how it might be used in a Java application.  `Scanner` can be used to perform simple text scanning and parsing using regular expression or delimiter pattern matching.  It can be used for single lines of text, or even massive files containing thousands of lines.  For our sample code we're using the [2014 GDP data courtesy of Plotly](https://github.com/plotly/datasets/blob/master/2014_world_gdp_with_codes.csv), which is in the common `CSV` file format.  There are only a couple hundred lines of text in this file, but it should give us an interesting scenario in which to properly parse some real-world data.

To begin we start with the `delimiterTestByDirectTypes` method:

```java
private static final String FILE_PATH = "2014_world_gdp_with_codes.csv";
// Pads trio of values by 35 spaces, with explicit alignment of floats on decimal point.
private static final String OUTPUT_FORMAT = "%-35s%10.2f%25s";
// Matches CSVs, with first value including optional quotations, spaces, etc.
private static final String REGEX_PATTERN = "(\"?[-()',\\w\\s]+\"?),(\\d*\\.?\\d*?),(\\w+)\n";

/**
    * Retrieves and outputs Scanner results using Scanner delimiter and direct next[TYPE] type method calls.
    */
private static void delimiterTestByDirectTypes() {
    try {
        // Create Scanner to parse passed file, using either comma- or newline-delimiter.
        Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

        // Iterate through new lines when scanner has a next value.
        while (scanner.hasNextLine() && scanner.hasNext()) {
            // Get next values directly, without sanity checks.
            Object country = scanner.next();
            Double gdp = scanner.nextDouble();
            Object code = scanner.next();

            // Output values using OUTPUT_FORMAT.
            Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
        }

        // Close scanner after completion.
        scanner.close();
    } catch (InputMismatchException exception) {
        // Output unexpected InputMismatchExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

This method passes our local `File` into a new `Scanner` instance and specifies the use of a delimiter of `"[,\\n]"`.  This delimiter is necessary because our CSV file separates each value within a record using a comma, but it also contains multiple records separated by a newline (`\n`), so we need to inform the `Scanner` instance that both of these delimiters should be accounted for.

Once our `Scanner` instance is created we check if it has a new line and `next()` value, which is the default type retrieved by a `Scanner` instance and assumes the `token` it will find next is a `String`.  From there, we _directly_ extract each subsequent `token` by calling `scanner.next()` or `scanner.nextDouble()`, depending what type of value we're extracting.  Both our `country` and `code` values should be `Strings`, while `gdp` is a float or `Double`.  Once extracted, we output everything into a clean, padded format.

The result of executing this method is as follows:

```
----------------- DELIMITER TEST, WITH INVALID TYPES -----------------
Afghanistan                             21.71                      AFG
Albania                                 13.40                      ALB
Algeria                                227.80                      DZA
American Samoa                           0.75                      ASM
Andorra                                  4.80                      AND
Angola                                 131.40                      AGO
Anguilla                                 0.18                      AIA
Antigua and Barbuda                      1.24                      ATG
Argentina                              536.20                      ARG
Armenia                                 10.88                      ARM
Aruba                                    2.52                      ABW
Australia                             1483.00                      AUS
Austria                                436.10                      AUT
Azerbaijan                              77.91                      AZE
[EXPECTED] java.util.InputMismatchException
	at java.base/java.util.Scanner.throwFor(Scanner.java:860)
	at java.base/java.util.Scanner.next(Scanner.java:1497)
	at java.base/java.util.Scanner.nextDouble(Scanner.java:2467)
	at io.airbrake.Main.delimiterTestByDirectTypes(Main.java:105)
	at io.airbrake.Main.main(Main.java:26)
```

So, everything was working just as expected until we reached the 15th record, at which point an `InputMismatchException` was thrown.  If we look at the source data a bit we can see why we ran into a problem:

```csv
Afghanistan,21.71,AFG
Albania,13.40,ALB
Algeria,227.80,DZA
American Samoa,0.75,ASM
Andorra,4.80,AND
Angola,131.40,AGO
Anguilla,0.18,AIA
Antigua and Barbuda,1.24,ATG
Argentina,536.20,ARG
Armenia,10.88,ARM
Aruba,2.52,ABW
Australia,1483.00,AUS
Austria,436.10,AUT
Azerbaijan,77.91,AZE
"Bahamas, The",8.65,BHM
```

Here we see our data isn't as "clean" as we originally thought.  The "The Bahamas" is alphabetized by ignoring the word "The", this record actually has _three_ comma delimiters, rather than the expected three of all previous records.  As a result, our first call to `scanner.next()` for this record returns `"Bahamas`, because it uses the first comma it finds as a delimiter.  Thus, the next call to `scanner.nextDouble()` _tries_ to evaluate the value of ` The"` as a `Double`, which obviously fails, resulting in the `InputMismatchException` we see above.

One possible solution would be to perform some kind of sanity checks before we explicitly call the `scanner.nextDouble()` method.  To assist with this and allow our code to be a bit more future-proof we've added the `getScannerValueByType(Class<T> clazz, Scanner scanner)` helper method:

```java
/**
* Retrieves the appropriate Scanner.next[Type] method result based on passed <T>type</T>.
*
* @param clazz Class matching type to retrieve.
* @param scanner Scanner instance from which to retrieve value.
* @param <T> Type to retrieve.
* @return Retrieved value, or null.
*/
private static <T> Object getScannerValueByType(Class<T> clazz, Scanner scanner) {
    switch (clazz.getSimpleName()) {
        case "Byte":
            if (scanner.hasNextByte())
                return scanner.nextByte();
            break;
        case "Double":
            if (scanner.hasNextDouble())
                return scanner.nextDouble();
            break;
        case "Float":
            if (scanner.hasNextFloat())
                return scanner.nextFloat();
            break;
        case "Integer":
            if (scanner.hasNextInt())
                return scanner.nextInt();
            break;
        case "Long":
            if (scanner.hasNextLong())
                return scanner.nextLong();
            break;
        case "Short":
            if (scanner.hasNextShort())
                return scanner.nextShort();
            break;
        case "String":
            if (scanner.hasNext())
                return scanner.next();
            break;
        default:
            if (scanner.hasNext())
                return scanner.next();
            break;
    }
    return null;
}
```

The purpose of this method is merely to invoke the appropriate `scanner.next[Type]` method based on the `<T>` type that was passed to it.  Furthermore, we explicitly perform a sanity check using the appropriate `scanner.hasNext[Type]` method prior to actually returning a value, to ensure that no unexpected `InputMismatchExceptions` are thrown.

The `delimiterTestByTypes()` method is similar to our previous test, but we use the results of `getScannerValueByType(...)` for each record:

```java
/**
* Retrieves and outputs Scanner results using Scanner delimiter and getScannerValueByType() results.
*/
private static void delimiterTestByTypes() {
    try {
        // Create Scanner to parse passed file, using either comma- or newline-delimiter.
        Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

        // Iterate through new lines when scanner has a next value.
        while (scanner.hasNextLine() && scanner.hasNext()) {
            // Get next values indirectly through getScannerValueByType() method.
            Object country = getScannerValueByType(String.class, scanner);
            Double gdp = (Double) getScannerValueByType(Double.class, scanner);
            Object code = getScannerValueByType(String.class, scanner);

            // Output values using OUTPUT_FORMAT.
            Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
        }

        // Close scanner after completion.
        scanner.close();
    } catch (InputMismatchException exception) {
        // Output unexpected InputMismatchExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Executing this code no longer throws any `InputMismatchExceptions`, but we still see some strange behavior:

```
---------------------- DELIMITER TEST, BY TYPES ----------------------
Afghanistan                             21.71                      AFG
Albania                                 13.40                      ALB
Algeria                                227.80                      DZA
American Samoa                           0.75                      ASM
Andorra                                  4.80                      AND
Angola                                 131.40                      AGO
Anguilla                                 0.18                      AIA
Antigua and Barbuda                      1.24                      ATG
Argentina                              536.20                      ARG
Armenia                                 10.88                      ARM
Aruba                                    2.52                      ABW
Australia                             1483.00                      AUS
Austria                                436.10                      AUT
Azerbaijan                              77.91                      AZE
"Bahamas                                   nu                     The"
8.65                                       nu                      BHM
```

Once again, everything works fine until we get to the "Bahamas, The" record.  Just as before, since our delimiter pattern is merely `[,\\n]`, the `Scanner` has a difficult time handling records with more than two comma-separators.  In fact, the best solution is not to rely on the delimiter setting at all, but to instead use a more complex RegEx pattern to match (and then extract) each value from each record.  To accomplish this we have one last method, `resultStreamTest()`:

```java
/**
* Retrieves and outputs Scanner results using Java 9 findAll() pattern matching method.
*/
private static void resultStreamTest() {
    try {
        // Create Scanner to parse passed file.
        Scanner scanner = new Scanner(new File(FILE_PATH));
        // Find all regex matches from REGEX_PATTERN.
        Stream<MatchResult> resultStream = scanner.findAll(REGEX_PATTERN);
        // Use ResultStream to collect results into a list.
        List<MatchResult> list = resultStream.collect(Collectors.toList());
        // Iterate MatchResults to extract and output values.
        for (MatchResult result : list) {
            String country = result.group(1);
            Double gdp = Double.valueOf(result.group(2));
            String code = result.group(3);
            // Output values using OUTPUT_FORMAT.
            Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
        }

        // Close scanner after completion.
        scanner.close();
    } catch (InputMismatchException exception) {
        // Output unexpected InputMismatchExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Here we're taking advantage of the new [`findAll(Pattern pattern)`](https://docs.oracle.com/javase/9/docs/api/java/util/Scanner.html#findAll-java.util.regex.Pattern-) method added to the `Scanner` class in Java 9.  This presents a much more modern programming pattern that allows us to chain functions, predicates, and other methods onto the iterable result of `Stream<MatchResult>`.  In this case, we're collecting all results into a `List<MatchResult>`, which we then iterate through in a `for` loop and extract each record's value to be assigned to the relevant local variable and output.

The majority of the work here is accomplished in the `REGEX_PATTERN` passed to `scanner.findAll(...)`:

```java
private static final String REGEX_PATTERN = "(\"?[-()',\\w\\s]+\"?),(\\d*\\.?\\d*?),(\\w+)\n";
```

You can see the regex pattern [in action on regexr.com](https://regexr.com/3k585), but the basic purpose is to ensure we're capturing only three values from each line, and that we handle all unusual formatting and characters that are possible in the country name/first field, such as extra commas, quotation marks, parenthesis, and so forth.

The final result is an accurate and clean extraction and log output of all our GDP data:

```
------------------------- RESULT STREAM TEST -------------------------
Afghanistan                             21.71                      AFG
Albania                                 13.40                      ALB
Algeria                                227.80                      DZA
American Samoa                           0.75                      ASM
Andorra                                  4.80                      AND
Angola                                 131.40                      AGO
Anguilla                                 0.18                      AIA
Antigua and Barbuda                      1.24                      ATG
Argentina                              536.20                      ARG
Armenia                                 10.88                      ARM
Aruba                                    2.52                      ABW
Australia                             1483.00                      AUS
Austria                                436.10                      AUT
Azerbaijan                              77.91                      AZE
"Bahamas, The"                           8.65                      BHM
Bahrain                                 34.05                      BHR
Bangladesh                             186.60                      BGD
Barbados                                 4.28                      BRB
Belarus                                 75.25                      BLR
Belgium                                527.80                      BEL
Belize                                   1.67                      BLZ
Benin                                    9.24                      BEN
Bermuda                                  5.20                      BMU
Bhutan                                   2.09                      BTN
Bolivia                                 34.08                      BOL
Bosnia and Herzegovina                  19.55                      BIH
Botswana                                16.30                      BWA
Brazil                                2244.00                      BRA
British Virgin Islands                   1.10                      VGB
Brunei                                  17.43                      BRN
Bulgaria                                55.08                      BGR
Burkina Faso                            13.38                      BFA
Burma                                   65.29                      MMR
Burundi                                  3.04                      BDI
Cabo Verde                               1.98                      CPV
Cambodia                                16.90                      KHM
Cameroon                                32.16                      CMR
Canada                                1794.00                      CAN
Cayman Islands                           2.25                      CYM
Central African Republic                 1.73                      CAF
Chad                                    15.84                      TCD
Chile                                  264.10                      CHL
China                                10360.00                      CHN
Colombia                               400.10                      COL
Comoros                                  0.72                      COM
"Congo, Democratic Republic of the"     32.67                      COD
"Congo, Republic of the"                14.11                      COG
Cook Islands                             0.18                      COK
Costa Rica                              50.46                      CRI
Cote d'Ivoire                           33.96                      CIV
Croatia                                 57.18                      HRV
Cuba                                    77.15                      CUB
Curacao                                  5.60                      CUW
Cyprus                                  21.34                      CYP
Czech Republic                         205.60                      CZE
Denmark                                347.20                      DNK
Djibouti                                 1.58                      DJI
Dominica                                 0.51                      DMA
Dominican Republic                      64.05                      DOM
Ecuador                                100.50                      ECU
Egypt                                  284.90                      EGY
El Salvador                             25.14                      SLV
Equatorial Guinea                       15.40                      GNQ
Eritrea                                  3.87                      ERI
Estonia                                 26.36                      EST
Ethiopia                                49.86                      ETH
Falkland Islands (Islas Malvinas)        0.16                      FLK
Faroe Islands                            2.32                      FRO
Fiji                                     4.17                      FJI
Finland                                276.30                      FIN
France                                2902.00                      FRA
French Polynesia                         7.15                      PYF
Gabon                                   20.68                      GAB
"Gambia, The"                            0.92                      GMB
Georgia                                 16.13                      GEO
Germany                               3820.00                      DEU
Ghana                                   35.48                      GHA
Gibraltar                                1.85                      GIB
Greece                                 246.40                      GRC
Greenland                                2.16                      GRL
Grenada                                  0.84                      GRD
Guam                                     4.60                      GUM
Guatemala                               58.30                      GTM
Guernsey                                 2.74                      GGY
Guinea-Bissau                            1.04                      GNB
Guinea                                   6.77                      GIN
Guyana                                   3.14                      GUY
Haiti                                    8.92                      HTI
Honduras                                19.37                      HND
Hong Kong                              292.70                      HKG
Hungary                                129.70                      HUN
Iceland                                 16.20                      ISL
India                                 2048.00                      IND
Indonesia                              856.10                      IDN
Iran                                   402.70                      IRN
Iraq                                   232.20                      IRQ
Ireland                                245.80                      IRL
Isle of Man                              4.08                      IMN
Israel                                 305.00                      ISR
Italy                                 2129.00                      ITA
Jamaica                                 13.92                      JAM
Japan                                 4770.00                      JPN
Jersey                                   5.77                      JEY
Jordan                                  36.55                      JOR
[...]
Sri Lanka                               71.57                      LKA
Sudan                                   70.03                      SDN
Suriname                                 5.27                      SUR
Swaziland                                3.84                      SWZ
Sweden                                 559.10                      SWE
Switzerland                            679.00                      CHE
Syria                                   64.70                      SYR
Taiwan                                 529.50                      TWN
Tajikistan                               9.16                      TJK
Tanzania                                36.62                      TZA
Thailand                               373.80                      THA
Timor-Leste                              4.51                      TLS
Togo                                     4.84                      TGO
Tonga                                    0.49                      TON
Trinidad and Tobago                     29.63                      TTO
Tunisia                                 49.12                      TUN
Turkey                                 813.30                      TUR
Turkmenistan                            43.50                      TKM
Tuvalu                                   0.04                      TUV
Uganda                                  26.09                      UGA
Ukraine                                134.90                      UKR
United Arab Emirates                   416.40                      ARE
United Kingdom                        2848.00                      GBR
United States                        17420.00                      USA
Uruguay                                 55.60                      URY
Uzbekistan                              63.08                      UZB
Vanuatu                                  0.82                      VUT
Venezuela                              209.20                      VEN
Vietnam                                187.80                      VNM
Virgin Islands                           5.08                      VGB
West Bank                                6.64                      WBG
Yemen                                   45.45                      YEM
Zambia                                  25.61                      ZMB
Zimbabwe                                13.74                      ZWE
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java InputMismatchException, with code samples showing how to use the Scanner class for simple text parsing.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
# Java Exception Handling - UnsupportedClassVersionError

Moving along through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be taking a closer look at the **UnsupportedClassVersionError**.  The `UnsupportedClassVersionError` can be rather confusing, but the simplest explanation is that it occurs when the Java Virtual Machine (`JVM`) attempts to access a class file that was compiled using a different Java version than is currently supported.

In this article we'll dig into what might cause a particular version to be unsupported, where `UnsupportedClassVersionErrors` fit into the larger [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), and how changing the way in which classes are compiled can sometimes work around these potential issues.  Let's get going!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - [`java.lang.ClassFormatError`](https://docs.oracle.com/javase/8/docs/api/java/lang/ClassFormatError.html)
                    - `UnsupportedClassVersionError`

## When Should You Use It?

Unlike most of our [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) articles thus far, this one won't include a great deal of functional source code.  Since the `UnsupportedClassVersionError` is thrown due to compilation and versioning issues, as opposed to most errors that are due to problems in the source code, we'll forego a lot of code in this post and focus more on the explanation and nuances of compilation issues within Java.

As previously mentioned, an `UnsupportedClassVersionError` is thrown when the `JVM` attempts to use a class that was compiled using an incompatible or unsupported Java version than what is currently running.  For example, the most recent stable version of Java is `8`, and there have been about ten major versions since the beta release way back in 1994.  Each new major version brings a great deal of features and syntactic improvements, just like with most languages, which dramatically improve the flexibility and ease of use for developers that are able to upgrade to the new platform.

However, migrating an existing application to the latest and greatest major Java version isn't always feasible.  Many mission-critical systems have been built over the years using Java.  Google, Amazon, YouTube, eBay... the list of massive companies and projects that rely on Java goes on and on.  In many cases, it simply isn't feasible to upgrade existing, functional systems to the newest releases, given the potential of introducing massive, unforeseen issues when doing so.

This conundrum may invariably lead to a scenario in which a Java application that was _compiled_ using one (typically older) version of Java, must be executed on a different version.  For the most part, backward compatibility is supported by newer versions, meaning that older applications can be run on newer versions of the `JVM`.  However, some situations may arise where there are incompatibilities between the version the application (or `class`) was compiled on, and the version that is attempting to execute said application.  It is these situations where an `UnsupportedClassVersionError` may be thrown.

For example, consider the following (extremely simple) `Main.java` file:

```java
public class Main {
    public static void main(String[] args) {
        System.out.println(String.format("Main.class running on %s", System.getProperty("java.version")));
    }
}
```

We've got almost nothing going on in this code.  When the `main(String[] args)` method is executed, we print out a confirmation message that also includes the `java.version` property value, which will indicate the _currently executing_ Java version.  Thus, on my own system that is running Java 8, executing the `main(String[] args)` method produces the following output:

```
Main.class running on 1.8.0_131
```

This means that I'm running on the latest stable major version `1.8` (aka `8`), with a minor version of `131`.  I happen to have a the unstable `Java 9` installed, which I'm able to verify by running the `update-alternatives` command from the terminal:

```bash
$ sudo update-alternatives --config java

There are 4 choices for the alternative java (providing /usr/bin/java).

  Selection    Path                                            Priority   Status
------------------------------------------------------------
* 0            /usr/lib/jvm/oracle_jdk8/jre/bin/java            2000      auto mode
  1            /usr/lib/jvm/java-7-openjdk-amd64/jre/bin/java   1071      manual mode
  2            /usr/lib/jvm/java-8-openjdk-amd64/jre/bin/java   1081      manual mode
  3            /usr/lib/jvm/java-9-openjdk-amd64/bin/java       1091      manual mode
  4            /usr/lib/jvm/oracle_jdk8/jre/bin/java            2000      manual mode
```

I can use the unstable `Java 9` version by selecting it from the prompt.  After doing so, let's execute the `Main` class again and see what we get:

```bash
$ java Main
Main.class running on 9-internal
```

Cool!  Since it's preferable to use the stable release in most cases, I'll switch back to `Java 8` for now.

Now, what if we want to compile an application using a specific version of Java?  We can accomplish that by providing a few options flags to the [`javac`](http://docs.oracle.com/javase/8/docs/technotes/tools/windows/javac.html) compiler.

For example, if we wanted to compile the `Main` class that was written in version `1.7` we'd use the `-source 1.7` flag:

```bash
$ javac -source 1.7 Main.java
warning: [options] bootstrap class path not set in conjunction with -source 1.7
1 warning
```

As you can see, we're given a warning in indicating that we need to specify the bootstrap path.  The `bootstrap` classes are a collection of classes packaged up into an [`rt.jar`](http://docs.oracle.com/javase/7/docs/technotes/tools/solaris/jdkfiles.html) file, which is included with each `JRE` or `JDK` installation.  This file essentially houses all the runtime classes that comprise the core Java API.  Thus, the `javac` compiler needs to know where it can find the `rt.jar` file that is from the matching Java version we're compiling from.

Let's now try adding the `-bootclasspath` option and tell the compiler where to look.  Since no output is typically shown when successfully compiling, we also added the `-verbose` flag to see what's going on:

```bash
$ javac -source 1.7 -verbose -bootclasspath /usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar Main.java
[parsing started RegularFileObject[Main.java]]
[parsing completed 11ms]
[search path for source files: .]
[search path for class files: /usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/cldrdata.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/dnsns.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/jaccess.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/jfxrt.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/localedata.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/nashorn.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunec.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunjce_provider.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunpkcs11.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/zipfs.jar,.]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Object.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/String.class)]]
[checking Main]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Serializable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/AutoCloseable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/System.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Comparable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/CharSequence.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/util/Locale.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/PrintStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Appendable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Closeable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/FilterOutputStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/OutputStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Flushable.class)]]
[wrote RegularFileObject[Main.class]]
[total 127ms]
```

This confirms that our `Main.class` was successfully compiled, so let's try executing it:

```bash
$ java Main
Main.class running on 1.8.0_131
```

Interesting.  So, even though we explicitly sourced this compile using version `1.7`, it was still able to execute just fine on our version `1.8` installation.  As previously mentioned, this is expected due to backward compatibility of most newer Java versions.

Let's change our `JVM` to run on version `1.7`, and try executing the `Main` class again:

```bash
update-alternatives: using /usr/lib/jvm/java-7-openjdk-amd64/jre/bin/java to provide /usr/bin/java (java) in manual mode

$ java Main
Main.class running on 1.7.0_95
```

As expected, we're able to run our script sourced and targeting version `1.7` in a `JVM` using `Java 7` (i.e. `1.7`).  Now, let's try compiling _from_ one version, but _targeting_ a different version.  We can do this by adding the `-target` flag to the `javac` command:

```bash
$ javac -source 1.7 -target 1.8 -verbose -bootclasspath /usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar
 Main.java
[parsing started RegularFileObject[Main.java]]
[parsing completed 16ms]
[search path for source files: .]
[search path for class files: /usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/cldrdata.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/dnsns.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/jaccess.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/jfxrt.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/localedata.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/nashorn.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunec.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunjce_provider.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/sunpkcs11.jar,/usr/lib/jvm/oracle_jdk8/jre/lib/ext/zipfs.jar,.]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Object.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/String.class)]]
[checking Main]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Serializable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/AutoCloseable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/System.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Comparable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/CharSequence.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/util/Locale.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/PrintStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/lang/Appendable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Closeable.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/FilterOutputStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/OutputStream.class)]]
[loading ZipFileIndexFileObject[/usr/lib/jvm/java-7-openjdk-amd64/jre/lib/rt.jar(java/io/Flushable.class)]]
[wrote RegularFileObject[Main.class]]
[total 145ms]
```

Here we're `sourcing` from version `1.7`, but `targeting` version `1.8`.  Given that we're still running our `JVM` on version `1.7`, what happens now when we try to execute the `Main` class?

```bash
$ java Main
Exception in thread "main" java.lang.UnsupportedClassVersionError: Main : Unsupported major.minor version 52.0
```

Lo and behold, our friend the `UnsupportedClassVersionError` finally rears its ugly head.  As the error message indicates, the `Main.class` file is targeting a `major.minor version` that is not supported.  Obviously the number `52.0` that is provided doesn't look anything like the `1.7` and `1.8` versions we've been seeing up to now.  This is merely because of how the actual [bytecode of a Java `.class` file](https://en.wikipedia.org/wiki/Java_class_file#General_layout) is setup.  Specifically, the `major` version of `52` is equivalent to `Java SE 8`, whereas a version of `51` would be `Java SE 7`.  In other words, the error is telling us `Java 8` (`1.8`) is unsupported, because its a newer version than the `1.7` we're currently running on, _and_ because the `Main.class` was compiled as `1.7`.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java UnsupportedClassVersionError, with command and code samples illustrating how compilation across Java versions works.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
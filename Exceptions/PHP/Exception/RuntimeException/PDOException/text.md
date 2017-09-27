# PHP Exception Handling - PDOException

Moving along through our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll be going over the **PDOException**.  `PHP Data Objects` (or `PDO`) are a collection of APIs and interfaces that attempt to streamline and consolidate the various ways databases can be accessed and manipulated into a singular package.  Thus, the `PDOException` is thrown anytime something goes wrong while using the [`PDO`](http://php.net/manual/en/intro.pdo.php) class, or related extensions.

In this article we'll examine the `PDOException` by first looking at where it resides in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), then we'll take a closer look at some fully functional code that will illustrate the basics of using `PDO` for database manipulation, and how that might lead to `PDOExceptions`, so let's get to it!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `PDOException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("Logging.php");

/**
 * Class Connector
 *
 * Helper class for establishing PDO connections and making queries.
 */
class Connector
{
    private $connection;
    private $statement;

    /**
     * Connector constructor.
     *
     * @param string $dbname
     * @param string $username
     * @param string $password
     * @param string $uri
     * @param string $host
     */
    public function __construct(string $dbname, string $username, string $password, string $uri = 'mysql', string $host = 'localhost')
    {
        $this->connection = new PDO("$uri:dbname=$dbname;host=$host", $username, $password);

    }

    /**
     * Execute the passed query as PDO statement.
     *
     * @param string $query
     */
    public function executeQuery(string $query)
    {
        $this->statement = $this->connection->prepare($query);
        $this->statement->execute();
    }

    /**
     * Fetch results of current statement using passed FETCH_ return type.
     *
     * @param int $returnType see: http://php.net/manual/en/pdo.constants.php
     * @return array
     */
    public function fetchResults(int $returnType = PDO::FETCH_OBJ)
    {
        $results = [];
        while ($row = $this->statement->fetch($returnType)) {
            $results[] = $row;
        }
        return $results;
    }
}

function executeExamples()
{
    Logging::LineSeparator("OUTPUT ACTORS");
    outputActors();

    Logging::LineSeparator("INVALID PASSWORD");
    invalidPassword();

    Logging::LineSeparator("INVALID DATABASE");
    invalidDatabase();
}

function outputActors()
{
    try {
        // Create connector helper.
        $connector = new Connector('sakila', 'dev', 'password');
        // Execute query.
        $connector->executeQuery('SELECT * FROM actor ORDER BY last_name LIMIT 5');
        // Fetch results of query.
        $results = $connector->fetchResults();
        // Output results.
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function invalidPassword()
{
    try {
        $connector = new Connector('sakila', 'dev', 'hunter2');
        $connector->executeQuery('SELECT * FROM film ORDER BY release_year LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function invalidDatabase()
{
    try {
        $connector = new Connector('askila', 'dev', 'password');
        $connector->executeQuery('SELECT * FROM city ORDER BY city LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
```

```php
<?php
// Logging.php
require('kint.php');

/**
 * Provides basic logging/output functionality.
 */
class Logging {

    /**
     * Logs the passed object, string, or Throwable instance to the console.
     *
     * @param object|string $a Message or value to be logged.
     * @param object|bool $b Secondary value, such as boolean for Throwables indicating if error was expected.
     */
    public static function Log($a, $b = null) {
        if (is_string($a) || is_numeric($a)) {
            Logging::LogString($a);
        } elseif ($a instanceof Throwable) {
            Logging::LogThrowable($a, is_null($b) ? true : $b);
        } else {
            Logging::LogObject($a);
        }
    }

    /**
     * Logs the passed object.
     *
     * @param mixed $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject($object) {
        Kint_Renderer_Cli::$force_utf8 = true;
        Kint_Renderer_Text::$decorations = false;
        Kint::dump($object);
    }

    /**
     * Logs the passed string value.
     *
     * @param string $value Value to be logged.
     */
    private static function LogString(string $value) {
        print_r("{$value}\n");
    }

    /**
     * Logs the passed Throwable object.  
     * Includes message, className if error was expected, and stack trace.
     *
     * Uses internal Reflection to retrieve protected/private properties.
     *
     * @param Throwable $throwable Throwable object to be output.
     * @param bool $expected Indicates if error was expected or not.
     */
    private static function LogThrowable(Throwable $throwable, bool $expected = true) {
        $expected = $expected ? "EXPECTED" : "UNEXPECTED";
        $message = substr($throwable->xdebug_message, 1);
        // Output whether error was expected or not, the class name, the message, and stack trace.
        print_r("[{$expected}] {$message}\n");
        // Add line separator to keep it tidy.
        self::LineSeparator();
    }

    /**
     * Outputs a dashed line separator with
     * inserted text centered in the middle.
     *
     * @param array ...$args Insert, length, and separator character.
     */
    public static function LineSeparator(...$args) {
        $insert = empty($args[0]) ? "" : $args[0];
        $length = empty($args[1]) ? 40 : $args[1];
        $separator = empty($args[2]) ? '-' : $args[2];

        $output = $insert;

        if (strlen($insert) == 0) {
            $output = str_repeat($separator, $length);
        } elseif (strlen($insert) < $length) {
            // Update length based on insert length, less a space for margin.
            $length -= (strlen($insert) + 2);
            // Halve the length and floor left side.
            $left = floor($length / 2);
            $right = $left;
            // If odd number, add dropped remainder to right side.
            if ($length % 2 != 0) $right += 1;

            // Create separator strings.
            $left = str_repeat($separator, $left);
            $right = str_repeat($separator, $right);

            // Surround insert with separators.
            $output = "{$left} {$insert} {$right}";
        }

        print_r("{$output}\n");
    }
}
```

## When Should You Use It?

As stated by the [official documentation](http://php.net/manual/en/class.pdoexception.php), the `PDOException` should never be directly thrown by your own code.  Instead, it is thrown by the various `PDO` extensions whenever an error occurs while handling a database connection or query.  To understand what this means, let's first take a look at how `PDO` works, and why it might be better than "traditional" database handling in PHP.

At the most basic level, `PDO` is merely an abstraction layer between your PHP code and the underlying database your code is accessing.  The beauty of this abstraction is that your code can be written, for the most part, in a database-independent manner.  Connecting to a database, performing a query, retrieving the results, and other common tasks are all performed identically -- using the same objects and methods -- within `PDO`.  Thus, if your application needs to transition from, say, MySQL to PostgreSQL, only a few minor adjustments will need to be made to keep most data-access code functional.

Another big advantage to `PDO` is its built-in support for `data binding` techniques.  Data binding effectively allows your application code to automatically synchronize local data to its database source objects, and vice versa.  When a change is made client-side, such as a user altering a value, a data binding can automatically update that matching value in the database, with (relatively) little effort on the part of the developer to make this happen.

There are many other advantages of `PDO`, but, for now, let's jump into the example code to see how it works.  For this setup we're running MySQL 5.7 with the [`sakila sample database`](https://dev.mysql.com/doc/sakila/en/) for testing purposes.  Our basic goal is to use the [`PDO_MYSQL`](http://php.net/manual/en/ref.pdo-mysql.php) driver to allow us to connect to a MySQL database via `PDO`, just as we would with any of the other PDO-supported database types.

To simplify the process of establishing a PDO connection and execute some queries, we've created a basic `Connector` helper class:

```php
/**
 * Class Connector
 *
 * Helper class for establishing PDO connections and making queries.
 */
class Connector
{
    private $connection;
    private $statement;

    /**
     * Connector constructor.
     *
     * @param string $dbname
     * @param string $username
     * @param string $password
     * @param string $uri
     * @param string $host
     */
    public function __construct(string $dbname, string $username, string $password, string $uri = 'mysql', string $host = 'localhost')
    {
        $this->connection = new PDO("$uri:dbname=$dbname;host=$host", $username, $password);

    }

    /**
     * Execute the passed query as PDO statement.
     *
     * @param string $query
     */
    public function executeQuery(string $query)
    {
        $this->statement = $this->connection->prepare($query);
        $this->statement->execute();
    }

    /**
     * Fetch results of current statement using passed FETCH_ return type.
     *
     * @param int $returnType see: http://php.net/manual/en/pdo.constants.php
     * @return array
     */
    public function fetchResults(int $returnType = PDO::FETCH_OBJ)
    {
        $results = [];
        while ($row = $this->statement->fetch($returnType)) {
            $results[] = $row;
        }
        return $results;
    }
}
```

The constructor accepts most of the connection-based parameters and creates a new `PDO` instance, which automatically attempts to connect to the specified data source.  `executeQuery(string $query)` prepares and then executes the passed `$query` string, while `fetchResults(int $returnType = PDO::FETCH_OBJ)` attempts to fetch the results of the previously provided `statement`, if applicable.  It's worth noting that the [`fetch($fetch_style, ...)`](http://php.net/manual/en/pdostatement.fetch.php) method's first parameter determines how the retrieved data is formatted before being returned.  There are tons of different options here, but we've opted for the `FETCH_OBJ` mode as default, which returns an anonymous class containing a field for each column and value.  However, this is where data binding can be easily implemented.  For example, `PDO::FETCH_CLASS` can be used to automatically create a new instance of a specified PHP class, mapping the retrieved values to their respective properties within the class definition.  Pretty handy!

Anyway, we don't need anything that fancy right now, so let's test out our `Connector` class by trying to fetch a handful of actors from the `sakila` sample database:

```php
function outputActors()
{
    try {
        // Create connector helper.
        $connector = new Connector('sakila', 'dev', 'password');
        // Execute query.
        $connector->executeQuery('SELECT * FROM actor ORDER BY last_name LIMIT 5');
        // Fetch results of query.
        $results = $connector->fetchResults();
        // Output results.
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Everything looks good, so executing the `outputActors()` function works as intended, outputting the first alphabetical actors in the table:

```
------------ OUTPUT ACTORS -------------
array (5) [
    0 => stdClass (4) (
        public 'actor_id' -> string (3) "182"
        public 'first_name' -> string (6) "DEBBIE"
        public 'last_name' -> string (6) "AKROYD"
        public 'last_update' -> string (19) "2006-02-15 04:34:33"
    )
    1 => stdClass (4) (
        public 'actor_id' -> string (2) "58"
        public 'first_name' -> string (9) "CHRISTIAN"
        public 'last_name' -> string (6) "AKROYD"
        public 'last_update' -> string (19) "2006-02-15 04:34:33"
    )
    2 => stdClass (4) (
        public 'actor_id' -> string (2) "92"
        public 'first_name' -> string (7) "KIRSTEN"
        public 'last_name' -> string (6) "AKROYD"
        public 'last_update' -> string (19) "2006-02-15 04:34:33"
    )
    3 => stdClass (4) (
        public 'actor_id' -> string (3) "145"
        public 'first_name' -> string (3) "KIM"
        public 'last_name' -> string (5) "ALLEN"
        public 'last_update' -> string (19) "2006-02-15 04:34:33"
    )
    4 => stdClass (4) (
        public 'actor_id' -> string (3) "118"
        public 'first_name' -> string (4) "CUBA"
        public 'last_name' -> string (5) "ALLEN"
        public 'last_update' -> string (19) "2006-02-15 04:34:33"
    )
]
```

However, let's see what happens if we try a connection with an invalid password:

```php
function invalidPassword()
{
    try {
        $connector = new Connector('sakila', 'dev', 'hunter2');
        $connector->executeQuery('SELECT * FROM film ORDER BY release_year LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Unsurprisingly, this throws a new `PDOException`, indicating that database access was denied:

```
----------- INVALID PASSWORD -----------
[EXPECTED] PDOException: SQLSTATE[HY000] [1045] Access denied for user 'dev'@'localhost' (using password: YES)
```

Similarly, here we have a small typo in the name of our database, so let's see what happens when we try to connect to the `askila` database to collect some `city` info:

```php
function invalidDatabase()
{
    try {
        $connector = new Connector('askila', 'dev', 'password');
        $connector->executeQuery('SELECT * FROM city ORDER BY city LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Once again, a `PDOException` is thrown that indicates the particular problem:

```
----------- INVALID DATABASE -----------
[EXPECTED] PDOException: SQLSTATE[HY000] [1049] Unknown database 'askila'
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PDOException class in PHP, including functional code samples illustrating how to use PDO and catch PDOExceptions.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php
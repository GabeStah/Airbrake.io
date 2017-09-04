# PHP Exception Handling - PharException

Making our way through our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll explore the PharException.  `Phar` is shorthand for `PHP Archive`.  A `phar` file is a convenient way to compress and store an entire PHP application in a single executable file.  This functionality is similar to [`JAR`](https://docs.oracle.com/javase/tutorial/deployment/jar/basicsindex.html) files found within the Java ecosystem.

Throughout this article we'll examine what a `PharException` might be used for, starting with a brief look at where it sits in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also dig into how `phars` are created and executed, so we can show how `PharExceptions` are most commonly thrown.  Let's get going!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - `PharException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("Logging.php");

/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publicationMonth;
    private $publicationYear;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Minimum publication month.
    const PUBLICATION_MONTH_MIN = 1;
    // Maximum publication month.
    const PUBLICATION_MONTH_MAX = 12;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount, int $publicationMonth, int $publicationYear) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationMonth($publicationMonth);
        $this->setPublicationYear($publicationYear);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Book author.
     */
    public function getAuthor(): string {
        return $this->author;
    }

    /**
     * Set the author.
     *
     * @param string $value Author value to be set.
     */
    public function setAuthor(string $value) {
        // Check if length exceeds maximum.
        if (strlen($value) > self::AUTHOR_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::AUTHOR_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Author containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->author = $value;
    }

    /**
     * Get the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Set the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the month of publication.
     *
     * @return int Numeric publication month.
     */
    public function getPublicationMonth(): int {
        return $this->publicationMonth;
    }

    /**
     * Set the month of publication.
     *
     * @param int $month Numeric publication month.
     */
    public function setPublicationMonth(int $month) {
        if ($month < self::PUBLICATION_MONTH_MIN || $month > self::PUBLICATION_MONTH_MAX) {
            throw new OutOfRangeException("Invalid publication month: $month.  Must be between " . self::PUBLICATION_MONTH_MIN . " and " . self::PUBLICATION_MONTH_MAX, E_COMPILE_ERROR);
        }
        $this->publicationMonth = $month;
    }

    /**
     * Get the year of publication.
     *
     * @return int Numeric publication year.
     */
    public function getPublicationYear(): int {
        return $this->publicationYear;
    }

    /**
     * Set the year of publication.
     *
     * @param int $year Numeric publication year.
     */
    public function setPublicationYear(int $year) {
        $this->publicationYear = $year;
    }

    /**
     * Get the title.
     *
     * @return string Book title.
     */
    public function getTitle(): string {
        return $this->title;
    }

    /**
     * Set the title.
     *
     * @param string $value Title value to be set.
     */
    public function setTitle(string $value) {
        // Check if length exceeds maximum.
        if (strlen($value) > self::TITLE_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::TITLE_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Title containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->title = $value;
    }

    /**
     * Magic method triggers when inaccessible instance method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public function __call(string $name, array $args) {
        throw new BadMethodCallException("Instance method Book->$name() doesn't exist");
    }

    /**
     * Magic method triggers when inaccessible static method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public static function __callstatic(string $name, array $args) {
        throw new BadMethodCallException("Static method Book::$name() doesn't exist");
    }
}

function executeExamples()
{
    try {
        Logging::LineSeparator("A SONG OF ICE AND FIRE");
        Logging::Log(new Book("A Game of Thrones", "George R.R. Martin", 848, 8, 1996));
        Logging::Log(new Book("A Clash of Kings", "George R.R. Martin", 761, 11, 1998));
        Logging::Log(new Book("A Storm of Swords", "George R.R. Martin", 1177, 8, 2000));
    } catch (OutOfRangeException $exception) {
        // Output expected OutOfRangeExceptions.
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
        print_r($object);
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

```php
<?php
// createAdvancedPhar.php

include("D:\work\Airbrake.io\lib\php\Logging.php");

try {
    # Set the archive name.
    $phar = new Phar("advanced.phar");

    # Begin buffering phar creation.
    $phar->startBuffering();

    # Add files within current directory.
    $phar->buildFromDirectory(dirname(__FILE__));

    # Set stub to contents of stub.php file.
    $phar->setStub(file_get_contents("stub.php"));

    # Finish buffering phar creation.
    $phar->stopBuffering();
} catch (PharException $exception) {
    // Output expected PharException.
    Logging::Log($exception);
} catch (Exception $exception) {
    // Output unexpected Exceptions.
    Logging::Log($exception, false);
}
```

```php
#!/usr/bin/env php
<?php
// stub.php

$web = 'index.php';

if (in_array('phar', stream_get_wrappers()) && class_exists('Phar', 0)) {
    Phar::interceptFileFuncs();
    set_include_path('phar://' . __FILE__ . PATH_SEPARATOR . get_include_path());
    Phar::webPhar(null, $web);
    include 'phar://' . __FILE__ . '/' . Extract_Phar::START;
    return;
}

if (@(isset($_SERVER['REQUEST_URI']) && isset($_SERVER['REQUEST_METHOD']) && ($_SERVER['REQUEST_METHOD'] == 'GET' || $_SERVER['REQUEST_METHOD'] == 'POST'))) {
    Extract_Phar::go(true);
    $mimes = array(
        'phps' => 2,
        'c' => 'text/plain',
        'cc' => 'text/plain',
        'cpp' => 'text/plain',
        'c++' => 'text/plain',
        'dtd' => 'text/plain',
        'h' => 'text/plain',
        'log' => 'text/plain',
        'rng' => 'text/plain',
        'txt' => 'text/plain',
        'xsd' => 'text/plain',
        'php' => 1,
        'inc' => 1,
        'avi' => 'video/avi',
        'bmp' => 'image/bmp',
        'css' => 'text/css',
        'gif' => 'image/gif',
        'htm' => 'text/html',
        'html' => 'text/html',
        'htmls' => 'text/html',
        'ico' => 'image/x-ico',
        'jpe' => 'image/jpeg',
        'jpg' => 'image/jpeg',
        'jpeg' => 'image/jpeg',
        'js' => 'application/x-javascript',
        'midi' => 'audio/midi',
        'mid' => 'audio/midi',
        'mod' => 'audio/mod',
        'mov' => 'movie/quicktime',
        'mp3' => 'audio/mp3',
        'mpg' => 'video/mpeg',
        'mpeg' => 'video/mpeg',
        'pdf' => 'application/pdf',
        'png' => 'image/png',
        'swf' => 'application/shockwave-flash',
        'tif' => 'image/tiff',
        'tiff' => 'image/tiff',
        'wav' => 'audio/wav',
        'xbm' => 'image/xbm',
        'xml' => 'text/xml',
    );

    header("Cache-Control: no-cache, must-revalidate");
    header("Pragma: no-cache");

    $basename = basename(__FILE__);
    if (!strpos($_SERVER['REQUEST_URI'], $basename)) {
        chdir(Extract_Phar::$temp);
        include $web;
        return;
    }
    $pt = substr($_SERVER['REQUEST_URI'], strpos($_SERVER['REQUEST_URI'], $basename) + strlen($basename));
    if (!$pt || $pt == '/') {
        $pt = $web;
        header('HTTP/1.1 301 Moved Permanently');
        header('Location: ' . $_SERVER['REQUEST_URI'] . '/' . $pt);
        exit;
    }
    $a = realpath(Extract_Phar::$temp . DIRECTORY_SEPARATOR . $pt);
    if (!$a || strlen(dirname($a)) < strlen(Extract_Phar::$temp)) {
        header('HTTP/1.0 404 Not Found');
        echo "<html>\n <head>\n  <title>File Not Found<title>\n </head>\n <body>\n  <h1>404 - File ", $pt, " Not Found</h1>\n </body>\n</html>";
        exit;
    }
    $b = pathinfo($a);
    if (!isset($b['extension'])) {
        header('Content-Type: text/plain');
        header('Content-Length: ' . filesize($a));
        readfile($a);
        exit;
    }
    if (isset($mimes[$b['extension']])) {
        if ($mimes[$b['extension']] === 1) {
            include $a;
            exit;
        }
        if ($mimes[$b['extension']] === 2) {
            highlight_file($a);
            exit;
        }
        header('Content-Type: ' .$mimes[$b['extension']]);
        header('Content-Length: ' . filesize($a));
        readfile($a);
        exit;
    }
}

class Extract_Phar
{
    static $temp;
    static $origdir;
    const GZ = 0x1000;
    const BZ2 = 0x2000;
    const MASK = 0x3000;
    const START = 'code.php';
    const LEN = 6652;

    static function go($return = false)
    {
        $fp = fopen(__FILE__, 'rb');
        fseek($fp, self::LEN);
        $L = unpack('V', $a = fread($fp, 4));
        $m = '';

        do {
            $read = 8192;
            if ($L[1] - strlen($m) < 8192) {
                $read = $L[1] - strlen($m);
            }
            $last = fread($fp, $read);
            $m .= $last;
        } while (strlen($last) && strlen($m) < $L[1]);

        if (strlen($m) < $L[1]) {
            die('ERROR: manifest length read was "' .
                strlen($m) .'" should be "' .
                $L[1] . '"');
        }

        $info = self::_unpack($m);
        $f = $info['c'];

        if ($f & self::GZ) {
            if (!function_exists('gzinflate')) {
                die('Error: zlib extension is not enabled -' .
                    ' gzinflate() function needed for zlib-compressed .phars');
            }
        }

        if ($f & self::BZ2) {
            if (!function_exists('bzdecompress')) {
                die('Error: bzip2 extension is not enabled -' .
                    ' bzdecompress() function needed for bz2-compressed .phars');
            }
        }

        $temp = self::tmpdir();

        if (!$temp || !is_writable($temp)) {
            $sessionpath = session_save_path();
            if (strpos ($sessionpath, ";") !== false)
                $sessionpath = substr ($sessionpath, strpos ($sessionpath, ";")+1);
            if (!file_exists($sessionpath) || !is_dir($sessionpath)) {
                die('Could not locate temporary directory to extract phar');
            }
            $temp = $sessionpath;
        }

        $temp .= '/pharextract/'.basename(__FILE__, '.phar');
        self::$temp = $temp;
        self::$origdir = getcwd();
        @mkdir($temp, 0777, true);
        $temp = realpath($temp);

        if (!file_exists($temp . DIRECTORY_SEPARATOR . md5_file(__FILE__))) {
            self::_removeTmpFiles($temp, getcwd());
            @mkdir($temp, 0777, true);
            @file_put_contents($temp . '/' . md5_file(__FILE__), '');

            foreach ($info['m'] as $path => $file) {
                $a = !file_exists(dirname($temp . '/' . $path));
                @mkdir(dirname($temp . '/' . $path), 0777, true);
                clearstatcache();

                if ($path[strlen($path) - 1] == '/') {
                    @mkdir($temp . '/' . $path, 0777);
                } else {
                    file_put_contents($temp . '/' . $path, self::extractFile($path, $file, $fp));
                    @chmod($temp . '/' . $path, 0666);
                }
            }
        }

        chdir($temp);

        if (!$return) {
            include self::START;
        }
    }

    static function tmpdir()
    {
        if (strpos(PHP_OS, 'WIN') !== false) {
            if ($var = getenv('TMP') ? getenv('TMP') : getenv('TEMP')) {
                return $var;
            }
            if (is_dir('/temp') || mkdir('/temp')) {
                return realpath('/temp');
            }
            return false;
        }
        if ($var = getenv('TMPDIR')) {
            return $var;
        }
        return realpath('/tmp');
    }

    static function _unpack($m)
    {
        $info = unpack('V', substr($m, 0, 4));
        $l = unpack('V', substr($m, 10, 4));
        $m = substr($m, 14 + $l[1]);
        $s = unpack('V', substr($m, 0, 4));
        $o = 0;
        $start = 4 + $s[1];
        $ret['c'] = 0;

        for ($i = 0; $i < $info[1]; $i++) {
            $len = unpack('V', substr($m, $start, 4));
            $start += 4;
            $savepath = substr($m, $start, $len[1]);
            $start += $len[1];
            $ret['m'][$savepath] = array_values(unpack('Va/Vb/Vc/Vd/Ve/Vf', substr($m, $start, 24)));
            $ret['m'][$savepath][3] = sprintf('%u', $ret['m'][$savepath][3]
                & 0xffffffff);
            $ret['m'][$savepath][7] = $o;
            $o += $ret['m'][$savepath][2];
            $start += 24 + $ret['m'][$savepath][5];
            $ret['c'] |= $ret['m'][$savepath][4] & self::MASK;
        }
        return $ret;
    }

    static function extractFile($path, $entry, $fp)
    {
        $data = '';
        $c = $entry[2];

        while ($c) {
            if ($c < 8192) {
                $data .= @fread($fp, $c);
                $c = 0;
            } else {
                $c -= 8192;
                $data .= @fread($fp, 8192);
            }
        }

        if ($entry[4] & self::GZ) {
            $data = gzinflate($data);
        } elseif ($entry[4] & self::BZ2) {
            $data = bzdecompress($data);
        }

        if (strlen($data) != $entry[0]) {
            die("Invalid internal .phar file (size error " . strlen($data) . " != " .
                $stat[7] . ")");
        }

        if ($entry[3] != sprintf("%u", crc32($data) & 0xffffffff)) {
            die("Invalid internal .phar file (checksum error)");
        }

        return $data;
    }

    static function _removeTmpFiles($temp, $origdir)
    {
        chdir($temp);

        foreach (glob('*') as $f) {
            if (file_exists($f)) {
                is_dir($f) ? @rmdir($f) : @unlink($f);
                if (file_exists($f) && is_dir($f)) {
                    self::_removeTmpFiles($f, getcwd());
                }
            }
        }

        @rmdir($temp);
        clearstatcache();
        chdir($origdir);
    }
}

Extract_Phar::go();
__HALT_COMPILER(); ?>
```

## When Should You Use It?

Before we can understand what might cause a `PharException`, we should briefly go over how a `phar` is created and how it can be executed.  To create a `phar` file we just need to use the [`Phar`](http://php.net/manual/en/class.phar.php) class.  To start, here we have the `createInvalidPhar.php` script:

```php
<?php
// createInvalidPhar.php

include("D:\work\Airbrake.io\lib\php\Logging.php");

try {
    # Set the archive name.
    $phar = new Phar("invalid.phar");

    # Begin buffering phar creation.
    $phar->startBuffering();

    # Add files within current directory.
    $phar->buildFromDirectory(dirname(__FILE__));

    # Set invalid stub.
    $phar->setStub("");

    # Finish buffering phar creation.
    $phar->stopBuffering();
} catch (PharException $exception) {
    // Output expected PharException.
    Logging::Log($exception);
} catch (Exception $exception) {
    // Output unexpected Exceptions.
    Logging::Log($exception, false);
}
```

As you can deduce by the file name, this configuration isn't quite right to create a valid `phar` file.  As it happens, if we execute the above script we get a `PharException`, indicating we've attempted to create an invalid `stub` for our `phar` file:

```
[EXPECTED] PharException: illegal stub for phar "D:/work/Airbrake.io/Exceptions/PHP/Exception/PharException/invalid.phar" in D:\work\Airbrake.io\Exceptions\PHP\Exception\PharException\createInvalidPhar.php on line 17
```

As it happens, a `phar` archive must be made up of at least three components:

- A manifest describing the archive contents.
- The file contents.
- A simple PHP file called the `stub`.

The [`stub`](http://php.net/manual/en/phar.fileformat.stub.php) can contain just about any code, but, _at minimum_, it must at least contain an opening PHP tag and the `__HALT_COMPILER();` token:

```php
<?php __HALT_COMPILER();
```

In the `createInvalidPhar.php` script above, you'll notice the call to `setStub("")` attempts to create an empty `stub`, which is invalid, hence the `PharException`.

Let's fix this issue in the `createMinimalPhar.php` script:

```php
<?php
// createMinimalPhar.php

include("D:\work\Airbrake.io\lib\php\Logging.php");

try {
    # Set the archive name.
    $phar = new Phar("minimal.phar");

    # Begin buffering phar creation.
    $phar->startBuffering();

    # Add files within current directory.
    $phar->buildFromDirectory(dirname(__FILE__));

    # Set stub to minimal.
    $phar->setStub("<?php __HALT_COMPILER();");

    # Finish buffering phar creation.
    $phar->stopBuffering();
} catch (PharException $exception) {
    // Output expected PharException.
    Logging::Log($exception);
} catch (Exception $exception) {
    // Output unexpected Exceptions.
    Logging::Log($exception, false);
}
```

Now that we have the minimal `stub` contents included, executing this script works fine and a new `minimal.phar` file is created within the project directory.  However, what happens if we try to execute it?

```bash
$ php minimal.phar
$
```

Nothing.  While the `minimal.phar` file contains all the contents of our project directory, it doesn't have any instruction on what file to run when executed.  Let's add a bit more now in the `createBasicPhar.php` script:

```php
<?php
// createBasicPhar.php

include("D:\work\Airbrake.io\lib\php\Logging.php");

try {
    # Set the archive name.
    $phar = new Phar("basic.phar");

    # Begin buffering phar creation.
    $phar->startBuffering();

    # Add files within current directory.
    $phar->buildFromDirectory(dirname(__FILE__));

    # Create basic stub and assign default executable file.
    $stub = $phar->createDefaultStub('code.php');

    # Add the header to enable execution.
    $stub = "#!/usr/bin/env php \n" . $stub;

    # Set stub.
    $phar->setStub($stub);

    # Finish buffering phar creation.
    $phar->stopBuffering();
} catch (PharException $exception) {
    // Output expected PharException.
    Logging::Log($exception);
} catch (Exception $exception) {
    // Output unexpected Exceptions.
    Logging::Log($exception, false);
}
```

Here we've added a couple more lines to allow the produced `phar` to be executable.  Calling `createDefaultStub($indexfile = null)` allows us to specify the `index` (or default) file that will be executed when executing this `phar`.  We also need to make sure it can be executed via the `php` command from the terminal, so we prefix the `stub` contents with `"#!/usr/bin/env php \n"`.  Everything else is the same as before, so now let's try executing the `basic.phar` file that was just created:

```bash
$ php basic.phar
-------- A SONG OF ICE AND FIRE --------
Book Object
(
    [author:Book:private] => George R.R. Martin
    [pageCount:Book:private] => 848
    [publicationMonth:Book:private] => 8
    [publicationYear:Book:private] => 1996
    [title:Book:private] => A Game of Thrones
)
Book Object
(
    [author:Book:private] => George R.R. Martin
    [pageCount:Book:private] => 761
    [publicationMonth:Book:private] => 11
    [publicationYear:Book:private] => 1998
    [title:Book:private] => A Clash of Kings
)
Book Object
(
    [author:Book:private] => George R.R. Martin
    [pageCount:Book:private] => 1177
    [publicationMonth:Book:private] => 8
    [publicationYear:Book:private] => 2000
    [title:Book:private] => A Storm of Swords
)
```

The `code.php` file just creates a few `Book` objects and outputs them to the log, so that's what is expected and exactly what we see above.  This confirms that we were able to specify the `code.php` file should be the default file called when our `basic.phar` file is executed.

Finally, let's take a look at the `createAdvancedPhar.php` script:

```php
<?php
// createAdvancedPhar.php

include("D:\work\Airbrake.io\lib\php\Logging.php");

try {
    # Set the archive name.
    $phar = new Phar("advanced.phar");

    # Begin buffering phar creation.
    $phar->startBuffering();

    # Add files within current directory.
    $phar->buildFromDirectory(dirname(__FILE__));

    # Set stub to contents of stub.php file.
    $phar->setStub(file_get_contents("stub.php"));

    # Finish buffering phar creation.
    $phar->stopBuffering();
} catch (PharException $exception) {
    // Output expected PharException.
    Logging::Log($exception);
} catch (Exception $exception) {
    // Output unexpected Exceptions.
    Logging::Log($exception, false);
}
```

Here we're illustrating that, rather than creating a stub via the `createDefaultStub()` function, we can manually create our own stub file, which we've added to `stub.php` in our project directory.  I won't include the full contents of this file here (scroll up to the full source to see it), but here's a snippet:

```php
#!/usr/bin/env php
<?php
// stub.php

$web = 'index.php';

if (in_array('phar', stream_get_wrappers()) && class_exists('Phar', 0)) {
    Phar::interceptFileFuncs();
    set_include_path('phar://' . __FILE__ . PATH_SEPARATOR . get_include_path());
    Phar::webPhar(null, $web);
    include 'phar://' . __FILE__ . '/' . Extract_Phar::START;
    return;
}

// ...

Extract_Phar::go();
__HALT_COMPILER(); ?>
```

This is basically just the default stub code, placed into the `stub.php` file.  However, the ability to manually create your own stubs opens up a lot of avenues for customizing `phar` creation.  As before, running `createAdvancedPhar.php` creates a new `advanced.phar` file, which we can execute as before to produce the same `Book` collection output we saw from the `basic.phar` file.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PharException class in PHP, including functional code samples showing how to create, execute, and modify phar archives.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php
<?php

include("d:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    $number = 1234.567;
    $message = 'This is a message!';

    Logging::LineSeparator('FORMAT DATE');
    formatDate(new DateTime());
    Logging::LineSeparator('FORMAT INVALID DATE');
    formatDate(null);

    Logging::LineSeparator('FORMAT NUMBER (en_US)');
    formatNumber($number);
    Logging::LineSeparator('FORMAT NUMBER (fr_FR)');
    formatNumber($number, 'fr_FR');
    Logging::LineSeparator('FORMAT NUMBER (de_CH)');
    formatNumber($number, 'de_CH');
    Logging::LineSeparator('FORMAT INVALID NUMBER');
    formatNumber($number, 'en_US', 24601);

    Logging::LineSeparator('FORMAT CURRENCY');
    formatCurrency($number);
    Logging::LineSeparator('FORMAT INVALID CURRENCY');
    formatCurrency($number, 'ABCDE');

    Logging::LineSeparator('FORMAT MESSAGE');
    formatMessage(array($message));
    Logging::LineSeparator('FORMAT INVALID MESSAGE');
    formatMessage(array($message), 'en_US', null);
}

function executeInvalidTests() {
    $number = 1234.567;
    $message = 'This is a message!';

    Logging::LineSeparator('FORMAT INVALID DATE');
    formatDate(null);

    Logging::LineSeparator('FORMAT INVALID NUMBER');
    formatNumber($number, 'en_US', 24601);

    Logging::LineSeparator('FORMAT INVALID CURRENCY');
    formatCurrency($number, 'ABCDE');

    Logging::LineSeparator('FORMAT INVALID MESSAGE');
    formatMessage(array($message), 'en_US', null);
}

/**
 * Format currency value.
 *
 * @param mixed $value Currency value.
 * @param null|string $currency Currency type.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatCurrency($value,
                        ?string $currency = 'USD',
                        ?string $locale = 'en_US',
                        ?int $style = NumberFormatter::CURRENCY)
{
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->formatCurrency($value, $currency));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format date value.
 *
 * @param mixed $value Date value.
 * @param null|string $locale Locale.
 * @param null|string $timezone Timezone.
 * @param int|null $dateType Date type.
 * @param int|null $timeType Time type.
 * @param int|null $calendarType Calendar type.
 */
function formatDate($value,
                    ?string $locale = 'en_US',
                    ?string $timezone = 'America/Los_Angeles',
                    ?int $dateType = IntlDateFormatter::FULL,
                    ?int $timeType = IntlDateFormatter::FULL,
                    ?int $calendarType = IntlDateFormatter::GREGORIAN)
{
    try {
        $formatter = new IntlDateFormatter($locale, $dateType, $timeType, $timezone, $calendarType);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format message value.
 *
 * @param array $value Message value.
 * @param null|string $locale Locale.
 * @param null|string $pattern Message formatting pattern.
 */
function formatMessage(array $value, ?string $locale = 'en_US', ?string $pattern = '{0}') {
    try {
        $formatter = new MessageFormatter($locale, $pattern);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format number value.
 *
 * @param mixed $value Number value.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatNumber($value, ?string $locale = 'en_US', ?int $style = NumberFormatter::DECIMAL) {
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Determine if passed object is valid formatter instance.
 *
 * @param object $formatter Formatter to check.
 * @return bool
 */
function isFormatter($formatter) {
    foreach (array('IntlDateFormatter', 'MessageFormatter', 'NumberFormatter') as $class) {
        if ($formatter instanceof $class) return true;
    }
}

/**
 * Throws a new IntlException, if intl.use_exceptions setting
 * disabled, passed formatter is valid type, and error was produced.
 *
 * @param object $formatter Formatter to retrieve error from.
 * @throws IntlException
 */
function throwFormatterException($formatter) {
    // Ensure object is valid formatter.
    if (!isFormatter($formatter)) return;
    // Confirm that use_exceptions setting is disabled.
    if (ini_get('intl.use_exceptions') == 0) {
        $errorCode = $formatter->getErrorCode();
        // Check for failure.
        if (intl_is_failure($errorCode)) {
            Logging::Log("Formatter failed with error code: {$errorCode}.  Throwing exception...");
            throw new IntlException($formatter->getErrorMessage(), $errorCode);
        }
    }
}

//executeExamples();
executeInvalidTests();
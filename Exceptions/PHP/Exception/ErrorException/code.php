<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

/**
 * Access invalid file,
 * with error_reporting and set_error_handler calls.
 *
 * @param int $errorLevel Error level constant.
 * @param string|null $handlerFunc Error handler function to be used.
 */
function accessInvalidFile($errorLevel, $handlerFunc = null) {
    error_reporting($errorLevel);

    set_error_handler($handlerFunc);

    getFileContents("invalid.txt");
}

/**
 * Custom error handler.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandler($severity, $message, $file, $line) {
    throw new ErrorException($message, 0, $severity, $file, $line);
}

/**
 * Custom error handler, which checks if error
 * severity is handled by error_reporting.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandlerWithReportingCheck($severity, $message, $file, $line) {
    // Check if error number is handled by error_reporting setting.
    if (!(error_reporting() & $severity)) {
        return;
    }
    throw new ErrorException($message, 0, $severity, $file, $line);
}

/**
 * Executes examples.
 */
function executeExamples()
{
    Logging::LineSeparator("E_ERROR", 40, '=');
    accessInvalidFile(E_ERROR);

    Logging::LineSeparator("E_ERROR w/ ErrorHandler");
    accessInvalidFile(E_ERROR, 'errorHandler');

    Logging::LineSeparator("E_ERROR w/ ReportingCheck");
    accessInvalidFile(E_ERROR, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_WARNING", 40, '=');
    accessInvalidFile(E_WARNING);

    Logging::LineSeparator("E_WARNING w/ ErrorHandler");
    accessInvalidFile(E_WARNING, 'errorHandler');

    Logging::LineSeparator("E_WARNING w/ ReportingCheck");
    accessInvalidFile(E_WARNING, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_NOTICE", 40, '=');
    accessInvalidFile( E_NOTICE);

    Logging::LineSeparator("E_NOTICE w/ ErrorHandler");
    accessInvalidFile( E_NOTICE, 'errorHandler');

    Logging::LineSeparator("E_NOTICE w/ ReportingCheck");
    accessInvalidFile( E_NOTICE, 'errorHandlerWithReportingCheck');
}

/**
 * Get contents of file via file_get_contents().
 *
 * @param string $path File path.
 * @return bool|string Retrieved file contents.
 */
function getFileContents(string $path) {
    try {
        // Attempt to get file contents.
        return file_get_contents($path);
    } catch (ErrorException $exception) {
        // Catch expected ErrorExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Catch unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
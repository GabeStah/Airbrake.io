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

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

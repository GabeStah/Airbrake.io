<?php

include("D:\work\Airbrake.io\lib\php\Book.php");
include("D:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    try {
        Logging::LineSeparator('NORMAL Book INSTANCE');
        Logging::Log(new Book('The Stand', 'Stephen King', 1153, new DateTime('1990-5-1')));

        Logging::LineSeparator('REFLECTING Book CLASS');
        $reflection = new ReflectionClass('Book');
        Logging::Log($reflection);

        Logging::LineSeparator('CONSTANTS');
        Logging::Log($reflection->getConstants());

        Logging::LineSeparator('METHODS');
        Logging::Log($reflection->getMethods());

        Logging::LineSeparator('NEW INSTANCE');
        Logging::Log($reflection->newInstance('The Shining', 'Stephen King', 447, new DateTime('7-1-1980')));

        Logging::LineSeparator('REFLECTING Invalid CLASS');
        Logging::Log(new ReflectionClass('Invalid'));
    } catch (ReflectionException $exception) {
        // Output expected ReflectionException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
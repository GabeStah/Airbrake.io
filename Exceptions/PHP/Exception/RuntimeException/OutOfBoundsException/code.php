<?php

include("D:\work\Airbrake.io\lib\php\Book.php");
include("D:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples() {
    try {
        $book = new Book('A Game of Thrones', 'George R.R. Martin', 848, new DateTime('2005-08-06'));

        $tyrion = new Character('Tyrion Lannister', 'Badass little guy with a big heart.');
        $daenerys = new Character('Daenerys Targaryen', 'Mother of Dragons, Breaker of Chains, yadda yadda yadda');
        $jon = new Character('Jon Snow', 'Sorta emo, but people seem to like him.');

        $book->addCharacter($tyrion, 'tyrion');
        $book->addCharacter($daenerys);
        $book->addCharacter($jon);

        Logging::LineSeparator("A GAME OF THRONES");
        Logging::Log($book);

        Logging::LineSeparator("INVALID KEY");
        Logging::Log($book->getCharacter('tyrone'));
    } catch (OutOfBoundsException $exception) {
        // Output expected OutOfBoundsExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
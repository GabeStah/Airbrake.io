<?php

include("C:\Users\Rich\Gabe\work\Airbrake.io\lib\php\Book.php");
include("C:\Users\Rich\Gabe\work\Airbrake.io\lib\php\Logging.php");

function executeExamples() {
    try {
        $book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new DateTime('2007-03-27'));

        $kvothe = new Character('Kvothe');
        $bast = new Character('Bast');
        $chronicler = new Character('Chronicler');
        $denna = new Character('Denna');
        $auri = new Character('Auri');
        $wilem = new Character('Wilem');

        $book->addCharacter($kvothe);
        $book->addCharacter($bast);
        $book->addCharacter($chronicler);
        $book->addCharacter($denna);
        $book->addCharacter($auri);

        Logging::LineSeparator("THE NAME OF THE WIND");
        Logging::Log($book);

        Logging::LineSeparator("ADDITIONAL CHARACTER");
        Logging::Log($book->addCharacter($wilem));
    } catch (OverflowException $exception) {
        // Output expected OverflowExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
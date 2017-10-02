<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");
include("D:\work\Airbrake.io\lib\php\Book.php");
include('Publication.php');

function executeExamples()
{
    Logging::LineSeparator("VALID PUBLICATION TYPE");
    setValidPublicationType();

    Logging::LineSeparator("INVALID PUBLICATION TYPE");
    setInvalidPublicationType();

    Logging::LineSeparator("ASSIGN PUBLICATION TYPE");
    assignPublicationType();
}

function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital', 848, new DateTime('1996-08-06'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function setInvalidPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Clash of Kings", "George R. R. Martin", 'poem', 761, new DateTime('1998-11-16'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function assignPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'novel', 1177, new DateTime('2000-08-08'));
        // Output valid publication.
        Logging::Log($publication);
        // Directly modify publicationType to invalid type.
        $publication->publicationType = 'epic';
        // Output current publication type.
        Logging::Log($publication->getPublicationType());
    } catch (RangeException $exception) {
        // Output expected RangeException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
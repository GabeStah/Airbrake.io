<?php

include("/home/ubuntu/workspace/lib/php/Logging.php");

function executeExamples()
{
    Logging::LineSeparator("FIND MAXIMUM SCALE ACCURACY");
    Logging::Log(test());
}

function test() {
    Logging::Log("Hello there");
}

executeExamples();
<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    // Create document.
    $document = new DOMDocument('1.0');

    // Append 'books' element.
    appendElementToDocument(new DOMElement('books'), $document);

    Logging::LineSeparator();

    // Append '$' element.
    appendElementByNameToDocument('$', $document);

    Logging::LineSeparator();

    crossDocumentAppendTest();

    Logging::LineSeparator();

    crossDocumentAppendTestSuccess();
}

/**
 * Create and append DOMElement, by name, to passed DOMDocument.
 *
 * @param string $name
 * @param DOMDocument $document
 * @return DOMNode|null
 */
function appendElementByNameToDocument(string $name, DOMDocument $document) : ?DOMNode {
    try {
        $node = $document->appendChild(new DOMElement($name));
        Logging::Log("Successfully appended element (by name) [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}

/**
 * Append passed DOMElement to passed DOMDocument.
 *
 * @param DOMElement $element
 * @param DOMDocument $document
 * @return DOMNode|null
 */
function appendElementToDocument(DOMElement $element, DOMDocument $document) : ?DOMNode {
    try {
        $node = $document->appendChild($element);
        Logging::Log("Successfully appended element [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}

/**
 * Append element to unattached document.
 */
function crossDocumentAppendTest() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        appendElementToDocument($elementA, $documentB);

        Logging::Log("Successfully appended element [{$elementA->localName}] to [documentB].");
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Successfully appends element to unattached document.
 */
function crossDocumentAppendTestSuccess() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        $node = appendElementToDocument($elementA, $documentB);

        // Check if appendElementToDocument returned DOMNode or null.
        if (!$node) {
            // If $node not found, import elementA into documentB, then try appending again.
            $importedNode = $documentB->importNode($elementA, true);
            $node = appendElementToDocument($importedNode, $documentB);
            Logging::Log("Successfully appended element [{$node->localName}] to [documentB].");
        }
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
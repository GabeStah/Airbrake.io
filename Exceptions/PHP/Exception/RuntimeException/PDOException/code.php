<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

/**
 * Class Connector
 *
 * Helper class for establishing PDO connections and making queries.
 */
class Connector
{
    private $connection;
    private $statement;

    /**
     * Connector constructor.
     *
     * @param string $dbname
     * @param string $username
     * @param string $password
     * @param string $uri
     * @param string $host
     */
    public function __construct(string $dbname, string $username, string $password, string $uri = 'mysql', string $host = 'localhost')
    {
        $this->connection = new PDO("$uri:dbname=$dbname;host=$host", $username, $password);

    }

    /**
     * Execute the passed query as PDO statement.
     *
     * @param string $query
     */
    public function executeQuery(string $query)
    {
        $this->statement = $this->connection->prepare($query);
        $this->statement->execute();
    }

    /**
     * Fetch results of current statement using passed FETCH_ return type.
     *
     * @param int $returnType see: http://php.net/manual/en/pdo.constants.php
     * @return array
     */
    public function fetchResults(int $returnType = PDO::FETCH_OBJ)
    {
        $results = [];
        while ($row = $this->statement->fetch($returnType)) {
            $results[] = $row;
        }
        return $results;
    }
}

function executeExamples()
{
    Logging::LineSeparator("OUTPUT ACTORS");
    outputActors();

    Logging::LineSeparator("INVALID PASSWORD");
    invalidPassword();

    Logging::LineSeparator("INVALID DATABASE");
    invalidDatabase();
}

function outputActors()
{
    try {
        // Create connector helper.
        $connector = new Connector('sakila', 'dev', 'password');
        // Execute query.
        $connector->executeQuery('SELECT * FROM actor ORDER BY last_name LIMIT 5');
        // Fetch results of query.
        $results = $connector->fetchResults();
        // Output results.
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function invalidPassword()
{
    try {
        $connector = new Connector('sakila', 'dev', 'hunter2');
        $connector->executeQuery('SELECT * FROM film ORDER BY release_year LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function invalidDatabase()
{
    try {
        $connector = new Connector('askila', 'dev', 'password');
        $connector->executeQuery('SELECT * FROM city ORDER BY city LIMIT 5');
        $results = $connector->fetchResults();
        Logging::Log($results);
    } catch (PDOException $exception) {
        // Output expected PDOException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
<?php
include __DIR__ . '/vendor/autoload.php';

// Create new Notifier instance.
$notifier = new Airbrake\Notifier(array(
    'projectId' => 142985,
    'projectKey' => '59f97eff61254f63924b180e79802b41',
));

// Add environment data to all notices.
$notifier->addFilter(function($notice) {
    $notice['context']['environment'] = 'production';
    return $notice;
});

// Add filter to remove sensitive data before submission.
$notifier->addFilter(function($notice) {
    if (isset($notice['params']['password'])) {
        unset($notice['params']['password']);
    }
    return $notice;
});

// Add filter to ignore all Exception class types.
$notifier->addFilter(function($notice) {
    if ($notice['errors'][0]['type'] == 'Exception') {
        // Ignore this exception.
        return null;
    }
    return $notice;
});

// Set global notifier instance.
Airbrake\Instance::set($notifier);

// Register error and exception handlers.
$handler = new Airbrake\ErrorHandler($notifier);
$handler->register();

// Somewhere in the app...
try {
    throw new Exception('Uh oh, something broke!');
} catch (Exception $e) {
    // Create a new notice instance and attach the exception.
    $notice = Airbrake\Instance::buildNotice($e);
    // Add the 'android' field to the context array with additional values.
    $notice['context']['android'] = array(
        'version' => '7.1.2',
        'timestamp' => time(),
    );
    // Send the created noticed to Airbrake.io.
    Airbrake\Instance::sendNotice($notice);
}
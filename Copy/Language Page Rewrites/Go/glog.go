package main

import (
	"errors"

	"github.com/airbrake/glog"
	"github.com/airbrake/gobrake"
)

var projectID int64 = 144783
var projectAPIKey = "f1c9d04409afd9001a2f7c328120c864"

// Create notifier instance with Project ID and Project API Keys
var notifier = gobrake.NewNotifier(projectID, projectAPIKey)

func main() {
	// Always close notifier
	defer notifier.Close()
	// Always notify on panic
	defer notifier.NotifyOnPanic()

	// Set glog instance
	glog.Gobrake = notifier

	// Create a new error and log it with glog
	glog.Errorf("Error logged: %s", errors.New("uh oh, something broke"))
}

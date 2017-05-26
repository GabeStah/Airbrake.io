package main

import (
	"errors"

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

	// Create a new error and send via notifier
	notifier.Notify(errors.New("oh oh, something broke"), nil)
}

func init() {
	// Create a filter that executes over every error notice.
	notifier.AddFilter(func(notice *gobrake.Notice) *gobrake.Notice {
		// Always specify the contextual 'environment' field is set to 'development'
		notice.Context["environment"] = "development"
		return notice
	})
}

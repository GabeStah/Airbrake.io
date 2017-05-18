import airbrake.*;

public class Main {

    public static void main(String[] args) {
        try {
            throw new Exception("Uh oh, an error!");
        }
        catch (Exception exception) {
            // Create new AirbrakeNotifier.
            AirbrakeNotifier notifier = new AirbrakeNotifier();
            // Create new AirbrakeNotice via Builder, passing API_KEY, exception, and optional environment string.
            AirbrakeNotice notice = new AirbrakeNoticeBuilder("966795e9ddb0543867ccf847df898318", exception, "development") {
                {
                    // Configure custom parameters.
                    addSessionKey("userId", 12345);
                }
            }.newNotice();
            // Pass generated notice to notifier, pushing exception to Airbrake.io.
            notifier.notify(notice);
        }
    }
}

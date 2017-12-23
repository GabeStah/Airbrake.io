package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {
    public static void main(String[] args) {
        Logging.lineSeparator("https://airbrake.io");
        HttpResponse httpResponse = HttpResponse.create("https://airbrake.io");
        Logging.log(httpResponse.getResponse());

        Logging.lineSeparator("htps://airbrake.io");
        HttpResponse httpResponse2 = HttpResponse.create("htps://airbrake.io");
        Logging.log(httpResponse2.getResponse());

        Logging.lineSeparator("https://airbrakeio");
        HttpResponse httpResponse3 = HttpResponse.create("https://airbrakeio");
        Logging.log(httpResponse3.getResponse());

        Logging.lineSeparator("https://airbrake,io");
        HttpResponse httpResponse4 = HttpResponse.create("https://airbrake,io");
        Logging.log(httpResponse4.getResponse());
    }
}
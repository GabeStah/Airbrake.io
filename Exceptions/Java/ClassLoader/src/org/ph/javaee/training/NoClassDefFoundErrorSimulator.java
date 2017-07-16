// NoClassDefFoundErrorSimulator.java
package org.ph.javaee.training;

import io.airbrake.utility.Logging;
import org.ph.javaee.training.util.JavaEETrainingUtil;

/**
 * NoClassDefFoundErrorTraining1
 * @author Pierre-Hugues Charbonneau
 *
 */
public class NoClassDefFoundErrorSimulator {


    /**
     * @param args
     */
    public static void main(String[] args) {
        Logging.log(System.getProperty("java.class.path"));
        Logging.log("java.lang.NoClassDefFoundError Simulator - Training 1");
        System.out.println("Author: Pierre-Hugues Charbonneau");
        System.out.println("http://javaeesupportpatterns.blogspot.com");

        // Print current Classloader context
        System.out.println("\nCurrent ClassLoader chain: "+JavaEETrainingUtil.getCurrentClassloaderDetail());

        // 1. Create a new instance of CallerClassA
        CallerClassA caller = new CallerClassA();

        // 2. Execute method of the caller
        caller.doSomething();

        System.out.println("done!");
    }
}
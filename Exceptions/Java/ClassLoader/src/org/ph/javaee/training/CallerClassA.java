package org.ph.javaee.training;

import io.airbrake.test.ReferencingClassA;
import org.ph.javaee.training.util.JavaEETrainingUtil;

/**
 * CallerClassA
 * @author Pierre-Hugues Charbonneau
 *
 */
public class CallerClassA {

    private final static String CLAZZ = CallerClassA.class.getName();

    static {
        System.out.println("Classloading of "+CLAZZ+" in progress..."+JavaEETrainingUtil.getCurrentClassloaderDetail());
    }

    public CallerClassA() {
        System.out.println("Creating a new instance of "+CallerClassA.class.getName()+"...");
    }

    public void doSomething() {

        // Create a new instance of io.airbrake.test.ReferencingClassA
        ReferencingClassA referencingClass = new ReferencingClassA();
    }
}
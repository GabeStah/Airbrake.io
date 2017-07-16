//package org.ph.javaee.training;
//
//import org.ph.javaee.training.util.JavaEETrainingUtil;
//
///**
// * io.airbrake.test.ReferencingClassA
// * @author Pierre-Hugues Charbonneau
// *
// */
//public class io.airbrake.test.ReferencingClassA {
//
//    private final static String CLAZZ = io.airbrake.test.ReferencingClassA.class.getName();
//
//    static {
//        System.out.println("Classloading of "+CLAZZ+" in progress..."+JavaEETrainingUtil.getCurrentClassloaderDetail());
//    }
//
//    public io.airbrake.test.ReferencingClassA() {
//        System.out.println("Creating a new instance of "+io.airbrake.test.ReferencingClassA.class.getName()+"...");
//    }
//
//    public void doSomething() {
//        //nothing to do...
//    }
//}
package org.ph.javaee.training.util;

import java.util.Stack;
import java.lang.ClassLoader;

/**
 * JavaEETrainingUtil
 * @author Pierre-Hugues Charbonneau
 *
 */
public class JavaEETrainingUtil {

    /**
     * getCurrentClassloaderDetail
     * @return
     */
    public static String getCurrentClassloaderDetail() {

        StringBuffer classLoaderDetail = new StringBuffer();
        Stack<ClassLoader> classLoaderStack = new Stack<>();

        ClassLoader currentClassLoader = Thread.currentThread().getContextClassLoader();

        classLoaderDetail.append("\n-----------------------------------------------------------------\n");

        // Build a Stack of the current ClassLoader chain
        while (currentClassLoader != null) {

            classLoaderStack.push(currentClassLoader);

            currentClassLoader = currentClassLoader.getParent();
        }

        // Print ClassLoader parent chain
        while(classLoaderStack.size() > 0) {

            ClassLoader classLoader = classLoaderStack.pop();

            // Print current
            classLoaderDetail.append(classLoader);

            if (classLoaderStack.size() > 0) {
                classLoaderDetail.append("\n--- delegation ---\n");
            } else {
                classLoaderDetail.append(" **Current ClassLoader**");
            }
        }

        classLoaderDetail.append("\n-----------------------------------------------------------------\n");

        return classLoaderDetail.toString();
    }
}
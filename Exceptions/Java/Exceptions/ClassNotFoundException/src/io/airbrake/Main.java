// Main.java
package io.airbrake;

import io.airbrake.utility.*;

public class Main {
    private static String className;

    public static void main(String[] args) {
        // Set className to Integer class.
        className = "java.lang.Integer";
        Logging.lineSeparator(className);

        // Output result of each getClassX method.
        Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
        Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
        Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));

        // Set className to InvalidClassName (an unknown class).
        className = "io.airbrake.InvalidClassName";
        Logging.lineSeparator(className);
        // Output result of each getClassX method.
        Logging.log(String.format("getClassByName(String) result: %s", getClassByName(className)));
        Logging.log(String.format("getClassFromLoaderByName(String) result: %s", getClassFromLoaderByName(className)));
        Logging.log(String.format("getSystemClassFromLoaderByName(String) result: %s", getSystemClassFromLoaderByName(className)));
    }

    /**
     * Get Class by name via Class.forName(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getClassByName(String name) {
        try {
            // Retrieve class by name.
            return Class.forName(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Get Class by name via MyClassLoader.loadClass(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getClassFromLoaderByName(String name) {
        try {
            // Create new loader instance.
            MyClassLoader loader = new MyClassLoader();
            // Retrieve class by name.
            return loader.loadClass(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Get Class by name via MyClassLoader.findSystemClassByName(String).
     * @param name Class name.
     * @return Class|null
     */
    private static Class<?> getSystemClassFromLoaderByName(String name) {
        try {
            // Create new loader instance.
            MyClassLoader loader = new MyClassLoader();
            // Retrieve system class by name.
            return loader.findSystemClassByName(name);
        } catch (ClassNotFoundException exception) {
            // Output exception ClassNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}


// Main.java
package io.airbrake;

import io.airbrake.utility.*;

import java.lang.reflect.Method;

public class Main {
    public static void main(String[] args) {
        Logging.lineSeparator("LIST LOADED LIBRARIES");
        listLoadedLibraries();

        Logging.lineSeparator("UNLOAD ZIPFILE CLASS");
        unloadLibraryByName("java.util.zip.ZipFile", "size");

        Logging.lineSeparator("LOAD INVALID LIBRARY");
        loadLibraryByName("InvalidLibrary");
    }

    /**
     * Lists all currently loaded native libraries.
     *
     * See: https://stackoverflow.com/questions/1007861/how-do-i-get-a-list-of-jni-libraries-which-are-loaded#comment10270887_1008631
     * See: https://pastebin.com/aDgGqjEr
     * See: https://pastebin.com/eVXFdgr9
     */
    private static void listLoadedLibraries() {
        AllLoadedNativeLibrariesInJVM.listAllLoadedNativeLibrariesFromJVM();
    }

    /**
     * Loads a native library using the passed name.
     *
     * @param name Name of library to load.
     */
    private static void loadLibraryByName(String name) {
        try {
            // Attempt to load library.
            System.loadLibrary(name);
        } catch (UnsatisfiedLinkError error) {
            // Output expected UnsatisfiedLinkErrors.
            Logging.log(error);
        } catch (Error | Exception error) {
            // Output unexpected Errors and Exceptions.
            Logging.log(error, false);
        }
    }

    /**
     * Unloads native library using passed libraryName.
     *
     * @param libraryName Name of library to unload.
     */
    private static void unloadLibraryByName(String libraryName, String methodName) {
        try {
            CustomClassLoader cl = new CustomClassLoader();
            Class ca = null;
            ca = cl.findClass(libraryName);
            Object a = ca.newInstance();
            Method p = ca.getMethod(methodName);
            p.invoke(a);
            p = null;
            ca = null;
            a = null;
            cl = null;
            System.gc();
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}

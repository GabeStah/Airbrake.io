// MyClassLoader.java
package io.airbrake;

public class MyClassLoader extends ClassLoader {

    public MyClassLoader() {  }

    public Class<?> findSystemClassByName(String name)
        throws ClassNotFoundException
    {
        return findSystemClass(name);
    }
}

package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.*;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        // Create book and test serialization.
        Book nameOfTheWind = new Book("The Name of the Wind", "Patrick Rothfuss", 662, new GregorianCalendar(2007, 2, 27).getTime());
        TestBookSerialization(nameOfTheWind);

        // Create and test another book.
        Book wiseMansFear = new Book("The Wise Man's Fear", "Patrick Rothfuss", 994, new GregorianCalendar(2011, 2, 1).getTime());
        TestBookSerialization(wiseMansFear);
    }

    /**
     * Tests serialization of passed Book object.
     *
     * @param book Book to be tested.
     */
    private static void TestBookSerialization(Book book) {
        // Determine serialization file name.
        String fileName = String.format("%s.ser", book.toFilename());
        // Output file info.
        Logging.lineSeparator(String.format("SAVING TO FILE: %s", fileName), 75);
        // Serialize Book to file.
        Serialize(book, fileName);
        // Deserialize Book from file.
        Deserialize(fileName);
    }

    /**
     * Serializes the passed object to the passed filePath.
     *
     * @param object Object that is being serialized.
     * @param filePath File path where object should be stored.
     */
    private static void Serialize(Object object, String filePath) {
        try {
            FileOutputStream fileOutputStream = new FileOutputStream(filePath);
            ObjectOutputStream objectOutputStream = new ObjectOutputStream(fileOutputStream);
            objectOutputStream.writeObject(object);
            fileOutputStream.close();
            Logging.log(String.format("SERIALIZED [%s]: %s", object.getClass().getName(), object));
        } catch (NotSerializableException exception) {
            // Output expected NotSerializeableExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOException.
            Logging.log(exception, false);
        }
    }

    /**
     * Deserializes object found in passed filePath.
     *
     * @param filePath Path to file where serialized object is found.
     * @param <T> Type of object that is expected.
     */
    private static <T> void Deserialize(String filePath) {
        try {
            FileInputStream fileInputStream = new FileInputStream(filePath);
            ObjectInputStream objectInputStream = new ObjectInputStream(fileInputStream);
            T object = (T) objectInputStream.readObject();
            objectInputStream.close();
            fileInputStream.close();
            Logging.log(String.format("DESERIALIZED [%s]: %s", object.getClass().getName(), object));
        } catch (NotSerializableException exception) {
            // Output expected NotSerializeableExceptions.
            Logging.log(exception);
        } catch (IOException | ClassNotFoundException exception) {
            // Output unexpected IOExceptions and ClassNotFoundExceptions.
            Logging.log(exception, false);
        }
    }
}

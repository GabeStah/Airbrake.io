// DigitalBook.java
package io.airbrake;

import java.util.Date;

public class DigitalBook extends Book {

    private PublicationType publicationType;

    public DigitalBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.DIGITAL;
    }
}

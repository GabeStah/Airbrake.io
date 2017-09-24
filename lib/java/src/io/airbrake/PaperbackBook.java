// PaperbackBook.java
package io.airbrake;

import java.util.Date;

public class PaperbackBook extends Book {

    private PublicationType publicationType;

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.PAPERBACK;
    }
}

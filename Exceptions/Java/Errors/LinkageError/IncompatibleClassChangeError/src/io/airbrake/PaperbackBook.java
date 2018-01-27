package io.airbrake;

import java.text.DateFormat;
import java.util.Date;

public class PaperbackBook extends BaseBook {
    public PaperbackBook(String title, String author) {
        super(title, author);
    }

    public PaperbackBook(String title, String author, Integer pageCount) {
        super(title, author, pageCount);
    }

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
    }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, in PAPERBACK format, and published %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()));
    }
}

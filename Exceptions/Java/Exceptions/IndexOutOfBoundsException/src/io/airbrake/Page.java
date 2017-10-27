package io.airbrake;

public class Page
{
    private String content;

    public Page() { }

    public Page(String content) {
        setContent(content);
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    /**
     * Gets a string representation of Page.
     *
     * @return String Formatted string of Page.
     */
    public String toString() {
        return getContent();
    }
}

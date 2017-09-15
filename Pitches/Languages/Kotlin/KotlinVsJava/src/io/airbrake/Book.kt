// Book.kt
package io.airbrake

import com.fasterxml.jackson.databind.ObjectMapper
import com.fasterxml.jackson.annotation.*
import io.airbrake.utility.Logging

import java.util.Date

@JsonIgnoreProperties(ignoreUnknown = true)
class Book {

    private var author: String? = null

    private val maximumPageCount: Int = 4000

    private var pageCount: Int? = null
        @Throws(IllegalArgumentException::class)
        set(pageCount) {
            if (pageCount!! > maximumPageCount) {
                throw IllegalArgumentException(String.format("Page count value [%d] exceeds maximum limit [%d].", pageCount, maximumPageCount))
            }
            field = pageCount
        }

    private var publishedAt: Date? = null

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    val tagline: String
        get() = String.format("'%s' by %s is %d pages.", title, author, pageCount)

    private var title: String? = null

    /**
     * Constructs an empty book.
     */
    constructor()

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    constructor(title: String, author: String) {
        this.author = author
        this.title = title
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    constructor(title: String, author: String, pageCount: Int?) {
        this.author = author
        this.pageCount = pageCount
        this.title = title
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    constructor(title: String, author: String, pageCount: Int?, publishedAt: Date) {
        this.author = author
        this.pageCount = pageCount
        this.title = title
        this.publishedAt = publishedAt
    }

    /**
     * Publish current book.
     *
     * If book already published, throws IllegalStateException.
     */
    fun publish() {
        if (publishedAt == null) {
            publishedAt = Date()
            Logging.log(String.format("Published '%s' by %s.", title, author))
        } else {
            throw IllegalStateException(
                    String.format("Cannot publish '%s' by %s (already published on %s).",
                            title,
                            author,
                            publishedAt))
        }
    }

    /**
     * Output to JSON.
     *
     * @return Current Book formatted as String.
     */
    fun toJsonString(): String {
        return ObjectMapper().writeValueAsString(this)
    }

    /**
     * Throw an Exception.
     */
    fun throwException(message: String) {
        throw Exception(message)
    }
}
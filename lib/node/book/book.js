// Book module - Book.js
let Enum = require('enum');

/**
 * Publication types enumeration.
 *
 * @type {*|Enum} Publication types.
 */
let PublicationType = new Enum(['Digital', 'Paperback', 'Hardcover']);

/**
 * Constructs a basic book, with page count, publication date, and publication type.
 *
 * @param title Book title.
 * @param author Book author.
 * @param pageCount Book page count.
 * @param publishedAt Book publication date.
 * @param publicationType Book publication type.
 * @constructor
 */
function Book (title, author, pageCount, publishedAt = null, publicationType = PublicationType.Digital) {
  this.setAuthor(author);
  this.setPageCount(pageCount);
  this.setPublicationType(publicationType);
  this.setPublishedAt(publishedAt);
  this.setTitle(title);
}

/**
 * Get author of book.
 *
 * @returns {*} Author name.
 */
Book.prototype.getAuthor = function () {
  return this.author;
};

/**
 * Get page count of book.
 *
 * @returns {*} Page count.
 */
Book.prototype.getPageCount = function () {
  return this.pageCount;
};

/**
 * Get publication type of book.
 *
 * @returns {*} Publication type.
 */
Book.prototype.getPublicationType = function () {
  return this.publicationType;
};

/**
 * Get publication date of book.
 *
 * @returns {*} Publication date.
 */
Book.prototype.getPublishedAt = function () {
  return this.publishedAt;
};

/**
 * Get a formatted tagline with author, title, page count, publication date, and publication type.
 *
 * @returns {string} Formatted tagline.
 */
Book.prototype.getTagline = function() {
  return `'${this.getTitle()}' by ${this.getAuthor()} is ${this.getPageCount()} pages, published ${this.getPublishedAt()} as ${this.getPublicationType().key} type.`
};

/**
 * Get title of book.
 *
 * @returns {*} Book title.
 */
Book.prototype.getTitle = function () {
  return this.title;
};

/**
 * Set author of book.
 *
 * @param value Author.
 */
Book.prototype.setAuthor = function (value) {
  this.author = value;
};

/**
 * Set page count of book.
 *
 * @param value Page count.
 */
Book.prototype.setPageCount = function (value) {
  this.pageCount = value;
};

/**
 * Set publication type of book.
 *
 * @param value Publication type.
 */
Book.prototype.setPublicationType = function (value) {
  this.publicationType = value;
};

/**
 * Set publication date of book.
 *
 * @param value Publication date.
 */
Book.prototype.setPublishedAt = function (value) {
  this.publishedAt = value;
};

/**
 * Set title of book.
 *
 * @param value Title.
 */
Book.prototype.setTitle = function (value) {
  this.title = value;
};

/**
 * Get string representation of book.
 *
 * @returns {string} String representation.
 */
Book.prototype.toString = function () {
  return this.getTagline();
};

/**
 * Exports Book class.
 *
 * @type {Book} Book constructor.
 */
module.exports = Book;
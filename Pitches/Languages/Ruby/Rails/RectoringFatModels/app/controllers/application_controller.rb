# application_controller.rb
class ApplicationController < ActionController::Base
  protect_from_forgery with: :exception

  helper_method :favorite_book
  helper_method :favorite_book_in_library?

  def favorite_book
    @favorite_book ||= Book.find session[:favorite_book_id] if session[:favorite_book_id]
    if @favorite_book
      @favorite_book
    else
      Book.new(title: Book::DEFAULT_TITLE)
    end
  end

  def favorite_book_in_library?
    book = favorite_book
    # Confirm that book is in library.
    unless book.library
      # If not, add library.
      book.library = true
      # Return true.
      true
    end
  end
end

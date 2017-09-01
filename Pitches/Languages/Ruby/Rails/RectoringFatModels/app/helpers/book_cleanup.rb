class BookCleanup
  def initialize
  end

  # Remove featured flag from all books with rating below 3.
  def cleanup
    Book.where('rating < ?', 3).update_all(featured: false)
  end
end
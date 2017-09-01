class Book
  def initialize(title, author, ratings)
    @author = author
    @ratings = ratings
    @title = title
  end

  def add_rating(rating)
    @ratings.push(rating)
  end

  def rating
    # Average ratings.
    @ratings.inject{:+}.to_f / @ratings.size
  end
end
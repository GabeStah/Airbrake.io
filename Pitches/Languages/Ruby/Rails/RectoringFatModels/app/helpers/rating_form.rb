class RatingForm
  extend ActiveModel::Naming
  include ActiveModel::Conversion
  include ActiveModel::Validations

  attribute :user, Integer
  attribute :book, Integer
  attribute :rating, Integer

  validates :book,
            presence: true
  validates :rating,
            presence: true,
            numericality: {
                only_integer: true,
                greater_than_or_equal_to: 1,
                less_than_or_equal_to: 5
            }

  def persisted?
    false
  end

  def save
    if valid?
      persist!
      true
    else
      false
    end
  end

  private

    def persist!
      rating = Rating.new(:rating)
      user = User.find(:user)
      book = Book.find(:book)
      book.add_rating!(rating, user)
    end
end
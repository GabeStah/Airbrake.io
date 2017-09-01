class Rating
  def initialize(value)
    @value = value || 1
  end

  def <=>(other)
    other.to_s <=> to_s
  end

  def eql?(other)
    to_s == other.to_s
  end

  def hash
    @value.hash
  end

  def to_s
    @value.to_s
  end
end
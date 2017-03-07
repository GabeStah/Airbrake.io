# class-singleton-method.rb
class Author

end

# Define instance
author = Author.new

# Singleton method
def author.name
    puts "Self inside class singleton method is: #{self}"
    puts "Self.class inside class singleton method is: #{self.class}"
    return "John Doe"
end

puts "Author class singleton method 'name' is: #{author.name}"

# Define second instance without singleton method
new_author = Author.new
puts "New class method 'name' should be undefined: #{new_author.name}"
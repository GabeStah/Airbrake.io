# class-instance-method.rb
class Author

    # Instance method
    def name
        puts "Self inside class instance method is: #{self}"
        puts "Self.class inside class instance method is: #{self.class}"
        return "John Doe"
    end
end

# Define instance
author = Author.new
puts "Author class instance method 'name' is: #{author.name}"
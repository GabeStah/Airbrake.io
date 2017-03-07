# class-method.rb
class Author
    # Define class variable
    @@name = "John Doe"

    # Getter method
    def self.name
        puts "Self inside class method is: #{self}"
        return @@name
    end
end

puts "Author class method 'name' is: #{Author.name}"
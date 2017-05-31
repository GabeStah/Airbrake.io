def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    long_book_example
    short_book_example
    alternate_short_book_example
    freeze_example
end

class Book
    attr_accessor :author, :page_count, :title

    def initialize(args = {})
        @author = args[:author]
        # Invoke custom setter for passed page_count value, if applicable.
        self.page_count = args[:page_count]
        @title = args[:title]        
    end

    def page_count=(value)
        min, max = 1, 1000
        # Check if value outside allowed bounds.
        if value < min || value > max
            # Raise RangeError if outside bounds
            raise RangeError, "Value of [#{value}] outside bounds of [#{min}] to [#{max}]."
        else
            @page_count = value
        end
    end
end

def long_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Leo Tolstoy', page_count: 1225, title: 'War and Peace')
        # If no error, explicitly generate RuntimeError
        raise RuntimeError, "Oh no, our book is too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def short_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Dr. Seuss', page_count: 72, title: 'Green Eggs and Ham')
        # If no error, explicitly generate RuntimeError
        raise RuntimeError, "Oh no, our book is too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def alternate_short_book_example
    begin
        # Create a new book
        book = Book.new(author: 'Dr. Seuss', page_count: 72, title: 'Green Eggs and Ham')
        # If no previous error generate a default error
        raise "Oh no, our book is still too short!"
    rescue RangeError => e
        print_exception(e, true)
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

def freeze_example
    begin
        # Declare mutable string
        name = 'Jane Doe'
        name << ' is awesome!'
        puts name
        # Declare string and freeze (make it immutable)
        name = 'John Smith'.freeze
        name << ' is awesome!'
        puts name
    rescue RuntimeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

# Execute examples.
execute_examples
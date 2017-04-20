def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    create_book
    invalid_book_method
    book_with_author
end

class Book
    # Create getter/setter for author and title attribute.
    attr_accessor :author, :title

    def initialize(args = {})
        @author = args[:author]
        @title = args[:title]
    end
end

def create_book
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def invalid_book_method
    begin
        # Create a new book
        book = Book.new(title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
        # Output book author (invalid method).
        puts book.author
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def book_with_author
    begin
        # Create a new book
        book = Book.new(author: 'Stephen King', title: 'The Stand')
        # Output book class type.
        puts book
        # Output book title.
        puts book.title
        # Output book author.
        puts book.author
    rescue NoMethodError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

# Execute examples.
execute_examples
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    create_book
    create_abridged_book
    create_invalid_abridged_book
end

class Book
    attr_accessor :author, :page_count, :title

    def initialize(args = {})
        @author = args[:author]
        @page_count = args[:page_count]
        @title = args[:title]
    end
end

class AbridgedBook < Book
    # Add page_count getter.
    attr_reader :page_count

    def initialize(args = {})
        # Executes initialize for the parent superclass.
        super(args)
        # Invoke custom setter for passed page_count value, if applicable.
        self.page_count = args[:page_count]
    end

    def page_count=(value)
        min, max = 1, 1000
        # Check if value outside allowed bounds.
        if value < min || value > max
            raise RangeError, "Value of [#{value}] outside bounds of [#{min}] to [#{max}]."
        else
            @page_count = value
        end
    end
end

def create_book
    begin
        # Create a new book
        book = Book.new(author: 'Patrick Rothfuss', page_count: 662, title: 'The Name of the Wind')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def create_abridged_book
    begin
        # Create a new book
        book = AbridgedBook.new(author: 'Stephen King', page_count: 823, title: 'The Stand')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def create_invalid_abridged_book
    begin
        # Create a new book
        book = AbridgedBook.new(author: 'Leo Tolstoy', page_count: 1225, title: 'War and Peace')
        # Output class type.
        puts book
        # Output fields.
        puts book.author
        puts book.title
        puts book.page_count    
    rescue RangeError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

# Execute examples.
execute_examples
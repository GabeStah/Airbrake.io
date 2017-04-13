def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples()
    invalid_name_example
    valid_name_example
    invalid_constant_example
    valid_constant_example
end

def invalid_name_example
    begin
        # Output a title value which is undeclared.
        puts title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

def valid_name_example
    begin
        title = 'The Stand'
        # Output a title value, after declaration.
        puts title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

def invalid_constant_example
    begin
        # Assign an invalid (lowercase) constant value of :title.
        String.const_set :title, 'The Shining'
        puts String::title
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def valid_constant_example
    begin
        # Assign an valid constant value of :title.
        String.const_set :TITLE, 'The Shining'
        puts String::TITLE
    rescue NameError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

class Author
    def name
        puts 'Stephen King'
    end
end
puts Author.new.name

# Execute examples.
execute_examples
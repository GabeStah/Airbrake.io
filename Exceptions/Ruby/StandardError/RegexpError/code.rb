def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    regex_example
    word_character_example
    word_example
    capture_example
    invalid_pattern_example
end

def regex_example
    begin
        phrase = 'The quick brown fox jumps over the lazy dog.'
        # Define regex.
        regex = /fox/
        # Check for match result.
        result = regex.match(phrase)
        # Output result.
        puts result #=> fox
        # Show the class.
        puts result.class #=> MatchData
        # Get the regexp.
        puts result.regexp #=> (?-mix:fox)
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def word_character_example
    begin
        result = /\w/.match('The quick brown fox jumps over the lazy dog.')
        puts result #=> T
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def word_example
    begin
        result = /\w+/.match('The quick brown fox jumps over the lazy dog.')
        puts result #=> The
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def capture_example
    begin
        # Capture a full word.
        result = /(\w+)/.match('The quick brown fox jumps over the lazy dog.')
        # Output captures.
        puts result.captures
        # Show that captures is an array.
        puts result.captures.class
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def invalid_pattern_example
    begin
        regex = Regexp.new('+');
        result = regex.match('The quick brown fox jumps over the lazy dog.')
        puts result
    rescue RegexpError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

# Execute examples.
execute_examples
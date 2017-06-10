require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

def execute_examples
    array_example
    Logging.line_separator
    invalid_array_example
    Logging.line_separator
    string_example
    Logging.line_separator
    invalid_string_example
end

def array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then grab first (4) records and output them to log.
        Logging.log(titles.sort.first(4))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def invalid_array_example
    begin
        titles = [
            "Do Androids Dream of Electric Sheep?",
            "Something Wicked This Way Comes",
            "The Hitchhiker's Guide to the Galaxy",
            "Pride and Prejudice",
            "Eats, Shoots & Leaves: The Zero Tolerance Approach to Punctuation",
        ]
        # Sort array then try to get first '4' records as string and output.
        Logging.log(titles.sort.first('4'))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at zero index.
        Logging.log(title.insert(0, 'The '))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def invalid_string_example
    begin
        title = "Hitchhiker's Guide to the Galaxy"
        # Insert string at invalid '0' index.
        Logging.log(title.insert('0', 'The '))
    rescue TypeError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

# Execute examples.
execute_examples
# 1
FILE_NAME = "names.csv"
NAMES = [
    "Alice Zebra",
    "Bob Yelma",
    "Christine Xylophone",
    "Dan Williams"
]

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def append_data(name, data)
    # Create new file in read-write mode, truncating file
    File.open(name, "w+") { |file|
        # Loop through data elements
        data.each_with_index { |value, index|
            # Add each element as line
            file.puts("#{index},#{value}")
        }
    }
end

begin
    # Create file
    append_data(FILE_NAME, NAMES)
    # Open in read mode (default)
    file = File.open(FILE_NAME)
    # Read file and output data
    puts file.read
    # Read again
    puts file.readline
rescue EOFError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 2
begin
    # Create file
    append_data(FILE_NAME, NAMES)
    # Open in read mode (default)
    File.open(FILE_NAME) { |file|
        # Read file and output data
        puts file.read
    }
    # Open in read mode (default)
    File.open(FILE_NAME) { |file|
        # Read again, starting from top
        puts file.readline
    }
rescue EOFError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
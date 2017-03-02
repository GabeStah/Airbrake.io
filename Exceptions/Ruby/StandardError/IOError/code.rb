# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    line = "Jane Doe"
    # Open or create new file
    File.open("names.txt", "a") { |file|
        # Append line
        file << line
        puts "New line added: #{line}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    line = "Jane Doe"
    # Open or create new file
    File.open("names.txt") { |file|
        # Append line
        file << line
        puts "New line added: #{line}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Open or create new file
    File.open("names.txt", "w") { |file|
        # Read line
        puts "File line read: #{file.read}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 4
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Open or create new file
    File.open("names.txt", "a+") { |file|
        # Read line
        puts "File line read: #{file.read}"
    }
rescue IOError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
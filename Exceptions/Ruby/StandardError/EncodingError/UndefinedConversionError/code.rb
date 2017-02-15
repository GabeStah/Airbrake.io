# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Attempting to convert trademark symbol
    puts "\u2122".encode("IBM437")
rescue Encoding::UndefinedConversionError => e
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
    # Attempting to convert trademark symbol
    puts "\u2122".encode("UTF-16")
rescue Encoding::UndefinedConversionError => e
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
    # Attempting to convert trademark symbol
    puts "Trademark Symbol: \u2122".encode("IBM437", invalid: :replace, undef: :replace)
rescue Encoding::UndefinedConversionError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
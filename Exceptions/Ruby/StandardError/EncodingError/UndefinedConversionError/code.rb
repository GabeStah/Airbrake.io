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

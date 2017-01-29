# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # UTF-8 instance
    utf8 = "hello"
    # Convert to ASCII
    forced = "hello".encode('ASCII')
    # Compare the two
    puts utf8.include? forced
rescue Encoding::CompatibilityError => e
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
    # UTF-8 instance
    utf8 = "résumé"
    # Convert to ASCII
    forced = "résumé".force_encoding('ASCII')
    # Output the encoding of forced
    puts forced.encoding
    # Compare the two
    puts utf8.include? forced
rescue Encoding::CompatibilityError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

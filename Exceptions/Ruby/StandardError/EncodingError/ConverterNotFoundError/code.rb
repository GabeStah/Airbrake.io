# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates ConverterNotFoundError
    puts "hello".encode('UTF-89')
rescue Encoding::ConverterNotFoundError => e
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
    # Generates ConverterNotFoundError
    puts "hello".encode(Encoding::UTF_89)
rescue Encoding::ConverterNotFoundError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

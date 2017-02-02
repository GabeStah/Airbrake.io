# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates InvalidByteSequenceError
    puts "\xE3".encode("ASCII-8BIT").encode("UTF-8")
rescue Encoding::InvalidByteSequenceError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

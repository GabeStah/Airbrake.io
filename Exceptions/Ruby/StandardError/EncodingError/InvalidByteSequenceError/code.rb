# 1
puts ["\x68", "\x65", "\x6c", "\x6c", "\x6f"].join

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Generates InvalidByteSequenceError
    puts "\xC2".encode('ASCII')
rescue Encoding::InvalidByteSequenceError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 1
for index in 0..9 do
    puts index
end

# 2
for index in 0..9 do
    puts index
    # Break from enumeration if index is 5 or greater.
    break if index >= 5
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    for index in 0..9 do
        puts index
        # Raise StopIteration if index is 5 or greater.
        raise StopIteration if index >= 5
    end
rescue StopIteration => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 4
for index in 0..9 do
    puts index
    # Break from enumeration if index is 5 or greater.
    break if index >= 5
end
puts "Loop complete."

# 5
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    for index in 0..9 do
        puts index
        # Raise StopIteration if index is 5 or greater.
        raise StopIteration if index >= 5
    end
    puts "Loop complete."
rescue StopIteration => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
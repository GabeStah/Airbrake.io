# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a random sample from array, with too many arguments
    puts data.sample(3, 5)
rescue ArgumentError => e
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
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a negative number of random elements
    puts data.sample(-5)
rescue ArgumentError => e
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
    # Create array of numbers 1 through 10
    data = *(1...10)
    # Try to grab a negative number of random elements
    puts data.sample(-5)
rescue => e
    print_exception(e, false)
end

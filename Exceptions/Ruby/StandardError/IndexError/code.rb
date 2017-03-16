# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    index = 5
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index)
rescue IndexError => e
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
    index = 5
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index, "No name found.")
rescue IndexError => e
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
    index = 1
    names = ["Alice", "Bob", "Christine", "Dan"]
    puts names.fetch(index) { |i|
        "No name found at index: #{i}."
    }
rescue IndexError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
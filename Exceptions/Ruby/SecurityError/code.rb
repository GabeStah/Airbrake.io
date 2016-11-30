# 1
name = "Bob"
puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
puts "Tainting poor, old #{name}."
name.taint
puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    $SAFE = 1
    name = "Bob"
    puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    puts "Tainting poor, old #{name}."
    name.taint
    eval %{
        puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    }
rescue SecurityError => e
    print_exception(e, true)
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    $SAFE = 3
    name = "Bob"
    eval %{
        puts "Is #{name} tainted? #{name.tainted? ? 'Yes' : 'No'}"
    }
rescue SecurityError => e
    print_exception(e, true)
end

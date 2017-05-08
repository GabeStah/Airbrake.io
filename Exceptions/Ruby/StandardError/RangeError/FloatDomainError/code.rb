def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    # Convert 3.75 to integer.
    to_integer(3.75)
    # Convert 12345.67890 to integer.
    to_integer(12345.67890)
    # Convert Infinity to integer.
    to_integer(Float::INFINITY)

    # Convert 3.75 to rational.
    to_rational(3.75)
    # Convert 12345.67890 to rational.
    to_rational(12345.67890)
    # Convert Infinity to rational.
    to_rational(Float::NAN)
end

def to_integer(value)
    begin
        # Convert value to integer.
        i = value.to_i
        puts "#{value} converted to #{i}"
    rescue FloatDomainError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end 
end

def to_rational(value)
    begin
        # Convert value to rational number.
        r = value.to_r
        puts "#{value} converted to #{r}"
    rescue FloatDomainError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

# Execute examples.
execute_examples
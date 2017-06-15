require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

def execute_examples
    division_example
    Logging.line_separator
    zero_division_example
    Logging.line_separator
    floating_zero_division_example
    Logging.line_separator
    negative_floating_zero_division_example
end

def division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator.
        denominator = 5
        # Divide and output the result.
        Logging.log(numerator / denominator)
        #=> 3
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero.
        denominator = 0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> (EXPLICIT) ZeroDivisionError: divided by 0
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to zero as float.
        denominator = 0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> Infinity
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def negative_floating_zero_division_example
    begin
        # Set the numerator.
        numerator = 15
        # Set the denominator to negative zero as float.
        denominator = -0.0
        # Try to divide and output the result.
        Logging.log(numerator / denominator)
        #=> -Infinity
    rescue ZeroDivisionError => e
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

# Execute examples.
execute_examples
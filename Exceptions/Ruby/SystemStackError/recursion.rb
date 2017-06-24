require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

class MathClass
    class << self
        attr_accessor :count, :data, :value
    end

    def initialize(args = {})
        reset(args)
    end

    # Reset all class attributes, if necessary.
    def reset(args = {})
        self.class.count = args[:count] || 0
        self.class.data = args[:data] || []
        self.class.value = args[:data] || 1
    end

    # Increment the iteration counter.
    def increment_count(args = {})
        output = args[:output].nil? ? false : args[:output]
        Logging.line_separator if output
        self.class.count += 1
        Logging.log(self.class.count) if output
    end

    # Simple recursive method, incrementing the counter and executing itself.
    def recursion
        begin
            # Increment counter.
            increment_count
            # Recursively call.
            recursion
        rescue SystemStackError => e
            # Log stack overflow exception.
            # Exclude backtrace since it may contains tens of thousands of identical lines.
            Logging.log(e, { backtrace: false })
            # Output number of iterations required to hit overflow.
            Logging.log("Recursion iteration count: #{self.class.count}")
        rescue => e
            Logging.log(e, { explicit: false, backtrace: false })
        end    
    end

    # Double the +value+ attribute ad naseum, recursively calling self.
    def double
        begin
            # Double value.
            self.class.value *= 2
            # Add to array to test if memory runs out before overflow.
            self.class.data.push(self.class.value)
            # Increment counter.
            increment_count
            # Recursively double.
            double
        rescue SystemStackError => e
            # Log stack overflow exception.
            # Exclude backtrace since it may contains tens of thousands of identical lines.
            Logging.log(e, { backtrace: false })
            # Output number of iterations required to hit overflow.
            Logging.log("Doubling iteration count: #{self.class.count}")
        rescue => e
            Logging.log(e, { explicit: false })
        end          
    end
end

def execute_examples
    math_class = MathClass.new()
    math_class.double
    math_class.reset
    math_class.recursion
end

# Execute examples.
execute_examples
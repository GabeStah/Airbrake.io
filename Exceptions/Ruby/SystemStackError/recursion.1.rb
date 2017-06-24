require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

class MathClass
    class << self
        attr_accessor :count, :data, :value
    end

    def initialize(args = {})
        reset(args)
    end

    def reset(args = {})
        self.class.count = args[:count] || 0
        self.class.data = args[:data] || []
        self.class.value = args[:data] || 1
    end

    def iterate_counter
        Logging.line_separator
        self.class.count += 1
        Logging.log(self.class.count)
    end

    def recursion
        begin
            iterate_counter
            recursion
        rescue SystemStackError => e
            # Log exception type.
            Logging.log(e)
        rescue => e
            Logging.log(e, { explicit: false })
        end    
    end

    def double_value
        begin
            self.class.value *= 2
            self.class.data.push(self.class.value)
            iterate_counter
            Logging.log(self.class.value)
            double_value
        rescue SystemStackError => e
            # Log exception type.
            Logging.log(e)
        rescue => e
            Logging.log(e, { explicit: false })
        end          
    end
end

def execute_examples
    math_class = MathClass.new()
    #math_class.doubler
    math_class.recursion
    initial = 2
    maximum = 1
    #maximum = 18446744073709551616
    #squarer(initial, maximum, 0)
    # doubler(1)
    # COUNT = 0
    # infinite_recurser
end

def squarer(value, maximum, count)
    begin
        if value != maximum
            value = value**2
            Logging.log(value)
            Logging.line_separator
            count += 1
            Logging.log(count)
            Logging.line_separator
            squarer(value, maximum, count)
        end
    rescue SystemStackError => e
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def doubler(value)
    begin
        value *= 2
        DATA.push(value)
        Logging.log(value)
        Logging.line_separator
        #COUNT += 1
        Logging.log(COUNT)
        Logging.line_separator
        doubler(value)
    rescue SystemStackError => e
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def infinite_recurser
    Logging.line_separator
    #COUNT += 1
    Logging.log(COUNT)
    Logging.line_separator
    infinite_recurser
end

# Execute examples.
execute_examples
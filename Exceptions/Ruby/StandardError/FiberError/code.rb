# 1
class Counter
    def iterator
        return enum_for(:iterator) if not block_given?
        yield 0
        yield 1
        yield 2
        yield 3
        yield 4
    end
end

counter = Counter.new.iterator
# Output object
puts counter
# Iterate a few times
puts counter.next
puts counter.next
puts counter.next

# 2
class Counter
    def initialize
        @fiber = Fiber.new do
            count = 0
            # Infinite loop
            loop do
                # Yield the current count
                Fiber.yield count
                # Iterate
                count += 1
            end
        end
    end

    # Calls the next Fiber.yield enumeration
    def next
        @fiber.resume
    end
end

counter = Counter.new
# Output object
puts counter
# Iterate a few times
puts counter.next
puts counter.next
puts counter.next

# 3
require 'fiber'

class Counter
    def initialize
        @fiber = Fiber.new do
            count = 0
            # Yield the current count
            Fiber.yield count
            # Indicated termination
            "Fiber Terminated"
        end
    end

    # Calls the next Fiber.yield enumeration
    def next
        # Check if fiber is alive
        puts "Is fiber alive?: #{@fiber.alive? ? 'Yes' : 'No'}"
        @fiber.resume
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    counter = Counter.new
    # Output object
    puts counter
    # Iterate a few times
    puts counter.next
    puts counter.next
    puts counter.next
rescue FiberError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
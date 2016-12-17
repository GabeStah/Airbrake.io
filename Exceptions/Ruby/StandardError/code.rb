# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    raise "Uh oh!"
rescue IndexError => e
    print_exception(e, true)
rescue NameError => e
    print_exception(e, true)
rescue RegexpError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end


# 2
class MissingName < StandardError
    def initialize(msg="Name is missing!")
        super
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    raise MissingName
rescue => e
    print_exception(e, true)
end

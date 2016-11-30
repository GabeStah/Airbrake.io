# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    pid = Process.fork { sleep 0.5 }
rescue NotImplementedError => e
    print_exception(e, true)
end

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    groups = Process.groups
rescue NotImplementedError => e
    print_exception(e, true)
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    pid = Process.fork { sleep 0.5 }
rescue ScriptError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 4
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    pid = Process.fork { sleep 0.5 }
rescue => e
    print_exception(e, false)
end

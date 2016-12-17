# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
    end
rescue Interrupt => e
    print_exception(e, true)
rescue SignalException => e
    print_exception(e, false)
rescue Exception => e
    print_exception(e, false)
end

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
    end
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Loop indefinitely
    count = 0
    while true
        count = count + 1
        puts count
        sleep 1
        if count >= 5 then
            Process.kill('INT', Process.pid)
        end
    end
rescue Interrupt => e
    print_exception(e, true)
rescue SignalException => e
    print_exception(e, false)
rescue Exception => e
    print_exception(e, false)
end

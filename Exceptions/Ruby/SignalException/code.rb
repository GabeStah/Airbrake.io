# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Sending `TERM` signal to active process
    Process.kill('TERM', Process.pid)
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Sending `KILL` signal to active process
    Process.kill('KILL', Process.pid)
rescue SignalException => e
    print_exception(e, true)
rescue Exception => e
    print_exception(e, false)
end

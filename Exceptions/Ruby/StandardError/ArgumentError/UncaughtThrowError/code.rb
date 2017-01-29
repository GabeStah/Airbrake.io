# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    count = catch (:alert) do
        count = 0
        while true
            count = count + 1
            puts "Count #{count} at #{Time.now.getutc}"
            sleep 0.5
            throw :alert, count if count >= 9
        end
    end
    puts "Throw has been caught at count #{count}!"
rescue UncaughtThrowError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 2
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    count = catch (:error) do
        count = 0
        while true
            count = count + 1
            puts "Count #{count} at #{Time.now.getutc}"
            sleep 0.5
            throw :alert, count if count >= 9
        end
    end
    puts "Throw has been caught at count #{count}!"
rescue UncaughtThrowError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

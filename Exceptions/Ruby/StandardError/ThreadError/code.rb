module Logging
    class << self
        # Outputs the +message+ to console with timestamp.
        # If +timestamp+ is +false+, only +message+ is output.
        def log(message, timestamp=true)
            puts "#{timestamp ? "[#{Time.now.strftime("%T")}] " : nil}#{message}"
        end
    end
end

def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    #thread_example
    stop_thread_example
end

def get_threads
    threads = []
    # Create first thread and add to array.
    threads << Thread.new do
        Logging.log 'Sub-thread 1 is sleeping.'
        # Sleep for 2 seconds.
        sleep(2)
        Logging.log 'Sub-thread 1 is terminating.'
    end
    # Create second thread and add to array.
    threads << Thread.new do
        Logging.log 'Sub-thread 2 is terminating.'
    end
    # Return threads array.
    threads
end

def thread_example
    begin
        Logging.log 'Main thread has started.'
        # Loop through all threads.
        get_threads.each do |thread|
            # Join sub-threads to executing (main) thread.
            thread.join
        end
        Logging.log 'Main thread is terminating.'
    rescue ThreadError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end    
end

def stop_thread_example
    begin
        Logging.log 'Main thread has started.'
        # Loop through all threads.
        get_threads.each do |thread|
            # Join sub-threads to executing (main) thread.
            thread.join
            # Stop execution of the current (main) thread.
            Thread.stop
        end
        Logging.log 'Main thread is terminating.'
    rescue ThreadError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

# Execute examples.
execute_examples
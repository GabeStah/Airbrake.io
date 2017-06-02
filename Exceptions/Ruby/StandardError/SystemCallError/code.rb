def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

def execute_examples
    invalid_path_example
    invalid_path_example_2
    invalid_path_example_3
    invalid_path_example_4
end

def invalid_path_example
    begin
        File.open('missing/file/path')        
    rescue SystemCallError => e
        print_exception(e, true)
    rescue => e
        print_exception(e, false)
    end
end

def invalid_path_example_2
    begin
        File.open('missing/file/path')        
    rescue SystemCallError => e
        puts 'Rescued by SystemCallError statement.'
        print_exception(e, true)        
    rescue Errno::ENOENT => e
        puts 'Rescued by Errno::ENOENT statement.'
        print_exception(e, true)        
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end

def invalid_path_example_3
    begin
        File.open('missing/file/path')        
    rescue Errno::ENOENT => e
        puts 'Rescued by Errno::ENOENT statement.'
        print_exception(e, true)        
    rescue SystemCallError => e
        puts 'Rescued by SystemCallError statement.'
        print_exception(e, true)
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end

def invalid_path_example_4
    begin
        File.open('missing/file/path')        
    rescue Errno::ENOENT, SystemCallError => e
        puts 'Rescued by combined statement.'
        print_exception(e, true)        
    rescue => e
        puts 'Rescued by StandardError (default) statement.'
        print_exception(e, false)
    end
end

# Execute examples.
execute_examples
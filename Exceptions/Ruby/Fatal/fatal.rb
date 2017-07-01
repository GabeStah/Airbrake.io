require 'require_all'
require 'FileUtils'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

def valid_example
    # Set valid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\accessible\data.csv'    
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end   
end

def invalid_example
    # Set invalid path.
    path = 'D:\work\Airbrake.io\Exceptions\Ruby\Fatal\inaccessible\data.csv'
    begin
        # Open data.csv file as read-write, truncating existing.
        file = File.open(path, 'w+')
        # Add data lines.
        file.puts('id, first, last')
        file.puts('1, Alice, Smith')
        file.puts('2, Bob, Turner')                 
    rescue Exception => e
        # Rescue inexplicit exceptions.
        Logging.log(e, { explicit: false })
    end     
end

def raise_fatal_error
    begin
        # Get the fatal exception object.
        # Loop through all objects in ObjectSpace.
        fatal = ObjectSpace.each_object(Class).find do |klass|
            # Return match of 'fatal' object.
            klass < Exception && klass.inspect == 'fatal'
        end
        # Raise new fatal exception object.
        raise fatal.new('Uh oh, something is seriously broken!')
    rescue fatal => e
        # Try to rescue our fatal exception object.
        Logging.log(e)
    rescue Exception => e
        # Try to rescue all other exceptions.
        Logging.log(e, { explicit: false })        
    end   
end

def execute_examples
    valid_example
    invalid_example
    raise_fatal_error
end

# Execute examples.
execute_examples
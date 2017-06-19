require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

def execute_examples
    exit_process
    Logging.line_separator
    at_exit_example
end

def exit_process
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was 
        # skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

def at_exit_example
    # Specify +at_exit+ block.  Fires after 
    # the exit calls are completed.
    at_exit { Logging.log("This is part of the at_exit block.") }    
    begin
        # Exit the current process.
        exit
        # Log a message to indicate exit call was 
        # skipped for some reason (should never fire).
        Logging.log("Exit skipped.")
    rescue SystemExit => e
        # Log exit message.
        Logging.log("Exiting process.")
        # Log exception type.
        Logging.log(e)
    rescue => e
        Logging.log(e, { explicit: false })
    end    
end

# Execute examples.
execute_examples
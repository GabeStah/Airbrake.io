require 'require_all'
require_all 'D:/work/Airbrake.io/lib/**/*.rb'

class Book
    # Create getter/setter for author and title attribute.
    attr_accessor :author, :title

    def initialize(args = {})
        @author = args[:author]
        @title = args[:title]

        # Define finalizer method for garbage collection cleanup.
        ObjectSpace.define_finalizer(self, finalize)
    end

    def finalize
        # Output message indicating destruction of this instance.
        proc { Logging.log("Destroying '#{@title}' by #{@author}.") }
    end
end

def execute_examples
    finalizer_example
end

def finalizer_example
    begin
        # Create Book instance.
        book = Book.new( { title: "The Stand", author: "Stephen King"} )
        # Exit the current process.
        exit
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
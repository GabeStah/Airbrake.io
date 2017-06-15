module Logging
    extend Utility

    class << self
        # Outputs +value+ to console.
        # +args+ may include:
        #    +:explicit+ (Boolean) - Is +Exception+ class +value+ expected? [default: true]
        #    +:timestamp+ (Boolean) - Should timestamp be included? [default: false]
        #
        # Examples:
        #    
        #    Logging.log('My message') #=> "My message"
        #    Logging.log('My message', { timestamp: true} ) #=> "[12:00:05] My message"
        #
        #    begin
        #       raise Exception.new('An exception!')
        #    rescue Exception => e
        #       Logging.log(e) 
        #    end
        #    #=> (EXPLICIT) Exception: An exception!
        #    #=>    (...backtrace...)
        def log(value, args = {})
            # Check if exception was explicit.
            explicit = args[:explicit].nil? ? true : args[:explicit]
            # Get timestamp if necessary.
            timestamp = args[:timestamp] ? formatted_timestamp : ""

            if value.is_a?(Exception)
                # If +value+ is an +Exception+ type output formatted exception.
                puts timestamp << formatted_exception(value, explicit)
            elsif value.is_a?(String)
                # If +value+ is a +String+ directly output
                puts timestamp << value                
            else
                # If +value+ is anything else output.
                puts timestamp if !timestamp.empty?
                puts value
            end 
        end

        # Output the specified +separator+ +count+ times to log.
        # +args may include:
        #    +:count+ (Integer) - Number of characters to output. [default: 20]
        #    +:separator+ (String) - Character or string to duplicate and output. [default: '-']
        def line_separator(args = {})
            count = args[:count].nil? ? 20 : args[:count]
            separator = args[:separator].nil? ? '-' : args[:separator]

            # Concatenate and output.
            puts separator * count
        end

        private

            def formatted_exception(exception, explicit)
                # Set explicit or inexplicit tag.
                output = "(#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}) "
                # Add class and message.
                output << "#{exception.class}: #{exception.message}\n"
                # Append backtrace with leading tabs.
                output << "\t" << exception.backtrace.join("\n\t")
                # Return output string.
                output
            end

            def formatted_timestamp
                "[#{Time.now.strftime("%T")}] "
            end
    end
end
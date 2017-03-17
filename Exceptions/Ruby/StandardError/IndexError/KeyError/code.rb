# 1
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published)
rescue KeyError => e
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
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published, Time.now.to_s)
rescue KeyError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    book = {
        title: "The Stand",
        author: "Stephen King",
        page_count: 823,
    }

    puts book.fetch(:published) { |key|
        "Key '#{key}' not found."
    }
rescue KeyError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end
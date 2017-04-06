# 1
# Create a book hash.
book = {
    title: 'The Stand',
    author: 'Stephen King',
    published_at: 1978
}

# Returns the title of the book argument.
def get_title(book)
    puts 'Retrieving title...'
    puts book[:title]
    return book[:title]
end

# Call the get_title method, with an associated code block.
get_title(book) { puts 'Block has been executed!' }

# 2
# Create a book hash.
book = {
    title: 'The Stand',
    author: 'Stephen King',
    published_at: 1978
}

# Returns the title of the book argument.
def get_title(book)
    puts 'Yielding...'
    # Yields to the code block associated with method call.
    yield book[:author]
    puts 'Retrieving title...'
    puts book[:title]
    return book[:title]
end

# Call the get_title method, with an associated code block.
get_title(book) { puts 'Block has been executed!' }


# 3
# Create a book hash.
book = {
    title: 'The Stand',
    author: 'Stephen King',
    published_at: 1978
}

# Returns the title of the book argument.
def get_title(book)
    puts 'Yielding...'
    # Yields to the code block associated with method call.
    yield book[:author]
    puts 'Retrieving title...'
    puts book[:title]
    return book[:title]
end

# Call the get_title method, with an associated code block.
get_title(book) do |author|
    puts "Author is: #{author}"
    puts 'Block has been executed!'
end


# 3
def print_exception(exception, explicit)
    puts "[#{explicit ? 'EXPLICIT' : 'INEXPLICIT'}] #{exception.class}: #{exception.message}"
    puts exception.backtrace.join("\n")
end

begin
    # Create a book hash.
    book = {
        title: 'The Stand',
        author: 'Stephen King',
        published_at: 1978
    }

    # Returns the title of the book argument.
    def get_title(book)
        puts 'Yielding...'
        # Yields to the code block associated with method call.
        yield book[:author]
        puts 'Retrieving title...'
        puts book[:title]
        return book[:title]
    end

    # Call the get_title method, WITHOUT an associated code block.
    get_title(book)
rescue LocalJumpError => e
    print_exception(e, true)
rescue => e
    print_exception(e, false)
end

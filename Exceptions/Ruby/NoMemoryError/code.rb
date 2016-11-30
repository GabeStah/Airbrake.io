# 1
begin
    limit = 2**31 - 1
    puts "Limit: #{limit}"
    puts "a" * limit
rescue NoMemoryError => e
    puts "#{e.class}: #{e.message}"
    puts e.backtrace.join("\n")
end

# 2
begin
    limit = 2**31 - 2
    puts "Limit: #{limit}"
    puts "a" * limit
rescue NoMemoryError => e
    puts "#{e.class}: #{e.message}"
    puts e.backtrace.join("\n")
end

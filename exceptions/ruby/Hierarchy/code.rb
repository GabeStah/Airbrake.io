# SecurityError
name = "John Doe"
proc = Proc.new do
  $SAFE = 4
  name.untaint
end
proc.call

# SignalException
pid = fork do
   Signal.trap('HUP') { puts "trapped"; exit }
end

begin
  Process.kill('HUP', pid)
  sleep
rescue SignalException => e
  puts "Received Exception #{e}"
end

# StandardError
def test
  raise "Error"
end
test rescue "Hello"   #=> "Hello"

# SystemExit
begin
  exit
  puts "Unreachable"
rescue SystemExit => e
  puts "Received Exception #{e}"
end
puts "ADDITIONAL"

# SystemStackError
def increment(v)
    increment(v+1)
end
increment(1)

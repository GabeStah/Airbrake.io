require 'sinatra/base'
require 'airbrake'

Airbrake.configure do |c|
  c.project_id = 142535
  c.project_key = 'c25391b87ebc657115438d5f134c59f1'

  # Display debug output.
  c.logger.level = Logger::DEBUG
end

class MyApp < Sinatra::Base
  use Airbrake::Rack::Middleware

  get('/') {
    # Indirectly raise a ZeroDivisionError for testing.
    1/0
  }
end

run MyApp.run!
require 'airbrake-ruby'

# Create a notifier with :default name.
Airbrake.configure do |c|
  c.project_id = 142185
  c.project_key = '6dce873e4ee97eb3edc2f039f2f8e49a'
end

# Create a notifier named :staging.
Airbrake.configure(:staging) do |c|
  c.project_id = 142185
  c.project_key = '6dce873e4ee97eb3edc2f039f2f8e49a'
end

# Create a notifier named :production.
Airbrake.configure(:production) do |c|
  c.project_id = 142187
  c.project_key = '64c5008265330374f45514701f96963c'
end

Airbrake.add_filter do |notice|
  if notice[:errors].any? { |error| error[:type] == 'RuntimeError' }
    notice.ignore!
  end
end

Airbrake.notify('Staging error!', { time: Time.now })
Airbrake[:production].notify('Production error!', { time: Time.now })


begin
  puts [1, 2, 3, 4, 5].sample(-3)
rescue ArgumentError => ex
  # Notify by passing direct Exception.
  Airbrake.notify(ex)
end

# Notify by passing a String.
Airbrake.notify('There was an error!')

# Notify by passing an object which can be converted to a String.
Airbrake.notify(3.1415926535)

# Create an instance of Airbrake::Notice.
notice = Airbrake.build_notice('Uh oh, something broke!')
# Add custom parameters to this particular notice.
notice[:params][:username] = 'admin'
# Notify by passing the generates instance of Airbrake::Notice.
Airbrake.notify(notice)


begin
  puts [1, 2, 3, 4, 5].sample(-3)
rescue ArgumentError => ex
  # Synchronous notification by passing a direct Exception.
  Airbrake.notify_sync(ex, {
    user: 'foo@bar.com',
    context: 'lib',
    environment: 'production'
  })
end

# Synchronous notification by passing a String, using the :staging notifier.
Airbrake[:staging].notify_sync('There was an error!')
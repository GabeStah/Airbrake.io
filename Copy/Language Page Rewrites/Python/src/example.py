from airbrake.notifier import Airbrake

manager = Airbrake(project_id=144031, api_key="5a2fb879e83b40479b90284263193376")

try:
    # Raising an exception.
    1 / 0
except ZeroDivisionError as e:
    # Sends a 'division by zero' exception to Airbrake.
    manager.notify(e)
except:
    # Sends all other exceptions to Airbrake.
    manager.capture()

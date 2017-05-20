import logging


class Logger(logging.Logger):
    def __init__(self, name, level=0):
        super(Logger, self).__init__(name, level)
        handler = logging.StreamHandler()
        # Default to WARNING level.
        handler.setLevel(level or logging.WARNING)
        self.addHandler(handler)
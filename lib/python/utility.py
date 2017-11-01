class Logging:

    @classmethod
    def log(cls, value):
        cls.output(value)

    @classmethod
    def output(cls, value):
        print(value)
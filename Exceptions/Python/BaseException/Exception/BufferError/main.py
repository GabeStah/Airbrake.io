import io

from gw_utility.logging import Logging


def main():
    buffer_test()


def output_buffer(view: memoryview):
    Logging.line_separator("BUFFER OUTPUT")
    Logging.log(f'tobytes(): {view.tobytes()}')
    Logging.log(f'tolist(): {view.tolist()}')
    Logging.log(f'hex(): {view.hex()}')


def buffer_test():
    try:
        # Create byte array with string 'Hello'.
        array = io.BytesIO(b'Hello')
        # Create a read-write copy of the bytearray.
        view = array.getbuffer()
        # Output copied memory view.
        output_buffer(view)
        # Add string ' world!' to existing bytearray.
        array.write(b' world!')
    except BufferError as error:
        # Output expected BufferErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

import socket
import ssl

from gw_utility.logging import Logging


def main():
    try:
        host = 'airbrake.io'

        Logging.line_separator('AIRBRAKE.IO HTTP REQUEST', 60)
        s = get_socket(host, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST', 60)
        s = get_socket(host, True)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTP REQUEST w/o BLOCKING', 60)
        s = get_socket(host, False, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())

        Logging.line_separator('AIRBRAKE.IO HTTPS REQUEST w/o BLOCKING', 60)
        s = get_socket(host, True, False)
        get_socket_response(s, f'GET / HTTP/1.1\r\nHost: {host}\r\n\r\n'.encode())
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def get_socket(host, is_ssl=False, is_blocking=True):
    """Retrieves a socket connection to host.

    :param host: Host to connect to.
    :param is_ssl: Determines if SSL connection should be established.
    :param is_blocking: Determines if socket should be blocking.
    :return: Socket or SSLSocket.
    """
    try:
        if is_ssl:
            # If SSL is necessary then wrap socket in SSLContext object.
            context = ssl.SSLContext(ssl.PROTOCOL_SSLv23)
            s = context.wrap_socket(socket.socket(socket.AF_INET, socket.SOCK_STREAM))
            s.setblocking(is_blocking)
            s.connect((host, 443))
            return s
        else:
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.setblocking(is_blocking)
            s.connect((host, 80))
            return s
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def get_socket_response(s, request, max_bytes=4096):
    """Retrieves and logs request response from passed socket, up to maximum bytes.

    :param s: Socket with which to send request.
    :param request: Request (as bytes).
    :param max_bytes: Maximum number of bytes to receive.
    :return: Response data (as bytearray).
    """
    try:
        # Confirm that socket exists.
        if s is None:
            return None

        data = bytearray()

        # Send request.
        s.send(request)

        while True:
            # Get response and extend data array.
            response = s.recv(max_bytes)
            data.extend(response)

            # Break if no bytes, otherwise loop until max_bytes (or all available bytes) received.
            if len(response) == 0 or len(data) >= max_bytes or len(data) == len(response):
                break

        # Close socket.
        s.close()

        # Output decoded response.
        if data is not None:
            Logging.log(data.decode())

        # Return data.
        return data
    except BlockingIOError as error:
        # Output expected BlockingIOErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

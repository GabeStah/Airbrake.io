const net = require('net');
const logging = require('gw-logging');

function executeTests () {
  try {
    // Create server via socket.
    const server = net.createServer((socket) => {
      socket.end('Socket disconnected.\n');
    }).on('error', (e) => {
      throw e;
    });

    // Open server at port 24601.
    logging.lineSeparator(`SERVER OPENED AT PORT: ${24601}`, 50);
    server.listen(24601);

    // Perform series of client connections to different ports.
    logging.lineSeparator('CONNECT TO PORT: 24601', 50);
    connectToPort(24601);

    logging.lineSeparator('CONNECT TO PORT: 31234', 50);
    connectToPort(31234);

    logging.lineSeparator('CONNECT TO PORT: 1000000', 50);
    connectToPort(1000000);
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function connectToPort (port) {
  try {
    // Create connection at passed port.
    let client = net.createConnection({ port: port }, () => {
      // Log and output from client.
      logging.log(`Connected to server at port: ${port}`);
      client.write('Hello world!\r\n');
    });

    // Log data, if applicable.
    client.on('data', (data) => {
      logging.log(data.toString());
      client.end();
    });

    // When connection ends, log disconnection from server.
    client.on('end', () => {
      logging.log('Disconnected from server.');
    });
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();

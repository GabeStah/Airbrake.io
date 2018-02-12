/**
 * index.js
 */
const logging = require('logging');

//
// const assert = require('assert');
// const EventEmitter = require('events');
// const { SocketListSend } = require('internal/socket_list');
//
// {
//   const child = Object.assign(new EventEmitter(), { connected: false });
//   assert.strictEqual(child.listenerCount('internalMessage'), 0);
//
//   //const socket = new SocketList();
//   const list = new SocketListSend(child, 'test');
//
//   assert.strictEqual(child.listenerCount('internalMessage'), 0);
// }

function executeTests () {
  //test();
  //test2();
  //test3();
  test4();
}

function test () {
  const { execFile } = require('child_process');
  const child = execFile('node', ['--version'], (error, stdout, stderr) => {
    if (error) {
      throw error;
    }
    logging.log(stdout);
  });
}

function test2 () {
  try {
    const { spawn } = require('child_process');
    const ls = spawn('ls', ['-lh', '/usr']);
    //ls.kill('SIGHUP');
    //const ls = spawn('dir');

    ls.stdout.on('data', (data) => {
      console.log(`stdout: ${data}`);
    });

    ls.stderr.on('data', (data) => {
      console.log(`stderr: ${data}`);
    });

    ls.on('close', (code) => {
      console.log(`child process exited with code ${code}`);
    });
  } catch (e) {
    if (e instanceof Error && e.code === 'ERR_CHILD_CLOSED_BEFORE_REPLY') {
      // Output expected ERR_CHILD_CLOSED_BEFORE_REPLY RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function test3 () {
    try {
        const { spawn } = require('child_process');
        const grep = spawn('grep', ['ssh']);


        grep.on('close', (code, signal) => {
            console.log(
                `child process terminated due to receipt of signal ${signal}`);
        });

// Send SIGHUP to process
        grep.kill('SIGHUP');
    } catch (e) {
        if (e instanceof Error && e.code === 'ERR_CHILD_CLOSED_BEFORE_REPLY') {
            // Output expected ERR_CHILD_CLOSED_BEFORE_REPLY RangeErrors.
            logging.log(e);
        } else {
            // Output unexpected Errors.
            logging.log(e, false);
        }
    }
}

function test4 () {
    try {
        const net = require('net');

        let server = net.createServer(function (socket) {
            socket.write('Echo server\r\n');
            socket.pipe(socket);
        });

        server.listen(1337, '127.0.0.1');
        server.close();

        let client = new net.Socket();
        //client.destroy();
        client.connect(1337, '127.0.0.1', function() {
            console.log('Connected');
            client.write('Hello, server! Love, Client.');
        });

        client.on('data', function(data) {
            console.log('Received: ' + data);
            client.destroy(); // kill client after server's response
        });

        client.on('close', function() {
            console.log('Connection closed');
        });
    } catch (e) {
        if (e instanceof Error && e.code === 'ERR_CHILD_CLOSED_BEFORE_REPLY') {
            // Output expected ERR_CHILD_CLOSED_BEFORE_REPLY RangeErrors.
            logging.log(e);
        } else {
            // Output unexpected Errors.
            logging.log(e, false);
        }
    }
}

/**
 * Allocates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to allocate.
 * @returns {Buffer} Allocated Buffer.
 */
function allocateBuffer (size) {
  try {
    let buffer = Buffer.alloc(size);
    logging.log(`Successfully allocated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Instantiates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to instantiate.
 * @returns {Buffer} Instantiated Buffer.
 */
function instantiateBuffer (size) {
  try {
    let buffer = new Buffer(size);
    logging.log(`Successfully instantiated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
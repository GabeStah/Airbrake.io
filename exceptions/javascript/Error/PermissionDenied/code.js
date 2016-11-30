// 1
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    console.log(frames[0].document);
} catch (e) {
    if (e instanceof Error) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

// 2
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
  	console.log(frames[0].document);
} catch (e) {
    printError(e, false);
}

// 3
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function repeat() {
  repeat();
}

try {
  	repeat();
} catch (e) {
    if (e instanceof Error) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

// 4
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
  	console.log(frames[0].document);
} catch (e) {
	if (e.message.toLowerCase().indexOf('permission denied') == 0) {
    	printError(e, true);
    } else {
    	printError(e, false);
    }
}

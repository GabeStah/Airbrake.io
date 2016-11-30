// 1
console.log(String.fromCodePoint(0x265B));
console.log(String.fromCodePoint(9819));

// 3
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    console.log(String.fromCodePoint(-10));
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

// 4
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    console.log(String.fromCodePoint(-10));
} catch (e) {
    if (e instanceof RangeError) {
        if (e.message.toLowerCase().indexOf('code point') !== -1) {
            printError(e, true);
        } else {
            printError(e, false);
        }
    } else {
        printError(e, false);
    }
}

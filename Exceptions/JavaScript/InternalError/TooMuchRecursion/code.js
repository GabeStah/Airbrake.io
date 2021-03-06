// 1
var countdown = function(value) {
    console.log(value);
    return (value > 0) ? countdown(value - 1) : value;
};
countdown(10);

// 2
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
var countdown = function(value) {
    //console.log(value);
    try {
        if (value > 0) countdown(value - 1);
    } catch (e) {
        if (e instanceof InternalError) {
            printError(e, true);
        } else {
            printError(e, false);
        }
    }
};
countdown(10000);

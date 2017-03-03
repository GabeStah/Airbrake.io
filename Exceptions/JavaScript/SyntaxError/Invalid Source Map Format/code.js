function printError(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    console.log(`add.caller is ${add.caller}.`)
    console.log(`add.caller.name is ${add.caller.name}.`)
    return x + y;
}

try {
    var value = increment(1);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

//# sourceMappingURL=code-min.js.map
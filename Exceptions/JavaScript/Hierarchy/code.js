// Error
try {
  throw new Error('Uh oh');
} catch (e) {
  console.log(e.name + ': ' + e.message);
}

// EvalError
try {
  throw new EvalError('Uh oh', 'myFile.js', 25);
} catch (e) {
  console.log(e instanceof EvalError); // true
  console.log(e.message);              // "Uh oh"
  console.log(e.name);                 // "EvalError"
  console.log(e.fileName);             // "myFile.js"
  console.log(e.lineNumber);           // 25
}

// InternalError
function recurse(){
    recurse();
}
recurse();

// RangeError
try {
    var float = 1.2345;
    float.toFixed(21);
} catch (e) {
    console.log(e.name + ': ' + e.message);
}

// ReferenceError
try {
  var a = myUndefinedVariable;
} catch (e) {
  console.log(e.name + ': ' + e.message);
}

// SyntaxError
try {
  eval("this will fail");
} catch (e) {
  console.log(e.name + ': ' + e.message);
}

// TypeError
try {
  var foo = null;
  foo.myMethod();
} catch (e) {
  console.log(e.name + ': ' + e.message);
}

// URIError
try {
  decodeURI('%foo%bar%');
} catch (e) {
  console.log(e.name + ': ' + e.message);
}

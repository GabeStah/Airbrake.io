# Airbrake.io

Assorted projects for [`Airbrake.io`](https://airbrake.io/).

## Exception Checklist

### JavaScript

- [x] Error: Permission denied to access property "x"
- [x] InternalError: too much recursion
- [x] RangeError: argument is not a valid code point
    - [x] RangeError: invalid array length
    - [x] RangeError: precision is out of range
    - [x] RangeError: radix must be an integer
    - [ ] RangeError: repeat count must be less than infinity
    - [ ] RangeError: repeat count must be non-negative
    - [ ] ReferenceError: "x" is not defined
- [ ] ReferenceError: assignment to undeclared variable "x"
    - [ ] ReferenceError: deprecated caller or arguments usage
    - [ ] ReferenceError: invalid assignment left-hand side
    - [ ] ReferenceError: reference to undefined property "x"
- [ ] SyntaxError: "use strict" not allowed in function with non-simple parameters
    - [ ] SyntaxError: "x" is not a legal ECMA-262 octal constant
    - [ ] SyntaxError: JSON.parse: bad parsing
    - [ ] SyntaxError: Malformed formal parameter
    - [ ] SyntaxError: Unexpected token
    - [ ] SyntaxError: Using //@ to indicate sourceURL pragmas is deprecated. Use //# instead
    - [ ] SyntaxError: missing ) after argument list
    - [ ] SyntaxError: missing ; before statement
    - [ ] SyntaxError: missing ] after element list
    - [ ] SyntaxError: missing } after property list
    - [ ] SyntaxError: redeclaration of formal parameter "x"
    - [ ] SyntaxError: return not in function
    - [ ] SyntaxError: test for equality (==) mistyped as assignment (=)?
    - [ ] SyntaxError: unterminated string literal
- [ ] TypeError: "x" has no properties
    - [ ] TypeError: "x" is (not) "y"
    - [ ] TypeError: "x" is not a constructor
    - [ ] TypeError: "x" is not a function
    - [ ] TypeError: "x" is read-only
    - [ ] TypeError: More arguments needed
    - [ ] TypeError: invalid Array.prototype.sort argument
    - [ ] TypeError: property "x" is non-configurable and can't be deleted
    - [ ] TypeError: variable "x" redeclares argument
- [ ] Warning: -file- is being assigned a //# sourceMappingURL, but already has one
    - [ ] Warning: JavaScript 1.6's for-each-in loops are deprecated
    - [ ] Warning: unreachable code after return statement

### Ruby

- [x] NoMemoryError
- [x] ScriptError
  - [x] LoadError
  - [x] NotImplementedError
  - [x] SyntaxError
- [x] SecurityError
- [ ] SignalException
  - [ ] Interrupt
- [ ] StandardError -- default for rescue
  - [ ] ArgumentError
    - [ ] UncaughtThrowError
  - [ ] EncodingError
  - [ ] FiberError
  - [ ] IOError
    - [ ] EOFError
  - [ ] IndexError
    - [ ] KeyError
    - [ ] StopIteration
  - [ ] LocalJumpError
  - [ ] NameError
    - [ ] NoMethodError
  - [ ] RangeError
    - [ ] FloatDomainError
  - [ ] RegexpError
  - [ ] RuntimeError -- default for raise
  - [ ] SystemCallError
    - [ ] Errno::* __Part of `SystemCallError`?__
  - [ ] ThreadError
  - [ ] TypeError
  - [ ] ZeroDivisionError
- [ ] SystemExit
- [ ] SystemStackError
- [ ] fatal – impossible to rescue

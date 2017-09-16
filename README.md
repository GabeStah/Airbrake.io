# Airbrake.io

Assorted projects for [`Airbrake.io`](https://airbrake.io/).

## Exception Checklist

### PHP

- [x] `Throwable`
  - [x] `Error`
    - [x] `ArithmeticError`
      - [x] `DivisionByZeroError`
    - [x] `AssertionError`
    - [x] `ParseError`
    - [x] `TypeError`
  - [x] `Exception`
    - [x] `ClosedGeneratorException`
    - [x] `DOMException`
    - [x] `ErrorException`
    - [ ] `IntlException`
    - [ ] `LogicException`
      - [x] `BadFunctionCallException`
        - [x] `BadMethodCallException`
      - [x] `DomainException`
      - [x] `InvalidArgumentException`
      - [x] `LengthException`
      - [x] `OutOfRangeException`
    - [x] `PharException`
    - [x] `ReflectionException`
    - [x] `RuntimeException`
      - [ ] `mysqli_sql_exception`
      - [x] `OutOfBoundsException`
      - [ ] `OverflowException`
      - [ ] `PDOException`
      - [ ] `RangeException`
      - [ ] `UnderflowException`
      - [ ] `UnexpectedValueException`

### JavaScript

- [x] Error: Permission denied to access property "x"
- [x] InternalError: too much recursion
- RangeError
    - [x] RangeError: argument is not a valid code point
    - [x] RangeError: invalid array length
    - [x] RangeError: precision is out of range
    - [x] RangeError: radix must be an integer
    - [x] RangeError: repeat count must be less than infinity
    - [x] RangeError: repeat count must be non-negative
- ReferenceError
    - [x] ReferenceError: "x" is not defined
    - [x] ReferenceError: assignment to undeclared variable "x"
    - [x] ReferenceError: deprecated caller or arguments usage
    - [x] ReferenceError: invalid assignment left-hand side
    - [x] ReferenceError: reference to undefined property "x"
- SyntaxError
    - [x] SyntaxError: "use strict" not allowed in function with non-simple parameters
    - [x] SyntaxError: "x" is not a legal ECMA-262 octal constant
    - [x] SyntaxError: JSON.parse: bad parsing
    - [x] SyntaxError: Malformed formal parameter
    - [x] SyntaxError: Unexpected token
    - [x] SyntaxError: Using //@ to indicate sourceURL pragmas is deprecated. Use //# instead
    - [x] SyntaxError: missing ) after argument list
    - [x] SyntaxError: missing ; before statement
    - [x] SyntaxError: missing ] after element list
    - [x] SyntaxError: missing } after property list
    - [x] SyntaxError: redeclaration of formal parameter "x"
    - [x] SyntaxError: return not in function
    - [x] SyntaxError: test for equality (==) mistyped as assignment (=)?
    - [x] SyntaxError: unterminated string literal
- TypeError
    - [x] TypeError: "x" has no properties
    - [x] TypeError: "x" is (not) "y"
    - [x] TypeError: "x" is not a constructor
    - [x] TypeError: "x" is not a function
    - [x] TypeError: "x" is read-only
    - [x] TypeError: More arguments needed
    - [x] TypeError: invalid Array.prototype.sort argument
    - [x] TypeError: property "x" is non-configurable and can't be deleted
    - [x] TypeError: variable "x" redeclares argument
- Warning
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
- [x] SignalException
  - [x] Interrupt
- [x] StandardError -- default for rescue
  - [x] ArgumentError
    - [x] UncaughtThrowError
  - [x] EncodingError
    - [x] CompatibilityError
    - [x] ConverterNotFoundError
    - [x] InvalidByteSequenceError
    - [x] UndefinedConversionError
  - [x] FiberError
  - [x] IOError
    - [x] EOFError
  - [x] IndexError
    - [x] KeyError
    - [x] StopIteration
  - [x] LocalJumpError
  - [x] NameError
    - [x] NoMethodError
  - [x] RangeError
    - [x] FloatDomainError
  - [x] RegexpError
  - [x] RuntimeError -- default for raise
  - [x] SystemCallError
    - [x] Errno::*
  - [x] ThreadError
  - [x] TypeError
  - [x] ZeroDivisionError
- [x] SystemExit
- [x] SystemStackError
- [x] fatal – impossible to rescue

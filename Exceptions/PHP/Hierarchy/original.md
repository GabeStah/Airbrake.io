PHP’s built in exception classes makes **PHP exception handling** simple! PHP exceptions are broken down into a hierarchy, with **LogicException** and **RuntimeException** serving as the parent classes. Both `LogicException` and `RuntimeException` extend the `Exception` class. Here’s a look at the entire PHP `Exception` class hierarchy.

- `Exception`
  - `LogicException`
    - `BadFunctionCallException`
      - `BadMethodCallException`
    - `DomainException`
    - `InvalidArgumentException`
    - `LengthException`
    - `OutOfRangeException`
  - `RuntimeException`
    - `OutOfBoundsException`
    - `OverflowException`
    - `RangeException`
    - `UnderflowException`
    - `UnexpectedValueException`

`LogicException` refers to to any error that occurs due to faulty programmatic logic. For example, if an argument of an undesired data type is passed, `LogicException` should be thrown. Here are quick descriptions of its child classes.

`BadFunctionCallException` – Exception thrown if a callback refers to an undefined function or there are missing arguments.
`BadMethodCallException` – Extends `BadFunctionCallException`. Exception thrown if a callback refers to an undefined method or there are missing arguments.
`DomainException` – Exception thrown if a value doesn't match a defined valid data domain.
`InvalidArgumentException` – Exception thrown if an argument differs from expected type.
`LengthException` – Exception thrown if a length is invalid.
`OutOfRangeException` – Exception thrown when an illegal index was requested. This represents errors that should be detected at compile time.

`RuntimeException` refers to any error which can only occur on runtime. For example, if a function outputs an unacceptable value, a `RuntimeException` should be thrown. Here are its child classes:

- `OutOfBoundsException` – Exception thrown if a value is not a valid key.
- `OverflowException` – Exception thrown when adding an element to a full container.
- `RangeException` – Exception thrown to indicate range errors during program execution. The runtime version of `DomainException`.
- `UnderflowException` – Exception thrown when performing an invalid operation on an empty container.
- `UnexpectedValueException` – Exception thrown if a value does not match with a set of values.

These built-in exception classes are helpful PHP exception handling tools! Used with Airbrake’s [PHP exception tracker](https://airbrake.io/languages/php_bug_tracker) your debugging process will be a breeze! Good luck!
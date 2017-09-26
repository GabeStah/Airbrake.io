# Behavioral Design Patterns: State

As we approach the end of our thorough [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series we'll be looking into the **state design pattern**.  The `state pattern` allows you to programmatically change the behavior of a class based on changes made to the underlying _state_ of said class.  In this article we'll look at both a real world example and a fully-functional C# code sample of the `state design pattern`, so let's get to it!

## In the Real World

The `state pattern` consists of three basic components:

- `Context` - The base object that will contain a `State` object, indicating what state (and therefore what behavior) is currently implemented.
- `State` - An interface or abstract class defining the basic characteristics (methods, properties, etc) of all `ConcreteState` objects.
- `ConcreteState` - These individual classes implement the base `State` interface/abstract class.  Each `ConcreteState` can implement its own logic and behavior, which will affect the `Context` instance when it is assigned to a particular `ConcreteState`.

As you can start to see, the purpose of the `state design pattern` is to allow `Context` objects to adjust their behavior solely because of the change of the current `ConcreteState(s)` that may be applied.  While this sort of logic _can_ be performed with traditional `if-else` control statements, it's far cleaner to apply `State` objects to a `Context` object, so said `Context` doesn't need to be aware of how the `State` logic is implemented.

In the real world, this can be seen all over, particularly in digital services and technologies.  For example, consider making a purchase with your debit card, or depositing money into that same checking account.  Behind the scenes, it's likely that your account behaves as a `Context` object, with various `ConcreteStates` assigned to it, dependent on the characteristics of your account.  If your bank has a minimum balance requirement before your account accrues interest, this change in behavior could be easily handled with a handful of `ConcreteState` objects.  When your balance meets or exceeds the minimum balance threshold, the state of your account changes, and additional behaviors (such as applying interest) may be automatically put into action.

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// Program.cs
namespace State
{
    internal class Program
    {
        private static void Main()
        {
            // Create a new account.
            var account = new Account("Alice Smith");

            // Make a few deposits.
            account.Deposit(450);
            account.Deposit(500);
            // This deposit should increase balance 
            // enough to begin accruing interest.
            account.Deposit(550);
            account.Deposit(805);

            // Accrue interest.
            account.AccrueInterest();

            // Make a few withdrawals.
            account.Withdraw(2500);
            account.Withdraw(1500);
        }
    }
}
```

```cs
// Account.cs
using Utility;

namespace State
{
    /// <summary>
    /// Customer account, which retains an ongoing balance and state.
    /// 
    /// Behaves as a Context object in the State pattern.
    /// </summary>
    internal class Account
    {
        private readonly string _owner;

        public double Balance => AccountState.Balance;
        public AccountState AccountState { get; set; }

        public Account(string owner)
        {
            _owner = owner;
            AccountState = new ZeroInterestAccountState(0, this);
        }

        public void Deposit(double amount)
        {
            // Ensure deposit was successful.
            if (!AccountState.Deposit(amount)) return;
            Logging.LineSeparator($"Deposited: {amount:C}");
            Logging.Log(ToString());
        }

        public double? AccrueInterest()
        {
            var interest = AccountState.AccrueInterest();
            Logging.LineSeparator($"Interest Earned: {interest:C}");
            Logging.Log(ToString());

            return interest;
        }

        public void Withdraw(double amount)
        {
            // Ensure withdrawal was successful.
            if (!AccountState.Withdraw(amount)) return;
            Logging.LineSeparator($"Withdrew: {amount:C}.");
            Logging.Log(ToString());
        }

        public override string ToString()
        {
            var output = $"{"ACCOUNT OWNER",-20}{"BALANCE",-20}STATE\n";
            output += $"{_owner,-20}{Balance,-20:C}{AccountState.GetType().Name}";

            return output;
        }
    }
}
```

```cs
// AccountState.cs
namespace State
{
    /// <summary>
    /// Base state to which an Account can be assigned.
    /// Cannot be directly inherited.
    /// 
    /// Behaves as a State in the State pattern.
    /// </summary>
    internal abstract class AccountState
    {
        protected double InterestRate;
        protected double LowerLimit;
        protected double UpperLimit;

        public Account Account { get; set; }
        public double Balance { get; set; }

        public abstract double? AccrueInterest();
        public abstract bool Deposit(double amount);
        public abstract void TryStateChange();
        public abstract bool Withdraw(double amount);
    }
}
```

```cs
// InterestAccountState.cs
namespace State
{
    /// <summary>
    /// State that indicates the Account is actively accruing interest.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class InterestAccountState : AccountState
    {
        private new const double InterestRate = 0.05;
        private new const double LowerLimit = 1_000;
        private new const double UpperLimit = 1_000_000;

        public InterestAccountState(AccountState accountState)
            : this(accountState.Balance, accountState.Account)
        {
        }

        public InterestAccountState(double balance, Account account)
        {
            Balance = balance;
            Account = account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        public override double? AccrueInterest()
        {
            var accruedInterest = InterestRate * Balance;
            Balance += accruedInterest;
            TryStateChange();
            return accruedInterest;
        }

        public override void TryStateChange()
        {
            if (Balance < 0.0)
            {
                Account.AccountState = new OverdrawnAccountState(this);
            }
            else if (Balance < LowerLimit)
            {
                Account.AccountState = new ZeroInterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Balance -= amount;
            TryStateChange();
            return true;
        }
    }
}
```

```cs
// OverdrawnAccountState.cs
using Utility;

namespace State
{
    /// <summary>
    /// State that indicates the Account is overdrawn.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class OverdrawnAccountState : AccountState
    {
        private new const double InterestRate = 0;
        private new const double LowerLimit = -1_000;
        private new const double UpperLimit = 0;

        public OverdrawnAccountState(AccountState accountState)
        {
            Balance = accountState.Balance;
            Account = accountState.Account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        /// <summary>
        /// Accrue current interest.
        /// </summary>
        /// <returns>Null, since account is overdrawn.</returns>
        public override double? AccrueInterest()
        {
            return null;
        }

        public override void TryStateChange()
        {
            if (Balance > UpperLimit)
            {
                Account.AccountState = new ZeroInterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Logging.Log($"ALERT: Unable to withdraw {amount:C} due to lack of funds.");
            Logging.Log(Account.ToString());
            // Withdrawal failed.
            return false;
        }
    }
}
```

```cs
// ZeroInterestAccountState.cs
namespace State
{
    /// <summary>
    /// State that indicates the Account is not accruing interest.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class ZeroInterestAccountState : AccountState
    {
        private new const double InterestRate = 0;
        private new const double LowerLimit = 0;
        private new const double UpperLimit = 1_000;

        public ZeroInterestAccountState(AccountState accountState) :
            this(accountState.Balance, accountState.Account)
        {
        }

        public ZeroInterestAccountState(double balance, Account account)
        {
            Balance = balance;
            Account = account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        public override double? AccrueInterest()
        {
            var accruedInterest = InterestRate * Balance;
            Balance += accruedInterest;
            TryStateChange();
            return accruedInterest;
        }

        public override void TryStateChange()
        {
            if (Balance < LowerLimit)
            {
                Account.AccountState = new OverdrawnAccountState(this);
            }
            else if (Balance > UpperLimit)
            {
                Account.AccountState = new InterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Balance -= amount;
            TryStateChange();
            return true;
        }
    }
}
````

```cs
// <Utility/>Logging.cs
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Output(value, outputType);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        public static void Log(string value, object arg0)
        {
            Debug.WriteLine(value, arg0);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(string value, object arg0, object arg1)
        {
            Debug.WriteLine(value, arg0, arg1);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(string value, object arg0, object arg1, object arg2)
        {
            Debug.WriteLine(value, arg0, arg1, arg2);
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Output(value, outputType);
        }

        private static void Output(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            if (value is IXmlSerializable)
            {
                Debug.WriteLine(value);
            }
            else
            {
                Debug.WriteLine(outputType == OutputType.Timestamp
                    ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                    : ObjectDumper.Dump(value));
            }
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= insert.Length + 2;
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

## How It Works In Code

Our code sample continues with the bank account example, since just about everyone will have experience with the mechanics of depositing and withdrawing from a simple checking account.  We start with the `Context` object, since this is the element upon which the entire state system is based.  In this case, our `Context` object is the `Account` class:

```cs
using Utility;

namespace State
{
    /// <summary>
    /// Customer account, which retains an ongoing balance and state.
    /// 
    /// Behaves as a Context object in the State pattern.
    /// </summary>
    internal class Account
    {
        private readonly string _owner;

        public double Balance => AccountState.Balance;
        public AccountState AccountState { get; set; }

        public Account(string owner)
        {
            _owner = owner;
            AccountState = new ZeroInterestAccountState(0, this);
        }

        public void Deposit(double amount)
        {
            // Ensure deposit was successful.
            if (!AccountState.Deposit(amount)) return;
            Logging.LineSeparator($"Deposited: {amount:C}");
            Logging.Log(ToString());
        }

        public double? AccrueInterest()
        {
            var interest = AccountState.AccrueInterest();
            Logging.LineSeparator($"Interest Earned: {interest:C}");
            Logging.Log(ToString());

            return interest;
        }

        public void Withdraw(double amount)
        {
            // Ensure withdrawal was successful.
            if (!AccountState.Withdraw(amount)) return;
            Logging.LineSeparator($"Withdrew: {amount:C}.");
            Logging.Log(ToString());
        }

        public override string ToString()
        {
            var output = $"{"ACCOUNT OWNER",-20}{"BALANCE",-20}STATE\n";
            output += $"{_owner,-20}{Balance,-20:C}{AccountState.GetType().Name}";

            return output;
        }
    }
}
```

As you can see, the `Account` class performs the basic functions: depositing money, withdrawing money, and accruing interest on the current balance.  However, to accomplish these actions and change the behavior, we need the underlying `AccountState` object, which each `Account` tracks and changes for itself, as necessary:

```cs
namespace State
{
    /// <summary>
    /// Base state to which an Account can be assigned.
    /// Cannot be directly inherited.
    /// 
    /// Behaves as a State in the State pattern.
    /// </summary>
    internal abstract class AccountState
    {
        protected double InterestRate;
        protected double LowerLimit;
        protected double UpperLimit;

        public Account Account { get; set; }
        public double Balance { get; set; }

        public abstract double? AccrueInterest();
        public abstract bool Deposit(double amount);
        public abstract void TryStateChange();
        public abstract bool Withdraw(double amount);
    }
}
```

`AccountState` is the base `State` object of our `state pattern` example, as it defines all the base properties and methods that each inherited `ConcreteAccountState` object will require.  With the base `AccountState` abstract class in place, we can start implementing some specific `ConcreteAccountStates`, starting with the `ZeroInterestAccountState`:

```cs
namespace State
{
    /// <summary>
    /// State that indicates the Account is not accruing interest.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class ZeroInterestAccountState : AccountState
    {
        private new const double InterestRate = 0;
        private new const double LowerLimit = 0;
        private new const double UpperLimit = 1_000;

        public ZeroInterestAccountState(AccountState accountState) :
            this(accountState.Balance, accountState.Account)
        {
        }

        public ZeroInterestAccountState(double balance, Account account)
        {
            Balance = balance;
            Account = account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        public override double? AccrueInterest()
        {
            var accruedInterest = InterestRate * Balance;
            Balance += accruedInterest;
            TryStateChange();
            return accruedInterest;
        }

        public override void TryStateChange()
        {
            if (Balance < LowerLimit)
            {
                Account.AccountState = new OverdrawnAccountState(this);
            }
            else if (Balance > UpperLimit)
            {
                Account.AccountState = new InterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Balance -= amount;
            TryStateChange();
            return true;
        }
    }
}
```

Everything works as you might expect, but it's worth noting the `TryStateChange()` method that is invoked inside every other major method call.  This is where this specific `ConcreteState` object determines if the assigned `Account.AccountState` should be changed to a different state or not.  In this case, if the balance falls below the `LowerLimit` of `$0`, we want to be sure the account is now considered overdrawn.  On the other hand, if the balance exceeds the `UpperLimit` of `$1,000`, the account should start accruing interest.

Now, the `TryStateChange()` method and logic _could_ be placed within the base `AccountState` object, but keeping it separated, and inside each individual `ConcreteAccountState` class, ensures that each class can have distinctly specific logic and behavior in the future, regardless of what other states may be doing.  It's also likely that in real-world code we'd opt to implement all the various calls to the `TryStateChange()` method in a more elegant manner, perhaps by linking all `Withdraw`, `Deposit`, and similar methods to an `event`, that could then ensure `TryStateChange()` logic is invoked when appropriate.  But, for our purposes, this simple setup will suffice.

Next we have the `InterestAccountState` which, as we just saw, is applied when the balance exceeds `$1,000`:

```cs
namespace State
{
    /// <summary>
    /// State that indicates the Account is actively accruing interest.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class InterestAccountState : AccountState
    {
        private new const double InterestRate = 0.05;
        private new const double LowerLimit = 1_000;
        private new const double UpperLimit = 1_000_000;

        public InterestAccountState(AccountState accountState)
            : this(accountState.Balance, accountState.Account)
        {
        }

        public InterestAccountState(double balance, Account account)
        {
            Balance = balance;
            Account = account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        public override double? AccrueInterest()
        {
            var accruedInterest = InterestRate * Balance;
            Balance += accruedInterest;
            TryStateChange();
            return accruedInterest;
        }

        public override void TryStateChange()
        {
            if (Balance < 0.0)
            {
                Account.AccountState = new OverdrawnAccountState(this);
            }
            else if (Balance < LowerLimit)
            {
                Account.AccountState = new ZeroInterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Balance -= amount;
            TryStateChange();
            return true;
        }
    }
}
```

For the most part, the behavior is the same here as with the `ZeroInterestAccountState` class, except calling `AccrueInterest()` actually calculates and adds the applied interest, based on the current interest rate, to the `Account.Balance`.

Finally, the `OverdrawnAccountState` class is for `Accounts` with a `Balance` below `$0`:

```cs
using Utility;

namespace State
{
    /// <summary>
    /// State that indicates the Account is overdrawn.
    /// 
    /// Behaves as a ConcreteState in the State pattern.
    /// </summary>
    internal class OverdrawnAccountState : AccountState
    {
        private new const double InterestRate = 0;
        private new const double LowerLimit = -1_000;
        private new const double UpperLimit = 0;

        public OverdrawnAccountState(AccountState accountState)
        {
            Balance = accountState.Balance;
            Account = accountState.Account;
        }

        public override bool Deposit(double amount)
        {
            Balance += amount;
            TryStateChange();
            return true;
        }

        /// <summary>
        /// Pay current interest.
        /// </summary>
        /// <returns>Null, since account is overdrawn.</returns>
        public override double? AccrueInterest()
        {
            return null;
        }

        public override void TryStateChange()
        {
            if (Balance > UpperLimit)
            {
                Account.AccountState = new ZeroInterestAccountState(this);
            }
        }

        public override bool Withdraw(double amount)
        {
            Logging.Log($"ALERT: Unable to withdraw {amount:C} due to lack of funds.");
            Logging.Log(Account.ToString());
            // Withdrawal failed.
            return false;
        }
    }
}
```

No interest can be accrued, nor can any withdrawals be made, so any attempt to do so issues a warning indicating a lack of funds.

To tie everything together and test it out we just create a new `Account`, then perform some deposits:

```cs
// Program.cs
namespace State
{
    internal class Program
    {
        private static void Main()
        {
            // Create a new account.
            var account = new Account("Alice Smith");

            // Make a few deposits.
            account.Deposit(450);
            account.Deposit(500);

            // ...
        }
    }
}
```

This produces two outputs, indicating the deposited values, along with the current `Balance` and `AccountState`:

```
---------- Deposited: $450.00 ----------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         $450.00             ZeroInterestAccountState
---------- Deposited: $500.00 ----------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         $950.00             ZeroInterestAccountState
```

Since we haven't exceeded the `$1,000` minimum to start earning interest, `Alice's` account remains in the `ZeroInterestAccountState`.  However, let's try depositing a few more times, so the `Balance` exceeds that limit:

```cs
// This deposit should increase balance 
// enough to begin accruing interest.
account.Deposit(550);
account.Deposit(805);
```

Sure enough, the `AccountState` automatically changes to the `InterestAccountState`:

```
---------- Deposited: $550.00 ----------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         $1,500.00           InterestAccountState
---------- Deposited: $805.00 ----------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         $2,305.00           InterestAccountState
```

Now we can successfully accrue some interest on the account:

```cs
// Pay interest.
account.AccrueInterest();
```

```
------- Interest Earned: $115.25 -------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         $2,420.25           InterestAccountState
```

Finally, let's try withdrawing far more than the account contains.  We'll start with `$2,500`, which succeeds because, even though the total `Balance` is only `2,420.25`, an `OverdrawnAccountState` is allowed to go all the way down to the `LowerLimit` of `-$1,000`:

```cs
// Make a few withdrawals.
account.Withdraw(2500);
```

This puts Alice in the negative:

```
--------- Withdrew: $2,500.00. ---------
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         ($79.75)            OverdrawnAccountState
```

Now let's try another withdrawal of over `$1,000` on her already overdrawn account, and see what happens:

```cs
account.Withdraw(1500);
```

As expected, no action is taken and alert is given, indicating the severe lack of funds:

```
ALERT: Unable to withdraw $1,500.00 due to lack of funds.
ACCOUNT OWNER       BALANCE             STATE
Alice Smith         ($79.75)            OverdrawnAccountState
```

That's the gist of it!  I hope this article gave you a bit more information on what the `state design pattern` is, and how it can be easily implemented in your own code.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 22 of our Software Design Pattern series in which examine the state design pattern using fully-functional C# example code.

# Structural Design Patterns: Proxy

Today we're looking at the last of the `Structural` design patterns within our comprehensive [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  The proxy design pattern is intended to act as a simple wrapper for another object.  The `proxy` object can be directly accessed by the user and can perform logic or configuration changes required by the underlying `subject` object, without giving the client direct access to said `subject`.

In this article we'll examine the `proxy design pattern` in more detail, looking at a real world example, along with a fully-functional C# code sample that will help to illustrate how the pattern might be implemented, so let's get started!

## In the Real World

In the real world, many of us use a `proxy pattern` on a daily basis: Every time we hand our debit card to the barista or allow Amazon to charge our credit card for that next sweet bargain, we're using a dominant (and powerful) `proxy design pattern` found throughout the modern world.  A debit card is merely a representational proxy of a bank account.  Money orders and checks act in much the same manner, which [Jerry Seinfeld explores](https://www.youtube.com/watch?v=CWD6C5Ty7R8) in one of his great bits: "A check is like a note from your mother that says, 'I don't have any money, but if you contact these people I'm sure they'll stick up for me.  If you could just trust me this one time.  I don't have any money, but I have these.  I wrote on these...  Is this of any value at all?'"

Just like checks, your credit card acts as a `proxy` for your bank account.  It's a (relatively) secure way for a client (such as your favorite local coffee shop) to post a charge to your underlying account, without allowing that vendor direct access to said account.  This kind of system is used all the time due to its simplicity and ease of use.

## How It Works In Code

For the `proxy design pattern` code sample we'll stick with the credit card example above and implement a few classes that illustrate the basic concepts that a client would use to interact with your underlying account by going through a proxy of a credit or debit card.  For simplicity, we'll start with the full working code sample below, after which we'll explore it in more detail:

```cs
using Utility;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountChargeTests();
            Logging.LineSeparator();
            OverchargeTests();
        }

        internal static void AccountChargeTests()
        {
            // Create new debit card proxy instance.
            var debitCard = new DebitCard();
            // Attempt two successful charges.
            debitCard.ChargeAccount(125.50M);
            debitCard.ChargeAccount(500);
            // Attempt overcharge.
            debitCard.ChargeAccount(432.10M);
        }

        internal static void OverchargeTests()
        {
            // Create new credit card proxy instance and allow overcharges.
            var creditCard = new CreditCard();
            // Attempt two successful charges.
            creditCard.ChargeAccount(125.50M);
            creditCard.ChargeAccount(500);
            // Attempt overcharge.
            creditCard.ChargeAccount(432.10M);
        }
    }

    /// <summary>
    /// Subject class that defines account info.
    /// </summary>
    interface IAccount
    {
        decimal Balance { get; set; }
        bool ChargeAccount(decimal amount);
    }

    /// <summary>
    /// RealSubject class that handles account management.
    /// </summary>
    class Account : IAccount
    {
        // Set default balance to $1000.
        private decimal _balance = 1000;

        public decimal Balance
        {
            get => _balance;
            set
            {
                // Log output when balance is changed.
                if (value == _balance) return;
                Logging.Log($"Balance changed from {_balance:C2} to {value:C2}.");
                _balance = value;
            }
        }

        /// <summary>
        /// Attempt to charge account the passed amount.
        /// </summary>
        /// <param name="amount">Amount to charge.</param>
        /// <returns>Success or failure of charge.</returns>
        public bool ChargeAccount(decimal amount)
        {
            // Check if balance meets or exceeds charge amount.
            if (Balance >= amount)
            {
                // Modify balance.
                Balance -= Balance;
                // Log successful charge message.
                Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
                // Charge was successful.
                return true;
            }
            // Log that charge failed due to insufficient funds.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} failed due to insufficient funds.");
            // Charge failed.
            return false;
        }
    }

    /// <summary>
    /// Proxy class for Account handles Credit transacations.
    /// </summary>
    class CreditCard : IAccount
    {
        private readonly Account _account;
        public decimal Balance { get; set; }

        /// <summary>
        /// Instantiate CreditCard proxy class while also creating new Account instance.
        /// </summary>
        public CreditCard()
        {
            _account = new Account();
        }

        /// <summary>
        /// Instantiate CreditCard proxy class with passed Account instance.
        /// </summary>
        /// <param name="account">Underlying Account instance to use.</param>
        public CreditCard(Account account)
        {
            _account = account;
        }

        /// <summary>
        /// Charge passed amount to underlying account.
        /// </summary>
        /// <param name="amount">Amount of charge.</param>
        /// <returns>Succes or failure of charge.</returns>
        public bool ChargeAccount(decimal amount)
        {
            // Check if balance meets or exceeds charge amount.
            if (_account.Balance >= amount)
            {
                // Log successful charge message.
                Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
            }
            else
            {
                // Log successful charge message, indicating overcharge occurred.
                Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded, as overcharge.");
            }
            // Modify balance.
            _account.Balance -= amount;
            // Charge was successful.
            return true;
        }
    }

    /// <summary>
    /// Proxy class for Account that handles Debit transacations.
    /// </summary>
    class DebitCard : IAccount
    {
        private readonly Account _account;
        public decimal Balance { get; set; }

        /// <summary>
        /// Instantiate DebitCard proxy class while also creating new Account instance.
        /// </summary>
        public DebitCard()
        {
            _account = new Account();
        }

        /// <summary>
        /// Instantiate DebitCard proxy class with passed Account instance.
        /// </summary>
        /// <param name="account">Underlying Account instance to use.</param>
        public DebitCard(Account account)
        {
            _account = account;
        }

        /// <summary>
        /// Charge passed amount to underlying account.
        /// </summary>
        /// <param name="amount">Amount of charge.</param>
        /// <returns>Succes or failure of charge.</returns>
        public bool ChargeAccount(decimal amount)
        {
            // Check if balance meets or exceeds charge amount.
            if (_account.Balance >= amount)
            {
                // Log successful charge message.
                Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
                // Modify balance.
                _account.Balance -= amount;
                // Charge was successful.
                return true;
            }

            // Log that charge failed due to insufficient funds.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} failed due to insufficient funds.  Current balance: {_account.Balance:C2}.");
            // Charge failed.
            return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="System.Diagnostics.Debug.WriteLine"/> 
        /// if DEBUG mode is enabled, otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        public static void LineSeparator(int length = 40)
        {
#if DEBUG
            Debug.WriteLine(new string('-', length));
#else
            Console.WriteLine(new string('-', length));
#endif
        }
    }
}
```

---

The basic structure of a `proxy pattern` consists of three components:

- `Subject`: The baseline interface that defines the fundamental components of the subject in question.
- `RealSubject`: The actual subject class that implements the `Subject` interface, and which the `Proxy` class will access behind the scenes and "wrap" to enable easier client interaction.
- `Proxy`: The wrapper class that enables the client to perform actions upon the underlying `RealSubject` object, without performing any direct actions upon it.

Therefore, we begin by implementing the `Subject` interface, which we're calling `IAccount` to represent a bank account.  For this exceptionally simple example we're just tracking one property (`Balance`) and one method (`ChargeAccount(decimal amount)`), which are both self-explanatory in function:

```cs
/// <summary>
/// Subject class that defines account info.
/// </summary>
interface IAccount
{
    decimal Balance { get; set; }
    bool ChargeAccount(decimal amount);
}
```

Now we need to create a `RealSubject` class that will implement the `IAccount` interface, so here we have the `Account` class:

```cs
/// <summary>
/// RealSubject class that handles account management.
/// </summary>
class Account : IAccount
{
    // Set default balance to $1000.
    private decimal _balance = 1000;

    public decimal Balance
    {
        get => _balance;
        set
        {
            // Log output when balance is changed.
            if (value == _balance) return;
            Logging.Log($"Balance changed from {_balance:C2} to {value:C2}.");
            _balance = value;
        }
    }

    /// <summary>
    /// Attempt to charge account the passed amount.
    /// </summary>
    /// <param name="amount">Amount to charge.</param>
    /// <returns>Success or failure of charge.</returns>
    public bool ChargeAccount(decimal amount)
    {
        // Check if balance meets or exceeds charge amount.
        if (Balance >= amount)
        {
            // Modify balance.
            Balance -= Balance;
            // Log successful charge message.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
            // Charge was successful.
            return true;
        }
        // Log that charge failed due to insufficient funds.
        Logging.Log($"{this.GetType().Name} charge of {amount:C2} failed due to insufficient funds.");
        // Charge failed.
        return false;
    }
}
```

_Note_: It's worth pointing out that we've elected to specify a default balance value of `$1000` for this example, whereas this amount would obviously be grabbed from a data service in production code.

Most of the logic takes place in the `ChargeAccount(decimal amount)` method, where we check if the `Balance` is greater than or equal to the charge `amount`, and either reduce the balance appropriately and issue a success message, or issue a failure message.

Finally, we need to create at least one `Proxy` class that will attach itself to our `RealSubject` class (`Account`) in some way, and then perform actions that "wrap" those underlying class methods.  For this example we've actually defined two proxy classes with `CreditCard` and `DebitCard`, which both perform similar actions.  However, their major difference is that `CreditCard` allows charge attempts to exceed the current `Balance` by performing an overcharge (i.e. simulating a credit card), whereas the `DebitCard` class prevents charges that would exceed the current `Balance`.

In both cases, it's important that we access an `Account` class instance, so we've defined two constructors, one where an `Account` instance can be passed, and the other where a new `Account` instance is generated during construction:

```cs
/// <summary>
/// Proxy class for Account handles Credit transactions.
/// </summary>
class CreditCard : IAccount
{
    private readonly Account _account;
    public decimal Balance { get; set; }

    /// <summary>
    /// Instantiate CreditCard proxy class while also creating new Account instance.
    /// </summary>
    public CreditCard()
    {
        _account = new Account();
    }

    /// <summary>
    /// Instantiate CreditCard proxy class with passed Account instance.
    /// </summary>
    /// <param name="account">Underlying Account instance to use.</param>
    public CreditCard(Account account)
    {
        _account = account;
    }

    /// <summary>
    /// Charge passed amount to underlying account.
    /// </summary>
    /// <param name="amount">Amount of charge.</param>
    /// <returns>Success or failure of charge.</returns>
    public bool ChargeAccount(decimal amount)
    {
        // Check if balance meets or exceeds charge amount.
        if (_account.Balance >= amount)
        {
            // Log successful charge message.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
        }
        else
        {
            // Log successful charge message, indicating overcharge occurred.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded, as overcharge.");
        }
        // Modify balance.
        _account.Balance -= amount;
        // Charge was successful.
        return true;
    }
}

/// <summary>
/// Proxy class for Account that handles Debit transactions.
/// </summary>
class DebitCard : IAccount
{
    private readonly Account _account;
    public decimal Balance { get; set; }

    /// <summary>
    /// Instantiate DebitCard proxy class while also creating new Account instance.
    /// </summary>
    public DebitCard()
    {
        _account = new Account();
    }

    /// <summary>
    /// Instantiate DebitCard proxy class with passed Account instance.
    /// </summary>
    /// <param name="account">Underlying Account instance to use.</param>
    public DebitCard(Account account)
    {
        _account = account;
    }

    /// <summary>
    /// Charge passed amount to underlying account.
    /// </summary>
    /// <param name="amount">Amount of charge.</param>
    /// <returns>Success or failure of charge.</returns>
    public bool ChargeAccount(decimal amount)
    {
        // Check if balance meets or exceeds charge amount.
        if (_account.Balance >= amount)
        {
            // Log successful charge message.
            Logging.Log($"{this.GetType().Name} charge of {amount:C2} succeeded.");
            // Modify balance.
            _account.Balance -= amount;
            // Charge was successful.
            return true;
        }

        // Log that charge failed due to insufficient funds.
        Logging.Log($"{this.GetType().Name} charge of {amount:C2} failed due to insufficient funds.  Current balance: {_account.Balance:C2}.");
        // Charge failed.
        return false;
    }
}
```

Now, to make use of the `proxy pattern` classes we start by instantiating one of our `proxy` classes.  We'll begin with the `DebitCardTest()` method:

```cs
internal static void DebitCardTest()
{
    // Create new debit card proxy instance.
    var debitCard = new DebitCard();
    // Attempt two successful charges.
    debitCard.ChargeAccount(125.50M);
    debitCard.ChargeAccount(500);
    // Attempt overcharge.
    debitCard.ChargeAccount(432.10M);
}
```

As mentioned, we first create an instance of `DebitCard()` (which creates a new, associated instance of `Account`), then we attempt three charges to the account.  Since this is the `DebitCard` `proxy` class, our expectation is that an attempt to overcharge the account will result in a failed transaction.  Sure enough, the log output shows our two successful charges followed by the third failed charge:

```
DebitCard charge of $125.50 succeeded.
Balance changed from $1,000.00 to $874.50.
DebitCard charge of $500.00 succeeded.
Balance changed from $874.50 to $374.50.
DebitCard charge of $432.10 failed due to insufficient funds.  Current balance: $374.50.
```

Now let's try the `CreditCardTest()` method, which performs the same series of charges, but should allow our final charge to succeed by performing an overcharge:

```cs
internal static void CreditCardTest()
{
    // Create new credit card proxy instance and allow overcharges.
    var creditCard = new CreditCard();
    // Attempt two successful charges.
    creditCard.ChargeAccount(125.50M);
    creditCard.ChargeAccount(500);
    // Attempt overcharge.
    creditCard.ChargeAccount(432.10M);
}
```

As expected, the output shows that all three charges succeeded, however, since the last charge resulted in an overcharge (which is explicitly allowed by the `CreditCard` `proxy` class), the final account balance is `negative $57.60`.

```
CreditCard charge of $125.50 succeeded.
Balance changed from $1,000.00 to $874.50.
CreditCard charge of $500.00 succeeded.
Balance changed from $874.50 to $374.50.
CreditCard charge of $432.10 succeeded, as overcharge.
Balance changed from $374.50 to ($57.60).
```

---

This was a tiny sample of what the `proxy design pattern` can do, but should help to give a sense of its potential.  Feel free to check out more design patterns in our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 13 of our Software Design Pattern series in which examine the proxy design pattern using fully-functional C# example code.
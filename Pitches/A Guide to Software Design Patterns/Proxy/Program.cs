using Utility;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            DebitCardTest();
            Logging.LineSeparator();
            CreditCardTest();
        }

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
    /// Proxy class for Account handles Credit transcations.
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
    /// Proxy class for Account that handles Debit transcations.
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
}

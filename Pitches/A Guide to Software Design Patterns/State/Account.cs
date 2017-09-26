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
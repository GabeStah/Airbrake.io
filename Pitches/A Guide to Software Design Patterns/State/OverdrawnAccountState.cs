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
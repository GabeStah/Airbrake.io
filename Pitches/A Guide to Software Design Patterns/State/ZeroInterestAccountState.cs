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
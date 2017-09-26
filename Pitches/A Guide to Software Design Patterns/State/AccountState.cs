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
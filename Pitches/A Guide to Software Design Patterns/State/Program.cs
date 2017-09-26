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

            // Pay interest.
            account.AccrueInterest();

            // Make a few withdrawals.
            account.Withdraw(2500);
            account.Withdraw(1500);
        }
    }
}

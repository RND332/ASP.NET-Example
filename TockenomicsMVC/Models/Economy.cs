namespace TockenomicsMVC.Models
{
    class Treasury 
    {
        public double USDBalance { get; private set; }
        public double TokenBalance { get; private set; }
        public Treasury(double usd, double token)
        {
            if (usd <= 0) throw new ArgumentException("USD balance must be greater than 0");
            if (token <= 0) throw new ArgumentException("Token balance must be greater than 0");

            USDBalance = usd;
            TokenBalance = token;
        }
        public double TokenPrice() 
        {
            return USDBalance / TokenBalance;
        }
        public double MarketCap() 
        {
            return TokenBalance * TokenPrice();
        }
        public double Sell(double amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if (amount > USDBalance) throw new ArgumentException("Amount must be less than or equal to USD Balance");

            TokenBalance += amount;                 // Selled tokens are added to the treasury tokens balance
            USDBalance -= (amount * TokenPrice());  // Calculate the amount of USD that which should be send to the user and withdraw it from the treasury
            return amount * TokenPrice();           // Return the amount of USD that should be send to the user
        }
        public double Buy(double amount) 
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if (amount > TokenBalance) throw new ArgumentException("Amount must be less than or equal to Token Balance");

            TokenBalance -= amount;                 // Bought tokens are subtracted from the treasury tokens balance
            USDBalance += (amount * TokenPrice());  // Calculate the amount of tokens that which should be send to the user and withdraw it from the treasury
            return amount * TokenPrice();           // Return the amount of tokens that should be send to the user
        } 
    }
    public class Economy
    {
        public double MaxEmmissionsPerDay { get; private set; }
        public double TotalSupply { get; private set; }
        private Treasury Treasury { get; private set; }
        public Economy(double MaxEmmissionsPerDay, double TreasuryUSDInitalBalance, double TreasuryTokenInitalBalance)
        {
            if (MaxEmmissionsPerDay <= 0)           throw new ArgumentException("Max Emmissions Per Day must be greater than 0");
            if (TreasuryUSDInitalBalance <= 0)      throw new ArgumentException("USD Balance must be greater than 0");
            if (TreasuryTokenInitalBalance <= 0)    throw new ArgumentException("Token Balance must be greater than 0");

            this.MaxEmmissionsPerDay = MaxEmmissionsPerDay;
            this.TotalSupply = TreasuryTokenInitalBalance;
            Treasury = new Treasury(TreasuryUSDInitalBalance, TreasuryTokenInitalBalance);
        }
        public double Sell(double amount)
        {
            if (amount > TotalSupply) throw new Exception("You can't sell more than total supply");

            TotalSupply -= amount;                  // Remove the amount of tokens from the total supply
            var USDAmount = Treasury.Sell(amount);  // Sell the amount of tokens and get the amount of USD that should be send to the user
            return USDAmount;                       // Return the amount of USD that should be send to the user
        }
        public double Buy(double amount)
        {
            if (amount > TotalSupply) throw new Exception("You can't buy more than total supply");
            if (amount > Treasury.TokenBalance) throw new Exception("You can't buy more than treasury balance");

            TotalSupply += amount;                  // Add the amount of tokens to the total supply
            var TokenAmount = Treasury.Buy(amount); // Buy the amount of tokens and get the amount of USD that should be send to the user
            return TokenAmount;                     // Return the amount of USD that should be send to the user
        }
        public double Emmision(double amount)
        {
            if (amount / Treasury.TokenPrice() > this.MaxEmmissionsPerDay) // Check if the amount of tokens that should be emmited is greater than the max emmissions per day
            {
                this.TotalSupply += (this.MaxEmmissionsPerDay); // If so, add the max emmissions per day to the total supply
                this.Treasury.USDBalance += amount; // Add the max emmissions per day to the treasury balance
            }
            else 
            {
                var _toBeEmmited = amount / Treasury.TokenPrice();
                this.TotalSupply += _toBeEmmited;
                this.Treasury.USDBalance += amount;
            }
        }
    }
}

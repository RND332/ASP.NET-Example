namespace TockenomicsMVC.Models.Interfaces
{
    public interface IEconomy
    {
        double MaxEmmissionsPerDay { get; }
        double TotalSupply { get; }
        Treasury Treasury { get; }
        double Sell(double amount);
        double Buy(double amount);
        double Emmision(double amount);
    }
}

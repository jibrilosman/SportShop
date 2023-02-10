using Stripe;

namespace OnlineSportShop.ViewModel
{
    public class StripeDashboardVM
    {
        public Balance Balance { get; set; }
        public List<BalanceTransaction> Transactions { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Charge> Charges { get; set; }
        public List<Dispute> Disputes { get; set; }
        public List<Refund> Refunds { get; set; }

    }
}

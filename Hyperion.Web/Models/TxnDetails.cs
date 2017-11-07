namespace Hyperion.Web.Models
{
    public class TxnDetails
    {

        public string Channel { get; set; }
        public string Payer { get; set; }
        public string Payee { get; set; }
        public string PayerName { get; set; }

        public string Amount { get; set; }
        public string MerchantId { get; set; }
        public string TxnRefnum { get; set; }
        public string Expiry { get; set; }

        public string ReturnUrl { get; set; }


    }
}
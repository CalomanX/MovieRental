namespace MovieRental.PaymentProviders
{
    public class PaymentProcessor
    {

        private Dictionary<string, IPaymentProvider> _paymentProviders = new();




        public PaymentProcessor AddPaymentProvider(string method, IPaymentProvider provider)
        {
            _paymentProviders[method] = provider;
            return this;
        }


        public async Task<bool> ProcessPayment(string paymentMethod, double price)
        {
            if (!_paymentProviders.ContainsKey(paymentMethod))
            {
                throw new ArgumentException($"Payment method {paymentMethod} is not supported.");
            }
            var provider = _paymentProviders[paymentMethod];
            return await provider.Pay(price);
        }
    }
}

namespace MovieRental.Customer
{
    public interface ICustomerFeatures
    {
        Task<Customer> Save(Customer rental);
    }
}

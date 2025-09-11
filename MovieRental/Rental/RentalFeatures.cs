using Microsoft.EntityFrameworkCore;

using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
    public class RentalFeatures : IRentalFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        private readonly PaymentProcessor _paymentprocessor;

        public RentalFeatures(MovieRentalDbContext movieRentalDb, PaymentProcessor paymentProcessor)
        {
            _movieRentalDb = movieRentalDb;
            _paymentprocessor = paymentProcessor;
        }

        /* FIX: Make this method async
         * 
         * Blocking calls can be a source of performance issues specially in heavy load APIs.
         * 
         * Why async? In (very) simple terms its like you have this fast-food restaurant with 5 frontend employees (available threads) and a lot of clients (making requests) in a line. 
         * Without async, the first client make a request and an available employee (thread from the pool) will make all the work for that client. 
         * He will wait for the cook to prepare the meal (blocking call) and then after that deliver the meal to the client. Only now he can take another request;
         * Easy to see that somewhere along the line all waiters will be full and no more clients can be served (thread starvation - all threads busy waiting).
         * 
         * With async, the first client make a request and the first employee (thread from the pool) will take the request and ask the cook to prepare the meal (non-blocking call).
         * Then will go back to the line and take another request. When the meal is ready, the cook will call the employee to deliver the meal to the client.
         * Thread starvation is now a lot less likely to happen as the waiter is not locked waiting for the cook to prepare the meal.
         * 
         * Async/await is just sugar coating for callbacks and state machines, running on old tech (TPL) but it makes life a LOT easyer.
		 */
        public async Task<Rental> Save(Rental rental)
        {
            await _movieRentalDb.Rentals.AddAsync(rental);
            await _movieRentalDb.SaveChangesAsync();
            return rental;
        }

        //TODO: finish this method and create an endpoint for it
        public async Task<IEnumerable<Rental>> GetRentalsByCustomerName(string customerName)
        {
            if (!string.IsNullOrEmpty(customerName))
            {
                return await _movieRentalDb.Rentals.Where(c => c.Customer != null && c.Customer.Name == customerName).ToListAsync();
            }
            return [];
        }

        public async Task Rent(RentalPayment rental)
        {
            try
            {
                var paymentResult = await _paymentprocessor.ProcessPayment(rental.PaymentMethod, rental.Price);
                if (!paymentResult)
                {
                   throw new PaymentException("Something wrong happened with the payment!");
                }
                await Save(rental);
            }
            catch (Exception)
            {
                // Try to recover payment
                throw;
            }

        }
    }
}

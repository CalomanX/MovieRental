using Microsoft.AspNetCore.Mvc;

using MovieRental.Customer;

using System.Runtime.CompilerServices;

namespace MovieRental.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private ICustomerFeatures _customerFeatures;

        public CustomerController(Customer.ICustomerFeatures customerFeatures)
        {
            _customerFeatures = customerFeatures;
        }

        [HttpPost]
        public async Task<ActionResult<Customer.Customer>> AddCustomer([FromBody] Customer.Customer customer)
        {
            try
            {
                var newCustomer = await _customerFeatures.Save(customer);
                return Ok(newCustomer);
            }
            catch
            {
                // LOG the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        } 
    }
}

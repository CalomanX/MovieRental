using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;
using MovieRental.PaymentProviders;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {

        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental)
        {
	        return Ok(await _features.Save(rental));
        }

        [HttpGet("{customerName}")]
        public async Task<ActionResult<IEnumerable<Rental.Rental>>> GetRentalsByCustomerName(string customerName)
        {
            try
            {
                var rentals = await _features.GetRentalsByCustomerName(customerName);
                if (!rentals.Any())
                {
                    return NotFound("No rentals found for this customer");
                }
                return Ok(rentals);
            }
            catch
            {
                // LOG the exception
                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpPost("Rent")]
        public async Task<IActionResult> Rent([FromBody] Rental.RentalPayment rental)
        {
            try
            {
                await _features.Rent(rental);
                return Ok("Rental processed successfully.");
            }
            catch (PaymentException pex )
            {
                return BadRequest(pex.Message);
            }
            catch (Exception ex)
            {
                // LOG the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }


    }
}

using Market.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CustomersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(customer).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        



    }
}

using Market.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrdersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _dbContext.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            
             // Преобразование даты в формат UTC
             order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
            

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("customer/{customerId}/total-amount")]
        public async Task<ActionResult<decimal>> GetTotalAmountByCustomerId(int customerId)
        {
            var totalAmount = await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .SumAsync(o => o.TotalAmount);

            return Ok(totalAmount);
        }


        [HttpGet("ordersname")]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            var ordersWithCustomerNames = _dbContext.Orders
                .Select(o => new
                {
                    OrderId = o.Id,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.Name
                })
                .ToList();

            return Ok(ordersWithCustomerNames);



        }
    }
}

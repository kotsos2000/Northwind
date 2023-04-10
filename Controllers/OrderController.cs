using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Classes;

namespace Northwind.Controllers
{
    [ApiController]
    [Route("[controller]")]

    // initialize DbContext and Mapper
    public class OrderController : ControllerBase
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;


        public OrderController(NorthwindContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("/orders/orderid/{id}")] // GET A Single Order using the KEY id
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var result = await _context.Orders.FindAsync(id);

            if (result is not null)
            {
                return Ok(_mapper.Map<OrderDTO>(result));
            }
            else 
            {
                return NotFound($"Order with id {id} not found."); 
            }
        }

        [HttpGet("/orders/getByCustomer/{customerId}")] // GET Order(s) by CustomerID
        public async Task<ActionResult<List<OrderDTO>>> GetOrderByCustomer(string customerId)
        {
            if (customerId.Length != 5)
            {
                return BadRequest("CustomerID should have a length of 5.");
            }
            var result = await _context.Orders.Where( o=> o.CustomerId == customerId).ToListAsync();
            var DTOresult = result.Select(o => _mapper.Map<OrderDTO>(o)).ToList();

            if (result.Count>0)
            {
                return Ok(DTOresult);
            }
            else
            {
                return NotFound($"Order with customerID {customerId} not found.");
            }
        }

        [HttpGet("/orders/getByCustomerAndEmployee/{customerId}/{employeeId}")] // GET Order(s) by CustomerID
        public async Task<ActionResult<List<OrderDTO>>> GetOrderByCustomer(string customerId,int employeeId)
        {
            if (customerId.Length != 5)
            {
                return BadRequest("CustomerID should have a length of 5.");
            }

            var result = await _context.Orders.Where(o => o.CustomerId == customerId && o.EmployeeId == employeeId ).ToListAsync();
            var DTOresult = result.Select( o=> _mapper.Map<OrderDTO>(o) ).ToList();

            if (DTOresult.Count > 0)
            {
                return Ok(DTOresult);
            }
            else
            {
                return NotFound($"Order with customerID {customerId} and employeeID {employeeId} not found.");
            }
        }

        [HttpGet("/orders")] // GET All Orders
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            return await _context.Orders.Take(100).Select(c=> _mapper.Map<OrderDTO>(c)).ToListAsync();
        }

        [HttpPost("/orders")] // POST Order
        public async Task<ActionResult<OrderDTO>> PostCustomer(OrderDTO ord)
        {
            // wrong input will be handled by the Database restraints
            _context.Orders.Add(_mapper.Map<Order>(ord));
            await _context.SaveChangesAsync();
            return Ok(ord);
        }


        [HttpDelete("/orders/orderid/{id}")] // DELETE A Single Order using KEY id
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await _context.Orders.FindAsync(id);

            if (result is not null)
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound($"Order with id {id} not found.");
            }
        }

    }

}
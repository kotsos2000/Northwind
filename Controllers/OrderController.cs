using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Controllers
{
    [ApiController]
    // initialize DbContext and Mapper
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;

        private readonly OrderInterface orderInterface;

        public OrdersController(NorthwindContext context,IMapper mapper, OrderInterface orderInterface)
        {
            _context = context;
            _mapper = mapper;
            this.orderInterface = orderInterface;
        }

        [HttpGet("/api/orders/orderid/{id}")] // GET A Single Order using the KEY id
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var result = await orderInterface.GetOrder(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("/api/orders/getByCustomer/{customerId}")] // GET Order(s) by CustomerID
        public async Task<ActionResult<List<OrderDTO>>> GetOrderByCustomer(string customerId)
        {
            var result = await orderInterface.GetOrderByCustomer(customerId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("/api/orders/getByCustomerAndEmployee/{customerId}/{employeeId}")] // GET Order(s) by CustomerID
        public async Task<ActionResult<List<OrderDTO>>> GetOrderByCustomerAndEmployee(string customerId,int employeeId)
        {
            var result = await orderInterface.GetOrderByCustomerAndEmployee(customerId,employeeId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("/api/orders")] // GET All Orders
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var result = await orderInterface.GetOrders();
            return result.Data;  
        }

        [HttpPost("/api/orders")] // POST Order
        public async Task<ActionResult<Order>> PostCustomer(OrderDTO ord)
        {
            try
            {
                var result = await orderInterface.PostCustomer(ord);
                return Ok(result);
            }
            catch
            {
                var response = new ServiceResponse<OrderDTO>();
                response.Message = "Failed to create order";
                response.Success = false;
                return BadRequest(response);
            }
        }


        [HttpDelete("/api/orders/orderid/{id}")] // DELETE A Single Order using KEY id
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await orderInterface.DeleteOrder(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
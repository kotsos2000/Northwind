using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Services.OrderService
{
    public class OrderService : OrderInterface
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;

        public OrderService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<Order>> DeleteOrder(int id)
        {
            var response = new ServiceResponse<Order>();

            var result = await _context.Orders.FindAsync(id);

            if (result is not null)
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
                response.Data = result;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Order with id {id} not found.";
                return response;
            }
        }

        public async Task<ServiceResponse<OrderDTO>> GetOrder(int id)
        {
            var response = new ServiceResponse<OrderDTO>();
            var result = await _context.Orders.FindAsync(id);

            if (result is not null)
            {
                response.Data = _mapper.Map<OrderDTO>(result);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Order with id {id} not found.";
                return response;
            }
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetOrderByCustomer(string customerId)
        {
            var response = new ServiceResponse<List<OrderDTO>>();

            if (customerId.Length != 5)
            {
                response.Success = false;
                response.Message = "CustomerID should have a length of 5.";
                return response;
            }
            var result = await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
            var DTOresult = result.Select(o => _mapper.Map<OrderDTO>(o)).ToList();

            if (result.Count > 0)
            {
                response.Data = DTOresult;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Order with customerID {customerId} not found.";
                return response;
            }
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetOrderByCustomerAndEmployee(string customerId, int employeeId)
        {
            var response = new ServiceResponse<List<OrderDTO>>();
            if (customerId.Length != 5)
            {
                response.Success = false;
                response.Message = "CustomerID should have a length of 5.";
                return response;
            }

            var result = await _context.Orders.Where(o => o.CustomerId == customerId && o.EmployeeId == employeeId).ToListAsync();
            var DTOresult = result.Select(o => _mapper.Map<OrderDTO>(o)).ToList();

            if (DTOresult.Count > 0)
            {
                response.Data = DTOresult;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Order with customerID {customerId} and employeeID {employeeId} not found.";
                return response;
            }
        }

        public async Task<ServiceResponse<List<Order>>> GetOrders()
        {
            var response = new ServiceResponse<List<Order>>();
            response.Data = await _context.Orders.Take(100).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<Order>> PostCustomer(OrderDTO ord)
        {
            var response = new ServiceResponse<Order>();
            _context.Orders.Add(ord);
            await _context.SaveChangesAsync();
            response.Data = await _context.Orders.LastAsync();
            return response;
        }
    }
}

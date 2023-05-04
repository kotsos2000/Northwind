using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Northwind.Services.OrderService
{
    public interface OrderInterface
    {
        Task<ServiceResponse<OrderDTO>> GetOrder(int id);
        Task<ServiceResponse<List<OrderDTO>>> GetOrderByCustomer(string customerId);
        Task<ServiceResponse<List<OrderDTO>>> GetOrderByCustomerAndEmployee(string customerId, int employeeId);
        Task<ServiceResponse<List<Order>>> GetOrders();
        Task<ServiceResponse<Order>> PostCustomer(OrderDTO ord);
        Task<ServiceResponse<Order>> DeleteOrder(int id);
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading.Tasks; 

namespace Northwind.Services.NorthwindService
{
    public interface CustomerInterface
    {
        Task<ServiceResponse<CustomerDTO>> GetCustomer(string id);

        Task<ServiceResponse<List<CustomerDTO>>> GetCustomers();

        Task<ServiceResponse<List<CustomerDTO>>> GetCustomersByCompany(string companyName);

        Task<ServiceResponse<CustomerDTO>> PostCustomer(CustomerDTO cust);

        Task<ServiceResponse<CustomerDTO>> PutCustomer(CustomerDTO cust);

        Task<ServiceResponse<CustomerDTO>> DeleteCustomer(string id);
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.NorthwindService;

namespace Northwind.Controllers
{
    [ApiController]
    // initialize DbContext and Mapper
    public class CustomersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;

        private readonly CustomerInterface customerInterface;


        public CustomersController(NorthwindContext context, IMapper mapper, CustomerInterface customerInterface)
        {
            _context = context;
            _mapper = mapper;
            this.customerInterface = customerInterface;
        }


        [HttpGet("/api/customers/customerid/{id}")] // GET A Single Customer using KEY id
        public async Task<ActionResult<CustomerDTO>> GetCustomer(string id)
        {
            var response = await customerInterface.GetCustomer(id);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);

        }

        [HttpGet("/api/customers")] // GET All Customers
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomers()
        {
            var response = await customerInterface.GetCustomers();
            return Ok(response.Data);
        }

        [HttpGet("/api/customers/companyName/{companyName}")] // GET Customer(s) based on company name
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomersByCompany(string companyName)
        {
            var result = await customerInterface.GetCustomersByCompany(companyName);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpPost("/api/customers")] // POST Customer

        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO cust)
        {
            var result = await customerInterface.PostCustomer(cust);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPut("/api/customers")] // PUT Customer
        public async Task<ActionResult<CustomerDTO>> PutCustomer(CustomerDTO cust)
        {
            var result = await customerInterface.PutCustomer(cust);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }

        }

        [HttpDelete("/api/customers/customerid/{id}")] // DELETE A Single Customer using KEY id
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var result = await customerInterface.DeleteCustomer(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

    }

}
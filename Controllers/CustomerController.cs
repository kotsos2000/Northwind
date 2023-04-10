using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Classes;

namespace Northwind.Controllers
{
    [ApiController]
    [Route("[controller]")]

    // initialize DbContext and Mapper
    public class CustomerController : ControllerBase
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;


        public CustomerController(NorthwindContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("/customers/customerid/{id}")] // GET A Single Customer using KEY id
        public async Task<ActionResult<CustomerDTO>> GetCustomer(string id)
        {
            if (id.Length != 5)
            {
                return BadRequest("ID should have a length of 5.");
            }
            var result = await _context.Customers.FindAsync(id);

            if (result is not null)
            {
                return Ok(_mapper.Map<CustomerDTO>(result));
            }
            else 
            {
                return NotFound($"Customer with id {id} not found."); 
            }
        }

        [HttpGet("/customers")] // GET All Customers
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomers()
        {
            return await _context.Customers.Select( c => _mapper.Map<CustomerDTO>(c)).ToListAsync();
        }

        [HttpGet("/customers/companyName/{companyName}")] // GET Customer(s) based on company name
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomersByCompany(string companyName)
        {
            var result = await _context.Customers.Where(c=> c.CompanyName.Contains(companyName)).ToListAsync();
            var DTOresult = result.Select(c => _mapper.Map<CustomerDTO>(c)).ToList();

            if (DTOresult.Count>0)
            {
                return Ok(DTOresult);
            }
            else
            {
                return NotFound($"Customers that work at company {companyName} not found.");
            }
        }

        [HttpPost("/customers")] // POST Customer

        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO cust)
        {
            if (cust.CustomerId.Length !=5)
            {
                return BadRequest("ID should have a length of 5.");
            }
            // wrong input will be handled by the Database restraints
            _context.Customers.Add(_mapper.Map<Customer>(cust));
            await _context.SaveChangesAsync();
            return Ok(cust);
        }

        [HttpPut("/customers")] // PUT Customer
        public async Task<ActionResult<CustomerDTO>> PutCustomer(CustomerDTO cust)
        {
            if (cust.CustomerId.Length != 5)
            {
                return BadRequest("ID should have a length of 5.");
            }

            var result = await _context.Customers.FindAsync(cust.CustomerId);
            if (result != null)
            {
                result.CompanyName = cust.CompanyName;
                result.ContactName = cust.ContactName;
                result.ContactTitle = cust.ContactTitle;
                result.Address = cust.Address;
                result.City = cust.City;
                result.Region = cust.Region;
                result.PostalCode = cust.PostalCode;
                result.Country = cust.Country;
                result.Phone = cust.Phone;
                result.Fax = cust.Fax;

                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<CustomerDTO>(result));
            }
            else
            {
                return NotFound($"Customer with id {cust.CustomerId} not found.");
            }
        }

        [HttpDelete("/customers/customerid/{id}")] // DELETE A Single Customer using KEY id
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            if (id.Length != 5)
            {
                return BadRequest("ID should have a length of 5.");
            }

            var result = await _context.Customers.FindAsync(id);

            if (result is not null)
            {
                _context.Customers.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound($"Customer with id {id} not found.");
            }
        }

    }

}
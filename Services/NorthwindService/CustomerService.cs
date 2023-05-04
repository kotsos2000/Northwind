using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;

namespace Northwind.Services.NorthwindService
{
    public class CustomerService : CustomerInterface
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;


        public CustomerService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CustomerDTO>> DeleteCustomer(string id)
        {
            var response = new ServiceResponse<CustomerDTO>();

            if (id.Length != 5)
            {
                response.Success = false;
                response.Message = "ID should have a length of 5.";
                return response;
            }

            var result = await _context.Customers.FindAsync(id);

            if (result is not null)
            {
                _context.Customers.Remove(result);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<CustomerDTO>(result);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Customer with id {id} not found.";
                return response;
            }
        }

        public async Task<ServiceResponse<CustomerDTO>> GetCustomer(string id)
        {
            var response = new ServiceResponse<CustomerDTO>();

            if (id.Length != 5)
            {
                response.Success = false;
                response.Message = "ID should have a length of 5";
                return response;
            }

            var result = await _context.Customers.FindAsync(id);

            if (result is not null)
            {
                response.Data = _mapper.Map<CustomerDTO>(result);
            }
            else
            {
                response.Success = false;
                response.Message = $"Customer with ID {id} not found";
            }
            return response;

        }

        public async Task<ServiceResponse<List<CustomerDTO>>> GetCustomers()
        {
            var response = new ServiceResponse<List<CustomerDTO>>();

            var result = await _context.Customers.Select(c => _mapper.Map<CustomerDTO>(c)).ToListAsync();

            response.Data = result;
            return response;
        }

        public async Task<ServiceResponse<List<CustomerDTO>>> GetCustomersByCompany(string companyName)
        {
            var response = new ServiceResponse<List<CustomerDTO>>();

            var result = await _context.Customers.Where(c => c.CompanyName.Contains(companyName)).ToListAsync();
            var DTOresult = result.Select(c => _mapper.Map<CustomerDTO>(c)).ToList();

            if (DTOresult.Count > 0)
            {
                response.Data = DTOresult;
            }
            else
            {
                response.Success = false;
                response.Message = $"Customers that work at company {companyName} not found.";
            }
            return response;

        }

        public async Task<ServiceResponse<CustomerDTO>> PostCustomer(CustomerDTO cust)
        {
            var response = new ServiceResponse<CustomerDTO>();

            if (cust.CustomerId.Length != 5)
            {
                response.Success = false;
                response.Message = "ID should have a length of 5.";
                return response;
            }
            // wrong input will be handled by the Database restraints
            _context.Customers.Add(_mapper.Map<Customer>(cust));
            response.Data = cust;
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<CustomerDTO>> PutCustomer(CustomerDTO cust)
        {
            var response = new ServiceResponse<CustomerDTO>();

            if (cust.CustomerId.Length != 5)
            {
                response.Success = false;
                response.Message = "ID should have a length of 5.";
                return response;
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
                response.Data = _mapper.Map<CustomerDTO>(result);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = $"Customer with id {cust.CustomerId} not found.";
                return response;
            }
        }
    }
}

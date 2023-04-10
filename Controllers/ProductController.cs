using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Classes;

namespace Northwind.Controllers
{
    [ApiController]
    [Route("[controller]")]

    // initialize DbContext and Mapper
    public class ProductController : ControllerBase
    {
        private readonly NorthwindContext _context;

        private readonly IMapper _mapper;

        public ProductController(NorthwindContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("/products/productid/{id}")] // GET A Single Product using KEY id
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var result = await _context.Products.FindAsync(id);

            if (result is not null)
            {
                return Ok(_mapper.Map<ProductDTO>(result));
            }
            else 
            {
                return NotFound($"Product with id {id} not found."); 
            }
        }

        [HttpGet("/products")] // GET All Products
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            return await _context.Products.Select(p => _mapper.Map<ProductDTO>(p)).ToListAsync();
        }

        [HttpPost("/products")] // POST Product

        public async Task<ActionResult<ProductDTO>> PostCustomer(ProductDTO prod)
        {
            // wrong input will be handled by the Database restraints
            _context.Products.Add(_mapper.Map<Product>(prod));
            await _context.SaveChangesAsync();
            return Ok(prod);
        }

        [HttpPut("/products")] // PUT Product using the PUT DTO that has the ProductId field
        public async Task<ActionResult<ProductDTO>> PutProduct(ProductPUTDTO prod)
        {

            var result = await _context.Products.FindAsync(prod.ProductId);
            if (result != null)
            {
                result.ProductName = prod.ProductName;
                result.SupplierId = prod.SupplierId;
                result.CategoryId = prod.CategoryId;
                result.QuantityPerUnit = prod.QuantityPerUnit;
                result.UnitPrice = prod.UnitPrice;
                result.UnitsInStock = prod.UnitsInStock;
                result.UnitsOnOrder = prod.UnitsOnOrder;    
                result.ReorderLevel = prod.ReorderLevel;
                result.Discontinued = prod.Discontinued;

                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<ProductDTO>(result));
            }
            else
            {
                return NotFound($"Product with id {prod.ProductId} not found.");
            }
        }

        [HttpDelete("/products/productid/{id}")] // DELETE A Single Product using KEY id
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _context.Products.FindAsync(id);

            if (result is not null)
            {
                _context.Products.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound($"Product with id {id} not found.");
            }
        }

    }

}
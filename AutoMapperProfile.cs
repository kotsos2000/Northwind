using AutoMapper;

namespace Northwind
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<Product,ProductDTO>();
            CreateMap<ProductDTO, Product>();
            
        }
    }
}

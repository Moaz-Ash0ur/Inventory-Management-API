using AutoMapper;
using Inventory.BLL.DTOs;
using Inventory.BLL.DTOs.User;
using Inventory.DAL.Models;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domains;

namespace Inventory.BLL.ProfileMapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
         
            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Product
            CreateMap<Product, ProductDto>().ReverseMap();

            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // Supplier
            CreateMap<Supplier, SupplierDto>().ReverseMap();

            // Purchase
            CreateMap<Purchase, PurchaseDto>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.PurchaseItems))
           .ReverseMap();

            // Sale
            CreateMap<Sale, SaleDto>()
           .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems))
           .ReverseMap();

            // PurchaseItem
            CreateMap<PurchaseItem, PurchaseItemDto>().ReverseMap();

      

            // SaleItem
            CreateMap<SaleItem, SaleItemDto>().ReverseMap();

            // StockTransaction
            CreateMap<StockTransaction, StockTransactionDto>().ReverseMap();

            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>()
           .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
           .ReverseMap()
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));



        }

    }
}

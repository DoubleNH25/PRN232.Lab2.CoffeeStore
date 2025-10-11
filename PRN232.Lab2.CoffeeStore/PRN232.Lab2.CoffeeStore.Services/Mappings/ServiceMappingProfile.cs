using AutoMapper;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Services.Models.Business;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        // Entity -> Business
        CreateMap<Category, CategoryModel>();
        CreateMap<Product, ProductModel>();
        CreateMap<Order, OrderModel>();
        CreateMap<OrderDetail, OrderDetailModel>();
        CreateMap<Payment, PaymentModel>();

        // Business -> Response
        CreateMap<CategoryModel, CategoryResponseModel>();
        CreateMap<ProductModel, ProductResponseModel>();
        CreateMap<OrderModel, OrderResponseModel>();
        CreateMap<OrderDetailModel, OrderDetailResponseModel>();
        CreateMap<PaymentModel, PaymentResponseModel>();

        // Request -> Business
        CreateMap<CategoryCreateRequestModel, CategoryModel>();
        CreateMap<CategoryUpdateRequestModel, CategoryModel>();
        CreateMap<ProductCreateRequestModel, ProductModel>();
        CreateMap<ProductUpdateRequestModel, ProductModel>();
        CreateMap<OrderCreateRequestModel, OrderModel>();
        CreateMap<OrderUpdateRequestModel, OrderModel>();
        CreateMap<OrderDetailCreateRequestModel, OrderDetailModel>();
        CreateMap<OrderDetailUpdateRequestModel, OrderDetailModel>();
        CreateMap<PaymentCreateRequestModel, PaymentModel>();
        CreateMap<PaymentUpdateRequestModel, PaymentModel>();

        // Business -> Entity
        CreateMap<CategoryModel, Category>();
        CreateMap<ProductModel, Product>();
        CreateMap<OrderModel, Order>();
        CreateMap<OrderDetailModel, OrderDetail>();
        CreateMap<PaymentModel, Payment>();
    }
}

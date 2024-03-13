using AutoMapper;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.ViewModel;
using ThucTapProject.Helper;
using System.Runtime.Intrinsics.Arm;
using ThucTapProject.ViewModel.GetAllOrderModel;
using System.Security.Principal;

namespace ThucTapProject.Services
{
    public class AutoMapperProfiles
    {
        public static Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserEditModel, User>();
                cfg.CreateMap<UserInformation, User>()
                    .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => CommonFunctions.NameFormat(src.UserName)));
                cfg.CreateMap<UserInformation, Accountt>()
                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName.Trim()))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.DecentralizationId, opt => opt.MapFrom(src => (int)Roles.clien))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)Account_status.notValidated))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.EnhancedHashPassword(src.Password, 13)));
                cfg.CreateMap<Accountt, AccountView>();
                cfg.CreateMap<Decentralization, DecentralizationView>();
                cfg.CreateMap<ProductTypeEditModel, ProductType>();
                cfg.CreateMap<ProductType, ProductTypeView>();
                cfg.CreateMap<ProductEditModel, Product>();
                cfg.CreateMap<Product, ProductView>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Product_status)src.Status))
                    .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src => src.CalDiscountPrice()));
                cfg.CreateMap<CartItemEditModel, CartItem>();
                cfg.CreateMap<CartItem, CartItemView>()
                    .ForMember(dest => dest.NameProduct, opt => opt.MapFrom(src => src.Product.NameProduct))
                    .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Product.Price))
                    .ForMember(dest => dest.discount, opt => opt.MapFrom(src => (double)src.Product.Discount))
                    .ForMember(dest => dest.TotalDiscountPrice, opt => opt.MapFrom(src => src.Product.CalDiscountPrice()*src.Quantity));
                cfg.CreateMap<OrderEditModel, Order>();
                cfg.CreateMap<CartItem, OrderDetail>();
                cfg.CreateMap<OrderDetail, OrderDetailsView>()
                    .ForMember(dest => dest.NameProduct, opt => opt.MapFrom(src => src.Product.NameProduct))
                    .ForMember(dest => dest.AvatarImageProduct, opt => opt.MapFrom(src => src.Product.AvatarImageProduct));
                cfg.CreateMap<Order, OrderInforView>();
                cfg.CreateMap<Order, OrderInforStatus>()
                    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(scr => scr.ActualPrice))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(scr => scr.OrderStatus.StatusName));
                cfg.CreateMap<ProductReviewEditModel,ProductReview>();
                cfg.CreateMap<ProductReview, ProductReviewView>()
                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(scr => scr.User.Accountt.AccountName))
                    .ForMember(dest => dest.Avatar, opt => opt.MapFrom(scr => scr.User.Accountt.Avatar));
            });

            return new Mapper(config);
        }
    }
}

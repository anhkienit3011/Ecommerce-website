using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThucTapProject.DAO;
using ThucTapProject.EditModel;
using ThucTapProject.Entities;
using ThucTapProject.Helper;
using ThucTapProject.IServices;
using ThucTapProject.Page;
using ThucTapProject.ViewModel;
using ThucTapProject.ViewModel.response;

namespace ThucTapProject.Services {
    public class ProductService : IProductService {
        private readonly AppDbContext _appContext;
        private Mapper _mapper;

        public ProductService() {
            _appContext = new AppDbContext();
            _mapper = AutoMapperProfiles.InitializeAutoMapper();
        }
        public async Task<ApiResponse> Create(ProductEditModel NewProductED) {
            Product NewProduct = _mapper.Map<Product>(NewProductED);
            NewProduct.NameProduct = CommonFunctions.NameFormat(NewProduct.NameProduct);
            NewProduct.NumberOfViews = 0;
            NewProduct.CreateAt = DateTime.Now;
            NewProduct.Status = (int)Product_status.InStock;

            if (NewProductED.AvatarImageProductFile != null) {
                NewProduct.AvatarImageProduct = await CloudinaryService.uploadImage(NewProductED.AvatarImageProductFile);
            }
            _appContext.Add(NewProduct);
            _appContext.SaveChanges();

            return new ApiResponse {
                success = true,
                message = "Thêm sản phẩm thành công",
                data = _mapper.Map<ProductView>(NewProduct)
            };
        }

        public ApiResponse Get(int ProductId) {
            Product? product = _appContext.Product.FirstOrDefault(x => x.ProductId == ProductId);
            if (product != null) {
                product.NumberOfViews += 1;
                _appContext.SaveChanges();
                return new ApiResponse { success = true, data = _mapper.Map<ProductView>(product) };
            }
            return new ApiResponse { success = true, message = "Sản phẩm không tồn tại" };
        }

        public async Task<ApiResponse> GetAll(Pagination pagination) {
            IEnumerable<ProductView> data = _appContext.Product
                .Select(c => _mapper.Map<ProductView>(c))
                .AsEnumerable();
            var paginateData = pageResult<ProductView>.ToPageResult(pagination, data);

            return new ApiResponse { success = true, data = new pageResult<ProductView>(pagination, paginateData) };
        }

        public async Task<ApiResponse> Modify(int productId, ProductEditModel NewProductED) {
            Product? productToBeModify = _appContext.Product.FirstOrDefault(c => c.ProductId == productId);
            if (productToBeModify != null) {

                if (NewProductED.AvatarImageProductFile != null) {
                    if (productToBeModify.AvatarImageProduct != null) {
                        await CloudinaryService.deleteImage(productToBeModify.AvatarImageProduct);
                    }
                    productToBeModify.AvatarImageProduct = await CloudinaryService.uploadImage(NewProductED.AvatarImageProductFile);
                }
                // đã kiểm tra loại sản phẩm ở ModelState
                /*if(_appContext.ProductType.Any(c => c.ProductTypeId == NewProductED.ProductTypeId)) {
                    productToBeModify.NameProduct = NewProductED.NameProduct;
                }*/
                productToBeModify.NameProduct = NewProductED.NameProduct;
                productToBeModify.Discount = NewProductED.Discount;
                productToBeModify.UpdateAt = DateTime.Now;
                productToBeModify.Price = NewProductED.Price;
                productToBeModify.Title = NewProductED.Title;
                productToBeModify.ProductTypeId = NewProductED.ProductTypeId;
                _appContext.Update(productToBeModify);
                _appContext.SaveChanges();

                return new ApiResponse { success = true, message = "Sửa sản phẩm thành công" };
            }
            return new ApiResponse { success = false, message = "Sản phẩm không tồn tại" };
        }

        public async Task<ApiResponse> Remove(int productId) {
            Product? productToBeRemove = _appContext.Product.FirstOrDefault(c => c.ProductId == productId);
            if (productToBeRemove != null) {
                _appContext.ProductReview.Where(c => c.ProductId == productId).ExecuteDelete();
                _appContext.OrderDetail.Where(c => c.ProductId == productId).ExecuteDelete();
                _appContext.CartItem.Where(c => c.ProductId == productId).ExecuteDelete();

                if (productToBeRemove.AvatarImageProduct != null) {
                    await CloudinaryService.deleteImage(productToBeRemove.AvatarImageProduct);
                }
                _appContext.Remove(productToBeRemove);
                _appContext.SaveChanges();
                return new ApiResponse { success = true, message = "Xóa sản phẩm thành công" };
            }
            return new ApiResponse { success = false, message = "Sản phẩm không tồn tại" };
        }

        public async Task<ApiResponse> Search(string SearchKey, Pagination pagination, double? from, double? to, string sortBy) {
            #region searh by key


            SearchKey = SearchKey.Trim().ToLower();
            string[] KeyList = SearchKey.Split();

            IQueryable<ProductView> ProductList = _appContext.Product.ToList()
            .Where(c => c.NameProduct.ToLower().Contains(SearchKey)
                    || KeyList.Any(b => c.NameProduct.ToLower().Contains(b)))
            .Select(d => _mapper.Map<ProductView>(d)).AsQueryable();
            #endregion

            #region filter by price
            if (from.HasValue) {
                ProductList = ProductList.Where(c => c.Price >= from);
            }
            if (to.HasValue) {
                ProductList = ProductList.Where(c => c.Price <= to);
            }
            #endregion

            #region sort
            ProductList = ProductList.OrderByDescending(c => c.Status);
            switch (sortBy) {
                case "price_asc":
                    ProductList = ProductList.OrderBy(c => c.CalDiscountPrice());
                    break;
                case "price_desc":
                    ProductList = ProductList.OrderByDescending(c => c.CalDiscountPrice());
                    break;
                case "discount_asc":
                    ProductList = ProductList.OrderBy(c => c.Discount);
                    break;
                case "discount_desc":
                    ProductList = ProductList.OrderByDescending(c => c.Discount);
                    break;
                case "View_asc":
                    ProductList = ProductList.OrderBy(c => c.NumberOfViews);
                    break;
                case "View_desc":
                    ProductList = ProductList.OrderByDescending(c => c.NumberOfViews);
                    break;
                case "name_asc":
                    ProductList = ProductList.OrderBy(c => c.NameProduct);
                    break;
                case "name_desc":
                    ProductList = ProductList.OrderByDescending(c => c.NameProduct);
                    break;
                default:
                    ProductList = ProductList.OrderBy(c => c.CalDiscountPrice());
                    break;
            }

            #endregion
            var paginatedData = pageResult<ProductView>.ToPageResult(pagination, ProductList);
            await TangLuotXem(paginatedData);
            return new ApiResponse { success = true, data = new pageResult<ProductView>(pagination, paginatedData) };
        }
        public async Task<ApiResponse> GetFavoritProducts(Pagination pagination) {
            // lấy giá trị tổng lượt xem trung bình của mỗi loại sản phẩm
            Dictionary<int, double> ProductTypeAverageViews = _appContext.Product
                .GroupBy(c => c.ProductTypeId)
                .Select(g => new {
                            productTypeId = g.Key,
                            averageView = g.Average(c => c.NumberOfViews)
            }).ToDictionary(gg => gg.productTypeId, gg => gg.averageView) ;

            // lấy tối đa 10 sản phẩm của mỗi loại mà lượt view trung bình
            // của sp đó lớn hoặc bằng so với lượt view trung bình của cùng loại đó
            var Products = new List<ProductView>();
            foreach (var productTypeGroup in _appContext.Product.GroupBy(c => c.ProductTypeId).ToList()) {
                // lấy 10 sản phẩm của từng loại sp
                var productsOfType = productTypeGroup.OrderByDescending(c => c.NumberOfViews)
                    .Where(c => Convert.ToDouble(c.NumberOfViews) >= ProductTypeAverageViews[c.ProductTypeId])
                    .Take(3)
                    .Select(c => _mapper.Map<ProductView>(c));
                Products.AddRange(productsOfType);
            }
            var paginatedData = pageResult<ProductView>.ToPageResult(pagination, Products);
            return new ApiResponse { success = true, data = new pageResult<ProductView>(pagination, paginatedData) };
        }

        public async Task<ApiResponse> GetRelatedProducts(int ProductId, Pagination pagination) {
            Product? product = _appContext.Product.FirstOrDefault(c => c.ProductId == ProductId);
            if (product != null) {
                int ProductTypeId = product.ProductTypeId;
                var RelatedProduct = _appContext.Product
                    .Where(c => c.ProductTypeId == ProductTypeId && c.ProductId != ProductId)
                    .Select(c => _mapper.Map<ProductView>(c))
                    .AsEnumerable();

                var paginatedData = pageResult<ProductView>.ToPageResult(pagination, RelatedProduct);
                return new ApiResponse { success = true, data = new pageResult<ProductView>(pagination, paginatedData) };
            }
            return new ApiResponse {success = false, message= "Sản phẩm không tồn tại" };
        }

        private async Task TangLuotXem(IEnumerable<ProductView> Products) {
            foreach (var productView in Products) {
                await _appContext.Product.Where(c => c.ProductId == productView.ProductId)
                    .ExecuteUpdateAsync(opt => opt.SetProperty(c => c.NumberOfViews, c => c.NumberOfViews + 1));
            }
        }
    }
}

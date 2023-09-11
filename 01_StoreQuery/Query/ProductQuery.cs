using _0_FrameWork.Application;
using _01_StoreQuery.Contracts.Product;
using _01_StoreQuery.Contracts.ProductPicture;
using DiscountManagement.InfraStructure.EfCore;
using InventoryManagement.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.InfraStructure.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_StoreQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;

        public ProductQuery(ShopContext shopContext,
            InventoryContext inventoryContext,
            DiscountContext discountContext)
        {
            _shopContext = shopContext;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }



        public ProductQueryModel GetDetails(string slug)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice,x.IsInStock }).ToList();

            var discount = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
               .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

            var product = _shopContext.Products.Include(x => x.Category)
                .OrderByDescending(x => x.Id)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Category = x.Category.Name,
                    Slug = x.Slug,
                    CategorySlug=x.Category.Slug,
                    Code = x.Code,
                    Pictures = MapProductPictures(x.ProductPictures),
                    Keywords=x.KeyWords
                }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);
            if (product==null)
            {
                return new ProductQueryModel();
            }

            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                product.IsInStock =productInventory.IsInStock;
                var productDiscount = discount.FirstOrDefault(x => x.ProductId == product.Id);
                if (productDiscount != null)
                {
                    int disRate = productDiscount.DiscountRate;
                    product.DiscountRate = disRate;
                    product.HasDiscount = disRate > 0;
                    var discountAmount = Math.Round((price * disRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    product.DiscountExpireDate = productDiscount.EndDate.ToDiscountFormat();

                }
            }

            return product;
        }
        public static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> Pictures)
        {
            return Pictures.Select(x => new ProductPictureQueryModel 
            { 
                IsRemoved=x.IsRemoved,
                Picture=x.Picture,
                PictureAlt=x.PictureAlt,
                PictureTitle=x.PictureTitle,
                ProductId=x.ProductId

            }).Where(x => !x.IsRemoved).ToList();
        }
        public List<ProductQueryModel> GetLatestArrivals()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discount = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
               .Select(x => new { x.DiscountRate, x.ProductId }).ToList();

            var products = _shopContext.Products.Include(x=>x.Category)
                .OrderByDescending(x=>x.Id)
                .Take(6)
                .Select(x=>new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Category=x.Category.Name,
                Slug = x.Slug,
            }).AsNoTracking().ToList();
            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory!=null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var productDiscount = discount.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productDiscount != null)
                    {
                        int disRate = productDiscount.DiscountRate;
                        product.DiscountRate = disRate;
                        product.HasDiscount = disRate > 0;
                        var discountAmount = Math.Round((price * disRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();

                    }
                }               
            }
            return products;
        }

        public List<ProductQueryModel> Search(string value)
        {
            var inventory = _inventoryContext.Inventory.Select(x =>
                new { x.ProductId, x.UnitPrice }).ToList();
            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

            var query = _shopContext.Products
                .Include(x => x.Category)
                .Select(product => new ProductQueryModel
                {
                    Id = product.Id,
                    Category = product.Category.Name,
                    CategorySlug = product.Category.Slug,
                    Name = product.Name,
                    Picture = product.Picture,
                    PictureAlt = product.PictureAlt,
                    PictureTitle = product.PictureTitle,
                    Description = product.Description,
                    Slug = product.Slug
                }).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(value))
                query = query.Where(x => x.Name.Contains(value) || x.Description.Contains(value));

            var products = query.OrderByDescending(x => x.Id).ToList();
            ;

            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount == null) continue;

                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round((price * discountRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }

            return products;
        }
    }
}

using _01_StoreQuery.Contracts.ProductCategory;
using ShopManagement.InfraStructure.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_StoreQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _Context;

        public ProductCategoryQuery(ShopContext context)
        {
            _Context = context;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _Context.ProductCategories
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                })
                .ToList();
        }
    }
}

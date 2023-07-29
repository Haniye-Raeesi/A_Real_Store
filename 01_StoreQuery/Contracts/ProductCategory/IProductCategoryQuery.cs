using System.Collections.Generic;

namespace _01_StoreQuery.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        public List<ProductCategoryQueryModel> GetProductCategories();
    }
}

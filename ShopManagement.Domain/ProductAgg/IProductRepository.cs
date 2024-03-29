﻿using _0_FrameWork.Domain;
using ShopManagement.Application.Contracts.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Domain.ProductAgg
{
   public interface IProductRepository: IRepository<long,Product>
    {
        List<ProductViewModel> Search(ProductSearchModel SearchModel);
        EditProduct GetDetails(long Id);
        Product GetProductWithCategory(long Id);
        List<ProductViewModel> GetProducts();
    }
}

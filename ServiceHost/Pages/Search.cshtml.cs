using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_StoreQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        public List<ProductQueryModel> products;
        public string Value;

        public SearchModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string value)
        {
             products= _productQuery.Search(value);
            Value = value;
        }
    }
}

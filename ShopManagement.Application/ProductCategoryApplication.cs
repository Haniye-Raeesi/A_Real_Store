using _0_Framework.Application;
using _0_FrameWork.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System.Collections.Generic;

namespace ShopManagement.Application
{

    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (_productCategoryRepository.Exists(x=>x.Name==command.Name))
            {
               return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد.");
            }
            else
            {
                var slug = command.Slug.Slugify();
                var productCategory = new ProductCategory(command.Name,command.Description,command.Picture
                    ,command.PictureAlt,command.PictureTitle,command.KeyWords,command.MetaDescrioption,slug);
                _productCategoryRepository.Create(productCategory);
                _productCategoryRepository.Save();
                return operation.Successful();
            }
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);
            if (productCategory==null)
            {
                return operation.Failed("رکورد با اطلاعات درخواست شده یافت نشد!");
            }
            if (_productCategoryRepository.Exists(x=>x.Name==command.Name && x.Id!=command.Id))
            {
                return operation.Failed("امکان ثبت رکورد تکراری وجود ندارد");

            }
            var Slug = command.Slug.Slugify();
            productCategory.Edit(command.Name,command.Description,command.Picture
                ,command.PictureAlt,command.PictureTitle,command.KeyWords,command.MetaDescrioption,Slug);
            _productCategoryRepository.Save();
            return operation.Successful();
        }

        public EditProductCategory GetDetails(long Id)
        {
            return _productCategoryRepository.GetDetails(Id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return _productCategoryRepository.GetProductCategories();
        }

        
        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }

       
    }
}

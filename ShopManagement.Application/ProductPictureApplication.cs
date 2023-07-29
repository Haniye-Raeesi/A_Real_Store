using _0_FrameWork.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository)
        {
            _productPictureRepository = productPictureRepository;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var Operation = new OperationResult();
            if (_productPictureRepository.Exists(x=>x.ProductId==command.ProductId && x.Picture==command.Picture))
            {
                return Operation.Failed(ApplicationMessages.Duplicated);
            }
            var ProductPicture = new ProductPicture(command.ProductId,command.Picture,command.PictureAlt,command.PictureTitle);
            _productPictureRepository.Create(ProductPicture);
            _productPictureRepository.Save();
            return Operation.Successful();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var operation = new OperationResult();
            var ProductPicture = _productPictureRepository.GetByProductAndCategory(command.Id);
            if (ProductPicture==null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            if (_productPictureRepository.Exists(x => x.ProductId == command.ProductId 
            && x.Picture == command.Picture && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.Duplicated);
            }

           

            ProductPicture.Edit(command.ProductId,command.Picture,command.PictureAlt,command.PictureTitle);
            _productPictureRepository.Save();
            return operation.Successful();
            
        }

        public EditProductPicture GetDetails(long id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public OperationResult Remove(long Id)
        {
            var operation = new OperationResult();
            var ProductPicture = _productPictureRepository.Get(Id);
            if (ProductPicture==null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            ProductPicture.Remove();
            _productPictureRepository.Save();
            return operation.Successful();
        }

        public OperationResult Restore(long Id)
        {
            var operation = new OperationResult();
            var ProductPicture = _productPictureRepository.Get(Id);
            if (ProductPicture == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }
            ProductPicture.Restore();
            _productPictureRepository.Save();
            return operation.Successful();
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}

using _0_FrameWork.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;
using System;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly ISlideRepository _slideRepository;

        public SlideApplication(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }

        public OperationResult Create(CreateSlide command)
        {
            var Operation = new OperationResult();
            if (_slideRepository.Exists(x=>x.Picture == command.Picture))
            {
                return Operation.Failed(ApplicationMessages.Duplicated);
            }
            var Slide = new Slide(command.Picture,command.PictureAlt,command.PictureTitle
                ,command.Heading,command.Title,command.Text,command.BtnText,command.Link);
            _slideRepository.Create(Slide);
            _slideRepository.Save();
            return Operation.Successful();
        }

        public OperationResult Edit(EditSlide command)
        {
            var Operation = new OperationResult();
            var slide = _slideRepository.Get(command.Id);
            if (slide==null)
            {
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            }
           
            slide.Edit(command.Picture, command.PictureAlt, command.PictureTitle
                , command.Heading, command.Title, command.Text, command.BtnText, command.Link);
            _slideRepository.Save();
            return Operation.Successful();
        }

        public EditSlide GetDetails(long Id)
        {
          return  _slideRepository.GetDetails(Id);
        }

        public List<SlideViewModel> GetList()
        {
            return _slideRepository.GetList();
        }

        public OperationResult Remove(long Id)
        {
            var Operation = new OperationResult();
            var slide = _slideRepository.Get(Id);
            if (slide==null)
            {
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            }
            slide.Remove();
            _slideRepository.Save();
            return Operation.Successful();
        }

        public OperationResult Restore(long Id)
        {
            var Operation = new OperationResult();
            var slide = _slideRepository.Get(Id);
            if (slide == null)
            {
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            }
            slide.Restore();
            _slideRepository.Save();
            return Operation.Successful();
        }
    }
}

using _0_FrameWork.Domain;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountManagement.Domain.CustomerDiscountAgg
{
   public interface ICustomerDiscountRepository:IRepository<long,CustomerDiscount>
    {
        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
        EditCustomerDiscount GetDetails(long Id);
    }
}

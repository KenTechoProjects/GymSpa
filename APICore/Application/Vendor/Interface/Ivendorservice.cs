using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Application.Vendor.Interface
{
   public  interface Ivendorservice
    {
        Task<ResponseParam> Create_nightlife_product(Create_nightlife_productReq create_Nightlife_ProductReq);
    }
}

using Domain.Application.GymSpa.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Application.GymSpa.Connectors
{
    /// <summary>
    /// T1 is the returned object type while T2 is the feed in object type
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public interface IBService<T1> where T1 : class
    {
        
        Task<T1> Add(object entity) ;
        Task<object> Add(ErrorLogDbDTO entity) ;
       
        Task<T1> AddRange(Type entity);
        Task<T1> View(object entity, long Id, long errorLogDbId = 0, long orderId = 0, long categoryId = 0, int vendorId = 0, string vendorCode = "", string staffId = "", DateTime? aloteddate = null, long productId = 0, string memberId = null);

        Task<T1> ViewAll(Expression<Func<Type, bool>> predicate);
        Task<T1> ViewAll(object type);
        Task<T1> FileExists(object entity);

        // Task<T1> DeleteRange<T2>(T2 entity) where T2 : class;
        Task<T1> Update(object entity, long typeId);

        // Task<ResponseParam> Send_emailpaymentnotification_vendor(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);
        //Task<T1> Send_emailnotification_pnaadmin(Orderto_pna_admin_emailreq orderto_Pna_Admin_Emailreq);
    }
}

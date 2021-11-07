
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Application.TransactionNotification
{
   public  interface ITransactionNotification
    {
        Task<ResponseParam> CR(TransactionEmailDTO transactionEmailDTO);
        Task<ResponseParam> DR(TransactionEmailDTO transactionEmailDTO);
    }
}

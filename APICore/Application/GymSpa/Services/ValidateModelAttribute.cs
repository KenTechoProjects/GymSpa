//using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Web.Http.Filters;

namespace APICore.Application.GymSpa.Services
{
   
    public class ValidateModelAttribute //: ActionFilterAttribute
    {

        //public ValidateModelAttribute(int age)
        //{
        //    Age = age;
        //}

        //public int Age { get; }

        //public string GetErrorMessage() =>
        //    $"Classic movies must be age not later than {Age}.";

        //protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        //{
        //    var movie = (Staff)validationContext.ObjectInstance;
        //    var releaseYear = ((DateTime)value).Year;

        //    if (movie.Genre == Genre.Classic && releaseYear > Age)
        //    {
        //        return new ValidationResult(GetErrorMessage());
        //    }

        //    return ValidationResult.Success;
        //}
    }
}

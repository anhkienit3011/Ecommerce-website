using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucTapProject.Helper
{
    public enum ErrorMessage
    {
        Success,
        Failure,
        NotFound,
        NameErrors,
        PhoneErrors,
        EmailErrors,
        PhoneExits,
        EmailExits
    }

    class ErrHelper
    {
        public static string Log(ModelStateDictionary objectModel)
        {
            return string.Join("; ", objectModel.Values.SelectMany(c => c.Errors).Select(c => c.ErrorMessage));
        }
    }
}

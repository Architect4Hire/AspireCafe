using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Enums
{
    public enum DocumentType
    {
        Order = 1,
        Product = 2,
        Customer = 3,
        Employee = 4,
        Supplier = 5
    }

    public enum Error
    {
        None,
        NotFound,
        InvalidInput,
        Unauthorized,
        Forbidden,
        InternalServerError
    }
}


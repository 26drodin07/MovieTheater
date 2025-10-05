using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Get
{
    public record PricePolicyGetDTO (int Id, DateTime? startDate, bool? IsToEnd, decimal? price);

}

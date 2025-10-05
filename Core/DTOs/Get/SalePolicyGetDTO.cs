using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Get
{
    public record SalePolicyGetDTO (int Id, DateTime? startDate, decimal? saleCoof);

}

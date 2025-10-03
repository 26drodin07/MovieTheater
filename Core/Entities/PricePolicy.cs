using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Цена на конкретный сеанс с привязкой к временному интервалу
    /// </summary>
    public class PricePolicy
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateOnly PolicyStart { get; set; }
        public DateOnly? PolicyEnd { get; set; }
    }
}

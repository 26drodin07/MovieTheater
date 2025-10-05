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
        public DateTime PolicyStart { get; set; }
        /// <summary>
        /// До конца сеанса
        /// </summary>
        public bool IsToEnd { get; set; } = false;
        public int MovieSessionId { get; set; }
        public MovieSession MovieSession { get; set; }

    }
}

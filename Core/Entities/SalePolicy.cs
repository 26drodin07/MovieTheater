using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Скидки (глобально для всех сеансов)
    /// </summary>
    public class SalePolicy
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime PolicyStart { get; set; }
    }
}

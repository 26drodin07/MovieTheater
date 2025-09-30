using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Скидки (глобально для всех сеансов)
    /// </summary>
    internal class SalePolicy
    {
        public int Value { get; set; }
        public DateOnly PolicyStart { get; set; }
    }
}

using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class TheaterExtensions
    {
        public static async Task<SalePolicy?> GetCurrentSaleAsync(this IQueryable<SalePolicy> source) 
        {
            return await source.Where(sp => sp.PolicyStart < DateTime.UtcNow).OrderBy(sp => sp.PolicyStart).LastOrDefaultAsync();
        }
        public static DateTime AddTime(this DateTime time, DateTime timeToAdd)
        {
            return time.AddHours(timeToAdd.Hour).AddMinutes(timeToAdd.Minute).AddSeconds(timeToAdd.Second);
        }
    }
}

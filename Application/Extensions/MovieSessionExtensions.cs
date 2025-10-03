using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class MovieSessionExtensions
    {
        public static decimal GetCurrentPrice(this MovieSession movieSession)
        {
            var pricePolicy = movieSession.GetActivePricePolicy();
            return pricePolicy?.Price ?? movieSession.FallBackPrice;
        }
        public static PricePolicy? GetActivePricePolicy(this MovieSession movieSession)
        {
            return movieSession.PricePolices.SingleOrDefault(x =>
            x.PolicyStart.ToDateTime(new()) > DateTime.UtcNow &&
            (x.PolicyEnd == null || x.PolicyEnd?.ToDateTime(new()) < DateTime.UtcNow));
        }
    }
}

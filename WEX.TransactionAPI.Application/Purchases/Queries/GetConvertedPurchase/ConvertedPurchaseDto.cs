using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEX.TransactionAPI.Application.Purchases.Queries.GetConvertedPurchase
{
    public class ConvertedPurchaseDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly TransactionDate { get; set; }
        public decimal OriginalPurchaseAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}

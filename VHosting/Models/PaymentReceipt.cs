using System;
using System.Collections.Generic;

namespace VHosting
{
    public partial class PaymentReceipt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriptionType { get; set; }
        public string ReceiptLink { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}

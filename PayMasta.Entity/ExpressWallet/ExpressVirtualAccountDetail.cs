using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.ExpressWallet
{
    public class ExpressVirtualAccountDetail
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string VirtualAccountId { get; set; }
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string WalletId { get; set; }
        public string PhoneNumber { get; set; }
        public string WalletType { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string CurrentBalance { get; set; }
        public string AccountReference { get; set; }
        public string MerchantId { get; set; }
        public bool NameMatch { get; set; }
        public string DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Address { get; set; }
        public string Bvn { get; set; }
        public string JsonData { get; set; }
        public long UserId { get; set; }
        public string AuthJson { get; set; }
    }
}

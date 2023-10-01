using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.VirtualAccountDetail
{
	public class VirtualAccountDetail : BaseEntity
	{
		public string VirtualAccountId { get; set; }
		public string ProfileID { get; set; }
		public string Pin { get; set; }
		public string deviceNotificationToken { get; set; }
		public string PhoneNumber { get; set; }
		public string Gender { get; set; }
		public string DateOfBirth { get; set; }
		public string Address { get; set; }
		public string Bvn { get; set; }
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }
		public string CurrentBalance { get; set; }
		public string JsonData { get; set; }
		public long UserId { get; set; }
		public string AuthToken { get; set; }
		public string AuthJson { get; set; }


	}
}

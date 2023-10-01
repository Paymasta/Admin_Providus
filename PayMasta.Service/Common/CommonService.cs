using Newtonsoft.Json;
using PayMasta.Entity.ErrorLog;
using PayMasta.Repository.Common;
using PayMasta.Utilities;
using PayMasta.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.Common
{
    public class CommonService: ICommonService
    {
        private readonly ICommonReporsitory _commonReporsitory;
        public CommonService()
        {
            _commonReporsitory=new CommonReporsitory();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }
        public async Task<InvoiceNumberResponse> GetInvoiceNumber(int digit = 6)
        {
            var result = new InvoiceNumberResponse();
            try
            {
                using (var dbConnection = Connection)
                {
                    var Invoice = await _commonReporsitory.GetInvoiceNumber(dbConnection);
                    if (Invoice != null)
                    {
                        result.Id = Invoice.Id;
                        result.InvoiceNumber = Invoice.InvoiceNumber;
                        int power = digit - result.Id.ToString().Length;
                        var str = Math.Pow(10, power).ToString().Replace("1", "");
                        result.AutoDigit = str + result.Id.ToString();
                        result.Guid = Invoice.Guid;
                    }
                }
            }
            catch (Exception ex)
            {
                result.InvoiceNumber = CommonSetting.GetUniqueNumber();
            }
            return result;
        }

        public bool IsSessionValid(string token)
        {
            bool res = false;
            var result = _commonReporsitory.GetUserSessionByToken(token);
            if (result != null && result.IsActive == true && result.IsDeleted == false)
            {
                var userData = _commonReporsitory.GetUserByIdForSession(result.UserId);
                if (userData.IsActive == true && userData.IsDeleted == false && userData.Status == 1)
                {
                    res = true;
                }

            }

            return res;
        }

        public async Task<int> GetProvidusBankResponse(string request)
        {
            int result = 0;
            try
            {
                var json = JsonConvert.SerializeObject(request);
                using (var dbConnection = Connection)
                {
                    var req = new ErrorLog
                    {
                        ClassName = "Function name GetProvidusBankResponse",
                        CreatedDate = DateTime.Now,
                        ErrorMessage = json,
                        JsonData = json,
                        MethodName = "GetProvidusBankResponse"
                    };
                    var notifications = await _commonReporsitory.InsertProvidusBankResponse(req, dbConnection);
                    if(notifications>0)
                    {
                        result = notifications;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            return result;
        }

    }
}

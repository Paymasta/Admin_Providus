using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.TransactionsVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Transactions
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private string connectionString;

        public TransactionsRepository()
        {
            connectionString = AppSetting.ConnectionStrings;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public async Task<EmployeeTranactionDetail> GetEmployeeDetailByUserId(long userId, IDbConnection exdbConnection = null)
        {
            string query = @"select 
                                             UM.Id UserId
                                            ,UM.Guid UserGuid
                                            ,ISNULL(UM.FirstName,'NA')FirstName
                                            ,ISNULL(UM.LastName,'')LastName
                                            ,UM.Email
                                            ,UM.CountryCode
                                            ,UM.PhoneNumber
                                            ,UM.EmployerId
                                            ,CASE WHEN UM.EmployerName IS NULL THEN 'NA' WHEN  UM.EmployerName='' THEN'NA' ELSE  UM.EmployerName END EmployerName
                                            ,EM.EarnedAmount
                                            ,CAST(EM.UsableAmount as decimal(12,2)) AvailableAmount
                                            ,ED.EndDate-ED.StartDate [TotalWorkingHours]
                                            from UserMaster UM
                                            LEFT JOIN EarningMaster EM ON EM.UserId=UM.Id
                                            INNER JOIN EmployerDetail ED ON ED.Id=UM.EmployerId
                                            WHERE UM.Id=@Id AND  UM.IsActive=1 and UM.IsDeleted=0;";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeeTranactionDetail>(query,
                        new
                        {
                            Id = userId,
                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeeTranactionDetail>(query,
                        new
                        {
                            Id = userId,

                        })).FirstOrDefault();
            }
        }

        public async Task<List<EmployeeTransactions>> GetEmployeeTransactionByUserId(long userId,int pageSize,int pageNumber,int month, IDbConnection exdbConnection = null)
        {

            string query = @"select 
                                    COUNT(WT.WalletTransactionId) OVER() as TotalCount
                                    ,ROW_NUMBER() OVER(ORDER BY WT.WalletTransactionId DESC) AS RowNumber
                                    ,WT.WalletTransactionId
                                    ,WT.Guid
                                    ,WT.TotalAmount
                                    ,WS.ServiceName BillerName
                                    ,WT.CreatedAt 
                                    from WalletTransaction WT
                                    INNER join WalletService WS ON WS.Id=WT.ServiceCategoryId
                                    INNER JOIN UserMaster UM ON UM.Id=WT.SenderId
                                    WHERE WT.SenderId=@UserId
                                    AND (
										(@month IS NULL OR @month=0) OR (MONTH(WT.CreatedAt)=@month AND YEAR(WT.CreatedAt)=YEAR(GETDATE()))
										)
                                    ORDER BY WT.WalletTransactionId DESC 
                                    OFFSET @pageSize * (@pageNumber - 1) ROWS 
                                    FETCH NEXT @pageSize ROWS ONLY OPTION (RECOMPILE);";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<EmployeeTransactions>(query,
                        new
                        {
                            UserId = userId,
                            month=month,
                            pageNumber =pageNumber,
                            pageSize = pageSize,
                        })).ToList();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<EmployeeTransactions>(query,
                        new
                        {
                            UserId = userId,
                            month = month,
                            pageNumber = pageNumber,
                            pageSize = pageSize,
                        })).ToList();
            }
        }
    }
}

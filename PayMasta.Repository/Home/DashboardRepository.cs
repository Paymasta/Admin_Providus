using Dapper;
using PayMasta.Utilities;
using PayMasta.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Home
{
    public class DashboardRepository : IDashboardRepository
    {
        private string connectionString;

        public DashboardRepository()
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

        public async Task<Dashboard> GetDashboardData(DateTime? fromDate = null, DateTime? toDate = null,int month=0, IDbConnection exdbConnection = null)
        {

            string query = @"SELECT
                                                        TotalEmployees=(SELECT 
                                                                            COUNT(Id) 
                                                                         FROM [dbo].[UserMaster] 
                                                                         WHERE UserType  IN (4) 
                                                                         AND IsDeleted=0 AND IsActive=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
                                                        TotalUpdateProfileRequest=(SELECT 
                                                                     COUNT(UPR.Id) 
                                                                  FROM [dbo].[UpdateUserProfileRequest] UPR
									                              INNER JOIN  UserMaster UM on UM.Id=UPR.UserId
                                                                    WHERE UM.IsActive=1 and UM.IsDeleted=0 AND UPR.Status<>1
										                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,UPR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(UPR.CreatedAt)=@month AND YEAR(UPR.CreatedAt)=YEAR(GETDATE()))
											                               )
										                            ),
                                                        TotalUser=(SELECT 
                                                                     COUNT(Id) 
                                                                  FROM [dbo].[UserMaster] 
                                                                  WHERE IsActive=1 and IsDeleted=0 AND UserType IN (3,4)
									                               AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
									                              ),
							                            TotalEmployer=(SELECT 
                                                                          COUNT(Id) 
                                                                        FROM [dbo].[UserMaster] 
                                                                        WHERE UserType  IN (3) 
                                                                        AND IsDeleted=0  AND IsActive=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                            ),
							                            TotalQueries=(SELECT 
                                                                          COUNT(SP.Id) 
                                                                        FROM [dbo].[SupportMaster]  SP
											                             INNER JOIN  UserMaster UM on UM.Id=SP.UserId
                                                                        WHERE 
                                                                         UM.IsDeleted=0  AND UM.IsActive=1
											                              AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,SP.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(SP.CreatedAt)=@month AND YEAR(SP.CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
                                                        TotalTransactions=(SELECT 
                                                                          COUNT(WT.WalletTransactionId) 
                                                                        FROM [dbo].[WalletTransaction]  WT
											                             INNER JOIN  UserMaster UM on UM.Id=WT.SenderId
                                                                        WHERE 
                                                                         UM.IsDeleted=0  AND UM.IsActive=1
											                              AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,WT.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(WT.CreatedAt)=@month AND YEAR(WT.CreatedAt)=YEAR(GETDATE()))
											                               )
																		  ),
							                            TotalWithdrawRequest=(SELECT 
                                                                          COUNT(AAR.Id) 
                                                                        FROM [dbo].[AccessAmountRequest]  AAR
											                             INNER JOIN  UserMaster UM on UM.Id=AAR.UserId
                                                                        WHERE 
                                                                         UM.IsDeleted=0  AND UM.IsActive=1
											                              AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,AAR.CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		    -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(AAR.CreatedAt)=@month AND YEAR(AAR.CreatedAt)=YEAR(GETDATE()))
											                               )),
													 TotalCommisionEarning=( SELECT 
                                                                           SUM(convert(decimal(18,2),cast(CommisionAmount as float)) ) 
                                                                         FROM [dbo].[WalletTransaction] 
                                                                         WHERE TransactionStatus  IN (1) 
                                                                         AND IsDeleted=0 AND IsActive=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
														TotalEWASent=( SELECT 
                                                                           SUM(cast(AccessAmount as decimal(18,2))) 
                                                                         FROM [dbo].[AccessAmountRequest] 
                                                                         WHERE AdminStatus  IN (1) 
                                                                         AND IsDeleted=0 AND IsActive=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             ),
														TotalEWACommisionEarned=(SELECT 
                                                                           SUM(convert(decimal(18,2),cast(CommissionCharge as float)) ) 
                                                                         FROM [dbo].[AccessAmountRequest] 
                                                                         WHERE AdminStatus  IN (1) 
                                                                         AND IsDeleted=0 AND IsActive=1
											                             AND (
												                            (@fromDate IS NULL OR @todate is null) 
												                             OR 
												                            (CONVERT(DATE,CreatedAt) BETWEEN  Convert(Date,@fromDate) AND Convert(Date,@todate))
											                               )
																		   -------------month
																		    AND (
												                            (@month IS NULL OR @month=0) OR (MONTH(CreatedAt)=@month AND YEAR(CreatedAt)=YEAR(GETDATE()))
											                               )
											                             );";
            if (exdbConnection == null)
            {
                using (var dbConnection = Connection)
                {
                    return (await dbConnection.QueryAsync<Dashboard>(query,
                        new
                        {

                            fromDate = fromDate,
                            todate= toDate,
                            month=month,

                        })).FirstOrDefault();
                }
            }
            else
            {
                return (await exdbConnection.QueryAsync<Dashboard>(query,
                        new
                        {
                            fromDate = fromDate,
                            todate = toDate,
                            month = month,
                        })).FirstOrDefault();
            }
        }
    }
}

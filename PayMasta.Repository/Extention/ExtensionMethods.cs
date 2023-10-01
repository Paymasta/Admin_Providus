using Dapper;
using PayMasta.Entity.ErrorLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.Extention
{
    public static class ExtensionMethods
    {

        public static void ErrorLog(this String errorMessage, string className, string methodName, IDbConnection exdbConnection)
        {
            try
            {
                using (var dbConnection = exdbConnection)
                {

                    var error = new ErrorLog
                    {
                        ErrorMessage = errorMessage,
                        ClassName = className,
                        MethodName = methodName,
                        JsonData = string.Empty,
                        CreatedDate = DateTime.UtcNow,
                    };
                    string query = @"INSERT INTO [dbo].[ErrorLog]
                                                   ([ErrorMessage]
                                                   ,[ClassName]
                                                   ,[MethodName]
                                                   ,[JsonData]
                                                   ,[CreatedDate])
                                             VALUES
                                                   (@ErrorMessage
                                                   , @ClassName
                                                   , @MethodName
                                                   , @JsonData
                                                   , @CreatedDate)";

                    var d = exdbConnection.Execute(query, error);
                }
            }
            catch
            {


            }
        }
    }
}

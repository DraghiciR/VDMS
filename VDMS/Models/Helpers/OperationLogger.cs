using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VDMS.Models.Helpers
{
    public class OperationLogger
    {
        private static VDMSModel _db = new VDMSModel();
        public static void LogUserEvent(string IDuser, string IDaffecteduser, string typeOperation)
        {
            UserLog userlog = new UserLog { UserID = IDuser, AffectedUserID = IDaffecteduser, OperationType = typeOperation, LogDate = DateTime.Now };
            _db.UserLogs.Add(userlog);
            _db.SaveChanges();
            try
            {
                
            }
            catch (Exception ex)
            {

                ExceptionLogger.LogException(ex, "UserLog.LogUserEvent");
            }
            
        }

        public static void LogDocumentEvent(string IDuser, int IDdoc, string typeOperation)
        {
            DocumentLog documentlog = new DocumentLog { UserID = IDuser, DocID = IDdoc, OperationType = typeOperation, LogDate = DateTime.Now };
            _db.DocumentLogs.Add(documentlog);
            _db.SaveChanges();
        }
    }
}
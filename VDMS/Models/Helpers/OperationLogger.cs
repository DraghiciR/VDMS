using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VDMS.Models.Helpers
{
    public enum OperationType
    {
        [Description("Create")]
        Create,
        [Description("Edit")]
        Edit,
        [Description("View")]
        View,
        [Description("Delete")]
        Delete
    }
    
    public class OperationLogger
    {
        private static VDMSModel _db = new VDMSModel();
        public static void LogUserEvent(string IDuser, string IDaffecteduser, string typeOperation)
        {
            try
            {
                UserLog userlog = new UserLog { UserID = IDuser, AffectedUserID = IDaffecteduser, OperationType = typeOperation, LogDate = DateTime.Now };
                _db.UserLogs.Add(userlog);
                _db.SaveChanges();
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
            try
            {
                
            }
            catch (Exception ex)
            {

                ExceptionLogger.LogException(ex, "OperationLogger.LogDocumentEvent");
            }
           
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VDMS.Models.Helpers
{
    // Create our own utility for exceptions
    public sealed class ExceptionLogger
    {       
        // All methods are static, so this can be private
        private ExceptionLogger()
        { }

        /// <summary>
        /// Log an Exception to the ErrorLog.txt file in the App_Data folder.
        /// </summary>
        /// <param name="exc">the exception object </param>
        /// <param name="source">  a string containing details about the source of the exception </param>
        
        public static void LogException(Exception exc, string source)
        {
            // Include logic for logging exceptions
            // Get the absolute path to the log file
            string logFile = "../App_Data/VdmsLog.txt";
            logFile = HttpContext.Current.Server.MapPath(logFile);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }
    }
}
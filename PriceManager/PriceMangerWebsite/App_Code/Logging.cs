using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

/// <summary>
/// Summary description for Logging
/// </summary>
public class Logging
{
    private static string LogPath = ConfigurationManager.AppSettings["LogPath"];

	public Logging()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void WriteLog(LogType logType, string text)
    {
        try
        {
            string file = HttpContext.Current.Server.MapPath(LogPath);
            string dir = file.Substring(0, file.LastIndexOf(@"\"));
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            FileStream fStream;
            if (File.Exists(file))
                fStream = File.Open(file, FileMode.Append, FileAccess.Write);
            else
                fStream = File.Open(file, FileMode.CreateNew, FileAccess.ReadWrite);
            StreamWriter objStreamWriter = new StreamWriter(fStream, System.Text.Encoding.UTF8);
            objStreamWriter.WriteLine(logType.ToString() + " | " + text + "  |  " + DateTime.Now.ToString());
            objStreamWriter.Close();
            objStreamWriter.Dispose();
        }
        catch { }
    }
}

public enum LogType
{
    Error = 1,
    Warning = 2,
    Info = 3,
    Critical = 4
}
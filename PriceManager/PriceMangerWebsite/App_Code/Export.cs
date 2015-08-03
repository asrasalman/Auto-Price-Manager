using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for Export
/// </summary>
public class Export
{
	public Export()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void ExportToCsv(DataTable dataTable, string fileName)
    {
        HttpResponse response = HttpContext.Current.Response;
        
        response.Clear();
        response.Buffer = true;
        response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        response.Charset = "";
        response.ContentType = "application/text";

        //GridView1.AllowPaging = false;
        //GridView1.DataBind();

        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < dataTable.Columns.Count; k++)
        {
            //add separator header
            sb.Append(dataTable.Columns[k].ColumnName + ',');
        }
        //append new line
        sb.Append("\r\n");

        foreach (DataRow dr in dataTable.Rows)
        {
            foreach (DataColumn dc in dataTable.Columns)
            {
                //add separator
                sb.Append(dr[dc].ToString() + ',');
            }
            //append new line
            sb.Append("\r\n");
        }
        response.Output.Write(sb.ToString());
        response.Flush();
        response.End();
    }

    public static string CreateCsv(DataTable dataTable, string fileName)
    {
       
        string folder = "csv";
        string filePath = folder + @"\" + fileName;
        string file = AppDomain.CurrentDomain.BaseDirectory + filePath;
        FileStream fStream;

        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + folder))
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + folder);

        if (File.Exists(file))
            fStream = File.Open(file, FileMode.Truncate, FileAccess.Write);
        else
            fStream = File.Open(file, FileMode.CreateNew, FileAccess.ReadWrite);

        System.Text.UTF8Encoding enc = new UTF8Encoding(false);

        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < dataTable.Columns.Count; k++)
        {
            //add separator header
            sb.Append(dataTable.Columns[k].ColumnName.Replace(",", "") + ',');
        }
        //append new line
        sb.Append("\r\n");

        foreach (DataRow dr in dataTable.Rows)
        {
            foreach (DataColumn dc in dataTable.Columns)
            {
                //add separator
                sb.Append(dr[dc].ToString().Replace(",", "") + ',');
            }
            //append new line
            sb.Append("\r\n");
        }

        

        using (var writer = new StreamWriter(fStream, enc))
        {
            writer.Write(sb.ToString());
            writer.Close();
            writer.Dispose();
            fStream.Close();
            fStream.Dispose();
        }

        return file;
        
    }

    public static void ExportToExcel(DataTable dataTable, string fileName)
    {
        HttpResponse response = HttpContext.Current.Response;

        response.Clear();
        response.Buffer = true;
        response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        response.Charset = "utf-8";
        response.ContentType = "application/ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //GridView1.AllowPaging = false;
        //GridView1.DataBind();

        StringBuilder sb = new StringBuilder();
       sb.Append("<Table border='1' bgColor='#ffffff' " +
      "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
      "style='font-size:10.0pt; font-family:Calibri; background:white;'> <tr>");
        for (int k = 0; k < dataTable.Columns.Count; k++)
        {
            //add separator header
            
            sb.Append("<td>");
            sb.Append("</b>");
            sb.Append(dataTable.Columns[k].ColumnName );
            sb.Append("</b>");
            sb.Append("</td>");
            
        }
        //append new line
        sb.Append("</tr>");

        foreach (DataRow dr in dataTable.Rows)
        {
            sb.Append("<tr>");
            foreach (DataColumn dc in dataTable.Columns)
            {
                //add separator
                sb.Append("<td>");
                sb.Append(dr[dc].ToString());
                sb.Append("</td>");
            }
            //append new line
            sb.Append("</tr>");
        }
        response.Output.Write(sb.ToString());
        response.Flush();
        response.End();
    }
}
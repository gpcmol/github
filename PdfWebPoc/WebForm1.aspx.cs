using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace PdfWebApp {
  public partial class WebForm1 : Page {
    protected void Page_Load(object sender, EventArgs e) {
      string id = Request["id"] ?? "0";

      Warning[] warnings;
      string[] streamIds;
      string mimeType = string.Empty;
      string encoding = string.Empty; //enter code here`
      string extension = string.Empty;

      ReportViewer viewer = new ReportViewer();
      viewer.LocalReport.Refresh();
      viewer.LocalReport.ReportPath = "bin\\Report1.rdlc"; //This is your rdlc name.
      //  viewer.LocalReport.SetParameters(param);

      ReportDataSource rds = new ReportDataSource();
      rds.Name = "DataSet1"; //This refers to the dataset name in the RDLC file
      rds.Value = EmployeeRepository.GetAllEmployees();
      viewer.LocalReport.DataSources.Add(rds);

      viewer.LocalReport.EnableExternalImages = true;
      var _templateImage = "http://pngimg.com/upload/audi_PNG1736.png";
      viewer.LocalReport.SetParameters(new ReportParameter("ImageName", _templateImage));

      viewer.LocalReport.SetParameters(new ReportParameter("Id", id));

      byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      // byte[] bytes = viewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

      // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.          

      // System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

      Response.Buffer = true;
      Response.Clear();
      Response.ContentType = mimeType;
      Response.AddHeader("content-disposition", "inline; filename= filename" + "." + extension);
      Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
      Response.Flush(); // send it to the client to download  
      Response.End();
    }
  }
}
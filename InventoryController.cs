using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Structure;
using Test.Domain.Model;
using System.Threading;
using System.Data;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using Test.Reports;
using System.IO;
using CrystalDecisions.Shared;


namespace Test.Controllers
{
    public class InventoryController : BaseController
    {
        //
        // GET: /Inventory/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult POWiseDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult POWiseDetailsList(String poNumber = "", String SDate = "", String EDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    POWiseDetailsEntity _Model = new POWiseDetailsEntity();
                    _Model.PONumber = poNumber;
                    _Model.StartDate = SDate;
                    _Model.EndDate = EDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetPOWiseDetailsRecord, _Model);
                    List<POWiseDetailsEntity> ItemList = null;
                    ItemList = new List<POWiseDetailsEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new POWiseDetailsEntity()
                            {
                                PONumber = dr["PONumber"].ToString(),
                                GRNNumber = dr["GRNNumber"].ToString(),
                                SupplierName = dr["SupplierName"].ToString(),
                                ItemDetails = dr["ItemDetails"].ToString(),
                                Quantity = dr["Quantity"].ToString(),
                                QtyUnit = dr["QtyUnit"].ToString(),
                                Price = dr["Price"].ToString(),
                                PriceUnit = dr["PriceUnit"].ToString(),
                                TotalValue = dr["TotalValue"].ToString(),
                                GRNDate = dr["GRNDate"].ToString()
                                //PQCQty = dr["PQCQty"].ToString(),
                                //PQCBal = dr["PQCBal"].ToString()
                                //PackingQty = dr["PackingQty"].ToString() 
                            });
                        }
                        iCount += 1;
                    }
                    var RecordCount = dt.Rows.Count;
                    var Record = ItemList;
                    return Json(new { Result = "OK", Records = Record, TotalRecordCount = RecordCount });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult POWiseDetailsExcel(String poNumber = "")
        {
            POWiseDetailsEntity _Model = new POWiseDetailsEntity();
            _Model.PONumber = poNumber;
           
            DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetPOWiseDetailsRecord, _Model);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='" + "2px" + "'b>");

            ////For Header
            sb.Append("<td><td><td><b><font face=Arial size=2>" + "PO & GRN Details Information" + "</font></b></td></td></td>");
            //write column headings
            sb.Append("<tr>");

            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                sb.Append("<td><b><font face=Arial size=2>" + dc.ColumnName + "</font></b></td>");
            }
            sb.Append("</tr>");

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    sb.Append("<td><font face=Arial size=" + "14px" + ">" + dr[dc].ToString() + "</font></td>");
                }
                sb.Append("</tr>");
            }
            ////For Footer
            sb.Append("<tr>");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<td>");
            sb.Append("<td>");
            sb.Append("<td>");
            sb.Append("<td><b><font face=Arial size=2>" + "Powered By: Hasib, MIS Department" + "</font></b></td>");
            sb.Append("</td>");
            sb.Append("</td>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</tr>");
            sb.Append("</table>");

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=POWise" + "_" + DateTime.Now.ToString("dd-MMM-yy")+"(" + poNumber +")"+".xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.ContentType = "Application/x-msexcel"
            //this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";            
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            //return File(buffer, "application/vnd.ms-excel", "SalesReport.xls");
            //return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesReport.xlsx");
            return File(buffer, "application/vnd.ms-excel");

            //var service = new ExcelService();
            //var stream=service.
            //var memoryStream = stream as MemoryStream;
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader=("content-disposition","attachment; filename=ExcelDemo.xlsx");
            //Response.BinaryWrite(MemoryStream.ToArray());
        }


        public ActionResult DateWisePODetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DateWisePODetailsList(String poNumber = "", String SDate = "", String EDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    POWiseDetailsEntity _Model = new POWiseDetailsEntity();                    
                    _Model.StartDate = SDate;
                    _Model.EndDate = EDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetDateWisePODetailsRecord, _Model);
                    List<POWiseDetailsEntity> ItemList = null;
                    ItemList = new List<POWiseDetailsEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new POWiseDetailsEntity()
                            {
                                PONumber = dr["PONumber"].ToString(),
                                GRNNumber = dr["GRNNumber"].ToString(),
                                SupplierName = dr["SupplierName"].ToString(),
                                ItemDetails = dr["ItemDetails"].ToString(),
                                Quantity = dr["Quantity"].ToString(),
                                QtyUnit = dr["QtyUnit"].ToString(),
                                Price = dr["Price"].ToString(),
                                PriceUnit = dr["PriceUnit"].ToString(),
                                TotalValue = dr["TotalValue"].ToString(),
                                GRNDate = dr["GRNDate"].ToString()                                
                            });
                        }
                        iCount += 1;
                    }
                    var RecordCount = dt.Rows.Count;
                    var Record = ItemList;
                    return Json(new { Result = "OK", Records = Record, TotalRecordCount = RecordCount });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



    }
}

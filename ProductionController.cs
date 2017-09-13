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
    public class ProductionController : BaseController
    {
        //
        // GET: /Production/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DailyProduction()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DailyProductionList(String PDate = "", String Sitem = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.ProductionDate = PDate;
                    _Model.SortStatus = Sitem;                    
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllDailyProductionRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {
                                BuyerName = dr["BuyerName"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                PONumber = dr["PONumber"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                OrderQuantity = dr["OrderQuantity"].ToString(),
                                KnittingQty = dr["KnittingQty"].ToString(),
                                KnittingCum = dr["KnittingCum"].ToString(),
                                KnittingBal = dr["KnittingBal"].ToString(),
                                AssemblingQty = dr["AssemblingQty"].ToString(),
                                AssemblingCum = dr["AssemblingCum"].ToString(),
                                AssemblingBal = dr["AssemblingBal"].ToString(),
                                PQCQty = dr["PQCQty"].ToString(),
                                PQCCum = dr["PQCCum"].ToString(),
                                PQCBal = dr["PQCBal"].ToString()
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

        [HttpPost]
        public JsonResult DailyProductionTotalRecord(String PDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.ProductionDate = PDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetDailyProductionTotalRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {                               
                                KnittingQty = dr["KnittingQty"].ToString(),                               
                                AssemblingQty = dr["AssemblingQty"].ToString(),                                
                                PQCQty = dr["PQCQty"].ToString(),
                                PackQty = dr["PackQty"].ToString() 
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


        public ActionResult DailyProductiononExcel(String PDate = "", String Sitem = "")
        {
            DailyProductionEntity _Model = new DailyProductionEntity();
            _Model.ProductionDate = PDate;
            _Model.SortStatus = Sitem;
            DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllDailyProductionRecord, _Model);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='" + "2px" + "'b>");

            ////For Header
            sb.Append("<td><td><td><b><font face=Arial size=2>" + "Daily Production" + "</font></b></td></td></td>");
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

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=DailyProduction" + "_" + DateTime.Now.ToString("dd-MMM-yy") + ".xls");
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


        public ActionResult MonthlyProduction()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MonthlyProductionList(String styleName="", String SDate = "", String EDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.FactoryLot = styleName;
                    _Model.StartDate = SDate;
                    _Model.EndDate = EDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllMonthlyProductionRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {
                                BuyerName = dr["BuyerName"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                //PONumber = dr["PONumber"].ToString(),
                                //ColorName = dr["ColorName"].ToString(),
                                OrderQuantity = dr["OrderQuantity"].ToString(),
                                KnittingQty = dr["KnittingQty"].ToString(),
                                //KnittingCum = dr["KnittingCum"].ToString(),
                                //KnittingBal = dr["KnittingBal"].ToString(),
                                AssemblingQty = dr["AssemblingQty"].ToString(),
                                //AssemblingCum = dr["AssemblingCum"].ToString(),
                                //AssemblingBal = dr["AssemblingBal"].ToString(),
                                PQCQty = dr["PQCQty"].ToString(),
                                PackingQty = dr["PackingQty"].ToString()
                                //PQCCum = dr["PQCCum"].ToString(),
                                //PQCBal = dr["PQCBal"].ToString()
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

        public ActionResult MonthlyProductiononExcel(String styleName = "", String SDate = "", String EDate = "")
        {
            DailyProductionEntity _Model = new DailyProductionEntity();
            _Model.FactoryLot = styleName;
            _Model.StartDate = SDate;
            _Model.EndDate = EDate;
            DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllMonthlyProductionRecord, _Model);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='" + "2px" + "'b>");

            ////For Header
            sb.Append("<td><td><td><b><font face=Arial size=2>" + "Monthly & Style Wise Production" + "</font></b></td></td></td>");
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

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=MonthlyProduction" + "_" + DateTime.Now.ToString("dd-MMM-yy") + ".xls");
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


        public ActionResult StyleDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult StyleDetailsList(String styleName = "", String Sitem = "",  int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.FactoryLot = styleName;
                    _Model.SortStatus = Sitem;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetStyleDetailsListRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {
                                BuyerName = dr["BuyerName"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                PONumber = dr["PONumber"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                OrderQuantity = dr["OrderQuantity"].ToString(),
                                KnittingQty = dr["KnittingQty"].ToString(),
                                //KnittingCum = dr["KnittingCum"].ToString(),
                                //KnittingBal = dr["KnittingBal"].ToString(),
                                AssemblingQty = dr["AssemblingQty"].ToString(),
                                //AssemblingCum = dr["AssemblingCum"].ToString(),
                                //AssemblingBal = dr["AssemblingBal"].ToString(),
                                PQCQty = dr["PQCQty"].ToString(),
                                PackingQty = dr["PackingQty"].ToString(),
                                ProductionDate = dr["ScanDate"].ToString()
                                //PQCCum = dr["PQCCum"].ToString(),
                                //PQCBal = dr["PQCBal"].ToString()
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

        public ActionResult StyleDetailsExcel(String styleName = "", String Sitem = "")
        {
            DailyProductionEntity _Model = new DailyProductionEntity();
            _Model.FactoryLot = styleName;
            _Model.SortStatus = Sitem;
            DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetStyleDetailsListRecord, _Model);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='" + "2px" + "'b>");

            ////For Header
            sb.Append("<td><td><td><b><font face=Arial size=2>" + "Style Details Information" + "</font></b></td></td></td>");
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

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=StyleDetails" + "_" + DateTime.Now.ToString("dd-MMM-yy") + ".xls");
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



        public ActionResult PackingInput()
        {
            return View();
        }

        public ActionResult PackingDetails()
        {
            return View();
        }

        public ActionResult PackingTotalInfor(String PackDate)
        {
            if (PackDate.ToString().Trim() == "")
            {
                return Json(new { Result = "ERROR", Message = "Enter Destination ID" });
            }
            else
            {
                try
                {
                    PackingInputEntity obj = (PackingInputEntity)GetPackingTotalInfor(PackDate);

                    return Json(obj);
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }        
        }

        [HttpPost]
        public JsonResult PackingInputList( String CDate="", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    PackingInputEntity _Model = new PackingInputEntity();
                    _Model.InputDate = CDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllPackingInputRecord, _Model);
                    List<PackingInputEntity> ItemList = null;
                    ItemList = new List<PackingInputEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new PackingInputEntity()
                            {
                                Id = dr["ID"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                Color = dr["Color"].ToString(),
                                Size = dr["Size"].ToString(),
                                Quantity = dr["Quantity"].ToString(),
                                CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString())
                                //CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString()).ToString().ToString()
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

        [HttpPost]
        public JsonResult AddUpdatePackingInputDetils(PackingInputEntity _Model)
        {
            try
            {                
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SavePackingInputInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdatePackingInputInfo, _Model);
                if (isUpdate)
                {
                    var addedModel = _Model;
                    return Json(new { Result = "OK", Record = addedModel });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Information failed to save" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        
        public ActionResult LineEfficiency()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LineEfficiencyList(String CDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    LineEfficiencyEntity _Model = new LineEfficiencyEntity();
                    _Model.InputDate = CDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllLineEfficiencyRecord, _Model);
                    List<LineEfficiencyEntity> ItemList = null;
                    ItemList = new List<LineEfficiencyEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new LineEfficiencyEntity()
                            {
                                Id = dr["ID"].ToString(),
                                LineNumber = dr["LineNumber"].ToString(),
                                SMV = dr["SMV"].ToString(),
                                Manpower = dr["Manpower"].ToString(),
                                WorkingHour = dr["WHOUR"].ToString(),
                                CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString())
                                //CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString()).ToString().ToString()
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

        [HttpPost]
        public JsonResult AddUpdateLineEfficiencyDetils(LineEfficiencyEntity _Model)
        {
            try
            {
                _Model.Name = "LINE";
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveLineEfficiencyInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateLineEfficiencyInfo, _Model);
                if (isUpdate)
                {
                    var addedModel = _Model;
                    return Json(new { Result = "OK", Record = addedModel });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Information failed to save" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult MMAllocation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MMAllocationList(String CDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    MMAllocationEntity _Model = new MMAllocationEntity();
                    _Model.InputDate = CDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllMMAllocationRecord, _Model);
                    List<MMAllocationEntity> ItemList = null;
                    ItemList = new List<MMAllocationEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new MMAllocationEntity()
                            {
                                Id = dr["ID"].ToString(),
                                SectionName = dr["SectionName"].ToString(),
                                Manpower = dr["Manpower"].ToString(),
                                ActiveMachineQty = dr["ActiveMachineQty"].ToString(),
                                InActiveMachineQty = dr["InActiveMachineQty"].ToString(),
                                WorkingHour = dr["WorkingHour"].ToString(),
                                LostMinute = dr["LostMinute"].ToString(),
                                Others = dr["Others"].ToString(),
                                Remarks = dr["Remarks"].ToString(),
                                CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString())
                                //CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString()).ToString().ToString()
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

        [HttpPost]
        public JsonResult AddUpdateMMAllocation(MMAllocationEntity _Model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveMMAllocationRecord, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateMMAllocationRecord, _Model);
                if (isUpdate)
                {
                    var addedModel = _Model;
                    return Json(new { Result = "OK", Record = addedModel });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Information failed to save" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public bool DuplicateEfficiencyData(String iSectioName, String iCreatedOn)
        {
            try
            {
                EfficiencyPostEntity obj = (EfficiencyPostEntity)GetDuplicateEfficiencyData(iSectioName, iCreatedOn);
                //var obj1 = GetDupMail(UserID);                
                if (obj.SectionName == null)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        


        public ActionResult EfficiencyPosting()
        {
            return View();
        }

        public JsonResult EfficiencyPostingRecord(String PostDate="", String iSectionName="")
        {
            bool result = true;
            EfficiencyPostEntity _Model = new EfficiencyPostEntity();
            _Model.CreatedOn = PostDate;
            try
            {
                if (DuplicateEfficiencyData(_Model.SectionName, _Model.CreatedOn) != false)
                {
                    //DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetEfficiencyPostingRecordUpdate, _Model);                    
                    //return Json(new { Result = "Message", Message = "Invoice already Exists!." });                    
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
                else
                {                  
                    _Model.SectionName = iSectionName;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetEfficiencyPostingRecord, _Model);                 
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult EfficiencyPostingRecordUpdate()
        {
           
            try
            {
                   DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetEfficiencyPostingRecordUpdate, null);                   
                   return Json(new { Result = "OK" });              
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult Efficiency()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EfficiencyList(String CDate = "", String Sitem = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    EfficiencyViewEntity _Model = new EfficiencyViewEntity();
                    _Model.InputDate = CDate;                    
                    _Model.CheckPoints = Sitem;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllEfficiencyRecord, _Model);
                    List<EfficiencyViewEntity> ItemList = null;
                    ItemList = new List<EfficiencyViewEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new EfficiencyViewEntity()
                            {
                                FactoryLot = dr["FactoryLot"].ToString(),
                                SMV = dr["SMV"].ToString(),
                                ProductionQty = dr["ProductionQty"].ToString(),
                                ProductionMin = dr["ProductionMin"].ToString(),
                                AvailableMin = dr["AvailableMin"].ToString(),
                                LostMin = dr["LostMin"].ToString(),
                                StdEfficiency = dr["StdEfficiency"].ToString(),
                                OverallEfficiency = dr["OverallEfficiency"].ToString(),
                                CheckPoints = dr["CheckPoints"].ToString(),
                                Manpower = dr["Manpower"].ToString(),
                                ActiveMachineQty = dr["ActiveMachineQty"].ToString(),
                                InActiveMachineQty = dr["InActiveMachineQty"].ToString(),
                                WorkingHour = dr["WorkingHour"].ToString(),
                                Remarks = dr["Remarks"].ToString(),
                                CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString())
                                //CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString()).ToString().ToString()
                            });
                        }
                        iCount += 1;
                    }
                    var RecordCount = dt.Rows.Count;
                    var Record = ItemList;
                    Session["Efficiency"] = ItemList;
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

        public ActionResult EfficiencyReport()
        {
            EfficiencyVeiwReportEntity obj;

            ReportClass rptH = new ReportClass();
            ArrayList al = new ArrayList();
            rptH.FileName = Server.MapPath("/Reports/EfficiencyReport.rpt");
            rptH.Load();

            List<EfficiencyViewEntity> ItemList = (List<EfficiencyViewEntity>)Session["Efficiency"];
            foreach (EfficiencyViewEntity dr in ItemList)
            {
                obj = new EfficiencyVeiwReportEntity();

                obj.CheckPoints = dr.CheckPoints;
                obj.FactoryLot = dr.FactoryLot;
                obj.SMV = dr.SMV;
                obj.ProductionQty = dr.ProductionQty;
                obj.ProductionMin = dr.ProductionMin;
                obj.AvailableMin = dr.AvailableMin;
                obj.LostMin = dr.LostMin;
                obj.StdEfficiency = Convert.ToDouble( dr.StdEfficiency);
                obj.OverallEfficiency = dr.OverallEfficiency;
                obj.Manpower = dr.Manpower;
                obj.ActiveMachineQty = dr.ActiveMachineQty;
                obj.InActiveMachineQty=dr.InActiveMachineQty;
                obj.WorkingHour = dr.WorkingHour;
                obj.Remarks = dr.Remarks;
                obj.CurrentDate = dr.CurrentDate;

                al.Add(obj);
            }
            rptH.SetDataSource(al);
            MemoryStream stream = (MemoryStream)rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }


        public ActionResult SizeWise()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SizeWiseList(String styleName = "", String SDate = "", String EDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.FactoryLot = styleName;
                    _Model.StartDate = SDate;
                    _Model.EndDate = EDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetSizeWiseRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {
                                BuyerName = dr["BuyerName"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                //PONumber = dr["PONumber"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                Size = dr["Size"].ToString(),
                                OrderQuantity = dr["OrderQuantity"].ToString(),
                                KnittingQty = dr["KnittingQty"].ToString(),  
                                KnittingBal = dr["KnittingBal"].ToString(),
                                AssemblingQty = dr["AssemblingQty"].ToString(), 
                                AssemblingBal = dr["AssemblingBal"].ToString(),
                                PQCQty = dr["PQCQty"].ToString(),
                                PQCBal = dr["PQCBal"].ToString()
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

        public ActionResult SizeWiseExcel(String styleName = "", String SDate = "", String EDate = "")
        {
            DailyProductionEntity _Model = new DailyProductionEntity();
            _Model.FactoryLot = styleName;
            _Model.StartDate = SDate;
            _Model.EndDate = EDate;
            DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetSizeWiseRecord, _Model);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='" + "2px" + "'b>");

            ////For Header
            sb.Append("<td><td><td><b><font face=Arial size=2>" + "Style Size Wise Details" + "</font></b></td></td></td>");
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

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=StyleSizeDetails" + "_" + DateTime.Now.ToString("dd-MMM-yy") + ".xls");
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


        public ActionResult CutoffDate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CutoffDateList(String styleName = "", String SDate = "", String EDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DailyProductionEntity _Model = new DailyProductionEntity();
                    _Model.FactoryLot = styleName;
                    _Model.StartDate = SDate;
                    _Model.EndDate = EDate;
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetCutoffDateRecord, _Model);
                    List<DailyProductionEntity> ItemList = null;
                    ItemList = new List<DailyProductionEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new DailyProductionEntity()
                            {
                                //BuyerName = dr["BuyerName"].ToString(),
                                Country = dr["Country"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                PONumber = dr["PONumber"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                Size = dr["Size"].ToString(),
                                OrderQuantity = dr["OrderQuantity"].ToString(),
                                KnittingQty = dr["KnittingQty"].ToString(),
                                //KnittingBal = dr["KnittingBal"].ToString(),
                                AssemblingQty = dr["AssemblingQty"].ToString(),
                                //AssemblingBal = dr["AssemblingBal"].ToString(),
                                PQCQty = dr["PQCQty"].ToString(),
                                //PQCBal = dr["PQCBal"].ToString()
                                CutoffDate = dr["CutoffDate"].ToString(),
                                FOBDate = dr["FOBDate"].ToString()
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


        public ActionResult SubcontactInfo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubcontactInfoList(String CDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    SubcontactInfoEntity _Model = new SubcontactInfoEntity();
                    _Model.InputDate = CDate;

                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetSubcontactRecord, _Model);
                    List<SubcontactInfoEntity> ItemList = null;
                    ItemList = new List<SubcontactInfoEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new SubcontactInfoEntity()
                            {
                                Id = dr["Id"].ToString(),
                                SectionName = dr["SectionName"].ToString(),
                                FactoryLot = dr["FactoryLot"].ToString(),
                                Quantity = dr["Quantity"].ToString(),
                                CurrentDate = Convert.ToDateTime(dr["CurrentDate"].ToString())
                                
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

        [HttpPost]
        public JsonResult AddUpdateSubcontactInfo(SubcontactInfoEntity _Model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveSubcontactInfoRecord, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateSubcontactInfoRecord, _Model);
                if (isUpdate)
                {
                    var addedModel = _Model;
                    return Json(new { Result = "OK", Record = addedModel });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Information failed to save" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Domain.Model;
using System.Data;
using Test.Structure;
using Test.Models;

namespace Test.Controllers
{
    public class SalesController : BaseController
    {
        //
        // GET: /Sales/

        public ActionResult Index()
        {
            SalesCarList iModel = new SalesCarList();
            iModel.StartDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
            iModel.EndDate = DateTime.Today.Date.ToString("dd/MM/yyyy");
            return View(iModel);
        }


        [HttpPost]
        public JsonResult jGetSalesList( string StartDate = "", string EndDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CommonEntity iSet = new CommonEntity();
                iSet.StartDate = StartDate;
                iSet.EndDate = EndDate;
                DataTable dt = (DataTable)ExecuteDB(TestTask.MG_GetSaleCarDetailsInfo, iSet);

                List<SalesCarList> ItemList = null;
                ItemList = new List<SalesCarList>();
                
                int iCount = 0;
                int offset = 0;
                offset = jtStartIndex / jtPageSize;
                double dlTrNo = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                    {
                        ItemList.Add(new SalesCarList()
                        {
                            SL = (iCount + 1).ToString(),
                            Id = dr["Id"].ToString(),
                            HaatDate = dr["HaatDate"].ToString(),
                            Regno = dr["RegNo"].ToString(),
                            CarReview = dr["CarDetails"].ToString(),
                            BuyerID = dr["BuyerID"].ToString(),
                            BuyerName = dr["BuyerDetails"].ToString(),
                            TotalDeduct = dr["TotalDeduct"].ToString(),
                            TotalDeposit = dr["TotalDeposit"].ToString(),
                            TotalBuyerPayment = dr["TotalBuyerPayment"].ToString(),
                            TotalBuyerDues = dr["TotalBuyerDues"].ToString()
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
    }
}

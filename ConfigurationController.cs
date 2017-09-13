using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Structure;
using Test.Domain.Model;
using System.Threading;
using System.Data;

namespace Test.Controllers
{
    public class ConfigurationController : BaseController
    {
        //
        // GET: /Configuration/
        /// <summary>
        /// Hasib,,, Configuratin Controller.
        /// </summary>
        /// <returns></returns>

        public ActionResult ServiceName()
        {
            return View();
        }
        public ActionResult ServiceDetails()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ServiceNameList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllTrServicemasterRecord, null);
                    List<TrServicemasterEntity> ItemList = null;
                    ItemList = new List<TrServicemasterEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new TrServicemasterEntity()
                            {
                                Id = dr["ID"].ToString(),
                                Servicename = dr["Servicename"].ToString(),
                                Description = dr["Description"].ToString()
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
        public JsonResult AddUpdateServiceName(TrServicemasterEntity _Model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveTrServicemasterInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateTrServicemasterInfo, _Model);
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
        [HttpPost]
        public JsonResult DeleteServiceName(string ID)
        {
            try
            {
                Thread.Sleep(50);
                bool isUpdate = false;
                isUpdate = (bool)ExecuteDB(TestTask.AG_DeleteTrServicemasterInfoById, ID);
                if (isUpdate)
                    return Json(new { Result = "OK" });
                else
                    return Json(new { Result = "ERROR", Message = "Failed to Delete" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult AllServiceNameListItem()
        {
            try
            {
                var jList = GetAllServiceNameListItem().Select(c => new { DisplayText = c.Text, Value = c.Value });
                return Json(new { Result = "OK", Options = jList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ServiceNameDetilsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllTrServicedetailsRecord, null);
                    List<TrServicedetailsEntity> ItemList = null;
                    ItemList = new List<TrServicedetailsEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new TrServicedetailsEntity()
                            {
                                Id = dr["ID"].ToString(),
                                Srvicenameid = dr["Srvicenameid"].ToString(),
                                Detailsname = dr["Detailsname"].ToString(),
                                Govfee = dr["Govfee"].ToString(),
                                Servicefee = dr["Servicefee"].ToString(),
                                Othersfee = dr["Othersfee"].ToString(),
                                Fixedfigure =Convert.ToBoolean( dr["Fixedfigure"].ToString()),
                                Cc = dr["Cc"].ToString(),
                                Sit = dr["Sit"].ToString()
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
        public JsonResult AddUpdateServiceNameDetils(TrServicedetailsEntity _Model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveTrServicedetailsInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateTrServicedetailsInfo, _Model);
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
        [HttpPost]
        public JsonResult DeleteServiceNameDetils(string ID)
        {
            try
            {
                Thread.Sleep(50);
                bool isUpdate = false;
                isUpdate = (bool)ExecuteDB(TestTask.AG_DeleteTrServicedetailsInfoById, ID);
                if (isUpdate)
                    return Json(new { Result = "OK" });
                else
                    return Json(new { Result = "ERROR", Message = "Failed to Delete" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult SectionMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SectionMasterNameList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllIESectionMasterRecord, null);
                    List<IeSectionmasterEntity> ItemList = null;
                    ItemList = new List<IeSectionmasterEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new IeSectionmasterEntity()
                            {
                                Id = dr["ID"].ToString(),
                                SectionName = dr["Sectionname"].ToString(),
                                CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString())
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
        public JsonResult AddUpdateSectionName(IeSectionmasterEntity _Model)
        {
            try
            {
                _Model.ModifiedBy="E8E5C8F1-2199-4F43-839C-6F79D450003B";
                _Model.ModifiedOn=DateTime.Now;
                _Model.CreatedOn=DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveIeSectionmasterInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateIeSectionmasterInfo, _Model);
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

        [HttpPost]
        public JsonResult DeleteSectionName(string ID)
        {
            try
            {
                Thread.Sleep(50);
                bool isUpdate = false;
                isUpdate = (bool)ExecuteDB(TestTask.AG_DeleteIeSectionNameInfoById, ID);
                if (isUpdate)
                    return Json(new { Result = "OK" });
                else
                    return Json(new { Result = "ERROR", Message = "Failed to Delete" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




        public ActionResult ProcessDetails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AllSectionNameListItem()
        {
            try
            {
                var jList = GetAllSectionNameListItem().Select(c => new { DisplayText = c.Text, Value = c.Value });
                return Json(new { Result = "OK", Options = jList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult ProcessNameDetilsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllIEProcessNameDetilsRecord, null);
                    List<IeProcessdetailsEntity> ItemList = null;
                    ItemList = new List<IeProcessdetailsEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new IeProcessdetailsEntity()
                            {
                                Id = dr["ID"].ToString(),
                                SectionMasterID = dr["SectionMasterID"].ToString(),
                                ProcessName = dr["ProcessName"].ToString(),
                                CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString())
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
        public JsonResult AddUpdateProcessNameDetils(IeProcessdetailsEntity _Model)
        {
            try
            {
                _Model.ModifiedBy = "E8E5C8F1-2199-4F43-839C-6F79D450003B";
                _Model.ModifiedOn = DateTime.Now;
                _Model.CreatedOn = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveIeProcessdetailsInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateIeProcessdetailsInfo, _Model);
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

        [HttpPost]
        public JsonResult DeleteProcessNameDetils(string ID)
        {
            try
            {
                Thread.Sleep(50);
                bool isUpdate = false;
                isUpdate = (bool)ExecuteDB(TestTask.AG_DeleteProcessNameDetilsById, ID);
                if (isUpdate)
                    return Json(new { Result = "OK" });
                else
                    return Json(new { Result = "ERROR", Message = "Failed to Delete" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult OrderMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrderMasterList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllIEOrderMasterRecord, null);
                    List<IeOrdermasterEntity> ItemList = null;
                    ItemList = new List<IeOrdermasterEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new IeOrdermasterEntity()
                            {
                                Id = dr["ID"].ToString(),
                                StyleName = dr["StyleName"].ToString(),
                                BuyerName = dr["BuyerName"].ToString(),
                                OrderQty = dr["OrderQty"].ToString(),
                                CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString())
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
        public JsonResult AddUpdateOrderMaster(IeOrdermasterEntity _Model)
        {
            try
            {
                _Model.ModifiedBy = "E8E5C8F1-2199-4F43-839C-6F79D450003B";
                _Model.ModifiedOn = DateTime.Now;
                _Model.CreatedOn = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveIeOrdermasterInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateIeOrdermasterInfo, _Model);
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




        public ActionResult ColorMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ColorMasterList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                try
                {
                    DataTable dt = (DataTable)ExecuteDB(TestTask.AG_GetAllIEColorMasterRecord, null);
                    List<IeColormasterEntity> ItemList = null;
                    ItemList = new List<IeColormasterEntity>();
                    int iCount = 0;
                    int offset = 0;
                    offset = jtStartIndex / jtPageSize;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (iCount >= jtStartIndex && iCount < (jtPageSize * (offset + 1)))
                        {
                            ItemList.Add(new IeColormasterEntity()
                            {
                                Id = dr["ID"].ToString(),
                                ColorName = dr["ColorName"].ToString(),
                                CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString())
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
        public JsonResult AddUpdateColorMaster(IeColormasterEntity _Model)
        {
            try
            {
                _Model.ModifiedBy = "E8E5C8F1-2199-4F43-839C-6F79D450003B";
                _Model.ModifiedOn = DateTime.Now;
                _Model.CreatedOn = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }


                bool isUpdate = false;
                if (_Model.Id == null)
                    isUpdate = (bool)ExecuteDB(TestTask.AG_SaveIeColorMasterInfo, _Model);
                else
                    isUpdate = (bool)ExecuteDB(TestTask.AG_UpdateIeColorMasterInfo, _Model);
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

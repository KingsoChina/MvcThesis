using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICSharpCode.SharpZipLib.Zip;

namespace MvcThesis
{
    public class ThesisHelper
    {
        
        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="inputfile">上传的控件名</param>
        /// <returns></returns>
        public static string UpLoadXls(HttpPostedFileBase inputfile)
        {
            string orifilename = string.Empty;
            string uploadfilepath = string.Empty;
            string modifyfilename = string.Empty;
            string fileExtend = "";//文件扩展名
            int fileSize = 0;//文件大小
            try
            {
                if (inputfile.FileName != string.Empty)
                {
                    //得到文件的大小
                    fileSize = inputfile.ContentLength;
                    if (fileSize == 0)
                    {
                        throw new Exception("导入的Excel文件大小为0，请检查是否正确！");
                    }
                    //得到扩展名
                    fileExtend = inputfile.FileName.Substring(inputfile.FileName.LastIndexOf(".") + 1);
                    if (fileExtend.ToLower() != "xls")
                    {
                        throw new Exception("你选择的文件格式不正确，只能导入EXCEL文件！");
                    }
                    //路径
                    uploadfilepath = System.Web.HttpContext.Current.Server.MapPath("~/upload/Import");
                    //新文件名
                    modifyfilename = System.Guid.NewGuid().ToString();
                    modifyfilename += "." + inputfile.FileName.Substring(inputfile.FileName.LastIndexOf(".") + 1);
                    //判断是否有该目录
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(uploadfilepath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    orifilename = uploadfilepath + "\\" + modifyfilename;
                    //如果存在,删除文件
                    if (File.Exists(orifilename))
                    {
                        File.Delete(orifilename);
                    }
                    // 上传文件
                    inputfile.SaveAs(orifilename);
                }
                else
                {
                    throw new Exception("请选择要导入的Excel文件!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return modifyfilename;
        }


        //读取Eccel返回Dataset
        public static DataSet ExcelToDS(string Path,string TableName = "")
        {
            string strConn = "Provider='Microsoft.Jet.OLEDB.4.0';Data Source='" +Path+ "';" + "Extended Properties='Excel 8.0'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            TableName = TableName == "" ?  "[sheet1$]" :"["+TableName+"$]";
            strExcel = "select * from "+TableName;
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet(); myCommand.Fill(ds, "table1");
            conn.Close();
            return ds;
        }
        public static string C(string item,string value="")
        {
            MvcThesisMembershipContext db = new MvcThesisMembershipContext();
            if (value == "")
                return db.Settings.Single(m => m.Title == item).Content;
            else {
                db.Settings.Single(m => m.Title == item).Content = value;
                return value;
            }
                
        }
        /// <summary>
        /// 覆盖word文件
        /// </summary>
        /// <param name="inputfile">上传的控件名</param>
        /// <param name="fileurl">文件路径</param>
        public static bool CoverDoc(HttpPostedFileBase inputfile, string fileurl)
        {
            string uploadfilepath = string.Empty;
            string fileExtend = "";//文件扩展名
            int fileSize = 0;//文件大小
            try
            {
                if (inputfile.FileName != string.Empty)
                {
                    //得到文件的大小
                    fileSize = inputfile.ContentLength;
                    if (fileSize == 0)
                    {
                        throw new Exception("导入的word文件大小为0，请检查是否正确！");
                    }
                    //得到扩展名
                    fileExtend = inputfile.FileName.Substring(inputfile.FileName.LastIndexOf(".") + 1);
                    if (fileExtend.ToLower() != "doc" && fileExtend.ToLower() != "docx")
                    {
                        throw new Exception("你选择的文件格式不正确，只能导入Word文件！");
                    }
                    //路径
                    uploadfilepath = System.Web.HttpContext.Current.Server.MapPath(fileurl);
                    //如果存在,删除文件
                    if (File.Exists(uploadfilepath))
                    {
                        File.Delete(uploadfilepath);
                    }
                    // 上传文件
                    inputfile.SaveAs(uploadfilepath);
                    return true;
                }
                else
                {
                    throw new Exception("请选择要导入的Word文件!");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 上传word文件
        /// </summary>
        /// <param name="inputfile">上传的控件名</param>
        /// <param name="newfilename">新文件名</param>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static string UpLoadDoc(HttpPostedFileBase inputfile,string dirPath,string newfilename="")
        {
            string orifilename = string.Empty;
            string uploadfilepath = string.Empty;
            string modifyfilename = string.Empty;
            string fileExtend = "";//文件扩展名
            int fileSize = 0;//文件大小
            try
            {
                if (inputfile.FileName != string.Empty)
                {
                    //得到文件的大小
                    fileSize = inputfile.ContentLength;
                    if (fileSize == 0)
                    {
                        throw new Exception("导入的word文件大小为0，请检查是否正确！");
                    }
                    //得到扩展名
                    fileExtend = inputfile.FileName.Substring(inputfile.FileName.LastIndexOf(".") + 1);
                    if (fileExtend.ToLower() != "doc" && fileExtend.ToLower() != "docx")
                    {
                        throw new Exception("你选择的文件格式不正确，只能导入Word文件！");
                    }
                    string RelativePath = "~/upload/Document/" + dirPath;
                    //路径
                    uploadfilepath = System.Web.HttpContext.Current.Server.MapPath(RelativePath);
                    //新文件名
                    modifyfilename = newfilename == "" ? System.Guid.NewGuid().ToString() : newfilename;
                    modifyfilename += "." + inputfile.FileName.Substring(inputfile.FileName.LastIndexOf(".") + 1);
                    //判断是否有该目录
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(uploadfilepath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    orifilename = uploadfilepath + "\\" + modifyfilename;
                    //如果存在,删除文件
                    if (File.Exists(orifilename))
                    {
                        File.Delete(orifilename);
                    }
                    // 上传文件
                    inputfile.SaveAs(orifilename);
                    return RelativePath + "/" + modifyfilename;
                }
                else
                {
                    throw new Exception("请选择要导入的Word文件!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static bool DocumentToZip(string filePath,string DirPath)
        {
            FastZip zip = new FastZip();
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            DirPath = System.Web.HttpContext.Current.Server.MapPath(DirPath);
            zip.CreateZip(filePath, DirPath, true, "");
            return true;
        }
           

    }

}
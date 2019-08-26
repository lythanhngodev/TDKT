using Newtonsoft.Json;
using QLDN2019.Class;
using QLNT.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace TDKT.Services
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //[ScriptMethod(UseHttpGet = true)]
        [WebMethod]
        public string getCBCC()
        {
            try
            {
                return JSonHelper.ToJson(SqlHelper.ExecuteDataset(
                                                                    SiteMaster._vConnectString, CommandType.Text,
                                                                    "SELECT DoiTuongID, TenDoiTuong, Tuoi FROM dbo.tblDMDoiTuong", null
                                                                    ).Tables[0]);
            }
            catch (Exception ex)
            {
                return JSonHelper.ToJson(new DataTable());
            }
        }
        [WebMethod(EnableSession = true)]
        public string getCBCCfromPHP(object data)
        {
            try
            {
                string siteContent = string.Empty;
                string url = data.ToString();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); // Tạo yêu cầu
                request.AutomaticDecompression = DecompressionMethods.GZip; // Loại nén dữ liệu
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())  // Gửi đến query php
                    using (Stream responseStream = response.GetResponseStream())               // Lấy dữ liệu trả về từ php
                        using (StreamReader streamReader = new StreamReader(responseStream))       // Đọc dữ liệu
                        {
                            siteContent = streamReader.ReadToEnd(); // Ghi dữ liệu
                        }
                DataSet ds = (DataSet)JsonConvert.DeserializeObject(siteContent, (typeof(DataSet)));
                DataTable dt = ds.Tables[0];
                int success = 0;
                string sql = string.Empty;
                foreach (DataRow item in dt.Rows)
                {
                    sql = string.Empty;
                    sql = @"INSERT INTO dbo.tblDMDoiTuong ( DoiTuongID, TenDoiTuong, Tuoi ) OUTPUT inserted.* VALUES (N'" + item["DoiTuongID"].ToString()+ @"',N'" + item["TenDoiTuong"].ToString() + @"',N'" + item["Tuoi"].ToString() + @"')";
                    try
                    {
                        DataSet ds1 = SqlHelper.ExecuteDataset(SiteMaster._vConnectString, CommandType.Text, sql, null);
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            success++;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return JSonHelper.ToJson("Thêm thành công: " + success + "<br>" + "Thêm không thành công: "+(dt.Rows.Count- success));
            }
            catch (Exception ex)
            {
                return JSonHelper.ToJson(ex.ToString());
            }
        }

        [WebMethod(EnableSession = true)]
        public string setCBCCtoPHP(string url, string postData)
        {
            string webpageContent = string.Empty;
            try
            {
                String deviceToken = HttpUtility.UrlEncode("5E66BF7E-3642-4B5B-80C1-6BFB73698A96");
                postData = String.Format("deviceToken={0}&data={1}", deviceToken, getCBCC());
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                // Gửi dữ liệu
                using (Stream webpageStream = webRequest.GetRequestStream())
                {
                    webpageStream.Write(byteArray, 0, byteArray.Length);
                }
                // Lấy kết quả trả về
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        webpageContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw or return an appropriate response/exception
            }

            return JSonHelper.ToJson(webpageContent);
        }
    }
}

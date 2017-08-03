using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DianCan
{
    public partial class upload : System.Web.UI.Page
    {
        bool ChangeImageSize(string imgPath, int width, long maxFileSize)
        {
            var bmp = (Bitmap)System.Drawing.Image.FromFile(imgPath);
            Size s = new Size(width, width * bmp.Height / bmp.Width);
            var newBmp = new Bitmap(bmp, s);
            var ms = TrySaveJpeg(newBmp, maxFileSize);
            newBmp.Dispose();
            bmp.Dispose();

            if (ms == null) return false;
            FileStream fs = new FileStream(imgPath, FileMode.Create);
            ms.CopyTo(fs);
            fs.Close();
            return true;
        }

        Stream TrySaveJpeg(Bitmap bitmap, long maxByteSize)
        {
            MemoryStream msOld = new MemoryStream(),
                msNew = new MemoryStream();
            var codec = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(c => c.MimeType == "image/jpeg");
            var encParam = new EncoderParameters(1);

            encParam.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 0L);
            bitmap.Save(msOld, codec, encParam);
            if (msOld.Length > maxByteSize)
            {
                return null; //最差画质
            }
            encParam.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bitmap.Save(msNew, codec, encParam);

            if (msNew.Length < maxByteSize)
            {
                msNew.Seek(0, SeekOrigin.Begin);
                return msNew; //最佳画质
            }
            //假设文件大小和图像质量成正相关 这里没有具体测试 姑且认为成立
            //用二分法找到最合适的图像质量 
            long start = 1, end = 99;
            while (start < end)
            {
                long qua = (start + end) / 2;
                encParam.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qua);
                msNew.SetLength(0);
                bitmap.Save(msNew, codec, encParam);
                if (msNew.Length == maxByteSize)
                {
                    msNew.Seek(0, SeekOrigin.Begin);
                    return msNew;
                }
                else if (msNew.Length > maxByteSize)
                {
                    end = qua - 1;
                }
                else if (msNew.Length < maxByteSize)
                {
                    start = qua + 1;
                    MemoryStream temp = msNew;
                    msNew = msOld;
                    msOld = temp; //保存一个小于指定大小 并且最接近的
                }
            }
            msOld.Seek(0, SeekOrigin.Begin);
            return msOld;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.Form["filepath"] != null)
            {
                string filepath = Convert.ToString((Request.Form["filepath"]));
                HttpFileCollection files = Request.Files;
                string msg = string.Empty;
                string error = "图片上传成功";
                string imgurl;
                if (files.Count > 0)
                {
                    if (files[0].FileName != "")
                    {

                        files[0].SaveAs(Server.MapPath("/FoodImg/" + filepath + "/") + System.IO.Path.GetFileName(files[0].FileName));
                        msg = " 成功! 文件大小为:" + files[0].ContentLength;
                        imgurl = "/FoodImg/" + filepath + "/" + System.IO.Path.GetFileName(files[0].FileName);
                        string res = "{ error:'" + error + "', msg:'" + msg + "'}";
                        int rowsCount = 0;

                        try
                        {
                            if (filepath == "FoodAdd")
                            {
                                //string FoodName = Request.Form["FoodName"].ToString();
                                //string Category = Request.Form["Category"].ToString();
                                //string ShopID = Request.Form["ShopID"].ToString();
                                //string MenuID = Request.Form["MenuID"].ToString();
                                //string Des = Request.Form["Des"].ToString();
                                //string Price = Request.Form["Price"].ToString();
                                //string AdminID = Request.Form["AdminID"].ToString();
                                //string IsShow = Request.Form["IsShow"].ToString();
                                //int num = DBConnection.FoodInfo.InsertFoodInfo(FoodName,Convert.ToInt32(Category), imgurl,Convert.ToInt32(ShopID),Convert.ToInt32(MenuID), Des, Price, AdminID,Convert.ToInt32(IsShow));
                                res = "{ error:'" + error + "', msg:'" + 1 + "',imgurl:'" + imgurl+"'}";
                            }

                            else {
                                res = "{ error:'-1', msg:'文件未选择'}";
                            }
                            Response.Clear();
                            Response.Write(res);
                            Response.End();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}
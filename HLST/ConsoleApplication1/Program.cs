using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using BLL;
namespace ConsoleApplication1
{
    class Program
    {
        static string AppID = "16081609326748";
        static string AppSecret = "C89FD269-0798-46B6-BCE5-1BAC3B01E3AE";
        static void Main(string[] args)
        {
            Thread t = new Thread(Run);
            t.Start();
            
        }
        public static void Run()
        {
            while (true)
            {
                string url = "http://localhost:49599/api/DingCan/SelectFoodByPageIndex";
                string data = "intPageIndex=0&&eachPageNum=10&&Category=-1";
                string result = HttpPost(url, data);
                Console.WriteLine(result);
                Thread.Sleep(8000);
            }
            
        }
        public static string HttpPost(string Url, string postDataStr)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = postDataStr.Length;
        StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
        writer.Write(postDataStr);
        writer.Flush();
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        string encoding = response.ContentEncoding;
        if (encoding == null || encoding.Length < 1)
        {
            encoding = "UTF-8"; //默认编码  
        }
        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
        string retString = reader.ReadToEnd();
        return retString;
    }
    }
    

}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SQLUI;
using Basic;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Configuration;
using IBM.Data.DB2.Core;
using MyOffice;
using NPOI;
namespace 智慧調劑台管理系統_WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            string str = System.Environment.CurrentDirectory;
            //string jsonstring = MyFileStream.LoadFileAllText(@"C:\Users\HS\Documents\智慧調劑台管理系統_WebApi\智慧調劑台管理系統_WebApi\bin\Release\net5.0\test.txt", "utf-8");
            string jsonstring = MyFileStream.LoadFileAllText(@$"{str}\test.txt", "utf-8");
            return "Api test sucess!";
        }
        [Route("price")]
        [HttpGet]
        public string Get_price()
        {
            String MyDb2ConnectionString = "server=DBGW1.VGHKS.GOV.TW:50000;database=DBDSNP;uid=APUD07;pwd=UD07AP;";
            IBM.Data.DB2.Core.DB2Connection MyDb2Connection = new IBM.Data.DB2.Core.DB2Connection(MyDb2ConnectionString);
            Console.Write($"開啟DB2連線....");
            MyDb2Connection.Open();
            Console.WriteLine($"DB2連線成功!");
            IBM.Data.DB2.Core.DB2Command MyDB2Command = MyDb2Connection.CreateCommand();
            MyDB2Command.CommandText = "SELECT * FROM UD.UDDRGVWA WHERE HID = '2A0'";

            var reader = MyDB2Command.ExecuteReader();
            List<string> list_colname_UDDRGVWA = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                list_colname_UDDRGVWA.Add(name);
            }
            while (reader.Read())
            {

            }
            reader.Close();
            MyDB2Command.CommandText = "SELECT * FROM UD.UDPRDPF WHERE HID = '2A0'";
            List<string> list_colname_UDPRDPF = new List<string>();
            var reader_UDPRDPF = MyDB2Command.ExecuteReader();
            for (int i = 0; i < reader_UDPRDPF.FieldCount; i++)
            {
                string name = reader_UDPRDPF.GetName(i);
                list_colname_UDPRDPF.Add(name);
            }
            return list_colname_UDDRGVWA.JsonSerializationt() + list_colname_UDPRDPF.JsonSerializationt();
        }

        [Route("OrderLastWriteTime")]
        public string Get_OrderLastWriteTime()
        {

            DateTime dateTime = System.IO.File.GetLastWriteTime(@"C:\itinvd0304.txt");
            return dateTime.ToDateTimeString();
        }
    }

 
}

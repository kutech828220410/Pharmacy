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
    public class MedPriceController : ControllerBase
    {
        public class class_MedPrice
        {
            public string 藥品碼 { get; set; }
            public string 售價 { get; set; }
            public string 成本價 { get; set; }
            public string 最近一次售價 { get; set; }
            public string 最近一次成本價 { get; set; }
            public string ATC { get; set; }

        }

        [HttpGet]
        public string Get()
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
            List<class_MedPrice> class_MedPrices = new List<class_MedPrice>();
            while (reader.Read())
            {
                class_MedPrice class_MedPrice = new class_MedPrice();
                class_MedPrice.藥品碼 = reader["UDDRGNO"].ToString();
                class_MedPrice.售價 = reader["UDPRICE"].ToString();
                class_MedPrice.成本價 = reader["UDWCOST"].ToString();
                class_MedPrice.最近一次售價 = reader["UDOLDPRI"].ToString();
                class_MedPrice.最近一次成本價 = reader["UDOLDWCO"].ToString();
                class_MedPrice.ATC = reader["UDATCCOD"].ToString();

                
                class_MedPrices.Add(class_MedPrice);
            }
            reader.Close();
           
            return class_MedPrices.JsonSerializationt();
        }
    }
}

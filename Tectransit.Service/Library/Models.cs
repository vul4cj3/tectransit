using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tectransit.Service.Library
{
    class Models
    {
    }

    /* --- TRANSDEPOT use --- */
    //Postdata
    public class data
    {
        public long accountid { get; set; }
        public string accountname { get; set; }
        public string shippingno { get; set; }
        public string mawbno { get; set; }
        public string flightno { get; set; }
        public string totalweight { get; set; }
        public string total { get; set; }
        public List<dataItems> items { get; set; }

    }

    public class dataItems
    {
        public string clearanceno { get; set; }
        public string transferno { get; set; }
        public string weight { get; set; }
        public string totalitem { get; set; }
        public string receiver { get; set; }
        public string receiveraddr { get; set; }
        public string receiverphone { get; set; }
        public string taxid { get; set; }
        public List<dataDetail> detail { get; set; }

    }

    public class dataDetail
    {
        public string product { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public int unitprice { get; set; }
        public string origin { get; set; }

    }

    //回傳結果
    public class result
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string errormsg { get; set; }

    }

    /* --- TRANSTECECO use --- */
    //Postdata
    public class Ecodata
    {
        public string TRACKINGNO { get; set; }
        public string TRANSFERNO { get; set; }
        public string CUSTOMERID { get; set; }        
        public List<EcoItems> DETAIL { get; set; }

    }

    public class EcoItems
    {
        public string TWDATE { get; set; }
        public string TWPLACE { get; set; }
        public string TWSTATUS { get; set; }
        public string ENDATE { get; set; }
        public string ENPLACE { get; set; }
        public string ENSTATUS { get; set; }
    }

    //回傳結果
    public class Ecoresult
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string error { get; set; }

    }
}

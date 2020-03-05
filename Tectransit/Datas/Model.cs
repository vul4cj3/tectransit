using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tectransit.Datas
{
    public class Model
    {
    }

    public class MenuInfo
    {
        public string MENUID { set; get; }
        public string MENUCODE { set; get; }
        public string PARENTCODE { set; get; }
        public string MENUURL { set; get; }
        public string MENUNAME { set; get; }
        public string MENUDESC { set; get; }
        public string MENUSEQ { set; get; }
        public string ICONURL { set; get; }
        public string ISBACK { set; get; }
        public string ISVISIBLE { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string HASPOWER { set; get; }
    }

    public class MenuInfo_F
    {
        public long MENUID { set; get; }
        public string MENUCODE { set; get; }
        public string PARENTCODE { set; get; }
        public string MENUURL { set; get; }
        public string MENUNAME { set; get; }
        public int MENUSEQ { set; get; }
    }

    public class RoleInfo
    {
        public long ROWID { set; get; }
        public long ROLEID { set; get; }
        public string ROLESEQ { set; get; }
        public string ROLECODE { set; get; }
        public string ROLENAME { set; get; }
        public string ROLEDESC { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string ISENABLE { set; get; }
    }

    public class UserRoleInfo
    {
        public string ROLEID { set; get; }
        public string ROLESEQ { set; get; }
        public string ROLECODE { set; get; }
        public string ROLENAME { set; get; }
        public string HASPOWER { set; get; }
    }

    public class UserInfo
    {
        public long ROWID { set; get; }
        public long USERID { set; get; }
        public string USERSEQ { set; get; }
        public string USERCODE { set; get; }
        public string USERNAME { set; get; }
        public string USERDESC { set; get; }
        public string EMAIL { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string ISENABLE { set; get; }
    }

    public class UserLogInfo
    {
        public long ROWID { set; get; }
        public string USERCODE { set; get; }
        public string USERNAME { set; get; }
        public string POSITION { set; get; }
        public string TARGET { set; get; }
        public string MESSAGE { set; get; }
        public string LOGDATE { set; get; }
    }

    public class RankInfo
    {
        public long ROWID { set; get; }
        public long RANKID { set; get; }
        public string RANKTYPE { set; get; }
        public string RANKSEQ { set; get; }
        public string RANKCODE { set; get; }
        public string RANKNAME { set; get; }
        public string RANKDESC { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string ISENABLE { set; get; }
    }

    public class AccountInfo
    {
        public long ROWID { set; get; }
        public long USERID { set; get; }
        public string USERSEQ { set; get; }
        public string USERCODE { set; get; }
        public string COMPANYNAME { set; get; }
        public string RATEID { set; get; }
        public string USERNAME { set; get; }
        public string USERDESC { set; get; }
        public string WAREHOUSENO { set; get; }
        public string EMAIL { set; get; }
        public string TAXID { set; get; }
        public string IDPHOTO_F { set; get; }
        public string IDPHOTO_B { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDRESS { set; get; }
        public string LOGINCOUNT { set; get; }
        public string LASTLOGINDATE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string ISENABLE { set; get; }
    }

    public class RankAccountInfo
    {
        public string RANKID { set; get; }
        public string RANKSEQ { set; get; }
        public string RANKCODE { set; get; }
        public string RANKNAME { set; get; }
        public string HASPOWER { set; get; }
    }

    public class DeclarantInfo
    {
        public long ROWID { set; get; }
        public long ID { set; get; }
        public long TYPE { set; get; }
        public string NAME { set; get; }
        public string TAXID { set; get; }
        public string IDPHOTO_F { set; get; }
        public string IDPHOTO_B { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDR { set; get; }
        public string APPOINTMENT { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class BannerInfo
    {
        public long BANID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string IMGURL { set; get; }
        public string URL { set; get; }
        public string BANSEQ { set; get; }
        public string UPSDATE { set; get; }
        public string UPEDATE { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class NewsInfo
    {
        public long NEWSID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string NEWSSEQ { set; get; }
        public string UPSDATE { set; get; }
        public string UPEDATE { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class AboutCate
    {
        public long CATEID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string ABOUTSEQ { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class AboutInfo
    {
        public long ABOUTID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string ABOUTSEQ { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public long CATEID { set; get; }
        public string CATETITLE { set; get; }
    }

    public class FaqCate
    {
        public long CATEID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string FAQSEQ { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class FaqInfo
    {
        public long FAQID { set; get; }
        public string TITLE { set; get; }
        public string DESCR { set; get; }
        public string FAQSEQ { set; get; }
        public string ISTOP { set; get; }
        public string ISENABLE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
        public string CATEID { set; get; }
    }

    public class StationInfo
    {
        public long STATIONID { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string COUNTRYCODE { set; get; }
        public string RECEIVER { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDRESS { set; get; }
        public string STATIONSEQ { set; get; }
        public string REMARK { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class MemStationInfo
    {
        public long STATIONID { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string COUNTRYCODE { set; get; }
        public string RECEIVER { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDRESS { set; get; }
        public string USERNAME { set; get; }
        public string WAREHOUSENO { set; get; }
    }

    /*----------------- 個人會員用 ----------------*/

    public class TransferMInfo
    {
        public long ID { set; get; }
        public long ACCOUNTID { set; get; }
        public string ACCOUNTCODE { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string TRANSFERNO { set; get; }
        public string TRANSFERCOMPANY { set; get; }
        public string PLENGTH { set; get; }
        public string PWIDTH { set; get; }
        public string PHEIGHT { set; get; }
        public string PWEIGHT { set; get; }
        public string PVALUEPRICE { set; get; }
        public string TOTAL { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string ISMULTRECEIVER { set; get; }
        public string STATUS { set; get; }
        public string REMARK { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class TransferHInfo
    {
        public long ID { set; get; }
        public string BOXNO { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string REMARK { set; get; }
        public long TRANSFERID_M { set; get; }
    }

    public class TransferDInfo
    {
        public long ID { set; get; }
        public string PRODUCT { set; get; }
        public string PRODUCTURL { set; get; }
        public string UNITPRICE { set; get; }
        public string QUANTITY { set; get; }
        public string REMARK { set; get; }
        public long TRANSFERID_M { set; get; }
        public long TRANSFERID_H { set; get; }
    }

    public class DeclarantMemInfo
    {
        public long ID { set; get; }
        public long TYPE { set; get; }
        public string NAME { set; get; }
        public string TAXID { set; get; }
        public string IDPHOTOF { set; get; }
        public string IDPHOTOB { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDR { set; get; }
        public string APPOINTMENT { set; get; }
        public long TRANSFERID_M { set; get; }
        public long SHIPPINGID_M { set; get; }
    }

    public class TransferNONInfo
    {
        public long ID { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string TRANSFERNO { set; get; }
        public string TRANSFERCOMPANY { set; get; }
        public string ACCOUNTNAME { set; get; }
        public string PLENGTH { set; get; }
        public string PWIDTH { set; get; }
        public string PHEIGHT { set; get; }
        public string PWEIGHT { set; get; }
        public string PVALUEPRICE { set; get; }
        public string STATUS { set; get; }
        public string REMARK { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class ShippingMInfo
    {
        public long ID { set; get; }
        public long ACCOUNTID { set; get; }
        public string ACCOUNTCODE { set; get; }
        public string COMPANYNAME { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string SHIPPINGNO { set; get; }
        public string TRACKINGNO { set; get; }
        public string TRACKINGDESC { set; get; }
        public string TRACKINGREMARK { set; get; }
        public string PLENGTH { set; get; }
        public string PWIDTH { set; get; }
        public string PHEIGHT { set; get; }
        public string PWEIGHT { set; get; }
        public string PVALUEPRICE { set; get; }
        public string MAWBNO { set; get; }
        public string CLEARANCENO { set; get; }
        public string HAWBNO { set; get; }
        public string TOTAL { set; get; }
        public string TOTALPRICE { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string ISMULTRECEIVER { set; get; }
        public string TRACKINGTYPE { set; get; }
        public string STATUS { set; get; }
        public string PAYTYPE { set; get; }
        public string PAYSTATUS { set; get; }
        public string REMARK1 { set; get; }
        public string REMARK2 { set; get; }
        public string REMARK3 { set; get; }
        public string PAYDATE { set; get; }
        public string EXPORTDATE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class ShippingHInfo
    {
        public long ID { set; get; }
        public string TRANSFERNO { set; get; }
        public string BOXNO { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string REMARK { set; get; }
        public long SHIPPINGID_M { set; get; }
    }

    public class ShippingDInfo
    {
        public long ID { set; get; }
        public string PRODUCT { set; get; }
        public string PRODUCTURL { set; get; }
        public string UNITPRICE { set; get; }
        public string QUANTITY { set; get; }
        public string REMARK { set; get; }
        public long SHIPPINGID_M { set; get; }
        public long SHIPPINGID_H { set; get; }
    }

    /*----------------- upload ----------------*/
    public class IDImgList
    {
        public string ID { set; get; }
        public string IDPHOTOF { set; get; }
        public string IDPHOTOB { set; get; }
    }

    public class IDFileList
    {
        public string ID { set; get; }
        public string APPOINTMENT { set; get; }
    }

    /*----------------- 廠商用 ----------------*/
    public class ShippingMCusInfo
    {
        public long ID { set; get; }
        public long ACCOUNTID { set; get; }
        public string ACCOUNTCODE { set; get; }
        public string COMPANYNAME { set; get; }
        public string STATIONCODE { set; get; }
        public string STATIONNAME { set; get; }
        public string SHIPPINGNO { set; get; }
        public string TRACKINGNO { set; get; }
        public string MAWBNO { set; get; }
        public string CLEARANCENO { set; get; }
        public string HAWBNO { set; get; }
        public string TRANSFERNO { set; get; }        
        public string TOTAL { set; get; }
        public string TRACKINGTYPE { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string ISMULTRECEIVER { set; get; }
        public string STATUS { set; get; }
        public string REMARK1 { set; get; }
        public string REMARK2 { set; get; }
        public string REMARK3 { set; get; }
        public string PAYDATE { set; get; }
        public string EXPORTDATE { set; get; }
        public string CREDATE { set; get; }
        public string CREBY { set; get; }
        public string UPDDATE { set; get; }
        public string UPDBY { set; get; }
    }

    public class ShippingHCusInfo
    {
        public long ID { set; get; }
        public string BOXNO { set; get; }
        public string RECEIVER { set; get; }
        public string RECEIVERADDR { set; get; }
        public string REMARK { set; get; }
        public long SHIPPINGID_M { set; get; }
    }

    public class ShippingDCusInfo
    {
        public long ID { set; get; }
        public string PRODUCT { set; get; }
        public string PRODUCTURL { set; get; }
        public string UNITPRICE { set; get; }
        public string QUANTITY { set; get; }
        public string REMARK { set; get; }
        public long SHIPPINGID_M { set; get; }
        public long SHIPPINGID_H { set; get; }
    }

    public class DeclarantCusInfo
    {
        public long ID { set; get; }
        public long TYPE { set; get; }
        public string NAME { set; get; }
        public string TAXID { set; get; }
        public string IDPHOTOF { set; get; }
        public string IDPHOTOB { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDR { set; get; }
        public string APPOINTMENT { set; get; }
        public long SHIPPINGID_M { set; get; }
    }
}

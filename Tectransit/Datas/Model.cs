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
        public string USERNAME { set; get; }
        public string USERDESC { set; get; }
        public string WAREHOUSENO { set; get; }
        public string EMAIL { set; get; }
        public string TAXID { set; get; }
        public string IDPHOTO_F { set; get; }
        public string IDPHOTO_B { set; get; }
        public string PHONE { set; get; }
        public string MOBILE { set; get; }
        public string ADDR { set; get; }
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
}

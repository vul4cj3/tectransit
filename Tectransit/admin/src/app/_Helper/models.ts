export interface MenuInfo {
  menuid: number;
  menucode: string;
  parentcode: string;
  menuurl: string;
  menuname: string;
  menudesc: string;
  iconurl: string;
  isback: string;
  isvisible: string;
  isenable: string;
  haspower: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface RoleInfo {
  rowid: number;
  roleid: number;
  roleseq: string;
  rolecode: string;
  rolename: string;
  roledesc: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface RoleUserMapInfo {
  roleid: number;
  rolecode: string;
  rolename: string;
  haspower: string;
}

export interface UserInfo {
  rowid: number;
  userid: number;
  userseq: string;
  usercode: string;
  username: string;
  userdesc: string;
  email: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface UserLog {
  rowid: number;
  usercode: string;
  username: string;
  position: string;
  target: string;
  message: string;
  logdate: string;
}

export interface RankInfo {
  rowid: number;
  rankid: number;
  ranktype: string;
  rankseq: string;
  rankcode: string;
  rankname: string;
  rankdesc: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface AccountInfo {
  rowid: number;
  userid: number;
  userseq: string;
  usercode: string;
  username: string;
  userdesc: string;
  warehouseno: string;
  companyname: string;
  rateid: string;
  email: string;
  taxid: string;
  idphoto_f: string;
  idphoto_b: string;
  phone: string;
  mobile: string;
  addr: string;
  logincount: string;
  lastlogindate: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface RankAccMapInfo {
  rankid: number;
  rankcode: string;
  rankname: string;
  haspower: string;
}

export interface DeclarantInfo {
  rowid: number;
  type: number;
  name: string;
  taxid: string;
  phone: string;
  mobile: string;
  addr: string;
  idphoto_f: string;
  idphoto_b: string;
  appointment: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface BannerInfo {
  banid: number;
  title: string;
  descr: string;
  imgurl: string;
  url: string;
  banseq: string;
  upsdate: string;
  upedate: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface NewsInfo {
  newsid: number;
  title: string;
  descr: string;
  newsseq: string;
  upsdate: string;
  upedate: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface AboutCate {
  cateid: number;
  title: string;
  descr: string;
  aboutseq: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface AboutInfo {
  aboutid: number;
  title: string;
  descr: string;
  aboutseq: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
  cateid: string;
}

export interface FaqCate {
  cateid: number;
  title: string;
  descr: string;
  faqseq: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
}

export interface FaqInfo {
  faqid: number;
  title: string;
  descr: string;
  faqseq: string;
  istop: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
  isenable: string;
  cateid: string;
}

export interface StationInfo {
  stationid: number;
  stationcode: string;
  stationname: string;
  countrycode: string;
  receiver: string;
  phone: string;
  mobile: string;
  address: string;
  stationseq: string;
  remark: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

/* ----- 個人會員 ----- */

export interface TransferMInfo {
  id: number;
  accountid: number;
  accountcode: string;
  accountname: string;
  companyname: string;
  stationcode: string;
  stationname: string;
  transferno: string;
  transfercompany: string;
  plength: string;
  pwidth: string;
  pheight: string;
  pweight: string;
  pvalueprice: string;
  total: string;
  receiver: string;
  receiverphone: string;
  receiveraddr: string;
  ismultreceiver: string;
  status: string;
  remark: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface TransferHInfo {
  id: number;
  boxno: string;
  receiver: string;
  receiverphone: string;
  receiveraddr: string;
  remark: string;
  transferiD_M: number;
}

export interface TransferDInfo {
  id: number;
  product: string;
  producturl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  transferiD_M: number;
  transferiD_H: number;
}

export interface DeclarantMemInfo {
  id: number;
  name: string;
  taxid: string;
  phone: string;
  mobile: string;
  addr: string;
  idphotof: string;
  idphotob: string;
  appointment: string;
  transferiD_M: number;
  shippingiD_M: number;
}

export interface TransferNONInfo {
  id: number;
  stationcode: string;
  stationname: string;
  transferno: string;
  transfercompany: string;
  accountname: string;
  plength: string;
  pwidth: string;
  pheight: string;
  pweight: string;
  pvalueprice: string;
  status: string;
  remark: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface ShippingMInfo {
  id: number;
  accountid: number;
  accountcode: string;
  accountname: string;
  stationcode: string;
  stationname: string;
  shippingno: string;
  trackingno: string;
  trackingdesc: string;
  trackingmark: string;
  plength: string;
  pwidth: string;
  pheight: string;
  pweight: string;
  pvalueprice: string;
  mawbno: string;
  clearanceno: string;
  hawbno: string;
  total: string;
  totalprice: string;
  receiver: string;
  receiverphone: string;
  receiveraddr: string;
  ismultreceiver: string;
  trackingtype: string;
  status: string;
  paytype: string;
  paystatus: string;
  remark1: string;
  remark2: string;
  remark3: string;
  paydate: string;
  exportdate: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface ShippingHInfo {
  id: number;
  transferno: string;
  boxno: string;
  receiver: string;
  receiverphone: string;
  receiveraddr: string;
  remark: string;
  shippingiD_M: number;
}

export interface ShippingDInfo {
  id: number;
  product: string;
  producturl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  shippingiD_M: number;
  shippingiD_H: number;
}

/* ----- 廠商會員 ----- */

export interface ShippingMCusInfo {
  id: number;
  accountid: number;
  accountcode: string;
  companyname: string;
  stationcode: string;
  stationname: string;
  shippingno: string;
  trackingno: string;
  mawbno: string;
  clearanceno: string;
  hawbno: string;
  transferno: string;
  total: string;
  trackingtype: number;
  receiver: string;
  receiveraddr: string;
  receiverphone: string;
  ismultreceiver: string;
  status: string;
  remark1: string;
  remark2: string;
  remark3: string;
  paydate: string;
  exportdate: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface ShippingHCusInfo {
  id: number;
  boxno: string;
  receiver: string;
  receiveraddr: string;
  receiverphone: string;
  remark: string;
  shippingiD_M: number;
}

export interface ShippingDCusInfo {
  id: number;
  product: string;
  producturl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  shippingiD_M: number;
  shippingiD_H: number;
}

export interface DeclarantCusInfo {
  id: number;
  name: string;
  taxid: string;
  phone: string;
  mobile: string;
  addr: string;
  idphotof: string;
  idphotob: string;
  appointment: string;
  shippingiD_M: number;
}



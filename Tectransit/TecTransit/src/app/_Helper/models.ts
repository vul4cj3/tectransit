export interface MenuInfo {
  menuid: number;
  menucode: string;
  parentcode: string;
  menuname: string;
  menuurl: string;
  menuseq: number;
}

export interface AccountInfo {
  userid: number;
  usercode: string;
  username: string;
  userdesc: string;
  warehouseno: string;
  companyname: string;
  rateid: string;
  userseq: string;
  email: string;
  phone: string;
  mobile: string;
  address: string;
  taxid: string;
  idphoto_f: string;
  idphoto_b: string;
  logincount: string;
  lastlogin: string;
  credate: string;
  creby: string;
  upddate: string;
  updby: string;
}

export interface DeclarantInfo {
  rowid: number;
  id: number;
  type: number;
  name: string;
  taxid: string;
  phone: string;
  mobile: string;
  addr: string;
  idphotof: string;
  idphotob: string;
  appointment: string;
}

export interface MemStationInfo {
  stationid: number;
  stationcode: string;
  stationname: string;
  countrycode: string;
  username: string;
  warehouseno: string;
  receiver: string;
  phone: string;
  mobile: string;
  address: string;
}

export interface AboutCate {
  cateid: number;
  title: string;
  descr: string;
  istop: string;
}

export interface AboutInfo {
  cateid: number;
  catetitle: string;
  aboutid: number;
  title: string;
  descr: string;
  credate: string;
  upddate: string;
}

export interface BannerInfo {
  id: number;
  title: string;
  descr: string;
  imgurl: string;
  upsdate: string;
  upedate: string;
  istop: string;
  banseq: string;
}

export interface NewsInfo {
  newsid: number;
  title: string;
  descr: string;
  upsdate: string;
  upedate: string;
  istop: string;
  newsseq: string;
  credate: string;
  upddate: string;
}

export interface FaqCate {
  cateid: number;
  title: string;
  descr: string;
  istop: string;
}

export interface FaqInfo {
  faqid: number;
  title: string;
  descr: string;
  credate: string;
  upddate: string;
}

/* ----- 個人會員 ----- */

export interface TransferMInfo {
  id: number;
  accountid: number;
  accountcode: string;
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
  companyname: string;
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

/* ----- ImageUpload&FileUpload ----- */

export interface IDImgList {
  id: string;
  idphotof: string;
  idphotob: string;
}

export interface IDFileList {
  id: string;
  appointment: string;
}

/* ----- 廠商會員 ----- */

export interface ShippingMCusInfo {
  id: number;
  accountid: number;
  accountname: number;
  shippingno: string;
  mawbno: string;
  flightnum: string;
  total: string;
  totalweight: string;
  trackingtype: number;
  receiver: string;
  receiveraddr: string;
  receiverphone: string;
  ismultreceiver: string;
  status: number;
  remark1: string;
  remark2: string;
  remark3: string;
  mawbdate: string;
  paydate: string;
  exportdate: string;
  credate: string;
  createby: string;
  upddate: string;
  updby: string;
}

export interface ShippingHCusInfo {
  id: number;
  clearanceno: string;
  transferno: string;
  trackingno: string;
  depotstatus: number;
  receiver: string;
  receiveraddr: string;
  receiverphone: string;
  weight: string;
  totalitem: string;
  remark1: string;
  remark2: string;
  remark3: string;
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
  shippingiD_H: number;
}

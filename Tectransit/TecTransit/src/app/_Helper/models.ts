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

export interface TransferHInfo {
  transid: number;
  accountid: string;
  accountcode: string;
  stationcode: string;
  stationname: string;
  trasferno: string;
  trasfercompany: string;
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

export interface TransferDInfo {
  transid: number;
  product: string;
  producturl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  transhid: string;
}

export interface TransferDInfoCombine {
  transid: number;
  product: string;
  producturl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  transhid: string;
  trasferno: string;
}

export interface ShippingHInfo {
  shippingid: number;
  shippingno: string;
  stationcode: string;
  trackingno: string;
  trackingdesc: string;
  trackingmark: string;
  plength: string;
  pwidth: string;
  pheight: string;
  pweight: string;
  ptrackingno: string;
  total: string;
  receiver: string;
  receiver_addr: string;
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

export interface ShippingDInfo {
  shippingid: number;
  packname: string;
  packurl: string;
  unitprice: string;
  quantity: string;
  remark: string;
  shippingid_h: string;
}

export interface DeclarantInfo {
  id: number;
  type: string;
  name: string;
  taxid: string;
  phone: string;
  mobile: string;
  addr: string;
  idphotO_F: string;
  idphotO_B: string;
  appointment: string;
}

export interface IDImgList {
  id: string;
  idphotof: string;
  idphotob: string;
}

export interface IDFileList {
  id: string;
  appointment: string;
}

export interface ShippingMCusInfo {
  id: number;
  accountid: number;
  stationcode: string;
  stationname: string;
  shippingno: string;
  trackingno: string;
  mawbno: string;
  trasferno: string;
  total: string;
  trackingtype: number;
  receiver: string;
  receiveraddr: string;
  ismultreceiver: string;
  status: number;
  remark1: string;
  remark2: string;
  remark3: string;
  paydate: string;
  exportdate: string;
  credate: string;
  createby: string;
  upddate: string;
  updby: string;
}

export interface ShippingHCusInfo {
  id: number;
  boxno: string;
  receiver: string;
  receiveraddr: string;
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

export interface AccountInfo {
  userid: number;
  usercode: string;
  username: string;
  userdesc: string;
  warehouseno: string;
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

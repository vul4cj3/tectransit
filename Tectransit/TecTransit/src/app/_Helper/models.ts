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

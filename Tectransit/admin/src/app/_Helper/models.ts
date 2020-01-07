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

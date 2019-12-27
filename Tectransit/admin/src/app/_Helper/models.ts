export interface MenuInfo {
  menucode: string;
  parentcode: string;
  menuurl: string;
  menuname: string;
  iconurl: string;
  haspower: string;
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

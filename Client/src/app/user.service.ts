import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface UserName {
  display: string;
  first: string;
  last: string;
}
export interface UserAddress {
  street: string;
  city: string;
  country: string;
  postalCode: string;
}
export interface UserBase {
  name: UserName;
  address: UserAddress;
  mail: string;
}
export interface UserData extends UserBase {
  password: string;
  id: number;
}

export class UserCreds {
  password: string = ""
  id: number = 0
  constructor(pwd: string = "", id: number = 0){
    this.id = id;
    this.password = pwd;
  }
}

export class UserStats {
  userID: number = 0;
  rating: number = 0;
  ratingCount: number = 0;
  description: string = "Empty";
  avatarURL: string = "https://demos.creative-tim.com/argon-dashboard-angular/assets/img/theme/team-4-800x800.jpg";
  headline: string = "Empty Profile";

  constructor() { }
}

export interface USerFetchResult {
  user: UserData;
  stats: UserStats;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  static hasLocalUser() {
    var id = sessionStorage.getItem("user-id");
    var password = sessionStorage.getItem("user-password");

    return id != null && password != null;
  }
  static getLocalUser() {
    var id = sessionStorage.getItem("user-id");
    var password = sessionStorage.getItem("user-password");

    var creds: UserCreds = new UserCreds();
    if (id != null)
      creds.id = +id;
    else
      creds.id = 0;
    if (password != null)
      creds.password = password;
    else
      creds.password = "";

    return creds;
  }
  static setLocalUser(creds: UserCreds) {
    sessionStorage.setItem("user-id", creds.id.toString());
    sessionStorage.setItem("user-password", creds.password);
  }

  getUserStats(id: number) {
    const url = `/api/User/stats/${id}/`;
    return this.http.get<UserStats>(url, { observe: 'response' });
  }
  getUserPublic(id: number) {
    const url = `/api/User/public/${id}`;
    return this.http.get<UserData>(url, { observe: 'response' });
  }
  getUser(creds: UserCreds) {
    const url = `/api/User/get/${creds.id}/${creds.password}`;
    return this.http.get<UserData>(url, { observe: 'response' });
  }
  fetchUser(id: number) {
    const url = `/api/User/fetch/${id}`;
    return this.http.get<USerFetchResult>(url, { observe: 'response' });
  }
  registerUser(base: UserBase, password: string) {
    const url = `/api/User/create/${password}`;
    return this.http.post<UserData>(url, base, { observe: 'response' });
  }
  loginUser(mail: string, password: string) {
    const url = `/api/User/find/${mail}/${password}`;
    return this.http.get<UserData>(url, { observe: 'response' });
  }

  loginWithLocal() {
    return this.getUser(UserService.getLocalUser());
  }

  static createEmptyUser(): UserData {
    // Main St, Queens, NY 11367, Vereinigte Staaten
    const user: UserData = {
      id: 0, password: "", mail: "sample@mail.tld",
      address: { street: "Main St, 1a", city: "New Your", postalCode: "11367", country: "USA" },
      name: {display: "JohnDoe432", first: "John", last: "Doe"}
    };

    return user;
  }
}

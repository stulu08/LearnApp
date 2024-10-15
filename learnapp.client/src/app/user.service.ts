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
  number: string;
  city: string;
  country: string;
  postal: Number;
}
export interface UserContact {
  phone: string;
  mail: string;
}
export interface UserBase {
  name: UserName;
  address: UserAddress;
  contact: UserContact;
}
export interface UserData extends UserBase {
  id: number;
}

@Injectable({
  providedIn: 'root'
})

export class UserService {
  constructor(private http: HttpClient) { }

  getUser(id: number): Observable<UserData> {
    const url = `/user/get?id=${id}`; 
    return this.http.get<UserData>("/user/get");
  }
}

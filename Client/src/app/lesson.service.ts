import { HttpClient } from '@angular/common/http';
import { Injectable, Optional } from '@angular/core';
import { UserData, UserService, UserStats } from './user.service';
import { SafeUrl } from '@angular/platform-browser';

export interface ImageUploadResult {
  path: string;
}

export class SubjectSelectors {
  name: string = "";
  value: Subject | undefined;

  static getSelectors() {
    var subjects: SubjectSelectors[] = [];

    for (let i = 0; i < Subject.Max; i++) {
      var sub: Subject = i as Subject;
      subjects.push({ name: SubjectSelectors.SubjectToString(sub), value: sub });
    }
    return subjects;
  }
  static SubjectToString(subject: Subject): string {
    switch (subject) {
      case Subject.German:
      case Subject.English:
      case Subject.Math:
      case Subject.Chemistry:
      case Subject.Biology:
      case Subject.Physics:
      case Subject.History:
      case Subject.Politics:
        return Subject[subject];
      case Subject.ComputerScience:
        return "Computer Science";
      default:
        return "Invalid";
    }
  }
}

export enum Subject {
  German, English, Math,
  Chemistry, Biology, Physics,
  ComputerScience, History, Politics,
  Max
}

export interface Lesson {
  id: number;
  user: number;
  titel: string;
  lesson: Subject;
  description: string;
  tags: string[];
  duration: number;
  price: number;
  rating: number;
}
export interface LessonFetchResponse {
  lesson: Lesson;
  user: UserData;
  stats: UserStats;
}

@Injectable({
  providedIn: 'root'
})
export class LessonService {
  constructor(private http: HttpClient) { }

  getLesson(id: number) {
    const url = `/api/Lesson/get/${id}`;
    return this.http.get<Lesson>(url, { observe: 'response' });
  }
  fetchLesson(id: number) {
    const url = `/api/Lesson/fetch/${id}`;
    return this.http.get<LessonFetchResponse>(url, { observe: 'response' });
  }
  createLesson(baseLesson: Lesson, password: string) {
    const url = `/api/Lesson/create/${password}`;
    return this.http.post<Lesson>(url, baseLesson, { observe: 'response' });
  }
  suggestLessons(max: number, offset: number) {
    const url = `/api/Lesson/suggest/${UserService.getLocalUser().id}/${offset}/${max}`;
    return this.http.get<Lesson[]>(url, { observe: 'response' });
  }
  getUserLessons(max: number, offset: number, user: number) {
    const url = `/api/Lesson/user/${user}/${offset}/${max}`;
    return this.http.get<Lesson[]>(url, { observe: 'response' });
  }
  search(query: string, max: number, offset: number) {
    const url = `/api/Lesson/search/${query}/${UserService.getLocalUser().id}/${offset}/${max}`;
    return this.http.get<Lesson[]>(url, { observe: 'response' });
  }
  uploadTempImg(file: File) {
    var creds = UserService.getLocalUser();

    const url = `api/Lesson/thumbnail/push/${creds.id}/${creds.password}`;
    const formData = new FormData();
    formData.append('file', file, file.name);
    
    return this.http.post<ImageUploadResult>(url, formData, { observe: 'response' });
  }
  getTempImg(id: number) {
    const url = `api/Lesson/thumbnail/temp/${id}`;
    return this.http.get(url, { responseType: 'blob' });
  }
  getImage(id: number) {
    const url = `api/Lesson/thumbnail/get/${id}`;
    return this.http.get(url, { responseType: 'blob' });
  }
}

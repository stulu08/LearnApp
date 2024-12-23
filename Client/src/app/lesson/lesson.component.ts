import { Component, OnInit } from '@angular/core';
import { Lesson, LessonService, Subject, SubjectSelectors } from '../lesson.service';
import { UserData, UserService, UserStats } from '../user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { LoadingService } from '../loading.service';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-lesson',
  templateUrl: './lesson.component.html',
  styleUrl: './lesson.component.css'
})
export class LessonComponent implements OnInit {

  lesson: Lesson = {
    id: 0,
    user: 0,
    titel: 'Invalid Lesson',
    lesson: Subject.ComputerScience,
    description:
      "The lesson was not loaded correctly, maybe you didnt specify an lesson in the url?",
    tags: ['Error', 'Invalid', 'Lesson'],
    duration: 1, // Duration in hours
    price: 0,
    rating: 0
  };
  user: UserData = UserService.createEmptyUser();
  userStats: UserStats = new UserStats();

  lessonImage: SafeUrl = "https://www.studis-online.de/Studienfuehrer/Bilder/physik1200x600.jpg";

  constructor(private lessonService: LessonService, private uService: UserService, private route: ActivatedRoute,
    private router: Router, private sanitizer: DomSanitizer, private loading: LoadingService) {

  }
  ngOnInit(): void {
    this.loading.showLoading();
    this.route.queryParams.subscribe(result => {
      if (result["id"] != undefined) {
        this.loadLesson(result["id"]);
      } else {
        this.router.navigate(["404"]);
      }
    });
  }

  loadLesson(id: number) {
    this.lessonService.fetchLesson(id).subscribe(result => {
      if (result.body) {
        this.lesson = result.body.lesson;
        this.user = result.body.user;
        this.userStats = result.body.stats;
        this.loadImage();
      } else {
        this.loading.hideLoading()
      }
    });
  }

  loadImage() {
    this.lessonService.getImage(this.lesson.id).subscribe({
      next: (blob) => {
        const objectURL = URL.createObjectURL(blob);
        this.lessonImage = this.sanitizer.bypassSecurityTrustUrl(objectURL); // Secure the blob URL for Angular
        this.loading.hideLoading()
      },
      error: (err) => {
        this.loading.hideLoading()
        console.log("Could not load lesson image");
      }
    });
  }

  getSubjectName(subject: Subject): string {
    return SubjectSelectors.SubjectToString(subject);
  }
}

import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Lesson, LessonService, Subject, SubjectSelectors } from '../lesson.service';
import { UserService, UserStats } from '../user.service';


@Component({
  selector: 'lesson-view',
  templateUrl: './lesson-view.component.html',
  styleUrl: './lesson-view.component.css',
})
export class LessonViewComponent implements OnInit {
  @Input({ required: true }) lesson!: Lesson;
  @Input() stats: UserStats | undefined;
  

  image: SafeUrl = "assets/images/loading.gif";

  constructor(private lessonService: LessonService, private uService: UserService, private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    if (this.stats == undefined) {
      this.uService.getUserStats(this.lesson.user).subscribe(result => {
        if (result.body) {
          this.stats = result.body;
        }
      });
    }

    this.lessonService.getImage(this.lesson.id).subscribe(result => {
      const objectURL = URL.createObjectURL(result);
      this.image = this.sanitizer.bypassSecurityTrustUrl(objectURL); // Secure the blob URL for Angular
    })
  }




  getSubjectName(subject: Subject): string {
    return SubjectSelectors.SubjectToString(subject);
  }
}

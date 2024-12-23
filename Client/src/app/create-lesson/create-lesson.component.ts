import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserData, UserService, UserStats } from '../user.service';
import { Lesson, LessonService, Subject, SubjectSelectors } from '../lesson.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { MessageService } from 'primeng/api';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { LoadingService } from '../loading.service';

interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrl: './create-lesson.component.css'
})

export class CreateLessonComponent {
  lessonForm: FormGroup;
  tempImageURL: SafeUrl | undefined;

  subjectsSelector = SubjectSelectors.getSelectors();
  selectedSubject: SubjectSelectors | undefined;
  user: UserData = UserService.createEmptyUser();
  userStats: UserStats = new UserStats();

  constructor(private uService: UserService, private router: Router, private lService: LessonService, private loading: LoadingService,
    private fb: FormBuilder, private messageService: MessageService, private sanitizer: DomSanitizer) {
    this.lessonForm = this.fb.group({
      titel: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
      subject: ['', [Validators.required]],
      description: ['', [Validators.required]],
      tags: ['', [Validators.required]],
      duration: ['', [Validators.required, Validators.min(10),Validators.max(300)]],
      price: ['', [Validators.required, Validators.min(5)]],
    });
  }

  ngOnInit() {
    this.loading.showLoading();

    if (UserService.hasLocalUser()) {
      this.uService.fetchUser(UserService.getLocalUser().id).pipe(catchError(e => {
        this.router.navigate(["home"]);
        return throwError(() => e);
      })).subscribe(result => {
        if (result.body) {
          this.user = result.body.user;
          this.userStats = result.body.stats;
          this.loading.hideLoading();
        } else {
          this.router.navigate(["home"]);
        }
      });
    } else {
      this.router.navigate(["home"]);
    }

  }

  onPhotoUpload(event: any) {
    if (event.files.length < 1) {
      this.messageService.add({ severity: 'error', summary: `No file`, detail: "No file specified!" })
      return;
    }

    this.lService.uploadTempImg(event.files[0]).subscribe(result => {
      this.lService.getTempImg(this.user.id).subscribe({
        next: (blob) => {
          const objectURL = URL.createObjectURL(blob);
          this.tempImageURL = this.sanitizer.bypassSecurityTrustUrl(objectURL); // Secure the blob URL for Angular
        },
        error: (err) => {
          this.messageService.add({ severity: 'error', summary: `Upload failed`, detail: err })
        }
      }
      );
    });
  }

  onSubmit() {
    if (!this.lessonForm.valid) {
      this.messageService.add({ severity: 'error', summary: `Missing data`, detail: "You need to fill out all required fields!" })
      console.log(this.lessonForm);
      return;
    }
    if (this.tempImageURL == undefined) {
      this.messageService.add({ severity: 'error', summary: `Missing data`, detail: "You need to upload an image!" })
      return;
    }
    var lessonData: Lesson = {
      id: 0,
      user: this.user.id,
      titel: this.lessonForm.value.titel,
      lesson: this.lessonForm.value.subject.value,
      description: this.lessonForm.value.description,
      tags: this.lessonForm.value.tags,
      duration: this.lessonForm.value.duration,
      price: this.lessonForm.value.price,
      rating: 0
    }

    this.lService.createLesson(lessonData, UserService.getLocalUser().password)
      .pipe(catchError(error => this.handleError(error, "Creating lesson")))
      .subscribe(result => {
        if (result.body == null) {
          this.messageService.add({ severity: 'error', summary: `Creating failed` })
          return;
        }
        this.messageService.add({ severity: 'success', summary: 'Successfully created component' });
        this.router.navigate(["lesson"], { queryParams: { id: result.body.id} });
      }
    );

  }

  handleError(error: HttpErrorResponse, name: string) {
    this.messageService.add({ severity: 'error', summary: `${name} failed`, detail: error.error });
    return throwError(() => error);
  }
}

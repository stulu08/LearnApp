import { Component, OnInit } from '@angular/core';
import { Lesson, LessonService, Subject, SubjectSelectors } from '../lesson.service';
import { UserData, UserService, UserStats } from '../user.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../loading.service';
import { SelectItem } from 'primeng/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-suggest',
  templateUrl: './suggest.component.html',
  styleUrl: './suggest.component.css'
})
export class SuggestComponent implements OnInit {
  search: string | undefined;
  searchSubmitted: string | undefined;

  constructor(private lessonService: LessonService, private loading: LoadingService,
    private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.loading.showLoading();

    this.route.queryParams.subscribe(result => {
      if (result["search"] != undefined) {
        this.search = result["search"];
        this.searchSubmitted = result["search"];
      }
    });
  }


  searchBtnClick() {
    this.loading.showLoading();

    this.router.navigate(['suggest'], { queryParams: { search: this.search } });
    this.searchSubmitted = this.search;

  }

  getSubjectName(subject: Subject): string {
    return SubjectSelectors.SubjectToString(subject);
  }
}

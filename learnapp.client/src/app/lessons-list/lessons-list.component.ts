import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Lesson, LessonService } from '../lesson.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../loading.service';
import { UserData, UserService, UserStats } from '../user.service';
import { count } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'lessons-list',
  templateUrl: './lessons-list.component.html',
  styleUrl: './lessons-list.component.css'
})
export class LessonsListComponent implements OnInit, OnChanges {
  @Input() count: number = 10;
  @Input() grid: boolean = true;
  @Input() search: string | undefined;
  @Input() moreBtn: boolean = false;
  @Input() increment: number = 10;

  lessons: Lesson[];
  loadOffset: number = 0;

  canLoadMore: boolean = true;

  constructor(private uService: UserService, private route: ActivatedRoute, private router: Router,
    private loading: LoadingService, private lessonService: LessonService) {
    this.lessons = [];
  }


  ngOnInit(): void {
  }

  loadLessons(ammount: number) {
    if (ammount < 1) {
      this.loading.hideLoading()
      return;
    }

    console.log(`loading ${ammount} more lessons`);

    if (this.search) {
      console.log("seaching for " + this.search);
      this.lessonService.search(this.search, ammount, this.loadOffset).subscribe(result => this.addLessons(result));
      return;
    }

    this.lessonService.suggestLessons(ammount, this.loadOffset).subscribe(result => this.addLessons(result));
  }


  addLessons(result: HttpResponse<Lesson[]>) {
    if (result.body) {
      for (let i: number = 0; i < result.body.length; i++) {
        this.lessons.push(result.body[i]);
        this.loadOffset++;
      }
      this.loading.hideLoading()
    }
    if (this.loadOffset != this.count)
      this.canLoadMore = false;
  }

  ngOnChanges(changes: SimpleChanges) {

    const searchChange = changes['search'];
    if (searchChange) {
      this.loadOffset = 0;
      this.lessons = [];
      this.canLoadMore = true;
      this.loadLessons(this.count);
      return;
    }

    const countChange = changes['count'];
    if (countChange) {
      this.loadLessons(this.count - this.loadOffset);
      return;
    }

    
  }
  moreBtnClick() {
    this.loadLessons(this.increment);
  }

}

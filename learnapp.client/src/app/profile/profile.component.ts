import { Component, OnInit } from '@angular/core';
import { UserService, UserData, UserStats } from '../user.service';
import { RatingModule } from 'primeng/rating';
import { AvatarModule } from 'primeng/avatar'
import { AvatarGroupModule } from 'primeng/avatargroup';
import { queueScheduler } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../loading.service';
import { Lesson, LessonService } from '../lesson.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  user: UserData = UserService.createEmptyUser();
  userStats: UserStats = new UserStats();
  canLoadMore: boolean = true;
  lessons: Lesson[];

  constructor(private uService: UserService, private route: ActivatedRoute, private router: Router,
    private loading: LoadingService, private lessonService: LessonService) {
    this.lessons = [];
  }

  ngOnInit() {
    this.loading.showLoading();
    this.route.queryParams.subscribe(result => {
      if (result["id"] != undefined) {
        this.loadProfile(result["id"]);
      }
    });
  }


  loadProfile(idString: string) {
    var id: number;

    console.log("Requesting user public info and stats from user " + idString)

    if (idString != null) {
      id = +idString;
    }
    else {
      this.router.navigate(["404"]);
      return;
    }

    if (id == 0) {
      this.router.navigate(["404"]);
      return;
    }


    this.uService.fetchUser(id).subscribe(result => {
      if (result.body) {
        this.user = result.body.user;
        this.userStats = result.body.stats;
        this.loadLessons(10);
      }
    });
  }

  loadLessons(count: number) {
    this.loading.showLoading();
    var offset = this.lessons.length;

    this.lessonService.getUserLessons(count, offset, this.user.id).subscribe(result => {
      if (result.body) {
        for (let i: number = 0; i < result.body.length; i++) {
          this.lessons.push(result.body[i]);
        }
        this.loading.hideLoading()
      }

      if (this.lessons.length != (offset + count))
        this.canLoadMore = false;
    });
  }
}

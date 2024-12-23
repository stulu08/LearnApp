import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../loading.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  lessonSearch: string = "";

  constructor(private loading: LoadingService, private router: Router) {
    this.loading.showLoading();
  }

  search() {
    this.router.navigate(['suggest'], { queryParams: { search: this.lessonSearch } });
  }
}

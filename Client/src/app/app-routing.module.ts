import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ProfileComponent } from './profile/profile.component';
import { LessonComponent } from './lesson/lesson.component';
import { CreateLessonComponent } from './create-lesson/create-lesson.component';
import { SuggestComponent } from './suggest/suggest.component';

const routes: Routes = [
  { path: "", redirectTo: "home", pathMatch: 'full' },
  { path: "home", component: HomeComponent },
  { path: "profile", component: ProfileComponent },
  { path: "register", component: RegisterComponent },
  { path: "login", component: RegisterComponent },
  { path: "lesson", component: LessonComponent },
  { path: "create", component: CreateLessonComponent },
  { path: "suggest", component: SuggestComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

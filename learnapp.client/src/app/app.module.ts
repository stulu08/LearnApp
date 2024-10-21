import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { TabMenuModule } from 'primeng/tabmenu';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { DataViewModule } from 'primeng/dataview';
import { provideRouter } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CardModule } from 'primeng/card';
import { ReactiveFormsModule } from '@angular/forms';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { RatingModule } from 'primeng/rating';
import { MessageService } from 'primeng/api';
import { AvatarModule } from 'primeng/avatar';
import { PanelModule } from 'primeng/panel';
import { LessonComponent } from './lesson/lesson.component';
import { CreateLessonComponent } from './create-lesson/create-lesson.component';
import { InputMaskModule } from 'primeng/inputmask';
import { InputNumberModule } from 'primeng/inputnumber';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { ChipsModule } from 'primeng/chips';
import { EditorModule } from 'primeng/editor';
import { FileUploadModule } from 'primeng/fileupload';
import { LoadingComponent } from './loading/loading.component';
import { SuggestComponent } from './suggest/suggest.component';
import { CommonModule } from '@angular/common';
import { LessonViewComponent } from './lesson-view/lesson-view.component';
import { LessonsListComponent } from './lessons-list/lessons-list.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    RegisterComponent,
    LessonComponent,
    CreateLessonComponent,
    LoadingComponent,
    SuggestComponent,
    LessonViewComponent,
    LessonsListComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, PanelModule, InputMaskModule, InputNumberModule,
    AppRoutingModule, BrowserAnimationsModule, TabMenuModule, RatingModule,
    CarouselModule, ButtonModule, TagModule, DataViewModule, FloatLabelModule,
    CardModule, ReactiveFormsModule, MessageModule, ToastModule, AvatarModule,
    DropdownModule, FormsModule, ChipsModule, EditorModule, FileUploadModule,
    CommonModule,
  ],
  providers: [provideAnimationsAsync(), provideAnimations(), MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }

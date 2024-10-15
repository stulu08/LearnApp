import { Component, OnInit, model } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { TabMenuModule } from 'primeng/tabmenu';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})

export class AppComponent implements OnInit {

  public items: MenuItem[] = [
    { label: 'Home', routerLink: "home", icon: 'pi pi-home' },
    { label: "Profile", routerLink: "profile", icon: 'pi pi-user' },
    { label: 'Tasks', icon: 'pi pi-book' },
    { label: 'Management', icon: 'pi pi-building' }
  ]

  constructor(private uService: UserService) {}

  ngOnInit() {
    this.uService.getUser(3).subscribe((result) => {
      this.items[1].label = result.name.display;
    })
  }
  title = 'learnapp.client';
}

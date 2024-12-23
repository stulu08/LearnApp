import { Component, OnInit, model } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { TabMenu } from 'primeng/tabmenu';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})

export class AppComponent implements OnInit {
  public items: MenuItem[] = [
    { label: 'Home', routerLink: "home", icon: 'pi pi-home' },
    { label: 'Sign In/Create Account', routerLink: "login", icon: 'pi pi-user' },
  ]

  constructor(private uService: UserService) {
  }


  ngOnInit() {
    if (!UserService.hasLocalUser()) {
      console.warn("Not logged in!")
      return;
    }
    this.uService.loginWithLocal().subscribe(result => {
      if (result.body?.name.display != undefined) {
        console.log("Logged in as " + result.body?.name.display)

        this.items = [
          { label: 'Home', routerLink: "home", icon: 'pi pi-home' },
          { label: 'Lessons', routerLink: "suggest", icon: 'pi pi-book' },
          { label: 'Teach', routerLink: "create", icon: 'pi pi-building' },
          {
            label: result.body?.name.display, routerLink: "profile",
            queryParams: { "id": result.body?.id }, icon: 'pi pi-user',
          },
        ];
      }
    });

  }
  title = 'learnapp.client';
}

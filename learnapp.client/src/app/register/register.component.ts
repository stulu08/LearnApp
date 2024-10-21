import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { catchError, throwError } from 'rxjs';
import { UserBase, UserCreds, UserService } from '../user.service';
import { LoadingService } from '../loading.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  userForm: FormGroup;
  registerMode: boolean = false;
  
  constructor(private fb: FormBuilder, private router: Router, private messageService: MessageService,
    private userService: UserService, private loading: LoadingService) {
    this.registerMode = location.href.toLowerCase().includes("register");

    if (this.registerMode) {
      this.userForm = this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(32)]],
        mail: ['', [Validators.required, Validators.email, Validators.minLength(6), Validators.maxLength(50)]],
        display: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
        first: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
        last: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
        street: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
        city: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
        country: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]],
        postalCode: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]]
      });
    }
    else {
      this.userForm = this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(32)]],
        mail: ['', [Validators.required, Validators.email, Validators.minLength(6), Validators.maxLength(50)]],
        display: [''],
        first: [''],
        last: [''],
        street: [''],
        city: [''],
        country: [''],
        postalCode: ['']
      });
    }

  }
  getErrorMessage(controlName: string): string {
    const control = this.userForm.get(controlName);

    if (control?.hasError('required')) {
      return `Field is required.`;
    }
    if (control?.hasError('minlength')) {
      const requiredLength = control.getError("minlength").requiredLength;
      return `Must be at least ${requiredLength} characters long.`;
    }
    if (control?.hasError('maxlength')) {
      const requiredLength = control.getError("maxlength").requiredLength;
      return `Cannot be longer than ${requiredLength} characters.`;
    }
    if (control?.hasError('email')) {
      return `Please enter a valid email address.`;
    }

    return "Invalid value";
  }
  
  onSwitch() {
    if (this.registerMode) {
      this.router.navigate(["/login"]);
    } else {
      this.router.navigate(["/register"]);
    }
  }
  onSubmit() {
    if (this.userForm.valid) {
      if (this.registerMode) {
        this.register();
      } else {
        this.login();
      }
    } else {
      this.messageService.add({ severity: 'error', summary: this.registerMode ? 'Register failed' : 'Login failed', detail: "You need to fille out every field!" });

      console.log('Form is invalid');
    }
  }
  handleError(error: HttpErrorResponse, name : string) {
    this.messageService.add({ severity: 'error', summary: `${name} failed`, detail: error.error });
    this.loading.hideLoading();
    return throwError(() => error);
  }

  login() {
    var mail = this.userForm.value.mail;
    var password = this.userForm.value.password;

    this.loading.showLoading();
    this.userService.loginUser(mail, password).pipe(catchError(error => this.handleError(error, "Login"))).subscribe(
      result => {
        if (result.body == null) {
          this.messageService.add({ severity: 'error', summary: 'Login failed' });
          this.loading.hideLoading();
          return;
        }

        this.messageService.add({ severity: 'success', summary: 'Successfully logged in' });

        UserService.setLocalUser({ password: password, id: result.body.id })
        window.location.href = window.location.origin;
      });
  }
  register() {
    const userData: UserBase = {
      name: {
        display: this.userForm.value.display,
        first: this.userForm.value.first,
        last: this.userForm.value.last
      },
      address: {
        street: this.userForm.value.street,
        city: this.userForm.value.city,
        country: this.userForm.value.country,
        postalCode: this.userForm.value.postalCode
      },
      mail: this.userForm.value.mail,
    };
    var password = this.userForm.value.password;

    this.loading.showLoading();

    this.userService.registerUser(userData, password).pipe(catchError(error => this.handleError(error, "Registration"))).subscribe(result => {
      if (result.body == null) {
        this.messageService.add({ severity: 'error', summary: 'Registration error' });
        this.loading.hideLoading();
        return;
      }
      this.messageService.add({ severity: 'success', summary: 'Successfully registered' });

      UserService.setLocalUser({ password: password, id: result.body.id })
      window.location.href = window.location.origin;
    });
  }
}

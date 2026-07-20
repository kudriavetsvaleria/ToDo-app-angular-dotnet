import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  errorMessage = '';

  registerForm = new FormGroup({
    username: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    const credentials = this.registerForm.getRawValue();

    this.authService.register(credentials).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        this.authService.saveUsername(response.username);
        this.router.navigate(['/tasks']);
      },
      error: (error) => {
        this.errorMessage = error.status === 409
          ? 'Це ім\'я користувача вже зайняте'
          : 'Не вдалося зареєструватися';
      },
    });
  }
}

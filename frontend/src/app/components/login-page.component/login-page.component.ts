import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { TokenStorageService } from '../../services/token-storage.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

export interface LoginResponse {
  token: string;
}

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export class LoginPageComponent implements OnInit {
  form: any = {
    username: null,
    password: null
  };
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.authService.isLoggedIn = true;
      this.router.navigate([this.authService.redirectUrl]);
    }
  }

  onSubmit(): void {
    const { username, password } = this.form;
    
    // Validate form
    if (!username || !password) {
      this.errorMessage = 'Please enter username and password';
      return;
    }

    this.errorMessage = ''; // Clear previous errors
    
    this.http.post<LoginResponse>("https://localhost:7198/api/Login/login", { username, password })
      .subscribe({
        next: (data) => {
          // Save the token from the response
          this.tokenStorage.saveToken(data.token);
          
          // Decode the JWT to get the user ID
          const payload = this.decodeToken(data.token);
          if (payload && payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']) {
            const userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
            this.tokenStorage.saveUser(userId);
          }
          
          this.authService.isLoggedIn = true;
          this.router.navigate([this.authService.redirectUrl]);
          window.location.reload();
        },
        error: (error) => {
          console.error('Login failed', error);
          
          // Show specific error message from backend
          if (error.status === 404) {
            this.errorMessage = 'User not found. Please check your username.';
          } else if (error.status === 401) {
            this.errorMessage = 'Invalid password. Please try again.';
          } else {
            this.errorMessage = 'Login failed. Please try again.';
          }
        }
      });
  }

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  
}
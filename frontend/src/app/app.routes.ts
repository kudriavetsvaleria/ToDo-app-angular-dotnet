import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { TaskListComponent } from './features/tasks/task-list/task-list.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'tasks', component: TaskListComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];

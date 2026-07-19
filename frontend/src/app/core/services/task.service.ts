import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Task, PagedResult, CreateTaskRequest } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = `${environment.apiUrl}/tasks`;

  constructor(private http: HttpClient) { }

  getTasks(page: number, pageSize: number, search: string | null, categoryId: number | null): Observable<PagedResult<Task>> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }
    if (categoryId !== null) {
      params = params.set('categoryId', categoryId);
    }

    return this.http.get<PagedResult<Task>>(this.apiUrl, { params });
  }

  createTask(request: CreateTaskRequest): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, request);
  }
}

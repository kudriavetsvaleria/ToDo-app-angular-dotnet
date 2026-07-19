import { Component, OnInit, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../../core/services/task.service';
import { CategoryService } from '../../../core/services/category.service';
import { Task, CreateTaskRequest } from '../../../core/models/task.model';
import { Category } from '../../../core/models/category.model';

@Component({
  selector: 'app-task-list',
  imports: [DatePipe, ReactiveFormsModule],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  categories: Category[] = [];
  selectedCategoryId: number | null = null;
  isPanelOpen = false;

  page = 1;
  pageSize = 8;
  totalPages = 0;
  totalCount = 0;

  allCount = 0;
  categoryCounts: Record<number, number> = {};

  today = new Date();

  private fb = inject(FormBuilder);

  form = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    dueDate: [''],
    categoryId: [null as number | null],
  });

  constructor(
    private taskService: TaskService,
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.loadTasks();

    this.categoryService.getCategories().subscribe((categories) => {
      this.categories = categories;
      this.loadCounts();
    });
  }

  loadTasks(): void {
    this.taskService.getTasks(this.page, this.pageSize, null, this.selectedCategoryId).subscribe((result) => {
      this.tasks = result.items;
      this.totalPages = result.totalPages;
      this.totalCount = result.totalCount;
    });
  }

  loadCounts(): void {
    this.taskService.getTasks(1, 1, null, null).subscribe((result) => {
      this.allCount = result.totalCount;
    });

    this.categories.forEach((category) => {
      this.taskService.getTasks(1, 1, null, category.id).subscribe((result) => {
        this.categoryCounts[category.id] = result.totalCount;
      });
    });
  }

  selectCategory(categoryId: number | null): void {
    this.selectedCategoryId = categoryId;
    this.page = 1;
    this.loadTasks();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }
    this.page = page;
    this.loadTasks();
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  openPanel(): void {
    this.form.reset({ categoryId: this.selectedCategoryId });
    this.isPanelOpen = true;
  }

  closePanel(): void {
    this.isPanelOpen = false;
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    const value = this.form.value;
    const request: CreateTaskRequest = {
      title: value.title!,
      description: value.description || null,
      dueDate: value.dueDate || null,
      categoryId: value.categoryId ? Number(value.categoryId) : null,
    };

    this.taskService.createTask(request).subscribe(() => {
      this.page = 1;
      this.loadTasks();
      this.loadCounts();
      this.closePanel();
    });
  }

}

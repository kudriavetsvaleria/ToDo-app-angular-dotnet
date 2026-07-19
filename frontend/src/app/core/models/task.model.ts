export interface Task {
  id: number;
  title: string;
  description: string | null;
  isCompleted: boolean;
  dueDate: string | null;
  createdAt: string;
  categoryId: number | null;
  categoryName: string | null;
}

export interface CreateTaskRequest {
  title: string;
  description: string | null;
  dueDate: string | null;
  categoryId: number | null;
}

export interface UpdateTaskRequest {
  title: string;
  description: string | null;
  isCompleted: boolean;
  dueDate: string | null;
  categoryId: number | null;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

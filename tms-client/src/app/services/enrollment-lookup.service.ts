import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface StudentOption {
  id: number;
  registrationNumber: string;
  name: string;
}

export interface CourseOption {
  id: number;
  code: string;
  title: string;
}

export interface EnrollPayload {
  studentId: number;
  courseId: number;
  term: string;
  notes?: string;
  backupCourses?: string[];
}

interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

@Injectable({
  providedIn: 'root'
})
export class EnrollmentLookupService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5188/api';

  getStudents(): Observable<StudentOption[]> {
    return this.http.get<StudentOption[]>(`${this.apiUrl}/v2/students`);
  }

  getCourses(): Observable<CourseOption[]> {
    return this.http.get<PagedResponse<CourseOption>>(`${this.apiUrl}/courses?pageSize=50`).pipe(
      map(res => res.items)
    );
  }

  submitEnrollment(payload: EnrollPayload): Observable<unknown> {
    return this.http.post(
      `${this.apiUrl}/v2/courses/${payload.courseId}/enrollments`,
      payload
    );
  }
}
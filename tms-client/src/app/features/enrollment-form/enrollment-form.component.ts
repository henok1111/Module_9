import { Component, OnInit, inject, signal } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  Validators,
  ReactiveFormsModule,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import { 
  EnrollmentLookupService, 
  StudentOption, 
  CourseOption 
} from "../../services/enrollment-lookup.service";

@Component({
  selector: "app-enrollment-form",
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: "./enrollment-form.component.html",
  styleUrl: "./enrollment-form.component.scss",
})
export class EnrollmentFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private lookupService = inject(EnrollmentLookupService);

  students = signal<StudentOption[]>([]);
  courses = signal<CourseOption[]>([]);

  submitted = signal(false);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  form = this.fb.nonNullable.group({
    studentId: ["", Validators.required],
    courseId: ["", Validators.required],
    term: ["Fall 2026", Validators.required],
    notes: [""],
    backupCourses: this.fb.array<FormControl<string>>([]),
  });

  ngOnInit(): void {
    this.lookupService.getStudents().subscribe({
      next: (data) => this.students.set(data),
      error: (err) => {
        console.error("Students error:", err);
        this.errorMessage.set("Failed to load students list.");
      }
    });

    this.lookupService.getCourses().subscribe({
      next: (data) => {
        this.courses.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error("Courses error:", err);
        this.errorMessage.set("Failed to load courses list.");
        this.isLoading.set(false);
      }
    });
  }

  get backups() {
    return this.form.controls.backupCourses;
  }

  addBackup() {
    this.backups.push(
      this.fb.control("", {
        nonNullable: true,
        validators: Validators.required,
      }),
    );
  }

  removeBackup(index: number) {
    this.backups.removeAt(index);
  }

  submit() {
  if (this.form.valid) {
    const rawValue = this.form.getRawValue();

    const payload = {
      studentId: Number(rawValue.studentId),
      courseId: Number(rawValue.courseId),
      term: rawValue.term,
      notes: rawValue.notes || undefined,
      backupCourses: rawValue.backupCourses.filter(b => b.trim().length > 0)
    };

    this.lookupService.submitEnrollment(payload).subscribe({
      next: (res) => {
        console.log("Enrollment success:", res);
        this.submitted.set(true);
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.detail || "Failed to submit enrollment.");
      }
    });
  } else {
    this.form.markAllAsTouched();
  }
}
}
import { Component, signal, computed, inject } from "@angular/core";
import { RouterLink } from "@angular/router";
import { rxResource } from "@angular/core/rxjs-interop";
import { CourseCardComponent } from "../../ui/course-card/course-card.component";
import { CourseService } from "../../services/course.service";
import { Course } from "../../models/course.model";
import { PendingSummaryComponent } from "../pending-summary/pending-summary.component";

@Component({
  selector: "app-student-dashboard",
  standalone: true,
  imports: [CourseCardComponent, RouterLink, PendingSummaryComponent],
  templateUrl: "./student-dashboard.component.html",
  styleUrl: "./student-dashboard.component.scss",
})
export class StudentDashboardComponent {
  private api = inject(CourseService);

  studentName = signal("Liya Kebede");
  earnedCredits = signal(45);

  graduationStatus = computed(() =>
    this.earnedCredits() >= 120 ? "Eligible for Graduation" : "In Progress",
  );

  registerForClass() {
    this.earnedCredits.update((c) => c + 3);
  }

  selectedCourse = signal<Course | null>(null);

  coursesResource = rxResource({
    stream: () => this.api.getAll(),
  });

  handleEnroll(course: Course) {
    this.selectedCourse.set(course);
    console.log("Enrollment requested for:", course.title);
  }
}
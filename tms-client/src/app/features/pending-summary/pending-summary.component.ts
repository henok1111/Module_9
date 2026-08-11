import { Component, inject } from "@angular/core";
import { EnrollmentStore } from "../../store/enrollment.store";

@Component({
  selector: "tms-pending-summary",
  standalone: true,
  templateUrl: "./pending-summary.component.html",
  styleUrl: "./pending-summary.component.scss",
})
export class PendingSummaryComponent {
  store = inject(EnrollmentStore);
}
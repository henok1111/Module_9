import { inject, computed } from '@angular/core';
import { signalStore, withState, withMethods, withComputed, patchState } from '@ngrx/signals';
import { setAllEntities, removeEntity, withEntities } from '@ngrx/signals/entities';
import { catchError, EMPTY, tap } from 'rxjs';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course.model';

interface CourseState {
  loading: boolean;
  error: string | null;
}

const initialState: CourseState = {
  loading: false,
  error: null,
};

export const CourseStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withEntities<Course>(),
  withComputed((store) => ({
    courseCount: computed(() => store.entities().length),
  })),
  withMethods((store, svc = inject(CourseService)) => ({
    loadCourses() {
      patchState(store, { loading: true, error: null });

      svc.getAll().pipe(
        tap((courses) => {
          patchState(store, setAllEntities(courses));
          patchState(store, { loading: false });
        }),
        catchError((err) => {
          patchState(store, {
            loading: false,
            error: 'Failed to load courses.',
          });
          return EMPTY;
        })
      ).subscribe();
    },

    deleteCourse(id: string) {
      // 1. Snapshot BEFORE mutating — order matters, must happen first
      const previousSnapshot = store.entities();

      // 2. Instant UI feedback — remove immediately
      patchState(store, removeEntity(id));

      // 3. Fire the real request
      svc.delete(id).pipe(
        catchError((err) => {
          // 4. Server rejected it — restore exactly what was there before
          patchState(store, setAllEntities(previousSnapshot));
          patchState(store, {
            error: 'Cannot delete course: active student enrollments exist.',
          });
          return EMPTY;
        })
      ).subscribe();
    },
  }))
);
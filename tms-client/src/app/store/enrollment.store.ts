import { computed, inject } from '@angular/core';
import {
  signalStore,
  withComputed,
  withMethods,
  patchState,
  withState,
  withHooks,
} from '@ngrx/signals';
import {
  withEntities,
  setAllEntities,
  updateEntity,
} from '@ngrx/signals/entities';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, concatMap, tap, catchError, switchMap, EMPTY } from 'rxjs';
import { EnrollmentService } from '../services/enrollment.service';
import { LiveSyncService } from '../services/live-sync';
import { Enrollment } from '../models/enrollment.model';

export const EnrollmentStore = signalStore(
  { providedIn: 'root' },

  withState({ isLoading: false, error: null as string | null }),

  withEntities<Enrollment>(),

  withComputed((store) => ({
    pendingCount: computed(
      () => store.entities().filter((e: Enrollment) => e.status?.toLowerCase() === 'pending').length
    ),
  })),

  withMethods((
    store,
    api = inject(EnrollmentService),
    sync = inject(LiveSyncService)
  ) => ({
    loadEnrollments: rxMethod<void>(
      pipe(
        tap(() => patchState(store, { isLoading: true, error: null })),
        concatMap(() =>
          api.getAll().pipe(
            tap(rows => patchState(store, setAllEntities(rows), { isLoading: false })),
            catchError(err => {
              patchState(store, { isLoading: false, error: err.message });
              return EMPTY;
            })
          )
        )
      )
    ),

approveEnrollment: rxMethod<number | string>(
  pipe(
    tap(id => {
      const numericId = Number(id);
      patchState(store, updateEntity({ id: numericId, changes: { status: 'Approved' } }));
    }),
    concatMap(id => {
      const numericId = Number(id);
      const stringId = String(id);

      // Pass stringId to the service method
      return api.approve(stringId).pipe(
        catchError(err => {
          // Rollback local state on error
          patchState(store, updateEntity({ id: numericId, changes: { status: 'Pending' } }));
          patchState(store, { error: 'Server rejected the approval.' });
          return EMPTY;
        })
      );
    })
  )
),

    listenForLiveUpdates: rxMethod<void>(
      pipe(
        tap(() => sync.connect()),
        switchMap(() => sync.events$),
        tap((event: any) => {
          const rawId = event.id ?? event.enrollmentId ?? event.studentId;
          const targetId = Number(rawId);

          if (!isNaN(targetId)) {
            patchState(
              store,
              updateEntity({
                id: targetId,
                changes: { status: event.status }
              })
            );
          }
        })
      )
    ),
  })),

  withHooks({
    onInit(store) {
      store.loadEnrollments();
      store.listenForLiveUpdates();
    },
  })
);
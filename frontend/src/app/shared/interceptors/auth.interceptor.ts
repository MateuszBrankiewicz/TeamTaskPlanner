import { HttpClient, HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { catchError, switchMap, throwError } from "rxjs";
import { AuthService } from "../services/auth.service";

export const authInterceptor : HttpInterceptorFn = (req,next) => {
  const http = inject(HttpClient);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if(error.status === 401){
        console.log("blad 401");
        return http.post<{ email: string }>("http://localhost:5078/api/auth/refresh-token", {}, { withCredentials: true }).pipe(
          switchMap(tokenResponse => {
            console.log("Odswiezono")
            authService.authUser.set({ userName: tokenResponse.email, isLoggedIn: true })
            const newReq = req.clone();
              console.log(newReq);
              return next(newReq);
          }),
          catchError(refreshError => {
            console.log(refreshError);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  )
}

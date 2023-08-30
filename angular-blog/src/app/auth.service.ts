import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Response, UserClaim } from './UserClaim';
import { Observable, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  constructor(private http: HttpClient ) { 

  }

  public logIn(email:string, password:string){
    
    console.log(`form auth service ${email} - ${password}`)
    return this.http.post<Response>('https://localhost:7018/Account/Login',{email:email, password:password},{withCredentials:true}) // проверить потом можно ли без тип Response
  }

  public logOut(){
    return this.http.get('https://localhost:7018/Blog/Blogs', {withCredentials:true})
  }
  public user() {
    console.log("cookie is " + document.cookie)
    
    return this.http.get<UserClaim[]>('https://localhost:7018/Account/GetUserClaims', {withCredentials:true});
  } 

   
  public isSignedIn(): Observable<boolean> {
    return this.user().pipe(
      map((userClaims)=>{
        const hasClaims = userClaims.length>0;
        return !hasClaims ? false: true;
      }),
      catchError((error) => {
        console.log("error from isSignedIn")
        return of(false);
      }));
    
  }
}

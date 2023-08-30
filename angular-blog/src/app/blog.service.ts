import { Injectable } from '@angular/core';
import { Blog } from './models/blog';
import { HttpClient } from '@angular/common/http'
import { Observable, map, of } from 'rxjs';
import { Article } from './models/article';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(private http:HttpClient) { }

  getBlogs():Observable<Blog[]>{
    return this.http.get<Blog[]>("https://localhost:7018/Blog/Blogs")
  }
  getBlog(id:number):Observable<Blog>{
    console.log(`id is ${id}`)
    const blog = this.http.get<Blog>(`https://localhost:7018/Blog/GetBlog?id=${id}`)
    return blog;
  }
  getArticles():Observable<Article[]>{
    return this.http.get<Article[]>("https://localhost:7018/Blog/Articles");
  }
  getAuth(){
    //todo разобраться  в куки получениии и сделать авторизацию
  }
}

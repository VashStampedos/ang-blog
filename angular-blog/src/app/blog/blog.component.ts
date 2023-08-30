import { Component, OnInit } from '@angular/core';
import { Blog } from '../models/blog';
import { BlogService } from '../blog.service';
import { Article } from '../models/article';
import { map } from 'rxjs';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})


//сделать чтобы арткилы открывалиьс по нажатию
// и у каждого блога отдельно
export class BlogComponent {
  
  blogs:Blog[]=[];
  articles:Article[]=[];
  selectedBlog?:Blog;
  constructor(private blogService:BlogService) {
        
  }

  ngOnInit(){
    
    this.blogService.getBlogs().subscribe(x=> this.blogs=x);
   
  }

  ShowArticleOfBlog(blog:Blog){
    this.selectedBlog = blog;
  }
}

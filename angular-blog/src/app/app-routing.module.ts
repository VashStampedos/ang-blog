import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BlogComponent } from './blog/blog.component';
import { ArticleComponent } from './article/article.component';
import { AuthGuard } from './auth-guard';
import { UserComponent } from './user/user.component';
import { AuthComponent } from './auth/auth.component';

const routes: Routes = [
  { path: '', redirectTo:'/blogs', pathMatch:'full'},
  { path: 'blogs', component: BlogComponent},
  { path: 'articles/:id', component: ArticleComponent, pathMatch:'full' },//canActivate:[AuthGuard]
  { path: 'user', component:UserComponent},
  { path: 'login', component:AuthComponent}
 

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

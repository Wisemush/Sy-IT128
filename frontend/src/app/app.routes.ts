import { Routes } from '@angular/router';
import { ListPostsComponent } from './components/list-posts/list-posts.component';
import { PostDetailComponent } from './components/post-detail/post-detail.component';
import { LoginPageComponent } from './components/login-page.component/login-page.component';
import { RegisterPageComponent } from './components/register-page.component/register-page.component';

export const routes: Routes = [
    {path: '', component: ListPostsComponent},
    {path: 'posts/:id', component: PostDetailComponent},
    {path: 'login', component: LoginPageComponent},
    {path: 'register', component: RegisterPageComponent}
];
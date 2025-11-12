import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface Post {
  id: number;
  title: string;
  body: string;
  userId?: number;
  userName?: string;
  firstName?: string;
  lastName?: string;
  dateCreated?: string;  // Add this line
}
@Component({
  selector: 'app-list-posts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list-posts.component.html',
  styleUrls: ['./list-posts.component.css']
})
export class ListPostsComponent implements OnInit {  // Keep this name
  posts: Post[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.initData();
  }

  initData() {
    this.http.get<Post[]>('https://localhost:7198/api/Post/list')
      .subscribe({
        next: (data) => {
          this.posts = data;
          console.log(this.posts);
        },
        error: (error) => {
          console.error('Error fetching posts:', error);
        }
      });
  }
}
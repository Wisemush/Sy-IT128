import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

interface Post {
  id: number;
  title: string;
  body: string;
  userName?: string;
  dateCreated?: string;
}

@Component({
  selector: 'app-post-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './post-detail.html',
  styleUrl: './post-detail.css',
})
export class PostDetailComponent implements OnInit, OnDestroy {  // Changed name
  private routeSub: Subscription = new Subscription();
  private id: number = 0;
  post?: Post;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.initData();
    });
  }

  initData() {
    this.http.get<Post>(`https://localhost:7198/api/Post/${this.id}`)
      .subscribe({
        next: (data) => {
          this.post = data;
          console.log(this.post);
        },
        error: (error) => {
          console.error('Error fetching post:', error);
        }
      });
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

}
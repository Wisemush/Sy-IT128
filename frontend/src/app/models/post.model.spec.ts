import { Post } from './post.model';

describe('Post interface', () => {
  it('should create a valid Post object', () => {
    const post: Post = { id: 1, title: 'Hello', body: 'World' };
    expect(post).toBeTruthy();
  });
});

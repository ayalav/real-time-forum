export interface Post {
    id: number;
    title: string;
    content: string;
    showComments?: boolean;
    comments: Comment[];
  }

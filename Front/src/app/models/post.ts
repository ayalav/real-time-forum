export interface Post {
    id: number;
    title: string;
    content: string;
    userName?: string;
    showComments?: boolean;
    comments: Comment[];
  }

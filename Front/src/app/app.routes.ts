import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'register',
        loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent)
    },
    {
        path: 'forum',
        loadComponent: () => import('./components/forum/forum.component').then(m => m.ForumComponent),
        canActivate: [AuthGuard] 
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];
import { computed, effect, Injectable, signal } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ThemeServiceService {
  private _isDarkTheme = signal<boolean>(true);
  public readonly isDarkTheme = this._isDarkTheme.asReadonly();
  public readonly themeName = computed(() => {
    this._isDarkTheme() ? 'dark' : 'light';
  })
  public readonly themeClass = computed(() => this._isDarkTheme() ? 'dark-theme' : 'light-theme')
  constructor() {
    this.initializeTheme();
    effect(() => {
      this.applyTheme(this._isDarkTheme());
    })
  }
  private initializeTheme(){
    const savedTheme = localStorage.getItem('theme');
    if(savedTheme){
      this._isDarkTheme.set(savedTheme === 'dark');
    }else{
      this._isDarkTheme.set(true);
    }
  }
  private applyTheme(isDark : boolean){
    document.body.classList.remove('light-theme', 'dark-theme');
    if(isDark){
      document.body.classList.add('dark-theme');
      localStorage.setItem('theme','dark');
    }else{
      document.body.classList.add('light-theme');
      localStorage.setItem('theme','light');
    }
  }
  public setTheme(isDark:boolean){
    this._isDarkTheme.set(isDark);
  }
  public toggleTheme(){
    this._isDarkTheme.update(current => !current);
  }
  public getCurrentTheme(){
    return this._isDarkTheme()
  }
}

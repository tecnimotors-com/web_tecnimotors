import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent  implements OnInit, OnDestroy {
  ngOnInit(): void {
    setTimeout(() => {
      window.scrollTo(0, 0);
      this.initializePreLoader();
    }, 0);
    this.initializePreLoader();
  }

  ngOnDestroy(): void {
    this.finalizePreLoader();
  }

  private initializePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');

    if (preloaderWrapper) {
      preloaderWrapper.classList.remove('loaded');

      setTimeout(() => {
        preloaderWrapper.classList.add('loaded');
      }, 1000);
    } else {
      console.error('Preloader not found!');
    }
  }

  private finalizePreLoader(): void {
    const preloaderWrapper = document.getElementById('preloader');
    if (preloaderWrapper) {
      preloaderWrapper.classList.add('loaded');
    }
  }
}
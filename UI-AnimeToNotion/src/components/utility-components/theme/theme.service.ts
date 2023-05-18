import { Injectable, Renderer2, RendererFactory2 } from "@angular/core";

@Injectable()
export class ThemeService {

  isDarkTheme: boolean = true;
  renderer: Renderer2 | null = null;

  constructor(private rendererFactory: RendererFactory2)
  {
    this.renderer = this.rendererFactory.createRenderer(null, null);
  }  

  switchTheme(darkTheme: boolean) {

    if (darkTheme === true) {
      this.renderer!.addClass(document.body, 'dark-theme');
      this.isDarkTheme = true;
    }
    else {
      this.renderer!.removeClass(document.body, 'dark-theme');
      this.isDarkTheme = false;
    }

  }

  isCurrentlyDarkTheme() {
    return this.isDarkTheme;
  }

}

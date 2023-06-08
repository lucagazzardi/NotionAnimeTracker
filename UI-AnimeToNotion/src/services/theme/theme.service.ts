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

    // Adds stylesheet to suppress all the transitions
    const css = document.createElement('style')
    css.type = 'text/css'
    css.appendChild(
      document.createTextNode(
        `* {
           -webkit-transition: none !important;
           -moz-transition: none !important;
           -o-transition: none !important;
           -ms-transition: none !important;
           transition: none !important;
        }`
      )
    )
    document.head.appendChild(css);

    // Toggle theme
    if (darkTheme === true) {
      this.renderer!.addClass(document.body, 'dark-theme');
      this.isDarkTheme = true;
    }
    else {
      this.renderer!.removeClass(document.body, 'dark-theme');
      this.isDarkTheme = false;
    }

    // Removes the stylesheet
    const _ = window.getComputedStyle(css).opacity
    document.head.removeChild(css)
  }

  isCurrentlyDarkTheme() {
    return this.isDarkTheme;
  }

}

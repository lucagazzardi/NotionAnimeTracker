import { animation, style, animate, trigger, transition, useAnimation, keyframes } from '@angular/animations';

export const opacityOnEnter = animation([

  animate('150ms ease-in', keyframes([
    style({ opacity: 0 }),
    style({ opacity: 1 })
  ]))

]);

export const scaleUpOnEnter = animation([

  animate('100ms linear', keyframes([
    style({ transform: 'scale(.9)', opacity: 0, offset: 0 }),
    style({ transform: 'scale(1)', opacity: 1, offset: 1 })
  ]))

]);

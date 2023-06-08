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

export const totalScaleUp_OpacityOnEnter = animation([

  animate('200ms ease-in-out', keyframes([
    style({ transform: 'scale(.2)', opacity: 0, offset: 0 }),
    style({ transform: 'scale(1)', opacity: 1, offset: 1 })
  ]))

]);

export const totalScaleDown_OpacityOnLeave = animation([

  animate('200ms ease-in-out', keyframes([
    style({ transform: 'scale(1)', opacity: 1, offset: 0 }),
    style({ transform: 'scale(.2)', opacity: 0, offset: 1 })
  ]))

]);

export const totalScaleUp_Opacity_MarginOnEnter = animation([

  animate('200ms ease-in-out', keyframes([
    style({ transform: 'scale(.2)', opacity: 0, marginTop: '-10px', offset: 0 }),
    style({ transform: 'scale(1)', opacity: 1, marginTop: '0', offset: 1 })
  ]))

]);

export const totalScaleUp_Opacity_MarginOnLeave = animation([

  animate('200ms ease-in-out', keyframes([
    style({ transform: 'scale(1)', opacity: 1, marginTop: '0', offset: 0 }),
    style({ transform: 'scale(.2)', opacity: 0, marginTop: '-10px', offset: 1 })
  ]))

]);

export const YMovement_Opacity = animation([
  animate('200ms ease-in-out', keyframes([
    style({ transform: 'translateY(-50px)', opacity: 0, offset: 0 }),
    style({ transform: 'translateY(0) scale(1)', opacity: 1, offset: 1 })
  ]))
])

export const YMovement_Opacity_Leave = animation([
  animate('200ms ease-in-out', keyframes([
    style({ transform: 'translateY(0) scale(1)', opacity: 1, offset: 0 }),
    style({ transform: 'translateY(-50px)', opacity: 0, offset: 1 })
  ]))
])

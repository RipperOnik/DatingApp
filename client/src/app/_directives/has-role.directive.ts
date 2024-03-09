import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AccountService } from '../_services/account.service';
import { map, take } from 'rxjs';
import { User } from '../_models/user';

@Directive({
  selector: '[appHasRole]', // *appHasRole = ["Admin", "AnotherRole"]
})
export class HasRoleDirective implements OnInit {
  user: User = {} as User;
  @Input() appHasRole: string[] = [];
  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
  }
  ngOnInit(): void {
    if (this.user.roles.some((r) => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef); // render child views
    } else {
      this.viewContainerRef.clear(); // remove child elements from the DOM
    }
  }
}

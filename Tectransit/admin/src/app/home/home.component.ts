import { Component, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { AuthenticationService } from '../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {

  sidebarHover = false;
  activeSidebarMobile = false;
  SidebarMobileclass = false;
  activeHeaderMobile = false;
  activeFixedFooter = false;

  activeSidebar = true;
  activeSearch = false;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
  }

  @HostListener('window:resize', ['$event']) onResize(event) {
    if (event.target.innerWidth < 991.98) {
      console.log(event.target.innerWidth);
      this.SidebarMobileclass = true;
    } else {
      this.SidebarMobileclass = false;
    }
  }

  toggleSidebar() {
    this.activeSidebar = !this.activeSidebar;
  }

  toggleSidebarMobile() {
    this.activeSidebarMobile = !this.activeSidebarMobile;
  }

  toggleHeaderMobile() {
    this.activeHeaderMobile = !this.activeHeaderMobile;
  }

  srhtoggle() {
    this.activeSearch = !this.activeSearch;
  }

  logOut() {
    this.authenticationService.logout();
    // redirect to login page
    this.router.navigate(['/login']);
  }

}

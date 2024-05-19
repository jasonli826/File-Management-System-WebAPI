import { Component ,OnInit,ViewChild,ElementRef,ChangeDetectorRef  } from '@angular/core';
import { AuthService } from './service/authService';
import { TitleService } from './service/titleService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit {
  @ViewChild('closebutton') closebutton!: ElementRef<HTMLButtonElement>;
  loginForm: FormGroup;
  title = 'AngularCRUD';
  ImagePath = 'image/banner5.gif'
  ActiveDownload: string = 'Inactive';
  ActiveUpload: string = 'Inactive';
  ActiveUserManagement:string ='inactive';
  showDownloadButton: boolean = false;
  showLoginButton: boolean = true;
  ShowUploadButton : boolean = false;
  ShowUserRoleButton : boolean = false;
  ShowUserManagementButton : boolean = false;
  ShowLoginButton:boolean = true;
  ShowLogoutButton:boolean = false;
  userInfo: any;
  ShowProjectButton:boolean = false;
  pageTitle:string = "Download";
  buttonText : string = "Login";
  ActivateAddEditDepartComp :boolean = false;
  constructor(private Auth :AuthService,private cdr: ChangeDetectorRef,private titleService: TitleService,private formBuilder: FormBuilder,private router: Router){
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }
  // ngAfterViewInit(): void {
  //   // Get the #menu element
  //   const menu = document.getElementById("menu");
  //   console.log('document.location.href.split("#")[0]',document.location.href.split("#")[0]);
  //   // Perform null check
  //   if (menu) {
  //     // If the #menu element exists, select its <a> elements
  //     const a = menu.getElementsByTagName("a");
  //      console.log('a',a);
  //     // Loop through each <a> element
  //     for (let i = 0; i < a.length; i++) {
  //       console.log("a[i].href.split",a[i].href.split("#")[0]);
  //       // Check if the href of the current <a> matches the current URL (excluding hash fragments)
  //       if (a[i].href.split("#")[0] === document.location.href.split("#")[0]) {
  //         // If there's a match, add the class "current" to the current <a> element
  //         a[i].className = "active";
  //       }
  //     }
  //   }
  // }
 
  ngOnInit(): void {
    
    const menu = document.getElementById("menu");
    console.log("menu",menu);
    // Perform null check
    if (menu) {
      // If the #menu element exists, select its <a> elements
      const a = menu.getElementsByTagName("a");
    
      // Loop through each <a> element
      for (let i = 0; i < a.length; i++) {
        // Check if the href of the current <a> matches the current URL (excluding hash fragments)
        if (a[i].href.split("#")[0] === document.location.href.split("#")[0]) {
          // If there's a match, add the class "current" to the current <a> element
          a[i].className = "active";
        }
      }
    }
   


    this.titleService.getPageTitle().subscribe(title => {
      this.pageTitle = title;
    });
    this.Auth.userInfo$.subscribe((userData: any) => {
      console.log(userData);
      
      this.userInfo = userData;
      console.log(this.userInfo);
      
      // Check if user has access to the "Upload" menu
        if(this.userInfo!=null)
        {
            this.ShowUploadButton = this.userInfo && this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Upload'));
            this.ShowUploadButton = this.userInfo && this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Upload'));
            this.ShowUserRoleButton = this.userInfo && this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Role'));
            this.ShowUserManagementButton = this.userInfo && this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('User Management'));
            this.ShowProjectButton = this.userInfo && this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Project'));
            this.showDownloadButton = true;
           // this.ShowLogInLogoutButton = (this.userInfo==null? false:true);
          
        }
        
        

    });
  }
  DownloadClick()
  {
      this.pageTitle="Download File";

  }
  ProjectClick()
  {
    this.pageTitle="Project";
  }
  
  LoginClick()
  {

   // this.pageTitle="Login";
  }
  UploadClick()
  {
      this.pageTitle="Upload File";

  }
  UserRoleClick()
  {
      this.pageTitle="User Role";

  }
  UserManagementClick()
  {
      this.pageTitle="User Management";

  }
  LogoutClick()
  {
    this.Auth.clearUserData();
    this.userInfo = null;
    // this.showLoginButton = true;
    this.ShowUploadButton= false;
    this.ShowUserRoleButton = false;
    this.ShowUserManagementButton = false;
    this.ShowProjectButton = false;
    this.ShowLoginButton=true;
    this.ShowLogoutButton= false;
    this.router.navigate(['/download']);

  }
  popupWindow()
  {

      this.loginForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
      });
      this.ActivateAddEditDepartComp = true;
    

  }
   myFunction() {
    var x = document.getElementById("myTopnav");
    if (x) {
      if (x.className === "topnav") {
        x.className += " responsive";
      } else {
        x.className = "topnav";
      }
    }
  }
  closeClick(){

    this.ActivateAddEditDepartComp= false;
  }
  closeModal() {
    this.ShowLoginButton = false;
    this.ShowLogoutButton = true;
    this.ActivateAddEditDepartComp= false;
    console.log('Login successful'); // Add this line
    document.getElementById('exampleModal')!.classList.remove('show');
    document.body.classList.remove('modal-open');
    const modalBackdrop = document.getElementsByClassName('modal-backdrop')[0];
    modalBackdrop.parentNode!.removeChild(modalBackdrop);
    this.closeClick();
  }
  handleLoginSuccess() {
    this.closeModal();
  }
}

import { Component,OnInit,Input,ViewChild,ElementRef,EventEmitter,Output  } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from '../service/authService';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TitleService } from '../service/titleService';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { AppComponent } from '../app.component';
import { Renderer2 } from '@angular/core';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild('closebutton') closebutton!: ElementRef<HTMLButtonElement>;
  //@ViewChild('LoginButton') myModalButton!: ElementRef<HTMLButtonElement>;
  @Input() loginForm: FormGroup;
  submitted = false;
  pageTitle:string = 'Login';
  dismissModal:boolean=false;
  loading = false;
  error = '';
  userInfo:any;
 @Output() loginSuccess: EventEmitter<void> = new EventEmitter<void>();
  constructor(private formBuilder: FormBuilder,private AuthService: AuthService, private router: Router,private titleService: TitleService,private mainPage:AppComponent,private renderer: Renderer2) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // this.titleService.setPageTitle('Login');
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.titleService.getPageTitle().subscribe(title => {
      this.pageTitle = title;
    });
    this.router.navigate(['/download']);
  }

// convenience getter for easy access to form fields
get f() { return this.loginForm.controls; }
onSubmit() {
  this.submitted = true;

  // stop here if form is invalid
  if (this.loginForm.invalid) {
      return;
  }

  this.loading = true;
  this.AuthService.LoginSystem(this.loginForm.controls['username'].value, this.loginForm.controls['password'].value)
  .subscribe(
    (response: any) => {
      console.log(response);
      if (response === "User Name or Password is invalid") {
        // Show error message if userId is "0"
        this.error = 'User Name or Password is invalid';
        alert(this.error);
        this.loading = false;
        this.AuthService.setUserData(null);
         return;
      } 
      else if(response==="This User Is Inactive")
      {
        this.error = 'this login account is inactive';
        alert(this.error);
        this.loading = false;
        this.AuthService.setUserData(null);
        return;
      }
      else {
        this.userInfo = response;
        console.log(this.userInfo);
        this.AuthService.setUserData(this.userInfo);
        this.loading = false;
        console.log("click");
        this.userInfo = response;
        this.AuthService.setUserData(this.userInfo);
        this.loading = false;
        this.loginSuccess.emit(); // Emitting true indicating login success
        //this.closeModalEvent.emit();
        //this.modalService.closeLoginModal();
      }
    },
    error => {
      this.error = error;
      this.loading = false;
    },
    ()=>{
       this.loading=false;

    }
  );
 
}
}

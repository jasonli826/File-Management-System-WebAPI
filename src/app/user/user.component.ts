import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from '../service/authService';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  constructor(private service: ApiserviceService,private Auth:AuthService, private router: Router ) { }
  selectedUserRoles: any[] = []; 
  disableUserIdInput: boolean = false;
  disableUserPasswordInput:boolean=false;
  userList: any[] = [];
  userRoleList: any[] = []; 
  showPassword: boolean = false;
  resetButtonVisible:boolean = false;
  cancelButtonVisible:boolean = false;
  userInfo:any;
  ButtonText:string = "Save";
  user_dto = {
    UserId: '',
    UserName: '',
    Password:'',
    Remark:'',
    isActive:true,
    RoleIds:[0],
    Created_by:'',
    Update_by:'',

  };
  ngOnInit(): void {
    this.Auth.userInfo$.subscribe((userData: any) => {
      console.log(userData);
      
      this.userInfo = userData;
      console.log(this.userInfo);
      // Check if user has access to the "Upload" menu
        if(this.userInfo==null)
        {
          this.router.navigate(['/download']);
        }
        if(this.userInfo!=null)
        {
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('User Management')))
            {
                this.router.navigate(['/download']);

            }
          
        }
    });
    this.loadUserRoleList();
    this.refreshUserList();
  }
  cancelPasswordClick() {
   this.user_dto.Password="";
   this.resetButtonVisible= true;
   this.disableUserPasswordInput = true;
   this.cancelButtonVisible= false;
}
  loadUserRoleList()
  {
    this.service.getAllRoleList().subscribe(data => {
      this.userRoleList = data;
    });

  }
  clear()
  {

    this.user_dto = {
      UserId: '',
      UserName: '',
      Password:'',
      Remark:'',
      isActive:true,
      RoleIds:[0],
      Created_by:this.userInfo==null?"":this.userInfo.userId,
      Update_by:this.userInfo==null?"":this.userInfo.userId
    
  
    };
    this.selectedUserRoles=[];
    this.loadUserRoleList();
    this.disableUserIdInput = false;
    this.ButtonText ="Save";
    this.resetButtonVisible =false,
    this.disableUserPasswordInput= false;
    this.cancelButtonVisible= false;
    this.resetButtonVisible = false;
    this.refreshUserList();
  }
  enablePasswordClick () {
    this.disableUserPasswordInput =false;
    this.cancelButtonVisible= true;
    this.resetButtonVisible = false;

}
  clickSave()
  {

      if(this.user_dto.UserId==='')
      {
        alert("please enter User Id");
        return;
      }
        if(this.user_dto.UserName==='')
        {
          alert("please enter UserName Id");
          return;
        }

          if( this.selectedUserRoles.length===0)
          {
            alert("please select the desired user role for this user");
            return;

          }
          this.user_dto.RoleIds = this.selectedUserRoles.map(role => role.RoleID);
          console.log(this.user_dto);
          if(this.ButtonText=="Save")
         {
          if(this.user_dto.Password==='')
            {
              alert("please enter your password");
              return;
            }
            if(this.user_dto.Password.length<8)
              {
                alert("Password must have at least 8 charactors");
                return;
    
              }
          this.user_dto.Created_by = this.userInfo.userId;
          this.user_dto.Update_by = this.userInfo.userId;
          this.service.addUser(this.user_dto).subscribe(res => {
            alert(res.toString());
            this.refreshUserList();
          });
          this.clear();
        }
        else 
        {
          console.log("disableUserPasswordInput "+this.disableUserPasswordInput );
          if(this.disableUserPasswordInput ===true)
            {
                this.user_dto.Password= "";

            }
            else 
            {
                  if(this.user_dto.Password=="")
                  {
                      alert("Please Enter your reset password");
                      return;
                  }
                  if(this.user_dto.Password.length<8)
                    {
                      alert("Password must have at least 8 charactors");
                      return;

                    }

            }
          this.user_dto.Update_by = this.userInfo.userId;
          this.service.updateUser(this.user_dto).subscribe(res => {
            alert(res.toString());
            this.refreshUserList();
          });
          this.clear();
        }
  }
  searchUser()
  {
    this.user_dto.RoleIds= this.selectedUserRoles.map(role => role.RoleID);
    console.log(this.user_dto.RoleIds);
    this.service.searchUsers(this.user_dto).subscribe(data => {
      this.userList = data
    });

  }
  refreshUserList()
  {

    this.service.getAllUsers().subscribe(data => {
      this.userList = data
    });

  }
  editClick(item: any) {
    this.loadUserRoleList();
    console.log(item);
    this.user_dto.UserId = item.UserId;
    this.user_dto.UserName = item.UserName;
    this.disableUserPasswordInput = true;
    this.resetButtonVisible = true;
    this.cancelButtonVisible = false;
    // this.user_dto.Password = item.Password;
    // this.user_dto.EffectiveDateFrom = item.EffectiveDateFrom;
    // this.user_dto.EffectiveDateEnd = item.EffectiveDateEnd;
    this.user_dto.Remark = item.Remark;
    this.user_dto.isActive = item.isActive;
    this.selectedUserRoles = [];
    // Set selectedUserRoles based on RoleIds
    for (const role of this.userRoleList) {
        if (item.RoleIds.includes(role.RoleID)) {
            this.selectedUserRoles.push(role);
            role.selected = true;
        } else {
            role.selected = false;
        }
    }
    console.log(this.selectedUserRoles);
    this.disableUserIdInput = true;
    this.ButtonText = "Update";
  

  }
  toggleSelection(userRole: any) {
      userRole.selected = !userRole.selected;
}

moveItem(direction: string) {
    if (direction === 'right') {
        // Move selected items from userRoleList to selectedUserRoles
        this.userRoleList.forEach(role => {
            if (role.selected && !this.selectedUserRoles.includes(role)) {
                this.selectedUserRoles.push(role);
                role.selected = true;
            }
        });
        // Remove moved items from userRoleList
        this.userRoleList = this.userRoleList.filter(role => !role.selected);
    } else if (direction === 'left') {
        // Move selected items from selectedUserRoles to userRoleList
        this.selectedUserRoles.forEach(role => {
            if (role.selected && !this.userRoleList.includes(role)) {
                this.userRoleList.push(role);
                role.selected = true;
            }
        });
        // Remove moved items from selectedUserRoles
        this.selectedUserRoles = this.selectedUserRoles.filter(role => !role.selected);
    }
    // Remove duplicates between userRoleList and selectedUserRoles
    this.removeDuplicates();
}
removeDuplicates() {
    // Create sets of Role_Name for userRoleList and selectedUserRoles
    const userRoleNames = new Set();
    const selectedUserRoleNames = new Set();

    // Filter out duplicates from userRoleList and selectedUserRoles
    this.userRoleList = this.userRoleList.filter(role => {
        if (userRoleNames.has(role.Role_Name)) {
            return false; // Skip duplicate
        } else {
            userRoleNames.add(role.Role_Name);
            return true;
        }
    });

    this.selectedUserRoles = this.selectedUserRoles.filter(role => {
        if (selectedUserRoleNames.has(role.Role_Name)) {
            return false; // Skip duplicate
        } else {
            selectedUserRoleNames.add(role.Role_Name);
            return true;
        }
    });
  }
}

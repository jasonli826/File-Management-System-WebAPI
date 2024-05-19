import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from 'src/app/service/authService';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-add-edit-role',
  templateUrl: './add-edit-role.component.html',
  styleUrls:[ './add-edit-role.component.css']
})
export class AddEditRoleComponent implements OnInit{
  roleData = {
    roleName: '',
    Description: '',
  };
  RoleList: any[] = [];
  role =
  {
    RoleID:0,
    Role_Name :'',
    Description:'',
    Created_by:'',
    Created_Date:Date.now,
    Updated_by:'',
    Updated_Date:Date.now,
    Status:'Active'


  };
  constructor(private service: ApiserviceService,private Auth:AuthService, private router: Router) { }
  userInfo:any;
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
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('User Role')))
            {
                this.router.navigate(['/download']);

            }
          
        }
    });
    this.refreshRoleList();
  }
  editClick(item: any) {
    this.role = item;
    this.roleData.roleName = this.role.Role_Name;
    this.roleData.Description = this.role.Description;
  }
  clickClear(){

       this.roleData.roleName= "";
       this.roleData.Description = "";

  }
  clickSave()
  {
    if(this.role.RoleID!=0)
    {
      this.role.Role_Name = this.roleData.roleName;
      this.role.Description = this.roleData.Description;
      this.service.updateRole(this.role).subscribe(res => {
        alert(res.toString());
        this.clickClear();
        this.refreshRoleList();
  
      });
    }
    else
    {
      //this.role.RoleID =0;
      this.role.Role_Name = this.roleData.roleName;
      this.role.Description = this.roleData.Description;
      this.service.addRole(this.role).subscribe(res => {
        alert(res.toString());
        this.clickClear();
        this.refreshRoleList();

      });
    }

  }
  isAnyCheckboxChecked(): boolean {
    console.log("the result "+this.RoleList.some(item => item.checked));
    return this.RoleList.some(item => item.checked);
  }
  toggleCheckbox(item: any): void {
    item.checked = !item.checked;
    console.log(item.checked);
  }
  refreshRoleList() {
    this.service.getAllRoleList().subscribe(data => {
      this.RoleList = data;
    });
  }
  toggleSelectAll(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target && target.checked) {
      this.RoleList.forEach(item => {
        item.selected = true;
      });
    } else {
      this.RoleList.forEach(item => {
        item.selected = false;
      });
    }
  }
  
  clickActive()
  {
    for (const item of this.RoleList) {
      console.log("checked:"+item.checked);
      if (item.checked) {
         this.role =item;
         this.role.Status="Active";
         this.service.updateRole(this.role).subscribe(res => {
          alert("The Status has been changed to active");
          this.refreshRoleList();
      }
    );
    }

  }
  
}
clickInactive()
{
  for (const item of this.RoleList) {
    console.log("checked:"+item.checked);
    if (item.checked) {
       this.role =item;
       this.role.Status="Inactive";
       this.service.updateRole(this.role).subscribe(res => {
        alert("The Status has been changed to inactive");
        this.refreshRoleList();
    }
  );
  }

}

}

}

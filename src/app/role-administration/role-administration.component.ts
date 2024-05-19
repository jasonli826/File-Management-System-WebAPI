import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from '../service/authService';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-role-administration',
  templateUrl: './role-administration.component.html',
  styleUrls: ['./role-administration.component.css']
})
export class RoleAdministrationComponent implements OnInit{
  constructor(private service: ApiserviceService,private Auth:AuthService, private router: Router) { }
  RoleList: any[] = [];
  RoleControlList: any[] =[];
  temp: any[] = [];
  MenuItemList: any[]=[];
  viewupload=false;
  viewuserrole= false;
  userInfo:any
  viewusermanagement = false;
  selectedRoleId: number = 0;
  RoleDesc:string='';
  role_dto = {
    roleID: 0, // Initialize roleID with a default value
    menuIds: [0],
    created_by: 'Administrator',
    updated_by: 'Administrator'
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
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('User Role')))
            {
                this.router.navigate(['/download']);

            }
          
        }
    });
    //this.refreshEmpList();
    if(this.RoleList.length==0&&this.MenuItemList.length==0)
      {
        this.loadRoleList();
        this.loadMenuList();
      }
  }
  selectAllCheckbox(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target !== null) {
      const checked = target.checked;
      this.MenuItemList.forEach(menuItem => {
        menuItem.checked = checked;
      });
    }
  
    // Uncheck the "Select All" checkbox if any individual checkbox is unchecked
    if (!target.checked) {
      const selectAllCheckbox = document.getElementById('selectAllCheckbox') as HTMLInputElement;
      if (selectAllCheckbox) {
        selectAllCheckbox.checked = false;
      }
    }
  }
  select(event: Event)
  {
    const target = event.target as HTMLInputElement;
    if (!target.checked) {
      const selectAllCheckbox = document.getElementById('selectAllCheckbox') as HTMLInputElement;
      if (selectAllCheckbox) {
        selectAllCheckbox.checked = false;
      }
    }
  }
  loadRoleList() {
    this.service.getAllRoleList().subscribe(
      (data: any) => {
        console.log(data);
        this.temp = data;
        this.RoleList = this.temp.filter(role=> role.Status == 'Active');
        console.log(this.RoleList);
      },
      (error: any) => {
        console.error('Error fetching role list:', error);
      }
    );
  }
  loadMenuList()
  {
    this.service.getMenuItemList().subscribe(
      (data: any) => {
        console.log(data);
        this.MenuItemList =data;
        console.log(this.MenuItemList);
      },
      (error: any) => {
        console.error('Error fetching MenuItem list:', error);
      }
    );

  }
  
  onChange() {
    console.log("selected value is " + this.selectedRoleId);
    const selectedRole = this.RoleList.find((role: any) => role.RoleID === this.selectedRoleId);
    if (selectedRole) {
      // If a role is found, set the RoleDesc
      this.RoleDesc = selectedRole.Description; // Assuming there's a property RoleName
      this.service.GetRoleControlListByID(this.selectedRoleId).subscribe(
        (data: any) => {
          console.log(data);
          this.RoleControlList = data;
          console.log(this.RoleControlList);
  
          // Populate MenuItemList checkboxes based on RoleControlList
          if (this.RoleControlList&&this.RoleControlList.length>0) {
            this.MenuItemList.forEach(menuItem => {
              const found = this.RoleControlList.find(control => control.MenuId === menuItem.MenuID);
              menuItem.checked = found ? true : false;
            });
          }
          else 
          {
            this.MenuItemList.forEach(menuItem => {
              const found = this.RoleControlList.find(control => control.MenuId === menuItem.MenuID);
              menuItem.checked = false;
            });

          }
        },
        (error: any) => {
          console.error('Error fetching MenuItem list:', error);
        }
      );
  
    } else {
      // If no role is found, reset the RoleDesc
        this.RoleDesc = '';
        if(this.MenuItemList&&this.MenuItemList.length>0)
        {
            this.MenuItemList.forEach(menuItem => {
            menuItem.checked =  false;
          });
        }
        const selectAllCheckbox = document.getElementById('selectAllCheckbox') as HTMLInputElement;
        if (selectAllCheckbox) {
          selectAllCheckbox.checked = false;
        }
    }
  }

	
  SaveAccessControl(event: Event)  {
    event.preventDefault();
    this.role_dto.roleID = this.selectedRoleId;
    const selectedIds: number[] = []; // Declare selectedIds as an array of numbers
    if (this.MenuItemList) {
      this.MenuItemList.forEach(menuItem => {
        if (menuItem.checked) {
          selectedIds.push(menuItem.MenuID);
        }
      });
    }
    this.role_dto.menuIds = selectedIds;
    console.log('Selected IDs:', selectedIds);
    this.service.addRoleControl(this.role_dto).subscribe(res => {
      alert(res.toString());
      this.loadRoleList();
    });
  }
} 

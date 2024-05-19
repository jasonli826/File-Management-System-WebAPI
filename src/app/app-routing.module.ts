import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RoleAdministrationComponent } from './role-administration/role-administration.component';
import { EmployeeComponent } from './employee/employee.component';
import { DepartmentComponent } from './department/department.component';
import { DownloadComponent } from './download/download.component';
import { UploadComponent } from './upload/upload.component';
import { AddEditRoleComponent } from './role-administration/add-edit-role/add-edit-role.component';
import { UserComponent } from './user/user.component';
import { ProjectComponent } from './project/project.component';
const routes: Routes = [
  {path:'login',component:LoginComponent},
  { path: 'employee', component: EmployeeComponent },
  { path: 'department', component: DepartmentComponent },
  { path:'download',component:DownloadComponent},
  { path:'upload',component:UploadComponent},
  {path:'roleadministration',component:RoleAdministrationComponent},
  {path:'addeditrole',component:AddEditRoleComponent},
  {path:'user',component:UserComponent},
  {path:'project',component:ProjectComponent},
  {path:'',component:DownloadComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DepartmentComponent } from './department/department.component';
import { ShowDepartmentComponent } from './department/show-department/show-department.component';
import { AddEditDepartmentComponent } from './department/add-edit-department/add-edit-department.component';
import { EmployeeComponent } from './employee/employee.component';
import { ShowEmployeeComponent } from './employee/show-employee/show-employee.component';
import { AddEditEmployeeComponent } from './employee/add-edit-employee/add-edit-employee.component';
import { ApiserviceService } from './apiservice.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DownloadComponent } from './download/download.component';
import { UploadComponent } from './upload/upload.component';
import { RoleAdministrationComponent } from './role-administration/role-administration.component';
import { AddEditRoleComponent } from './role-administration/add-edit-role/add-edit-role.component';
import { ProjectComponent } from './project/project.component';
import { UserComponent } from './user/user.component';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { LoginComponent } from './login/login.component';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  declarations: [
    AppComponent,
    DepartmentComponent,
    ShowDepartmentComponent,
    AddEditDepartmentComponent,
    EmployeeComponent,
    ShowEmployeeComponent,
    AddEditEmployeeComponent,
    DownloadComponent,
    UploadComponent,
    RoleAdministrationComponent,
    AddEditRoleComponent,
    UserComponent,
    LoginComponent,
    ProjectComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule ,
    DropDownsModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatCardModule,

  ],
  providers: [ApiserviceService, provideAnimationsAsync()],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHeaders,HttpParams  } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiserviceService {
  readonly apiUrl = "http://localhost:53535/api/";
  readonly photoUrl = "http://localhost:53535/Photos/";

  constructor(private http: HttpClient) { }

   LoginSystem(userId:string,password:string): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'Login/Login/'+userId+'/'+password);
   }
  // Department
  getDepartmentList(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'department/GetDepartment');
  }

  addDepartment(dept: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'department/AddDepartment', dept, httpOptions);
  }

  updateDepartment(dept: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<any>(this.apiUrl + 'department/UpdateDepartment/', dept, httpOptions);
  }

  deleteDepartment(deptId: number): Observable<number> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.delete<number>(this.apiUrl + 'department/DeleteDepartment/' + deptId, httpOptions);
  }

  searchFileList( fileType:string, projectName:string):Observable<any[]>{

    return this.http.get<any[]>(this.apiUrl + 'FileDetail/SearchFileList?fileType='+fileType+`&projectName=`+projectName);
  }
  // Employee
  getEmployeeList(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'EmployeeAngular/GetEmployee');
  }
  getMenuItemList(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'MenuItem/GetMenuItems');
  }
  getFileList(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'FileDetail/GetFileList');
  }
  deleteFile(Id:number):Observable<number>
  {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.delete<number>(this.apiUrl + 'FileDetail/DeleteFile/' + Id, httpOptions);

  } 
  downloadFile(Id:number)
  {

    return this.http.get(this.apiUrl + `FileDetail/DownloadFile?id=`+Id, { responseType:'blob' });

  }

  addEmployee(emp: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'EmployeeAngular/AddEmployee', emp, httpOptions);
  }

  updateEmployee(emp: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<any>(this.apiUrl + 'EmployeeAngular/UpdateEmployee/', emp, httpOptions);
  }

  deleteEmployee(empId: number): Observable<number> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.delete<number>(this.apiUrl + 'EmployeeAngular/DeleteEmployee/' + empId, httpOptions);
  }

  uploadPhoto(photo: any) {
    return this.http.post(this.apiUrl + 'EmployeeAngular/savefile', photo);
  }

  getAllDepartmentNames(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'EmployeeAngular/GetDepartment');
  }
  getAllRoleList(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'Role/GetRoleList');
  }
  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl + 'User/GetUserList');
  }
  searchUsers(user:any): Observable<any[]> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any[]>(this.apiUrl + 'User/SearchUser', user, httpOptions);
  }
  getAllProjects():Observable<any[]>{
    return this.http.get<any[]>(this.apiUrl + 'Project/GetProjectList');

  }
  searchProjects(project:any): Observable<any[]> {
    var currentDate = new Date();
    var isoDateString = currentDate.toISOString();
    project.created_Date = isoDateString;
    project.updated_Date = isoDateString;
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    const params = new HttpParams({ fromObject: project }); // Convert project object to query parameters
    return this.http.get<any[]>(this.apiUrl + 'Project/SearchProject', { params: params, ...httpOptions });
  }

  updateRole(role: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<any>(this.apiUrl + 'Role/UpdateRole/', role, httpOptions);
  }
  addRole(role: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'Role/AddRole/', role, httpOptions);
  }
  addUser(user: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'User/AddUser/', user, httpOptions);
  }
  addProject(project: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'Project/AddProject/', project, httpOptions);
  }
  updateUser(user: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<any>(this.apiUrl + 'User/UpdateUser/', user, httpOptions);
  }
  updateProject(project: any): Observable<any> {
    console.log(project);
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.put<any>(this.apiUrl + 'Project/UpdateProject/', project, httpOptions);
  }
  GetRoleControlListByID(Id:number): Observable<any[]> {

    return this.http.get<any[]>(this.apiUrl + 'RoleControl/GetRoleControlByID/'+Id);
  }
  addRoleControl(role: any): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.apiUrl + 'RoleControl/AddRoleControl/', role, httpOptions);
  }
 uploadfile(fileDetails: File,uploadData:any) {
    let formParams = new FormData();
    console.log(uploadData);
    formParams.append('fileDetails', fileDetails);
    formParams.append('version', uploadData.verionNo);
    formParams.append('uploadType',uploadData.uploadType);
    formParams.append('releaseBy', uploadData.releaseBy);
    formParams.append('releaseDate', uploadData.dateOfReleasse);
    formParams.append('releaseContent', uploadData.releaseContent);
    formParams.append('uploadBy',uploadData.uploadBy);
    formParams.append('projectName',uploadData.projectName);
    console.log(formParams);
    return this.http.post(this.apiUrl +'FileDetail/AddNewFile?version='+uploadData.verionNo+"&uploadType="+uploadData.uploadType+"&releaseBy="+uploadData.releaseBy+"&releaseDate="+uploadData.dateOfReleasse+"&releaseContent="+uploadData.releaseContent+"&uploadBy="+uploadData.uploadBy+"&projectName="+uploadData.projectName,formParams);
  }

}

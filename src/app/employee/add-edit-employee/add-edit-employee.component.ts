import { Component, Input, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { ShowEmployeeComponent } from '../show-employee/show-employee.component';
@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {

  constructor(private service: ApiserviceService,private showCompt:ShowEmployeeComponent) { }
  @Input() emp: any;
  EmployeeID = "";
  EmployeeName = "";
  Department = "";
  DateOfJoining = "";
  PhotoFileName = "";
  PhotoFilePath = "";
  DepartmentList: any = [];

  ActivateAddEditEmpComp: boolean = true;
  ngOnInit(): void {
    this.loadEmployeeList();
    this.ActivateAddEditEmpComp = true;
  }

  loadEmployeeList() {

    this.service.getAllDepartmentNames().subscribe((data: any) => {
      this.DepartmentList = data;

      this.EmployeeID = this.emp.EmployeeID;
      this.EmployeeName = this.emp.EmployeeName;
      this.Department = this.emp.Department;
      this.DateOfJoining = this.emp.DateOfJoining;
      this.PhotoFileName = this.emp.PhotoFileName;
      this.PhotoFilePath = this.service.photoUrl + this.PhotoFileName;
    });
  }

  addEmployee() {
    var val = {
      EmployeeID: this.EmployeeID,
      EmployeeName: this.EmployeeName,
      Department: this.Department,
      DateOfJoining: this.DateOfJoining,
      PhotoFileName: this.PhotoFileName
    };

  //   fetch('http://localhost:53535/api/employee/addemployee', {
  //     method: 'Post',
  //     headers: {
  //         'Accept': 'application/json',
  //         'Content-Type': 'application/json'
  //     },
  //     body: JSON.stringify(val)
  // })
  //     .then(res => res.json())
  //     .then((result) => {
  //         alert(result);
  //        // this.setState({popupWindowVisible:true})
  //         this.showCompt.refreshEmpList();
  //     }, (error) => {
  //         alert('Failed');
  //     })
    
  //     this.ActivateAddEditEmpComp = false;
    this.service.addEmployee(val).subscribe(res => {
      alert(res.toString());
      this.showCompt.refreshEmpList();
    });
         this.ActivateAddEditEmpComp = false;
  }

  updateEmployee() {
    var val = {
      EmployeeId: this.EmployeeID,
      EmployeeName: this.EmployeeName,
      Department: this.Department,
      DateOfJoining: this.DateOfJoining,
      PhotoFileName: this.PhotoFileName
    };

  //   fetch('http://localhost:53535/api/employee/updateemployee', {
  //     method: 'PUT',
  //     headers: {
  //         'Accept': 'application/json',
  //         'Content-Type': 'application/json'
  //     },
  //     body: JSON.stringify(val)
  // })
  //     .then(res => res.json())
  //     .then((result) => {
  //         alert(result);
  //         this.showCompt.refreshEmpList();
  //        // this.setState({popupWindowVisible:true})
  //     }, (error) => {
  //         alert('Failed');
  //     })
        this.service.updateEmployee(val).subscribe(res => {
      alert(res.toString());
      this.showCompt.refreshEmpList();
    });
      this.ActivateAddEditEmpComp = false;

  }


  uploadPhoto(event: any) {
    var file = event.target.files[0];
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);

    this.service.uploadPhoto(formData).subscribe((data: any) => {
      this.PhotoFileName = data.toString();
      this.PhotoFilePath = this.service.photoUrl + this.PhotoFileName;
    })
  }
}

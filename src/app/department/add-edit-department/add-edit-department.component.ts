import { Component, OnInit, Input } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { ShowDepartmentComponent } from 'src/app/department/show-department/show-department.component';
@Component({
  selector: 'app-add-edit-department',
  templateUrl: './add-edit-department.component.html',
  styleUrls: ['./add-edit-department.component.css']
})
export class AddEditDepartmentComponent implements OnInit {

  constructor(private service: ApiserviceService,private showdept:ShowDepartmentComponent) { }

  @Input() depart: any;
  DepartmentId = "";
  DepartmentName = "";

  ngOnInit(): void {

    this.DepartmentId = this.depart.DepartmentId;
    this.DepartmentName = this.depart.DepartmentName;
  }

  addDepartment() {
    var dept = {
      DepartmentId: this.DepartmentId,
      DepartmentName: this.DepartmentName
    };


  //   fetch('http://localhost:53535/api/department/adddepartment', {
  //     method: 'Post',
  //     headers: {
  //         'Accept': 'application/json',
  //         'Content-Type': 'application/json'
  //     },
  //     body: JSON.stringify(dept)
  // })
  //     .then(res => res.json())
  //     .then((result) => {
  //         alert(result);
  //        // this.setState({popupWindowVisible:true})
  //         this.showdept.refreshDepList();
  //     }, (error) => {
  //         alert('Failed');
  //     })
    

    this.service.addDepartment(dept).subscribe(res => {
      alert(res.toString());
      this.showdept.refreshDepList();
    });
  }

  updateDepartment() {
    var dept = {
      DepartmentId: this.DepartmentId,
      DepartmentName: this.DepartmentName
    };
    this.service.updateDepartment(dept).subscribe(res => {
      alert(res.toString());
      this.showdept.refreshDepList();
    });


  //   fetch('http://localhost:53535/api/department/updatedepartment', {
  //     method: 'put',
  //     headers: {
  //         'Accept': 'application/json',
  //         'Content-Type': 'application/json'
  //     },
  //     body: JSON.stringify(dept)
  // })
  //     .then(res => res.json())
  //     .then((result) => {
  //         alert(result);
  //        // this.setState({popupWindowVisible:true})
  //         this.showdept.refreshDepList();
  //     }, (error) => {
  //         alert('Failed');
  //     })
  }
}

import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from '../service/authService';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project',

  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {
  constructor(private service: ApiserviceService,private Auth:AuthService, private router: Router ) { }
  projectName :string ='';
  ButtonText:string ='Save';
  projectList:any[]=[];
  userInfo:any;
  project_dto =
  {
    project_ID:0,
    project_Name :'',
    created_by:'',
    created_Date:Date.now,
    updated_by:'',
    updated_Date:Date.now,
    isActive:true


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
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Project')))
            {
                this.router.navigate(['/download']);

            }
          
        }
    });
    
    this.refreshProjectList();
  }
  clear()
  {

    this.project_dto = {
    project_ID:0,
    project_Name :'',
    created_by:'',
    created_Date:Date.now,
    updated_by:'',
    updated_Date:Date.now,
    isActive:true
    
  
    };
    this.ButtonText='Save';
  }
  saveClick()
  {
    if(this.project_dto.project_Name==='')
      {
        alert("Project Name is requird");
        return;
      }
      
      if(this.ButtonText=="Update")
        {
           this.project_dto.updated_by = this.userInfo.userId;
           console.log(this.project_dto.updated_by);
          this.service.updateProject(this.project_dto).subscribe(res => {
            alert(res.toString());
            this.refreshProjectList();
          });
          this.clear();

        }
        else 
        {
          this.project_dto.created_by = this.userInfo.userId;
           console.log(this.project_dto.created_by);
          this.service.addProject(this.project_dto).subscribe(res => {
            alert(res.toString());
            this.refreshProjectList();
          });
          this.clear();


        }

  }
  searchProject()
  {
    this.service.searchProjects(this.project_dto).subscribe(data => {
      this.projectList = data
    });
  }
  editClick(item: any) {
    this.project_dto.project_ID = item.Project_ID;
   this.project_dto.project_Name = item.Project_Name;
  if(item.Status=='Active')
    this.project_dto.isActive =true;
  else 
    this.project_dto.isActive = false;
   this.ButtonText="Update";
  }
  refreshProjectList()
  {

    this.service.getAllProjects().subscribe(data => {
      this.projectList = data
    });

  }
}

import { Component, OnInit} from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { AuthService } from '../service/authService';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit  {
  file: File | null = null;
  fileExtension="";
  projectList:any =[];
  projectTempList:any=[];
  ModalTitle = "";
  projectId = 1;
  dropdownVisible: boolean = false;
  ActivateAddEditEmpComp: boolean = false;
  emp: any;
  userInfo : any;
  errorMessage = "";
  uploadData = {
    uploadType: 'GUI',
    verionNo: '',
    releaseBy:'',
    releaseContent: '',
    dateOfReleasse:new Date().toISOString().substring(0, 10),
    uploadBy:'',
    projectName:''
  };

  constructor(private service: ApiserviceService,private Auth:AuthService, private router: Router) { 
  }
  filteredItems: any= [];
  selectedItem: any;
  filterItems(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    
    this.filteredItems = this.projectList.filter((item:any) => item.Project_Name.toLowerCase().includes(value.toLowerCase()));
    console.log(this.filteredItems);
    if(this.filteredItems.length>0)
      {
        this.dropdownVisible = true;
        this.filteredItems =  this.filteredItems;
      }
    // Show the dropdown list when filtering
    else 
    {
      this.dropdownVisible = false;
      this.loadProjectList();
    }
  }

  selectItem(item: any) {
    console.log('Selected item:', item.Project_Name);
    this.selectedItem = item;
    (document.getElementById('searchInput') as HTMLInputElement).value = item.Project_Name;
    this.projectId = item.Project_ID;
     // Update input field value
    this.dropdownVisible= false;
    // No need to toggle dropdown visibility here
  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible; // Toggle dropdown visibility
  }

  // closeDropdown() {
  //   this.dropdownVisible = false; // Close dropdown
  // }
  ngOnInit(): void {
    this.Auth.userInfo$.subscribe((userData: any) => {
      console.log(userData);
      
      this.userInfo = userData;
      console.log(this.userInfo);
      // Check if user has access to the "Upload" menu
        if(this.userInfo==null)
        {
          this.router.navigate(['/login']);
        }
        if(this.userInfo!=null)
        {
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Upload')))
            {
                this.router.navigate(['/login']);

            }
          
        }
    });
    this.loadProjectList();
    //this.refreshEmpList();
  }
  onFilechange(event: any) {
    console.log(event.target.files[0])
    this.file = event.target.files[0]
    this.fileExtension = event.target.files[0].name.split(".").pop();
    console.log("file extension:"+this.fileExtension);

  }
  loadProjectList()
  {
    this.service.getAllProjects().subscribe(data => {
      this.projectList = data
      this.filteredItems = data;
    });

  }
  clearUploadData()
  {
    this.uploadData = {
      uploadType: 'GUI',
      verionNo: '',
      releaseBy:'',
      releaseContent: '',
      dateOfReleasse:new Date().toISOString().substring(0, 10),
      uploadBy :(this.userInfo==null?"":this.userInfo.userId),
      projectName:''
    };

  }
  
  upload() {
    this.uploadData.projectName=(document.getElementById('searchInput') as HTMLInputElement).value;
    if(this.uploadData.projectName=="")
      {
         alert("Project Name is required");
         return;
  
      }
      console.log(this.projectList);
      console.log("this updata data project name "+this.uploadData.projectName);
      console.log(this.projectList.filter((item:any) => item.Project_Name.toLowerCase().includes(this.uploadData.projectName.toLowerCase()).length));
      if(this.projectList.filter((item:any) => item.Project_Name.toLowerCase().includes(this.uploadData.projectName.toLowerCase())).length==0)
      {
            alert("Project Name is invalid and please re-select");
            return;

      }
    if(this.uploadData.verionNo=="")
    {
       alert("Version No is required");
       return;

    }
    if(this.uploadData.releaseContent=="")
    {
       alert("releaseContent is required");
       return;

    }
    console.log("file extension:"+this.fileExtension);
    if (this.fileExtension .toLowerCase() !== 'zip') {
        console.log("Please Select a zip file");
        this.errorMessage = "Please Select a zip file";
        alert("please select a zip file");
        return;
    }
    
    if (this.file) {
      this.uploadData.releaseBy =this.userInfo.userId;
      this.uploadData.uploadBy = this.userInfo.userId;
      this.service.uploadfile(this.file, this.uploadData).subscribe(
          resp => {
              console.log("Response from server:", resp); // Check server response
              alert("File Upload Successfully");
              this.clearUploadData();
              // Check the response and handle it accordingly
          },
          error => {
              console.error("Upload failed:", error);
              alert("Upload failed. Please try again.");
          }
      );
  } else {
      alert("Please select a file first");
  }

  }

}

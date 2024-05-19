import { Component, OnInit } from '@angular/core';
import { ApiserviceService } from 'src/app/apiservice.service';
import { TitleService } from '../service/titleService';
import { AuthService } from '../service/authService';
@Component({
  selector: 'app-download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.css']
  
})
export class DownloadComponent implements OnInit {
  constructor(private service: ApiserviceService,private titleService: TitleService,private Auth:AuthService) { }
  FileList: any = [];
  filteredItems: any= [];
  selectedItem: any;
  userInfo: any;
  projectList:any =[];
  showDeleteButton:boolean=false;
  searchData = {
    fileType: '',
    projectName:''
  };
  projectId = 1;
  dropdownVisible: boolean = false;
  filterItems(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    
    this.filteredItems = this.projectList.filter((item:any) => item.Project_Name.toLowerCase().includes(value.toLowerCase()));
  
    // Check if there are filtered items to display in the dropdown
    if (this.filteredItems.length > 0) {
      this.dropdownVisible = true; // Show the dropdown list
    } else {
      this.dropdownVisible = false; // Hide the dropdown list if no items match the filter
    }
  }
  

  selectItem(item: any) {
    console.log('Selected item:', item.Project_Name);
    this.selectedItem = item;
    (document.getElementById('searchInput') as HTMLInputElement).value = item.Project_Name;
    console.log("text:"+(document.getElementById('searchInput') as HTMLInputElement).value );
    this.projectId = item.Project_ID;
    this.dropdownVisible = false; 
    console.log("type"+this.searchData.fileType);
    this.searchData.projectName = item.Project_Name;
    this.service.searchFileList(this.searchData.fileType,this.searchData.projectName).subscribe(data => {
      this.FileList = data;
    });

    

  }


  loadProjectList()
  {
    this.service.getAllProjects().subscribe(data => {
      this.projectList = data
      this.filteredItems = data;
    });

  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible; // Toggle dropdown visibility
  }

  ngOnInit(): void {
    this.loadProjectList();
    // this.titleService.setPageTitle('Download File');
    this.Auth.userInfo$.subscribe((userData: any) => {
      console.log(userData);
      
      this.userInfo = userData;
      console.log(this.userInfo);
      // Check if user has access to the "Upload" menu

        if(this.userInfo!=null)
        {
          if(this.userInfo.accessList.length>0)
            {
            if(!this.userInfo.accessList.some((menu:any) => menu.Menu_Description.includes('Delete File`')))
            {
                this.showDeleteButton= true;

            }
          }
          else 
          {
              this.showDeleteButton = false;

          }
          
        }
    });

    this.refreshFileList();
  }

  // addClick() {
  // }
  deleteClick(item: any) {
    if (confirm('Are you sure you want to delete this file??')) {
      this.service.deleteFile(item.Id).subscribe(data => {
        alert(data.toString());
        this.refreshFileList();
      })
    }
  }
  TypeChanged(event: any)
  {
    const selectedValue = event.target.value;
    console.log("Selected value:", selectedValue);
    this.searchData.projectName= (document.getElementById('searchInput') as HTMLInputElement).value;
    this.searchData.fileType = selectedValue;
    this.service.searchFileList(this.searchData.fileType,this.searchData.projectName).subscribe(data => {
      this.FileList = data;
    });

  }
  downloadClick(item: any) {
    
  
          this.service.downloadFile(item.Id).subscribe(data => {
            console.log(data);
            const blob = new Blob([data], { type: 'application/x-zip' });
            const url = window.URL.createObjectURL(blob);
           // window.open(url);
           var anchor = document.createElement("a");
            anchor.download = item.FileName;
            anchor.href = url;
            anchor.click();
          });
  }


  closeClick() {
    // this.ActivateAddEditEmpComp = false;
    // this.refreshEmpList();
  }

  refreshFileList() {
    this.service.getFileList().subscribe(data => {
      this.FileList = data;
    });
  }

}


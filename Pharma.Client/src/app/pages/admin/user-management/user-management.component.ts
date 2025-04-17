import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../service/user.service';
import { ToastMessageService } from '../../../service/toast-message.service';

@Component({
  selector: 'app-user-management',
  standalone: false,
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {



  /**
   *
   */
  constructor(private userService: UserService, private toastService: ToastMessageService) {
    this.getAllUsers();

  }
  pageNumber: number = 0;
  pageSize: number = 10;
  searchParams: any;
  users: any[] = [];
  totalRecords: number = 0;
  displayModal: boolean = false;
  user: any = null;

  ngOnInit(): void {
  }

  loadUser(event: any) {
    this.pageNumber = event.first;
    this.pageSize = event.rows;

    this.getAllUsers();

  }

  getAllUsers() {
    const pageNumber = (this.pageNumber / this.pageSize) + 1;
    const pageSize = this.pageSize;
    const searchParams = this.searchParams ? this.searchParams : "";
    this.userService.getAllUsers(pageNumber, pageSize, searchParams).subscribe({
      next: (res: any) => {
        this.users = res.body.items;
        console.log(this.users);
        this.totalRecords = res.body.totalCount;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  updateUserStatus(_t38: any) {
    this.userService.updateUserStatus(_t38.id, _t38.status).subscribe({
      next: (res: any) => {
        this.toastService.showSuccess("Success", "User status updated successfully");
        this.displayModal = false;
        this.user = null;
        this.getAllUsers();
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  userStatuses = [
    { label: 'Active', value: 1 },
    { label: 'Inactive', value: 2 },
    { label: 'Pending', value: 3 },
    { label: 'NotExist', value: 4 },
    { label: 'Deactivated', value: 5 },
    { label: 'Deleted', value: 6 },
  ];



  showModal(selectedUser: any) {
    this.user = selectedUser;
    console.log(this.user);
    this.displayModal = true;
  }

  onStatusChange(newStatus: number) {
    const payload = {
      id: this.user.id,
      status: newStatus
    };

    this.updateUserStatus(payload);
  }



  onHide() {
    this.displayModal = false;
    this.user = null;
  }


}

import { Component, OnInit } from '@angular/core';
import { LedgerService } from '../../../service/ledger.service';



@Component({
  selector: 'app-ledger',
  standalone: false,
  templateUrl: './ledger.component.html',
  styleUrl: './ledger.component.css'
})
export class LedgerComponent implements OnInit {
  first: number = 0;
  rows: number = 10;
  fromDate: Date = new Date();
  toDate: Date = new Date();
  ledgerType: any[] = [{
    label: 'Daily',
    value: 'daily'
  },
  {
    label: 'Weekly',
    value: 'weekly'
  },
  {
    label: 'Monthly',
    value: 'monthly'
  }
  ];
  selectedLedgerType: any = this.ledgerType[0].value;
  ledgerPayment: any[] = [];
  totalRecords: number = 0;
  /**
   *
   */
  constructor(private ledgerService: LedgerService) {

  }

  ngOnInit(): void {
  }

  loadLedgerPayment(event: any) {
    this.first = event.first;
    this.rows = event.rows;

    this.getAllPayment();
  }


  getAllPayment() {
    const pageNumber = (this.first / this.rows) + 1;
    const pageSize = this.rows;
    const selectedLedgerType = "daily";
    this.ledgerService.getTransactionDetails(this.fromDate.toISOString(), this.toDate.toISOString(), pageNumber.toString(), pageSize.toString(), selectedLedgerType).subscribe({
      next: (res: any) => {
        this.ledgerPayment = res.body.items;
        this.totalRecords = res.body.totalCount;
        console.log(res.body);
      },
      error: () => {
        console.log("error");
      }
    })
  }
}



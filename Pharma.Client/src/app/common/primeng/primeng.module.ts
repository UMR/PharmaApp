import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DatePicker } from 'primeng/datepicker';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { PasswordModule } from 'primeng/password';
import { SpeedDialModule } from 'primeng/speeddial';
import { DialogModule } from 'primeng/dialog';
import { InputOtpModule } from 'primeng/inputotp';
import { TableModule } from 'primeng/table';
import { ProgressBarModule } from 'primeng/progressbar';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { SidebarModule } from 'primeng/sidebar';
import { PanelMenuModule } from 'primeng/panelmenu';
import { ImageModule } from 'primeng/image';
import { TabViewModule } from 'primeng/tabview';
import { SplitterModule } from 'primeng/splitter';
import { PaginatorModule } from 'primeng/paginator';
import { SelectButtonModule } from 'primeng/selectbutton';
import { Chip } from 'primeng/chip';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToolbarModule } from 'primeng/toolbar';
import { TagModule } from 'primeng/tag';
import { CarouselModule } from 'primeng/carousel';
import { InputGroupModule } from 'primeng/inputgroup';
import { StepperModule } from 'primeng/stepper';
import { CheckboxModule } from 'primeng/checkbox';
import { InputMaskModule } from 'primeng/inputmask';
import { MeterGroup } from 'primeng/metergroup';
import { TextareaModule } from 'primeng/textarea';
import { BadgeModule } from 'primeng/badge';
import { InputNumber } from 'primeng/inputnumber';
import { AutoComplete } from 'primeng/autocomplete';
import { Select } from 'primeng/select';
import { OverlayBadge, OverlayBadgeModule } from 'primeng/overlaybadge';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { AccordionModule } from 'primeng/accordion';
import { PickListModule } from 'primeng/picklist';
import { MessageModule } from 'primeng/message';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AutoComplete,
    FileUploadModule,
    ToastModule,
    DatePicker,
    FloatLabelModule,
    InputTextModule,
    CardModule,
    PasswordModule,
    SpeedDialModule,
    DialogModule,
    InputOtpModule,
    TableModule,
    ProgressBarModule,
    DropdownModule,
    ButtonModule,
    SidebarModule,
    PanelMenuModule,
    TextareaModule,
    ImageModule,
    TabViewModule,
    SplitterModule,
    PaginatorModule,
    SelectButtonModule,
    TooltipModule,
    Chip,
    ConfirmDialogModule,
    ToolbarModule,
    TagModule,
    CarouselModule,
    InputGroupModule,
    StepperModule,
    CheckboxModule,
    InputMaskModule,
    BadgeModule,
    InputNumber,
    MeterGroup,
    Select,
    OverlayBadgeModule,
    SplitButtonModule,
    ProgressSpinnerModule,
    AccordionModule,
    PickListModule,
    MessageModule
  ],
  exports: [
    ToastModule,
    AutoComplete,
    FileUploadModule,
    DatePicker,
    FloatLabelModule,
    InputTextModule,
    CardModule,
    PasswordModule,
    SpeedDialModule,
    DialogModule,
    InputOtpModule,
    TableModule,
    ProgressBarModule,
    DropdownModule,
    ButtonModule,
    SidebarModule,
    PanelMenuModule,
    TextareaModule,
    ImageModule,
    TabViewModule,
    SplitterModule,
    PaginatorModule,
    SelectButtonModule,
    TooltipModule,
    Chip,
    ConfirmDialogModule,
    ToolbarModule,
    TagModule,
    CarouselModule,
    InputGroupModule,
    StepperModule,
    CheckboxModule,
    InputMaskModule,
    BadgeModule,
    InputNumber,
    MeterGroup,
    Select,
    OverlayBadge,
    SplitButtonModule,
    ProgressSpinnerModule,
    AccordionModule,
    PickListModule,
    MessageModule
  ],
  providers: [
    MessageService, ConfirmationService
  ]
})
export class PrimengModule { }

**1. Project Overview**
The Mini Account Management System is a lightweight web application built using ASP.NET Core Razor Pages and MS SQL Server,
 designed to handle essential accounting functions such as 
user and role management 
chart of accounts,
 and voucher entry. 
It incorporates role-based access control and secure authentication via ASP.NET Identity.

**2. Technologies Used**
Backend Framework: ASP.NET Core with Razor Pages
Database: Microsoft SQL Server (Stored Procedures only)
Authentication & Authorization: ASP.NET Identity with custom roles
UI: Razor Pages with Bootstrap for responsive forms and tables
Additional Tools: Table-Valued Parameters in SQL Server for efficient voucher entry

**3. Core Features**
**3.1 User Roles & Permissions**
-Implemented roles: Admin, Accountant, Viewer
-Role-based authorization enforced via Authorize attributes
-Role initialization at app startup using a dedicated RoleInitializer service
-Admin role can manage users and assign roles
**3.2 Chart of Accounts**
-Create, update, delete account records with hierarchical parent-child relationships
-Managed exclusively through stored procedures (sp_ManageChartOfAccounts)
-Accounts displayed in hierarchical dropdowns for voucher entry
**3.3 Voucher Entry Module**
-Support for Journal, Payment, and Receipt vouchers
-Voucher header data: type, date, reference number, creator
-Voucher detail lines with multi-entry debit and credit amounts, account selection, and narration
-Data saved efficiently via SQL Server stored procedure (sp_SaveVoucher) using table-valued parameters
-Server-side validation to ensure total debits equal total credits before saving
-UI supports dynamic adding/removing of voucher entry rows
**3.4 Voucher Viewing & Reporting**
-List of vouchers displayed using sp_GetVouchers stored procedure
-Detailed voucher entry lines fetched with sp_GetVoucherEntriesByVoucherId
-Role-based access to voucher viewing for Admin, Accountant, and Viewer roles

**4. Security & Access Control**
-Full authentication integrated with ASP.NET Identity
-Role-based authorization restricts features and pages accordingly
-Sensitive operations (e.g., voucher entry, user management) limited to authorized roles
-Login and registration pages extended to support role selection during user registration
-User passwords hashed and stored securely
-Email confirmation enabled, but can be disabled or replaced with dummy sender as per requirements

**5. Database Design Highlights**
-Vouchers table stores voucher headers
-VoucherEntries table stores voucher line items linked by VoucherId
-ChartOfAccounts stores account master data with parent-child relationships
-User and role management tables managed by ASP.NET Identity schema
-All data modifications performed via stored procedures for security and performance

**7. Possible Future Enhancements**
-Implement export to Excel and PDF reports
-Add audit trails for voucher changes
-Support for multi-currency and tax calculations
-Enhance user management with profile editing and password reset
-Responsive mobile-friendly UI improvements
-Real email integration for registration confirmation and notifications

**Here are some Screenshots Of the Projects:**
Home page before login : ![image](https://github.com/user-attachments/assets/4e219fd7-2aa4-44d7-9946-25e9f798d29d)
Home Page after login(Admin) : ![image](https://github.com/user-attachments/assets/e1f82584-cea1-41be-ae40-fc81ff0f05e7)
Edit Role Page(Admin) : ![image](https://github.com/user-attachments/assets/6d95678f-3ea8-49bd-8097-ef8f272c9504)
Home Page after login(Accountant) : ![image](https://github.com/user-attachments/assets/c5d58259-e21b-4252-88f9-e2c65fcbdcf5)
Home Page after login(Viewer) : ![image](https://github.com/user-attachments/assets/a6db6cd7-f7fb-43f7-9ce9-078c07e46dfc)
Manage User (Admin only) : ![image](https://github.com/user-attachments/assets/dcb293a4-0a3b-4d29-926e-5ac7edf786b5)
Charts of Account (Admin, Accountant) : ![image](https://github.com/user-attachments/assets/f1a62480-98a8-49cd-8b31-155fab58df0d)
Voucher Entry Page (Accountant) : ![image](https://github.com/user-attachments/assets/024f9354-7d9c-44ae-8633-6ae971273fc2)
Show List of voucher(Everyone) : ![image](https://github.com/user-attachments/assets/15072458-cdb5-49b6-82fc-50d06a9efc69)
Voucher details page(everyone) : ![image](https://github.com/user-attachments/assets/d0e7ce8b-1fd4-4f5f-a405-9663268573d0)




 


# Pharmacy Inventory & Prescription Ledger System
A robust Pharmacy Inventory and Prescription Ledger System built with ASP.NET Core MVC and C# using the Repository Design Pattern.
Utilizes Razor Pages for the dynamic frontend, ADO.NET for high-performance data access, and MS SQL Server as the database.

## Domain
This system manages medication inventory for a pharmacy dispensary. Core entities include medications with fields for name, description, image, price, stock quantity, and active/inactive status.

## Features
- View all medications in stock
- Add new medications with image upload
- Edit existing medication details
- Delete medications from inventory
- View medication details
- Image validation (type and size limits)
- Image upload and cleanup on delete/update

## Project Structure
```
PharmacyInventory/
├── PharmacyInventory.csproj
├── Program.cs
├── appsettings.json
├── Properties/
│   └── launchSettings.json
├── Data/
│   ├── MedicationRepository.cs       (ADO.NET CRUD operations)
│   └── MedicationImageService.cs     (Image upload/validation/delete)
├── Models/
│   └── Medication.cs                 (Data model with validation attributes)
├── Pages/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Index.cshtml / Index.cshtml.cs  (Home page)
│   ├── Error.cshtml / Error.cshtml.cs  (Error page)
│   ├── Shared/
│   │   └── _Layout.cshtml
│   └── Medications/
│       ├── Index.cshtml / Index.cshtml.cs
│       ├── Create.cshtml / Create.cshtml.cs
│       ├── Edit.cshtml / Edit.cshtml.cs
│       ├── Delete.cshtml / Delete.cshtml.cs
│       └── Details.cshtml / Details.cshtml.cs
├── wwwroot/
│   └── images/
│       └── medications/
└── medications script.txt
```

## Setup
1. Run the SQL script `medications script.txt` against your MS SQL Server to create the `dbo.medications` table.
2. Update the `DefaultConnection` connection string in `appsettings.json` to point to your database.
3. Create the folder `wwwroot/images/medications` for medication image storage.
4. Build and run: `dotnet run`

## NuGet Packages
- Microsoft.Data.SqlClient
- Microsoft.Extensions.Configuration

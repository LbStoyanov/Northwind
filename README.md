# Northwind

## Database Setup

If you're using LocalDb, no additional setup is required. However, if you're using a different SQL Server instance, update the connection string in the `appsettings.json` file.

- Open `appsettings.json` in the project root directory.
- Locate the `"ConnectionStrings"` section and modify the `NorthwindDatabase` connection string as follows:

  For example, if you're using LocalDb:
  ```json
  "ConnectionStrings": {
    "NorthwindDatabase": "Server=(LocalDb)\\MSSQLLocalDB;Database=Northwind;Trusted_Connection=True;"
  }


## API and MVC URL Setup

The MVC project communicates with the API project. By default, the API is expected to run on `https://localhost:7128`.

If you're running the API on a different port, you must adjust the URL in the MVC project and potentially in the API project.

### In the MVC project:
- Open the `Program.cs` file in the `Northwind.MVC` project.
- Locate the line where the `HttpClient` is configured:
  ```csharp
  client.BaseAddress = new Uri("https://localhost:7128");
- Change `https://localhost:7128` to the appropriate URL and port where your API is running.
-  Ensure the API is configured to run on the correct port. This can be modified in the          launchSettings.json file under the Profiles section: "applicationUrl": "https://localhost:5001" // Change the port here if necessary




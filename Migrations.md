# Creating Migrations 

1. In Visual Studio, open **Package Manager Console** from `Tools -> NuGet Package Manager -> Package Manager Console`. You can also use **Command Line** (then you have to use dotnet commands)
   
2. FIRST AND FOREMOST, change your directory to `.csproj` file (project file). Otherwise, the commands won’t execute. It can look like that:
`cd  C:\Users\aleks\OneDrive\Pulpit\StudyBuddy\StudyBuddy\StudyBuddy.csproj`

3. Drop the existing database (if any): `Drop-Database -Context <DbContextName> -Force` or `dotnet ef database drop --context <DbContextName> --force`

4. If you already have some present migrations in “Migrations” folder, remove them either using the command below or manually (in case of troubles):
`dotnet ef migrations remove --context <DbContextName>` or `Remove-Migration -Context <DbContextName>`

5. After ensuring the model changes are correctly set up in DbContext (or other way you’ve called it), create a new migration:
`Add-Migration InitialCreate -Context <DbContextName>` or `dotnet ef migrations add InitialCreate --context <DbContextName>`

This generates the script I’ve sent based on current entity models. 
 
6. Then we should apply this migration into the database (update it): `Update-Database -Context ApplicationDbContext <DbContextName>` or `dotnet ef database update --context <DbContextName>`

7. Good Luck!


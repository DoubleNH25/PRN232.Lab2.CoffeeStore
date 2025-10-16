@echo off
echo Creating Coffee Store Database...
echo.

sqlcmd -S (localdb)\mssqllocaldb -i Database.sql
if %errorlevel% neq 0 (
    echo Error creating database
    pause
    exit /b %errorlevel%
)

echo Database created successfully!
echo.

echo Seeding data...
sqlcmd -S (localdb)\mssqllocaldb -d CoffeeStoreDB -i SeedData.sql
if %errorlevel% neq 0 (
    echo Error seeding data
    pause
    exit /b %errorlevel%
)

echo Data seeded successfully!
echo.

echo Setup completed!
pause
@echo off
echo CoffeeStore API Postman Tests Import Script
echo ==========================================

echo Installing Newman (Postman CLI)...
npm install -g newman

echo.
echo Importing Postman Collection and Environment...
echo Make sure Postman is running before proceeding.
echo.

echo To manually import:
echo 1. Open Postman
echo 2. Click "Import" in the top left
echo 3. Select the following files:
echo    - CoffeeStore_API_Tests.postman_collection.json
echo    - CoffeeStore_Environment.postman_environment.json
echo.

echo To run tests with Newman:
echo newman run CoffeeStore_API_Tests.postman_collection.json -e CoffeeStore_Environment.postman_environment.json
echo.

echo To generate HTML report:
echo newman run CoffeeStore_API_Tests.postman_collection.json -e CoffeeStore_Environment.postman_environment.json -r html
echo.

pause
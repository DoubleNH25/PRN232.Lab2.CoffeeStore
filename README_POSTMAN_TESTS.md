# CoffeeStore API Postman Tests

This repository contains comprehensive Postman tests for the CoffeeStore API, covering all required functionality including authentication, paging, sorting, searching, field selection, HTTP status codes, JSON schema validation, actual data value checks, and edge cases.

## Prerequisites

1. [Postman](https://www.postman.com/downloads/) installed
2. CoffeeStore API running locally on `https://localhost:7228`
3. Database initialized with seed data

## Importing the Collection

1. Open Postman
2. Click on "Import" in the top left corner
3. Select the following files from this repository:
   - `CoffeeStore_API_Tests.postman_collection.json`
   - `CoffeeStore_Environment.postman_environment.json`
4. The collection will appear in your sidebar

## Environment Variables

The environment file includes the following variables:

| Variable | Description | Default Value |
|----------|-------------|---------------|
| `baseUrl` | Base URL of the API | `https://localhost:7228` |
| `accessToken` | JWT access token (auto-populated) | Empty |
| `refreshToken` | Refresh token (auto-populated) | Empty |
| `adminEmail` | Admin user email | `admin@coffeestore.com` |
| `adminPassword` | Admin user password | `Admin@123` |
| `userId` | User ID (auto-populated) | Empty |

## Test Organization

The collection is organized into the following folders:

### 1. Authentication
- Register new user
- Login user
- Refresh token
- Logout user
- Unauthorized access tests
- Forbidden access tests

### 2. Products
- Get all products with paging
- Sorting tests
- Searching functionality
- Field selection
- Get product by ID
- Create, update, and delete products
- Authorization tests
- Validation error tests

### 3. Categories
- Get all categories with paging
- Sorting tests
- Searching functionality
- Field selection
- Get category by ID
- Create, update, and delete categories
- Authorization tests
- Validation error tests

### 4. Orders
- Get all orders with paging
- Sorting tests
- Searching functionality
- Field selection
- Get order by ID
- Create, update, and delete orders
- Authorization tests
- Validation error tests

### 5. Payments
- Get all payments with paging
- Sorting tests
- Searching functionality
- Field selection
- Get payment by ID
- Create, update, and delete payments
- Authorization tests
- Validation error tests

### 6. Edge Cases
- Empty results handling
- Out of bound page numbers
- Invalid tokens
- Validation errors

## Running the Tests

### Option 1: Run All Tests
1. Select the entire collection
2. Click "Run" above the collection name
3. In the Collection Runner, click "Run CoffeeStore API Tests"

### Option 2: Run Individual Folders
1. Expand the collection
2. Right-click on any folder (e.g., "Authentication")
3. Select "Run" → "Run Authentication"

### Option 3: Run Individual Requests
1. Click on any request
2. Click "Send" to execute the request
3. View results in the response pane

## Test Features

### Authentication/Authorization
- All protected endpoints test access token handling
- Unauthorized access tests (401 status codes)
- Forbidden access tests (403 status codes)
- Token refresh functionality

### Paging
- Tests correct handling of `page` and `pageSize` parameters
- Validates item count per page
- Checks `totalCount` and `totalPages` calculations
- Verifies page boundary handling

### Sorting/Searching
- Ascending and descending sort validation
- Search term matching in relevant fields
- Combined sorting and searching tests

### Field Selection
- Tests `select` parameter functionality
- Verifies only requested fields are returned
- Ensures essential fields are always present when needed

### HTTP Status Codes
- All requests validate appropriate status codes:
  - 200: Successful GET, PUT operations
  - 201: Successful POST operations
  - 204: Successful DELETE operations
  - 400: Validation errors
  - 401: Unauthorized access
  - 403: Forbidden access
  - 404: Resource not found

### JSON Schema Validation
- All responses are validated against expected structures
- Required fields are checked
- Data types are verified

### Actual Data Value Checks
- Product names, prices, and other values are verified
- Order statuses and dates are validated
- Payment amounts and methods are checked
- Category names and descriptions are confirmed

### Edge Cases
- Empty search results
- Out-of-bound page numbers
- Invalid tokens
- Validation errors
- Non-existent resource access

## Test Scripts

Each request includes comprehensive test scripts that:

1. Validate HTTP status codes
2. Check response structure and JSON schema
3. Verify actual data values where appropriate
4. Handle authentication tokens automatically
5. Save relevant data for use in subsequent requests

## Customization

To customize the tests for your environment:

1. Modify the environment variables in `CoffeeStore_Environment.postman_environment.json`
2. Update the `baseUrl` if your API runs on a different port
3. Change `adminEmail` and `adminPassword` to match your test user credentials

## Troubleshooting

### Tests Failing
- Ensure the API is running on `https://localhost:7228`
- Verify database is initialized with seed data
- Check that environment variables are correctly set
- Confirm the admin user exists with the specified credentials

### Authentication Issues
- Run the "Login User" request first to obtain tokens
- Check that `accessToken` and `refreshToken` are populated in the environment
- If tokens expire, run "Refresh Token" or "Login User" again

### Missing Data
- Some tests depend on data created by previous requests
- Run tests in order or ensure required data exists in the database
- Check that `productId`, `categoryId`, `orderId`, and `paymentId` variables are set

## Report Generation

To generate test reports using Newman:

1. Install Newman globally:
   ```bash
   npm install -g newman
   ```

2. Run the collection and generate HTML report:
   ```bash
   newman run CoffeeStore_API_Tests.postman_collection.json -e CoffeeStore_Environment.postman_environment.json -r html
   ```

3. For JSON report:
   ```bash
   newman run CoffeeStore_API_Tests.postman_collection.json -e CoffeeStore_Environment.postman_environment.json -r json
   ```

4. For both HTML and JSON reports:
   ```bash
   newman run CoffeeStore_API_Tests.postman_collection.json -e CoffeeStore_Environment.postman_environment.json -r html,json
   ```

## Contributing

To add new tests or modify existing ones:

1. Open the collection in Postman
2. Make your changes
3. Export the updated collection
4. Replace the existing `CoffeeStore_API_Tests.postman_collection.json` file
5. Commit and push your changes

## Support

For issues with the Postman tests, please create an issue in this repository with:
1. Description of the problem
2. Screenshots of failing tests
3. Steps to reproduce
4. Environment information